IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_StudentFileEntryStaging_after_upd')
	DROP TRIGGER [dbo].[tri_StudentFileEntryStaging_after_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	16 Oct 2019
-- CR No.			INT19-0017
-- Description:		Fix insert log to [StudentFileEntryLOG] wrongly
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	04 Sep 2019
-- CR No.			CRE19-001 (VSS 2019 - Claim Creation)
-- Description:		Add columns
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	14 September 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Trigger for StudentFileEntryStaging
-- =============================================   

CREATE TRIGGER [dbo].[tri_StudentFileEntryStaging_after_upd]
   ON		[dbo].[StudentFileEntryStaging]
   AFTER	INSERT, UPDATE
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

    INSERT INTO StudentFileEntryStagingLOG (
		System_Dtm,
		Student_File_ID,
		Student_Seq,
		Class_Name,
		Class_No,
		Contact_No,
		Doc_Code,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field2_1,
		Encrypt_Field2_2,
		Encrypt_Field3,
		Encrypt_Field4,
		DOB,
		Exact_DOB,
		Sex,
		Date_of_Issue,
		Permit_To_Remain_Until,
		Foreign_Passport_No,
		EC_Serial_No,
		EC_Reference_No,
		EC_Reference_No_Other_Format,
		Reject_Injection,
		Injected,
		Upload_Warning,
		Acc_Process_Stage,
		Acc_Process_Stage_Dtm,
		Voucher_Acc_ID,
		Temp_Voucher_Acc_ID,
		Acc_Type,
		Acc_Doc_Code,
		Temp_Acc_Record_Status,
		Temp_Acc_Validate_Dtm,
		Acc_Validation_Result,
		Validated_Acc_Found,
		Validated_Acc_Unmatch_Result,
		Vaccination_Process_Stage,
		Vaccination_Process_Stage_Dtm,
		Entitle_ONLYDOSE,
		Entitle_1STDOSE,
		Entitle_2NDDOSE,
		Entitle_Inject,
		Entitle_Inject_Fail_Reason,
		Ext_Ref_Status,
		DH_Vaccine_Ref_Status,
		Transaction_ID,
		Transaction_Result,
		Create_By,
		Create_Dtm,
		Update_By,
		Update_Dtm,
		Last_Rectify_By,
		Last_Rectify_Dtm,
		Original_Student_File_ID,
		Original_Student_Seq
	)
	SELECT
		GETDATE(),
		Student_File_ID,
		Student_Seq,
		Class_Name,
		Class_No,
		Contact_No,
		Doc_Code,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field2_1,
		Encrypt_Field2_2,
		Encrypt_Field3,
		Encrypt_Field4,
		DOB,
		Exact_DOB,
		Sex,
		Date_of_Issue,
		Permit_To_Remain_Until,
		Foreign_Passport_No,
		EC_Serial_No,
		EC_Reference_No,
		EC_Reference_No_Other_Format,
		Reject_Injection,
		Injected,
		Upload_Warning,
		Acc_Process_Stage,
		Acc_Process_Stage_Dtm,
		Voucher_Acc_ID,
		Temp_Voucher_Acc_ID,
		Acc_Type,
		Acc_Doc_Code,
		Temp_Acc_Record_Status,
		Temp_Acc_Validate_Dtm,
		Acc_Validation_Result,
		Validated_Acc_Found,
		Validated_Acc_Unmatch_Result,
		Vaccination_Process_Stage,
		Vaccination_Process_Stage_Dtm,
		Entitle_ONLYDOSE,
		Entitle_1STDOSE,
		Entitle_2NDDOSE,
		Entitle_Inject,
		Entitle_Inject_Fail_Reason,
		Ext_Ref_Status,
		DH_Vaccine_Ref_Status,
		Transaction_ID,
		Transaction_Result,
		Create_By,
		Create_Dtm,
		Update_By,
		Update_Dtm,
		Last_Rectify_By,
		Last_Rectify_Dtm,
		Original_Student_File_ID,
		Original_Student_Seq
	FROM
		inserted


END
GO
