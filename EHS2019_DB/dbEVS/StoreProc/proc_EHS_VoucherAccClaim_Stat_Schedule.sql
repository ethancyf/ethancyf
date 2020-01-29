IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_VoucherAccClaim_Stat_Schedule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[proc_EHS_VoucherAccClaim_Stat_Schedule]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN	
-- Modified date:	11 Sep 2019
-- CR No.			CRE19-006 (DHC)
-- Description:		Including scheme [HCVSDHC]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	14 May 2018
-- CR No.:			CRE17-010
-- Description:		OCSSS Integration
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	10 Jan 2018
-- CR No.:			CRE14-026  
-- Description:		Add Deceased Status + Account SFC logic
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	20 April 2017
-- CR No.:			CRE16-025  (Lowering voucher eligibility age)
-- Description:		1. Retrieve the eligible age with latest effective season instead of first season
--					2. Change max claim calculation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	7 July 2016
-- CR No.:			I-CRE16-003 (Fix XSS)
-- Description:		Adjust the numbers in subreport 05-Prof (HCVS) to be bigint
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	24 December 2015
-- CR No.:			I-CRP15-001
-- Description:		Fix overflow in eHSD0001
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	5 November 2015
-- CR No.:			CRP15-002
-- Description:		Fix value of total in elderly voucher statistic report
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	13 April 2015
-- CR No.:			CRE13-019-02
-- Description:		Extend HCVS to China
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT14-0011
-- Modified by:		Lawrence TSANG
-- Modified date:	02 Jul 2014
-- Description:		Fix arithmetic overflow
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE13-018
-- Modified by:		Karl LAM
-- Modified date:	05 Feb 2014
-- Description:		Add house keeping procedure to delete records from temp tables before predefined period (@ExpiryReportDtm)
--					enhance eHSD0001 for change voucher amount to $1
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT14-0004
-- Modified by:		Karl LAM
-- Modified date:	07 Jan 2014
-- Description:		enhance eHSD0001 to support add-on voucher in same year
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT14-0003
-- Modified by:		Karl LAM
-- Modified date:	06 Jan 2014
-- Description:		Synchonize report logic to Old Report (before ceiling) in calculating voucher account counts in sub report 02 03 04
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT14-0001
-- Modified by:		Karl LAM
-- Modified date:	02 Jan 2014
-- Description:		Fix eHA counts in eHSD0001-04
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE13-006
-- Modified by:		Karl LAM
-- Modified date:	12 Nov 2013
-- Description:		Update sub Report eHSD0001-02
--					Rename all physical temp table to add prefix 'RpteHSD0001'
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE13-002 Adding 4 data fields "SPID", "Practice No.", "Practice Name (In English)" and "Practice Name (In Chinese)" in HCVS daily report
-- Modified by:		Tommy LAM
-- Modified date:	29 Apr 2013
-- Description:		Insert additional data fields - [SP_ID], [Practice_Display_Seq], [Practice_Name] and [Practice_Name_Chi] into DB Table - [_TransactionSummary]
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE12-008-02 Allowing different subsidy level for each scheme at different date period
-- Modified by:		Twinsen CHAN
-- Modified date:	4 Jan 2013
-- Description:		1. Remove joining by SchemeClaim.Scheme_Seq
--					2. MaxClaim is bounded by SubsidizeGroupClaim.Last_Service_Dtm			
-- =============================================
-- =============================================
-- Modification History
-- Modified by:     Helen LAM
-- Modified date:   29 December 2011
-- CR No.:          CRP11-016
-- Description:     bug fix for statistic report eHSD0001
--                  (1) Fixed the un-matching Total Account in sheet "01-eHA" and Total Account in "02-Voucher Bal"
--                  (2) Fixed the counting of Secondary Reason For Visit to count for all 1st, 2nd and 3rd Secondary
-- =============================================
-- Modification History
-- Modified by:		Tony FUNG
-- Modified date:	7 October 2011
-- CR No.:		CRE11-024-02
-- Description:		(1) Added the 'ROP' profession for "optometrist".  The data preparation for following tables are modified:
-- 						- _VoucherClaimByProfSummary
--						- _VoucherclaimPerVoucherSummary
--						- _VoucherclaimPerReasonForVisitSummary
--					(2) Changed the zero balance user calculation to be able to compute available claims for ages
--						from the schemeclaim and subsidizegroupclaim tables.
--					(3) Added the defer input and the Secondary Reason for Visit results to the report
--						eHSD0001-02 Report on Balance Summary of eHealth Accounts (for HCVS-eligible only)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Tony FUNG
-- Modified date:	8 September 2011
-- CR No.:			CRP11-009
-- Description:		Modifications to temp tables to preserve ordering:
--						- _VoucherClaimByVoucherSummary
--						- _VoucherclaimPerReasonForVisitSummary
-- =============================================
-- =============================================  
-- Modification History  
-- Modified by:  Pak Ho LEE  
-- Modified date: 06 Sep 2011  
-- CR No.:   CRP11-010  
-- Description:  Performance tuning (version 2) after including terminate accounts  
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Tony FUNG
-- Modified date:	15 July 2011
-- CR No.:			CRP11-007
-- Description:		Performance tuning after including terminate accounts
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	14 June 2011
-- CR No.:			CRE11-007
-- Description:		Modify the report due to terminated accounts
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	31 Dec 2010
-- Description:		(1) Grant 15 vouchers for aged 72
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Eric Tse
-- Modified date:	27 October 2010
-- Description:		(1) Filter invalidated transaction
--					(2) fit with new report layout standard					
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	14 January 2010
-- Description:		(1) Add comments
--					(2) Handle first create account: For the same person (same Doc_Code + Encrypt_Field1) having multiple accounts,
--						only the account with smallest Create_Dtm will be handled
--					(3) Calculate the number of validated accounts based on @Report_Dtm
--					(4) Change the logic on retrieving VoucherTransaction
--					(5) No need to decrypt the Encrypt_Field1
--					(6) Fix the Identity_Num data type
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	5 Jan 2010
-- Description:		Calculate the voucher granted
--					- 70 year old had 5 vouchers
--					- > 70 year old had 10 vouchers
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 03 Oct 2008
-- Description:	Statistics for getting voucher account and claim
--				1. Save related data to temp table (_VoucherSummary, 
--				 _VoucherAccSummary, _VoucherClaimByProfSummary, _VoucherclaimPerVoucherSummary,
--				 _VoucherclaimPerReasonForVisitSummary, _TransactionSummary)
--				2. Insert row to FileGenerationQueue for excel generator
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	09 July 2009
-- Description:		1. Remove Inserting row to FileGenerationQueue which
--					   will be handled by window schedule job
--					2. Return "S" when the whole stored proc run successfully
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	16 Oct 2009
-- Description:		1. Re-define the voucher account
--					2. Save related data to temp table (_VoucherSummary, 
--				 _VoucherAccSummary, _VoucherClaimByProfSummary, _VoucherclaimPerVoucherSummary,
--				 _VoucherclaimPerReasonForVisitSummary, _TransactionSummary, 
--				 _VoucherAccBalanceSummary,	_UsageZeroBalanceVoucherAcc)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	19 Nov 2009
-- Description:		Refine the calculataion of the Voucher account (without claim) to exclude removed cases
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_VoucherAccClaim_Stat_Schedule]
	@cutOffDtm datetime = NULL
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @scheme_code			CHAR(10)
	DECLARE @scheme_code_mainland	CHAR(10)
	DECLARE @scheme_code_DHC		CHAR(10)

	DECLARE @subsidize_code AS CHAR(10)	

	DECLARE @reportDtm DATETIME
	DECLARE @ExpiryReportDtm DATETIME

	DECLARE @noOfVoucher INT
	
	-- For EligibilityRule Setting
	DECLARE @latest_scheme_seq AS SMALLINT
	DECLARE @rule_name AS VARCHAR(20)
	DECLARE @type AS CHAR(10)
	DECLARE @eligibileAge INT
	
	DECLARE @Str_NA as varchar(10)

-- ======================================================
-- Initialization: Variables and Data Tables 
-- ======================================================
	SET @scheme_code = 'HCVS'
	SET @subsidize_code = 'EHCVS'

	
	SET @scheme_code = 'HCVS'
	SET @scheme_code_mainland = 'HCVSCHN'
	SET @scheme_code_DHC = 'HCVSDHC'


	IF @cutOffDtm IS NULL BEGIN
		SET @cutOffDtm = CONVERT(varchar(11), GETDATE(), 106) + ' 00:00'
	END

	SET @reportDtm = DATEADD(day, -1, @cutOffDtm)
	SET @ExpiryReportDtm = DATEADD(day, -14, @ReportDtm)

	--Prepare the account with SFC logic
	EXEC [proc_Prepare_RptAccountSFC] @CutoffDtm, 0


	SELECT @Str_NA = Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName='NA'


-- =============================================
-- Create Temporary Table
-- =============================================
	CREATE table #VoucherTransaction (
		Transaction_ID			CHAR(20),
		Transaction_Dtm			DATETIME,
		Scheme_Code				CHAR(10),
		Voucher_Acc_ID			CHAR(15),
		Temp_Voucher_Acc_ID		CHAR(15),
		Service_Type			CHAR(5),
		SP_ID					CHAR(8),
		Practice_Display_Seq	SMALLINT,
		Doc_Code				CHAR(20),		
		Unit					INT,
		Total_Amount_RMB		MONEY,
		Conversion_Rate			DECIMAL(9, 3),
		Reason_for_Visit_L1		SMALLINT DEFAULT 0,
		ReasonforVisit_S1_L1	SMALLINT DEFAULT 0,
		ReasonforVisit_S2_L1	SMALLINT DEFAULT 0,
		ReasonforVisit_S3_L1	SMALLINT DEFAULT 0,
		Co_Payment				VARCHAR(50),
		Co_Payment_RMB			VARCHAR(50),
		Payment_Type			VARCHAR(20),
		Record_Status			CHAR(1),
		Identity_Num			VARBINARY(100),
		Age						INT,
		Create_By_SmartID		CHAR(1),
		Is_Terminate			CHAR(1),
		Total_WriteOff_Amt		INT,
		WriteOff_Exist			BIT DEFAULT 0,
		Deceased				BIT ,
		Logical_DOD				DATETIME,
		HKIC_Symbol				CHAR(1)	,
		OCSSS_Ref_Status		CHAR(1)	,
		Manual_Reimburse		CHAR(1) ,
		DHC_Service				CHAR(1)
	)

	CREATE INDEX IX_VAT ON #VoucherTransaction (Transaction_ID)
	CREATE NONCLUSTERED INDEX IX_VAT1 ON #VoucherTransaction (Identity_Num)
	CREATE NONCLUSTERED INDEX IX_VAT2 ON #VoucherTransaction (Service_Type)
	CREATE NONCLUSTERED INDEX IX_VAT3 ON #VoucherTransaction (Service_Type, Reason_for_Visit_L1)

--
	CREATE TABLE #Account (
		Voucher_Acc_ID			CHAR(15),
		Temp_Voucher_Acc_ID		CHAR(15),
		Identity_Num			VARBINARY(100),
		Doc_Code				CHAR(20),
		DOB						DATETIME,	
		Exact_DOB				CHAR(1),	
		Age						INT,
		Create_Dtm				DATETIME,
		First_Create_Dtm		DATETIME,
		Is_Terminate			CHAR(1),
		Record_Status			CHAR(1),		
		Total_WriteOff_Amt		INT,
		WriteOff_Exist			BIT DEFAULT 0,
		Deceased				BIT,
		Logical_DOD				DATETIME	
	)

	CREATE INDEX IX_VAT ON #Account (Identity_Num)
	CREATE NONCLUSTERED INDEX IX_VAT1 ON #Account (DOB)

--
	--CREATE TABLE #WriteOffAccount (
	--	Identity_Num			VARBINARY(100),
	--	Doc_Code				CHAR(20),
	--	DOB						DATETIME,
	--	Exact_DOB				CHAR(1),
	--	Age						INT,
	--	Total_WriteOff_Amt		INT,
	--	Account_Seq				INT
	--)
	
	--CREATE INDEX IX_WOA on #WriteOffAccount (identity_num)	
	--CREATE NONCLUSTERED INDEX IX_WOA1 ON #WriteOffAccount (dob)

--
	CREATE TABLE #max_avail_claims(
		AccYear				INT,
		SeasonEntitlement	INT,
		MaxClaim			INT,
		Capping				INT,
		Year_Priority		INT,
		Last_Service_Dtm	DATETIME,
		Min_Eligible_Age	INT
	)

--
	CREATE TABLE #age_max_claims
	(
		Age			INT,
		MaxClaim	INT
	)

--
	--Deceased account max claim on each age and year
	CREATE TABLE #Deceased_Eligible
	(
		Season				SMALLINT, 
		SubSeq				INT, 
		Age					INT,
		Claim_Period_From	DATETIME,
		Last_Service_Dtm	DATETIME,
		Num_Subsidize		INT
	)

--
	CREATE TABLE #decease_max_claim(
		Decease_Year	INT,
		Season			INT,
		DOD_From		DATETIME,
		DOD_To			DATETIME,
		Age_From		INT,
		Age_To			INT,
		Entitlement		INT
	)



	----------------------------------------------
	-- Get Current Eligible Age For Claim Voucher 
	----------------------------------------------
	SELECT 
		@latest_scheme_seq = MAX(Scheme_Seq)
	FROM 
		SubsidizeGroupClaim
	WHERE	
		Scheme_Code = @scheme_code
		AND Subsidize_Code = @subsidize_code
		AND Record_Status = 'A'
		AND Claim_Period_From <= @reportDtm
		AND DATEADD(dd, 1, Last_Service_Dtm) > @reportDtm

	-- Get HCVS - EHCVS Eligible Min. Age (Start)
	SET @rule_name = 'MinAge'
	SET @type = 'AGE'

	SELECT 
		@eligibileAge = [Value]
	FROM 
		EligibilityRule
	WHERE	
		Scheme_Code = @scheme_code
		AND Scheme_Seq = @latest_scheme_seq
		AND Subsidize_Code = @subsidize_code
		AND Rule_Name = @rule_name
		AND [Type] = @type

