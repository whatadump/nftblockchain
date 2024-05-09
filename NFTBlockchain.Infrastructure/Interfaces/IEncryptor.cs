namespace NFTBlockchain.Infrastructure.Interfaces;

public interface IEncryptor
{
    KeyPair GenerateKeys();
    string Sign(string data, string privateKey);
    bool VerifySign(string publicKey, string data, string sign);
}