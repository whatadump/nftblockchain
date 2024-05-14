namespace NFTBlockchain.Domain.Rules;

using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Options;
using Microsoft.Extensions.DependencyInjection;

class OwningRule : IRule<NFTBlock>
{
    public void Execute(IEnumerable<Block<NFTBlock>> builtBlocks, Block<NFTBlock> newData)
    {
        var options = Application.ServiceProvider.GetRequiredService<NFTFileOptions>();
        var arbiterPublicKey = options.ArbiterPublicKey;
        
        var block = newData.Data;
        if (block.Data.From == block.Data.To)
        {
            if (builtBlocks.Any(x => x.Data.Data.WorkOfArt == block.Data.WorkOfArt))
            {   
                if (block.Data.From == arbiterPublicKey)
                {
                    throw new ApplicationException("Арбитр не может передать самому себе чужое произведение искусства");
                }
                throw new ApplicationException(
                    "Вы пытаетесь зарегистрировать уже зарегистированное произведение искусства.");
            }
                
        }
        else
        {
            foreach (var b in builtBlocks.Reverse())
            {
                if (b.Data.Data.WorkOfArt == newData.Data.Data.WorkOfArt)
                {
                    if (b.Data.Data.To == block.Data.From || b.Data.Data.From == arbiterPublicKey)
                        return;
                    throw new ApplicationException(
                        "Вы пытаетесь передать произведение искусства которое вам не пренадлежит.");
                }
            }

            throw new ApplicationException("Вы пытаетесь передать произведение искусства которое еще не было зарегистрировано.");
        }
    }
}