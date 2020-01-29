IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ExchangeRate_Upd_RecordStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ExchangeRate_Upd_RecordStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- ==========================================================================================
-- Author:	Chris YIM
-- CR No.:  CRE13-019-02
-- Create Date:	06 Jan 2015
-- Description:	Update record's status to table - [ExchangeRate] by [ExchangeRate_ID]
-- ==========================================================================================
CREATE PROCEDURE [dbo].[proc_ExchangeRate_Upd_RecordStatus]
	@ExchangeRate_ID varchar(8),
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
	IF (SELECT TSMP FROM ExchangeRate
		Where  ExchangeRate_ID = @ExchangeRate_ID
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

	UPDATE ExchangeRate
	SET
		Record_Status = @Record_Status,
		Update_By = @Update_By,
		Update_Dtm = getdate()
	Where  
		ExchangeRate_ID = @ExchangeRate_ID

END
GO

GRANT EXECUTE ON [dbo].[proc_ExchangeRate_Upd_RecordStatus] TO HCVU
GO
