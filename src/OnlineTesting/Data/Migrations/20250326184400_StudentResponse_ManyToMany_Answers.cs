using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTesting.Data.Migrations
{
    /// <inheritdoc />
    public partial class StudentResponse_ManyToMany_Answers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentResponses_Answers_AnswerId",
                table: "StudentResponses");

            migrationBuilder.DropIndex(
                name: "IX_StudentResponses_AnswerId",
                table: "StudentResponses");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "StudentResponses");

            migrationBuilder.CreateTable(
                name: "StudentResponseAnswers",
                columns: table => new
                {
                    AnswerId = table.Column<int>(type: "int", nullable: false),
                    StudentResponseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentResponseAnswers", x => new { x.AnswerId, x.StudentResponseId });
                    table.ForeignKey(
                        name: "FK_StudentResponseAnswers_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentResponseAnswers_StudentResponses_StudentResponseId",
                        column: x => x.StudentResponseId,
                        principalTable: "StudentResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentResponseAnswers_StudentResponseId",
                table: "StudentResponseAnswers",
                column: "StudentResponseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentResponseAnswers");

            migrationBuilder.AddColumn<int>(
                name: "AnswerId",
                table: "StudentResponses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StudentResponses_AnswerId",
                table: "StudentResponses",
                column: "AnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentResponses_Answers_AnswerId",
                table: "StudentResponses",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
