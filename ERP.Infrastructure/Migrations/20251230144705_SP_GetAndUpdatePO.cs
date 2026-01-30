using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SP_GetAndUpdatePO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR ALTER PROCEDURE [dbo].[SP_GetAndUpdatePO]
    @Action NVARCHAR(20),           -- GETALL / GETHDRDET / DELETE / UPDATE
    @PONO NVARCHAR(20) = NULL,
    @FRAN VARCHAR(10) = NULL,
    @SUPPLIER NVARCHAR(20) = NULL,
    @JSONData NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        ------------------------------------------------------------
        -- 1️⃣ GET ALL HEADER ROWS (PO Inquiry)
        ------------------------------------------------------------
        IF @Action = 'GETALL'
        BEGIN
            SELECT *
            FROM POHDR
            WHERE FRAN IS NOT NULL 
              AND SUPPLIER IS NOT NULL
            ORDER BY PODT DESC, PONO DESC;
            RETURN;
        END

        ------------------------------------------------------------
        -- 2️⃣ GET HEADER + DETAILS
        ------------------------------------------------------------
        IF @Action = 'GETHDRDET'
        BEGIN
            SELECT *
            FROM POHDR
            WHERE PONO = @PONO AND FRAN = @FRAN AND SUPPLIER = @SUPPLIER;

            SELECT *
            FROM PODET
            WHERE PONO = @PONO AND FRAN = @FRAN AND SUPPLIER = @SUPPLIER
            ORDER BY POSRL;

            RETURN;
        END

        ------------------------------------------------------------
        -- 3️⃣ DELETE PO
        ------------------------------------------------------------
        IF @Action = 'DELETE'
        BEGIN
            DELETE FROM PODET
            WHERE PONO = @PONO AND FRAN = @FRAN;

            DELETE FROM POHDR
            WHERE PONO = @PONO AND FRAN = @FRAN;

            SELECT 'Deleted Successfully' AS Result;
            RETURN;
        END

        ------------------------------------------------------------
        -- 4️⃣ UPDATE PO (Header + Detail)
        ------------------------------------------------------------
        IF @Action = 'UPDATE'
        BEGIN
            BEGIN TRANSACTION;

            DECLARE 
                @CURRENCY NVARCHAR(20),
                @NOOFITEMS INT,
                @DISCOUNT DECIMAL(18,2),
                @TOTALVALUE DECIMAL(18,2);

            SET @CURRENCY   = JSON_VALUE(@JSONData, '$.header.CURRENCY');
            SET @NOOFITEMS  = JSON_VALUE(@JSONData, '$.header.NOOFITEMS');
            SET @DISCOUNT   = JSON_VALUE(@JSONData, '$.header.DISCOUNT');
            SET @TOTALVALUE = JSON_VALUE(@JSONData, '$.header.TOTALVALUE');

            -- UPDATE HEADER
            UPDATE POHDR
            SET 
                CURRENCY   = @CURRENCY,
                NOOFITEMS  = @NOOFITEMS,
                DISCOUNT   = @DISCOUNT,
                TOTALVALUE = @TOTALVALUE,
                UPDATEDT   = GETDATE()
            WHERE PONO=@PONO AND FRAN=@FRAN AND SUPPLIER=@SUPPLIER;

            IF @@ROWCOUNT = 0
            BEGIN
                ROLLBACK TRANSACTION;
                SELECT CAST('{""result"":""Failed"",""message"":""PO not found or mismatch""}' AS NVARCHAR(MAX)) AS Response;
                RETURN;
            END

            -- DELETE OLD DETAILS
            DELETE FROM PODET
            WHERE PONO=@PONO AND FRAN=@FRAN AND SUPPLIER=@SUPPLIER;

            -- INSERT UPDATED DETAILS
            INSERT INTO PODET
            (
                FRAN, BRCH, WHSE, PODT, POTYPE, PONO,
                POSRL, MAKE, PART, QTY, UNITPRICE, DISCOUNT,
                VATVALUE, DISCOUNTVALUE, TOTALVALUE,
                SUPPLIER, CREATEDT, CREATETM, CREATEBY, CREATEREMARKS,
                UPDATEDT, UPDATETM, UPDATEBY, UPDATEREMARKS,
                PLANTYPE, PLANNO, PLANSRL, VATPERCENTAGE
            )
            SELECT 
                @FRAN,
                JSON_VALUE(value,'$.BRCH'),
                JSON_VALUE(value,'$.WHSE'),
                GETDATE(),
                JSON_VALUE(value,'$.POTYPE'),
                @PONO,
                JSON_VALUE(value,'$.POSRL'),
                JSON_VALUE(value,'$.MAKE'),
                JSON_VALUE(value,'$.PART'),
                JSON_VALUE(value,'$.QTY'),
                JSON_VALUE(value,'$.UNITPRICE'),
                JSON_VALUE(value,'$.DISCOUNT'),
                JSON_VALUE(value,'$.VATVALUE'),
                JSON_VALUE(value,'$.DISCOUNTVALUE'),
                JSON_VALUE(value,'$.TOTALVALUE'),
                @SUPPLIER,
                GETDATE(),
                JSON_VALUE(value,'$.CREATETM'),
                JSON_VALUE(value,'$.CREATEBY'),
                JSON_VALUE(value,'$.CREATEREMARKS'),
                GETDATE(),
                JSON_VALUE(value,'$.UPDATETM'),
                JSON_VALUE(value,'$.UPDATEBY'),
                ISNULL(JSON_VALUE(value,'$.UPDATEMARKS'), ''),
                JSON_VALUE(value,'$.PLANTYPE'),
                JSON_VALUE(value,'$.PLANNO'),
                JSON_VALUE(value,'$.PLANSRL'),
                JSON_VALUE(value,'$.VATPERCENTAGE')
            FROM OPENJSON(@JSONData, '$.details');

            COMMIT TRANSACTION;

            SELECT CAST('{""result"":""success""}' AS NVARCHAR(MAX)) AS Response;
            RETURN;
        END
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SELECT CAST('{""result"":""error"",""message"":""' + ERROR_MESSAGE() + '""}' AS NVARCHAR(MAX)) AS Response;
    END CATCH
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DROP PROCEDURE IF EXISTS [dbo].[SP_GetAndUpdatePO];
");
        }
    }
}
