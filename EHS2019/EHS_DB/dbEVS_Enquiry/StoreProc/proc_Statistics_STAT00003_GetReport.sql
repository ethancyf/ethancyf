IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Statistics_STAT00003_GetReport]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Statistics_STAT00003_GetReport]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	21 Apr 2015
-- Description:		1. Refine District Structure
-- =============================================
-- =============================================  
-- Modification History  
-- CR No.:			INT13-0011
-- Modified by:		Koala CHENG  
-- Modified date:	14 May 2013  
-- Description:		Fix database collation problem
-- =============================================  
-- =============================================
-- Author:			Tommy Lam
-- Create date:		20 Nov 2012
-- Description:		Get Report for Statistics - STAT00003
-- =============================================

CREATE PROCEDURE [dbo].[proc_Statistics_STAT00003_GetReport]
	@statistic_row_type		char(1),
	@statistic_col_type		char(1),
	@period_type			char(1),
	@period_from			datetime,
	@period_to				datetime,
	@scheme_code			char(10),
	@statistics_unit		char(1),
	@subsidize_display_code	char(25),
	@transaction_method		varchar(5000),
	@category_filter		varchar(5000)
AS BEGIN

-- =============================================
-- Declaration
-- =============================================

	DECLARE @grand_total	varchar(12)

	DECLARE @data_count		int

	DECLARE @earliest_period_from	datetime
	DECLARE @latest_period_to		datetime
	DECLARE @temp_period			varchar(10)

	CREATE TABLE #VoucherTransaction (
		Transaction_Period		varchar(10) COLLATE Chinese_Taiwan_Stroke_CI_AS,
		Transaction_ID			char(20) COLLATE Chinese_Taiwan_Stroke_CI_AS,
		Total_Unit				int,
		Service_Category		varchar(15) COLLATE Chinese_Taiwan_Stroke_CI_AS
	)

	CREATE TABLE #PivotTable (
		Seq		int,
		Period	varchar(10) COLLATE Chinese_Taiwan_Stroke_CI_AS
	)

	CREATE TABLE #PivotTableTemp (
		Seq		int,
		Period	varchar(10) COLLATE Chinese_Taiwan_Stroke_CI_AS
	)

	CREATE TABLE #PivotTableRow (
		Row_Name	varchar(30) COLLATE Chinese_Taiwan_Stroke_CI_AS
	)

	DECLARE @PivotTableColumn table (
		Column_Name	varchar(30) COLLATE Chinese_Taiwan_Stroke_CI_AS
	)

	DECLARE @pivot_table_column_name		varchar(MAX)
	DECLARE @pivot_table_column_value		varchar(MAX)
	DECLARE @pivot_table_column_name_alias	varchar(MAX)

	DECLARE @pivot_table_column_total				varchar(MAX)
	DECLARE @pivot_table_column_total_with_null		varchar(MAX)
	DECLARE @pivot_table_row_total					varchar(MAX)

	DECLARE @pivot_table_column_header			varchar(MAX)

	DECLARE @sql_script						varchar(MAX)
	DECLARE @sql_script_pivot_table_insert	varchar(MAX)

-- =============================================
-- Validation
-- =============================================

	IF @statistic_row_type IS NULL OR @statistic_col_type IS NULL
		RETURN

	IF @statistic_row_type <> 'Y' AND @statistic_row_type <> 'M' AND @statistic_row_type <> 'D'
		RETURN

	IF @statistic_col_type <> 'D' AND @statistic_col_type <> 'P'
		RETURN

	IF @period_type IS NULL
		RETURN

	IF @period_type <> 'T' AND @period_type <> 'S'
		RETURN

	IF @period_from = ''
		SET @period_from = NULL

	IF @period_to = ''
		SET @period_to = NULL

	IF @period_from > @period_to AND (NOT(@period_from IS NULL)) AND (NOT(@period_to IS NULL))
		RETURN

	IF @scheme_code IS NULL
		SET @scheme_code = ''

	IF @subsidize_display_code IS NULL
		SET @subsidize_display_code = ''

	IF @statistics_unit IS NULL
		RETURN

	IF @statistics_unit <> 'T' AND @statistics_unit <> 'S'
		RETURN

	IF @transaction_method IS NULL
		SET @transaction_method = ''

	IF @category_filter IS NULL
		SET @category_filter = ''

