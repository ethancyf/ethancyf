IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Prepare_RptAccountSFC]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Prepare_RptAccountSFC]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	16 Jul 2020
-- CR. No			INT20-0025
-- Description:		(1) Add WITH (NOLOCK)
-- =============================================
-- ==============================================
-- Modification History
-- Modified by:		Winnie SUEN	
-- Modified date:	11 Sep 2019
-- CR No.			CRE19-006 (DHC)
-- Description:		Including scheme [HCVSDHC]
-- ===============================================
---- =============================================
---- Modification History
---- Created by:		Marco CHOI
---- Creation date:		01 Jan 2018
---- Version:			
---- CR No.:			CRE14-016
---- Description:		- Add output column
----					- change DOD data source
----					- modified 2017 eligible age data source
----					- add generation check
---- =============================================
---- =============================================
---- Modification History
---- Created by:		Koala CHENG
---- Creation date:		30 Jun 2017
---- Version:			
---- CR No.:			CRE16-025-03
---- Description:		- Filter out account/transaction if [Create_Dtm] < cut off date
---- =============================================
---- =============================================
---- Modification History
---- Modified by:		Marco CHOI
---- Modified date:		14 May 2017
---- Version:			
---- CR No.:			CRD16-025-03
---- Description:		- Modify taking scheme rule seq logic
----					- #AccountFinal data output change to AccountSFC table
----					- Modify #Account Join DeathRecordEntry method
----					- remove Insert Special Account into #Account 
----					- Modify Account_Started_Dtm update script logic with new 65 age voucher claim rule on 2017-Jul-1
----					- Modify logic of removing deceased
----					- tune Store Proc performance
----						- Remove generated voucher transaction to physical table
----						- Remove #VoucherTransactionFinal
----						- Remove Indices
----						- Remove unnecessary output column
---- =============================================
CREATE PROCEDURE [dbo].[proc_Prepare_RptAccountSFC] (
	@cutOffDtm DATETIME,
	@forceExecute BIT = 0
)
AS
BEGIN
-- =============================================
-- Generation Check
-- =============================================
	DECLARE @RptAccountSFC_GenDate DATETIME

	SET @RptAccountSFC_GenDate = (SELECT CONVERT(DATETIME, Variable_Value) 
									FROM SystemVariable WITH (NOLOCK)
									WHERE Variable_ID = 'AccountSFC_Generation_Check') 
	
	IF @RptAccountSFC_GenDate < @cutoffDtm OR @forceExecute = 1
	BEGIN
-- =============================================
-- Declaration
-- =============================================
		DECLARE @reportDtm DATETIME
		SET @reportDtm = DATEADD(day, -1, @cutOffDtm)

		DECLARE @eligibleAge int

		DECLARE @2017_1st_EligibleAge int	
		DECLARE @2017_2nd_EligibleAge int	

		DECLARE @65ClaimRuleEffDtm VARCHAR(10)	
		SET @65ClaimRuleEffDtm = (SELECT CONVERT(DATE, Value) 
									FROM EligibilityRule WITH (NOLOCK) 
									WHERE [Type] = 'SERVICEDTM' 
										AND Scheme_Code = 'HCVS' 
										AND Rule_Group_Code <> 'G0001')	

		DECLARE @scheme_code CHAR(10)
		SET @scheme_code = 'HCVS'
	
		DECLARE @scheme_code_mainland CHAR(10)
		SET @scheme_code_mainland = 'HCVSCHN'
		
		DECLARE @scheme_code_DHC CHAR(10)
		SET @scheme_code_DHC = 'HCVSDHC'


		DECLARE @rule_name AS varchar(20)
		DECLARE @type AS char(10)
		SET @rule_name = 'MinAge'
		SET @type = 'AGE'

		DECLARE @Current_Scheme_Seq int

		TRUNCATE TABLE RptAccountSFC

		--RAISERROR ('Deleted old data in stat tables' , 0, 1) WITH NOWAIT

