namespace NFTBlockchain.Domain.Services;

using System.Security.Cryptography;
using Apps;
using Infrastructure;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
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
    
    public async Task<string?> RegisterArtwork(Stream filestream, string title, string privateKey, ApplicationUser user)
    {
        if (user is null)
        {
            return "Неверный пользователь";
        }

        if (user.PrivateKeyHash != _hashFunction.GetHash(privateKey))
        {
            return "Передан неверный закрытый ключ";
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

            var artworkMetadata = await _context.Artworks.SingleOrDefaultAsync();
            if (artworkMetadata is null)
            {
                artworkMetadata = new();
                artworkMetadata.ArtworkHash = imageHash;
                artworkMetadata.Title = title;
                await _context.Artworks.AddAsync(artworkMetadata);
                await _context.SaveChangesAsync();
            }
        }
        catch (ApplicationException e)
        {
            return e.Message;
        }

        return null;
    }
}