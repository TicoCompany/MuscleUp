using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuscleUp.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class MigrationAdicionandoEstruturaDoTreino : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Treinos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdAluno = table.Column<int>(type: "int", nullable: true),
                    IdAcademia = table.Column<int>(type: "int", nullable: false),
                    IdProfessor = table.Column<int>(type: "int", nullable: true),
                    Nome = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Divisao = table.Column<int>(type: "int", nullable: false),
                    Publico = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Tempo = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treinos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Treinos_Academias_IdAcademia",
                        column: x => x.IdAcademia,
                        principalTable: "Academias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Treinos_Alunos_IdAluno",
                        column: x => x.IdAluno,
                        principalTable: "Alunos",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Treinos_Usuarios_IdProfessor",
                        column: x => x.IdProfessor,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GruposMuscularesTrabalhados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdTreino = table.Column<int>(type: "int", nullable: false),
                    GrupoMuscular = table.Column<int>(type: "int", nullable: false),
                    DivisaoDeTreino = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GruposMuscularesTrabalhados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GruposMuscularesTrabalhados_Treinos_IdTreino",
                        column: x => x.IdTreino,
                        principalTable: "Treinos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TreinosPublicosEDestinadosDoAluno",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdTreino = table.Column<int>(type: "int", nullable: false),
                    IdProfessorQueDestinou = table.Column<int>(type: "int", nullable: true),
                    IdAluno = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreinosPublicosEDestinadosDoAluno", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreinosPublicosEDestinadosDoAluno_Alunos_IdAluno",
                        column: x => x.IdAluno,
                        principalTable: "Alunos",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreinosPublicosEDestinadosDoAluno_Treinos_IdTreino",
                        column: x => x.IdTreino,
                        principalTable: "Treinos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreinosPublicosEDestinadosDoAluno_Usuarios_IdProfessorQueDes~",
                        column: x => x.IdProfessorQueDestinou,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ExerciciosDoTreino",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdExercicio = table.Column<int>(type: "int", nullable: false),
                    IdMembroTrabalhado = table.Column<int>(type: "int", nullable: false),
                    Repeticao = table.Column<int>(type: "int", nullable: false),
                    Serie = table.Column<int>(type: "int", nullable: false),
                    GrupoMuscularTrabalhadoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciciosDoTreino", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciciosDoTreino_Exercicios_IdExercicio",
                        column: x => x.IdExercicio,
                        principalTable: "Exercicios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciciosDoTreino_GruposMuscularesTrabalhados_GrupoMuscular~",
                        column: x => x.GrupoMuscularTrabalhadoId,
                        principalTable: "GruposMuscularesTrabalhados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciciosDoTreino_GrupoMuscularTrabalhadoId",
                table: "ExerciciosDoTreino",
                column: "GrupoMuscularTrabalhadoId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciciosDoTreino_IdExercicio",
                table: "ExerciciosDoTreino",
                column: "IdExercicio");

            migrationBuilder.CreateIndex(
                name: "IX_GruposMuscularesTrabalhados_IdTreino",
                table: "GruposMuscularesTrabalhados",
                column: "IdTreino");

            migrationBuilder.CreateIndex(
                name: "IX_Treinos_IdAcademia",
                table: "Treinos",
                column: "IdAcademia");

            migrationBuilder.CreateIndex(
                name: "IX_Treinos_IdAluno",
                table: "Treinos",
                column: "IdAluno");

            migrationBuilder.CreateIndex(
                name: "IX_Treinos_IdProfessor",
                table: "Treinos",
                column: "IdProfessor");

            migrationBuilder.CreateIndex(
                name: "IX_TreinosPublicosEDestinadosDoAluno_IdAluno",
                table: "TreinosPublicosEDestinadosDoAluno",
                column: "IdAluno");

            migrationBuilder.CreateIndex(
                name: "IX_TreinosPublicosEDestinadosDoAluno_IdProfessorQueDestinou",
                table: "TreinosPublicosEDestinadosDoAluno",
                column: "IdProfessorQueDestinou");

            migrationBuilder.CreateIndex(
                name: "IX_TreinosPublicosEDestinadosDoAluno_IdTreino",
                table: "TreinosPublicosEDestinadosDoAluno",
                column: "IdTreino");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciciosDoTreino");

            migrationBuilder.DropTable(
                name: "TreinosPublicosEDestinadosDoAluno");

            migrationBuilder.DropTable(
                name: "GruposMuscularesTrabalhados");

            migrationBuilder.DropTable(
                name: "Treinos");
        }
    }
}