-- =============================================
-- Create temporary table
-- =============================================
		CREATE TABLE #VoucherTransaction (
			Transaction_ID			CHAR(20),
			Transaction_Dtm			DATETIME,
			Service_Receive_Dtm		DATETIME,
			Voucher_Acc_ID			CHAR(15),
			Temp_Voucher_Acc_ID		CHAR(15),
			Service_Type			CHAR(5),
			SP_ID					CHAR(8),
			Practice_Display_Seq	SMALLINT,
			Doc_Code				CHAR(20),
			Identity_Num			VARBINARY(100),
			Age						INT,
			DOB						DATETIME,	
			Exact_DOB				CHAR(1),
			Create_By_SmartID		CHAR(1),
			SourceApp				VARCHAR(10),
			Is_Terminate			CHAR(1),
			Scheme_Code				CHAR(10)
		)
--
		CREATE TABLE #Account (
			Voucher_Acc_ID			CHAR(15),
			Temp_Voucher_Acc_ID		CHAR(15),
			Identity_Num			VARBINARY(100),
			Doc_Code				CHAR(20),
			DOB						DATETIME,	
			Exact_DOB				CHAR(1),	
			Eligible				CHAR(1),
			Age						INT,
			Create_Dtm				DATETIME,		
			Account_Start_Dtm		DATETIME,
			Is_Terminate			CHAR(1),
			Record_Status			CHAR(1),		
			First_Tx_Dtm			DATETIME,
			DOD						DATETIME,
			Exact_DOD				CHAR(1),
			Logical_DOD				DATETIME,
			Deceased				BIT DEFAULT 0
		)
	--
		CREATE TABLE #AccountFinal (
			Voucher_Acc_ID			CHAR(15),
			Temp_Voucher_Acc_ID		CHAR(15),
			Identity_Num			VARBINARY(100),
			Doc_Code				CHAR(20),
			DOB						DATETIME,	
			Exact_DOB				CHAR(1),	
			Eligible				CHAR(1),
			Age						INT,
			Create_Dtm				DATETIME,		
			Account_Start_Dtm		DATETIME,
			Is_Terminate			CHAR(1),
			Record_Status			CHAR(1),		
			First_Tx_Dtm			DATETIME,
			Logical_DOD				DATETIME,
			Deceased				BIT DEFAULT 0,	
			Acc_Seq					INT
		)
		
--
	
		CREATE TABLE #TerminateAccount (
			Identity_Num			VARBINARY(100),
			Doc_Code				CHAR(20)
		)

--
	
		CREATE TABLE #Deceased_Eligible
		(
			Season					SMALLINT, 
			SubSeq					INT, 
			Age						INT,
			Claim_Period_From		DATETIME,
			Last_Service_Dtm		DATETIME,
			Num_Subsidize			INT
		)

