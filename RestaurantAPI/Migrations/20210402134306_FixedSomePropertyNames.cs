using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantAPI.Migrations
{
    public partial class FixedSomePropertyNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Adresses_AdressId",
                table: "Restaurants");

            migrationBuilder.DropTable(
                name: "Adresses");

            migrationBuilder.RenameColumn(
                name: "Desctiption",
                table: "Restaurants",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Categoty",
                table: "Restaurants",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "AdressId",
                table: "Restaurants",
                newName: "AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurants_AdressId",
                table: "Restaurants",
                newName: "IX_Restaurants_AddressId");

            migrationBuilder.RenameColumn(
                name: "Prize",
                table: "Dishes",
                newName: "Price");

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Addresses_AddressId",
                table: "Restaurants",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Addresses_AddressId",
                table: "Restaurants");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Restaurants",
                newName: "Desctiption");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Restaurants",
                newName: "Categoty");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "Restaurants",
                newName: "AdressId");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurants_AddressId",
                table: "Restaurants",
                newName: "IX_Restaurants_AdressId");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Dishes",
                newName: "Prize");

            migrationBuilder.CreateTable(
                name: "Adresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<int>(type: "int", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adresses", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Adresses_AdressId",
                table: "Restaurants",
                column: "AdressId",
                principalTable: "Adresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
