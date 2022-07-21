using System.Text;

namespace ncrypt.Library.Code;

[RenderUI (Class = RenderClass.Format)]
public class Base64Service
{
    [RenderUI]
    public String Encode(String input)
    {
        var bytes = Encoding.ASCII.GetBytes (input);
        return Convert.ToBase64String (bytes);
    }

    [RenderUI]
    public String Decode(String input)
    {
        var bytes = Convert.FromBase64String (input);
        return Encoding.ASCII.GetString (bytes);
    }
}
