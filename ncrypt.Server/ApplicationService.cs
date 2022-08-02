using ncrypt.Library;
using ncrypt.Server.Model;
using System.Reflection;
using System.Text;

namespace ncrypt.Server;

public class ApplicationService
{
    public List<List<Type>> LoadGroupedTypes (String assemblyName)
    {
        List<List<Type>> groupedTypes = new ();
        Assembly lib = Assembly.Load ("ncrypt.Library");
        var services = lib.GetTypes ()
            .Where (t => t.CustomAttributes
                .Any (a => a.AttributeType == typeof (RenderUI))
            ).ToList ();

        groupedTypes = services.GroupBy (
            s => s.GetCustomAttribute<RenderUI> ()!.Class,
            (key, g) => g.ToList ()
        ).ToList ();

        return groupedTypes;
    }

    public String ExecuteSequence (ExecuteSequenceModel model, ref StringBuilder errors)
    {
        String result = String.Empty;
        try
        {
            FormatInputModel fim = new (model.Input, model.InputConvertType);
            String input = FormatInput (fim, ref errors);

            foreach (var generic in model.sequence)
            {
                // Get meta information
                StringBuilder genericErrors = new ();
                Type serviceType = generic.ServiceType;
                MethodInfo method = serviceType.GetMethod (generic.SelectedMethod)!;

                // Match input with method parameters
                FormatMethodParametersModel fmpm = new (method, input, generic);
                var methodParameters = FormatMethodParameters (fmpm, ref genericErrors);

                // Match input with constructor parameters
                FormatConstructorParametersModel fcpm = new (serviceType, generic);
                var constructorParameters = FormatConstructorParameters (fcpm, ref genericErrors);

                // Check if errors where thrown before method execution
                if (!genericErrors.ToString ().Equals (String.Empty))
                {
                    errors.Append (genericErrors);
                    continue;
                }

                // Create service and invoke method
                var instance = Activator.CreateInstance (serviceType, constructorParameters);
                input = (String) method.Invoke (instance, methodParameters)!;
            }

            result = Converter.FromHex (input, model.OutputConvertType);
        }
        catch (TargetInvocationException e)
        {
            errors.Clear ();
            errors.Append("Invalid Format");
        }

        return result;
    }

    private String FormatInput (FormatInputModel model, ref StringBuilder errors)
    {
        String result = String.Empty;
        try
        {
            result = Converter.ToHex (model.Input, model.InputConvertType);
        }
        catch (FormatException e)
        {
            errors.AppendLine ("Invalid Input Format");
        }

        return result;
    }

    private Object[] FormatMethodParameters (
        FormatMethodParametersModel model, ref StringBuilder errors)
    {
        List<Object> result = new ();
        foreach (var parameter in model.Method.GetParameters ())
        {
            try
            {
                if (parameter.Name!.Equals ("input"))
                    result.Add (model.Input);
                else
                    result.Add (model.Generic.Result[parameter.Name]);
            }
            catch (KeyNotFoundException e)
            {
                var splitted = e.Message.Split ('\'');
                if (splitted.Count () == 3)
                    errors.AppendLine ($"Parameter {splitted[1]} is missing");
                continue;
            }
        }

        return result.ToArray ();
    }

    private Object[] FormatConstructorParameters (
        FormatConstructorParametersModel model, ref StringBuilder errors)
    {
        List<Object> constructorParams = new ();
        ParameterInfo[] parameters = model.Service.GetConstructors ().First ().GetParameters ();
        foreach (var parameter in parameters)
        {
            try
            {
                constructorParams.Add (model.Generic.Result[parameter.Name!]);
            }
            catch (KeyNotFoundException e)
            {
                var splitted = e.Message.Split ('\'');
                if (splitted.Count () == 3)
                    errors.AppendLine ($"Parameter {splitted[1]} is missing");
                continue;
            }
        }

        return constructorParams.ToArray ();
    }
}
