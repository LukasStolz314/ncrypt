using ncrypt.Library;

namespace ncrypt.Server.Model;

public class ParameterModel
{
    public String Name { get; init; }

    public Object Value { get; set; } = String.Empty;

    public ConvertType? ConvertType { get; set; }

    public ParameterModel (String name, Object value)
    {
        Name = name;
        Value = value;
    }

    public ParameterModel (String name, ConvertType? convertType)
    {
        Name = name;
        ConvertType = convertType;
    }

    public void Update(Object? value, ConvertType? convertType)
    {
        if(value is not null)
            Value = value;

        if(convertType is not null)
            ConvertType = convertType;
    }
}
