namespace NFTBlockchain.Infrastructure.Interfaces;

using Models;

internal interface IBlockchain : IEnumerable<BlockchainBlock>
{
    void AddBlock(BlockchainBlock data);
}