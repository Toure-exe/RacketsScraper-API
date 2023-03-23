﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RacketsScrapper.Infrastructure;

#nullable disable

namespace RacketsScrapper.Infrastructure.Migrations
{
    [DbContext(typeof(RacketDbContext))]
    partial class RacketDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RacketsScrapper.Domain.Racket", b =>
                {
                    b.Property<int>("RacketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RacketId"));

                    b.Property<string>("Anno")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Bilanciamento")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ColoreDue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ColoreUno")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Eta")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Forma")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LivelloDiGioco")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lunghezza")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nucleo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumeroArticolo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Peso")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Prezzo")
                        .HasColumnType("float");

                    b.Property<int>("Profilo")
                        .HasColumnType("int");

                    b.Property<int>("PuntoDiEquilibrio")
                        .HasColumnType("int");

                    b.Property<string>("Sesso")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telaio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TipoDiGioco")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TipoDiProdotto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RacketId");

                    b.ToTable("Rackets");
                });
#pragma warning restore 612, 618
        }
    }
}
