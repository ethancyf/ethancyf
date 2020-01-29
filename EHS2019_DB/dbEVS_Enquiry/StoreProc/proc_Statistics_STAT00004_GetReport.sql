IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Statistics_STAT00004_GetReport]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Statistics_STAT00004_GetReport]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Modification History  
-- CR No.:			INT13-0011
-- Modified by:		Koala CHENG  
-- Modified date:	14 May 2013  
-- Description:		Fix database collation problem
-- =============================================  
-- =============================================
-- Author:		Tommy Tse
-- Create date:		19 Nov 2012
-- Description:		Get Report for Statistics - STAT00004
-- =============================================

CREATE PROCEDURE [dbo].[proc_Statistics_STAT00004_GetReport] 
	@submission_method  VARCHAR(5000), -- ExternalWS, IVRS, WEB-FULLY, WEB-FULLN, WEB-TEXTY, WEB-TEXTN [Delimiter ',']
	@period_from		DATETIME, --YYYY-MM-DD
	@period_to			DATETIME, --YYYY-MM-DD
	@scheme_code		VARCHAR(10), --HCVS, CIVSS, EVSS, RVP
	@period_type		VARCHAR(1) --T, S
AS BEGIN

-- =============================================
-- Validation Of Input Parameters
-- =============================================
	IF @submission_method IS NULL  
	  SET @submission_method = ''  

	IF @period_from = ''
		SET @period_from = NULL

	IF @period_to = ''
		SET @period_to = NULL

	IF @period_from > @period_to
		RETURN

	IF @scheme_code IS NULL
		SET @scheme_code = ''

	IF @period_type IS NULL
		SET @period_type = ''

-- =============================================
-- Insert Column Name Of Result Table
-- =============================================

	DECLARE @PivotTableColumn TABLE (
		Column_Name	VARCHAR(30) COLLATE Chinese_Taiwan_Stroke_CI_AS
	)

	INSERT INTO @PivotTableColumn (
		Column_Name
	)
	EXECUTE proc_SchemeBackOffice_GetEligibleProfessional
		@scheme_code,
		@period_from,
		@period_to

	DECLARE @column_count	INT

	SELECT @column_count = COUNT(1) FROM @PivotTableColumn

	IF (@column_count) < 1
		RETURN

	DECLARE @grand_total		VARCHAR(12)
	SET @grand_total = 'Total'