-- =============================================
-- Preparation: Entitlement Mapping (For alive)
-- =============================================

	--------------------------------------------------------------------
	-- Prepare Mapping Table: Entitlement, Max. Claim and Eligible Age
	--------------------------------------------------------------------

	-- Example of #max_avail_claims
	-- *****************************************************************************************
	-- YearSeq	Entitlement	MaxClaim	Capping	YearPriority	Last_Service_Dtm	Eligible_Age
	-- -------	-----------	----------	-------	------------	----------------	------------
	-- 1		250			250			NULL	1				2009-12-31			69
	-- 2		250			500			NULL	1				2010-12-31			69
	-- 3		250			750			NULL	1				2011-12-31			69
	-- 4		500			1250		NULL	1				2012-12-31			69
	-- 5		1000		2250		NULL	1				2013-12-31			69
	-- 6		2000		4250		4000	1				2014-12-31			69
	-- 7		2000		6250		4000	1				2015-12-31			69
	-- 8		2000		8250		4000	1				2016-12-31			69
	-- 9		2000		10250		4000	1				2017-12-31			64
	-- 10		2000		12250		4000	1				2018-12-31			64
	-- *****************************************************************************************

	INSERT INTO #max_avail_claims (	
		SeasonEntitlement,
		MaxClaim,
		Capping,
		Year_Priority,
		Last_Service_Dtm,
		Min_Eligible_Age)
	SELECT
		SGC1.Num_Subsidize,
		SUM(SGC2.Num_Subsidize) as [MaxClaim] ,
		SGC1.Num_Subsidize_Ceiling,
		ROW_NUMBER() OVER (PARTITION BY YEAR(SGC1.Last_Service_Dtm) ORDER BY SGC1.Last_Service_Dtm DESC) AS [Year_Priority],
		SGC1.Last_service_Dtm,
		ER.Value
	FROM 
		(SELECT * FROM SchemeClaim WHERE Scheme_Code = @scheme_code AND Record_Status <> 'I') SC
			INNER JOIN 
				(SELECT * FROM SubsidizeGroupClaim WHERE Scheme_Code = @scheme_code AND Record_Status <> 'I') SGC1 
					ON SC.Scheme_Code = SGC1.Scheme_Code
			INNER JOIN 
				(SELECT * FROM SubsidizeGroupClaim WHERE Scheme_Code = @scheme_code AND Record_Status <> 'I') SGC2 
					ON SGC1.Scheme_Code = SGC2.Scheme_Code 
						AND SGC1.Claim_Period_From >= SGC2.Claim_Period_From
			INNER JOIN 
				(SELECT 
					Scheme_Code, Scheme_Seq, Rule_Name, [Type], MIN(Value) as Value
				FROM EligibilityRule
				WHERE 
					Scheme_Code = @scheme_code	
					AND Rule_Name = @rule_name
					AND [Type] = @type
				GROUP BY Scheme_Code, Scheme_seq, Rule_Name, [Type])  as ER
					ON SGC1.Scheme_Code = ER.Scheme_Code 
						AND SGC1.scheme_Seq = ER.Scheme_Seq
	WHERE
		((@reportDtm >= SC.Effective_Dtm AND @reportDtm < SC.Expiry_Dtm)
			OR SC.Expiry_Dtm <= @reportDtm)
		AND ((@reportDtm >= SGC1.Claim_Period_From AND @reportDtm < SGC1.Claim_Period_To) 
				OR SGC1.Claim_Period_To <= @reportDtm)
	GROUP by 
		SGC1.Scheme_Code, SGC1.Scheme_seq, SGC1.Num_Subsidize_Ceiling, SGC1.Last_Service_Dtm, SGC1.Num_Subsidize, ER.Value
	ORDER BY SGC1.Scheme_Seq

	-- ---------------------------------------------------------------------------
	-- No. of Records in WriteOff Full Profile (Must retrieve before grouping !!!)
	-- ---------------------------------------------------------------------------
	DECLARE @FullWriteOffProfileCount INT
	SELECT @FullWriteOffProfileCount  = COUNT(Year_Priority) FROM #max_avail_claims


	-- ---------------------------------------------------------------
	-- Group the entitlement by year basis
	-- ---------------------------------------------------------------
	UPDATE MX1
	SET 
		MX1.SeasonEntitlement = MX2.SeasonEntitlement
	FROM 
		#max_avail_claims MX1
			INNER JOIN
				(SELECT YEAR(Last_Service_Dtm) AS Service_Year, SUM(SeasonEntitlement) AS SeasonEntitlement
				FROM #max_avail_claims
				GROUP BY YEAR(Last_Service_Dtm)) MX2
					ON MX2.Service_Year = YEAR(MX1.Last_Service_Dtm) 

	--Only use the latest information in the year
	DELETE FROM #max_avail_claims WHERE Year_Priority > 1


	-- ---------------------------------------------------------------
	-- Patch "AccYear" : Seq. of Year
	-- ---------------------------------------------------------------
	UPDATE 
		#max_avail_claims 
	SET 
		AccYear = AccYear.offset
	FROM	
		(SELECT	
			ROW_NUMBER() OVER (ORDER BY Last_Service_Dtm) AS Offset, 
			Last_Service_Dtm 
		FROM #max_avail_claims) AccYear
	WHERE	
		AccYear.Last_Service_Dtm = #max_avail_claims.Last_Service_Dtm


-- ====================================================
-- Preparation: Age and Max. Claim Mapping (For alive)
-- ====================================================

	-- Example of #age_max_claims
	-- *****************************
	-- Age	MaxClaim	
	-- ---	--------	
	-- 65	2000
	-- 66	4000
	-- 67	4000
	-- 68	4000
	-- 69	4000
	-- 70	4000
	-- 71	4000
	-- 72	6000
	-- 73	8000
	-- 74	10000
	-- 75	11000
	-- 76	11500
	-- 77	11750
	-- 78	12000
	-- 79	12250
	-- *****************************

	-- Calculate the max claim amount for each age
	DECLARE @MaxClaimAmt AS INT
	DECLARE @AgeClaimAmt AS INT
	DECLARE @tempAge AS INT		-- Current Age

	SET @tempAge = @EligibileAge + 1
	SET @AgeClaimAmt = 0

	SELECT @MaxClaimAmt = max(MaxClaim) from #max_avail_claims

	WHILE @AgeClaimAmt < @MaxClaimAmt
	BEGIN
	
		-- get entitle if eligible for that year
		SELECT @AgeClaimAmt = SUM(SeasonEntitlement)
		FROM
			#max_avail_claims
		WHERE
			@tempAge - (YEAR(@reportDtm) - YEAR(Last_Service_Dtm)) > Min_Eligible_Age


		-- insert data to #age_max_claims	
		INSERT INTO #age_max_claims (Age, MaxClaim) VALUES
		(@tempAge, @AgeClaimAmt)
		
		SELECT @tempAge = @tempAge + 1
	END

-- ====================================================
-- Preparation: Entitlement Mapping (For Deceased)
-- ====================================================

	-- Example of #Deceased_eligible
	-- ***********************************************************
	-- Season	SubSeq	Age	From		To			Entitlement
	-- ------	------  --- ----------	----------	-----------
	-- 11		1		64	2018-01-01	2018-12-31	2000
	-- 10		2		64	2017-07-01	2017-12-31	2000
	-- 10		1		69	2017-01-01	2017-12-31	2000
	-- 9		1		69	2016-01-01	2016-12-31	2000
	-- 8		1		69	2015-01-01	2015-12-31	2000
	-- 7		1		69	2014-06-07	2014-12-31	1000
	-- 6		1		69	2014-01-01	2014-06-06	1000
	-- 5		1		69	2013-01-01	2013-12-31	1000
	-- 4		1		69	2012-01-01	2012-12-31	500
	-- 3		1		69	2011-01-01	2011-12-31	250
	-- 2		1		69	2010-01-01	2010-12-31	250
	-- 1		1		69	2009-01-01	2009-12-31	250
	-- ***********************************************************

	INSERT INTO #Deceased_Eligible(
		Season,
		SubSeq,
		Age,
		Claim_Period_From,
		Last_Service_Dtm,
		Num_Subsidize)
	SELECT
		SGC.Scheme_Seq as Season,
		ROW_NUMBER() OVER(PARTITION BY ER.Scheme_Seq ORDER BY ER.Scheme_Seq) AS SubSeq,
		CONVERT(INT, ER.Value) as Age,
		CASE WHEN ER2.Value IS NULL THEN SGC.Claim_Period_From ELSE CONVERT(DATETIME, ER2.Value) END,
		SGC.Last_Service_Dtm,
		SGC.Num_Subsidize
	FROM 
		SchemeClaim SC
			INNER JOIN SubsidizeGroupClaim SGC
				ON SC.Scheme_Code = SGC.Scheme_Code
					AND SGC.Scheme_Code = @scheme_code
					AND SGC.Record_Status <> 'I'
			LEFT JOIN 
				(SELECT 
					* 
				FROM 
					EligibilityRule 
				WHERE 
					Rule_Name = @rule_name
					AND [Type] = @type
				) ER 
					ON SGC.Scheme_Code = ER.Scheme_Code 
						AND SGC.Scheme_Seq = ER.Scheme_Seq
			LEFT JOIN  
				(SELECT 
					Scheme_Code, Scheme_Seq, Value, Rule_Group_Code
				FROM 
					EligibilityRule
				WHERE 
					[Type] = 'SERVICEDTM'
					AND scheme_code= @scheme_code
					AND Rule_Group_Code <> 'G0001'
				) ER2
					ON SGC.Scheme_Code = ER2.Scheme_Code 
					AND SGC.scheme_Seq = ER2.Scheme_Seq
					AND ER.Rule_Group_Code = ER2.Rule_Group_Code
	WHERE
		((@reportDtm >= SC.Effective_Dtm AND @reportDtm < SC.Expiry_Dtm)
				OR SC.Expiry_Dtm <= @reportDtm)
		AND ((@reportDtm >= SGC.Claim_Period_From AND @reportDtm < SGC.Claim_Period_To)
				OR SGC.Claim_Period_To <= @reportDtm)
	ORDER BY  SGC.Scheme_Seq DESC, SubSeq DESC


-- ======================================================
-- Preparation: Age and Max. Claim Mapping (For Deceased)
-- ======================================================

	-- Example of #decease_max_claim [Total 61 Records In Year 2018]
	-- ********************************************************************************
	-- DeathYear	SeasonYear	DOD_From	DOD_To		Age_From	Age_To	Entitlement
	-- ---------	----------	----------	----------	--------	------	-----------
	-- 2018			2009		2018-01-01	2018-12-31	79			79		12250
	-- 2018			2010		2018-01-01	2018-12-31	78			78		12000
	-- 2018			2011		2018-01-01	2018-12-31	77			77		11750
	-- 2018			2012		2018-01-01	2018-12-31	76			76		11500
	-- 2018			2013		2018-01-01	2018-12-31	75			75		11000
	-- 2018			2014		2018-01-01	2018-12-31	74			74		10000
	-- 2018			2015		2018-01-01	2018-12-31	73			73		8000
	-- 2018			2016		2018-01-01	2018-12-31	72			72		6000
	-- 2018			2017		2018-01-01	2018-12-31	66			71		4000
	-- 2018			2018		2018-01-01	2018-12-31	65			65		2000
	-- 2017			2009		2017-01-01	2017-12-31	78			78		10250
	-- 2017			2010		2017-01-01	2017-12-31	77			77		10000
	-- 2017			2011		2017-01-01	2017-12-31	76			76		9750
	-- -- Skip --
	-- 2011			2011		2011-01-01	2011-12-31	70			70		250
	-- 2010			2009		2010-01-01	2010-12-31	71			71		500
	-- 2010			2010		2010-01-01	2010-12-31	70			70		250
	-- 2009			2009		2009-01-01	2009-12-31	70			70		250
	-- ********************************************************************************

	INSERT INTO #decease_max_claim (
		Decease_year, SEASON, DOD_From, DOD_To, Age_From, Age_To, Entitlement	
	)
	Select 
		d2.SeasonYear AS YEAR_Of_DOD
		,d1.SeasonYear AS Season
		,d2.claim_period_from AS DOD_From
		,d2.Last_Service_Dtm AS DOD_From
		,d2.SeasonYear - YEAR(d1.Claim_Period_From) + (d1.age + 1) AS Age_From
		,d2.SeasonYear - YEAR(d1.Claim_Period_From) + (d1.age + 1) AS Age_To
		,SUM(d2.Num_Subsidize) OVER(PARTITION BY d1.SeasonYear ORDER BY d2.season ROWS BETWEEN unbounded preceding AND current row) AS Entitlement
	FROM 
		(SELECT 
			YEAR(Claim_Period_From) AS SeasonYear, 
			MIN(Age) AS Age, 
			MIN(Claim_Period_From) AS Claim_Period_From, 
			MAX(Last_Service_Dtm) AS Last_Service_Dtm, 
			SUM(Num_Subsidize) AS Num_Subsidize 
		FROM #Deceased_eligible 
		GROUP BY YEAR(Claim_Period_From)) d1
			INNER JOIN 
				(SELECT 
					YEAR(Claim_Period_From) AS SeasonYear, * 
				FROM #Deceased_eligible 
				WHERE subseq = 1) d2
					ON d1.SeasonYear <= d2.SeasonYear
	ORDER BY 
		d2.SeasonYear desc, d2.season desc, d1.SeasonYear

	UPDATE
		#decease_max_claim
	SET
		Age_To = Age_To + 5
	WHERE
		SEASON = 2017