-- =============================================
-- Initialization
-- =============================================

	SET @grand_total = 'Total'

	-- Get Column for District
	IF @statistic_col_type = 'D'
		BEGIN

			INSERT INTO @PivotTableColumn (
				Column_Name
			)
			SELECT district_board_shortname_SD
			FROM DistrictBoard
			WHERE area_code <> '4'
			ORDER BY Display_Seq

		END

	-- Get Column for Profession
	ELSE IF @statistic_col_type = 'P'
		BEGIN

			INSERT INTO @PivotTableColumn (
				Column_Name
			)
			EXECUTE proc_SchemeBackOffice_GetEligibleProfessional
				@scheme_code,
				@period_from,
				@period_to

		END

	IF (SELECT COUNT(1) FROM @PivotTableColumn) < 1
		RETURN

	-- Prepare Column List for Dynamic SQL String
	SELECT
		@pivot_table_column_name = COALESCE(@pivot_table_column_name + ',[' + LTRIM(RTRIM(Column_Name)) + ']', '[' + LTRIM(RTRIM(Column_Name)) + ']'),
		@pivot_table_column_value = COALESCE(@pivot_table_column_value + ',ISNULL([' + LTRIM(RTRIM(Column_Name)) + '],0)', 'ISNULL([' + LTRIM(RTRIM(Column_Name)) + '],0)'),
		@pivot_table_column_name_alias = COALESCE(@pivot_table_column_name_alias + ',ISNULL(PTT.[' + LTRIM(RTRIM(Column_Name)) + '],0)', 'ISNULL(PTT.[' + LTRIM(RTRIM(Column_Name)) + '],0)'),
		@pivot_table_column_total = COALESCE(@pivot_table_column_total + '+[' + LTRIM(RTRIM(Column_Name)) + ']', '[' + LTRIM(RTRIM(Column_Name)) + ']'),
		@pivot_table_column_total_with_null = COALESCE(@pivot_table_column_total_with_null + '+ISNULL([' + LTRIM(RTRIM(Column_Name)) + '],0)', 'ISNULL([' + LTRIM(RTRIM(Column_Name)) + '],0)'),
		@pivot_table_row_total = COALESCE(@pivot_table_row_total + ',ISNULL(SUM([' + LTRIM(RTRIM(Column_Name)) + ']),0)', 'ISNULL(SUM([' + LTRIM(RTRIM(Column_Name)) + ']),0)'),
		@pivot_table_column_header = COALESCE(@pivot_table_column_header + ',[' + LTRIM(RTRIM(Column_Name)) + '] int', '[' + LTRIM(RTRIM(Column_Name)) + '] int')
	FROM @PivotTableColumn

	SET @pivot_table_column_name_alias = @pivot_table_column_name_alias + ',ISNULL(PTT.[' + @grand_total + '],0)'
	SET @pivot_table_row_total = @pivot_table_row_total + ',ISNULL(SUM([' + @grand_total + ']),0)'
	SET @pivot_table_column_header = @pivot_table_column_header + ',[' + @grand_total + '] int'

	-- Add Column for Result dynamically
	EXECUTE ('ALTER TABLE #PivotTable ADD ' + @pivot_table_column_header)
	EXECUTE ('ALTER TABLE #PivotTableTemp ADD ' + @pivot_table_column_header)

	SET @sql_script_pivot_table_insert = ' ([Seq],[Period],' + @pivot_table_column_name + ',[' + @grand_total + ']) '

