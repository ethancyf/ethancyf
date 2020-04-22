IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSM0008_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSM0008_Report_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================    
-- Modification History    
-- Modified by:      
-- Modified date:     
-- Description:      
-- =============================================    
-- =============================================    
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Author:			Dickson Law
-- Create date:		12 Jan 2018
-- Description:		Get eHealth (Subsidies) Accounts with pneumococcal claim transactions (VSS/EVSS/OMPCV13E/RVP)
-- =============================================  

CREATE PROCEDURE [proc_EHS_eHSM0008_Report_get]
	@Cutoff_Dtm datetime = NULL -- The date to gen report
AS BEGIN
	
SET NOCOUNT ON;

-- =============================================  
-- Declaration  
-- =============================================  
	IF @Cutoff_Dtm IS NULL BEGIN
		SET @Cutoff_Dtm = CONVERT(VARCHAR(10), GETDATE(), 120)
	END

	DECLARE @AGE			INT	=65
	DECLARE @SCHEMEGROUP1	INT	=1
	DECLARE @SCHEMEGROUP2	INT	=2
	DECLARE @SCHEME1	VARCHAR(10)	='VSS'
	DECLARE @SCHEME2	VARCHAR(10)	='EVSS'
	DECLARE @SCHEME3	VARCHAR(10)	='OMPCV13E'
	DECLARE @SCHEME4	VARCHAR(10)	='RVP'
	
	DECLARE @TargetScheme TABLE (
		SchemeGroup     INT,
		Scheme			VARCHAR(10)
	)
						
	CREATE TABLE #statNoOfAcc_withPVClaim_byDoc (
		_transaction_Dtm			datetime,
		_service_Receive_Dtm		datetime,
		_voucher_acc_ID			CHAR(15),
		_temp_voucher_acc_ID	CHAR(15),
		_special_acc_ID			CHAR(15),
		_doc_code				CHAR(20),
		_doc_ID					VARBINARY(100),
		_dob_group				INT,
		_scheme_group			INT,
		_PV						INT,
		_PV13					INT,
		_deceased				CHAR(1)
	)

	CREATE TABLE #InjectedPV_byDoc (
		_doc_ID					VARBINARY(100),
		_doc_code				CHAR(20),
		_scheme_group			INT,
		_PV						INT,
		_PV13					INT
	)

	CREATE TABLE #statNoOfAcc_withPVClaim__byDoc_distinct_group (
		_service_rank   INT,
		_doc_ID			VARBINARY(100),
		_doc_code		CHAR(20),
		_dob_group		INT,
		_PV				INT,
		_PV13			INT,
		_scheme_group	INT,
		_deceased		CHAR(1)
	)

	CREATE TABLE #statNoOfAcc_withPVClaim__byDoc_distinct_all (
		_service_rank   INT,
		_doc_ID			VARBINARY(100),
		_doc_code		CHAR(20),
		_dob_group		INT,
		_PV				INT,
		_PV13			INT,
		_deceased		CHAR(1)
	)
	
	CREATE TABLE #result_table (
		_display_seq	TINYINT,
		_result_value1	VARCHAR(200) DEFAULT '',
		_result_value2	VARCHAR(100) DEFAULT '',
		_result_value3	VARCHAR(100) DEFAULT '',
		_result_value4	VARCHAR(100) DEFAULT ''
	)
	
