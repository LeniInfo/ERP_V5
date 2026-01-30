using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SP_Load_Param : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        CREATE OR ALTER PROCEDURE [dbo].[SP_Load_Param]
        (
            @FRAN VARCHAR(10) = NULL,
            @PARAMTYPE VARCHAR(50) = NULL
        )
        AS
        BEGIN
            SET NOCOUNT ON;

            SELECT 
                PARAMVALUE,
                PARAMDESC
            FROM PARAMS
            WHERE PARAMTYPE = @PARAMTYPE
              AND FRAN = @FRAN
            ORDER BY PARAMVALUE;
        END
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DROP PROCEDURE IF EXISTS [dbo].[SP_Load_Param]
        ");
        }
    }
}