-- =============================================
-- Preparation: Get Account
-- =============================================

	INSERT INTO #Account(
		Voucher_Acc_ID,
		Temp_Voucher_Acc_ID,
		Identity_Num,
		Doc_Code,
		DOB,
		Exact_DOB,
		Age,
		Create_Dtm,
		Is_Terminate,
		Record_Status,
		Deceased,
		Logical_DOD)
	SELECT
		Voucher_Acc_ID,
		Temp_voucher_acc_id	,
		Identity_Num,			
		Doc_Code,
		Dob,
		Exact_dob,
		Age,
		Create_Dtm,
		Is_Terminate,
		Record_Status,
		Deceased,
		logical_DOD
	FROM 
		RptAccountSFC

	-- ---------------------------------------------------------------
	-- Patch "Total_WriteOff_Amt" and "WriteOff_Exist"
	-- ---------------------------------------------------------------
	UPDATE	
		#Account 
	SET		
		Total_WriteOff_Amt = ISNULL(WriteOffTotal, 0), 
		WriteOff_Exist = 1
	FROM
		(SELECT DISTINCT WO.Encrypt_Field1,   
			WO.Doc_Code,   
			WO.DOB, 
			WO.Exact_DOB,
			WriteOffTotal = ISNULL(SUM(WO.WriteOff_Unit), 0) 
		FROM 
			eHASubsidizeWriteOff WO 
		WHERE
			WO.Scheme_Code = @scheme_code  
			AND WO.Create_Dtm < @cutoffDtm
		GROUP BY 
			WO.Encrypt_Field1, WO.Doc_Code, WO.DOB, WO.Exact_DOB, DATEDIFF(YY, DOB, @reportDtm)
		HAVING 
			COUNT(WO.Encrypt_Field1) =  @FullWriteOffProfileCount --check full profile 
		) WriteOFF
	 WHERE	
		WriteOFF.Encrypt_Field1 = #Account.Identity_Num
		AND	
			((WriteOFF.Doc_Code = #Account.Doc_Code)
			or 
			(WriteOFF.Doc_Code = 'HKBC' AND #Account.Doc_Code = 'HKIC') 
			or 
			(WriteOFF.Doc_Code = 'HKIC' AND #Account.Doc_Code = 'HKBC')  
			)
		 AND #Account.Identity_Num = WriteOFF.Encrypt_Field1
		 AND #Account.Exact_DOB = WriteOFF.Exact_DOB
		 and #Account.DOB = WriteOFF.DOB	


-- =============================================
-- Preparation: Get HCVS transaction
-- =============================================

	INSERT INTO #VoucherTransaction(
		Transaction_ID,
		Transaction_Dtm,
		Scheme_Code,
		Voucher_Acc_ID,
		Temp_Voucher_Acc_ID,
		Service_Type,
		SP_ID,
		Practice_Display_Seq,
		Doc_Code,
		Unit,
		Total_Amount_RMB,
		Conversion_Rate,
		Reason_for_visit_L1,
		ReasonforVisit_S1_L1,
		ReasonforVisit_S2_L1,
		ReasonforVisit_S3_L1,
		Co_Payment,
		Co_Payment_RMB,
		Payment_Type,
		Record_Status,
		--Identity_Num,
		--Age,
		Create_By_SmartID,
		Identity_Num,
		Age,
		Deceased,
		Logical_DOD,
		Is_Terminate,
		Total_WriteOff_Amt,
		WriteOff_Exist,
		HKIC_Symbol,
		OCSSS_Ref_Status,
		Manual_Reimburse,
		DHC_Service
		)
	SELECT
		VT.Transaction_ID,
		VT.Transaction_Dtm,
		VT.Scheme_Code,
		ISNULL(VT.Voucher_Acc_ID, ''),
		ISNULL(VT.Temp_Voucher_Acc_ID, ''),
		VT.Service_Type,
		VT.SP_ID,
		VT.Practice_Display_Seq,
		VT.Doc_Code,
		TD.Unit,
		TD.Total_Amount_RMB,
		TD.ExchangeRate_Value,
		CONVERT(SMALLINT, ISNULL(TAF1.AdditionalFieldValueCode, 0)) AS Reason_for_Visit_L1, 
		CONVERT(SMALLINT, ISNULL(TAF2.AdditionalFieldValueCode, 0)) AS ReasonforVisit_S1_L1, 
		CONVERT(SMALLINT, ISNULL(TAF3.AdditionalFieldValueCode, 0)) AS ReasonforVisit_S2_L1, 
		CONVERT(SMALLINT, ISNULL(TAF4.AdditionalFieldValueCode, 0)) AS ReasonforVisit_S3_L1,
		TAF5.AdditionalFieldValueCode AS [Co_Payment],
		TAF6.AdditionalFieldValueCode AS [Co_Payment_RMB],
		TAF7.AdditionalFieldValueCode AS [Payment_Type],
		VT.Record_Status,
		--NULL AS [Identity_Num],
		--NULL AS [Age],
		VT.Create_By_SmartID,	
		CASE WHEN VP.Voucher_Acc_ID IS NULL THEN TP.Encrypt_Field1 ELSE VP.Encrypt_Field1 END AS [Identity_Num],
		CASE WHEN VP.Voucher_Acc_ID IS NULL THEN ACTP.Age ELSE ACVP.Age END AS [Age],
		CASE WHEN VP.Voucher_Acc_ID IS NULL THEN ACTP.Deceased ELSE ACVP.Deceased END AS [Deceased],
		CASE WHEN VP.Voucher_Acc_ID IS NULL THEN ACTP.logical_DOD ELSE ACVP.logical_DOD END AS [logical_DOD],
		CASE WHEN VP.Voucher_Acc_ID IS NULL THEN ACTP.Is_Terminate ELSE ACVP.Is_Terminate END AS [Is_Terminate],
		CASE WHEN VP.Voucher_Acc_ID IS NULL THEN ACTP.Total_WriteOff_Amt ELSE ACVP.Total_WriteOff_Amt END AS [Total_WriteOff_Amt],
		CASE WHEN VP.Voucher_Acc_ID IS NULL THEN ACTP.WriteOff_Exist ELSE ACVP.WriteOff_Exist END AS [WriteOff_Exist],
		VT.HKIC_Symbol,
		VT.OCSSS_Ref_Status,
		VT.Manual_Reimburse,
		VT.DHC_Service
	FROM
		VoucherTransaction VT
			INNER JOIN TransactionDetail TD
				ON VT.Transaction_ID = TD.Transaction_ID
			LEFT OUTER JOIN TransactionAdditionalField TAF1
				ON VT.Transaction_ID = TAF1.Transaction_ID
					AND TAF1.AdditionalFieldID = 'Reason_for_Visit_L1'
			LEFT OUTER JOIN TransactionAdditionalField TAF2
				ON VT.Transaction_ID = TAF2.Transaction_ID
					AND TAF2.AdditionalFieldID = 'ReasonforVisit_S1_L1'
			LEFT OUTER JOIN TransactionAdditionalField TAF3
				ON VT.Transaction_ID = TAF3.Transaction_ID
					AND TAF3.AdditionalFieldID = 'ReasonforVisit_S2_L1'
			LEFT OUTER JOIN TransactionAdditionalField TAF4
				ON VT.Transaction_ID = TAF4.Transaction_ID
					AND TAF4.AdditionalFieldID = 'ReasonforVisit_S3_L1'
			LEFT OUTER JOIN TransactionAdditionalField TAF5
				ON VT.Transaction_ID = TAF5.Transaction_ID
					AND TAF5.AdditionalFieldID = 'CoPaymentFee'
			LEFT OUTER JOIN TransactionAdditionalField TAF6
				ON VT.Transaction_ID = TAF6.Transaction_ID
					AND TAF6.AdditionalFieldID = 'CoPaymentFeeRMB'
			LEFT OUTER JOIN TransactionAdditionalField TAF7
				ON VT.Transaction_ID = TAF7.Transaction_ID
					AND TAF7.AdditionalFieldID = 'PaymentType'
			LEFT JOIN PersonalInformation VP
				ON VT.Voucher_Acc_ID = VP.Voucher_Acc_ID
			LEFT JOIN #Account ACVP 
				ON VP.Encrypt_Field1 = ACVP.identity_num
			LEFT JOIN TempPersonalInformation TP
				ON VT.Temp_Voucher_Acc_ID = TP.Voucher_Acc_ID
			LEFT JOIN #Account ACTP
				ON TP.Encrypt_Field1 = ACTP.identity_num
	WHERE
		NOT EXISTS(
			SELECT
				1
			FROM
				StatStatusFilterMapping
			WHERE 
				(Report_id = 'ALL' OR Report_id = 'eHSD0001') 
				AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status' 
				AND ((Effective_Date IS NULL OR Effective_Date <= @cutOffDtm) AND (Expiry_Date IS NULL OR @cutOffDtm < Expiry_Date))
				AND Status_Value = VT.Record_Status 
			)
		AND (VT.Scheme_Code = @scheme_code OR VT.Scheme_Code = @scheme_code_mainland OR VT.Scheme_Code = @scheme_code_DHC)
		AND VT.Transaction_Dtm < @cutoffDtm
		AND (VT.Invalidation IS NULL OR NOT EXISTS (
				SELECT
					1
				FROM
					StatStatusFilterMapping
				WHERE
					(Report_id = 'ALL' OR Report_id = 'eHSD0001') 
					AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'
					AND ((Effective_Date is null or Effective_Date <= @cutOffDtm) AND (Expiry_Date is null or @cutOffDtm < Expiry_Date))
					AND Status_Value = VT.Invalidation 
				)
			)


	---- ---------------------------------------------------------------
	---- Patch "Identity_Num"
	---- ---------------------------------------------------------------
	-- 1.Validated Account
	--UPDATE
	--	#VoucherTransaction
	--SET
	--	Identity_Num = P.Encrypt_Field1
	--FROM
	--	#VoucherTransaction VT
	--		INNER JOIN PersonalInformation P
	--			ON VT.Voucher_Acc_ID = P.Voucher_Acc_ID
	--WHERE
	--	VT.Voucher_Acc_ID <> ''

	---- 2.Temporary Account
	--UPDATE
	--	#VoucherTransaction
	--SET
	--	Identity_Num = TP.Encrypt_Field1
	--FROM
	--	#vouchertransaction VT
	--		INNER JOIN TempPersonalInformation TP
	--			ON VT.Temp_Voucher_Acc_ID = TP.Voucher_Acc_ID
	--WHERE
	--	VT.Voucher_Acc_ID = ''

	---- ---------------------------------------------------------------
	---- Patch "Age", "Deceased", "Logical_DOD" and "Is_Terminate"
	---- ---------------------------------------------------------------
	--UPDATE
	--	#VoucherTransaction
	--SET
	--	Age = ACVP.Age,
	--	Deceased = ACVP.Deceased,
	--	Logical_DOD = ACVP.logical_DOD,
	--	Is_Terminate = ACVP.Is_Terminate
	--FROM
	--	#Vouchertransaction VT
	--		INNER JOIN RptAccountSFC ACVP
	--			ON VT.Identity_Num = ACVP.Identity_Num 

	---- ---------------------------------------------------------------
	---- Patch "Total_WriteOff_Amt" and "WriteOff_Exist"
	---- ---------------------------------------------------------------
	--UPDATE	
	--	#VoucherTransaction 
	--SET		
	--	Total_WriteOff_Amt = A.Total_WriteOff_Amt, 
	--	WriteOff_Exist = 1
	--FROM
	--	#Vouchertransaction VT
	--		INNER JOIN #Account A
	--			ON VT.Identity_Num = A.Identity_Num 
	
-- Remove VoucherTran that cannot match with SFC Account
	DELETE #VoucherTransaction WHERE Age IS NULL


-- =============================================
-- Remove the data in the statistics tables for today
-- =============================================

	DELETE FROM RpteHSD0001VoucherSummary WHERE report_dtm = @reportDtm	OR (report_dtm <= @ExpiryReportDtm)							-- Summary of cumulative totals
	DELETE FROM RpteHSD0001_01_eHA_Summary WHERE report_dtm = @reportDtm	OR (report_dtm <= @ExpiryReportDtm)						-- eHSD0001-01: Report on Number of eHealth Accounts (for HCVS-eligible only) 
	DELETE FROM RpteHSD0001_02_AvailableVoucher_Summary WHERE report_dtm = @reportDtm	OR (report_dtm <= @ExpiryReportDtm)			-- eHSD0001-02: Report on Balance Summary of eHealth Accounts (for HCVS-eligible only)
	DELETE FROM RpteHSD0001_03_WriteOffVoucher_Summary WHERE report_dtm = @reportDtm	OR (report_dtm <= @ExpiryReportDtm)			-- eHSD0001-03: Report on Usage of Zero Balance eHealth Accounts (for HCVS-eligible only)
	DELETE FROM RpteHSD0001_04_TotalEntitledVoucher_Summary WHERE report_dtm = @reportDtm	OR (report_dtm <= @ExpiryReportDtm)		-- eHSD0001-04: Report on Given Voucher Summary on eHealth Account (for HCVS-eligible only)
	DELETE FROM RpteHSD0001_05_VoucherClaimByProfSummary WHERE report_dtm = @reportDtm	OR (report_dtm <= @ExpiryReportDtm)				-- eHSD0001-05: Report on Number of Voucher Claimed by Profession (HCVS)
	DELETE FROM RpteHSD0001_06a_VoucherClaimPerVoucherSummary WHERE report_dtm = @reportDtm	OR (report_dtm <= @ExpiryReportDtm)			-- eHSD0001-06a: Report on Number of Voucher Claim per Transaction by Profession (HCVS)
	DELETE FROM RpteHSD0001_06b_VoucherClaimPerVoucherSummary WHERE report_dtm = @reportDtm	OR (report_dtm <= @ExpiryReportDtm)			-- eHSD0001-06b: Report on Number of Voucher Claim per Transaction by Profession (HCVSDHC)
	DELETE FROM RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary WHERE report_dtm = @reportDtm	OR (report_dtm <= @ExpiryReportDtm)	-- eHSD0001-07a: Report on Number of Claim Transactions by Profession and Reason for Visit Level 1 (HCVS)
	DELETE FROM RpteHSD0001_07b_VoucherClaimPerReasonForVisitSummary WHERE report_dtm = @reportDtm	OR (report_dtm <= @ExpiryReportDtm)	-- eHSD0001-07b: Report on Number of Claim Transactions by Profession and Reason for Visit Level 1 (HCVSDHC)
	DELETE FROM RpteHSD0001_08a_TransactionSummary WHERE report_dtm = @reportDtm	OR (report_dtm <= @ExpiryReportDtm)						-- eHSD0001-08a: Raw data of Voucher Claim Transactions (HCVS)
	DELETE FROM RpteHSD0001_08b_HCVSDHCTransactionSummary WHERE report_dtm = @reportDtm	OR (report_dtm <= @ExpiryReportDtm)						-- eHSD0001-08b: Raw data of Voucher Claim Transactions (HCVSDHC)
	DELETE FROM RpteHSD0001_08c_HCVSCHNTransactionSummary WHERE report_dtm = @reportDtm	OR (report_dtm <= @ExpiryReportDtm)				-- eHSD0001-08c: Raw data of Voucher Claim Transactions (HCVSCHN)


-- =============================================
-- Process data for statistics
-- =============================================

-- +------------------------------------------------------------------------------------------------------------------+
-- |                                                     Summary                                                      |
-- +------------------------------------------------------------------------------------------------------------------+

--					  | Total Voucher Amount Claimed ($) | No. of Transaction | No. of SP
-- -------------------+----------------------------------+--------------------+-----------								
-- Any Voucher Scheme |                                  |                    |
-- HCVS				  |                                  |                    |
-- HCVSCHN			  |                                  |                    |
-- HCVSDHC			  |                                  |                    |

	INSERT INTO RpteHSD0001VoucherSummary (System_dtm, report_dtm, noOfVoucherClaimed, noOfTransaction, noOfSP, Scheme_Code)
	SELECT
		GETDATE(),
		@reportDtm,
		ISNULL(SUM(CONVERT(bigint, Unit)), 0),
		COUNT(1),
		COUNT(DISTINCT sp_id),
		'ALL'
	FROM
		#VoucherTransaction

	--

	INSERT INTO RpteHSD0001VoucherSummary (System_dtm, report_dtm, noOfVoucherClaimed, noOfTransaction, noOfSP, Scheme_Code)
	SELECT
		GETDATE(),
		@reportDtm,
		ISNULL(SUM(CONVERT(bigint, Unit)), 0),
		COUNT(1),
		COUNT(DISTINCT sp_id),
		'HCVS'
	FROM
		#VoucherTransaction
	WHERE
		Scheme_Code = @Scheme_Code
	
	--
	
	INSERT INTO RpteHSD0001VoucherSummary (System_dtm, report_dtm, noOfVoucherClaimed, noOfTransaction, noOfSP, Scheme_Code)
	SELECT
		GETDATE(),
		@reportDtm,
		ISNULL(SUM(CONVERT(bigint, Unit)), 0),
		COUNT(1),
		COUNT(DISTINCT sp_id),
		'HCVSCHN'
	FROM
		#VoucherTransaction
	WHERE
		Scheme_Code = @scheme_code_mainland
	
	--
	
	INSERT INTO RpteHSD0001VoucherSummary (System_dtm, report_dtm, noOfVoucherClaimed, noOfTransaction, noOfSP, Scheme_Code)
	SELECT
		GETDATE(),
		@reportDtm,
		ISNULL(SUM(CONVERT(bigint, Unit)), 0),
		COUNT(1),
		COUNT(DISTINCT sp_id),
		'HCVSDHC'
	FROM
		#VoucherTransaction
	WHERE
		Scheme_Code = @scheme_code_DHC

-- +------------------------------------------------------------------------------------------------------------------+
-- |                eHSD0001-01: Report on Number of eHealth Accounts (covering Voucher Scheme only)                  |
-- +------------------------------------------------------------------------------------------------------------------+

--						|   Without Claim  |    With Claim    |      Total       |  | Total No. of Validated Account 
-- ---------------------+------------------+------------------+------------------+--+--------------------------------	
--						| Alive | Deceased | Alive | Deceased | Alive | Deceased |  |
-- ---------------------+-------+----------+-------+----------+-------+----------+--+--------------------------------
-- Any Voucher Scheme	|       |          |       |          |       |          |  |
-- HCVS					|       |          |       |          |       |          |  |
-- HCVSCHN				|       |          |       |          |       |          |  |
-- HCVSDHC				|       |          |       |          |       |          |  |

	DECLARE @WithoutClaim_All_Alive			int
	DECLARE @WithoutClaim_HCVS_Alive		int
	DECLARE @WithoutClaim_HCVSCHN_Alive		int
	DECLARE @WithoutClaim_HCVSDHC_Alive		int
	DECLARE @WithoutClaim_All_Deceased		int
	DECLARE @WithoutClaim_HCVS_Deceased		int
	DECLARE @WithoutClaim_HCVSCHN_Deceased	int
	DECLARE @WithoutClaim_HCVSDHC_Deceased	int
	DECLARE @WithClaim_All_Alive			int
	DECLARE @WithClaim_HCVS_Alive			int
	DECLARE @WithClaim_HCVSCHN_Alive		int
	DECLARE @WithClaim_HCVSDHC_Alive		int
	DECLARE @WithClaim_All_Deceased			int
	DECLARE @WithClaim_HCVS_Deceased		int
	DECLARE @WithClaim_HCVSCHN_Deceased		int
	DECLARE @WithClaim_HCVSDHC_Deceased		int
	DECLARE @totalVA_Alive					int
	DECLARE @totalVA_Deceased				int
	DECLARE @ValidVA						int
	DECLARE @ValidateAccountActive			int
	DECLARE @ValidateAccountSuspend			int
	DECLARE @ValidateAccountTerminate		int
	DECLARE @ValidateAccountTotal			int
	DECLARE @TerminateVoucher				int
	

	CREATE table #ValidateAccount (    
	  Record_Status   Char(1),    
	  Record_Count    int    
	)

	-- --------------------------------
	-- Without Claim + HCVS & HCVSCHN & HCVSDHC
	-- --------------------------------
	SELECT	@WithoutClaim_All_Alive = COUNT(DISTINCT Identity_Num)  
	FROM	#Account A  
	WHERE	NOT EXISTS (  
				SELECT	1
				FROM	#VoucherTransaction
				WHERE	Identity_Num = A.identity_num
			)
			AND A.Is_Terminate = 'N'
			AND A.Deceased = 0 

	SELECT	@WithoutClaim_All_Deceased = COUNT(DISTINCT Identity_Num)  
	FROM	#Account A  
	WHERE	NOT EXISTS (  
				SELECT	1
				FROM	#VoucherTransaction  
				WHERE	Identity_num = A.Identity_Num
			)
			AND A.Is_Terminate = 'N' 
			AND A.Deceased = 1 
				
	-- --------------------------------
	-- Without Claim + HCVS 
	-- --------------------------------
	SELECT	@WithoutClaim_HCVS_Alive = COUNT(DISTINCT identity_num)  
	FROM	#Account A 
	WHERE	NOT EXISTS (  
					SELECT	Identity_Num
					FROM	#VoucherTransaction  
					WHERE	Identity_num = A.Identity_Num
								AND Scheme_Code = @scheme_code
			)
				AND a.Is_Terminate = 'N' 
				AND a.Deceased = 0

	SELECT	@WithoutClaim_HCVS_Deceased = COUNT(DISTINCT identity_num)  
	FROM	#Account A  
	WHERE	NOT EXISTS (  
					SELECT	Identity_Num
					FROM	#VoucherTransaction 
					WHERE	Identity_num = A.Identity_Num
								AND Scheme_Code = @scheme_code
			)
				AND a.Is_Terminate = 'N' 
				AND a.Deceased = 1
				
	-- --------------------------------
	-- Without Claim + HCVSCHN
	-- --------------------------------
	SELECT	@WithoutClaim_HCVSCHN_Alive = COUNT(DISTINCT identity_num)  
	FROM	#Account A
	WHERE	NOT EXISTS (  
					SELECT	Identity_Num
					FROM	#VoucherTransaction 
					WHERE	Identity_num = A.Identity_Num
								AND Scheme_Code = @scheme_code_mainland
			)
				AND a.Is_Terminate = 'N' 
				AND a.Deceased=0

	SELECT	@WithoutClaim_HCVSCHN_Deceased = COUNT(DISTINCT identity_num)  
	FROM	#Account A 
	WHERE	NOT EXISTS (  
					SELECT	Identity_Num
					FROM	#VoucherTransaction
					WHERE	Identity_num = A.Identity_Num
								AND Scheme_Code = @scheme_code_mainland
			)
				AND a.Is_Terminate = 'N' 
				AND a.Deceased=1

	-- --------------------------------
	-- Without Claim + HCVSDHC
	-- --------------------------------
	SELECT	@WithoutClaim_HCVSDHC_Alive = COUNT(DISTINCT identity_num)  
	FROM	#Account A
	WHERE	NOT EXISTS (  
					SELECT	Identity_Num
					FROM	#VoucherTransaction 
					WHERE	Identity_num = A.Identity_Num
								AND Scheme_Code = @scheme_code_DHC
			)
				AND a.Is_Terminate = 'N' 
				AND a.Deceased=0

	SELECT	@WithoutClaim_HCVSDHC_Deceased = COUNT(DISTINCT identity_num)  
	FROM	#Account A 
	WHERE	NOT EXISTS (  
					SELECT	Identity_Num
					FROM	#VoucherTransaction
					WHERE	Identity_num = A.Identity_Num
								AND Scheme_Code = @scheme_code_DHC
			)
				AND a.Is_Terminate = 'N' 
				AND a.Deceased=1

	-- --------------------------------
	-- With Claim + HCVS & HCVSCHN & HCVSDHC
	-- --------------------------------
	SELECT
		@WithClaim_All_Alive = COUNT(DISTINCT identity_num)
	FROM
		#VoucherTransaction
	WHERE
		Is_Terminate = 'N'
		AND Deceased=0

	SELECT
		@WithClaim_All_Deceased = COUNT(DISTINCT identity_num)
	FROM
		#VoucherTransaction
	WHERE
		Is_Terminate = 'N'
		AND Deceased=1
			
	-- --------------------------------
	-- With Claim + HCVS
	-- --------------------------------
	SELECT
		@WithClaim_HCVS_Alive = COUNT(DISTINCT identity_num)
	FROM
		#VoucherTransaction
	WHERE
		Is_Terminate = 'N'
		AND Scheme_Code = @scheme_code
		AND Deceased=0

	SELECT
		@WithClaim_HCVS_Deceased = COUNT(DISTINCT identity_num)
	FROM
		#VoucherTransaction
	WHERE
		Is_Terminate = 'N'
		AND Scheme_Code = @scheme_code
		AND Deceased = 1
	
	-- --------------------------------
	-- With Claim + HCVSCHN
	-- --------------------------------
	SELECT
		@WithClaim_HCVSCHN_Alive = COUNT(DISTINCT identity_num)
	FROM
		#VoucherTransaction
	WHERE
		Is_Terminate = 'N'
		AND Scheme_Code = @scheme_code_mainland
		AND Deceased=0

	SELECT
		@WithClaim_HCVSCHN_Deceased = COUNT(DISTINCT identity_num)
	FROM
		#VoucherTransaction
	WHERE
		Is_Terminate = 'N'
		AND Scheme_Code = @scheme_code_mainland
		AND Deceased=1

	-- --------------------------------
	-- With Claim + HCVSDHC
	-- --------------------------------
	SELECT
		@WithClaim_HCVSDHC_Alive = COUNT(DISTINCT identity_num)
	FROM
		#VoucherTransaction
	WHERE
		Is_Terminate = 'N'
		AND Scheme_Code = @scheme_code_DHC
		AND Deceased=0

	SELECT
		@WithClaim_HCVSDHC_Deceased = COUNT(DISTINCT identity_num)
	FROM
		#VoucherTransaction
	WHERE
		Is_Terminate = 'N'
		AND Scheme_Code = @scheme_code_DHC
		AND Deceased=1

	-- --------------------------------
	-- Total
	-- --------------------------------
	select @totalVA_Alive = @WithoutClaim_All_Alive + @WithClaim_All_Alive
	select @totalVA_Deceased = @WithoutClaim_All_Deceased + @WithClaim_All_Deceased

	-- -----------------------------------
	-- No. of Validated Account By Status
	-- -----------------------------------
	SET @ValidateAccountActive = 0  
	SET @ValidateAccountSuspend = 0  
	SET @ValidateAccountTerminate = 0

	INSERT INTO #ValidateAccount(    
		Record_Status, 
		Record_Count    
	 )    
	 Select 
		Record_Status, 
		Count(1)   
	 FROM 
		#Account 
	 Where
		Voucher_Acc_ID <> '' 
		AND Create_Dtm < @cutOffDtm 
	 GROUP BY Record_Status

	 SELECT @ValidVA = SUM(Record_Count) FROM #ValidateAccount WHERE Record_Status <> 'D'    
  
	 SELECT @ValidateAccountActive = ISNULL(Record_Count,0) FROM #ValidateAccount WHERE Record_Status = 'A'    
  
	 SELECT @ValidateAccountSuspend = ISNULL(Record_Count,0) FROM #ValidateAccount WHERE Record_Status = 'S'    
  
	 SELECT @ValidateAccountTerminate = ISNULL(Record_Count,0) FROM #ValidateAccount WHERE Record_Status = 'D'

	SELECT
		@ValidateAccountTotal = 
			@ValidateAccountActive +
			@ValidateAccountSuspend +
			@ValidateAccountTerminate

	-- ------------------------------------
	-- Total Voucher in Terminated Account 
	-- ------------------------------------
	SELECT
		@TerminateVoucher = SUM(Unit)
	FROM
		#VoucherTransaction
	WHERE
		Is_Terminate = 'Y'

	-- ---------------------------------------
	-- Result to "RpteHSD0001_01_eHA_Summary"
	-- ---------------------------------------
	INSERT INTO RpteHSD0001_01_eHA_Summary(
		System_dtm, report_dtm, 
		AliveAccWithoutClaim, AliveAccWithClaim, AliveTotalAcc, 
		DeceasedAccWithoutClaim, DeceasedAccWithClaim, DeceasedTotalAcc, 
		ValidAcc,
		ValidateAccountActive, ValidateAccountSuspend, ValidateAccountTerminate, ValidateAccountTotal,
		TerminateVoucher, Scheme_Code)
	SELECT	
		GETDATE(), @reportDtm, 
		@WithoutClaim_All_Alive, @WithClaim_All_Alive, @totalVA_Alive, 
		@WithoutClaim_All_Deceased, @WithClaim_All_Deceased, @totalVA_Deceased, 
		@ValidVA,
		ISNULL(@ValidateAccountActive, 0), ISNULL(@ValidateAccountSuspend, 0), ISNULL(@ValidateAccountTerminate, 0), ISNULL(@ValidateAccountTotal, 0),
		ISNULL(@TerminateVoucher, 0), 'ALL'
	
	--

	INSERT INTO RpteHSD0001_01_eHA_Summary(
		System_dtm, report_dtm, 
		AliveAccWithoutClaim, DeceasedAccWithoutClaim, 
		AliveAccWithClaim, DeceasedAccWithClaim, 
		Scheme_Code)
	SELECT 
		GETDATE(), @reportDtm, 
		@WithoutClaim_HCVS_Alive, @WithoutClaim_HCVS_Deceased, 
		@WithClaim_HCVS_Alive,  @WithClaim_HCVS_Deceased, 
		@scheme_code
	
	--
	
	INSERT INTO RpteHSD0001_01_eHA_Summary(
		System_dtm, report_dtm, 
		AliveAccWithoutClaim, DeceasedAccWithoutClaim, 
		AliveAccWithClaim, DeceasedAccWithClaim, 
		Scheme_Code)
	SELECT 
		GETDATE(), @reportDtm, 
		@WithoutClaim_HCVSCHN_Alive, @WithoutClaim_HCVSCHN_Deceased, 
		@WithClaim_HCVSCHN_Alive,  @WithClaim_HCVSCHN_Deceased, 
		@scheme_code_mainland
	   
	INSERT INTO RpteHSD0001_01_eHA_Summary(
		System_dtm, report_dtm, 
		AliveAccWithoutClaim, DeceasedAccWithoutClaim, 
		AliveAccWithClaim, DeceasedAccWithClaim, 
		Scheme_Code)
	SELECT 
		GETDATE(), @reportDtm, 
		@WithoutClaim_HCVSDHC_Alive, @WithoutClaim_HCVSDHC_Deceased, 
		@WithClaim_HCVSDHC_Alive,  @WithClaim_HCVSDHC_Deceased, 
		@scheme_code_DHC

