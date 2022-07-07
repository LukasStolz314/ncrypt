using ncrypt.Domain;
using System.Security.Cryptography;
using System.Text;

namespace ncrypt.Library.Cipher;

public class RSACipher
{
    public RSAKeyPairResult GenerateKeyPair(Int32 keySize)
    {
        // Create key pair and export to Base64 String
        String privateKey;
        String publicKey;
        using(RSA rsa = RSA.Create(keySize))
        {
            privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey ());
            publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey ());
        }

        // Write public key to pem format
        StringBuilder publicSB = new ();
        publicSB.AppendLine ("-----BEGIN PUBLIC KEY-----");
        publicSB.AppendLine (Converter.ToStringWithFixedLineLength (publicKey, 64));
        publicSB.AppendLine ("-----END PUBLIC KEY-----");

        // Write private key to pem format
        StringBuilder privateSB = new ();
        privateSB.AppendLine ("-----BEGIN RSA PRIVATE KEY-----");
        privateSB.AppendLine (Converter.ToStringWithFixedLineLength (privateKey, 64));
        privateSB.AppendLine ("-----END RSA PRIVATE KEY-----");

        // Return result object
        return new (publicSB.ToString (), privateSB.ToString (), keySize);
    }
}
