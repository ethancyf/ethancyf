IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSSF_Class_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSSF_Class_Report_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	17 Sep 2020
-- CR No.			CRE20-003-02 (Batch Upload - Phase 2 Vacc Check Report)
-- Description:		Add Worksheet "Follow Up Client" for VF000/001 report
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	24 Aug 2020
-- CR No.			CRE20-003 (Batch Upload)
-- Description:		Add columns (Second Vaccination Date)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	27 Jul 2020
-- CR No.			CRE19-031 (VSS MMR Upload)
-- Description:		Add columns (HKICSymbol, Service_Receive_Dtm)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	24 Dec 2019
-- CR No.:			CRE19-025 (Display of unmatched PV for batch upload under RVP)
-- Description:		[Class] worksheet
--					- Display undefined PV with for reference only remarks
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	9 Sep 2019
-- CR No.:			CRE19-001-04 (VSS 2019/20 RVP Pre-check)
-- Description:		Handle EHSVF003
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	10 Aug 2019
-- CR No.:			CRE19-001 (VSS 2019/20)
-- Description:		[Class] worksheet
--					- Show Validated account > temp account > StudentFileEntry
--					- Re-order sequence of account matching result columns 
--					- Mask patient name
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	30 Oct 2018
-- CR No.:			CRE18-011 (Check vaccination record of students with rectified information in rectification file)
-- Description:		Include HA/DH demographics not matched information to "Reject Reason" column
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	19 October 2018
-- CR No.:			CRE18-010 (Adding one short form of student vaccination file upon the first upload)
-- Description:		Add new report eHS(S)SF001B
--					Include HA/DH connection fail information to "Reject Reason" column
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	17 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Get StudentFileEntryStaging
-- =============================================    

CREATE PROCEDURE [dbo].[proc_EHS_eHSSF_Class_Report_get]
	@Input_Student_File_ID		VARCHAR(15),
	@File_ID					VARCHAR(9)
AS BEGIN

	SET NOCOUNT ON;
	
