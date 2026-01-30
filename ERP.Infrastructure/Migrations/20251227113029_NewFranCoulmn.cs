using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewFranCoulmn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SAASCUSTOMERID",
                schema: "dbo",
                table: "FRAN",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SAASCUSTOMER",
                columns: table => new
                {
                    SAASCUSTOMERID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SAASCUSTOMERNAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    PHONE1 = table.Column<decimal>(type: "numeric(22,0)", nullable: false, defaultValue: 0m),
                    PHONE2 = table.Column<decimal>(type: "numeric(22,0)", nullable: false, defaultValue: 0m),
                    EMAIL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    ADDRESS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValue: ""),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "GETDATE()"),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValue: ""),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "'1900-01-01'"),
                    UPDATETM = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "'1900-01-01'"),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    UPDATEMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAASCUSTOMER", x => x.SAASCUSTOMERID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SAASCUSTOMER");

            migrationBuilder.DropColumn(
                name: "SAASCUSTOMERID",
                schema: "dbo",
                table: "FRAN");
        }
    }
}
