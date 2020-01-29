IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSSF_Class_Precheck_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSSF_Class_Precheck_Report_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	10 Aug 2019
-- CR No.:			CRE19-001 (VSS 2019/20)
-- Description:		[Class] worksheet
--					- Show Validated account > temp account > StudentFileEntry
--					- Re-order sequence of account matching result columns 
-- =============================================  

CREATE PROCEDURE [dbo].[proc_EHS_eHSSF_Class_Precheck_Report_get]
	@Input_Student_File_ID	varchar(15),
	@File_ID				varchar(9)
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @CutOffDtm				DATETIME

	SET @CutOffDtm =  CONVERT(date, getdate())
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	SET NOCOUNT ON 
	
	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key
	
	--DECLARE @Input_Student_File_ID	varchar(15)
	--SET @Input_Student_File_ID = 'SF2018081700003'
	--
	
	SET @File_ID = UPPER(@File_ID)

	DECLARE @VaccineToInjectDisplay AS VARCHAR(20)

	CREATE TABLE #Last3VaccineRowTT (
		Student_Seq INT,
		Vaccine_Seq INT,
		Vaccine VARCHAR(200)
	)

	CREATE TABLE #Last3VaccineSIVTT (
		Student_Seq INT,
		Vaccine VARCHAR(200)
	)

	CREATE TABLE #Last3VaccinePVTT (
		Student_Seq INT,
		Vaccine VARCHAR(200)
	)

	CREATE TABLE #Last3VaccineMMRTT (
		Student_Seq INT,
		Vaccine VARCHAR(200)
	)

	CREATE TABLE #StudentTT (
		TableID		INT,		
		Student_File_ID		VARCHAR(15),
		Student_Seq		INT,
		Class_Name	NVARCHAR(20),
		Class_No	NVARCHAR(20),
		Chi_Name_Upload NVARCHAR(MAX),
		Eng_Surname_Upload VARCHAR(MAX),
		Eng_GivenName_Upload VARCHAR(MAX),
		Sex_Upload		CHAR(1),
		DOB_Upload	 DATETIME,
		Exact_DOB_Upload	CHAR(1),
		EC_Age_Upload	SMALLINT,
		EC_Date_of_Registration_Upload DATETIME,
		Doc_Code_Upload	CHAR(20),
		Contact_No	VARCHAR(20),

		Voucher_Acc_ID	CHAR(15),
		Temp_Voucher_Acc_ID		CHAR(15),
		Temp_Acc_Record_Status	CHAR(1),
		Temp_Acc_Record_Status_Desc	VARCHAR(100),
		Temp_Acc_Validate_Dtm	DATETIME,
		Acc_Validation_Result	VARCHAR(1000),
		Validated_Acc_Found		CHAR(1),
		Validated_Acc_Unmatch_Result	VARCHAR(1000),
		Chi_Name	NVARCHAR(MAX),
		Eng_Surname		VARCHAR(MAX),
		Eng_GivenName	VARCHAR(MAX),
		Sex		CHAR(1),
		DOB	 DATETIME,
		Exact_DOB	CHAR(1),
		EC_Age	SMALLINT,
		EC_Date_of_Registration DATETIME,
		Doc_Code	CHAR(20),
		DocNumber	VARCHAR(MAX),
		Date_of_Issue	DATETIME,
		Permit_To_Remain_Until	 DATETIME,
		Foreign_Passport_No	 VARCHAR(20),
		EC_Serial_No	VARCHAR(10),
		EC_Reference_No		VARCHAR(40),
		Reject_Injection	VARCHAR(1),

		Vaccination_Process_Stage_Dtm	DATETIME,
		HA_Vaccine_Ref_Status	VARCHAR(10),
		DH_Vaccine_Ref_Status	VARCHAR(10),

		SIV_Entitle_ONLYDOSE	VARCHAR(1000),
		SIV_Entitle_1STDOSE		VARCHAR(1000),
		SIV_Entitle_2NDDOSE		VARCHAR(1000),
		SIV_Last3Vaccine		VARCHAR(200),
		SIV_Entitle_Inject_Fail_Reason VARCHAR(1000),

		PV_Entitle_ONLYDOSE		VARCHAR(1000),
		PV13_Entitle_ONLYDOSE	VARCHAR(1000),
		PV_Last3Vaccine			VARCHAR(200),
		PV_Entitle_Inject_Fail_Reason VARCHAR(1000),
		PV13_Entitle_Inject_Fail_Reason VARCHAR(1000),

		MMR_Entitle_1STDOSE		VARCHAR(1000),
		MMR_Entitle_2NDDOSE		VARCHAR(1000),
		MMR_Last3Vaccine		VARCHAR(200),
		MMR_Entitle_Inject_Fail_Reason VARCHAR(1000),

		Remarks	VARCHAR(1000)			
	)

	IF @File_ID IN ('EHSVF000')
	BEGIN

		SELECT @VaccineToInjectDisplay = S.Display_Code
		FROM StudentFileHeader H
		INNER JOIN Subsidize S
		ON H.Subsidize_Code = S.Subsidize_Code
			AND H.Student_File_ID = @Input_Student_File_ID
			

		-- Last 3 SIV Vaccine
		INSERT INTO #Last3VaccineRowTT (Student_Seq, Vaccine_Seq, Vaccine)
		SELECT Student_Seq, 
			ROW_NUMBER() OVER(PARTITION BY Student_Seq ORDER BY Service_Receive_Dtm DESC),
			FORMAT(Service_Receive_Dtm, 'yyyy/MM/dd') + ' ' + RTRIM(s.Vaccine_Type) + ' (' + Available_Item_Desc + ')'
		FROM StudentFileEntryVaccine v INNER JOIN Subsidize s
			ON v.Subsidize_Code = s.Subsidize_Code
		WHERE Student_File_ID = @Input_Student_File_ID AND v.Subsidize_Item_Code NOT IN ('PV','PV13','MMR')

		INSERT INTO #Last3VaccineSIVTT (Student_Seq, Vaccine)
		SELECT t.Student_Seq,
			STUFF(
			(
				SELECT CHAR(10) + Vaccine FROM #Last3VaccineRowTT a WHERE a.Student_Seq = t.Student_Seq AND Vaccine_Seq <= 3 FOR XML PATH('')
			),1,1,'') 
		FROM (SELECT DISTINCT Student_Seq FROM #Last3VaccineRowTT  ) t


		-- Last 3 PV + PCV13 Vaccine
		DELETE FROM #Last3VaccineRowTT
		INSERT INTO #Last3VaccineRowTT (Student_Seq, Vaccine_Seq, Vaccine)
		SELECT Student_Seq, 
			ROW_NUMBER() OVER(PARTITION BY Student_Seq ORDER BY Service_Receive_Dtm DESC),
			FORMAT(Service_Receive_Dtm, 'yyyy/MM/dd') + ' ' + CASE WHEN RTRIM(s.Vaccine_Type) = 'PV' THEN '23vPPV' 
																   WHEN RTRIM(s.Vaccine_Type) = 'PV13' THEN 'PCV13' 	
																	ELSE RTRIM(s.Vaccine_Type) END + ' (' + Available_Item_Desc + ')'
		FROM StudentFileEntryVaccine v INNER JOIN Subsidize s
			ON v.Subsidize_Code = s.Subsidize_Code
		WHERE Student_File_ID = @Input_Student_File_ID AND v.Subsidize_Item_Code IN ('PV','PV13')

		INSERT INTO #Last3VaccinePVTT (Student_Seq, Vaccine)
		SELECT t.Student_Seq,
			STUFF(
			(
				SELECT CHAR(10) + Vaccine FROM #Last3VaccineRowTT a WHERE a.Student_Seq = t.Student_Seq AND Vaccine_Seq <= 3 FOR XML PATH('')
			),1,1,'') 
		FROM (SELECT DISTINCT Student_Seq FROM #Last3VaccineRowTT  ) t

		-- Last 3 MMR Vaccine
		DELETE FROM #Last3VaccineRowTT
		INSERT INTO #Last3VaccineRowTT (Student_Seq, Vaccine_Seq, Vaccine)
		SELECT Student_Seq, 
			ROW_NUMBER() OVER(PARTITION BY Student_Seq ORDER BY Service_Receive_Dtm DESC),
			FORMAT(Service_Receive_Dtm, 'yyyy/MM/dd') + ' ' + RTRIM(s.Vaccine_Type) + ' (' + Available_Item_Desc + ')'
		FROM StudentFileEntryVaccine v INNER JOIN Subsidize s
			ON v.Subsidize_Code = s.Subsidize_Code
		WHERE Student_File_ID = @Input_Student_File_ID AND v.Subsidize_Item_Code IN ('MMR')

		INSERT INTO #Last3VaccineMMRTT (Student_Seq, Vaccine)
		SELECT t.Student_Seq,
			STUFF(
			(
				SELECT CHAR(10) + Vaccine FROM #Last3VaccineRowTT a WHERE a.Student_Seq = t.Student_Seq AND Vaccine_Seq <= 3 FOR XML PATH('')
			),1,1,'') 
		FROM (SELECT DISTINCT Student_Seq FROM #Last3VaccineRowTT  ) t


		INSERT INTO #StudentTT(
			TableID,		
			Student_File_ID,
			Student_Seq,
			Class_Name,
			Class_No,
			Chi_Name_Upload,
			Eng_Surname_Upload,
			Eng_GivenName_Upload,
			Sex_Upload,
			DOB_Upload,
			Exact_DOB_Upload,
			EC_Age_Upload,
			EC_Date_of_Registration_Upload,
			Doc_Code_Upload,
			Contact_No,
			Voucher_Acc_ID,
			Temp_Voucher_Acc_ID,
			Temp_Acc_Record_Status,
			Temp_Acc_Record_Status_Desc,
			Temp_Acc_Validate_Dtm,
			Acc_Validation_Result,
			Validated_Acc_Found,
			Validated_Acc_Unmatch_Result,
			Chi_Name,
			Eng_Surname,
			Eng_GivenName,
			Sex,
			DOB,
			Exact_DOB,
			EC_Age,
			EC_Date_of_Registration,
			Doc_Code,
			DocNumber,
			Date_of_Issue,
			Permit_To_Remain_Until,
			Foreign_Passport_No,
			EC_Serial_No,
			EC_Reference_No,
			Reject_Injection,
			Vaccination_Process_Stage_Dtm,
			HA_Vaccine_Ref_Status,
			DH_Vaccine_Ref_Status,
		
			SIV_Entitle_ONLYDOSE,
			SIV_Entitle_1STDOSE,
			SIV_Entitle_2NDDOSE,
			SIV_Last3Vaccine,
			SIV_Entitle_Inject_Fail_Reason,

			PV_Entitle_ONLYDOSE,
			PV13_Entitle_ONLYDOSE,
			PV_Last3Vaccine,
			PV_Entitle_Inject_Fail_Reason,
			PV13_Entitle_Inject_Fail_Reason,
			
			MMR_Entitle_1STDOSE,
			MMR_Entitle_2NDDOSE,
			MMR_Last3Vaccine,
			MMR_Entitle_Inject_Fail_Reason,

			Remarks		
		)
		SELECT 
			DENSE_RANK() OVER( ORDER BY Class_Seq.Seq) AS TABLEID,
			-- =================================================================
			-- Section 1: Category & contact information
			-- =================================================================
			E.Student_File_ID,
			E.Student_Seq,
			E.Class_Name,
			Class_No,
			CONVERT(NVARCHAR(MAX), DecryptByKey(E.Encrypt_Field4)) AS Chi_Name_Upload,
			CONVERT(VARCHAR(MAX), DecryptByKey(E.Encrypt_Field2_1)) AS Eng_Surname_Upload,
			CONVERT(VARCHAR(MAX), DecryptByKey(E.Encrypt_Field2_2)) AS Eng_GivenName_Upload,
			E.Sex AS Sex_Upload,
			E.DOB AS DOB_Upload,
			E.Exact_DOB AS Exact_DOB_Upload,
			NULL AS EC_Age_Upload,
			NULL AS EC_Date_of_Registration_Upload,
			REPLACE(E.Doc_Code,'_','/') AS Doc_Code_Upload,
			Contact_No,
			-- =================================================================
			-- Section 2: Account matching result
			-- =================================================================
			E.Voucher_Acc_ID,
			E.Temp_Voucher_Acc_ID,
			E.Temp_Acc_Record_Status,
			S1.Status_Description,
			Temp_Acc_Validate_Dtm,
			Acc_Validation_Result,
			ISNULL(Validated_Acc_Found, 'N'),
			Validated_Acc_Unmatch_Result,
			CASE
				WHEN PInfo.Encrypt_Field3 IS NULL 
				THEN 
					CONVERT(NVARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field3))
					--CASE WHEN LEN(CONVERT(NVARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field3))) > 0
					--THEN CONVERT(NVARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field3))
					--ELSE CONVERT(NVARCHAR(MAX), DecryptByKey(E.Encrypt_Field3))
					--END
				ELSE 
					CONVERT(NVARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field3))
					--CASE WHEN LEN(CONVERT(NVARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field3))) > 0
					--THEN CONVERT(NVARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field3))
					--ELSE CONVERT(NVARCHAR(MAX), DecryptByKey(E.Encrypt_Field3))
					--END
			END AS Chi_Name,
			CASE
				WHEN PInfo.Encrypt_Field3 IS NULL 
				THEN dbo.func_split_engname(CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2)), 'S')
				ELSE dbo.func_split_engname(CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2)), 'S')
			END AS Eng_Surname,
			CASE
				WHEN PInfo.Encrypt_Field3 IS NULL 
				THEN dbo.func_split_engname(CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2)), 'G')
				ELSE dbo.func_split_engname(CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2)), 'G')
			END AS Eng_GivenName,	
			CASE
				WHEN PInfo.Sex IS NULL THEN TPInfo.Sex
				ELSE PInfo.Sex
			END AS Sex,	
			CASE
				WHEN PInfo.DOB IS NULL THEN TPInfo.DOB
				ELSE PInfo.DOB
			END AS DOB,
			CASE
				WHEN PInfo.Exact_DOB IS NULL THEN TPInfo.Exact_DOB
				ELSE PInfo.Exact_DOB
			END AS Exact_DOB,
			CASE
				WHEN PInfo.EC_Age IS NULL THEN TPInfo.EC_Age
				ELSE PInfo.EC_Age
			END AS EC_Age,
			CASE
				WHEN PInfo.EC_Date_of_Registration IS NULL THEN TPInfo.EC_Date_of_Registration
				ELSE PInfo.EC_Date_of_Registration
			END AS EC_Date_of_Registration,
			CASE
				WHEN PInfo.Doc_Code IS NULL THEN TPInfo.Doc_Code
				ELSE PInfo.Doc_Code
			END AS Doc_Code,
			CASE
				WHEN PInfo.Encrypt_Field1 IS NULL 
				THEN 
						CASE WHEN TPInfo.Doc_Code = 'ADOPC'
						THEN
							CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field11))
							+ '/' + CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field1))
						ELSE
					CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field1))
						END
					ELSE
						CASE WHEN PInfo.Doc_Code = 'ADOPC'
						THEN
							CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field11))
							+ '/' + CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field1))
				ELSE
					CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field1))
						END
			END AS DocNumber,
			CASE
				WHEN PInfo.Date_of_Issue IS NULL THEN TPInfo.Date_of_Issue
				ELSE PInfo.Date_of_Issue
			END AS Date_of_Issue,
			CASE
				WHEN PInfo.Permit_To_Remain_Until IS NULL THEN TPInfo.Permit_To_Remain_Until
				ELSE PInfo.Permit_To_Remain_Until
			END AS Permit_To_Remain_Until,
			CASE
				WHEN PInfo.Foreign_Passport_No IS NULL THEN TPInfo.Foreign_Passport_No
				ELSE PInfo.Foreign_Passport_No
			END AS Foreign_Passport_No,
			CASE
				WHEN PInfo.EC_Serial_No IS NULL THEN TPInfo.EC_Serial_No
				ELSE PInfo.EC_Serial_No
			END AS EC_Serial_No,
			CASE
				WHEN PInfo.EC_Reference_No IS NULL THEN TPInfo.EC_Reference_No
				ELSE PInfo.EC_Reference_No
			END AS EC_Reference_No,
			Reject_Injection,
			-- =================================================================
			-- Section 3: Vaccination checking result (generated by system)
			-- =================================================================
			CASE WHEN ISNULL(Vaccination_Process_Stage_Dtm, '') = '' THEN NULL ELSE Vaccination_Process_Stage_Dtm END AS Vaccination_Process_Stage_Dtm,
			Ext_Ref_Status,
			DH_Vaccine_Ref_Status,
			-- SIV
			CASE WHEN SIV.Remark_ONLYDOSE IS NOT NULL THEN SUBSTRING(SIV.Remark_ONLYDOSE,0, CHARINDEX('|||', SIV.Remark_ONLYDOSE))  ELSE SIV.Entitle_ONLYDOSE END,
			CASE WHEN SIV.Remark_1STDOSE IS NOT NULL THEN SUBSTRING(SIV.Remark_1STDOSE,0, CHARINDEX('|||', SIV.Remark_1STDOSE))  ELSE SIV.Entitle_1STDOSE END,
			CASE WHEN SIV.Remark_2NDDOSE IS NOT NULL THEN SUBSTRING(SIV.Remark_2NDDOSE,0, CHARINDEX('|||', SIV.Remark_2NDDOSE))  ELSE SIV.Entitle_2NDDOSE END,
			ISNULL(L3V_SIV.Vaccine, ''),
			CASE WHEN SIV.Entitle_Inject_Fail_Reason <> '' THEN SUBSTRING(SIV.Entitle_Inject_Fail_Reason,0, CHARINDEX('|||', SIV.Entitle_Inject_Fail_Reason)) ELSE '' END,

			-- PV + PV13
			CASE WHEN PV.Remark_ONLYDOSE IS NOT NULL THEN SUBSTRING(PV.Remark_ONLYDOSE,0, CHARINDEX('|||', PV.Remark_ONLYDOSE))  ELSE PV.Entitle_ONLYDOSE END,
			CASE WHEN PV13.Remark_ONLYDOSE IS NOT NULL THEN SUBSTRING(PV13.Remark_ONLYDOSE,0, CHARINDEX('|||', PV13.Remark_ONLYDOSE))  ELSE PV13.Entitle_ONLYDOSE END,
			ISNULL(L3V_PV.Vaccine, ''),
			CASE WHEN PV.Entitle_Inject_Fail_Reason <> '' THEN SUBSTRING(PV.Entitle_Inject_Fail_Reason,0, CHARINDEX('|||', PV.Entitle_Inject_Fail_Reason)) ELSE '' END,
			CASE WHEN PV13.Entitle_Inject_Fail_Reason <> '' THEN SUBSTRING(PV13.Entitle_Inject_Fail_Reason,0, CHARINDEX('|||', PV13.Entitle_Inject_Fail_Reason)) ELSE '' END,

			-- MMR
			CASE WHEN MMR.Remark_1STDOSE IS NOT NULL THEN SUBSTRING(MMR.Remark_1STDOSE,0, CHARINDEX('|||', MMR.Remark_1STDOSE))  ELSE MMR.Entitle_1STDOSE END,
			CASE WHEN MMR.Remark_2NDDOSE IS NOT NULL THEN SUBSTRING(MMR.Remark_2NDDOSE,0, CHARINDEX('|||', MMR.Remark_2NDDOSE))  ELSE MMR.Entitle_2NDDOSE END,
			ISNULL(L3V_MMR.Vaccine, ''),
			CASE WHEN MMR.Entitle_Inject_Fail_Reason <> '' THEN SUBSTRING(MMR.Entitle_Inject_Fail_Reason,0, CHARINDEX('|||', MMR.Entitle_Inject_Fail_Reason)) ELSE '' END,

			SUBSTRING(CONCAT(
					IIF(SUBSTRING(Ext_Ref_Status,2,1) IN ('C','S'),' / HA connection failed',''), 
					IIF(SUBSTRING(DH_Vaccine_Ref_Status,2,1) IN ('C','S'),' / DH connection failed',''),
					IIF(SUBSTRING(Ext_Ref_Status,2,1) IN ('P'),' / HA demographics not matched',''),
					IIF(SUBSTRING(DH_Vaccine_Ref_Status,2,1) IN ('P'),' / DH demographics not matched','')),4,10000) AS Entitle_Inject_Fail_Reason

		FROM StudentFileEntry E
		LEFT JOIN StatusData S1
			ON E.Temp_Acc_Record_Status = S1.Status_Value
			AND S1.Enum_Class = 'TempAccountRecordStatusClass'
		LEFT JOIN PersonalInformation PInfo
			ON E.Voucher_Acc_ID = PInfo.Voucher_Acc_ID 
			AND PInfo.Doc_Code = E.Acc_Doc_Code
		LEFT JOIN TempPersonalInformation TPInfo
			ON E.Temp_Voucher_Acc_ID = TPInfo.Voucher_Acc_ID
			AND TPInfo.Doc_Code = E.Acc_Doc_Code
		INNER JOIN (SELECT MIN(Student_Seq) AS Seq , Class_Name 
						FROM StudentFileEntry 
						WHERE Student_File_ID = @Input_Student_File_ID
						GROUP BY Class_Name) Class_Seq
			ON Class_seq.Class_Name = E.Class_Name
		--INNER JOIN VoucherAccount VA 
		--	ON PInfo.Voucher_Acc_ID = VA.Voucher_Acc_ID

		-- Join SIV vaccine entitle dose
		LEFT JOIN (SELECT a.* FROM StudentFileEntrySubsidizePrecheck a 
					INNER JOIN Subsidize s
					ON a.Subsidize_Code = S.Subsidize_Code 
						AND S.Subsidize_Item_Code = 'SIV'
						AND a.Student_File_ID = @Input_Student_File_ID) SIV
			ON E.Student_File_ID = SIV.Student_File_ID
				AND E.Student_Seq = SIV.Student_Seq
		-- Join PV vaccine entitle dose
		LEFT JOIN (SELECT a.* FROM StudentFileEntrySubsidizePrecheck a 
					INNER JOIN Subsidize s
					ON a.Subsidize_Code = S.Subsidize_Code 
						AND S.Subsidize_Item_Code = 'PV'
						AND a.Student_File_ID = @Input_Student_File_ID) PV
			ON E.Student_File_ID = PV.Student_File_ID
				AND E.Student_Seq = PV.Student_Seq
		-- Join PV13 vaccine entitle dose
		LEFT JOIN (SELECT a.* FROM StudentFileEntrySubsidizePrecheck a 
					INNER JOIN Subsidize s
					ON a.Subsidize_Code = S.Subsidize_Code 
						AND S.Subsidize_Item_Code = 'PV13'
						AND a.Student_File_ID = @Input_Student_File_ID) PV13
			ON E.Student_File_ID = PV13.Student_File_ID
				AND E.Student_Seq = PV13.Student_Seq
		-- Join MMR vaccine entitle dose
		LEFT JOIN (SELECT a.* FROM StudentFileEntrySubsidizePrecheck a 
					INNER JOIN Subsidize s
					ON a.Subsidize_Code = S.Subsidize_Code 
						AND S.Subsidize_Item_Code = 'MMR'
						AND a.Student_File_ID = @Input_Student_File_ID) MMR
			ON E.Student_File_ID = MMR.Student_File_ID
				AND E.Student_Seq = MMR.Student_Seq
		-- Join Last 3 vaccine temp table 
		LEFT JOIN #Last3VaccineSIVTT L3V_SIV
			ON L3V_SIV.Student_Seq = e.Student_Seq
		LEFT JOIN #Last3VaccinePVTT L3V_PV
			ON L3V_PV.Student_Seq = e.Student_Seq
		LEFT JOIN #Last3VaccineMMRTT L3V_MMR
			ON L3V_MMR.Student_Seq = e.Student_Seq
		WHERE E.Student_File_ID = @Input_Student_File_ID

	END


	-- ========================================================================================================================
	-- Resident:  SIV + PV + PV13
	-- ========================================================================================================================
	DECLARE @ResidentCount AS INT
	SELECT @ResidentCount = COUNT(1) FROM #StudentTT T
	WHERE Class_Name = 'RESIDENT'

	IF @ResidentCount = 0 
		SELECT ''
	ELSE
		SELECT 
			-- Section 1 - Class & account information
			Student_Seq,
			Class_Name,
			Class_No,
			CASE WHEN (Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL) OR Chi_Name = '' THEN 
								ISNULL([dbo].[func_mask_ChiName](Chi_Name_Upload), '')
						ELSE
								[dbo].[func_mask_ChiName](Chi_Name)
						END AS Chi_Name_Upload,
			CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Eng_Surname_Upload,'') ELSE Eng_Surname END AS Eng_Surname_Upload,
			CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN 
							CASE WHEN Eng_GivenName_Upload = '' THEN ''
							ELSE ISNULL([dbo].[func_get_givenname_initial](Eng_GivenName_Upload), '') END							
						ELSE
							CASE WHEN Eng_GivenName = '' THEN ''
							ELSE [dbo].[func_get_givenname_initial](Eng_GivenName) END	
						END AS Eng_GivenName_Upload,
			CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN Sex_Upload ELSE Sex END,
			CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN dbo.func_format_DOB(DOB_Upload, Exact_DOB_Upload, 'en-us', EC_Age_Upload, EC_Date_of_Registration_Upload) ELSE dbo.func_format_DOB(DOB, Exact_DOB, 'en-us', EC_Age, EC_Date_of_Registration) END AS DOB_Upload,
			CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN Doc_Code_Upload ELSE Doc_Code END,
			--ISNULL(Contact_No, '') AS Contact_No,
			'',
			-- Section 2 - Account matching result
			ISNULL(dbo.func_format_voucher_account_number('V', Voucher_Acc_ID), '') AS Voucher_Acc_ID, 
			ISNULL(Validated_Acc_Found, 'N'),
			ISNULL(Validated_Acc_Unmatch_Result, '') AS Validated_Acc_Unmatch_Result,
			ISNULL(dbo.func_format_voucher_account_number('T', Temp_Voucher_Acc_ID), '') AS Temp_Voucher_Acc_ID,
			ISNULL(Temp_Acc_Record_Status_Desc, '') AS Temp_Acc_Record_Status_Desc,
			CASE WHEN Temp_Acc_Validate_Dtm IS NULL THEN '' ELSE FORMAT(Temp_Acc_Validate_Dtm, 'dd MMM yyyy') END AS Temp_Acc_Validate_Dtm,
			SUBSTRING(Acc_Validation_Result, 0, CHARINDEX('|||', Acc_Validation_Result)) AS Acc_Validation_Result,
			--ISNULL(Reject_Injection, '') AS Reject_Injection,	
			--Chi_Name,
			--Eng_Surname,
			--Eng_GivenName,
			--Sex,
			--DOB,
			--Doc_Code,
			--LTRIM(DocNumber),
			--CASE WHEN Date_of_Issue IS NULL THEN '' ELSE FORMAT(Date_of_Issue, 'dd MMM yyyy') END AS Date_of_Issue,
			--CASE WHEN Permit_To_Remain_Until IS NULL THEN '' ELSE FORMAT(Permit_To_Remain_Until, 'dd MMM yyyy') END AS Permit_To_Remain_Until,
			--ISNULL(Foreign_Passport_No,'') AS Foreign_Passport_No,
			--ISNULL(EC_Serial_No,'') AS EC_Serial_No,
			--ISNULL(EC_Reference_No,'') AS EC_Reference_No,
			'',
			-- Section 3 - Section 3 - Vaccination checking result (generated by system)
			CASE WHEN Vaccination_Process_Stage_Dtm IS NULL THEN NULL ELSE FORMAT(Vaccination_Process_Stage_Dtm, 'dd MMM yyyy') END AS Vaccination_Process_Stage_Dtm,
				
			CASE WHEN ISNULL(SIV_Entitle_ONLYDOSE, '') = 'N' THEN 'No' ELSE SIV_Entitle_ONLYDOSE END AS SIV_Entitle_ONLYDOSE,
			CASE WHEN ISNULL(SIV_Entitle_1STDOSE, '') = 'N' THEN 'No' ELSE SIV_Entitle_1STDOSE END AS SIV_Entitle_1STDOSE,
			CASE WHEN ISNULL(SIV_Entitle_2NDDOSE, '') = 'N' THEN 'No' ELSE SIV_Entitle_2NDDOSE END AS SIV_Entitle_2NDDOSE,
			SIV_Entitle_Inject_Fail_Reason,
			SIV_Last3Vaccine,
		

			CASE WHEN ISNULL(PV_Entitle_ONLYDOSE, '') = 'N' THEN 'No' ELSE PV_Entitle_ONLYDOSE END AS PV_Entitle_ONLYDOSE,
			PV_Entitle_Inject_Fail_Reason,
			CASE WHEN ISNULL(PV13_Entitle_ONLYDOSE, '') = 'N' THEN 'No' ELSE PV13_Entitle_ONLYDOSE END AS PV13_Entitle_ONLYDOSE,
			PV13_Entitle_Inject_Fail_Reason,
			PV_Last3Vaccine,
		
		

			--CASE WHEN ISNULL(MMR_Entitle_1STDOSE, '') = 'N' THEN 'No' ELSE MMR_Entitle_1STDOSE END AS MMR_Entitle_1STDOSE,
			--CASE WHEN ISNULL(MMR_Entitle_2NDDOSE, '') = 'N' THEN 'No' ELSE MMR_Entitle_2NDDOSE END AS MMR_Entitle_2NDDOSE,
			--MMR_Entitle_Inject_Fail_Reason,
			--MMR_Last3Vaccine,
		

			Remarks
		FROM #StudentTT T
		WHERE Class_Name = 'RESIDENT'
		ORDER BY Class_Name, Student_Seq

	-- ========================================================================================================================
	-- HCW:  SIV + MMR
	-- ========================================================================================================================
	DECLARE @HCWCount AS INT
	SELECT @HCWCount = COUNT(1) FROM #StudentTT T
	WHERE Class_Name = 'HCW'

	
	IF @HCWCount = 0 
		SELECT ''
	ELSE
		SELECT 
			-- Section 1 - Class & account information
			Student_Seq,
			Class_Name,
			Class_No,
			CASE WHEN (Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL) OR Chi_Name = '' THEN 
								ISNULL([dbo].[func_mask_ChiName](Chi_Name_Upload), '')
						ELSE
								[dbo].[func_mask_ChiName](Chi_Name)
						END AS Chi_Name_Upload,
			CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Eng_Surname_Upload,'') ELSE Eng_Surname END AS Eng_Surname_Upload,
			CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN 
							CASE WHEN Eng_GivenName_Upload = '' THEN ''
							ELSE ISNULL([dbo].[func_get_givenname_initial](Eng_GivenName_Upload), '') END							
						ELSE
							CASE WHEN Eng_GivenName = '' THEN ''
							ELSE [dbo].[func_get_givenname_initial](Eng_GivenName) END	
						END AS Eng_GivenName_Upload,
			CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN Sex_Upload ELSE Sex END,
			CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN dbo.func_format_DOB(DOB_Upload, Exact_DOB_Upload, 'en-us', EC_Age_Upload, EC_Date_of_Registration_Upload) ELSE dbo.func_format_DOB(DOB, Exact_DOB, 'en-us', EC_Age, EC_Date_of_Registration) END AS DOB_Upload,
			CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN Doc_Code_Upload ELSE Doc_Code END,
			--ISNULL(Contact_No, '') AS Contact_No,
			'',
			-- Section 2 - Account matching result
			ISNULL(dbo.func_format_voucher_account_number('V', Voucher_Acc_ID), '') AS Voucher_Acc_ID, 
			ISNULL(Validated_Acc_Found, 'N'),
			ISNULL(Validated_Acc_Unmatch_Result, '') AS Validated_Acc_Unmatch_Result,
			ISNULL(dbo.func_format_voucher_account_number('T', Temp_Voucher_Acc_ID), '') AS Temp_Voucher_Acc_ID,
			ISNULL(Temp_Acc_Record_Status_Desc, '') AS Temp_Acc_Record_Status_Desc,
			CASE WHEN Temp_Acc_Validate_Dtm IS NULL THEN '' ELSE FORMAT(Temp_Acc_Validate_Dtm, 'dd MMM yyyy') END AS Temp_Acc_Validate_Dtm,
			SUBSTRING(Acc_Validation_Result, 0, CHARINDEX('|||', Acc_Validation_Result)) AS Acc_Validation_Result,
			--ISNULL(Reject_Injection, '') AS Reject_Injection,	
			--Chi_Name,
			--Eng_Surname,
			--Eng_GivenName,
			--Sex,
			--DOB,
			--Doc_Code,
			--LTRIM(DocNumber),
			--CASE WHEN Date_of_Issue IS NULL THEN '' ELSE FORMAT(Date_of_Issue, 'dd MMM yyyy') END AS Date_of_Issue,
			--CASE WHEN Permit_To_Remain_Until IS NULL THEN '' ELSE FORMAT(Permit_To_Remain_Until, 'dd MMM yyyy') END AS Permit_To_Remain_Until,
			--ISNULL(Foreign_Passport_No,'') AS Foreign_Passport_No,
			--ISNULL(EC_Serial_No,'') AS EC_Serial_No,
			--ISNULL(EC_Reference_No,'') AS EC_Reference_No,
			'',
			-- Section 3 - Section 3 - Vaccination checking result (generated by system)
			CASE WHEN Vaccination_Process_Stage_Dtm IS NULL THEN NULL ELSE FORMAT(Vaccination_Process_Stage_Dtm, 'dd MMM yyyy') END AS Vaccination_Process_Stage_Dtm,
				
			CASE WHEN ISNULL(SIV_Entitle_ONLYDOSE, '') = 'N' THEN 'No' ELSE SIV_Entitle_ONLYDOSE END AS SIV_Entitle_ONLYDOSE,
			CASE WHEN ISNULL(SIV_Entitle_1STDOSE, '') = 'N' THEN 'No' ELSE SIV_Entitle_1STDOSE END AS SIV_Entitle_1STDOSE,
			CASE WHEN ISNULL(SIV_Entitle_2NDDOSE, '') = 'N' THEN 'No' ELSE SIV_Entitle_2NDDOSE END AS SIV_Entitle_2NDDOSE,
			SIV_Entitle_Inject_Fail_Reason,
			SIV_Last3Vaccine,

			--CASE WHEN ISNULL(PV_Entitle_ONLYDOSE, '') = 'N' THEN 'No' ELSE PV_Entitle_ONLYDOSE END AS PV_Entitle_ONLYDOSE,
			--PV_Entitle_Inject_Fail_Reason,
			--CASE WHEN ISNULL(PV13_Entitle_ONLYDOSE, '') = 'N' THEN 'No' ELSE PV13_Entitle_ONLYDOSE END AS PV13_Entitle_ONLYDOSE,
			--PV13_Entitle_Inject_Fail_Reason,
			--PV_Last3Vaccine,

			CASE WHEN ISNULL(MMR_Entitle_1STDOSE, '') = 'N' THEN 'No' ELSE MMR_Entitle_1STDOSE END AS MMR_Entitle_1STDOSE,
			CASE WHEN ISNULL(MMR_Entitle_2NDDOSE, '') = 'N' THEN 'No' ELSE MMR_Entitle_2NDDOSE END AS MMR_Entitle_2NDDOSE,
			MMR_Entitle_Inject_Fail_Reason,
			MMR_Last3Vaccine,

			Remarks
		FROM #StudentTT T
		WHERE Class_Name = 'HCW'
		ORDER BY Class_Name, Student_Seq



	-- ========================================================================================================================
	-- PID:  SIV
	-- ========================================================================================================================
	DECLARE @PIDCount AS INT
	SELECT @PIDCount = COUNT(1) FROM #StudentTT T
	WHERE Class_Name = 'PID'
	
	IF @PIDCount = 0 
		SELECT ''
	ELSE
		SELECT 
			-- Section 1 - Class & account information
			Student_Seq,
			Class_Name,
			Class_No,
			CASE WHEN (Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL) OR Chi_Name = '' THEN 
								ISNULL([dbo].[func_mask_ChiName](Chi_Name_Upload), '')
						ELSE
								[dbo].[func_mask_ChiName](Chi_Name)
						END AS Chi_Name_Upload,
			CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Eng_Surname_Upload,'') ELSE Eng_Surname END AS Eng_Surname_Upload,
			CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN 
							CASE WHEN Eng_GivenName_Upload = '' THEN ''
							ELSE ISNULL([dbo].[func_get_givenname_initial](Eng_GivenName_Upload), '') END							
						ELSE
							CASE WHEN Eng_GivenName = '' THEN ''
							ELSE [dbo].[func_get_givenname_initial](Eng_GivenName) END	
						END AS Eng_GivenName_Upload,
			CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN Sex_Upload ELSE Sex END,
			CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN dbo.func_format_DOB(DOB_Upload, Exact_DOB_Upload, 'en-us', EC_Age_Upload, EC_Date_of_Registration_Upload) ELSE dbo.func_format_DOB(DOB, Exact_DOB, 'en-us', EC_Age, EC_Date_of_Registration) END AS DOB_Upload,
			CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN Doc_Code_Upload ELSE Doc_Code END,
			--ISNULL(Contact_No, '') AS Contact_No,
			'',
			-- Section 2 - Account matching result
			ISNULL(dbo.func_format_voucher_account_number('V', Voucher_Acc_ID), '') AS Voucher_Acc_ID, 
			ISNULL(Validated_Acc_Found, 'N'),
			ISNULL(Validated_Acc_Unmatch_Result, '') AS Validated_Acc_Unmatch_Result,
			ISNULL(dbo.func_format_voucher_account_number('T', Temp_Voucher_Acc_ID), '') AS Temp_Voucher_Acc_ID,
			ISNULL(Temp_Acc_Record_Status_Desc, '') AS Temp_Acc_Record_Status_Desc,
			CASE WHEN Temp_Acc_Validate_Dtm IS NULL THEN '' ELSE FORMAT(Temp_Acc_Validate_Dtm, 'dd MMM yyyy') END AS Temp_Acc_Validate_Dtm,
			SUBSTRING(Acc_Validation_Result, 0, CHARINDEX('|||', Acc_Validation_Result)) AS Acc_Validation_Result,
			--ISNULL(Reject_Injection, '') AS Reject_Injection,	
			--Chi_Name,
			--Eng_Surname,
			--Eng_GivenName,
			--Sex,
			--DOB,
			--Doc_Code,
			--LTRIM(DocNumber),
			--CASE WHEN Date_of_Issue IS NULL THEN '' ELSE FORMAT(Date_of_Issue, 'dd MMM yyyy') END AS Date_of_Issue,
			--CASE WHEN Permit_To_Remain_Until IS NULL THEN '' ELSE FORMAT(Permit_To_Remain_Until, 'dd MMM yyyy') END AS Permit_To_Remain_Until,
			--ISNULL(Foreign_Passport_No,'') AS Foreign_Passport_No,
			--ISNULL(EC_Serial_No,'') AS EC_Serial_No,
			--ISNULL(EC_Reference_No,'') AS EC_Reference_No,
			'',
			-- Section 3 - Section 3 - Vaccination checking result (generated by system)
			CASE WHEN Vaccination_Process_Stage_Dtm IS NULL THEN NULL ELSE FORMAT(Vaccination_Process_Stage_Dtm, 'dd MMM yyyy') END AS Vaccination_Process_Stage_Dtm,
				
			CASE WHEN ISNULL(SIV_Entitle_ONLYDOSE, '') = 'N' THEN 'No' ELSE SIV_Entitle_ONLYDOSE END AS SIV_Entitle_ONLYDOSE,
			CASE WHEN ISNULL(SIV_Entitle_1STDOSE, '') = 'N' THEN 'No' ELSE SIV_Entitle_1STDOSE END AS SIV_Entitle_1STDOSE,
			CASE WHEN ISNULL(SIV_Entitle_2NDDOSE, '') = 'N' THEN 'No' ELSE SIV_Entitle_2NDDOSE END AS SIV_Entitle_2NDDOSE,
			SIV_Entitle_Inject_Fail_Reason,
			SIV_Last3Vaccine,

			--CASE WHEN ISNULL(PV_Entitle_ONLYDOSE, '') = 'N' THEN 'No' ELSE PV_Entitle_ONLYDOSE END AS PV_Entitle_ONLYDOSE,
			--PV_Entitle_Inject_Fail_Reason,
			--CASE WHEN ISNULL(PV13_Entitle_ONLYDOSE, '') = 'N' THEN 'No' ELSE PV13_Entitle_ONLYDOSE END AS PV13_Entitle_ONLYDOSE,
			--PV13_Entitle_Inject_Fail_Reason,
			--PV_Last3Vaccine,

			--CASE WHEN ISNULL(MMR_Entitle_1STDOSE, '') = 'N' THEN 'No' ELSE MMR_Entitle_1STDOSE END AS MMR_Entitle_1STDOSE,
			--CASE WHEN ISNULL(MMR_Entitle_2NDDOSE, '') = 'N' THEN 'No' ELSE MMR_Entitle_2NDDOSE END AS MMR_Entitle_2NDDOSE,
			--MMR_Entitle_Inject_Fail_Reason,
			--MMR_Last3Vaccine,

			Remarks
		FROM #StudentTT T
		WHERE Class_Name = 'PID'
		ORDER BY Class_Name, Student_Seq



	--
	
	CLOSE SYMMETRIC KEY sym_Key
	
	DROP TABLE #StudentTT

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSSF_Class_Precheck_Report_get] TO HCVU
GO
