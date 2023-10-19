using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCCoachBuster.Migrations
{
    public partial class FotoBytes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Foto",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "FotoBytes",
                table: "Planes",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoBytes",
                table: "Planes");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Foto",
                table: "Usuario",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
