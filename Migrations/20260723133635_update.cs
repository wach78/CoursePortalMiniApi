using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoursePortalMiniApi.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Courses_Name_StartDate",
                table: "Courses",
                columns: new[] { "Name", "StartDate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Courses_Name_StartDate",
                table: "Courses");
        }
    }
}
