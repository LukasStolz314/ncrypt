using System.Text;

namespace ncrypt.Library.Code;

[SelectableService]
public class HexService
{
    [SelectableFunction]
    public String Encode(String plainText)
    {
        var bytes = Encoding.ASCII.GetBytes (plainText);
        return Convert.ToHexString (bytes);
    }

    [SelectableFunction]
    public String Decode(String encodedText)
    {
        var bytes = Convert.FromHexString (encodedText);
        return Encoding.ASCII.GetString (bytes);
    }
}
