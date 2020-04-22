IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementSummarySP_get_byBankForPaymentFile]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementSummarySP_get_byBankForPaymentFile]
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
-- Description:		Reimbursement Summary (By SP) for Payment File (ych480:Encryption)
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

CREATE PROCEDURE [dbo].[proc_ReimbursementSummarySP_get_byBankForPaymentFile] 
	@reimburse_id	char(15),
	@scheme_code	char(10)
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
	OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key
-- =============================================
-- Return results
-- =============================================

	SELECT
		T.SP_ID AS [spID],
		T.Practice_Display_Seq AS [practiceNo],
		CONVERT(varchar(40), DecryptByKey(A.Encrypt_Field2)) AS [spName],
		COUNT(Distinct t.Transaction_ID) AS [noTran],
		SUM(TD.Unit) AS [vouchersClaimed],
		SUM(TD.Total_Amount) AS [totalAmount],
		SUM(TD.Total_Amount_RMB) AS [totalAmountRMB]
	FROM
		ServiceProvider A
			INNER JOIN VoucherTransaction T
				ON A.SP_ID = T.SP_ID
			INNER JOIN ReimbursementAuthTran R
				ON T.Transaction_ID = R.Transaction_ID
					AND R.Authorised_Status = 'R'
			INNER JOIN TransactionDetail TD
				ON T.Transaction_ID = TD.Transaction_ID
	WHERE  
		R.reimburse_id = @reimburse_id
			AND R.Scheme_Code = @scheme_code
	GROUP BY
		T.SP_ID, T.Practice_Display_Seq, A.Encrypt_Field2
	ORDER BY
		T.sp_id, T.Practice_Display_Seq

	CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementSummarySP_get_byBankForPaymentFile] TO HCVU
GO
