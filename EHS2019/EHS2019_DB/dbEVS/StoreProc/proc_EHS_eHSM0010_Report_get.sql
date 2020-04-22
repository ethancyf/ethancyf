IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSM0010_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSM0010_Report_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- ==============================================
-- Modification History
-- Modified by:		Winnie SUEN	
-- Modified date:	11 Sep 2019
-- CR No.			CRE19-006 (DHC)
-- Description:		Display column [DHC-related Services]
-- ===============================================
-- =============================================
-- Modification History
-- CR No.:			CRE18-020 (HKIC Symbol Others)
-- Modified by:		Winnie SUEN
-- Modified date:	25 Feb 2019
-- Description:		Add HKIC Symbol 'Others'
-- =============================================
-- =============================================
-- Author:			Winnie SUEN
-- Create date:		24 Sep 2018
-- CR No.:			CRE17-010 (OCSSS integration)
-- Description:		Get OCSSS checking result for eHA who made/intended to make voucher claim
-- =============================================

CREATE PROCEDURE [proc_EHS_eHSM0010_Report_get]
	@request_dtm			DATETIME = null,		-- The reference date to get @target_period_from and @target_period_to
	@target_period_from		DATETIME = null,		-- The Target Period From. If defined, it will override the value from the @request_dtm
	@target_period_to		DATETIME = null			-- The Target Period To. If defined, it will override the value from the @request_dtm
AS BEGIN
	
SET NOCOUNT ON;

