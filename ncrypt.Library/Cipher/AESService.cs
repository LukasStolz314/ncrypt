using ncrypt.Domain;
using System.Security.Cryptography;

namespace ncrypt.Library.Cipher;

[SelectableService]
public class AESService
{
    private Byte[] _key;
    private CipherMode _mode;
    private ConvertType _inputType;
    private ConvertType _outputType;

    public AESService(String key, CipherMode mode,
        ConvertType inputType, ConvertType outputType)
    {
        _key = Converter.ToByteArray (key, inputType);
        _mode = mode;
        _inputType = inputType;
        _outputType = outputType;
    }

    [SelectableFunction]
    public String Encrypt(String input, String iv)
    {
        // Create aes with given parameters
        Aes aes = CreateAes (iv);
        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

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
        String result = Converter.ToString (resultBytes, _outputType);
        return result;
    }

    [SelectableFunction]
    public String Decrypt(String input, String iv)
    {
        //Create aes with given parameters
        Aes aes = CreateAes (iv);
        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        // Decrypt cipher text with generated decryptor
        String result;
        using (MemoryStream ms = new (Converter.ToByteArray (input, _inputType)))
        {
            using (CryptoStream cs = new (ms, decryptor, CryptoStreamMode.Read))
            {
                using (StreamReader sr = new (cs))
                {
                    result = sr.ReadToEnd ();
                }
            }
        }

        return result;
    }

    private Aes CreateAes(String iv)
    {
        Aes aes = Aes.Create ();
        aes.Key = _key;
        aes.Mode = _mode;
        aes.IV = Converter.ToByteArray (iv, _inputType);

        return aes;
    }
}
