using System.Text;

namespace ncrypt.Library;

public enum ConvertType
{
    HEX,
    BASE64,
    ASCII
}

public class Converter
{
    public static String ToString (Byte[] key, ConvertType type) => type switch
    {
        ConvertType.BASE64 => Convert.ToBase64String (key),
        ConvertType.ASCII => Encoding.ASCII.GetString (key),
        ConvertType.HEX => Convert.ToHexString (key),
        _ => throw new NotImplementedException ()
    };

    public static String OtherString(String input, ConvertType type)
    {
        String result = input;
        if(type == ConvertType.HEX)
        {
            Byte[] bytes = Encoding.Default.GetBytes (input);
            String hex = BitConverter.ToString (bytes);
            result = hex.Replace ("-", "");
        }

        return result;
    }

    public static String ToStringWithFixedLineLength(String text, Int32 lineLength)
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

    public static Byte[] ToByteArray (String key, ConvertType type) => type switch
    {
        ConvertType.BASE64 => Convert.FromBase64String (key),
        ConvertType.ASCII => Encoding.ASCII.GetBytes (key),
        ConvertType.HEX => Convert.FromHexString (key),
        _ => throw new NotImplementedException ()
    };
}