--SET @target_period_from = '2019-11-01'
--SET @target_period_to = '2019-12-01'
-- =============================================  
-- Declaration  
-- =============================================  

	-- Date Period
	IF @request_dtm is null
		SET @request_dtm = GETDATE()

	-- First Day of Last Month, ensure the time start from 00:00 (datetime compare logic use ">=")
	IF @target_period_from is null
		SET @target_period_from = CONVERT(datetime, CONVERT(varchar(10), DATEADD(MONTH, DATEDIFF(MONTH, 0, @request_dtm)-1, 0), 105), 105)
	ELSE
		SET @target_period_from = CONVERT(datetime, CONVERT(varchar(10), @target_period_from, 105), 105)

	-- Last Day of Last Month, ensure the time start from 00:00 (datetime compare logic use "<", so should be First Day of Current Month)
	IF @target_period_to is null
		SET @target_period_to = CONVERT(datetime, CONVERT(varchar(10), DATEADD(MONTH, DATEDIFF(MONTH, 0, @request_dtm), 0), 105), 105)
	ELSE
		SET @target_period_to = CONVERT(datetime, CONVERT(varchar(10), @target_period_to, 105), 105)

	-- 
	DECLARE @Report_ID	VARCHAR(30)	= 'eHSM0010'
	
	DECLARE @Subsidize_Type CHAR(10) = 'VOUCHER'

	DECLARE @TargetScheme TABLE (
		Scheme			VARCHAR(10)
	)
	

	CREATE TABLE #VT (
		Transaction_ID			char(20),
		Transaction_Dtm			datetime,
		Voucher_Acc_ID			char(15),
		Temp_Voucher_Acc_ID		char(15),
		Scheme_Code				char(10),
		Service_Receive_Dtm		datetime,		
		doc_code				char(20),
		Encrypt_Field1			varbinary(100),
		Encrypt_Field2			varbinary(100),
		Encrypt_Field11			varbinary(100),
		SP_ID					char(8),	
		Practice_Display_Seq	smallint,
		Transaction_Status		char(1),
		Reimbursement_Status	char(1),
		Claim_Amount			int,		
		SourceApp				varchar(10),	
		HKIC_Symbol				char(1),-- A/C/R/U
		OCSSS_Ref_Status		char(1),	-- V: Valid; I: Invalid; C: Connection Fail; N: Turn Off OCSSS Checking,  C & N consider as Fail
		DHC_Service				char(1)
	)
		
	CREATE TABLE #VT_ConnectFail (
		Transaction_ID			char(20),
		Transaction_Dtm			datetime,
		Voucher_Acc_ID			char(15),
		Temp_Voucher_Acc_ID		char(15),
		Scheme_Code				char(10),
		Service_Receive_Dtm		datetime,		
		doc_code				CHAR(20),
		Encrypt_Field1			VARBINARY(100),
		Encrypt_Field2			VARBINARY(100),
		Encrypt_Field11			VARBINARY(100),
		SP_ID					char(8),	
		Practice_Display_Seq	smallint,
		Transaction_Status		char(1),
		Reimbursement_Status	char(1),
		Claim_Amount			int,		
		SourceApp				varchar(10),	
		HKIC_Symbol				CHAR(1),
		OCSSS_Ref_Status		CHAR(1),
		DHC_Service				char(1)	
	)

	CREATE TABLE #OCSSS_SearchResult (
		HKIC_Symbol			char(1),	-- A/C/R/U
		OCSSS_Ref_Status	char(1),	-- V: Valid; I: Invalid; C: Connection Fail; N: Turn Off OCSSS Checking,  C & N consider as Fail
		Encrypt_Field1		varbinary(100)
	)

	-- Result Table
	DECLARE @ResultTable_01 AS TABLE(
		Result_Seq int identity(1,1),			-- Sorting Sequence
		Result_Value1 varchar(200) DEFAULT '',	
		Result_Value2 varchar(100) DEFAULT '',	
		Result_Value3 varchar(100) DEFAULT '',	
		Result_Value4 varchar(100) DEFAULT '',	
		Result_Value5 varchar(100) DEFAULT ''
	)

	DECLARE @ResultTable_02 AS TABLE(
		Result_Seq int identity(1,1),			-- Sorting Sequence
		Result_Value1 varchar(200) DEFAULT '',	
		Result_Value2 varchar(100) DEFAULT '',	
		Result_Value3 varchar(100) DEFAULT '',	
		Result_Value4 varchar(100) DEFAULT '',	
		Result_Value5 varchar(100) DEFAULT '',
		Result_Value6 varchar(100) DEFAULT '',	
		Result_Value7 varchar(100) DEFAULT '',	
		Result_Value8 varchar(100) DEFAULT '',	
		Result_Value9 varchar(100) DEFAULT '',	
		Result_Value10 varchar(100) DEFAULT '',
		Result_Value11 varchar(100) DEFAULT '',	
		Result_Value12 varchar(100) DEFAULT '',	
		Result_Value13 varchar(100) DEFAULT '',	
		Result_Value14 varchar(100) DEFAULT '',	
		Result_Value15 varchar(100) DEFAULT '',
		Result_Value16 varchar(100) DEFAULT '',	
		Result_Value17 varchar(100) DEFAULT '',	
		Result_Value18 nvarchar(100) DEFAULT '',	
		Result_Value19 varchar(100) DEFAULT '',	
		Result_Value20 varchar(100) DEFAULT '',
		Result_Value21 varchar(100) DEFAULT '',	
		Result_Value22 varchar(100) DEFAULT '',	
		Result_Value23 varchar(100) DEFAULT '',
		Result_Value24 varchar(100) DEFAULT '',
		Result_Value25 varchar(100) DEFAULT '',
		Result_Value26 varchar(100) DEFAULT ''
	)

	DECLARE @Remark AS TABLE (
		Result_Seq int identity(1,1),	
		Result_Value1	varchar(1000)	DEFAULT ''
	)

