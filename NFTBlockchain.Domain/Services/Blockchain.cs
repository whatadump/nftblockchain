namespace NFTBlockchain.Domain.Services;

using System.Collections;
using System.Text.Json;
using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

class Blockchain : IBlockchain
{
    private List<BlockchainBlock> _blocks = [];
    private readonly IHashFunction _hashFunction;
    private readonly ILogger<Blockchain> _logger;
    private readonly NFTFileOptions _options;

    public Blockchain(IHashFunction hashFunction)
    {
        _hashFunction = hashFunction;
        _logger = Application.ServiceProvider.GetRequiredService<ILogger<Blockchain>>();
        _options = Application.ServiceProvider.GetRequiredService<NFTFileOptions>();
        
        ReadFromFile(_options.BlockchainFilename);
    }
    
    public void AddBlock(BlockchainBlock block)
    {
        var tail = _blocks.LastOrDefault();
        if (block.ParentHash == tail?.Hash)
        {
            var expectedHash = BlockchainBlock.CalculateHash(_hashFunction, block.Data, block.ParentHash);
            if (expectedHash == block.Hash)
            {
                _blocks.Add(block);
                WriteToFile(_options.BlockchainFilename);
            }
            else
                throw new ApplicationException($"У блока {block} неверный хэш. Ожидаемый хэш: {expectedHash}.");
        }
        else
            throw new ApplicationException($"{block.ParentHash} не следует за {tail} текущего блока");
    }

    public void ReadFromFile(string filename)
    {
        try
        {
            var fileStream = File.OpenRead(filename);
            _blocks = JsonSerializer.Deserialize<List<BlockchainBlock>>(fileStream) ?? throw new Exception();
        }
        catch (Exception e)
        {
            _logger.LogError("Не удалось прочитать файл цепочки блоков. Используется пустая цепочка");
        }
    }

    public void WriteToFile(string filename)
    {
        var stream = File.OpenWrite(filename);
        JsonSerializer.Serialize(stream, _blocks);
    }

    public IEnumerator<BlockchainBlock> GetEnumerator() => _blocks.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}