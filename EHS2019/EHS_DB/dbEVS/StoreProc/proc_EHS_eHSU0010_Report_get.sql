IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSU0010_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSU0010_Report_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Author:			Chris YIM
-- Create date:		05 Dec 2019
-- CR No.			CRE19-013
-- Description:		Claim pattern
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSU0010_Report_get]
	@request_time		DATETIME,
	@SPID				CHAR(8),
	@Period_Type		CHAR(1),
	@Period_From		DATETIME,
	@Transaction_Status VARCHAR(5000),
	@CompareOperator	VARCHAR(2),
	@CompareValue		INT
AS
BEGIN
	SET NOCOUNT ON;
	-- ===========================================================
	-- 1.0	DECLARATION
	-- ===========================================================
	DECLARE @IN_dtmRequest_Time		DATETIME
	DECLARE @IN_chrSPID				CHAR(8)
	DECLARE @IN_chrPeriod_Type		CHAR(1)
	DECLARE @IN_dtmPeriod_From		DATETIME
	DECLARE @IN_varRecordStatus		VARCHAR(5000)
	DECLARE @IN_varCompareOperator	VARCHAR(2)
	DECLARE @IN_intCompareValue		INT

	DECLARE @IN_dtmTransactionDateStart DATETIME
	DECLARE @IN_dtmTransactionDateEnd	DATETIME
	DECLARE @IN_dtmServiceDateStart		DATETIME
	DECLARE @IN_dtmServiceDateEnd		DATETIME

	DECLARE @chrIsAllRecordStatus	CHAR(1)
	DECLARE @current_dtm			DATETIME
	DECLARE @delimiter				VARCHAR(5)
	DECLARE @seq					INT

	DECLARE @tblRecordStatus TABLE(
		Record_Status CHAR(1)
	)

	DECLARE @tblDateWithHighestNoOfTran TABLE(
		Service_Receive_Dtm DATETIME
	)

	DECLARE @tblDateWithHighestClaimAmt TABLE(
		Service_Receive_Dtm DATETIME
	)

	CREATE TABLE #VoucherTransactionBase (
		SP_ID					CHAR(8), 
		Transaction_ID			VARCHAR(20),
		Transaction_Dtm			DATETIME,
		Service_Receive_Dtm		DATETIME,
		Practice_Display_Seq	SMALLINT,
		Claim_Amount			MONEY,
		Record_Status			CHAR(1),
		Create_By_SmartID		CHAR(1),
		Practice_Name			NVARCHAR(200),
		Building				VARCHAR(100),
		District				CHAR(4),
		Service_Category_Code	VARCHAR(3)	
	)

	--CREATE NONCLUSTERED INDEX IX_VoucherTransactionBase_SP_ID
	--	ON #VoucherTransactionBase (SP_ID); 

	CREATE TABLE #Content (
		Seq		INT,
		Seq2	INT,
		Col01	VARCHAR(1000),
		Col02	VARCHAR(1000)
	)

	CREATE TABLE #Criteria (
		Seq		INT,
		Seq2	INT,
		Col01	VARCHAR(1000),
		Col02	VARCHAR(1000)
	)

	CREATE TABLE #01_Result (
		Seq		INT,
		Seq2	INT,
		Col01	VARCHAR(1000),
		Col02	NVARCHAR(1000),
		Col03	VARCHAR(1000),
		Col04	VARCHAR(1000),
		Col05	VARCHAR(1000),
		Col06	VARCHAR(1000),
		Col07	VARCHAR(1000)
	)

	CREATE TABLE #02_YEARMONTH (
		YearMonth	CHAR(6)
	)

	CREATE TABLE #02_Result (
		Seq		INT,
		Seq2	INT,
		Col01	VARCHAR(1000),
		Col02	VARCHAR(1000),
		Col03	VARCHAR(1000),
		Col04	VARCHAR(1000),
		Col05	VARCHAR(1000),
		Col06	VARCHAR(1000),
		Col07	VARCHAR(1000),
		Col08	VARCHAR(1000),
		Col09	VARCHAR(1000),
		Col10	VARCHAR(1000),
		Col11	VARCHAR(1000)
	)

	CREATE TABLE #03_Result (
		Seq		INT,
		Seq2	INT,
		Col01	VARCHAR(1000),
		Col02	VARCHAR(1000),
		Col03	VARCHAR(1000),
		Col04	VARCHAR(1000),
		Col05	VARCHAR(1000),
		Col06	VARCHAR(1000),
		Col07	VARCHAR(1000)
	)

	CREATE TABLE #04_Result (
		Seq		INT,
		Seq2	INT,
		Col01	VARCHAR(1000),
		Col02	VARCHAR(1000),
		Col03	VARCHAR(1000)
	)

	CREATE TABLE #05_Result (
		Seq		INT,
		Seq2	INT,
		Col01	VARCHAR(1000),
		Col02	VARCHAR(1000)
	)

	CREATE TABLE #06_Result (
		Seq		INT,
		Seq2	INT,
		Col01	VARCHAR(1000),
		Col02	VARCHAR(1000),
		Col03	VARCHAR(1000)
	)

	CREATE TABLE #07_Result (
		Seq		INT,
		Seq2	INT,
		Col01	VARCHAR(1000),
		Col02	VARCHAR(1000),
		Col03	VARCHAR(1000),
		Col04	VARCHAR(1000),
		Col05	VARCHAR(1000),
		Col06	VARCHAR(1000),
		Col07	VARCHAR(1000)
	)

	CREATE TABLE #08_Result (
		Seq		INT,
		Seq2	INT,
		Col01	VARCHAR(1000),
		Col02	VARCHAR(1000),
		Col03	VARCHAR(1000),
		Col04	VARCHAR(1000),
		Col05	VARCHAR(1000),
		Col06	VARCHAR(1000),
		Col07	VARCHAR(1000),
		Col08	VARCHAR(1000),
		Col09	VARCHAR(1000)
	)

	CREATE TABLE #09_Result (
		Seq		INT,
		Seq2	INT,
		Col01	VARCHAR(1000),
		Col02	VARCHAR(1000),
		Col03	VARCHAR(1000),
		Col04	VARCHAR(1000),
		Col05	VARCHAR(1000),
		Col06	VARCHAR(1000),
		Col07	VARCHAR(1000),
		Col08	VARCHAR(1000),
		Col09	VARCHAR(1000)
	)

	CREATE TABLE #Remarks (
		Seq		INT,
		Seq2	INT,
		Col01	VARCHAR(1000),
		Col02	VARCHAR(1000)
	)

	-- ===========================================================
	-- 1.1	Initialization
	-- ===========================================================
	SET @current_dtm = GETDATE()
	SET @delimiter = ','

	-- ===========================================================
	-- 1.2	COPY PARAMETERS
	-- ===========================================================
	SET @IN_dtmRequest_Time	= @request_time
	SET @IN_chrSPID = @SPID
	SET @IN_chrPeriod_Type = @Period_Type
	SET @IN_dtmPeriod_From = @Period_From
	SET @IN_varCompareOperator = @CompareOperator
	SET @IN_intCompareValue	= @CompareValue

	SET @IN_dtmTransactionDateStart = NULL
	SET @IN_dtmTransactionDateEnd = NULL
	SET @IN_dtmServiceDateStart = NULL
	SET @IN_dtmServiceDateEnd = NULL

	SET @IN_varRecordStatus = NULL

	IF @Transaction_Status IS NOT NULL
	BEGIN
		IF @Transaction_Status = ''
			SET @IN_varRecordStatus = NULL
		ELSE 
			SET @IN_varRecordStatus = @Transaction_Status
	END

	-- Selected 'Service Date'
	IF @IN_chrPeriod_Type = 'S'
		BEGIN
			SET @IN_dtmServiceDateStart = @IN_dtmPeriod_From
			--SET @IN_dtmServiceDateEnd = @IN_dtmPeriod_To
		END

	-- Selected 'Transaction Date'
	ELSE IF @IN_chrPeriod_Type = 'T'
		BEGIN
			SET @IN_dtmTransactionDateStart = @IN_dtmPeriod_From
			--SET @IN_dtmTransactionDateEnd = DATEADD(DAY, 1, @IN_dtmPeriod_To)
		END

	-- ===========================================================
	-- 2. PREPARE TABLE TO STORE INPUTTED RECORD STATUS
	-- ===========================================================
	IF @IN_varRecordStatus IS NOT NULL
	BEGIN
		INSERT @tblRecordStatus
				SELECT * FROM func_split_string(@IN_varRecordStatus, @delimiter)

		SET @chrIsAllRecordStatus = 'N'
	END
	ELSE 
		SET @chrIsAllRecordStatus = 'Y'


	-- ===========================================================
	-- 3. PREPARE DATA FOR OUTPUT: CRITERIA
	-- ===========================================================
		-- ---------------------------------------------
		-- Declaration
		-- ---------------------------------------------
		DECLARE @varSPName				VARCHAR(50)
		DECLARE @varSPNameCHI			NVARCHAR(50)
		DECLARE @varPeriod_Type			VARCHAR(50)
		DECLARE @chrPeriod_Type			VARCHAR(50)
		DECLARE @chrPeriod_Format		VARCHAR(50)
		DECLARE @varPeriod				VARCHAR(50)
		DECLARE @varTransactionStatus	VARCHAR(5000)
		DECLARE @varListTransaction		VARCHAR(50)

		-- ---------------------------------------------
		-- Initialization
		-- ---------------------------------------------

		-- Service Provider Name
		EXEC [proc_SymmetricKey_open]

		SET @varSPName = (SELECT CONVERT(VARCHAR(MAX), DecryptByKey(Encrypt_Field2)) FROM ServiceProvider WITH (NOLOCK) WHERE SP_ID = @IN_chrSPID)
		SET @varSPNameCHI = (SELECT CONVERT(NVARCHAR(MAX), DecryptByKey(Encrypt_Field3)) FROM ServiceProvider WITH (NOLOCK) WHERE SP_ID = @IN_chrSPID)

		EXEC [proc_SymmetricKey_close]

		-- Type of Date
		IF @IN_chrPeriod_Type = 'S'
			BEGIN
				SET @chrPeriod_Type = 'Service Date'
				SET @varPeriod_Type = 'Service date'
			END
		ELSE IF @IN_chrPeriod_Type = 'T'
			BEGIN
				SET @chrPeriod_Type = 'Transaction Date'
				SET @varPeriod_Type = 'Transaction date'
			END

		-- Date
		SET @chrPeriod_Format = 'Month and Year'
		SET @varPeriod = FORMAT(@Period_From, 'MMMM, yyyy')

		-- Transaction Status
		IF @chrIsAllRecordStatus= 'Y'
			SET @varTransactionStatus = 'Any'
		ELSE
			BEGIN
				SET @varTransactionStatus = 
					(SELECT 
						[Status_Description] + ', ' 
					FROM StatusData WITH (NOLOCK)
					WHERE [Status_Value] IN 
						(SELECT [Record_Status] FROM @tblRecordStatus)
						AND Enum_Class = 'HCVUClaimTransManagementStatus'
					ORDER BY Display_Order
					for xml path(''))

				SET @varTransactionStatus = SUBSTRING(@varTransactionStatus, 1, LEN(@varTransactionStatus)- 1) 
			END

		-- List Transaction
		SET @varListTransaction =  CONVERT(VARCHAR(MAX),'Claim Amount') + ' ' + CONVERT(VARCHAR(2),@IN_varCompareOperator) + ' ' + CONVERT(VARCHAR(MAX),@IN_intCompareValue)

	-- =========================================================================
	-- 3.1 PREPARE TABLE TO RETRIEVE POSSIBLE SERVICE PROVIDER WITH TRANSACTIONS
	-- =========================================================================
	DECLARE @01_dtmPeriod_From	DATETIME
	DECLARE @01_dtmPeriod_TO	DATETIME

	SET @01_dtmPeriod_From = @IN_dtmPeriod_From
	SET @01_dtmPeriod_TO = DATEADD(DAY,-1,DATEADD(MONTH,1,@IN_dtmPeriod_From))

	INSERT #VoucherTransactionBase(
		SP_ID,
		Transaction_ID,
		Transaction_Dtm,
		Service_Receive_Dtm,
		Practice_Display_Seq,
		Claim_Amount,
		Record_Status,
		Create_By_SmartID,
		Practice_Name,
		Building,
		District,
		Service_Category_Code
	)
	SELECT
		VT.SP_ID,
		VT.Transaction_ID,
		VT.Transaction_Dtm,
		VT.Service_Receive_Dtm,
		VT.Practice_Display_Seq,
		VT.Claim_Amount,
		VT.Record_Status,
		VT.Create_By_SmartID,
		P.Practice_Name,
		P.Building,
		P.District,
		Prof.Service_Category_Code
	FROM
		VoucherTransaction VT WITH (NOLOCK)
			INNER JOIN Practice P WITH (NOLOCK)
				ON VT.Practice_Display_Seq = P.Display_Seq AND VT.SP_ID = P.SP_ID
			INNER JOIN Professional Prof WITH (NOLOCK)
				ON Prof.Professional_Seq = P.Professional_Seq AND Prof.SP_ID = P.SP_ID
			LEFT JOIN @tblRecordStatus tblRS
				ON LTRIM(RTRIM(VT.Record_Status)) = tblRS.Record_Status
	WHERE
		VT.SP_ID = @IN_chrSPID
		AND (
				(@IN_chrPeriod_Type = 'S' AND @01_dtmPeriod_From <= VT.Service_Receive_Dtm AND VT.Service_Receive_Dtm < DATEADD(DAY, 1, @01_dtmPeriod_TO))
			OR 
				(@IN_chrPeriod_Type = 'T' AND @01_dtmPeriod_From <= VT.Transaction_Dtm AND VT.Transaction_Dtm < DATEADD(DAY, 1, @01_dtmPeriod_TO))
			)
		AND VT.Scheme_Code = 'HCVS'
		AND (VT.Invalidation IS NULL OR VT.Invalidation NOT IN ('I'))
		AND (@chrIsAllRecordStatus = 'Y' OR tblRS.Record_Status IS NOT NULL)

	-- =============================================
	-- 4. RETURN RESULTS
	-- =============================================
	-- ---------------------------------------------
	-- To Excel Sheet (01): Content
	-- ---------------------------------------------

		-- ---------------------------------------------
		-- Insert Data: Content
		-- ---------------------------------------------

		SET @seq = 0

		INSERT INTO #Content (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Sub Report ID', 'Sub Report Name')

		SET @seq = @seq + 1

		INSERT INTO #Content (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'eHS(S)U0010-' + RIGHT('0' + CONVERT(VARCHAR(MAX),@seq),2), 'Report of transaction by practice (According to the Selected Month only and divide them by practices)')

		SET @seq = @seq + 1

		INSERT INTO #Content (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'eHS(S)U0010-' + RIGHT('0' + CONVERT(VARCHAR(MAX),@seq),2), 'Report of claim pattern in previous 12 months (A general claim pattern by month of transaction date and no need to divide them by practices)')

		SET @seq = @seq + 1

		INSERT INTO #Content (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'eHS(S)U0010-' + RIGHT('0' + CONVERT(VARCHAR(MAX),@seq),2), 'Report of claim pattern by day (Breakdown in each service date of the selected month regardless the practice)')

		SET @seq = @seq + 1

		INSERT INTO #Content (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'eHS(S)U0010-' + RIGHT('0' + CONVERT(VARCHAR(MAX),@seq),2), 'Report of means of input')

		SET @seq = @seq + 1

		INSERT INTO #Content (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'eHS(S)U0010-' + RIGHT('0' + CONVERT(VARCHAR(MAX),@seq),2), 'Report of claim pattern by percentile')

		SET @seq = @seq + 1

		INSERT INTO #Content (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'eHS(S)U0010-' + RIGHT('0' + CONVERT(VARCHAR(MAX),@seq),2), 'Report of transaction breakdown by "Amount Claimed"')

		SET @seq = @seq + 1

		INSERT INTO #Content (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'eHS(S)U0010-' + RIGHT('0' + CONVERT(VARCHAR(MAX),@seq),2), 'Report of transaction with "Amount Claimed" ' + @IN_varCompareOperator + ' ' + FORMAT(@IN_intCompareValue,'C0'))

		SET @seq = @seq + 1

		INSERT INTO #Content (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'eHS(S)U0010-' + RIGHT('0' + CONVERT(VARCHAR(MAX),@seq),2), 'Report of transaction about the service date with the highest "No. of transactions"')

		SET @seq = @seq + 1

		INSERT INTO #Content (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'eHS(S)U0010-' + RIGHT('0' + CONVERT(VARCHAR(MAX),@seq),2), 'Report of transaction about the service date with the highest total "Amount Claimed"')

		SET @seq = @seq + 1

		INSERT INTO #Content (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '', '')

		SET @seq = @seq + 1

		INSERT INTO #Content (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '', '')

		SET @seq = @seq + 1

		INSERT INTO #Content (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Report Generation Time: ' + CONVERT(varchar(10), @current_dtm, 111) + ' ' + CONVERT(varchar(5), @current_dtm, 114), '')

		SET @seq = @seq + 1

		-- ---------------------------------------------
		-- Return Result: Content
		-- ---------------------------------------------

		SELECT Col01, Col02 FROM #Content ORDER BY Seq, Seq2, Col01

	-- ---------------------------------------------
	-- To Excel Sheet (02): Criteria
	-- ---------------------------------------------

		-- ---------------------------------------------
		-- Insert Data: Criteria
		-- ---------------------------------------------

		SET @seq = 0

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Criteria', '')

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Service Provider ID', @IN_chrSPID)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Service Provider Name', @varSPName)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Type of Date', @chrPeriod_Type)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Format of Date', @chrPeriod_Format)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Date', @varPeriod)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Transaction Status', @varTransactionStatus)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'List Transaction', @varListTransaction)

		SET @seq = @seq + 1

		-- ---------------------------------------------
		-- Return Result: Criteria
		-- ---------------------------------------------

		SELECT Col01, Col02 FROM #Criteria ORDER BY Seq, Seq2, Col01

	-- ---------------------------------------------
	-- To Excel Sheet (03): 01-Practice
	-- ---------------------------------------------
		
		-- ---------------------------------------------
		-- Insert Data: 01-Practice
		-- ---------------------------------------------
		SET @seq = 0

		INSERT INTO #01_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07)
		VALUES (@seq, NULL, @varPeriod_Type  + ' period: ' + CONVERT(VARCHAR(10), @01_dtmPeriod_From, 111) + ' to ' + CONVERT(VARCHAR(10), @01_dtmPeriod_TO, 111), 
				'', '', '', '', '', '')

		SET @seq = @seq + 1

		INSERT INTO #01_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07)
		VALUES (@seq, NULL, '', '', '', '', '', '', '')

		SET @seq = @seq + 1

		INSERT INTO #01_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07)
		VALUES (@seq, NULL, 'SP ID', @IN_chrSPID, '', '', '', '', '')

		SET @seq = @seq + 1

		IF @varSPNameCHI = '' 
		BEGIN 
			INSERT INTO #01_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07)
			VALUES (@seq, NULL, 'SP Name', @varSPName, '', '', '', '', '')
		END
		ELSE
			INSERT INTO #01_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07)
			VALUES (@seq, NULL, 'SP Name', @varSPName + ' (' + @varSPNameCHI + ')', '', '', '', '', '' )

		SET @seq = @seq + 1

		INSERT INTO #01_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07)
		VALUES (@seq, NULL, '', '', '', '', '', '', '')
			
		SET @seq = @seq + 1

		INSERT INTO #01_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07)
		VALUES (@seq, NULL, 
				'Practice No.',			'Practice Name (In English)',	'Practice Address (In English)',		'Health Profession',
				'No. of Transactions',	'Voucher Amount Claimed',		'Average Voucher Amount / Transaction')

		SET @seq = @seq + 1

		-- Transaction by each practice
		INSERT INTO #01_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07)
		SELECT 
			@seq
			,NULL
			,R.Display_Seq
			,R.Practice_Name
			,R.Building
			,R.Service_Category_Code		
			,R.[NoOfTrans]
			,R.[ClaimAmount]	
			,R.[Average]	
		FROM
			(SELECT 
				VTB.Practice_Display_Seq AS [Display_Seq]
				,VTB.Practice_Name
				,VTB.Building
				,VTB.Service_Category_Code		
				,COUNT(VTB.Transaction_ID) AS [NoOfTrans]		
				,SUM(VTB.Claim_Amount) AS [ClaimAmount]
				,SUM(VTB.Claim_Amount)/COUNT(VTB.Transaction_ID) AS [Average]
			FROM
				#VoucherTransactionBase VTB
			GROUP BY
				VTB.Practice_Display_Seq
				,VTB.Practice_Name
				,VTB.Building
				,VTB.Service_Category_Code) R	
		ORDER BY
			R.Display_Seq

		SET @seq = @seq + 1

		-- Total
		INSERT INTO #01_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07)
		SELECT 
			@seq
			,NULL
			,''
			,''
			,''
			,'Total'		
			,R.[NoOfTrans]
			,R.[ClaimAmount]	
			,R.[Average]	
		FROM
			(SELECT 
				COUNT(VTB.Transaction_ID) AS [NoOfTrans]		
				,SUM(VTB.Claim_Amount) AS [ClaimAmount]
				,SUM(VTB.Claim_Amount)/COUNT(VTB.Transaction_ID) AS [Average]
			FROM
				#VoucherTransactionBase VTB
			GROUP BY
				VTB.SP_ID) R	

		---- ---------------------------------------------
		---- Return Result: Criteria
		---- ---------------------------------------------

		SELECT Col01, Col02, Col03, Col04, Col05, Col06, Col07 FROM #01_Result ORDER BY Seq, Seq2, Col01

	-- ----------------------------------------------------------
	-- To Excel Sheet (04): 02-Claim pattern (Mths)
	-- ----------------------------------------------------------
	--  Required to use transaction date to get transaction

		DECLARE @02_dtmPeriod_From	DATETIME
		DECLARE @02_dtmPeriod_TO	DATETIME

		SET @02_dtmPeriod_From = DATEADD(YEAR,-1,DATEADD(MONTH,1,@IN_dtmPeriod_From))
		SET @02_dtmPeriod_TO = DATEADD(DAY,-1,DATEADD(MONTH,1,@IN_dtmPeriod_From))

		-- ---------------------------------------------
		-- Preparation Data: 02-Claim pattern (Mths)
		-- ---------------------------------------------
		DECLARE @02_dtmPeriod_CURRENT	DATETIME
		DECLARE @02_intPeriod_CURRENT	INT
		DECLARE @02_intPeriod_FROM		INT

		SET @02_dtmPeriod_CURRENT = @02_dtmPeriod_TO
		SET @02_intPeriod_CURRENT = CAST((CONVERT(VARCHAR(4),DATEPART(YYYY,@02_dtmPeriod_TO)) + RIGHT('0' + CONVERT(VARCHAR(2),DATEPART(MM,@02_dtmPeriod_TO)), 2)) AS INT)
		SET @02_intPeriod_FROM = CAST((CONVERT(VARCHAR(4),DATEPART(YYYY,@02_dtmPeriod_From)) + RIGHT('0' + CONVERT(VARCHAR(2),DATEPART(MM,@02_dtmPeriod_From)), 2)) AS INT)

		WHILE (SELECT @02_intPeriod_CURRENT) >= @02_intPeriod_FROM  
			BEGIN  
				INSERT #02_YEARMONTH
					SELECT @02_intPeriod_CURRENT

				SET @02_dtmPeriod_CURRENT = DATEADD(MM,-1,@02_dtmPeriod_CURRENT)
				SET @02_intPeriod_CURRENT = CAST((CONVERT(VARCHAR(4),DATEPART(YYYY,@02_dtmPeriod_CURRENT)) + RIGHT('0' + CONVERT(VARCHAR(2),DATEPART(MM,@02_dtmPeriod_CURRENT)), 2)) AS INT)				
			
				IF @02_intPeriod_CURRENT < @02_intPeriod_FROM
					BREAK
			END  

		-- ---------------------------------------------
		-- Insert Data: 02-Claim pattern (Mths)
		-- ---------------------------------------------
		SET @seq = 0

		INSERT INTO #02_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11)
		VALUES (@seq, NULL, 'Transaction date period: ' + CONVERT(VARCHAR(10), @02_dtmPeriod_From, 111) + ' to ' + CONVERT(VARCHAR(10), @02_dtmPeriod_TO, 111), 
				'', '', '', '', '', '', '', '', '', '')

		SET @seq = @seq + 1

		INSERT INTO #02_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11)
		VALUES (@seq, NULL, '', '', '', '', '', '', '', '', '', '', '')

		SET @seq = @seq + 1

		INSERT INTO #02_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11)
		VALUES (@seq, NULL, '', '', '', '', '', '', '', '', '', '', '')
			
		SET @seq = @seq + 1

		INSERT INTO #02_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11)
		VALUES 
			(@seq, 
			NULL, 
			'Month (Transaction Date)',			
			'No. of Claims',	
			'Total Voucher Amount ($)',		
			'Average Voucher Amount / Claim',
			'No. of Claims (Rank among all EHCPs)',	
			'Total Voucher Amount ($) (Rank among all EHCPs)',	
			'Average Voucher Amount/ Claim (Rank among all EHCPs)',
			'Health Profession',			
			'No. of Claims (Rank among the same profession)',
			'Total Voucher Amount ($) (Rank among the same profession)',
			'Average Voucher Amount/ Claim (Rank among the same profession)'
			)

		SET @seq = @seq + 1

		-- Transaction by each practice
		INSERT INTO #02_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11)
		SELECT 
			@seq + ROW_NUMBER() OVER(ORDER BY Pt1.YearMonth DESC)
			,CONVERT(INT,Pt1.YearMonth)
			,[YearMonth] = 
				CASE 
					WHEN SUBSTRING(Pt1.YearMonth,5,2) = '01' THEN 'January '	+  CONVERT(CHAR(4),SUBSTRING(Pt1.YearMonth,1,4))
					WHEN SUBSTRING(Pt1.YearMonth,5,2) = '02' THEN 'February '	+  CONVERT(CHAR(4),SUBSTRING(Pt1.YearMonth,1,4))
					WHEN SUBSTRING(Pt1.YearMonth,5,2) = '03' THEN 'March '		+  CONVERT(CHAR(4),SUBSTRING(Pt1.YearMonth,1,4))
					WHEN SUBSTRING(Pt1.YearMonth,5,2) = '04' THEN 'April '		+  CONVERT(CHAR(4),SUBSTRING(Pt1.YearMonth,1,4))
					WHEN SUBSTRING(Pt1.YearMonth,5,2) = '05' THEN 'May '		+  CONVERT(CHAR(4),SUBSTRING(Pt1.YearMonth,1,4))
					WHEN SUBSTRING(Pt1.YearMonth,5,2) = '06' THEN 'June '		+  CONVERT(CHAR(4),SUBSTRING(Pt1.YearMonth,1,4))
					WHEN SUBSTRING(Pt1.YearMonth,5,2) = '07' THEN 'July '		+  CONVERT(CHAR(4),SUBSTRING(Pt1.YearMonth,1,4))
					WHEN SUBSTRING(Pt1.YearMonth,5,2) = '08' THEN 'August '		+  CONVERT(CHAR(4),SUBSTRING(Pt1.YearMonth,1,4))
					WHEN SUBSTRING(Pt1.YearMonth,5,2) = '09' THEN 'September '	+  CONVERT(CHAR(4),SUBSTRING(Pt1.YearMonth,1,4))
					WHEN SUBSTRING(Pt1.YearMonth,5,2) = '10' THEN 'October '	+  CONVERT(CHAR(4),SUBSTRING(Pt1.YearMonth,1,4))
					WHEN SUBSTRING(Pt1.YearMonth,5,2) = '11' THEN 'November '	+  CONVERT(CHAR(4),SUBSTRING(Pt1.YearMonth,1,4))
					WHEN SUBSTRING(Pt1.YearMonth,5,2) = '12' THEN 'December '	+  CONVERT(CHAR(4),SUBSTRING(Pt1.YearMonth,1,4))
					ELSE Pt1.YearMonth
				END
			,Pt1.NoOfTrans
			,Pt1.ClaimAmount
			,Pt1.Average
			,Pt2.Rank_NoOfTrans
			,Pt2.Rank_ClaimAmount
			,Pt2.Rank_Average
			,Pt3.Rank_Service_Category_Code_Prof
			,Pt3.Rank_NoOfTrans_Prof
			,Pt3.Rank_ClaimAmount_Prof
			,Pt3.Rank_Average_Prof
		FROM
			(SELECT 
				YM.YearMonth
				,ISNULL(VT_SP.NoOfTrans, 0) AS [NoOfTrans]
				,ISNULL(VT_SP.ClaimAmount, 0) AS [ClaimAmount]
				,ISNULL(VT_SP.Average, 0) AS [Average]
			FROM 
				#02_YEARMONTH YM
					LEFT OUTER JOIN 
						(SELECT 
							(CONVERT(VARCHAR(4),DATEPART(YYYY,Transaction_Dtm)) + RIGHT('0' + CONVERT(VARCHAR(2),DATEPART(MM,Transaction_Dtm)), 2)) AS [YearMonth]
							,COUNT(Transaction_ID) AS [NoOfTrans]		
							,SUM(Claim_Amount) AS [ClaimAmount]
							,SUM(Claim_Amount)/COUNT(Transaction_ID) AS [Average]
						FROM
							VoucherTransaction VT WITH (NOLOCK) 
								LEFT JOIN @tblRecordStatus tblRS
									ON LTRIM(RTRIM(VT.Record_Status)) = tblRS.Record_Status
						WHERE
							SP_ID = @IN_chrSPID
							AND (@02_dtmPeriod_From <= Transaction_Dtm AND Transaction_Dtm <  DATEADD(DAY, 1, @02_dtmPeriod_TO))
							AND Scheme_Code = 'HCVS'
							AND (@chrIsAllRecordStatus = 'Y' OR tblRS.Record_Status IS NOT NULL)
							AND (VT.Invalidation IS NULL OR VT.Invalidation NOT IN ('I'))
						GROUP BY
							(CONVERT(VARCHAR(4),DATEPART(YYYY,Transaction_Dtm)) + RIGHT('0' + CONVERT(VARCHAR(2),DATEPART(MM,Transaction_Dtm)), 2))
						) VT_SP
					ON YM.YearMonth = VT_SP.YearMonth) Pt1
			INNER JOIN
			(SELECT
				--YM.YearMonth
				--,VT_Total_SP.YearMonth
				--,VT_Total_SP.Total_SP
				[Rank_YearMonth] =
					CASE 
						WHEN VT_RANK.YearMonth IS NULL THEN YM.YearMonth
						ELSE VT_RANK.YearMonth
					END
				,[Rank_NoOfTrans] =
					CASE 
						WHEN VT_Total_SP.Total_SP IS NULL THEN 1
						WHEN VT_RANK.Rank_NoOfTrans IS NULL THEN VT_Total_SP.Total_SP + 1
						ELSE VT_RANK.Rank_NoOfTrans
					END
				,[Rank_ClaimAmount] =
					CASE 
						WHEN VT_Total_SP.Total_SP IS NULL THEN 1
						WHEN VT_RANK.Rank_ClaimAmount IS NULL THEN VT_Total_SP.Total_SP + 1
						ELSE VT_RANK.Rank_ClaimAmount
					END
				,[Rank_Average] =
					CASE 
						WHEN VT_Total_SP.Total_SP IS NULL THEN 1
						WHEN VT_RANK.Rank_Average IS NULL THEN VT_Total_SP.Total_SP + 1
						ELSE VT_RANK.Rank_Average
					END
			FROM 
			#02_YEARMONTH YM
				LEFT OUTER JOIN 
					(
					SELECT DISTINCT
						VT.YearMonth
						,MAX(VT.SP_Count) OVER(PARTITION BY YearMonth) AS [Total_SP]
					FROM
						(
						SELECT 
							(CONVERT(VARCHAR(4),DATEPART(YYYY,Transaction_Dtm)) + RIGHT('0' + CONVERT(VARCHAR(2),DATEPART(MM,Transaction_Dtm)), 2)) AS [YearMonth]
							,COUNT(DISTINCT SP_ID) AS [SP_Count] 
						FROM
							VoucherTransaction VT WITH (NOLOCK)
								LEFT JOIN @tblRecordStatus tblRS
									ON LTRIM(RTRIM(VT.Record_Status)) = tblRS.Record_Status
						WHERE
							(@02_dtmPeriod_From <= Transaction_Dtm AND Transaction_Dtm <  DATEADD(DAY, 1, @02_dtmPeriod_TO))
							AND Scheme_Code = 'HCVS'
							AND (@chrIsAllRecordStatus = 'Y' OR tblRS.Record_Status IS NOT NULL)
							AND (VT.Invalidation IS NULL OR VT.Invalidation NOT IN ('I'))
						GROUP BY
							(CONVERT(VARCHAR(4),DATEPART(YYYY,Transaction_Dtm)) + RIGHT('0' + CONVERT(VARCHAR(2),DATEPART(MM,Transaction_Dtm)), 2))
						) VT
					) VT_Total_SP
				ON YM.YearMonth = VT_Total_SP.YearMonth
				LEFT OUTER JOIN 
					(
					SELECT
						VT.YearMonth
						,VT.Rank_NoOfTrans
						,VT.Rank_ClaimAmount
						,VT.Rank_Average
					FROM
						(
						SELECT 
							(CONVERT(VARCHAR(4),DATEPART(YYYY,Transaction_Dtm)) + RIGHT('0' + CONVERT(VARCHAR(2),DATEPART(MM,Transaction_Dtm)), 2)) AS [YearMonth]
							,SP_ID
							,COUNT(Transaction_ID) AS [NoOfTrans]		
							,RANK() OVER (PARTITION BY (CONVERT(VARCHAR(4),DATEPART(YYYY,Transaction_Dtm)) + RIGHT('0' + CONVERT(VARCHAR(2),DATEPART(MM,Transaction_Dtm)), 2))
											ORDER BY COUNT(Transaction_ID) DESC) AS [Rank_NoOfTrans]  
							,SUM(Claim_Amount) AS [ClaimAmount]
							,RANK() OVER (PARTITION BY (CONVERT(VARCHAR(4),DATEPART(YYYY,Transaction_Dtm)) + RIGHT('0' + CONVERT(VARCHAR(2),DATEPART(MM,Transaction_Dtm)), 2))
											ORDER BY SUM(Claim_Amount) DESC) AS [Rank_ClaimAmount]  
							,SUM(Claim_Amount)/COUNT(Transaction_ID) AS [Average]
							,RANK() OVER (PARTITION BY (CONVERT(VARCHAR(4),DATEPART(YYYY,Transaction_Dtm)) + RIGHT('0' + CONVERT(VARCHAR(2),DATEPART(MM,Transaction_Dtm)), 2))
											ORDER BY (SUM(Claim_Amount)/COUNT(Transaction_ID)) DESC) AS [Rank_Average]  
						FROM
							VoucherTransaction VT WITH (NOLOCK)
								LEFT JOIN @tblRecordStatus tblRS
									ON LTRIM(RTRIM(VT.Record_Status)) = tblRS.Record_Status
						WHERE
							(@02_dtmPeriod_From <= Transaction_Dtm AND Transaction_Dtm <  DATEADD(DAY, 1, @02_dtmPeriod_TO))
							AND Scheme_Code = 'HCVS'
							AND (@chrIsAllRecordStatus = 'Y' OR tblRS.Record_Status IS NOT NULL)
							AND (VT.Invalidation IS NULL OR VT.Invalidation NOT IN ('I'))
						GROUP BY
							(CONVERT(VARCHAR(4),DATEPART(YYYY,Transaction_Dtm)) + RIGHT('0' + CONVERT(VARCHAR(2),DATEPART(MM,Transaction_Dtm)), 2)), SP_ID
						) VT
					WHERE
						VT.SP_ID = @IN_chrSPID
					) VT_RANK
				ON YM.YearMonth = VT_RANK.YearMonth) Pt2
			ON Pt1.YearMonth = Pt2.Rank_YearMonth
			INNER JOIN 
			(SELECT
				--YM.YearMonth
				--,SP.SP_ID
				--,VT_Total_SP_S.*
				--,VT_Total_SP_Prof.*
				--,VT_RANK_Prof.*
				--,YM.YearMonth
				--,VT_Total_SP_S.Service_Category_Code
				--,VT_Total_SP_Prof.YearMonth
				--,VT_Total_SP_Prof.Service_Category_Code
				--,VT_Total_SP_Prof.Total_SP
				[Rank_YearMonth_Prof] =
					CASE 
						WHEN VT_Total_SP_Prof.Total_SP IS NULL 
						THEN YM.YearMonth

						WHEN VT_Total_SP_Prof.Total_SP IS NOT NULL 
							AND VT_Total_SP_S.Service_Category_Code = VT_Total_SP_Prof.Service_Category_Code 
							AND VT_RANK_Prof.Service_Category_Code IS NULL 
						THEN YM.YearMonth

						WHEN VT_Total_SP_Prof.Total_SP IS NOT NULL 
							AND VT_Total_SP_S.Service_Category_Code = VT_Total_SP_Prof.Service_Category_Code 
							AND VT_Total_SP_Prof.Service_Category_Code = VT_RANK_Prof.Service_Category_Code 
						THEN YM.YearMonth

						ELSE NULL
					END
				,[Rank_Service_Category_Code_Prof] =
					CASE 
						WHEN VT_Total_SP_Prof.Total_SP IS NULL 
						THEN VT_Total_SP_S.Service_Category_Code

						WHEN VT_Total_SP_Prof.Total_SP IS NOT NULL 
							AND VT_Total_SP_S.Service_Category_Code = VT_Total_SP_Prof.Service_Category_Code 
							AND VT_RANK_Prof.Service_Category_Code IS NULL 
						THEN VT_Total_SP_S.Service_Category_Code

						WHEN VT_Total_SP_Prof.Total_SP IS NOT NULL 
							AND VT_Total_SP_S.Service_Category_Code = VT_Total_SP_Prof.Service_Category_Code 
							AND VT_Total_SP_Prof.Service_Category_Code = VT_RANK_Prof.Service_Category_Code 
						THEN VT_RANK_Prof.Service_Category_Code
							
						ELSE NULL
					END

				,[Rank_NoOfTrans_Prof] =
					CASE 
						WHEN VT_Total_SP_Prof.Total_SP IS NULL THEN 1

						WHEN VT_Total_SP_Prof.Total_SP IS NOT NULL 
							AND VT_Total_SP_S.Service_Category_Code = VT_Total_SP_Prof.Service_Category_Code 
							AND VT_RANK_Prof.Service_Category_Code IS NULL 
						THEN VT_Total_SP_Prof.Total_SP + 1

						WHEN VT_Total_SP_Prof.Total_SP IS NOT NULL 
							AND VT_Total_SP_S.Service_Category_Code = VT_Total_SP_Prof.Service_Category_Code 
							AND VT_Total_SP_Prof.Service_Category_Code = VT_RANK_Prof.Service_Category_Code 
						THEN VT_RANK_Prof.Rank_NoOfTrans

						ELSE NULL
					END
				,[Rank_ClaimAmount_Prof] =
					CASE 
						WHEN VT_Total_SP_Prof.Total_SP IS NULL THEN 1

						WHEN VT_Total_SP_Prof.Total_SP IS NOT NULL 
							AND VT_Total_SP_S.Service_Category_Code = VT_Total_SP_Prof.Service_Category_Code 
							AND VT_RANK_Prof.Service_Category_Code IS NULL 
						THEN VT_Total_SP_Prof.Total_SP + 1

						WHEN VT_Total_SP_Prof.Total_SP IS NOT NULL 
							AND VT_Total_SP_S.Service_Category_Code = VT_Total_SP_Prof.Service_Category_Code 
							AND VT_Total_SP_Prof.Service_Category_Code = VT_RANK_Prof.Service_Category_Code 
						THEN VT_RANK_Prof.Rank_ClaimAmount
							
						ELSE NULL
					END
				,[Rank_Average_Prof] =
					CASE 
						WHEN VT_Total_SP_Prof.Total_SP IS NULL THEN 1

						WHEN VT_Total_SP_Prof.Total_SP IS NOT NULL 
							AND VT_Total_SP_S.Service_Category_Code = VT_Total_SP_Prof.Service_Category_Code 
							AND VT_RANK_Prof.Service_Category_Code IS NULL 
						THEN VT_Total_SP_Prof.Total_SP + 1

						WHEN VT_Total_SP_Prof.Total_SP IS NOT NULL 
							AND VT_Total_SP_S.Service_Category_Code = VT_Total_SP_Prof.Service_Category_Code 
							AND VT_Total_SP_Prof.Service_Category_Code = VT_RANK_Prof.Service_Category_Code 
						THEN VT_RANK_Prof.Rank_Average
							
						ELSE NULL
					END
			FROM 
			#02_YEARMONTH YM
				CROSS JOIN 
					(SELECT 
						Prof.Service_Category_Code
					FROM
						VoucherTransaction VT WITH (NOLOCK)
							INNER JOIN Practice P WITH (NOLOCK)
								ON VT.Practice_Display_Seq = P.Display_Seq AND VT.SP_ID = P.SP_ID
							INNER JOIN Professional Prof WITH (NOLOCK)
								ON Prof.Professional_Seq = P.Professional_Seq AND Prof.SP_ID = P.SP_ID
							LEFT JOIN @tblRecordStatus tblRS
								ON LTRIM(RTRIM(VT.Record_Status)) = tblRS.Record_Status
					WHERE
						VT.SP_ID = @IN_chrSPID						
						AND (@02_dtmPeriod_From <= Transaction_Dtm AND Transaction_Dtm < DATEADD(DAY, 1, @02_dtmPeriod_TO))
						AND VT.Scheme_Code = 'HCVS'
						AND (@chrIsAllRecordStatus = 'Y' OR tblRS.Record_Status IS NOT NULL)
						AND (VT.Invalidation IS NULL OR VT.Invalidation NOT IN ('I'))
					GROUP BY
						Prof.Service_Category_Code
					) VT_Total_SP_S
				CROSS JOIN 
					(SELECT @IN_chrSPID AS [SP_ID]) SP
				LEFT OUTER JOIN 
					(
					SELECT DISTINCT
						VT_Total_SP_Prof.YearMonth
						,VT_Total_SP_Prof.Service_Category_Code
						,MAX(VT_Total_SP_Prof.SP_Count) OVER(PARTITION BY VT_Total_SP_Prof.YearMonth, VT_Total_SP_Prof.Service_Category_Code) AS [Total_SP]
					FROM
						(
						SELECT 
							(CONVERT(VARCHAR(4),DATEPART(YYYY,Transaction_Dtm)) + RIGHT('0' + CONVERT(VARCHAR(2),DATEPART(MM,Transaction_Dtm)), 2)) AS [YearMonth]
							,Prof.Service_Category_Code
							,COUNT(DISTINCT VT.SP_ID) AS [SP_Count] 
						FROM
							VoucherTransaction VT WITH (NOLOCK)
								INNER JOIN Practice P WITH (NOLOCK)
									ON VT.Practice_Display_Seq = P.Display_Seq AND VT.SP_ID = P.SP_ID
								INNER JOIN Professional Prof WITH (NOLOCK)
									ON Prof.Professional_Seq = P.Professional_Seq AND Prof.SP_ID = P.SP_ID
								LEFT JOIN @tblRecordStatus tblRS
									ON LTRIM(RTRIM(VT.Record_Status)) = tblRS.Record_Status
						WHERE
							(@02_dtmPeriod_From <= Transaction_Dtm AND Transaction_Dtm < DATEADD(DAY, 1, @02_dtmPeriod_TO))
							AND VT.Scheme_Code = 'HCVS'
							AND (@chrIsAllRecordStatus = 'Y' OR tblRS.Record_Status IS NOT NULL)
							AND (VT.Invalidation IS NULL OR VT.Invalidation NOT IN ('I'))
						GROUP BY
							(CONVERT(VARCHAR(4),DATEPART(YYYY,Transaction_Dtm)) + RIGHT('0' + CONVERT(VARCHAR(2),DATEPART(MM,Transaction_Dtm)), 2))
							,Prof.Service_Category_Code
						) VT_Total_SP_Prof
					) VT_Total_SP_Prof
				ON YM.YearMonth = VT_Total_SP_Prof.YearMonth --AND VT_Total_SP_Prof.Service_Category_Code = VT_Total_SP_S.Service_Category_Code
				LEFT OUTER JOIN 
					(
					SELECT
						VT_RANK_Prof.YearMonth
						,VT_RANK_Prof.Service_Category_Code
						,VT_RANK_Prof.Rank_NoOfTrans
						,VT_RANK_Prof.Rank_ClaimAmount
						,VT_RANK_Prof.Rank_Average
					FROM
						(SELECT 
							(CONVERT(VARCHAR(4),DATEPART(YYYY,Transaction_Dtm)) + RIGHT('0' + CONVERT(VARCHAR(2),DATEPART(MM,Transaction_Dtm)), 2)) AS [YearMonth]
							,VT.SP_ID
							,Prof.Service_Category_Code		
							,COUNT(Transaction_ID) AS [NoOfTrans]		
							,RANK() OVER (PARTITION BY (CONVERT(VARCHAR(4),DATEPART(YYYY,Transaction_Dtm)) + RIGHT('0' + CONVERT(VARCHAR(2),DATEPART(MM,Transaction_Dtm)), 2)), Prof.Service_Category_Code
											ORDER BY COUNT(Transaction_ID) DESC) AS [Rank_NoOfTrans]  
							,SUM(Claim_Amount) AS [ClaimAmount]
							,RANK() OVER (PARTITION BY (CONVERT(VARCHAR(4),DATEPART(YYYY,Transaction_Dtm)) + RIGHT('0' + CONVERT(VARCHAR(2),DATEPART(MM,Transaction_Dtm)), 2)), Prof.Service_Category_Code
											ORDER BY SUM(Claim_Amount) DESC) AS [Rank_ClaimAmount]  
							,SUM(Claim_Amount)/COUNT(Transaction_ID) AS [Average]
							,RANK() OVER (PARTITION BY (CONVERT(VARCHAR(4),DATEPART(YYYY,Transaction_Dtm)) + RIGHT('0' + CONVERT(VARCHAR(2),DATEPART(MM,Transaction_Dtm)), 2)), Prof.Service_Category_Code
											ORDER BY (SUM(Claim_Amount)/COUNT(Transaction_ID)) DESC) AS [Rank_Average]  
						FROM
							VoucherTransaction VT WITH (NOLOCK)
								INNER JOIN Practice P WITH (NOLOCK)
									ON VT.Practice_Display_Seq = P.Display_Seq AND VT.SP_ID = P.SP_ID
								INNER JOIN Professional Prof WITH (NOLOCK)
									ON Prof.Professional_Seq = P.Professional_Seq AND Prof.SP_ID = P.SP_ID
								LEFT JOIN @tblRecordStatus tblRS
									ON LTRIM(RTRIM(VT.Record_Status)) = tblRS.Record_Status
						WHERE
							(@02_dtmPeriod_From <= Transaction_Dtm AND Transaction_Dtm < DATEADD(DAY, 1, @02_dtmPeriod_TO))
							AND VT.Scheme_Code = 'HCVS'
							AND (@chrIsAllRecordStatus = 'Y' OR tblRS.Record_Status IS NOT NULL)
							AND (VT.Invalidation IS NULL OR VT.Invalidation NOT IN ('I'))
						GROUP BY
							(CONVERT(VARCHAR(4),DATEPART(YYYY,Transaction_Dtm)) + RIGHT('0' + CONVERT(VARCHAR(2),DATEPART(MM,Transaction_Dtm)), 2))
							,VT.SP_ID
							,Prof.Service_Category_Code
						) VT_RANK_Prof
					WHERE
						VT_RANK_Prof.SP_ID = @IN_chrSPID
					) VT_RANK_Prof
				ON YM.YearMonth = VT_RANK_Prof.YearMonth) Pt3
			ON Pt1.YearMonth = Pt3.Rank_YearMonth_Prof
		ORDER BY Pt1.YearMonth DESC

		SET @seq = @seq + 1

		-- ---------------------------------------------
		-- Remove Duplicate Row
		-- ---------------------------------------------

		DECLARE @SeqNo INT, @YearMonth INT, @PreviousYearMonth INT

		SET @PreviousYearMonth = 0

		DECLARE Result02_Cursor CURSOR FOR 
		SELECT Seq, Seq2 FROM #02_Result

		OPEN Result02_Cursor

		FETCH NEXT FROM Result02_Cursor 
		INTO @SeqNo, @YearMonth

		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF @PreviousYearMonth = @YearMonth
				BEGIN
					UPDATE #02_Result SET Col01 = '', Col02 = '', Col03 = '', Col04 = '', Col05 = '', Col06 = '', Col07 = '' WHERE Seq = @SeqNo
				END

			SET @PreviousYearMonth = @YearMonth

			FETCH NEXT FROM Result02_Cursor 
			INTO @SeqNo, @YearMonth
		END

		CLOSE Result02_Cursor
		DEALLOCATE Result02_Cursor

		-- ---------------------------------------------
		-- Return Result: 02-Claim pattern (Mths)
		-- ---------------------------------------------

		SELECT Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11 FROM #02_Result ORDER BY Seq, Seq2 DESC

	-- ---------------------------------------------
	-- To Excel Sheet (05): 03-Claim (Days)
	-- ---------------------------------------------

		-- ---------------------------------------------
		-- Insert Data: 03-Claim (Days)
		-- ---------------------------------------------
		SET @seq = 0

		INSERT INTO #03_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07)
		VALUES (@seq, NULL, @varPeriod_Type  + ' period: ' + CONVERT(VARCHAR(10), @01_dtmPeriod_From, 111) + ' to ' + CONVERT(VARCHAR(10), @01_dtmPeriod_TO, 111), 
				'', '', '', '', '', '')

		SET @seq = @seq + 1

		INSERT INTO #03_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07)
		VALUES (@seq, NULL, '', '', '', '', '', '', '')

		SET @seq = @seq + 1

		INSERT INTO #03_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07)
		VALUES (@seq, NULL, '', '', '', '', '', '', '')
			
		SET @seq = @seq + 1

		INSERT INTO #03_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07)
		VALUES (@seq, NULL, 
				'Service Date',		'No. of Transactions',	'Voucher Amount Claimed ($)',		'Average Voucher Amount / Transaction ($)',
				'Practice No.',		'Practice District',	'Day')

		SET @seq = @seq + 1

		SET DATEFIRST 1

		-- Transaction by each day
		INSERT INTO #03_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07)
		SELECT 
			@seq
			,NULL
			,R.Service_Receive_Dtm		
			,R.[NoOfTrans]
			,R.[ClaimAmount]	
			,R.[Average]
			,R.[PracticeID]
			,R.[District]
			,R.[Day]
		FROM
			(
			SELECT 
				CONVERT(VARCHAR(11), VTB.Service_Receive_Dtm, 113) AS [Service_Receive_Dtm]
				,COUNT(VTB.Transaction_ID) AS [NoOfTrans]		
				,SUM(VTB.Claim_Amount) AS [ClaimAmount]
				,SUM(VTB.Claim_Amount)/COUNT(VTB.Transaction_ID) AS [Average]
				,[Day] = 
					CASE 
						WHEN DATEPART(WEEKDAY,VTB.Service_Receive_Dtm) = 1 THEN 'Monday'
						WHEN DATEPART(WEEKDAY,VTB.Service_Receive_Dtm) = 2 THEN 'Tuesday'
						WHEN DATEPART(WEEKDAY,VTB.Service_Receive_Dtm) = 3 THEN 'Wednesday'
						WHEN DATEPART(WEEKDAY,VTB.Service_Receive_Dtm) = 4 THEN 'Thursday'
						WHEN DATEPART(WEEKDAY,VTB.Service_Receive_Dtm) = 5 THEN 'Friday'
						WHEN DATEPART(WEEKDAY,VTB.Service_Receive_Dtm) = 6 THEN 'Saturday'
						WHEN DATEPART(WEEKDAY,VTB.Service_Receive_Dtm) = 7 THEN 'Sunday'
						ELSE ''
					END
				,[PracticeID] =
				STUFF((SELECT 
					', ' + CAST(VTB_P.Practice_Display_Seq AS VARCHAR(MAX))
					--,RTRIM(LTRIM(P.District))
				FROM
					#VoucherTransactionBase VTB_P
				WHERE
					VTB_P.Service_Receive_Dtm = VTB.Service_Receive_Dtm
				GROUP BY
					VTB_P.Service_Receive_Dtm, VTB_P.Practice_Display_Seq
				FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)'),1,2,'')

				,[District] =
				STUFF((SELECT 
					', ' + CAST(D.district_name AS VARCHAR(MAX))
					--,RTRIM(LTRIM(P.District))
				FROM
					#VoucherTransactionBase VTB_D
						INNER JOIN district D
							ON VTB_D.District = D.district_code
				WHERE
					VTB_D.Service_Receive_Dtm = VTB.Service_Receive_Dtm
				GROUP BY
					VTB_D.Service_Receive_Dtm, D.district_name
				FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)'),1,2,'')
			FROM
				#VoucherTransactionBase VTB
			GROUP BY
				VTB.Service_Receive_Dtm
			) R	
		ORDER BY
			R.Service_Receive_Dtm

		SET @seq = @seq + 1

		-- Total
		INSERT INTO #03_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07)
		SELECT 
			@seq
			,NULL
			,'Total'
			,R.[NoOfTrans]
			,R.[ClaimAmount]	
			,R.[Average]	
			,''
			,''
			,''
		FROM
			(SELECT 
				COUNT(VTB.Transaction_ID) AS [NoOfTrans]		
				,SUM(VTB.Claim_Amount) AS [ClaimAmount]
				,SUM(VTB.Claim_Amount)/COUNT(VTB.Transaction_ID) AS [Average]
			FROM
				#VoucherTransactionBase VTB
			GROUP BY
				VTB.SP_ID) R	

		-- ---------------------------------------------
		-- Return Result: 03-Claim (Days)
		-- ---------------------------------------------

		SELECT Col01, Col02, Col03, Col04, Col05, Col06, Col07 FROM #03_Result ORDER BY Seq, Seq2, Col01

	-- ---------------------------------------------
	-- To Excel Sheet (06): 04-Means of input
	-- ---------------------------------------------
		DECLARE @SumOfTran INT

		-- ---------------------------------------------
		-- Insert Data: 04-Means of input
		-- ---------------------------------------------
		SET @seq = 0

		INSERT INTO #04_Result (Seq, Seq2, Col01, Col02, Col03)
		VALUES (@seq, NULL, @varPeriod_Type  + ' period: ' + CONVERT(VARCHAR(10), @01_dtmPeriod_From, 111) + ' to ' + CONVERT(VARCHAR(10), @01_dtmPeriod_TO, 111), 
				'', '')

		SET @seq = @seq + 1

		INSERT INTO #04_Result (Seq, Seq2, Col01, Col02, Col03)
		VALUES (@seq, NULL, '', '', '')

		SET @seq = @seq + 1

		INSERT INTO #04_Result (Seq, Seq2, Col01, Col02, Col03)
		VALUES (@seq, NULL, '', '', '')
			
		SET @seq = @seq + 1

		INSERT INTO #04_Result (Seq, Seq2, Col01, Col02, Col03)
		VALUES (@seq, NULL, 'Means of input', 'No. of Transactions', '% of Transactions')

		SET @seq = @seq + 1

		INSERT INTO #04_Result (Seq, Seq2, Col01, Col02, Col03)
		VALUES (@seq, NULL, 'Manual', '0', '0')

		SET @seq = @seq + 1

		INSERT INTO #04_Result (Seq, Seq2, Col01, Col02, Col03)
		VALUES (@seq, NULL, 'Smart IC', '0', '0')

		SET @seq = @seq + 1

		-- ---------------------------------------------
		-- Update no. of Transactions
		-- ---------------------------------------------
		UPDATE 
			#04_Result 
		SET 
			Col02 = R.NoOfTrans
		FROM
			(SELECT 
				[MeanOfInput] = 
					CASE
						WHEN VTB.Create_By_SmartID IS NULL THEN 'Manual'
						WHEN VTB.Create_By_SmartID = 'N' THEN 'Manual'
						WHEN VTB.Create_By_SmartID = 'Y' THEN 'Smart IC'
					END,
				COUNT(1) AS [NoOfTrans]		
			FROM
				#VoucherTransactionBase VTB
			GROUP BY
				VTB.Create_By_SmartID) R
		WHERE
			#04_Result.Col01 = R.MeanOfInput

		-- ---------------------------------------------
		-- Update % of Transactions
		-- ---------------------------------------------
		SET @SumOfTran = (SELECT SUM(CONVERT(INT,Col02)) FROM #04_Result WHERE Col01 IN ('Manual','Smart IC'))

		IF @SumOfTran IS NOT NULL 
		BEGIN
			IF @SumOfTran > 0
			BEGIN
				UPDATE 
					#04_Result
				SET 
					Col03 = (CONVERT(INT,Col02) + 0.0) / @SumOfTran * 100
				WHERE
					Col01 IN ('Manual','Smart IC')
			END
		END

		-- ---------------------------------------------
		-- Return Result: 04-Means of input
		-- ---------------------------------------------

		SELECT Col01, Col02, Col03 FROM #04_Result ORDER BY Seq, Seq2

	-- ---------------------------------------------
	-- To Excel Sheet (07): 05-Percentile
	-- ---------------------------------------------

		-- ---------------------------------------------
		-- Insert Data: 05-Percentile
		-- ---------------------------------------------
		SET @seq = 0

		INSERT INTO #05_Result (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @varPeriod_Type  + ' period: ' + CONVERT(VARCHAR(10), @01_dtmPeriod_From, 111) + ' to ' + CONVERT(VARCHAR(10), @01_dtmPeriod_TO, 111), 
				'')

		SET @seq = @seq + 1

		INSERT INTO #05_Result (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '', '')

		SET @seq = @seq + 1

		INSERT INTO #05_Result (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '', '')
			
		SET @seq = @seq + 1

		INSERT INTO #05_Result (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Claim Pattern', 'Amount Claimed ($)')

		SET @seq = @seq + 1

		INSERT INTO #05_Result (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '25 percentile', '0')

		SET @seq = @seq + 1

		INSERT INTO #05_Result (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '50 percentile', '0')

		SET @seq = @seq + 1

		INSERT INTO #05_Result (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '75 percentile', '0')

		SET @seq = @seq + 1

		INSERT INTO #05_Result (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Minimum', '0')

		SET @seq = @seq + 1

		INSERT INTO #05_Result (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Maximum', '0')

		SET @seq = @seq + 1

		-- ---------------------------------------------
		-- Update no. of Transactions
		-- ---------------------------------------------

		-----------------
		-- 25 percentile
		-----------------
		UPDATE 
			#05_Result
		SET 
			Col02 = 
			(SELECT
				(R_ASC.Claim_Amount + R_DESC.Claim_Amount) / 2.0 AS [ClaimedAmount]
			FROM
				(
				SELECT
					[SeqNo] = 1
					,R_ASC_Sub1.Claim_Amount	
				FROM
					(SELECT TOP 25 PERCENT
						ROW_NUMBER() OVER(ORDER BY VTB.Claim_Amount) AS RANK_Claim_Amount
						,Claim_Amount		
					FROM
						#VoucherTransactionBase VTB
					) R_ASC_Sub1
				WHERE 
					R_ASC_Sub1.RANK_Claim_Amount =
					(SELECT
						MAX(R_ASC_Sub2.RANK_Claim_Amount)			
					FROM
						(SELECT TOP 25 PERCENT
							ROW_NUMBER() OVER(ORDER BY VTB.Claim_Amount) AS RANK_Claim_Amount
							,Claim_Amount		
						FROM
							#VoucherTransactionBase VTB
						) R_ASC_Sub2
					)
				) R_ASC
				INNER JOIN
				(
				SELECT
					[SeqNo] = 1
					,R_DESC_Sub1.Claim_Amount
				FROM
					(SELECT TOP 75 PERCENT
						ROW_NUMBER() OVER(ORDER BY VTB.Claim_Amount DESC) AS RANK_Claim_Amount
						,Claim_Amount		
					FROM
						#VoucherTransactionBase VTB
					) R_DESC_Sub1
				WHERE 
					R_DESC_Sub1.RANK_Claim_Amount  = 
					(SELECT
						MAX(R_DESC_Sub2.RANK_Claim_Amount)
					FROM
						(SELECT TOP 75 PERCENT
							ROW_NUMBER() OVER(ORDER BY VTB.Claim_Amount DESC) AS RANK_Claim_Amount
							,Claim_Amount		
						FROM
							#VoucherTransactionBase VTB
						) R_DESC_Sub2
					)

				) R_DESC
					ON R_ASC.SeqNo = R_DESC.SeqNo)
		WHERE
			#05_Result.Col01 = '25 percentile'

		-----------------
		-- 50 percentile
		-----------------
		UPDATE 
			#05_Result
		SET 
			Col02 = 
			(SELECT
				(R_ASC.Claim_Amount + R_DESC.Claim_Amount) / 2.0 AS [ClaimedAmount]
			FROM
				(
				SELECT
					[SeqNo] = 1
					,R_ASC_Sub1.Claim_Amount	
				FROM
					(SELECT TOP 50 PERCENT
						ROW_NUMBER() OVER(ORDER BY VTB.Claim_Amount) AS RANK_Claim_Amount
						,Claim_Amount		
					FROM
						#VoucherTransactionBase VTB
					) R_ASC_Sub1
				WHERE 
					R_ASC_Sub1.RANK_Claim_Amount =
					(SELECT
						MAX(R_ASC_Sub2.RANK_Claim_Amount)			
					FROM
						(SELECT TOP 50 PERCENT
							ROW_NUMBER() OVER(ORDER BY VTB.Claim_Amount) AS RANK_Claim_Amount
							,Claim_Amount		
						FROM
							#VoucherTransactionBase VTB
						) R_ASC_Sub2
					)
				) R_ASC
				INNER JOIN
				(
				SELECT
					[SeqNo] = 1
					,R_DESC_Sub1.Claim_Amount
				FROM
					(SELECT TOP 50 PERCENT
						ROW_NUMBER() OVER(ORDER BY VTB.Claim_Amount DESC) AS RANK_Claim_Amount
						,Claim_Amount		
					FROM
						#VoucherTransactionBase VTB
					) R_DESC_Sub1
				WHERE 
					R_DESC_Sub1.RANK_Claim_Amount  = 
					(SELECT
						MAX(R_DESC_Sub2.RANK_Claim_Amount)
					FROM
						(SELECT TOP 50 PERCENT
							ROW_NUMBER() OVER(ORDER BY VTB.Claim_Amount DESC) AS RANK_Claim_Amount
							,Claim_Amount		
						FROM
							#VoucherTransactionBase VTB
						) R_DESC_Sub2
					)

				) R_DESC
					ON R_ASC.SeqNo = R_DESC.SeqNo)
		WHERE
			#05_Result.Col01 = '50 percentile'

		-----------------
		-- 75 percentile
		-----------------
		UPDATE 
			#05_Result
		SET 
			Col02 = 
			(SELECT
				(R_ASC.Claim_Amount + R_DESC.Claim_Amount) / 2.0 AS [ClaimedAmount]
			FROM
				(
				SELECT
					[SeqNo] = 1
					,R_ASC_Sub1.Claim_Amount	
				FROM
					(SELECT TOP 75 PERCENT
						ROW_NUMBER() OVER(ORDER BY VTB.Claim_Amount) AS RANK_Claim_Amount
						,Claim_Amount		
					FROM
						#VoucherTransactionBase VTB
					) R_ASC_Sub1
				WHERE 
					R_ASC_Sub1.RANK_Claim_Amount =
					(SELECT
						MAX(R_ASC_Sub2.RANK_Claim_Amount)			
					FROM
						(SELECT TOP 75 PERCENT
							ROW_NUMBER() OVER(ORDER BY VTB.Claim_Amount) AS RANK_Claim_Amount
							,Claim_Amount		
						FROM
							#VoucherTransactionBase VTB
						) R_ASC_Sub2
					)
				) R_ASC
				INNER JOIN
				(
				SELECT
					[SeqNo] = 1
					,R_DESC_Sub1.Claim_Amount
				FROM
					(SELECT TOP 25 PERCENT
						ROW_NUMBER() OVER(ORDER BY VTB.Claim_Amount DESC) AS RANK_Claim_Amount
						,Claim_Amount		
					FROM
						#VoucherTransactionBase VTB
					) R_DESC_Sub1
				WHERE 
					R_DESC_Sub1.RANK_Claim_Amount  = 
					(SELECT
						MAX(R_DESC_Sub2.RANK_Claim_Amount)
					FROM
						(SELECT TOP 25 PERCENT
							ROW_NUMBER() OVER(ORDER BY VTB.Claim_Amount DESC) AS RANK_Claim_Amount
							,Claim_Amount		
						FROM
							#VoucherTransactionBase VTB
						) R_DESC_Sub2
					)

				) R_DESC
					ON R_ASC.SeqNo = R_DESC.SeqNo)
		WHERE
			#05_Result.Col01 = '75 percentile'

		-- ---------------------------------------------
		-- Update % of Transactions
		-- ---------------------------------------------
		UPDATE 
			#05_Result
		SET 
			Col02 = CASE 
						WHEN Col01 = 'Minimum' THEN ISNULL(R.[MinAmountClaimed],'0') 
						WHEN Col01 = 'Maximum' THEN ISNULL(R.[MaxAmountClaimed],'0')  
					END
		FROM
			(SELECT 
				MIN(Claim_Amount) AS [MinAmountClaimed]		
				,MAX(Claim_Amount) AS [MaxAmountClaimed]		
			FROM
				#VoucherTransactionBase VTB
			) R
		WHERE 
			Col01 IN ('Minimum','Maximum')

		-- ---------------------------------------------
		-- Return Result: 05-Percentile
		-- ---------------------------------------------

		SELECT Col01, Col02 FROM #05_Result ORDER BY Seq, Seq2

	-- ---------------------------------------------
	-- To Excel Sheet (08): 06-Amt range
	-- ---------------------------------------------
		-- Ceilling of Voucher
		DECLARE @Ceiling AS INT
		SET @Ceiling = (SELECT Num_Subsidize_Ceiling FROM SubsidizeGroupClaim WHERE Scheme_Code = 'HCVS' AND Claim_Period_From <= @01_dtmPeriod_From AND @01_dtmPeriod_From < Claim_Period_To) 

		-- Group the summary by range
		DECLARE @VoucherGroupRange FLOAT
		SET @VoucherGroupRange = 500.0

		-- ---------------------------------------------
		-- Insert Data: 06-Amt range
		-- ---------------------------------------------
		SET @seq = 0

		INSERT INTO #06_Result (Seq, Seq2, Col01, Col02, Col03)
		VALUES (@seq, NULL, @varPeriod_Type  + ' period: ' + CONVERT(VARCHAR(10), @01_dtmPeriod_From, 111) + ' to ' + CONVERT(VARCHAR(10), @01_dtmPeriod_TO, 111), 
				'', '')

		SET @seq = @seq + 1

		INSERT INTO #06_Result (Seq, Seq2, Col01, Col02, Col03)
		VALUES (@seq, NULL, '', '', '')

		SET @seq = @seq + 1

		INSERT INTO #06_Result (Seq, Seq2, Col01, Col02, Col03)
		VALUES (@seq, NULL, '', '', '')
			
		SET @seq = @seq + 1

		INSERT INTO #06_Result (Seq, Seq2, Col01, Col02, Col03)
		VALUES (@seq, NULL, 'Voucher Amount ($)', 'No. of Claims', '% of Claims')

		SET @seq = @seq + 1

		-- --------------
		-- Add Group Range
		-- --------------
		DECLARE @CurrentAmt INT
		DECLARE @GroupRange VARCHAR(1000)

		SET @CurrentAmt = 0

		WHILE @CurrentAmt < @Ceiling
		BEGIN
			IF @CurrentAmt >= @VoucherGroupRange 
				BEGIN
					SET @GroupRange = CONVERT(VARCHAR(1000), @CurrentAmt+1) + '-' + CONVERT(VARCHAR(1000), @CurrentAmt+@VoucherGroupRange)
				END
			ELSE 
				BEGIN
					SET @GroupRange = 'Below ' + CONVERT(VARCHAR(1000), @VoucherGroupRange) 
				END					

			INSERT INTO #06_Result (Seq, Seq2, Col01, Col02, Col03)
			VALUES (@seq, @CurrentAmt, @GroupRange, '0', '0')
			
			SET @seq = @seq + 1

			SET @CurrentAmt = @CurrentAmt + @VoucherGroupRange
		END

		-- ---------------------------------------------
		-- Update no. of claims
		-- ---------------------------------------------
		UPDATE 
			#06_Result
		SET
			Col02 = R.NoOfTrans
		FROM
			(SELECT 
				[GroupRange] = 
					CASE 
						WHEN CONVERT(INT, VTB.Claim_Amount) % CONVERT(INT, @VoucherGroupRange) = 0 THEN FLOOR(VTB.Claim_Amount / @VoucherGroupRange) - 1
						ELSE FLOOR(VTB.Claim_Amount / @VoucherGroupRange)
					END * @VoucherGroupRange,
				COUNT(1) AS [NoOfTrans]		
			FROM
				#VoucherTransactionBase VTB
			GROUP BY
				(CASE 
					WHEN CONVERT(INT, VTB.Claim_Amount) % CONVERT(INT, @VoucherGroupRange) = 0 THEN FLOOR(VTB.Claim_Amount / @VoucherGroupRange) - 1
					ELSE FLOOR(VTB.Claim_Amount / @VoucherGroupRange) 
				END) * @VoucherGroupRange
			) R
		WHERE
			#06_Result.Seq2 = R.GroupRange

		-- ---------------------------------------------
		-- Update % of Claims
		-- ---------------------------------------------
		IF @SumOfTran IS NOT NULL 
		BEGIN
			IF @SumOfTran > 0
			BEGIN
				UPDATE 
					#06_Result
				SET 
					Col03 = (CONVERT(INT,Col02) + 0.0) / @SumOfTran * 100
				WHERE
					Seq2 IS NOT NULL
			END
		END

		-- ---------------------------------------------
		-- Update % of Claims
		-- ---------------------------------------------
		INSERT INTO #06_Result (Seq, Seq2, Col01, Col02, Col03)
		VALUES (@seq, NULL, 'Total no. of Claims', @SumOfTran, '')

		SET @seq = @seq + 1

		-- ---------------------------------------------
		-- Return Result: 06-Amt range
		-- ---------------------------------------------

		SELECT Col01, Col02, Col03 FROM #06_Result ORDER BY Seq, Seq2

	-- ---------------------------------------------
	-- To Excel Sheet (09): 07- Over amt
	-- ---------------------------------------------
		DECLARE @SumOfTranOverAmt INT

		-- ---------------------------------------------
		-- Insert Data: 07- Over amt
		-- ---------------------------------------------
		SET @seq = 0

		INSERT INTO #07_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07)
		VALUES (@seq, NULL, @varPeriod_Type  + ' period: ' + CONVERT(VARCHAR(10), @01_dtmPeriod_From, 111) + ' to ' + CONVERT(VARCHAR(10), @01_dtmPeriod_TO, 111), 
				'', '', '', '', '', '')

		SET @seq = @seq + 1

		INSERT INTO #07_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07)
		VALUES (@seq, NULL, '', '', '', '', '', '', '')

		SET @seq = @seq + 1

		INSERT INTO #07_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07)
		VALUES (@seq, NULL, 'Total no. of transactions:', '', '', '', '', '', '')
			
		SET @seq = @seq + 1

		INSERT INTO #07_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07)
		VALUES (@seq, NULL, '', '', '', '', '', '', '')
			
		SET @seq = @seq + 1

		INSERT INTO #07_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07)
		VALUES (@seq, NULL, 
				'Transaction ID',		'Transaction Time',	'Service Date',		'Amount Claimed ($)',
				'Means of Input',		'Practice No.',		'District of Practice')

		SET @seq = @seq + 1

		-- ---------------------------------------------
		-- Transaction over amount claimed XXXX
		-- ---------------------------------------------
		INSERT INTO #07_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07)
		SELECT 
			@seq
			,1
			,R.[Transaction_ID]		
			,R.[Transaction_Dtm]		
			,R.[Service_Receive_Dtm]
			,R.[ClaimAmount]	
			,R.[MeansOfInput]
			,R.[PracticeID]
			,R.[District]
		FROM
			(
			SELECT 
				[Transaction_ID] = dbo.func_format_system_number(VTB.Transaction_ID)
				,[Transaction_Dtm] = Format(VTB.Transaction_Dtm, 'yyyy/MM/dd hh:mm:ss')
				,[Service_Receive_Dtm] = CONVERT(VARCHAR(11), VTB.Service_Receive_Dtm, 113)
				,[ClaimAmount] = VTB.Claim_Amount 
				,[MeansOfInput] = 
					CASE 
						WHEN VTB.Create_By_SmartID IS NULL THEN 'Manual'
						WHEN VTB.Create_By_SmartID = 'N' THEN 'Manual'
						WHEN VTB.Create_By_SmartID = 'Y' THEN 'Smart IC'
					END
				,[PracticeID] = VTB.Practice_Display_Seq

				,[District] = CAST(D.district_name AS VARCHAR(MAX))
			FROM
				#VoucherTransactionBase VTB
					INNER JOIN district D
						ON VTB.District = D.district_code
			WHERE
				VTB.Claim_Amount > @IN_intCompareValue
			) R	

		SET @seq = @seq + 1

		-- ---------------------------------------------
		-- Update no. of transactions
		-- ---------------------------------------------
		SET @SumOfTranOverAmt = (SELECT COUNT(1) FROM #07_Result WHERE Seq2 IS NOT NULL)

		UPDATE 
			#07_Result
		SET 
			Col01 = 'Total no. of transactions: ' + CONVERT(VARCHAR(1000),@SumOfTranOverAmt)
		WHERE
			Col01 = 'Total no. of transactions:'

		-- ---------------------------------------------
		-- Return Result: 07- Over amt
		-- ---------------------------------------------

		SELECT Col01, Col02, Col03, Col04, Col05, Col06, Col07 FROM #07_Result ORDER BY Seq, Seq2, Col03

	-- ---------------------------------------------
	-- To Excel Sheet (10): 08-Highest tx
	-- ---------------------------------------------
		DECLARE @SumOfTranWithHighestTotalAmt INT
		DECLARE @ServiceDateWithHighestTotalAmt VARCHAR(1000)

		SET @ServiceDateWithHighestTotalAmt = 'N/A'

		-- ---------------------------------------------
		-- Find the highest daily no. of transactions
		-- ---------------------------------------------
		SELECT
			@SumOfTranWithHighestTotalAmt = ISNULL(MAX(R.NoOfTrans),0)
		FROM 
			(SELECT
				VTB.Service_Receive_Dtm,
				COUNT(1) AS [NoOfTrans]		
			FROM
				#VoucherTransactionBase VTB
			GROUP BY
				VTB.Service_Receive_Dtm) R

		-- ---------------------------------------------------------------
		-- Find service date with the highest daily no. of transactions
		-- ---------------------------------------------------------------
		IF @SumOfTranWithHighestTotalAmt IS NOT NULL
		BEGIN
			IF @SumOfTranWithHighestTotalAmt > 0 
			BEGIN
				-- Collect service date
				INSERT @tblDateWithHighestNoOfTran(Service_Receive_Dtm)
				SELECT
					VTB.Service_Receive_Dtm	
				FROM
					#VoucherTransactionBase VTB
				GROUP BY
					VTB.Service_Receive_Dtm
				HAVING COUNT(VTB.Service_Receive_Dtm) = @SumOfTranWithHighestTotalAmt

				-- Concatenate the service date in one string
				SET @ServiceDateWithHighestTotalAmt = STUFF((SELECT ', ' + CONVERT(VARCHAR(11), [Service_Receive_Dtm], 113) FROM @tblDateWithHighestNoOfTran FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)'),1,2,'')
			END
		END

		-- ---------------------------------------------
		-- Insert Data: 08-Highest tx
		-- ---------------------------------------------
		SET @seq = 0

		INSERT INTO #08_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09)
		VALUES (@seq, NULL, @varPeriod_Type  + ' period: ' + CONVERT(VARCHAR(10), @01_dtmPeriod_From, 111) + ' to ' + CONVERT(VARCHAR(10), @01_dtmPeriod_TO, 111), 
				'', '', '', '', '', '', '', '')

		SET @seq = @seq + 1

		INSERT INTO #08_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09)
		VALUES (@seq, NULL, '', '', '', '', '', '', '', '', '')

		SET @seq = @seq + 1

		INSERT INTO #08_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09)
		VALUES (@seq, NULL, 'Total no. of transactions: ' + CONVERT(VARCHAR(1000), @SumOfTranWithHighestTotalAmt), '', '', '', '', '', '', '', '')
			
		SET @seq = @seq + 1

		INSERT INTO #08_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09)
		VALUES (@seq, NULL, 
			'Service date with highest total daily no. of transaction: ' + @ServiceDateWithHighestTotalAmt, 
			'', '', '', '', '', '', '', '')
			
		SET @seq = @seq + 1

		INSERT INTO #08_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09)
		VALUES (@seq, NULL, '', '', '', '', '', '', '', '', '')
			
		SET @seq = @seq + 1
		
		-- -----------------------------------------------------------
		-- Transaction of the highest daily no. of claims (All Date)
		-- -----------------------------------------------------------
		IF @SumOfTranWithHighestTotalAmt IS NOT NULL
		BEGIN
			IF @SumOfTranWithHighestTotalAmt > 0 
			BEGIN
				DECLARE @TimeDiffThreshold AS INT
				SET @TimeDiffThreshold = 300 -- 5 minutes

				DECLARE @TotalTimeDiff INT
				DECLARE @TotalNoOfTimeDiff INT
				DECLARE @AverageTimeDiff INT

				DECLARE @DateHighestTran AS DATETIME


				DECLARE Result08_Cursor CURSOR FOR 
					SELECT Service_Receive_Dtm FROM @tblDateWithHighestNoOfTran

				OPEN Result08_Cursor

				FETCH NEXT FROM Result08_Cursor INTO @DateHighestTran

				WHILE @@FETCH_STATUS = 0
				BEGIN
					-- ------------------------------------------------------------
					-- Transaction of the highest daily no. of claims (Each Date)
					-- ------------------------------------------------------------
					-- Header
					INSERT INTO #08_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09)
					VALUES (@seq, NULL, 
							'Transaction ID',		'Transaction Time',	'Service Date',		'Amount Claimed ($)',
							'Means of Input',		'Practice No.',		'District of Practice', 'Time between 2 transactions', 'Less than 5 min')

					SET @seq = @seq + 1

					-- Content
					INSERT INTO #08_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09)
					SELECT 
						@seq
						,1
						,[Transaction_ID] = dbo.func_format_system_number(R1.[Transaction_ID])		
						,[Transaction_Dtm] = Format(R1.[Transaction_Dtm], 'yyyy/MM/dd HH:mm:ss')
						,[Service_Receive_Dtm] = CONVERT(VARCHAR(11), R1.[Service_Receive_Dtm], 113)
						,R1.[ClaimAmount]	
						,R1.[MeansOfInput]
						,R1.[PracticeID]
						,R1.[District]
						,[TimeDiff] = 
							CASE	
								WHEN R1.Transaction_Dtm IS NULL OR R2.Transaction_Dtm IS NULL THEN 'NA'
								WHEN R1.Transaction_Dtm IS NOT NULL OR R2.Transaction_Dtm IS NOT NULL 
									THEN CONVERT(VARCHAR(1000), DATEDIFF(SS,R2.Transaction_Dtm,R1.Transaction_Dtm) / 3600) + ':' +
											RIGHT('0' + CONVERT(VARCHAR(1000), (DATEDIFF(SS,R2.Transaction_Dtm,R1.Transaction_Dtm) / 60) % 60),2) + ':' +
											RIGHT('0' + CONVERT(VARCHAR(1000), DATEDIFF(SS,R2.Transaction_Dtm,R1.Transaction_Dtm) % 60),2)
								ELSE 'NA'
							END
						,[LessThan] = 
							CASE	
								WHEN R1.Transaction_Dtm IS NULL OR R2.Transaction_Dtm IS NULL THEN 'N'
								WHEN R1.Transaction_Dtm IS NOT NULL OR R2.Transaction_Dtm IS NOT NULL THEN 
									CASE
										WHEN DATEDIFF(SS,R2.Transaction_Dtm,R1.Transaction_Dtm) < @TimeDiffThreshold THEN 'Y'
										ELSE 'N'
									END
								ELSE 'N'
							END
					FROM
						(SELECT 
							[SeqNo] = ROW_NUMBER() OVER(ORDER BY VTB.Transaction_Dtm)
							,[Transaction_ID] = VTB.Transaction_ID
							,[Transaction_Dtm] = VTB.Transaction_Dtm
							,[Service_Receive_Dtm] = VTB.Service_Receive_Dtm
							,[ClaimAmount] = VTB.Claim_Amount 
							,[MeansOfInput] = 
								CASE 
									WHEN VTB.Create_By_SmartID IS NULL THEN 'Manual'
									WHEN VTB.Create_By_SmartID = 'N' THEN 'Manual'
									WHEN VTB.Create_By_SmartID = 'Y' THEN 'Smart IC'
								END
							,[PracticeID] = VTB.Practice_Display_Seq

							,[District] = CAST(D.district_name AS VARCHAR(MAX))
						FROM
							#VoucherTransactionBase VTB
								INNER JOIN district D
									ON VTB.District = D.district_code
						WHERE
							VTB.Service_Receive_Dtm = @DateHighestTran
						) R1	
						LEFT OUTER JOIN
						(SELECT 
							[SeqNo] = ROW_NUMBER() OVER(ORDER BY VTB.Transaction_Dtm)
							,[Transaction_ID] = VTB.Transaction_ID
							,[Transaction_Dtm] = VTB.Transaction_Dtm
							,[Service_Receive_Dtm] = VTB.Service_Receive_Dtm
							,[ClaimAmount] = VTB.Claim_Amount 
							,[MeansOfInput] = 
								CASE 
									WHEN VTB.Create_By_SmartID IS NULL THEN 'Manual'
									WHEN VTB.Create_By_SmartID = 'N' THEN 'Manual'
									WHEN VTB.Create_By_SmartID = 'Y' THEN 'Smart IC'
								END
							,[PracticeID] = VTB.Practice_Display_Seq

							,[District] = CAST(D.district_name AS VARCHAR(MAX))
						FROM
							#VoucherTransactionBase VTB
								INNER JOIN district D
									ON VTB.District = D.district_code
						WHERE
							VTB.Service_Receive_Dtm = @DateHighestTran
						) R2
							ON R1.[SeqNo] = R2.[SeqNo] + 1
						
						
					SET @seq = @seq + 1

					-- Total
					SELECT 
						@TotalTimeDiff = 
							SUM(CASE	
								WHEN R1.Transaction_Dtm IS NULL OR R2.Transaction_Dtm IS NULL THEN 0
								WHEN R1.Transaction_Dtm IS NOT NULL OR R2.Transaction_Dtm IS NOT NULL THEN  DATEDIFF(SS,R2.Transaction_Dtm,R1.Transaction_Dtm) 
								ELSE 0
							END),
						@TotalNoOfTimeDiff = 
							SUM(CASE	
								WHEN R1.Transaction_Dtm IS NULL OR R2.Transaction_Dtm IS NULL THEN 0
								WHEN R1.Transaction_Dtm IS NOT NULL OR R2.Transaction_Dtm IS NOT NULL THEN 1
								ELSE 0
							END)
					FROM
						(SELECT 
							[SeqNo] = ROW_NUMBER() OVER(ORDER BY VTB.Transaction_Dtm)
							,[Transaction_ID] = VTB.Transaction_ID
							,[Transaction_Dtm] = VTB.Transaction_Dtm
							,[Service_Receive_Dtm] = VTB.Service_Receive_Dtm
							,[ClaimAmount] = VTB.Claim_Amount 
							,[MeansOfInput] = 
								CASE 
									WHEN VTB.Create_By_SmartID IS NULL THEN 'Manual'
									WHEN VTB.Create_By_SmartID = 'N' THEN 'Manual'
									WHEN VTB.Create_By_SmartID = 'Y' THEN 'Smart IC'
								END
							,[PracticeID] = VTB.Practice_Display_Seq

							,[District] = CAST(D.district_name AS VARCHAR(MAX))
						FROM
							#VoucherTransactionBase VTB
								INNER JOIN district D
									ON VTB.District = D.district_code
						WHERE
							VTB.Service_Receive_Dtm = @DateHighestTran
						) R1	
						LEFT OUTER JOIN
						(SELECT 
							[SeqNo] = ROW_NUMBER() OVER(ORDER BY VTB.Transaction_Dtm)
							,[Transaction_ID] = VTB.Transaction_ID
							,[Transaction_Dtm] = VTB.Transaction_Dtm
							,[Service_Receive_Dtm] = VTB.Service_Receive_Dtm
							,[ClaimAmount] = VTB.Claim_Amount 
							,[MeansOfInput] = 
								CASE 
									WHEN VTB.Create_By_SmartID IS NULL THEN 'Manual'
									WHEN VTB.Create_By_SmartID = 'N' THEN 'Manual'
									WHEN VTB.Create_By_SmartID = 'Y' THEN 'Smart IC'
								END
							,[PracticeID] = VTB.Practice_Display_Seq

							,[District] = CAST(D.district_name AS VARCHAR(MAX))
						FROM
							#VoucherTransactionBase VTB
								INNER JOIN district D
									ON VTB.District = D.district_code
						WHERE
							VTB.Service_Receive_Dtm = @DateHighestTran
						) R2
							ON R1.[SeqNo] = R2.[SeqNo] + 1
						
					IF @TotalNoOfTimeDiff > 0
						BEGIN
							SET @AverageTimeDiff = @TotalTimeDiff / @TotalNoOfTimeDiff
						END
					ELSE
						BEGIN
							SET @AverageTimeDiff = 0
						END

					INSERT INTO #08_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09)
					VALUES (@seq, NULL, '', '', 'Total', 
							(SELECT SUM(Claim_Amount) FROM #VoucherTransactionBase WHERE Service_Receive_Dtm = @DateHighestTran), 
							'', '', 'Average time between transactions', 
							CONVERT(VARCHAR(1000), (((@AverageTimeDiff) / 60) /60) % 60) + ':' +
								RIGHT('0' + CONVERT(VARCHAR(1000), ((@AverageTimeDiff) / 60) % 60),2) + ':' +
								RIGHT('0' + CONVERT(VARCHAR(1000), (@AverageTimeDiff) % 60),2), 
							'')
			
					SET @seq = @seq + 1

					-- Spacing
					INSERT INTO #08_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09)
					VALUES (@seq, NULL, '', '', '', '', '', '', '', '', '')
			
					SET @seq = @seq + 1

					FETCH NEXT FROM Result08_Cursor INTO @DateHighestTran
				END

				CLOSE Result08_Cursor
				DEALLOCATE Result08_Cursor
			END
		END

		-- ---------------------------------------------
		-- Return Result: 08-Highest tx
		-- ---------------------------------------------

		SELECT Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09 FROM #08_Result ORDER BY Seq, Seq2, Col02

	-- ---------------------------------------------
	-- To Excel Sheet (11): 09-Highest amt
	-- ---------------------------------------------
		DECLARE @HighestTotalClaimAmount INT
		DECLARE @ServiceDateWithHighestTotalClaimAmount VARCHAR(1000)

		SET @ServiceDateWithHighestTotalClaimAmount = 'N/A'

		-- ---------------------------------------------
		-- Find the highest daily claimed amount
		-- ---------------------------------------------
		SELECT
			@HighestTotalClaimAmount = ISNULL(MAX(R.[TotalClaimAmount]),0)
		FROM 
			(SELECT
				VTB.Service_Receive_Dtm,
				SUM(VTB.Claim_Amount) AS [TotalClaimAmount]		
			FROM
				#VoucherTransactionBase VTB
			GROUP BY
				VTB.Service_Receive_Dtm) R

		-- ---------------------------------------------------------
		-- Find service date with the highest daily claimed amount
		-- ---------------------------------------------------------
		IF @HighestTotalClaimAmount IS NOT NULL
		BEGIN
			IF @HighestTotalClaimAmount > 0 
			BEGIN
				-- Collect service date
				INSERT @tblDateWithHighestClaimAmt(Service_Receive_Dtm)
				SELECT
					VTB.Service_Receive_Dtm	
				FROM
					#VoucherTransactionBase VTB
				GROUP BY
					VTB.Service_Receive_Dtm
				HAVING SUM(VTB.Claim_Amount) = @HighestTotalClaimAmount

				-- Concatenate the service date in one string
				SET @ServiceDateWithHighestTotalClaimAmount = STUFF((SELECT ', ' + CONVERT(VARCHAR(11), [Service_Receive_Dtm], 113) FROM @tblDateWithHighestClaimAmt FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)'),1,2,'')
			END
		END

		-- ---------------------------------------------
		-- Insert Data: 09-Highest amt
		-- ---------------------------------------------
		SET @seq = 0

		INSERT INTO #09_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09)
		VALUES (@seq, NULL, @varPeriod_Type  + ' period: ' + CONVERT(VARCHAR(10), @01_dtmPeriod_From, 111) + ' to ' + CONVERT(VARCHAR(10), @01_dtmPeriod_TO, 111), 
				'', '', '', '', '', '', '', '')

		SET @seq = @seq + 1

		INSERT INTO #09_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09)
		VALUES (@seq, NULL, '', '', '', '', '', '', '', '', '')

		SET @seq = @seq + 1

		INSERT INTO #09_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09)
		VALUES (@seq, NULL, 'Highest total daily voucher amount ($): ' + CONVERT(VARCHAR(1000), @HighestTotalClaimAmount), '', '', '', '', '', '', '', '')
			
		SET @seq = @seq + 1

		INSERT INTO #09_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09)
		VALUES (@seq, NULL, 
			'Service date with highest total daily voucher amount: ' + @ServiceDateWithHighestTotalClaimAmount, 
			'', '', '', '', '', '', '', '')
			
		SET @seq = @seq + 1

		INSERT INTO #09_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09)
		VALUES (@seq, NULL, '', '', '', '', '', '', '', '', '')
			
		SET @seq = @seq + 1
		
		-- -----------------------------------------------------------
		-- Transaction of the highest daily no. of claims (All Date)
		-- -----------------------------------------------------------
		IF @HighestTotalClaimAmount IS NOT NULL
		BEGIN
			IF @HighestTotalClaimAmount > 0 
			BEGIN
				--DECLARE @TimeDiffThreshold AS INT
				--SET @TimeDiffThreshold = 300 -- 5 minutes

				--DECLARE @TotalTimeDiff INT
				--DECLARE @TotalNoOfTimeDiff INT

				DECLARE @DateHighestClaimAmt AS DATETIME

				DECLARE Result09_Cursor CURSOR FOR 
					SELECT Service_Receive_Dtm FROM @tblDateWithHighestClaimAmt

				OPEN Result09_Cursor

				FETCH NEXT FROM Result09_Cursor INTO @DateHighestClaimAmt

				WHILE @@FETCH_STATUS = 0
				BEGIN
					-- ------------------------------------------------------------
					-- Transaction of the highest daily no. of claims (Each Date)
					-- ------------------------------------------------------------
					-- Header
					INSERT INTO #09_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09)
					VALUES (@seq, NULL, 
							'Transaction ID',		'Transaction Time',	'Service Date',		'Amount Claimed ($)',
							'Means of Input',		'Practice No.',		'District of Practice', 'Time between 2 transactions', 'Less than 5 min')

					SET @seq = @seq + 1

					-- Content
					INSERT INTO #09_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09)
					SELECT 
						@seq
						,1
						,[Transaction_ID] = dbo.func_format_system_number(R1.[Transaction_ID])		
						,[Transaction_Dtm] = Format(R1.[Transaction_Dtm], 'yyyy/MM/dd HH:mm:ss')
						,[Service_Receive_Dtm] = CONVERT(VARCHAR(11), R1.[Service_Receive_Dtm], 113)
						,R1.[ClaimAmount]	
						,R1.[MeansOfInput]
						,R1.[PracticeID]
						,R1.[District]
						,[TimeDiff] = 
							CASE	
								WHEN R1.Transaction_Dtm IS NULL OR R2.Transaction_Dtm IS NULL THEN 'NA'
								WHEN R1.Transaction_Dtm IS NOT NULL OR R2.Transaction_Dtm IS NOT NULL 
									THEN CONVERT(VARCHAR(1000), DATEDIFF(SS,R2.Transaction_Dtm,R1.Transaction_Dtm) / 3600) + ':' +
										 RIGHT('0' + CONVERT(VARCHAR(1000), (DATEDIFF(SS,R2.Transaction_Dtm,R1.Transaction_Dtm) / 60) % 60),2) + ':' +
										 RIGHT('0' + CONVERT(VARCHAR(1000), DATEDIFF(SS,R2.Transaction_Dtm,R1.Transaction_Dtm) % 60),2)
								ELSE 'NA'
							END
						,[LessThan] = 
							CASE	
								WHEN R1.Transaction_Dtm IS NULL OR R2.Transaction_Dtm IS NULL THEN 'N'
								WHEN R1.Transaction_Dtm IS NOT NULL OR R2.Transaction_Dtm IS NOT NULL THEN 
									CASE
										WHEN DATEDIFF(SS,R2.Transaction_Dtm,R1.Transaction_Dtm) < @TimeDiffThreshold THEN 'Y'
										ELSE 'N'
									END
								ELSE 'N'
							END
					FROM
						(SELECT 
							[SeqNo] = ROW_NUMBER() OVER(ORDER BY VTB.Transaction_Dtm)
							,[Transaction_ID] = VTB.Transaction_ID
							,[Transaction_Dtm] = VTB.Transaction_Dtm
							,[Service_Receive_Dtm] = VTB.Service_Receive_Dtm
							,[ClaimAmount] = VTB.Claim_Amount 
							,[MeansOfInput] = 
								CASE 
									WHEN VTB.Create_By_SmartID IS NULL THEN 'Manual'
									WHEN VTB.Create_By_SmartID = 'N' THEN 'Manual'
									WHEN VTB.Create_By_SmartID = 'Y' THEN 'Smart IC'
								END
							,[PracticeID] = VTB.Practice_Display_Seq

							,[District] = CAST(D.district_name AS VARCHAR(MAX))
						FROM
							#VoucherTransactionBase VTB
								INNER JOIN district D
									ON VTB.District = D.district_code
						WHERE
							VTB.Service_Receive_Dtm = @DateHighestClaimAmt
						) R1	
						LEFT OUTER JOIN
						(SELECT 
							[SeqNo] = ROW_NUMBER() OVER(ORDER BY VTB.Transaction_Dtm)
							,[Transaction_ID] = VTB.Transaction_ID
							,[Transaction_Dtm] = VTB.Transaction_Dtm
							,[Service_Receive_Dtm] = VTB.Service_Receive_Dtm
							,[ClaimAmount] = VTB.Claim_Amount 
							,[MeansOfInput] = 
								CASE 
									WHEN VTB.Create_By_SmartID IS NULL THEN 'Manual'
									WHEN VTB.Create_By_SmartID = 'N' THEN 'Manual'
									WHEN VTB.Create_By_SmartID = 'Y' THEN 'Smart IC'
								END
							,[PracticeID] = VTB.Practice_Display_Seq

							,[District] = CAST(D.district_name AS VARCHAR(MAX))
						FROM
							#VoucherTransactionBase VTB
								INNER JOIN district D
									ON VTB.District = D.district_code
						WHERE
							VTB.Service_Receive_Dtm = @DateHighestClaimAmt
						) R2
							ON R1.[SeqNo] = R2.[SeqNo] + 1
						
						
					SET @seq = @seq + 1

					-- Total
					SELECT 
						@TotalTimeDiff = 
							SUM(CASE	
								WHEN R1.Transaction_Dtm IS NULL OR R2.Transaction_Dtm IS NULL THEN 0
								WHEN R1.Transaction_Dtm IS NOT NULL OR R2.Transaction_Dtm IS NOT NULL THEN  DATEDIFF(SS,R2.Transaction_Dtm,R1.Transaction_Dtm) 
								ELSE 0
							END),
						@TotalNoOfTimeDiff = 
							SUM(CASE	
								WHEN R1.Transaction_Dtm IS NULL OR R2.Transaction_Dtm IS NULL THEN 0
								WHEN R1.Transaction_Dtm IS NOT NULL OR R2.Transaction_Dtm IS NOT NULL THEN 1
								ELSE 0
							END)
					FROM
						(SELECT 
							[SeqNo] = ROW_NUMBER() OVER(ORDER BY VTB.Transaction_Dtm)
							,[Transaction_ID] = VTB.Transaction_ID
							,[Transaction_Dtm] = VTB.Transaction_Dtm
							,[Service_Receive_Dtm] = VTB.Service_Receive_Dtm
							,[ClaimAmount] = VTB.Claim_Amount 
							,[MeansOfInput] = 
								CASE 
									WHEN VTB.Create_By_SmartID IS NULL THEN 'Manual'
									WHEN VTB.Create_By_SmartID = 'N' THEN 'Manual'
									WHEN VTB.Create_By_SmartID = 'Y' THEN 'Smart IC'
								END
							,[PracticeID] = VTB.Practice_Display_Seq

							,[District] = CAST(D.district_name AS VARCHAR(MAX))
						FROM
							#VoucherTransactionBase VTB
								INNER JOIN district D
									ON VTB.District = D.district_code
						WHERE
							VTB.Service_Receive_Dtm = @DateHighestClaimAmt
						) R1	
						LEFT OUTER JOIN
						(SELECT 
							[SeqNo] = ROW_NUMBER() OVER(ORDER BY VTB.Transaction_Dtm)
							,[Transaction_ID] = VTB.Transaction_ID
							,[Transaction_Dtm] = VTB.Transaction_Dtm
							,[Service_Receive_Dtm] = VTB.Service_Receive_Dtm
							,[ClaimAmount] = VTB.Claim_Amount 
							,[MeansOfInput] = 
								CASE 
									WHEN VTB.Create_By_SmartID IS NULL THEN 'Manual'
									WHEN VTB.Create_By_SmartID = 'N' THEN 'Manual'
									WHEN VTB.Create_By_SmartID = 'Y' THEN 'Smart IC'
								END
							,[PracticeID] = VTB.Practice_Display_Seq

							,[District] = CAST(D.district_name AS VARCHAR(MAX))
						FROM
							#VoucherTransactionBase VTB
								INNER JOIN district D
									ON VTB.District = D.district_code
						WHERE
							VTB.Service_Receive_Dtm = @DateHighestClaimAmt
						) R2
							ON R1.[SeqNo] = R2.[SeqNo] + 1

					IF @TotalNoOfTimeDiff > 0
						BEGIN
							SET @AverageTimeDiff = @TotalTimeDiff / @TotalNoOfTimeDiff
						END
					ELSE
						BEGIN
							SET @AverageTimeDiff = 0
						END

					INSERT INTO #09_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09)
					VALUES (@seq, NULL, '', '', 'Total', 
							(SELECT SUM(Claim_Amount) FROM #VoucherTransactionBase WHERE Service_Receive_Dtm = @DateHighestClaimAmt), 
							'', '', 'Average time between transactions', 
							CONVERT(VARCHAR(1000), (((@AverageTimeDiff) / 60) /60) % 60) + ':' +
								RIGHT('0' + CONVERT(VARCHAR(1000), ((@AverageTimeDiff) / 60) % 60),2) + ':' +
								RIGHT('0' + CONVERT(VARCHAR(1000), (@AverageTimeDiff) % 60),2), 
							'')
			
					SET @seq = @seq + 1

					-- Spacing
					INSERT INTO #09_Result (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09)
					VALUES (@seq, NULL, '', '', '', '', '', '', '', '', '')
			
					SET @seq = @seq + 1

					FETCH NEXT FROM Result09_Cursor INTO @DateHighestClaimAmt
				END

				CLOSE Result09_Cursor
				DEALLOCATE Result09_Cursor
			END
		END

		-- ---------------------------------------------
		-- Return Result: 09-Highest amt
		-- ---------------------------------------------

		SELECT Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09 FROM #09_Result ORDER BY Seq, Seq2, Col02

	-- ---------------------------------------------
	-- To Excel Sheet (12): Remarks
	-- ---------------------------------------------
		
		DECLARE @CommonNoteStat1 varchar(1000)
		DECLARE @CommonNoteStat1a varchar(1000)
		DECLARE @CommonNoteStat1b varchar(1000)
		DECLARE @CommonNoteStat1c varchar(1000)
		--DECLARE @CommonNoteStat1d varchar(1000)

		DECLARE @CommonNoteStat2 varchar(1000)
		DECLARE @CommonNoteStat2a varchar(1000)
		DECLARE @CommonNoteStat2b varchar(1000)
		DECLARE @CommonNoteStat2c varchar(1000)

		DECLARE @CommonNoteStat3 varchar(1000)
		DECLARE @CommonNoteStat31 varchar(1000)
		DECLARE @CommonNoteStat31a varchar(1000)

		DECLARE @CommonNoteStat32 varchar(1000)
		DECLARE @CommonNoteStat32a varchar(1000)
		DECLARE @CommonNoteStat32b varchar(1000)
		DECLARE @CommonNoteStat32c varchar(1000)
		DECLARE @CommonNoteStat32d varchar(1000)

		DECLARE @CommonNoteStat33 varchar(1000)
		DECLARE @CommonNoteStat33a varchar(1000)
		DECLARE @CommonNoteStat33b varchar(1000)
		DECLARE @CommonNoteStat33c varchar(1000)

		DECLARE @CommonNoteStat38 varchar(1000)
		DECLARE @CommonNoteStat38a varchar(1000)

		DECLARE @CommonNoteStat39 varchar(1000)
		DECLARE @CommonNoteStat39a varchar(1000)

		-- ---------------------------------------------
		-- Prepare Data: Remarks
		-- ---------------------------------------------

		SET @CommonNoteStat1 = '1. Transactions:'
		SET @CommonNoteStat1a = '   a. HCVS claim transaction only'
		SET @CommonNoteStat1b = '   b. All claim transactions created under service providers (either created by back office users or service providers (or the delegated users))'
		SET @CommonNoteStat1c = '   c. Exclude those reimbursed transactions with invalidation status marked as Invalidated.'
		--SET @CommonNoteStat1d = '   d. Exclude voided/deleted transactions.'

		SET @CommonNoteStat2 = '2. Selection Criteria:'
		SET @CommonNoteStat2a = '   a. Type of Date : The type of transaction apply to all worksheet except worksheet ''02-Claim pattern (Mths)'''
		SET @CommonNoteStat2b = '   b. Transaction Status: The transaction status apply to all transactions retrieved in the report'
		SET @CommonNoteStat2c = '   c. All worksheets except ''02 - Claim (Mths)'' are with the same transaction pool based on the selection criteria with different breakdown and filtering'

		SET @CommonNoteStat3 = '3. Extraction Result'
		SET @CommonNoteStat31 = '   01 - Practice'
		SET @CommonNoteStat31a = '      a. Based on the selection criteria to extract the transactions in the selected month and provide summary by practice basis'

		SET @CommonNoteStat32 = '   02 - Claim (Mths)'
		SET @CommonNoteStat32a = '      a. Starting from the selected month to extract past 12 month transaction data (including the selected month)'
		SET @CommonNoteStat32b = '      b. Extract transaction by the selected transaction status '
		SET @CommonNoteStat32c = '      c.Transaction are extracted based on transaction date (no matter the selected selection criteria is service date or transaction date)'
		SET @CommonNoteStat32d = '         (if the selection criteria is service date, then the monthly summary may not be syn. with other worksheets)'

		SET @CommonNoteStat33 = '   03 - Claim (Days)'
		SET @CommonNoteStat33a = '      a. Extracted transactions are the same as that in worksheet ''01 - Practice'''
		SET @CommonNoteStat33b = '      b. Breakdown by service date of ectracted transaction'
		SET @CommonNoteStat33c = '      c. If the selection criteria is transaction date and there is back date claim to last month, the list will also show service date of last month'

		SET @CommonNoteStat38 = '   08 - Highest tx'
		SET @CommonNoteStat38a = '      a. List out all transactions with service date on the service date with the highest "No. of transactions"'

		SET @CommonNoteStat39 = '   09 - Highest amt'
		SET @CommonNoteStat39a = '      a. List out all transactions with service date on the service date with the highest voucher amount'

		-- ---------------------------------------------
		-- Insert Data: Remarks
		-- ---------------------------------------------
		SET @seq = 0

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '(A) Legend', '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '1. Profession Type Legend', '')
	
		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		SELECT @Seq, NULL, Service_Category_Code, Service_Category_Desc FROM Profession

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '', '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '(A) Common Note(s) for the report', '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat1, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat1a, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat1b, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat1c, '')

		SET @seq = @seq + 1

		--INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		--VALUES (@seq, NULL, @CommonNoteStat1d, '')

		--SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '', '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat2, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat2a, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat2b, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat2c, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '', '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat3, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat31, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat31a, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '', '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat32, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat32a, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat32b, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat32c, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat32d, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '', '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat33, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat33a, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat33b, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat33c, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '', '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat38, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat38a, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '', '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat39, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat39a, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '', '')

		SET @seq = @seq + 1

		-- ---------------------------------------------
		-- Return Result: Remarks
		-- ---------------------------------------------

		SELECT Col01, Col02 FROM #Remarks ORDER BY Seq, Seq2, Col01

	-- =============================================
	-- 6. RELEASE RESOURCE
	-- =============================================
	DROP TABLE #VoucherTransactionBase
	DROP TABLE #Content
	DROP TABLE #Criteria
	DROP TABLE #01_Result
	DROP TABLE #02_YEARMONTH
	DROP TABLE #02_Result
	DROP TABLE #03_Result
	DROP TABLE #04_Result
	DROP TABLE #05_Result
	DROP TABLE #06_Result
	DROP TABLE #07_Result
	DROP TABLE #08_Result
	DROP TABLE #09_Result
	DROP TABLE #Remarks
END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSU0010_Report_get] TO HCVU
GO
