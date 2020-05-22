IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSU0011_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSU0011_Report_get]
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
-- ============================================
-- Author:			Winnie SUEN	
-- Create date:		11 Nov 2019
-- CR No.			CRE17-014 (Enhance eHSU0002)
-- Description:		Generate report for eHSU0011 
--					eHSU0002 report with additional figures include Ranking, Average Unit and Percentile
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSU0011_Report_get]
	@request_time		datetime,
	@period_type		char(1),
	@From_Date			varchar(17),
	@To_Date			varchar(17),
	@Scheme_Code		char(10),
	@ProfessionList		VARCHAR(100), --DIT,ENU,POD,RCM,RCP,RDT,RMP,RMT,RNU,ROP,ROT,RPT,RRD,SPT
	@Percentile			VARCHAR(100) = NULL --25,50,75
	

AS BEGIN

-- =============================================
-- Declaration
-- =============================================

	DECLARE @In_request_time		datetime = @request_time
	DECLARE @In_period_type			char(1) = @period_type
	DECLARE @In_From_Date			varchar(17) = @From_Date
	DECLARE @In_To_Date				varchar(17) = @To_Date
	DECLARE @In_Scheme_Code			char(10) = @Scheme_Code
	DECLARE @In_ProfessionList		VARCHAR(100) = @ProfessionList
	DECLARE @In_Percentile			VARCHAR(100) = @Percentile


	DECLARE @DateTypeFN			char(30)
	DECLARE @DisplayFromDate	varchar(17)
	DECLARE @DisplayToDate		varchar(17)

	DECLARE @status_data_enum_class_sp_status	varchar(50)

	DECLARE @period_from	datetime
	DECLARE @period_to		datetime

	DECLARE @si_scheme_code		char(10)

	DECLARE @web	varchar(5)
	DECLARE @ivrs	varchar(5)
	DECLARE @pcs	varchar(5)

	CREATE TABLE #ReportResult (
		Seq		int,
		Col01	varchar(120),
		Col02	varchar(40),
		Col03	varchar(30),
		Col04	varchar(30),
		Col05	varchar(30),
		Col06	varchar(30),
		Col07	varchar(30),
		Col08	varchar(30),
		Col09	varchar(30),
		Col10	varchar(30),
		Col11	varchar(30),
		Col12	varchar(30),
		Col13	varchar(30),
		Col14	varchar(30),
		Col15	varchar(30),
		Col16	varchar(30),
		Col17	varchar(30),
		Col18	varchar(30),
		Col19	varchar(30),
		Col20	varchar(30),
		Col21	varchar(30),
		Col22	varchar(30),
		Col23	varchar(30),
		Col24	varchar(30),
		Col25	varchar(30)
	)

	-- Create Worksheet Criteria Table
	DECLARE @WSCriteria AS TABLE(
		Seq		INT,
		Col01	VARCHAR(1000),
		Col02	VARCHAR(1000)
	)


	--Get profession list  
	DECLARE @tbl_Profession TABLE(
		Service_Category_Code  VARCHAR(3)
	)
		
	DECLARE @PercentileTable  TABLE(
		Pct_Group		INT IDENTITY(1,1),
		Pct_Value		INT
	)

	CREATE TABLE #ReportDataPercetile(
		SP_ID				VARCHAR(8),
		Service_Type		VARCHAR(5),
		Pct_Group			INT,
		Pct_Value			INT,
		Low_Median_Value	MONEY,
		High_Median_Value	MONEY,
		Median_Value		MONEY
	)
-- =============================================
-- Validation
-- =============================================

	IF @In_period_type IS NULL
		RETURN

	IF @In_period_type <> 'T' AND @In_period_type <> 'S'
		RETURN

	IF @In_From_Date IS NULL OR @In_To_Date IS NULL
		RETURN

	IF @In_From_Date = '' OR @In_To_Date = ''
		RETURN

	SET @period_from = @In_From_Date
	SET @period_to = @In_To_Date

	IF @period_from > @period_to
		RETURN

	IF @In_Scheme_Code IS NULL
		RETURN

	IF @In_Scheme_Code = ''
		RETURN

	IF @In_ProfessionList = ''
		SET @In_ProfessionList = NULL

	IF @In_Percentile = ''
		SET @In_Percentile = NULL

