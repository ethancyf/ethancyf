IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileHeaderStaging_move]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileHeaderStaging_move]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	13 Aug 2020
-- CR No.			CRE20-003 (Batch Upload)
-- Description:		Add columns
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	20 Jul 2020
-- CR No.			CRE19-031 (VSS MMR Upload)
-- Description:		Add columns (HKICSymbol, Service_Receive_Dtm)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG	
-- Modified date:	31 Sep 2019
-- CR No.			CRE19-001-04 (RVP Precheck)
-- Description:		Move table [StudentFileEntrySubsidizePrecheckStaging] too
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG	
-- Modified date:	05 Aug 2019
-- CR No.			CRE19-001
-- Description:		Handle new column [Service_Receive_Dtm_2ndDose], [Final_Checking_Report_Generation_Date_2ndDose] and [Upload_Precheck]
--					Move table [StudentFileEntryVaccineStaging] too
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	23 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Move the records in StudentFileHeader/EntryStaging to permanent
-- =============================================  

CREATE PROCEDURE [dbo].[proc_StudentFileHeaderStaging_move]
	@Student_File_ID			varchar(15),
	@Update_By					varchar(20)
AS BEGIN

	SET NOCOUNT ON;
	
-- =============================================
-- Declaration
-- =============================================
	DECLARE @Now datetime = GETDATE()
	

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	DELETE StudentFileHeader WHERE Student_File_ID = @Student_File_ID
	
	--
	
	DELETE
		StudentFileEntry
	WHERE
		Student_File_ID = @Student_File_ID
			AND Student_Seq IN (
				SELECT Student_Seq FROM StudentFileEntryStaging WHERE Student_File_ID = @Student_File_ID
			)

	--
	
	DELETE
		StudentFileEntryVaccine
	WHERE
		Student_File_ID = @Student_File_ID
			AND Student_Seq IN (
				SELECT Student_Seq FROM StudentFileEntryStaging WHERE Student_File_ID = @Student_File_ID
			)

	--
	
	DELETE
		StudentFileEntrySubsidizePrecheck
	WHERE
		Student_File_ID = @Student_File_ID
			AND Student_Seq IN (
				SELECT Student_Seq FROM StudentFileEntrySubsidizePrecheckStaging WHERE Student_File_ID = @Student_File_ID
			)

	--

	DELETE
		StudentFileEntryMMRField
	WHERE
		Student_File_ID = @Student_File_ID
			AND Student_Seq IN (
				SELECT Student_Seq FROM StudentFileEntryMMRFieldStaging WHERE Student_File_ID = @Student_File_ID
			)

	--

	INSERT INTO StudentFileHeader (
		Student_File_ID,
		Upload_Precheck,
		School_Code,
		SP_ID,
		Practice_Display_Seq,
		Service_Receive_Dtm,
		Service_Receive_Dtm_2,
		Scheme_Code,
		Scheme_Seq,
		Dose,
		Final_Checking_Report_Generation_Date,
		Final_Checking_Report_Generation_Date_2,
		Remark,
		Record_Status,
		Upload_By,
		Upload_Dtm,
		Last_Rectify_By,
		Last_Rectify_Dtm,
		Claim_Upload_By,
		Claim_Upload_Dtm,
		File_Confirm_By,
		File_Confirm_Dtm,
		Request_Remove_By,
		Request_Remove_Dtm,
		Request_Remove_Function,
		Confirm_Remove_By,
		Confirm_Remove_Dtm,
		Request_Claim_Reactivate_By,
		Request_Claim_Reactivate_Dtm,
		Confirm_Claim_Reactivate_By,
		Confirm_Claim_Reactivate_Dtm,
		Name_List_File_ID,
		Vaccination_Report_File_ID,
		Vaccination_Report_File_ID_2,
		Onsite_Vaccination_File_ID,
		Onsite_Vaccination_File_ID_2,
		Claim_Creation_Report_File_ID,
		Rectification_File_ID,
		Update_By,
		Update_Dtm,
		Service_Receive_Dtm_2ndDose,
		Service_Receive_Dtm_2ndDose_2,
		Final_Checking_Report_Generation_Date_2ndDose,
		Final_Checking_Report_Generation_Date_2ndDose_2,
		Subsidize_Code,
		Original_Student_File_ID,
		Request_Rectify_Status
	)
	SELECT
		Student_File_ID,
		Upload_Precheck,
		School_Code,
		SP_ID,
		Practice_Display_Seq,
		Service_Receive_Dtm,
		Service_Receive_Dtm_2,
		Scheme_Code,
		Scheme_Seq,
		Dose,
		Final_Checking_Report_Generation_Date,
		Final_Checking_Report_Generation_Date_2,
		Remark,
		Record_Status,
		Upload_By,
		Upload_Dtm,
		Last_Rectify_By,
		Last_Rectify_Dtm,
		Claim_Upload_By,
		Claim_Upload_Dtm,
		File_Confirm_By,
		File_Confirm_Dtm,
		Request_Remove_By,
		Request_Remove_Dtm,
		Request_Remove_Function,
		Confirm_Remove_By,
		Confirm_Remove_Dtm,
		Request_Claim_Reactivate_By,
		Request_Claim_Reactivate_Dtm,
		Confirm_Claim_Reactivate_By,
		Confirm_Claim_Reactivate_Dtm,
		Name_List_File_ID,
		Vaccination_Report_File_ID,
		Vaccination_Report_File_ID_2,
		Onsite_Vaccination_File_ID,
		Onsite_Vaccination_File_ID_2,
		Claim_Creation_Report_File_ID,
		Rectification_File_ID,
		@Update_By,
		@Now AS [Update_Dtm],
		Service_Receive_Dtm_2ndDose,
		Service_Receive_Dtm_2ndDose_2,
		Final_Checking_Report_Generation_Date_2ndDose,
		Final_Checking_Report_Generation_Date_2ndDose_2,
		Subsidize_Code,
		Original_Student_File_ID,
		Request_Rectify_Status
	FROM
		StudentFileHeaderStaging
	WHERE
		Student_File_ID = @Student_File_ID
		
	--
	
	INSERT INTO StudentFileEntry (
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
		Entitle_3RDDOSE,
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
		Original_Student_Seq,
		HKIC_Symbol,
		Service_Receive_Dtm
	)
	SELECT
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
		Entitle_3RDDOSE,
		Entitle_Inject,
		Entitle_Inject_Fail_Reason,
		Ext_Ref_Status,
		DH_Vaccine_Ref_Status,
		Transaction_ID,
		Transaction_Result,
		Create_By,
		Create_Dtm,
		@Update_By,
		@Now AS [Update_Dtm],
		Last_Rectify_By,
		Last_Rectify_Dtm,
		Original_Student_File_ID,
		Original_Student_Seq,
		HKIC_Symbol,
		Service_Receive_Dtm
	FROM
		StudentFileEntryStaging
	WHERE
		Student_File_ID = @Student_File_ID


		--
	
	INSERT INTO StudentFileEntryVaccine (
		Student_File_ID,
		Student_Seq,
		Provider,
		Vaccine_Seq,
		Scheme_Code,
		Scheme_Seq,
		Subsidize_Code,
		Subsidize_Item_Code,
		Subsidize_Desc,
		Subsidize_Desc_Chi,
		ForBar,
		Available_Item_Code,
		Available_Item_Desc,
		Available_Item_Desc_Chi,
		Service_Receive_Dtm,
		Record_Type,
		Is_Unknown_Vaccine,
		Practice_Name,
		Practice_Name_Chi,
		Create_Dtm
	)
	SELECT
		Student_File_ID,
		Student_Seq,
		Provider,
		Vaccine_Seq,
		Scheme_Code,
		Scheme_Seq,
		Subsidize_Code,
		Subsidize_Item_Code,
		Subsidize_Desc,
		Subsidize_Desc_Chi,
		ForBar,
		Available_Item_Code,
		Available_Item_Desc,
		Available_Item_Desc_Chi,
		Service_Receive_Dtm,
		Record_Type,
		Is_Unknown_Vaccine,
		Practice_Name,
		Practice_Name_Chi,
		Create_Dtm
	FROM
		StudentFileEntryVaccineStaging
	WHERE
		Student_File_ID = @Student_File_ID

	--
	
	INSERT INTO StudentFileEntrySubsidizePrecheck (
		Student_File_ID,
		Student_Seq,
		Class_Name,
		Scheme_Code,
		Scheme_Seq,
		Subsidize_Code,
		Entitle_ONLYDOSE,
		Entitle_1STDOSE,
		Entitle_2NDDOSE,
		Remark_ONLYDOSE,
		Remark_1STDOSE,
		Remark_2NDDOSE,
		Entitle_Inject_Fail_Reason,
		Inject_ONLYDOSE_1STDOSE,
		Inject_2NDDOSE,
		Create_Dtm
	)
	SELECT
		Student_File_ID,
		Student_Seq,
		Class_Name,
		Scheme_Code,
		Scheme_Seq,
		Subsidize_Code,
		Entitle_ONLYDOSE,
		Entitle_1STDOSE,
		Entitle_2NDDOSE,
		Remark_ONLYDOSE,
		Remark_1STDOSE,
		Remark_2NDDOSE,
		Entitle_Inject_Fail_Reason,
		Inject_ONLYDOSE_1STDOSE,
		Inject_2NDDOSE,
		Create_Dtm
	FROM
		StudentFileEntrySubsidizePrecheckStaging
	WHERE
		Student_File_ID = @Student_File_ID

	--

	INSERT INTO StudentFileEntryMMRField (
		Student_File_ID,
		Student_Seq,
		Non_immune_to_measles,
		Ethnicity,
		Category1,
		Category2,
		Lot_Number
	)
	SELECT
		Student_File_ID,
		Student_Seq,
		Non_immune_to_measles,
		Ethnicity,
		Category1,
		Category2,
		Lot_Number
	FROM
		StudentFileEntryMMRFieldStaging
	WHERE
		Student_File_ID = @Student_File_ID

	--

	DELETE StudentFileHeaderStaging WHERE Student_File_ID = @Student_File_ID
	DELETE StudentFileEntryStaging WHERE Student_File_ID = @Student_File_ID
	DELETE StudentFileEntryVaccineStaging WHERE Student_File_ID = @Student_File_ID
	DELETE StudentFileEntrySubsidizePrecheckStaging WHERE Student_File_ID = @Student_File_ID
	DELETE StudentFileEntryMMRFieldStaging WHERE Student_File_ID = @Student_File_ID
	

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeaderStaging_move] TO HCVU
GO