-- =============================================
-- Append Profession Records Into String
-- =============================================

	DECLARE @pivot_table_column_data_type	VARCHAR(15)

	SET @pivot_table_column_data_type = 'int'

	DECLARE @pivot_table_column_name		VARCHAR(MAX)
	DECLARE @pivot_table_column_name_alias	VARCHAR(MAX)
	DECLARE @pivot_table_column_name_value	VARCHAR(MAX)
	DECLARE @pivot_table_column_total		VARCHAR(MAX)
	DECLARE @pivot_table_column_header		VARCHAR(MAX)
	DECLARE @pivot_table_temp_column_header	VARCHAR(MAX)
	DECLARE @empty_value					VARCHAR(MAX)

	SELECT
		@pivot_table_column_name = COALESCE(@pivot_table_column_name + ',[' + LTRIM(RTRIM(Column_Name)) + ']', '[' + LTRIM(RTRIM(Column_Name)) + ']'),
		@pivot_table_column_name_alias = COALESCE(@pivot_table_column_name_alias + ',ISNULL(PTT.[' + LTRIM(RTRIM(Column_Name)) + '],0)', 'ISNULL(PTT.[' + LTRIM(RTRIM(Column_Name)) + '],0)'),
		@pivot_table_column_name_value = COALESCE(@pivot_table_column_name_value + ',''' + LTRIM(RTRIM(Column_Name)) + '''', '''' + LTRIM(RTRIM(Column_Name)) + ''''),
		@pivot_table_column_total = COALESCE(@pivot_table_column_total + '+[' + LTRIM(RTRIM(Column_Name)) + ']', '[' + LTRIM(RTRIM(Column_Name)) + ']'),
		@pivot_table_column_header = COALESCE(@pivot_table_column_header + ',[' + LTRIM(RTRIM(Column_Name)) + '] ' + @pivot_table_column_data_type, '[' + LTRIM(RTRIM(Column_Name)) + '] ' + @pivot_table_column_data_type),
		@pivot_table_temp_column_header = COALESCE(@pivot_table_temp_column_header + ',[' + LTRIM(RTRIM(Column_Name)) + '] int', '[' + LTRIM(RTRIM(Column_Name)) + '] int'),
		@empty_value = COALESCE(@empty_value + ',0','0')
	FROM @PivotTableColumn

-- =============================================
-- Create The Structure Of Temporary Table and Result Table
-- =============================================

	CREATE TABLE #PivotTable (
		Total	INT
	)

	CREATE TABLE #PivotTableTemp (
		Total	INT
	)

	EXECUTE ('ALTER TABLE #PivotTable ADD ' + @pivot_table_column_header)
	EXECUTE ('ALTER TABLE #PivotTableTemp ADD ' + @pivot_table_temp_column_header)

	DECLARE @sql_script_pivot_table_insert	VARCHAR(MAX)
	DECLARE @sql_script						VARCHAR(MAX)

	--Add Header Row
	--SET @sql_script_pivot_table_insert = ' (' + @pivot_table_column_name + ',[' + @grand_total + ']' + ') '

	--SET @sql_script = 'INSERT INTO #PivotTable' + @sql_script_pivot_table_insert + 'SELECT ' + @pivot_table_column_name_value + ',''' + @grand_total + ''''
	
	--EXECUTE (@sql_script)

-- =============================================
-- Transform GetData Result Table Into Temporary Table
-- =============================================

	CREATE TABLE #SPProfession (
		ProfessionCode			VARCHAR(10) COLLATE Chinese_Taiwan_Stroke_CI_AS,
		SPID					VARCHAR(15) COLLATE Chinese_Taiwan_Stroke_CI_AS
	)

	INSERT INTO #SPProfession (
		ProfessionCode,
		SPID
	)
	EXECUTE proc_Statistics_STAT00004_GetData @submission_method, @period_from, @period_to, @scheme_code, @period_type

	DECLARE @data_count	INT

	SELECT @data_count = COUNT(1) FROM #SPProfession

	IF (@data_count) < 1
	SET @sql_script = ' values (' + @empty_value + ',0' + ')'
	ELSE
	SET @sql_script = 'SELECT *, ' + @pivot_table_column_total + ' FROM (
			SELECT
				ProfessionCode,
				SPID
			FROM
				#SPProfession
		) DATA
		PIVOT (
			COUNT(SPID)
			FOR ProfessionCode
			IN ('+ @pivot_table_column_name + ')) FUNC';

	SET @sql_script_pivot_table_insert = ' (' + @pivot_table_column_name + ',[' + @grand_total + ']' + ') '

	SET @sql_script = 'INSERT INTO #PivotTableTemp' + @sql_script_pivot_table_insert + @sql_script

--	SELECT * from #PivotTableTemp

	EXECUTE (@sql_script)

-- =============================================
-- Insert Record From Temporary Table To Result Table
-- =============================================

	SET @sql_script_pivot_table_insert = ' (' + @pivot_table_column_name + ',[' + @grand_total + ']' + ') '

	SET @sql_script = 'SELECT ' + @pivot_table_column_name_alias + ',ISNULL(PTT.[' + @grand_total + '],0)' + ' FROM #PivotTableTemp PTT'

	SET @sql_script = 'INSERT INTO #PivotTable' + @sql_script_pivot_table_insert + @sql_script

	EXECUTE (@sql_script)

-- =============================================
-- Return Result Table
-- =============================================

	SET @pivot_table_column_name = @pivot_table_column_name + ',[' + @grand_total + ']'

	SET @sql_script = 'SELECT ' + @pivot_table_column_name + ' FROM #PivotTable'

	EXECUTE (@sql_script)

-- =============================================
-- Drop Temporary Tables
-- =============================================

	DROP TABLE #SPProfession

	DROP TABLE #PivotTableTemp

	DROP TABLE #PivotTable

End

GO

GRANT EXECUTE ON [dbo].[proc_Statistics_STAT00004_GetReport] TO HCVU
GO