-- +------------------------------------------------------------------------------------------------------------------+
-- |      eHSD0001-02: Report on Available Voucher Summary on eHealth Account (covering Voucher Scheme only)        |
-- +------------------------------------------------------------------------------------------------------------------+

-- Voucher Balance --
-- No. of Voucher Account --
-- No. of Vouchers --

	CREATE TABLE #VoucherRemain
	(
		noOfVoucher				BIGINT,
		noOfVoucherAC			BIGINT,
		noOfVoucher_Alive		BIGINT,
		noOfVoucherAC_Alive		BIGINT,
		noOfVoucher_Deceased	BIGINT,
		noOfVoucherAC_Deceased	BIGINT
	)

	CREATE TABLE #tmp_vtvt_all_ages
	(
		Identity_Num		VARBINARY(100),
		Claim_Age			INT,
		Sum_Unit			INT,
		Availclaim			INT,
		Total_WriteOff_Amt	INT	,
		Balance				INT,
		WriteOff_Exist		BIT DEFAULT 0,
		Deceased			BIT DEFAULT 0 
	)


	CREATE TABLE #vtvt_all_ages
	(
		Identity_Num		VARBINARY(100),
		Claim_Age			INT,
		Sum_Unit			BIGINT,
		AvailClaim			INT,
		Total_WriteOff_Amt	INT,
		Total_Refund_Amt	INT DEFAULT 0,
		Balance				INT,
		WriteOff_Exist		BIT DEFAULT 0 ,
		Deceased			BIT DEFAULT 0,
		Seq_No				INT
	)


