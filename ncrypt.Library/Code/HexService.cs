using System.Text;

namespace ncrypt.Library.Code;

[RenderUI (Class = RenderClass.Format)]
public class HexService
{
    [RenderUI]
    public String Encode(String input)
    {
        var bytes = Encoding.ASCII.GetBytes (input);
        return Convert.ToHexString (bytes);
    }

    [RenderUI]
    public String Decode(String input)
    {
        var bytes = Convert.FromHexString (input);
        return Encoding.ASCII.GetString (bytes);
    }
}
