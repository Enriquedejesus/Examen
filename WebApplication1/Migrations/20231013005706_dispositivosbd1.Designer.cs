﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication1.Models;

#nullable disable

namespace WebApplication1.Migrations
{
    [DbContext(typeof(DispositivoDBContext))]
    [Migration("20231013005706_dispositivosbd1")]
    partial class dispositivosbd1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebApplication1.Models.Dispositivo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Estatus")
                        .HasColumnType("int");

                    b.Property<decimal>("Latitud")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("LitrosRegistrados")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Longitud")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Nombre")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Prioridad")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Dispositivos");
                });
#pragma warning restore 612, 618
        }
    }
}
