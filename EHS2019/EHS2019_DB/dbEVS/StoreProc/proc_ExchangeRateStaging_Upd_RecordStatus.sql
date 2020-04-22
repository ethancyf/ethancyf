IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ExchangeRateStaging_Upd_RecordStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ExchangeRateStaging_Upd_RecordStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- ==========================================================================================
-- Author:	Chris YIM
-- CR No.:  CRE13-019-02
-- Create Date:	02 Jan 2015
-- Description:	Update record's status to table - [ExchangeRateStaging] by [ExchangeRateStaging_ID]
-- ==========================================================================================
CREATE PROCEDURE [dbo].[proc_ExchangeRateStaging_Upd_RecordStatus]
	@ExchangeRate_ID varchar(8),
	@ExchangeRateStaging_ID varchar(9),
	@Record_Status char(1),
	@Update_By varchar(20),
	@TSMP binary(8)
AS
BEGIN
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM ExchangeRateStaging
		Where  ExchangeRateStaging_ID = @ExchangeRateStaging_ID
		) != @TSMP
	BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
	END

-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	DECLARE @Record_Type as char(1)
	
    SELECT
		@Record_Type = Record_Type
    FROM
        ExchangeRateStaging
    WHERE
        ExchangeRateStaging_ID = @ExchangeRateStaging_ID


	-- =============================================
	-- UPDATE TABLE
	-- =============================================
	IF @Record_Status = 'A'	-- Approved
	BEGIN
		UPDATE ExchangeRateStaging 
		SET
			ExchangeRate_ID = @ExchangeRate_ID,
			Record_Status = @Record_Status,
			Approve_By = @Update_By,
			Approve_Dtm = getdate()
		Where  
			ExchangeRateStaging_ID = @ExchangeRateStaging_ID
	END

	IF @Record_Status = 'R'	-- Rejected
	BEGIN
		UPDATE ExchangeRateStaging 
		SET
			Record_Status = @Record_Status,
			Reject_By = @Update_By,
			Reject_Dtm = getdate()
		Where  
			ExchangeRateStaging_ID = @ExchangeRateStaging_ID
	END

	IF @Record_Status = 'D'	-- Deleted
	BEGIN
		IF @Record_Type = 'I'	-- New Record
		BEGIN
			UPDATE ExchangeRateStaging 
			SET
				Record_Status = @Record_Status,
				Delete_By = @Update_By,
				Delete_Dtm = getdate()
			Where  
				ExchangeRateStaging_ID = @ExchangeRateStaging_ID
		END

		IF @Record_Type = 'D'	-- Approved Record
		BEGIN
			UPDATE ExchangeRateStaging 
			SET
				Record_Status = @Record_Status,
				Approve_By = @Update_By,
				Approve_Dtm = getdate()
			Where  
				ExchangeRateStaging_ID = @ExchangeRateStaging_ID
		END
	END

END
GO

GRANT EXECUTE ON [dbo].[proc_ExchangeRateStaging_Upd_RecordStatus] TO HCVU
GO
