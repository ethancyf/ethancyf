IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_PPC0002_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_PPC0002_Report_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE18-020 (HKIC Symbol Others)
-- Modified by:		Winnie SUEN
-- Modified date:	25 Feb 2019
-- Description:		Show HKIC Symbol Description
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	22 Jan 2019
-- CR No.:			INT18-0030
-- Description:		Fix the data type of Chinese name to NVARCHAR
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
-- Modified by:	    Dickson Law
-- Modified date:   27 Dec 2017
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Description:		Get [Date of Death] and [Date of Death Flag] from personal information
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
-- Description:		Randomly generate the valid claim transaction - PPC0002
-- =============================================

-- =============================================
-- Rule for generation of Report PPC0002
-- =============================================
-- 1. Target number of transaction
--	  a. Mandatory, and
--	  b. Maximum allowed 500
--
-- 2. Number of transaction with the highest amount claimed
--	  a. Optional, and
--    b. Sort by "Claim_Amount", and
--    c. Random Selection when handles same amount cases
--
-- 3. Number of manual input
--    a. Optional, and
--    b. Exclude the selected transaction
--    c. If insufficient, Smart IC input will be selected within the defined period
--
-- 4. Number of Smart IC input
--    a. Optional, and
--    b. Exclude the selected transaction
--    c. If insufficient, manual input will be selected within the defined period
--
-- 5. Target number of transaction >= No. of highest amount claimed + No. of manual input + No. of Smart IC input
--
-- 6. If Target number of transaction > No. of highest amount claimed + No. of manual input + No. of Smart IC input, the remainder will be randomly selected within the defined period.
--    e.g. Target number of transaction		= 100
--		   No. of highest amount claimed	= 10
--		   No. of manual input				= 50
--         No. of Smart IC input			= 30
--		   Remainder (Random Selection)		= 100 - 10 - 50 - 30 = 10
--
-- 7. If have insufficient transaction in the defined period, the remainders will be selected from the previous months (up to maximum 6 months) until the target number of transaction is fulfilled
--    a. Optional
--    b. Remainder(s) will be randomly selected first from Manual input, then Smart IC input (regardless the highest amount)
--    c. Get the transaction month by month(up to maximum 6 months) until fulfill the the user’s selection
--
-- 8. If no. of highest amount, smart IC input and manual input are not selected, the number of transactions will be randomly selected within the defined period.
--

CREATE PROCEDURE [dbo].[proc_EHS_PPC0002_Report_get]
	@request_time DATETIME,
	@SPList VARCHAR(5000),
	@ProfessionList VARCHAR(5000),
	@Scheme VARCHAR(5000),
	@Period_Type CHAR(1),				-- "T" = Transaction Date, "S" = Service Date
	@Period_Format CHAR(1),				-- "E" = Exact Date, "M" = Month and Year
	@Period_From DATETIME,
	@Period_To DATETIME,
	@Transaction_Status VARCHAR(5000),
	@IncludeBackOfficeClaims BIT,
	@IncludeInvalidationClaims BIT,
	@TargetNoOfTransaction INT,
	@NoOfHighestAmountClaimed INT,		-- "-1" = Not checked the checkbox
	@NoOfManualInput INT,				-- "-1" = Not checked the checkbox
	@NoOfSmartICInput INT,				-- "-1" = Not checked the checkbox
	@IncludePreviousMonth BIT			-- IF @Period_Format = "E", this argument @IncludePreviousMonth will be ignored.		
