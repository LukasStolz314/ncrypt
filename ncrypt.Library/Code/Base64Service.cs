using System.Text;

namespace ncrypt.Library.Code;

[SelectableService]
public class Base64Service
{
    [SelectableFunction]
    public String Encode(Byte[] input)
    {
        return Convert.ToBase64String (input);
    }

    [SelectableFunction]
    public String Decode(Byte[] input)
    {
        var value = Encoding.UTF8.GetString (input);
        var bytes = Convert.FromBase64String (value);
        return Encoding.UTF8.GetString (bytes);
    }
}
