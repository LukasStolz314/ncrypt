using Microsoft.JSInterop;
using ncrypt.Library;
using ncrypt.Server.Model;
using System.Reflection;

namespace ncrypt.Server;

public class ApplicationService
{
    private IJSRuntime _ijsRuntime;
    public ApplicationService(IJSRuntime ijsRuntime)
    {
        _ijsRuntime = ijsRuntime;
    }

    private Dictionary<Type, String> handledException = new()
    {
        [typeof(TargetInvocationException)] = "Invalid format",
        [typeof(KeyNotFoundException)] = "Invalid format",
        [typeof(NullReferenceException)] = "Something is missing",
    };

    public List<List<Type>> LoadGroupedTypes()
    {
        List<List<Type>> groupedTypes = new();
        Assembly lib = Assembly.Load("ncrypt.Library");
        var services = lib.GetTypes()
            .Where(t => t.CustomAttributes
                .Any(a => a.AttributeType == typeof(RenderUI))
            ).ToList();

        String path = Path.Combine(Environment.CurrentDirectory, "PluginServices");

        Directory.CreateDirectory(path);

        foreach (String dll in Directory.GetFiles(path, "*.dll"))
        {
            var types = Assembly.LoadFile(dll).GetTypes();
            var selected = types.Where(t => t.CustomAttributes.Any(a => a.AttributeType.Name == "RenderUI"));
            services.AddRange(selected);
        }

        groupedTypes = services.GroupBy(
            s => s.GetCustomAttribute<RenderUI>()!.Class,
            (key, g) => g.ToList()
        ).ToList();

        return groupedTypes;
    }

    public void ExecuteCopy(ExecuteCopyModel model)
    {
        var instance = Activator.CreateInstance(model.Method.DeclaringType!);
        String hex = Converter.ToHex(model.Text, model.OutputConvertType);
        String hexResult = (String?) model.Method.Invoke(
            instance, new object[] { hex }) ?? String.Empty;

        String result = Converter.FromHex(hexResult, model.OutputConvertType);

        _ijsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", result);
    }

    public String ExecuteSequence(ExecuteSequenceModel model, out String errors)
    {
        String result = String.Empty;
        try
        {
            FormatInputModel fim = new(model.Input, model.InputConvertType);
            String input = FormatInput(fim);

            foreach (var generic in model.sequence)
            {
                // Get meta information
                Type serviceType = generic.ServiceType;
                MethodInfo method = serviceType.GetMethod(generic.SelectedMethod)!;

                // Match input with method parameters
                FormatMethodParametersModel fmpm = new(method, input, generic);
                var methodParameters = FormatMethodParameters(fmpm);

                // Match input with constructor parameters
                FormatConstructorParametersModel fcpm = new(serviceType, generic);
                var constructorParameters = FormatConstructorParameters(fcpm);

                // Create service and invoke method
                var instance = Activator.CreateInstance(serviceType, constructorParameters);
                input = (String)method.Invoke(instance, methodParameters)!;
            }

            result = Converter.FromHex(input, model.OutputConvertType);
            errors = String.Empty;
        }
        catch (Exception e)
        {
            errors = GetMessageFromException(e);
        }

        return result;
    }

    private String FormatInput(FormatInputModel model)
    {
        String result = String.Empty;

        result = Converter.ToHex(model.Input, model.InputConvertType);

        return result;
    }

    private Object?[] FormatMethodParameters(FormatMethodParametersModel model)
    {
        List<Object?> result = new();
        foreach (var parameter in model.Method.GetParameters())
        {
            if (parameter.Name!.Equals("input"))
                result.Add(model.Input);
            else
            {
                model.Generic.Result.TryGetValue(parameter.Name, out Object? value);
                result.Add(value);
            }
        }

        return result.ToArray();
    }

    private Object?[] FormatConstructorParameters(FormatConstructorParametersModel model)
    {
        List<Object?> constructorParams = new();
        ParameterInfo[] parameters = model.Service.GetConstructors().First().GetParameters();
        foreach (var parameter in parameters)
        {
            model.Generic.Result.TryGetValue(parameter.Name!, out Object? value);
            constructorParams.Add(value);
        }

        return constructorParams.ToArray();
    }

    private String GetMessageFromException(Exception e)
    {
        handledException.TryGetValue(e.GetType(), out String? message);

        return message ?? "Something went wrong";
    }
}
