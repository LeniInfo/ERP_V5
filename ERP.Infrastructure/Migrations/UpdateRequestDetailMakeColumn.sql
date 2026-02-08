-- Update MAKE column size in REQUESTDET table
-- Run this script directly on your database

USE [ERPV4]; -- Update if your database name is different
GO

-- Check current column size and update if needed
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[REQUESTDET]') AND name = 'MAKE')
BEGIN
    -- Check current max length
    DECLARE @CurrentMaxLength INT;
    SELECT @CurrentMaxLength = character_maximum_length 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = 'dbo' 
    AND TABLE_NAME = 'REQUESTDET' 
    AND COLUMN_NAME = 'MAKE';
    
    -- Only alter if current length is less than 50
    IF @CurrentMaxLength < 50
    BEGIN
        ALTER TABLE [dbo].[REQUESTDET]
        ALTER COLUMN [MAKE] NVARCHAR(50) NOT NULL;
        PRINT 'Updated MAKE column size to 50 characters';
    END
    ELSE
    BEGIN
        PRINT 'MAKE column is already 50 characters or larger';
    END
END
ELSE
BEGIN
    PRINT 'Column MAKE does not exist in REQUESTDET table';
END
GO

PRINT 'Script completed successfully!';
GO