-- =============================================
-- Declaration
-- =============================================
	DECLARE @CutOffDtm				DATETIME

	SET @CutOffDtm =  CONVERT(date, getdate())

	DECLARE @VaccineToInjectDisplay AS VARCHAR(20)
	DECLARE @RowCount AS INT 
	DECLARE @MaxRowCount AS INT 
	DECLARE @VaccineType AS VARCHAR(10)
	DECLARE @SchemeCode AS VARCHAR(10)
	DECLARE @SubsidizeCode AS VARCHAR(10)

	CREATE TABLE #Last3VaccineRowTT (
		Student_Seq INT,
		Vaccine_Seq INT,
		Vaccine VARCHAR(4000)
	)

	CREATE TABLE #Last3VaccineTT (
		Student_Seq INT,
		Vaccine VARCHAR(4000)
	)

	CREATE TABLE #StudentTT (
		TableID		INT,		
		Student_File_ID		VARCHAR(15),
		Student_Seq		INT,
		Class_Name	NVARCHAR(40),
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
		With_Validated_Acc	CHAR(1),
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
		HKIC_Symbol		VARCHAR(1),
		Service_Receive_Dtm	DATETIME,

		Vaccination_Process_Stage_Dtm	DATETIME,
		HA_Vaccine_Ref_Status	VARCHAR(10),
		DH_Vaccine_Ref_Status	VARCHAR(10),
		Entitle_ONLYDOSE	CHAR(1),
		Entitle_1STDOSE		CHAR(1),
		Entitle_2NDDOSE		CHAR(1),
		Entitle_3RDDOSE		CHAR(1),
		Entitle_Inject	CHAR(1),
		Last3Vaccine	VARCHAR(4000),
		Entitle_Inject_Fail_Reason	VARCHAR(1000),

		Injected	CHAR(1),

		Transaction_ID	CHAR(20),
		Transaction_Status	CHAR(1),
		Transaction_Status_Desc	VARCHAR(100),
		Transaction_result	VARCHAR(1000),

		Require_Follow_Up	CHAR(1)
	)

	CREATE TABLE #Control (
		Seq				INT IDENTITY(1,1),
		DynamicSheet	INT,
		Action			CHAR(1),
		Action_Content	VARCHAR(1000)
	)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

	SET @File_ID = UPPER(@File_ID)

	-- Vaccine
	SELECT 
		@VaccineType = Vaccine_Type,
		@SchemeCode = Scheme_Code,
		@SubsidizeCode = h.Subsidize_Code
	FROM 
		StudentFileHeader h
			INNER JOIN Subsidize s 
			ON h.Subsidize_Code = s.Subsidize_Code
				AND h.Student_File_ID =  @Input_Student_File_ID
		
	INSERT INTO #Last3VaccineRowTT (Student_Seq, Vaccine_Seq, Vaccine)
	SELECT Student_Seq, 
		ROW_NUMBER() OVER(PARTITION BY Student_Seq ORDER BY Service_Receive_Dtm DESC),
		FORMAT(Service_Receive_Dtm, 'yyyy/MM/dd') + ' ' 
			+ CASE	WHEN RTRIM(s.Vaccine_Type) = 'PV' THEN IIF(v.Is_Unknown_Vaccine = 1 OR v.Record_Type = 'H', RTRIM(v.Subsidize_Desc),'23vPPV') -- Display ori vaccine name for undefined PV
					WHEN RTRIM(s.Vaccine_Type) = 'PV13' THEN 'PCV13' 	
					ELSE RTRIM(s.Vaccine_Type) END
			+ ' (' + Available_Item_Desc + ')'		
			+ CASE WHEN v.Record_Type = 'H' THEN + ' (' + SD.Status_Description + ')' ELSE '' END 
	FROM StudentFileEntryVaccine v 
		INNER JOIN Subsidize s
			ON v.Subsidize_Code = s.Subsidize_Code
		INNER JOIN StatusData SD
			ON v.Record_Type = SD.Status_Value AND SD.Enum_Class = 'VaccinationRecordRecordType'
	WHERE Student_File_ID = @Input_Student_File_ID AND (
		(@VaccineType = 'QIV' AND v.Subsidize_Item_Code NOT IN ('PV','PV13','MMR'))
		OR (@VaccineType IN ('PV','PV13') AND v.Subsidize_Item_Code IN ('PV','PV13'))
		OR (@VaccineType IN ('MMR') AND v.Subsidize_Item_Code IN ('MMR'))
	)

	INSERT INTO #Last3VaccineTT (Student_Seq, Vaccine)
	SELECT t.Student_Seq,
		STUFF(
		(
			SELECT CHAR(10) + Vaccine FROM #Last3VaccineRowTT a WHERE a.Student_Seq = t.Student_Seq AND Vaccine_Seq <= 3 FOR XML PATH('')
		),1,1,'') 
	FROM (SELECT DISTINCT Student_Seq FROM #Last3VaccineRowTT  ) t
	

-- =============================================
-- Return results
-- =============================================
	
	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key
	
	-- 

	IF @File_ID IN ('EHSVF001','EHSVF002','EHSVF005','EHSVF006')
	BEGIN

		SELECT @VaccineToInjectDisplay = S.Display_Code
		FROM StudentFileHeader H
		INNER JOIN Subsidize S
		ON H.Subsidize_Code = S.Subsidize_Code
			AND H.Student_File_ID = @Input_Student_File_ID
		
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
			With_Validated_Acc,
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
			HKIC_Symbol,
			Service_Receive_Dtm,
			Vaccination_Process_Stage_Dtm,
			HA_Vaccine_Ref_Status,
			DH_Vaccine_Ref_Status,
			Entitle_ONLYDOSE,
			Entitle_1STDOSE,
			Entitle_2NDDOSE,
			Entitle_3RDDOSE,
			Entitle_Inject,
			Last3Vaccine,
			Entitle_Inject_Fail_Reason,
			Injected,
			Require_Follow_Up
		)
		SELECT 
			DENSE_RANK() OVER( ORDER BY Class_Seq.Seq) AS TABLEID,
			Student_File_ID,
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
			E.Voucher_Acc_ID,
			E.Temp_Voucher_Acc_ID,
			E.Temp_Acc_Record_Status,
			S1.Status_Description,
			Temp_Acc_Validate_Dtm,
			Acc_Validation_Result,
			ISNULL(Validated_Acc_Found, 'N'),
			Validated_Acc_Unmatch_Result,
			[With_Validated_Acc] =
				Case 
					WHEN E.Voucher_Acc_ID IS NOT NULL THEN 'Y'
					WHEN E.Temp_Voucher_Acc_ID IS NOT NULL THEN 
						CASE 
							WHEN TVA.Voucher_Acc_ID IS NOT NULL THEN
								CASE 
									WHEN TVA.Validated_Acc_ID <> '' THEN 'Y'
									ELSE ''
								END
							ELSE ''
						END
					ELSE ''
				END,
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
				THEN 
					CASE 
						WHEN CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2)) = '' THEN ''
						ELSE dbo.func_split_engname(CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2)), 'S')
					END
				ELSE 
					CASE 
						WHEN CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2)) = '' THEN ''
						ELSE dbo.func_split_engname(CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2)), 'S')
					END				
			END AS Eng_Surname,
			CASE
				WHEN PInfo.Encrypt_Field3 IS NULL 
				THEN 
					CASE 
						WHEN CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2)) = '' THEN ''
						ELSE dbo.func_split_engname(CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2)), 'G')
					END				
				ELSE 
					CASE 
						WHEN CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2)) = '' THEN ''
						ELSE dbo.func_split_engname(CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2)), 'G')
					END						
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
			E.HKIC_Symbol,
			E.Service_Receive_Dtm,
			CASE WHEN ISNULL(Entitle_Inject, '') = '' THEN NULL ELSE Vaccination_Process_Stage_Dtm END AS Vaccination_Process_Stage_Dtm,
			Ext_Ref_Status,
			DH_Vaccine_Ref_Status,
			Entitle_ONLYDOSE,
			Entitle_1STDOSE,
			Entitle_2NDDOSE,
			Entitle_3RDDOSE,
			Entitle_Inject,
			ISNULL(L3V.Vaccine, ''),
			CASE WHEN ISNULL(Entitle_Inject_Fail_Reason, '') = '' THEN NULL ELSE Entitle_Inject_Fail_Reason END AS Entitle_Inject_Fail_Reason,
			Injected,
			CASE WHEN (E.Acc_Type IS NULL)
					OR (E.Acc_Type = 'T'	AND E.Temp_Acc_Record_Status IN ('R','I'))
					OR (ISNULL(Validated_Acc_Unmatch_Result,'') <> '') THEN 'Y'
				ELSE 'N'
				END AS [Require_Follow_Up]
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
		LEFT JOIN TempVoucherAccount TVA
			ON E.Temp_Voucher_Acc_ID = TVA.Voucher_Acc_ID
		INNER JOIN (SELECT MIN(Student_Seq) AS Seq , Class_Name 
						FROM StudentFileEntry 
						WHERE Student_File_ID = @Input_Student_File_ID
						GROUP BY Class_Name) Class_Seq
			ON Class_seq.Class_Name = E.Class_Name
		--INNER JOIN VoucherAccount VA 
		--	ON PInfo.Voucher_Acc_ID = VA.Voucher_Acc_ID
		LEFT JOIN #Last3VaccineTT L3V
			ON L3V.Student_Seq = e.Student_Seq
		WHERE Student_File_ID = @Input_Student_File_ID

	END
	ELSE IF @File_ID = 'EHSVF003'
	BEGIN
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
			HKIC_Symbol,
			Service_Receive_Dtm,
			Vaccination_Process_Stage_Dtm,
			HA_Vaccine_Ref_Status,
			DH_Vaccine_Ref_Status,
			Entitle_ONLYDOSE,
			Entitle_1STDOSE,
			Entitle_2NDDOSE,
			Entitle_3RDDOSE,
			Entitle_Inject,
			Last3Vaccine,
			Entitle_Inject_Fail_Reason,
			Injected,
			Transaction_ID,
			Transaction_Status,
			Transaction_Status_Desc,
			Transaction_result			
		)
		SELECT 
			DENSE_RANK() OVER( ORDER BY Class_Seq.Seq) AS TABLEID,
			Student_File_ID,
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
				THEN 
					CASE 
						WHEN CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2)) = '' THEN ''
						ELSE dbo.func_split_engname(CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2)), 'S')
					END
				ELSE 
					CASE 
						WHEN CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2)) = '' THEN ''
						ELSE dbo.func_split_engname(CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2)), 'S')
					END		
			END AS Eng_Surname,
			CASE
				WHEN PInfo.Encrypt_Field3 IS NULL 
				THEN 
					CASE 
						WHEN CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2)) = '' THEN ''
						ELSE dbo.func_split_engname(CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2)), 'G')
					END				
				ELSE 
					CASE 
						WHEN CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2)) = '' THEN ''
						ELSE dbo.func_split_engname(CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2)), 'G')
					END		
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
			E.HKIC_Symbol,
			VT.Service_Receive_Dtm,
			CASE WHEN ISNULL(Entitle_Inject, '') = '' THEN NULL ELSE Vaccination_Process_Stage_Dtm END AS Vaccination_Process_Stage_Dtm,
			E.Ext_Ref_Status,
			E.DH_Vaccine_Ref_Status,
			Entitle_ONLYDOSE,
			Entitle_1STDOSE,
			Entitle_2NDDOSE,
			Entitle_3RDDOSE,
			Entitle_Inject,
			L3V.Vaccine,
			CASE WHEN ISNULL(Entitle_Inject_Fail_Reason, '') = '' THEN NULL ELSE Entitle_Inject_Fail_Reason END AS Entitle_Inject_Fail_Reason,
			Injected,
			[dbo].func_format_system_number(E.Transaction_ID),
			VT.Record_Status,
			S2.Status_Description,
			Transaction_result				
	FROM StudentFileEntry E
		LEFT JOIN StatusData S1
			ON E.Temp_Acc_Record_Status = S1.Record_Status
			AND S1.Enum_Class = 'TempAccountRecordStatusClass'
		LEFT JOIN VoucherTransaction VT
			ON VT.Transaction_ID = E.Transaction_ID
			AND VT.Doc_Code = E.Acc_Doc_Code
		LEFT JOIN StatusData S2
			ON VT.Record_Status = S2.Status_Value
			AND S2.Enum_Class = 'ClaimTransStatus'
		LEFT JOIN PersonalInformation PInfo
			ON E.Voucher_Acc_ID = PInfo.Voucher_Acc_ID 
			--AND VT.Doc_Code = PInfo.Doc_Code
			AND PInfo.Doc_Code = E.Acc_Doc_Code
		LEFT JOIN TempPersonalInformation TPInfo
			ON E.Temp_Voucher_Acc_ID = TPInfo.Voucher_Acc_ID 
			--AND VT.Doc_Code = TPInfo.Doc_Code
			AND TPInfo.Doc_Code = E.Acc_Doc_Code
		INNER JOIN (SELECT MIN(Student_Seq) AS Seq , Class_Name 
						FROM StudentFileEntry 
						WHERE Student_File_ID = @Input_Student_File_ID
						GROUP BY Class_Name) Class_Seq
			ON Class_seq.Class_Name = E.Class_Name
		--INNER JOIN VoucherAccount VA WITH (NOLOCK)
		--	ON PInfo.Voucher_Acc_ID = VA.Voucher_Acc_ID
		LEFT JOIN #Last3VaccineTT L3V
			ON L3V.Student_Seq = E.Student_Seq
	WHERE Student_File_ID = @Input_Student_File_ID
		AND NOT EXISTS (
			SELECT
				1
			FROM
				StatStatusFilterMapping
			WHERE
				(Report_id = 'ALL' OR Report_id = @File_ID) 
					AND Table_Name = 'VoucherTransaction'
					AND Status_Name = 'Record_Status' 
					AND ((Effective_Date IS NULL OR Effective_Date <= @cutOffDtm) AND (Expiry_Date IS NULL OR @cutOffDtm < Expiry_Date))
				AND Status_Value = VT.Record_Status 
		)
		AND (VT.Invalidation IS NULL OR NOT EXISTS (
			SELECT
				1
			FROM
				StatStatusFilterMapping
			WHERE
				(report_id = 'ALL' OR report_id = @File_ID) 
					AND Table_Name = 'VoucherTransaction'
					AND Status_Name = 'Invalidation'
					AND ((Effective_Date IS NULL OR Effective_Date <= @cutOffDtm) AND (Expiry_Date IS NULL OR @cutOffDtm < Expiry_Date))
					AND Status_Value = VT.Invalidation 
			)
		)
	END

	--ORDER BY Class_Name, Student_Seq

	-- ===========================================
	-- Set Report Worksheet Dynamic Control
	-- For VF0001:  WS1: Content  WS2: Follow Up Client		WS3: Class (Dynamic)	WS4: Remark
	-- For Others:  WS1: Content  WS2: Class (Dynamic)		WS3: Remark
	-- ===========================================
	DECLARE @ShowFollowUpClientWS VARCHAR(1) = 'N'

	IF @File_ID = 'EHSVF001' 
		SET @ShowFollowUpClientWS = 'Y'
	ELSE
		SET @ShowFollowUpClientWS = 'N'

	INSERT INTO #Control (DynamicSheet, Action, Action_Content)
	SELECT IIF(@ShowFollowUpClientWS = 'Y', 3, 2) AS [Sheet], 
			'A', ISNULL(MAX(TableID), 0)		-- 'A': Add / 'D': Delete / 'R': Rename
	FROM #StudentTT

	-- ===============================================
	-- Return Control Setting
	-- ===============================================
	SELECT DynamicSheet AS [Sheet], Action, Action_Content FROM #Control
	
	-- ===============================================
	-- Follow Up Client
	-- ===============================================
	IF @ShowFollowUpClientWS = 'Y'
	BEGIN
		-- Add "Follow Up Client" Worksheet before Class Worksheet
		DECLARE @RequireFollowUpCount AS INT
		SELECT @RequireFollowUpCount = COUNT(1) FROM #StudentTT T
		WHERE Require_Follow_Up = 'Y'
		
		IF @RequireFollowUpCount = 0 
			SELECT ''
		ELSE
		BEGIN
			IF @File_ID = 'EHSVF001'
			BEGIN
				IF @SchemeCode = 'VSS' AND @SubsidizeCode = 'VNIAMMR'
				BEGIN
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
						[HKIC_Symbol] = ISNULL(SD.Status_Description, ''),
						'',
						-- Section 2 - Account matching result
						ISNULL(dbo.func_format_voucher_account_number('V', Voucher_Acc_ID), '') AS Voucher_Acc_ID, 
						ISNULL(Validated_Acc_Found, 'N'),
						ISNULL(Validated_Acc_Unmatch_Result, '') AS Validated_Acc_Unmatch_Result,
						ISNULL(dbo.func_format_voucher_account_number('T', Temp_Voucher_Acc_ID), '') AS Temp_Voucher_Acc_ID,
						ISNULL(Temp_Acc_Record_Status_Desc, '') AS Temp_Acc_Record_Status_Desc,
						CASE WHEN Temp_Acc_Validate_Dtm IS NULL THEN '' ELSE FORMAT(Temp_Acc_Validate_Dtm, 'dd MMM yyyy') END AS Temp_Acc_Validate_Dtm,
						SUBSTRING(Acc_Validation_Result, 0, CHARINDEX('|||', Acc_Validation_Result)) AS Acc_Validation_Result,
						CASE WHEN Require_Follow_Up = 'Y' THEN 'Y' ELSE 'N' END AS [Require_Follow_Up],		-- Added by Winnie [CRE20-003-02]
						CASE WHEN ISNULL(Reject_Injection, 'N') = 'N' THEN 'Y' ELSE 'N' END AS Confirm_Injection,	-- Reverse the meaning to display
						'',
						-- Section 3 - Section 3 - Vaccination checking result (generated by system)
						CASE WHEN Vaccination_Process_Stage_Dtm IS NULL THEN NULL ELSE FORMAT(Vaccination_Process_Stage_Dtm, 'dd MMM yyyy') END AS Vaccination_Process_Stage_Dtm,
						[Service_Receive_Dtm] = FORMAT(Service_Receive_Dtm, 'yyyy/MM/dd'),
						CASE WHEN ISNULL(Entitle_ONLYDOSE, '') = 'N' THEN 'No' ELSE Entitle_ONLYDOSE END AS Entitle_ONLYDOSE,
						CASE WHEN ISNULL(Entitle_1STDOSE, '') = 'N' THEN 'No' ELSE Entitle_1STDOSE END AS Entitle_1STDOSE,
						CASE WHEN ISNULL(Entitle_2NDDOSE, '') = 'N' THEN 'No' ELSE Entitle_2NDDOSE END AS Entitle_2NDDOSE,
						CASE WHEN ISNULL(Entitle_3RDDOSE, '') = 'N' THEN 'No' ELSE Entitle_3RDDOSE END AS Entitle_3RDDOSE,
						CASE WHEN ISNULL(Entitle_Inject, '') = 'N' THEN 'No' ELSE Entitle_Inject END AS Entitle_Inject,
						ISNULL(Last3Vaccine,''),
						[Entitle_Inject_Fail_Reason] = SUBSTRING(CONCAT(							
									CASE 
										WHEN ISNULL(Entitle_Inject_Fail_Reason,'') <> '' THEN 
											CASE 
												WHEN CHARINDEX('|||', Entitle_Inject_Fail_Reason) = 0 THEN ' / ' + Entitle_Inject_Fail_Reason
												WHEN CHARINDEX('|||', Entitle_Inject_Fail_Reason) > 0 THEN ' / ' + SUBSTRING(Entitle_Inject_Fail_Reason, 0, CHARINDEX('|||', Entitle_Inject_Fail_Reason)) 
											END
										ELSE '' 
									END,
									IIF(SUBSTRING(HA_Vaccine_Ref_Status,2,1) IN ('C','S'),' / HA connection failed',''), 
									IIF(SUBSTRING(DH_Vaccine_Ref_Status,2,1) IN ('C','S'),' / DH connection failed',''),
									IIF(SUBSTRING(HA_Vaccine_Ref_Status,2,1) IN ('P'),' / HA demographics not matched',''),
									IIF(SUBSTRING(DH_Vaccine_Ref_Status,2,1) IN ('P'),' / DH demographics not matched','')),4,10000)
					FROM #StudentTT T
						LEFT OUTER JOIN
							StatusData SD
								ON Enum_Class = 'HKICSymbol' AND T.HKIC_Symbol = SD.Status_Value
					WHERE Require_Follow_Up = 'Y'
					ORDER BY Class_Name, Student_Seq
				END

				ELSE

				BEGIN
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
						'',
						-- Section 2 - Account matching result
						ISNULL(dbo.func_format_voucher_account_number('V', Voucher_Acc_ID), '') AS Voucher_Acc_ID, 
						ISNULL(Validated_Acc_Found, 'N'),
						ISNULL(Validated_Acc_Unmatch_Result, '') AS Validated_Acc_Unmatch_Result,
						ISNULL(dbo.func_format_voucher_account_number('T', Temp_Voucher_Acc_ID), '') AS Temp_Voucher_Acc_ID,
						ISNULL(Temp_Acc_Record_Status_Desc, '') AS Temp_Acc_Record_Status_Desc,
						CASE WHEN Temp_Acc_Validate_Dtm IS NULL THEN '' ELSE FORMAT(Temp_Acc_Validate_Dtm, 'dd MMM yyyy') END AS Temp_Acc_Validate_Dtm,
						SUBSTRING(Acc_Validation_Result, 0, CHARINDEX('|||', Acc_Validation_Result)) AS Acc_Validation_Result,
						CASE WHEN Require_Follow_Up = 'Y' THEN 'Y' ELSE 'N' END AS [Require_Follow_Up],		-- Added by Winnie [CRE20-003-02]
						CASE WHEN ISNULL(Reject_Injection, 'N') = 'N' THEN 'Y' ELSE 'N' END AS Confirm_Injection,	-- Reverse the meaning to display
						'',
						-- Section 3 - Section 3 - Vaccination checking result (generated by system)
						CASE WHEN Vaccination_Process_Stage_Dtm IS NULL THEN NULL ELSE FORMAT(Vaccination_Process_Stage_Dtm, 'dd MMM yyyy') END AS Vaccination_Process_Stage_Dtm,
						CASE WHEN ISNULL(Entitle_ONLYDOSE, '') = 'N' THEN 'No' ELSE Entitle_ONLYDOSE END AS Entitle_ONLYDOSE,
						CASE WHEN ISNULL(Entitle_1STDOSE, '') = 'N' THEN 'No' ELSE Entitle_1STDOSE END AS Entitle_1STDOSE,
						CASE WHEN ISNULL(Entitle_2NDDOSE, '') = 'N' THEN 'No' ELSE Entitle_2NDDOSE END AS Entitle_2NDDOSE,
						CASE WHEN ISNULL(Entitle_Inject, '') = 'N' THEN 'No' ELSE Entitle_Inject END AS Entitle_Inject,
						ISNULL(Last3Vaccine,''),
						[Entitle_Inject_Fail_Reason] = SUBSTRING(CONCAT(							
									CASE 
										WHEN ISNULL(Entitle_Inject_Fail_Reason,'') <> '' THEN 
											CASE 
												WHEN CHARINDEX('|||', Entitle_Inject_Fail_Reason) = 0 THEN ' / ' + Entitle_Inject_Fail_Reason
												WHEN CHARINDEX('|||', Entitle_Inject_Fail_Reason) > 0 THEN ' / ' + SUBSTRING(Entitle_Inject_Fail_Reason, 0, CHARINDEX('|||', Entitle_Inject_Fail_Reason)) 
											END
										ELSE '' 
									END,
									IIF(SUBSTRING(HA_Vaccine_Ref_Status,2,1) IN ('C','S'),' / HA connection failed',''), 
									IIF(SUBSTRING(DH_Vaccine_Ref_Status,2,1) IN ('C','S'),' / DH connection failed',''),
									IIF(SUBSTRING(HA_Vaccine_Ref_Status,2,1) IN ('P'),' / HA demographics not matched',''),
									IIF(SUBSTRING(DH_Vaccine_Ref_Status,2,1) IN ('P'),' / DH demographics not matched','')),4,10000)
					FROM #StudentTT T
					WHERE Require_Follow_Up = 'Y'
					ORDER BY Class_Name, Student_Seq

				END
			END
		END
	END

	-- ===============================================
	-- Return Class
	-- ===============================================
	SET @RowCount = 1
	SELECT @MaxRowCount = ISNULL(MAX(TableID), 0) FROM #StudentTT
	
	WHILE @RowCount <=  @MaxRowCount
	BEGIN 
		IF @File_ID = 'EHSVF001'
		BEGIN
			IF @SchemeCode = 'VSS' AND @SubsidizeCode = 'VNIAMMR'
			BEGIN
				SELECT 
					-- Section 1 - Class & account information
					Student_Seq,
					Class_Name,
					Class_No,
					--CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Chi_Name_Upload,'') ELSE Chi_Name END AS Chi_Name_Upload,
					CASE WHEN (Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL) OR Chi_Name = '' THEN 
								ISNULL([dbo].[func_mask_ChiName](Chi_Name_Upload), '')
						ELSE
								[dbo].[func_mask_ChiName](Chi_Name)
						END AS Chi_Name_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Eng_Surname_Upload,'') ELSE Eng_Surname END AS Eng_Surname_Upload,
					--CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Eng_GivenName_Upload,'') ELSE Eng_GivenName END AS Eng_GivenName_Upload,
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
					[HKIC_Symbol] = ISNULL(SD.Status_Description, ''),
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
					CASE WHEN Require_Follow_Up = 'Y' THEN 'Y' ELSE 'N' END AS [Require_Follow_Up],		-- Added by Winnie [CRE20-003-02]
					CASE WHEN ISNULL(Reject_Injection, 'N') = 'N' THEN 'Y' ELSE 'N' END AS Confirm_Injection,	-- Reverse the meaning to display
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
					[Service_Receive_Dtm] = FORMAT(Service_Receive_Dtm, 'yyyy/MM/dd'),
					CASE WHEN ISNULL(Entitle_ONLYDOSE, '') = 'N' THEN 'No' ELSE Entitle_ONLYDOSE END AS Entitle_ONLYDOSE,
					CASE WHEN ISNULL(Entitle_1STDOSE, '') = 'N' THEN 'No' ELSE Entitle_1STDOSE END AS Entitle_1STDOSE,
					CASE WHEN ISNULL(Entitle_2NDDOSE, '') = 'N' THEN 'No' ELSE Entitle_2NDDOSE END AS Entitle_2NDDOSE,
					CASE WHEN ISNULL(Entitle_3RDDOSE, '') = 'N' THEN 'No' ELSE Entitle_3RDDOSE END AS Entitle_3RDDOSE,
					CASE WHEN ISNULL(Entitle_Inject, '') = 'N' THEN 'No' ELSE Entitle_Inject END AS Entitle_Inject,
					ISNULL(Last3Vaccine,''),
					[Entitle_Inject_Fail_Reason] = SUBSTRING(CONCAT(							
								CASE 
									WHEN ISNULL(Entitle_Inject_Fail_Reason,'') <> '' THEN 
										CASE 
											WHEN CHARINDEX('|||', Entitle_Inject_Fail_Reason) = 0 THEN ' / ' + Entitle_Inject_Fail_Reason
											WHEN CHARINDEX('|||', Entitle_Inject_Fail_Reason) > 0 THEN ' / ' + SUBSTRING(Entitle_Inject_Fail_Reason, 0, CHARINDEX('|||', Entitle_Inject_Fail_Reason)) 
										END
									ELSE '' 
								END,
								IIF(SUBSTRING(HA_Vaccine_Ref_Status,2,1) IN ('C','S'),' / HA connection failed',''), 
								IIF(SUBSTRING(DH_Vaccine_Ref_Status,2,1) IN ('C','S'),' / DH connection failed',''),
								IIF(SUBSTRING(HA_Vaccine_Ref_Status,2,1) IN ('P'),' / HA demographics not matched',''),
								IIF(SUBSTRING(DH_Vaccine_Ref_Status,2,1) IN ('P'),' / DH demographics not matched','')),4,10000)
				FROM #StudentTT T
					LEFT OUTER JOIN
						StatusData SD
							ON Enum_Class = 'HKICSymbol' AND T.HKIC_Symbol = SD.Status_Value
				WHERE TableID = @RowCount
				ORDER BY Class_Name, Student_Seq
			END

			ELSE

			BEGIN
				SELECT 
					-- Section 1 - Class & account information
					Student_Seq,
					Class_Name,
					Class_No,
					--CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Chi_Name_Upload,'') ELSE Chi_Name END AS Chi_Name_Upload,
					CASE WHEN (Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL) OR Chi_Name = '' THEN 
								ISNULL([dbo].[func_mask_ChiName](Chi_Name_Upload), '')
						ELSE
								[dbo].[func_mask_ChiName](Chi_Name)
						END AS Chi_Name_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Eng_Surname_Upload,'') ELSE Eng_Surname END AS Eng_Surname_Upload,
					--CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Eng_GivenName_Upload,'') ELSE Eng_GivenName END AS Eng_GivenName_Upload,
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
					CASE WHEN Require_Follow_Up = 'Y' THEN 'Y' ELSE 'N' END AS [Require_Follow_Up],		-- Added by Winnie [CRE20-003-02]
					CASE WHEN ISNULL(Reject_Injection, 'N') = 'N' THEN 'Y' ELSE 'N' END AS Confirm_Injection,	-- Reverse the meaning to display
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
					CASE WHEN ISNULL(Entitle_ONLYDOSE, '') = 'N' THEN 'No' ELSE Entitle_ONLYDOSE END AS Entitle_ONLYDOSE,
					CASE WHEN ISNULL(Entitle_1STDOSE, '') = 'N' THEN 'No' ELSE Entitle_1STDOSE END AS Entitle_1STDOSE,
					CASE WHEN ISNULL(Entitle_2NDDOSE, '') = 'N' THEN 'No' ELSE Entitle_2NDDOSE END AS Entitle_2NDDOSE,
					CASE WHEN ISNULL(Entitle_Inject, '') = 'N' THEN 'No' ELSE Entitle_Inject END AS Entitle_Inject,
					ISNULL(Last3Vaccine,''),
					[Entitle_Inject_Fail_Reason] = SUBSTRING(CONCAT(							
								CASE 
									WHEN ISNULL(Entitle_Inject_Fail_Reason,'') <> '' THEN 
										CASE 
											WHEN CHARINDEX('|||', Entitle_Inject_Fail_Reason) = 0 THEN ' / ' + Entitle_Inject_Fail_Reason
											WHEN CHARINDEX('|||', Entitle_Inject_Fail_Reason) > 0 THEN ' / ' + SUBSTRING(Entitle_Inject_Fail_Reason, 0, CHARINDEX('|||', Entitle_Inject_Fail_Reason)) 
										END
									ELSE '' 
								END,
								IIF(SUBSTRING(HA_Vaccine_Ref_Status,2,1) IN ('C','S'),' / HA connection failed',''), 
								IIF(SUBSTRING(DH_Vaccine_Ref_Status,2,1) IN ('C','S'),' / DH connection failed',''),
								IIF(SUBSTRING(HA_Vaccine_Ref_Status,2,1) IN ('P'),' / HA demographics not matched',''),
								IIF(SUBSTRING(DH_Vaccine_Ref_Status,2,1) IN ('P'),' / DH demographics not matched','')),4,10000)
				FROM #StudentTT T
				WHERE TableID = @RowCount
				ORDER BY Class_Name, Student_Seq

			END

		END
		
		IF @File_ID = 'EHSVF002'
		BEGIN
			IF @SchemeCode = 'VSS' AND @SubsidizeCode = 'VNIAMMR'
			BEGIN
				SELECT 
					Student_Seq,
					Class_Name,
					Class_No,
					CASE WHEN (Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL) OR Chi_Name = '' THEN ISNULL(Chi_Name_Upload,'') ELSE Chi_Name END AS Chi_Name_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Eng_Surname_Upload,'') ELSE Eng_Surname END AS Eng_Surname_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Eng_GivenName_Upload,'') ELSE Eng_GivenName END AS Eng_GivenName_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN Sex_Upload ELSE Sex END AS Sex,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN dbo.func_format_DOB(DOB_Upload, Exact_DOB_Upload, 'en-us', EC_Age_Upload, EC_Date_of_Registration_Upload) ELSE dbo.func_format_DOB(DOB, Exact_DOB, 'en-us', EC_Age, EC_Date_of_Registration) END AS DOB_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN Doc_Code_Upload ELSE Doc_Code END As Doc_Code,
					[HKIC_Symbol] = ISNULL(SD.Status_Description, ''),
					--ISNULL(Contact_No, '') AS Contact_No,
					'',
					CASE WHEN ISNULL(Reject_Injection, 'N') = 'N' 
						THEN -- Check entitle
							CASE WHEN ISNULL(Entitle_Inject, '') = 'N'
							THEN '-'
							ELSE @VaccineToInjectDisplay--Entitle_Inject
							END
						ELSE -- SP confirm not to inject
							'-'
					END AS Available_To_Inject,
					CASE WHEN ISNULL(Reject_Injection, 'N') = 'N' 
						THEN -- Show entitle fail reason (Empty = entitle, not empty = not entitle)																								
							CASE WHEN ISNULL(Entitle_Inject_Fail_Reason,'') <> '' THEN SUBSTRING(Entitle_Inject_Fail_Reason,0, CHARINDEX('|||', Entitle_Inject_Fail_Reason)) ELSE '' END
						ELSE -- SP confirm not to inject
							'Service provider confirm not to inject' 
					END AS Reject_Reason,
					'','','',
					[Service_Receive_Dtm] = FORMAT(Service_Receive_Dtm, 'yyyy/MM/dd')
				FROM #StudentTT T
					LEFT OUTER JOIN
						StatusData SD
							ON Enum_Class = 'HKICSymbol' AND T.HKIC_Symbol = SD.Status_Value
				WHERE TableID = @RowCount
				ORDER BY Class_Name, Student_Seq
			END

			ELSE

			BEGIN
				SELECT 
					Student_Seq,
					Class_Name,
					Class_No,
					CASE WHEN (Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL) OR Chi_Name = '' THEN ISNULL(Chi_Name_Upload,'') ELSE Chi_Name END AS Chi_Name_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Eng_Surname_Upload,'') ELSE Eng_Surname END AS Eng_Surname_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Eng_GivenName_Upload,'') ELSE Eng_GivenName END AS Eng_GivenName_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN Sex_Upload ELSE Sex END AS Sex,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN dbo.func_format_DOB(DOB_Upload, Exact_DOB_Upload, 'en-us', EC_Age_Upload, EC_Date_of_Registration_Upload) ELSE dbo.func_format_DOB(DOB, Exact_DOB, 'en-us', EC_Age, EC_Date_of_Registration) END AS DOB_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN Doc_Code_Upload ELSE Doc_Code END As Doc_Code,
					[With_Validated_Acc],
					--ISNULL(Contact_No, '') AS Contact_No,
					'',
					CASE WHEN ISNULL(Reject_Injection, 'N') = 'N' 
						THEN -- Check entitle
							CASE WHEN ISNULL(Entitle_Inject, '') = 'N'
							THEN '-'
							ELSE @VaccineToInjectDisplay--Entitle_Inject
							END
						ELSE -- SP confirm not to inject
							'-'
					END AS Available_To_Inject,
					CASE WHEN ISNULL(Reject_Injection, 'N') = 'N' 
						THEN -- Show entitle fail reason (Empty = entitle, not empty = not entitle)																								
							CASE WHEN ISNULL(Entitle_Inject_Fail_Reason,'') <> '' THEN SUBSTRING(Entitle_Inject_Fail_Reason,0, CHARINDEX('|||', Entitle_Inject_Fail_Reason)) ELSE '' END
						ELSE -- SP confirm not to inject
							'Service provider confirm not to inject' 
					END AS Reject_Reason
				FROM #StudentTT
				WHERE TableID = @RowCount
				ORDER BY Class_Name, Student_Seq

			END

		END
		
		IF @File_ID = 'EHSVF003'
		BEGIN
			IF @SchemeCode = 'VSS' AND @SubsidizeCode = 'VNIAMMR'
			BEGIN
				SELECT 
					-- Section 1 - Class & account information
					Student_Seq,
					Class_Name,
					Class_No,
					--CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Chi_Name_Upload,'') ELSE Chi_Name END AS Chi_Name_Upload,
					CASE WHEN (Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL) OR Chi_Name = '' THEN 
								ISNULL([dbo].[func_mask_ChiName](Chi_Name_Upload), '')
						ELSE
								[dbo].[func_mask_ChiName](Chi_Name)
						END AS Chi_Name_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Eng_Surname_Upload,'') ELSE Eng_Surname END AS Eng_Surname_Upload,
					--CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Eng_GivenName_Upload,'') ELSE Eng_GivenName END AS Eng_GivenName_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN 
							CASE WHEN Eng_GivenName_Upload = '' THEN ''
							ELSE ISNULL([dbo].[func_get_givenname_initial](Eng_GivenName_Upload), '') END							
						ELSE
							CASE WHEN Eng_GivenName = '' THEN ''
							ELSE [dbo].[func_get_givenname_initial](Eng_GivenName) END	
						END AS Eng_GivenName_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN Sex_Upload ELSE Sex END AS Sex,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN dbo.func_format_DOB(DOB_Upload, Exact_DOB_Upload, 'en-us', EC_Age_Upload, EC_Date_of_Registration_Upload) ELSE dbo.func_format_DOB(DOB, Exact_DOB, 'en-us', EC_Age, EC_Date_of_Registration) END AS DOB_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN Doc_Code_Upload ELSE Doc_Code END As Doc_Code,
					[HKIC_Symbol] = ISNULL(SD.Status_Description, ''),
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
					CASE WHEN ISNULL(Reject_Injection, 'N') = 'N' THEN 'Y' ELSE 'N' END AS Confirm_Injection,	-- Reverse the meaning to display	
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
					-- Section 3 - Vaccination checking result (generated by system)
					[Vaccination_Process_Stage_Dtm] = 
						CASE 
							WHEN Vaccination_Process_Stage_Dtm IS NULL THEN '' 
							ELSE FORMAT(Vaccination_Process_Stage_Dtm, 'dd MMM yyyy') 
						END,
					[Service_Receive_Dtm] = FORMAT(Service_Receive_Dtm, 'yyyy/MM/dd'),
					[Entitle_ONLYDOSE] = CASE WHEN ISNULL(Entitle_ONLYDOSE, '') = 'N' THEN 'No' ELSE ISNULL(Entitle_ONLYDOSE, '') END,
					[Entitle_1STDOSE] = CASE WHEN ISNULL(Entitle_1STDOSE, '') = 'N' THEN 'No' ELSE ISNULL(Entitle_1STDOSE, '') END,
					[Entitle_2NDDOSE] = CASE WHEN ISNULL(Entitle_2NDDOSE, '') = 'N' THEN 'No' ELSE ISNULL(Entitle_2NDDOSE, '') END,
					[Entitle_3RDDOSE] = CASE WHEN ISNULL(Entitle_3RDDOSE, '') = 'N' THEN 'No' ELSE ISNULL(Entitle_3RDDOSE, '') END,
					[Entitle_Inject] = CASE WHEN ISNULL(Entitle_Inject, '') = 'N' THEN 'No' ELSE ISNULL(Entitle_Inject, '') END,
					ISNULL(Last3Vaccine,''),
					[Entitle_Inject_Fail_Reason] = SUBSTRING(CONCAT(							
								CASE 
									WHEN ISNULL(Entitle_Inject_Fail_Reason,'') <> '' THEN 
										CASE 
											WHEN CHARINDEX('|||', Entitle_Inject_Fail_Reason) = 0 THEN ' / ' + Entitle_Inject_Fail_Reason
											WHEN CHARINDEX('|||', Entitle_Inject_Fail_Reason) > 0 THEN ' / ' + SUBSTRING(Entitle_Inject_Fail_Reason, 0, CHARINDEX('|||', Entitle_Inject_Fail_Reason)) 
										END
									ELSE '' 
								END,
								IIF(SUBSTRING(HA_Vaccine_Ref_Status,2,1) IN ('C','S'),' / HA connection failed',''), 
								IIF(SUBSTRING(DH_Vaccine_Ref_Status,2,1) IN ('C','S'),' / DH connection failed',''),
								IIF(SUBSTRING(HA_Vaccine_Ref_Status,2,1) IN ('P'),' / HA demographics not matched',''),
								IIF(SUBSTRING(DH_Vaccine_Ref_Status,2,1) IN ('P'),' / DH demographics not matched','')),4,10000),
					'',
					-- Section 4 - Vaccination record filled by service provider
					[Injected] = 
						CASE 
							WHEN Injected IS NULL THEN '' 
							WHEN Injected = 'N' THEN 'No' 
							WHEN Injected = '1' THEN 'Y' 
							WHEN Injected = '2' THEN 'Y' 
							ELSE Injected
						END,
					'',
					-- Section 5 - Claim record (generated by system)
					[Transaction_ID] = LTRIM(RTRIM(ISNULL(Transaction_ID, ''))),
					[Service_Receive_Dtm] = CASE WHEN Service_Receive_Dtm IS NULL THEN '' ELSE FORMAT(Service_Receive_Dtm, 'yyyy/MM/dd') END,
					[Transaction_Status_Desc] = ISNULL(Transaction_Status_Desc, ''),
					[Transaction_result] = 
						CASE 
							WHEN Transaction_result IS NULL THEN ''
							WHEN CHARINDEX('|||', Transaction_result) = 0 THEN Transaction_result
							WHEN CHARINDEX('|||', Transaction_result) > 0 THEN SUBSTRING(Transaction_result, 0, CHARINDEX('|||', Transaction_result)) 
							ELSE '' 
						END
				FROM #StudentTT T
					LEFT OUTER JOIN
						StatusData SD
							ON Enum_Class = 'HKICSymbol' AND T.HKIC_Symbol = SD.Status_Value
				WHERE TableID = @RowCount
				ORDER BY Class_Name, Student_Seq
			END

			ELSE

			BEGIN
				SELECT 
					-- Section 1 - Class & account information
					Student_Seq,
					Class_Name,
					Class_No,
					--CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Chi_Name_Upload,'') ELSE Chi_Name END AS Chi_Name_Upload,
					CASE WHEN (Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL) OR Chi_Name = '' THEN 
								ISNULL([dbo].[func_mask_ChiName](Chi_Name_Upload), '')
						ELSE
								[dbo].[func_mask_ChiName](Chi_Name)
						END AS Chi_Name_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Eng_Surname_Upload,'') ELSE Eng_Surname END AS Eng_Surname_Upload,
					--CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Eng_GivenName_Upload,'') ELSE Eng_GivenName END AS Eng_GivenName_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN 
							CASE WHEN Eng_GivenName_Upload = '' THEN ''
							ELSE ISNULL([dbo].[func_get_givenname_initial](Eng_GivenName_Upload), '') END							
						ELSE
							CASE WHEN Eng_GivenName = '' THEN ''
							ELSE [dbo].[func_get_givenname_initial](Eng_GivenName) END	
						END AS Eng_GivenName_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN Sex_Upload ELSE Sex END AS Sex,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN dbo.func_format_DOB(DOB_Upload, Exact_DOB_Upload, 'en-us', EC_Age_Upload, EC_Date_of_Registration_Upload) ELSE dbo.func_format_DOB(DOB, Exact_DOB, 'en-us', EC_Age, EC_Date_of_Registration) END AS DOB_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN Doc_Code_Upload ELSE Doc_Code END As Doc_Code,
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
					CASE WHEN ISNULL(Reject_Injection, 'N') = 'N' THEN 'Y' ELSE 'N' END AS Confirm_Injection,	-- Reverse the meaning to display	
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
					-- Section 3 - Vaccination checking result (generated by system)
					[Vaccination_Process_Stage_Dtm] = 
						CASE 
							WHEN Vaccination_Process_Stage_Dtm IS NULL THEN '' 
							ELSE FORMAT(Vaccination_Process_Stage_Dtm, 'dd MMM yyyy') 
						END,
					[Entitle_ONLYDOSE] = CASE WHEN ISNULL(Entitle_ONLYDOSE, '') = 'N' THEN 'No' ELSE ISNULL(Entitle_ONLYDOSE, '') END,
					[Entitle_1STDOSE] = CASE WHEN ISNULL(Entitle_1STDOSE, '') = 'N' THEN 'No' ELSE ISNULL(Entitle_1STDOSE, '') END,
					[Entitle_2NDDOSE] = CASE WHEN ISNULL(Entitle_2NDDOSE, '') = 'N' THEN 'No' ELSE ISNULL(Entitle_2NDDOSE, '') END,
					[Entitle_Inject] = CASE WHEN ISNULL(Entitle_Inject, '') = 'N' THEN 'No' ELSE ISNULL(Entitle_Inject, '') END,
					ISNULL(Last3Vaccine,''),
					[Entitle_Inject_Fail_Reason] = SUBSTRING(CONCAT(							
								CASE 
									WHEN ISNULL(Entitle_Inject_Fail_Reason,'') <> '' THEN 
										CASE 
											WHEN CHARINDEX('|||', Entitle_Inject_Fail_Reason) = 0 THEN ' / ' + Entitle_Inject_Fail_Reason
											WHEN CHARINDEX('|||', Entitle_Inject_Fail_Reason) > 0 THEN ' / ' + SUBSTRING(Entitle_Inject_Fail_Reason, 0, CHARINDEX('|||', Entitle_Inject_Fail_Reason)) 
										END
									ELSE '' 
								END,
								IIF(SUBSTRING(HA_Vaccine_Ref_Status,2,1) IN ('C','S'),' / HA connection failed',''), 
								IIF(SUBSTRING(DH_Vaccine_Ref_Status,2,1) IN ('C','S'),' / DH connection failed',''),
								IIF(SUBSTRING(HA_Vaccine_Ref_Status,2,1) IN ('P'),' / HA demographics not matched',''),
								IIF(SUBSTRING(DH_Vaccine_Ref_Status,2,1) IN ('P'),' / DH demographics not matched','')),4,10000),
					'',
					-- Section 4 - Vaccination record filled by service provider
					[Injected] = 
						CASE 
							WHEN Injected IS NULL THEN '' 
							WHEN Injected = 'N' THEN 'No' 
							WHEN Injected = '1' THEN 'Y' 
							WHEN Injected = '2' THEN 'Y' 
							ELSE Injected
						END,
					'',
					-- Section 5 - Claim record (generated by system)
					[Transaction_ID] = LTRIM(RTRIM(ISNULL(Transaction_ID, ''))),
					[Service_Receive_Dtm] = CASE WHEN Service_Receive_Dtm IS NULL THEN '' ELSE FORMAT(Service_Receive_Dtm, 'yyyy/MM/dd') END,
					[Transaction_Status_Desc] = ISNULL(Transaction_Status_Desc, ''),
					[Transaction_result] = 
						CASE 
							WHEN Transaction_result IS NULL THEN ''
							WHEN CHARINDEX('|||', Transaction_result) = 0 THEN Transaction_result
							WHEN CHARINDEX('|||', Transaction_result) > 0 THEN SUBSTRING(Transaction_result, 0, CHARINDEX('|||', Transaction_result)) 
							ELSE '' 
						END
				FROM #StudentTT
				WHERE TableID = @RowCount
				ORDER BY Class_Name, Student_Seq
			END
		END

		IF @File_ID = 'EHSVF005'
		BEGIN
			IF @SchemeCode = 'VSS' AND @SubsidizeCode = 'VNIAMMR'
			BEGIN
				SELECT 
					Student_Seq,
					Class_Name,
					Class_No,
					--CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Chi_Name_Upload,'') ELSE Chi_Name END AS Chi_Name_Upload,
					CASE WHEN (Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL) OR Chi_Name = '' THEN 
								ISNULL([dbo].[func_mask_ChiName](Chi_Name_Upload), '')
						ELSE
								[dbo].[func_mask_ChiName](Chi_Name)
						END AS Chi_Name_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Eng_Surname_Upload,'') ELSE Eng_Surname END AS Eng_Surname_Upload,
					--CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Eng_GivenName_Upload,'') ELSE Eng_GivenName END AS Eng_GivenName_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN 
							CASE WHEN Eng_GivenName_Upload = '' THEN ''
							ELSE ISNULL([dbo].[func_get_givenname_initial](Eng_GivenName_Upload), '') END							
						ELSE
							CASE WHEN Eng_GivenName = '' THEN ''
							ELSE [dbo].[func_get_givenname_initial](Eng_GivenName) END	
						END AS Eng_GivenName_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN Sex_Upload ELSE Sex END AS Sex,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN dbo.func_format_DOB(DOB_Upload, Exact_DOB_Upload, 'en-us', EC_Age_Upload, EC_Date_of_Registration_Upload) ELSE dbo.func_format_DOB(DOB, Exact_DOB, 'en-us', EC_Age, EC_Date_of_Registration) END AS DOB_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN Doc_Code_Upload ELSE Doc_Code END As Doc_Code,
					[HKIC_Symbol] = ISNULL(SD.Status_Description, '')
					--ISNULL(Contact_No, '') AS Contact_No
				FROM #StudentTT T
					LEFT OUTER JOIN
						StatusData SD
							ON Enum_Class = 'HKICSymbol' AND T.HKIC_Symbol = SD.Status_Value
				WHERE TableID = @RowCount
				ORDER BY Class_Name, Student_Seq
			END

			ELSE

			BEGIN
				SELECT 
					Student_Seq,
					Class_Name,
					Class_No,
					--CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Chi_Name_Upload,'') ELSE Chi_Name END AS Chi_Name_Upload,
					CASE WHEN (Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL) OR Chi_Name = '' THEN 
								ISNULL([dbo].[func_mask_ChiName](Chi_Name_Upload), '')
						ELSE
								[dbo].[func_mask_ChiName](Chi_Name)
						END AS Chi_Name_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Eng_Surname_Upload,'') ELSE Eng_Surname END AS Eng_Surname_Upload,
					--CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Eng_GivenName_Upload,'') ELSE Eng_GivenName END AS Eng_GivenName_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN 
							CASE WHEN Eng_GivenName_Upload = '' THEN ''
							ELSE ISNULL([dbo].[func_get_givenname_initial](Eng_GivenName_Upload), '') END							
						ELSE
							CASE WHEN Eng_GivenName = '' THEN ''
							ELSE [dbo].[func_get_givenname_initial](Eng_GivenName) END	
						END AS Eng_GivenName_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN Sex_Upload ELSE Sex END AS Sex,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN dbo.func_format_DOB(DOB_Upload, Exact_DOB_Upload, 'en-us', EC_Age_Upload, EC_Date_of_Registration_Upload) ELSE dbo.func_format_DOB(DOB, Exact_DOB, 'en-us', EC_Age, EC_Date_of_Registration) END AS DOB_Upload,
					CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN Doc_Code_Upload ELSE Doc_Code END As Doc_Code
					--ISNULL(Contact_No, '') AS Contact_No
				FROM #StudentTT
				WHERE TableID = @RowCount
				ORDER BY Class_Name, Student_Seq
			END

		END

		IF @File_ID = 'EHSVF006'
		BEGIN
			SELECT 
				Student_Seq,
				Class_Name,
				Class_No,
				CASE WHEN (Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL) OR Chi_Name = '' THEN ISNULL(Chi_Name_Upload,'') ELSE Chi_Name END AS Chi_Name_Upload,
				CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Eng_Surname_Upload,'') ELSE Eng_Surname END AS Eng_Surname_Upload,
				CASE WHEN Voucher_Acc_ID IS NULL AND Temp_Voucher_Acc_ID IS NULL THEN ISNULL(Eng_GivenName_Upload,'') ELSE Eng_GivenName END AS Eng_GivenName_Upload
			FROM #StudentTT
			WHERE TableID = @RowCount
			ORDER BY Class_Name, Student_Seq
		END

		SET @RowCount = @RowCount +1
	END
		

	
	CLOSE SYMMETRIC KEY sym_Key
	
	DROP TABLE #StudentTT
	DROP TABLE #Last3VaccineTT
	DROP TABLE #Last3VaccineRowTT
	DROP TABLE #Control

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSSF_Class_Report_get] TO HCVU
GO