-- =============================================
-- Initialization
-- =============================================

	SET @status_data_enum_class_sp_status = 'ServiceProviderStatus'

	IF @In_Scheme_Code = 'EVSS'
		SET @si_scheme_code = 'EVSSHSIVSS'
	ELSE
		SET @si_scheme_code = @In_Scheme_Code

	SET @web = 'WEB'
	SET @ivrs = 'IVRS'
	SET @pcs = 'PCS'


	--Set Date type full name
	SET @DateTypeFN =	CASE @In_period_type 
							WHEN 'T' THEN 'Transaction Date'
							WHEN 'S' THEN 'Service Date'
						END

	SELECT @DisplayFromDate = FORMAT(@period_from, 'yyyy/MM/dd' , 'en-US')
	SELECT @DisplayToDate = FORMAT(@period_to, 'yyyy/MM/dd' , 'en-US')


	-- Prepare profession list string
	DECLARE @HealthProfessionList VARCHAR(8000) 
	SELECT @HealthProfessionList = COALESCE(@HealthProfessionList + ', ', '') + Service_Category_Desc 
	FROM (
		SELECT	rtrim(p.Service_Category_Desc) + ' (' + rtrim(p.Service_Category_Code) + ')' as Service_Category_Desc
		FROM Profession p with(nolock)
			 INNER JOIN @tbl_Profession i
			 ON p.Service_Category_Code = i.Service_Category_Code collate DATABASE_DEFAULT 
	) a
	ORDER BY Service_Category_Desc

	IF @HealthProfessionList IS NULL 
	BEGIN
		SET @HealthProfessionList  = 'Any'
	END

	
	-- Set Percentile Table
	INSERT INTO @PercentileTable (Pct_Value)
	SELECT Item FROM func_split_string(@In_Percentile, ',') 

	
-- ---------------------------------------------
-- For Excel Sheet: Criteria
-- ---------------------------------------------

	INSERT INTO @WSCriteria (Seq, Col01, Col02)
	VALUES (1, 'Criteria', '')

	INSERT INTO @WSCriteria (Seq, Col01, Col02)
	VALUES (2, 'Type of Date', @DateTypeFN)

	INSERT INTO @WSCriteria (Seq, Col01, Col02)
	VALUES (3, 'Date', @DisplayFromDate +' to '+ @DisplayToDate)

	INSERT INTO @WSCriteria (Seq, Col01, Col02)
	VALUES (4, 'Scheme', @In_Scheme_Code)

	INSERT INTO @WSCriteria (Seq, Col01, Col02)
	VALUES (5, 'Health Profession' , rtrim(@HealthProfessionList))    

	INSERT INTO @WSCriteria (Seq, Col01, Col02)
	VALUES (6, 'Percentile (Units) (%)' , ISNULL(@In_Percentile, 'N/A')) 


