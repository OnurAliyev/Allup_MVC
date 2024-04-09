using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_MiniProject_Allup.Migrations
{
    /// <inheritdoc />
    public partial class Propertyname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LogoUrl",
                table: "Brands",
                newName: "BrandLogoUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BrandLogoUrl",
                table: "Brands",
                newName: "LogoUrl");
        }
    }
}
