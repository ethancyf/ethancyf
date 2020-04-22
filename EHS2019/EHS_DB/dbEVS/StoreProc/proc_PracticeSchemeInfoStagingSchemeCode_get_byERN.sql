IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeSchemeInfoStagingSchemeCode_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeSchemeInfoStagingSchemeCode_get_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Kathy LEE
-- Create date:		18 Jun 209
-- Description:		Get the distinct scheme code in practice scheme information in staging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	01 July 2009
-- Description:		Retieve mscheme_code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	04 Aug 2009
-- Description:		Rename MScheme_Code -> Scheme_Code
-- =============================================
CREATE Procedure [dbo].[proc_PracticeSchemeInfoStagingSchemeCode_get_byERN]
	@enrolment_ref_no char(15)
as
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

	Select distinct Scheme_Code
	FROM  PracticeSchemeInfoStaging
	WHERE Enrolment_Ref_No  = @enrolment_ref_no

END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInfoStagingSchemeCode_get_byERN] TO HCVU
GO
