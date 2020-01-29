IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ExchangeRate_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ExchangeRate_Add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- ==========================================================================================
-- Author:	Chris YIM
-- CR No.:  CRE13-019-02
-- Create Date:	02 Janurary 2015
-- Description:	Insert record into table - [ExchangeRate]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_ExchangeRate_Add]
    @ExchangeRate_ID varchar(9),
    @Effective_Date datetime,
	@ExchangeRate_Value float(5),
	@Record_Status char(1),
    @Create_By varchar(20),
	@Create_Dtm datetime,
	@Creation_Approve_By varchar(20),
	@Creation_Approve_Dtm datetime
AS BEGIN
-- ============================================================
-- Declaration
-- ============================================================
-- ============================================================
-- Validation
-- ============================================================
-- ============================================================
-- Initialization
-- ============================================================
-- ============================================================
-- Return results
-- ============================================================

    INSERT INTO ExchangeRate (
        [ExchangeRate_ID],
        [Effective_Date],
        [ExchangeRate_Value],
        [Record_Status],
        [Create_By],
        [Create_Dtm],
		[Update_By],
		[Update_Dtm],
		[Creation_Approve_By],
		[Creation_Approve_Dtm]
		)
    VALUES (
        @ExchangeRate_ID,
        @Effective_Date,
        @ExchangeRate_Value,
        @Record_Status,
        @Create_By,
        @Create_Dtm ,
        @Creation_Approve_By,
        GETDATE(),
        @Creation_Approve_By,
        @Creation_Approve_Dtm)
END
GO

GRANT EXECUTE ON [dbo].[proc_ExchangeRate_Add] TO HCVU
GO
