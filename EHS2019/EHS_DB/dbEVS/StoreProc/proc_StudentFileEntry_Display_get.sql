IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntry_Display_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntry_Display_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	1 Sep 2020
-- CR No.			CRE20-003 (Enhancement on Programme or Scheme using batch upload)
-- Description:		Handle Field Diff for Temp Account (Compare TA & VA)
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
-- Modified date:	17 Jul 2020
-- CR No.			CRE19-031 (VSS MMR Upload)
-- Description:		Add columns
-- =============================================
-- ==============================================
-- Modification History
-- Modified by:		Winnie SUEN	
-- Modified date:	22 Oct 2019
-- CR No.			CRE19-004 (RVP 2019-20)
-- Description:		Fix loading temp account when doc type is changed by Vaccination File Rectification
-- ===============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM		
-- Modified date:	09 Jul 2019
-- CR No.			CRE19-001
-- Description:		For SP search entry
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntry_Display_get]
	@Student_File_ID	varchar(15)
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================

	CREATE TABLE #tblAcc(
		SeqNo			INT,
		Voucher_Acc_ID	CHAR(15),
		Acc_Type		CHAR(1),
		Real_Voucher_Acc_ID	CHAR(15),
		Real_Acc_Type	CHAR(1)
	)

	CREATE NONCLUSTERED INDEX IX_ACC_SEQNO
		ON #tblAcc (SeqNo); 

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

	INSERT #tblAcc(
			SeqNo,
			Voucher_Acc_ID,
			Acc_Type,
			Real_Voucher_Acc_ID,
			Real_Acc_Type)
	SELECT
		Student_Seq,
		CASE
			WHEN Acc_Type IS NULL THEN ''
			WHEN Acc_Type = 'V' THEN SFE.Voucher_Acc_ID
			WHEN Acc_Type = 'T' THEN SFE.Temp_Voucher_Acc_ID
			ELSE ''
		END,
		Acc_Type,
		CASE
			WHEN Acc_Type IS NULL THEN NULL
			WHEN Acc_Type = 'V' THEN SFE.Voucher_Acc_ID
			WHEN Acc_Type = 'T' THEN 
				CASE
					WHEN TVA.Validated_Acc_ID <> '' THEN TVA.Validated_Acc_ID
					ELSE SFE.Temp_Voucher_Acc_ID
				END
			ELSE ''
		END,
		CASE
			WHEN Acc_Type IS NULL THEN NULL
			WHEN Acc_Type = 'V' THEN 'V'
			WHEN Acc_Type = 'T' THEN 
				CASE
					WHEN TVA.Validated_Acc_ID <> '' THEN 'V'
					ELSE 'T'
				END
			ELSE ''
		END
	FROM
		StudentFileEntry SFE
			LEFT OUTER JOIN TempVoucherAccount TVA
				ON SFE.Temp_Voucher_Acc_ID = TVA.Voucher_Acc_ID
	WHERE
		Student_File_ID = @Student_File_ID



