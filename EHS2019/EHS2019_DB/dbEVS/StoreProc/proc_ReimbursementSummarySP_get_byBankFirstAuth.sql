IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementSummarySP_get_byBankFirstAuth]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementSummarySP_get_byBankFirstAuth]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	07 March 2018
-- CR No.:			I-CRE17-007
-- Description:		Performance Tuning
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
-- Modified date:   14 Aug 2009
-- Description:	    Remove the @tran_status
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

CREATE PROCEDURE 	[dbo].[proc_ReimbursementSummarySP_get_byBankFirstAuth] 
							@reimburse_id		 char(15)
							,@scheme_code		 char(10)
WITH RECOMPILE
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Return results
-- =============================================

	SELECT
		a.spID,
		practiceNo,
		SP.[Encrypt_Field2],
		noTran,
		vouchersClaimed,
		totalAmount,
		[TotalAmountRMB]
	INTO #TempResult
	FROM
	(
		select 
			VT.SP_ID as spID, 
			VT.Practice_Display_Seq as practiceNo,
			COUNT(Distinct VT.Transaction_ID) as noTran,
			SUM(TD.Unit) as vouchersClaimed,
			SUM(TD.Total_Amount) as totalAmount,
			SUM(ISNULL(TD.Total_Amount_RMB, 0)) AS [TotalAmountRMB]

		from 
			ReimbursementAuthTran RT WITH (NOLOCK)
			INNER JOIN VoucherTransaction VT WITH (NOLOCK)
			ON RT.Transaction_ID = VT.Transaction_ID
				AND VT.Record_Status='A'
				AND RT.Authorised_status = '1'
				AND RT.Scheme_Code = @scheme_code
				AND RT.Reimburse_ID = @reimburse_id
			INNER JOIN TransactionDetail TD WITH (NOLOCK)
			ON VT.Transaction_ID = TD.Transaction_ID
		GROUP BY 
			VT.SP_ID, VT.practice_display_seq
	) a
	INNER JOIN ServiceProvider SP WITH (NOLOCK)
		ON a.spID = SP.SP_ID
	ORDER BY
		a.spID, a.practiceNo


	OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

-- =============================================
-- Return results
-- =============================================
	-- Decrypt value from temp table to gain performance benefit
	SELECT
		spID,
		practiceNo,
		convert(varchar(40), DecryptByKey([Encrypt_Field2])) as spName,
		noTran,
		vouchersClaimed,
		totalAmount,
		[TotalAmountRMB]
	FROM #TempResult

	CLOSE SYMMETRIC KEY sym_Key
-- =============================================
-- House Keeping
-- =============================================
	DROP TABLE #TempResult

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementSummarySP_get_byBankFirstAuth] TO HCVU
GO
