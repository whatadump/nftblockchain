namespace NFTBlockchain.Domain.Services;

using System.Collections;
using Infrastructure.Interfaces;
using Infrastructure.Models;

class Blockchain : IBlockchain
{
    private readonly List<BlockchainBlock> _blocks = [];
    private readonly IHashFunction _hashFunction;

    public Blockchain(IHashFunction hashFunction)
    {
        _hashFunction = hashFunction;
    }
    
    public void AddBlock(BlockchainBlock block)
    {
        var tail = _blocks.LastOrDefault();
        if (block.ParentHash == tail?.Hash)
        {
            var expectedHash = BlockchainBlock.CalculateHash(_hashFunction, block.Data, block.ParentHash);
            if (expectedHash == block.Hash)
                _blocks.Add(block);
            else
                throw new ApplicationException($"У блока {block} неверный хэш. Ожидаемый хэш: {expectedHash}.");
        }
        else
            throw new ApplicationException($"{block.ParentHash} не следует за {tail} текущего блока");
    }

    public IEnumerator<BlockchainBlock> GetEnumerator() => _blocks.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}