-- ======================================================
-- Initialization: Variables and Data Tables 
-- ======================================================

		----------------------------------------------
		-- Prepare Terminated Account Table
		----------------------------------------------	
		INSERT INTO #TerminateAccount (
			Identity_Num,
			Doc_Code
		)
		SELECT DISTINCT
			[Identity_Num] = P.Encrypt_Field1,
			[Doc_Code] = CASE P.Doc_Code WHEN 'HKBC' THEN 'HKIC' ELSE P.Doc_Code END 
		FROM
			VoucherAccount VA WITH (NOLOCK)
			INNER JOIN PersonalInformation P WITH (NOLOCK)
				ON VA.Voucher_Acc_ID = P.Voucher_Acc_ID
		WHERE
			VA.Record_Status = 'D' -- Terminated

		--RAISERROR ('Get terminated account completed', 0, 1) WITH NOWAIT

		----------------------------------------------
		-- Get Current Eligible Age For Claim Voucher 
		----------------------------------------------
		SELECT	
			@2017_1st_EligibleAge = [Value] 
		FROM	
			EligibilityRule WITH (NOLOCK)
		WHERE	
			Scheme_Code = @scheme_code
			AND Scheme_Seq = 10
			AND Rule_Name = @rule_name
			AND [Type] = @type
			AND Rule_Group_Code = 'G0001'

		SELECT	
			@2017_2nd_EligibleAge = [Value] 
		FROM	
			EligibilityRule WITH (NOLOCK)
		WHERE	
			Scheme_Code = @scheme_code
			AND Scheme_Seq = 10
			AND Rule_Name = @rule_name
			AND [Type] = @type
			AND Rule_Group_Code = 'G0002'
			
		SELECT	
			@current_scheme_seq = MAX(Scheme_Seq)								
		FROM 
			SubsidizeGroupClaim WITH (NOLOCK)
		WHERE	
			Scheme_Code = @scheme_code
			AND Record_Status = 'A'
			AND Claim_Period_FROM <= @reportDtm
			AND DATEADD(dd, 1, Last_Service_Dtm) > @reportDtm	

		SELECT
			@eligibleAge = (
				CASE WHEN @reportDtm >= @65ClaimRuleEffDtm  
				THEN MIN ([Value] )	--64
				ELSE MAX(value)	--69
				END
			)
		FROM
			EligibilityRule WITH (NOLOCK)
		WHERE
			Scheme_Code = @scheme_code
				AND Rule_Name = @rule_name
				AND [Type] = @type
				AND Scheme_Seq = @current_scheme_seq

		--RAISERROR ( 'Get eligibility rule completed', 0, 1) WITH NOWAIT

		------------------------------------------------
		-- Prepare Eligible Mapping Table For Deceased
		------------------------------------------------
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

		INSERT INTO #Deceased_Eligible
		(
			Season,
			SubSeq,
			Age,
			Claim_Period_From,
			Last_Service_Dtm,
			Num_Subsidize
		)
		SELECT
			SGC.Scheme_Seq as Season,
			ROW_NUMBER() OVER(PARTITION BY ER.Scheme_Seq ORDER BY ER.Scheme_Seq) AS SubSeq,
			CONVERT(INT, ER.Value) as Age,
			CASE WHEN ER2.Value IS NULL THEN SGC.Claim_Period_From ELSE CONVERT(DATETIME, ER2.Value) END,
			SGC.Last_Service_Dtm,
			SGC.Num_Subsidize
		FROM 
			SchemeClaim SC WITH (NOLOCK)
				INNER JOIN SubsidizeGroupClaim SGC WITH (NOLOCK)
					ON SC.Scheme_Code = SGC.Scheme_Code
						AND SGC.Scheme_Code = 'HCVS'
						AND SGC.Record_Status <> 'I'
				LEFT JOIN 
					(SELECT 
						* 
					FROM 
						EligibilityRule WITH (NOLOCK) 
					WHERE 
						Rule_Name = 'MinAge' 
						AND [Type] = 'Age'
					) ER 
						ON SGC.Scheme_Code = ER.Scheme_Code 
							AND SGC.Scheme_Seq = ER.Scheme_Seq
				LEFT JOIN  
					(SELECT 
						Scheme_Code, Scheme_Seq, Value, Rule_Group_Code
					FROM 
						EligibilityRule WITH (NOLOCK)
					WHERE 
						[Type] = 'SERVICEDTM'
						AND scheme_code='HCVS'
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
		ORDER BY  SGC.Scheme_seq DESC, SubSeq DESC

		UPDATE 
			#Deceased_eligible
		SET 
			Last_Service_Dtm = dateadd (DD, -1, @65ClaimRuleEffDtm)
		WHERE 
			Season = 10 AND SubSeq = 1 

		--RAISERROR ('Get eligible mapping table for deceased completed', 0, 1) WITH NOWAIT
		
-- =============================================
-- Preparation: Get Transaction
-- =============================================
		----------------------
		-- Validated Account
		----------------------
		INSERT INTO #VoucherTransaction (
			Transaction_ID,
			Transaction_Dtm,
			Service_Receive_Dtm,
			Voucher_Acc_ID,
			Temp_Voucher_Acc_ID,
			Service_Type,
			SP_ID,
			Practice_Display_Seq,
			Doc_Code,
			Identity_Num,
			Age,
			DOB,							
			Exact_DOB,				
			Create_By_SmartID,
			SourceApp,
			Scheme_Code	
		)
		SELECT
			VT.Transaction_ID,
			VT.Transaction_Dtm,
			VT.Service_Receive_Dtm,
			ISNULL(VT.Voucher_Acc_ID, ''),
			ISNULL(VT.Temp_Voucher_Acc_ID, ''),
			VT.Service_Type,
			VT.SP_ID,
			VT.Practice_Display_Seq,
			VT.Doc_Code,
			P.Encrypt_Field1,
			DATEDIFF(yy, P.DOB, @reportDtm),
			P.DOB,
			P.Exact_DOB,
			VT.Create_By_SmartID,
			VT.SourceApp,
			VT.Scheme_Code	
		FROM
			VoucherTransaction VT WITH (NOLOCK)
				INNER JOIN PersonalInformation P WITH (NOLOCK)
					ON VT.Voucher_Acc_ID = P.Voucher_Acc_ID 
		WHERE
			NOT EXISTS (
				SELECT
					1
				FROM
					StatStatusFilterMapping WITH (NOLOCK)
				WHERE
					(Report_id = 'ALL' OR Report_id = 'eHSD0001') 
						AND Table_Name = 'VoucherTransaction'
						AND Status_Name = 'Record_Status' 
						AND ((Effective_Date IS NULL OR Effective_Date <= @cutOffDtm) AND (Expiry_Date IS NULL OR @cutOffDtm < Expiry_Date))
						AND Status_Value = VT.Record_Status 
				)
			AND (VT.Scheme_Code = @scheme_code OR VT.Scheme_Code = @scheme_code_mainland OR VT.Scheme_Code = @scheme_code_DHC)
			AND (VT.Invalidation IS NULL OR NOT EXISTS (
				SELECT
					1
				FROM
					StatStatusFilterMapping WITH (NOLOCK)
				WHERE
					(report_id = 'ALL' OR report_id = 'eHSD0001') 
						AND Table_Name = 'VoucherTransaction'
						AND Status_Name = 'Invalidation'
						AND ((Effective_Date IS NULL OR Effective_Date <= @cutOffDtm) AND (Expiry_Date IS NULL OR @cutOffDtm < Expiry_Date))
						AND Status_Value = VT.Invalidation 
				)
			)
			AND VT.Voucher_Acc_ID <> ''
	
		----------------------
		-- Temporary Account
		----------------------
		INSERT INTO #VoucherTransaction (
			Transaction_ID,
			Transaction_Dtm,
			Service_Receive_Dtm,
			Voucher_Acc_ID,
			Temp_Voucher_Acc_ID,
			Service_Type,
			SP_ID,
			Practice_Display_Seq,
			Doc_Code,
			Identity_Num,
			Age,
			DOB,							
			Exact_DOB,				
			Create_By_SmartID,
			SourceApp,
			Scheme_Code	
		)
		SELECT
			VT.Transaction_ID,
			VT.Transaction_Dtm,
			VT.Service_Receive_Dtm,
			ISNULL(VT.Voucher_Acc_ID, ''),
			ISNULL(VT.Temp_Voucher_Acc_ID, ''),
			VT.Service_Type,
			VT.SP_ID,
			VT.Practice_Display_Seq,
			VT.Doc_Code,
			TP.Encrypt_Field1,
			DATEDIFF(yy, TP.DOB, @reportDtm),
			TP.DOB,
			TP.Exact_DOB,
			VT.Create_By_SmartID,
			VT.SourceApp,
			VT.Scheme_Code	
		FROM
			VoucherTransaction VT WITH (NOLOCK)
				INNER JOIN TempPersonalInformation TP WITH (NOLOCK)
					ON VT.Temp_Voucher_Acc_ID = TP.Voucher_Acc_ID
		WHERE
			NOT EXISTS (
				SELECT
					1
				FROM
					StatStatusFilterMapping WITH (NOLOCK)
				WHERE
					(report_id = 'ALL' OR report_id = 'eHSD0001') 
						AND Table_Name = 'VoucherTransaction'
						AND Status_Name = 'Record_Status' 
						AND ((Effective_Date IS NULL OR Effective_Date <= @cutOffDtm) AND (Expiry_Date IS NULL OR @cutOffDtm < Expiry_Date))
						AND Status_Value = VT.Record_Status 
				)
			AND (VT.Scheme_Code = @scheme_code OR VT.Scheme_Code = @scheme_code_mainland OR VT.Scheme_Code = @scheme_code_DHC)
			AND (VT.Invalidation IS NULL OR NOT EXISTS (
				SELECT
					1
				FROM
					StatStatusFilterMapping WITH (NOLOCK)
				WHERE
					(report_id = 'ALL' OR report_id = 'eHSD0001') 
						AND Table_Name = 'VoucherTransaction'
						AND Status_Name = 'Invalidation'
						AND ((Effective_Date IS NULL OR Effective_Date <= @cutOffDtm) AND (Expiry_Date IS NULL OR @cutOffDtm < Expiry_Date))
						AND Status_Value = VT.Invalidation 
				)
			)
			AND VT.Voucher_Acc_ID = ''	

		-- ---------------------------------------------------------------
		-- Patch "Is_Terminate" : Check whether transaction is terminated
		-- ---------------------------------------------------------------
		UPDATE
			#VoucherTransaction
		SET
			Is_Terminate = 'Y'
		FROM
			#VoucherTransaction A
				INNER JOIN #TerminateAccount T
					ON A.Doc_Code = T.Doc_Code
						AND A.Identity_Num = T.Identity_Num

		UPDATE
			#VoucherTransaction
		SET
			Is_Terminate = 'N'
		WHERE
			Is_Terminate IS NULL

		
		--RAISERROR ('Get voucher transaction completed', 0, 1) WITH NOWAIT

