namespace NFTBlockchain.Infrastructure.Interfaces;

using DTO;
using Entities;

public interface IArtworkService
{
    public Task<ValueTuple<string?, string>> RegisterArtwork(Stream filestream, string title, string privateKey, ApplicationUser user);

    public Task<ArtworkDTO?> GetArtworkByHash(string hash);

    public Task<IReadOnlyCollection<ArtworkDTO>> GetAllArtworks();

    public Task<string?> TransferArtwork(string privateKey, ApplicationUser? sender, ApplicationUser? recipient, string artwork);

    public Task<IReadOnlyCollection<ArtworkDTO>?> GetArtworksByAuthor(string authorId);

    public Task<ApplicationUser?> GetAuthorByPublicKeyHash(string authorPublicKey);

    public Task<bool> IsUserArbiter(ApplicationUser user);
}