namespace NFTBlockchain.Infrastructure.Models;

public record struct Block<T>(string Hash, string ParentHash, string Raw, T Data);