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
    [Migration("20231026145123_vacio")]
    partial class vacio
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

                    b.Property<int>("PlanId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PlanId");

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

                    b.Property<int>("Puntuacion")
                        .HasColumnType("int");

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

                    b.Property<string>("Nombre")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

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

                    b.Property<int>("planId")
                        .HasColumnType("int");

                    b.Property<int>("usuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("planId");

                    b.HasIndex("usuarioId");

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

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("RolId")
                        .HasColumnType("int");

                    b.Property<int>("Telefono")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RolId");

                    b.ToTable("Usuario", (string)null);
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Wod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("DiaId")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DiaId");

                    b.ToTable("Wod");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.WodXEjercicio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("GrupoEjercicioId")
                        .HasColumnType("int");

                    b.Property<int?>("GrupoEjerciciosId")
                        .HasColumnType("int");

                    b.Property<int>("WodId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GrupoEjerciciosId");

                    b.HasIndex("WodId");

                    b.ToTable("WodXEjercicio");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Dia", b =>
                {
                    b.HasOne("MVCCoachBuster.Models.Plan", "Plan")
                        .WithMany("Dias")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Plan");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Plan", b =>
                {
                    b.HasOne("MVCCoachBuster.Models.Usuario", "Entrenador")
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Entrenador");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Suscripcion", b =>
                {
                    b.HasOne("MVCCoachBuster.Models.Plan", "plan")
                        .WithMany()
                        .HasForeignKey("planId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MVCCoachBuster.Models.Usuario", "usuario")
                        .WithMany()
                        .HasForeignKey("usuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("plan");

                    b.Navigation("usuario");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Usuario", b =>
                {
                    b.HasOne("MVCCoachBuster.Models.Rol", "Rol")
                        .WithMany("Usuarios")
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Wod", b =>
                {
                    b.HasOne("MVCCoachBuster.Models.Dia", "Dia")
                        .WithMany("Wods")
                        .HasForeignKey("DiaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dia");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.WodXEjercicio", b =>
                {
                    b.HasOne("MVCCoachBuster.Models.GrupoEjercicios", "GrupoEjercicios")
                        .WithMany("WodXEjercicio")
                        .HasForeignKey("GrupoEjerciciosId");

                    b.HasOne("MVCCoachBuster.Models.Wod", "Wod")
                        .WithMany("WodXEjercicio")
                        .HasForeignKey("WodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GrupoEjercicios");

                    b.Navigation("Wod");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Dia", b =>
                {
                    b.Navigation("Wods");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.GrupoEjercicios", b =>
                {
                    b.Navigation("WodXEjercicio");
                });

            modelBuilder.Entity("MVCCoachBuster.Models.Plan", b =>
                {
                    b.Navigation("Dias");
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
