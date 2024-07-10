using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankAppWithAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserSurname",
                schema: "UserIdentity",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserFirstName",
                schema: "UserIdentity",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserLastName",
                schema: "UserIdentity",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserFirstName",
                schema: "UserIdentity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserLastName",
                schema: "UserIdentity",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserSurname",
                schema: "UserIdentity",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