-- =============================================
-- Return results
-- =============================================

	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	--

	SELECT 
		SFE.Student_File_ID,
		SFE.Student_Seq,
		Class_Name,
		Class_No,
		RIGHT('0000000000' + ISNULL(Class_No,''), 10) AS [Class_No_Sort],
		Contact_No,
		[Doc_Code] =
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN SFE.Doc_Code
				WHEN ACC.Real_Acc_Type = 'V'	THEN PInfo.Doc_Code
				WHEN ACC.Real_Acc_Type = 'T'	THEN TPInfo.Doc_Code
				ELSE ''
			END,
		[Doc_No] = 
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN CONVERT(VARCHAR(MAX), DecryptByKey(SFE.Encrypt_Field1))
				WHEN ACC.Real_Acc_Type = 'V'	THEN CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field1))
				WHEN ACC.Real_Acc_Type = 'T'	THEN CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field1))
				ELSE ''
			END,
		[Prefix] = 
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN ''
				WHEN ACC.Real_Acc_Type = 'V'	THEN 
					CASE WHEN PInfo.Encrypt_Field11 IS NULL THEN '' ELSE CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field11)) END
				WHEN ACC.Real_Acc_Type = 'T'	THEN
					CASE WHEN TPInfo.Encrypt_Field11 IS NULL THEN '' ELSE CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field11)) END
				ELSE ''
			END,
		[DocCode_DocNo] = 
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN LEFT(SFE.Doc_Code + Space(20), 20) + '' + CONVERT(varchar(MAX), DecryptByKey(SFE.Encrypt_Field1))
				WHEN ACC.Real_Acc_Type = 'V'	THEN 
					CASE 
						WHEN PInfo.Encrypt_Field11 IS NULL THEN LEFT(PInfo.Doc_Code + Space(20), 20) + CONVERT(varchar(MAX), DecryptByKey(PInfo.Encrypt_Field1))
						ELSE LEFT(PInfo.Doc_Code + Space(20), 20) + CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field11)) + CONVERT(varchar(MAX), DecryptByKey(PInfo.Encrypt_Field1))
					END
				WHEN ACC.Real_Acc_Type = 'T'	THEN 
					CASE 
						WHEN TPInfo.Encrypt_Field11 IS NULL THEN LEFT(TPInfo.Doc_Code + Space(20), 20) + CONVERT(varchar(MAX), DecryptByKey(TPInfo.Encrypt_Field1))
						ELSE LEFT(TPInfo.Doc_Code + Space(20), 20) + CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field11))  + CONVERT(varchar(MAX), DecryptByKey(TPInfo.Encrypt_Field1))
					END
				ELSE ''
			END,
		[Name_EN] =
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN CONVERT(VARCHAR(MAX), DecryptByKey(SFE.Encrypt_Field2))
				WHEN ACC.Real_Acc_Type = 'V'	THEN CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2))
				WHEN ACC.Real_Acc_Type = 'T'	THEN CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2))
				ELSE ''
			END,
		[Surname_EN] =
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN CONVERT(VARCHAR(MAX), DecryptByKey(SFE.Encrypt_Field2_1))
				WHEN ACC.Real_Acc_Type = 'V'	THEN CASE 
														WHEN CHARINDEX(',',CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2))) > 0 THEN
																LEFT(CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2)) ,
																	CHARINDEX(',',CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2))) - 1)
														ELSE
															CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2))
													 END
				WHEN ACC.Real_Acc_Type = 'T'	THEN CASE 
														WHEN CHARINDEX(',',CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2))) > 0 THEN
																LEFT(CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2)) ,
																	CHARINDEX(',',CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2))) - 1)
														ELSE
															CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2))
													 END
				ELSE ''
			END,
		[Given_Name_EN] =
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN CONVERT(VARCHAR(MAX), DecryptByKey(SFE.Encrypt_Field2_2))
				WHEN ACC.Real_Acc_Type = 'V'	
					THEN 
						CASE 
							WHEN CHARINDEX(',',CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2))) > 0 THEN
								RTRIM(LTRIM(
									SUBSTRING(CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2)) ,
												CHARINDEX(',',CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2))) + 1,
												LEN(CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2))) - CHARINDEX(',',CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2)))
									)))
							ELSE ''
						END
				WHEN ACC.Real_Acc_Type = 'T'	
					THEN 
						CASE 
							WHEN CHARINDEX(',',CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2))) > 0 THEN
								RTRIM(LTRIM(
									SUBSTRING(CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2)) ,
												CHARINDEX(',',CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2))) + 1,
												LEN(CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2))) - CHARINDEX(',',CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2)))
									)))
							ELSE ''
						END
				ELSE ''
			END,
		[Name_CH] =
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN CONVERT(NVARCHAR(MAX), DecryptByKey(SFE.Encrypt_Field3))
				WHEN ACC.Real_Acc_Type = 'V'	THEN CONVERT(NVARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field3))
				WHEN ACC.Real_Acc_Type = 'T'	THEN CONVERT(NVARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field3))
				ELSE ''
			END,
		[NameEN_NameCH] = 
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN CONVERT(VARCHAR(MAX), DecryptByKey(SFE.Encrypt_Field2)) + ' ' + CONVERT(NVARCHAR(MAX), DecryptByKey(SFE.Encrypt_Field3))
				WHEN ACC.Real_Acc_Type = 'V'	THEN CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2)) + ' ' + CONVERT(NVARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field3))
				WHEN ACC.Real_Acc_Type = 'T'	THEN CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2)) + ' ' + CONVERT(NVARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field3))
				ELSE ''
			END,	
		[Name_CH_Excel] = CONVERT(NVARCHAR(MAX), DecryptByKey(SFE.Encrypt_Field4)),
		[DOB] =
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN SFE.DOB
				WHEN ACC.Real_Acc_Type = 'V'	THEN PInfo.DOB
				WHEN ACC.Real_Acc_Type = 'T'	THEN TPInfo.DOB
				ELSE ''
			END,
		[Exact_DOB] =
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN SFE.Exact_DOB
				WHEN ACC.Real_Acc_Type = 'V'	THEN PInfo.Exact_DOB
				WHEN ACC.Real_Acc_Type = 'T'	THEN TPInfo.Exact_DOB
				ELSE ''
			END,
		[Sex] =
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN SFE.Sex
				WHEN ACC.Real_Acc_Type = 'V'	THEN PInfo.Sex
				WHEN ACC.Real_Acc_Type = 'T'	THEN TPInfo.Sex
				ELSE SFE.Sex
			END,
		[Date_of_Issue] =
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN SFE.Date_of_Issue
				WHEN ACC.Real_Acc_Type = 'V'	THEN PInfo.Date_of_Issue
				WHEN ACC.Real_Acc_Type = 'T'	THEN TPInfo.Date_of_Issue
				ELSE NULL
			END,
		[Permit_To_Remain_Until] =
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN SFE.Permit_To_Remain_Until
				WHEN ACC.Real_Acc_Type = 'V'	THEN PInfo.Permit_To_Remain_Until
				WHEN ACC.Real_Acc_Type = 'T'	THEN TPInfo.Permit_To_Remain_Until
				ELSE NULL
			END,
		[Foreign_Passport_No] =
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN SFE.Foreign_Passport_No
				WHEN ACC.Real_Acc_Type = 'V'	THEN PInfo.Foreign_Passport_No
				WHEN ACC.Real_Acc_Type = 'T'	THEN TPInfo.Foreign_Passport_No
				ELSE NULL
			END,
		[EC_Serial_No] =
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN SFE.EC_Serial_No
				WHEN ACC.Real_Acc_Type = 'V'	THEN PInfo.EC_Serial_No
				WHEN ACC.Real_Acc_Type = 'T'	THEN TPInfo.EC_Serial_No
				ELSE NULL
			END,
		[EC_Reference_No] =
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN SFE.EC_Reference_No
				WHEN ACC.Real_Acc_Type = 'V'	THEN PInfo.EC_Reference_No
				WHEN ACC.Real_Acc_Type = 'T'	THEN TPInfo.EC_Reference_No
				ELSE NULL
			END,
		[EC_Reference_No_Other_Format] =
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN SFE.EC_Reference_No_Other_Format
				WHEN ACC.Real_Acc_Type = 'V'	THEN PInfo.EC_Reference_No_Other_Format
				WHEN ACC.Real_Acc_Type = 'T'	THEN TPInfo.EC_Reference_No_Other_Format
				ELSE NULL
			END,
		[EC_Age] =
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN NULL
				WHEN ACC.Real_Acc_Type = 'V'	THEN PInfo.EC_Age
				WHEN ACC.Real_Acc_Type = 'T'	THEN TPInfo.EC_Age
				ELSE NULL
			END,
		[EC_Date_of_Registration] =
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN NULL
				WHEN ACC.Real_Acc_Type = 'V'	THEN PInfo.EC_Date_of_Registration
				WHEN ACC.Real_Acc_Type = 'T'	THEN TPInfo.EC_Date_of_Registration
				ELSE NULL
			END,
		Reject_Injection,
		Injected,
		Upload_Warning,
		
		Acc_Process_Stage,
		Acc_Process_Stage_Dtm,
		SFE.Voucher_Acc_ID,
		SFE.Temp_Voucher_Acc_ID,
		SFE.Acc_Type,
		Acc_Doc_Code,
		Temp_Acc_Record_Status,
		Temp_Acc_Validate_Dtm,
		[Manual_Validation] = 
			CASE
				WHEN Acc_Doc_Code IS NOT NULL AND ACC.Real_Acc_Type = 'T' THEN 
					CASE
						WHEN 
							(TPInfo.Deceased = 'N' OR TPInfo.Deceased IS NULL) AND
							(TPInfo.Validating IS NULL or TPInfo.Validating <> 'Y') AND
							(Acc_Doc_Code = 'ADOPC' AND (TPInfo.Exact_DOB in ('T', 'U', 'V') OR DT.Force_Manual_Validate = 'Y')) OR 
							(Acc_Doc_Code = 'Doc/I' AND (TPInfo.Date_of_Issue < '1 Sep 2003' OR DT.Force_Manual_Validate = 'Y')) OR
							(Acc_Doc_Code = 'EC' AND (TPInfo.Date_of_issue < '23 Jun 2003' OR TPInfo.Exact_DOB in ('T', 'U', 'V') OR DT.Force_Manual_Validate = 'Y')) OR
							(Acc_Doc_Code = 'HKBC' AND (TPInfo.Exact_DOB in ('T', 'U', 'V') OR DT.Force_Manual_Validate = 'Y' )) OR
							(Acc_Doc_Code = 'HKIC' AND DT.Force_Manual_Validate = 'Y') OR
							(Acc_Doc_Code = 'ID235B' AND DT.Force_Manual_Validate = 'Y') OR
							(Acc_Doc_Code = 'REPMT' AND (TPInfo.Date_of_Issue < '4 Jun 2007' OR DT.Force_Manual_Validate = 'Y')) OR
							(Acc_Doc_Code = 'VISA' AND DT.Force_Manual_Validate = 'Y') OR
							(Acc_Doc_Code = 'OW' AND DT.Force_Manual_Validate = 'Y') OR
							(Acc_Doc_Code = 'TW' AND DT.Force_Manual_Validate = 'Y') OR
							(Acc_Doc_Code = 'RFNo8' AND DT.Force_Manual_Validate = 'Y')
						THEN
							'Y'
						ELSE 'N'
					END
				ELSE 'N'
			END,

		[Acc_Validation_Result] = Acc_Validation_Result,
		[Acc_Validation_Result_EN] = 
			CASE
				WHEN PATINDEX('%|||%',Acc_Validation_Result) > 0 THEN LEFT(Acc_Validation_Result, PATINDEX('%|||%',Acc_Validation_Result) - 1)
				ELSE Acc_Validation_Result
			END,
		[Acc_Validation_Result_CHI] = 
			CASE
				WHEN PATINDEX('%|||%',Acc_Validation_Result) > 0 THEN RIGHT(Acc_Validation_Result, LEN(Acc_Validation_Result) - PATINDEX('%|||%',Acc_Validation_Result) - 3 + 1)
				ELSE ''
			END,

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
		SFE.Ext_Ref_Status,
		SFE.DH_Vaccine_Ref_Status,
		
		SFE.Transaction_ID,
		[Transaction_Voucher_Acc_ID] = VT.Voucher_Acc_ID,
		[Transaction_Temp_Voucher_Acc_ID] = VT.Temp_Voucher_Acc_ID,
		[Transaction_Record_Status] = VT.Record_Status,
		[Transaction_Record_Status_Desc_EN] = SD_VT.Status_Description,
		[Transaction_Record_Status_Desc_CHI] = SD_VT.Status_Description_Chi,
		[Transaction_Record_Status_Desc_CN] = SD_VT.Status_Description_CN,
		Transaction_Result,
		[Fail_Reason] = Transaction_Result,
		[Fail_Reason_EN] = 
			CASE
				WHEN PATINDEX('%|||%',Transaction_Result) > 0 THEN LEFT(Transaction_Result, PATINDEX('%|||%',Transaction_Result) - 1)
				ELSE Transaction_Result
			END,
		[Fail_Reason_CHI] = 
			CASE
				WHEN PATINDEX('%|||%',Transaction_Result) > 0 THEN RIGHT(Transaction_Result, LEN(Transaction_Result) - PATINDEX('%|||%',Transaction_Result) - 3 + 1)
				ELSE ''
			END,
		
		SFE.Create_By,
		SFE.Create_Dtm,
		SFE.Update_By,
		SFE.Update_Dtm,
		Last_Rectify_By,
		Last_Rectify_Dtm,
		SFE.TSMP,
		[Real_Voucher_Acc_ID] = ACC.Real_Voucher_Acc_ID,
		[Real_Account_ID_Reference_No] = 
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN NULL
				WHEN ACC.Real_Acc_Type = 'V'	THEN dbo.func_format_voucher_account_number('V', ACC.Real_Voucher_Acc_ID)
				WHEN ACC.Real_Acc_Type = 'T'	THEN dbo.func_format_voucher_account_number('T', ACC.Real_Voucher_Acc_ID)
				ELSE ''
			END,
		[Real_Acc_Type] = ACC.Real_Acc_Type,
		[Real_Record_Status] =
			CASE
				WHEN ACC.Real_Acc_Type IS NULL	THEN SFE.Temp_Acc_Record_Status
				WHEN ACC.Real_Acc_Type = 'V'	THEN VA.Record_Status
				WHEN ACC.Real_Acc_Type = 'T'	THEN TVA.Record_Status
				ELSE ''
			END,
		[Original_NameEN] = CONVERT(VARCHAR(MAX), DecryptByKey(SFE.Encrypt_Field2)),
		[Original_NameCN] = CONVERT(NVARCHAR(MAX), DecryptByKey(SFE.Encrypt_Field3)),
		[Original_DOB] = SFE.DOB,
		[Original_Exact_DOB] = SFE.Exact_DOB,
		[Original_SEX] = SFE.SEX,
		[Original_DateOfIssue] = SFE.Date_of_Issue,
		[Original_PermitToRemainUntil] = SFE.Permit_To_Remain_Until,
		[Original_ForeignPassportNo] = SFE.Foreign_Passport_No,
		[Original_ECSerialNo] = SFE.EC_Serial_No,
		[Original_ECReferenceNo] = SFE.EC_Reference_No,
		[Original_EC_ReferenceNoOtherFormat] = SFE.EC_Reference_No_Other_Format,
		[Field_Diff] =
			CASE
				WHEN ACC.Real_Acc_Type = 'V' THEN
					CASE
						WHEN 
							(CONVERT(VARCHAR(MAX), DecryptByKey(SFE.Encrypt_Field2)) <> CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2))) OR
							((CONVERT(VARCHAR(MAX), DecryptByKey(SFE.Encrypt_Field3)) <> '') AND 
								(CONVERT(NVARCHAR(MAX), DecryptByKey(SFE.Encrypt_Field3)) <> CONVERT(NVARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field3)))) OR
							(SFE.DOB <> PInfo.DOB) OR
							(SFE.SEX <> PInfo.SEX) OR
							(SFE.Exact_DOB <> PInfo.Exact_DOB) OR
							(SFE.Date_of_Issue IS NOT NULL AND (SFE.Date_of_Issue <> PInfo.Date_of_Issue) 
								AND (LTRIM(RTRIM(PInfo.Doc_Code)) IN ('HKIC','Doc/I','EC','REPMT'))) OR
							(SFE.Permit_To_Remain_Until IS NOT NULL AND (SFE.Permit_To_Remain_Until <> PInfo.Permit_To_Remain_Until) 
								AND (LTRIM(RTRIM(PInfo.Doc_Code)) = 'ID235B')) OR
							(SFE.Foreign_Passport_No IS NOT NULL 
								AND SFE.Foreign_Passport_No <> ''
								AND (SFE.Foreign_Passport_No <> PInfo.Foreign_Passport_No) 
								AND (LTRIM(RTRIM(PInfo.Doc_Code)) = 'VISA')) OR
							(SFE.EC_Serial_No IS NOT NULL 
								AND SFE.EC_Serial_No <> ''
								AND (SFE.EC_Serial_No <> PInfo.EC_Serial_No) 
								AND (LTRIM(RTRIM(PInfo.Doc_Code)) = 'EC')) OR
							(SFE.EC_Reference_No IS NOT NULL 
								AND SFE.EC_Reference_No <> ''
								AND (
										(SFE.EC_Reference_No_Other_Format IS NULL AND PInfo.EC_Reference_No_Other_Format = 'Y')
									OR
										(SFE.EC_Reference_No_Other_Format = 'Y' AND PInfo.EC_Reference_No_Other_Format IS NULL)
									OR
										(SFE.EC_Reference_No_Other_Format = 'Y' AND SFE.EC_Reference_No <> PInfo.EC_Reference_No) 
									OR 
										(SFE.EC_Reference_No_Other_Format IS NULL AND REPLACE(REPLACE(REPLACE(SFE.EC_Reference_No,'-',''),'(',''),')','') <> PInfo.EC_Reference_No)
									)
								AND (LTRIM(RTRIM(PInfo.Doc_Code)) = 'EC'))
						THEN 
							'Y'
						ELSE 
							'N'
					END
				WHEN ACC.Real_Acc_Type = 'T' AND DocNoPInfo.Voucher_Acc_ID IS NOT NULL THEN
					CASE
						WHEN 
							(CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2)) <> CONVERT(VARCHAR(MAX), DecryptByKey(DocNoPInfo.Encrypt_Field2))) OR
							((CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field3)) <> '') AND 
								(CONVERT(NVARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field3)) <> CONVERT(NVARCHAR(MAX), DecryptByKey(DocNoPInfo.Encrypt_Field3)))) OR
							(TPInfo.DOB <> DocNoPInfo.DOB) OR
							(TPInfo.SEX <> DocNoPInfo.SEX) OR
							(TPInfo.Exact_DOB <> DocNoPInfo.Exact_DOB) OR
							(TPInfo.Date_of_Issue IS NOT NULL AND (TPInfo.Date_of_Issue <> DocNoPInfo.Date_of_Issue) 
								AND (LTRIM(RTRIM(DocNoPInfo.Doc_Code)) IN ('HKIC','Doc/I','EC','REPMT'))) OR
							(TPInfo.Permit_To_Remain_Until IS NOT NULL AND (TPInfo.Permit_To_Remain_Until <> DocNoPInfo.Permit_To_Remain_Until) 
								AND (LTRIM(RTRIM(DocNoPInfo.Doc_Code)) = 'ID235B')) OR
							(TPInfo.Foreign_Passport_No IS NOT NULL 
								AND TPInfo.Foreign_Passport_No <> ''
								AND (TPInfo.Foreign_Passport_No <> DocNoPInfo.Foreign_Passport_No) 
								AND (LTRIM(RTRIM(DocNoPInfo.Doc_Code)) = 'VISA')) OR
							(TPInfo.EC_Serial_No IS NOT NULL 
								AND TPInfo.EC_Serial_No <> ''
								AND (TPInfo.EC_Serial_No <> DocNoPInfo.EC_Serial_No) 
								AND (LTRIM(RTRIM(DocNoPInfo.Doc_Code)) = 'EC')) OR
							(TPInfo.EC_Reference_No IS NOT NULL 
								AND TPInfo.EC_Reference_No <> ''
								AND (
										(TPInfo.EC_Reference_No_Other_Format IS NULL AND DocNoPInfo.EC_Reference_No_Other_Format = 'Y')
									OR
										(TPInfo.EC_Reference_No_Other_Format = 'Y' AND DocNoPInfo.EC_Reference_No_Other_Format IS NULL)
									OR
										(TPInfo.EC_Reference_No_Other_Format = 'Y' AND TPInfo.EC_Reference_No <> DocNoPInfo.EC_Reference_No) 
									OR 
										(TPInfo.EC_Reference_No_Other_Format IS NULL AND TPInfo.EC_Reference_No <> DocNoPInfo.EC_Reference_No)
									)
								AND (LTRIM(RTRIM(DocNoPInfo.Doc_Code)) = 'EC'))
						THEN 
							'Y'
						ELSE 
							'N'
					END
				ELSE 
					'N'
			END,

		Original_Student_File_ID,
		Original_Student_Seq,
		SFE.HKIC_Symbol,
		SFE.Service_Receive_Dtm,
		SFE.Manual_Add,

		SFEMMR.Non_immune_to_measles,
		SFEMMR.Ethnicity,
		SFEMMR.Category1,
		SFEMMR.Category2,
		SFEMMR.Lot_Number

	FROM
		StudentFileEntry SFE
			INNER JOIN #tblAcc ACC
				ON SFE.Student_Seq = ACC.SeqNo
			LEFT OUTER JOIN StudentFileEntryMMRField SFEMMR
				ON SFE.Student_File_ID = SFEMMR.Student_File_ID AND SFE.Student_Seq = SFEMMR.Student_Seq
			LEFT OUTER JOIN VoucherAccount VA
				ON ACC.Real_Voucher_Acc_ID = VA.Voucher_Acc_ID 
					AND ACC.Real_Acc_Type = 'V'
			LEFT OUTER JOIN TempVoucherAccount TVA
				ON ACC.Real_Voucher_Acc_ID = TVA.Voucher_Acc_ID 
					AND LTRIM(RTRIM(TVA.Validated_Acc_ID)) = '' 
					AND ACC.Real_Acc_Type = 'T'
			LEFT OUTER JOIN PersonalInformation PInfo
				ON VA.Voucher_Acc_ID = PInfo.Voucher_Acc_ID AND SFE.Acc_Doc_Code = PInfo.Doc_Code	
			LEFT OUTER JOIN TempPersonalInformation TPInfo
				ON TVA.Voucher_Acc_ID = TPInfo.Voucher_Acc_ID 
					-- AND SFE.Acc_Doc_Code = TPInfo.Doc_Code  -- Winnie SUEN CRE19-004 (RVP 2019-20)
			LEFT OUTER JOIN DocType DT
				ON TPInfo.Doc_Code = DT.Doc_Code
			LEFT OUTER JOIN VoucherTransaction VT 
				ON SFE.Transaction_ID = VT.Transaction_ID
			LEFT OUTER JOIN StatusData SD_VT 
				ON VT.Record_Status = SD_VT.Status_Value AND Enum_Class = 'ClaimTransStatus'
			LEFT OUTER JOIN [PersonalInformation] DocNoPInfo -- Retrieve VoucherAccount By Identity of Temp Account
				ON TPInfo.Encrypt_Field1 = DocNoPInfo.Encrypt_Field1 AND   
					TPInfo.Doc_Code = DocNoPInfo.Doc_Code

	WHERE
		SFE.Student_File_ID = @Student_File_ID
	ORDER BY
		SFE.Student_Seq
	

	--
	
	CLOSE SYMMETRIC KEY sym_Key
	
	
	DROP TABLE #tblAcc

END


GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_Display_get] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_Display_get] TO HCVU
GO