-- =============================================  
-- Validation   
-- =============================================  
-- =============================================  
-- Initialization  
-- =============================================  

	-- Target Scheme
	INSERT INTO @TargetScheme (Scheme)
	SELECT DISTINCT Scheme_Code
	FROM
		SubsidizeGroupClaim SGC
	INNER JOIN 
		Subsidize S ON SGC.Subsidize_Code = S.Subsidize_Code
	INNER JOIN
		SubsidizeItem SI ON SI.Subsidize_Item_Code = S.Subsidize_Item_Code
	WHERE
		SI.Subsidize_Type = @Subsidize_Type
	

	DECLARE @OCSSS_Effective_Dtm datetime
	SELECT @OCSSS_Effective_Dtm = Parm_Value1 FROM SystemParameters where Parameter_Name = 'OCSSSStartDate'

	-- ===========================================
	-- (i) Voucher claim transaction summary
	-- ===========================================

	INSERT INTO #VT (Transaction_ID,Transaction_Dtm,Voucher_Acc_ID,	Temp_Voucher_Acc_ID,Scheme_Code,	
					Service_Receive_Dtm, doc_code, SP_ID, Practice_Display_Seq, Transaction_Status, Reimbursement_Status,
					Claim_Amount, SourceApp, HKIC_Symbol, OCSSS_Ref_Status, DHC_Service)
	SELECT
		Transaction_ID,			
		Transaction_Dtm,			
		Voucher_Acc_ID,			
		Temp_Voucher_Acc_ID,
		Scheme_Code,	
		Service_Receive_Dtm,		
		doc_code,	
		SP_ID,				
		Practice_Display_Seq,
		Record_Status,
		NULL,
		Claim_Amount,		
		SourceApp,
		HKIC_Symbol,
		VT.OCSSS_Ref_Status,
		DHC_Service
	FROM
		VoucherTransaction VT WITH (NOLOCK)
	INNER JOIN
		@TargetScheme S ON VT.Scheme_Code = S.Scheme
	WHERE 
		Manual_Reimburse = 'N'  -- Exlcude Back Office Claim
		AND Transaction_Dtm >= @target_period_from AND Transaction_Dtm < @target_period_to
		AND Transaction_Dtm >= @OCSSS_Effective_Dtm
		AND HKIC_Symbol IS NOT NULL
		AND (
			VT.Invalidation IS NULL
			OR VT.Invalidation NOT IN (
				SELECT Status_Value
				FROM StatStatusFilterMapping
				WHERE ( report_id = 'ALL' OR report_id = @Report_ID)
					AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'
					AND (( Effective_Date IS NULL OR Effective_Date <= @request_dtm)
						AND ( Expiry_Date IS NULL OR Expiry_Date >= @request_dtm ))
				)
			)
		AND VT.Record_Status NOT IN (
			SELECT Status_Value
			FROM StatStatusFilterMapping
			WHERE ( report_id = 'ALL'OR report_id = @Report_ID)
				AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'
				AND (( Effective_Date IS NULL OR Effective_Date <= @request_dtm)
				AND ( Expiry_Date IS NULL OR Expiry_Date >= @request_dtm))
			)	
			
	-- ==================================
	-- Patch Doc No.
	-- ==================================
	
	-- Validated Acct
	UPDATE #VT
	SET Encrypt_Field1 = PI.Encrypt_Field1, Encrypt_Field2 = PI.Encrypt_Field2, Encrypt_Field11 = PI.Encrypt_Field11
	FROM #VT T
	INNER JOIN PersonalInformation PI WITH (NOLOCK) ON T.Voucher_Acc_ID = PI.Voucher_Acc_ID
	WHERE 
		T.Voucher_Acc_ID <> ''

	-- Temp Acct
	UPDATE #VT
	SET Encrypt_Field1 = PI.Encrypt_Field1, Encrypt_Field2 = PI.Encrypt_Field2, Encrypt_Field11 = PI.Encrypt_Field11
	FROM #VT T
	INNER JOIN TempPersonalInformation PI WITH (NOLOCK) ON T.Temp_Voucher_Acc_ID = PI.Voucher_Acc_ID
	WHERE 
		T.Temp_Voucher_Acc_ID <> ''


	-- ===========================================
	-- (ii) Symbol C / U search result summary
	-- ===========================================	
	INSERT INTO #OCSSS_SearchResult (HKIC_Symbol, OCSSS_Ref_Status, Encrypt_Field1)
	SELECT 		
		HKIC_Symbol, 
		OCSSS_Ref_Status,
		Encrypt_Field1
	FROM 
		OCSSSCheckResult R WITH (NOLOCK)
	INNER JOIN
		@TargetScheme S ON R.Scheme_Code = S.Scheme
	WHERE
		System_Dtm >= @target_period_from AND System_Dtm < @target_period_to

