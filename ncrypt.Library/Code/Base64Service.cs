using System.Text;

namespace ncrypt.Library.Code;

[SelectableService]
public class Base64Service
{
    [SelectableFunction]
    public String Encode(String input)
    {
        var bytes = Encoding.ASCII.GetBytes (input);
        return Convert.ToBase64String (bytes);
    }

    [SelectableFunction]
    public String Decode(String input)
    {
        var bytes = Convert.FromBase64String (input);
        return Encoding.ASCII.GetString (bytes);
    }
}
