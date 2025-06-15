using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeHub.Migrations
{
    /// <inheritdoc />
    public partial class AdduserNameInUserAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "UserAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "UserAddress");
        }
    }
}
