IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementSummaryBank_get_bySPForPaymentFile]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementSummaryBank_get_bySPForPaymentFile]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	21 January 2015
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
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:	10 November 2010
-- Description:	    Exclude [VoucherTransaction].[Record_Status] = 'A' in filtering criteria
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		27 Dec 2008
-- Description:		Reimbursement Bank Summary For Payment File (By SP)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:	19 August 2009
-- Description:	    Add @scheme_code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   08 Sep 2009
-- Description:	    Inner join TransactionDetail to get the total unit and total amount
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    
-- Modified date:  
-- Description:	    
-- =============================================

CREATE PROCEDURE [dbo].[proc_ReimbursementSummaryBank_get_bySPForPaymentFile]
	@reimburse_id			char(15),
	@sp_id         			varchar(8),
	@practice_display_seq	smallint,
	@scheme_code			char(10)
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
		SUM(TD.Total_Amount) AS [totalAmount],
		SUM(TD.Total_Amount_RMB) AS [totalAmountRMB]
	FROM
		ServiceProvider A
			INNER JOIN Practice P
				ON A.SP_ID = P.SP_ID
			INNER JOIN VoucherTransaction T
				ON A.SP_ID = T.SP_ID
					AND P.Display_Seq = T.Practice_Display_Seq
			INNER JOIN ReimbursementAuthTran R
				ON T.Transaction_ID = R.Transaction_ID
					AND R.Authorised_Status = 'R'
			INNER JOIN TransactionDetail TD
				ON T.Transaction_ID = TD.Transaction_ID
	WHERE
		R.Reimburse_ID = @reimburse_id
			AND A.SP_ID = @sp_id 
			AND T.Practice_Display_Seq = @practice_display_seq
			AND R.Scheme_Code = @scheme_code
	GROUP BY
		T.Bank_Account_No, P.Practice_Name

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementSummaryBank_get_bySPForPaymentFile] TO HCVU
GO
