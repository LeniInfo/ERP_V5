using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SP_CalculateTotalValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR ALTER PROCEDURE [dbo].[SP_CalculateTotalValue]
    @Qty DECIMAL(18,2),
    @UnitPrice DECIMAL(18,2),
    @DiscountPct DECIMAL(5,2),
    @VatPct DECIMAL(5,2),
    @DiscountValue DECIMAL(18,2) OUTPUT,
    @VatValue DECIMAL(18,2) OUTPUT,
    @TotalValue DECIMAL(18,2) OUTPUT
AS
BEGIN
    BEGIN TRY
        SET NOCOUNT ON;

        -- NULL SAFETY
        SET @Qty = ISNULL(@Qty, 0);
        SET @UnitPrice = ISNULL(@UnitPrice, 0);
        SET @DiscountPct = ISNULL(@DiscountPct, 0);
        SET @VatPct = ISNULL(@VatPct, 0);

        -- CALCULATIONS
        SET @DiscountValue = (@Qty * @UnitPrice) * @DiscountPct / 100.0;

        SET @VatValue = ((@Qty * @UnitPrice) - @DiscountValue) * @VatPct / 100.0;

        SET @TotalValue = (@Qty * @UnitPrice) - @DiscountValue + @VatValue;
    END TRY
    BEGIN CATCH
        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrState INT = ERROR_STATE();

        RAISERROR('Error in SP_CalculateTotalValue: %s', @ErrSeverity, @ErrState, @ErrMsg);

        -- Safety defaults
        SET @DiscountValue = 0;
        SET @VatValue = 0;
        SET @TotalValue = 0;
    END CATCH
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DROP PROCEDURE IF EXISTS [dbo].[SP_CalculateTotalValue];
");
        }
    }
}
