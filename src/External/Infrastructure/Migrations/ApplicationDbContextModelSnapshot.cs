﻿// <auto-generated />
using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Games.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<string>("DownloadLink")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("FileSize")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Publisher")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Games");

                    b.HasDiscriminator().HasValue("Game");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Domain.Orders.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("StatusId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Domain.Orders.OrderItem", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("Domain.Orders.OrderStatus", b =>
                {
                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Value");

                    b.ToTable("OrderStatuses");

                    b.HasData(
                        new
                        {
                            Value = 0,
                            Name = "Pending"
                        },
                        new
                        {
                            Value = 1,
                            Name = "Paid"
                        },
                        new
                        {
                            Value = 2,
                            Name = "Canceled"
                        });
                });

            modelBuilder.Entity("Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Login")
                        .IsUnique();

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasDiscriminator().HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Domain.Games.DlcGame", b =>
                {
                    b.HasBaseType("Domain.Games.Game");

                    b.Property<Guid>("BaseGameId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("BaseGameId");

                    b.HasDiscriminator().HasValue("DlcGame");
                });

            modelBuilder.Entity("Domain.Games.FullGame", b =>
                {
                    b.HasBaseType("Domain.Games.Game");

                    b.HasDiscriminator().HasValue("FullGame");
                });

            modelBuilder.Entity("Domain.Games.Subscription", b =>
                {
                    b.HasBaseType("Domain.Games.Game");

                    b.Property<int>("PeriodInDays")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Subscription");
                });

            modelBuilder.Entity("Domain.Users.Admin", b =>
                {
                    b.HasBaseType("Domain.Users.User");

                    b.HasDiscriminator().HasValue("Admin");
                });

            modelBuilder.Entity("Domain.Users.Customer", b =>
                {
                    b.HasBaseType("Domain.Users.User");

                    b.HasDiscriminator().HasValue("Customer");
                });

            modelBuilder.Entity("Domain.Games.Game", b =>
                {
                    b.OwnsOne("Domain.ValueObjects.Money", "Price", b1 =>
                        {
                            b1.Property<Guid>("GameId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasPrecision(18, 3)
                                .HasColumnType("decimal(18,3)")
                                .HasColumnName("Amount");

                            b1.HasKey("GameId");

                            b1.ToTable("Games");

                            b1.WithOwner()
                                .HasForeignKey("GameId");

                            b1.OwnsOne("Domain.Enums.Currency", "Currency", b2 =>
                                {
                                    b2.Property<Guid>("MoneyGameId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<int>("Value")
                                        .HasColumnType("int")
                                        .HasColumnName("Currency");

                                    b2.HasKey("MoneyGameId");

                                    b2.ToTable("Games");

                                    b2.WithOwner()
                                        .HasForeignKey("MoneyGameId");
                                });

                            b1.Navigation("Currency")
                                .IsRequired();
                        });

                    b.Navigation("Price")
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Orders.Order", b =>
                {
                    b.HasOne("Domain.Users.Customer", null)
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Orders.OrderStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Status");
                });

            modelBuilder.Entity("Domain.Orders.OrderItem", b =>
                {
                    b.HasOne("Domain.Games.Game", null)
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Orders.Order", null)
                        .WithMany("Items")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Domain.ValueObjects.Money", "Price", b1 =>
                        {
                            b1.Property<Guid>("OrderItemId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasPrecision(18, 3)
                                .HasColumnType("decimal(18,3)")
                                .HasColumnName("Amount");

                            b1.HasKey("OrderItemId");

                            b1.ToTable("OrderItems");

                            b1.WithOwner()
                                .HasForeignKey("OrderItemId");

                            b1.OwnsOne("Domain.Enums.Currency", "Currency", b2 =>
                                {
                                    b2.Property<Guid>("MoneyOrderItemId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<int>("Value")
                                        .HasColumnType("int")
                                        .HasColumnName("Currency");

                                    b2.HasKey("MoneyOrderItemId");

                                    b2.ToTable("OrderItems");

                                    b2.WithOwner()
                                        .HasForeignKey("MoneyOrderItemId");
                                });

                            b1.Navigation("Currency")
                                .IsRequired();
                        });

                    b.Navigation("Price")
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Games.DlcGame", b =>
                {
                    b.HasOne("Domain.Games.FullGame", "BaseGame")
                        .WithMany("DlcGames")
                        .HasForeignKey("BaseGameId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("BaseGame");
                });

            modelBuilder.Entity("Domain.Orders.Order", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Domain.Games.FullGame", b =>
                {
                    b.Navigation("DlcGames");
                });
#pragma warning restore 612, 618
        }
    }
}
