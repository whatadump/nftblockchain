using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NFTBlockchain.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToArtworkHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ARTWORK_HASH_UNIQUE_INDEX",
                table: "artwork_metadata",
                column: "artwork_hash",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ARTWORK_HASH_UNIQUE_INDEX",
                table: "artwork_metadata");
        }
    }
}
