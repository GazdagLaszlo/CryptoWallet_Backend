﻿// <auto-generated />
using System;
using CryptoWallet.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CryptoWallet.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250508045522_DatabaseChanges")]
    partial class DatabaseChanges
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CryptoWallet.Model.Cryptocurrency", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)")
                        .HasAnnotation("Relational:JsonPropertyName", "symbol");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float")
                        .HasAnnotation("Relational:JsonPropertyName", "current_price");

                    b.Property<long>("TotalSupply")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Cryptocurrencies");
                });

            modelBuilder.Entity("CryptoWallet.Model.PortfolioItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<string>("CryptocurrencyId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("WalletId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CryptocurrencyId");

                    b.HasIndex("WalletId");

                    b.ToTable("PortfolioItems");
                });

            modelBuilder.Entity("CryptoWallet.Model.PriceHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CryptocurrencyId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CryptocurrencyId");

                    b.ToTable("PriceHistories");
                });

            modelBuilder.Entity("CryptoWallet.Model.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<string>("CryptocurrencyId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CryptocurrencyId");

                    b.HasIndex("UserId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("CryptoWallet.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CryptoWallet.Model.Wallet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("CryptoWallet.Model.PortfolioItem", b =>
                {
                    b.HasOne("CryptoWallet.Model.Cryptocurrency", "Cryptocurrency")
                        .WithMany()
                        .HasForeignKey("CryptocurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CryptoWallet.Model.Wallet", "Wallet")
                        .WithMany("Cryptocurrencies")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cryptocurrency");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("CryptoWallet.Model.PriceHistory", b =>
                {
                    b.HasOne("CryptoWallet.Model.Cryptocurrency", "Cryptocurrency")
                        .WithMany("PriceHistories")
                        .HasForeignKey("CryptocurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cryptocurrency");
                });

            modelBuilder.Entity("CryptoWallet.Model.Transaction", b =>
                {
                    b.HasOne("CryptoWallet.Model.Cryptocurrency", "Cryptocurrency")
                        .WithMany()
                        .HasForeignKey("CryptocurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CryptoWallet.Model.User", "User")
                        .WithMany("Transactions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cryptocurrency");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CryptoWallet.Model.Wallet", b =>
                {
                    b.HasOne("CryptoWallet.Model.User", "User")
                        .WithOne("Wallet")
                        .HasForeignKey("CryptoWallet.Model.Wallet", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CryptoWallet.Model.Cryptocurrency", b =>
                {
                    b.Navigation("PriceHistories");
                });

            modelBuilder.Entity("CryptoWallet.Model.User", b =>
                {
                    b.Navigation("Transactions");

                    b.Navigation("Wallet")
                        .IsRequired();
                });

            modelBuilder.Entity("CryptoWallet.Model.Wallet", b =>
                {
                    b.Navigation("Cryptocurrencies");
                });
#pragma warning restore 612, 618
        }
    }
}
