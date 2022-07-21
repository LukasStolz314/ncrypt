using System.Text;

namespace ncrypt.Library.Code;

[RenderUI (Class = RenderClass.Format)]
public class HexService
{
    [RenderUI]
    public String Encode(String plainText)
    {
        var bytes = Encoding.ASCII.GetBytes (plainText);
        return Convert.ToHexString (bytes);
    }

    [RenderUI]
    public String Decode(String encodedText)
    {
        var bytes = Convert.FromHexString (encodedText);
        return Encoding.ASCII.GetString (bytes);
    }
}
