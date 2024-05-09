namespace NFTBlockchain.Domain.Models;

using Infrastructure.Interfaces;

internal record NFTBlock(NFTTransfer Data, string Sign, int Nonce) : ISigned<NFTTransfer>, IProofOfWork
{
    public string PublicKey => Data.From;
}