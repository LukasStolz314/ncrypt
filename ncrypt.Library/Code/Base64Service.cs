using System.Text;

namespace ncrypt.Library.Code;

[SelectableService]
public class Base64Service
{
    [SelectableFunction]
    public String Encode(String plainText)
    {
        var bytes = Encoding.ASCII.GetBytes (plainText);
        return Convert.ToBase64String (bytes);
    }

    [SelectableFunction]
    public String Decode(String encodedText)
    {
        var bytes = Convert.FromBase64String (encodedText);
        return Encoding.ASCII.GetString (bytes);
    }
}
