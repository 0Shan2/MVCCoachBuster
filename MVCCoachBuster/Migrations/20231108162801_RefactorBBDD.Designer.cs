﻿// <auto-generated />
using System;
using MVCCoachBuster.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MVCCoachBuster.Migrations
{
    [DbContext(typeof(CoachBusterContext))]
    [Migration("20231108162801_RefactorBBDD")]
    partial class RefactorBBDD
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.23")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MVCCoachBuster.Models.Dia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("IdPlan")
                        .HasColumnType("int");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdPlan");

                    b.ToTable("Dia");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.GrupoEjercicios", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Instrucciones")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("URLVideo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GrupoEjercicios", (string)null);
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Plan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Descripcion")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Foto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Planes", (string)null);
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Rol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("Id");

                    b.ToTable("Rol", (string)null);
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Suscripcion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("IdPlan")
                        .HasColumnType("int");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdPlan");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Suscripcion", (string)null);
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Contrasena")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Foto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdRol")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("Telefono")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdRol");

                    b.ToTable("Usuario", (string)null);
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Wod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("IdDia")
                        .HasColumnType("int");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdDia");

                    b.ToTable("Wod");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.WodXEjercicio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("IdGrupoEjercicios")
                        .HasColumnType("int");

                    b.Property<int>("IdWod")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdGrupoEjercicios");

                    b.HasIndex("IdWod");

                    b.ToTable("WodXEjercicio");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Dia", b =>
                {
                    b.HasOne("MVCCoachBuster.Models.Plan", "Plan")
                        .WithMany("Dia")
                        .HasForeignKey("IdPlan")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Plan");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Plan", b =>
                {
                    b.HasOne("MVCCoachBuster.Models.Usuario", "UsuEntrenador")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UsuEntrenador");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Suscripcion", b =>
                {
                    b.HasOne("MVCCoachBuster.Models.Plan", "Plan")
                        .WithMany()
                        .HasForeignKey("IdPlan")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MVCCoachBuster.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Plan");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Usuario", b =>
                {
                    b.HasOne("MVCCoachBuster.Models.Rol", "Rol")
                        .WithMany("Usuarios")
                        .HasForeignKey("IdRol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Wod", b =>
                {
                    b.HasOne("MVCCoachBuster.Models.Dia", "Dia")
                        .WithMany("Wod")
                        .HasForeignKey("IdDia");

                    b.Navigation("Dia");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.WodXEjercicio", b =>
                {
                    b.HasOne("MVCCoachBuster.Models.GrupoEjercicios", "GrupoEjercicios")
                        .WithMany("WodXEjercicio")
                        .HasForeignKey("IdGrupoEjercicios")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MVCCoachBuster.Models.Wod", "Wod")
                        .WithMany("WodXEjercicio")
                        .HasForeignKey("IdWod")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GrupoEjercicios");

                    b.Navigation("Wod");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Dia", b =>
                {
                    b.Navigation("Wod");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.GrupoEjercicios", b =>
                {
                    b.Navigation("WodXEjercicio");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Plan", b =>
                {
                    b.Navigation("Dia");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Rol", b =>
                {
                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Wod", b =>
                {
                    b.Navigation("WodXEjercicio");
                });
#pragma warning restore 612, 618
        }
    }
}
