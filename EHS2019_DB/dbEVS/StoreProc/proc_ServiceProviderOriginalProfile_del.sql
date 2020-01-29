IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderOriginalProfile_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderOriginalProfile_del]
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
-- Description:	Delete Service Provider Profile Original record
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProviderOriginalProfile_del]
	@enrolment_ref_no char(15)
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DELETE	FROM ServiceProviderOriginal
	WHERE	Enrolment_Ref_No = @enrolment_ref_no

	DELETE	FROM PracticeOriginal
	WHERE	Enrolment_Ref_No = @enrolment_ref_no

	DELETE	FROM BankAccountOriginal
	WHERE	Enrolment_Ref_No = @enrolment_ref_no

	DELETE	FROM ProfessionalOriginal
	WHERE	Enrolment_Ref_No = @enrolment_ref_no
	
	DELETE	FROM SchemeInformationOriginal
	WHERE	Enrolment_Ref_No = @enrolment_ref_no
	
	DELETE FROM MedicalOrganizationOriginal
	WHERE	Enrolment_Ref_No = @enrolment_ref_no
	
	DELETE FROM PracticeSchemeInfoOriginal
	WHERE	Enrolment_Ref_No = @enrolment_ref_no
	
	-- Delete third party enrolment information
	DELETE FROM ThirdPartyAdditionalFieldOriginal
	WHERE	Enrolment_Ref_No = @enrolment_ref_no
	
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderOriginalProfile_del] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderOriginalProfile_del] TO HCSP
GO