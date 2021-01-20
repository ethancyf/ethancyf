IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntryStaging_get_forVaccineCheck]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntryStaging_get_forVaccineCheck]
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
-- Modified date:	20 Jul 2020
-- CR No.			CRE19-031 (VSS MMR Upload)
-- Description:		Add columns (HKICSymbol, Service_Receive_Dtm)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	11 Sep 2019
-- CR No.			CRE19-001-04 (RVP Pre-check)
-- Description:		Trim School Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	18 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Get StudentFileEntryStaging for vaccination checking
-- =============================================    

CREATE PROCEDURE [dbo].[proc_StudentFileEntryStaging_get_forVaccineCheck]
	@StudentFileID AS VARCHAR(15),
	@HA_Connection_Fail_Only AS BIT,
	@DH_Connection_Fail_Only AS BIT
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

	-- ====================================
	-- Validated Account
	-- ====================================
	SELECT 
		H.Student_File_ID,
		RTRIM(H.School_Code) AS [School_Code],
		H.SP_ID,
		H.Practice_Display_Seq,
		H.Service_Receive_Dtm AS [Service_Receive_Dtm_Header],
		H.Service_Receive_Dtm_2 AS [Service_Receive_Dtm_2_Header],
		H.Scheme_Code,
		H.Scheme_Seq,
		H.Dose,
		H.Claim_Upload_By,
		E.Student_Seq,
		E.Class_Name,
		E.Class_No,
		E.Acc_Process_Stage,
		E.Vaccination_Process_Stage,
		E.Ext_Ref_Status AS [HA_Vaccine_Ref_Status],
		E.DH_Vaccine_Ref_Status AS [DH_Vaccine_Ref_Status],
		E.Acc_Type,
		E.Entitle_ONLYDOSE,
		E.Entitle_1STDOSE,
		E.Entitle_2NDDOSE,
		E.Entitle_3RDDOSE,
		E.Entitle_Inject,
		E.Injected,
		E.Service_Receive_Dtm,
		E.HKIC_Symbol,
		VA.Record_Status AS [Acc_Record_Status],
		PI.Voucher_Acc_ID,
		PI.Doc_Code,
		PI.DOB,
		PI.Exact_DOB,
		PI.Sex,
		convert(varchar, DecryptByKey(PI.[Encrypt_Field1])) as Doc_No,
		convert(varchar(40), DecryptByKey(PI.[Encrypt_Field2])) as Eng_Name,
		PI.EC_Serial_No
	FROM
		StudentFileEntryStaging E WITH (NOLOCK)
		INNER JOIN StudentFileHeaderStaging H WITH (NOLOCK)
		ON E.Student_File_ID = H.Student_File_ID
		INNER JOIN PersonalInformation PI WITH (NOLOCK)
		ON E.Acc_Type = 'V' AND E.Voucher_Acc_ID = PI.Voucher_Acc_ID AND E.Acc_Doc_code = PI.Doc_Code
		INNER JOIN VoucherAccount VA WITH (NOLOCK)
		ON PI.Voucher_Acc_ID = VA.Voucher_Acc_ID
	WHERE
		H.Student_File_ID = @StudentFileID
		--AND E.Vaccination_Checking_Status = 'P'
		AND (@HA_Connection_Fail_Only = 0 OR (@HA_Connection_Fail_Only = 1 AND E.Ext_Ref_Status LIKE '_C_'))
		AND (@DH_Connection_Fail_Only = 0 OR (@DH_Connection_Fail_Only = 1 AND E.DH_Vaccine_Ref_Status LIKE '_C_'))
	UNION ALL
	-- ====================================
	-- Temp Account
	-- ====================================
	SELECT 
		H.Student_File_ID,
		RTRIM(H.School_Code) AS [School_Code],
		H.SP_ID,
		H.Practice_Display_Seq,
		H.Service_Receive_Dtm AS [Service_Receive_Dtm_Header],
		H.Service_Receive_Dtm_2 AS [Service_Receive_Dtm_2_Header],
		H.Scheme_Code,
		H.Scheme_Seq,
		H.Dose,
		H.Claim_Upload_By,
		E.Student_Seq,
		E.Class_Name,
		E.Class_No,
		E.Acc_Process_Stage,
		E.Vaccination_Process_Stage,
		E.Ext_Ref_Status AS [HA_Vaccine_Ref_Status],
		E.DH_Vaccine_Ref_Status AS [DH_Vaccine_Ref_Status],
		E.Acc_Type,
		E.Entitle_ONLYDOSE,
		E.Entitle_1STDOSE,
		E.Entitle_2NDDOSE,
		E.Entitle_3RDDOSE,
		E.Entitle_Inject,
		E.Injected,
		E.Service_Receive_Dtm,
		E.HKIC_Symbol,
		VA.Record_Status AS [Acc_Record_Status],
		PI.Voucher_Acc_ID,
		PI.Doc_Code,
		PI.DOB,
		PI.Exact_DOB,
		PI.Sex,
		convert(varchar, DecryptByKey(PI.[Encrypt_Field1])) as Doc_No,
		convert(varchar(40), DecryptByKey(PI.[Encrypt_Field2])) as Eng_Name,
		PI.EC_Serial_No
	FROM
		StudentFileEntryStaging E WITH (NOLOCK)
		INNER JOIN StudentFileHeaderStaging H WITH (NOLOCK)
		ON E.Student_File_ID = H.Student_File_ID
		INNER JOIN TempPersonalInformation PI WITH (NOLOCK)
		ON E.Acc_Type = 'T' AND E.Temp_Voucher_Acc_ID = PI.Voucher_Acc_ID AND E.Acc_Doc_code = PI.Doc_Code
		INNER JOIN TempVoucherAccount VA WITH (NOLOCK)
		ON PI.Voucher_Acc_ID = VA.Voucher_Acc_ID
	WHERE
		H.Student_File_ID = @StudentFileID
		--AND E.Vaccination_Checking_Status = 'P'
		AND (@HA_Connection_Fail_Only = 0 OR (@HA_Connection_Fail_Only = 1 AND E.Ext_Ref_Status LIKE '_C_'))
		AND (@DH_Connection_Fail_Only = 0 OR (@DH_Connection_Fail_Only = 1 AND E.DH_Vaccine_Ref_Status LIKE '_C_'))
	UNION ALL
	-- ====================================
	-- No Account
	-- ====================================
	SELECT 
		H.Student_File_ID,
		RTRIM(H.School_Code) AS [School_Code],
		H.SP_ID,
		H.Practice_Display_Seq,
		H.Service_Receive_Dtm AS [Service_Receive_Dtm_Header],
		H.Service_Receive_Dtm_2 AS [Service_Receive_Dtm_2_Header],
		H.Scheme_Code,
		H.Scheme_Seq,
		H.Dose,
		H.Claim_Upload_By,
		E.Student_Seq,
		E.Class_Name,
		E.Class_No,
		E.Acc_Process_Stage,
		E.Vaccination_Process_Stage,
		E.Ext_Ref_Status AS [HA_Vaccine_Ref_Status],
		E.DH_Vaccine_Ref_Status AS [DH_Vaccine_Ref_Status],
		E.Acc_Type,
		E.Entitle_ONLYDOSE,
		E.Entitle_1STDOSE,
		E.Entitle_2NDDOSE,
		E.Entitle_3RDDOSE,
		E.Entitle_Inject,
		E.Injected,
		E.Service_Receive_Dtm,
		E.HKIC_Symbol,
		NULL AS [Acc_Record_Status],
		NULL AS [Voucher_Acc_ID],
		NULL AS [Doc_Code],
		NULL AS [DOB],
		NULL AS [Exact_DOB],
		NULL AS [Sex],
		NULL AS [Doc_No],
		NULL AS [Eng_Name],
		NULL AS [EC_Serial_No]
	FROM
		StudentFileEntryStaging E WITH (NOLOCK)
		INNER JOIN StudentFileHeaderStaging H WITH (NOLOCK)
		ON E.Student_File_ID = H.Student_File_ID
	WHERE
		H.Student_File_ID = @StudentFileID
		AND E.Acc_Type IS NULL
		--AND E.Vaccination_Checking_Status = 'P'
		AND (@HA_Connection_Fail_Only = 0 OR (@HA_Connection_Fail_Only = 1 AND E.Ext_Ref_Status LIKE '_C_'))
		AND (@DH_Connection_Fail_Only = 0 OR (@DH_Connection_Fail_Only = 1 AND E.DH_Vaccine_Ref_Status LIKE '_C_'))
	ORDER BY
		E.Student_Seq
	
	--
	
	EXEC [proc_SymmetricKey_close]
	

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryStaging_get_forVaccineCheck] TO HCVU
GO

