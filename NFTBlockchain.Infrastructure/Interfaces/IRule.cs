namespace NFTBlockchain.Infrastructure.Interfaces;

using Models;

internal interface IRule<T>
{
    void Execute(IEnumerable<Block<T>> builtBlocks, Block<T> newData);
}