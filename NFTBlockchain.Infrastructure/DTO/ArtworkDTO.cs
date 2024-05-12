namespace NFTBlockchain.Infrastructure.DTO;

using Entities;
using Models;

public record ArtworkDTO(Block<NFTBlock> Block, string? Title, ApplicationUser? Owner);