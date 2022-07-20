using System.Security.Cryptography;

namespace ncrypt.Library.Cipher;

[SelectableService]
public class AESService
{
    private Byte[] _key;
    private CipherMode _mode;

    public AESService(Byte[] key, CipherMode mode)
    {
        _key = key;
        _mode = mode;
    }

    [SelectableFunction]
    public Byte[] Encrypt(String input, Byte[] iv)
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
        return resultBytes;
    }

    [SelectableFunction]
    public String Decrypt(Byte[] input, Byte[] iv)
    {
        //Create aes with given parameters
        Aes aes = CreateAes (iv);
        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        // Decrypt cipher text with generated decryptor
        String result;
        using (MemoryStream ms = new (input))
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

    private Aes CreateAes(Byte[] iv)
    {
        Aes aes = Aes.Create ();
        aes.Key = _key;
        aes.Mode = _mode;
        aes.IV = iv;

        return aes;
    }
}
