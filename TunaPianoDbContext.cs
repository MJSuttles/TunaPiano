using Microsoft.EntityFrameworkCore;
using TunaPiano.Models;

public class TunaPianoDbContext : DbContext
{
  public DbSet<Artist> Artists { get; set; }
  public DbSet<Song> Songs { get; set; }
  public DbSet<SongGenre> SongGenres { get; set; }
  public DbSet<Genre> Genres { get; set; }

  public TunaPianoDbContext(DbContextOptions<TunaPianoDbContext> context) : base(context)
  {

  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {

    // Seed Data
  }
}
