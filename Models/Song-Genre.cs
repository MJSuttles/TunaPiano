using System.ComponentModel.DataAnnotations;

namespace TunaPiano.Models;

public class SongGenre
{
  public int Id { get; set; }
  public int SongId { get; set; }
  public int GenreId { get; set; }
}
