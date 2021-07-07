IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSM0009_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSM0009_Report_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Martin Tang
-- Modified date:	16 June 2021
-- Description:		Extend patient name's maximum length (varbinary 100->200)
-- =============================================
-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE18-020 (HKIC Symbol Others)
-- Modified by:		Winnie SUEN
-- Modified date:	25 Feb 2019
-- Description:		Add HKIC Symbol 'Others'
-- =============================================
-- =============================================
-- Author:			Winnie SUEN
-- Create date:		8 Oct 2018
-- CR No.:			CRE17-010 (OCSSS integration)
-- Description:		Get OCSSS checking result for eHA who made/intended to make vaccine claim
-- =============================================

CREATE PROCEDURE [proc_EHS_eHSM0009_Report_get]
	@request_dtm			DATETIME = null,		-- The reference date to get @target_period_from and @target_period_to
	@target_period_from		DATETIME = null,		-- The Target Period From. If defined, it will override the value from the @request_dtm
	@target_period_to		DATETIME = null			-- The Target Period To. If defined, it will override the value from the @request_dtm
AS BEGIN
	
SET NOCOUNT ON;

-- =============================================  
-- Declaration  
-- =============================================  

	--Determine Scheme seq    
	DECLARE @current_scheme_Seq INT
	DECLARE @schemeDate	DATETIME
	DECLARE @Current_Scheme_desc VARCHAR(20)
	DECLARE @Current_Season_Start_Dtm	DATETIME
		
	DECLARE @Scheme_Code AS VARCHAR(10)
	
	DECLARE @Scheme1 VARCHAR(10) = 'VSS'
	DECLARE @Scheme2 VARCHAR(10) = 'ENHVSSO'

	-- Date Period
	IF @request_dtm is null
		SET @request_dtm = GETDATE()

	SET @Scheme_Code = 'VSS'  
	SET @schemeDate = DATEADD(dd, -1, @request_dtm)  

	EXEC @current_scheme_Seq = [proc_EHS_GetSchemeSeq_Stat] @Scheme_Code, @schemeDate  
	SELECT @Current_scheme_desc = Season_Desc FROM VaccineSeason WHERE Scheme_Code = @Scheme_Code AND Scheme_Seq = @current_scheme_Seq AND Subsidize_Item_Code = 'SIV'
	
	-- Determine Season Start Date
	DECLARE @SeasonStartDate datetime
	SELECT @SeasonStartDate = CONVERT(NVARCHAR(4), DATEPART(YEAR,@schemeDate)) + Parm_Value1 FROM SystemParameters where Parameter_Name = 'eHSM0009_Vaccine_Season_StartDate'
	
	IF @schemeDate >= @SeasonStartDate
		SELECT @Current_Season_Start_Dtm = @SeasonStartDate
	ELSE
		SELECT @Current_Season_Start_Dtm = DATEADD(YEAR,-1,@SeasonStartDate)
		
		
	-- Season Start Date, ensure the time start from 00:00 (datetime compare logic use ">=")
	IF @target_period_from is null
		SET @target_period_from = CONVERT(datetime, CONVERT(varchar(10), @Current_Season_Start_Dtm, 105), 105)
	ELSE
		SET @target_period_from = CONVERT(datetime, CONVERT(varchar(10), @target_period_from, 105), 105)

	-- Last Day of Last Month, ensure the time start from 00:00 (datetime compare logic use "<", so should be First Day of Current Month)
	IF @target_period_to is null
		SET @target_period_to = CONVERT(datetime, CONVERT(varchar(10), DATEADD(MONTH, DATEDIFF(MONTH, 0, @request_dtm), 0), 105), 105)
	ELSE
		SET @target_period_to = CONVERT(datetime, CONVERT(varchar(10), @target_period_to, 105), 105)

	DECLARE @OCSSS_Effective_Dtm datetime
	SELECT @OCSSS_Effective_Dtm = Parm_Value1 FROM SystemParameters where Parameter_Name = 'OCSSSStartDate'

	-- 
	DECLARE @Report_ID	VARCHAR(30)	= 'eHSM0009'

	DECLARE @Str_NA as varchar(10)
	DECLARE @Str_Valid varchar(10)
	DECLARE @Str_ConnectionFailed varchar(50)


	DECLARE @TargetScheme TABLE (
		Scheme			VARCHAR(10)
	)

	CREATE TABLE #VT (
		Transaction_ID			char(20),
		Transaction_Dtm			datetime,
		Voucher_Acc_ID			char(15),
		Temp_Voucher_Acc_ID		char(15),
		Special_Acc_ID			char(15),
		Invalid_Acc_ID			char(15),
		Scheme_Code				char(10),
		doc_code				char(20),
		identity_num			varchar(20),
		Encrypt_Field1			varbinary(100),
		Encrypt_Field2			varbinary(200),
		Encrypt_Field11			varbinary(100),
		SP_ID					char(8),	
		Practice_Display_Seq	smallint,
		Manual_Reimburse		char(1),
		HKIC_Symbol				char(1),-- A/C/R/U/O
		OCSSS_Ref_Status		char(1)	-- V: Valid; I: Invalid; C: Connection Fail; N: Turn Off OCSSS Checking,  C & N consider as Fail
	)

	CREATE TABLE #OCSSS_SearchResult (
		Scheme_Code			char(10),
		HKIC_Symbol			char(1),	-- A/C/R/U/O
		OCSSS_Ref_Status	char(1),	-- V: Valid; I: Invalid; C: Connection Fail; N: Turn Off OCSSS Checking,  C & N consider as Fail
		Encrypt_Field1		varbinary(100)
	)

	-- 02
	CREATE TABLE #VT_StayLimit (
		SeqNo					int,
		Transaction_ID			char(20),
		Transaction_Dtm			datetime,
		Voucher_Acc_ID			char(15),
		Temp_Voucher_Acc_ID		char(15),
		Special_Acc_ID			char(15),
		Invalid_Acc_ID			char(15),
		Scheme_Code				char(10),
		doc_code				char(20),
		identity_num			varchar(20),
		Encrypt_Field1			varbinary(100),
		Encrypt_Field2			varbinary(200),
		Encrypt_Field11			varbinary(100),
		SP_ID					char(8),	
		Practice_Display_Seq	smallint,
		Manual_Reimburse		char(1),
		HKIC_Symbol				char(1),-- A/C/R/U/O
		OCSSS_Ref_Status		char(1)	-- V: Valid; I: Invalid; C: Connection Fail; N: Turn Off OCSSS Checking,  C & N consider as Fail
	)


	-- Content Page    
	DECLARE @ContentTable table (   
		Display_Seq		INT IDENTITY(1,1),     
		Value1			VARCHAR(200)DEFAULT '',      
		Value2			VARCHAR(200)DEFAULT '' 
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
		Result_Value8 varchar(100) DEFAULT ''
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
	
	SELECT @Str_NA = Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName='NA'
	SELECT @Str_Valid = Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName='OCSSSResultValid'
	SELECT @Str_ConnectionFailed = Description 	FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName='OCSSSResultConnectionFailed'

		
	INSERT INTO @TargetScheme (Scheme) VALUES (@Scheme1),(@Scheme2)


	-- ===========================================
	-- Excel worksheet (01)
	-- (i) Vaccine claim transaction summary
	-- ===========================================

	INSERT INTO #VT (Transaction_ID,Transaction_Dtm,Voucher_Acc_ID,	Temp_Voucher_Acc_ID,Special_Acc_ID,Invalid_Acc_ID,Scheme_Code,	
					doc_code, SP_ID, Practice_Display_Seq, Manual_Reimburse, HKIC_Symbol, OCSSS_Ref_Status)
	SELECT
		Transaction_ID,			
		Transaction_Dtm,			
		ISNULL(Voucher_Acc_ID, ''),
		ISNULL(Temp_Voucher_Acc_ID, ''),
		ISNULL(Special_Acc_ID, ''),
		ISNULL(Invalid_Acc_ID, ''),
		Scheme_Code,	
		doc_code,	
		SP_ID,				
		Practice_Display_Seq,
		Manual_Reimburse,
		HKIC_Symbol,
		VT.OCSSS_Ref_Status
	FROM
		VoucherTransaction VT WITH (NOLOCK)
	INNER JOIN
		@TargetScheme S ON VT.Scheme_Code = S.Scheme
	WHERE 
		Transaction_Dtm >= @target_period_from AND Transaction_Dtm < @target_period_to
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

	EXEC [proc_SymmetricKey_open]
			
	-- Validated Acct
	UPDATE #VT
	SET 
		identity_num = CONVERT(varchar, DecryptByKey(PI.Encrypt_Field1))
		,Encrypt_Field1 = PI.Encrypt_Field1
		,Encrypt_Field2 = PI.Encrypt_Field2
		,Encrypt_Field11 = PI.Encrypt_Field11
	FROM #VT T
	INNER JOIN PersonalInformation PI WITH (NOLOCK) ON T.Voucher_Acc_ID = PI.Voucher_Acc_ID
	WHERE 
		T.Voucher_Acc_ID <> ''

	-- Temp Acct
	UPDATE #VT
	SET
		identity_num = CONVERT(varchar, DecryptByKey(PI.Encrypt_Field1))
		,Encrypt_Field1 = PI.Encrypt_Field1
		,Encrypt_Field2 = PI.Encrypt_Field2
		,Encrypt_Field11 = PI.Encrypt_Field11
	FROM #VT T
	INNER JOIN TempPersonalInformation PI WITH (NOLOCK) ON T.Temp_Voucher_Acc_ID = PI.Voucher_Acc_ID
	WHERE          
		T.Voucher_Acc_ID = ''          
		AND T.Temp_Voucher_Acc_ID <> ''          
		AND T.Special_Acc_ID = ''  

	-- Special Acct
	UPDATE #VT
	SET 
		identity_num = CONVERT(varchar, DecryptByKey(PI.Encrypt_Field1))
		,Encrypt_Field1 = PI.Encrypt_Field1
		,Encrypt_Field2 = PI.Encrypt_Field2
		,Encrypt_Field11 = PI.Encrypt_Field11
	FROM #VT T
	INNER JOIN SpecialPersonalInformation PI WITH (NOLOCK) ON T.Special_Acc_ID = PI.Special_Acc_ID
	WHERE          
		T.Voucher_Acc_ID = ''          
		AND T.Special_Acc_ID <> ''          
		AND T.Invalid_Acc_ID = ''
		
			
	EXEC [proc_SymmetricKey_close]

	-- ===========================================
	-- (iii) Searching with HKIC Symbol C or U Code with invalid result
	-- ===========================================	
	INSERT INTO #OCSSS_SearchResult (Scheme_Code, HKIC_Symbol, OCSSS_Ref_Status, Encrypt_Field1)
	SELECT 		
		Scheme_Code
		,HKIC_Symbol 
		,OCSSS_Ref_Status
		,Encrypt_Field1
	FROM 
		OCSSSCheckResult R WITH (NOLOCK)
	INNER JOIN
		@TargetScheme S ON R.Scheme_Code = S.Scheme
	WHERE
		System_Dtm >= @target_period_from AND System_Dtm < @target_period_to
		AND HKIC_Symbol IN ('C','U')
		AND OCSSS_Ref_Status = 'I'


	-- ===========================================
	-- Excel worksheet (02)
	-- Find all tx with HKIC Symbol 'C'/'U' and all other claim tx with same recipient
	-- ===========================================		
	INSERT INTO #VT_StayLimit (SeqNo,Transaction_ID,Transaction_Dtm,Voucher_Acc_ID,	Temp_Voucher_Acc_ID,Special_Acc_ID,Invalid_Acc_ID,
						Scheme_Code,doc_code,identity_num,Encrypt_Field1,Encrypt_Field2,Encrypt_Field11, 
						SP_ID, Practice_Display_Seq, Manual_Reimburse, HKIC_Symbol, OCSSS_Ref_Status)
	SELECT
		L.SeqNo
		,VT.Transaction_ID		
		,VT.Transaction_Dtm		
		,VT.Voucher_Acc_ID		
		,VT.Temp_Voucher_Acc_ID	
		,VT.Special_Acc_ID
		,VT.Invalid_Acc_ID
		,VT.Scheme_Code			
		,VT.doc_code			
		,VT.identity_num		
		,VT.Encrypt_Field1		
		,VT.Encrypt_Field2		
		,VT.Encrypt_Field11		
		,VT.SP_ID				
		,VT.Practice_Display_Seq
		,VT.Manual_Reimburse	
		,VT.HKIC_Symbol			
		,VT.OCSSS_Ref_Status
	FROM
		#VT VT
	INNER JOIN
		(	SELECT DISTINCT
				identity_num
				,DENSE_RANK() OVER (ORDER BY identity_num) AS [SeqNo]		 
			FROM 
				#VT 
			WHERE 
				HKIC_Symbol IN ('C','U')
		) L ON VT.identity_num = L.identity_num
		

