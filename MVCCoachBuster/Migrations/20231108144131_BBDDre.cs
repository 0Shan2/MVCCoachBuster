using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCCoachBuster.Migrations
{
    public partial class BBDDre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dia_Planes_PlanId",
                table: "Dia");

            migrationBuilder.DropForeignKey(
                name: "FK_Planes_Usuario_UsuarioId",
                table: "Planes");

            migrationBuilder.DropForeignKey(
                name: "FK_Suscripcion_Planes_planId",
                table: "Suscripcion");

            migrationBuilder.DropForeignKey(
                name: "FK_Suscripcion_Usuario_usuarioId",
                table: "Suscripcion");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Rol_RolId",
                table: "Usuario");

            migrationBuilder.DropForeignKey(
                name: "FK_Wod_Dia_DiaId",
                table: "Wod");

            migrationBuilder.DropForeignKey(
                name: "FK_WodXEjercicio_Wod_WodId",
                table: "WodXEjercicio");

            migrationBuilder.DropIndex(
                name: "IX_Planes_UsuarioId",
                table: "Planes");

            migrationBuilder.DropColumn(
                name: "Puntuacion",
                table: "GrupoEjercicios");

            migrationBuilder.RenameColumn(
                name: "GrupoEjercicioId",
                table: "WodXEjercicio",
                newName: "IdWod");

            migrationBuilder.RenameColumn(
                name: "usuarioId",
                table: "Suscripcion",
                newName: "UsuarioId");

            migrationBuilder.RenameColumn(
                name: "planId",
                table: "Suscripcion",
                newName: "PlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Suscripcion_usuarioId",
                table: "Suscripcion",
                newName: "IX_Suscripcion_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Suscripcion_planId",
                table: "Suscripcion",
                newName: "IX_Suscripcion_PlanId");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Planes",
                newName: "IdUsuario");

            migrationBuilder.AlterColumn<int>(
                name: "WodId",
                table: "WodXEjercicio",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "IdGrupoEjercicios",
                table: "WodXEjercicio",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "DiaId",
                table: "Wod",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "IdDia",
                table: "Wod",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Wod",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "RolId",
                table: "Usuario",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "IdRol",
                table: "Usuario",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "Suscripcion",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PlanId",
                table: "Suscripcion",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "IdPlan",
                table: "Suscripcion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuario",
                table: "Suscripcion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsuEntrenadorId",
                table: "Planes",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PlanId",
                table: "Dia",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "IdPlan",
                table: "Dia",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Dia",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Dia",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Planes_UsuEntrenadorId",
                table: "Planes",
                column: "UsuEntrenadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dia_Planes_PlanId",
                table: "Dia",
                column: "PlanId",
                principalTable: "Planes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Planes_Usuario_UsuEntrenadorId",
                table: "Planes",
                column: "UsuEntrenadorId",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suscripcion_Planes_PlanId",
                table: "Suscripcion",
                column: "PlanId",
                principalTable: "Planes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suscripcion_Usuario_UsuarioId",
                table: "Suscripcion",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Rol_RolId",
                table: "Usuario",
                column: "RolId",
                principalTable: "Rol",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wod_Dia_DiaId",
                table: "Wod",
                column: "DiaId",
                principalTable: "Dia",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WodXEjercicio_Wod_WodId",
                table: "WodXEjercicio",
                column: "WodId",
                principalTable: "Wod",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dia_Planes_PlanId",
                table: "Dia");

            migrationBuilder.DropForeignKey(
                name: "FK_Planes_Usuario_UsuEntrenadorId",
                table: "Planes");

            migrationBuilder.DropForeignKey(
                name: "FK_Suscripcion_Planes_PlanId",
                table: "Suscripcion");

            migrationBuilder.DropForeignKey(
                name: "FK_Suscripcion_Usuario_UsuarioId",
                table: "Suscripcion");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Rol_RolId",
                table: "Usuario");

            migrationBuilder.DropForeignKey(
                name: "FK_Wod_Dia_DiaId",
                table: "Wod");

            migrationBuilder.DropForeignKey(
                name: "FK_WodXEjercicio_Wod_WodId",
                table: "WodXEjercicio");

            migrationBuilder.DropIndex(
                name: "IX_Planes_UsuEntrenadorId",
                table: "Planes");

            migrationBuilder.DropColumn(
                name: "IdGrupoEjercicios",
                table: "WodXEjercicio");

            migrationBuilder.DropColumn(
                name: "IdDia",
                table: "Wod");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Wod");

            migrationBuilder.DropColumn(
                name: "IdRol",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "IdPlan",
                table: "Suscripcion");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "Suscripcion");

            migrationBuilder.DropColumn(
                name: "UsuEntrenadorId",
                table: "Planes");

            migrationBuilder.DropColumn(
                name: "IdPlan",
                table: "Dia");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Dia");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Dia");

            migrationBuilder.RenameColumn(
                name: "IdWod",
                table: "WodXEjercicio",
                newName: "GrupoEjercicioId");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Suscripcion",
                newName: "usuarioId");

            migrationBuilder.RenameColumn(
                name: "PlanId",
                table: "Suscripcion",
                newName: "planId");

            migrationBuilder.RenameIndex(
                name: "IX_Suscripcion_UsuarioId",
                table: "Suscripcion",
                newName: "IX_Suscripcion_usuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Suscripcion_PlanId",
                table: "Suscripcion",
                newName: "IX_Suscripcion_planId");

            migrationBuilder.RenameColumn(
                name: "IdUsuario",
                table: "Planes",
                newName: "UsuarioId");

            migrationBuilder.AlterColumn<int>(
                name: "WodId",
                table: "WodXEjercicio",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DiaId",
                table: "Wod",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RolId",
                table: "Usuario",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "usuarioId",
                table: "Suscripcion",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "planId",
                table: "Suscripcion",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Puntuacion",
                table: "GrupoEjercicios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PlanId",
                table: "Dia",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Planes_UsuarioId",
                table: "Planes",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dia_Planes_PlanId",
                table: "Dia",
                column: "PlanId",
                principalTable: "Planes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Planes_Usuario_UsuarioId",
                table: "Planes",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Suscripcion_Planes_planId",
                table: "Suscripcion",
                column: "planId",
                principalTable: "Planes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Suscripcion_Usuario_usuarioId",
                table: "Suscripcion",
                column: "usuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Rol_RolId",
                table: "Usuario",
                column: "RolId",
                principalTable: "Rol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wod_Dia_DiaId",
                table: "Wod",
                column: "DiaId",
                principalTable: "Dia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WodXEjercicio_Wod_WodId",
                table: "WodXEjercicio",
                column: "WodId",
                principalTable: "Wod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
