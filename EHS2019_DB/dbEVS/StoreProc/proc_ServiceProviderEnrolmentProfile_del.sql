IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderEnrolmentProfile_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderEnrolmentProfile_del]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE12-001
-- Modified by:		Koala CHENG
-- Modified date:	16 Jan 2012
-- Description:		Delete Service Provider Profile including Tables ThirdPartyEnrolment
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	02 Jun 2009
-- Description:		Remove "ERNProcessedEnrolment"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	02 Jun 2009
-- Description:		Also delete "PracticeSchemeInfoEnrolment", "MedicalOrganizationEnrolment",
--					"ERNProcessedEnrolment"
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 4 June 2008
-- Description:	Delete Service Provider Profile including
--				Tables "ServiceProviderEnrolment", "PracticeEnrolment",
--				"BankAccountEnrolment" and "ProfessionalEnrolment"
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProviderEnrolmentProfile_del]
	@enrolment_ref_no char(15)
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DELETE	FROM ServiceProviderEnrolment
	WHERE	Enrolment_Ref_No = @enrolment_ref_no

	DELETE	FROM PracticeEnrolment
	WHERE	Enrolment_Ref_No = @enrolment_ref_no

	DELETE	FROM BankAccountEnrolment
	WHERE	Enrolment_Ref_No = @enrolment_ref_no

	DELETE	FROM ProfessionalEnrolment
	WHERE	Enrolment_Ref_No = @enrolment_ref_no
	
	DELETE	FROM SchemeInformationEnrolment
	WHERE	Enrolment_Ref_No = @enrolment_ref_no
	
	DELETE FROM MedicalOrganizationEnrolment
	WHERE	Enrolment_Ref_No = @enrolment_ref_no
	
	DELETE FROM PracticeSchemeInfoEnrolment
	WHERE	Enrolment_Ref_No = @enrolment_ref_no
	
	-- Delete third party enrolment information
	DELETE FROM ThirdPartyAdditionalFieldEnrolment
	WHERE	Enrolment_Ref_No = @enrolment_ref_no
	
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderEnrolmentProfile_del] TO HCVU
GO
