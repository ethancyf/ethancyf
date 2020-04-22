IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeInformationEnrolment_get_byERN_HCVU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeInformationEnrolment_get_byERN_HCVU]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 6 August 2009
-- Description:	Retrieve the Scheme Information from Table
--				SchemeInformationEnrolment and SchemeBackOffice
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_SchemeInformationEnrolment_get_byERN_HCVU]
	@enrolment_ref_no char(15)
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

	SELECT	S.Enrolment_Ref_No,
			S.Scheme_Code,
			E.Display_Seq
			
	FROM	SchemeInformationEnrolment S
				INNER JOIN SchemeBackOffice E
					ON S.Scheme_Code = E.Scheme_Code
					
	WHERE	S.Enrolment_Ref_No = @enrolment_ref_no
				AND GETDATE() BETWEEN E.Effective_Dtm AND E.Expiry_Dtm
	
END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformationEnrolment_get_byERN_HCVU] TO HCVU
GO
