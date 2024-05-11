namespace NFTBlockchain.Infrastructure.Interfaces;

using Entities;

public interface IArtworkService
{
    public Task<string?> RegisterArtwork(Stream filestream, string title, string privateKey, ApplicationUser user);
}