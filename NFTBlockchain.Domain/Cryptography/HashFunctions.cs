namespace NFTBlockchain.Domain.Cryptography;

using System.Security.Cryptography;
using System.Text;
using Force.Crc32;
using Infrastructure.Interfaces;

public class SHA256Hash : IHashFunction
{
    public string GetHash(string data)
    {
        var sha = SHA256.HashData(Encoding.UTF8.GetBytes(data));
        return string.Concat(sha.Select(x => $"{x:x2}"));
    }
}