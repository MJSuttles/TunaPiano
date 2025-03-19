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



// Update an Artist



// Delete an Artist



// Genre Calls

// Get All Genres



// Create a Genre



// Update a Genre



// Delete a Genre



app.Run();
