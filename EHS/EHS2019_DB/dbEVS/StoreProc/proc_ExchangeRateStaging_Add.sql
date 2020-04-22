IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ExchangeRateStaging_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ExchangeRateStaging_Add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- ==========================================================================================
-- Author:	Chris YIM
-- CR No.:  CRE13-019-02
-- Create Date:	31 December 2014
-- Description:	Insert record into table - [ExchangeRateStaging]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_ExchangeRateStaging_Add]
    @ExchangeRateStaging_ID varchar(9),
    @Effective_Date datetime,
	@ExchangeRate_Value float(5),
	@Record_Status char(1),
	@Record_Type char(1),
	@ExchangeRate_ID varchar(9),
    @Create_By varchar(20)
AS BEGIN
-- ============================================================
-- Declaration
-- ============================================================
-- ============================================================
-- Validation
-- ============================================================
	IF (SELECT COUNT(1) FROM ExchangeRateStaging
		Where  [Record_Status] = 'P'
		) > 0
	BEGIN
		IF @Record_Type = 'I'
		BEGIN
			RAISERROR('00006', 16, 1)
			RETURN @@error
		END
		ELSE IF @Record_Type = 'D'
		BEGIN
			RAISERROR('00011', 16, 1)
			RETURN @@error
		END
	END

-- ============================================================
-- Initialization
-- ============================================================
-- ============================================================
-- Return results
-- ============================================================

    INSERT INTO ExchangeRateStaging (
        [ExchangeRateStaging_ID],
        [Effective_Date],
        [ExchangeRate_Value],
        [Record_Status],
		[Record_Type],
		[ExchangeRate_ID],
        [Create_By],
        [Create_Dtm]
		)
    VALUES (
        @ExchangeRateStaging_ID,
        @Effective_Date,
        @ExchangeRate_Value,
        @Record_Status,
		@Record_Type,
		@ExchangeRate_ID,
        @Create_By,
        GETDATE())
END
GO

GRANT EXECUTE ON [dbo].[proc_ExchangeRateStaging_Add] TO HCVU
GO
