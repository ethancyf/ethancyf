IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementSummaryTxn_get_byCutoffForPaymentFile]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementSummaryTxn_get_byCutoffForPaymentFile]
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
-- Description:		Reimbursement Summary For Payment File(By Cutoff, First Auth) (ych480:encryption)
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

CREATE PROCEDURE [dbo].[proc_ReimbursementSummaryTxn_get_byCutoffForPaymentFile] 
	@reimburse_id			char(15),
	@sp_id					char(8),
	@practice_display_seq	smallint,
	@bank_acc_no			varchar(30),
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
	OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key
-- =============================================
-- Return results
-- =============================================

	SELECT
		T.Transaction_ID AS [tranNum],
		T.Transaction_Dtm AS [tranDate],
		CONVERT(varchar(40), DecryptByKey(A.Encrypt_Field2)) AS [SPName],
		T.SP_ID AS [SPID],
		T.Bank_Account_No AS [bankAccountid],
		p.Practice_Name AS [practiceid],
		SUM(TD.Unit) AS [voucherRedeem],
		--T.Per_Voucher_Value AS [voucherAmount],
		0 AS [voucherAmount],
		SUM(TD.Total_Amount) AS [totalAmount],
		SUM(TD.Total_Amount_RMB) AS [totalAmountRMB],
		T.Record_Status AS [status],
		R.Authorised_Status AS [authorised_status]
	FROM
		ServiceProvider A
			INNER JOIN Practice P
				ON A.SP_ID = P.SP_ID
			INNER JOIN VoucherTransaction t
				ON A.SP_ID = T.SP_ID
					AND P.Display_Seq = T.Practice_Display_Seq
			INNER JOIN ReimbursementAuthTran R
				ON T.Transaction_ID = R.Transaction_ID
					AND R.Authorised_status = 'R'
			INNER JOIN TransactionDetail TD
				ON T.Transaction_ID = TD.Transaction_ID
	WHERE
		R.Reimburse_ID = @reimburse_id
			AND T.SP_ID = @sp_id
			AND T.Bank_Account_No = @bank_acc_no
			AND R.Scheme_Code = @scheme_code
			AND T.Practice_Display_Seq = @practice_display_Seq

	GROUP BY
		T.Transaction_ID,
		T.Transaction_Dtm,
		A.Encrypt_Field2,
		T.SP_ID,
		T.Bank_Account_No,
		p.Practice_Name,		
		--T.Per_Voucher_Value,		
		T.Record_Status,
		R.Authorised_Status

	CLOSE SYMMETRIC KEY sym_Key
END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementSummaryTxn_get_byCutoffForPaymentFile] TO HCVU
GO