----

	INSERT INTO #VT_ConnectFail	(Transaction_ID,Transaction_Dtm,Voucher_Acc_ID,Temp_Voucher_Acc_ID,Scheme_Code,Service_Receive_Dtm,
								doc_code,Encrypt_Field1,Encrypt_Field2,Encrypt_Field11,SP_ID,Practice_Display_Seq,Transaction_Status,
								Reimbursement_Status,Claim_Amount,SourceApp,HKIC_Symbol,OCSSS_Ref_Status, DHC_Service)
	SELECT
		Transaction_ID,
		Transaction_Dtm,
		Voucher_Acc_ID,
		Temp_Voucher_Acc_ID	,
		Scheme_Code,
		Service_Receive_Dtm,		
		doc_code,
		Encrypt_Field1,
		Encrypt_Field2,
		Encrypt_Field11,
		SP_ID,	
		Practice_Display_Seq,
		Transaction_Status,
		Reimbursement_Status,
		Claim_Amount,
		SourceApp,
		HKIC_Symbol,
		OCSSS_Ref_Status,
		DHC_Service
	FROM
		#VT
	WHERE
		OCSSS_Ref_Status IN ('C','N')
		AND HKIC_Symbol IN ('C','U')

	-- ===========================================
	-- Patch the Reimbursement_Status         
	-- ===========================================	

	-- Patch the Reimbursement_Status for transaction created in payment outside eHS
	UPDATE #VT_ConnectFail        
	SET Reimbursement_Status = 'R'        
	WHERE Transaction_Status = 'R'  	

	UPDATE #VT_ConnectFail
	SET Reimbursement_Status = CASE RAT.Authorised_Status        
									WHEN 'R' THEN 'G'        
									ELSE RAT.Authorised_Status        
							   END        
	FROM #VT_ConnectFail VT
	INNER JOIN ReimbursementAuthTran RAT        
	ON VT.Transaction_ID = RAT.Transaction_ID 
	      
	-- Patch the Transaction_Status
	UPDATE #VT_ConnectFail        
	SET Transaction_Status = 'R'        
	WHERE Reimbursement_Status = 'G'  	

