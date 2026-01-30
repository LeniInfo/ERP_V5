using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SP_Save_PO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR ALTER PROCEDURE [dbo].[SP_Save_PO]
(
    @psFran        VARCHAR(10),
    @psDOCPrefix   VARCHAR(10),
    @Mode          NVARCHAR(50),
    @JSONData      NVARCHAR(MAX)
)
AS
BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @ldNextSEQNO NVARCHAR(20);
    DECLARE @NewDOCNo NVARCHAR(20);

    SET @ldNextSEQNO = (SELECT dbo.GENERATE_DOCNO_PO(@psFran, 'PO'));
    SET @NewDOCNo    = @ldNextSEQNO;

    -------------------------------------------------------
    -- HEADER
    -------------------------------------------------------
    DECLARE @Header TABLE
    (
        FRAN VARCHAR(10),
        BRCH VARCHAR(10),
        WHSE VARCHAR(10),
        POTYPE VARCHAR(10),
        VENDOR VARCHAR(20),
        CREATEBY VARCHAR(50),
        CREATEREMARKS VARCHAR(200)
    );

    INSERT INTO @Header
    SELECT
        JSON_VALUE(@JSONData, '$.header.FRAN'),
        JSON_VALUE(@JSONData, '$.header.BRCH'),
        JSON_VALUE(@JSONData, '$.header.WHSE'),
        JSON_VALUE(@JSONData, '$.header.POTYPE'),
        JSON_VALUE(@JSONData, '$.header.VENDOR'),
        JSON_VALUE(@JSONData, '$.header.CREATEBY'),
        JSON_VALUE(@JSONData, '$.header.CREATEREMARKS');

    -------------------------------------------------------
    -- DETAIL VARIABLES
    -------------------------------------------------------
    DECLARE 
        @Qty DECIMAL(18,2),
        @UnitPrice DECIMAL(18,2),
        @DiscountPct DECIMAL(5,2),
        @VatPct DECIMAL(5,2),
        @DiscountValue DECIMAL(18,2),
        @VatValue DECIMAL(18,2),
        @TotalValue DECIMAL(18,2);

    -------------------------------------------------------
    -- DETAILS TABLE
    -------------------------------------------------------
    DECLARE @Details TABLE
    (
        FRAN VARCHAR(10),
        BRCH VARCHAR(10),
        WHSE VARCHAR(10),
        PODT DATETIME,
        POTYPE VARCHAR(10),
        PONO NVARCHAR(20),
        POSRL NVARCHAR(20),
        MAKE NVARCHAR(50),
        PART NVARCHAR(50),
        QTY DECIMAL(18,2),
        UNITPRICE DECIMAL(18,2),
        DISCOUNT DECIMAL(5,2),
        VATPERCENTAGE DECIMAL(5,2),
        DISCOUNTVALUE DECIMAL(18,2),
        VATVALUE DECIMAL(18,2),
        TOTALVALUE DECIMAL(18,2),
        VENDOR NVARCHAR(20),
        CREATEDT DATETIME,
        CREATEBY NVARCHAR(50),
        CREATEREMARKS NVARCHAR(200),
        PLANTYPE NVARCHAR(50),
        PLANNO NVARCHAR(50),
        PLANSRL DECIMAL(18,2)
    );

    DECLARE @HeaderRow TABLE
    (
        FRAN VARCHAR(10),
        BRCH VARCHAR(10),
        WHSE VARCHAR(10),
        POTYPE VARCHAR(10),
        VENDOR VARCHAR(20),
        CREATEBY VARCHAR(50),
        CREATEREMARKS VARCHAR(200)
    );

    INSERT INTO @HeaderRow SELECT * FROM @Header;

    -------------------------------------------------------
    -- CURSOR
    -------------------------------------------------------
    DECLARE detail_cursor CURSOR FOR
        SELECT j.value
        FROM OPENJSON(@JSONData, '$.details') j;

    DECLARE @jsonRow NVARCHAR(MAX);

    OPEN detail_cursor;
    FETCH NEXT FROM detail_cursor INTO @jsonRow;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SELECT
            @Qty = TRY_CONVERT(DECIMAL(18,2), NULLIF(JSON_VALUE(@jsonRow, '$.QTY'), '')),
            @UnitPrice = TRY_CONVERT(DECIMAL(18,2), NULLIF(JSON_VALUE(@jsonRow, '$.UNITPRICE'), '')),
            @DiscountPct = TRY_CONVERT(DECIMAL(5,2), NULLIF(JSON_VALUE(@jsonRow, '$.DISCOUNT'), '')),
            @VatPct = TRY_CONVERT(DECIMAL(5,2), NULLIF(JSON_VALUE(@jsonRow, '$.VATPERCENTAGE'), ''));

        EXEC SP_CalculateTotalValue
            @Qty = @Qty,
            @UnitPrice = @UnitPrice,
            @DiscountPct = @DiscountPct,
            @VatPct = @VatPct,
            @DiscountValue = @DiscountValue OUTPUT,
            @VatValue = @VatValue OUTPUT,
            @TotalValue = @TotalValue OUTPUT;

        INSERT INTO @Details
        SELECT
            H.FRAN, H.BRCH, H.WHSE, GETDATE(), H.POTYPE, @NewDOCNo,
            JSON_VALUE(@jsonRow, '$.POSRL'),
            JSON_VALUE(@jsonRow, '$.MAKE'),
            JSON_VALUE(@jsonRow, '$.PART'),
            @Qty,
            @UnitPrice,
            @DiscountPct,
            @VatPct,
            @DiscountValue,
            @VatValue,
            @TotalValue,
            H.VENDOR,
            GETDATE(),
            H.CREATEBY,
            H.CREATEREMARKS,
            JSON_VALUE(@jsonRow, '$.PLANTYPE'),
            JSON_VALUE(@jsonRow, '$.PLANNO'),
            TRY_CONVERT(DECIMAL(18,2), NULLIF(JSON_VALUE(@jsonRow, '$.PLANSRL'), ''))
        FROM @HeaderRow H;

        FETCH NEXT FROM detail_cursor INTO @jsonRow;
    END

    CLOSE detail_cursor;
    DEALLOCATE detail_cursor;

    -------------------------------------------------------
    -- INSERT POHDR
    -------------------------------------------------------
    INSERT INTO POHDR
    (
        FRAN, BRCH, WHSE, POTYPE, PONO, PODT,
        SUPPLIER, SUPPLIERREFNO, CURRENCY, NOOFITEMS,
        DISCOUNT, TOTALVALUE,
        CREATEDT, CREATETM, CREATEBY, CREATEREMARKS,
        UPDATEDT, UPDATETM, UPDATEBY, UPDATEREMARKS
    )
    SELECT
        FRAN, BRCH, WHSE, POTYPE, @NewDOCNo, GETDATE(),
        VENDOR, 'REFERENCE', 'INR',
        COUNT(*),
        SUM(DISCOUNT),
        SUM(TOTALVALUE),
        GETDATE(), GETDATE(), CREATEBY, CREATEREMARKS,
        GETDATE(), GETDATE(), 'SYSTEM', ''
    FROM @Details
    GROUP BY FRAN, BRCH, WHSE, POTYPE, VENDOR, CREATEBY, CREATEREMARKS;

    -------------------------------------------------------
    -- INSERT PODET
    -------------------------------------------------------
    INSERT INTO PODET
    (
        FRAN, BRCH, WHSE, PODT, POTYPE, PONO, POSRL, MAKE, PART, QTY,
        UNITPRICE, DISCOUNT, VATVALUE, DISCOUNTVALUE, TOTALVALUE, SUPPLIER,
        CREATEDT, CREATETM, CREATEBY, CREATEREMARKS,
        PLANTYPE, PLANNO, PLANSRL, VATPERCENTAGE,
        UPDATETM, UPDATEDT, UPDATEBY, UPDATEREMARKS
    )
    SELECT
        FRAN, BRCH, WHSE, PODT, POTYPE, PONO, POSRL, MAKE, PART, QTY,
        UNITPRICE, DISCOUNT, VATVALUE, DISCOUNTVALUE, TOTALVALUE, VENDOR,
        CREATEDT, GETDATE(), CREATEBY, CREATEREMARKS,
        PLANTYPE, PLANNO, PLANSRL, VATPERCENTAGE,
        GETDATE(), GETDATE(), 'SYSTEM', ''
    FROM @Details;

    SELECT 1 AS keyvalue;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    DECLARE @Err NVARCHAR(MAX) = ERROR_MESSAGE();
    RAISERROR(@Err, 16, 1);
END CATCH
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DROP PROCEDURE IF EXISTS [dbo].[SP_Save_PO];
");
        }
    }
}
