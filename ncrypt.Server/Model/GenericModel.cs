using ncrypt.Library;
using System.Reflection;

namespace ncrypt.Server.Model;

public class GenericModel
{
    #region Value-Properties
    
    public String Name { get; set; }
    public Type ServiceType { get; set; }
    public String SelectedMethod { get; set; } = "";    
    public Dictionary<String, Object> Result { get; } = new ();    

    private List<MethodInfo>? _serviceMethods;
    public List<MethodInfo> ServiceMethods
    {
        get 
        {
            if (_serviceMethods is null || _serviceMethods.Count == 0)
            {
                _serviceMethods = ServiceType.GetMethods ()
                    .Where (m => m.CustomAttributes
                         .Any (a => a.AttributeType == typeof (RenderUI))
                    ).ToList ();
            }

            return _serviceMethods;
        }
    }

    #endregion

    public GenericModel (Type service, String? name = null)
    {
        Name = name ?? service.Name.Replace("Service", "");
        ServiceType = service;
    }

    #region Helpers

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

    public String GetUINameOfParameter(ParameterInfo parameter) 
        => parameter.GetCustomAttribute<UIParam>()?.Name ?? parameter.Name!;

    #endregion

    public void UpdateResult(String key, Object value)
    {
        if(Result.ContainsKey(key))
            Result[key] = value;
        else
            Result.Add(key, value);
    }
}
