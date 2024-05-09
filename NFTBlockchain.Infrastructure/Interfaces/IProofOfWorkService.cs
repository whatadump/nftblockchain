namespace NFTBlockchain.Infrastructure.Interfaces;

internal interface IProofOfWorkService<T> where T : IProofOfWork
{
    T Proof(int height, T block);
}