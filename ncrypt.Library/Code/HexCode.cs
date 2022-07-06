using System.Text;

namespace ncrypt.Library.Code;

public class HexCode
{
    public String Encode(String plainText)
    {
        var bytes = Encoding.ASCII.GetBytes (plainText);
        return Convert.ToHexString (bytes);
    }

    public String Decode(String encodedText)
    {
        var bytes = Convert.FromHexString (encodedText);
        return Encoding.ASCII.GetString (bytes);
    }
}
