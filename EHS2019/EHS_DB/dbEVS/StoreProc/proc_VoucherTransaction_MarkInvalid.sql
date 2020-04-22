IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_MarkInvalid]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_MarkInvalid]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
	
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		22 March 2010
-- Description:		Mark invalid for a transaction
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherTransaction_MarkInvalid]
	@Transaction_ID			char(20),
	@TSMP					timestamp,
	@Update_By				varchar(20),
	@Invalidation_Type		char(2),
	@Invalidation_Remark	varchar(255)
AS BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM VoucherTransaction WHERE Transaction_ID = @Transaction_ID) != @TSMP
	BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
	END
	
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Process data
-- =============================================
	UPDATE
		VoucherTransaction
	SET
		Invalidation = 'P',
		Update_By = @Update_By,
		Update_Dtm = GETDATE()
	WHERE
		Transaction_ID = @Transaction_ID

	INSERT INTO TransactionInvalidation (
		Transaction_ID,
		Record_Status,
		Invalidation_Type,
		Invalidation_Remark,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By
	) VALUES (
		@Transaction_ID,
		'A',
		@Invalidation_Type,
		@Invalidation_Remark,
		GETDATE(),
		@Update_By,
		GETDATE(),
		@Update_By
	)

END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_MarkInvalid] TO HCVU
GO
