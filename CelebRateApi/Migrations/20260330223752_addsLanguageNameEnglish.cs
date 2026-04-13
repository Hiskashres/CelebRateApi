using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CelebRateApi.Migrations
{
    /// <inheritdoc />
    public partial class addsLanguageNameEnglish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Languages_LanguageId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Languages",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "FullNameEnglish",
                table: "Languages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "LanguageId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Languages_LanguageId",
                table: "AspNetUsers",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "LanguageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Languages_LanguageId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullNameEnglish",
                table: "Languages");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Languages",
                newName: "FullName");

            migrationBuilder.AlterColumn<int>(
                name: "LanguageId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Languages_LanguageId",
                table: "AspNetUsers",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "LanguageId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
