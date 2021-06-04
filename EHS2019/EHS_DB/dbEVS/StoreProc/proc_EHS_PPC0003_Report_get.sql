IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_PPC0003_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_PPC0003_Report_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE21-004, CRE21-005
-- Modified by:		Koala CHENG
-- Modified date:	31 May 2021
-- Description:		Exclude COVID-19 from VSS and RVP
--					Randomize the ordering of transaction output of each SP
--					Add remarks item (B)8
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	30 August 2016
-- CR No.:			CRE16-002
-- Description:		Get [Category_Code] from table VoucherTransaction
-- =============================================
-- =============================================
-- Author:			Chris YIM
-- Create date:		31 May 2016
-- CR No.			CRE15-016
-- Description:		Randomly generate the valid claim transaction - PPC0003
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_PPC0003_Report_get]
	@request_time DATETIME,
	@SPList VARCHAR(5000),
	@PercentageOfSP INT,
	@ProfessionList VARCHAR(5000),
	@Scheme VARCHAR(5000),
	@Period_Type CHAR(1),
	@Period_From DATETIME,
	@Period_To DATETIME,
	@Transaction_Status VARCHAR(5000),
	@IncludeBackOfficeClaims BIT,
	@IncludeInvalidationClaims BIT
AS
BEGIN
	SET NOCOUNT ON;
	-- ===========================================================
	-- 1.0	DECLARATION
	-- ===========================================================
	DECLARE @IN_varSPList VARCHAR(5000)
	DECLARE @IN_intPercentageOfSPs INT
	DECLARE @IN_varProfessional VARCHAR(5000)
	DECLARE @IN_varSchemeCode VARCHAR(5000)
	DECLARE @IN_chrPeriod_Type CHAR(1)
	DECLARE @IN_dtmPeriod_From DATETIME
	DECLARE @IN_dtmPeriod_To DATETIME
	DECLARE @IN_varRecordStatus VARCHAR(5000)
	DECLARE @IN_chrIncludeBackOfficeClaim BIT
	DECLARE @IN_chrIncludeInvalidatedClaim BIT

	DECLARE @IN_dtmTransactionDateStart DATETIME
	DECLARE @IN_dtmTransactionDateEnd DATETIME
	DECLARE @IN_dtmServiceDateStart DATETIME
	DECLARE @IN_dtmServiceDateEnd DATETIME

	DECLARE @chrIsImportedSP CHAR(1)
	DECLARE @chrIsAllProfessional CHAR(1)
	DECLARE @chrIsAllRecordStatus CHAR(1)
	DECLARE @chrIsAllSchemeCode CHAR(1)

	DECLARE @current_dtm DATETIME
	DECLARE @delimiter VARCHAR(5)
	DECLARE @SPListDelimiter VARCHAR(5)
	DECLARE @SPListParmName VARCHAR(30)
	DECLARE @SPListGenerationID CHAR(12)

	DECLARE @SPListUpperLimit INT
	DECLARE @TotalSPUpperLimit INT
	DECLARE @RptTransactionPerPracticeUpperLimit INT
	DECLARE @RptTotalTransactionUpperLimit INT

	DECLARE @ExceedLimit CHAR(1)

	DECLARE @seq int
	DECLARE @ConstantPercentage VARCHAR(10)
	DECLARE @ConstantNumber VARCHAR(10)

	DECLARE @tblGenerationRule TABLE(
		CASE_ID INT IDENTITY(1,1) PRIMARY KEY,
		LOWER_LIMIT INT,
		UPPER_LIMIT INT,
		GENERATION_TYPE varchar(10),
		GENERATION_VALUE INT
	)

	DECLARE @tblSPList TABLE(
		SP_ID CHAR(8) PRIMARY KEY
	)

	DECLARE @tblProfessional TABLE(
		Service_Category_Code VARCHAR(5)
	)

	DECLARE @tblRecordStatus TABLE(
		Record_Status CHAR(1)
	)

	DECLARE @tblSchemeCode TABLE(
		Scheme_Code CHAR(10)
	)

	CREATE TABLE #ServiceProviderBase (
		SP_ID CHAR(8) NOT NULL
	)

	--CREATE NONCLUSTERED INDEX IX_ServiceProviderBase_SP_ID
	--	ON #ServiceProviderBase (SP_ID); 

	CREATE TABLE #VoucherTransactionBase (
		SP_ID CHAR(8), 
		Transaction_ID CHAR(20),
		Practice_Display_Seq SMALLINT,
	)

	CREATE NONCLUSTERED INDEX IX_VoucherTransactionBase_SP_ID
		ON #VoucherTransactionBase (SP_ID); 

	CREATE TABLE #01_NoOfTransaction_ResultTable    
	(         
		SP_ID CHAR(8) NOT NULL,    
		Practice_Display_Seq SMALLINT NOT NULL,    
		MatchedTransaction INT NOT NULL,    
		GeneratedTransaction INT NULL,    
		ExceedLimitPerSP CHAR(1) DEFAULT 'N'
	)  

	CREATE NONCLUSTERED INDEX IX_NoOfTransaction_ResultTable_SP_ID
		ON #01_NoOfTransaction_ResultTable (SP_ID); 

	CREATE TABLE #02_PostPaymentCheck_ResultTable    
	(    
		DisplaySeq INT IDENTITY(1,1),     
		Col01 VARCHAR(255) DEFAULT '',    
		Col02 VARCHAR(255) DEFAULT '',    
		Col03 VARCHAR(255) DEFAULT '',    
		Col04 VARCHAR(255) DEFAULT '',    
		Col05 VARCHAR(255) DEFAULT '',    
		Col06 VARCHAR(255) DEFAULT '',    
		Col07 VARCHAR(255) DEFAULT '',    
		Col08 VARCHAR(255) DEFAULT '',    
		Col09 VARCHAR(255) DEFAULT '',    
		Col10 VARCHAR(255) DEFAULT ''
	)  

	CREATE NONCLUSTERED INDEX IX_ResultTable_Col01
		ON #02_PostPaymentCheck_ResultTable (Col01); 

	CREATE NONCLUSTERED INDEX IX_ResultTable_Col02
		ON #02_PostPaymentCheck_ResultTable (Col02); 

	CREATE TABLE #Criteria (
		Seq		int,
		Seq2	int,
		Col01	varchar(1000),
		Col02	varchar(1000)
	)

	CREATE TABLE #Remarks (
		Seq		int,
		Seq2	int,
		Col01	varchar(1000),
		Col02	varchar(1000)
	)

	-- ===========================================================
	-- 1.1	Initialization
	-- ===========================================================
	SET @current_dtm = GETDATE()
	SET @delimiter = ','
	SET @SPListDelimiter = ':'
	SET @SPListParmName = NULL
	SET @SPListGenerationID = NULL
	SET @IN_intPercentageOfSPs = NULL

	SET @ExceedLimit = NULL

	SET @ConstantPercentage = 'PERCENT'
	SET @ConstantNumber = 'NUMBER'

	INSERT @tblGenerationRule(LOWER_LIMIT,UPPER_LIMIT,GENERATION_TYPE,GENERATION_VALUE)
		VALUES(NULL,14,@ConstantPercentage,100)

	INSERT @tblGenerationRule(LOWER_LIMIT,UPPER_LIMIT,GENERATION_TYPE,GENERATION_VALUE)
		VALUES(14,300,@ConstantNumber,15)

	INSERT @tblGenerationRule(LOWER_LIMIT,UPPER_LIMIT,GENERATION_TYPE,GENERATION_VALUE)
		VALUES(300,999,@ConstantPercentage,5)

	INSERT @tblGenerationRule(LOWER_LIMIT,UPPER_LIMIT,GENERATION_TYPE,GENERATION_VALUE)
		VALUES(999,NULL,@ConstantPercentage,10)

	SET @SPListUpperLimit = (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'PostPaymentCheck0003SPListUpperLimit')
	SET @TotalSPUpperLimit = (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'PostPaymentCheck0003TotalSPUpperLimit')
	SET @RptTransactionPerPracticeUpperLimit = (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'PostPaymentCheck0003RptTxPerPracticeUpperLimit')
	SET @RptTotalTransactionUpperLimit = (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'PostPaymentCheck0003RptTotalTxUpperLimit')

	-- ===========================================================
	-- 1.2	COPY PARAMETERS
	-- ===========================================================
	SET @IN_intPercentageOfSPs = @PercentageOfSP

	SET @IN_chrPeriod_Type = @Period_Type
	SET @IN_dtmPeriod_From = @Period_From
	SET @IN_dtmPeriod_To = @Period_To

	SET @IN_chrIncludeBackOfficeClaim = @IncludeBackOfficeClaims
	SET @IN_chrIncludeInvalidatedClaim = @IncludeInvalidationClaims

	SET @IN_dtmTransactionDateStart = NULL
	SET @IN_dtmTransactionDateEnd = NULL
	SET @IN_dtmServiceDateStart = NULL
	SET @IN_dtmServiceDateEnd = NULL

	SET @IN_varSPList = NULL
	SET @IN_varProfessional = NULL
	SET @IN_varSchemeCode = NULL
	SET @IN_varRecordStatus = NULL

	IF @SPList IS NOT NULL
	BEGIN
		IF @SPList = ''
			SET @IN_varSPList = NULL
		ELSE 
			SET @IN_varSPList = @SPList
	END

	IF @ProfessionList IS NOT NULL
	BEGIN
		IF @ProfessionList = ''
			SET @IN_varProfessional = NULL
		ELSE 
			SET @IN_varProfessional = @ProfessionList
	END

	IF @Scheme IS NOT NULL
	BEGIN
		IF @Scheme = ''
			SET @IN_varSchemeCode = NULL
		ELSE 
			SET @IN_varSchemeCode = @Scheme
	END

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
			SET @IN_dtmServiceDateEnd = @IN_dtmPeriod_To
		END

	-- Selected 'Transaction Date'
	ELSE IF @IN_chrPeriod_Type = 'T'
		BEGIN
			SET @IN_dtmTransactionDateStart = @IN_dtmPeriod_From
			SET @IN_dtmTransactionDateEnd = DATEADD(DAY, 1, @IN_dtmPeriod_To)
		END

	-- ===========================================================
	-- 2.1 PREPARE TABLE TO STORE IMPORTED SERVICE PROVIDER
	-- ===========================================================
	SET @chrIsImportedSP = 'N'

	IF @IN_varSPList IS NOT NULL
	BEGIN
		IF CHARINDEX(@SPListDelimiter,@IN_varSPList) > 0
		BEGIN
			SET @SPListGenerationID = SUBSTRING(RIGHT(@IN_varSPList,LEN(@IN_varSPList) - CHARINDEX(@SPListDelimiter,@IN_varSPList)), 1, CHARINDEX(@SPListDelimiter,RIGHT(@IN_varSPList,LEN(@IN_varSPList) - CHARINDEX(@SPListDelimiter,@IN_varSPList)))-1)
			SET @SPListParmName = SUBSTRING(RIGHT(@IN_varSPList,LEN(@IN_varSPList) - CHARINDEX(@SPListDelimiter,@IN_varSPList)), CHARINDEX(@SPListDelimiter,RIGHT(@IN_varSPList,LEN(@IN_varSPList) - CHARINDEX(@SPListDelimiter,@IN_varSPList)))+1 ,LEN(RIGHT(@IN_varSPList,LEN(@IN_varSPList) - CHARINDEX(@SPListDelimiter,@IN_varSPList))))

			INSERT @tblSPList(SP_ID)
					SELECT Parm_Value FROM FileGenerationQueueAdditionalParameter
						WHERE Generation_ID = @SPListGenerationID AND Parm_Name = @SPListParmName
			
			IF (SELECT COUNT(1) FROM @tblSPList) > 0 	
				SET @chrIsImportedSP = 'Y'
		END
	END

	-- ===========================================================
	-- 2.2 PREPARE TABLE TO STORE INPUTTED PROFESSIONAL
	-- ===========================================================

	IF @IN_varProfessional IS NOT NULL
	BEGIN

		INSERT @tblProfessional
				SELECT * FROM func_split_string(@IN_varProfessional, @delimiter)

		SET @chrIsAllProfessional = 'N'
	END
	ELSE 
		SET @chrIsAllProfessional = 'Y'

	-- ===========================================================
	-- 2.3 PREPARE TABLE TO STORE INPUTTED RECORD STATUS
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
	-- 2.4 PREPARE TABLE TO STORE INPUTTED SCHEME CODE
	-- ===========================================================
	IF @IN_varSchemeCode IS NOT NULL
	BEGIN

		INSERT @tblSchemeCode
				SELECT * FROM func_split_string(@IN_varSchemeCode, @delimiter)

		SET @chrIsAllSchemeCode = 'N'
	END
	ELSE 
		SET @chrIsAllSchemeCode = 'Y'

	-- =========================================================================
	-- 3.1 PREPARE TABLE TO RETRIEVE POSSIBLE SERVICE PROVIDER WITH TRANSACTIONS
	-- =========================================================================
	INSERT
		#VoucherTransactionBase
		(SP_ID,
		Transaction_ID,
		Practice_Display_Seq)
	SELECT
		VT.SP_ID,
		VT.Transaction_ID,
		VT.Practice_Display_Seq
	FROM 
		VoucherTransaction VT
			LEFT JOIN TransactionDetail TD
				ON VT.Transaction_ID = TD.Transaction_ID 
					AND TD.Scheme_Code IN ('VSS','RVP')
					AND TD.Subsidize_Item_Code = 'C19'
			LEFT JOIN @tblProfessional tblProf
				ON LTRIM(RTRIM(VT.Service_Type)) = tblProf.Service_Category_Code
			LEFT JOIN @tblRecordStatus tblRS
				ON LTRIM(RTRIM(VT.Record_Status)) = tblRS.Record_Status
			LEFT JOIN @tblSchemeCode tblSC
				ON LTRIM(RTRIM(VT.Scheme_Code)) = LTRIM(RTRIM(tblSC.Scheme_Code))
	WHERE 
		((@IN_dtmTransactionDateStart IS NULL AND @IN_dtmTransactionDateEnd IS NULL)
			OR (Transaction_Dtm >= @IN_dtmTransactionDateStart AND Transaction_Dtm < @IN_dtmTransactionDateEnd))
		AND ((@IN_dtmServiceDateStart IS NULL AND @IN_dtmServiceDateEnd IS NULL)
			OR (Service_Receive_Dtm >= @IN_dtmServiceDateStart AND Service_Receive_Dtm <= @IN_dtmServiceDateEnd))
		AND (@IN_chrIncludeBackOfficeClaim IS NULL
			OR @IN_chrIncludeBackOfficeClaim = 1
			OR (@IN_chrIncludeBackOfficeClaim = 0 AND (VT.Manual_Reimburse IS NULL OR VT.Manual_Reimburse <> 'Y')))
		AND (@IN_chrIncludeInvalidatedClaim IS NULL 
			OR @IN_chrIncludeInvalidatedClaim = 1
			OR (@IN_chrIncludeInvalidatedClaim = 0 AND (Invalidation IS NULL OR Invalidation NOT IN ('I','P'))))
		AND (@chrIsAllProfessional = 'Y' OR tblProf.Service_Category_Code IS NOT NULL)
		AND (@chrIsAllRecordStatus = 'Y' OR tblRS.Record_Status IS NOT NULL)
		AND (@chrIsAllSchemeCode = 'Y' OR tblSC.Scheme_Code IS NOT NULL)
		AND TD.Transaction_ID IS NULL

	--===========================================================
	--3.2. RANDOM SELECTION OF SERVICE PROVIDER
	--===========================================================

	IF @IN_intPercentageOfSPs IS NOT NULL
	BEGIN
		--------------------------
		-- Declaration
		--------------------------
		DECLARE @intNumRowSP INT
		DECLARE @floatPercentageSP FLOAT
		DECLARE @intResultSetSP INT

		--------------------------
		-- Setting
		--------------------------

		SET @floatPercentageSP = @IN_intPercentageOfSPs / 100.0
		SET @intResultSetSP = (SELECT COUNT(SP_ID) FROM 
									(SELECT DISTINCT SP_ID FROM #VoucherTransactionBase VTB
										WHERE NOT EXISTS (SELECT SP_ID FROM @tblSPList WHERE SP_ID = VTB.SP_ID)) AS SP)
		--SET @intNumRowSP = ROUND((@intResultSetSP + 0.0) * @floatPercentageSP, 0)

		------------------------------------------
		-- Randomly Selection of Service Provider
		------------------------------------------
		IF @floatPercentageSP IS NOT NULL AND @intResultSetSP IS NOT NULL
		BEGIN
			--------------------------------------------
			-- Insert Imported Service Provider
			--------------------------------------------
			IF @chrIsImportedSP = 'Y'
				INSERT #ServiceProviderBase(SP_ID)
					SELECT SP_ID FROM @tblSPList

			--------------------------------------------
			-- Insert Randomly Selected Service Provider
			--------------------------------------------
			IF @intResultSetSP > 0
			BEGIN
				IF (@intResultSetSP + 0.0) * @floatPercentageSP < 1
				BEGIN
					SET @intNumRowSP = 1
				END
				ELSE
				BEGIN
					SET @intNumRowSP = ROUND((@intResultSetSP + 0.0) * @floatPercentageSP, 0)
				END

				IF ((SELECT COUNT(1) FROM @tblSPList) + @intNumRowSP) > @TotalSPUpperLimit
				BEGIN
					SET @intNumRowSP = @TotalSPUpperLimit - (SELECT COUNT(1) FROM @tblSPList)
				END

				INSERT #ServiceProviderBase(SP_ID)
					SELECT TOP (@intNumRowSP) SP_ID
						FROM (SELECT DISTINCT SP_ID FROM #VoucherTransactionBase VTB
								WHERE NOT EXISTS (SELECT SP_ID FROM @tblSPList WHERE SP_ID = VTB.SP_ID)) AS SP
					ORDER BY NEWID()	
			END
		END
	END

	-- ===========================================================
	-- 4. RETRIEVE TRANSACTIONS OF EACH SERVICE PROVIDER
	-- ===========================================================

		-- ---------------------------------------------
		-- Prepare Data for output: Criteria
		-- ---------------------------------------------
		-- ---------------------------------------------
		-- Declaration
		-- ---------------------------------------------
		DECLARE @varCriteriaNoOfImportedSP varchar(50)
		DECLARE @varCriteriaPercentageOfSP varchar(10)
		DECLARE @varProfessionList varchar(5000)
		DECLARE @varScheme varchar(5000)
		DECLARE @chrPeriod_Type varchar(50)
		DECLARE @chrPeriod_Format varchar(50)
		DECLARE @varPeriod varchar(50)
		DECLARE @varTransactionStatus varchar(5000)
		DECLARE @chrIncludeBackOfficeClaims varchar(3)
		DECLARE @chrIncludeInvalidationClaims varchar(3)



		-- ---------------------------------------------
		-- Initialization
		-- ---------------------------------------------

		Set @varCriteriaNoOfImportedSP = CONVERT(VARCHAR(10),(SELECT COUNT(1) FROM @tblSPList)) + ' imported'

		Set @varCriteriaPercentageOfSP = CONVERT(VARCHAR(3), @PercentageOfSP) + '%'

		IF @chrIsAllProfessional = 'Y'
			SET @varProfessionList = 'Any'
		ELSE
			BEGIN
				SET @varProfessionList = 
					(SELECT 
						[Service_Category_Desc] + ' (' + [Service_Category_Code] + ')' + ', ' 
					FROM Profession WITH (NOLOCK)
					WHERE [Service_Category_Code] IN 
						(SELECT [Service_Category_Code] FROM @tblProfessional)
					for xml path(''))

				SET @varProfessionList = SUBSTRING(@varProfessionList, 1, LEN(@varProfessionList)- 1) 
			END

		IF @chrIsAllSchemeCode = 'Y'
			SET @varScheme = 'Any'
		ELSE
			BEGIN
				SET @varScheme = 
					(SELECT LTRIM(RTRIM(Scheme_Code)) + ', ' FROM @tblSchemeCode	for xml path(''))
				SET @varScheme = SUBSTRING(@varScheme, 1, LEN(@varScheme)- 1) 
			END

		IF @Period_Type = 'S'
			SET @chrPeriod_Type = 'Service Date'
		ELSE IF @Period_Type = 'T'
			SET @chrPeriod_Type = 'Transaction Date'

		SET @chrPeriod_Format = 'Date'
		SET @varPeriod = FORMAT(@Period_From, 'dd-MM-yyyy') + ' to ' + FORMAT(@Period_To, 'dd-MM-yyyy')

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

		IF @IncludeBackOfficeClaims = 1
			SET @chrIncludeBackOfficeClaims = 'Yes'
		ELSE
			SET @chrIncludeBackOfficeClaims = 'No'

		IF @IncludeInvalidationClaims = 1
			SET @chrIncludeInvalidationClaims = 'Yes'
		ELSE
			SET @chrIncludeInvalidationClaims = 'No'

	-- ===========================================================
	-- 4.2. CHECK EXCEED LIMIT - Total Transaction
	-- ===========================================================

	--------------------------
	-- Initialization
	--------------------------
	INSERT #01_NoOfTransaction_ResultTable(SP_ID,Practice_Display_Seq,MatchedTransaction)
		SELECT 
			SP_ID,
			Practice_Display_Seq,
			COUNT(1) AS [Total_Transaction]
		FROM #VoucherTransactionBase VTB
		WHERE SP_ID IN (SELECT SP_ID FROM #ServiceProviderBase)
		GROUP BY
			SP_ID, Practice_Display_Seq
		ORDER BY SP_ID, Practice_Display_Seq

	---------------------------------------------------------------------------
	-- Update no. of generated transaction for each practice
	---------------------------------------------------------------------------
	UPDATE #01_NoOfTransaction_ResultTable
	SET GeneratedTransaction =
		(SELECT 
			(CASE 
				WHEN GENERATION_TYPE = @ConstantPercentage THEN ROUND((MatchedTransaction + 0.0) * GENERATION_VALUE / 100.0 , 0)
				ELSE GENERATION_VALUE 
			END) 
			FROM @tblGenerationRule
			WHERE (LOWER_LIMIT IS NULL OR LOWER_LIMIT < MatchedTransaction)
				AND (UPPER_LIMIT IS NULL OR UPPER_LIMIT >= MatchedTransaction))

	UPDATE #01_NoOfTransaction_ResultTable
	SET 
		ExceedLimitPerSP = 'Y',
		GeneratedTransaction = RT_UPDATE.GeneratedTransaction
	FROM
		(SELECT
			SP_ID,
			Practice_Display_Seq,
			[GeneratedTransaction] =  
				CASE
					WHEN BALANCE < 0 THEN 0
					ELSE BALANCE
				END
		FROM
			(SELECT
				SP_ID,
				Practice_Display_Seq,
				@RptTransactionPerPracticeUpperLimit - SUM(GeneratedTransaction) OVER(PARTITION BY SP_ID ORDER BY SP_ID, Practice_Display_Seq ROWS BETWEEN unbounded preceding AND current row) AS NETVALUE,
				GeneratedTransaction + (@RptTransactionPerPracticeUpperLimit - SUM(GeneratedTransaction) OVER(PARTITION BY SP_ID ORDER BY SP_ID, Practice_Display_Seq ROWS BETWEEN unbounded preceding AND current row)) AS BALANCE
			FROM 
				#01_NoOfTransaction_ResultTable
			) RT_CALCULATE
		WHERE 
			NETVALUE < 0
		) RT_UPDATE
	WHERE 
		#01_NoOfTransaction_ResultTable.SP_ID = RT_UPDATE.SP_ID AND #01_NoOfTransaction_ResultTable.Practice_Display_Seq = RT_UPDATE.Practice_Display_Seq

	---------------------------------------------------------------------------
	-- Check total no. of generated transaction
	---------------------------------------------------------------------------	
	IF (SELECT SUM(GeneratedTransaction) FROM #01_NoOfTransaction_ResultTable) > @RptTotalTransactionUpperLimit
		SET @ExceedLimit = 'Y'
	ELSE
		SET @ExceedLimit = 'N'

	-- ===========================================================
	-- 4.3 IF EXCEEDS, NO RANDOM SELECTION OF TRANSACTIONS
	-- ===========================================================

	IF @ExceedLimit = 'Y'
	BEGIN
		SELECT 
			'ExceedLimit' = @ExceedLimit,
			'PercentageOfSP' = @varCriteriaPercentageOfSP,
			'SPList' = @varCriteriaNoOfImportedSP,
			'ProfessionList' = @varProfessionList,
			'Scheme' = @varScheme,
			'Period_Type' = @chrPeriod_Type,
			'Period' = @varPeriod,
			'Transaction_Status' = @varTransactionStatus,
			'IncludeBackOfficeClaims' = @chrIncludeBackOfficeClaims,
			'IncludeInvalidationClaims' = @chrIncludeInvalidationClaims
	END
	ELSE IF @ExceedLimit = 'N'
	BEGIN
	-- ===========================================================
	-- 4.4 IF NOT EXCEED, RANDOM SELECTION OF TRANSACTIONS
	-- ===========================================================		
		--------------------------
		-- Declaration
		--------------------------
		DECLARE @intNumRow INT
		DECLARE @floatPercentage FLOAT
		DECLARE @intResultSet INT
		--------------------------
		-- Setting
		--------------------------
		--SET @floatPercentage = @IN_intPercentOfTransactions / 100.0

		------------------------------------------
		-- Cursor For Loop Each Service Provider
		------------------------------------------
		SET NOCOUNT ON
		DECLARE @RT02_SP_ID CHAR(8)
		DECLARE @RT02_Practice_Display_Seq SMALLINT
		DECLARE @RT02_GeneratedTransaction INT

		DECLARE Transaction_Cursor CURSOR FOR 
			SELECT SP_ID, Practice_Display_Seq, GeneratedTransaction FROM #01_NoOfTransaction_ResultTable

		OPEN Transaction_Cursor

		FETCH NEXT FROM Transaction_Cursor INTO @RT02_SP_ID, @RT02_Practice_Display_Seq, @RT02_GeneratedTransaction

		WHILE @@FETCH_STATUS = 0
		BEGIN

			------------------------------------------
			-- Randomly Selection of Transaction
			------------------------------------------
			INSERT #02_PostPaymentCheck_ResultTable(Col01,Col02)
				SELECT TOP (@RT02_GeneratedTransaction) 
					@RT02_SP_ID, 
					VTB.Transaction_ID 
				FROM (SELECT Transaction_ID 
						FROM #VoucherTransactionBase 
						WHERE SP_ID = @RT02_SP_ID AND Practice_Display_Seq = @RT02_Practice_Display_Seq) AS VTB
				ORDER BY NEWID()

		FETCH NEXT FROM Transaction_Cursor INTO @RT02_SP_ID, @RT02_Practice_Display_Seq, @RT02_GeneratedTransaction
		END

		CLOSE Transaction_Cursor
		DEALLOCATE Transaction_Cursor

	-- =============================================
	-- 5. RETURN RESULTS
	-- =============================================
	-- ---------------------------------------------
	-- To Control: ExceedLimit
	-- ---------------------------------------------
		SELECT 'ExceedLimit' = @ExceedLimit

	-- ---------------------------------------------
	-- To Excel Sheet (01): Content
	-- ---------------------------------------------

		SELECT 'Report Generation Time: ' + CONVERT(varchar(10), @current_dtm, 111) + ' ' + CONVERT(varchar(5), @current_dtm, 114)

	-- ---------------------------------------------
	-- To Excel Sheet (02): Criteria
	-- ---------------------------------------------

		---- ---------------------------------------------
		---- Insert Data: Criteria
		---- ---------------------------------------------

		SET @seq = 0

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Criteria', '')

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Service Provider Supplementary List', @varCriteriaNoOfImportedSP)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Percentage of Service Provider after Excluded Supplementary List', @varCriteriaPercentageOfSP)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Health Profession', @varProfessionList)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Scheme', @varScheme)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Type of Date', @chrPeriod_Type)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Date', @varPeriod)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Transaction Status', @varTransactionStatus)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Include Back Office Claim', @chrIncludeBackOfficeClaims)

		SET @seq = @seq + 1

		INSERT INTO #Criteria (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, 'Include Pending Invalidation and Invalidated Claims', @chrIncludeInvalidationClaims)

		SET @seq = @seq + 1

		---- ---------------------------------------------
		---- Return Result: Criteria
		---- ---------------------------------------------

		SELECT Col01, Col02 FROM #Criteria ORDER BY Seq, Seq2, Col01

	-- ---------------------------------------------
	-- To Excel Sheet (03): Imported Supplementary List
	-- ---------------------------------------------
		
		EXEC [proc_SymmetricKey_open]

		SELECT 
			'SPID' = SPList.SP_ID,
			'SP Name (In English)' = CONVERT(varchar(MAX), DecryptByKey(SP.Encrypt_Field2)),
			'SP Name (In Chinese)' = CONVERT(NVARCHAR(MAX), DecryptByKey(SP.Encrypt_Field3)),
			'Generated Transactions in the Report' = 
				CASE
					WHEN VTB.SP_ID IS NULL THEN 'N'
					ELSE 'Y'
				END
		FROM 
			@tblSPList SPList
				INNER JOIN ServiceProvider SP
					ON SPList.SP_ID = SP.SP_ID
				LEFT JOIN (SELECT DISTINCT SP_ID FROM #VoucherTransactionBase) VTB
					ON SPList.SP_ID = VTB.SP_ID

		EXEC [proc_SymmetricKey_close]
	-- ---------------------------------------------
	-- To Excel Sheet (04): 01-Number of Transaction
	-- ---------------------------------------------

		SELECT SP_ID, Practice_Display_Seq, MatchedTransaction, GeneratedTransaction, ExceedLimitPerSP FROM #01_NoOfTransaction_ResultTable ORDER BY SP_ID, Practice_Display_Seq

	-- -------------------------------------------------
	-- To Excel Sheet (05): 02-Post Payment Check Report
	-- -------------------------------------------------

		EXEC [proc_SymmetricKey_open]

		SELECT
			'SPID' = RT.Col01,
			'Practice No.' = VT.Practice_Display_Seq,
			'Transaction ID' = [dbo].func_format_system_number(RT.Col02),
			'eHealth (Subsidies) Account Name (In English)' = 
				CASE
					WHEN PInfo.Encrypt_Field2 IS NULL THEN CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field2))
					ELSE CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field2))
				END,
			'eHealth (Subsidies) Account Name (In Chinese)' = 
				CASE
					WHEN PInfo.Encrypt_Field3 IS NULL THEN CONVERT(NVARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field3))
					ELSE CONVERT(NVARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field3))
				END,
			'Document Type' = VT.Doc_Code,
			'Identity Document No.' = 
				CASE
					WHEN PInfo.Encrypt_Field1 IS NULL 
					THEN 
						[dbo].func_mask_doc_id(VT.Doc_Code, CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field1)), CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field11)))
					ELSE
						[dbo].func_mask_doc_id(VT.Doc_Code, CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field1)), CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field11)))
				END,
			'Transaction Time' = CONVERT(VARCHAR(19),VT.Transaction_Dtm,121),

			'Service Date' = CONVERT(VARCHAR(10),VT.Service_Receive_Dtm,121),
			'Scheme' = VT.Scheme_Code,
			'Subsidies' = 
				CASE 
					WHEN SI.Subsidize_Type <> 'VACCINE' THEN 'N/A'
					ELSE SGC.Display_Code_For_Claim
				END,
			'Dose' =
				CASE 
					WHEN SI.Subsidize_Type <> 'VACCINE' THEN 'N/A'
					ELSE 
						CASE 
							WHEN SIDS.Available_Item_Code = 'ONLYDOSE' THEN 'Only Dose'
							ELSE SIDS.Available_Item_Desc
						END
				END,
			'Category' =
				CASE 
					WHEN CC.Category_Name IS NULL THEN 'N/A'
					ELSE CC.Category_Name
				END,
			'Transaction Status' = (SELECT Status_Description FROM StatusData WHERE Enum_Class = 'ClaimTransStatus' AND Status_Value = VT.Record_Status)
		FROM 
			#02_PostPaymentCheck_ResultTable RT
				INNER JOIN VoucherTransaction VT
					ON RT.Col02 = VT.Transaction_ID
				INNER JOIN TransactionDetail TD
					ON VT.Transaction_ID = TD.Transaction_ID
				LEFT JOIN PersonalInformation PInfo
					ON VT.Voucher_Acc_ID = PInfo.Voucher_Acc_ID AND VT.Doc_Code = PInfo.Doc_Code
				LEFT JOIN TempPersonalInformation TPInfo
					ON VT.Temp_Voucher_Acc_ID = TPInfo.Voucher_Acc_ID AND VT.Doc_Code = TPInfo.Doc_Code
				INNER JOIN SubsidizeGroupClaim SGC
					ON TD.Scheme_Code = SGC.Scheme_Code AND TD.Subsidize_Code = SGC.Subsidize_Code AND TD.Scheme_Seq = SGC.Scheme_Seq
				INNER JOIN SubsidizeItem SI
					ON TD.Subsidize_Item_Code = SI.Subsidize_Item_Code
				INNER JOIN SubsidizeItemDetails SIDS
					ON TD.Subsidize_Item_Code = SIDS.Subsidize_Item_Code AND TD.Available_Item_Code = SIDS.Available_Item_Code
				--LEFT JOIN TransactionAdditionalField TAF
				--	ON VT.Transaction_ID = TAF.Transaction_ID AND AdditionalFieldID = 'CategoryCode'
				LEFT JOIN ClaimCategory CC
					ON VT.Category_Code = CC.Category_Code

		ORDER BY NEWID()
		--ORDER BY RT.Col01, VT.Practice_Display_Seq,RT.Col02

		EXEC [proc_SymmetricKey_close]

	-- ---------------------------------------------
	-- To Excel Sheet (06): Remarks
	-- ---------------------------------------------
		
		-- Create Temp Table

		DECLARE @SchemeBackOffice TABLE (
			Scheme_Code		char(10),
			Display_Code	char(25),
			Display_Seq		smallint,
			Scheme_Desc		varchar(100)
		)

		DECLARE @CommonNoteStat01 varchar(1000)
		DECLARE @CommonNoteStat02 varchar(1000)
		DECLARE @CommonNoteStat03 varchar(1000)
		DECLARE @CommonNoteStat04 varchar(1000)
		DECLARE @CommonNoteStat05 varchar(1000)
		DECLARE @CommonNoteStat06 varchar(1000)
		DECLARE @CommonNoteStat07 varchar(1000)
		DECLARE @CommonNoteStat08 varchar(1000)

		-- ---------------------------------------------
		-- Prepare Data: Remarks
		-- ---------------------------------------------

		INSERT INTO @SchemeBackOffice (Scheme_Code, Display_Code, Display_Seq, Scheme_Desc)
		SELECT
			Scheme_Code,
			MAX(Display_Code),
			MAX(Display_Seq),
			MAX(Scheme_Desc)
		FROM SchemeBackOffice WITH (NOLOCK)
		WHERE Effective_Dtm <= @request_time AND Record_Status = 'A'
		GROUP BY Scheme_Code

		SET @CommonNoteStat01 = '1. All service providers are counted including delisted status and those in the exception list.'
		SET @CommonNoteStat02 = '2. Maximum of ' + CONVERT(VARCHAR(10),@RptTransactionPerPracticeUpperLimit) + ' transactions are retrieved randomly for each service provider.'
		SET @CommonNoteStat03 = '3. For practice with less than ' + (SELECT CONVERT(VARCHAR(10),UPPER_LIMIT + 1) FROM @tblGenerationRule WHERE CASE_ID = 1) + ' transactions, all transaction will be retrieved.'
		SET @CommonNoteStat04 = '4. For practice with number of transactions between ' + (SELECT CONVERT(VARCHAR(10),LOWER_LIMIT + 1) + ' and ' + CONVERT(VARCHAR(10),UPPER_LIMIT) + ', ' + CONVERT(VARCHAR(10),GENERATION_VALUE) FROM @tblGenerationRule WHERE CASE_ID = 2) + ' transaction will be retrieved.'
		SET @CommonNoteStat05 = '5. For practice with number of transactions between ' + (SELECT CONVERT(VARCHAR(10),LOWER_LIMIT + 1) + ' and ' + CONVERT(VARCHAR(10),UPPER_LIMIT) + ', ' + CONVERT(VARCHAR(10),GENERATION_VALUE) FROM @tblGenerationRule WHERE CASE_ID = 3) + '% of total transaction will be retrieved.'
		SET @CommonNoteStat06 = '6. For practice more than ' + (SELECT CONVERT(VARCHAR(10),LOWER_LIMIT) + ' transactions, ' + CONVERT(VARCHAR(10),GENERATION_VALUE) FROM @tblGenerationRule WHERE CASE_ID = 4) + '% of total transaction will be retrieved.'
		SET @CommonNoteStat07 = '7. Maximum of ' + CONVERT(VARCHAR(10),@TotalSPUpperLimit) + ' service providers will be handled.'
		SET @CommonNoteStat08 = '8. All COVID-19 claims under VSS and RVP are excluded.'
		---- ---------------------------------------------
		---- Insert Data: Remarks
		---- ---------------------------------------------

		SET @seq = 0

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '(A) Legend', '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '1. Identity Document Type', '')
	
		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		SELECT @Seq, NULL, Doc_Display_Code, Doc_Name FROM DocType ORDER BY Display_Seq

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '', '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '2. Scheme Name', '')
	
		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		SELECT @seq, NULL, Display_Code, Scheme_Desc FROM @SchemeBackOffice

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '', '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '3. Subsidy', '')
	
		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		SELECT DISTINCT @Seq, NULL, Display_Code_For_Claim, Legend_Desc_For_Claim
		FROM SubsidizeGroupClaim a
			INNER JOIN Subsidize b ON a.Subsidize_Code = b.Subsidize_Code
			INNER JOIN Subsidizeitem c ON b.subsidize_item_Code = c.Subsidize_Item_Code AND c.Subsidize_Type = 'VACCINE'
		ORDER BY Display_Code_For_Claim

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '', '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, '(B) Common Note(s) for the report', '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat01, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat02, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat03, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat04, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat05, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat06, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat07, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		VALUES (@seq, NULL, @CommonNoteStat08, '')

		SET @seq = @seq + 1

		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
		SELECT @Seq, NULL, '9. All are accumulative data unless specified as below:', ''

		SET @seq = @seq + 1
		INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			SELECT @Seq, NULL, RTRIM(a.Display_Code_For_Claim) + ' : ' + REPLACE(CONVERT(VARCHAR(12),CONVERT(DATETIME,Claim_Period_From ),106),' ',' '), ''
			FROM SubsidizeGroupClaim a    
			inner join SubsidizeGroupClaimItemDetails b   
			ON a.scheme_code = b.scheme_code and a.scheme_seq = b.scheme_seq and a.subsidize_code = b.subsidize_code    
			and b.Subsidize_Item_Code NOT IN ('PV', 'EHAPP_R', 'PV13')
			WHERE b.record_status = 'A'    
			GROUP BY a.Display_Code_For_Claim,a.scheme_seq, Claim_Period_From    
			ORDER BY a.Display_Code_For_Claim,a.scheme_seq   

		SET @seq = @seq + 1

		-- ---------------------------------------------
		-- Return Result: Remarks
		-- ---------------------------------------------

		SELECT Col01, Col02 FROM #Remarks ORDER BY Seq, Seq2, Col01

	END

	-- =============================================
	-- 6. RELEASE RESOURCE
	-- =============================================
	DROP TABLE #ServiceProviderBase
	DROP TABLE #VoucherTransactionBase
	DROP TABLE #01_NoOfTransaction_ResultTable
	DROP TABLE #02_PostPaymentCheck_ResultTable
	DROP TABLE #Criteria
	DROP TABLE #Remarks
END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_PPC0003_Report_get] TO HCVU
GO
