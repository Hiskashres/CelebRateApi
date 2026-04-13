using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CelebRateApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedAllTranslateionEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rates_AspNetUsers_UserId",
                table: "Rates");

            migrationBuilder.DropForeignKey(
                name: "FK_Rates_CelebCategories_CelebId_CategoryId",
                table: "Rates");

            migrationBuilder.DropTable(
                name: "CelebLanguages");

            migrationBuilder.DropTable(
                name: "CelebsTags");

            migrationBuilder.DropColumn(
                name: "TagName",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Celebs");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Celebs");

            migrationBuilder.DropColumn(
                name: "Specialty1",
                table: "CelebCategories");

            migrationBuilder.DropColumn(
                name: "Specialty2",
                table: "CelebCategories");

            migrationBuilder.DropColumn(
                name: "Specialty3",
                table: "CelebCategories");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Rates",
                newName: "ApplicationUserId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Languages",
                newName: "ShortName");

            migrationBuilder.AddColumn<int>(
                name: "CelebCategoryCategoryId",
                table: "Rates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CelebCategoryCelebId",
                table: "Rates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Languages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CategoryTranslations",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTranslations", x => new { x.CategoryId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_CategoryTranslations_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "LanguageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CelebCategoryTranslations",
                columns: table => new
                {
                    CelebId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Specialty1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialty2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialty3 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CelebCategoryTranslations", x => new { x.LanguageId, x.CelebId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_CelebCategoryTranslations_CelebCategories_CelebId_CategoryId",
                        columns: x => new { x.CelebId, x.CategoryId },
                        principalTable: "CelebCategories",
                        principalColumns: new[] { "CelebId", "CategoryId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CelebCategoryTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "LanguageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CelebTranslations",
                columns: table => new
                {
                    CelebId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CelebTranslations", x => new { x.CelebId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_CelebTranslations_Celebs_CelebId",
                        column: x => x.CelebId,
                        principalTable: "Celebs",
                        principalColumn: "CelebId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CelebTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "LanguageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagTranslations",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    TagName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagTranslations", x => new { x.TagId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_TagTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "LanguageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagTranslations_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rates_CelebCategoryCelebId_CelebCategoryCategoryId",
                table: "Rates",
                columns: new[] { "CelebCategoryCelebId", "CelebCategoryCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTranslations_LanguageId",
                table: "CategoryTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_CelebCategoryTranslations_CelebId_CategoryId",
                table: "CelebCategoryTranslations",
                columns: new[] { "CelebId", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_CelebTranslations_LanguageId",
                table: "CelebTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_TagTranslations_LanguageId",
                table: "TagTranslations",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_AspNetUsers_ApplicationUserId",
                table: "Rates",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_CelebCategories_CelebCategoryCelebId_CelebCategoryCategoryId",
                table: "Rates",
                columns: new[] { "CelebCategoryCelebId", "CelebCategoryCategoryId" },
                principalTable: "CelebCategories",
                principalColumns: new[] { "CelebId", "CategoryId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_CelebCategories_CelebId_CategoryId",
                table: "Rates",
                columns: new[] { "CelebId", "CategoryId" },
                principalTable: "CelebCategories",
                principalColumns: new[] { "CelebId", "CategoryId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rates_AspNetUsers_ApplicationUserId",
                table: "Rates");

            migrationBuilder.DropForeignKey(
                name: "FK_Rates_CelebCategories_CelebCategoryCelebId_CelebCategoryCategoryId",
                table: "Rates");

            migrationBuilder.DropForeignKey(
                name: "FK_Rates_CelebCategories_CelebId_CategoryId",
                table: "Rates");

            migrationBuilder.DropTable(
                name: "CategoryTranslations");

            migrationBuilder.DropTable(
                name: "CelebCategoryTranslations");

            migrationBuilder.DropTable(
                name: "CelebTranslations");

            migrationBuilder.DropTable(
                name: "TagTranslations");

            migrationBuilder.DropIndex(
                name: "IX_Rates_CelebCategoryCelebId_CelebCategoryCategoryId",
                table: "Rates");

            migrationBuilder.DropColumn(
                name: "CelebCategoryCategoryId",
                table: "Rates");

            migrationBuilder.DropColumn(
                name: "CelebCategoryCelebId",
                table: "Rates");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Languages");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Rates",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ShortName",
                table: "Languages",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "TagName",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Celebs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Celebs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Specialty1",
                table: "CelebCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Specialty2",
                table: "CelebCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Specialty3",
                table: "CelebCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CelebLanguages",
                columns: table => new
                {
                    CelebId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CelebLanguages", x => new { x.CelebId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_CelebLanguages_Celebs_CelebId",
                        column: x => x.CelebId,
                        principalTable: "Celebs",
                        principalColumn: "CelebId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CelebLanguages_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "LanguageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CelebsTags",
                columns: table => new
                {
                    CelebId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false),
                    CelebCategoryCategoryId = table.Column<int>(type: "int", nullable: true),
                    CelebCategoryCelebId = table.Column<int>(type: "int", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CelebsTags", x => new { x.CelebId, x.TagId });
                    table.ForeignKey(
                        name: "FK_CelebsTags_CelebCategories_CelebCategoryCelebId_CelebCategoryCategoryId",
                        columns: x => new { x.CelebCategoryCelebId, x.CelebCategoryCategoryId },
                        principalTable: "CelebCategories",
                        principalColumns: new[] { "CelebId", "CategoryId" });
                    table.ForeignKey(
                        name: "FK_CelebsTags_Celebs_CelebId",
                        column: x => x.CelebId,
                        principalTable: "Celebs",
                        principalColumn: "CelebId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CelebsTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CelebLanguages_LanguageId",
                table: "CelebLanguages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_CelebsTags_CelebCategoryCelebId_CelebCategoryCategoryId",
                table: "CelebsTags",
                columns: new[] { "CelebCategoryCelebId", "CelebCategoryCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_CelebsTags_TagId",
                table: "CelebsTags",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_AspNetUsers_UserId",
                table: "Rates",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_CelebCategories_CelebId_CategoryId",
                table: "Rates",
                columns: new[] { "CelebId", "CategoryId" },
                principalTable: "CelebCategories",
                principalColumns: new[] { "CelebId", "CategoryId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
