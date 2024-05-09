namespace NFTBlockchain.Infrastructure.Interfaces;

public interface IHashFunction
{
    public string GetHash(string data);
}