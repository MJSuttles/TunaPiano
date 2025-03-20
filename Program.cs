using System.ComponentModel;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using TunaPiano.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// allows passing datetimes wihtout time zone data
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<TunaPianoDbContext>(builder.Configuration["TunaPianoDbConnectionString"]);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Endpoints

// Song Calls

// Get All Songs

app.MapGet("/api/songs", (TunaPianoDbContext db) =>
{
    return db.Songs
        .Include(s => s.Genres)
        .ToList();
});

// Get a Single Song with associated genres and artist details

app.MapGet("/api/songs/{id}", (TunaPianoDbContext db, int id) =>
{
    Song? song = db.Songs
        .Include(s => s.Artist)
        .Include(s => s.Genres)
        .SingleOrDefault(s => s.Id == id);

    if (song == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(song);
});

// Create a Song

app.MapPost("/api/songs", (TunaPianoDbContext db, Song newSong) =>
{
    // ✅ Ensure the artist exists
    bool artistExists = db.Artists.Any(a => a.Id == newSong.ArtistId);
    if (!artistExists)
    {
        return Results.BadRequest("Artist not found.");
    }

    db.Songs.Add(newSong);
    db.SaveChanges();

    return Results.Created($"/api/songs/{newSong.Id}", newSong);
});


// Update a Song

app.MapPut("/api/songs/{id}", async (TunaPianoDbContext db, int id, Song song) =>
{
    Song updatedSong = db.Songs.SingleOrDefault(s => s.Id == id);
    if (updatedSong == null)
    {
        return Results.NotFound();
    }

    updatedSong.Title = song.Title;
    updatedSong.ArtistId = song.ArtistId;
    updatedSong.Album = song.Album;
    updatedSong.Length = song.Length;

    db.SaveChanges();
    return Results.NoContent();
});

// Delete a Song

app.MapDelete("/api/songs/{id}", (TunaPianoDbContext db, int id) =>
{
    Song song = db.Songs.SingleOrDefault(s => s.Id == id);
    if (song == null)
    {
        return Results.NotFound();
    }

    db.Songs.Remove(song);
    db.SaveChanges();
    return Results.NoContent();
});

// Artist Calls

// Get All Artists

app.MapGet("/api/artists", (TunaPianoDbContext db) =>
{
    return db.Artists.ToList();
});

// Get a Single Artist with associated songs

app.MapGet("/api/artists/{id}", (TunaPianoDbContext db, int id) =>
{
    Artist? artist = db.Artists
    .Include(a => a.Songs)
    .SingleOrDefault(a => a.Id == id);

    if (artist == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(artist);
});

// Create an Artist

app.MapPost("/api/artists", (TunaPianoDbContext db, Artist newArtist) =>
{
    db.Artists.Add(newArtist);
    db.SaveChanges();
    return Results.Created($"/api/artists/{newArtist.Id}", newArtist);
});

// Update an Artist

app.MapPut("/api/artists/{id}", (TunaPianoDbContext db, int id, Artist artist) =>
{
    Artist artistToUpdate = db.Artists.SingleOrDefault(a => a.Id == id);
    if (artistToUpdate == null)
    {
        return Results.NotFound();
    }

    artistToUpdate.Name = artist.Name;
    artistToUpdate.Age = artist.Age;
    artistToUpdate.Bio = artist.Bio;

    db.SaveChanges();
    return Results.NoContent();
});

// Delete an Artist

app.MapDelete("/api/artists/{id}", (TunaPianoDbContext db, int id) =>
{
    Artist artist = db.Artists.SingleOrDefault(a => a.Id == id);
    if (artist == null)
    {
        return Results.NotFound();
    }
    db.Artists.Remove(artist);
    db.SaveChanges();
    return Results.NoContent();
});

// Genre Calls

// Get All Genres

app.MapGet("/api/genres", (TunaPianoDbContext db) =>
{
    return db.Genres.ToList();
});

// Get a Single Genre with Its Associated Songs

app.MapGet("/api/genres/{id}", (TunaPianoDbContext db, int id) =>
{
    Genre? genre = db.Genres
        .Include(g => g.Songs)
        .SingleOrDefault(g => g.Id == id);

    if (genre == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(genre);
});

// Popular Songs by Genre

app.MapGet("/api/genres/popular", async (TunaPianoDbContext db) =>
{
    List<PopularGenreDto> popularGenres = await db.Genres
        .Select(g => new PopularGenreDto
        {
            Id = g.Id,
            Description = g.Description,
            SongCount = g.Songs.Count
        })
        .OrderByDescending(g => g.SongCount)
        .ToListAsync();

    return Results.Ok(new { genres = popularGenres });
});

// Create a Genre

app.MapPost("/api/genres", (TunaPianoDbContext db, Genre newGenre) =>
{
    db.Genres.Add(newGenre);
    db.SaveChanges();
    return Results.Created($"/api/genres/{newGenre.Id}", newGenre);
});

// Update a Genre

app.MapPut("/api/genres/{id}", (TunaPianoDbContext db, int id, Genre genre) =>
{
    Genre genreToUpdate = db.Genres.SingleOrDefault(g => g.Id == id);
    if (genreToUpdate == null)
    {
        return Results.NotFound();
    }

    genreToUpdate.Description = genre.Description;

    db.SaveChanges();
    return Results.NoContent();
});

// Delete a Genre

app.MapDelete("/api/genres/{id}", (TunaPianoDbContext db, int id) =>
{
    Genre genre = db.Genres.SingleOrDefault(g => g.Id == id);
    if (genre == null)
    {
        return Results.NotFound();
    }

    db.Genres.Remove(genre);
    db.SaveChanges();
    return Results.NoContent();
});

// Search Calls

app.MapGet("/api/songs/genre/{genreName}", async (TunaPianoDbContext db, string genreName) =>
{
    // ✅ Fetch songs directly by genre name (case-insensitive)
    List<SongDto> songs = await db.Songs
        .Where(s => s.Genres.Any(g => g.Description.ToLower() == genreName.ToLower())) // ✅ Use Description, not Id
        .Select(s => new SongDto
        {
            Id = s.Id,
            Title = s.Title,
            ArtistId = s.ArtistId,
            Album = s.Album,
            Length = s.Length
        })
        .ToListAsync();

    if (!songs.Any())
    {
        return Results.NotFound($"No songs found for genre '{genreName}'.");
    }

    return Results.Ok(new { songs });
});



app.Run();
