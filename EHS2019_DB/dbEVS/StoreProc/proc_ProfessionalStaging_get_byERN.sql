IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalStaging_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalStaging_get_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 24 May 2008
-- Description:	Retrieve the professional information form
--				table "ProfessionalStaging"
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionalStaging_get_byERN]
	@enrolment_ref_no	char(15)
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
	SELECT	Enrolment_Ref_No, Professional_Seq, SP_ID,
			Service_Category_Code, Registration_Code,
			Record_Status, Create_Dtm, Create_By
	FROM	ProfessionalStaging
	WHERE	Enrolment_Ref_No = @enrolment_ref_no
	ORDER BY Professional_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalStaging_get_byERN] TO HCVU
GO