-- =============================================
-- Preparation: Get Account
-- =============================================
		----------------------
		-- Validated Account
		----------------------
		INSERT INTO #Account
		(
			Voucher_Acc_ID,
			Temp_Voucher_Acc_ID,
			Identity_Num,
			Doc_Code,
			DOB,
			Exact_DOB,
			Create_Dtm,
			Is_Terminate,
			Record_Status,
			Account_Start_Dtm,
			Deceased,
			Exact_DOD,
			DOD
		)
		SELECT 
			P.Voucher_Acc_ID,
			NULL,
			P.Encrypt_Field1,
			P.Doc_Code,
			P.DOB,
			P.Exact_DOB,
			VA.Create_Dtm,
			CASE VA.Record_Status WHEN 'D' THEN 'Y' ELSE 'N' END AS [Is_Terminate],
			VA.Record_Status,
			VA.Create_Dtm,
			CASE WHEN VA.Deceased = 'Y' THEN 1 ELSE 0 END,
			P.Exact_DOD,
			P.DOD			
		FROM 
			VoucherAccount VA WITH (NOLOCK)
				INNER JOIN PersonalInformation P WITH (NOLOCK) 
					ON VA.Voucher_Acc_ID = P.Voucher_Acc_ID 
		WHERE 
			P.Create_Dtm < @cutOffDtm

		--RAISERROR ('Get validated account completed', 0, 1) WITH NOWAIT

		----------------------
		-- Temporary Account
		----------------------
		INSERT INTO #Account
		(
			Voucher_Acc_ID,
			Temp_Voucher_Acc_ID,
			Identity_Num,
			Doc_Code,
			DOB,
			Exact_DOB,
			Create_Dtm,
			Record_Status,	--CRE16-014
			Account_Start_Dtm,
			Deceased,
			Exact_DOD,
			DOD
		)
		SELECT 
			TVA.Validated_acc_id,
			TP.Voucher_Acc_ID,		
			TP.Encrypt_Field1,
			TP.Doc_Code,
			TP.DOB,
			TP.Exact_DOB,
			TVA.Create_Dtm,
			TVA.Record_Status,	--CRE16-014
			TVA.Create_Dtm,
			CASE WHEN TVA.Deceased = 'Y' THEN 1 ELSE 0 END,
			TP.Exact_DOD,
			TP.DOD
		FROM
			TempVoucherAccount TVA WITH (NOLOCK)
				INNER JOIN TempPersonalInformation TP WITH (NOLOCK)
					ON TVA.Voucher_Acc_ID = TP.Voucher_Acc_ID
		WHERE 
			TVA.Record_Status NOT IN ('V', 'D')
			AND	TVA.account_purpose IN ('C', 'V')
			AND TP.Create_Dtm < @cutOffDtm

		--RAISERROR ('Get temp account completed', 0, 1) WITH NOWAIT

		----------------------------------------------
		-- Patch "Logical DOD"
		----------------------------------------------
		UPDATE #Account set Logical_DOD  = DOD
		WHERE Exact_DOD =  'D' 
	
		UPDATE #Account set Logical_DOD = dateadd(day, -1, dateadd(month,1, Cast((Cast(Year(DOD) as char(4)) + '-' + cast(DATENAME(MONTH, DOD) as char(3)) + '-01')  as datetime))) -- End of Month
		WHERE Exact_DOD =  'M' 

		UPDATE #Account set Logical_DOD = Cast((Cast(Year(DOD) as char(4)) + '-Dec-31')  as datetime) --End of Year
		WHERE Exact_DOD =  'Y' 


		----------------------------------------------
		-- Patch "Age"
		----------------------------------------------
		-- 1. For Alive
		UPDATE #account
		SET Age =  YEAR(@reportDtm) - YEAR(DOB)
		WHERE Deceased = 0
	
		-- 2. For Deceased	
		UPDATE #account
		SET Age =  YEAR(Logical_DOD) - YEAR(DOB)
		WHERE Deceased = 1


		----------------------------------------------
		-- Patch "Eligible"
		----------------------------------------------
		-- 1. For Alive
		UPDATE #Account
		SET Eligible = 'Y'
		WHERE 
			YEAR(@reportDtm) - YEAR(DOB) > @eligibleAge
			AND Deceased = 0
	
		-- 2. For Deceased	
		UPDATE #Account
		SET Eligible = 'Y'
		WHERE 
			Age > (SELECT Age
					FROM #Deceased_eligible DE
					WHERE Logical_DOD >= DE.Claim_Period_From AND Logical_DOD <= DE.Last_Service_Dtm)
			AND Deceased = 1
	
		-- 3. Remove Account When Eligible IS NULL
		DELETE FROM #Account
		WHERE Eligible IS NULL


		-- -----------------------------------------------------------
		-- Patch "Is_Terminate" : Check whether account is terminated
		-- -----------------------------------------------------------
		UPDATE
			#Account
		SET
			Is_Terminate = 'Y'
		FROM
			#Account A
				INNER JOIN #TerminateAccount T
					ON A.Doc_Code = T.Doc_Code
						AND A.Identity_Num = T.Identity_Num
		WHERE
			Is_Terminate IS NULL

		UPDATE
			#Account
		SET
			Is_Terminate = 'N'
		WHERE
			Is_Terminate IS NULL


		-- -----------------------------------------------------------
		-- Patch "Doc_Code" : Update all HKBC to HKIC	
		-- -----------------------------------------------------------
		UPDATE
			#account
		SET
			Doc_Code = 'HKIC'
		WHERE
			Doc_Code = 'HKBC'


		-- -----------------------------------------------------------
		-- Patch "First_Tx_Dtm" 
		-- -----------------------------------------------------------
		UPDATE 
			#Account 
		SET 
			First_Tx_Dtm = b.First_Tx_Dtm
		FROM 
			#Account A, 
			(SELECT Identity_Num, First_Tx_Dtm = MIN(Transaction_Dtm) 
			FROM #VoucherTransaction 
			WHERE Transaction_Dtm < @cutoffDtm
			GROUP BY Identity_Num) B
		WHERE A.identity_num = B.Identity_Num

		--RAISERROR ('Find first tx date completed', 0, 1) WITH NOWAIT


		-- -----------------------------------------------------------
		-- Patch "Account_Start_Dtm" 
		-- -----------------------------------------------------------

		-- 1. Account Created BEFORE 2017-07-01 new claim rule effective date
		-- **************** Remarks ******************
		-- Amend Account_Start_Dtm by eligiblity	
		-- Age 65 voucher rule effective on 2017-Jul-1
		-- *******************************************
		UPDATE #Account 
		SET 
			Account_Start_Dtm = 
			CASE 
				-- age : 2017-DOB >= 70 
				WHEN YEAR(@65ClaimRuleEffDtm) - YEAR(DOB) > @2017_1st_EligibleAge THEN	
					FORMATMESSAGE('%d-01-01',YEAR(DATEADD(YEAR, @2017_1st_EligibleAge +1 , DOB)))
				-- age : 2017-DOB between 65-69 
				WHEN YEAR(@65ClaimRuleEffDtm) - YEAR(DOB) BETWEEN @2017_2nd_EligibleAge +1 AND @2017_1st_EligibleAge THEN
					@65ClaimRuleEffDtm
				-- age : 2017-DOB < 65 
				ELSE
					FORMATMESSAGE('%d-01-01',YEAR(DATEADD(YEAR, @2017_2nd_EligibleAge +1 , dob)))
				END
		WHERE 
			YEAR(Account_Start_Dtm)  - YEAR(DOB) <= @2017_1st_EligibleAge
			AND Create_Dtm < @65ClaimRuleEffDtm


		-- 2. Account Created AFTER 2017-07-01 new claim rule effective date
		UPDATE #Account 
		SET 
			Account_Start_Dtm = FORMATMESSAGE('%d-01-01',YEAR(DATEADD(YEAR, @2017_2nd_EligibleAge +1 , dob)))
		WHERE 
			YEAR(Account_Start_Dtm)  - YEAR(dob) <= @2017_2nd_EligibleAge
			AND Create_Dtm >= @65ClaimRuleEffDtm
		

		-- 3. Account Start Date > First Transaction Date
		-- ********************** Remarks **********************		
		-- If the First_Transaction_Dtm < Account_Start_Dtm, SET Account_Start_Dtm = First_Transaction_Dtm (***INCLUDE tx after AS at dtm)
		-- Case 1: Program logic before writeoff enhancement, insert transaction before insert account
		-- Case 2: Rectify --> modify X account, new C account created, transaction updated to new C account, while transaction date no change
		-- *****************************************************
		UPDATE 
			#Account 
		SET 
			Account_Start_Dtm = TotalTx.First_Tx_Dtm
		FROM
			#Account A,
			(SELECT Identity_Num, First_Tx_Dtm = MIN(Transaction_Dtm) 
			FROM #VoucherTransaction
			GROUP BY Identity_Num) TotalTx
		WHERE A.identity_num = TotalTx.Identity_Num and
			A.First_Tx_Dtm < A.Account_Start_Dtm
	

