using System.Security.Cryptography;

namespace ncrypt.Library.Hash;

public enum HashType
{
    MD5,
    SHA1,
    SHA256,
    SHA384,
    SHA512,
}

[RenderUI (Class = RenderClass.Hash)]
public class SHAService
{
    public static HashAlgorithm GetHashInstance (HashType type) => type switch
    {
        HashType.MD5 => MD5.Create (),
        HashType.SHA1 => SHA1.Create (),
        HashType.SHA256 => SHA256.Create (),
        HashType.SHA384 => SHA384.Create (),
        HashType.SHA512 => SHA512.Create (),
        _ => throw new NotImplementedException (),
    };

    [RenderUI]
    public String Hash(String data, HashType type)
    {
        Byte[] resultBytes;
        using (var alg = GetHashInstance (type))
        {
            var dataToHash = Converter.ToByteArray (data, ConvertType.ASCII);
            resultBytes = alg.ComputeHash (dataToHash);
        }

        String result = Converter.ToString (resultBytes, ConvertType.HEX);
        return result;
    }
}
