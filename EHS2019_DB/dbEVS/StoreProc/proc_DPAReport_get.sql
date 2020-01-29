IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DPAReport_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DPAReport_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	11 August 2015
-- CR No.:			CRE15-008
-- Description:		Simplified Chinese version of HCVSCHN reimbursement file
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
-- Create date:		29 Jul 2008
-- Description:		Get DPA report source (ych480:Encryption)
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
-- Modified date:   26 Feb 2009
-- Description:	    The bank account will be selected from transaction table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   14 Aug 2009
-- Description:	    Add the Scheme code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   21 Aug 2009
-- Description:	    Output the display_code instead of Scheme_code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   4 September 2009
-- Description:	    (1) Reformat the code
--					(2) Pre-calculate the total unit and total amount in @TransactionSum
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   23 September 2009
-- Description:	    Retrieve [SchemeClaim].[Scheme_Code] in second table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   29 September 2009
-- Description:	    Handle expired scheme
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Tommy LAM
-- Modified date:   25 Oct 2012
-- Description:	    Add 2 Columns - "[Profession] and [total_trans]" to Output
-- =============================================

CREATE PROCEDURE [dbo].[proc_DPAReport_get] 
	@reimburse_id		char(15), 
	@cutoff_Date_str	char(11),
	@scheme_code		char(10)
WITH RECOMPILE
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
	DECLARE @TransactionSum AS table (
		Transaction_ID		char(20),
		Total_Unit			int,
		Total_Amount		money,
		Total_Amount_RMB	money
	)
	
	DECLARE @EffectiveScheme table (
		Scheme_Code		char(10),
		Scheme_Seq		smallint
	)

	DECLARE @ReimbursementCurrency varchar(10)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	SELECT
		@ReimbursementCurrency = Reimbursement_Currency
	FROM
		SchemeClaim
	WHERE
		Scheme_Code = @scheme_code

	--

	INSERT INTO @TransactionSum (
		Transaction_ID,
		Total_Unit,
		Total_Amount,
		Total_Amount_RMB
	)
	SELECT
		VT.Transaction_ID,
		SUM(TD.Unit),
		SUM(TD.Total_Amount),
		SUM(TD.Total_Amount_RMB)
		
	FROM
		VoucherTransaction VT
			INNER JOIN TransactionDetail TD
				ON VT.Transaction_ID = TD.Transaction_ID
			INNER JOIN ReimbursementAuthTran RAT
				ON VT.Transaction_ID = RAT.Transaction_ID
			INNER JOIN ReimbursementAuthorisation RA
				ON RAT.Reimburse_ID = RA.Reimburse_ID
					AND RAT.Scheme_Code = RA.Scheme_Code
					AND RAT.Authorised_Status = RA.Authorised_Status
	
	WHERE
		RAT.Reimburse_ID = @reimburse_id
			AND RAT.Scheme_Code = @scheme_code
			AND RA.Record_Status = 'A'
	
	GROUP BY
		VT.Transaction_ID
		
	INSERT INTO @EffectiveScheme (
		Scheme_Code,
		Scheme_Seq
	)
	SELECT
		Scheme_Code,
		MAX(Scheme_Seq)
	FROM
		SchemeClaim
	WHERE
		GETDATE() >= Effective_Dtm
	GROUP BY
		Scheme_Code
-- =============================================
-- Return results
-- =============================================

	OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key
	
	SELECT  
		CAST(B.SP_ID AS varchar) + ' (' + CAST(B.SP_Practice_Display_Seq AS varchar) + ')' AS [SP_ID_PRACTICE],
		B.Display_Seq AS [Bank_Acc_Display_Seq],
		CASE @ReimbursementCurrency
			WHEN 'HKDRMB' THEN
				CASE ISNULL(CONVERT(nvarchar(40), DecryptByKey(SP.Encrypt_Field3)), '')
					WHEN '' THEN CONVERT(varchar(40), DecryptByKey(SP.Encrypt_Field2))
					ELSE CONVERT(nvarchar(40), DecryptByKey(SP.Encrypt_Field3))
				END
			ELSE CONVERT(varchar(40), DecryptByKey(SP.Encrypt_Field2))
		END AS [SP_Name],
		PRO.Service_Category_Code AS [Profession],
		B.Bank_Account_No,
		B.Bank_Acc_Holder,
		COUNT(TS.Transaction_ID) AS [total_trans],
		SUM(TS.Total_Unit) AS [voucher_claim], 
		SUM(TS.Total_Amount) AS [total_amount],
		SUM(TS.Total_Amount_RMB) AS [total_amount_rmb],
		RA.Authorised_Dtm AS [Report_Date]
		
	FROM
		@TransactionSum TS
			INNER JOIN VoucherTransaction T
				ON TS.Transaction_ID = T.Transaction_ID
			INNER JOIN ServiceProvider SP
				ON T.SP_ID = SP.SP_ID 
			INNER JOIN Practice P
				ON T.SP_ID = P.SP_ID 
					AND T.Practice_Display_Seq = P.Display_Seq
			INNER JOIN Professional PRO
				ON P.SP_ID = PRO.SP_ID
					AND P.Professional_Seq = PRO.Professional_Seq
			INNER JOIN BankAccount B
				ON T.SP_ID = B.SP_ID 
					AND T.Practice_Display_Seq = B.SP_Practice_Display_Seq
					AND T.Bank_Acc_Display_Seq = B.Display_Seq
			INNER JOIN ReimbursementAuthTran RT
				ON T.Transaction_ID = RT.Transaction_ID
			INNER JOIN ReimbursementAuthorisation RA
				ON RT.Reimburse_ID = RA.Reimburse_ID 
					AND RT.Scheme_Code = RA.Scheme_Code
					AND RT.Authorised_Status = RA.Authorised_Status

	WHERE RA.Record_Status = 'A'

	GROUP BY
		B.SP_ID,
		B.SP_Practice_Display_Seq,
		B.Display_Seq,
		SP.Encrypt_Field2,
		SP.Encrypt_Field3,
		PRO.Service_Category_Code,
		B.Bank_Account_No,
		B.Bank_Acc_Holder,
		RA.Authorised_Dtm

	ORDER BY
		B.SP_ID ASC,
		B.SP_Practice_Display_Seq ASC,
		B.Display_Seq ASC

	CLOSE SYMMETRIC KEY sym_Key

	SELECT
		@cutoff_Date_str AS [CutoffDate],
		@reimburse_id AS [ReimburseID],
		GETDATE() AS [GenerateDate],
		SC.Display_Code AS [SchemeCode],
		ES.Scheme_Code
		
	FROM 
		@EffectiveScheme ES
			INNER JOIN SchemeClaim SC
				ON ES.Scheme_Code = SC.Scheme_Code
					AND ES.Scheme_Seq = SC.Scheme_Seq
	
	WHERE ES.Scheme_Code = @scheme_code

END
GO

GRANT EXECUTE ON [dbo].[proc_DPAReport_get] TO HCVU
GO
