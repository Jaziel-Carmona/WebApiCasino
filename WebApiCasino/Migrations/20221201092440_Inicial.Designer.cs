﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApiCasino;

#nullable disable

namespace WebApiCasino.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20221201092440_Inicial")]
    partial class Inicial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WebApiCasino.Entidades.Carta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("NumeroCarta")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Cartas");
                });

            modelBuilder.Entity("WebApiCasino.Entidades.Participante", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Edad")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreParticipante")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Participantes");
                });

            modelBuilder.Entity("WebApiCasino.Entidades.ParticipanteRifaCarta", b =>
                {
                    b.Property<int>("IdParticipante")
                        .HasColumnType("int");

                    b.Property<int>("IdRifa")
                        .HasColumnType("int");

                    b.Property<int>("IdCarta")
                        .HasColumnType("int");

                    b.Property<int?>("CartaId")
                        .HasColumnType("int");

                    b.Property<int?>("ParticipanteId")
                        .HasColumnType("int");

                    b.Property<int?>("RifaId")
                        .HasColumnType("int");

                    b.HasKey("IdParticipante", "IdRifa", "IdCarta");

                    b.HasIndex("CartaId");

                    b.HasIndex("ParticipanteId");

                    b.HasIndex("RifaId");

                    b.ToTable("ParticipanteRifaCarta");
                });

            modelBuilder.Entity("WebApiCasino.Entidades.Premios", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("NombrePremio")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("NumPremio")
                        .HasColumnType("int");

                    b.Property<int>("RifaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RifaId");

                    b.ToTable("Premios");
                });

            modelBuilder.Entity("WebApiCasino.Entidades.Rifa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Fecha_Realizacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("NombreRifa")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Rifas");
                });

            modelBuilder.Entity("WebApiCasino.Entidades.ParticipanteRifaCarta", b =>
                {
                    b.HasOne("WebApiCasino.Entidades.Carta", "Carta")
                        .WithMany("ParticipanteRifaCarta")
                        .HasForeignKey("CartaId");

                    b.HasOne("WebApiCasino.Entidades.Participante", "Participante")
                        .WithMany("ParticipanteRifaCarta")
                        .HasForeignKey("ParticipanteId");

                    b.HasOne("WebApiCasino.Entidades.Rifa", "Rifa")
                        .WithMany("ParticipanteRifaCarta")
                        .HasForeignKey("RifaId");

                    b.Navigation("Carta");

                    b.Navigation("Participante");

                    b.Navigation("Rifa");
                });

            modelBuilder.Entity("WebApiCasino.Entidades.Premios", b =>
                {
                    b.HasOne("WebApiCasino.Entidades.Rifa", "Rifa")
                        .WithMany("Premios")
                        .HasForeignKey("RifaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rifa");
                });

            modelBuilder.Entity("WebApiCasino.Entidades.Carta", b =>
                {
                    b.Navigation("ParticipanteRifaCarta");
                });

            modelBuilder.Entity("WebApiCasino.Entidades.Participante", b =>
                {
                    b.Navigation("ParticipanteRifaCarta");
                });

            modelBuilder.Entity("WebApiCasino.Entidades.Rifa", b =>
                {
                    b.Navigation("ParticipanteRifaCarta");

                    b.Navigation("Premios");
                });
#pragma warning restore 612, 618
        }
    }
}
