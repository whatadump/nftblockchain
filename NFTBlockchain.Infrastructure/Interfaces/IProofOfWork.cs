namespace NFTBlockchain.Infrastructure.Interfaces;

internal interface IProofOfWork
{
    int Nonce { get; }
}