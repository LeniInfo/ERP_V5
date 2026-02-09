-- Add missing columns to REQUESTHDR table
-- Run this script directly on your database

USE [YourDatabaseName]; -- Replace with your actual database name
GO

-- Check if columns exist before adding them
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[REQUESTHDR]') AND name = 'DESCARABIC')
BEGIN
    ALTER TABLE [dbo].[REQUESTHDR]
    ADD [DESCARABIC] NVARCHAR(300) NULL;
    PRINT 'Added column DESCARABIC';
END
ELSE
BEGIN
    PRINT 'Column DESCARABIC already exists';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[REQUESTHDR]') AND name = 'VATVALUE')
BEGIN
    ALTER TABLE [dbo].[REQUESTHDR]
    ADD [VATVALUE] DECIMAL(22,3) NOT NULL DEFAULT 0;
    PRINT 'Added column VATVALUE';
END
ELSE
BEGIN
    PRINT 'Column VATVALUE already exists';
END
GO

-- Also check if DESCEN exists (it should based on the configuration)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[REQUESTHDR]') AND name = 'DESCEN')
BEGIN
    ALTER TABLE [dbo].[REQUESTHDR]
    ADD [DESCEN] NVARCHAR(300) NULL;
    PRINT 'Added column DESCEN';
END
ELSE
BEGIN
    PRINT 'Column DESCEN already exists';
END
GO

PRINT 'Script completed successfully!';
GO
