using System.Text;

namespace ncrypt.Library;

public enum ConvertType
{
    HEX,
    UTF8,
    ASCII
}

internal class Converter
{
    internal static String ToString (Byte[] key, ConvertType type) => type switch
    {
        ConvertType.UTF8 => Encoding.UTF8.GetString (key),
        ConvertType.ASCII => Encoding.ASCII.GetString (key),
        ConvertType.HEX => Convert.ToHexString (key),
        _ => throw new NotImplementedException ()
    };

    internal static String ToStringWithFixedLineLength(String text, Int32 lineLength)
    {
        String remainedText = text;
        StringBuilder sb = new();
        
        while(remainedText.Count() > lineLength)
        {
            sb.AppendLine (new(remainedText.Take (lineLength).ToArray()));
            remainedText = remainedText.Remove (0, lineLength);
        }

        sb.Append (remainedText);

        return sb.ToString();
    }

    internal static Byte[] ToByteArray (String key, ConvertType type) => type switch
    {
        ConvertType.UTF8 => Encoding.UTF8.GetBytes (key),
        ConvertType.ASCII => Encoding.ASCII.GetBytes (key),
        ConvertType.HEX => Convert.FromHexString (key),
        _ => throw new NotImplementedException ()
    };
}
