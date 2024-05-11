namespace NFTBlockchain.Domain.Apps;

using System.Text.Json;
using Cryptography;
using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Models;
using Rules;
using Services;

public class NFTApp
{
    private readonly ITypedBlockchain<NFTBlock> _blockchain;
    private readonly IEncryptor _encryptor;
    private readonly IProofOfWorkService<NFTBlock> _proofOfWorkService;

    public NFTApp()
    {
        var hashFunction = new SHA256Hash();
        var lowLevelBlockchain = new Blockchain(hashFunction);
        var proofOfWorkRule = new ProofOfWorkRule<NFTBlock>();
        _encryptor = new RSAEncryptor();
        _blockchain = new TypedBlockchain<NFTBlock>(lowLevelBlockchain, hashFunction,
            new SignCheckRule<NFTBlock, NFTTransfer>(_encryptor),
            new OwningRule(),
            proofOfWorkRule);
        _proofOfWorkService =
            new ProofOfWorkService<NFTBlock>(_blockchain, x => x with { Nonce = x.Nonce + 1 }, proofOfWorkRule);
    }

    public void RegisterWorkOfArt(KeyPair author, string workOfArt)
        => TransferWorkOfArt(author, author.PublicKey, workOfArt);

    public void TransferWorkOfArt(KeyPair from, string to, string workOfArt)
    {
        var block = new NFTTransfer(workOfArt, from.PublicKey, to);
        var transactionString = JsonSerializer.Serialize(block);
        var sign = _encryptor.Sign(transactionString, from.PrivateKey);
        var nftBlock = _proofOfWorkService.Proof(_blockchain.Count(), new NFTBlock(block, sign, 0));
        var result = _blockchain.BuildBlock(nftBlock);
        _blockchain.AcceptBlock(result);
    }

    public bool WorkOfArtExists(string workOfArt) => _blockchain.Any(x => x.Data.Data.WorkOfArt == workOfArt);
}

