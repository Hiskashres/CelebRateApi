using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CelebRateApi.Migrations
{
    /// <inheritdoc />
    public partial class addCelebLastName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "CelebTranslations",
                newName: "lastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "CelebTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "CelebTranslations");

            migrationBuilder.RenameColumn(
                name: "lastName",
                table: "CelebTranslations",
                newName: "Name");
        }
    }
}
