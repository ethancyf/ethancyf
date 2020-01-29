IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderOriginalProfile_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderOriginalProfile_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	03 Jul 2018
-- CR No.			CRE17-016
-- Description:		Checking of PCD status during VSS enrolment
--					Add [Join_PCD] column
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	12 Feb 2016
-- CR No.			CRE16-019 (To implement token sharing between eHS(S) and eHRSS)
-- Description:		Rename PPI-ePR to eHRSS
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE16-002
-- Modified by:		Lawrence TSANG
-- Modified date:	9 August 2016
-- Description:		Add Clinic_Type
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE15-004
-- Modified by:		Winnie SUEN
-- Modified date:	19 June 2015
-- Description:		Add field Provide_Service
-- =============================================
-- =============================================
-- CR No.:		CRE12-001
-- Author:		Koala CHENG
-- Create date: 16 Jan 2012
-- Description:	Copy Service Provider Profile Enrolment to Original
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProviderOriginalProfile_add]
	@enrolment_ref_no char(15)
AS
BEGIN
	
	SET NOCOUNT ON;

	INSERT INTO [ServiceProviderOriginal]
		(Enrolment_Ref_No,
		Enrolment_Dtm,
		Room,
		[Floor],
		Block,
		Building,
		Building_Chi,
		District,
		Address_Code,
		Phone_Daytime,
		Fax,
		Email,
		Already_Joined_EHR,
		Join_EHR,
		Join_PCD,
		Application_Printed,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Batch_ID
		)
	SELECT 
		Enrolment_Ref_No,
		Enrolment_Dtm,
		Room,
		[Floor],
		Block,
		Building,
		Building_Chi,
		District,
		Address_Code,
		Phone_Daytime,
		Fax,
		Email,
		Already_Joined_EHR,
		Join_EHR,
		Join_PCD,
		Application_Printed,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field3,
		Batch_ID
	FROM ServiceProviderEnrolment
	WHERE Enrolment_Ref_No = @enrolment_ref_no

	INSERT INTO [PracticeOriginal]
		(Enrolment_Ref_No,
		Display_Seq,
		Practice_Name,
		Room,
		[Floor],
		Block,
		Building,
		Building_Chi,
		District,
		Address_Code,
		Service_Category_Code,
		Registration_Code,
		Professional_Seq,
		MO_Display_Seq,
		Practice_Name_Chi,
		Phone_Daytime
		)
	SELECT 
		Enrolment_Ref_No,
		Display_Seq,
		Practice_Name,
		Room,
		[Floor],
		Block,
		Building,
		Building_Chi,
		District,
		Address_Code,
		Service_Category_Code,
		Registration_Code,
		Professional_Seq,
		MO_Display_Seq,
		Practice_Name_Chi,
		Phone_Daytime
	FROM PracticeEnrolment
	WHERE Enrolment_Ref_No = @enrolment_ref_no

	INSERT INTO [BankAccountOriginal]
		(Enrolment_Ref_No,
		Display_Seq,
		SP_Practice_Display_Seq,
		Bank_Name,
		Branch_Name,
		Bank_Account_No,
		Bank_Acc_Holder
		)
	SELECT 
		Enrolment_Ref_No,
		Display_Seq,
		SP_Practice_Display_Seq,
		Bank_Name,
		Branch_Name,
		Bank_Account_No,
		Bank_Acc_Holder
	FROM BankAccountEnrolment
	WHERE Enrolment_Ref_No = @enrolment_ref_no

	INSERT INTO [ProfessionalOriginal]
		(Enrolment_Ref_No,
		Professional_Seq,
		Service_Category_Code,
		Registration_Code
		)
	SELECT 
		Enrolment_Ref_No,
		Professional_Seq,
		Service_Category_Code,
		Registration_Code
	FROM ProfessionalEnrolment
	WHERE Enrolment_Ref_No = @enrolment_ref_no
 
	INSERT INTO [SchemeInformationOriginal]
		(Enrolment_Ref_No,
		Scheme_Code
		)
	SELECT 
		Enrolment_Ref_No,
		Scheme_Code
	FROM SchemeInformationEnrolment
	WHERE Enrolment_Ref_No = @enrolment_ref_no

	INSERT INTO [MedicalOrganizationOriginal]
		(Enrolment_Ref_No,
		Display_Seq,
		MO_Eng_Name,
		MO_Chi_Name,
		Room,
		[Floor],
		Block,
		Building,
		Building_Chi,
		District,
		Address_Code,
		BR_Code,
		Phone_Daytime,
		Email,
		Fax,
		Relationship,
		Relationship_Remark
		)
	SELECT 
		Enrolment_Ref_No,
		Display_Seq,
		MO_Eng_Name,
		MO_Chi_Name,
		Room,
		[Floor],
		Block,
		Building,
		Building_Chi,
		District,
		Address_Code,
		BR_Code,
		Phone_Daytime,
		Email,
		Fax,
		Relationship,
		Relationship_Remark
	FROM MedicalOrganizationEnrolment
	WHERE Enrolment_Ref_No = @enrolment_ref_no

	INSERT INTO [PracticeSchemeInfoOriginal]
		(Enrolment_Ref_No,
		Subsidize_Code,
		Practice_Display_Seq,
		Service_Fee,
		Scheme_Code,
		ProvideServiceFee,
		Provide_Service,
		Clinic_Type
		)
	SELECT 
		Enrolment_Ref_No,
		Subsidize_Code,
		Practice_Display_Seq,
		Service_Fee,
		Scheme_Code,
		ProvideServiceFee,
		Provide_Service,
		Clinic_Type
	FROM PracticeSchemeInfoEnrolment
	WHERE Enrolment_Ref_No = @enrolment_ref_no

	INSERT INTO [ThirdPartyAdditionalFieldOriginal]
		(Sys_Code,
		Enrolment_Ref_No,
		Practice_Display_Seq,
		AdditionalFieldID,
		AdditionalFieldValueCode,
		Create_dtm
		)
	SELECT 
		Sys_Code,
		Enrolment_Ref_No,
		Practice_Display_Seq,
		AdditionalFieldID,
		AdditionalFieldValueCode,
		Create_dtm
	FROM ThirdPartyAdditionalFieldEnrolment
	WHERE Enrolment_Ref_No = @enrolment_ref_no

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderOriginalProfile_add] TO HCVU
GO
