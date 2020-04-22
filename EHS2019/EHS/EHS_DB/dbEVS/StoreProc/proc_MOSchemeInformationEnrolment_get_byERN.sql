IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MOSchemeInformationEnrolment_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MOSchemeInformationEnrolment_get_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 17 August 2008
-- Description:	Get the Medical Organization Scheme Information to Table
--				MOSchemeInformationEnrolment
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_MOSchemeInformationEnrolment_get_byERN]
	@enrolment_ref_no char(15)
AS
BEGIN

	SET NOCOUNT ON;

   SELECT		Enrolment_Ref_No,
				Scheme_Code,
				MO_Practice_Display_Seq,
				Service_Fee_From,
				Service_Fee_To
				
	FROM  MOSchemeInformationEnrolment
	WHERE Enrolment_Ref_No  = @enrolment_ref_no
	
END
GO

GRANT EXECUTE ON [dbo].[proc_MOSchemeInformationEnrolment_get_byERN] TO HCPUBLIC
GO
