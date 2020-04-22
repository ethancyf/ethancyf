IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementSummaryTxn_get_bySPCutoffStatusBank]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementSummaryTxn_get_bySPCutoffStatusBank]
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
-- Description:		Reimbursement Summary (By Txn) (ych480:Encryption)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   02 Dec 2008
-- Description:	    Add to select the total amount. Total amount will be calculated based on the Claim_Amount field
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
-- Description:	    Group the transaction due to join with TransactionDetail
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   10 Sep 2009
-- Description:	    Remove the TD.Per_Unit_Value in grouping fields
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

CREATE PROCEDURE [dbo].[proc_ReimbursementSummaryTxn_get_bySPCutoffStatusBank]
	@sp_id         			varchar(8),
	@practice_display_seq	smallint,
	@bank_acc_no			varchar(30),
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
		P.Practice_name AS [practiceid], 
		SUM(TD.Unit) AS [voucherRedeem], 
		--TD.Per_Unit_Value AS [voucherAmount],
		T.Record_Status AS [status],
		R.Authorised_Status AS [authorised_status],
		SUM(TD.Total_Amount) AS [totalAmount],
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
			AND T.Bank_Account_No = @bank_acc_no
			AND T.Confirmed_Dtm <= @cutoff_dtm
			AND R.Scheme_Code = @scheme_code
			AND R.Authorised_Status = @authorised_status

	GROUP BY
		T.Transaction_ID, 
		T.Transaction_Dtm,
		A.Encrypt_Field2,
		T.SP_ID,
		T.Bank_Account_No, 
		P.Practice_name,		
		T.Record_Status,
		R.Authorised_Status

	CLOSE SYMMETRIC KEY sym_Key
	
END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementSummaryTxn_get_bySPCutoffStatusBank] TO HCVU
GO
