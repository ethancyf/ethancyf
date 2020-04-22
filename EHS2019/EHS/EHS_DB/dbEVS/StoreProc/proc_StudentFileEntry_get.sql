IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntry_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntry_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN	
-- Modified date:	04 Sep 2019
-- CR No.			CRE19-001 (VSS 2019 - Claim Creation)
-- Description:		Retrieve new column
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM		
-- Modified date:	09 Jul 2019
-- CR No.			CRE19-001
-- Description:		Grant EXECUTE right for role HCSP
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	17 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Get StudentFileEntryStaging
-- =============================================    

CREATE PROCEDURE [dbo].[proc_StudentFileEntry_get]
	@Student_File_ID	varchar(15)
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

	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	--

	SELECT 
		Student_File_ID,
		Student_Seq,
		Class_Name,
		Class_No,
		Contact_No,
		Doc_Code,
		CONVERT(varchar(MAX), DecryptByKey(Encrypt_Field1)) AS [Doc_No],
		CONVERT(varchar(MAX), DecryptByKey(Encrypt_Field2)) AS [Name_EN],
		CONVERT(varchar(MAX), DecryptByKey(Encrypt_Field2_1)) AS [Surname_EN],
		CONVERT(varchar(MAX), DecryptByKey(Encrypt_Field2_2)) AS [Given_Name_EN],
		CONVERT(nvarchar(MAX), DecryptByKey(Encrypt_Field3)) AS [Name_CH],
		CONVERT(nvarchar(MAX), DecryptByKey(Encrypt_Field4)) AS [Name_CH_Excel],
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
		TSMP, 

		Original_Student_File_ID,
		Original_Student_Seq
	FROM
		StudentFileEntry
	WHERE
		Student_File_ID = @Student_File_ID
	ORDER BY
		Student_Seq
	
	--
	
	CLOSE SYMMETRIC KEY sym_Key
	

END


GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_get] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_get] TO HCVU
GO

