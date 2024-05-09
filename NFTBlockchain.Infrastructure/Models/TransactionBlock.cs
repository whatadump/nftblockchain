namespace NFTBlockchain.Infrastructure.Models;

using Interfaces;

internal record TransactionBlock(Transaction Data, string Sign, int Nonce) : ISigned<Transaction>, IProofOfWork
{
    public string PublicKey => Data.From;
}