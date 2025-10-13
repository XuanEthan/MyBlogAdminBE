using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBlogAdminService.Migrations
{
    /// <inheritdoc />
    public partial class FixPostEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Paragraph",
                table: "Posts",
                newName: "Content");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Posts",
                newName: "Paragraph");
        }
    }
}
