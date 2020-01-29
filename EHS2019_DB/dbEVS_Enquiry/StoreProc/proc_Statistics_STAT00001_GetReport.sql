IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Statistics_STAT00001_GetReport]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Statistics_STAT00001_GetReport]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	Winnie SUEN
-- Modified date: 21 Apr 2015
-- Description:	1. Refine District Structure
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
-- Create date:		08 Nov 2012
-- Description:		Get Report for Statistics - STAT00001
-- =============================================

CREATE PROCEDURE [dbo].[proc_Statistics_STAT00001_GetReport] 
	@statistic_row_type	char(1),
	@statistic_col_type	char(1),
	@period_from		datetime,
	@period_to			datetime,
	@cut_off_date		datetime,
	@age_from			int,
	@age_to				int,
	@category_filter	varchar(5000)
AS BEGIN

-- =============================================
-- Declaration
-- =============================================

	DECLARE @dept_health_code	char(2)
	DECLARE @grand_total		varchar(12)

	DECLARE @data_count	int

	DECLARE @earliest_period_from	datetime
	DECLARE @latest_period_to		datetime
	DECLARE @temp_period			varchar(10)

	CREATE TABLE #VoucherAccountCreation (
		Create_Date				varchar(10) COLLATE Chinese_Taiwan_Stroke_CI_AS,
		Voucher_Acc_ID			char(15),
		Create_By_Category		varchar(15) COLLATE Chinese_Taiwan_Stroke_CI_AS
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
	DECLARE @pivot_table_column_name_alias	varchar(MAX)

	DECLARE @pivot_table_column_total		varchar(MAX)
	DECLARE @pivot_table_row_total			varchar(MAX)

	DECLARE @pivot_table_column_header		varchar(MAX)

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

	IF @period_from = ''
		SET @period_from = NULL

	IF @period_to = ''
		SET @period_to = NULL

	IF @period_from > @period_to AND (NOT(@period_from IS NULL)) AND (NOT(@period_to IS NULL))
		RETURN

	IF @cut_off_date IS NULL
		RETURN

	IF @cut_off_date = ''
		RETURN

	IF @age_from < 0 OR @age_to < 0
		RETURN

	IF @age_from > @age_to AND (NOT(@age_from IS NULL)) AND (NOT(@age_to IS NULL))
		RETURN

	IF @category_filter IS NULL
		SET @category_filter = ''

-- =============================================
-- Initialization
-- =============================================

	SET @dept_health_code = 'DH'
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
				NULL,
				@period_from,
				@period_to

		END

	IF (SELECT COUNT(1) FROM @PivotTableColumn) < 1
		RETURN

	-- Prepare Column List for Dynamic SQL String
	SELECT
		@pivot_table_column_name = COALESCE(@pivot_table_column_name + ',[' + LTRIM(RTRIM(Column_Name)) + ']', '[' + LTRIM(RTRIM(Column_Name)) + ']'),
		@pivot_table_column_name_alias = COALESCE(@pivot_table_column_name_alias + ',ISNULL(PTT.[' + LTRIM(RTRIM(Column_Name)) + '],0)', 'ISNULL(PTT.[' + LTRIM(RTRIM(Column_Name)) + '],0)'),
		@pivot_table_column_total = COALESCE(@pivot_table_column_total + '+[' + LTRIM(RTRIM(Column_Name)) + ']', '[' + LTRIM(RTRIM(Column_Name)) + ']'),
		@pivot_table_row_total = COALESCE(@pivot_table_row_total + ',ISNULL(SUM([' + LTRIM(RTRIM(Column_Name)) + ']),0)', 'ISNULL(SUM([' + LTRIM(RTRIM(Column_Name)) + ']),0)'),
		@pivot_table_column_header = COALESCE(@pivot_table_column_header + ',[' + LTRIM(RTRIM(Column_Name)) + '] int', '[' + LTRIM(RTRIM(Column_Name)) + '] int')
	FROM @PivotTableColumn
	
	SET @pivot_table_column_name = @pivot_table_column_name + ',[' + @dept_health_code + ']'
	SET @pivot_table_column_name_alias = @pivot_table_column_name_alias + ',ISNULL(PTT.[' + @dept_health_code + '],0),ISNULL(PTT.[' + @grand_total + '],0)'
	SET @pivot_table_column_total = @pivot_table_column_total + '+[' + @dept_health_code + ']'
	SET @pivot_table_row_total = @pivot_table_row_total + ',ISNULL(SUM([' + @dept_health_code + ']),0),ISNULL(SUM([' + @grand_total + ']),0)'
	SET @pivot_table_column_header = @pivot_table_column_header + ',[' + @dept_health_code + '] int,[' + @grand_total + '] int'

	-- Add Column for Result dynamically
	EXECUTE ('ALTER TABLE #PivotTable ADD ' + @pivot_table_column_header)
	EXECUTE ('ALTER TABLE #PivotTableTemp ADD ' + @pivot_table_column_header)

	SET @sql_script_pivot_table_insert = ' ([Seq],[Period],' + @pivot_table_column_name + ',[' + @grand_total + ']) '

