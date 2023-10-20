using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCCoachBuster.Migrations
{
    public partial class Suscripcion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<int>(
                name: "planId",
                table: "Suscripcion",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "usuarioId",
                table: "Suscripcion",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suscripcion_planId",
                table: "Suscripcion",
                column: "planId");

            migrationBuilder.CreateIndex(
                name: "IX_Suscripcion_usuarioId",
                table: "Suscripcion",
                column: "usuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Suscripcion_Planes_planId",
                table: "Suscripcion",
                column: "planId",
                principalTable: "Planes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suscripcion_Usuario_usuarioId",
                table: "Suscripcion",
                column: "usuarioId",
                principalTable: "Usuario",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suscripcion_Planes_planId",
                table: "Suscripcion");

            migrationBuilder.DropForeignKey(
                name: "FK_Suscripcion_Usuario_usuarioId",
                table: "Suscripcion");

            migrationBuilder.DropIndex(
                name: "IX_Suscripcion_planId",
                table: "Suscripcion");

            migrationBuilder.DropIndex(
                name: "IX_Suscripcion_usuarioId",
                table: "Suscripcion");

            migrationBuilder.DropColumn(
                name: "planId",
                table: "Suscripcion");

            migrationBuilder.DropColumn(
                name: "usuarioId",
                table: "Suscripcion");

            migrationBuilder.AddColumn<byte[]>(
                name: "FotoBytes",
                table: "Planes",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
