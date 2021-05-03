IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntryStaging_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntryStaging_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	20 Aug 2020
-- CR No.			CRE20-003 (Batch Upload)
-- Description:		Add columns (Manual Add)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	15 Jul 2020
-- CR No.			CRE19-031 (VSS MMR Upload)
-- Description:		Add columns
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM		
-- Modified DATE:	18 Sep 2019
-- CR No.			CRE19-001
-- Description:		Change length of class name
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN	
-- Modified DATE:	04 Sep 2019
-- CR No.			CRE19-001 (VSS 2019 - Claim Creation)
-- Description:		Retrieve new column
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM		
-- Modified DATE:	28 Aug 2019
-- CR No.			CRE19-001
-- Description:		Grant EXECUTE right for role HCSP
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified DATE:	10 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Add StudentFileEntryStaging
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntryStaging_add]
	@Student_File_ID				VARCHAR(15),
	@Student_Seq					INT,
	@Class_Name						NVARCHAR(40),
	@Class_No						NVARCHAR(10),
	@Contact_No						VARCHAR(20),
	@Doc_Code						CHAR(20),
	@Doc_No							VARCHAR(20),
	@Name_EN						VARCHAR(100),
	@Surname_EN_Original			VARCHAR(40),
	@Given_Name_EN_Original			VARCHAR(40),
	@Name_CH						NVARCHAR(40),
	@Name_CH_Excel					NVARCHAR(40),
	@DOB							DATETIME,
	@Exact_DOB						CHAR(1),
	@Sex							CHAR(1),
	@DATE_of_Issue					DATETIME,
	@Permit_To_Remain_Until			DATETIME,
	@Foreign_Passport_No			VARCHAR(20),
	@EC_Serial_No					VARCHAR(10),
	@EC_Reference_No				VARCHAR(40),
	@EC_Reference_No_Other_Format	CHAR(1),
	@Reject_Injection				CHAR(1),
	@Injected						CHAR(1),
	@Upload_Warning					VARCHAR(200),
	
	@Acc_Process_Stage				VARCHAR(20),
	@Acc_Process_Stage_Dtm			DATE,
	@Voucher_Acc_ID					CHAR(15),
	@Temp_Voucher_Acc_ID			CHAR(15),
	@Acc_Type						CHAR(1),
	@Acc_Doc_Code					CHAR(20),
	@Temp_Acc_Record_Status			CHAR(1),
	@Temp_Acc_ValiDATE_Dtm			DATETIME,
	@Acc_Validation_Result			VARCHAR(1000),
	@ValiDATEd_Acc_Found			CHAR(1),
	@ValiDATEd_Acc_Unmatch_Result	VARCHAR(1000),
	
	@Vaccination_Process_Stage		VARCHAR(20),
	@Vaccination_Process_Stage_Dtm	DATE,
	@Entitle_ONLYDOSE				CHAR(1),
	@Entitle_1STDOSE				CHAR(1),
	@Entitle_2NDDOSE				CHAR(1),
	@Entitle_3RDDOSE				CHAR(1),
	@Entitle_Inject					CHAR(1),
	@Entitle_Inject_Fail_Reason		VARCHAR(1000),
	@Ext_Ref_Status					VARCHAR(10),
	@DH_Vaccine_Ref_Status			VARCHAR(10),
	
	@Transaction_ID					CHAR(20),
	@Transaction_Result				VARCHAR(1000),
	
	@Create_By						VARCHAR(20),
	@Create_Dtm						DATETIME,
	@UpDATE_By						VARCHAR(20),
	@UpDATE_Dtm						DATETIME,
	@Last_Rectify_By				VARCHAR(20),
	@Last_Rectify_Dtm				DATETIME,
	
	@Original_Student_File_ID		VARCHAR(15),
	@Original_Student_Seq			INT,
	@HKIC_Symbol					CHAR(1),
	@Service_Receive_Dtm			DATETIME,
	@Manual_Add						CHAR(1)
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
	
	EXEC [proc_SymmetricKey_open]

	INSERT INTO StudentFileEntryStaging (
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
		DATE_of_Issue,
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
		Temp_Acc_ValiDATE_Dtm,
		Acc_Validation_Result,
		ValiDATEd_Acc_Found,
		ValiDATEd_Acc_Unmatch_Result,
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
		UpDATE_By,
		UpDATE_Dtm,
		Last_Rectify_By,
		Last_Rectify_Dtm,
		Original_Student_File_ID,
		Original_Student_Seq,
		HKIC_Symbol,
		Service_Receive_Dtm,
		Manual_Add

	) VALUES (
		@Student_File_ID,
		@Student_Seq,
		@Class_Name,
		@Class_No,
		@Contact_No,
		@Doc_Code,
		EncryptByKey(KEY_GUID('sym_Key'), @Doc_No),
		EncryptByKey(KEY_GUID('sym_Key'), @Name_EN),
		EncryptByKey(KEY_GUID('sym_Key'), @Surname_EN_Original),
		EncryptByKey(KEY_GUID('sym_Key'), @Given_Name_EN_Original),
		EncryptByKey(KEY_GUID('sym_Key'), @Name_CH),
		EncryptByKey(KEY_GUID('sym_Key'), @Name_CH_Excel),
		@DOB,
		@Exact_DOB,
		@Sex,
		@DATE_of_Issue,
		@Permit_To_Remain_Until,
		@Foreign_Passport_No,
		@EC_Serial_No,		
		@EC_Reference_No,
		@EC_Reference_No_Other_Format,
		@Reject_Injection,
		@Injected,
		@Upload_Warning,
		@Acc_Process_Stage,
		@Acc_Process_Stage_Dtm,
		@Voucher_Acc_ID,
		@Temp_Voucher_Acc_ID,
		@Acc_Type,
		@Acc_Doc_Code,
		@Temp_Acc_Record_Status,
		@Temp_Acc_ValiDATE_Dtm,
		@Acc_Validation_Result,
		@ValiDATEd_Acc_Found,
		@ValiDATEd_Acc_Unmatch_Result,
		@Vaccination_Process_Stage,
		@Vaccination_Process_Stage_Dtm,
		@Entitle_ONLYDOSE,
		@Entitle_1STDOSE,
		@Entitle_2NDDOSE,
		@Entitle_3RDDOSE,
		@Entitle_Inject,
		@Entitle_Inject_Fail_Reason,
		@Ext_Ref_Status,
		@DH_Vaccine_Ref_Status,
		@Transaction_ID,
		@Transaction_Result,
		@Create_By,
		@Create_Dtm,
		@UpDATE_By,
		@UpDATE_Dtm,
		@Last_Rectify_By,
		@Last_Rectify_Dtm,
		@Original_Student_File_ID,
		@Original_Student_Seq,
		@HKIC_Symbol,
		@Service_Receive_Dtm,
		@Manual_Add
	)
	
	EXEC [proc_SymmetricKey_close]
	

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryStaging_add] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryStaging_add] TO HCSP
GO

