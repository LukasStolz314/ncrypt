using System.Text;

namespace ncrypt.Library.Code;

[SelectableService]
public class HexService
{
    [SelectableFunction]
    public String Encode(Byte[] input)
    {
        return Convert.ToHexString (input);
    }

    [SelectableFunction]
    public String Decode(Byte[] input)
    {
        var value = Encoding.UTF8.GetString (input);
        var result = Convert.FromHexString (value);
        return Encoding.UTF8.GetString (result);
    }
}
