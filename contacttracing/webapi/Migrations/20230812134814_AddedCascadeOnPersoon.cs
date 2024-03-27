using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class AddedCascadeOnPersoon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aanwezigheden_AspNetUsers_IdPersoon",
                table: "Aanwezigheden");

            migrationBuilder.DropForeignKey(
                name: "FK_Aanwezigheden_Restaurants_IdRestaurant",
                table: "Aanwezigheden");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.AddForeignKey(
                name: "FK_Aanwezigheden_AspNetUsers_IdPersoon",
                table: "Aanwezigheden",
                column: "IdPersoon",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Aanwezigheden_Restaurants_IdRestaurant",
                table: "Aanwezigheden",
                column: "IdRestaurant",
                principalTable: "Restaurants",
                principalColumn: "IdRestaurant",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aanwezigheden_AspNetUsers_IdPersoon",
                table: "Aanwezigheden");

            migrationBuilder.DropForeignKey(
                name: "FK_Aanwezigheden_Restaurants_IdRestaurant",
                table: "Aanwezigheden");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.AddForeignKey(
                name: "FK_Aanwezigheden_AspNetUsers_IdPersoon",
                table: "Aanwezigheden",
                column: "IdPersoon",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Aanwezigheden_Restaurants_IdRestaurant",
                table: "Aanwezigheden",
                column: "IdRestaurant",
                principalTable: "Restaurants",
                principalColumn: "IdRestaurant",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
