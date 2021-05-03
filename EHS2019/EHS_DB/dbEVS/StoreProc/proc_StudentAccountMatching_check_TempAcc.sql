IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentAccountMatching_check_TempAcc]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentAccountMatching_check_TempAcc]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Martin Tang
-- Modified date:	20 Apr 2021
-- Description:		Extend patient name's maximum length
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	16 Oct 2019
-- CR No.:			INT19-0017
-- Description:		Fix student without English give name
-- =============================================
-- =============================================
-- Author:			Winnie SUEN
-- Create date:		1 Aug 2019
-- CR No.:			CRE19-001 (VSS 2019)
-- Description:		Check for temp account is converted to validated acct 
--					Overwrite student entry with validate acct personal info
--					Will not update Acc_Validation_Result, Validated_Acc_Unmatch_Result
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentAccountMatching_check_TempAcc]
	@Acc_Process_Stage				varchar(20)
AS BEGIN

	SET NOCOUNT ON;
	
-- =============================================
-- Declaration
-- =============================================

	DECLARE @TempStudentFileEntry AS TABLE (
		Student_File_ID			varchar(15),
		Student_Seq				int,
		Temp_Voucher_Acc_ID		char(15)
	)

	DECLARE @Result AS TABLE (
		Student_File_ID			varchar(15),
		Student_Seq				int,
		Voucher_Acc_ID			char(15),
		Temp_Voucher_Acc_ID		char(15),
		Acc_Type				char(1),				
		Temp_Acc_Record_Status	char(1),
		Temp_Acc_Validate_Dtm	datetime,
		Validated_Acc_Found		char(1),
		Acc_Doc_Code			char(20),
		Name_EN					VARCHAR(100),
		Surname_EN				VARCHAR(100),
		Given_Name_EN			VARCHAR(100),
		Name_CH					NVARCHAR(6),
		DOB						Datetime,
		Exact_DOB				CHAR(1),
		Sex						CHAR(1),
		Date_of_Issue			Datetime,
		Permit_To_Remain_Until	Datetime,
		Foreign_Passport_No		VARCHAR(20),
		EC_Serial_No			VARCHAR(10),
		EC_Reference_No			VARCHAR(40),
		EC_Reference_No_Other_Format	CHAR(1)
	)
	
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	-- Retrieve the student account list mark with Temp acct only

	-- From Staging
	-- Recurd Stauts: 
	-- Uploaded (Pending Verification) / Rectified (Pending Verification) / Claim (Pending Claim Creation)
	INSERT INTO @TempStudentFileEntry (Student_File_ID, Student_Seq, Temp_Voucher_Acc_ID)
	SELECT
		ST.Student_File_ID,
		ST.Student_Seq,
		ST.Temp_Voucher_Acc_ID
	FROM
		StudentFileHeaderStaging SH
		INNER JOIN StudentFileEntryStaging ST ON SH.Student_File_ID = ST.Student_File_ID
	WHERE
		SH.Record_Status IN ('PU','PR','PT')
		AND ISNULL(ST.Acc_Process_Stage,'') <> ''
		AND ISNULL(ST.Acc_Type, '') = 'T'
		AND ISNULL(ST.Voucher_Acc_ID, '') = ''
		AND ST.Temp_Voucher_Acc_ID IS NOT NULL

	UNION

	-- From Permanent
	-- Record Status: (All except Claim Suspended / Completed / Removed)
	SELECT
		ST.Student_File_ID,
		ST.Student_Seq,
		ST.Temp_Voucher_Acc_ID
	FROM
		StudentFileHeader SH
		INNER JOIN StudentFileEntry ST ON SH.Student_File_ID = ST.Student_File_ID
	WHERE
		SH.Record_Status NOT IN ('CS','C','R')   
		AND ISNULL(ST.Acc_Process_Stage,'') <> ''
		AND ISNULL(ST.Acc_Type, '') = 'T'
		AND ISNULL(ST.Voucher_Acc_ID, '') = ''
		AND ST.Temp_Voucher_Acc_ID IS NOT NULL


