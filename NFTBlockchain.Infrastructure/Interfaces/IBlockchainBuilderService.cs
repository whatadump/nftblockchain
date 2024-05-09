namespace NFTBlockchain.Infrastructure.Interfaces;

using Models;

internal interface IBlockchainBuilderService
{
    void AcceptBlock(BlockchainBlock block);
    BlockchainBlock BuildBlock(string data);
}