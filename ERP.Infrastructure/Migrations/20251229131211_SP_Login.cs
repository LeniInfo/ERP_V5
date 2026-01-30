using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SP_Login : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
    CREATE OR ALTER PROCEDURE dbo.SP_Login
    (
        @UserId NVARCHAR(100),
        @Password NVARCHAR(100)
    )
    AS
    BEGIN
        SET NOCOUNT ON;

        DECLARE @SAASCUSTOMERID NVARCHAR(50);

        SELECT TOP 1 
            @SAASCUSTOMERID = SAASCUSTOMERID
        FROM USERS
        WHERE USERID = @UserId
          AND PASSWORD = @Password;
       IF(@SAASCUSTOMERID IS NOT NULL)
        BEGIN
        SELECT FRAN ,'1' AS FLAG
        FROM FRAN
        WHERE SAASCUSTOMERID = @SAASCUSTOMERID;
        END
      ELSE
       BEGIN
          SELECT '0' AS FLAG
       END
    END
");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP PROCEDURE IF EXISTS dbo.SP_Login
        ");
        }
    }
}
