using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCCoachBuster.Migrations
{
    public partial class PlanFoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Foto",
                table: "Planes",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Planes");
        }
    }
}
