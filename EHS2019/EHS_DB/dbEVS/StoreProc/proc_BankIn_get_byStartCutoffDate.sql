IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_BankIn_get_byStartCutoffDate]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_BankIn_get_byStartCutoffDate]
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
-- Author:			Clark Yip
-- Create date:		15 May 2008
-- Description:		Get record from BankIn table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   02 Feb 2009
-- Description:	    Change the noTran to the number of voucher transaction, not the number of records in bankfile
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   19 August 2009
-- Description:	    Get Scheme_Code and Display_Code from SchemeClaim
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   21 August 2009
-- Description:	    Change the checking logic from B.[Submitted_By] to R.[Authorised_By]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   25 Aug 2009
-- Description:	    Add the scheme_code in the inner join criteria
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	26 August 2009
-- Description:		Add criteria [ReimbursementAuthorisation].[Record_Status] = 'A' when inner join [ReimbursementAuthorisation]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   29 September 2009
-- Description:	    Handle expired scheme
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_BankIn_get_byStartCutoffDate]	
	@submitted_by	varchar(20),
	@Reimburse_ID	char(15),
	@start_dtm		datetime,
	@cutoff_dtm		datetime
WITH RECOMPILE
AS BEGIN
-- =============================================
-- Declaration
-- ============================================= 
	DECLARE @EffectiveScheme table (
		Scheme_Code		char(10),
		Scheme_Seq		smallint
	)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
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

	--Select record from BankIn table
	SELECT 
		B.Submission_Dtm AS [createDate],
		B.Completion_Dtm AS [completionTime],
		B.Transaction_File_Link AS [filePath],
		B.Reimburse_ID AS [reimburseID],
		(SELECT COUNT(1) FROM ReimbursementAuthTran A WHERE A.Reimburse_ID = B.Reimburse_ID AND A.Scheme_Code = B.Scheme_Code) AS [noTran],
		B.Vouchers_Count AS [vouchersClaimed],
		B.Total_Amount AS [totalAmount],
		(SELECT
			SUM(TD.Total_Amount_RMB)
		 FROM
			ReimbursementAuthTran A
				INNER JOIN VoucherTransaction VT
					ON A.Transaction_ID = VT.Transaction_ID
				INNER JOIN TransactionDetail TD
					ON VT.Transaction_ID = TD.Transaction_ID
		 WHERE
			A.Reimburse_ID = B.Reimburse_ID
				AND A.Scheme_Code = B.Scheme_Code
		) AS [totalAmountRMB],
		B.Scheme_Code,
		SC.Display_Code
		
	FROM
		BankIn B
			INNER JOIN ReimbursementAuthorisation R
				ON B.Reimburse_ID = R.Reimburse_ID
					AND B.Scheme_Code = R.Scheme_Code
					AND R.Authorised_Status = '2'
					AND R.Record_Status = 'A'
			INNER JOIN @EffectiveScheme ES
				ON B.Scheme_Code = ES.Scheme_Code
			INNER JOIN SchemeClaim SC
				ON ES.Scheme_Code = SC.Scheme_Code
					AND ES.Scheme_Seq = SC.Scheme_Seq
					
	WHERE
		R.Authorised_By = @submitted_by
			AND (@Reimburse_ID IS NULL OR B.Reimburse_ID = @Reimburse_ID)
			AND (
				(@start_dtm IS NULL AND @cutoff_dtm IS NULL) OR
					(
					@start_dtm = @cutoff_dtm AND DATEDIFF(mi, @start_dtm, submission_dtm) = 0
						OR
					@start_dtm <> @cutoff_dtm AND B.Submission_Dtm BETWEEN @start_dtm AND @cutoff_dtm
					)
				)
				
	ORDER BY
		B.Submission_Dtm DESC
	
END
GO

GRANT EXECUTE ON [dbo].[proc_BankIn_get_byStartCutoffDate] TO HCVU
GO