-- =============================================
-- Retrieve Data
-- =============================================

	-- Get Data
	INSERT INTO #VoucherAccountCreation (
		Create_Date,
		Voucher_Acc_ID,
		Create_By_Category
	)
	EXECUTE proc_Statistics_STAT00001_GetData
		@statistic_row_type,
		@statistic_col_type,
		@period_from,
		@period_to,
		@cut_off_date,
		@age_from,
		@age_to,
		@category_filter

	-- Count Data
	SELECT @data_count = COUNT(1) FROM #VoucherAccountCreation

	-- Generate Pivot Table for Result
	SET @sql_script = 'SELECT 0, *, ' + @pivot_table_column_total + '
		FROM (
			SELECT
				Create_Date,
				Voucher_Acc_ID,
				Create_By_Category
			FROM
				#VoucherAccountCreation
		) DATA
		PIVOT (
			COUNT(Voucher_Acc_ID)
			FOR Create_By_Category
			IN (' + @pivot_table_column_name + ')
		) FUNC'

	SET @sql_script = 'INSERT INTO #PivotTableTemp' + @sql_script_pivot_table_insert + @sql_script
	EXECUTE (@sql_script)

	-- Get All Period for Row of Result
	SET @earliest_period_from = @period_from
	SET @latest_period_to = @period_to

	IF @period_from IS NULL
		BEGIN
			IF @data_count < 1
				RETURN

			SELECT @temp_period = MIN(Create_Date) FROM #VoucherAccountCreation

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

			SELECT @temp_period = MAX(Create_Date) FROM #VoucherAccountCreation

			IF LEN(@temp_period) = 4
				SET @latest_period_to = @temp_period + '-1-1'
			ELSE IF LEN(@temp_period) = 7
				SET @latest_period_to = LEFT(@temp_period, 4) + '-' + RIGHT(@temp_period, 2) + '-1'
			ELSE IF LEN(@temp_period) = 10
				SET @latest_period_to = @temp_period
		END

	DROP TABLE #VoucherAccountCreation

	INSERT INTO #PivotTableRow (Row_Name)
	SELECT Period FROM func_get_period(@earliest_period_from, @latest_period_to, @statistic_row_type)

	-- Add Pivot Table with All Period for Result
	SET @sql_script = 'SELECT 0,PTR.[Row_Name],' + @pivot_table_column_name_alias + ' FROM
		#PivotTableRow PTR
			LEFT JOIN #PivotTableTemp PTT
				ON PTR.Row_Name = PTT.Period
		ORDER BY PTR.[Row_Name]'

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

GRANT EXECUTE ON [dbo].[proc_Statistics_STAT00001_GetReport] TO HCVU
GO
