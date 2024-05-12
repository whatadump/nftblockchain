namespace NFTBlockchain.Infrastructure.Models;

using NFTBlockchain.Infrastructure.Interfaces;

public record NFTBlock(NFTTransfer Data, string Sign, int Nonce) : ISigned<NFTTransfer>, IProofOfWork
{
    public string PublicKey => Data.From;
}