-- =============================================  
-- Validation   
-- =============================================  
-- =============================================  
-- Initialization  
-- =============================================  

	-- Create Scheme Group 
	INSERT INTO @TargetScheme (
		SchemeGroup,
		Scheme
	)
	VALUES
	(@SCHEMEGROUP1,@SCHEME1),
	(@SCHEMEGROUP1,@SCHEME2),
	(@SCHEMEGROUP1,@SCHEME3),
	(@SCHEMEGROUP2,@SCHEME4)


	-- insert record for the final output format  
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (0, 'eHS(S)M0008-01: Report on eHealth (Subsidies) Accounts with pneumococcal claim transactions')
	
	INSERT INTO #result_table (_display_seq)
	VALUES (1)
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (2, 'Reporting period: as at ' + CONVERT(VARCHAR, DATEADD(dd, - 1, @Cutoff_Dtm), 111))
	
	INSERT INTO #result_table (_display_seq)
	VALUES (3)
	
	-- (1) VSS/EVSS/OMPCV13E
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (11, '(1) '+@SCHEME1+'/'+@SCHEME2+'/'+@SCHEME3)
	
	INSERT INTO #result_table (_display_seq, _result_value1, _result_value2)
	VALUES (12, '' ,'eHealth (Subsidies) Account')
	
	INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4)
	VALUES (13, '','Alive', 'Deceased', 'Total')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (14 ,'a. Injected 23vPPV only')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (15 ,'b. Injected PCV13 only')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (16 ,'c. Injected both 23vPPV and PCV13')
	
	INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4)
	VALUES (17 ,'' ,'','','')
	
	INSERT INTO #result_table (_display_seq)
	VALUES (18)

	-- (2)  VSS/EVSS/OMPCV13E/RVP
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (21, '(2) '+@SCHEME4)
	
	INSERT INTO #result_table (_display_seq, _result_value1, _result_value2)
	VALUES (22, '' ,'eHealth (Subsidies) Account')
	
	INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4)
	VALUES (23, '','Alive', 'Deceased', 'Total')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (24 ,'a. Injected 23vPPV only')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (25 ,'age >=65*')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (26 ,'age < 65*')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (27 ,'b. Injected PCV13 only')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (28 ,'age >=65*')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (29 ,'age < 65*')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (30 ,'c. Injected both 23vPPV and PCV13')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (31 ,'age >=65*')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (32 ,'age < 65*')
	
	INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4)
	VALUES (33 ,'','','','')
	
	INSERT INTO #result_table (_display_seq)
	VALUES (34 )

	-- (3) RVP
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (41, '(3) eHS(S) (either injected under '+ @SCHEME1+'/'+@SCHEME2+'/'+@SCHEME4+'/'+@SCHEME3+')')
	
	INSERT INTO #result_table (_display_seq, _result_value1, _result_value2)
	VALUES (42, '' ,'eHealth (Subsidies) Account')
	
	INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4)
	VALUES (43, '','Alive', 'Deceased', 'Total')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (44 ,'a. Injected 23vPPV only')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (45 ,'age >=65*')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (46 ,'age < 65*')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (47 ,'b. Injected PCV13 only')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (48 ,'age >=65*')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (49 ,'age < 65*')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (50 ,'c. Injected both 23vPPV and PCV13')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (51 ,'age >=65*')
	
	INSERT INTO #result_table (_display_seq, _result_value1)
	VALUES (52 ,'age < 65*')
	
	INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4)
	VALUES (53 ,'','','','')
	
	-- Get Transaction with 23vPPV and PCV13
	INSERT INTO #statNoOfAcc_withPVClaim_byDoc (
		_transaction_Dtm		
		,_service_Receive_Dtm
		,_voucher_acc_ID
		,_temp_voucher_acc_ID
		,_special_acc_ID		
		,_doc_code		
		,_scheme_group			
		,_PV
		,_PV13
	)
	SELEct
		VT.Transaction_Dtm
		,VT.Service_Receive_Dtm
		,VT.Voucher_Acc_ID
		,VT.Temp_Voucher_Acc_ID
		,VT.special_acc_ID
		,VT.Doc_Code
		,TS.SchemeGroup
		,CASE WHEN Subsidize_Item_Code = 'PV' THEN 1 ELSE 0 END
		,CASE WHEN Subsidize_Item_Code = 'PV13' THEN 1 ELSE 0 END
		FROM VoucherTransaction VT    
			INNER JOIN TransactionDetail D    
			ON VT.Transaction_ID = D.Transaction_ID 
			INNER JOIN @TargetScheme TS   
			ON TS.Scheme = D.Scheme_Code   
		WHERE D.Subsidize_Item_Code IN ('PV','PV13')
		AND 
			VT.transaction_dtm <= @Cutoff_Dtm
	
		AND (
			VT.Invalidation IS NULL
			OR VT.Invalidation NOT IN (
				SELECT Status_Value
				FROM StatStatusFilterMapping
				WHERE ( report_id = 'ALL' OR report_id = 'M0008')
					AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'
					AND (( Effective_Date IS NULL OR Effective_Date <= @Cutoff_Dtm)
						AND ( Expiry_Date IS NULL OR Expiry_Date >= @Cutoff_Dtm ))
				)
			)
		AND VT.Record_Status NOT IN (
			SELECT Status_Value
			FROM StatStatusFilterMapping
			WHERE ( report_id = 'ALL'OR report_id = 'M0008')
				AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'
				AND (( Effective_Date IS NULL OR Effective_Date <= @Cutoff_Dtm)
				AND ( Expiry_Date IS NULL OR Expiry_Date >= @Cutoff_Dtm))
			)
		AND VT.Scheme_Code IN (SELECT Scheme FROM @TargetScheme)
	
	
	-- update the DOC ID, DOB group, deceased for validated ACC  
	-- DOB GROUP 1: age >=65  2: age < 65
	UPDATE
		#statNoOfAcc_withPVClaim_byDoc
	SET
		_doc_ID = pInfovACC.[Encrypt_Field1],
		_dob_group = 
		CASE WHEN YEAR(GETDATE()) - YEAR(pInfovACC.DOB) >= @AGE
			THEN 1
			ELSE 2 
		END,
		_deceased = pInfovACC.Deceased
	FROM
		(SELECT	
			pInfo.voucher_acc_ID,
			pInfo.Doc_Code,
			pInfo.[Encrypt_Field1],
			pInfo.DOB,
			[Deceased] = CASE WHEN pInfo.Deceased IS NULL THEN 'N' ELSE
				CASE WHEN pInfo.Deceased = 'Y' THEN 'Y' ELSE 'N' END
			END
		FROM personalInformation pInfo,
			voucherAccount vACC
		WHERE 
			 vAcc.voucher_acc_ID = pInfo.voucher_acc_ID
			AND vAcc.record_status <> 'D') pInfovACC
	WHERE 
		 pInfovACC.voucher_acc_ID = #statNoOfAcc_withPVClaim_byDoc._voucher_acc_ID 
				AND pInfovACC.Doc_Code  = #statNoOfAcc_withPVClaim_byDoc._Doc_Code
					AND (#statNoOfAcc_withPVClaim_byDoc._doc_ID IS NULL OR #statNoOfAcc_withPVClaim_byDoc._doc_ID = '')
	
	-- update the DOC ID, DOB group, deceased for special ACC 
	-- DOB GROUP 1: age >=65  2: age < 65
	UPDATE
		#statNoOfAcc_withPVClaim_byDoc
	SET
		_doc_ID = sInfovACC.[Encrypt_Field1],
		_dob_group = 
		CASE WHEN YEAR(GETDATE()) - YEAR(sInfovACC.DOB) >= @AGE
			THEN 1
			ELSE 2 
		END,
		_deceased = sInfovACC.Deceased
	FROM
		(SELECT	
			sInfo.special_acc_ID,
			sInfo.Doc_Code,
			sInfo.[Encrypt_Field1],
			sInfo.DOB,
			[Deceased] = CASE WHEN sInfo.Deceased IS NULL THEN 'N' ELSE
				CASE WHEN sInfo.Deceased = 'Y' THEN 'Y' ELSE 'N' END
			END
		FROM specialpersonalinformation sInfo,
				specialaccount sAcc
			WHERE  sAcc.special_acc_ID = sInfo.special_acc_ID
				AND sAcc.record_status NOT IN ('V', 'D')) sInfovACC
	WHERE 
		 sInfovACC.special_acc_ID = #statNoOfAcc_withPVClaim_byDoc._special_acc_ID
				AND sInfovACC.Doc_Code = #statNoOfAcc_withPVClaim_byDoc._Doc_Code
					AND (#statNoOfAcc_withPVClaim_byDoc._doc_ID IS NULL OR #statNoOfAcc_withPVClaim_byDoc._doc_ID = '')
	
	-- update the DOC ID, DOB group, deceased for temp ACC 
	-- DOB GROUP 1: age >=65  2: age < 65
	UPDATE
		#statNoOfAcc_withPVClaim_byDoc
	SET
		_doc_ID = tInfovACC.[Encrypt_Field1],
		_dob_group = 
		CASE WHEN YEAR(GETDATE()) - YEAR(tInfovACC.DOB) >= @AGE
			THEN 1
			ELSE 2 
		END,
		_deceased = tInfovACC.Deceased
	FROM
		(SELECT	
			tInfo.voucher_acc_ID,
			tInfo.Doc_Code,
			tInfo.[Encrypt_Field1],
			tInfo.DOB,
			[Deceased] = CASE WHEN tInfo.Deceased IS NULL THEN 'N' ELSE
				CASE WHEN tInfo.Deceased = 'Y' THEN 'Y' ELSE 'N' END
			END
		FROM temppersonalinformation tInfo,
				tempvoucheraccount tAcc
			WHERE tAcc.voucher_acc_ID = tInfo.voucher_acc_ID
				AND tAcc.record_status NOT IN ('V', 'D')) tInfovACC
	WHERE 
		tInfovACC.voucher_acc_ID = #statNoOfAcc_withPVClaim_byDoc._temp_voucher_acc_ID
				AND tInfovACC.Doc_Code = #statNoOfAcc_withPVClaim_byDoc._Doc_Code
					AND (#statNoOfAcc_withPVClaim_byDoc._doc_ID IS NULL OR #statNoOfAcc_withPVClaim_byDoc._doc_ID = '')		
	
	-- Patch all HKBC to HKIC
	---- HKIC and HKBC with same identity document no. will be counted as same account.
	update #statNoOfAcc_withPVClaim_byDoc 
	SET _doc_code = 'HKIC'
	WHERE _doc_code = 'HKBC'


	-- Get all ID injected PV in different scheme group
	INSERT INTO #InjectedPV_byDoc (
		_doc_ID,	
		_doc_code,						
		_scheme_group,	
		_PV,				
		_PV13			
	)
	SELECT DISTINCT 
		_doc_ID,
		_doc_code,
		_scheme_group,
		MAX(_PV) AS PV,
		MAX(_PV13)  AS PV13
	FROM #statNoOfAcc_withPVClaim_byDoc
	WHERE _scheme_group = @SCHEMEGROUP1 AND _doc_code = 'HKIC'
	GROUp BY  _doc_ID,_doc_code,_scheme_group

	INSERT INTO #InjectedPV_byDoc (
		_doc_ID,	
		_doc_code,				
		_scheme_group,	
		_PV,				
		_PV13			
	)
	SELECT DISTINCT 
		_doc_ID,
		_doc_code,
		_scheme_group,
		MAX(_PV) AS PV,
		MAX(_PV13)  AS PV13
	FROM #statNoOfAcc_withPVClaim_byDoc
	WHERE _scheme_group = @SCHEMEGROUP2 AND _doc_code = 'HKIC'
	GROUp BY  _doc_ID,_doc_code,_scheme_group

	INSERT INTO #InjectedPV_byDoc (
		_doc_ID,	
		_doc_code,				
		_scheme_group,	
		_PV,				
		_PV13			
	)
	SELECT DISTINCT 
		_doc_ID,
		_doc_code,
		_scheme_group,
		MAX(_PV) AS PV,
		MAX(_PV13)  AS PV13
	FROM #statNoOfAcc_withPVClaim_byDoc
	WHERE _scheme_group = @SCHEMEGROUP1 AND _doc_code <> 'HKIC'
	GROUp BY  _doc_ID,_doc_code,_scheme_group
	
	INSERT INTO #InjectedPV_byDoc (
		_doc_ID,	
		_doc_code,					
		_scheme_group,	
		_PV,				
		_PV13			
	)
	SELECT DISTINCT 
		_doc_ID,
		_doc_code,
		_scheme_group,
		MAX(_PV) AS PV,
		MAX(_PV13)  AS PV13
	FROM #statNoOfAcc_withPVClaim_byDoc
	WHERE _scheme_group = @SCHEMEGROUP2 AND _doc_code <> 'HKIC'
	GROUp BY  _doc_ID,_doc_code,_scheme_group

	
	-- Table for (1) VSS/EVSS/OMPCV13E and (2) RVP

	--Distinct DOC ID for HKIC								
	INSERT INTO #statNoOfAcc_withPVClaim__byDoc_distinct_group (
		_service_rank
		,_doc_ID			
		,_doc_code		
		,_dob_group			
		,_scheme_group
		,_PV
		,_PV13
		,_deceased		
	)	
	SELECT
		RANK() OVER(
			PARTITION BY
				SAPV._doc_Code,
				SAPV._doc_ID,
				SAPV._scheme_group
			ORDER BY
				SAPV._service_Receive_Dtm, SAPV._transaction_Dtm
		) AS _service_rank,
		SAPV._doc_ID,
		SAPV._doc_code,
		SAPV._dob_group,
		SAPV._scheme_group,
		IPV._PV,
		IPV._PV13,
		SAPV._deceased	
	FROM #statNoOfAcc_withPVClaim_byDoc SAPV
	INNER JOIN #InjectedPV_byDoc IPV 
		ON SAPV._doc_code = IPV._doc_code 
			AND SAPV._doc_ID = IPV._doc_ID	
				AND SAPV._scheme_group	= IPV._scheme_group
	WHERE SAPV._doc_code = 'HKIC' 

	--Distinct DOC ID for other doc code
	INSERT INTO #statNoOfAcc_withPVClaim__byDoc_distinct_group (
		_service_rank
		,_doc_ID			
		,_doc_code		
		,_dob_group			
		,_scheme_group
		,_PV
		,_PV13
		,_deceased		
	)	
	SELECT
		RANK() OVER(
			PARTITION BY
				SAPV._doc_Code,
				SAPV._doc_ID,
				SAPV._scheme_group
			ORDER BY
				SAPV._service_Receive_Dtm, SAPV._transaction_Dtm
		) AS _service_rank,
		SAPV._doc_ID,
		SAPV._doc_code,
		SAPV._dob_group,
		SAPV._scheme_group,
		IPV._PV,
		IPV._PV13,
		SAPV._deceased	
	FROM #statNoOfAcc_withPVClaim_byDoc SAPV
	INNER JOIN #InjectedPV_byDoc IPV 
		ON SAPV._doc_code = IPV._doc_code 
			AND SAPV._doc_ID = IPV._doc_ID	
				AND SAPV._scheme_group	= IPV._scheme_group
	WHERE SAPV._doc_code <> 'HKIC'

	--Delete injected 23vPPv / PCV13 with same docID and doc_code which is not the first record 
	DELETE
		#statNoOfAcc_withPVClaim__byDoc_distinct_group
	WHERE
		_service_rank <> 1

	-- Table for (3) eHS(S) (either injected under VSS/EVSS/RVP/OMPCV13E)
	INSERT INTO #statNoOfAcc_withPVClaim__byDoc_distinct_all
	(
	_service_rank
	,_doc_ID			
	,_doc_code		
	,_dob_group
	,_PV
	,_PV13
	,_deceased		
	)	
	SELECT
		RANK() OVER(
			PARTITION BY
				SAPV._doc_Code,
				SAPV._doc_ID
			ORDER BY
				SAPV._service_Receive_Dtm, SAPV._transaction_Dtm
		) AS _service_rank,
		SAPV._doc_ID,
		SAPV._doc_code,
		SAPV._dob_group,
		IPV._PV,
		IPV._PV13,
		SAPV._deceased	
	FROM #statNoOfAcc_withPVClaim_byDoc SAPV
	INNER JOIN 
	(
	SELECT DISTINCT _doc_ID, _doc_code,MAX(_PV)AS _PV,MAX(_PV13) AS _PV13 FROM #InjectedPV_byDoc
		GROUP BY  _doc_ID, _doc_code
	) IPV 
		ON SAPV._doc_code = IPV._doc_code 
			AND SAPV._doc_ID = IPV._doc_ID	
	WHERE SAPV._doc_code = 'HKIC'

	-- Table for (3) eHS(S) (either injected under VSS/EVSS/RVP/OMPCV13E)
	INSERT INTO #statNoOfAcc_withPVClaim__byDoc_distinct_all
	(
	_service_rank
	,_doc_ID			
	,_doc_code		
	,_dob_group
	,_PV
	,_PV13
	,_deceased		
	)	
	SELECT
		RANK() OVER(
			PARTITION BY
				SAPV._doc_Code,
				SAPV._doc_ID
			ORDER BY
				SAPV._service_Receive_Dtm, SAPV._transaction_Dtm
		) AS _service_rank,
		SAPV._doc_ID,
		SAPV._doc_code,
		SAPV._dob_group,
		IPV._PV,
		IPV._PV13,
		SAPV._deceased	
	FROM #statNoOfAcc_withPVClaim_byDoc SAPV
	INNER JOIN 
	(
	SELECT DISTINCT _doc_ID, _doc_code,MAX(_PV)AS _PV,MAX(_PV13) AS _PV13 FROM #InjectedPV_byDoc
		GROUP BY  _doc_ID, _doc_code
	) IPV 
		ON SAPV._doc_code = IPV._doc_code 
			AND SAPV._doc_ID = IPV._doc_ID	
	WHERE SAPV._doc_code <> 'HKIC'

	-- Delete injected 23vPPv / PCV13 with same docID and doc_code which is not the first record 	
	DELETE
		#statNoOfAcc_withPVClaim__byDoc_distinct_all
	WHERE
		_service_rank <> 1


