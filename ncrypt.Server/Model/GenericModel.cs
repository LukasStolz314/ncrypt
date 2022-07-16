using ncrypt.Library;
using System.Reflection;

namespace ncrypt.Server.Model;

public class GenericModel
{
    public String Name{ get; set; }
    public Type ModelResultType { get; set; }
    public Type ServiceType { get; set; }

    private List<PropertyInfo>? _properties;
    public List<PropertyInfo> Properties
    {
        get 
        {
            if (_properties is null || _properties.Count == 0)
            {
                _properties = ModelResultType.GetProperties().ToList();
            }

            return _properties;
        }
    }

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
        
        
    public GenericModel (String name, Type resultType, Type serviceType)
    {
        Name = name;
        ModelResultType = resultType;
        ServiceType = serviceType;
    }
}
