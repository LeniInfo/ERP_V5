using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GENERATE_DOCNO_PO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR ALTER FUNCTION [dbo].[GENERATE_DOCNO_PO]
(
    @psFran   VARCHAR(10),
    @psPrefix VARCHAR(10)
)
RETURNS VARCHAR(20)
AS
BEGIN
    DECLARE @DOCNo VARCHAR(20);
    DECLARE @SerialCount INT;
    DECLARE @SerialFormatted VARCHAR(6);

    -- Get the next serial number
    SELECT @SerialCount = ISNULL(MAX(CAST(RIGHT(PONO, 6) AS INT)), 0) + 1
    FROM POHDR
    WHERE FRAN = @psFran;

    -- Format as 6-digit number
    SET @SerialFormatted = RIGHT('000000' + CAST(@SerialCount AS VARCHAR(6)), 6);

    -- Combine with prefix
    SET @DOCNo = @psPrefix + @SerialFormatted;

    RETURN @DOCNo;
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DROP FUNCTION IF EXISTS [dbo].[GENERATE_DOCNO_PO];
");
        }
    }
}