-- =============================================
-- Retrieve Data
-- =============================================

	-- Get Data
	INSERT INTO #VoucherTransaction (
		Transaction_Period,
		Transaction_ID,
		Total_Unit,
		Service_Category
	)
	EXECUTE proc_Statistics_STAT00003_GetData
		@statistic_row_type,
		@statistic_col_type,
		@period_type,
		@period_from,
		@period_to,
		@scheme_code,
		@statistics_unit,
		@subsidize_display_code,
		@transaction_method,
		@category_filter

	-- Count Data
	SELECT @data_count = COUNT(1) FROM #VoucherTransaction

	-- Pivot Table for Transaction
	IF @statistics_unit = 'T'
		BEGIN

			SET @sql_script = 'SELECT 0, *, ' + @pivot_table_column_total + '
				FROM (
					SELECT
						Transaction_Period,
						Transaction_ID,
						Service_Category
					FROM
						#VoucherTransaction
				) DATA
				PIVOT (
					COUNT(Transaction_ID)
					FOR Service_Category
					IN (' + @pivot_table_column_name + ')
				) FUNC'

		END

	-- Pivot Table for Item of Transaction
	ELSE IF @statistics_unit = 'S'
		BEGIN

			SET @sql_script = 'SELECT 0, [Transaction_Period], ' + @pivot_table_column_value + ',' + @pivot_table_column_total_with_null + '
				FROM (
					SELECT
						Transaction_Period,
						Total_Unit,
						Service_Category
					FROM
						#VoucherTransaction
				) DATA
				PIVOT (
					SUM(Total_Unit)
					FOR Service_Category
					IN (' + @pivot_table_column_name + ')
				) FUNC'

		END

	-- Save Pivot Table
	SET @sql_script = 'INSERT INTO #PivotTableTemp' + @sql_script_pivot_table_insert + @sql_script
	EXECUTE (@sql_script)

	-- Get All Period for Row of Result
	SET @earliest_period_from = @period_from
	SET @latest_period_to = @period_to

	IF @period_from IS NULL
		BEGIN
			IF @data_count < 1
				RETURN

			SELECT @temp_period = MIN(Transaction_Period) FROM #VoucherTransaction

			IF LEN(@temp_period) = 4
				SET @earliest_period_from = @temp_period + '-1-1'
			ELSE IF LEN(@temp_period) = 7
				SET @earliest_period_from = LEFT(@temp_period, 4) + '-' + RIGHT(@temp_period, 2) + '-1'
			ELSE IF LEN(@temp_period) = 10
				SET @earliest_period_from = @temp_period
		END

	IF @period_to IS NULL
		BEGIN
			IF @data_count < 1
				RETURN

			SELECT @temp_period = MAX(Transaction_Period) FROM #VoucherTransaction

			IF LEN(@temp_period) = 4
				SET @latest_period_to = @temp_period + '-1-1'
			ELSE IF LEN(@temp_period) = 7
				SET @latest_period_to = LEFT(@temp_period, 4) + '-' + RIGHT(@temp_period, 2) + '-1'
			ELSE IF LEN(@temp_period) = 10
				SET @latest_period_to = @temp_period
		END

	DROP TABLE #VoucherTransaction

	INSERT INTO #PivotTableRow (Row_Name)
	SELECT Period FROM func_get_period(@earliest_period_from, @latest_period_to, @statistic_row_type)

	-- Add Pivot Table with All Period for Result
	SET @sql_script = 'SELECT 0,PTR.[Row_Name],' + @pivot_table_column_name_alias + '
		FROM #PivotTableRow PTR
			LEFT JOIN #PivotTableTemp PTT
				ON PTR.Row_Name = PTT.Period'

	SET @sql_script = 'INSERT INTO #PivotTable' + @sql_script_pivot_table_insert + @sql_script
	EXECUTE (@sql_script)
	DROP TABLE #PivotTableRow

	-- Add Grand Total of Pivot Table for Result
	SET @sql_script = 'INSERT INTO #PivotTable' + @sql_script_pivot_table_insert + 'SELECT 1,''' + @grand_total + ''',' + @pivot_table_row_total + ' FROM #PivotTableTemp'
	EXECUTE (@sql_script)
	DROP TABLE #PivotTableTemp

-- =============================================
-- Return results
-- =============================================

	SET @sql_script = 'SELECT [Period],' + @pivot_table_column_name + ',[' + @grand_total + '] FROM #PivotTable ORDER BY [Seq], [Period]'
	EXECUTE (@sql_script)

	DROP TABLE #PivotTable

END
GO

GRANT EXECUTE ON [dbo].[proc_Statistics_STAT00003_GetReport] TO HCVU
GO
