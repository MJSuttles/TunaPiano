using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TunaPiano.Migrations
{
    /// <inheritdoc />
    public partial class ManyToManyWithoutExplicitJoinTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GenreSong_Genres_GenresId",
                table: "GenreSong");

            migrationBuilder.DropForeignKey(
                name: "FK_GenreSong_Songs_SongsId",
                table: "GenreSong");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GenreSong",
                table: "GenreSong");

            migrationBuilder.RenameTable(
                name: "GenreSong",
                newName: "SongGenre");

            migrationBuilder.RenameIndex(
                name: "IX_GenreSong_SongsId",
                table: "SongGenre",
                newName: "IX_SongGenre_SongsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SongGenre",
                table: "SongGenre",
                columns: new[] { "GenresId", "SongsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SongGenre_Genres_GenresId",
                table: "SongGenre",
                column: "GenresId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SongGenre_Songs_SongsId",
                table: "SongGenre",
                column: "SongsId",
                principalTable: "Songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SongGenre_Genres_GenresId",
                table: "SongGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_SongGenre_Songs_SongsId",
                table: "SongGenre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SongGenre",
                table: "SongGenre");

            migrationBuilder.RenameTable(
                name: "SongGenre",
                newName: "GenreSong");

            migrationBuilder.RenameIndex(
                name: "IX_SongGenre_SongsId",
                table: "GenreSong",
                newName: "IX_GenreSong_SongsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GenreSong",
                table: "GenreSong",
                columns: new[] { "GenresId", "SongsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GenreSong_Genres_GenresId",
                table: "GenreSong",
                column: "GenresId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GenreSong_Songs_SongsId",
                table: "GenreSong",
                column: "SongsId",
                principalTable: "Songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
