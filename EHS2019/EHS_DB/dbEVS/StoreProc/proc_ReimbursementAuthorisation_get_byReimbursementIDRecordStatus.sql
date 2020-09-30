IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementAuthorisation_get_byReimbursementIDRecordStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ReimbursementAuthorisation_get_byReimbursementIDRecordStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Dickson LAW
-- Modified date:	07 March 2018
-- CR No.:			CRE17-004
-- Description:		Generate a new DPAR on EHCP basis
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
-- Create date:		13 August 2009
-- Description:		Get data from [ReimbursementAuthorisation]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	13 August 2009
-- Description:		Add @authorised_status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	26 August 2009
-- Description:		Add @scheme_code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
CREATE PROCEDURE [dbo].[proc_ReimbursementAuthorisation_get_byReimbursementIDRecordStatus]
	@reimburse_id		char(15),
	@authorised_status	char(1),
	@record_status		char(1),
	@scheme_code		char(10)
WITH RECOMPILE
AS BEGIN
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

	SELECT
		Authorised_Dtm,
		Authorised_Status,
		Authorised_By,
		Void_By,
		Void_Dtm,
		Void_Remark,
		Reimburse_ID,
		CutOff_Date,
		Record_Status,
		Create_By,
		Create_Dtm,
		Update_By,
		Update_Dtm,
		TSMP,
		Scheme_Code,
		ISNULL(Verification_Case_Available,'N')	AS Verification_Case_Available
	FROM
		ReimbursementAuthorisation
	
	WHERE		
		(@reimburse_id IS NULL OR Reimburse_ID = @reimburse_id)
			AND (@authorised_status IS NULL OR Authorised_Status = @authorised_status)
			AND (@record_status IS NULL OR Record_Status = @record_status)
			AND (@scheme_code IS NULL OR Scheme_Code = @scheme_code)
					
	ORDER BY
		Authorised_Dtm DESC

END
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementAuthorisation_get_byReimbursementIDRecordStatus] TO HCVU
GO
