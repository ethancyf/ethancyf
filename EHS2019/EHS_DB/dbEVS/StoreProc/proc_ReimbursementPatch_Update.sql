IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementPatch_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementPatch_Update]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		3 November 2010
-- Description:		Patch the ReimbursementAuthTran.Authorised_Status = 'R' to VoucherTransaction.Record_Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ReimbursementPatch_Update]
	@Reimburse_ID	char(15),
	@Scheme_Code	char(10)
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
	DECLARE @TransactionPatch table (
		Transaction_ID	char(20)
	)
	
	DECLARE @Update_By	varchar(20)


-- =============================================
-- Initialization
-- =============================================
	INSERT INTO @TransactionPatch (
		Transaction_ID
	)
	SELECT
		Transaction_ID
	FROM
		ReimbursementAuthTran
	WHERE
		Reimburse_ID = @Reimburse_ID
			AND Scheme_Code = @Scheme_Code

	SELECT
		@Update_By = Authorised_By
	FROM
		ReimbursementAuthorisation
	WHERE
		Reimburse_ID = @Reimburse_ID
			AND Scheme_Code = @Scheme_Code
			AND Authorised_Status = 'R'


-- =============================================
-- Return results
-- =============================================

	UPDATE
		VoucherTransaction
	SET
		Record_Status = 'R',
		Update_Dtm = GETDATE(),
		Update_By = @Update_By
	FROM
		VoucherTransaction VT
			INNER JOIN @TransactionPatch P
				ON VT.Transaction_ID = P.Transaction_ID	
					AND VT.Record_Status = 'A'


END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementPatch_Update] TO HCVU
GO
