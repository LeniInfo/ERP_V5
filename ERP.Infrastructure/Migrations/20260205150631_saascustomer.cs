using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class saascustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UPDATETM",
                table: "SaasCustomer",
                type: "datetime",
                nullable: true,
                defaultValueSql: "'1900-01-01'",
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValueSql: "'1900-01-01'");

            migrationBuilder.AlterColumn<string>(
                name: "UPDATEMARKS",
                table: "SaasCustomer",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UPDATEDT",
                table: "SaasCustomer",
                type: "date",
                nullable: true,
                defaultValueSql: "'1900-01-01'",
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValueSql: "'1900-01-01'");

            migrationBuilder.AlterColumn<string>(
                name: "UPDATEBY",
                table: "SaasCustomer",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "SAASCUSTOMERNAME",
                table: "SaasCustomer",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "PHONE2",
                table: "SaasCustomer",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(22,0)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "PHONE1",
                table: "SaasCustomer",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(22,0)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "EMAIL",
                table: "SaasCustomer",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CREATETM",
                table: "SaasCustomer",
                type: "datetime",
                nullable: true,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "CREATEREMARKS",
                table: "SaasCustomer",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CREATEDT",
                table: "SaasCustomer",
                type: "date",
                nullable: true,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "CREATEBY",
                table: "SaasCustomer",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ADDRESS",
                table: "SaasCustomer",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldDefaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "UPDATETM",
                table: "SaasCustomer",
                type: "date",
                nullable: false,
                defaultValueSql: "'1900-01-01'",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValueSql: "'1900-01-01'");

            migrationBuilder.AlterColumn<string>(
                name: "UPDATEMARKS",
                table: "SaasCustomer",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "UPDATEDT",
                table: "SaasCustomer",
                type: "date",
                nullable: false,
                defaultValueSql: "'1900-01-01'",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldDefaultValueSql: "'1900-01-01'");

            migrationBuilder.AlterColumn<string>(
                name: "UPDATEBY",
                table: "SaasCustomer",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "SAASCUSTOMERNAME",
                table: "SaasCustomer",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<decimal>(
                name: "PHONE2",
                table: "SaasCustomer",
                type: "numeric(22,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PHONE1",
                table: "SaasCustomer",
                type: "numeric(22,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EMAIL",
                table: "SaasCustomer",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CREATETM",
                table: "SaasCustomer",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "CREATEREMARKS",
                table: "SaasCustomer",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CREATEDT",
                table: "SaasCustomer",
                type: "date",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "CREATEBY",
                table: "SaasCustomer",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "ADDRESS",
                table: "SaasCustomer",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);
        }
    }
}
