IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementCancelAuthorization_get_byReimbursementID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementCancelAuthorization_get_byReimbursementID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE16-002 (Revamp VSS)
-- Modified by:		Koala CHENG
-- Modified date:	15 September 2016
-- Description:		Performance tuning
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
-- Author:			Lawrence TSANG
-- Create date:		17 August 2009
-- Description:		Get Cancel Authorization
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   18 August 2009
-- Description:	    Add @user_id
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	8 September 2009
-- Description:		1. Retrieve [TransactionDetail].[Unit] and [TransactionDetail].[Total_Amount]
--					2. Add distinct when count the no of transaction
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

CREATE PROCEDURE [dbo].[proc_ReimbursementCancelAuthorization_get_byReimbursementID] 
	@reimburse_id	char(15),
	@user_id		char(20)
WITH RECOMPILE
AS BEGIN

-- =============================================
-- Declaration
-- =============================================

	DECLARE @TransSummary TABLE (
		First_Authorised_Dtm		datetime,
		First_Authorised_By		varchar(20),
		Second_Authorised_Dtm		datetime,
		Second_Authorised_By		varchar(20),
		Authorised_Status	char(1),
		Reimburse_ID		char(15),
		Scheme_Code			char(10),
		Total_Transaction	int,
		Total_Voucher		int,
		Total_Amount		money,
		Total_Amount_RMB	money
	)

	DECLARE @EffectiveScheme table (
		Scheme_Code		char(10),
		Display_Code	char(25),
		Display_Seq		smallint
	)

-- =============================================
-- Initialization
-- =============================================

	INSERT INTO @EffectiveScheme (
		Scheme_Code,
		Display_Code,
		Display_Seq
	)
	SELECT
		DISTINCT 
		sc.Scheme_Code,
		sc.Display_Code,
		sc.Display_Seq
	FROM
		SchemeClaim sc
		INNER JOIN UserRole ur
		ON sc.Scheme_Code = ur.Scheme_Code AND ur.User_ID = @user_id
	WHERE
		GETDATE() >= sc.Effective_Dtm


-- =============================================
-- Get the all Authorized records
-- =============================================
	INSERT INTO @TransSummary (
		First_Authorised_Dtm,
		First_Authorised_By,
		Second_Authorised_Dtm,
		Second_Authorised_By,
		Authorised_Status,
		Reimburse_ID,
		Scheme_Code,
		Total_Transaction,
		Total_Voucher,
		Total_Amount,
		Total_Amount_RMB
	)
	SELECT 
		A.First_Authorised_Dtm,
		A.First_Authorised_By,
		A.Second_Authorised_Dtm,
		A.Second_Authorised_By,
		A.Authorised_Status,
		A.Reimburse_ID,
		A.Scheme_Code,
		COUNT(Distinct A.Transaction_ID) AS [Total_Transaction],
		SUM(TD.Unit) AS [Total_Voucher],
		SUM(TD.Total_Amount) AS [Total_Amount],
		SUM(ISNULL(TD.Total_Amount_RMB, 0)) AS [Total_Amount_RMB]
	FROM
		ReimbursementAuthTran A WITH (NOLOCK)
		INNER JOIN TransactionDetail TD WITH (NOLOCK)
		ON A.Transaction_ID = TD.Transaction_ID
	WHERE
		A.Reimburse_ID = @reimburse_id
	GROUP BY
		A.First_Authorised_Dtm,
		A.First_Authorised_By,
		A.Second_Authorised_Dtm,
		A.Second_Authorised_By,
		A.Authorised_Status,
		A.Reimburse_ID,
		A.Scheme_Code

-- =============================================
-- Return results
-- =============================================
	SELECT 
		CASE WHEN S.Authorised_Status = '1' THEN	
			S.First_Authorised_Dtm
		WHEN S.Authorised_Status = '2' THEN
			S.Second_Authorised_Dtm
		END AS 'Authorised_Dtm',

		CASE WHEN S.Authorised_Status = '1' THEN	
			S.First_Authorised_By
		WHEN S.Authorised_Status = '2' THEN
			S.Second_Authorised_By
		END AS 'Authorised_By',
		
		S.Authorised_Status,
		S.Reimburse_ID,
		S.Scheme_Code,
		ES.Display_Code,
		S.Total_Transaction,
		S.Total_Voucher,
		S.Total_Amount,
		S.Total_Amount_RMB,
		ES.Display_Seq
	FROM
		@TransSummary S
		INNER JOIN @EffectiveScheme ES
		ON S.Scheme_Code = ES.Scheme_Code
	WHERE
		S.Authorised_Status IN ('1','2')
	ORDER BY ES.Display_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementCancelAuthorization_get_byReimbursementID] TO HCVU
GO
