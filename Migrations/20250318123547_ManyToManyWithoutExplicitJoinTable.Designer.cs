﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TunaPiano.Migrations
{
    [DbContext(typeof(TunaPianoDbContext))]
    [Migration("20250318123547_ManyToManyWithoutExplicitJoinTable")]
    partial class ManyToManyWithoutExplicitJoinTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GenreSong", b =>
                {
                    b.Property<int>("GenresId")
                        .HasColumnType("integer");

                    b.Property<int>("SongsId")
                        .HasColumnType("integer");

                    b.HasKey("GenresId", "SongsId");

                    b.HasIndex("SongsId");

                    b.ToTable("SongGenre", (string)null);
                });

            modelBuilder.Entity("TunaPiano.Models.Artist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("integer");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Artists");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Age = 30,
                            Bio = "A passionate musician blending jazz and rock.",
                            Name = "John Doe"
                        },
                        new
                        {
                            Id = 2,
                            Age = 27,
                            Bio = "An indie-pop singer with a soulful voice.",
                            Name = "Lisa Ray"
                        },
                        new
                        {
                            Id = 3,
                            Age = 35,
                            Bio = "A Latin music sensation known for upbeat rhythms.",
                            Name = "Carlos Mendez"
                        },
                        new
                        {
                            Id = 4,
                            Age = 40,
                            Bio = "A classical pianist with modern interpretations.",
                            Name = "Sarah Williams"
                        },
                        new
                        {
                            Id = 5,
                            Age = 33,
                            Bio = "A rock guitarist and vocalist.",
                            Name = "Derek Blake"
                        });
                });

            modelBuilder.Entity("TunaPiano.Models.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Indie"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Rock"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Jazz"
                        },
                        new
                        {
                            Id = 4,
                            Description = "Latin"
                        },
                        new
                        {
                            Id = 5,
                            Description = "Pop"
                        });
                });

            modelBuilder.Entity("TunaPiano.Models.Song", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Album")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ArtistId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Length")
                        .HasColumnType("numeric");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.ToTable("Songs");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Album = "Night Rhythms",
                            ArtistId = 1,
                            Length = 3.45m,
                            Title = "Midnight Groove"
                        },
                        new
                        {
                            Id = 2,
                            Album = "Lost & Found",
                            ArtistId = 2,
                            Length = 4.12m,
                            Title = "Wandering Souls"
                        },
                        new
                        {
                            Id = 3,
                            Album = "Dance Fever",
                            ArtistId = 3,
                            Length = 3.33m,
                            Title = "Fiesta Latina"
                        },
                        new
                        {
                            Id = 4,
                            Album = "Classical Revivals",
                            ArtistId = 4,
                            Length = 5.20m,
                            Title = "Moonlit Sonata"
                        },
                        new
                        {
                            Id = 5,
                            Album = "Voltage High",
                            ArtistId = 5,
                            Length = 4.01m,
                            Title = "Electric Surge"
                        });
                });

            modelBuilder.Entity("GenreSong", b =>
                {
                    b.HasOne("TunaPiano.Models.Genre", null)
                        .WithMany()
                        .HasForeignKey("GenresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TunaPiano.Models.Song", null)
                        .WithMany()
                        .HasForeignKey("SongsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TunaPiano.Models.Song", b =>
                {
                    b.HasOne("TunaPiano.Models.Artist", null)
                        .WithMany("Songs")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TunaPiano.Models.Artist", b =>
                {
                    b.Navigation("Songs");
                });
#pragma warning restore 612, 618
        }
    }
}
