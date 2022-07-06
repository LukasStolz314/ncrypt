using System.Security.Cryptography;

namespace ncrypt.Domain;

public record AESResult(String PlainText, String CipherText,
    String Key, String IV, CipherMode Mode);
