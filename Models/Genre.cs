using System.ComponentModel.DataAnnotations;

namespace TunaPiano.Models;

public class GenreId
{
  public int Id { get; set; }
  public string Description { get; set; }
  public List<SongGenre> SongGenres { get; set; }
}