AS
BEGIN
	SET NOCOUNT ON;
	-- ===========================================================
	-- 1.0	DECLARATION
	-- ===========================================================
	DECLARE @IN_varSPList VARCHAR(5000)
	DECLARE @IN_varProfessional VARCHAR(5000)
	DECLARE @IN_varSchemeCode VARCHAR(5000)
	DECLARE @IN_chrPeriod_Type CHAR(1)
	DECLARE @IN_dtmPeriod_From DATETIME
	DECLARE @IN_dtmPeriod_To DATETIME
	DECLARE @IN_chrPeriod_Format CHAR(1)
	DECLARE @IN_varRecordStatus VARCHAR(5000)
	DECLARE @IN_bitIncludeBackOfficeClaim BIT
	DECLARE @IN_bitIncludeInvalidatedClaim BIT
	DECLARE @IN_intTargetNoOfTransaction INT
	DECLARE @IN_intNoOfHighestAmountClaimed INT
	DECLARE @IN_intNoOfManualInput INT
	DECLARE @IN_intNoOfSmartICInput INT
	DECLARE @IN_bitIncludePreviousMonth BIT

	DECLARE @IN_dtmTransactionDateStart DATETIME
	DECLARE @IN_dtmTransactionDateEnd DATETIME
	DECLARE @IN_dtmServiceDateStart DATETIME
	DECLARE @IN_dtmServiceDateEnd DATETIME

	--DECLARE @chrHasSPPracticeList CHAR(1)
	DECLARE @chrIsAllProfessional CHAR(1)
	DECLARE @chrIsAllRecordStatus CHAR(1)
	DECLARE @chrIsAllSchemeCode CHAR(1)

	DECLARE @current_dtm DATETIME
	DECLARE @delimiter VARCHAR(5)
	DECLARE @SPListDelimiter VARCHAR(5)
	DECLARE @SPListParmName VARCHAR(30)
	DECLARE @SPListGenerationID CHAR(12)

	DECLARE @dtmExtendTransactionDateStart DATETIME
	DECLARE @dtmExtendServiceDateStart DATETIME

	DECLARE @ConstantNotSpecified VARCHAR(50)
	DECLARE @ConstantHighestAmountClaimed VARCHAR(50)
	DECLARE @ConstantManualInput VARCHAR(50)
	DECLARE @ConstantSmartICInput VARCHAR(50)
	DECLARE @ConstantManualInputSupplement VARCHAR(50)
	DECLARE @ConstantSmartICInputSupplement VARCHAR(50)
	DECLARE @ConstantPreviousMonth VARCHAR(50)

	DECLARE @seq int

	DECLARE @tblSPList TABLE(
		SP_ID CHAR(8),
		Practice_Display_Seq SMALLINT
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

	CREATE TABLE #VoucherTransactionBase (
		SP_ID CHAR(8),
		Transaction_ID CHAR(20),
		Practice_Display_Seq SMALLINT,
		Claim_Amount Money,
		Create_By_SmartID CHAR(1)
	)

	CREATE NONCLUSTERED INDEX IX_VoucherTransactionBase_SP_ID
		ON #VoucherTransactionBase (SP_ID); 

	CREATE NONCLUSTERED INDEX IX_VoucherTransactionBase_SP_ID_Practice_Display_Seq
		ON #VoucherTransactionBase (SP_ID, Practice_Display_Seq); 

	CREATE NONCLUSTERED INDEX IX_VoucherTransactionBase_SP_ID_Claim_Amount
		ON #VoucherTransactionBase (SP_ID, Claim_Amount); 

	CREATE NONCLUSTERED INDEX IX_VoucherTransactionBase_Create_By_SmartID
		ON #VoucherTransactionBase (Create_By_SmartID); 

	CREATE TABLE #01_NoOfTransaction_ResultTable    
	(         
		SP_ID CHAR(8) NOT NULL,    
		Practice_Display_Seq SMALLINT NOT NULL,
		Period_In_Month	INT NOT NULL,			-- Remarks: Values [0-6], 0 = Current, 6 = Last 6 months
		Count_Transaction INT NULL,    
	)  

	CREATE NONCLUSTERED INDEX IX_NoOfTransaction_ResultTable_SP_ID_Practice_Display_Seq
		ON #01_NoOfTransaction_ResultTable (SP_ID, Practice_Display_Seq); 

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

	CREATE TABLE #Control (
		Seq		INT IDENTITY(1,1),
		Seq2	INT,
		Col01	CHAR(1),
		Col02	VARCHAR(1000)
	)

	CREATE TABLE #Content (
		Seq		INT IDENTITY(1,1),
		Seq2	INT,
		Col01	VARCHAR(1000),
		Col02	VARCHAR(1000)
	)

	CREATE TABLE #Criteria (
		Seq		INT IDENTITY(1,1),
		Seq2	INT,
		Col01	VARCHAR(1000),
		Col02	VARCHAR(1000)
	)


	CREATE TABLE #03_PostPaymentCheck_ResultTableForEachSP    
	(    
		DisplaySeq INT IDENTITY(1,1),     
		Col01 VARCHAR(1000) DEFAULT '',    
		Col02 VARCHAR(1000) DEFAULT '',    
		Col03 VARCHAR(1000) DEFAULT '',    
		Col04 NVARCHAR(1000) DEFAULT '',    
		Col05 VARCHAR(1000) DEFAULT '',    
		Col06 VARCHAR(1000) DEFAULT '',    
		Col07 VARCHAR(1000) DEFAULT '',    
		Col08 VARCHAR(1000) DEFAULT '',    
		Col09 VARCHAR(1000) DEFAULT '',    
		Col10 VARCHAR(1000) DEFAULT '',
		Col11 VARCHAR(1000) DEFAULT '',    
		Col12 VARCHAR(1000) DEFAULT '',    
		Col13 VARCHAR(1000) DEFAULT '',    
		Col14 VARCHAR(1000) DEFAULT '',    
		Col15 VARCHAR(1000) DEFAULT '',    
		Col16 NVARCHAR(1000) DEFAULT '',    
		Co117 VARCHAR(1000) DEFAULT '',    
		Col18 VARCHAR(1000) DEFAULT '',    
		Col19 NVARCHAR(1000) DEFAULT '',    
		Col20 VARCHAR(1000) DEFAULT '',
		Col21 VARCHAR(1000) DEFAULT '',    
		Col22 VARCHAR(1000) DEFAULT '',    
		Col23 VARCHAR(1000) DEFAULT '',    
		Col24 VARCHAR(1000) DEFAULT '',    
		Col25 VARCHAR(1000) DEFAULT '',    
		Col26 VARCHAR(1000) DEFAULT '',    
		Col27 VARCHAR(1000) DEFAULT ''
	)  

	CREATE TABLE #Remarks (
		Seq		INT,
		Seq2	INT,
		Col01	VARCHAR(1000),
		Col02	VARCHAR(1000)
	)

	DECLARE @Str_NA varchar(10)
	DECLARE @Str_Valid varchar(10)
	DECLARE @Str_ConnectionFailed varchar(50)
	-- ===========================================================
	-- 1.1	Initialization
	-- ===========================================================
	SET @current_dtm = GETDATE()
	SET @delimiter = ','
	SET @SPListDelimiter = ':'
	SET @SPListParmName = NULL
	SET @SPListGenerationID = NULL

	SET @dtmExtendServiceDateStart = NULL
	SET @dtmExtendTransactionDateStart = NULL

	SET @ConstantNotSpecified  = 'Not Specified'
	SET @ConstantHighestAmountClaimed  = 'Highest amount claimed'
	SET @ConstantManualInput  = 'Manual'
	SET @ConstantSmartICInput  = 'Smart IC'
	SET @ConstantManualInputSupplement = 'Manual (Supplement)'
	SET @ConstantSmartICInputSupplement = 'Smart IC (Supplement)'
	SET @ConstantPreviousMonth = 'Previous Months'
		
	SELECT @Str_NA = Description 				FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName='NA'
	SELECT @Str_Valid = Description 			FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName='OCSSSResultValid'
	SELECT @Str_ConnectionFailed = Description 	FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName='OCSSSResultConnectionFailed'

	-- ===========================================================
	-- 1.2	COPY & VERIFY PARAMETERS 
	-- ===========================================================
	SET @IN_chrPeriod_Type = @Period_Type
	SET @IN_dtmPeriod_From = @Period_From
	SET @IN_dtmPeriod_To = @Period_To
	SET @IN_chrPeriod_Format = @Period_Format

	SET @IN_bitIncludeBackOfficeClaim = @IncludeBackOfficeClaims
	SET @IN_bitIncludeInvalidatedClaim = @IncludeInvalidationClaims
	SET @IN_intTargetNoOfTransaction = 	@TargetNoOfTransaction
	SET @IN_intNoOfHighestAmountClaimed = @NoOfHighestAmountClaimed
	SET @IN_intNoOfManualInput = @NoOfManualInput
	SET @IN_intNoOfSmartICInput = @NoOfSmartICInput
	SET @IN_bitIncludePreviousMonth = @IncludePreviousMonth

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

	IF @IN_intNoOfHighestAmountClaimed IS NOT NULL
	BEGIN
		IF @IN_intNoOfHighestAmountClaimed = ''
			SET @IN_intNoOfHighestAmountClaimed = NULL

		IF @IN_intNoOfHighestAmountClaimed = -1
			SET @IN_intNoOfHighestAmountClaimed = NULL
	END

	IF @IN_intNoOfManualInput IS NOT NULL
	BEGIN
		IF @IN_intNoOfManualInput = ''
			SET @IN_intNoOfManualInput = NULL

		IF @IN_intNoOfManualInput = -1
			SET @IN_intNoOfManualInput = NULL
	END

	IF @IN_intNoOfSmartICInput IS NOT NULL
	BEGIN
		IF @IN_intNoOfSmartICInput = ''
			SET @IN_intNoOfSmartICInput = NULL

		IF @IN_intNoOfSmartICInput = -1
			SET @IN_intNoOfSmartICInput = NULL
	END

	IF @IN_bitIncludePreviousMonth IS NOT NULL
	BEGIN
		IF @IN_bitIncludePreviousMonth = ''
			SET @IN_bitIncludePreviousMonth = 0

		IF @IN_chrPeriod_Format = 'E'
			SET @IN_bitIncludePreviousMonth = 0
	END
	ELSE
		SET @IN_bitIncludePreviousMonth = 0

	-- ===========================================================
	-- 2. PREPARATION
	-- ===========================================================

	IF @IN_intTargetNoOfTransaction IS NOT NULL																			-- Rule 1a & 5
		AND ISNULL(@IN_intTargetNoOfTransaction,0) >= ISNULL(@IN_intNoOfHighestAmountClaimed,0) + ISNULL(@IN_intNoOfManualInput,0) + ISNULL(@IN_intNoOfSmartICInput,0)																	
	BEGIN
		-- ===========================================================
		-- 2.1 PREPARE TABLE TO STORE MANUAL INPUTTED SERVICE PROVIDER 
		-- ===========================================================
		--SET @chrHasSPPracticeList = 'N'

		IF @IN_varSPList IS NOT NULL
		BEGIN
			IF CHARINDEX(@SPListDelimiter,@IN_varSPList) > 0
			BEGIN
				--SET @SPListGenerationID = SUBSTRING(@IN_varSPList, 1, CHARINDEX(@SPListDelimiter,@IN_varSPList)-1)
				--SET @SPListParmName = SUBSTRING(@IN_varSPList, CHARINDEX(@SPListDelimiter,@IN_varSPList)+1 ,LEN(@IN_varSPList) - CHARINDEX(@SPListDelimiter,@IN_varSPList))

				SET @SPListGenerationID = SUBSTRING(RIGHT(@IN_varSPList,LEN(@IN_varSPList) - CHARINDEX(@SPListDelimiter,@IN_varSPList)), 1, CHARINDEX(@SPListDelimiter,RIGHT(@IN_varSPList,LEN(@IN_varSPList) - CHARINDEX(@SPListDelimiter,@IN_varSPList)))-1)
				SET @SPListParmName = SUBSTRING(RIGHT(@IN_varSPList,LEN(@IN_varSPList) - CHARINDEX(@SPListDelimiter,@IN_varSPList)), CHARINDEX(@SPListDelimiter,RIGHT(@IN_varSPList,LEN(@IN_varSPList) - CHARINDEX(@SPListDelimiter,@IN_varSPList)))+1 ,LEN(RIGHT(@IN_varSPList,LEN(@IN_varSPList) - CHARINDEX(@SPListDelimiter,@IN_varSPList))))

				---------------------------------------------------------------------------
				-- Use cursor to split SP ID and Practice Display Seq. and store it
				---------------------------------------------------------------------------
				SET NOCOUNT ON
				DECLARE @Parm_Seq INT

				DECLARE ServiceProviderPerPractice_Cursor CURSOR FOR 
					SELECT Parm_Seq FROM FileGenerationQueueAdditionalParameter
						WHERE 
							Generation_ID = @SPListGenerationID 
							AND Parm_Name = @SPListParmName

				OPEN ServiceProviderPerPractice_Cursor

				FETCH NEXT FROM ServiceProviderPerPractice_Cursor INTO @Parm_Seq

				WHILE @@FETCH_STATUS = 0
				BEGIN
		
					INSERT @tblSPList(SP_ID, Practice_Display_Seq)
					SELECT
						SP.SP_ID,
						Practice.Practice_Display_Seq
					FROM
						(SELECT 
							SUBSTRING(Parm_Value, 1, CHARINDEX(@SPListDelimiter, Parm_Value)-1)	AS SP_ID
						FROM FileGenerationQueueAdditionalParameter
						WHERE 
							Generation_ID = @SPListGenerationID 
							AND Parm_Name = @SPListParmName
							AND Parm_Seq = @Parm_Seq) SP
					CROSS JOIN		
						(SELECT Item AS Practice_Display_Seq FROM func_split_string(
							(SELECT 
								SUBSTRING(Parm_Value, CHARINDEX(@SPListDelimiter, Parm_Value)+1 ,LEN(Parm_Value) - CHARINDEX(@SPListDelimiter,Parm_Value)) AS Practice_Display_Seq_List				
							FROM FileGenerationQueueAdditionalParameter
							WHERE 
								Generation_ID = @SPListGenerationID 
								AND Parm_Name = @SPListParmName
								AND Parm_Seq = @Parm_Seq)
							, @delimiter)) Practice
				
					FETCH NEXT FROM ServiceProviderPerPractice_Cursor INTO @Parm_Seq
				END

				CLOSE ServiceProviderPerPractice_Cursor
				DEALLOCATE ServiceProviderPerPractice_Cursor

				--SELECT * FROM @tblSPList

				--IF (SELECT COUNT(1) FROM @tblSPList) > 0 	
				--	SET @chrHasSPPracticeList = 'Y'
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

		-- ===========================================================
		-- 3. PRE-CALCULATION
		-- ===========================================================
		-- ===========================================================
		-- 3.1. COUNTING POSSIBLE TRANSACTIONS
		-- ===========================================================
		--------------------------
		-- Initialization
		--------------------------
		IF @IN_bitIncludePreviousMonth = 1
		BEGIN
			IF @IN_chrPeriod_Type = 'S'
				BEGIN
					SET @dtmExtendServiceDateStart = DATEFROMPARTS(DATEPART(YYYY ,DATEADD(MM, -6, @IN_dtmServiceDateStart)), DATEPART(MM,DATEADD(MM, -6, @IN_dtmServiceDateStart)), 1)
				END

			ELSE IF @IN_chrPeriod_Type = 'T'
				BEGIN
					SET @dtmExtendTransactionDateStart = DATEFROMPARTS(DATEPART(YYYY ,DATEADD(MM, -6, @IN_dtmTransactionDateStart)), DATEPART(MM,DATEADD(MM, -6, @IN_dtmTransactionDateStart)), 1)
				END

			INSERT #01_NoOfTransaction_ResultTable(
				SP_ID,
				Practice_Display_Seq,
				Period_In_Month,
				Count_Transaction)	
			SELECT 
				VTC.SP_ID,
				VTC.Practice_Display_Seq,
				VTC.Period_In_Month,
				COUNT(VTC.Period_In_Month)
			FROM
			(SELECT
				VT.SP_ID,
				VT.Practice_Display_Seq,
				VT.Transaction_dtm,
				VT.Service_Receive_dtm,
				[Period_In_Month] =
					CASE
						-- Determine Transaction in which Previous Month
						WHEN (@dtmExtendTransactionDateStart IS NOT NULL)
						THEN 
							CASE
								WHEN DATEDIFF(MM, VT.Transaction_Dtm ,@IN_dtmTransactionDateStart) < 0
								THEN 0
								ELSE DATEDIFF(MM, VT.Transaction_Dtm ,@IN_dtmTransactionDateStart)
							END

						WHEN (@dtmExtendServiceDateStart IS NOT NULL)
						THEN
							CASE
								WHEN DATEDIFF(MM, VT.Service_Receive_Dtm ,@IN_dtmServiceDateStart) < 0
								THEN 0
								ELSE DATEDIFF(MM, VT.Service_Receive_Dtm ,@IN_dtmServiceDateStart)
							END
					END
			FROM 
				VoucherTransaction VT
					LEFT JOIN @tblProfessional tblProf
						ON LTRIM(RTRIM(VT.Service_Type)) = tblProf.Service_Category_Code
					LEFT JOIN @tblRecordStatus tblRS
						ON LTRIM(RTRIM(VT.Record_Status)) = tblRS.Record_Status
					LEFT JOIN @tblSchemeCode tblSC
						ON LTRIM(RTRIM(VT.Scheme_Code)) = LTRIM(RTRIM(tblSC.Scheme_Code))
					LEFT JOIN @tblSPList tblSPList
						ON LTRIM(RTRIM(VT.SP_ID)) = tblSPList.SP_ID AND VT.Practice_Display_Seq = tblSPList.Practice_Display_Seq
			WHERE
				((@dtmExtendTransactionDateStart IS NULL AND @IN_dtmTransactionDateEnd IS NULL)
					OR (Transaction_Dtm >= @dtmExtendTransactionDateStart AND Transaction_Dtm < @IN_dtmTransactionDateEnd))
				AND ((@dtmExtendServiceDateStart IS NULL AND @IN_dtmServiceDateEnd IS NULL)
					OR (Service_Receive_Dtm >= @dtmExtendServiceDateStart AND Service_Receive_Dtm <= @IN_dtmServiceDateEnd))
				AND (@IN_bitIncludeBackOfficeClaim IS NULL
					OR @IN_bitIncludeBackOfficeClaim = 1
					OR (@IN_bitIncludeBackOfficeClaim = 0 AND (VT.Manual_Reimburse IS NULL OR VT.Manual_Reimburse <> 'Y')))
				AND (@IN_bitIncludeInvalidatedClaim IS NULL 
					OR @IN_bitIncludeInvalidatedClaim = 1
					OR (@IN_bitIncludeInvalidatedClaim = 0 AND (Invalidation IS NULL OR Invalidation NOT IN ('I','P'))))
				AND (@chrIsAllProfessional = 'Y' OR tblProf.Service_Category_Code IS NOT NULL)
				AND (@chrIsAllRecordStatus = 'Y' OR tblRS.Record_Status IS NOT NULL)
				AND (@chrIsAllSchemeCode = 'Y' OR tblSC.Scheme_Code IS NOT NULL)
				AND tblSPList.SP_ID IS NOT NULL
			) AS VTC
		GROUP BY 
			VTC.SP_ID,
			VTC.Practice_Display_Seq,
			VTC.Period_In_Month

		END
		ELSE
		BEGIN
			INSERT #01_NoOfTransaction_ResultTable(
				SP_ID,
				Practice_Display_Seq,
				Period_In_Month,
				Count_Transaction)
			SELECT
				VT.SP_ID,
				VT.Practice_Display_Seq,
				0,
				COUNT(VT.Transaction_ID)
			FROM 
				VoucherTransaction VT
					LEFT JOIN @tblProfessional tblProf
						ON LTRIM(RTRIM(VT.Service_Type)) = tblProf.Service_Category_Code
					LEFT JOIN @tblRecordStatus tblRS
						ON LTRIM(RTRIM(VT.Record_Status)) = tblRS.Record_Status
					LEFT JOIN @tblSchemeCode tblSC
						ON LTRIM(RTRIM(VT.Scheme_Code)) = LTRIM(RTRIM(tblSC.Scheme_Code))
					LEFT JOIN @tblSPList tblSPList
						ON LTRIM(RTRIM(VT.SP_ID)) = tblSPList.SP_ID AND VT.Practice_Display_Seq = tblSPList.Practice_Display_Seq
			WHERE
				((@IN_dtmTransactionDateStart IS NULL AND @IN_dtmTransactionDateEnd IS NULL)
					OR (Transaction_Dtm >= @IN_dtmTransactionDateStart AND Transaction_Dtm < @IN_dtmTransactionDateEnd))
				AND ((@IN_dtmServiceDateStart IS NULL AND @IN_dtmServiceDateEnd IS NULL)
					OR (Service_Receive_Dtm >= @IN_dtmServiceDateStart AND Service_Receive_Dtm <= @IN_dtmServiceDateEnd))
				AND (@IN_bitIncludeBackOfficeClaim IS NULL
					OR @IN_bitIncludeBackOfficeClaim = 1
					OR (@IN_bitIncludeBackOfficeClaim = 0 AND (VT.Manual_Reimburse IS NULL OR VT.Manual_Reimburse <> 'Y')))
				AND (@IN_bitIncludeInvalidatedClaim IS NULL 
					OR @IN_bitIncludeInvalidatedClaim = 1
					OR (@IN_bitIncludeInvalidatedClaim = 0 AND (Invalidation IS NULL OR Invalidation NOT IN ('I','P'))))
				AND (@chrIsAllProfessional = 'Y' OR tblProf.Service_Category_Code IS NOT NULL)
				AND (@chrIsAllRecordStatus = 'Y' OR tblRS.Record_Status IS NOT NULL)
				AND (@chrIsAllSchemeCode = 'Y' OR tblSC.Scheme_Code IS NOT NULL)
				AND tblSPList.SP_ID IS NOT NULL
			GROUP BY
				VT.SP_ID,
				VT.Practice_Display_Seq		
		END

		-- For Checking Use
		-- SELECT * FROM #01_NoOfTransaction_ResultTable ORDER BY SP_ID, Practice_Display_Seq, Period_In_Month

		-- =========================================================================
		-- 4. RETRIEVE POSSIBLE TRANSACTIONS OF EACH SERVICE PROVIDER
		-- =========================================================================
		INSERT
			#VoucherTransactionBase
			(SP_ID,
			Transaction_ID,
			Practice_Display_Seq,
			Claim_Amount,
			Create_By_SmartID)
		SELECT
			VT.SP_ID,
			VT.Transaction_ID,
			VT.Practice_Display_Seq,
			VT.Claim_Amount,
			VT.Create_By_SmartID
		FROM 
			VoucherTransaction VT
				LEFT JOIN @tblProfessional tblProf
					ON LTRIM(RTRIM(VT.Service_Type)) = tblProf.Service_Category_Code
				LEFT JOIN @tblRecordStatus tblRS
					ON LTRIM(RTRIM(VT.Record_Status)) = tblRS.Record_Status
				LEFT JOIN @tblSchemeCode tblSC
					ON LTRIM(RTRIM(VT.Scheme_Code)) = LTRIM(RTRIM(tblSC.Scheme_Code))
				LEFT JOIN @tblSPList tblSPList
					ON LTRIM(RTRIM(VT.SP_ID)) = tblSPList.SP_ID AND VT.Practice_Display_Seq = tblSPList.Practice_Display_Seq
		WHERE
			((@IN_dtmTransactionDateStart IS NULL AND @IN_dtmTransactionDateEnd IS NULL)
				OR (Transaction_Dtm >= @IN_dtmTransactionDateStart AND Transaction_Dtm < @IN_dtmTransactionDateEnd))
			AND ((@IN_dtmServiceDateStart IS NULL AND @IN_dtmServiceDateEnd IS NULL)
				OR (Service_Receive_Dtm >= @IN_dtmServiceDateStart AND Service_Receive_Dtm <= @IN_dtmServiceDateEnd))
			AND (@IN_bitIncludeBackOfficeClaim IS NULL
				OR @IN_bitIncludeBackOfficeClaim = 1
				OR (@IN_bitIncludeBackOfficeClaim = 0 AND (VT.Manual_Reimburse IS NULL OR VT.Manual_Reimburse <> 'Y')))
			AND (@IN_bitIncludeInvalidatedClaim IS NULL 
				OR @IN_bitIncludeInvalidatedClaim = 1
				OR (@IN_bitIncludeInvalidatedClaim = 0 AND (Invalidation IS NULL OR Invalidation NOT IN ('I','P'))))
			AND (@chrIsAllProfessional = 'Y' OR tblProf.Service_Category_Code IS NOT NULL)
			AND (@chrIsAllRecordStatus = 'Y' OR tblRS.Record_Status IS NOT NULL)
			AND (@chrIsAllSchemeCode = 'Y' OR tblSC.Scheme_Code IS NOT NULL)
			AND tblSPList.SP_ID IS NOT NULL


		-- =============================================================
		-- 4.1 RANDOM SELECTION OF TRANSACTIONS OF EACH SERVICE PROVIDER
		-- =============================================================		
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
			DECLARE @CurrentCount INT
			DECLARE @REMAINDER INT
			DECLARE @Period_In_Month_Seq_For_Random_Generation INT
			DECLARE @Period_In_Month_NoOfTx_For_Random_Generation INT
			DECLARE @Extended_Period_In_Month INT

			DECLARE Transaction_Cursor CURSOR FOR 
				SELECT DISTINCT SP_ID, Practice_Display_Seq FROM #01_NoOfTransaction_ResultTable

			OPEN Transaction_Cursor

			FETCH NEXT FROM Transaction_Cursor INTO @RT02_SP_ID, @RT02_Practice_Display_Seq

			WHILE @@FETCH_STATUS = 0
			BEGIN
				--============================================
				-- Define Period
				--============================================
				---------------------------------------------
				-- Highest Amount Claimed (BY "Claim_Amount")
				---------------------------------------------
				IF @IN_intNoOfHighestAmountClaimed IS NOT NULL															-- Rule 2a
				BEGIN
					INSERT #02_PostPaymentCheck_ResultTable(Col01,Col02,Col03,Col04)
						SELECT TOP (@IN_intNoOfHighestAmountClaimed) 
							@RT02_SP_ID, 
							VTB.Transaction_ID,
							VTB.Practice_Display_Seq,
							@ConstantHighestAmountClaimed
						FROM
							#VoucherTransactionBase VTB
						WHERE 
							SP_ID = @RT02_SP_ID 
							AND Practice_Display_Seq = @RT02_Practice_Display_Seq
							AND NOT EXISTS
								(SELECT Col02 FROM #02_PostPaymentCheck_ResultTable WHERE Col02 = VTB.Transaction_ID)
						ORDER BY Claim_Amount DESC, NEWID()																-- Rule 2b & 2c
				END

				------------------------------------------
				-- Manual Input (Randomly Selection)
				------------------------------------------
				IF @IN_intNoOfManualInput IS NOT NULL																	-- Rule 3a
				BEGIN
					INSERT #02_PostPaymentCheck_ResultTable(Col01,Col02,Col03,Col04)
						SELECT TOP (@IN_intNoOfManualInput) 
							@RT02_SP_ID, 
							VTB.Transaction_ID,
							VTB.Practice_Display_Seq,
							@ConstantManualInput
						FROM
							#VoucherTransactionBase VTB
						WHERE 
							SP_ID = @RT02_SP_ID 
							AND Practice_Display_Seq = @RT02_Practice_Display_Seq
							AND Create_By_SmartID = 'N'
							AND NOT EXISTS
								(SELECT Col02 FROM #02_PostPaymentCheck_ResultTable WHERE Col02 = VTB.Transaction_ID)	-- Rule 3b
						ORDER BY NEWID()
				END

				------------------------------------------
				-- Smart IC Input (Randomly Selection)
				------------------------------------------
				IF @IN_intNoOfSmartICInput IS NOT NULL																	-- Rule 4a
				BEGIN
					INSERT #02_PostPaymentCheck_ResultTable(Col01,Col02,Col03,Col04)
						SELECT TOP (@IN_intNoOfSmartICInput) 
							@RT02_SP_ID, 
							VTB.Transaction_ID,
							VTB.Practice_Display_Seq,
							@ConstantSmartICInput
						FROM
							#VoucherTransactionBase VTB
						WHERE 
							SP_ID = @RT02_SP_ID 
							AND Practice_Display_Seq = @RT02_Practice_Display_Seq
							AND Create_By_SmartID = 'Y'
							AND NOT EXISTS
								(SELECT Col02 FROM #02_PostPaymentCheck_ResultTable WHERE Col02 = VTB.Transaction_ID)	-- Rule 4b
						ORDER BY NEWID()
				END

				---------------------------------------------
				-- Manual (Supplement) (Randomly Selection)
				---------------------------------------------
				IF @IN_intNoOfManualInput IS NOT NULL
				BEGIN
					SET @CurrentCount = (SELECT COUNT(1) FROM #02_PostPaymentCheck_ResultTable WHERE Col01 = @RT02_SP_ID AND Col03 = @RT02_Practice_Display_Seq AND Col04 = @ConstantManualInput)

					IF @IN_intNoOfManualInput > @CurrentCount															-- Rule 3c
					BEGIN
						SET @REMAINDER = 0

						SET @REMAINDER = @IN_intNoOfManualInput - @CurrentCount
				
						IF @REMAINDER > 0
						BEGIN
							INSERT #02_PostPaymentCheck_ResultTable(Col01,Col02,Col03,Col04)
								SELECT TOP (@REMAINDER) 
									@RT02_SP_ID, 
									VTB.Transaction_ID,
									VTB.Practice_Display_Seq,
									@ConstantManualInputSupplement
								FROM
									#VoucherTransactionBase VTB
								WHERE 
									SP_ID = @RT02_SP_ID 
									AND Practice_Display_Seq = @RT02_Practice_Display_Seq
									AND Create_By_SmartID = 'Y'
									AND NOT EXISTS
										(SELECT Col02 FROM #02_PostPaymentCheck_ResultTable WHERE Col02 = VTB.Transaction_ID)
								ORDER BY NEWID()
						END
					END
				END

				-------------------------------------------
				-- Smart IC (Supplement) (Randomly Selection)
				-------------------------------------------
				IF @IN_intNoOfSmartICInput IS NOT NULL
				BEGIN
					SET @CurrentCount = (SELECT COUNT(1) FROM #02_PostPaymentCheck_ResultTable WHERE Col01 = @RT02_SP_ID AND Col03 = @RT02_Practice_Display_Seq AND Col04 = @ConstantSmartICInput)

					IF @IN_intNoOfSmartICInput > @CurrentCount															-- Rule 4c
					BEGIN
						SET @REMAINDER = 0

						SET @REMAINDER = @IN_intNoOfSmartICInput - @CurrentCount
				
						IF @REMAINDER > 0
						BEGIN
							INSERT #02_PostPaymentCheck_ResultTable(Col01,Col02,Col03,Col04)
								SELECT TOP (@REMAINDER) 
									@RT02_SP_ID, 
									VTB.Transaction_ID,
									VTB.Practice_Display_Seq, 
									@ConstantSmartICInputSupplement
								FROM
									#VoucherTransactionBase VTB
								WHERE 
									SP_ID = @RT02_SP_ID 
									AND Practice_Display_Seq = @RT02_Practice_Display_Seq
									AND Create_By_SmartID = 'N'
									AND NOT EXISTS
										(SELECT Col02 FROM #02_PostPaymentCheck_ResultTable WHERE Col02 = VTB.Transaction_ID)
								ORDER BY NEWID()
						END
					END
				END

				------------------------------------------
				-- Randomly Selection of Transaction																	-- Rule 6 & 8
				------------------------------------------
				IF (@IN_intTargetNoOfTransaction > ISNULL(@IN_intNoOfHighestAmountClaimed,0) + ISNULL(@IN_intNoOfManualInput,0) + ISNULL(@IN_intNoOfSmartICInput,0))
				BEGIN
					SET @REMAINDER = 0

					SET @REMAINDER = @IN_intTargetNoOfTransaction - (SELECT COUNT(1) FROM #02_PostPaymentCheck_ResultTable WHERE Col01 = @RT02_SP_ID AND Col03 = @RT02_Practice_Display_Seq)

					INSERT #02_PostPaymentCheck_ResultTable(Col01,Col02,Col03,Col04)
						SELECT TOP (@REMAINDER) 
							@RT02_SP_ID, 
							VTB.Transaction_ID,
							VTB.Practice_Display_Seq,
							@ConstantNotSpecified
						FROM
							#VoucherTransactionBase VTB
						WHERE 
							SP_ID = @RT02_SP_ID 
							AND Practice_Display_Seq = @RT02_Practice_Display_Seq
							AND NOT EXISTS
								(SELECT Col02 FROM #02_PostPaymentCheck_ResultTable WHERE Col02 = VTB.Transaction_ID)
						ORDER BY NEWID()
				END

				--============================================
				-- Extended Period
				--============================================
				IF @IN_bitIncludePreviousMonth = 1																		-- Rule 7a
				BEGIN
					SET @REMAINDER = 0

					SET @REMAINDER = @IN_intTargetNoOfTransaction - (SELECT COUNT(1) FROM #02_PostPaymentCheck_ResultTable WHERE Col01 = @RT02_SP_ID AND Col03 = @RT02_Practice_Display_Seq)

					IF @REMAINDER > 0 AND (SELECT COUNT(1) FROM #01_NoOfTransaction_ResultTable	WHERE SP_ID = @RT02_SP_ID AND Practice_Display_Seq = @RT02_Practice_Display_Seq AND	Period_In_Month > 0) > 0
					BEGIN
					
						SET @Period_In_Month_Seq_For_Random_Generation = 0
						SET @Period_In_Month_NoOfTx_For_Random_Generation = 0

						SELECT 
							@Period_In_Month_Seq_For_Random_Generation = NET_COUNT.Period_In_Month,
							@Period_In_Month_NoOfTx_For_Random_Generation = NET_COUNT.NoOfTx_For_Random_Generation
						FROM (
							SELECT
								Period_In_Month,
								Count_Transaction,
								@REMAINDER - SUM(Count_Transaction) OVER(PARTITION BY SP_ID, Practice_Display_Seq ORDER BY SP_ID, Practice_Display_Seq,Period_In_Month ROWS BETWEEN unbounded preceding AND current row) AS BALANCE,
								Count_Transaction + (@REMAINDER - SUM(Count_Transaction) OVER(PARTITION BY SP_ID, Practice_Display_Seq ORDER BY SP_ID, Practice_Display_Seq,Period_In_Month ROWS BETWEEN unbounded preceding AND current row)) AS NoOfTx_For_Random_Generation
							FROM 
								#01_NoOfTransaction_ResultTable
							WHERE
								SP_ID = @RT02_SP_ID 
								AND Practice_Display_Seq = @RT02_Practice_Display_Seq 
								AND	Period_In_Month > 0
							) NET_COUNT
						WHERE
							NET_COUNT.BALANCE <= 0
							AND NET_COUNT.NoOfTx_For_Random_Generation > 0

						-------------------------------------------
						-- Set Previous Months																			-- Rule 7c
						-------------------------------------------
						IF @Period_In_Month_Seq_For_Random_Generation <> 0 AND @Period_In_Month_NoOfTx_For_Random_Generation <> 0 
							SET @Extended_Period_In_Month = @Period_In_Month_Seq_For_Random_Generation - 1
						ELSE
							SET @Extended_Period_In_Month = 6

						-------------------------------------------
						-- Get All Transaction in Previous Months
						-------------------------------------------
						INSERT #02_PostPaymentCheck_ResultTable(Col01,Col02,Col03,Col04)
							SELECT
								VT.SP_ID,
								VT.Transaction_ID,
								VT.Practice_Display_Seq,
								@ConstantPreviousMonth
							FROM 
								VoucherTransaction VT
									LEFT JOIN @tblProfessional tblProf
										ON LTRIM(RTRIM(VT.Service_Type)) = tblProf.Service_Category_Code
									LEFT JOIN @tblRecordStatus tblRS
										ON LTRIM(RTRIM(VT.Record_Status)) = tblRS.Record_Status
									LEFT JOIN @tblSchemeCode tblSC
										ON LTRIM(RTRIM(VT.Scheme_Code)) = LTRIM(RTRIM(tblSC.Scheme_Code))
							WHERE
								((@IN_dtmTransactionDateStart IS NULL)
									OR (Transaction_Dtm >= DATEADD(MM,@Extended_Period_In_Month*-1,@IN_dtmTransactionDateStart) AND Transaction_Dtm < @IN_dtmTransactionDateStart))
								AND ((@IN_dtmServiceDateStart IS NULL)
									OR (Service_Receive_Dtm >= DATEADD(MM,@Extended_Period_In_Month*-1,@IN_dtmServiceDateStart) AND Service_Receive_Dtm < @IN_dtmServiceDateStart))
								AND (@IN_bitIncludeBackOfficeClaim IS NULL
									OR @IN_bitIncludeBackOfficeClaim = 1
									OR (@IN_bitIncludeBackOfficeClaim = 0 AND (VT.Manual_Reimburse IS NULL OR VT.Manual_Reimburse <> 'Y')))
								AND (@IN_bitIncludeInvalidatedClaim IS NULL 
									OR @IN_bitIncludeInvalidatedClaim = 1
									OR (@IN_bitIncludeInvalidatedClaim = 0 AND (Invalidation IS NULL OR Invalidation NOT IN ('I','P'))))
								AND (@chrIsAllProfessional = 'Y' OR tblProf.Service_Category_Code IS NOT NULL)
								AND (@chrIsAllRecordStatus = 'Y' OR tblRS.Record_Status IS NOT NULL)
								AND (@chrIsAllSchemeCode = 'Y' OR tblSC.Scheme_Code IS NOT NULL)
								AND VT.SP_ID = @RT02_SP_ID
								AND VT.Practice_Display_Seq = @RT02_Practice_Display_Seq
								AND NOT EXISTS
									(SELECT Col02 FROM #02_PostPaymentCheck_ResultTable WHERE Col02 = VT.Transaction_ID)
							ORDER BY Transaction_Dtm DESC

						------------------------------------------------------------------
						-- Randomly Selection of Transaction in the Last of Previous Months
						------------------------------------------------------------------
						IF @Period_In_Month_Seq_For_Random_Generation <> 0 AND @Period_In_Month_NoOfTx_For_Random_Generation <> 0 
						BEGIN
							INSERT #02_PostPaymentCheck_ResultTable(Col01,Col02,Col03,Col04)
								SELECT TOP (@Period_In_Month_NoOfTx_For_Random_Generation)
									VT.SP_ID,
									VT.Transaction_ID,
									VT.Practice_Display_Seq,
									@ConstantPreviousMonth
								FROM 
									VoucherTransaction VT
										LEFT JOIN @tblProfessional tblProf
											ON LTRIM(RTRIM(VT.Service_Type)) = tblProf.Service_Category_Code
										LEFT JOIN @tblRecordStatus tblRS
											ON LTRIM(RTRIM(VT.Record_Status)) = tblRS.Record_Status
										LEFT JOIN @tblSchemeCode tblSC
											ON LTRIM(RTRIM(VT.Scheme_Code)) = LTRIM(RTRIM(tblSC.Scheme_Code))
								WHERE
									((@IN_dtmTransactionDateStart IS NULL)
										OR (Transaction_Dtm >= DATEADD(MM,@Period_In_Month_Seq_For_Random_Generation*-1,@IN_dtmTransactionDateStart) AND Transaction_Dtm < DATEADD(MM,(@Period_In_Month_Seq_For_Random_Generation-1)*-1,@IN_dtmTransactionDateStart)))
									AND ((@IN_dtmServiceDateStart IS NULL)
										OR (Service_Receive_Dtm >= DATEADD(MM,@Period_In_Month_Seq_For_Random_Generation*-1,@IN_dtmServiceDateStart) AND Service_Receive_Dtm < DATEADD(MM,(@Period_In_Month_Seq_For_Random_Generation-1)*-1,@IN_dtmServiceDateStart)))
									AND (@IN_bitIncludeBackOfficeClaim IS NULL
										OR @IN_bitIncludeBackOfficeClaim = 1
										OR (@IN_bitIncludeBackOfficeClaim = 0 AND (VT.Manual_Reimburse IS NULL OR VT.Manual_Reimburse <> 'Y')))
									AND (@IN_bitIncludeInvalidatedClaim IS NULL 
										OR @IN_bitIncludeInvalidatedClaim = 1
										OR (@IN_bitIncludeInvalidatedClaim = 0 AND (Invalidation IS NULL OR Invalidation NOT IN ('I','P'))))
									AND (@chrIsAllProfessional = 'Y' OR tblProf.Service_Category_Code IS NOT NULL)
									AND (@chrIsAllRecordStatus = 'Y' OR tblRS.Record_Status IS NOT NULL)
									AND (@chrIsAllSchemeCode = 'Y' OR tblSC.Scheme_Code IS NOT NULL)
									AND VT.SP_ID = @RT02_SP_ID
									AND VT.Practice_Display_Seq = @RT02_Practice_Display_Seq
								ORDER BY
									(CASE
										WHEN Create_By_SmartID = 'Y' THEN 2
										ELSE 1
									END),
									NEWID()																				-- Rule 7b
						END

					END 

				
				END

			FETCH NEXT FROM Transaction_Cursor INTO @RT02_SP_ID, @RT02_Practice_Display_Seq
			END

			CLOSE Transaction_Cursor
			DEALLOCATE Transaction_Cursor

		-- =============================================
		-- 5. RETURN RESULTS
		-- =============================================
		-- ---------------------------------------------
		-- To Control:
		-- ---------------------------------------------

			DECLARE @TempSPList TABLE (
				Seq		INT IDENTITY(1,1),
				SP_ID	CHAR(8)
			)

			-- ---------------------------------------------
			-- Insert Data: Criteria
			-- ---------------------------------------------
			INSERT @TempSPList (SP_ID)
				SELECT DISTINCT SP_ID FROM @tblSPList ORDER BY SP_ID

			INSERT #Control(Seq2, Col01) VALUES(3, 'D')
			INSERT #Control(Seq2, Col01) VALUES(4, 'D')
			INSERT #Control(Seq2, Col01) VALUES(5, 'D')
			INSERT #Control(Seq2, Col01) VALUES(6, 'D')
			INSERT #Control(Seq2, Col01) VALUES(7, 'D')
			INSERT #Control(Seq2, Col01) VALUES(8, 'D')
			INSERT #Control(Seq2, Col01) VALUES(9, 'D')
			INSERT #Control(Seq2, Col01) VALUES(10, 'D')
			INSERT #Control(Seq2, Col01) VALUES(11, 'D')
			INSERT #Control(Seq2, Col01) VALUES(12, 'D')

			UPDATE #Control
			SET 
				Col01 = 'R',
				Col02 = SP.SP_ID
			FROM
				#Control C
				LEFT JOIN @TempSPList SP
					ON C.Seq = SP.Seq
			WHERE SP_ID IS NOT NULL

			-- ---------------------------------------------
			-- Return Result: 
			-- ---------------------------------------------

			SELECT Seq2 AS [Sheet], Col01 AS [Action], Col02 AS [Action_Content] FROM #Control ORDER BY Seq2 DESC


		-- ---------------------------------------------
		-- To Excel Sheet (01): Content
		-- ---------------------------------------------
			---- ---------------------------------------------
			---- Insert Data: Content
			---- ---------------------------------------------

			INSERT #Content(Seq2, Col01, Col02)
			VALUES(0 ,'Sub Report ID' ,'Sub Report Name')
			
			INSERT #Content(Seq2) VALUES(1)
			INSERT #Content(Seq2) VALUES(2)
			INSERT #Content(Seq2) VALUES(3)
			INSERT #Content(Seq2) VALUES(4)
			INSERT #Content(Seq2) VALUES(5)
			INSERT #Content(Seq2) VALUES(6)
			INSERT #Content(Seq2) VALUES(7)
			INSERT #Content(Seq2) VALUES(8)
			INSERT #Content(Seq2) VALUES(9)
			INSERT #Content(Seq2) VALUES(10)

			UPDATE #Content
			SET 
				Col01 = 'PPC0002-' + RIGHT('0' + CONVERT(VARCHAR(2),C.seq2) ,2),
				Col02 = 'Randomly Generated Post Payment Check Report for ' + SP.SP_ID
			FROM
				#Content C
				LEFT JOIN @TempSPList SP
					ON C.Seq2 = SP.Seq
			WHERE SP_ID IS NOT NULL

			INSERT #Content(Seq2, Col01, Col02) VALUES('','','')

			INSERT #Content(Seq2, Col01, Col02) 
			VALUES('','Report Generation Time: ' + CONVERT(varchar(10), @current_dtm, 111) + ' ' + CONVERT(varchar(5), @current_dtm, 114),'')

			---- ---------------------------------------------
			---- Return Result: 
			---- ---------------------------------------------

			SELECT ISNULL(Col01,''), ISNULL(Col02,'') FROM #Content ORDER BY Seq, Seq2, Col01


		-- ---------------------------------------------
		-- To Excel Sheet (02): Criteria
		-- ---------------------------------------------
			-- ---------------------------------------------
			-- Prepare Data for output: Criteria
			-- ---------------------------------------------
			-- ---------------------------------------------
			-- Declaration
			-- ---------------------------------------------
			DECLARE @varProfessionList varchar(5000)
			DECLARE @varScheme varchar(5000)
			DECLARE @chrPeriod_Type varchar(50)
			DECLARE @chrPeriod_Format varchar(50)
			DECLARE @varPeriod varchar(50)
			DECLARE @varTransactionStatus varchar(5000)
			DECLARE @varIncludeBackOfficeClaims varchar(3)
			DECLARE @varIncludeInvalidationClaims varchar(3)
			DECLARE @varTargetNoOfTransaction varchar(10)
			DECLARE @varNoOfHighestAmountClaimed varchar(10)
			DECLARE @varNoOfManualInput varchar(10)
			DECLARE @varNoOfSmartICInput varchar(10)
			DECLARE @varIncludePreviousMonth varchar(3)

			-- ---------------------------------------------
			-- Initialization
			-- ---------------------------------------------

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

			IF @Period_Format = 'E'
				BEGIN
					SET @chrPeriod_Format = 'Exact Date'
					SET @varPeriod = FORMAT(@Period_From, 'dd-MM-yyyy') + ' to ' + FORMAT(@Period_To, 'dd-MM-yyyy')
				END
			ELSE IF @Period_Format = 'M'
				BEGIN
					SET @chrPeriod_Format = 'Month and Year'
					SET @varPeriod = FORMAT(@Period_From, 'MMMM, yyyy') + ' to ' + FORMAT(@Period_To, 'MMMM, yyyy')
				END

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
				SET @varIncludeBackOfficeClaims = 'Yes'
			ELSE
				SET @varIncludeBackOfficeClaims = 'No'

			IF @IncludeInvalidationClaims = 1
				SET @varIncludeInvalidationClaims = 'Yes'
			ELSE
				SET @varIncludeInvalidationClaims = 'No'

			SET @varTargetNoOfTransaction = @TargetNoOfTransaction

			IF @NoOfHighestAmountClaimed = -1 
				SET @varNoOfHighestAmountClaimed = 'N/A'
			ELSE
				SET @varNoOfHighestAmountClaimed = @NoOfHighestAmountClaimed

			IF @NoOfManualInput = -1 
				SET @varNoOfManualInput = 'N/A'
			ELSE
				SET @varNoOfManualInput = @NoOfManualInput

			IF @NoOfSmartICInput = -1 
				SET @varNoOfSmartICInput = 'N/A'
			ELSE
				SET @varNoOfSmartICInput = @NoOfSmartICInput

			IF @Period_Format = 'M'
			BEGIN
				IF @IncludePreviousMonth = 1
					SET @varIncludePreviousMonth = 'Yes'
				ELSE
					SET @varIncludePreviousMonth = 'No'
			END

			-- ---------------------------------------------
			-- Insert Data: Criteria
			-- ---------------------------------------------

			INSERT INTO #Criteria (Seq2, Col01, Col02)
			VALUES (NULL, 'Criteria', '')

			INSERT INTO #Criteria (Seq2, Col01, Col02)
			SELECT 
				NULL,
				'',
				SP_ID + ' (' +
				STUFF(
						(SELECT ', ' + CAST(Practice_Display_Seq AS VARCHAR(MAX)) 
						FROM @tblSPList 
						WHERE (SP_ID = Results.SP_ID) 
						FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)')
				,1,2,'') + ')'
			FROM @tblSPList Results
			GROUP BY SP_ID

			UPDATE #Criteria
			SET Col01 = 'Service Provider ID (Practice No.)'
			WHERE Seq = 2

			INSERT INTO #Criteria (Seq2, Col01, Col02)
			VALUES (NULL, 'Health Profession', @varProfessionList)

			INSERT INTO #Criteria (Seq2, Col01, Col02)
			VALUES (NULL, 'Scheme', @varScheme)

			INSERT INTO #Criteria (Seq2, Col01, Col02)
			VALUES (NULL, 'Type of Date', @chrPeriod_Type)

			INSERT INTO #Criteria (Seq2, Col01, Col02)
			VALUES (NULL, 'Format of Date', @chrPeriod_Format)

			INSERT INTO #Criteria (Seq2, Col01, Col02)
			VALUES (NULL, 'Date', @varPeriod)

			INSERT INTO #Criteria (Seq2, Col01, Col02)
			VALUES (NULL, 'Transaction Status', @varTransactionStatus)

			INSERT INTO #Criteria (Seq2, Col01, Col02)
			VALUES (NULL, 'Include Back Office Claim', @varIncludeBackOfficeClaims)

			INSERT INTO #Criteria (Seq2, Col01, Col02)
			VALUES (NULL, 'Include Pending Invalidation and Invalidated Claims', @varIncludeInvalidationClaims)

			INSERT INTO #Criteria (Seq2, Col01, Col02)
			VALUES (NULL, 'Targeted Number of Transaction', @varTargetNoOfTransaction)

			INSERT INTO #Criteria (Seq2, Col01, Col02)
			VALUES (NULL, 'Number of Transactions with the Highest Amount Claimed', @varNoOfHighestAmountClaimed)

			INSERT INTO #Criteria (Seq2, Col01, Col02)
			VALUES (NULL, 'Number of Manual Input', @varNoOfManualInput)

			INSERT INTO #Criteria (Seq2, Col01, Col02)
			VALUES (NULL, 'Number of Smart IC Input', @varNoOfSmartICInput)

			IF @Period_Format = 'M'
			BEGIN
				INSERT INTO #Criteria (Seq2, Col01, Col02)
				VALUES (NULL, 'If the defined period contains insufficient transaction, remainders will be selected from the previous months (up to maximum 6 months) until the target number of transaction is fulfilled', @varIncludePreviousMonth)
			END
			---- ---------------------------------------------
			---- Return Result: Criteria
			---- ---------------------------------------------

			SELECT Col01, Col02 FROM #Criteria ORDER BY Seq, Seq2, Col01

		---- -----------------------------------------------------------------
		---- To Excel Sheet (03 - 12): Randomly Generated Post Payment Check Report
		---- -----------------------------------------------------------------
		
			------------------------------------------
			-- Cursor For Loop Each Service Provider
			------------------------------------------
			SET NOCOUNT ON
			DECLARE @RT02_RESULT_SP_ID CHAR(8)

			DECLARE RT02_RESULT_Cursor CURSOR FOR 
				SELECT LTRIM(RTRIM(Col02)) FROM #Control ORDER BY Seq2

			OPEN RT02_RESULT_Cursor

			FETCH NEXT FROM RT02_RESULT_Cursor INTO @RT02_RESULT_SP_ID

			OPEN SYMMETRIC KEY sym_Key
			DECRYPTION BY ASYMMETRIC KEY asym_Key

			WHILE @@FETCH_STATUS = 0
			BEGIN
				------------------------------------------------------------------
				-- If has SPID, print the report. If not, print the empty string.
				------------------------------------------------------------------
				IF @RT02_RESULT_SP_ID IS NOT NULL
				BEGIN

					INSERT #03_PostPaymentCheck_ResultTableForEachSP(Col01)
					SELECT Col01 + ': ' + Col02 FROM #Content WHERE Seq2 = (SELECT Seq FROM #Control WHERE LTRIM(RTRIM(Col02)) = @RT02_RESULT_SP_ID)

					INSERT #03_PostPaymentCheck_ResultTableForEachSP(Col01)
					VALUES ('')

					INSERT #03_PostPaymentCheck_ResultTableForEachSP(Col01,Col03)
					VALUES ('Service Provider ID:',@RT02_RESULT_SP_ID)

					INSERT #03_PostPaymentCheck_ResultTableForEachSP(Col01,Col03)
					VALUES ('Service Provider Name:',(SELECT CONVERT(VARCHAR(MAX), DecryptByKey(Encrypt_Field2)) FROM ServiceProvider WHERE SP_ID = @RT02_RESULT_SP_ID))

					INSERT #03_PostPaymentCheck_ResultTableForEachSP(Col01,Col03)
					VALUES ('Date:', @varPeriod)

					INSERT #03_PostPaymentCheck_ResultTableForEachSP(Col01)
					VALUES ('')

					INSERT #03_PostPaymentCheck_ResultTableForEachSP(Col01,Col02,Col04,Col06,Col07)
					VALUES ('Practice No.', 'Practice Name (In English)', 'Practice Address (In English)', 'Health Profession', 'No. of Transaction Generated')

					INSERT #03_PostPaymentCheck_ResultTableForEachSP(Col01,Col02,Col04,Col06,Col07)
						SELECT
							'PracticeNo' = P.Display_Seq,
							'PracticeName' = P.Practice_Name,
							'Address' = (SELECT [dbo].[func_formatEngAddress](P.Room, P.[Floor], P.[Block], P.Building, P.District)),
							'Profession' = Pro.Service_Category_Code,
							'NoOfTransactionGenerated' = (SELECT COUNT(1) FROM #02_PostPaymentCheck_ResultTable WHERE Col01 = @RT02_RESULT_SP_ID AND Col03 = P.Display_Seq)
						FROM
							@tblSPList SP
								INNER JOIN Practice P
									ON SP.SP_ID = P.SP_ID AND SP.Practice_Display_Seq = P.Display_Seq
								INNER JOIN Professional Prof
									ON SP.SP_ID = Prof.SP_ID AND P.Professional_Seq = Prof.Professional_Seq
								INNER JOIN Profession Pro
									ON Pro.Service_Category_Code = Prof.Service_Category_Code
						WHERE
							SP.SP_ID = @RT02_RESULT_SP_ID
						ORDER BY SP.SP_ID, SP.Practice_Display_Seq

					INSERT #03_PostPaymentCheck_ResultTableForEachSP(Col01)
					VALUES ('')

					INSERT #03_PostPaymentCheck_ResultTableForEachSP(
						Col01, Col02, Col03, Col04, Col05, 
						Col06, Col07, Col08, Col09, Col10, 
						Col11, Col12, Col13, Col14, Col15, 
						Col16, Co117, Col18, Col19, Col20, 
						Col21, Col22, Col23, Col24, Col25,
						Col26, Col27)
					VALUES (
						'Order of Selection', 
						'Gender', 
						'eHealth (Subsidies) Account Name (In English)', 
						'eHealth (Subsidies) Account Name (In Chinese)', 
						'eHealth (Subsidies) Account ID / Reference No.',
						'Identity Document Type', 
						'Identity Document No.', 
						'HKIC Symbol',
						'OCSSS Checking Result', 
						'Transaction ID', 
						'Transaction Time', 
						'Service Date',
						'Scheme',
						'Subsidies',
						'Dose',
						'Category',
						'Amount Claimed ($)',
						N'Amount Claimed (¥)',
						'Conversion Rate',
						'Net Service fee charged ($)',
						N'Net Service fee charged (¥)',
						'Transaction Status',
						'Method of Retrieval',
						'Means of Input',
						'Practice No.',
						'Date of Death',
						'Date of Death Flag')

					INSERT #03_PostPaymentCheck_ResultTableForEachSP(
						Col01, Col02, Col03, Col04, Col05, 
						Col06, Col07, Col08, Col09, Col10, 
						Col11, Col12, Col13, Col14, Col15, 
						Col16, Co117, Col18, Col19, Col20, 
						Col21, Col22, Col23, Col24, Col25,
						Col26, Col27)
					SELECT
						'OrderOfSelection' = RT.RefNo,
						'Gender' = 
							CASE
								WHEN PInfo.Sex IS NULL THEN TPInfo.Sex
								ELSE PInfo.Sex
							END,
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
						'eHealth (Subsidies) Account ID / Reference No.' = 
							CASE
								WHEN PInfo.Voucher_Acc_ID IS NULL THEN [dbo].func_format_voucher_account_number('', TPInfo.Voucher_Acc_ID)
								ELSE [dbo].func_format_voucher_account_number('V', PInfo.Voucher_Acc_ID)
							END,
						'Identity Document Type' = VT.Doc_Code,
						'Identity Document No.' = 
							CASE
								WHEN PInfo.Encrypt_Field1 IS NULL 
								THEN 
									[dbo].func_mask_doc_id(VT.Doc_Code, CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field1)), CONVERT(VARCHAR(MAX), DecryptByKey(TPInfo.Encrypt_Field11)))
								ELSE
									[dbo].func_mask_doc_id(VT.Doc_Code, CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field1)), CONVERT(VARCHAR(MAX), DecryptByKey(PInfo.Encrypt_Field11)))
							END,
						'HKIC Symbol' = CASE WHEN ISNULL(SD1.Status_Description, '') = '' THEN  @Str_NA ELSE SD1.Status_Description END,
						'OCSSS Searching Result' = CASE 
							-- SP Claim and HKIC Symbol C or U
							WHEN  ISNULL(VT.Manual_Reimburse, '') = 'N' AND ISNULL(VT.HKIC_Symbol, '') IN ('C','U') THEN
								CASE 
									WHEN VT.OCSSS_Ref_Status = 'V' THEN @Str_Valid 
									WHEN VT.OCSSS_Ref_Status = 'C' THEN @Str_ConnectionFailed
									WHEN VT.OCSSS_Ref_Status = 'N' THEN @Str_ConnectionFailed
									ELSE @Str_NA
								END
							ELSE 
							-- SP Claim and HKIC Symbol A or R / VU claim / IVRS claim / old claim record
							@Str_NA                                             
							END, 
						'Transaction ID' = [dbo].func_format_system_number(RT.Transaction_ID),
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
						'Amount Claimed ($)' = CONVERT(VARCHAR(MAX),VT.Claim_Amount),
						'Amount Claimed (¥)' = 
							CASE 
								WHEN TD.Total_Amount_RMB IS NOT NULL THEN CONVERT(VARCHAR(MAX),TD.Total_Amount_RMB)
								ELSE 'N/A'
							END,
						'Conversion Rate' = 
							CASE 
								WHEN TD.ExchangeRate_Value IS NOT NULL THEN CONVERT(VARCHAR(MAX),TD.ExchangeRate_Value)
								ELSE 'N/A'
							END,
						'Net Service fee charged ($)' = 
							CASE 
								WHEN TAF_CPF.AdditionalFieldValueCode IS NOT NULL THEN TAF_CPF.AdditionalFieldValueCode
								ELSE 'N/A'
							END,
						'Net Service fee charged (¥)' = 
							CASE 
								WHEN TAF_CPFRMB.AdditionalFieldValueCode IS NOT NULL THEN TAF_CPFRMB.AdditionalFieldValueCode
								ELSE 'N/A'
							END,
						'Transaction Status' = SD.Status_Description,
						'Method of Retrieval' = RT.Method_Of_Retrieval,
						'Means of Input'=
							CASE 
								WHEN VT.Create_By_SmartID = 'Y' THEN 'Smart IC'
								ELSE 'Manual'
							END,
						'Practice No.' = RT.Practice_Display_Seq,
						'Date of Death' = 
							CASE
								WHEN PInfo.Deceased IS NOT NULL 
									THEN 
										CASE  
											WHEN PInfo.Deceased = 'Y' THEN CONVERT(VARCHAR(10),PInfo.DOD, 121) 
											ELSE 'N/A'
										END
								WHEN TPInfo.Deceased IS NOT NULL 
									THEN 
										CASE  
											WHEN TPInfo.Deceased = 'Y' THEN CONVERT(VARCHAR(10),TPInfo.DOD, 121) 
											ELSE 'N/A'
										END
								ELSE 
									'N/A'
							END,
							--CASE 
							--	WHEN DRE.DOD IS NOT NULL THEN CONVERT(VARCHAR(10),DRE.DOD, 121)
							--	ELSE 'N/A'
							--END,
						'Date of Death Flag' = 
							CASE
								WHEN PInfo.Deceased IS NOT NULL 
									THEN 
										CASE 
											WHEN PInfo.Deceased = 'Y' THEN PInfo.Exact_DOD 
											ELSE 'N/A'
										END
								WHEN TPInfo.Deceased IS NOT NULL 
									THEN 
										CASE 
											WHEN TPInfo.Deceased = 'Y' THEN TPInfo.Exact_DOD 
											ELSE 'N/A'
										END
								ELSE 
									'N/A'
							END

					FROM 
						(SELECT 
							ROW_NUMBER() OVER(PARTITION BY Col01 ORDER BY CONVERT(INT,Col03), DisplaySeq) AS 'RefNo',
							Col01 AS SP_ID,
							Col02 AS Transaction_ID,
							Col03 AS Practice_Display_Seq,
							Col04 AS Method_Of_Retrieval
						FROM #02_PostPaymentCheck_ResultTable
						WHERE Col01 = @RT02_RESULT_SP_ID
						) RT
							INNER JOIN VoucherTransaction VT
								ON RT.Transaction_ID = VT.Transaction_ID
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
							--LEFT JOIN TransactionAdditionalField TAF_CC
							--	ON VT.Transaction_ID = TAF_CC.Transaction_ID AND TAF_CC.AdditionalFieldID = 'CategoryCode'
							LEFT JOIN TransactionAdditionalField TAF_CPF
								ON VT.Transaction_ID = TAF_CPF.Transaction_ID AND TAF_CPF.AdditionalFieldID = 'CoPaymentFee'
							LEFT JOIN TransactionAdditionalField TAF_CPFRMB
								ON VT.Transaction_ID = TAF_CPFRMB.Transaction_ID AND TAF_CPFRMB.AdditionalFieldID = 'CoPaymentFeeRMB'
							LEFT JOIN ClaimCategory CC
								ON VT.Category_Code = CC.Category_Code
							INNER JOIN StatusData SD
								ON VT.Record_Status = SD.Status_Value  AND Enum_Class = 'ClaimTransStatus'
							LEFT JOIN StatusData SD1
								ON VT.HKIC_Symbol = SD1.Status_Value AND SD1.Enum_Class = 'HKICSymbol'
							--LEFT JOIN DeathRecordMatchResult DRMR
							--	ON (DRMR.EHA_Acc_ID = PInfo.Voucher_Acc_ID AND PInfo.Voucher_Acc_ID IS NOT NULL)
							--		OR (DRMR.EHA_Acc_ID = TPInfo.Voucher_Acc_ID AND TPInfo.Voucher_Acc_ID IS NOT NULL)
							--LEFT JOIN DeathRecordEntry DRE
							--	ON DRE.Encrypt_Field1 = DRMR.Encrypt_Field1
				
					ORDER BY CONVERT(INT,RT.Practice_Display_Seq), RT.RefNo

					---- ---------------------------------------------
					---- Return Result: Report
					---- ---------------------------------------------

					SELECT
						Col01, Col02, Col03, Col04, Col05, 
						Col06, Col07, Col08, Col09, Col10, 
						Col11, Col12, Col13, Col14, Col15, 
						Col16, Co117, Col18, Col19, Col20, 
						Col21, Col22, Col23, Col24, Col25,
						Col26, Col27
					FROM #03_PostPaymentCheck_ResultTableForEachSP ORDER BY DisplaySeq

					TRUNCATE TABLE #03_PostPaymentCheck_ResultTableForEachSP

				END
				ELSE
				BEGIN
					SELECT ''
				END

			FETCH NEXT FROM RT02_RESULT_Cursor INTO @RT02_RESULT_SP_ID
			END

			CLOSE SYMMETRIC KEY sym_Key

			CLOSE RT02_RESULT_Cursor
			DEALLOCATE RT02_RESULT_Cursor


		-- ---------------------------------------------
		-- To Excel Sheet (13): Remarks
		-- ---------------------------------------------
		
			-- Create Temp Table

			DECLARE @SchemeBackOffice TABLE (
				Scheme_Code		char(10),
				Display_Code	char(25),
				Display_Seq		smallint,
				Scheme_Desc		varchar(100)
			)

			DECLARE @CommonNoteStat01 varchar(1000)

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

			---- ---------------------------------------------
			---- Insert Data: Remarks
			---- ---------------------------------------------

			SET @seq = 0

			INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			VALUES (@seq, NULL, '(A) Legend', '')

			SET @Seq = @Seq + 1

			INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			VALUES (@seq, NULL, '1. Identity Document Type', '')
	
			SET @Seq = @Seq + 1

			INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			SELECT @Seq, NULL, Doc_Display_Code, Doc_Name FROM DocType ORDER BY Display_Seq

			SET @Seq = @Seq + 1

			INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			VALUES (@seq, NULL, '', '')

			SET @Seq = @Seq + 1

			INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			VALUES (@seq, NULL, '2. Profession Type', '')

			SET @seq = @seq + 1

			INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			SELECT @seq, NULL, Service_Category_Code, Service_Category_Desc FROM Profession WITH (NOLOCK)

			SET @seq = @seq + 1

			INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			VALUES (@seq, NULL, '', '')

			SET @Seq = @Seq + 1

			INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			VALUES (@seq, NULL, '3. Scheme Name', '')
	
			SET @Seq = @Seq + 1

			INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			SELECT @seq, NULL, Display_Code, Scheme_Desc FROM @SchemeBackOffice

			SET @Seq = @Seq + 1

			INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			VALUES (@seq, NULL, '', '')

			SET @Seq = @Seq + 1

			INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			VALUES (@seq, NULL, '4. Subsidy', '')
	
			SET @Seq = @Seq + 1

			INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			SELECT DISTINCT @Seq, NULL, Display_Code_For_Claim, Legend_Desc_For_Claim
			FROM SubsidizeGroupClaim a
				INNER JOIN Subsidize b ON a.Subsidize_Code = b.Subsidize_Code
				INNER JOIN Subsidizeitem c ON b.subsidize_item_Code = c.Subsidize_Item_Code AND c.Subsidize_Type = 'VACCINE'
			ORDER BY Display_Code_For_Claim

			SET @Seq = @Seq + 1

			INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			VALUES (@seq, NULL, '', '')

			SET @Seq = @Seq + 1

			INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			VALUES (@seq, NULL,'5. Date of Death Flag', '')

			SET @Seq = @Seq + 1

			INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			VALUES (@seq, NULL, 'D', 'Exact date DD/MM/YYYY')

			SET @Seq = @Seq + 1

			INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			VALUES (@seq, NULL, 'M', 'MM/YYYY')

			SET @Seq = @Seq + 1

			INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			VALUES (@seq, NULL, 'Y', 'Only year YYYY')

			SET @Seq = @Seq + 1

			INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			VALUES (@seq, NULL, '', '')

			SET @Seq = @Seq + 1

			INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			VALUES (@seq, NULL, '(B) Common Note(s) for the report', '')

			SET @Seq = @Seq + 1

			INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			VALUES (@seq, NULL, @CommonNoteStat01, '')

			SET @Seq = @Seq + 1

			INSERT INTO #Remarks (Seq, Seq2, Col01, Col02)
			SELECT @Seq, NULL, '2. All are accumulative data unless specified as below:', ''

			SET @Seq = @Seq + 1
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
	--DROP TABLE #ServiceProviderBase
	DROP TABLE #VoucherTransactionBase
	DROP TABLE #01_NoOfTransaction_ResultTable
	DROP TABLE #02_PostPaymentCheck_ResultTable
	DROP TABLE #Control
	DROP TABLE #Content
	DROP TABLE #Criteria
	DROP TABLE #Remarks
	DROP TABLE #03_PostPaymentCheck_ResultTableForEachSP

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_PPC0002_Report_get] TO HCVU
GO
