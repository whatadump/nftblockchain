namespace NFTBlockchain.Domain.Services;

using System.Security.Cryptography;
using Apps;
using Infrastructure;
using Infrastructure.DTO;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

public class ArtworkService : IArtworkService
{
    private readonly NFTApp _app;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;
    private readonly IHashFunction _hashFunction;

    public ArtworkService(NFTApp app, ApplicationDbContext context, IHashFunction hashFunction, IConfiguration config)
    {
        _app = app;
        _context = context;
        _hashFunction = hashFunction;
        _config = config;
    }
    
    public async Task<ValueTuple<string?, string>> RegisterArtwork(Stream filestream, string title, string privateKey, ApplicationUser user)
    {
        if (user is null)
        {
            return ("Неверный пользователь", string.Empty);
        }

        if (user.PrivateKeyHash != _hashFunction.GetHash(privateKey))
        {
            return ("Передан неверный закрытый ключ", string.Empty);
        }

        try
        {
            var pair = new KeyPair(user.PublicKey, privateKey);
            
            using var image = await Image.LoadAsync(filestream);
            using var memoryStream = new MemoryStream();
            await image.SaveAsync(memoryStream, new PngEncoder());
            memoryStream.Position = 0;
            var pngBytes = memoryStream.ToArray();
            memoryStream.Position = 0;
            var imageHashBytes = await SHA256.HashDataAsync(memoryStream);
            var imageHash = string.Concat(imageHashBytes.Select(x => $"{x:x2}"));

            _app.RegisterWorkOfArt(pair, imageHash);


            var imagePath = Path.Combine(_config.GetRequiredSection("Blockchain:NFTStorageFolder").Value, $"{imageHash}.png");
            await File.WriteAllBytesAsync(imagePath, pngBytes);

            var artworkMetadata = await _context.Artworks.SingleOrDefaultAsync(x => x.ArtworkHash == imageHash);
            if (artworkMetadata is null)
            {
                artworkMetadata = new();
                artworkMetadata.ArtworkHash = imageHash;
                artworkMetadata.Title = title;
                await _context.Artworks.AddAsync(artworkMetadata);
                await _context.SaveChangesAsync();
            }
            return (null, imageHash);
        }
        catch (ApplicationException e)
        {
            return (e.Message, string.Empty);
        }
    }

    public async Task<ArtworkDTO?> GetArtworkByHash(string hash)
    {
        if (string.IsNullOrEmpty(hash))
        {
            return null;
        }

        var block = _app.Reverse().FirstOrDefault(x => x.Data.Data.WorkOfArt == hash);
        
        if (block == default)
        {
            return null;
        }

        var user = await _context.Users.SingleOrDefaultAsync(x => x.PublicKey == block.Data.Data.To);
        var artworkMeta = await _context.Artworks.SingleOrDefaultAsync(x => x.ArtworkHash == hash);
        return new ArtworkDTO(block, artworkMeta?.Title, user);
    }

    public async Task<IReadOnlyCollection<ArtworkDTO>> GetAllArtworks()
    {
        if (!_app.Any())
        {
            return [];
        }

        var artworks = _app
            .Reverse()
            .GroupBy(x => x.Data.Data.WorkOfArt)
            .Select(x => x.First())
            .ToArray();

        var artworkHashes = artworks.Select(x => x.Data.Data.WorkOfArt).ToArray();
        var artworkOwners = artworks.Select(x => x.Data.Data.To).ToArray();

        var artworkMetadata = await _context.Artworks
            .Where(x => artworkHashes.Contains(x.ArtworkHash))
            .ToDictionaryAsync(x => x.ArtworkHash, x => x);

        var owners = await _context.Users
            .Where(x => artworkOwners.Contains(x.PublicKey))
            .ToDictionaryAsync(x => x.PublicKey, x => x);

        return artworks.Select(work =>
            {
                owners.TryGetValue(work.Data.Data.To, out var owner);
                artworkMetadata.TryGetValue(work.Data.Data.WorkOfArt, out var metadata);
                return new ArtworkDTO(work, metadata?.Title, owner);
            })
            .ToArray();
    }
}