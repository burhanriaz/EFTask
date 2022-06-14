using Microsoft.EntityFrameworkCore.Migrations;

namespace EFTask.Migrations
{
    public partial class two : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderedItems_Item_ItemId",
                table: "OrderedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedItems_Unit_UnitId",
                table: "OrderedItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderedItems_ItemId",
                table: "OrderedItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderedItems_UnitId",
                table: "OrderedItems");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "OrderedItems");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "OrderedItems");

            migrationBuilder.RenameColumn(
                name: "UnitID_Fk",
                table: "OrderedItems",
                newName: "UnitId_Fk");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedItems_ItemId_Fk",
                table: "OrderedItems",
                column: "ItemId_Fk");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedItems_UnitId_Fk",
                table: "OrderedItems",
                column: "UnitId_Fk");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedItems_Item_ItemId_Fk",
                table: "OrderedItems",
                column: "ItemId_Fk",
                principalTable: "Item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedItems_Unit_UnitId_Fk",
                table: "OrderedItems",
                column: "UnitId_Fk",
                principalTable: "Unit",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderedItems_Item_ItemId_Fk",
                table: "OrderedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedItems_Unit_UnitId_Fk",
                table: "OrderedItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderedItems_ItemId_Fk",
                table: "OrderedItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderedItems_UnitId_Fk",
                table: "OrderedItems");

            migrationBuilder.RenameColumn(
                name: "UnitId_Fk",
                table: "OrderedItems",
                newName: "UnitID_Fk");

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "OrderedItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "OrderedItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderedItems_ItemId",
                table: "OrderedItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedItems_UnitId",
                table: "OrderedItems",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedItems_Item_ItemId",
                table: "OrderedItems",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedItems_Unit_UnitId",
                table: "OrderedItems",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
