using System.Text;

namespace ncrypt.Library;

public enum ConvertType
{
    HEX,
    UTF8,
    ASCII
}

internal class KeyManager
{
    internal static String ToString (Byte[] key, ConvertType type) => type switch
    {
        ConvertType.UTF8 => Encoding.UTF8.GetString (key),
        ConvertType.ASCII => Encoding.ASCII.GetString (key),
        ConvertType.HEX => Convert.ToHexString (key),
        _ => throw new NotImplementedException ()
    };

    internal static Byte[] ToByteArray (String key, ConvertType type) => type switch
    {
        ConvertType.UTF8 => Encoding.UTF8.GetBytes (key),
        ConvertType.ASCII => Encoding.ASCII.GetBytes (key),
        ConvertType.HEX => Convert.FromHexString (key),
        _ => throw new NotImplementedException ()
    };
}
