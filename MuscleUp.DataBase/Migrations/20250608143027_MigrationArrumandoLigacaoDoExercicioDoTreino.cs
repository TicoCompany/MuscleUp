using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuscleUp.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class MigrationArrumandoLigacaoDoExercicioDoTreino : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciciosDoTreino_GruposMuscularesTrabalhados_GrupoMuscular~",
                table: "ExerciciosDoTreino");

            migrationBuilder.DropIndex(
                name: "IX_ExerciciosDoTreino_GrupoMuscularTrabalhadoId",
                table: "ExerciciosDoTreino");

            migrationBuilder.DropColumn(
                name: "GrupoMuscularTrabalhadoId",
                table: "ExerciciosDoTreino");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciciosDoTreino_IdMembroTrabalhado",
                table: "ExerciciosDoTreino",
                column: "IdMembroTrabalhado");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciciosDoTreino_GruposMuscularesTrabalhados_IdMembroTraba~",
                table: "ExerciciosDoTreino",
                column: "IdMembroTrabalhado",
                principalTable: "GruposMuscularesTrabalhados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciciosDoTreino_GruposMuscularesTrabalhados_IdMembroTraba~",
                table: "ExerciciosDoTreino");

            migrationBuilder.DropIndex(
                name: "IX_ExerciciosDoTreino_IdMembroTrabalhado",
                table: "ExerciciosDoTreino");

            migrationBuilder.AddColumn<int>(
                name: "GrupoMuscularTrabalhadoId",
                table: "ExerciciosDoTreino",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExerciciosDoTreino_GrupoMuscularTrabalhadoId",
                table: "ExerciciosDoTreino",
                column: "GrupoMuscularTrabalhadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciciosDoTreino_GruposMuscularesTrabalhados_GrupoMuscular~",
                table: "ExerciciosDoTreino",
                column: "GrupoMuscularTrabalhadoId",
                principalTable: "GruposMuscularesTrabalhados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
