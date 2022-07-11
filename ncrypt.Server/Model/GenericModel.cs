using ncrypt.Library;
using System.Reflection;

namespace ncrypt.Server.Model;

public class GenericModel
{
    public String Name{ get; set; }
    public Type ModelResultType { get; set; }
    public List<PropertyInfo> Properties { get; set; }
    public Type ServiceType { get; set; }
    public List<MethodInfo> ServiceMethods { get; set; }

    public GenericModel (String name, Type resultType, Type serviceType)
    {
        Name = name;
        ModelResultType = resultType;
        Properties = resultType.GetProperties().ToList();
        ServiceType = serviceType;
        ServiceMethods = serviceType.GetMethods ().Where(m => m.CustomAttributes.Any(a => a.AttributeType == typeof(SelectableFunction))).ToList ();
    }
}
