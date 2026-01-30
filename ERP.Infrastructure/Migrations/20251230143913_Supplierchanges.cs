using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Supplierchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POHDR_VENDOR_VENDOR",
                schema: "dbo",
                table: "POHDR");

            migrationBuilder.DropForeignKey(
                name: "FK_SINVHDR_VENDOR_VENDOR",
                schema: "dbo",
                table: "SINVHDR");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VENDOR",
                schema: "dbo",
                table: "VENDOR");

            migrationBuilder.RenameTable(
                name: "VENDOR",
                schema: "dbo",
                newName: "SUPPLIER",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "VENDOR",
                schema: "dbo",
                table: "SINVHDR",
                newName: "SUPPLIER");

            migrationBuilder.RenameIndex(
                name: "IX_SINVHDR_VENDOR",
                schema: "dbo",
                table: "SINVHDR",
                newName: "IX_SINVHDR_SUPPLIER");

            migrationBuilder.RenameColumn(
                name: "VENDORREFNO",
                schema: "dbo",
                table: "POHDR",
                newName: "SUPPLIERREFNO");

            migrationBuilder.RenameColumn(
                name: "VENDOR",
                schema: "dbo",
                table: "POHDR",
                newName: "SUPPLIER");

            migrationBuilder.RenameIndex(
                name: "IX_POHDR_VENDOR",
                schema: "dbo",
                table: "POHDR",
                newName: "IX_POHDR_SUPPLIER");

            migrationBuilder.RenameColumn(
                name: "VENDOR",
                schema: "dbo",
                table: "PODET",
                newName: "SUPPLIER");

            migrationBuilder.RenameColumn(
                name: "VENDOR",
                schema: "dbo",
                table: "SUPPLIER",
                newName: "SUPPLIERCODE");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SUPPLIER",
                schema: "dbo",
                table: "SUPPLIER",
                column: "SUPPLIERCODE");

            migrationBuilder.AddForeignKey(
                name: "FK_POHDR_SUPPLIER_SUPPLIER",
                schema: "dbo",
                table: "POHDR",
                column: "SUPPLIER",
                principalSchema: "dbo",
                principalTable: "SUPPLIER",
                principalColumn: "SUPPLIERCODE",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SINVHDR_SUPPLIER_SUPPLIER",
                schema: "dbo",
                table: "SINVHDR",
                column: "SUPPLIER",
                principalSchema: "dbo",
                principalTable: "SUPPLIER",
                principalColumn: "SUPPLIERCODE",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POHDR_SUPPLIER_SUPPLIER",
                schema: "dbo",
                table: "POHDR");

            migrationBuilder.DropForeignKey(
                name: "FK_SINVHDR_SUPPLIER_SUPPLIER",
                schema: "dbo",
                table: "SINVHDR");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SUPPLIER",
                schema: "dbo",
                table: "SUPPLIER");

            migrationBuilder.RenameTable(
                name: "SUPPLIER",
                schema: "dbo",
                newName: "VENDOR",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "SUPPLIER",
                schema: "dbo",
                table: "SINVHDR",
                newName: "VENDOR");

            migrationBuilder.RenameIndex(
                name: "IX_SINVHDR_SUPPLIER",
                schema: "dbo",
                table: "SINVHDR",
                newName: "IX_SINVHDR_VENDOR");

            migrationBuilder.RenameColumn(
                name: "SUPPLIERREFNO",
                schema: "dbo",
                table: "POHDR",
                newName: "VENDORREFNO");

            migrationBuilder.RenameColumn(
                name: "SUPPLIER",
                schema: "dbo",
                table: "POHDR",
                newName: "VENDOR");

            migrationBuilder.RenameIndex(
                name: "IX_POHDR_SUPPLIER",
                schema: "dbo",
                table: "POHDR",
                newName: "IX_POHDR_VENDOR");

            migrationBuilder.RenameColumn(
                name: "SUPPLIER",
                schema: "dbo",
                table: "PODET",
                newName: "VENDOR");

            migrationBuilder.RenameColumn(
                name: "SUPPLIERCODE",
                schema: "dbo",
                table: "VENDOR",
                newName: "VENDOR");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VENDOR",
                schema: "dbo",
                table: "VENDOR",
                column: "VENDOR");

            migrationBuilder.AddForeignKey(
                name: "FK_POHDR_VENDOR_VENDOR",
                schema: "dbo",
                table: "POHDR",
                column: "VENDOR",
                principalSchema: "dbo",
                principalTable: "VENDOR",
                principalColumn: "VENDOR",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SINVHDR_VENDOR_VENDOR",
                schema: "dbo",
                table: "SINVHDR",
                column: "VENDOR",
                principalSchema: "dbo",
                principalTable: "VENDOR",
                principalColumn: "VENDOR",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