-- ---------------------------------------------
-- For Excel Sheet: 01
-- ---------------------------------------------
	-- Create Header Information
	INSERT INTO #ReportResult (Seq, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16, Col17, Col18, Col19, Col20, Col21)
	VALUES (0, 'eHS(S)U0002-01: Statistics of transactions and units of EHCP from ' + CONVERT(varchar(10), @period_from, 20) + ' to ' + CONVERT(varchar(10), @period_to, 20) + ' broken down by means of input', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '')

	INSERT INTO #ReportResult (Seq, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16, Col17, Col18, Col19, Col20, Col21)
	VALUES (1, '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '')

	INSERT INTO #ReportResult (Seq, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16, Col17, Col18, Col19, Col20, Col21)
	VALUES (2, '', '', '', '', 'Internet (By Manual)', '', 'Internet (By Smart IC)', '', 'IVRS', '', 'PCS', '', 'Total No. of transactions', '', 'Total No. of units', '', 'Average units per transaction', '', 'Percentile 1 (Units)', 'Percentile 2 (Units)', 'Percentile 3 (Units)')

	INSERT INTO #ReportResult (Seq, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16, Col17, Col18, Col19, Col20, Col21)
	VALUES (3, 'SPID', LTRIM(RTRIM(@In_Scheme_Code)) + ' enrolment effective date', 'Profession', 'Status', 'No. of transactions', 'No. of units', 'No. of transactions', 'No. of units', 'No. of transactions', 'No. of units', 'No. of transactions', 'No. of units', 'Transactions', 'Rank', 'Units', 'Rank', 'Average', 'Rank', 'N/A','N/A','N/A')

	-- Percentile
	UPDATE R
	SET	R.Col19 = CONVERT(VARCHAR(10), Pct_Value) + '%'
	FROM #ReportResult R, @PercentileTable P
	WHERE R.Seq = 3 AND P.Pct_Group = 1

	UPDATE R
	SET	R.Col20 = CONVERT(VARCHAR(10), Pct_Value) + '%'
	FROM #ReportResult R, @PercentileTable P
	WHERE R.Seq = 3 AND P.Pct_Group = 2

	UPDATE R
	SET	R.Col21 = CONVERT(VARCHAR(10), Pct_Value) + '%'
	FROM #ReportResult R, @PercentileTable P
	WHERE R.Seq = 3 AND P.Pct_Group = 3

-- =============================================
-- Retrieve Data
-- =============================================

-- ---------------------------------------------
-- Get Raw Data by Transaction  
-- ---------------------------------------------

	SELECT
		VT.SP_ID,
		SI.Effective_Dtm,
		VT.Service_Type,
		SD.Status_Description,
		VT.Transaction_ID,
		SUM(TD.Unit) AS [No_Of_Unit],
		ISNULL(VT.Create_By_SmartID, 'N') AS [Create_By_SmartID],
		CASE VT.SourceApp
			WHEN 'IVRS' THEN @ivrs
			WHEN 'ExternalWS' THEN @pcs
			ELSE @web
		END AS [SourceApp]

	INTO
		#ReportRawData

	FROM
		VoucherTransaction VT WITH (NOLOCK)
			INNER JOIN TransactionDetail TD WITH (NOLOCK)
				ON VT.Transaction_ID = TD.Transaction_ID
			INNER JOIN ServiceProvider SP WITH (NOLOCK)
				ON VT.SP_ID = SP.SP_ID
			INNER JOIN SchemeInformation SI WITH (NOLOCK)
				ON VT.SP_ID = SI.SP_ID
					AND SI.Scheme_Code = @si_scheme_code
			INNER JOIN StatusData SD WITH (NOLOCK)
				ON SP.Record_Status = SD.Status_Value
					AND SD.Enum_Class = @status_data_enum_class_sp_status

	WHERE
		(VT.Record_Status <> 'I' AND VT.Record_Status <> 'D')
			AND ((@In_period_type = 'T' AND VT.Transaction_Dtm >= @period_from AND VT.Transaction_Dtm < DATEADD(dd, 1, @period_to))
				OR (@In_period_type = 'S' AND VT.Service_Receive_Dtm >= @period_from AND VT.Service_Receive_Dtm < DATEADD(dd, 1, @period_to)))
			AND (VT.Scheme_Code = @In_Scheme_Code)
			AND (ISNULL(VT.Invalidation, '') <> 'I')
			AND (@In_ProfessionList IS NULL OR EXISTS (SELECT * FROM func_split_string(@In_ProfessionList, ',') WHERE Item = VT.Service_Type))

	GROUP BY
		VT.SP_ID,
		SI.Effective_Dtm,
		VT.Service_Type,
		SD.Status_Description,
		VT.Transaction_ID,
		ISNULL(VT.Create_By_SmartID, 'N'),
		CASE VT.SourceApp
			WHEN 'IVRS' THEN @ivrs
			WHEN 'ExternalWS' THEN @pcs
			ELSE @web
		END

-- ---------------------------------------------
-- Get Data by Service Provider  
-- ---------------------------------------------

	SELECT
		SP_ID,
		Effective_Dtm,
		Service_Type,
		Status_Description,
		COUNT(Transaction_ID) AS [No_Of_Transaction],
		SUM(No_Of_Unit) AS [No_Of_Unit],
		Create_By_SmartID,
		SourceApp

	INTO
		#ReportData

	FROM
		#ReportRawData

	GROUP BY
		SP_ID,
		Effective_Dtm,
		Service_Type,
		Status_Description,
		Create_By_SmartID,
		SourceApp

	
	-- Total
	SELECT
		SP_ID,
		Effective_Dtm,
		Service_Type,
		Status_Description,
		SUM([No_Of_Transaction]) AS [No_Of_Transaction],
		SUM([No_Of_Unit]) AS [No_Of_Unit]
	INTO
		#ReportDataTotal

	FROM
		#ReportData

	GROUP BY
		SP_ID,
		Effective_Dtm,
		Service_Type,
		Status_Description



	-- Percentile	
	DECLARE @PValue AS INT
	DECLARE @PGrp AS INT

	IF @In_Percentile IS NOT NULL
	BEGIN
		DECLARE Pct_cursor CURSOR FOR  
		SELECT Pct_Group, Pct_Value FROM @PercentileTable

		OPEN Pct_cursor;  
		FETCH NEXT FROM Pct_cursor INTO @PGrp, @PValue
  
		WHILE @@FETCH_STATUS = 0  
		BEGIN  

			IF (@PValue = 0)
			BEGIN
				-- Example:	Percentile = 0%
				-- Get Min value
				INSERT INTO #ReportDataPercetile (SP_ID, Service_Type, Pct_Group, Pct_Value, Low_Median_Value, High_Median_Value)
				SELECT SP_ID, Service_Type, @PGrp, @PValue , MIN([No_Of_Unit]), MIN([No_Of_Unit])
				FROM dbo.#ReportRawData
				GROUP BY 
					SP_ID, Service_Type
			END

			ELSE IF (@PValue = 100)
			BEGIN
				-- Example:	Percentile = 100%
				-- Get Max value
				INSERT INTO #ReportDataPercetile (SP_ID, Service_Type, Pct_Group, Pct_Value, Low_Median_Value, High_Median_Value)
				SELECT SP_ID, Service_Type, @PGrp, @PValue , MAX([No_Of_Unit]), MAX([No_Of_Unit])
				FROM dbo.#ReportRawData
				GROUP BY 
					SP_ID, Service_Type
			END

			ELSE 
			BEGIN	
				-- Example:	Percentile = 25% 
				--	Low: Get Max value of Top 25% order by ASC
				--	High: Get Min value of Top 75% order by DESC
				DECLARE @PV_Low AS INT
				DECLARE @PV_High AS INT

				SET @PV_Low = @PValue
				SET @PV_High = 100 - @PValue

				INSERT INTO #ReportDataPercetile (SP_ID, Service_Type, Pct_Group, Pct_Value, Low_Median_Value, High_Median_Value)
				SELECT SP_ID, Service_Type, @PGrp, @PValue , MAX(T1.[No_Of_Unit]), MIN(T2.[No_Of_Unit])
				FROM dbo.#ReportDataTotal RDT
				-- Low Median
				CROSS APPLY		
					(	SELECT TOP (@PV_Low) PERCENT [No_Of_Unit]
						FROM #ReportRawData RD
						WHERE RD.SP_ID = RDT.SP_ID AND RD.Service_Type = RDT.Service_Type
						ORDER BY [No_Of_Unit]
					) AS T1
				-- High Median
				CROSS APPLY		
					(	SELECT TOP (@PV_High) PERCENT [No_Of_Unit]
						FROM #ReportRawData RD
						WHERE RD.SP_ID = RDT.SP_ID AND RD.Service_Type = RDT.Service_Type
						ORDER BY [No_Of_Unit] DESC
					) AS T2

				GROUP BY 
					SP_ID, Service_Type
			END

			FETCH NEXT FROM Pct_cursor INTO @PGrp, @PValue
		END  
  
		CLOSE Pct_cursor;  
		DEALLOCATE Pct_cursor;   

	END

	-- Get Median
	UPDATE
		#ReportDataPercetile
	SET
		Median_Value = ISNULL((Low_Median_Value + High_Median_Value) / 2.0, 0)


-- =============================================
-- Return results
-- =============================================

-- ---------------------------------------------
-- Create Report Result of Excel Sheet: 01  
-- ---------------------------------------------

	INSERT INTO #ReportResult (Seq, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16, Col17, Col18, Col19, Col20, Col21)
	SELECT
		4,
		LTRIM(RTRIM(RD.SP_ID)),
		CONVERT(varchar(10), RD.Effective_Dtm, 20),
		LTRIM(RTRIM(RD.Service_Type)),
		RD.Status_Description,
		CONVERT(varchar(10), ISNULL(RD1.No_Of_Transaction, 0)),
		CONVERT(varchar(10), ISNULL(RD1.No_Of_Unit, 0)),
		CONVERT(varchar(10), ISNULL(RD2.No_Of_Transaction, 0)),
		CONVERT(varchar(10), ISNULL(RD2.No_Of_Unit, 0)),
		CONVERT(varchar(10), ISNULL(RD3.No_Of_Transaction, 0)),
		CONVERT(varchar(10), ISNULL(RD3.No_Of_Unit, 0)),
		CONVERT(varchar(10), ISNULL(RD4.No_Of_Transaction, 0)),
		CONVERT(varchar(10), ISNULL(RD4.No_Of_Unit, 0)),
		CONVERT(varchar(10), ISNULL(RDT.No_Of_Transaction, 0)),
		CONVERT(varchar(10), DENSE_RANK () OVER (ORDER BY ISNULL(RDT.No_Of_Transaction,0) DESC)) TotalTransaction_Rank,
		CONVERT(varchar(10), ISNULL(RDT.No_Of_Unit, 0)),
		CONVERT(varchar(10), DENSE_RANK () OVER (ORDER BY ISNULL(RDT.No_Of_Unit,0) DESC)) TotalUnit_Rank,
		CASE WHEN ISNULL(RDT.No_Of_Transaction, 0) = 0 
				THEN 'N/A' 
				ELSE FORMAT( CONVERT(DECIMAL(10,1), ISNULL(RDT.No_Of_Unit, 0) * 1.0 / RDT.No_Of_Transaction), '#.#')
			END Average_Unit,
		CONVERT(varchar(10), DENSE_RANK () OVER (ORDER BY 
														CASE WHEN RDT.No_Of_Transaction IS NULL THEN 0 
														ELSE CONVERT(DECIMAL(10,1), ISNULL(RDT.No_Of_Unit, 0) * 1.0 / RDT.No_Of_Transaction)
														END 
												 DESC)) Average_Unit_Rank,

		-- Percentile
		CASE WHEN P1.Pct_Value IS NULL THEN 'N/A' ELSE FORMAT(ISNULL(P1.Median_Value, 0), '#.#') END,
		CASE WHEN P2.Pct_Value IS NULL THEN 'N/A' ELSE FORMAT(ISNULL(P2.Median_Value, 0), '#.#') END,
		CASE WHEN P3.Pct_Value IS NULL THEN 'N/A' ELSE FORMAT(ISNULL(P3.Median_Value, 0), '#.#') END
	FROM
		(SELECT DISTINCT SP_ID, Effective_Dtm, Service_Type, Status_Description FROM #ReportData WITH (NOLOCK)) RD
		LEFT JOIN #ReportData RD1 WITH (NOLOCK)
			ON RD.SP_ID = RD1.SP_ID
				AND RD.Service_Type = RD1.Service_Type
				AND RD1.Create_By_SmartID = 'N'
				AND RD1.SourceApp = @web
		LEFT JOIN #ReportData RD2 WITH (NOLOCK)
			ON RD.SP_ID = RD2.SP_ID
				AND RD.Service_Type = RD2.Service_Type
				AND RD2.Create_By_SmartID = 'Y'
				AND RD2.SourceApp = @web
		LEFT JOIN #ReportData RD3 WITH (NOLOCK)
			ON RD.SP_ID = RD3.SP_ID
				AND RD.Service_Type = RD3.Service_Type
				AND RD3.SourceApp = @ivrs
		LEFT JOIN #ReportData RD4 WITH (NOLOCK)
			ON RD.SP_ID = RD4.SP_ID
				AND RD.Service_Type = RD4.Service_Type
				AND RD4.SourceApp = @pcs
		LEFT JOIN #ReportDataTotal RDT WITH (NOLOCK)
			ON RD.SP_ID = RDT.SP_ID
				AND RD.Service_Type = RDT.Service_Type
		LEFT JOIN #ReportDataPercetile P1 WITH (NOLOCK)
			ON RD.SP_ID = P1.SP_ID
				AND RD.Service_Type = P1.Service_Type
				AND P1.Pct_Group = 1
		LEFT JOIN #ReportDataPercetile P2 WITH (NOLOCK)
			ON RD.SP_ID = P2.SP_ID
				AND RD.Service_Type = P2.Service_Type
				AND P2.Pct_Group = 2
		LEFT JOIN #ReportDataPercetile P3 WITH (NOLOCK)
			ON RD.SP_ID = P3.SP_ID
				AND RD.Service_Type = P3.Service_Type
				AND P3.Pct_Group = 3


-- ---------------------------------------------  
-- To Excel Sheet: Content  
-- --------------------------------------------- 

	SELECT 'Report Generation Time: ' + CONVERT(varchar(10), GETDATE(), 111) + ' ' + CONVERT(varchar(5), GETDATE(), 114)

-- ---------------------------------------------  
-- To Excel Sheet: Criteria
-- --------------------------------------------- 
	SELECT
		Col01,
		Col02
	FROM @WSCriteria
	ORDER BY Seq

-- ---------------------------------------------  
-- To Excel Sheet: 01  
-- --------------------------------------------- 

	SELECT
		Col01,
		Col02,
		Col03,
		Col04,
		Col05,
		Col06,
		Col07,
		Col08,
		Col09,
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
		Col20,
		Col21
	FROM #ReportResult
	ORDER BY Seq, Col01, Col03

-- --------------------------------------------------  
-- To Excel sheet:   eHSM0011-Remarks: Remarks  
-- --------------------------------------------------  
	DECLARE @tblRemark AS TABLE (
		Seq	INT identity(1,1),
		Result_Value1 NVARCHAR(MAX),    
		Result_Value2 NVARCHAR(MAX)  
	)

-- Lengend

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '(A) Legend', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '1.Health Profession', ''
	
	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT Service_Category_Code, Service_Category_Desc 
	FROM Profession
	ORDER BY Service_Category_Code


	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '2.Scheme', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT Display_Code, Scheme_Desc
	FROM SchemeClaim WITH (NOLOCK)
	WHERE Record_Status = 'A' AND Scheme_Seq = 1
	ORDER BY Scheme_Code

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '', ''

	-- Common Note

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '(B) Common Note(s) for the report', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '1. Enrolment of optometrists to HCVS since Dec 2011', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '2. Transactions:', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '   a. Exclude those reimbursed transactions with invalidation status marked as Invalidated.', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '   b. Exclude voided/deleted transactions.', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '3. Means of input', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '    Internet (By Manual)', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '       Input by manual input personal information in Internet web platform (all doc. types)', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '    Internet (By Smart IC)', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '       Use smart IC to read personal information in smart IC via Internet web platform', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '    IVRS', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '      Input data via IVRS', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '    PCS', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '       Input data via Private Clinic Solution (PCS) [obsoleted in May 2014]', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '4. No. of unit', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '    Voucher schemes:  Claim amount', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '    Vaccination schemes: No. of subsidy in the transaction', ''

	SELECT Result_Value1, Result_Value2
	FROM @tblRemark
	ORDER BY Seq


	--SELECT * FROM #ReportDataPercetile order by SP_ID, Service_Type, Pct_Group
	--SELECT * from #ReportRawData order by SP_ID, No_Of_Unit

	DROP TABLE #ReportResult
	DROP TABLE #ReportData
	DROP TABLE #ReportDataTotal
	DROP TABLE #ReportDataPercetile

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSU0011_Report_get] TO HCVU
GO
