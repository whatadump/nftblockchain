namespace NFTBlockchain.Infrastructure.Interfaces;

using Models;

internal interface ITypedBlockchain<T> : IEnumerable<Block<T>>
{
    Block<T> BuildBlock(T data);
    void AcceptBlock(Block<T> typedBlock);
}