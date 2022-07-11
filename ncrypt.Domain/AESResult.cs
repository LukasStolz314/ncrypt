using System.Security.Cryptography;

namespace ncrypt.Domain;

public class AESResult
{
    public String PlainText { get; init; }
    public String CipherText { get; init; }
    public String Key { get; init; }
    public String IV { get; init; }
    public CipherMode Mode { get; init; }

    public AESResult (String plainText, String cipherText, String key, String iv, CipherMode mode)
    {
        PlainText = plainText;
        CipherText = cipherText;
        Key = key;
        IV = iv;
        Mode = mode;
    }
}
