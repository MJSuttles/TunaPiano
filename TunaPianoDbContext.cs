using Microsoft.EntityFrameworkCore;
using TunaPiano.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using TunaPiano.Migrations;

public class TunaPianoDbContext : DbContext
{
  public DbSet<Artist> Artists { get; set; }
  public DbSet<Song> Songs { get; set; }
  public DbSet<Genre> Genres { get; set; }

  public TunaPianoDbContext(DbContextOptions<TunaPianoDbContext> context) : base(context)
  {

  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {

    // ✅ SEED THE MANY-TO-MANY RELATIONSHIP (SongGenre)
    modelBuilder.Entity<Song>()
        .HasMany(s => s.Genres)
        .WithMany(g => g.Songs)
        .UsingEntity(j => j.HasData(
            new { SongsId = 1, GenresId = 2 }, // "Midnight Groove" -> "Rock"
            new { SongsId = 1, GenresId = 3 }, // "Midnight Groove" -> "Jazz"
            new { SongsId = 2, GenresId = 1 }, // "Wandering Souls" -> "Indie"
            new { SongsId = 3, GenresId = 4 }, // "Fiesta Latina" -> "Latin"
            new { SongsId = 3, GenresId = 5 }, // "Fiesta Latina" -> "Pop"
            new { SongsId = 4, GenresId = 3 }, // "Moonlit Sonata" -> "Jazz"
            new { SongsId = 5, GenresId = 2 }, // "Electric Surge" -> "Rock"
            new { SongsId = 5, GenresId = 1 }  // "Electric Surge" -> "Indie"
        )); ;

    // Seed Data
    modelBuilder.Entity<Artist>().HasData(new Artist[]
    {
      new Artist { Id = 1, Name = "John Doe", Age = 30, Bio = "A passionate musician blending jazz and rock." },
      new Artist { Id = 2, Name = "Lisa Ray", Age = 27, Bio = "An indie-pop singer with a soulful voice." },
      new Artist { Id = 3, Name = "Carlos Mendez", Age = 35, Bio = "A Latin music sensation known for upbeat rhythms." },
      new Artist { Id = 4, Name = "Sarah Williams", Age = 40, Bio = "A classical pianist with modern interpretations." },
      new Artist { Id = 5, Name = "Derek Blake", Age = 33, Bio = "A rock guitarist and vocalist." }
    });

    modelBuilder.Entity<Song>().HasData(new Song[]
    {
      new Song { Id = 1, Title = "Midnight Groove", ArtistId = 1, Album = "Night Rhythms", Length = 3.45m },
      new Song { Id = 2, Title = "Wandering Souls", ArtistId = 2, Album = "Lost & Found", Length = 4.12m },
      new Song { Id = 3, Title = "Fiesta Latina", ArtistId = 3, Album = "Dance Fever", Length = 3.33m },
      new Song { Id = 4, Title = "Moonlit Sonata", ArtistId = 4, Album = "Classical Revivals", Length = 5.20m },
      new Song { Id = 5, Title = "Electric Surge", ArtistId = 5, Album = "Voltage High", Length = 4.01m }
    });

    modelBuilder.Entity<Genre>().HasData(new Genre[]
    {
      new Genre { Id = 1, Description = "Indie" },
      new Genre { Id = 2, Description = "Rock" },
      new Genre { Id = 3, Description = "Jazz" },
      new Genre { Id = 4, Description = "Latin" },
      new Genre { Id = 5, Description = "Pop" }
    });
  }
}
