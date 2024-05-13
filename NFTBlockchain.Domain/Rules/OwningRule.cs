namespace NFTBlockchain.Domain.Rules;

using Infrastructure.Interfaces;
using Infrastructure.Models;

class OwningRule : IRule<NFTBlock>
{
    public void Execute(IEnumerable<Block<NFTBlock>> builtBlocks, Block<NFTBlock> newData)
    {
        var block = newData.Data;
        if (block.Data.From == block.Data.To)
        {
            if (builtBlocks.Any(x => x.Data.Data.WorkOfArt == block.Data.WorkOfArt))
                throw new ApplicationException(
                    "Вы пытаетесь зарегистрировать уже зарегистированное произведение искусства.");
        }
        else
        {
            foreach (var b in builtBlocks.Reverse())
            {
                if (b.Data.Data.WorkOfArt == newData.Data.Data.WorkOfArt)
                {
                    if (b.Data.Data.To == block.Data.From)
                        return;
                    throw new ApplicationException(
                        "Вы пытаетесь передать произведение искусства которое вам не пренадлежит.");
                }
            }

            throw new ApplicationException("Вы пытаетесь передать произведение искусства которое еще не было зарегистрировано.");
        }
    }
}