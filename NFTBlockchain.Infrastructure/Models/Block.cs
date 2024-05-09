namespace NFTBlockchain.Infrastructure.Models;

internal record struct Block<T>(string Hash, string ParentHash, string Raw, T Data);