-- =============================================
-- Prepare Result Table
-- =============================================
	DECLARE @SP_Cnt				INT = 0
	DECLARE @Acc_Cnt			INT = 0
	DECLARE @Tx_Cnt				INT = 0
	DECLARE @Tx_AR_Cnt			INT = 0
	DECLARE @Tx_CU_Cnt			INT = 0
	DECLARE @Tx_CU_Valid_Cnt	INT = 0
	DECLARE @Tx_CU_Fail_Cnt		INT = 0
	DECLARE @Tx_CU_BOClaim_Cnt	INT = 0
	DECLARE @Tx_Others_Cnt		INT = 0


	-- -----------------------------------------
	-- Excel worksheet (01)
	-- -----------------------------------------

	-- insert record for the final output format  
	INSERT INTO @ResultTable_01 (Result_Value1)
	VALUES (REPLACE('eHS(S)M0009-01: Report on cumulative vaccination subsidies schemes claim transactions with HKIC Symbol ([DATE])', '[DATE]', @current_scheme_desc))    

	INSERT INTO @ResultTable_01 (Result_Value1)
	VALUES ('')
	
	INSERT INTO @ResultTable_01 (Result_Value1)
	VALUES ('Reporting period: ' + FORMAT(@target_period_from, 'yyyy/MM/dd') + ' to ' + FORMAT(DATEADD(dd, - 1, @target_period_to),'yyyy/MM/dd'))
	
	INSERT INTO @ResultTable_01 (Result_Value1)
	VALUES ('')


	-- (i) VSS claim transaction summary
	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3) VALUES
	('(i) VSS claim transaction summary' ,'','')

	SET @SP_Cnt = (SELECT COUNT(DISTINCT SP_ID) FROM #VT WHERE Scheme_Code = 'VSS')
	SET @Acc_Cnt = (SELECT COUNT(DISTINCT Encrypt_Field1) FROM #VT WHERE Scheme_Code = 'VSS')
	SET @Tx_Cnt = (SELECT COUNT(1) FROM #VT WHERE Scheme_Code = 'VSS')
	SET @Tx_AR_Cnt = (SELECT COUNT(1) FROM #VT WHERE Scheme_Code = 'VSS' AND HKIC_Symbol IN ('A','R'))
	SET @Tx_CU_Cnt = (SELECT COUNT(1) FROM #VT WHERE Scheme_Code = 'VSS' AND HKIC_Symbol IN ('C','U'))
	SET @Tx_CU_Valid_Cnt = (SELECT COUNT(1) FROM #VT WHERE Scheme_Code = 'VSS' AND HKIC_Symbol IN ('C','U') AND OCSSS_Ref_Status = 'V')	-- Valid
	SET @Tx_CU_Fail_Cnt = (SELECT COUNT(1) FROM #VT WHERE Scheme_Code = 'VSS' AND HKIC_Symbol IN ('C','U') AND OCSSS_Ref_Status IN ('C','N') AND Manual_Reimburse = 'N') -- Connect Fail
	SET @Tx_CU_BOClaim_Cnt = (SELECT COUNT(1) FROM #VT WHERE Scheme_Code = 'VSS' AND HKIC_Symbol IN ('C','U') AND OCSSS_Ref_Status = 'N' AND Manual_Reimburse = 'Y') -- Back Office Claim
	SET @Tx_Others_Cnt = (SELECT COUNT(1) FROM #VT WHERE Scheme_Code = 'VSS' AND HKIC_Symbol = 'O')

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3) VALUES
	('a.' ,'No.of service provider involved', @SP_Cnt),
	('b.' ,'No. of eHealth (Subsidies) Accounts involved', @Acc_Cnt),
	('c.' ,'No. of claim transactions', @Tx_Cnt),
	('' ,'    C1. With HKIC Symbol A or R Code', @Tx_AR_Cnt),
	('' ,'    C2. With HKIC Symbol C or U Code', @Tx_CU_Cnt),
	('' ,'                  Valid result from OCSSS', @Tx_CU_Valid_Cnt),
	('' ,'                  Connection fail to OCSSS', @Tx_CU_Fail_Cnt),
	('' ,'                  Back office claim', @Tx_CU_BOClaim_Cnt),
	('' ,'    C3. With HKIC Symbol Others', @Tx_Others_Cnt)

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	VALUES ('' ,'','','')

	-- (ii) ENHVSSO claim transaction summary
	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3) VALUES
	('(ii) ENHVSSO claim transaction summary' ,'','')

	SET @SP_Cnt = (SELECT COUNT(DISTINCT SP_ID) FROM #VT WHERE Scheme_Code = 'ENHVSSO')
	SET @Acc_Cnt = (SELECT COUNT(DISTINCT Encrypt_Field1) FROM #VT WHERE Scheme_Code = 'ENHVSSO')
	SET @Tx_Cnt = (SELECT COUNT(1) FROM #VT WHERE Scheme_Code = 'ENHVSSO')
	SET @Tx_AR_Cnt = (SELECT COUNT(1) FROM #VT WHERE Scheme_Code = 'ENHVSSO' AND HKIC_Symbol IN ('A','R'))
	SET @Tx_CU_Cnt = (SELECT COUNT(1) FROM #VT WHERE Scheme_Code = 'ENHVSSO' AND HKIC_Symbol IN ('C','U'))
	SET @Tx_CU_Valid_Cnt = (SELECT COUNT(1) FROM #VT WHERE Scheme_Code = 'ENHVSSO' AND HKIC_Symbol IN ('C','U') AND OCSSS_Ref_Status = 'V')	-- Valid
	SET @Tx_CU_Fail_Cnt = (SELECT COUNT(1) FROM #VT WHERE Scheme_Code = 'ENHVSSO' AND HKIC_Symbol IN ('C','U') AND OCSSS_Ref_Status IN ('C','N') AND Manual_Reimburse = 'N') -- Connect Fail
	SET @Tx_CU_BOClaim_Cnt = (SELECT COUNT(1) FROM #VT WHERE Scheme_Code = 'ENHVSSO' AND HKIC_Symbol IN ('C','U') AND OCSSS_Ref_Status = 'N' AND Manual_Reimburse = 'Y') -- Back Office Claim
	SET @Tx_Others_Cnt = (SELECT COUNT(1) FROM #VT WHERE Scheme_Code = 'ENHVSSO' AND HKIC_Symbol = 'O')

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3) VALUES
	('a.' ,'No.of service provider involved', @SP_Cnt),
	('b.' ,'No. of eHealth (Subsidies) Accounts involved', @Acc_Cnt),
	('c.' ,'No. of claim transactions', @Tx_Cnt),
	('' ,'    C1. With HKIC Symbol A or R Code', @Tx_AR_Cnt),
	('' ,'    C2. With HKIC Symbol C or U Code', @Tx_CU_Cnt),
	('' ,'                  Valid result from OCSSS', @Tx_CU_Valid_Cnt),
	('' ,'                  Connection fail to OCSSS', @Tx_CU_Fail_Cnt),
	('' ,'                  Back office claim', @Tx_CU_BOClaim_Cnt),
	('' ,'    C3. With HKIC Symbol Others', @Tx_Others_Cnt)

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	VALUES ('' ,'','','')

	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	VALUES ('' ,'','','')

	--===============================================================

	-- (iii) Searching with HKIC Symbol C or U Code with invalid result

	INSERT INTO @ResultTable_01 (Result_Value1)
	VALUES ('(iii) Searching with HKIC Symbol C or U Code with invalid result')
	
	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4,Result_Value5)
	VALUES ('','',@Scheme1,@Scheme2,'Total')
	
	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4,Result_Value5)
	SELECT
		'a.'
		,'No. of searching'
		,SUM(CASE WHEN Scheme_Code = @Scheme1 THEN Search_Cnt ELSE 0 END) AS [VSS]
		,SUM(CASE WHEN Scheme_Code = @Scheme2 THEN Search_Cnt ELSE 0 END) AS [ENHVSSO]
		,SUM(CASE WHEN Scheme_Code = 'Total' THEN Search_Cnt ELSE 0 END) AS [Total]

	FROM
	(	SELECT 		
			Scheme_Code
			,COUNT(1) AS [Search_Cnt]
		FROM 
			#OCSSS_SearchResult
		GROUP BY
			Scheme_Code
		UNION
		SELECT 		
			'Total'
			,COUNT(1) AS [Search_Cnt]
		FROM 
			#OCSSS_SearchResult
	)S
	
	INSERT INTO @ResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4,Result_Value5)
	SELECT
		'b.'
		,'No.of eHealth (Subsidies) Accounts involved'
		,SUM(CASE WHEN Scheme_Code = @Scheme1 THEN Acc_Cnt ELSE 0 END) AS [VSS]
		,SUM(CASE WHEN Scheme_Code = @Scheme2 THEN Acc_Cnt ELSE 0 END) AS [ENHVSSO]
		,SUM(CASE WHEN Scheme_Code = 'Total' THEN Acc_Cnt ELSE 0 END) AS [Total]
	FROM
	(	SELECT 		
			Scheme_Code
			,COUNT(DISTINCT Encrypt_Field1) AS [Acc_Cnt]
		FROM 
			#OCSSS_SearchResult
		GROUP BY
			Scheme_Code
		UNION
		SELECT 		
			'Total'
			,COUNT(DISTINCT Encrypt_Field1) AS [Acc_Cnt]
		FROM 
			#OCSSS_SearchResult
	)S

	INSERT INTO @ResultTable_01 (Result_Value1)
	VALUES ('')

	INSERT INTO @ResultTable_01 (Result_Value1)
	VALUES ('')

	INSERT INTO @ResultTable_01 (Result_Value1)
	VALUES ('Remark:')

	INSERT INTO @ResultTable_01 (Result_Value1)
	VALUES ('a. Transactions in ' + FORMAT(@Current_Season_Start_Dtm, 'MMM') +' is counted as vaccination season ' + @Current_Scheme_desc + ' in this report' )    
	
	INSERT INTO @ResultTable_01 (Result_Value1)
	VALUES ('b. OCSSS with effective date on ' + FORMAT(@OCSSS_Effective_Dtm, 'dd MMM yyyy'))

	--===============================================================

	-- -----------------------------------------
	-- Excel worksheet (02)
	-- -----------------------------------------
	INSERT INTO @ResultTable_02 (Result_Value1)
	VALUES (REPLACE('eHS(S)M0009-02: Report on vaccination subsidies schemes claim transactions for eHealth (Subsidies) Accounts with limit of stay ([DATE])', '[DATE]', @current_scheme_desc))    

	INSERT INTO @ResultTable_02 (Result_Value1)
	VALUES ('')
	
	INSERT INTO @ResultTable_02 (Result_Value1)
	VALUES ('Reporting period: ' + FORMAT(@target_period_from, 'yyyy/MM/dd') + ' to ' + FORMAT(DATEADD(dd, - 1, @target_period_to),'yyyy/MM/dd'))
	
	INSERT INTO @ResultTable_02 (Result_Value1)
	VALUES ('')

	INSERT INTO @ResultTable_02 (Result_Value1,Result_Value2,Result_Value3,Result_Value4,Result_Value5,Result_Value6,Result_Value7,Result_Value8)
	VALUES ('Seq no.', 'HKIC No.', 'Transaction ID', 'Transaction Time', 'Scheme', 'SPID (Practice No.)', 'HKIC Symbol', 'OCSSS Checking Result')
	
	EXEC [proc_SymmetricKey_open]
				
	INSERT INTO @ResultTable_02 (Result_Value1,Result_Value2,Result_Value3,Result_Value4,Result_Value5,Result_Value6,Result_Value7,Result_Value8)
	SELECT 
		SeqNo
		,dbo.func_mask_doc_id(Doc_Code, CONVERT(varchar, DecryptByKey(Encrypt_Field1)), CONVERT(varchar, DecryptByKey(Encrypt_Field11))) AS [Encrypt_Field1entity_id]
		,dbo.func_format_system_number(transaction_id) AS Transaction_ID
		,FORMAT(Transaction_Dtm, 'yyyy/MM/dd HH:mm:ss') AS Transaction_Time
		,Scheme_Code
		,SP_ID + '(' + CONVERT(VARCHAR(5),Practice_Display_Seq) + ')'
		,SD.Status_Description AS [HKICSymbol]
		,CASE 
			-- SP Claim and HKIC Symbol C or U
			WHEN  ISNULL(Manual_Reimburse, '') = 'N' AND ISNULL(HKIC_Symbol, '') IN ('C','U') THEN
			CASE 

				WHEN OCSSS_Ref_Status = 'V' THEN @Str_Valid 
				WHEN OCSSS_Ref_Status IN ('C','N') THEN @Str_ConnectionFailed
				ELSE @Str_NA
			END
		ELSE 
			-- SP Claim with HKIC Symbol A/R/Others or VU claim
			@Str_NA                                             
		END AS [OCSSS_Ref_Status]
	FROM
		#VT_StayLimit VT
	LEFT JOIN
		StatusData SD ON VT.HKIC_Symbol = SD.Status_Value AND SD.Enum_Class = 'HKICSymbol'
	ORDER BY
		SeqNo, Transaction_Dtm, SP_ID, Practice_Display_Seq

	EXEC [proc_SymmetricKey_close]

	-- -----------------------------------------
	-- Excel worksheet (Remark)
	-- -----------------------------------------

	INSERT INTO @Remark (Result_Value1)	VALUES 
	('(A) Common Note(s) for the report'),
	('1. eHealth (Subsidies) Accounts:'),
	('   a. eHealth (Subsidies) Account is one with same HKIC no..'),
	(''),
	('2. Transactions:'),
	('   a. Claim transactions are filtered by transaction date'),
	('   b. Claim transactions created under service providers (either created by back office users or service providers (or the delegated users))'),
	('   c. Exclude those reimbursed transactions with invalidation status marked as Invalidated.'),
	('   d. Exclude voided/deleted transactions.'),
	(''),
	(REPLACE('(B) Note(s) for Sub report 02 - Report on VSS claim transactions for eHealth (Subsidies) Account with limit of stay ([DATE])','[DATE]', @Current_Scheme_desc)),
	('1. Whenever the vaccine recipient had used HKIC with symbol C or U for vaccination claim'),
	('   a. Show all his/her other claim transaction(s) within the same reporting period regardless of the symbol of his/her HKIC in previous claim transaction(s).')

-- =============================================
-- Return results
-- =============================================	
			
	--------------------------
	-- Result Set 1: Content
	--------------------------
	INSERT INTO @ContentTable (Value1, Value2)
	SELECT 'eHS(S)M0009-01', REPLACE('Report on cumulative vaccination subsidies schemes claim transactions with HKIC Symbol ([DATE])', '[DATE]',  @Current_scheme_desc)

	INSERT INTO @ContentTable (Value1, Value2)
	SELECT 'eHS(S)M0009-02', REPLACE('Report on vaccination subsidies schemes claim transactions for eHealth (Subsidies) Accounts with limit of stay ([DATE])', '[DATE]', @Current_scheme_desc)

	INSERT INTO @ContentTable (Value1)
	VALUES ('')

	INSERT INTO @ContentTable (Value1)
	SELECT 'Report Generation Time: ' + FORMAT(GETDATE(), 'yyyy/MM/dd HH:mm') AS Result_Value
	
	SELECT
		Value1,
		Value2
	FROM
		@ContentTable
	ORDER BY
		Display_Seq

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
		Result_Value8
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
	DROP TABLE #VT_StayLimit
END
GO


GRANT EXECUTE ON [dbo].[proc_EHS_eHSM0009_Report_get] TO HCVU
GO


