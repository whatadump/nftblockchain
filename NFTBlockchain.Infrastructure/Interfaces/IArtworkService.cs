namespace NFTBlockchain.Infrastructure.Interfaces;

using DTO;
using Entities;

public interface IArtworkService
{
    public Task<ValueTuple<string?, string>> RegisterArtwork(Stream filestream, string title, string privateKey, ApplicationUser user);

    public Task<ArtworkDTO?> GetArtworkByHash(string hash);

    public Task<IReadOnlyCollection<ArtworkDTO>> GetAllArtworks();
}