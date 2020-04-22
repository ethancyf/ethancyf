IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransaction_CancelInvalidation]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VoucherTransaction_CancelInvalidation]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
	
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		23 March 2010
-- Description:		Cancel invalidation for a transaction
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherTransaction_CancelInvalidation]
	@Transaction_ID					char(20),
	@VoucherTransaction_TSMP		timestamp,
	@TransactionInvalidation_TSMP	timestamp,
	@Update_By						varchar(20)
AS BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM VoucherTransaction WHERE Transaction_ID = @Transaction_ID) != @VoucherTransaction_TSMP
	BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
	END
	
	IF (SELECT TSMP FROM TransactionInvalidation WHERE Transaction_ID = @Transaction_ID) != @TransactionInvalidation_TSMP
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
		TransactionInvalidation
	SET
		Record_Status = 'R',
		Update_Dtm = GETDATE(),
		Update_By = @Update_By
	WHERE
		Transaction_ID = @Transaction_ID

	DELETE FROM
		TransactionInvalidation
	WHERE
		Transaction_ID = @Transaction_ID

--

	UPDATE
		VoucherTransaction
	SET
		Invalidation = NULL,
		Update_By = @Update_By,
		Update_Dtm = GETDATE()
	WHERE
		Transaction_ID = @Transaction_ID
		
		
END
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransaction_CancelInvalidation] TO HCVU
GO
