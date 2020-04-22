IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementSummaryBank_get_bySP]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementSummaryBank_get_bySP]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	2 January 2015
-- CR No.:			CRE13-019-02
-- Description:		Extend HCVS to China
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	24 March 2015
-- CR No.:			INT15-0002
-- Description:		Set the stored procedure to recompile each time
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		22 Apr 2008
-- Description:		Reimbursement Summary (By bank)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   02 Dec 2008
-- Description:	    Total amount will be calculated based on the Claim_Amount field
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   13 Aug 2009
-- Description:	    Join the ReimbursementAuthTran table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   14 Aug 2009
-- Description:	    Remove the @tran_status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   19 August 2009
-- Description:	    Add @authorised_status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	4 September 2009
-- Description:		Retrieve [TransactionDetail].[Unit] and [TransactionDetail].[Total_Amount]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   08 Sep 2009
-- Description:	    Add distinct when count the no of transaction
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    
-- Modified date:   
-- Description:	    
-- =============================================

CREATE PROCEDURE [dbo].[proc_ReimbursementSummaryBank_get_bySP] 
	@sp_id         			varchar(8),
	@practice_display_seq	smallint,
	@cutoff_dtm	      		datetime,
	@scheme_code			char(10),
	@authorised_status		char(1)
WITH RECOMPILE
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
		T.Bank_Account_No AS [bankAccountid], 
		P.Practice_Name AS [practiceid],
		COUNT(Distinct T.Transaction_ID) AS [noTran],
		SUM(TD.Unit) AS [vouchersClaimed],
		SUM(TD.Total_Amount) AS [totalAmount] ,
		SUM(ISNULL(TD.Total_Amount_RMB, 0)) AS [TotalAmountRMB] 
		
	FROM 
		ServiceProvider A 
			INNER JOIN Practice P
				ON A.SP_ID = P.SP_ID
			INNER JOIN VoucherTransaction T
				ON A.SP_ID = T.SP_ID
					AND P.Display_Seq = T.Practice_Display_Seq
					AND T.Record_Status = 'A' 
			INNER JOIN TransactionDetail TD
				ON T.Transaction_ID = TD.Transaction_ID
			INNER JOIN ReimbursementAuthTran R
				ON T.Transaction_ID = R.Transaction_ID
				
	WHERE 
		A.SP_ID = @sp_id
			AND T.Practice_Display_Seq = @practice_display_seq
			AND T.Confirmed_Dtm <= @cutoff_dtm 
			AND R.Scheme_Code = @scheme_code
			AND R.Authorised_status = @authorised_status
			
	GROUP BY
		T.Bank_Account_No, P.Practice_Name

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementSummaryBank_get_bySP] TO HCVU
GO