-- =============================================    
-- Prepare Data
-- =============================================  

	-- (1) VSS/EVSS/OMPCV13E

	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =1 AND _PV13 = 0) AND  _scheme_group = 1 AND _deceased ='N')
		,_result_value3 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =1 AND _PV13 = 0) AND _scheme_group = 1 AND _deceased ='Y')
		,_result_value4 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =1 AND _PV13 = 0) AND _scheme_group = 1)
	WHERE _display_seq = 14
	
	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =0 AND _PV13 = 1) AND  _scheme_group = 1 AND _deceased ='N')
		,_result_value3 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =0 AND _PV13 = 1) AND _scheme_group = 1 AND _deceased ='Y')
		,_result_value4 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =0 AND _PV13 = 1) AND _scheme_group = 1)
	WHERE _display_seq = 15
	
	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =1 AND _PV13 = 1) AND  _scheme_group = 1 AND _deceased ='N')
		,_result_value3 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =1 AND _PV13 = 1) AND _scheme_group = 1 AND _deceased ='Y')
		,_result_value4 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =1 AND _PV13 = 1) AND _scheme_group = 1)
	WHERE _display_seq = 16
	
	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE  _scheme_group =1 AND _deceased ='N')
		,_result_value3 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE _scheme_group = 1 AND _deceased ='Y')
		,_result_value4 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE _scheme_group = 1)
	WHERE _display_seq = 17
	
	
	-- (2) RVP

	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =1 AND _PV13 = 0) AND  _scheme_group = 2 AND _deceased ='N')
		,_result_value3 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =1 AND _PV13 = 0) AND _scheme_group = 2 AND _deceased ='Y')
		,_result_value4 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =1 AND _PV13 = 0) AND _scheme_group = 2)
	WHERE _display_seq = 24
	
	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =1 AND _PV13 = 0) AND  _scheme_group = 2 AND _deceased ='N' AND _dob_group =1)
	WHERE _display_seq = 25
	
	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =1 AND _PV13 = 0) AND  _scheme_group = 2 AND _deceased ='N' AND _dob_group =2)
	WHERE _display_seq = 26
		
	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =0 AND _PV13 = 1) AND  _scheme_group = 2 AND _deceased ='N')
		,_result_value3 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =0 AND _PV13 = 1) AND _scheme_group = 2 AND _deceased ='Y')
		,_result_value4 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =0 AND _PV13 = 1) AND _scheme_group = 2)
	WHERE _display_seq = 27
	
	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =0 AND _PV13 = 1) AND  _scheme_group = 2 AND _deceased ='N' AND _dob_group =1)
	WHERE _display_seq = 28
	
	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =0 AND _PV13 = 1) AND  _scheme_group = 2 AND _deceased ='N' AND _dob_group =2)
	WHERE _display_seq = 29
	
	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =1 AND _PV13 = 1) AND  _scheme_group = 2 AND _deceased ='N')
		,_result_value3 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =1 AND _PV13 = 1) AND _scheme_group = 2 AND _deceased ='Y')
		,_result_value4 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =1 AND _PV13 = 1) AND _scheme_group = 2)
	WHERE _display_seq = 30
	
	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =1 AND _PV13 = 1) AND  _scheme_group = 2 AND _deceased ='N' AND _dob_group =1)
	WHERE _display_seq = 31
	
	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE (_PV =1 AND _PV13 = 1) AND  _scheme_group = 2 AND _deceased ='N' AND _dob_group =2)
	WHERE _display_seq = 32
	
	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE  _scheme_group = 2 AND _deceased ='N')
		,_result_value3 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE _scheme_group = 2 AND _deceased ='Y')
		,_result_value4 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_group WHERE _scheme_group = 2)
	WHERE _display_seq = 33



	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_all WHERE ( _PV =1 AND _PV13 = 0)  AND _deceased ='N')
		,_result_value3 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_all WHERE ( _PV =1 AND _PV13 = 0)  AND _deceased ='Y')
		,_result_value4 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_all WHERE ( _PV =1 AND _PV13 = 0))
	WHERE _display_seq = 44


	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_all WHERE ( _PV =1 AND _PV13 = 0)  AND _deceased ='N' AND _dob_group =1)
	WHERE _display_seq = 45
	
	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_all WHERE ( _PV =1 AND _PV13 = 0)  AND _deceased ='N' AND _dob_group =2)
	WHERE _display_seq = 46
	
	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_all WHERE ( _PV =0 AND _PV13 = 1)  AND _deceased ='N')
		,_result_value3= (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_all WHERE ( _PV =0 AND _PV13 = 1)  AND _deceased ='Y')
		,_result_value4 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_all WHERE ( _PV =0 AND _PV13 = 1))
	WHERE _display_seq = 47
	
	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_all WHERE ( _PV =0 AND _PV13 = 1) AND _deceased ='N' AND _dob_group =1)
	WHERE _display_seq = 48
	
	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_all WHERE ( _PV =0 AND _PV13 = 1) AND _deceased ='N' AND _dob_group =2)
	WHERE _display_seq = 49
	
	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_all WHERE ( _PV =1 AND _PV13 = 1)  AND _deceased ='N')
		,_result_value3 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_all WHERE ( _PV =1 AND _PV13 = 1)  AND _deceased ='Y')
		,_result_value4 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_all WHERE ( _PV =1 AND _PV13 = 1))
	WHERE _display_seq = 50
	
	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_all WHERE ( _PV =1 AND _PV13 = 1) AND _deceased ='N' AND _dob_group =1)
	WHERE _display_seq = 51
	
	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_all WHERE ( _PV =1 AND _PV13 = 1) AND _deceased ='N' AND _dob_group =2)
	WHERE _display_seq = 52
		
	UPDATE #result_table
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_all WHERE _deceased ='N')
		,_result_value3 = (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_all WHERE _deceased ='Y')
		,_result_value4= (SELECT COUNT(1) FROM #statNoOfAcc_withPVClaim__byDoc_distinct_all)
	WHERE _display_seq = 53
	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key


-- =============================================
-- Return results
-- =============================================	
			
	--------------------------
	-- Result Set 1: Content
	--------------------------
	SELECT	
		'Report Generation Time: ' + CONVERT(VARCHAR, GETDATE(), 111) + ' ' + CONVERT(VARCHAR(5), GETDATE(), 108) AS Result_Value
	
	--------------------------
	-- Result Set 2: 01
	--------------------------

	SELECT 
		_result_value1,
		_result_value2,
		_result_value3,
		_result_value4 
	FROM 
		#result_table
	

	DROP TABLE #statNoOfAcc_withPVClaim_byDoc

	DROP TABLE #InjectedPV_byDoc
	
	DROP TABLE #statNoOfAcc_withPVClaim__byDoc_distinct_group

	DROP TABLE #statNoOfAcc_withPVClaim__byDoc_distinct_all
	
	DROP TABLE #result_table

END
GO


GRANT EXECUTE ON [dbo].[proc_EHS_eHSM0008_Report_get] TO HCVU
GO


