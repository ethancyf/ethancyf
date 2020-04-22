IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0027_01_PIDVSSeHealthAccountClaimByDocumentType_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0027_01_PIDVSSeHealthAccountClaimByDocumentType_Stat]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
  
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	11 Sep 2017
-- CR No.:			CRE17-003 (Stop vaccine daily stat)
-- Description:		Stored procedure is no longer used
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	2 November 2015
-- CR No.:			CRE15-006
-- Description:		Rename of eHS
-- =============================================
-- =============================================  
-- CR No.:		CRE15-005-04
-- Author:		Winnie SUEN
-- Create date: 27 Aug 2015
-- Description: Retrieve eHealth Account with Claim by DOC Type Statistic for PIDVSS
--				Copy from proc_EHS_eHealthAccountClaimByDocumentType_CIVSS_Stat
-- =============================================  
/*
CREATE PROCEDURE [proc_EHS_eHSD0027_01_PIDVSSeHealthAccountClaimByDocumentType_Stat] 
	@Cutoff_Dtm AS DATETIME
AS
BEGIN
	
	SET NOCOUNT ON;

	-- =============================================  
	-- Declaration  
	-- =============================================  
	DECLARE @system_Dtm AS DATETIME

	-- =============================================  
	-- Validation   
	-- =============================================  
	-- =============================================  
	-- Initialization  
	-- =============================================  

	-- =============================================  
	-- Return results  
	-- =============================================  
	SET @system_Dtm = getdate()

	CREATE TABLE #statNoOfAcc_withClaim_byDoc (
		_Voucher_acc_ID CHAR(15),
		_temp_voucher_acc_ID CHAR(15),
		_special_acc_ID CHAR(15),
		_doc_code CHAR(20),
		_doc_ID VARBINARY(100),
		_Documentary_Proof VARCHAR(20)
		)

	CREATE TABLE #statNoOfAcc_withClaim_byDoc_distinct (
		_doc_code CHAR(20),
		_doc_ID VARBINARY(100)
		)
		
	CREATE TABLE #statNoOfAcc_withClaim_byDoc_byDocProof_distinct (
		_doc_code CHAR(20),
		_doc_ID VARBINARY(100),
		_Documentary_Proof VARCHAR(20)
		)		

	CREATE TABLE #result_table (
		_display_seq TINYINT,
		_result_value1 VARCHAR(200) DEFAULT '',
		_result_value2 VARCHAR(100) DEFAULT '',
		_result_value3 VARCHAR(100) DEFAULT '',
		_result_value4 VARCHAR(100) DEFAULT '',
		_result_value5 VARCHAR(100) DEFAULT '',
		_result_value6 VARCHAR(100) DEFAULT '',
		_result_value7 VARCHAR(100) DEFAULT '',
		_result_value8 VARCHAR(100) DEFAULT '',
		_result_value9 VARCHAR(100) DEFAULT '',
		_result_value10 VARCHAR(100) DEFAULT '',
		_result_value11 VARCHAR(100) DEFAULT ''
		)

	-- insert record for the final output format  
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (0, 'eHS(S)D0027-01: Report on eHealth (Subsidies) Accounts with PIDVSS claim transactions by document type')

	INSERT INTO #result_table (_display_seq)
	VALUES (1)

	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (2, 'Reporting period: as at ' + CONVERT(VARCHAR, DATEADD(dd, - 1, @Cutoff_Dtm), 111))

	INSERT INTO #result_table (_display_seq)
	VALUES (3)

	--===================================
	--(i) Identity Document
	--===================================
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (8, '(i) Identity Document')

	INSERT INTO #result_table (_display_seq)
	VALUES (9)

	INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5,
		_result_value6, _result_value7, _result_value8, _result_value9, _result_value10, _result_value11)
	VALUES (10, 'PIDVSS', '', '', '', '', '', '', 'Total', '', 'PIDVSS', '')


	INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5,
		_result_value6, _result_value7, _result_value8, _result_value9, _result_value10, _result_value11)
	VALUES (11, 'HKIC/HKBC', 'Doc/I', 'REPMT', 'ID235B', 'VISA', 'ADOPC', 'EC', '', '', 'HKIC', 'HKBC')

	INSERT INTO #result_table (_display_seq)
	VALUES (12)

	-- select the voucher transactions included for statistic  
	INSERT INTO #statNoOfAcc_withClaim_byDoc (
		_Voucher_acc_ID,
		_temp_voucher_acc_ID,
		_special_acc_ID,
		_doc_code,
		_Documentary_Proof
		)
		SELECT vTran.Voucher_acc_ID,
		vTran.temp_voucher_acc_ID,
		vTran.special_acc_ID,
		vTran.doc_code,
		TAF.AdditionalFieldValueCode FROM voucherTransaction vTran INNER JOIN TransactionAdditionalField TAF ON vTran.Transaction_ID = TAF.Transaction_ID
		AND TAF.AdditionalFieldID = 'DocumentaryProof' WHERE vTran.Scheme_Code IN ('PIDVSS')
		AND vTran.transaction_dtm <= @Cutoff_Dtm
		AND (
			vTran.Invalidation IS NULL
			OR vTran.Invalidation NOT IN (
				SELECT Status_Value
				FROM StatStatusFilterMapping
				WHERE ( report_id = 'ALL' OR report_id = 'eHSD0027')
					AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'
					AND (( Effective_Date IS NULL OR Effective_Date >= @cutoff_dtm)
						AND ( Expiry_Date IS NULL OR Expiry_Date < @cutoff_dtm ))
				)
			)
		AND vTran.Record_Status NOT IN (
			SELECT Status_Value
			FROM StatStatusFilterMapping
			WHERE ( report_id = 'ALL'OR report_id = 'eHSD0027')
				AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'
				AND (( Effective_Date IS NULL OR Effective_Date >= @cutoff_dtm)
					AND ( Expiry_Date IS NULL OR Expiry_Date < @cutoff_dtm))
			)



	-- update the DOC ID for validated ACC  
	UPDATE #statNoOfAcc_withClaim_byDoc
	SET _doc_ID = (
			SELECT pInfo.[Encrypt_Field1]
			FROM personalInformation pInfo,
				voucherAccount vACC
			WHERE pInfo.voucher_acc_ID COLLATE DATABASE_DEFAULT = #statNoOfAcc_withClaim_byDoc._Voucher_acc_ID COLLATE DATABASE_DEFAULT
				AND pInfo.Doc_Code COLLATE DATABASE_DEFAULT = #statNoOfAcc_withClaim_byDoc._Doc_Code COLLATE DATABASE_DEFAULT
				AND vAcc.voucher_acc_ID = pInfo.voucher_acc_ID
				AND vAcc.record_status <> 'D'
			)
	--and vAcc.create_by NOT IN (SELECT SP_ID FROM SPExceptionList)  
	WHERE #statNoOfAcc_withClaim_byDoc._doc_ID IS NULL OR #statNoOfAcc_withClaim_byDoc._doc_ID = ''

	-- update the DOC ID for special ACC  
	UPDATE #statNoOfAcc_withClaim_byDoc
	SET _doc_ID = (
			SELECT sInfo.[Encrypt_Field1]
			FROM specialpersonalinformation sInfo,
				specialaccount sAcc
			WHERE sInfo.special_acc_ID COLLATE DATABASE_DEFAULT = #statNoOfAcc_withClaim_byDoc._special_acc_ID COLLATE DATABASE_DEFAULT
				AND sInfo.Doc_Code COLLATE DATABASE_DEFAULT = #statNoOfAcc_withClaim_byDoc._Doc_Code COLLATE DATABASE_DEFAULT
				AND sAcc.special_acc_ID = sInfo.special_acc_ID
				AND sAcc.record_status NOT IN ('V', 'D')
			)
	--and sAcc.create_by NOT IN (SELECT SP_ID FROM SPExceptionList)  
	WHERE #statNoOfAcc_withClaim_byDoc._doc_ID IS NULL OR #statNoOfAcc_withClaim_byDoc._doc_ID = ''

	-- update the DOC ID for temp ACC  
	UPDATE #statNoOfAcc_withClaim_byDoc
	SET _doc_ID = (
			SELECT tInfo.[Encrypt_Field1]
			FROM temppersonalinformation tInfo,
				tempvoucheraccount tAcc
			WHERE tInfo.voucher_acc_ID COLLATE DATABASE_DEFAULT = #statNoOfAcc_withClaim_byDoc._temp_voucher_acc_ID COLLATE DATABASE_DEFAULT
				AND tInfo.Doc_Code COLLATE DATABASE_DEFAULT = #statNoOfAcc_withClaim_byDoc._Doc_Code COLLATE DATABASE_DEFAULT
				AND tAcc.voucher_acc_ID = tInfo.voucher_acc_ID
				AND tAcc.record_status NOT IN ('V', 'D')
			)
	--and tAcc.create_by NOT IN (SELECT SP_ID FROM SPExceptionList)  
	WHERE #statNoOfAcc_withClaim_byDoc._doc_ID IS NULL OR #statNoOfAcc_withClaim_byDoc._doc_ID = ''


	INSERT INTO #statNoOfAcc_withClaim_byDoc_distinct (_doc_code, _doc_ID)
	SELECT DISTINCT _doc_code, _doc_ID 
	FROM #statNoOfAcc_withClaim_byDoc 
	WHERE _doc_ID IS NOT NULL AND _doc_ID <> ''


	UPDATE #result_table
	SET _result_value1 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code IN ('HKIC', 'HKBC'))
	WHERE _display_seq = 12

	UPDATE #result_table
	SET _result_value2 = (
			SELECT count(1)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'Doc/I')
	WHERE _display_seq = 12

	UPDATE #result_table
	SET _result_value3 = (
			SELECT count(1)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'REPMT')
	WHERE _display_seq = 12

	UPDATE #result_table
	SET _result_value4 = (
			SELECT count(1)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'ID235B')
	WHERE _display_seq = 12

	UPDATE #result_table
	SET _result_value5 = (
			SELECT count(1)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'VISA')
	WHERE _display_seq = 12

	UPDATE #result_table
	SET _result_value6 = (
			SELECT count(1)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'ADOPC')
	WHERE _display_seq = 12

	UPDATE #result_table
	SET _result_value7 = (
			SELECT count(1)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'EC')
	WHERE _display_seq = 12

	UPDATE #result_table
	SET _result_value8 = (CONVERT(INT, _result_value1) + CONVERT(INT, _result_value2) + CONVERT(INT, _result_value3) + CONVERT(INT, _result_value4) + CONVERT(INT, _result_value5) + CONVERT(INT, _result_value6) + CONVERT(INT, _result_value7))
	WHERE _display_seq = 12

	UPDATE #result_table
	SET _result_value9 = ''
	WHERE _display_seq = 12

	UPDATE #result_table
	SET _result_value10 = (
			SELECT count(1)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'HKIC')
	WHERE _display_seq = 12

	UPDATE #result_table
	SET _result_value11 = (
			SELECT count(1)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'HKBC')
	WHERE _display_seq = 12

	--===================================
	--(ii) Type of Documentary Proof
	--===================================
	INSERT INTO #result_table (_display_seq)
	VALUES (15)

	INSERT INTO #result_table (_display_seq)
	VALUES (16)

	INSERT INTO #result_table (_display_seq, _result_value1
		)
	VALUES (17, '(ii) Type of Documentary Proof')

	INSERT INTO #result_table (_display_seq)
	VALUES (18)
	
	INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3)
	VALUES (19, 'Registration Card for People with Disabilities', 'Medical Certificate', 'Total')

	INSERT INTO #result_table (_display_seq)
	VALUES (20)
	
		
	INSERT INTO #statNoOfAcc_withClaim_byDoc_byDocProof_distinct (_doc_code, _doc_ID, _Documentary_Proof) 
		(
		SELECT DISTINCT 
			CASE WHEN _doc_code IN ('HKIC','HKBC') THEN 'HKICBC' ELSE _doc_code END AS doc_code
		, _doc_ID, _Documentary_Proof
		FROM #statNoOfAcc_withClaim_byDoc 
		WHERE _doc_ID IS NOT NULL AND _doc_ID <> ''
		)	
		
		
	UPDATE #result_table
	SET _result_value1 = (
			SELECT count(1)
			FROM #statNoOfAcc_withClaim_byDoc_byDocProof_distinct
			WHERE _Documentary_Proof = 'REG_CARD')
	WHERE _display_seq = 20

	
	UPDATE #result_table
	SET _result_value2 = (
			SELECT count(1)
			FROM #statNoOfAcc_withClaim_byDoc_byDocProof_distinct
			WHERE _Documentary_Proof = 'MED_CERT')
	WHERE _display_seq = 20
	
		
		
	UPDATE #result_table
	SET _result_value3 = (CONVERT(INT, _result_value1) + CONVERT(INT, _result_value2) + CONVERT(INT, _result_value3))
	WHERE _display_seq = 20

	--===================================
	-- Retrieve the final result  
	--===================================
	DELETE
	FROM RpteHSD0027PIDVSSeHealthAccountByDocumentTypeStat

	INSERT INTO RpteHSD0027PIDVSSeHealthAccountByDocumentTypeStat (
		Display_Seq,
		Col1,
		Col2,
		Col3,
		Col4,
		Col5,
		Col6,
		Col7,
		Col8,
		Col9,
		Col10,
		Col11
		)
	SELECT _display_Seq,
		_result_value1,
		_result_value2,
		_result_value3,
		_result_value4,
		_result_value5,
		_result_value6,
		_result_value7,
		_result_value8,
		_result_value9,
		_result_value10,
		_result_value11
	FROM #result_table
	ORDER BY _display_seq


	DROP TABLE #statNoOfAcc_withClaim_byDoc

	DROP TABLE #statNoOfAcc_withClaim_byDoc_distinct
	DROP TABLE #statNoOfAcc_withClaim_byDoc_byDocProof_distinct

	DROP TABLE #result_table
	
END
GO


GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0027_01_PIDVSSeHealthAccountClaimByDocumentType_Stat] TO HCVU
GO
*/


