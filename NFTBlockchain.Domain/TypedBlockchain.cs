namespace NFTBlockchain.Domain;

using System.Collections;
using System.Text.Json;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Services;

internal class TypedBlockchain<T> : ITypedBlockchain<T>
{
    private readonly IBlockchain _blockchain;
    private readonly IBlockchainBuilderService _builderService;
    private readonly IRule<T>[] _businessRules;

    public TypedBlockchain(IBlockchain blockchain, IHashFunction hashFunction, params IRule<T>[] businessRules)
    {
        _blockchain = blockchain;
        _builderService = new BlockchainBuilderService(hashFunction, _blockchain.LastOrDefault()?.Hash);
        _businessRules = businessRules;
    }

    public Block<T> BuildBlock(T data)
    {
        var dataStr = JsonSerializer.Serialize(data);
        var block = _builderService.BuildBlock(dataStr);
        var typedBlock = new Block<T>(block.Hash, block.ParentHash, dataStr, data);
        return typedBlock;
    }

    public void AcceptBlock(Block<T> typedBlock)
    {
        foreach (var rule in _businessRules)
        {
            rule.Execute(this, typedBlock);
        }

        var block = _builderService.BuildBlock(typedBlock.Raw);
        _blockchain.AddBlock(block);
        _builderService.AcceptBlock(block);
    }

    public IEnumerator<Block<T>> GetEnumerator()
    {
        return _blockchain.Select(x => new Block<T>(x.Hash, x.ParentHash, x.Data, JsonSerializer.Deserialize<T>(x.Data)!))
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}