-- =============================================
-- Prepare Result Table
-- =============================================
	DECLARE @Search_Cnt INT = 0
	DECLARE @Acc_Cnt	INT = 0
	DECLARE @Tx_Cnt		INT = 0
	DECLARE @Tx_Amt		INT = 0

	-- -----------------------------------------
	-- Excel worksheet (01)
	-- -----------------------------------------

	-- insert record for the final output format  
	INSERT INTO @ResultTable_01 (Result_Value1)
	VALUES ('eHS(S)M0010-01: Report on Online Checking System of the Eligibility of Non-permanent HKIC Holders (OCSSS) who made/ intended to make voucher claim')
	
	INSERT INTO @ResultTable_01 (Result_Value1)
	VALUES ('')
	
	INSERT INTO @ResultTable_01 (Result_Value1)
	VALUES ('Reporting period: ' + FORMAT(@target_period_from, 'yyyy/MM/dd') + ' to ' + FORMAT(DATEADD(dd, - 1, @target_period_to),'yyyy/MM/dd'))
	
	INSERT INTO @ResultTable_01 (Result_Value1)
	VALUES ('')
	
	-- (i) Voucher claim transaction summary
	
	INSERT INTO @ResultTable_01 (Result_Value1)
	VALUES ('(i) Voucher claim transaction summary')
	
	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2,Result_Value3,Result_Value4,Result_Value5)
	VALUES ('' ,'HKIC Symbol','No. of transactions','Amount of vouchers claimed ($)','No. of eHealth (Subsidies) Accounts involved')

	INSERT INTO @ResultTable_01 (Result_Value1)
	VALUES ('a. Overall')
	
	-- Overall
	-- A
	SELECT @Tx_Cnt = COUNT(Transaction_ID), @Tx_Amt = ISNULL(SUM(Claim_Amount), 0), @Acc_Cnt = COUNT(DISTINCT Encrypt_Field1) FROM #VT WHERE HKIC_Symbol = 'A'

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2,Result_Value3,Result_Value4,Result_Value5)
	VALUES ('','A', @Tx_Cnt, @Tx_Amt, @Acc_Cnt)

	-- R
	SELECT @Tx_Cnt = COUNT(Transaction_ID), @Tx_Amt = ISNULL(SUM(Claim_Amount), 0), @Acc_Cnt = COUNT(DISTINCT Encrypt_Field1) FROM #VT WHERE HKIC_Symbol = 'R'

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2,Result_Value3,Result_Value4,Result_Value5)
	VALUES ('','R', @Tx_Cnt, @Tx_Amt, @Acc_Cnt)

	-- C (Connect Success)
	SELECT @Tx_Cnt = COUNT(Transaction_ID), @Tx_Amt = ISNULL(SUM(Claim_Amount), 0), @Acc_Cnt = COUNT(DISTINCT Encrypt_Field1) FROM #VT WHERE HKIC_Symbol = 'C' AND OCSSS_Ref_Status NOT IN ('C','N')
		
	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2,Result_Value3,Result_Value4,Result_Value5)
	VALUES ('','C', @Tx_Cnt, @Tx_Amt, @Acc_Cnt)
	
	-- U (Connect Success)
	SELECT @Tx_Cnt = COUNT(Transaction_ID), @Tx_Amt = ISNULL(SUM(Claim_Amount), 0), @Acc_Cnt = COUNT(DISTINCT Encrypt_Field1) FROM #VT WHERE HKIC_Symbol = 'U' AND OCSSS_Ref_Status NOT IN ('C','N')

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2,Result_Value3,Result_Value4,Result_Value5)
	VALUES ('','U', @Tx_Cnt, @Tx_Amt, @Acc_Cnt)

	-- Others
	SELECT @Tx_Cnt = COUNT(Transaction_ID), @Tx_Amt = ISNULL(SUM(Claim_Amount), 0), @Acc_Cnt = COUNT(DISTINCT Encrypt_Field1) FROM #VT WHERE HKIC_Symbol = 'O'

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2,Result_Value3,Result_Value4,Result_Value5)
	VALUES ('','Others', @Tx_Cnt, @Tx_Amt, @Acc_Cnt)

	-- Failed to connect OCSSS
	SELECT @Tx_Cnt = COUNT(Transaction_ID), @Tx_Amt = ISNULL(SUM(Claim_Amount), 0), @Acc_Cnt = COUNT(DISTINCT Encrypt_Field1) FROM #VT WHERE HKIC_Symbol IN ('C','U') AND OCSSS_Ref_Status IN ('C','N')

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2,Result_Value3,Result_Value4,Result_Value5)
	VALUES ('','Failed to connect OCSSS', @Tx_Cnt, @Tx_Amt, @Acc_Cnt)

	-- Failed to connect OCSSS - C
	SELECT @Tx_Cnt = COUNT(Transaction_ID), @Tx_Amt = ISNULL(SUM(Claim_Amount), 0), @Acc_Cnt = COUNT(DISTINCT Encrypt_Field1) FROM #VT WHERE HKIC_Symbol = 'C' AND OCSSS_Ref_Status IN ('C','N')

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2,Result_Value3,Result_Value4,Result_Value5)
	VALUES ('','C', @Tx_Cnt, @Tx_Amt, @Acc_Cnt)
	
	-- Failed to connect OCSSS - U
	SELECT @Tx_Cnt = COUNT(Transaction_ID), @Tx_Amt = ISNULL(SUM(Claim_Amount), 0), @Acc_Cnt = COUNT(DISTINCT Encrypt_Field1) FROM #VT WHERE HKIC_Symbol = 'U' AND OCSSS_Ref_Status IN ('C','N')

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2,Result_Value3,Result_Value4,Result_Value5)
	VALUES ('','U', @Tx_Cnt, @Tx_Amt, @Acc_Cnt)

	-- Total
	SELECT @Tx_Cnt = COUNT(Transaction_ID), @Tx_Amt = ISNULL(SUM(Claim_Amount), 0), @Acc_Cnt = COUNT(DISTINCT Encrypt_Field1) FROM #VT

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2,Result_Value3,Result_Value4,Result_Value5)
	VALUES ('','Total', @Tx_Cnt, @Tx_Amt, @Acc_Cnt)
	

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	VALUES ('' ,'','','')

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	VALUES ('' ,'','','')

	--===============================================================

	-- (ii) Symbol C / U search result summary

	INSERT INTO @ResultTable_01 (Result_Value1)
	VALUES ('(ii) Symbol C / U search result summary')
	
	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	VALUES ('','HKIC Symbol','No. of search','No. of eHealth (Subsidies) Accounts involved')
	
	INSERT INTO @ResultTable_01 (Result_Value1)
	VALUES ('a. Failed to connect OCSSS')
	
	-- Connect Fail - C
	SELECT @Search_Cnt = COUNT(1), @Acc_Cnt = COUNT(DISTINCT Encrypt_Field1) FROM #OCSSS_SearchResult WHERE OCSSS_Ref_Status IN ('C','N') AND HKIC_Symbol = 'C'

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	VALUES ('' ,'C', @Search_Cnt, @Acc_Cnt)

	-- Connect Fail - U
	SELECT @Search_Cnt = COUNT(1), @Acc_Cnt = COUNT(DISTINCT Encrypt_Field1) FROM #OCSSS_SearchResult WHERE OCSSS_Ref_Status IN ('C','N') AND HKIC_Symbol = 'U'

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	VALUES ('' ,'U', @Search_Cnt, @Acc_Cnt)
	
	-- Connect Fail - Sub total
	SELECT @Search_Cnt = COUNT(1), @Acc_Cnt = COUNT(DISTINCT Encrypt_Field1) FROM #OCSSS_SearchResult WHERE OCSSS_Ref_Status IN ('C','N')

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	VALUES ('' ,'Sub total', @Search_Cnt, @Acc_Cnt)
		
	INSERT INTO @ResultTable_01 (Result_Value1)
	VALUES ('')

	-- Connected OCSSS
	INSERT INTO @ResultTable_01 (Result_Value1)
	VALUES ('b. Connected OCSSS')
	
	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	VALUES ('b1. Checked with valid residential status','HKIC Symbol','No. of checking','No. of eHealth (Subsidies) Accounts involved')

	-- Valid - C
	SELECT @Search_Cnt = COUNT(1), @Acc_Cnt = COUNT(DISTINCT Encrypt_Field1) FROM #OCSSS_SearchResult WHERE OCSSS_Ref_Status = 'V' AND HKIC_Symbol = 'C'

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	VALUES ('' ,'C', @Search_Cnt, @Acc_Cnt)

	-- Valid - U
	SELECT @Search_Cnt = COUNT(1), @Acc_Cnt = COUNT(DISTINCT Encrypt_Field1) FROM #OCSSS_SearchResult WHERE OCSSS_Ref_Status = 'V' AND HKIC_Symbol = 'U'

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	VALUES ('' ,'U', @Search_Cnt, @Acc_Cnt)
	
	-- Valid - Sub total
	SELECT @Search_Cnt = COUNT(1), @Acc_Cnt = COUNT(DISTINCT Encrypt_Field1) FROM #OCSSS_SearchResult WHERE OCSSS_Ref_Status = 'V'

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	VALUES ('' ,'Sub total', @Search_Cnt, @Acc_Cnt)

	INSERT INTO @ResultTable_01 (Result_Value1)
	VALUES ('b2. Checked with invalid residential status')

	-- Invalid - C
	SELECT @Search_Cnt = COUNT(1), @Acc_Cnt = COUNT(DISTINCT Encrypt_Field1) FROM #OCSSS_SearchResult WHERE OCSSS_Ref_Status = 'I' AND HKIC_Symbol = 'C'

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	VALUES ('' ,'C', @Search_Cnt, @Acc_Cnt)

	-- Invalid - U
	SELECT @Search_Cnt = COUNT(1), @Acc_Cnt = COUNT(DISTINCT Encrypt_Field1) FROM #OCSSS_SearchResult WHERE OCSSS_Ref_Status = 'I' AND HKIC_Symbol = 'U'

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	VALUES ('' ,'U', @Search_Cnt, @Acc_Cnt)
	
	-- Invalid - Sub total
	SELECT @Search_Cnt = COUNT(1), @Acc_Cnt = COUNT(DISTINCT Encrypt_Field1) FROM #OCSSS_SearchResult WHERE OCSSS_Ref_Status = 'I'

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	VALUES ('' ,'Sub total', @Search_Cnt, @Acc_Cnt)

	-- Grand Total of section (ii)
	SELECT @Search_Cnt = COUNT(1), @Acc_Cnt = COUNT(DISTINCT Encrypt_Field1) FROM #OCSSS_SearchResult

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	VALUES ('' ,'Grand Total of section (ii)', @Search_Cnt, @Acc_Cnt)

	--===============================================================

	-- -----------------------------------------
	-- Excel worksheet (02)
	-- -----------------------------------------
	INSERT INTO @ResultTable_02 (Result_Value1)
	VALUES ('eHS(S)M0010-02: Raw data of Voucher Claim Transactions with HKIC symbol C or U but failed to connect to OCSSS for checking residential status')
	
	INSERT INTO @ResultTable_02 (Result_Value1)
	VALUES ('')
	
	INSERT INTO @ResultTable_02 (Result_Value1)
	VALUES ('Reporting period: ' + FORMAT(@target_period_from, 'yyyy/MM/dd') + ' to ' + FORMAT(DATEADD(dd, - 1, @target_period_to),'yyyy/MM/dd'))
	
	INSERT INTO @ResultTable_02 (Result_Value1)
	VALUES ('')

	INSERT INTO @ResultTable_02 (Result_Value1,Result_Value2,Result_Value3,Result_Value4,Result_Value5,Result_Value6,Result_Value7,Result_Value8,Result_Value9,Result_Value10,
								Result_value11,Result_value12,Result_value13,Result_value14,Result_value15,Result_value16,Result_value17,Result_value18,Result_value19,Result_value20,
								Result_value21,Result_value22,Result_value23,Result_value24, Result_Value25, Result_Value26)
	VALUES ('Transaction ID', 'Transaction Time', 'Scheme', 'Service Date', 'eHealth (Subsidies) Account ID', 'eHealth (Subsidies) Account Name', 'HKIC No. of VR', 'HKIC Symbol of VR', 
			'Gone through OCSSS checking? (Y/N)',  'Voucher Amount Claimed ($)', 'Net Service Fee Charged ($)', 'Voucher Amount Claimed (￥)', 'Net Service Fee Charged (￥)', 
			'SPID', 'Profession','Practice No.','Practice Name (In English)','Practice Name (In Chinese)','District','District Board','Area',
			'Transaction Status', 'Reimbursement Status', 'Claim through Internet (Manual/ Card Reader)', 'Claim through IVRS', 'DHC-related Services')


	OPEN SYMMETRIC KEY sym_Key 
		DECRYPTION BY ASYMMETRIC KEY asym_Key

	INSERT INTO @ResultTable_02 (Result_Value1,Result_Value2,Result_Value3,Result_Value4,Result_Value5,Result_Value6,Result_Value7,Result_Value8,Result_Value9,Result_Value10,
								Result_value11,Result_value12,Result_value13,Result_value14,Result_value15,Result_value16,Result_value17,Result_value18,Result_value19,Result_value20,
								Result_value21,Result_value22,Result_value23,Result_value24, Result_Value25,Result_Value26)
	SELECT 
		dbo.func_format_system_number(VT.transaction_id) AS Transaction_ID,
		FORMAT(VT.Transaction_Dtm, 'yyyy/MM/dd HH:mm:ss') AS Transaction_Time,
		VT.Scheme_Code,
		FORMAT(VT.Service_Receive_Dtm, 'yyyy/MM/dd') AS Service_Date,
		CASE	WHEN VT.Voucher_Acc_ID <> ''		THEN dbo.func_format_voucher_account_number('V', VT.Voucher_Acc_ID)
				WHEN VT.Temp_Voucher_Acc_ID <> ''	THEN dbo.func_format_voucher_account_number('T', VT.Temp_Voucher_Acc_ID)
		END AS ehs_account_id,
		dbo.func_get_surname_n_initial(CONVERT(varchar, DecryptByKey(VT.Encrypt_Field2))) AS [ehs_account_eng_name],
		dbo.func_mask_doc_id(VT.Doc_Code, CONVERT(varchar, DecryptByKey(VT.Encrypt_Field1)), CONVERT(varchar, DecryptByKey(VT.Encrypt_Field11))) AS [Encrypt_Field1entity_id],		
		SD3.Status_Description AS [HKIC_Symbol],
		'N' AS [With_OCSSS_Checking],
		VT.Claim_Amount,		
		ISNULL(TAF1.AdditionalFieldValueCode, '') AS [Co_Payment],
		CASE	
			WHEN TD.Total_Amount_RMB IS NULL THEN ''
			ELSE CONVERT(VARCHAR(10), TD.Total_Amount_RMB )
		END AS [Total_Amount_RMB],
		ISNULL(TAF2.AdditionalFieldValueCode, '') AS [Co_Payment_RMB],
		VT.SP_ID,
		Prof.Service_Category_Code,
		VT.Practice_Display_Seq,
		P.Practice_Name,
		P.Practice_Name_Chi,
		D.District_Name,
		D.District_Board,
		DB.Area_Name,
		SD1.Status_Description AS [Transaction_Status],
		ISNULL(SD2.Status_Description, '') AS [Reimbursement_Status],  
		CASE WHEN VT.SourceApp <> 'IVRS' THEN 'Y' ELSE 'N' END AS [Claim_Through_Web],
		CASE WHEN VT.SourceApp = 'IVRS' THEN 'Y' ELSE 'N' END AS [Claim_Through_IVRS],
		CASE WHEN VT.Scheme_Code = 'HCVS' THEN ISNULL(VT.DHC_Service, 'N') ELSE 'N/A' END AS [DHC_Service]		
	FROM
		#VT_ConnectFail VT
	INNER JOIN 
		TransactionDetail TD WITH (NOLOCK) ON VT.Transaction_ID = TD.Transaction_ID
	INNER JOIN	
		ServiceProvider SP ON VT.SP_ID = SP.SP_ID
	INNER JOIN
		Practice P ON SP.SP_ID = P.SP_ID AND VT.Practice_Display_Seq = P.Display_Seq
	INNER JOIN
		Professional Prof ON P.SP_ID = Prof.SP_ID AND P.Professional_Seq = Prof.Professional_Seq
	INNER JOIN
		District D ON P.District = D.district_code
	INNER JOIN
		DistrictBoard DB ON D.district_board = DB.district_board
	LEFT JOIN 
		TransactionAdditionalField TAF1 WITH (NOLOCK) ON VT.Transaction_ID = TAF1.Transaction_ID AND TAF1.AdditionalFieldID = 'CoPaymentFee'
	LEFT JOIN 
		TransactionAdditionalField TAF2 WITH (NOLOCK) ON VT.Transaction_ID = TAF2.Transaction_ID AND TAF2.AdditionalFieldID = 'CoPaymentFeeRMB'
	LEFT JOIN
		StatusData SD1 ON VT.Transaction_Status = SD1.Status_Value AND SD1.Enum_Class = 'ClaimTransStatus'
	LEFT JOIN
		StatusData SD2 ON VT.Reimbursement_Status = SD2.Status_Value AND SD2.Enum_Class = 'ReimbursementStatus'   
	LEFT JOIN
		StatusData SD3 ON VT.HKIC_Symbol = SD3.Status_Value AND SD3.Enum_Class = 'HKICSymbol'
			
	ORDER BY
		VT.Transaction_Dtm

		
	CLOSE SYMMETRIC KEY sym_Key


	-- -----------------------------------------
	-- Excel worksheet (Remark)
	-- -----------------------------------------

	INSERT INTO @Remark (Result_Value1)	VALUES 
	('(A) Common Note(s) for the report'),
	('1. eHealth (Subsidies) Accounts:'),
	('   a. eHealth (Subsidies) Account is one with same HKIC no..'),
	('   b. Figures in sub-total, total and grand total no. of eHealth (Subsidies) Accounts involved have excluded duplicated eHealth (Subsidies) Accounts.'),
	(''),
	('2. Transactions:'),
	('   a. All claim transactions created under service providers (either service providers or the delegated users).'),
	('   b. Exclude those transactions created before OCSSS checking effective.'),
	('   c. Exclude those transactions created by back office users.'),
	('   d. Exclude those reimbursed transactions with invalidation status marked as Invalidated.'),
	('   e. Exclude voided/deleted transactions.'),
	(''),
	('3. HKIC Symbol:'),
	('   a. For HKIC Symbol Code ''A'', ''R'' or ''Others'', the OCSSS checking would not be performed.'),
	('   b. The OCSSS checking would be performed for HKIC Symbol Code ''C'' or ''U'' only.'),
	('   c. When failed to connect OCSSS checking for HKIC Symbol Code ''C'' or ''U'', the transaction can be made regardless the OCSSS checking result.')

