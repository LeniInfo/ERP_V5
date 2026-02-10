using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mastertype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MASTERTYPE",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MASTERTYPE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    MASTERCODE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NAMEAR = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PHONE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ADDRESS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VATNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SEQNO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SEQPREFIX = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CREATEDT = table.Column<DateTime>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UPDATEDT = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MASTERTYPE", x => new { x.FRAN, x.MASTERTYPE, x.MASTERCODE });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MASTERTYPE",
                schema: "dbo");
        }
    }
}
