using ncrypt.Domain;
using ncrypt.Library.Hash;
using System.Security.Cryptography;
using System.Text;

namespace ncrypt.Library.Cipher;

[SelectableService]
public class RSAService
{
    [SelectableFunction]
    public String GenerateKeyPair(Int32 keySize)
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
        publicSB.AppendLine ("-----BEGIN RSA PUBLIC KEY-----");
        publicSB.AppendLine (Converter.ToStringWithFixedLineLength (publicKey, 64));
        publicSB.AppendLine ("-----END RSA PUBLIC KEY-----");

        // Write private key to pem format
        StringBuilder privateSB = new ();
        privateSB.AppendLine ("-----BEGIN RSA PRIVATE KEY-----");
        privateSB.AppendLine (Converter.ToStringWithFixedLineLength (privateKey, 64));
        privateSB.AppendLine ("-----END RSA PRIVATE KEY-----");

        // Return result object
        StringBuilder result = new ();
        result.AppendLine (publicSB.ToString ());
        result.AppendLine ("");
        result.AppendLine (privateSB.ToString ());

        return result.ToString();
    }

    [SelectableFunction]
    public String Encrypt(String publicKey, String data)
    {
        Byte[] resultBytes;
        using (RSACryptoServiceProvider rsa = new ())
        {
            rsa.ImportFromPem(publicKey.ToCharArray());
            var dataToEncrypt = Converter.ToByteArray (data, ConvertType.ASCII);
            resultBytes = rsa.Encrypt (dataToEncrypt, false);
        }

        return Converter.ToString(resultBytes, ConvertType.HEX);
    }

    [SelectableFunction]
    public String Decrypt(String privateKey, String data)
    {
        Byte[] resultBytes;
        using (RSACryptoServiceProvider rsa = new ())
        {
            rsa.ImportFromPem(privateKey.ToCharArray());
            var dataToDecrypt = Converter.ToByteArray (data, ConvertType.HEX);
            resultBytes = rsa.Decrypt (dataToDecrypt, false);
        }

        return Converter.ToString(resultBytes, ConvertType.ASCII);
    }

    [SelectableFunction]
    public String Sign(String privateKey, String data, HashType halg)
    {
        Byte[] resultBytes;
        using (RSACryptoServiceProvider rsa = new())
        {
            rsa.ImportFromPem (privateKey.ToCharArray ());
            var dataToSign = Converter.ToByteArray (data, ConvertType.ASCII);
            resultBytes = rsa.SignData (dataToSign, SHAService.GetHashInstance(halg));
        }

        String result = Converter.ToString (resultBytes, ConvertType.HEX);
        return result;
    }

    [SelectableFunction]
    public Boolean Verify(String publicKey, String data, String signature, HashType halg)
    {
        Boolean result;
        using (RSACryptoServiceProvider rsa = new())
        {
            rsa.ImportFromPem (publicKey.ToCharArray ());
            var dataToVerify = Converter.ToByteArray (data, ConvertType.ASCII);
            var signatureToVerify = Converter.ToByteArray (signature, ConvertType.HEX);
            result = rsa.VerifyData (dataToVerify, SHAService.GetHashInstance(halg), signatureToVerify);
        }

        return result;
    }
}
