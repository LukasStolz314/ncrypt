using ncrypt.Library;
using System.Reflection;

namespace ncrypt.Server.Model;

public class GenericModel
{
    public String Name { get; set; }
    public Type ServiceType { get; set; }
    public String SelectedMethod { get; set; } = "";

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

    public ParameterInfo[] GetParametersOfMethod (String method)
        => ServiceMethods.First (x => x.Name.Equals (method)).GetParameters (); 
}
