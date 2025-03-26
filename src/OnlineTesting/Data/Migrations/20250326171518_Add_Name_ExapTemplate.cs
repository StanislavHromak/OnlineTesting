using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTesting.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Name_ExapTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ExamTemplates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ExamTemplates");
        }
    }
}
