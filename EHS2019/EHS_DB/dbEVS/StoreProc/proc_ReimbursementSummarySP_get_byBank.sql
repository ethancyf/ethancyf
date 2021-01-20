IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementSummarySP_get_byBank]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementSummarySP_get_byBank]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
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
-- Description:		Reimbursement Summary (By SP) (ych480:Encryption)
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
-- Modified by:		Lawrence TSANG
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
-- Modified by:		Clark YIP
-- Modified date:	30 Sep 2009
-- Description:		Handle the drop of obsolete columns
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:   
-- Description:	    
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_ReimbursementSummarySP_get_byBank] 
	@cutoff_dtm			datetime,
	@scheme_code		char(10),
	@authorised_status	char(1)
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
	EXEC [proc_SymmetricKey_open]
-- =============================================
-- Return results
-- =============================================

	SELECT
		T.SP_ID AS [spID],
		T.Practice_Display_Seq AS [practiceNo],
		CONVERT(varchar(40), DecryptByKey(A.Encrypt_Field2)) AS [spName],
		COUNT(Distinct T.Transaction_ID) AS [noTran],
		SUM(TD.Unit) AS [vouchersClaimed],
		SUM(TD.Total_Amount) AS [totalAmount],
		SUM(ISNULL(TD.Total_Amount_RMB, 0)) AS [TotalAmountRMB] 
		
	FROM
		ServiceProvider A
			INNER JOIN VoucherTransaction T
				ON A.SP_ID = T.SP_ID
					AND T.Record_Status = 'A'
			INNER JOIN TransactionDetail TD
				ON T.Transaction_ID = TD.Transaction_ID
			INNER JOIN ReimbursementAuthTran R
				ON T.Transaction_ID = R.Transaction_ID
				
	WHERE 
		T.Confirmed_Dtm <= @cutoff_dtm
			AND T.Scheme_Code = @scheme_code
			AND R.Authorised_status = @authorised_status
			
	GROUP BY
		T.SP_ID, T.Practice_Display_Seq, A.Encrypt_Field2
		
	ORDER BY
		T.SP_ID, T.Practice_Display_Seq

	EXEC [proc_SymmetricKey_close]
	
END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementSummarySP_get_byBank] TO HCVU
GO
