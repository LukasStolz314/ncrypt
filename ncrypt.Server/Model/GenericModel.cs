using ncrypt.Library;
using System.Reflection;

namespace ncrypt.Server.Model;

public class GenericModel
{
    public String Name { get; set; }
    public Type ServiceType { get; set; }
    public String SelectedMethod { get; set; } = "";
    public ConvertType OutputConvertType { get; set; }
    public ConvertType InputConvertType { get; set; }
    public List<ParameterModel> Parameters { get; } = new ();    

    private List<MethodInfo>? _serviceMethods;
    public List<MethodInfo> ServiceMethods
    {
        get 
        {
            if (_serviceMethods is null || _serviceMethods.Count == 0)
            {
                _serviceMethods = ServiceType.GetMethods ()
                    .Where (m => m.CustomAttributes
                         .Any (a => a.AttributeType == typeof (SelectableFunction))
                    ).ToList ();
            }

            return _serviceMethods;
        }
    }    

    public GenericModel (Type service, String? name = null)
    {
        Name = name ?? service.Name.Replace("Service", "");
        ServiceType = service;
    }

    public List<String> ServiceMethodNames 
        => ServiceMethods.Select (x => x.Name).ToList ();

    public ParameterInfo[] ConstructorParameters 
        => ServiceType.GetConstructors ().First ().GetParameters ();

    public ParameterInfo[] GetParametersOfMethod (String method)
    {
        var parameters = ServiceMethods
            .First (x => x.Name.Equals (method))
            .GetParameters ()
            .ToList();

        parameters.RemoveAll(x => x.Name.Equals ("input"));       

        return parameters.ToArray();
    }
    
    public Object GetResultValue (String paramName, Type paramType)
    {
        var param = Parameters.First (m => m.Name.Equals (paramName));
        var result = param.Value;

        if(paramType == typeof(Byte[]))
            result = Converter.ToByteArray((String)param.Value, param.ConvertType ?? ConvertType.HEX);

        return result;
    }
        

    public void UpdateParamValue(String key, Object value)
    {
        Func<ParameterModel, Boolean> nameFunc = m => m.Name.Equals(key);
        if(Parameters.Any(nameFunc))
            Parameters.First(nameFunc).Update(value, null);
        else
            Parameters.Add(new(key, value));
    }

    public void UpdateParamConvertType(String key, ConvertType convertType)
    {
        Func<ParameterModel, Boolean> nameFunc = m => m.Name.Equals(key);
        if(Parameters.Any(nameFunc))
            Parameters.First(nameFunc).Update(null, convertType);
        else
            Parameters.Add(new(key, convertType));
    }
}
