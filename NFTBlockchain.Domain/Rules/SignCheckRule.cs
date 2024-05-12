namespace NFTBlockchain.Domain.Rules;

using System.Text.Json;
using Infrastructure.Models;
using NFTBlockchain.Infrastructure.Interfaces;

class SignCheckRule<TBlockData, TSignedData> : IRule<TBlockData> where TBlockData : ISigned<TSignedData>
{
    private readonly IEncryptor _encryptor;
    
    public SignCheckRule(IEncryptor encryptor)
    {
        _encryptor = encryptor;
    }
    
    public void Execute(IEnumerable<Block<TBlockData>> builtBlocks, Block<TBlockData> newData)
    {
        var signed = newData.Data;
        var dataThatShouldBeSigned = JsonSerializer.Serialize(newData.Data.Data);
        if (!_encryptor.VerifySign(signed.PublicKey, dataThatShouldBeSigned, signed.Sign))
            throw new ApplicationException("Неверная подпись блока.");
    }
}