-- -------------------------------------
-- Preparation: Table "#vtvt_all_ages" 
-- -------------------------------------
	
	DECLARE @MaxClaimAge INT
	SET @MaxClaimAge = 200
	SET @MaxClaimAge = (SELECT MAX(Age) FROM #age_max_claims)

	-- -------------------------------------------
	-- Get Data into "#tmp_vtvt_all_ages" (Alive)
	-- -------------------------------------------
	-- 1. Update for existing accounts who made transactions (except for the max number of voucher remains,which are those accounts that have not used any voucher at all
	INSERT INTO #tmp_vtvt_all_ages (Identity_Num, Claim_Age, Sum_Unit, Availclaim, Total_WriteOff_Amt, WriteOff_Exist, Deceased)	
	SELECT 
			T.Identity_Num, 
			C.Age,
			0,
			C.MaxClaim,
			T.total_writeoff_amt,
			T.WriteOff_Exist,
			T.Deceased
	FROM #VoucherTransaction T, #age_max_claims C
	WHERE T.Is_Terminate = 'N'
	AND (T.Age = C.Age or (T.Age > @MaxClaimAge  and C.Age= @MaxClaimAge ))
	and T.Deceased = 0


	-- 2. Include those accounts who did not make any transactions
	SET @tempAge = @EligibileAge + 1
	WHILE @tempAge < @MaxClaimAge
	BEGIN	
		--Alive
		INSERT INTO #tmp_vtvt_all_ages (Identity_Num, Claim_Age, Sum_Unit, Availclaim, Total_WriteOff_Amt, WriteOff_Exist, Deceased)	 
		SELECT DISTINCT 
			Identity_num, 
			#age_max_claims.Age, 
			0,
			#age_max_claims.MaxClaim,
			A.Total_WriteOff_Amt,
			A.WriteOff_Exist,
			A.Deceased
		FROM	#Account A, #age_max_claims 
		where	A.Identity_Num NOT IN 
					(select distinct Identity_Num from #VoucherTransaction where Identity_Num IS NOT NULL AND Is_Terminate = 'N')
				AND a.age = @tempAge
				AND a.Is_Terminate = 'N'
				AND #age_max_claims.age = a.age
				AND Deceased = 0	
				
		SELECT @tempAge = @tempAge + 1
	END

			
	-- 3. include those accounts at the max age and those over the max age and did not make any transactions			
	INSERT INTO #tmp_vtvt_all_ages (Identity_Num, Claim_Age, Sum_Unit, Availclaim, Total_WriteOff_Amt, WriteOff_Exist, Deceased)	
	select  Identity_Num, 
			#age_max_claims.age,
			0,
			#age_max_claims.maxclaim,
			A.Total_WriteOff_Amt,
			A.WriteOff_Exist,
			A.Deceased
	from #Account A, #age_max_claims
	where A.Identity_Num NOT IN 
			(SELECT DISTINCT Identity_Num FROM #VoucherTransaction where Identity_Num IS NOT NULL AND Is_Terminate = 'N')
	AND A.Is_Terminate = 'N'
	AND A.Age >= @MaxClaimAge
	AND #age_max_claims.age = @MaxClaimAge	
	AND Deceased = 0	

	-- ---------------------------------------------
	-- Get Data into "#tmp_vtvt_all_ages" (Deceased)
	-- ---------------------------------------------
	-- 1. Update for existing accounts who made transactions (except for the max number of voucher remains,which are those accounts that have not used any voucher at all
	INSERT INTO #tmp_vtvt_all_ages (Identity_Num, Claim_Age, Sum_Unit, Availclaim, Total_WriteOff_Amt, WriteOff_Exist, Deceased)	
	SELECT DISTINCT
		T.Identity_num, 
		A.Age,
		0,
		C.Entitlement,
		T.Total_WriteOff_Amt,
		T.WriteOff_Exist,
		T.Deceased
	FROM 
		#VoucherTransaction T
			INNER JOIN #Account A
				ON T.Identity_Num = A.Identity_Num
					AND A.Deceased = 1
			INNER JOIN (SELECT *, MAX(Age_To) OVER(PARTITION BY Decease_Year ORDER BY Decease_Year) AS Over_Age_Range From #decease_max_claim) C
				ON (A.Logical_DOD >= C.DOD_From AND A.Logical_DOD <=C.DOD_To)
					AND ((A.Age >= C.Age_From AND A.Age <= C.Age_To) OR (A.Age > Over_Age_Range AND C.SEASON = 2009) )
	WHERE 
		T.Is_Terminate = 'N'

	-- 2. Include those accounts who did not make any transactions
	INSERT INTO #tmp_vtvt_all_ages (Identity_Num, Claim_Age, Sum_Unit, Availclaim, Total_WriteOff_Amt, WriteOff_Exist, Deceased)	 
	SELECT DISTINCT 
		Identity_Num, 
		A.Age, 
		0,
		C.Entitlement,
		A.Total_WriteOff_Amt,
		A.WriteOff_Exist,
		A.Deceased
	FROM	
		#Account A 
			INNER JOIN (SELECT *, MAX(Age_To) OVER(PARTITION BY Decease_Year ORDER BY Decease_Year) AS Over_Age_Range From #decease_max_claim) C
				ON (A.Logical_DOD >= C.DOD_From AND A.Logical_DOD <=C.DOD_To)
					AND ((A.Age >= C.Age_From AND A.Age <= C.Age_To) OR (A.Age > Over_Age_Range AND C.SEASON = 2009) )
	WHERE	
		NOT EXISTS
			(SELECT 1 FROM #vouchertransaction WHERE identity_num IS NOT NULL AND Is_Terminate = 'N' AND Identity_Num = A.Identity_Num)
		AND A.Is_Terminate = 'N'
		AND A.Deceased = 1

	-- ----------------------------------------------
	-- From "#tmp_vtvt_all_ages" to "#vtvt_all_ages"
	-- ----------------------------------------------
	-- Alter table add seq_no
	INSERT INTO #vtvt_all_ages (Identity_Num, Claim_Age, Sum_Unit, AvailClaim, Total_WriteOff_Amt, Balance, WriteOff_Exist, Deceased, Seq_No )
	SELECT Identity_Num, Claim_Age, Sum_Unit, Availclaim, Total_WriteOff_Amt, Balance, WriteOff_Exist, Deceased, 
		Seq_no = ROW_NUMBER() OVER (PARTITION BY Identity_Num, Claim_Age ORDER BY WriteOff_Exist Desc)
	FROM #tmp_vtvt_all_ages
              			      
	--delete duplicated
	DELETE FROM  #vtvt_all_ages where seq_no  > 1

	-- ----------------------------------------------
	-- Patch "#vtvt_all_ages" : Update used voucher
	-- ----------------------------------------------					
	UPDATE #vtvt_all_ages SET Sum_Unit = TxCount.Sum_Unit
	FROM
		(
		SELECT Sum_Unit = SUM(CONVERT(BIGINT, unit)), Identity_Num 
		FROM #VoucherTransaction 
		WHERE 
			Identity_Num IS NOT NULL AND Is_Terminate = 'N'  
		GROUP BY Identity_Num
		) TxCount
	WHERE TxCount.Identity_num = #vtvt_all_ages.Identity_Num

	-- ----------------------------------------------
	-- Patch "#vtvt_all_ages" : Update used voucher
	-- ----------------------------------------------					
	UPDATE #vtvt_all_ages SET Total_Refund_Amt = RF.TotalRefund
	FROM
		(
		SELECT TotalRefund = SUM(CONVERT(BIGINT, Refund_Amt)), Encrypt_Field1 AS Identity_Num 
		FROM VoucherRefund
		WHERE Record_Status = 'R'
		GROUP BY Encrypt_Field1
		) RF
	WHERE RF.Identity_num = #vtvt_all_ages.Identity_Num

	-- ------------------------------------------------------
	-- Patch "#vtvt_all_ages" : Caluclate available voucher
	-- ------------------------------------------------------		 
	-- Available voucher = Total Entitlement - Total Used Voucher - Total WriteOff + Total Refund
	UPDATE #vtvt_all_ages SET balance  = ISNULL(AvailClaim,0) - ISNULL(Sum_Unit,0) - ISNULL(Total_Writeoff_Amt,0) + Total_Refund_Amt WHERE WriteOff_Exist = 1
	
	-- Handle negative avilable voucher	
	UPDATE #vtvt_all_ages SET balance = 0 WHERE balance < 0 

-- -------------------------------------
-- Preparation: Table "#VoucherRemain" 
-- -------------------------------------

	-- Get WriteOff Accounts information
	DECLARE @CurrentCeiling INT
	SELECT @CurrentCeiling  = Capping FROM #max_avail_claims WHERE AccYear = (SELECT MAX(AccYear) FROM #max_avail_claims)

	SELECT @noOfVoucher = max(MaxClaim) FROM #max_avail_claims


	-- Initialize the VoucherRemain
	DECLARE @UpperBound int

	IF (@CurrentCeiling is null)
		BEGIN
			SET @UpperBound = @noOfVoucher
		END
	ELSE
		BEGIN
			SET @UpperBound = @CurrentCeiling
		END

	-- ------------------------------------------------------
	-- Build the content of "#VoucherRemain"
	-- ------------------------------------------------------	
	DECLARE @tempUsedVoucher INT

	SET @tempUsedVoucher = 0
	WHILE @tempUsedVoucher <= @UpperBound -- @noOfVoucher
	BEGIN
		INSERT INTO #VoucherRemain(
			noOfVoucher, noOfVoucherAC,
			noOfVoucher_Alive, noOfVoucherAC_Alive,
			noOfVoucher_Deceased, noOfVoucherAC_Deceased)
		values(
			@tempUsedVoucher,0,
			@tempUsedVoucher,0,
			@tempUsedVoucher,0)
	
		SELECT @tempUsedVoucher = @tempUsedVoucher + 1
	END

	-- ------------------------------------------------------
	-- Patch "#VoucherRemain" : Alive + Deceased
	-- ------------------------------------------------------		
	UPDATE #VoucherRemain 
	SET noOfVoucherAC = NumberOfAcc
	FROM 
	(
		SELECT	Bal = balance,
			NumberOfAcc = Count(balance)
		FROM #vtvt_all_ages 
		WHERE WriteOff_Exist = 1
		GROUP BY balance
	) Balance
	WHERE Bal = noOfVoucher

	-- ------------------------------------------------------
	-- Patch "#VoucherRemain" : Alive
	-- ------------------------------------------------------	
	UPDATE #VoucherRemain 
	SET noOfVoucherAC_Alive = NumberOfAcc
	FROM 
	(
		SELECT	Bal = balance,
			NumberOfAcc = Count(balance)
		FROM #vtvt_all_ages 
		WHERE WriteOff_Exist = 1
		AND Deceased = 0
		GROUP BY balance
	) Balance
	WHERE Bal = noOfVoucher_Alive

	-- ------------------------------------------------------
	-- Patch "#VoucherRemain" : Deceased
	-- ------------------------------------------------------	
	UPDATE #VoucherRemain 
	SET noOfVoucherAC_Deceased = NumberOfAcc
	FROM 
	(
		SELECT	Bal = balance,
			NumberOfAcc = Count(balance)
		FROM #vtvt_all_ages 
		WHERE WriteOff_Exist = 1
		AND Deceased = 1
		GROUP BY balance
	) Balance
	WHERE Bal = noOfVoucher_Deceased

-- ------------------------------------------------------
-- Result to "RpteHSD0001_02_AvailableVoucher_Summary"
-- ------------------------------------------------------
	-- Group the summary by range
	DECLARE @SubRpt02_VoucherGroupRange FLOAT
	SELECT @SubRpt02_VoucherGroupRange = 50.0
 
	-- --------------
	-- By Group Range
	-- --------------
    INSERT INTO RpteHSD0001_02_AvailableVoucher_Summary
	(
		system_dtm,
		report_dtm,
		voucherBalance,
		noOfVoucherAC,
		noOfVoucher,
		noOfVoucherAC_Alive,
		noOfVoucher_Alive,
		noOfVoucherAC_Deceased,
		noOfVoucher_Deceased,
		Display_Seq
	)
	SELECT 
		GETDATE(),
		@reportDtm,
		[voucherBalance] = CASE CEILING(noOfvoucher/@SubRpt02_VoucherGroupRange) 
								WHEN 0 THEN CAST(0 AS VARCHAR) 
								ELSE	CAST(CEILING(noOfvoucher/@SubRpt02_VoucherGroupRange) * CAST(@SubRpt02_VoucherGroupRange AS INT) - (CAST(@SubRpt02_VoucherGroupRange AS INT) - 1) AS VARCHAR) 
										+ ' - ' 
										+ CAST(CEILING(noOfvoucher/@SubRpt02_VoucherGroupRange) * CAST(@SubRpt02_VoucherGroupRange AS INT) AS VARCHAR) 
								END,
		[noOfVoucherAC] = SUM(noofVoucherAC),
		[noOfVoucher] = SUM(noOfvoucher * noofVoucherAC) ,
		[noOfVoucherAC_Alive] = SUM(noOfVoucherAC_Alive),
		[noOfVoucher_Alive] = SUM(noOfVoucher_Alive * noOfVoucherAC_Alive) ,
		[noOfVoucherAC_Deceased] = SUM(noOfVoucherAC_Deceased),
		[noOfVoucher_Deceased] = SUM(noOfVoucher_Deceased * noOfVoucherAC_Deceased) ,
		--'02',
		CEILING(noOfvoucher/@SubRpt02_VoucherGroupRange)
	FROM #VoucherRemain
	GROUP BY CEILING(noOfvoucher/@SubRpt02_VoucherGroupRange) ORDER BY CEILING(noOfvoucher/@SubRpt02_VoucherGroupRange)

	-- --------------
	-- Total
	-- --------------
	insert into RpteHSD0001_02_AvailableVoucher_Summary
	(
		system_dtm,
		report_dtm,
		voucherBalance,
		noOfVoucherAC,
		noOfVoucher,
		noOfVoucherAC_Alive,
		noOfVoucher_Alive,
		noOfVoucherAC_Deceased,
		noOfVoucher_Deceased,
		Display_Seq
	)
	SELECT GETDATE(),
			@reportDtm,
			'Total',
			SUM(noOfVoucherAC),
			SUM(noOfVoucher),
			SUM(noOfVoucherAC_Alive),
			SUM(noOfVoucher_Alive),
			SUM(noOfVoucherAC_Deceased),
			SUM(noOfVoucher_Deceased),
			(
			SELECT MAX(Display_Seq) + 1 
			FROM RpteHSD0001_02_AvailableVoucher_Summary 
			WHERE report_dtm = @reportDtm 
			)
	FROM RpteHSD0001_02_AvailableVoucher_Summary
	WHERE report_dtm = @reportDtm

-- +------------------------------------------------------------------------------------------------------------------+
-- |       eHSD0001-03: Report on Write Off Voucher Summary on eHealth Account (covering Voucher Scheme only)         |
-- +------------------------------------------------------------------------------------------------------------------+

create table #VoucherWriteOff
(
	noOfVoucher bigint,
	noOfVoucherAC bigint,
	noOfVoucher_Alive bigint,
	noOfVoucherAC_Alive bigint,
	noOfVoucher_Deceased bigint,
	noOfVoucherAC_Deceased bigint
)

-- initialize the VoucherWriteOff
	DECLARE @tempWriteOffVoucher int
	DECLARE @MaxWriteOff int
	
	SELECT @MaxWriteOff = Max(Total_WriteOff_Amt) FROM #vtvt_all_ages
		
	SET @tempWriteOffVoucher = 0
	WHILE (@tempWriteOffVoucher <= @MaxWriteOff)
	BEGIN

		INSERT INTO #VoucherWriteOff (
		noOfVoucher,
		noOfVoucherAC,
		noOfVoucher_Alive,
		noOfVoucherAC_Alive,
		noOfVoucher_Deceased,
		noOfVoucherAC_Deceased
		) 
		VALUES (
		@tempWriteOffVoucher,0,
		@tempWriteOffVoucher,0,
		@tempWriteOffVoucher,0
		)
		
		SELECT @tempWriteOffVoucher = @tempWriteOffVoucher + 1
	END


	UPDATE #VoucherWriteOff  
	SET noOfVoucherAC = NumberOfAcc 
	FROM
	(SELECT	Bal =  isnull(Total_Writeoff_Amt,0),
			NumberOfAcc = Count(isnull(Total_Writeoff_Amt,0))
	FROM #vtvt_all_ages WHERE WriteOff_Exist = 1	
	GROUP BY isnull(Total_Writeoff_Amt,0)) WriteOffBal
	WHERE Bal = noOfVoucher

	UPDATE #VoucherWriteOff  
	SET noOfVoucherAC_Alive = NumberOfAcc 
	FROM
	(SELECT	Bal =  isnull(Total_Writeoff_Amt,0),
			NumberOfAcc = Count(isnull(Total_Writeoff_Amt,0))
	FROM #vtvt_all_ages 
	WHERE WriteOff_Exist = 1
	AND Deceased = 0	
	GROUP BY isnull(Total_Writeoff_Amt,0)) WriteOffBal
	WHERE Bal = noOfVoucher

	UPDATE #VoucherWriteOff  
	SET noOfVoucherAC_Deceased = NumberOfAcc 
	FROM
	(SELECT	Bal =  isnull(Total_Writeoff_Amt,0),
			NumberOfAcc = Count(isnull(Total_Writeoff_Amt,0))
	FROM #vtvt_all_ages 
	WHERE WriteOff_Exist = 1
	AND Deceased = 1	
	GROUP BY isnull(Total_Writeoff_Amt,0)) WriteOffBal
	WHERE Bal = noOfVoucher


 -- Group the summary by range
 DECLARE @SubRpt03_WriteOffRange float
 SELECT @SubRpt03_WriteOffRange = 50.0
 

--Insert to rpt table

    INSERT INTO RpteHSD0001_03_WriteOffVoucher_Summary
	(
		system_dtm,
		report_dtm,
		voucherBalance,
		noOfVoucherAC,
		noOfVoucher,
		noOfVoucherAC_Alive,
		noOfVoucher_Alive,
		noOfVoucherAC_Deceased,
		noOfVoucher_Deceased,
		--SubReport_ID,
		Display_Seq
	)
	select 
		getdate(),
		@reportDtm,
		[voucherBalance] =	Case Ceiling(noOfvoucher/@SubRpt03_WriteOffRange) 
							when 0 then cast(0 as varchar) 
							else	cast(ceiling(noOfvoucher/@SubRpt03_WriteOffRange) * cast(@SubRpt03_WriteOffRange as int) - (cast(@SubRpt03_WriteOffRange as int) - 1) as varchar) 
									+ ' - ' 
									+ cast(ceiling(noOfvoucher/@SubRpt03_WriteOffRange) * cast(@SubRpt03_WriteOffRange as int) as varchar) 
							end,
		[noOfVoucherAC] = 	sum(noofVoucherAC),
		[noOfVoucher] =		sum(noOfvoucher * noofVoucherAC) ,
		[noOfVoucherAC_Alive] = 	sum(noofVoucherAC_Alive),
		[noOfVoucher_Alive] =		sum(noOfvoucher_Alive * noofVoucherAC_Alive) ,
		[noOfVoucherAC_Deceased] = 	sum(noofVoucherAC_Deceased),
		[noOfVoucher_Deceased] =		sum(noOfvoucher_Deceased * noofVoucherAC_Deceased),
		--'03',
		Ceiling(noOfvoucher/@SubRpt03_WriteOffRange)
	from #VoucherWriteOff
	GROUP BY Ceiling(noOfvoucher/@SubRpt03_WriteOffRange) ORDER BY  1
	
	If NOT EXISTS(SELECT top 1 * FROM #VoucherWriteOff)
	BEGIN 
	
		INSERT INTO RpteHSD0001_03_WriteOffVoucher_Summary
		(
			system_dtm,
			report_dtm,
			voucherBalance,
			noOfVoucherAC,
			noOfVoucher,
			noOfVoucherAC_Alive,
			noOfVoucher_Alive,
			noOfVoucherAC_Deceased,
			noOfVoucher_Deceased,
			--SubReport_ID,
			Display_Seq
		)
		SELECT 
			getdate(),
			@reportDtm,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			--'03',
			1		
	END
	

	INSERT INTO RpteHSD0001_03_WriteOffVoucher_Summary
	(
		system_dtm,
		report_dtm,
		voucherBalance,
		noOfVoucherAC,
		noOfVoucher,
		noOfVoucherAC_Alive,
		noOfVoucher_Alive,
		noOfVoucherAC_Deceased,
		noOfVoucher_Deceased,
		--SubReport_ID,
		Display_Seq
	)
	select getdate(),
			@reportDtm,
			'Total',
			isnull(sum(noOfVoucherAC),0),
			isnull(sum(noOfVoucher),0),
			ISNULL(sum(noOfVoucherAC_Alive),0),
			ISNULL(sum(noOfVoucher_Alive),0),
			ISNULL(sum(noOfVoucherAC_Deceased),0),
			ISNULL(sum(noOfVoucher_Deceased),0),
			--'03',
			(
			SELECT max(Display_Seq) + 1 
			FROM RpteHSD0001_03_WriteOffVoucher_Summary 
			WHERE report_dtm = @reportDtm 
			--AND SubReport_ID = '03'
			)
	from RpteHSD0001_03_WriteOffVoucher_Summary
	where report_dtm = @reportDtm 
	--AND SubReport_ID = '03'


-- +------------------------------------------------------------------------------------------------------------------+
-- |    eHSD0001-04: Report on Total Entitled Voucher Summary on eHealth Account (covering Voucher Scheme only)       |
-- +------------------------------------------------------------------------------------------------------------------+

	CREATE TABLE #AgeVoucherDistribution (
	--age int,
	MaxClaim bigint,
	VoucherAcc bigint,
	VoucherAcc_Alive bigint,
	VoucherAcc_Deceased bigint
	)

--Initialize Table	
	INSERT INTO #AgeVoucherDistribution (
	MaxClaim, VoucherAcc, VoucherAcc_Alive, VoucherAcc_Deceased
	)
	select distinct Entitlement, 0, 0, 0
	from #decease_max_claim	
	order by Entitlement
	
--Update Values		
	UPDATE #AgeVoucherDistribution 
	SET VoucherAcc = isnull(age.VoucherAcc,0)
	FROM
	(	SELECT	--Claim_Age, 
				VoucherAcc = count(Claim_Age), 
				MaxClaim = availclaim 
		FROM 
		 #vtvt_all_ages
		GROUP BY --Claim_Age, 
		availclaim 
		) Age
	WHERE	age.MaxClaim = #AgeVoucherDistribution.MaxClaim	

	UPDATE #AgeVoucherDistribution 
	SET VoucherAcc_Alive = isnull(age.VoucherAcc,0)
	FROM (
		SELECT	VoucherAcc = count(Claim_Age), 
				MaxClaim = availclaim 
		FROM #vtvt_all_ages
		WHERE Deceased = 0
		GROUP BY availclaim 
	) Age
	WHERE		
	age.MaxClaim = #AgeVoucherDistribution.MaxClaim
	
	UPDATE #AgeVoucherDistribution 
	SET VoucherAcc_Deceased = isnull(age.VoucherAcc,0)
	FROM (
		SELECT	VoucherAcc = count(Claim_Age), 
				MaxClaim = availclaim 
		FROM #vtvt_all_ages
		WHERE Deceased = 1
		GROUP BY availclaim 
	) Age
	WHERE		
	age.MaxClaim = #AgeVoucherDistribution.MaxClaim

	--INSERT RESULT
	INSERT INTO RpteHSD0001_04_TotalEntitledVoucher_Summary	
	(
		system_dtm,
		report_dtm,
		voucherBalance,
		noOfVoucherAC,
		noOfVoucher,
		noOfVoucherAC_Alive,
		noOfVoucher_Alive,
		noOfVoucherAC_Deceased,
		noOfVoucher_Deceased,
		Display_Seq
	)
	SELECT 
		getdate(),
		@reportDtm,
		MaxClaim,
		SUM(VoucherAcc),
		MaxClaim * SUM(VoucherAcc),
		SUM(VoucherAcc_Alive),
		MaxClaim * SUM(VoucherAcc_Alive),
		SUM(VoucherAcc_Deceased),
		MaxClaim * SUM(VoucherAcc_Deceased),
		row_number() over (Order by MaxClaim) 
	from #AgeVoucherDistribution
	GROUP BY MaxClaim
	
	INSERT INTO RpteHSD0001_04_TotalEntitledVoucher_Summary	
	(
		system_dtm,
		report_dtm,
		voucherBalance,
		noOfVoucherAC,
		noOfVoucher,
		noOfVoucherAC_Alive,
		noOfVoucher_Alive,
		noOfVoucherAC_Deceased,
		noOfVoucher_Deceased,
		--SubReport_ID,
		Display_Seq
	)
	select getdate(),
			@reportDtm,
			'Total',
			sum(noOfVoucherAC),
			sum(noOfVoucher),
			sum(noOfVoucherAC_Alive),
			sum(noOfVoucher_Alive),
			sum(noOfVoucherAC_Deceased),
			sum(noOfVoucher_Deceased),
			--'04',
			(SELECT max(Display_Seq) + 1 FROM RpteHSD0001_04_TotalEntitledVoucher_Summary
			where report_dtm = @reportDtm 
			--AND SubReport_ID = '04'
			)
	from RpteHSD0001_04_TotalEntitledVoucher_Summary
	where report_dtm = @reportDtm 

-- +------------------------------------------------------------------------------------------------------------------+
-- |                        eHSD0001-05: Report on Voucher Amount Claimed by Profession (HCVS)                        |
-- +------------------------------------------------------------------------------------------------------------------+

-- CRE11-024-02 HCVS Pilot Extension Part 2 [Start] -  added 'ROP' to corresponding code
-- ENU | RCM | RCP | RDT | RMP | RMT | RNU | ROP | ROT | RPT | RRD | Total --

declare @ENU bigint
declare	@RCM bigint
declare	@RCP bigint
declare	@RDT bigint
declare	@RMP bigint
declare	@RMT bigint
declare	@RNU bigint
declare @ROP bigint
declare	@ROT bigint
declare	@RPT bigint
declare	@RRD bigint
declare @total bigint

select 	@ENU = isnull(sum(CONVERT(bigint, unit)),0) from #vouchertransaction where service_type = 'ENU' AND Scheme_Code = 'HCVS'
select	@RCM = isnull(sum(CONVERT(bigint, unit)),0) from #vouchertransaction where service_type = 'RCM' AND Scheme_Code = 'HCVS'
select	@RCP = isnull(sum(CONVERT(bigint, unit)),0) from #vouchertransaction where service_type = 'RCP' AND Scheme_Code = 'HCVS'
select	@RDT = isnull(sum(CONVERT(bigint, unit)),0) from #vouchertransaction where service_type = 'RDT' AND Scheme_Code = 'HCVS'
select	@RMP = isnull(sum(CONVERT(bigint, unit)),0) from #vouchertransaction where service_type = 'RMP' AND Scheme_Code = 'HCVS'
select	@RMT = isnull(sum(CONVERT(bigint, unit)),0) from #vouchertransaction where service_type = 'RMT' AND Scheme_Code = 'HCVS'
select	@RNU = isnull(sum(CONVERT(bigint, unit)),0) from #vouchertransaction where service_type = 'RNU' AND Scheme_Code = 'HCVS'
select	@ROP = isnull(sum(CONVERT(bigint, unit)),0) from #vouchertransaction where service_type = 'ROP' AND Scheme_Code = 'HCVS'
select	@ROT = isnull(sum(CONVERT(bigint, unit)),0) from #vouchertransaction where service_type = 'ROT' AND Scheme_Code = 'HCVS'
select	@RPT = isnull(sum(CONVERT(bigint, unit)),0) from #vouchertransaction where service_type = 'RPT' AND Scheme_Code = 'HCVS'
select	@RRD = isnull(sum(CONVERT(bigint, unit)),0) from #vouchertransaction where service_type = 'RRD' AND Scheme_Code = 'HCVS'
select  @total = isnull(sum(CONVERT(bigint, unit)),0) from #vouchertransaction WHERE Scheme_Code = @scheme_code

insert into RpteHSD0001_05_VoucherClaimByProfSummary
(system_dtm, report_dtm, ENU, RCM, RCP, RDT,	RMP, RMT, RNU, ROP, ROT, RPT, RRD, total)		-- added 'ROP' CRE11-024-02
select getdate(), @reportDtm, @ENU, @RCM, @RCP, @RDT, @RMP, @RMT, @RNU, @ROP, @ROT, @RPT, @RRD, @total
-- CRE11-024-02 HCVS Pilot Extension Part 2 [End]

-- +------------------------------------------------------------------------------------------------------------------+
-- |                 eHSD0001-06: Report on Voucher Amount Claim per Transaction by Profession (HCVS)                 |
-- +------------------------------------------------------------------------------------------------------------------+

-- CRE11-024-02 HCVS Pilot Extension Part 2 [Start] -  added 'ROP' to corresponding code
-- No of voucher claim per transaction | ENU | RCM | RCP | RDT | RMP | RMT | RNU | ROP | ROT | RPT | RRD | Total --

 
 -- Group the summary by range
 
 
 CREATE TABLE #vtvt_trn_count_group
 (
	Scheme_Code		char(10),
	noOfClaim		varchar(20),
	ENU	int,
	RCM	int,
	RCP	int,
	RDT	int,
	RMP	int,
	RMT	int,
	RNU	int,
	ROT	int,
	RPT	int,
	RRD	int,
	ROP	int,
	DIT	int,
	POD	int,
	SPT	int,
	total	int
 )
 
-- prepare temporary table

CREATE TABLE #vtvt_trn_count
(
	Scheme_Code		char(10),
	service_type	char(5),
	unit	int,
	service_type_count int
)

insert into #vtvt_trn_count (Scheme_Code, service_type, unit, service_type_count)
select 
Scheme_Code,
service_type,
unit,
count(service_type)
from #vouchertransaction
WHERE 
	Scheme_Code = @scheme_code OR Scheme_Code = @scheme_code_DHC
group by Scheme_Code, service_type, unit

declare @tempClaimTran as int
set @tempClaimTran = 1
while  @tempClaimTran <= @noOfVoucher
begin
	
	-- HCVS
	select 	@ENU = 0
	select	@RCM = 0
	select	@RCP = 0
	select	@RDT = 0
	select	@RMP = 0
	select	@RMT = 0
	select	@RNU = 0
	select	@ROP = 0		
	select	@ROT = 0
	select	@RPT = 0
	select	@RRD = 0
	select  @total = 0

	select @ENU = ISNULL((select service_type_count from #vtvt_trn_count where service_type='ENU' and unit=@tempClaimTran and Scheme_Code = 'HCVS'),0)
	select @RCM = ISNULL((select service_type_count from #vtvt_trn_count where service_type='RCM' and unit=@tempClaimTran and Scheme_Code = 'HCVS'),0)
	select @RCP = ISNULL((select service_type_count from #vtvt_trn_count where service_type='RCP' and unit=@tempClaimTran and Scheme_Code = 'HCVS'),0)
	select @RDT = ISNULL((select service_type_count from #vtvt_trn_count where service_type='RDT' and unit=@tempClaimTran and Scheme_Code = 'HCVS'),0)
	select @RMP = ISNULL((select service_type_count from #vtvt_trn_count where service_type='RMP' and unit=@tempClaimTran and Scheme_Code = 'HCVS'),0)
	select @RMT = ISNULL((select service_type_count from #vtvt_trn_count where service_type='RMT' and unit=@tempClaimTran and Scheme_Code = 'HCVS'),0)
	select @RNU = ISNULL((select service_type_count from #vtvt_trn_count where service_type='RNU' and unit=@tempClaimTran and Scheme_Code = 'HCVS'),0)
	select @ROP = ISNULL((select service_type_count from #vtvt_trn_count where service_type='ROP' and unit=@tempClaimTran and Scheme_Code = 'HCVS'),0)
	select @ROT = ISNULL((select service_type_count from #vtvt_trn_count where service_type='ROT' and unit=@tempClaimTran and Scheme_Code = 'HCVS'),0)
	select @RPT = ISNULL((select service_type_count from #vtvt_trn_count where service_type='RPT' and unit=@tempClaimTran and Scheme_Code = 'HCVS'),0)
	select @RRD = ISNULL((select service_type_count from #vtvt_trn_count where service_type='RRD' and unit=@tempClaimTran and Scheme_Code = 'HCVS'),0)
	select  @total = ISNULL((select sum(service_type_count) from #vtvt_trn_count where unit = @tempClaimTran and Scheme_Code = 'HCVS'),0)


	insert into #vtvt_trn_count_group
	(
		Scheme_Code,			-- added CRE19-006-02
		noOfClaim,
		ENU,
		RCM,
		RCP,
		RDT,
		RMP,
		RMT,
		RNU,
		ROP,					-- added CRE11-024-02
		ROT,
		RPT,
		RRD,
		total
	)
	values
	(
		@scheme_code,			-- added CRE19-006-02
		@tempClaimTran,
		@ENU,
		@RCM,
		@RCP,
		@RDT,
		@RMP,
		@RMT,
		@RNU,
		@ROP,					-- added CRE11-024-02
		@ROT,
		@RPT,
		@RRD,
		@total
	)



	-- HCVSDHC
	declare @DIT int
	declare @POD int
	declare @SPT int
	
	select 	@DIT = 0
	select	@POD = 0
	select	@SPT = 0
	select  @total = 0

	select @DIT = ISNULL((select service_type_count from #vtvt_trn_count where service_type='DIT' and unit=@tempClaimTran and Scheme_Code = 'HCVSDHC'),0)
	select @POD = ISNULL((select service_type_count from #vtvt_trn_count where service_type='POD' and unit=@tempClaimTran and Scheme_Code = 'HCVSDHC'),0)
	select @SPT = ISNULL((select service_type_count from #vtvt_trn_count where service_type='SPT' and unit=@tempClaimTran and Scheme_Code = 'HCVSDHC'),0)
	select @total = ISNULL((select sum(service_type_count) from #vtvt_trn_count where unit = @tempClaimTran and Scheme_Code = 'HCVSDHC'),0)


	insert into #vtvt_trn_count_group
	(
		Scheme_Code,			-- added CRE19-006-02
		noOfClaim,
		DIT,
		POD,
		SPT,
		total
	)
	values
	(
		@scheme_code_DHC,		
		@tempClaimTran,
		@DIT,
		@POD,
		@SPT,
		@total
	)


	select @tempClaimTran = @tempClaimTran + 1
end

--Group by Range
 DECLARE @SubRpt06_NoClaimRange float
 SELECT @SubRpt06_NoClaimRange = 50.0
 

 --------------------------------------------------------
 -- 06a HCVS
 --------------------------------------------------------
 insert into RpteHSD0001_06a_VoucherClaimPerVoucherSummary
	(
		system_dtm,
		report_dtm,
		noOfClaim,
		ENU,
		RCM,
		RCP,
		RDT,
		RMP,
		RMT,
		RNU,
		ROP,					-- added CRE11-024-02
		ROT,
		RPT,
		RRD,
		total,
		SortOrder				-- added CRP11-009 on 20110908: preserve ordering
	)
 SELECT		getdate(),
			@reportDtm,
			[noOfClaim] =  Case Ceiling(cast(noOfClaim as int)/@SubRpt06_NoClaimRange) 
								when 0 then cast(0 as varchar) 
								else	cast(ceiling(cast(noOfClaim as int)/@SubRpt06_NoClaimRange) * cast(@SubRpt06_NoClaimRange as int) - (cast(@SubRpt06_NoClaimRange as int) - 1) as varchar) 
										+ ' - ' 
										+ cast(ceiling(cast(noOfClaim as int)/@SubRpt06_NoClaimRange) * cast(@SubRpt06_NoClaimRange as int) as varchar) 
								end,
			sum(ENU),
			sum(RCM),
			sum(RCP),
			sum(RDT),
			sum(RMP),
			sum(RMT),
			sum(RNU),
			sum(ROP),					-- added CRE11-024-02
			sum(ROT),
			sum(RPT),
			sum(RRD),
			sum(total),
			Ceiling(cast(noOfClaim as int)/@SubRpt06_NoClaimRange)
	FROM	#vtvt_trn_count_group
	WHERE	cast(noOfClaim as int) <= @UpperBound
			AND Scheme_Code = @scheme_code
	GROUP By Ceiling(cast(noOfClaim as int)/@SubRpt06_NoClaimRange) ORDER BY Ceiling(cast(noOfClaim as int)/@SubRpt06_NoClaimRange)
	
 insert into RpteHSD0001_06a_VoucherClaimPerVoucherSummary
	(
		system_dtm,
		report_dtm,
		noOfClaim,
		ENU,
		RCM,
		RCP,
		RDT,
		RMP,
		RMT,
		RNU,
		ROP,				
		ROT,
		RPT,
		RRD,
		total,
		SortOrder
	)
 SELECT		getdate(),
			@reportDtm,
			'> ' + cast(@UpperBound as varchar),
			sum(ENU),
			sum(RCM),
			sum(RCP),
			sum(RDT),
			sum(RMP),
			sum(RMT),
			sum(RNU),
			sum(ROP),					-- added CRE11-024-02
			sum(ROT),
			sum(RPT),
			sum(RRD),
			sum(total),
			Ceiling((@UpperBound + 1)/@SubRpt06_NoClaimRange)
	FROM	#vtvt_trn_count_group
	WHERE	cast(noOfClaim as int) > @UpperBound
			AND Scheme_Code = @scheme_code
-- Calculate Total row

declare @ENU_T int
declare	@RCM_T int
declare	@RCP_T int
declare	@RDT_T int
declare	@RMP_T int
declare	@RMT_T int
declare	@RNU_T int
declare	@ROP_T int				-- added CRE11-024-02
declare	@ROT_T int
declare	@RPT_T int
declare	@RRD_T int
declare @total_T int

	SELECT @ENU_T = ISNULL(SUM(ENU), 0) FROM #vtvt_trn_count_group WHERE Scheme_Code = @scheme_code
	SELECT @RCM_T = ISNULL(SUM(RCM), 0) FROM #vtvt_trn_count_group WHERE Scheme_Code = @scheme_code 
	SELECT @RCP_T = ISNULL(SUM(RCP), 0) FROM #vtvt_trn_count_group WHERE Scheme_Code = @scheme_code 
	SELECT @RDT_T = ISNULL(SUM(RDT), 0) FROM #vtvt_trn_count_group WHERE Scheme_Code = @scheme_code 
	SELECT @RMP_T = ISNULL(SUM(RMP), 0) FROM #vtvt_trn_count_group WHERE Scheme_Code = @scheme_code 
	SELECT @RMT_T = ISNULL(SUM(RMT), 0) FROM #vtvt_trn_count_group WHERE Scheme_Code = @scheme_code 
	SELECT @RNU_T = ISNULL(SUM(RNU), 0) FROM #vtvt_trn_count_group WHERE Scheme_Code = @scheme_code 
	SELECT @ROP_T = ISNULL(SUM(ROP), 0) FROM #vtvt_trn_count_group WHERE Scheme_Code = @scheme_code 
	SELECT @ROT_T = ISNULL(SUM(ROT), 0) FROM #vtvt_trn_count_group WHERE Scheme_Code = @scheme_code 
	SELECT @RPT_T = ISNULL(SUM(RPT), 0) FROM #vtvt_trn_count_group WHERE Scheme_Code = @scheme_code 
	SELECT @RRD_T = ISNULL(SUM(RRD), 0) FROM #vtvt_trn_count_group WHERE Scheme_Code = @scheme_code 
	SELECT @total_T = ISNULL(SUM(total), 0) FROM #vtvt_trn_count_group WHERE Scheme_Code = @scheme_code 

	insert into RpteHSD0001_06a_VoucherClaimPerVoucherSummary
	(
		system_dtm,
		report_dtm,
		noOfClaim,
		ENU,
		RCM,
		RCP,
		RDT,
		RMP,
		RMT,
		RNU,
		ROP,					-- added CRE11-024-02
		ROT,
		RPT,
		RRD,
		total,
		SortOrder				-- added CRP11-009 on 20110908: preserve ordering
	)
	SELECT
		getdate(),
		@reportDtm,
		'Total',
		@ENU_T,
		@RCM_T,
		@RCP_T,
		@RDT_T,
		@RMP_T,
		@RMT_T,
		@RNU_T,
		@ROP_T,					-- added CRE11-024-02
		@ROT_T,
		@RPT_T,
		@RRD_T,
		@total_T,
		--@tempClaimTran			-- added CRP11-009 on 20110908: preserve ordering
		(SELECT max(SortOrder) +1  FROM RpteHSD0001_06a_VoucherClaimPerVoucherSummary WHERE  report_dtm = @reportDtm )




-------------------------------------------------------------------
-- 06b HCVSDHC
-------------------------------------------------------------------
DECLARE @UpperBound_DHC int 

SELECT @UpperBound_DHC = MAX(Claim_Amt_Max) FROM ProfessionDHC WHERE Service_Category_Code IN ('DIT','POD','SPT')

IF @UpperBound_DHC > @UpperBound
BEGIN
	SET @UpperBound_DHC = @UpperBound
END

 insert into RpteHSD0001_06b_VoucherClaimPerVoucherSummary
	(
		system_dtm,
		report_dtm,
		noOfClaim,
		DIT,
		POD,
		SPT,
		total,
		SortOrder				-- added CRP11-009 on 20110908: preserve ordering
	)
 SELECT		getdate(),
			@reportDtm,
			[noOfClaim] =  Case Ceiling(cast(noOfClaim as int)/@SubRpt06_NoClaimRange) 
								when 0 then cast(0 as varchar) 
								else	cast(ceiling(cast(noOfClaim as int)/@SubRpt06_NoClaimRange) * cast(@SubRpt06_NoClaimRange as int) - (cast(@SubRpt06_NoClaimRange as int) - 1) as varchar) 
										+ ' - ' 
										+ cast(ceiling(cast(noOfClaim as int)/@SubRpt06_NoClaimRange) * cast(@SubRpt06_NoClaimRange as int) as varchar) 
								end,
			sum(DIT),
			sum(POD),
			sum(SPT),
			sum(total),
			Ceiling(cast(noOfClaim as int)/@SubRpt06_NoClaimRange)
	FROM	#vtvt_trn_count_group
	WHERE	cast(noOfClaim as int) <= @UpperBound_DHC
			AND Scheme_Code = @scheme_code_DHC
	GROUP By Ceiling(cast(noOfClaim as int)/@SubRpt06_NoClaimRange) ORDER BY Ceiling(cast(noOfClaim as int)/@SubRpt06_NoClaimRange)
	
 insert into RpteHSD0001_06b_VoucherClaimPerVoucherSummary
	(
		system_dtm,
		report_dtm,
		noOfClaim,
		DIT,
		POD,
		SPT,
		total,
		SortOrder
	)
 SELECT		getdate(),
			@reportDtm,
			'> ' + cast(@UpperBound_DHC as varchar),
			sum(DIT),
			sum(POD),
			sum(SPT),
			sum(total),
			Ceiling((@UpperBound_DHC + 1)/@SubRpt06_NoClaimRange)
	FROM	#vtvt_trn_count_group
	WHERE	cast(noOfClaim as int) > @UpperBound_DHC
			AND Scheme_Code = @scheme_code_DHC
-- Calculate Total row

declare @DIT_T int
declare	@POD_T int
declare	@SPT_T int
declare @DHC_total_T int

	SELECT @DIT_T = ISNULL(SUM(DIT), 0) FROM #vtvt_trn_count_group WHERE Scheme_Code = @scheme_code_DHC
	SELECT @POD_T = ISNULL(SUM(POD), 0) FROM #vtvt_trn_count_group WHERE Scheme_Code = @scheme_code_DHC 
	SELECT @SPT_T = ISNULL(SUM(SPT), 0) FROM #vtvt_trn_count_group WHERE Scheme_Code = @scheme_code_DHC 
	SELECT @DHC_total_T = ISNULL(SUM(total), 0) FROM #vtvt_trn_count_group WHERE Scheme_Code = @scheme_code_DHC

	insert into RpteHSD0001_06b_VoucherClaimPerVoucherSummary
	(
		system_dtm,
		report_dtm,
		noOfClaim,
		DIT,
		POD,
		SPT,
		total,
		SortOrder				-- added CRP11-009 on 20110908: preserve ordering
	)
	SELECT
		getdate(),
		@reportDtm,
		'Total',
		@DIT_T,
		@POD_T,
		@SPT_T,
		@DHC_total_T,
		--@tempClaimTran			-- added CRP11-009 on 20110908: preserve ordering
		(SELECT max(SortOrder) +1  FROM RpteHSD0001_06b_VoucherClaimPerVoucherSummary WHERE  report_dtm = @reportDtm )


-- +------------------------------------------------------------------------------------------------------------------+
-- |      eHSD0001-07a: Report on Number of Claim Transactions by Profession and Reason for Visit Level 1 (HCVS)       |
-- +------------------------------------------------------------------------------------------------------------------+

-- CRE11-024-02 HCVS Pilot Extension Part 2 [Start] -  added 'ROP' to corresponding code
-- Reason for Visit (Level 1) | ENU | RCM | RCP | RDT | RMP | RMT | RNU | ROP | ROT | RPT | RRD | Total --
-- CRE11-024-02 HCVS Pilot Extension Part 2 [End]

create table #r1
(
	reason_code int,
	reason varchar(50)
)

insert into #r1
(
	reason_code,
	reason
)
select distinct reason_l1_code, reason_l1 from ReasonForVisitL1

declare @r1 as int
set @r1 = 1

declare @r1Count as int
select @r1Count = count(1) from #r1

-- prepare temporary table

CREATE TABLE #vtvt_trn_count2
(
	service_type char(5),
	Reason_for_visit_L1 smallint,
	service_type_count int	
)

insert into #vtvt_trn_count2 (service_type, Reason_for_visit_L1, service_type_count)
select 
service_type,
Reason_for_visit_L1,
count(service_type)
from #vouchertransaction
WHERE Scheme_Code = 'HCVS'
group by service_type, Reason_for_visit_L1

declare @r1_desc as varchar(50)
while  @r1 <= @r1Count
begin
	
	select @r1_desc = ''
	select 	@ENU = 0
	select	@RCM = 0
	select	@RCP = 0
	select	@RDT = 0
	select	@RMP = 0
	select	@RMT = 0
	select	@RNU = 0
	select	@ROP = 0			-- added CRE11-024-02
	select	@ROT = 0
	select	@RPT = 0
	select	@RRD = 0
	select  @total = 0

	select	@r1_desc = reason from #r1 where reason_code = @r1

	select @ENU = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='ENU' and Reason_for_visit_L1=@r1),0)
	select @RCM = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='RCM' and Reason_for_visit_L1=@r1),0)
	select @RCP = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='RCP' and Reason_for_visit_L1=@r1),0)
	select @RDT = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='RDT' and Reason_for_visit_L1=@r1),0)
	select @RMP = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='RMP' and Reason_for_visit_L1=@r1),0)
	select @RMT = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='RMT' and Reason_for_visit_L1=@r1),0)
	select @RNU = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='RNU' and Reason_for_visit_L1=@r1),0)
	select @ROP = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='ROP' and Reason_for_visit_L1=@r1),0)		-- added CRE11-024-02
	select @ROT = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='ROT' and Reason_for_visit_L1=@r1),0)
	select @RPT = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='RPT' and Reason_for_visit_L1=@r1),0)
	select @RRD = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='RRD' and Reason_for_visit_L1=@r1),0)

	-- uses sum because the counting has been done in the grouping in the temp table
	select	@total = sum(service_type_count) from #vtvt_trn_count2 where Reason_for_visit_L1 = @r1
	
	insert into RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary
	(
		system_dtm,
		report_dtm,
		reason,
		ENU,
		RCM,
		RCP,
		RDT,
		RMP,
		RMT,
		RNU,
		ROP,				-- added CRE11-024-02
		ROT,
		RPT,
		RRD,
		total,
		SortOrder,			-- added CRP11-009 on 20110908: preserve ordering
		Secondary
	)
	values
	(
		getdate(),
		@reportDtm,
		@r1_desc,
		@ENU,
		@RCM,
		@RCP,
		@RDT,
		@RMP,
		case @r1
			when 4 then -1
			else @RMT
		end,
		@RNU,
		@ROP,				-- added CRE11-024-02
		@ROT,
		@RPT,
		@RRD,
		@total,
		@r1,				-- added CRP11-009 on 20110908: preserve ordering
		'N'					-- added CRE11-024-02
	)

	select @r1 = @r1 + 1
end


-- CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
-- ------------------------------------------------ --
-- Calculation for the Defer Input

select 	@ENU = 0
select	@RCM = 0
select	@RCP = 0
select	@RDT = 0
select	@RMP = 0
select	@RMT = 0
select	@RNU = 0
select	@ROP = 0
select	@ROT = 0
select	@RPT = 0
select	@RRD = 0
select  @total = 0

declare @TempR1 int
select @TempR1=@r1
select @r1 = 0
select @ENU = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='ENU' and Reason_for_visit_L1=@r1),0)
select @RCM = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='RCM' and Reason_for_visit_L1=@r1),0)
select @RCP = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='RCP' and Reason_for_visit_L1=@r1),0)
select @RDT = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='RDT' and Reason_for_visit_L1=@r1),0)
select @RMP = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='RMP' and Reason_for_visit_L1=@r1),0)
select @RMT = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='RMT' and Reason_for_visit_L1=@r1),0)
select @RNU = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='RNU' and Reason_for_visit_L1=@r1),0)
select @ROP = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='ROP' and Reason_for_visit_L1=@r1),0)		-- added CRE11-024-02
select @ROT = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='ROT' and Reason_for_visit_L1=@r1),0)
select @RPT = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='RPT' and Reason_for_visit_L1=@r1),0)
select @RRD = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='RRD' and Reason_for_visit_L1=@r1),0)
--select	@total = sum(service_type_count) from #vtvt_trn_count2 where Reason_for_visit_L1 = @r1									-- CRE11-024-02 comment out
select	@total = ISNULL((select sum(service_type_count) from #vtvt_trn_count2 where Reason_for_visit_L1 = @r1),0)					-- added CRE11-024-02

-- Insert for the Defer Input
select @r1_desc='Defer Input'
select @r1=@TempR1
insert into RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary
	(
		system_dtm,
		report_dtm,
		reason,
		ENU,
		RCM,
		RCP,
		RDT,
		RMP,
		RMT,
		RNU,
		ROP,
		ROT,
		RPT,
		RRD,
		total,
		SortOrder,
		Secondary
	)
	values
	(
		getdate(),
		@reportDtm,
		@r1_desc,
		@ENU,
		@RCM,
		@RCP,
		@RDT,
		@RMP,
		@RMT,
		@RNU,
		@ROP,
		@ROT,
		@RPT,
		@RRD,
		@total,
		@r1,
		'N'			
	)
-- ---------------------------------------------- --
-- CRE11-024-02 HCVS Pilot Extension Part 2 [End]

select @r1=@r1+1			-- added CRE11-024-02
select @ENU_T = 0
select @RCM_T = 0
select @RCP_T = 0
select @RDT_T = 0
select @RMP_T = 0
select @RMT_T = 0
select @RNU_T = 0
select @ROP_T = 0			-- added CRE11-024-02
select @ROT_T = 0
select @RPT_T = 0
select @RRD_T = 0
select @total_T = 0

	SELECT @ENU_T = ISNULL(SUM(service_type_count), 0) FROM #vtvt_trn_count2 WHERE service_type = 'ENU'
	SELECT @RCM_T = ISNULL(SUM(service_type_count), 0) FROM #vtvt_trn_count2 WHERE service_type = 'RCM'
	SELECT @RCP_T = ISNULL(SUM(service_type_count), 0) FROM #vtvt_trn_count2 WHERE service_type = 'RCP'
	SELECT @RDT_T = ISNULL(SUM(service_type_count), 0) FROM #vtvt_trn_count2 WHERE service_type = 'RDT'
	SELECT @RMP_T = ISNULL(SUM(service_type_count), 0) FROM #vtvt_trn_count2 WHERE service_type = 'RMP'
	SELECT @RMT_T = ISNULL(SUM(service_type_count), 0) FROM #vtvt_trn_count2 WHERE service_type = 'RMT'
	SELECT @RNU_T = ISNULL(SUM(service_type_count), 0) FROM #vtvt_trn_count2 WHERE service_type = 'RNU'
	SELECT @ROP_T = ISNULL(SUM(service_type_count), 0) FROM #vtvt_trn_count2 WHERE service_type = 'ROP'
	SELECT @ROT_T = ISNULL(SUM(service_type_count), 0) FROM #vtvt_trn_count2 WHERE service_type = 'ROT'
	SELECT @RPT_T = ISNULL(SUM(service_type_count), 0) FROM #vtvt_trn_count2 WHERE service_type = 'RPT'
	SELECT @RRD_T = ISNULL(SUM(service_type_count), 0) FROM #vtvt_trn_count2 WHERE service_type = 'RRD'
	SELECT @total_T = ISNULL(SUM(service_type_count), 0) FROM #vtvt_trn_count2 

	insert into RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary
	(
		system_dtm,
		report_dtm,
		reason,
		ENU,
		RCM,
		RCP,
		RDT,
		RMP,
		RMT,
		RNU,
		ROP,				-- added CRE11-024-02
		ROT,
		RPT,
		RRD,
		total,
		SortOrder,			-- added CRP11-009 on 20110908: preserve ordering
		Secondary
	)
	values
	(
		getdate(),
		@reportDtm,
		'Total',
		@ENU_T,
		@RCM_T,
		@RCP_T,
		@RDT_T,
		@RMP_T,
		@RMT_T,
		@RNU_T,
		@ROP_T,				-- added CRE11-024-02
		@ROT_T,
		@RPT_T,
		@RRD_T,
		@total_T,
		@r1, 				-- added CRP11-009 on 20110908: preserve ordering; value changed from 5 to @r1 for CRE11-024-02
		'N'					-- added CRE11-024-02
	)



-- CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
-- ------------------------------------------------ --
-- Calculation for Secondary Reason For Visit

CREATE TABLE #vtvt_trn_count3
(
	service_type char(5),
	ReasonForVisit_S_L1 smallint,
	service_type_count int	,
	v_type char(1)
)

--helen for count secondary reason for visits
insert into #vtvt_trn_count3 (service_type, ReasonForVisit_S_L1, service_type_count,v_type)
select 
service_type,
Reasonforvisit_S1_L1,
count(service_type),'1'
from #vouchertransaction
where ReasonForVisit_S1_L1 > 0
		AND Scheme_Code = 'HCVS'
group by service_type, ReasonForVisit_S1_L1

insert into #vtvt_trn_count3 (service_type, ReasonForVisit_S_L1, service_type_count,v_type)
select 
service_type,
Reasonforvisit_S2_L1,
count(service_type),'2'
from #vouchertransaction
where ReasonForVisit_S2_L1 > 0
		AND Scheme_Code = 'HCVS'
group by service_type, ReasonForVisit_S2_L1

insert into #vtvt_trn_count3 (service_type, ReasonForVisit_S_L1, service_type_count,v_type)
select 
service_type,
Reasonforvisit_S3_L1,
count(service_type),'3'
from #vouchertransaction
where ReasonForVisit_S3_L1 > 0
		AND Scheme_Code = 'HCVS'
group by service_type, ReasonForVisit_S3_L1


declare @r_tmp int
select @r_tmp=@r1
set @r1=1
while  @r1 <= @r1Count
begin
	
	select @r1_desc = ''
	select 	@ENU = 0
	select	@RCM = 0
	select	@RCP = 0
	select	@RDT = 0
	select	@RMP = 0
	select	@RMT = 0
	select	@RNU = 0
	select	@ROP = 0
	select	@ROT = 0
	select	@RPT = 0
	select	@RRD = 0
	select  @total = 0

	select	@r1_desc = reason from #r1 where reason_code = @r1
	
	select @ENU = ISNULL((select sum(service_type_count) from #vtvt_trn_count3 where service_type='ENU' and ReasonForVisit_S_L1=@r1 group by service_type,ReasonForVisit_S_L1),0)
	select @RCM = ISNULL((select sum(service_type_count) from #vtvt_trn_count3 where service_type='RCM' and ReasonForVisit_S_L1=@r1 group by service_type,ReasonForVisit_S_L1),0)
	select @RCP = ISNULL((select sum(service_type_count) from #vtvt_trn_count3 where service_type='RCP' and ReasonForVisit_S_L1=@r1 group by service_type,ReasonForVisit_S_L1),0)
	select @RDT = ISNULL((select sum(service_type_count) from #vtvt_trn_count3 where service_type='RDT' and ReasonForVisit_S_L1=@r1 group by service_type,ReasonForVisit_S_L1),0)
	select @RMP = ISNULL((select sum(service_type_count) from #vtvt_trn_count3 where service_type='RMP' and ReasonForVisit_S_L1=@r1 group by service_type,ReasonForVisit_S_L1),0)
	select @RMT = ISNULL((select sum(service_type_count) from #vtvt_trn_count3 where service_type='RMT' and ReasonForVisit_S_L1=@r1 group by service_type,ReasonForVisit_S_L1),0)
	select @RNU = ISNULL((select sum(service_type_count) from #vtvt_trn_count3 where service_type='RNU' and ReasonForVisit_S_L1=@r1 group by service_type,ReasonForVisit_S_L1),0)
	select @ROP = ISNULL((select sum(service_type_count) from #vtvt_trn_count3 where service_type='ROP' and ReasonForVisit_S_L1=@r1 group by service_type,ReasonForVisit_S_L1),0)
	select @ROT = ISNULL((select sum(service_type_count) from #vtvt_trn_count3 where service_type='ROT' and ReasonForVisit_S_L1=@r1 group by service_type,ReasonForVisit_S_L1),0)
	select @RPT = ISNULL((select sum(service_type_count) from #vtvt_trn_count3 where service_type='RPT' and ReasonForVisit_S_L1=@r1 group by service_type,ReasonForVisit_S_L1),0)
	select @RRD = ISNULL((select sum(service_type_count) from #vtvt_trn_count3 where service_type='RRD' and ReasonForVisit_S_L1=@r1 group by service_type,ReasonForVisit_S_L1),0)
	select	@total = ISNULL((Select sum(service_type_count) from #vtvt_trn_count3 where ReasonForVisit_S_L1 = @r1 group by ReasonForVisit_S_L1), 0)
	insert into RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary
	(
		system_dtm,
		report_dtm,
		reason,
		ENU,
		RCM,
		RCP,
		RDT,
		RMP,
		RMT,
		RNU,
		ROP,
		ROT,
		RPT,
		RRD,
		total,
		SortOrder,
		Secondary
	)
	values
	(
		getdate(),
		@reportDtm,
		@r1_desc,
		@ENU,
		@RCM,
		@RCP,
		@RDT,
		@RMP,
		case @r1
			when 4 then -1
			else @RMT
		end,
		@RNU,
		@ROP,
		@ROT,
		@RPT,
		@RRD,
		@total,
		@r_tmp + @r1,
		'Y'		
	)

	select @r1 = @r1 + 1
end

select @r1=@r_tmp+@r1

-- prepare total for secondary Reason for Visit
select @ENU_T = 0
select @RCM_T = 0
select @RCP_T = 0
select @RDT_T = 0
select @RMP_T = 0
select @RMT_T = 0
select @RNU_T = 0
select @ROP_T = 0
select @ROT_T = 0
select @RPT_T = 0
select @RRD_T = 0
select @total_T = 0

Select @ENU_T = (select IsNULL(sum(ENU), 0) from RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary where report_dtm= @reportDtm and Secondary='Y') 
Select @RCM_T = (select IsNULL(sum(RCM), 0) from RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary where report_dtm= @reportDtm and Secondary='Y')
Select @RCP_T = (select IsNULL(sum(RCP), 0) from RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary where report_dtm= @reportDtm and Secondary='Y')
Select @RDT_T = (select IsNULL(sum(RDT), 0) from RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary where report_dtm= @reportDtm and Secondary='Y')
Select @RMP_T = (select IsNULL(sum(RMP), 0) from RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary where report_dtm= @reportDtm and Secondary='Y')
Select @RMT_T = (select IsNULL(sum(RMT), 0) from RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary where report_dtm= @reportDtm and RMT>0 and Secondary='Y')
Select @RNU_T = (select IsNULL(sum(RNU), 0) from RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary where report_dtm= @reportDtm and Secondary='Y')
Select @ROP_T = (select IsNULL(sum(ROP), 0) from RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary where report_dtm= @reportDtm and Secondary='Y')
Select @ROT_T = (select IsNULL(sum(ROT), 0) from RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary where report_dtm= @reportDtm and Secondary='Y')
Select @RPT_T = (select IsNULL(sum(RPT), 0) from RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary where report_dtm= @reportDtm and Secondary='Y')
Select @RRD_T = (select IsNULL(sum(RRD), 0) from RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary where report_dtm= @reportDtm and Secondary='Y')
Select @total_T = (select IsNULL(sum(total), 0) from RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary where report_dtm= @reportDtm and Secondary='Y')

	insert into RpteHSD0001_07a_VoucherClaimPerReasonForVisitSummary
	(
		system_dtm,
		report_dtm,
		reason,
		ENU,
		RCM,
		RCP,
		RDT,
		RMP,
		RMT,
		RNU,
		ROP,
		ROT,
		RPT,
		RRD,
		total,
		SortOrder,
		Secondary
	)
	values
	(
		getdate(),
		@reportDtm,
		'Total',
		@ENU_T,
		@RCM_T,
		@RCP_T,
		@RDT_T,
		@RMP_T,
		@RMT_T,
		@RNU_T,
		@ROP_T,
		@ROT_T,
		@RPT_T,
		@RRD_T,
		@total_T,
		@r1,
		'Y'
	)



-- +------------------------------------------------------------------------------------------------------------------+
-- |      eHSD0001-07b: Report on Number of Claim Transactions by Profession and Reason for Visit Level 1 (HCVSDHC)   |
-- +------------------------------------------------------------------------------------------------------------------+

-- Reason for Visit (Level 1) | DIT | POD | SPT | Total --

set @r1 = 1
select @r1Count = count(1) from #r1

-- prepare temporary table

DELETE #vtvt_trn_count2
DELETE #vtvt_trn_count3

insert into #vtvt_trn_count2 (service_type, Reason_for_visit_L1, service_type_count)
select 
service_type,
Reason_for_visit_L1,
count(service_type)
from #vouchertransaction
WHERE Scheme_Code = @scheme_code_DHC
group by service_type, Reason_for_visit_L1

while  @r1 <= @r1Count
begin
	
	select @r1_desc = ''
	select @DIT = 0
	select @POD = 0
	select @SPT = 0
	select @total = 0

	select	@r1_desc = reason from #r1 where reason_code = @r1

	select @DIT = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='DIT' and Reason_for_visit_L1=@r1),0)
	select @POD = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='POD' and Reason_for_visit_L1=@r1),0)
	select @SPT = ISNULL((select service_type_count from #vtvt_trn_count2 where service_type='SPT' and Reason_for_visit_L1=@r1),0)

	-- uses sum because the counting has been done in the grouping in the temp table
	select	@total = ISNULL((SELECT sum(service_type_count) from #vtvt_trn_count2 where Reason_for_visit_L1 = @r1), 0)
		
	insert into RpteHSD0001_07b_VoucherClaimPerReasonForVisitSummary
	(
		system_dtm,
		report_dtm,
		reason,
		DIT,
		POD,
		SPT,
		total,
		SortOrder,			-- added CRP11-009 on 20110908: preserve ordering
		Secondary
	)
	values
	(
		getdate(),
		@reportDtm,
		@r1_desc,
		@DIT,
		@POD,
		@SPT,
		@total,
		@r1,				-- added CRP11-009 on 20110908: preserve ordering
		'N'					-- added CRE11-024-02
	)

	select @r1 = @r1 + 1
end

-- ---------------------------------------------- --

select @r1=@r1+1			-- added CRE11-024-02
select @DIT_T = 0
select @POD_T = 0
select @SPT_T = 0
select @total_T = 0

	SELECT @DIT_T = ISNULL(SUM(service_type_count), 0) FROM #vtvt_trn_count2 WHERE service_type = 'DIT'
	SELECT @POD_T = ISNULL(SUM(service_type_count), 0) FROM #vtvt_trn_count2 WHERE service_type = 'POD'
	SELECT @SPT_T = ISNULL(SUM(service_type_count), 0) FROM #vtvt_trn_count2 WHERE service_type = 'SPT'

	SELECT @total_T = ISNULL(SUM(service_type_count), 0) FROM #vtvt_trn_count2 

	insert into RpteHSD0001_07b_VoucherClaimPerReasonForVisitSummary
	(
		system_dtm,
		report_dtm,
		reason,
		DIT,
		POD,
		SPT,
		total,
		SortOrder,			-- added CRP11-009 on 20110908: preserve ordering
		Secondary
	)
	values
	(
		getdate(),
		@reportDtm,
		'Total',
		@DIT_T,
		@POD_T,
		@SPT_T,
		@total_T,
		@r1, 				-- added CRP11-009 on 20110908: preserve ordering; value changed from 5 to @r1 for CRE11-024-02
		'N'					-- added CRE11-024-02
	)


-- Calculation for Secondary Reason For Visit


--helen for count secondary reason for visits
insert into #vtvt_trn_count3 (service_type, ReasonForVisit_S_L1, service_type_count,v_type)
select 
service_type,
Reasonforvisit_S1_L1,
count(service_type),'1'
from #vouchertransaction
where ReasonForVisit_S1_L1 > 0
		AND Scheme_Code = @scheme_code_DHC
group by service_type, ReasonForVisit_S1_L1

insert into #vtvt_trn_count3 (service_type, ReasonForVisit_S_L1, service_type_count,v_type)
select 
service_type,
Reasonforvisit_S2_L1,
count(service_type),'2'
from #vouchertransaction
where ReasonForVisit_S2_L1 > 0
		AND Scheme_Code = @scheme_code_DHC
group by service_type, ReasonForVisit_S2_L1

insert into #vtvt_trn_count3 (service_type, ReasonForVisit_S_L1, service_type_count,v_type)
select 
service_type,
Reasonforvisit_S3_L1,
count(service_type),'3'
from #vouchertransaction
where ReasonForVisit_S3_L1 > 0
		AND Scheme_Code = @scheme_code_DHC
group by service_type, ReasonForVisit_S3_L1


select @r_tmp=@r1
set @r1=1
while  @r1 <= @r1Count
begin
	
	select @r1_desc = ''
	select @DIT = 0
	select @POD = 0
	select @SPT = 0
	select @total = 0

	select	@r1_desc = reason from #r1 where reason_code = @r1
	
	select @DIT = ISNULL((select sum(service_type_count) from #vtvt_trn_count3 where service_type='DIT' and ReasonForVisit_S_L1=@r1 group by service_type,ReasonForVisit_S_L1),0)
	select @POD = ISNULL((select sum(service_type_count) from #vtvt_trn_count3 where service_type='POD' and ReasonForVisit_S_L1=@r1 group by service_type,ReasonForVisit_S_L1),0)
	select @SPT = ISNULL((select sum(service_type_count) from #vtvt_trn_count3 where service_type='SPT' and ReasonForVisit_S_L1=@r1 group by service_type,ReasonForVisit_S_L1),0)
	select @total = ISNULL((Select sum(service_type_count) from #vtvt_trn_count3 where ReasonForVisit_S_L1 = @r1 group by ReasonForVisit_S_L1), 0)
	
	insert into RpteHSD0001_07b_VoucherClaimPerReasonForVisitSummary
	(
		system_dtm,
		report_dtm,
		reason,
		DIT,
		POD,
		SPT,
		total,
		SortOrder,
		Secondary
	)
	values
	(
		getdate(),
		@reportDtm,
		@r1_desc,
		@DIT,
		@POD,
		@SPT,
		@total,
		@r_tmp + @r1,
		'Y'		
	)

	select @r1 = @r1 + 1
end

select @r1=@r_tmp+@r1

-- prepare total for secondary Reason for Visit
select @DIT_T = 0
select @POD_T = 0
select @SPT_T = 0
select @total_T = 0

Select @DIT_T = (select IsNULL(sum(DIT), 0) from RpteHSD0001_07b_VoucherClaimPerReasonForVisitSummary where report_dtm= @reportDtm and Secondary='Y') 
Select @POD_T = (select IsNULL(sum(POD), 0) from RpteHSD0001_07b_VoucherClaimPerReasonForVisitSummary where report_dtm= @reportDtm and Secondary='Y')
Select @SPT_T = (select IsNULL(sum(SPT), 0) from RpteHSD0001_07b_VoucherClaimPerReasonForVisitSummary where report_dtm= @reportDtm and Secondary='Y')

Select @total_T = (select IsNULL(sum(total), 0) from RpteHSD0001_07b_VoucherClaimPerReasonForVisitSummary where report_dtm= @reportDtm and Secondary='Y')

	insert into RpteHSD0001_07b_VoucherClaimPerReasonForVisitSummary
	(
		system_dtm,
		report_dtm,
		reason,
		DIT,
		POD,
		SPT,
		total,
		SortOrder,
		Secondary
	)
	values
	(
		getdate(),
		@reportDtm,
		'Total',
		@DIT_T,
		@POD_T,
		@SPT_T,
		@total_T,
		@r1,
		'Y'
	)


-- +------------------------------------------------------------------------------------------------------------------+
-- |                            eHSD0001-08a: Raw data of Voucher Claim Transactions (HCVS)                            |
-- +------------------------------------------------------------------------------------------------------------------+

--Transaction ID | Transaction Time | Voucher Claimed | Profession | District | District Board | Area --
create table #transaction
(
	transaction_id char(20),
	transaction_dtm datetime,	
	service_type char(5),
	sp_id char(8),
	practice_display_seq smallint,
	practice_name nvarchar(100),
	practice_name_chi nvarchar(100),
	Unit int,
	Co_Payment varchar(50),
	district char(4) collate database_default,
	district_name char(15) collate database_default,
	district_board char(15) collate database_default,
	area_name char(50) collate database_default,
	address_code int,
	Transaction_Status char(1),
	Reimbursement_Status char(1),
	Create_By_SmartID char(1),
	HKIC_Symbol char(1),
	OCSSS_Ref_Status char(1),
	Manual_Reimburse char(1),
	DHC_Service		char(1)
)

CREATE INDEX IX_VAT on #transaction (transaction_id)

insert into #transaction
(
	transaction_id,
	transaction_dtm,	
	service_type,
	sp_id,
	practice_display_seq,
	practice_name,
	practice_name_chi,
	Unit,
	Co_Payment,
	district,
	address_code,
	Transaction_Status,
	Reimbursement_Status,
	Create_By_SmartID,
	HKIC_Symbol,
	OCSSS_Ref_Status,
	Manual_Reimburse,
	DHC_Service
)
select v.transaction_id,
	v.transaction_dtm,	
	v.service_type,
	v.sp_id,
	v.practice_display_seq,
	p.practice_name,
	p.practice_name_chi,
	v.Unit,
	v.Co_Payment,
	p.district,
	p.address_code,
	v.Record_Status AS [Transaction_Status],
	NULL AS [Reimbursement_Status],
	v.Create_By_SmartID,
	v.HKIC_Symbol,
	v.OCSSS_Ref_Status,
	v.Manual_Reimburse,
	v.DHC_Service
from #vouchertransaction v, practice p
where v.sp_id = p.sp_id  collate database_default
and v.practice_Display_Seq = p.display_seq
and v.transaction_dtm between dateadd(day, -7, @cutOffDtm) and @cutOffDtm
AND v.Scheme_Code = 'HCVS'

-- Patch the Reimbursement_Status 
-- for transaction created in payment outside eHS

UPDATE
	#transaction
SET
	Reimbursement_Status = 'R'
WHERE
	Transaction_Status = 'R'
  

-- Patch the Reimbursement_Status
          
UPDATE
	#transaction
SET
	Reimbursement_Status = CASE RAT.Authorised_Status
							WHEN 'R' THEN 'G'
							ELSE RAT.Authorised_Status
						   END
FROM
	#transaction VT
		INNER JOIN ReimbursementAuthTran RAT
			ON VT.Transaction_ID = RAT.Transaction_ID
WHERE
	VT.Transaction_Status = 'A'


-- Patch the Transaction_Status
          
UPDATE
	#transaction
SET
	Transaction_Status = 'R'
WHERE
	Reimbursement_Status = 'G'



DECLARE	@record_id int,
		@address_eng varchar(255),
		@address_chi nvarchar(255),
		@district_code char(5),
		@eh_eng varchar(255),
		@eh_chi varchar(255),
		@display_seq smallint,
		@sp_id varchar(8)

DECLARE avail_cursor cursor 
FOR	SELECT address_code, practice_display_seq, sp_id
FROM #transaction

OPEN avail_cursor 
FETCH next FROM avail_cursor INTO @record_id, @display_seq, @sp_id
WHILE @@Fetch_status = 0
BEGIN
	if @record_id IS NOT null
	BEGIN
		SELECT	@address_eng = '',
				@address_chi = '',
				@district_code = '',
				@eh_eng = '',
				@eh_chi = ''

		exec cpi_get_address_detail   @record_id 
								, @address_eng = @address_eng  OUTPUT 
    							, @address_chi = @address_chi    OUTPUT 
								, @district_code = @district_code    OUTPUT 
								, @eh_eng = @eh_eng	OUTPUT
								, @eh_chi = @eh_chi	OUTPUT

	UPDATE #transaction
	SET	District = @district_code
	WHERE Practice_display_seq = @display_seq
			and sp_id = @sp_id
	END

	FETCH next FROM avail_cursor INTO @record_id, @display_seq, @sp_id
END

CLOSE avail_cursor 
DEALLOCATE avail_cursor

UPDATE
	#transaction
SET
	#transaction.district_name = district.district_name,
	#transaction.district_board = district.district_board,
	#transaction.area_name = district_area.area_name
FROM
	district
		INNER JOIN DistrictBoard
			ON district.District_Board = DistrictBoard.District_Board
		INNER JOIN district_area
			ON DistrictBoard.area_code = district_area.area_code
WHERE
	#transaction.district = district.district_code collate database_default
		
--

insert into RpteHSD0001_08a_TransactionSummary
(
	System_dtm,
	report_dtm,
	transaction_id,
	transaction_dtm,
	voucher_claim,
	Co_Payment,
	service_type,	
	district_name,
	district_board,
	area_name,
    Create_By_SmartID,
	SP_ID,
	Practice_Display_Seq,
	Practice_Name,
	Practice_Name_Chi,
	Transaction_Status,
	Reimbursement_Status,
	HKIC_Symbol,
	OCSSS_Ref_Status,
	Manual_Reimburse,
	DHC_Service
)
select getdate(),
		@reportDtm,
		transaction_id,
		transaction_dtm,
		Unit,
		ISNULL(Co_Payment, ''),
		service_type,
		district_name,
		district_board,
		area_name,
		case when Create_By_SmartID = 'Y' THEN 'Card Reader' ELSE 'Manual' END Create_By_SmartID,
		sp_id,
		practice_display_seq,
		practice_name,
		practice_name_chi,
		SD1.Status_Description,
		ISNULL(SD2.Status_Description, ''),
		[HKIC_Symbol] = ISNULL(SD3.Status_Description, @Str_NA),		
		OCSSS_Ref_Status,
		Manual_Reimburse,
		ISNULL(DHC_Service, 'N')
from #transaction
	INNER JOIN StatusData SD1          
		ON Transaction_Status = SD1.Status_Value          
			AND SD1.Enum_Class = 'ClaimTransStatus'    
	LEFT JOIN StatusData SD2          
		ON Reimbursement_Status = SD2.Status_Value          
			AND SD2.Enum_Class = 'ReimbursementStatus' 
	LEFT JOIN StatusData SD3
		ON HKIC_Symbol = SD3.Status_Value 
			AND SD3.Enum_Class = 'HKICSymbol'			   



-- +------------------------------------------------------------------------------------------------------------------+
-- |                            eHSD0001-08b: Raw data of Voucher Claim Transactions (HCVSDHC)                            |
-- +------------------------------------------------------------------------------------------------------------------+

--Transaction ID | Transaction Time | Voucher Claimed | Profession | District | District Board | Area --
create table #HCVSDHCtransaction
(
	transaction_id char(20),
	transaction_dtm datetime,	
	service_type char(5),
	sp_id char(8),
	practice_display_seq smallint,
	practice_name nvarchar(100),
	practice_name_chi nvarchar(100),
	Unit int,
	Co_Payment varchar(50),
	district char(4) collate database_default,
	district_name char(15) collate database_default,
	district_board char(15) collate database_default,
	area_name char(50) collate database_default,
	address_code int,
	Transaction_Status char(1),
	Reimbursement_Status char(1),
	Create_By_SmartID char(1),
	HKIC_Symbol char(1),
	OCSSS_Ref_Status char(1),
	Manual_Reimburse char(1)
)

CREATE INDEX IX_VAT on #HCVSDHCtransaction (transaction_id)

insert into #HCVSDHCtransaction
(
	transaction_id,
	transaction_dtm,	
	service_type,
	sp_id,
	practice_display_seq,
	practice_name,
	practice_name_chi,
	Unit,
	Co_Payment,
	district,
	address_code,
	Transaction_Status,
	Reimbursement_Status,
	Create_By_SmartID,
	HKIC_Symbol,
	OCSSS_Ref_Status,
	Manual_Reimburse
)
select v.transaction_id,
	v.transaction_dtm,	
	v.service_type,
	v.sp_id,
	v.practice_display_seq,
	p.practice_name,
	p.practice_name_chi,
	v.Unit,
	v.Co_Payment,
	p.district,
	p.address_code,
	v.Record_Status AS [Transaction_Status],
	NULL AS [Reimbursement_Status],
	v.Create_By_SmartID,
	v.HKIC_Symbol,
	v.OCSSS_Ref_Status,
	v.Manual_Reimburse
from #vouchertransaction v, practice p
where v.sp_id = p.sp_id  collate database_default
and v.practice_Display_Seq = p.display_seq
and v.transaction_dtm between dateadd(day, -7, @cutOffDtm) and @cutOffDtm
AND v.Scheme_Code = 'HCVSDHC'


-- Patch the Reimbursement_Status 
-- for transaction created in payment outside eHS

UPDATE
	#HCVSDHCtransaction
SET
	Reimbursement_Status = 'R'
WHERE
	Transaction_Status = 'R'
  

-- Patch the Reimbursement_Status
          
UPDATE
	#HCVSDHCtransaction
SET
	Reimbursement_Status = CASE RAT.Authorised_Status
							WHEN 'R' THEN 'G'
							ELSE RAT.Authorised_Status
						   END
FROM
	#HCVSDHCtransaction VT
		INNER JOIN ReimbursementAuthTran RAT
			ON VT.Transaction_ID = RAT.Transaction_ID
WHERE
	VT.Transaction_Status = 'A'


-- Patch the Transaction_Status
          
UPDATE
	#HCVSDHCtransaction
SET
	Transaction_Status = 'R'
WHERE
	Reimbursement_Status = 'G'



DECLARE avail_cursor cursor 
FOR	SELECT address_code, practice_display_seq, sp_id
FROM #HCVSDHCtransaction

OPEN avail_cursor 
FETCH next FROM avail_cursor INTO @record_id, @display_seq, @sp_id
WHILE @@Fetch_status = 0
BEGIN
	if @record_id IS NOT null
	BEGIN
		SELECT	@address_eng = '',
				@address_chi = '',
				@district_code = '',
				@eh_eng = '',
				@eh_chi = ''

		exec cpi_get_address_detail   @record_id 
								, @address_eng = @address_eng  OUTPUT 
    							, @address_chi = @address_chi    OUTPUT 
								, @district_code = @district_code    OUTPUT 
								, @eh_eng = @eh_eng	OUTPUT
								, @eh_chi = @eh_chi	OUTPUT

	UPDATE #HCVSDHCtransaction
	SET	District = @district_code
	WHERE Practice_display_seq = @display_seq
			and sp_id = @sp_id
	END

	FETCH next FROM avail_cursor INTO @record_id, @display_seq, @sp_id
END

CLOSE avail_cursor 
DEALLOCATE avail_cursor

UPDATE
	#HCVSDHCtransaction
SET
	#HCVSDHCtransaction.district_name = district.district_name,
	#HCVSDHCtransaction.district_board = district.district_board,
	#HCVSDHCtransaction.area_name = district_area.area_name
FROM
	district
		INNER JOIN DistrictBoard
			ON district.District_Board = DistrictBoard.District_Board
		INNER JOIN district_area
			ON DistrictBoard.area_code = district_area.area_code
WHERE
	#HCVSDHCtransaction.district = district.district_code collate database_default
		
--

insert into RpteHSD0001_08b_HCVSDHCTransactionSummary
(
	System_dtm,
	report_dtm,
	transaction_id,
	transaction_dtm,
	voucher_claim,
	Co_Payment,
	service_type,	
	district_name,
	district_board,
	area_name,
    Create_By_SmartID,
	SP_ID,
	Practice_Display_Seq,
	Practice_Name,
	Practice_Name_Chi,
	Transaction_Status,
	Reimbursement_Status,
	HKIC_Symbol,
	OCSSS_Ref_Status,
	Manual_Reimburse
)
select getdate(),
		@reportDtm,
		transaction_id,
		transaction_dtm,
		Unit,
		ISNULL(Co_Payment, ''),
		service_type,
		district_name,
		district_board,
		area_name,
		case when Create_By_SmartID = 'Y' THEN 'Card Reader' ELSE 'Manual' END Create_By_SmartID,
		sp_id,
		practice_display_seq,
		practice_name,
		practice_name_chi,
		SD1.Status_Description,
		ISNULL(SD2.Status_Description, ''),
		[HKIC_Symbol] = ISNULL(SD3.Status_Description, @Str_NA),	
		OCSSS_Ref_Status,
		Manual_Reimburse
from #HCVSDHCtransaction
	INNER JOIN StatusData SD1          
		ON Transaction_Status = SD1.Status_Value          
			AND SD1.Enum_Class = 'ClaimTransStatus'    
	LEFT JOIN StatusData SD2          
		ON Reimbursement_Status = SD2.Status_Value          
			AND SD2.Enum_Class = 'ReimbursementStatus'    
	LEFT JOIN StatusData SD3
		ON HKIC_Symbol = SD3.Status_Value 
			AND SD3.Enum_Class = 'HKICSymbol'

-- +------------------------------------------------------------------------------------------------------------------+
-- |                          eHSD0001-08c: Raw data of Voucher Claim Transactions (HCVSCHN)                           |
-- +------------------------------------------------------------------------------------------------------------------+

create table #HCVSCHNTransaction
(
	transaction_id char(20),
	transaction_dtm datetime,	
	service_type char(5),
	sp_id char(8),
	practice_display_seq smallint,
	practice_name nvarchar(100),
	practice_name_chi nvarchar(100),
	Total_Amount_HKD money,
	Total_Amount_RMB money,
	Conversion_Rate	 decimal(9, 3),
	Co_Payment_RMB varchar(50),
	Payment_Type varchar(20),
	district char(4) collate database_default,
	district_name char(15) collate database_default,
	district_board char(15) collate database_default,
	area_name char(50) collate database_default,
	address_code int,
	Transaction_Status char(1),
	Reimbursement_Status char(1),
	Create_By_SmartID char(1),
	HKIC_Symbol char(1),
	OCSSS_Ref_Status char(1),
	Manual_Reimburse char(1)
)

CREATE INDEX IX_VAT on #HCVSCHNTransaction (transaction_id)

insert into #HCVSCHNTransaction
(
	transaction_id,
	transaction_dtm,	
	service_type,
	sp_id,
	practice_display_seq,
	practice_name,
	practice_name_chi,
	Total_Amount_HKD,
	Total_Amount_RMB,
	Conversion_Rate,
	Co_Payment_RMB,
	Payment_Type,
	district,
	address_code,
	Transaction_Status,
	Reimbursement_Status,
	Create_By_SmartID,
	HKIC_Symbol,
	OCSSS_Ref_Status,
	Manual_Reimburse
)
select v.transaction_id,
	v.transaction_dtm,	
	v.service_type,
	v.sp_id,
	v.practice_display_seq,
	p.practice_name,
	p.practice_name_chi,
	v.Unit,
	v.Total_Amount_RMB,
	v.Conversion_Rate,
	v.Co_Payment_RMB,
	v.Payment_Type,
	p.district,
	p.address_code,
	v.Record_Status AS [Transaction_Status],
	NULL AS [Reimbursement_Status],
	v.Create_By_SmartID,
	v.HKIC_Symbol,
	v.OCSSS_Ref_Status,
	v.Manual_Reimburse
from #vouchertransaction v, practice p
where v.sp_id = p.sp_id  collate database_default
and v.practice_Display_Seq = p.display_seq
and v.transaction_dtm between dateadd(day, -7, @cutOffDtm) and @cutOffDtm
AND v.Scheme_Code = 'HCVSCHN'

-- Patch the Reimbursement_Status 
-- for transaction created in payment outside eHS

UPDATE
	#HCVSCHNTransaction
SET
	Reimbursement_Status = 'R'
WHERE
	Transaction_Status = 'R'
  

-- Patch the Reimbursement_Status
          
UPDATE
	#HCVSCHNTransaction
SET
	Reimbursement_Status = CASE RAT.Authorised_Status
							WHEN 'R' THEN 'G'
							ELSE RAT.Authorised_Status
						   END
FROM
	#HCVSCHNTransaction VT
		INNER JOIN ReimbursementAuthTran RAT
			ON VT.Transaction_ID = RAT.Transaction_ID
WHERE
	VT.Transaction_Status = 'A'


-- Patch the Transaction_Status
          
UPDATE
	#HCVSCHNTransaction
SET
	Transaction_Status = 'R'
WHERE
	Reimbursement_Status = 'G'



DECLARE avail_cursor cursor 
FOR	SELECT address_code, practice_display_seq, sp_id
FROM #HCVSCHNTransaction

OPEN avail_cursor 
FETCH next FROM avail_cursor INTO @record_id, @display_seq, @sp_id
WHILE @@Fetch_status = 0
BEGIN
	if @record_id IS NOT null
	BEGIN
		SELECT	@address_eng = '',
				@address_chi = '',
				@district_code = '',
				@eh_eng = '',
				@eh_chi = ''

		exec cpi_get_address_detail   @record_id 
								, @address_eng = @address_eng  OUTPUT 
    							, @address_chi = @address_chi    OUTPUT 
								, @district_code = @district_code    OUTPUT 
								, @eh_eng = @eh_eng	OUTPUT
								, @eh_chi = @eh_chi	OUTPUT

	UPDATE #HCVSCHNTransaction
	SET	District = @district_code
	WHERE Practice_display_seq = @display_seq
			and sp_id = @sp_id
	END

	FETCH next FROM avail_cursor INTO @record_id, @display_seq, @sp_id
END

CLOSE avail_cursor 
DEALLOCATE avail_cursor

UPDATE
	#HCVSCHNTransaction
SET
	#HCVSCHNTransaction.district_name = district.district_name,
	#HCVSCHNTransaction.district_board = district.district_board,
	#HCVSCHNTransaction.area_name = district_area.area_name
FROM
	district
		INNER JOIN DistrictBoard
			ON district.District_Board = DistrictBoard.District_Board
		INNER JOIN district_area
			ON DistrictBoard.area_code = district_area.area_code
WHERE
	#HCVSCHNTransaction.district = district.district_code collate database_default

--

insert into RpteHSD0001_08c_HCVSCHNTransactionSummary
(
	System_dtm,
	report_dtm,
	transaction_id,
	transaction_dtm,
	Total_Amount_HKD,
	Total_Amount_RMB,
	Conversion_Rate,
	Co_Payment_RMB,
	Payment_Type,
	service_type,
	district_name,
	district_board,
	area_name,
    Create_By_SmartID,
	SP_ID,
	Practice_Display_Seq,
	Practice_Name,
	Practice_Name_Chi,
	Transaction_Status,
	Reimbursement_Status,
	HKIC_Symbol,
	OCSSS_Ref_Status,
	Manual_Reimburse
)
select getdate(),
		@reportDtm,
		transaction_id,
		transaction_dtm,
		Total_Amount_HKD,
		Total_Amount_RMB,
		Conversion_Rate,
		Co_Payment_RMB,
		ST.Data_Value,
		service_type,
		district_name,
		district_board,
		area_name,
		case when Create_By_SmartID = 'Y' THEN 'Card Reader' ELSE 'Manual' END Create_By_SmartID,
		sp_id,
		practice_display_seq,
		practice_name,
		practice_name_chi,
		SD1.Status_Description,
		ISNULL(SD2.Status_Description, ''),
		[HKIC_Symbol] = ISNULL(SD3.Status_Description, @Str_NA),	
		OCSSS_Ref_Status,
		Manual_Reimburse
from #HCVSCHNTransaction
	INNER JOIN StatusData SD1          
		ON Transaction_Status = SD1.Status_Value          
			AND SD1.Enum_Class = 'ClaimTransStatus'
	INNER JOIN StaticData ST
		ON Payment_Type = ST.Item_No
			AND ST.Column_Name = 'HCVSCHN_PAYMENTTYPE'
	LEFT JOIN StatusData SD2          
		ON Reimbursement_Status = SD2.Status_Value          
			AND SD2.Enum_Class = 'ReimbursementStatus'    
	LEFT JOIN StatusData SD3
		ON HKIC_Symbol = SD3.Status_Value 
			AND SD3.Enum_Class = 'HKICSymbol'

-- ---------------------------------------------
-- Drop the temporary tables
-- ---------------------------------------------
	
	DROP TABLE #VoucherTransaction
	DROP TABLE #Account
	--DROP TABLE #WriteOffAccount
	DROP TABLE #ValidateAccount
	DROP TABLE #VoucherRemain
	DROP TABLE #tmp_vtvt_all_ages	
	DROP TABLE #vtvt_all_ages
	DROP TABLE #max_avail_claims
	DROP TABLE #age_max_claims
	DROP TABLE #Deceased_eligible
	DROP TABLE #decease_max_claim
	DROP TABLE #VoucherWriteOff
	DROP TABLE #vtvt_trn_count
	DROP TABLE #r1
	DROP TABLE #vtvt_trn_count2
	DROP TABLE #vtvt_trn_count3
	DROP TABLE #transaction
	DROP TABLE #HCVSCHNTransaction
	DROP TABLE #AgeVoucherDistribution
	DROP TABLE #vtvt_trn_count_group
	DROP TABLE #HCVSDHCTransaction

	SELECT 'S' AS [Result]

END
GO

GRANT EXECUTE ON proc_EHS_VoucherAccClaim_Stat_Schedule TO HCVU
GO


