IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeInformationOriginal_get_byERN_HCVU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeInformationOriginal_get_byERN_HCVU]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
-- =============================================
-- CR No.:		CRE12-001
-- Author:		Koala CHENG
-- Create date: 16 Jan 2012
-- Description:	Retrieve the Scheme Information from Table
--				SchemeInformationOriginal and SchemeBackOffice
-- =============================================
CREATE PROCEDURE [dbo].[proc_SchemeInformationOriginal_get_byERN_HCVU]
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
			
	FROM	SchemeInformationOriginal S
				INNER JOIN SchemeBackOffice E
					ON S.Scheme_Code = E.Scheme_Code
					
	WHERE	S.Enrolment_Ref_No = @enrolment_ref_no
				AND GETDATE() BETWEEN E.Effective_Dtm AND E.Expiry_Dtm
	
END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformationOriginal_get_byERN_HCVU] TO HCVU
GO