-- =============================================
-- Return results
-- =============================================	
			
	--------------------------
	-- Result Set 1: Content
	--------------------------
	SELECT	
		'Report Generation Time: ' + FORMAT(GETDATE(), 'yyyy/MM/dd HH:mm') AS Result_Value
	
	--------------------------
	-- Result Set 2: 01
	--------------------------
	SELECT 
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5
	FROM 
		@ResultTable_01
	ORDER BY
		Result_Seq	

	--------------------------
	-- Result Set 3: 02
	--------------------------
	SELECT 
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6,
		Result_Value7,
		Result_Value8,
		Result_Value9,
		Result_Value10,
		Result_Value11,
		Result_Value12,
		Result_Value13,
		Result_Value14,
		Result_Value15,
		Result_Value16,
		Result_Value17,
		Result_Value18,
		Result_Value19,
		Result_Value20,
		Result_Value21,
		Result_Value22,
		Result_Value23,
		Result_Value24,
		Result_value25,
		Result_value26
	FROM 
		@ResultTable_02
	ORDER BY
		Result_Seq

	--------------------------
	-- Result Set 3: Remark
	--------------------------
	SELECT 
		Result_Value1
	FROM 
		@Remark
	ORDER BY
		Result_Seq	


	DROP TABLE #VT
	DROP TABLE #OCSSS_SearchResult

END
GO


GRANT EXECUTE ON [dbo].[proc_EHS_eHSM0010_Report_get] TO HCVU
GO


