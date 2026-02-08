using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ERPV5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "APPOINTMNET",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    APPOINTID = table.Column<decimal>(type: "numeric(22,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    APPOINTDT = table.Column<DateTime>(type: "date", nullable: false, defaultValue: new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    CUSTOMER = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    VEHICLEID = table.Column<decimal>(type: "numeric(22,0)", nullable: false, defaultValue: 0m),
                    ASSAIGNEDTO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    STATUS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    REMARKS = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false, defaultValue: ""),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValue: ""),
                    UPDATETM = table.Column<DateTime>(type: "datetime", nullable: false, defaultValue: new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    UPDATEMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APPOINTMNET", x => new { x.FRAN, x.APPOINTID });
                });

            migrationBuilder.CreateTable(
                name: "AUTHORITY",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    USERID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MENU = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SUBMENU = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AUMENUTEXT = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AUSUBMENUTEXT = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUTHORITY", x => new { x.FRAN, x.TYPE, x.USERID, x.MENU, x.SUBMENU });
                });

            migrationBuilder.CreateTable(
                name: "BRCH",
                schema: "dbo",
                columns: table => new
                {
                    BRCH = table.Column<string>(type: "nvarchar(450)", precision: 22, scale: 0, nullable: false),
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NAMEAR = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BRCH", x => x.BRCH);
                });

            migrationBuilder.CreateTable(
                name: "CHARTOFACCOUNTS",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ACCOUNTTYPE = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ACCOUNTCODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ACCOUNTNAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ACCOUNTBALANCE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    REMARKS = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHARTOFACCOUNTS", x => new { x.FRAN, x.ACCOUNTTYPE, x.ACCOUNTCODE });
                });

            migrationBuilder.CreateTable(
                name: "COMPETITOR",
                schema: "dbo",
                columns: table => new
                {
                    COMPETITOR = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ID = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NAMEAR = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PHONE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ADDRESS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VATNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMPETITOR", x => x.COMPETITOR);
                });

            migrationBuilder.CreateTable(
                name: "CORDHDR",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CORDTYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CORDNO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CORDDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CUSTOMER = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SEQNO = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    SEQPREFIX = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CURRENCY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NOOFITEMS = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    DISCOUNTVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    GROSSVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    NETVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    VATVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    TOTALVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORDHDR", x => new { x.FRAN, x.BRCH, x.WHSE, x.CORDTYPE, x.CORDNO });
                });

            migrationBuilder.CreateTable(
                name: "CRTN",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CRTNTYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CRTNCATG = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ID = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CRTNDESC = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LENGTH = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    WIDTH = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    HEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    VOLUME = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    MINWEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    MAXWEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRTN", x => new { x.FRAN, x.CRTNTYPE, x.CRTNCATG });
                });

            migrationBuilder.CreateTable(
                name: "CRTNDET",
                schema: "dbo",
                columns: table => new
                {
                    CDFRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CDBRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CDWHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CDCRTN = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CDCRTNTYPE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CDCRTNSRL = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    CDPART = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: false),
                    CDMAKE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CDQTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    CDCKDQTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    CDUNCKDQTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    CDPICKTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CDPICKNO = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    CDPICKSRL = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    CDREFTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CDREFNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CDREFSRL = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    CDLOTNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CDSTATUS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CDCUST = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CDPKGCODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CDCOO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CDHSCODE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CDNETWEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CDGROSSWEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CDSUGGCOO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CDSUGGHSCODE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CDUNITNETWEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CDUNITGROSSWEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CDGROSSWEIGHT_ADJUSTED = table.Column<decimal>(type: "decimal(22,13)", precision: 22, scale: 13, nullable: false),
                    CDREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CDCREATEDT = table.Column<DateTime>(type: "datetime", nullable: false),
                    CDCREATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    CDCREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CDCREATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CDUPDATEDT = table.Column<DateTime>(type: "datetime", nullable: false),
                    CDUPDATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    CDUPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CDUPDATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRTNDET", x => new { x.CDFRAN, x.CDBRCH, x.CDWHSE, x.CDCRTN, x.CDCRTNTYPE, x.CDCRTNSRL });
                });

            migrationBuilder.CreateTable(
                name: "CURRENCY",
                schema: "dbo",
                columns: table => new
                {
                    CURRENCY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BASECURRENCY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FACTOR1 = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    FACTOR2 = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    DECIMALPLACE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    ID = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CURRENCY", x => x.CURRENCY);
                });

            migrationBuilder.CreateTable(
                name: "CUSTOMER",
                schema: "dbo",
                columns: table => new
                {
                    CUSTOMER = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ID = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NAMEAR = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PHONE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ADDRESS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VATNO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUSTOMER", x => x.CUSTOMER);
                });

            migrationBuilder.CreateTable(
                name: "EMPLOYEEMASTER",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    EMPLOYEE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    NAMEAR = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    PHONE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    EMAIL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    ADDRESS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    NATIONALID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    HIREDATE = table.Column<DateTime>(type: "date", nullable: false, defaultValue: new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    ISACTIVE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false, defaultValue: ""),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValue: ""),
                    UPDATETM = table.Column<DateTime>(type: "datetime", nullable: false, defaultValue: new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    UPDATEMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEEMASTER", x => new { x.FRAN, x.EMPLOYEE });
                });

            migrationBuilder.CreateTable(
                name: "FINALPART",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "char(10)", nullable: false),
                    MAKE = table.Column<string>(type: "char(10)", nullable: false),
                    PART = table.Column<string>(type: "char(28)", nullable: false),
                    OHQTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    OOQTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    CMSALEQTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    LMSALEQTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    M3SALEQTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    M6SALEQTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    M12SALEQTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    M24SALEQTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINALPART", x => new { x.FRAN, x.MAKE, x.PART });
                });

            migrationBuilder.CreateTable(
                name: "FRAN",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NAMEAR = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SAASCUSTOMERID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    VATENABLED = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    NATUREOFBUSINESS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CUSTOMERCURRENCY = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRAN", x => x.FRAN);
                });

            migrationBuilder.CreateTable(
                name: "JOURNALENTRIES",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    JOURNELTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    JOURNELENTRYID = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    JOURNELENTRYDATE = table.Column<DateOnly>(type: "date", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    REFERENCE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ACCOUNTCODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    REFCUSTOMER = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    REFTYPE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    REFNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    REFDT = table.Column<DateTime>(type: "date", nullable: true),
                    AMOUNT = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    PAYMENTMETHOD = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CARDNUMBER = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CHEQUEDT = table.Column<DateTime>(type: "date", nullable: true),
                    REMARKS = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JOURNALENTRIES", x => new { x.FRAN, x.JOURNELTYPE, x.JOURNELENTRYID });
                    table.UniqueConstraint("AK_JOURNALENTRIES_FRAN_JOURNELENTRYID", x => new { x.FRAN, x.JOURNELENTRYID });
                });

            migrationBuilder.CreateTable(
                name: "LADET2",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    INVTYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    INVNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    INVSRL = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    INVDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CUSTOMER = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PART = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: false),
                    MAKE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    QTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    UNITRATE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    NETVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CURRENCYFACTOR = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    UNITGROSSWEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    UNITNETWEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    LATYPE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    LANO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LASRL = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    PACKNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PICKNO = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    CRTN = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    COO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HSCODE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LENGTH = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: true),
                    WIDTH = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    HEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    VOLUME = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    NETWEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    GROSSWEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    KEYINGROSSWEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CURRENCY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    STOREID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LADET2", x => new { x.FRAN, x.BRCH, x.WHSE, x.INVTYPE, x.INVNO, x.INVSRL });
                });

            migrationBuilder.CreateTable(
                name: "LAHDR",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LATYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LANO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LADT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    INVTYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    INVNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CUSTOMER = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SEQNO = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    VESSEL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PORTDEST = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ETD = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ETA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOADDT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NOOOFCRTN = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    REMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LAHDR", x => new { x.FRAN, x.BRCH, x.WHSE, x.LATYPE, x.LANO });
                });

            migrationBuilder.CreateTable(
                name: "MAKE",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MAKE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NAMEAR = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ID = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MAKE", x => new { x.FRAN, x.MAKE });
                });

            migrationBuilder.CreateTable(
                name: "OPLNDET",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PLANTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PLANNO = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    PLANSRL = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    PLANDT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VENDOR = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MAKE = table.Column<string>(type: "char(10)", nullable: false),
                    PART = table.Column<string>(type: "char(28)", nullable: false),
                    QTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    UNITPRICE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    NETVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    SUGGQTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    SUGGVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CURRENCY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OHQTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    OOQTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    POTYPE = table.Column<string>(type: "char(10)", nullable: false),
                    PONO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    POSRL = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    PMC = table.Column<string>(type: "char(10)", nullable: false),
                    FLAGPARENT = table.Column<string>(type: "char(10)", nullable: false),
                    SUBSPART = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: false),
                    FINALPART = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: false),
                    NOREORDERCODE = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    STOPSALECODE = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PLANSELECTION = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    REMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OPLNDET", x => new { x.FRAN, x.BRCH, x.WHSE, x.PLANTYPE, x.PLANNO, x.PLANSRL });
                });

            migrationBuilder.CreateTable(
                name: "OPLNHDR",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "char(10)", nullable: false),
                    BRCH = table.Column<string>(type: "char(10)", nullable: false),
                    WHSE = table.Column<string>(type: "char(10)", nullable: false),
                    PLANTYPE = table.Column<string>(type: "char(10)", nullable: false),
                    PLANNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PLANDT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TRANTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SEQNO = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    NOITEMS = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    NETVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    PLANSELECTION = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PLANCALCULATION = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OPLNHDR", x => new { x.FRAN, x.BRCH, x.WHSE, x.PLANTYPE, x.PLANNO });
                });

            migrationBuilder.CreateTable(
                name: "OPLNMST",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ID = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SELECTSQL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FILTERSQL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    GROUPBYSQL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ORDERBYSQL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OPLNMST", x => new { x.FRAN, x.TYPE, x.NAME });
                });

            migrationBuilder.CreateTable(
                name: "PACKHDR",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PACKTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PACKNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PACKDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CUSTOMER = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CURRENCY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CURRFACTOR = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    DESPFACTOR = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    NOOFCRTN = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    GROSSVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    NETVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    NETWEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    GROSSWEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    SEQPREFIX = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SEQNO = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    NOOFITEMS = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PACKHDR", x => new { x.FRAN, x.BRCH, x.WHSE, x.PACKTYPE, x.PACKNO });
                });

            migrationBuilder.CreateTable(
                name: "PARAMS",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PARAMTYPE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PARAMVALUE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ID = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PARAMVALUESTR1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PARAMVALUENUM1 = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    PARAMDESC = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PARAMCATEGORY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PARAMREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PARAMS", x => new { x.FRAN, x.PARAMTYPE, x.PARAMVALUE });
                });

            migrationBuilder.CreateTable(
                name: "PARTS",
                schema: "dbo",
                columns: table => new
                {
                    PART = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DESC = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MAKE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    STOCKKEY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BARCODE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SUBSPART = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: true),
                    FINALPART = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: true),
                    INVCLASS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CATEGORY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GROUP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    COO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FOB = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NETWEIGHT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    STOCK = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CMSALE = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LMSALE = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    M3SALE = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    M6SALE = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    M12SALE = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AVGM6 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ACTIVE = table.Column<bool>(type: "bit", nullable: false),
                    ID = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEDT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UPDATEDT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEMARKS = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PARTS", x => x.PART);
                });

            migrationBuilder.CreateTable(
                name: "PATCRTNHDR",
                schema: "dbo",
                columns: table => new
                {
                    CHFRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CHBRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CHWHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CHCRTNTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CHCRTN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CHLENGTH = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CHWIDTH = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CHHEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CHVOLUME = table.Column<decimal>(type: "decimal(22,9)", precision: 22, scale: 9, nullable: false),
                    CHNETWEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CHLOCNID = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    CHNOITEMS = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    CHTOTQTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    CHCUST = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CHPAKGRP = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CHPACKTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CHPACKNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CHCNTRID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CHSOURTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CHSOURNO = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CHGROSSWEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CHCASEMARK = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CHPACKINS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CHSTATUS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CHCRTNCATG = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CHCRTNPREFIX = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CHCRTNSEQNO = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    CHSHIPCASEMARK = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CHKEYINGROSSWEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CHLATYPE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CHLANO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CHLOTNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CHSINVNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CHSTOREID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CHINVTYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CHINVNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CHDESPSTATUS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CHCREATEDT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CHCREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CHCREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CHCREATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CHUPDATEDT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CHUPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CHUPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CHUPDATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PATCRTNHDR", x => new { x.CHFRAN, x.CHBRCH, x.CHWHSE, x.CHCRTNTYPE, x.CHCRTN });
                });

            migrationBuilder.CreateTable(
                name: "PGRPFACTOR",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TYPE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PRCGRP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VALUE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    FACTOR1 = table.Column<decimal>(type: "decimal(22,7)", precision: 22, scale: 7, nullable: false),
                    FACTOR2 = table.Column<decimal>(type: "decimal(22,7)", precision: 22, scale: 7, nullable: false),
                    FACTOR3 = table.Column<decimal>(type: "decimal(22,7)", precision: 22, scale: 7, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PGRPFACTOR", x => new { x.FRAN, x.TYPE, x.PRCGRP, x.NAME, x.VALUE });
                });

            migrationBuilder.CreateTable(
                name: "PGRPMST",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PRCTYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PRCGRP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FLAG1 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FLAG2 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FLAG3 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FACTOR1 = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    FACTOR2 = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    FACTOR3 = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PGRPMST", x => new { x.FRAN, x.PRCTYPE, x.PRCGRP });
                });

            migrationBuilder.CreateTable(
                name: "POALDT1",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ALOCSRL = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ALOCTYPE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ALOCDT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PART = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: false),
                    MAKE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ORDPART = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: false),
                    QTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    UNITPRICE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    NETVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    REFTYPE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    REFNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    REFSRL = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    REFSOURCE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    STOREID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POALDT1", x => new { x.FRAN, x.BRCH, x.WHSE, x.ALOCSRL });
                });

            migrationBuilder.CreateTable(
                name: "POB2B",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    B2BTYPE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    B2BNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    B2BSRL = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    B2BDT = table.Column<DateOnly>(type: "date", nullable: false),
                    MAKE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PART = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: false),
                    ORDPART = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: true),
                    QTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    ORDQTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    POQTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    UNITPRICE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    NETVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CURRENCY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CUSTOMER = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    REFTYPE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    REFNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    REFSRL = table.Column<decimal>(type: "decimal(6,0)", precision: 6, scale: 0, nullable: false),
                    UNITPACK = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    POTYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PONO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    POSRL = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    VENDOR = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    STOREID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POB2B", x => new { x.FRAN, x.BRCH, x.WHSE, x.B2BTYPE, x.B2BNO, x.B2BSRL });
                });

            migrationBuilder.CreateTable(
                name: "RECTHDR",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RECTTYPE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    RECTNO = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    RECTDT = table.Column<DateOnly>(type: "date", nullable: false),
                    NOOFITEMS = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    NETVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CURRENCY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    VENDOR = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SEQPREFIX = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SEQNO = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    REMARKS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECTHDR", x => new { x.FRAN, x.BRCH, x.WHSE, x.RECTTYPE, x.RECTNO });
                });

            migrationBuilder.CreateTable(
                name: "REPAIRHDR",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WORKSHOP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    REPAIRTYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    REPAIRNO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    REPAIRDT = table.Column<DateTime>(type: "date", nullable: false),
                    CUSTOMER = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    VEHICLEID = table.Column<decimal>(type: "numeric(10,0)", nullable: false),
                    CURRENCY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NOOFPARTS = table.Column<decimal>(type: "numeric(22,0)", nullable: false),
                    NOOFJOBS = table.Column<decimal>(type: "numeric(22,0)", nullable: false),
                    DISCOUNT = table.Column<decimal>(type: "numeric(22,3)", nullable: false),
                    TOTALVALUE = table.Column<decimal>(type: "numeric(22,3)", nullable: false),
                    SEQNO = table.Column<decimal>(type: "numeric(22,0)", nullable: false),
                    SEQPREFIX = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REPAIRHDR", x => new { x.FRAN, x.BRCH, x.WORKSHOP, x.REPAIRTYPE, x.REPAIRNO });
                });

            migrationBuilder.CreateTable(
                name: "REPAIRORDER",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    WORKSHOP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    REPAIRTYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    REPAIRNO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    REPAIRSRL = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    CUSTOMER = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    VEHICLEID = table.Column<decimal>(type: "numeric(22,0)", nullable: false, defaultValue: 0m),
                    WORKID = table.Column<decimal>(type: "numeric(10,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WORKTYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    WORKDT = table.Column<DateTime>(type: "date", nullable: false, defaultValue: new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    NOOFWORKS = table.Column<decimal>(type: "numeric(22,0)", nullable: false, defaultValue: 0m),
                    UNITPRICE = table.Column<decimal>(type: "numeric(22,3)", nullable: false, defaultValue: 0m),
                    DISCOUNT = table.Column<decimal>(type: "numeric(22,3)", nullable: false, defaultValue: 0m),
                    TOTALVALUE = table.Column<decimal>(type: "numeric(22,3)", nullable: false, defaultValue: 0m),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    UPDATETM = table.Column<DateTime>(type: "datetime", nullable: false, defaultValue: new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REPAIRORDER", x => new { x.FRAN, x.BRCH, x.WORKSHOP, x.REPAIRTYPE, x.REPAIRNO, x.REPAIRSRL });
                });

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

            migrationBuilder.CreateTable(
                name: "SALEHDR",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SALETYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SALENO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SALEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CUSTOMER = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CURRENCY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NOOFITEMS = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    DISCOUNT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    TOTALVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    SEQNO = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    SEQPREFIX = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SALESCHANNEL = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    INVOICENO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    INVOICEDATE = table.Column<DateOnly>(type: "date", nullable: false),
                    DUEDATE = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SALEHDR", x => new { x.FRAN, x.BRCH, x.WHSE, x.SALETYPE, x.SALENO });
                });

            migrationBuilder.CreateTable(
                name: "STORE",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BRCH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    STORE = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STORE", x => new { x.FRAN, x.BRCH, x.WHSE, x.STORE });
                });

            migrationBuilder.CreateTable(
                name: "SUBSPART",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MAKE = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: false),
                    PART = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: false),
                    FINLPART = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: false),
                    GRPNO = table.Column<decimal>(type: "decimal(4,0)", precision: 4, scale: 0, nullable: false),
                    PSSUBSEQ = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUBSPART", x => new { x.FRAN, x.MAKE, x.PART, x.FINLPART, x.GRPNO });
                });

            migrationBuilder.CreateTable(
                name: "SUPPLIER",
                schema: "dbo",
                columns: table => new
                {
                    SUPPLIERCODE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NAMEAR = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PHONE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EMAIL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ADDRESS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VATNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUPPLIER", x => x.SUPPLIERCODE);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    USERID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PASSWORD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SAASCUSTOMERID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    EMAILGROUP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TEAM = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => new { x.FRAN, x.USERID });
                });

            migrationBuilder.CreateTable(
                name: "VEHICLEMASTER",
                columns: table => new
                {
                    VECHILEID = table.Column<decimal>(type: "numeric(10,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    VIN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    CUSTOMER = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    MAKE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    MODEL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    MODELYEAR = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PLATENO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: ""),
                    MILEAGE = table.Column<decimal>(type: "numeric(22,0)", nullable: false, defaultValue: 0m),
                    CREATEDT = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "GETDATE()"),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValue: ""),
                    UPDATEDT = table.Column<DateTime>(type: "date", nullable: false, defaultValue: new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    UPDATETM = table.Column<DateTime>(type: "datetime", nullable: false, defaultValue: new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    UPDATEMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VEHICLEMASTER", x => x.VECHILEID);
                });

            migrationBuilder.CreateTable(
                name: "WHSE",
                schema: "dbo",
                columns: table => new
                {
                    WHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NAMEAR = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WHSE", x => x.WHSE);
                });

            migrationBuilder.CreateTable(
                name: "WORKINVDET",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    WORKSHOP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    WORKINVTYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    WORKINVNO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    WORKINVSRL = table.Column<decimal>(type: "numeric(22,0)", nullable: false, defaultValue: 0m),
                    WORKINVDT = table.Column<DateTime>(type: "date", nullable: false, defaultValue: new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    BILLTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    WORKTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    WORKID = table.Column<decimal>(type: "numeric(10,0)", nullable: false, defaultValue: 0m),
                    MAKE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    PART = table.Column<decimal>(type: "numeric(22,0)", nullable: false, defaultValue: 0m),
                    QTY = table.Column<decimal>(type: "numeric(22,0)", nullable: false, defaultValue: 0m),
                    UNITPRICE = table.Column<decimal>(type: "numeric(22,3)", nullable: false, defaultValue: 0m),
                    DISCOUNT = table.Column<decimal>(type: "numeric(22,3)", nullable: false, defaultValue: 0m),
                    VATPERCENTAGE = table.Column<decimal>(type: "numeric(22,3)", nullable: false, defaultValue: 0m),
                    VATVALUE = table.Column<decimal>(type: "numeric(22,3)", nullable: false, defaultValue: 0m),
                    TOTALVALUE = table.Column<decimal>(type: "numeric(22,3)", nullable: false, defaultValue: 0m),
                    REAPAIRTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    REAPAIRNO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    REPAIRSRL = table.Column<decimal>(type: "numeric(22,0)", nullable: false, defaultValue: 0m),
                    SALETYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    SALENO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    UPDATETM = table.Column<DateTime>(type: "datetime", nullable: false, defaultValue: new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WORKINVDET", x => new { x.FRAN, x.BRCH, x.WORKSHOP, x.WORKINVTYPE, x.WORKINVNO, x.WORKINVSRL });
                });

            migrationBuilder.CreateTable(
                name: "WORKINVHDR",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    WORKSHOP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    WORKINVTYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    WORKINVNO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    WORKINVDT = table.Column<DateTime>(type: "date", nullable: false, defaultValue: new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    CUSTOMER = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    VEHICLEID = table.Column<decimal>(type: "numeric(10,0)", nullable: false, defaultValue: 0m),
                    CURRENCY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    NOOFPARTS = table.Column<decimal>(type: "numeric(22,0)", nullable: false, defaultValue: 0m),
                    NOOFJOBS = table.Column<decimal>(type: "numeric(22,0)", nullable: false, defaultValue: 0m),
                    DISCOUNT = table.Column<decimal>(type: "numeric(22,3)", nullable: false, defaultValue: 0m),
                    VATVALUE = table.Column<decimal>(type: "numeric(22,3)", nullable: false, defaultValue: 0m),
                    TOTALVALUE = table.Column<decimal>(type: "numeric(22,3)", nullable: false, defaultValue: 0m),
                    SEQNO = table.Column<decimal>(type: "numeric(22,0)", nullable: false, defaultValue: 0m),
                    SEQPREFIX = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: ""),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    UPDATETM = table.Column<DateTime>(type: "datetime", nullable: false, defaultValue: new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WORKINVHDR", x => new { x.FRAN, x.BRCH, x.WORKSHOP, x.WORKINVTYPE, x.WORKINVNO });
                });

            migrationBuilder.CreateTable(
                name: "WORKMASTER",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    WORKID = table.Column<decimal>(type: "numeric(10,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WORKTYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    REMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    UNITPRICE = table.Column<decimal>(type: "numeric(22,3)", nullable: false, defaultValue: 0m),
                    ESTIMATED = table.Column<decimal>(type: "numeric(22,0)", nullable: false, defaultValue: 0m),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValue: ""),
                    UPDATETM = table.Column<DateTime>(type: "datetime", nullable: false, defaultValue: new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    UPDATEMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WORKMASTER", x => new { x.FRAN, x.WORKTYPE, x.WORKID });
                });

            migrationBuilder.CreateTable(
                name: "WORKSHOP",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WORKSHOP = table.Column<decimal>(type: "decimal(10,0)", precision: 10, scale: 0, nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WORKSHOP", x => new { x.FRAN, x.WORKSHOP });
                });

            migrationBuilder.CreateTable(
                name: "WORKSHOPMASTER",
                columns: table => new
                {
                    Fran = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    Workshop = table.Column<decimal>(type: "numeric(10,0)", nullable: false, defaultValue: 0m),
                    Brch = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, defaultValue: ""),
                    CreateTm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreateBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: ""),
                    CreateRemarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    UpdateTm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    UpdateBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "1900-01-01"),
                    UpdateRemarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WORKSHOPMASTER", x => new { x.Fran, x.Workshop });
                });

            migrationBuilder.CreateTable(
                name: "CORDDET",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CORDTYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CORDNO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CORDSRL = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CORDDT = table.Column<DateOnly>(type: "date", nullable: false),
                    MAKE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PART = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    QTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    ACCPQTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    NOTAVLQTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    PRICE = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    DISCOUNT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    VATPERCENTAGE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    VATVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    DISCOUNTVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    TOTALVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORDDET", x => new { x.FRAN, x.BRCH, x.WHSE, x.CORDTYPE, x.CORDNO, x.CORDSRL });
                    table.ForeignKey(
                        name: "FK_CORDDET_CORDHDR_FRAN_BRCH_WHSE_CORDTYPE_CORDNO",
                        columns: x => new { x.FRAN, x.BRCH, x.WHSE, x.CORDTYPE, x.CORDNO },
                        principalSchema: "dbo",
                        principalTable: "CORDHDR",
                        principalColumns: new[] { "FRAN", "BRCH", "WHSE", "CORDTYPE", "CORDNO" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JOURNALENTRYLINE",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    JOURNELENTRYID = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    JOURNELENTRYLINEID = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    ACCOUNTCODE = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    DEBIT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CREDIT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    REMARKS = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "date", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JOURNALENTRYLINE", x => new { x.FRAN, x.JOURNELENTRYID, x.JOURNELENTRYLINEID });
                    table.ForeignKey(
                        name: "FK_JOURNALENTRYLINE_JOURNALENTRIES_FRAN_JOURNELENTRYID",
                        columns: x => new { x.FRAN, x.JOURNELENTRYID },
                        principalSchema: "dbo",
                        principalTable: "JOURNALENTRIES",
                        principalColumns: new[] { "FRAN", "JOURNELENTRYID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LADET",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LATYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LANO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CRTNTYPE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CRTN = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DOCDT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CNTRNO = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CNTRDT = table.Column<DateOnly>(type: "date", nullable: false),
                    MSCRTN = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    PACKTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PACKNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CUSTOMER = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    INVTYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    INVNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SUBINVNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LADET", x => new { x.FRAN, x.BRCH, x.WHSE, x.LATYPE, x.LANO, x.CRTNTYPE, x.CRTN });
                    table.ForeignKey(
                        name: "FK_LADET_LAHDR_FRAN_BRCH_WHSE_LATYPE_LANO",
                        columns: x => new { x.FRAN, x.BRCH, x.WHSE, x.LATYPE, x.LANO },
                        principalSchema: "dbo",
                        principalTable: "LAHDR",
                        principalColumns: new[] { "FRAN", "BRCH", "WHSE", "LATYPE", "LANO" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PACKDET",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PACKTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PACKNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PACKSRL = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    CUSTOMER = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CRTNTYPE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CRTN = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    MSCRTN = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CRTNSRL = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    MAKE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PART = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: false),
                    QTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    UNITRATE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    NETVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    PICKTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PICKNO = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    PICKSRL = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    CORDTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CORDNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CORDSRL = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    LOTNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PDCOO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PDHSCODE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NETWEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    GROSSWEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    UNITNETWEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    UNITGROSSWEIGHT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    GROSSVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    PDSTOREID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PACKDET", x => new { x.FRAN, x.BRCH, x.WHSE, x.PACKTYPE, x.PACKNO, x.PACKSRL });
                    table.ForeignKey(
                        name: "FK_PACKDET_PACKHDR_FRAN_BRCH_WHSE_PACKTYPE_PACKNO",
                        columns: x => new { x.FRAN, x.BRCH, x.WHSE, x.PACKTYPE, x.PACKNO },
                        principalSchema: "dbo",
                        principalTable: "PACKHDR",
                        principalColumns: new[] { "FRAN", "BRCH", "WHSE", "PACKTYPE", "PACKNO" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RECTDET",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RECTTYPE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    RECTNO = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    RECTSRL = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    MAKE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PART = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: false),
                    QTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    UNITPRICE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    NETVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CURRENCY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    STOREID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    VENDOR = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    POTYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PONO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    POSRL = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    REMARKS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECTDET", x => new { x.FRAN, x.BRCH, x.WHSE, x.RECTTYPE, x.RECTNO, x.RECTSRL });
                    table.ForeignKey(
                        name: "FK_RECTDET_RECTHDR_FRAN_BRCH_WHSE_RECTTYPE_RECTNO",
                        columns: x => new { x.FRAN, x.BRCH, x.WHSE, x.RECTTYPE, x.RECTNO },
                        principalSchema: "dbo",
                        principalTable: "RECTHDR",
                        principalColumns: new[] { "FRAN", "BRCH", "WHSE", "RECTTYPE", "RECTNO" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SALEDET",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SALETYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SALENO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SALESRL = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SALEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    MAKE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PART = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    QTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    UNITPRICE = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    DISCOUNT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    VATPERCENTAGE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    VATVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    DISCOUNTVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    TOTALVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SALEDET", x => new { x.FRAN, x.BRCH, x.WHSE, x.SALETYPE, x.SALENO, x.SALESRL });
                    table.ForeignKey(
                        name: "FK_SALEDET_SALEHDR_FRAN_BRCH_WHSE_SALETYPE_SALENO",
                        columns: x => new { x.FRAN, x.BRCH, x.WHSE, x.SALETYPE, x.SALENO },
                        principalSchema: "dbo",
                        principalTable: "SALEHDR",
                        principalColumns: new[] { "FRAN", "BRCH", "WHSE", "SALETYPE", "SALENO" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "POHDR",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    POTYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PONO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PODT = table.Column<DateOnly>(type: "date", nullable: false),
                    SUPPLIER = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SUPPLIERREFNO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CURRENCY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NOOFITEMS = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DISCOUNT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    TOTALVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POHDR", x => new { x.FRAN, x.BRCH, x.WHSE, x.POTYPE, x.PONO });
                    table.ForeignKey(
                        name: "FK_POHDR_SUPPLIER_SUPPLIER",
                        column: x => x.SUPPLIER,
                        principalSchema: "dbo",
                        principalTable: "SUPPLIER",
                        principalColumn: "SUPPLIERCODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SINVHDR",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SINVTYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SINVNO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SINVDT = table.Column<DateOnly>(type: "date", nullable: false),
                    SUPPLIER = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CURRENCY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BLNO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BLDT = table.Column<DateOnly>(type: "date", nullable: true),
                    BUYERCODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SHIPPINGSTATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SHIPCOMPANYCODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ETA = table.Column<DateOnly>(type: "date", nullable: false),
                    PRODCOUNTRYCODE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    VESSELNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VESSELNAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SENDER = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PORTARRIVALDT = table.Column<DateOnly>(type: "date", nullable: false),
                    BONDEDARRVALDT = table.Column<DateOnly>(type: "date", nullable: false),
                    NOOFITEMS = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SEAFREIGHTCHARGES = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    INSURANCECHARGES = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    ODSCHARGES = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    ADDLCHARGES = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    INSPECTIONDOCNO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LETTEROFCREDITNO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DISCOUNTVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    GROSSVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    NETVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    VATVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    TOTALVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SINVHDR", x => new { x.FRAN, x.BRCH, x.WHSE, x.SINVTYPE, x.SINVNO });
                    table.ForeignKey(
                        name: "FK_SINVHDR_SUPPLIER_SUPPLIER",
                        column: x => x.SUPPLIER,
                        principalSchema: "dbo",
                        principalTable: "SUPPLIER",
                        principalColumn: "SUPPLIERCODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PODET",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    POTYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PONO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    POSRL = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PODT = table.Column<DateOnly>(type: "date", nullable: false),
                    SUPPLIER = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PLANTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PLANNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PLANSRL = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MAKE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PART = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    QTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    UNITPRICE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    DISCOUNT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    VATPERCENTAGE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    VATVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    DISCOUNTVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    TOTALVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PODET", x => new { x.FRAN, x.BRCH, x.WHSE, x.POTYPE, x.PONO, x.POSRL });
                    table.ForeignKey(
                        name: "FK_PODET_PARTS_PART",
                        column: x => x.PART,
                        principalSchema: "dbo",
                        principalTable: "PARTS",
                        principalColumn: "PART",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PODET_POHDR_FRAN_BRCH_WHSE_POTYPE_PONO",
                        columns: x => new { x.FRAN, x.BRCH, x.WHSE, x.POTYPE, x.PONO },
                        principalSchema: "dbo",
                        principalTable: "POHDR",
                        principalColumns: new[] { "FRAN", "BRCH", "WHSE", "POTYPE", "PONO" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SINVDET",
                schema: "dbo",
                columns: table => new
                {
                    FRAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BRCH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    WHSE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SINVTYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SINVNO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SINVSRL = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    SINVDT = table.Column<DateOnly>(type: "date", nullable: false),
                    VENDOR = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MAKE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PART = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: false),
                    ORDPART = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: false),
                    QTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    ORDQTY = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    UNITPRICE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    DISCOUNT = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    VATPERCENTAGE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    VATVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    DISCOUNTVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    TOTALVALUE = table.Column<decimal>(type: "decimal(22,3)", precision: 22, scale: 3, nullable: false),
                    CASENO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CONTAINERNO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    POTYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PONO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    POSRL = table.Column<decimal>(type: "decimal(22,0)", precision: 22, scale: 0, nullable: false),
                    CREATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    CREATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CREATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UPDATEDT = table.Column<DateOnly>(type: "date", nullable: false),
                    UPDATETM = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATEBY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UPDATEREMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SINVDET", x => new { x.FRAN, x.BRCH, x.WHSE, x.SINVTYPE, x.SINVNO, x.SINVSRL });
                    table.ForeignKey(
                        name: "FK_SINVDET_SINVHDR_FRAN_BRCH_WHSE_SINVTYPE_SINVNO",
                        columns: x => new { x.FRAN, x.BRCH, x.WHSE, x.SINVTYPE, x.SINVNO },
                        principalSchema: "dbo",
                        principalTable: "SINVHDR",
                        principalColumns: new[] { "FRAN", "BRCH", "WHSE", "SINVTYPE", "SINVNO" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PODET_PART",
                schema: "dbo",
                table: "PODET",
                column: "PART");

            migrationBuilder.CreateIndex(
                name: "IX_POHDR_SUPPLIER",
                schema: "dbo",
                table: "POHDR",
                column: "SUPPLIER");

            migrationBuilder.CreateIndex(
                name: "IX_SINVHDR_SUPPLIER",
                schema: "dbo",
                table: "SINVHDR",
                column: "SUPPLIER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "APPOINTMNET");

            migrationBuilder.DropTable(
                name: "AUTHORITY",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BRCH",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CHARTOFACCOUNTS",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "COMPETITOR",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CORDDET",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CRTN",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CRTNDET",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CURRENCY",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CUSTOMER",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EMPLOYEEMASTER");

            migrationBuilder.DropTable(
                name: "FINALPART",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FRAN",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "JOURNALENTRYLINE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "LADET",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "LADET2",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MAKE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "OPLNDET",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "OPLNHDR",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "OPLNMST",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PACKDET",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PARAMS",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PATCRTNHDR",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PGRPFACTOR",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PGRPMST",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "POALDT1",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "POB2B",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PODET",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RECTDET",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "REPAIRHDR");

            migrationBuilder.DropTable(
                name: "REPAIRORDER");

            migrationBuilder.DropTable(
                name: "SAASCUSTOMER");

            migrationBuilder.DropTable(
                name: "SALEDET",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SINVDET",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "STORE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SUBSPART",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "USERS",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VEHICLEMASTER");

            migrationBuilder.DropTable(
                name: "WHSE",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "WORKINVDET");

            migrationBuilder.DropTable(
                name: "WORKINVHDR");

            migrationBuilder.DropTable(
                name: "WORKMASTER");

            migrationBuilder.DropTable(
                name: "WORKSHOP",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "WORKSHOPMASTER");

            migrationBuilder.DropTable(
                name: "CORDHDR",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "JOURNALENTRIES",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "LAHDR",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PACKHDR",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PARTS",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "POHDR",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RECTHDR",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SALEHDR",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SINVHDR",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SUPPLIER",
                schema: "dbo");
        }
    }
}
