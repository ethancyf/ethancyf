IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementDetail_get_byTransactionID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementDetail_get_byTransactionID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		17 September 2009
-- Description:		Reimbursement Detail - Get Payment File Submit Date and Bank Payment Date
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	18 September 2009
-- Description:		Retrieve [ReimbursementAuthorisation].[Authorised_By]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ReimbursementDetail_get_byTransactionID] 
	@transaction_id			char(20)
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	SELECT
		RA.[Authorised_Dtm] AS [PaymentFileSubmitDtm],
		RA.[Authorised_By] AS [PaymentFileSubmitBy],
		BI.[Value_Date] AS [BankPaymentDtm]
	
	FROM
		ReimbursementAuthTran RAT
			INNER JOIN ReimbursementAuthorisation RA
				ON RAT.Reimburse_ID = RA.Reimburse_ID
					AND RAT.Scheme_Code = RA.Scheme_Code
					AND RA.Authorised_Status = 'R'
			INNER JOIN BankIn BI
				ON RA.Reimburse_ID = BI.Reimburse_ID
					AND RA.Scheme_Code = BI.Scheme_Code
					
	WHERE
		RAT.Transaction_ID = @transaction_id

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementDetail_get_byTransactionID] TO HCVU
GO
