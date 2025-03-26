using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTesting.Data.Migrations
{
    /// <inheritdoc />
    public partial class StudentTest_CurrentQuestionIndex_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentQuestionIndex",
                table: "StudentTests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentQuestionIndex",
                table: "StudentTests");
        }
    }
}
