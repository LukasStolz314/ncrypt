using ncrypt.Domain;
using ncrypt.Library.Hash;
using System.Security.Cryptography;
using System.Text;

namespace ncrypt.Library.Cipher;

[RenderUI (Class = RenderClass.Cipher)]
public class RSAService
{
    [RenderUI]
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

    [RenderUI]
    public String Encrypt(String publicKey, String input)
    {
        Byte[] resultBytes;
        String asciiKey = Converter.FromHex(publicKey, ConvertType.ASCII);
        using (RSACryptoServiceProvider rsa = new ())
        {
            rsa.ImportFromPem(asciiKey.ToCharArray());
            var dataToEncrypt = Converter.ToByteArray (input, ConvertType.HEX);
            resultBytes = rsa.Encrypt (dataToEncrypt, false);
        }

        String result = Converter.ToString (resultBytes, ConvertType.HEX);
        return result;
    }

    [RenderUI]
    public String Decrypt(String privateKey, String input)
    {
        Byte[] resultBytes;
        String asciiKey = Converter.FromHex(privateKey, ConvertType.ASCII);
        using (RSACryptoServiceProvider rsa = new ())
        {
            rsa.ImportFromPem(asciiKey.ToCharArray());
            var dataToDecrypt = Converter.ToByteArray (input, ConvertType.HEX);
            resultBytes = rsa.Decrypt (dataToDecrypt, false);
        }

        String result = Converter.ToString (resultBytes, ConvertType.HEX);
        return result;
    }

    [RenderUI]
    public String Sign(String privateKey, String input, HashType hash)
    {
        Byte[] resultBytes;
        using (RSACryptoServiceProvider rsa = new())
        {
            String asciiKey = Converter.FromHex(privateKey, ConvertType.ASCII);
            rsa.ImportFromPem (asciiKey.ToCharArray ());
            var dataToSign = Converter.ToByteArray (input, ConvertType.ASCII);
            resultBytes = rsa.SignData (dataToSign, SHAService.GetHashInstance(hash));
        }

        String result = Converter.ToString (resultBytes, ConvertType.HEX);
        return result;
    }

    [RenderUI]
    public String Verify(String publicKey, String input, String signature, HashType hash)
    {
        Boolean result;
        using (RSACryptoServiceProvider rsa = new())
        {
            String asciiKey = Converter.FromHex(publicKey, ConvertType.ASCII);
            rsa.ImportFromPem (asciiKey.ToCharArray ());
            var dataToVerify = Converter.ToByteArray (input, ConvertType.ASCII);
            var signatureToVerify = Converter.ToByteArray (signature, ConvertType.HEX);
            result = rsa.VerifyData (dataToVerify, SHAService.GetHashInstance(hash), signatureToVerify);
        }

        return result == true ? "Valid" : "Invalid";
    }
}