EXEC [proc_SymmetricKey_open]


	-- Get Validated acct person info if converted to Validated Account
	INSERT INTO @Result (Student_File_ID, Student_Seq, Voucher_Acc_ID, Temp_Voucher_Acc_ID, Acc_Type,
						Temp_Acc_Record_Status, Temp_Acc_Validate_Dtm, Validated_Acc_Found, Acc_Doc_Code,
						Name_EN, Surname_EN, Given_Name_EN, Name_CH, DOB, Exact_DOB, Sex, Date_of_Issue, 
						Permit_To_Remain_Until, Foreign_Passport_No, EC_Serial_No, EC_Reference_No, EC_Reference_No_Other_Format)
	SELECT
		SE.Student_File_ID,
		SE.Student_Seq,
		TA.Validated_Acc_ID,
		TA.Voucher_Acc_ID,
		'V',
		TA.Record_Status,
		TP.Check_Dtm,
		'Y',
		VP.Doc_Code,
		[Name_EN] = CONVERT(varchar(MAX), DecryptByKey(VP.Encrypt_Field2)),
		[Surname_EN] = 
			CASE 
				WHEN CHARINDEX(',',CONVERT(varchar(MAX), DecryptByKey(VP.Encrypt_Field2))) > 0 THEN
						LEFT(CONVERT(varchar(MAX), DecryptByKey(VP.Encrypt_Field2)) ,
							CHARINDEX(',',CONVERT(varchar(MAX), DecryptByKey(VP.Encrypt_Field2))) - 1)
				ELSE
					CONVERT(varchar(MAX), DecryptByKey(VP.Encrypt_Field2))
			END,
		[Given_Name_EN] =
				CASE 
					WHEN CHARINDEX(',',CONVERT(VARCHAR(MAX), DecryptByKey(VP.Encrypt_Field2))) > 0 THEN
						RTRIM(LTRIM(
							SUBSTRING(CONVERT(VARCHAR(MAX), DecryptByKey(VP.Encrypt_Field2)) ,
										CHARINDEX(',',CONVERT(VARCHAR(MAX), DecryptByKey(VP.Encrypt_Field2))) + 1,
										LEN(CONVERT(VARCHAR(MAX), DecryptByKey(VP.Encrypt_Field2))) - CHARINDEX(',',CONVERT(VARCHAR(MAX), DecryptByKey(VP.Encrypt_Field2)))
							)))
					ELSE ''
				END,
		[Name_CH] = CONVERT(nvarchar(MAX), DecryptByKey(VP.Encrypt_Field3)),
		VP.DOB,
		VP.Exact_DOB,
		VP.Sex,
		VP.Date_of_Issue,
		VP.Permit_To_Remain_Until,
		VP.Foreign_Passport_No,
		VP.EC_Serial_No,
		VP.EC_Reference_No,
		VP.EC_Reference_No_Other_Format

	FROM
		@TempStudentFileEntry SE
		INNER JOIN TempVoucherAccount TA 
			ON SE.Temp_Voucher_Acc_ID = TA.Voucher_Acc_ID AND TA.Record_Status = 'V' -- Validated Acct only
		INNER JOIN TempPersonalInformation TP 
			ON TA.Voucher_Acc_ID = TP.Voucher_Acc_ID
		INNER JOIN PersonalInformation VP
			ON VP.Voucher_Acc_ID = TA.Validated_Acc_ID AND VP.Doc_Code = TP.Doc_Code




	-- Overwrite Student Entry personal info
	UPDATE SE
	SET 
		Acc_Process_Stage = @Acc_Process_Stage,
		Acc_Process_Stage_Dtm = FORMAT( GETDATE() ,'yyyy-MM-dd'),
		Voucher_Acc_ID = R.Voucher_Acc_ID,
		Temp_Voucher_Acc_ID = R.Temp_Voucher_Acc_ID, 
		Acc_Type = R.Acc_Type, 
		Temp_Acc_Record_Status = R.Temp_Acc_Record_Status, 
		Temp_Acc_Validate_Dtm = R.Temp_Acc_Validate_Dtm,
		Validated_Acc_Found = R.Validated_Acc_Found,
		Acc_Doc_Code = R.Acc_Doc_Code,
		Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), R.Name_EN),
		Encrypt_Field2_1 = EncryptByKey(KEY_GUID('sym_Key'), R.Surname_EN),
		Encrypt_Field2_2 = EncryptByKey(KEY_GUID('sym_Key'), R.Given_Name_EN),
		Encrypt_Field3 = EncryptByKey(KEY_GUID('sym_Key'), R.Name_CH), 
		[Encrypt_Field4] = CASE WHEN R.Name_CH = ''
								THEN Encrypt_Field4
								ELSE EncryptByKey(KEY_GUID('sym_Key'), R.Name_CH)
							END, 
		DOB = R.DOB,
		Exact_DOB = R.Exact_DOB,
		Sex = R.Sex,
		Date_of_Issue = R.Date_of_Issue,
		Permit_To_Remain_Until = R.Permit_To_Remain_Until,
		Foreign_Passport_No = R.Foreign_Passport_No,
		EC_Serial_No = R.EC_Serial_No,
		EC_Reference_No = R.EC_Reference_No,
		EC_Reference_No_Other_Format = R.EC_Reference_No_Other_Format,
		Update_Dtm = GETDATE()
	FROM 
		StudentFileEntryStaging SE
	INNER JOIN @Result R
		ON SE.Student_File_ID = R.Student_File_ID 
			AND SE.Student_Seq = R.Student_Seq 
			AND SE.Temp_Voucher_Acc_ID = R.Temp_Voucher_Acc_ID
	
	
	UPDATE SE
	SET 
		Acc_Process_Stage = @Acc_Process_Stage,
		Acc_Process_Stage_Dtm = FORMAT( GETDATE() ,'yyyy-MM-dd'),
		Voucher_Acc_ID = R.Voucher_Acc_ID,
		Temp_Voucher_Acc_ID = R.Temp_Voucher_Acc_ID, 
		Acc_Type = R.Acc_Type, 
		Temp_Acc_Record_Status = R.Temp_Acc_Record_Status, 
		Temp_Acc_Validate_Dtm = R.Temp_Acc_Validate_Dtm,
		Validated_Acc_Found = R.Validated_Acc_Found,
		Acc_Doc_Code = R.Acc_Doc_Code,
		Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), R.Name_EN),
		Encrypt_Field2_1 = EncryptByKey(KEY_GUID('sym_Key'), R.Surname_EN),
		Encrypt_Field2_2 = EncryptByKey(KEY_GUID('sym_Key'), R.Given_Name_EN),
		Encrypt_Field3 = EncryptByKey(KEY_GUID('sym_Key'), R.Name_CH), 
		[Encrypt_Field4] = CASE WHEN R.Name_CH = ''
								THEN Encrypt_Field4
								ELSE EncryptByKey(KEY_GUID('sym_Key'), R.Name_CH)
							END, 
		DOB = R.DOB,
		Exact_DOB = R.Exact_DOB,
		Sex = R.Sex,
		Date_of_Issue = R.Date_of_Issue,
		Permit_To_Remain_Until = R.Permit_To_Remain_Until,
		Foreign_Passport_No = R.Foreign_Passport_No,
		EC_Serial_No = R.EC_Serial_No,
		EC_Reference_No = R.EC_Reference_No,
		EC_Reference_No_Other_Format = R.EC_Reference_No_Other_Format,
		Update_Dtm = GETDATE()
	FROM 
		StudentFileEntry SE
	INNER JOIN @Result R
		ON SE.Student_File_ID = R.Student_File_ID 
			AND SE.Student_Seq = R.Student_Seq 
			AND SE.Temp_Voucher_Acc_ID = R.Temp_Voucher_Acc_ID	
		

EXEC [proc_SymmetricKey_close]

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentAccountMatching_check_TempAcc] TO HCVU
GO
