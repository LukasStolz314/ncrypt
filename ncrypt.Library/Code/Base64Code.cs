using System.Text;

namespace ncrypt.Library.Code;
public class Base64Code
{
    public String Encode(String plainText)
    {
        var bytes = Encoding.ASCII.GetBytes (plainText);
        return Convert.ToBase64String (bytes);
    }

    public String Decode(String encodedText)
    {
        var bytes = Convert.FromBase64String (encodedText);
        return Encoding.ASCII.GetString (bytes);
    }
}
