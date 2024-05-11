namespace NFTBlockchain.Domain.Rules;

using Infrastructure.Models;
using NFTBlockchain.Infrastructure.Interfaces;

class ProofOfWorkRule<T> : IProofOfWorkRule<T> where T : IProofOfWork
{
    public void Execute(IEnumerable<Block<T>> builtBlocks, Block<T> newData)
    {
        var height = builtBlocks.Count();
        if (Execute(height, newData.Hash))
            throw new ApplicationException("Proof of work is incorrect for this block.");
    }

    public bool Execute(int height, string hash)
    {
        return hash.StartsWith('0');
        
        // Захардкодил ноль впереди чтобы вечность не считать POW
        // Возвращается на место при необходимости
        
        var complexity = (int)(Math.Log2(height + 1) + 1);
        for (int i = 0; i < complexity; i++)
        {
            if (hash[i] != '0')
                return false;
        }

        return true;
    }
}