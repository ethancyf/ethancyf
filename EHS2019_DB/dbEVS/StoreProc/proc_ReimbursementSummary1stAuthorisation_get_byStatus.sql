IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementSummary1stAuthorisation_get_byStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementSummary1stAuthorisation_get_byStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	09 May 2018
-- CR No.:			INT18-003
-- Description:		Fix duplicate scheme in reimbursment second authorisation
-- =============================================
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
-- Create date:		18 May 2008
-- Description:		Reimbursement 1st Authorization Summary (by Status)
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
-- Modified date:   17 Aug 2009
-- Description:	    Adopt to new reimbursement schema
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   18 Aug 2009
-- Description:	    Get also the Display_Code from SchemeClaim
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   20 August 2009
-- Description:	    Add checking logic on user role scheme: selecting the schemes that the user has rights
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
-- Modified by:	    Lawrence TSANG
-- Modified date:   29 September 2009
-- Description:	    Handle expired scheme
-- =============================================

CREATE PROCEDURE [dbo].[proc_ReimbursementSummary1stAuthorisation_get_byStatus] 
	@tran_status	char(1),
	@user_id		varchar(20)
WITH RECOMPILE
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
	DECLARE @UserRoleScheme TABLE (
		Scheme_Code		char(10)
	)
	
	DECLARE @EffectiveScheme table (
		Scheme_Code		char(10),
		Scheme_Seq		smallint
	)
		
	CREATE TABLE #Reimburse (
		Reimburse_ID CHAR(15)
	)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	INSERT INTO @UserRoleScheme (Scheme_Code)
	SELECT Scheme_Code FROM UserRole WHERE User_ID = @user_id
	
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
	-- Get all Reimburse ID which under first authorisation
	INSERT INTO #Reimburse
	SELECT DISTINCT Reimburse_ID FROM ReimbursementAuthTran RT WITH (NOLOCK)
	WHERE RT.Authorised_Status = '1'

	SELECT 
		RA.Authorised_Dtm AS [firstAuthorizedDate],
		a.[Reimburse_ID] AS [reimburseID],
		RA.Authorised_By AS [firstAuthorizedBy],
		a.[noTran],
		a.[noSP],
		a.[voucherRedeem],
		a.[voucherAmount],
		a.[TotalAmountRMB],
		a.Scheme_Code,
		SC.Display_Code
	FROM
	(
		SELECT 
			RT.Reimburse_ID AS [Reimburse_ID],
			COUNT(Distinct VT.Transaction_ID) AS [noTran],
			COUNT(DISTINCT CAST(VT.SP_ID AS varchar) + ' (' + CAST(VT.Practice_Display_Seq AS varchar) + ')') AS [noSP],
			SUM(TD.Unit) AS [voucherRedeem],
			SUM(TD.Total_Amount) AS [voucherAmount],
			SUM(ISNULL(TD.Total_Amount_RMB, 0)) AS [TotalAmountRMB],
			VT.Scheme_Code
		FROM ReimbursementAuthTran RT WITH (NOLOCK)
			INNER JOIN #Reimburse R
			ON RT.Reimburse_ID = R.Reimburse_ID
			INNER JOIN VoucherTransaction VT WITH (NOLOCK)
			ON RT.Transaction_ID = VT.Transaction_ID
				AND VT.Record_Status = 'A'		
				AND RT.Authorised_Status = '1'
			INNER JOIN TransactionDetail TD WITH (NOLOCK)
					ON VT.Transaction_ID = TD.Transaction_ID
		GROUP BY
			RT.Reimburse_ID, VT.Scheme_Code
	) a
	INNER JOIN ReimbursementAuthorisation RA WITH (NOLOCK)
		ON a.Reimburse_ID = RA.Reimburse_ID
			AND a.Scheme_Code = RA.scheme_Code
			AND RA.Record_Status = 'A'
			AND RA.Authorised_Status = '1'
	INNER JOIN @EffectiveScheme ES
		ON a.Scheme_Code = ES.Scheme_Code
	INNER JOIN SchemeClaim SC WITH (NOLOCK)
		ON ES.Scheme_Code = SC.Scheme_Code
			AND ES.Scheme_Seq = SC.Scheme_Seq
					
	WHERE 
		a.Scheme_Code IN (SELECT Scheme_Code FROM @UserRoleScheme)
	ORDER BY 
		SC.Display_Seq

-- =============================================
-- House Keeping
-- =============================================
	DROP TABLE #Reimburse

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementSummary1stAuthorisation_get_byStatus] TO HCVU
GO
