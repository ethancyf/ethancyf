IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntry_Batch_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntry_Batch_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:			
-- Modified date:		
-- CR No.:				
-- Description:			
-- =============================================
-- =============================================
-- Modification History
-- Created by:		Chris YIM		
-- Modified date:	10 Sep 2019
-- CR No.			CRE19-001
-- Description:		Create Batch File
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntry_Batch_add]
	@Original_Student_File_ID		VARCHAR(15)
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
		SFH.Student_File_ID,
		[Student_Seq] = ROW_NUMBER() OVER (PARTITION BY SFH.Student_File_ID ORDER BY tblBatch.Student_Seq),
		tblBatch.Class_Name,
		tblBatch.Class_No,
		tblBatch.Contact_No,
		tblBatch.Doc_Code,
		tblBatch.Encrypt_Field1,
		tblBatch.Encrypt_Field2,
		tblBatch.Encrypt_Field2_1,
		tblBatch.Encrypt_Field2_2,
		tblBatch.Encrypt_Field3,
		tblBatch.Encrypt_Field4,
		tblBatch.DOB,
		tblBatch.Exact_DOB,
		tblBatch.Sex,
		tblBatch.Date_of_Issue,
		tblBatch.Permit_To_Remain_Until,
		tblBatch.Foreign_Passport_No,
		tblBatch.EC_Serial_No,
		tblBatch.EC_Reference_No,
		tblBatch.EC_Reference_No_Other_Format,
		tblBatch.Reject_Injection,
		tblBatch.Injected,
		tblBatch.Upload_Warning,
		tblBatch.Acc_Process_Stage,
		tblBatch.Acc_Process_Stage_Dtm,
		tblBatch.Voucher_Acc_ID,
		tblBatch.Temp_Voucher_Acc_ID,
		tblBatch.Acc_Type,
		tblBatch.Acc_Doc_Code,
		tblBatch.Temp_Acc_Record_Status,
		tblBatch.Temp_Acc_Validate_Dtm,
		tblBatch.Acc_Validation_Result,
		tblBatch.Validated_Acc_Found,
		tblBatch.Validated_Acc_Unmatch_Result,
		tblBatch.Vaccination_Process_Stage,
		tblBatch.Vaccination_Process_Stage_Dtm,
		[Entitle_ONLYDOSE] = CASE WHEN SFESPC.Entitle_ONLYDOSE IS NOT NULL THEN SFESPC.Entitle_ONLYDOSE ELSE tblBatch.Entitle_ONLYDOSE END,
		[Entitle_1STDOSE] = CASE WHEN SFESPC.Entitle_1STDOSE IS NOT NULL THEN SFESPC.Entitle_1STDOSE ELSE tblBatch.Entitle_1STDOSE END,
		[Entitle_2NDDOSE] = CASE WHEN SFESPC.Entitle_2NDDOSE IS NOT NULL THEN SFESPC.Entitle_2NDDOSE ELSE tblBatch.Entitle_2NDDOSE END,
		[Entitle_Inject] = 
			CASE 
				WHEN SFESPC.Entitle_ONLYDOSE IS NOT NULL THEN 
					CASE 
						WHEN SFESPC.Entitle_ONLYDOSE = 'Y' THEN 'Y'
						WHEN SFESPC.Entitle_1STDOSE = 'Y' THEN 'Y'
						WHEN SFESPC.Entitle_2NDDOSE = 'Y' THEN 'Y'
						ELSE 'N'
					END
				ELSE tblBatch.Entitle_ONLYDOSE 
			END,

		[Entitle_Inject_Fail_Reason] = CASE WHEN SFESPC.Entitle_Inject_Fail_Reason IS NOT NULL THEN SFESPC.Entitle_Inject_Fail_Reason ELSE tblBatch.Entitle_Inject_Fail_Reason END,
		tblBatch.Ext_Ref_Status,
		tblBatch.DH_Vaccine_Ref_Status,
		tblBatch.Transaction_ID,
		tblBatch.Transaction_Result,
		tblBatch.Create_By,
		tblBatch.Create_Dtm,
		tblBatch.Update_By,
		tblBatch.Update_Dtm,
		tblBatch.Last_Rectify_By,
		tblBatch.Last_Rectify_Dtm,
		tblBatch.Student_File_ID,
		tblBatch.Student_Seq
	FROM
		StudentFileHeader SFH
			INNER JOIN 
				(SELECT 
					SFHPD.Scheme_Code,
					SFHPD.Scheme_Seq,
					[Subsidize_Code] = CASE 
						WHEN SFHPD.Subsidize_Code = 'RQIV' THEN 'RQIV'
						WHEN SFHPD.Subsidize_Code = 'RWQIV' THEN 'RQIV'
						WHEN SFHPD.Subsidize_Code = 'RDQIV' THEN 'RQIV'
						ELSE SFHPD.Subsidize_Code
					END,
					SFE.*
				FROM 
					studentFileHeaderPrecheckDate SFHPD
						INNER JOIN StudentFileEntry SFE ON 
							SFHPD.Student_File_ID = SFE.Student_File_ID AND SFHPD.Class_Name = SFE.Class_Name
				WHERE 
					SFHPD.Student_File_ID = @Original_Student_File_ID) tblBatch
			ON
				SFH.Original_Student_File_ID = tblBatch.Student_File_ID
				AND SFH.Scheme_Code = tblBatch.Scheme_Code
				AND SFH.Scheme_Seq = tblBatch.Scheme_Seq
				AND SFH.Subsidize_Code = tblBatch.Subsidize_Code

			LEFT OUTER JOIN StudentFileEntrySubsidizePrecheck SFESPC ON
				SFH.Original_Student_File_ID = SFESPC.Student_File_ID
				AND SFH.Scheme_Code = SFESPC.Scheme_Code
				AND SFH.Scheme_Seq = SFESPC.Scheme_Seq
				AND SFH.Subsidize_Code = (CASE 
												WHEN SFESPC.Subsidize_Code = 'RQIV' THEN 'RQIV'
												WHEN SFESPC.Subsidize_Code = 'RWQIV' THEN 'RQIV'
												WHEN SFESPC.Subsidize_Code = 'RDQIV' THEN 'RQIV'
												ELSE SFESPC.Subsidize_Code
										  END)
				AND tblBatch.Class_Name =  SFESPC.Class_Name
				AND tblBatch.Student_Seq = SFESPC.Student_Seq
			LEFT OUTER JOIN StudentFileEntryPrecheckSubsidizeInject SFEPSI ON
				SFH.Original_Student_File_ID = SFEPSI.Student_File_ID
				AND SFH.Scheme_Code = SFEPSI.Scheme_Code
				AND SFH.Scheme_Seq = SFEPSI.Scheme_Seq
				AND SFH.Subsidize_Code = (CASE 
												WHEN SFEPSI.Subsidize_Code = 'RQIV' THEN 'RQIV'
												WHEN SFEPSI.Subsidize_Code = 'RWQIV' THEN 'RQIV'
												WHEN SFEPSI.Subsidize_Code = 'RDQIV' THEN 'RQIV'
												ELSE SFEPSI.Subsidize_Code
										  END)
				AND tblBatch.Class_Name =  SFEPSI.Class_Name
				AND tblBatch.Student_Seq = SFEPSI.Student_Seq
				AND SFEPSI.Mark_Injection = 'Y'

	WHERE 
		SFEPSI.Mark_Injection IS NOT NULL
		
	CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_Batch_add] TO HCSP
GO

