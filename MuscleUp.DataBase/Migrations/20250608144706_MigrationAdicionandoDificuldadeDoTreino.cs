using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuscleUp.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class MigrationAdicionandoDificuldadeDoTreino : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DificuldadeDoTreino",
                table: "Treinos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DificuldadeDoTreino",
                table: "Treinos");
        }
    }
}