-- =============================================
-- Processing: Find out Target Account
-- =============================================	
		INSERT INTO #AccountFinal(
			Voucher_Acc_ID, 
			Temp_Voucher_Acc_ID,
			Identity_Num, 
			Doc_Code,
			DOB,
			Exact_DOB,
			Eligible,
			Age,
			Create_Dtm,
			Account_Start_Dtm,
			Is_Terminate,
			Record_Status,
			first_tx_dtm,
			Logical_DOD,
			Deceased, 
			Acc_Seq)	
		SELECT	
			Voucher_Acc_ID, 
			Temp_Voucher_Acc_ID,
			Identity_Num, 
			Doc_Code,
			DOB,
			Exact_DOB,
			Eligible,
			Age,
			Create_Dtm,
			Account_Start_Dtm,
			Is_Terminate,
			Record_Status,
			first_tx_dtm,
			Logical_DOD,
			Deceased,
			Acc_Seq = ROW_NUMBER() OVER (PARTITION BY Identity_Num, Doc_Code ORDER BY Account_Start_Dtm, Create_Dtm)
		FROM	
			#Account 
		WHERE	
			Account_Start_Dtm < @cutOffDtm

		--RAISERROR ( 'Outputting result to stat tables', 0, 1) WITH NOWAIT

-- =============================================
-- Result: RptAccountSFC
-- =============================================	
		INSERT INTO RptAccountSFC(		
			Voucher_Acc_ID,
			Temp_Voucher_Acc_ID	,
			Identity_Num,			
			Doc_Code,
			DOB,
			Exact_DOB,
			Age,
			Create_Dtm,
			Is_Terminate,
			Record_Status,
			Logical_DOD,
			Deceased)
		SELECT 		
			Voucher_Acc_ID,
			Temp_Voucher_Acc_ID	,
			Identity_Num,			
			Doc_Code,
			DOB,	
			Exact_DOB,
			Age,
			Create_Dtm,
			Is_Terminate,
			Record_Status,
			Logical_DOD,
			Deceased	
		FROM	
			#AccountFinal
		WHERE 
			Acc_Seq = 1

		--RAISERROR ( 'Outputting result to RptAccountSFC', 0, 1) WITH NOWAIT
	
-- =============================================
-- Finalization: Drop Temporary Tables
-- =============================================	
		DROP TABLE #Account
		DROP TABLE #AccountFinal
		DROP TABLE #vouchertransaction
		DROP TABLE #Deceased_eligible
		DROP TABLE #TerminateAccount
	
-- =============================================
-- Finalization: Update Generation Time
-- =============================================		
		UPDATE SystemVariable
		SET Variable_Value = format(@cutoffDtm, 'yyyyMMMdd')
			,Update_By = 'eHS'
			,Update_Dtm = GETDATE()
		WHERE Variable_ID = 'AccountSFC_Generation_Check' 

	END

END
GO

GRANT EXECUTE ON [dbo].[proc_Prepare_RptAccountSFC] TO HCVU
GO

