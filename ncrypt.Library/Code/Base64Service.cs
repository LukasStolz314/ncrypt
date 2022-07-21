using System.Text;

namespace ncrypt.Library.Code;

[RenderUI (Class = RenderClass.Format)]
public class Base64Service
{
    [RenderUI]
    public String Encode(String input)
    {
        var bytes = Convert.FromHexString(input);
        var result = Convert.ToBase64String(bytes);
        return Converter.ToHex(result, ConvertType.BASE64);
    }

    [RenderUI]
    public String Decode(String input)
    {
        var bytes = Converter.FromHex(input, ConvertType.BASE64);
        var result = Convert.FromBase64String(bytes);
        return Convert.ToHexString (result);
    }
}
