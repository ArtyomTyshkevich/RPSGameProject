﻿// <auto-generated />
using System;
using Chat.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Game.Data.Migrations
{
    [DbContext(typeof(GameDbContext))]
    partial class GameDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.36")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Game.Domain.Entities.GameRule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("FirstPlayerMove")
                        .HasColumnType("int");

                    b.Property<int>("GameResults")
                        .HasColumnType("int");

                    b.Property<int>("SecondPlayerMove")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("GameRules");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ae09c9e4-16ae-410b-8b3b-3473bfca4ba1"),
                            FirstPlayerMove = 0,
                            GameResults = 0,
                            SecondPlayerMove = 2
                        },
                        new
                        {
                            Id = new Guid("da0b28a3-8b89-4ba7-8fb8-85dfbff2283d"),
                            FirstPlayerMove = 2,
                            GameResults = 0,
                            SecondPlayerMove = 1
                        },
                        new
                        {
                            Id = new Guid("0a8f4450-6d8d-4b27-a27a-24a9a11ab42d"),
                            FirstPlayerMove = 1,
                            GameResults = 0,
                            SecondPlayerMove = 0
                        },
                        new
                        {
                            Id = new Guid("59982e94-bdab-44e8-8064-99e8bc048e94"),
                            FirstPlayerMove = 2,
                            GameResults = 2,
                            SecondPlayerMove = 0
                        },
                        new
                        {
                            Id = new Guid("79273bbd-5660-4f74-82c4-7e289df0ec30"),
                            FirstPlayerMove = 1,
                            GameResults = 2,
                            SecondPlayerMove = 2
                        },
                        new
                        {
                            Id = new Guid("3fcf738b-6f01-45fd-b1c3-870538aa0025"),
                            FirstPlayerMove = 0,
                            GameResults = 2,
                            SecondPlayerMove = 1
                        },
                        new
                        {
                            Id = new Guid("1ee056d4-62c1-4f7b-8d8f-b1cc79953def"),
                            FirstPlayerMove = 0,
                            GameResults = 1,
                            SecondPlayerMove = 0
                        },
                        new
                        {
                            Id = new Guid("2decef60-2a16-4953-9a88-4ddd42c3531c"),
                            FirstPlayerMove = 1,
                            GameResults = 1,
                            SecondPlayerMove = 1
                        },
                        new
                        {
                            Id = new Guid("e770cee5-7c6f-43fe-886d-066b9c90307d"),
                            FirstPlayerMove = 2,
                            GameResults = 1,
                            SecondPlayerMove = 2
                        });
                });

            modelBuilder.Entity("Game.Domain.Entities.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FirstPlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("GameResult")
                        .HasColumnType("int");

                    b.Property<int>("RoundNum")
                        .HasColumnType("int");

                    b.Property<Guid?>("SecondPlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Tipe")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FirstPlayerId");

                    b.HasIndex("SecondPlayerId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Game.Domain.Entities.Round", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("FirstPlayerMove")
                        .HasColumnType("int");

                    b.Property<Guid?>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("RoundResult")
                        .HasColumnType("int");

                    b.Property<int?>("SecondPlayerMove")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Rounds");
                });

            modelBuilder.Entity("Game.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Game.Domain.Entities.Room", b =>
                {
                    b.HasOne("Game.Domain.Entities.User", "FirstPlayer")
                        .WithMany()
                        .HasForeignKey("FirstPlayerId");

                    b.HasOne("Game.Domain.Entities.User", "SecondPlayer")
                        .WithMany()
                        .HasForeignKey("SecondPlayerId");

                    b.Navigation("FirstPlayer");

                    b.Navigation("SecondPlayer");
                });

            modelBuilder.Entity("Game.Domain.Entities.Round", b =>
                {
                    b.HasOne("Game.Domain.Entities.Room", null)
                        .WithMany("Rounds")
                        .HasForeignKey("RoomId");
                });

            modelBuilder.Entity("Game.Domain.Entities.Room", b =>
                {
                    b.Navigation("Rounds");
                });
#pragma warning restore 612, 618
        }
    }
}
