IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0033_01_PrepareData]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0033_01_PrepareData]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
  
-- ============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.			
-- Description:		
-- =============================================
-- =============================================    
-- Author:			Winnie SUEN
-- Create date:		04 Oct 2019
-- CR No.			CRE19-001-05 (PPP 2019-20 - Report)
-- Description:		PPPKG daily report - eHS(S)D0033-01: Report on eHealth (Subsidies) accounts created (by document type)
-- =============================================  


CREATE PROCEDURE [proc_EHS_eHSD0033_01_PrepareData] 
	@Cutoff_Dtm AS DATETIME
AS
BEGIN
	
	SET NOCOUNT ON;

	-- =============================================  
	-- Declaration  
	-- =============================================  
	DECLARE @system_Dtm AS DATETIME  
	DECLARE @Scheme_Code AS VARCHAR(10)
	DECLARE @Report_ID AS VARCHAR(30)  
	
	DECLARE @Claim_Period_From AS	DATETIME
	DECLARE @Scheme_Display_Code AS	VARCHAR(25)

	-- =============================================  
	-- Validation   
	-- =============================================  
	-- =============================================  
	-- Initialization  
	-- =============================================  
	SET @system_Dtm = GETDATE()
	SET @Scheme_Code = 'PPPKG'
	SET @Report_ID = 'eHSD0033' 

	SELECT 
		@Scheme_Display_Code = RTRIM(Display_Code),
		@Claim_Period_From = Claim_Period_From
	FROM SchemeClaim
	WHERE Scheme_Code = @Scheme_Code

	-- =============================================  
	-- Return results  
	-- =============================================  
	 CREATE TABLE #statNoOfAcc_byDoc (  
		_doc_code CHAR(20),
		_doc_ID VARBINARY(100),
		_Deceased CHAR(1) 
		)  
 	
	CREATE TABLE #statNoOfAcc_withClaim_byDoc (
		_Voucher_acc_ID CHAR(15),
		_temp_voucher_acc_ID CHAR(15),
		_special_acc_ID CHAR(15),
		_doc_code CHAR(20),
		_doc_ID VARBINARY(100),
		_Deceased CHAR(1)
		)

	CREATE TABLE #statNoOfAcc_withClaim_byDoc_distinct (
		_doc_code CHAR(20),
		_doc_ID VARBINARY(100),
		_Deceased CHAR(1)
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
		_result_value11 VARCHAR(100) DEFAULT '',
		_result_value12 VARCHAR(100) DEFAULT '',
		_result_value13 VARCHAR(100) DEFAULT '',
		_result_value14 VARCHAR(100) DEFAULT '',
		_result_value15 VARCHAR(100) DEFAULT '',
		_result_value16 VARCHAR(100) DEFAULT '',
		_result_value17 VARCHAR(100) DEFAULT '',
		_result_value18 VARCHAR(100) DEFAULT '',
		_result_value19 VARCHAR(100) DEFAULT '',
		_result_value20 VARCHAR(100) DEFAULT ''
		)

	-- insert record for the final output format  
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (0, 'eHS(S)D0033-01: Report on eHealth (Subsidies) accounts created (by document type)')

	INSERT INTO #result_table (_display_seq)
	VALUES (1)

	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (2, 'Reporting period: as at ' + CONVERT(VARCHAR, DATEADD(dd, - 1, @Cutoff_Dtm), 111))

	INSERT INTO #result_table (_display_seq)
	VALUES (3)

	--=======================================
	-- (i) eHealth (Subsidies) accounts
	--=======================================

	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (11, '(i) eHealth (Subsidies) accounts')

	INSERT INTO #result_table (_display_seq)
	VALUES (12)
	
	INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5,
		_result_value6, _result_value7, _result_value8, _result_value9, _result_value10, _result_value11, _result_value12,
		_result_value13, _result_value14, _result_value15, _result_value16, _result_value17, _result_value18, _result_value19)
	VALUES (13, '','HKIC/HKBC', 'Doc/I', 'REPMT', 'ID235B', 'VISA', 'ADOPC', 'EC', 'OC', 'OW', 'TW', 'IR', 'HKP', 'RFNo8', 'OTHER', 'Total', '', 'HKIC', 'HKBC')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (14 ,'Alive')

	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (15 ,'Deceased')

	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (16 ,'Total')

	-- --------------------------------------------- 
	-- Validated Account  
	-- ---------------------------------------------  
	INSERT INTO #statNoOfAcc_byDoc (
		_doc_code,
		_doc_ID,
		_Deceased
	)  
	SELECT  
		VP.Doc_Code,  
		VP.Encrypt_Field1,
		CASE WHEN VP.Deceased IS NULL THEN 'N' ELSE
			CASE WHEN VP.Deceased = 'Y' THEN 'Y' ELSE 'N' END
		END
	FROM  
		VoucherAccount VA  
		INNER JOIN PersonalInformation VP ON VA.Voucher_Acc_ID = VP.Voucher_Acc_ID  
	WHERE  
		VA.Effective_Dtm <= @Cutoff_Dtm  
   
   
	-- ---------------------------------------------  
	-- Temporary Account  
	-- ---------------------------------------------  
  
	INSERT INTO #statNoOfAcc_byDoc (
		_doc_code,
		_doc_ID,
		_Deceased
	)  
	SELECT  
		TP.Doc_Code,  
		TP.Encrypt_Field1, 
		CASE WHEN TP.Deceased IS NULL THEN 'N' ELSE
			CASE WHEN TP.Deceased = 'Y' THEN 'Y' ELSE 'N' END
		END
	FROM  
		TempVoucherAccount TA  
		INNER JOIN TempPersonalInformation TP ON TA.Voucher_Acc_ID = TP.Voucher_Acc_ID  
	WHERE  
		TA.Record_Status NOT IN ('V', 'D')  
		AND TA.Create_Dtm <= @Cutoff_Dtm  
  
	-- ---------------------------------------------  
	-- Special Account  
	-- ---------------------------------------------  
    
	INSERT INTO #statNoOfAcc_byDoc (
		_doc_code,
		_doc_ID,
		_Deceased
	)  
	SELECT  
		SP.Doc_Code,  
		SP.Encrypt_Field1,
		CASE WHEN SP.Deceased IS NULL THEN 'N' ELSE
			CASE WHEN SP.Deceased = 'Y' THEN 'Y' ELSE 'N' END
		END
	FROM  
		SpecialAccount SA  
		INNER JOIN SpecialPersonalInformation SP ON SA.Special_Acc_ID = SP.Special_Acc_ID     
	WHERE  
		SA.Record_Status NOT IN ('V', 'D')  
		AND SA.Create_Dtm <= @Cutoff_Dtm  
  

	-- ---------------------------------------------  
	-- Result
	-- ---------------------------------------------  
     
	
	UPDATE #result_table
	SET _result_value2 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code IN ('HKIC', 'HKBC') AND _Deceased ='N'),
	    _result_value3 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'Doc/I' AND _Deceased ='N'),
		_result_value4 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'REPMT' AND _Deceased ='N'),
		 _result_value5 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'ID235B' AND _Deceased ='N'),
		_result_value6 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'VISA' AND _Deceased ='N'),
		_result_value7 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'ADOPC' AND _Deceased ='N'),
		_result_value8 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'EC' AND _Deceased ='N'),
		_result_value9 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'OC' AND _Deceased ='N'),
		_result_value10 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'OW' AND _Deceased ='N'),
		_result_value11 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'TW' AND _Deceased ='N'),
		_result_value12 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'IR' AND _Deceased ='N'),
		_result_value13 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'HKP' AND _Deceased ='N'),
		_result_value14 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'RFNo8' AND _Deceased ='N'),			
		_result_value15 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'OTHER' AND _Deceased ='N'),
		_result_value16 = '',	-- Total
		_result_value17 = '',
		_result_value18 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'HKIC' AND _Deceased ='N'),
		_result_value19 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'HKBC' AND _Deceased ='N')
	WHERE _display_seq = 14
	
	UPDATE #result_table
	SET _result_value2 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code IN ('HKIC', 'HKBC') AND _Deceased ='Y'),
		_result_value3 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'Doc/I' AND _Deceased ='Y'),
		_result_value4 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'REPMT' AND _Deceased ='Y'),
		 _result_value5 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'ID235B' AND _Deceased ='Y'),
		_result_value6 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'VISA' AND _Deceased ='Y'),
		_result_value7 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'ADOPC' AND _Deceased ='Y'),
		_result_value8 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'EC' AND _Deceased ='Y'),
		_result_value9 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'OC' AND _Deceased ='Y'),
		_result_value10 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'OW' AND _Deceased ='Y'),
		_result_value11 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'TW' AND _Deceased ='Y'),
		_result_value12 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'IR' AND _Deceased ='Y'),
		_result_value13 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'HKP' AND _Deceased ='Y'),
		_result_value14 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'RFNo8' AND _Deceased ='Y'),						
		_result_value15 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'OTHER' AND _Deceased ='Y'),
		_result_value16 = '',	-- Total
		_result_value17 = '',
		_result_value18 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'HKIC' AND _Deceased ='Y'),
		_result_value19 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'HKBC' AND _Deceased ='Y')
	WHERE _display_seq = 15

	UPDATE #result_table
	SET	_result_value2 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code IN ('HKIC', 'HKBC')),
		_result_value3 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'Doc/I'),
		_result_value4 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'REPMT'),
		_result_value5 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'ID235B'),
		_result_value6 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'VISA'),
		_result_value7 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'ADOPC'),
		_result_value8 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'EC'),
		_result_value9 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'OC'),
		_result_value10 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'OW'),
		_result_value11 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'TW'),
		_result_value12 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'IR'),
		_result_value13 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'HKP'),		
		_result_value14 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'RFNo8'),		
		_result_value15 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'OTHER'),
		_result_value16 = '',	-- Total
		_result_value17 = '',
		_result_value18 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'HKIC'),
		_result_value19 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'HKBC')
	WHERE _display_seq = 16

	UPDATE #result_table
	SET _result_value16 = (CONVERT(INT, _result_value2) + CONVERT(INT, _result_value3) + CONVERT(INT, _result_value4) + CONVERT(INT, _result_value5) + 
						CONVERT(INT, _result_value6) + CONVERT(INT, _result_value7) + CONVERT(INT, _result_value8) + CONVERT(INT, _result_value9) + CONVERT(INT, _result_value10) + 
						CONVERT(INT, _result_value11) + CONVERT(INT, _result_value12) + CONVERT(INT, _result_value13) + CONVERT(INT, _result_value14) + CONVERT(INT, _result_value15))
	WHERE _display_seq = 14
	UPDATE #result_table
	SET _result_value16 = (CONVERT(INT, _result_value2) + CONVERT(INT, _result_value3) + CONVERT(INT, _result_value4) + CONVERT(INT, _result_value5) + 
						CONVERT(INT, _result_value6) + CONVERT(INT, _result_value7) + CONVERT(INT, _result_value8) + CONVERT(INT, _result_value9) + CONVERT(INT, _result_value10) + 
						CONVERT(INT, _result_value11) + CONVERT(INT, _result_value12) + CONVERT(INT, _result_value13) + CONVERT(INT, _result_value14) + CONVERT(INT, _result_value15))
	WHERE _display_seq = 15
	UPDATE #result_table
	SET _result_value16 = (CONVERT(INT, _result_value2) + CONVERT(INT, _result_value3) + CONVERT(INT, _result_value4) + CONVERT(INT, _result_value5) + 
						CONVERT(INT, _result_value6) + CONVERT(INT, _result_value7) + CONVERT(INT, _result_value8) + CONVERT(INT, _result_value9) + CONVERT(INT, _result_value10) + 
						CONVERT(INT, _result_value11) + CONVERT(INT, _result_value12) + CONVERT(INT, _result_value13) + CONVERT(INT, _result_value14) + CONVERT(INT, _result_value15))
	WHERE _display_seq = 16
	
  
	--=======================================
	-- (ii) eHealth (Subsidies) accounts with PPPKG claim transactions
	--=======================================
	
	INSERT INTO #result_table (_display_seq)
	VALUES (18)
	INSERT INTO #result_table (_display_seq)
	VALUES (19)
	INSERT INTO #result_table (_display_seq)
	VALUES (20)
			
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (21, '(ii) eHealth (Subsidies) accounts with ' + @Scheme_Display_Code + ' claim transactions since ' + FORMAT(@Claim_Period_From, 'dd MMM yyyy'))

	INSERT INTO #result_table (_display_seq)
	VALUES (22)
	
	INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5,
		_result_value6, _result_value7, _result_value8, _result_value9, _result_value10, _result_value11, _result_value12,
		_result_value13, _result_value14, _result_value15, _result_value16, _result_value17, _result_value18, _result_value19)
	VALUES (23, '','HKIC/HKBC', 'Doc/I', 'REPMT', 'ID235B', 'VISA', 'ADOPC', 'EC', 'OC', 'OW', 'TW', 'IR', 'HKP', 'RFNo8', 'OTHER', 'Total', '', 'HKIC', 'HKBC')

	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (24 ,'Alive')

	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (25 ,'Deceased')

	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (26 ,'Total')

	-- select the voucher transactions included for statistic  
	INSERT INTO #statNoOfAcc_withClaim_byDoc (
			_Voucher_acc_ID,
			_temp_voucher_acc_ID,
			_special_acc_ID,
			_doc_code
	)
	SELECT 
		vTran.Voucher_acc_ID,
		vTran.temp_voucher_acc_ID,
		vTran.special_acc_ID,
		vTran.doc_code
	FROM 
		voucherTransaction vTran 
	WHERE 
		vTran.Scheme_Code = @Scheme_Code
	AND 
		vTran.transaction_dtm <= @Cutoff_Dtm
	AND (
		vTran.Invalidation IS NULL
		OR vTran.Invalidation NOT IN (
			SELECT Status_Value
			FROM StatStatusFilterMapping
			WHERE ( report_id = 'ALL' OR report_id = @Report_ID)
				AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'
				AND (( Effective_Date IS NULL OR Effective_Date <= @cutoff_dtm)
					AND ( Expiry_Date IS NULL OR Expiry_Date >= @cutoff_dtm ))
			)
		)
	AND vTran.Record_Status NOT IN (
		SELECT Status_Value
		FROM StatStatusFilterMapping
		WHERE ( report_id = 'ALL'OR report_id = @Report_ID)
			AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'
			AND (( Effective_Date IS NULL OR Effective_Date <= @cutoff_dtm)
				AND ( Expiry_Date IS NULL OR Expiry_Date >= @cutoff_dtm))
		)



	-- update the DOC ID for validated ACC  
	UPDATE
		#statNoOfAcc_withClaim_byDoc
	SET
		_doc_ID = pInfovACC.[Encrypt_Field1],
		_Deceased = pInfovACC.Deceased
	FROM
		(SELECT	
			pInfo.voucher_acc_ID,
			pInfo.Doc_Code,
			pInfo.[Encrypt_Field1],
			[Deceased] = CASE WHEN pInfo.Deceased IS NULL THEN 'N' ELSE
				CASE WHEN pInfo.Deceased = 'Y' THEN 'Y' ELSE 'N' END
			END
		FROM personalInformation pInfo,
			voucherAccount vACC
		WHERE 
			 vAcc.voucher_acc_ID = pInfo.voucher_acc_ID
			AND vAcc.record_status <> 'D') pInfovACC
	WHERE 
		 pInfovACC.voucher_acc_ID = #statNoOfAcc_withClaim_byDoc._Voucher_acc_ID 
				AND pInfovACC.Doc_Code  = #statNoOfAcc_withClaim_byDoc._Doc_Code
					AND (#statNoOfAcc_withClaim_byDoc._doc_ID IS NULL OR #statNoOfAcc_withClaim_byDoc._doc_ID = '')


	-- update the DOC ID for special ACC  
	UPDATE
		#statNoOfAcc_withClaim_byDoc
	SET
		_doc_ID = sInfovACC.[Encrypt_Field1],
		_Deceased = sInfovACC.Deceased
	FROM
		(SELECT	
			sInfo.special_acc_ID,
			sInfo.Doc_Code,
			sInfo.[Encrypt_Field1],
			[Deceased] = CASE WHEN sInfo.Deceased IS NULL THEN 'N' ELSE
				CASE WHEN sInfo.Deceased = 'Y' THEN 'Y' ELSE 'N' END
			END
		FROM specialpersonalinformation sInfo,
				specialaccount sAcc
			WHERE  sAcc.special_acc_ID = sInfo.special_acc_ID
				AND sAcc.record_status NOT IN ('V', 'D')) sInfovACC
	WHERE 
		 sInfovACC.special_acc_ID = #statNoOfAcc_withClaim_byDoc._special_acc_ID
				AND sInfovACC.Doc_Code = #statNoOfAcc_withClaim_byDoc._Doc_Code
					AND (#statNoOfAcc_withClaim_byDoc._doc_ID IS NULL OR #statNoOfAcc_withClaim_byDoc._doc_ID = '')


	-- update the DOC ID for temp ACC  
	UPDATE
		#statNoOfAcc_withClaim_byDoc
	SET
		_doc_ID = tInfovACC.[Encrypt_Field1],
		_Deceased = tInfovACC.Deceased
	FROM
		(SELECT	
			tInfo.voucher_acc_ID,
			tInfo.Doc_Code,
			tInfo.[Encrypt_Field1],
			[Deceased] = CASE WHEN tInfo.Deceased IS NULL THEN 'N' ELSE
				CASE WHEN tInfo.Deceased = 'Y' THEN 'Y' ELSE 'N' END
			END
		FROM temppersonalinformation tInfo,
				tempvoucheraccount tAcc
			WHERE tAcc.voucher_acc_ID = tInfo.voucher_acc_ID
				AND tAcc.record_status NOT IN ('V', 'D')) tInfovACC
	WHERE 
		tInfovACC.voucher_acc_ID = #statNoOfAcc_withClaim_byDoc._temp_voucher_acc_ID
				AND tInfovACC.Doc_Code = #statNoOfAcc_withClaim_byDoc._Doc_Code
					AND (#statNoOfAcc_withClaim_byDoc._doc_ID IS NULL OR #statNoOfAcc_withClaim_byDoc._doc_ID = '')


	INSERT INTO #statNoOfAcc_withClaim_byDoc_distinct (_doc_code, _doc_ID, _Deceased)
	SELECT DISTINCT _doc_code, _doc_ID, _Deceased
	FROM #statNoOfAcc_withClaim_byDoc 
	WHERE _doc_ID IS NOT NULL AND _doc_ID <> '' AND _Deceased IS NOT NULL



	UPDATE #result_table
	SET _result_value2 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code IN ('HKIC', 'HKBC') AND _Deceased ='N'),
	    _result_value3 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'Doc/I' AND _Deceased ='N'),
		_result_value4 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'REPMT' AND _Deceased ='N'),
		 _result_value5 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'ID235B' AND _Deceased ='N'),
		_result_value6 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'VISA' AND _Deceased ='N'),
		_result_value7 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'ADOPC' AND _Deceased ='N'),
		_result_value8 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'EC' AND _Deceased ='N'),
		_result_value9 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'OC' AND _Deceased ='N'),
		_result_value10 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'OW' AND _Deceased ='N'),
		_result_value11 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'TW' AND _Deceased ='N'),
		_result_value12 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'IR' AND _Deceased ='N'),
		_result_value13 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'HKP' AND _Deceased ='N'),
		_result_value14 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'RFNo8' AND _Deceased ='N'),
		_result_value15 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'OTHER' AND _Deceased ='N'),
		_result_value16 = '',	-- Total
		_result_value17 = '',
		_result_value18 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'HKIC' AND _Deceased ='N'),
		_result_value19 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'HKBC' AND _Deceased ='N')
	WHERE _display_seq = 24

	UPDATE #result_table
	SET _result_value2 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code IN ('HKIC', 'HKBC') AND _Deceased ='Y'),
		_result_value3 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'Doc/I' AND _Deceased ='Y'),
		_result_value4 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'REPMT' AND _Deceased ='Y'),
		 _result_value5 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'ID235B' AND _Deceased ='Y'),
		_result_value6 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'VISA' AND _Deceased ='Y'),
		_result_value7 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'ADOPC' AND _Deceased ='Y'),
		_result_value8 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'EC' AND _Deceased ='Y'),
		_result_value9 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'OC' AND _Deceased ='Y'),
		_result_value10 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'OW' AND _Deceased ='Y'),
		_result_value11 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'TW' AND _Deceased ='Y'),
		_result_value12 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'IR' AND _Deceased ='Y'),
		_result_value13 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'HKP' AND _Deceased ='Y'),
		_result_value14 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'RFNo8' AND _Deceased ='Y'),
		_result_value15 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'OTHER' AND _Deceased ='Y'),
		_result_value16 = '',	-- Total
		_result_value17 = '',
		_result_value18 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'HKIC' AND _Deceased ='Y'),
		_result_value19 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'HKBC' AND _Deceased ='Y')
	WHERE _display_seq = 25

	UPDATE #result_table
	SET	_result_value2 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code IN ('HKIC', 'HKBC')),
		_result_value3 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'Doc/I'),
		_result_value4 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'REPMT'),
		_result_value5 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'ID235B'),
		_result_value6 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'VISA'),
		_result_value7 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'ADOPC'),
		_result_value8 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'EC'),
		_result_value9 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'OC'),
		_result_value10 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'OW'),
		_result_value11 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'TW'),
		_result_value12 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'IR'),
		_result_value13 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'HKP'),
		_result_value14 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'RFNo8'),
		_result_value15 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'OTHER'),
		_result_value16 = '',	-- Total
		_result_value17 = '',
		_result_value18 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'HKIC'),
		_result_value19 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_withClaim_byDoc_distinct
			WHERE _doc_code = 'HKBC')
	WHERE _display_seq = 26

	UPDATE #result_table
	SET _result_value16 = (CONVERT(INT, _result_value2) + CONVERT(INT, _result_value3) + CONVERT(INT, _result_value4) + CONVERT(INT, _result_value5) + 
						CONVERT(INT, _result_value6) + CONVERT(INT, _result_value7) + CONVERT(INT, _result_value8) + CONVERT(INT, _result_value9) + CONVERT(INT, _result_value10) + 
						CONVERT(INT, _result_value11) + CONVERT(INT, _result_value12) + CONVERT(INT, _result_value13) + CONVERT(INT, _result_value14) + CONVERT(INT, _result_value15))
	WHERE _display_seq = 24
	UPDATE #result_table
	SET _result_value16 = (CONVERT(INT, _result_value2) + CONVERT(INT, _result_value3) + CONVERT(INT, _result_value4) + CONVERT(INT, _result_value5) + 
						CONVERT(INT, _result_value6) + CONVERT(INT, _result_value7) + CONVERT(INT, _result_value8) + CONVERT(INT, _result_value9) + CONVERT(INT, _result_value10) + 
						CONVERT(INT, _result_value11) + CONVERT(INT, _result_value12) + CONVERT(INT, _result_value13) + CONVERT(INT, _result_value14) + CONVERT(INT, _result_value15))
	WHERE _display_seq = 25
	UPDATE #result_table
	SET _result_value16 = (CONVERT(INT, _result_value2) + CONVERT(INT, _result_value3) + CONVERT(INT, _result_value4) + CONVERT(INT, _result_value5) + 
						CONVERT(INT, _result_value6) + CONVERT(INT, _result_value7) + CONVERT(INT, _result_value8) + CONVERT(INT, _result_value9) + CONVERT(INT, _result_value10) + 
						CONVERT(INT, _result_value11) + CONVERT(INT, _result_value12) + CONVERT(INT, _result_value13) + CONVERT(INT, _result_value14) + CONVERT(INT, _result_value15))
	WHERE _display_seq = 26
		
	--===================================
	-- Retrieve the final result  
	--===================================
	DELETE FROM RpteHSD0033_01_PPPKG_eHA_ByDocType
	
	INSERT INTO RpteHSD0033_01_PPPKG_eHA_ByDocType (
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
		Col11,
		Col12,
		Col13,
		Col14,
		Col15,
		Col16,
		Col17,
		Col18,
		Col19,
		Col20
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
		_result_value11,
		_result_value12,
		_result_value13,
		_result_value14,
		_result_value15,
		_result_value16,
		_result_value17,
		_result_value18,
		_result_value19,
		_result_value20
	FROM #result_table
	ORDER BY _display_seq


	DROP TABLE #statNoOfAcc_byDoc
	
	DROP TABLE #statNoOfAcc_withClaim_byDoc

	DROP TABLE #statNoOfAcc_withClaim_byDoc_distinct

	DROP TABLE #result_table
	
END
GO


GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0033_01_PrepareData] TO HCVU
GO


