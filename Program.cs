using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;

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

// Get a Single Song wiht associated genres and artist details



// Create a Song



// Update a Song



// Delete a Song



// Artist Calls

// Get All Artists



// Get a Single Artist with associated songs



// Create an Artist



// Update an Artist



// Delete an Artist



// Genre Calls

// Get All Genres



// Create a Genre



// Update a Genre



// Delete a Genre



app.Run();
