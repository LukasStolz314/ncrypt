using System.Security.Cryptography;

namespace ncrypt.Library.Cipher;

[RenderUI (Class = RenderClass.Cipher)]
public class AESService
{
    private Byte[] _key;
    private CipherMode _mode;

    public AESService(String key, CipherMode mode)
    {
        _key = Converter.ToByteArray (key, ConvertType.HEX);
        _mode = mode;
    }

    [RenderUI]
    public String Encrypt(String input, String iv)
    {
        // Create aes with given parameters
        Aes aes = CreateAes (iv);
        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        input = Converter.FromHex (input, ConvertType.ASCII);

        // Encrypt plain text with generated encryptor
        Byte[] resultBytes;
        using (MemoryStream ms = new ())
        {
            using (CryptoStream cs = new (ms, encryptor, CryptoStreamMode.Write))
            {
                using (StreamWriter sw = new (cs))
                {
                    sw.Write(input);
                }
            }

            resultBytes = ms.ToArray ();
        }

        // Return result object
        String result = Converter.ToString (resultBytes, ConvertType.HEX);
        return result;
    }

    [RenderUI]
    public String Decrypt(String input, String iv)
    {
        //Create aes with given parameters
        Aes aes = CreateAes (iv);
        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        // Decrypt cipher text with generated decryptor
        String result;
        using (MemoryStream ms = new (Converter.ToByteArray (input, ConvertType.HEX)))
        {
            using (CryptoStream cs = new (ms, decryptor, CryptoStreamMode.Read))
            {
                using (StreamReader sr = new (cs))
                {
                    result = sr.ReadToEnd ();
                }
            }
        }

        result = Converter.ToHex(result, ConvertType.ASCII);
        return result;
    }

    private Aes CreateAes(String iv)
    {
        Aes aes = Aes.Create ();
        aes.Key = _key;
        aes.Mode = _mode;
        aes.IV = Converter.ToByteArray (iv, ConvertType.HEX);

        return aes;
    }
}
