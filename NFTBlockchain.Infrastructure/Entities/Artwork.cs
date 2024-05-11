namespace NFTBlockchain.Infrastructure.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

[Table("artwork_metadata")]
[Index(nameof(ArtworkHash), IsUnique = true, Name = "ARTWORK_HASH_UNIQUE_INDEX")]
[PrimaryKey(nameof(Id))]
public class Artwork
{
    [Column("id")]
    public long Id { get; set; }
    
    [Column("artwork_hash")]
    public string ArtworkHash { get; set; }
    
    [Column("title")]
    public string Title { get; set; }
}