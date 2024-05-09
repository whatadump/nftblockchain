namespace NFTBlockchain.Infrastructure.Interfaces;

internal interface IProofOfWorkRule<T> : IRule<T> where T : IProofOfWork
{
    bool Execute(int height, string hash);
}