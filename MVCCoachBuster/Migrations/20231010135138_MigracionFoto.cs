using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCCoachBuster.Migrations
{
    public partial class MigracionFoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Foto",
                table: "Usuario",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Usuario");
        }
    }
}
