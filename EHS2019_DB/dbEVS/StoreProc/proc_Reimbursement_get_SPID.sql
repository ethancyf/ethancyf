IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Reimbursement_get_SPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Reimbursement_get_SPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 04 Sep 2008
-- Description:	Get Service Provider Related to the Reimbursement
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	  Clark YIP
-- Modified date: 18 Aug 2009
-- Description:	  Change to new schema
-- =============================================
CREATE PROCEDURE [dbo].[proc_Reimbursement_get_SPID]
	@reimburse_id char(15)
AS
BEGIN
	SET NOCOUNT ON;
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

SELECT Distinct RA.Reimburse_ID, RA.CutOff_Date, VT.SP_ID, SPUser.Default_Language
FROM [dbo].[ReimbursementAuthorisation] RA
	INNER JOIN [dbo].[ReimbursementAuthTran] RAT ON RA.Reimburse_ID = RAT.Reimburse_ID AND RA.Scheme_Code=RAT.Scheme_Code
	INNER JOIN [dbo].[VoucherTransaction] VT ON VT.Transaction_ID = RAT.Transaction_ID
	LEFT OUTER JOIN [dbo].[HCSPUserAC] SPUser ON VT.SP_ID = SPUser.SP_ID
WHERE RA.Reimburse_ID = @reimburse_id AND RA.Record_Status = 'A' AND RA.Authorised_Status = 'R'


END

GO

GRANT EXECUTE ON [dbo].[proc_Reimbursement_get_SPID] TO HCVU
GO
