IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_TransactionHourly_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_TransactionHourly_Stat]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	21 Oct 2020
-- CR No.:			CRE20-015 (HA Scheme)
-- Description:		Enlarge size of [Scheme_Claim].[Scheme_Desc] from 100 -> 200
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT13-0030
-- Modified by:		Tommy LAM
-- Modified date:	23 Dec 2013
-- Description:		Group 6 Sub-reports (eHSD0010-03 to eHSD0010-08) to 1 Sub-report (eHSD0010-03)
--					Generate Scheme information dynamically for Sub-report (eHSD0010-01)
--					Generate Worksheet - "Remark" dynamically
--					Rename Table [_TransactionByHour] to [RpteHSD0010TransactionByHour]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE13-001 
-- Modified by:		Koala CHENG
-- Modified date:	8 May 2013
-- Description:		Add EHAPP in the eHSD0010-01: Report on Summary of daily transactions
--					Add sub report eHSD0010-08 (Report on No. of Transactions group by hour (EHAPP))
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	8 April 2011
-- Description:		Add HSIVSS in the eHSD0010-01: Report on Summary of daily transactions
-- =============================================
-- =============================================
-- Modification History
-- Author:		Cheuk LAI
-- Create date:		21 October 2010
-- Description:		Add new tab for Generate Report for eHS Transaction Summary (with HKIC & SmartIC)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	11 January 2010
-- Description:		Add HSIVSS
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 05 Jan 2009
-- Description:	Voucher Transaction Statistics group by hourly per Month
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	30 Oct 2009
-- Description:		Retrieve no. of transaction group by hour in different
--					scheme
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_TransactionHourly_Stat]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
declare @start_dtm varchar(50)
declare @end_dtm varchar(50)
declare @temp_dtm varchar(50)
declare @no_of_days int
declare @TurnONtoday int


select @no_of_days=14	-- excluding today
select @TurnONtoday = 0	-- 0 = NOT show today ; 1 = Show today for testing

select @start_dtm  =  CONVERT(VARCHAR(11), dateadd(day, -(@no_of_days), GETDATE()), 106)
select @end_dtm  =  CONVERT(VARCHAR(11), GETDATE()+ @TurnONtoday , 106)


-------------  Add new tab for Generate Report for eHS Transaction Summary (with HKIC & SmartIC) ----------------
declare @start_time_temp datetime
declare @start_time datetime
declare @end_time_temp datetime
declare @end_time datetime
declare @counter_time_temp datetime
declare @counter_time datetime

select @start_time_temp	= DateAdd(day, -(@no_of_days), GETDATE() ) 
select @start_time		= convert(varchar(4), YEAR(@start_time_temp)) + right('00'+ convert(varchar, MONTH(@start_time_temp)) , 2) + right('00'+ convert(varchar, DAY(@start_time_temp)), 2) + '  00:00:00'
select @end_time_temp	= DateAdd(day, (-1 + @TurnONtoday), GETDATE() )  
select @end_time		= convert(varchar(4), YEAR(@end_time_temp)) + right('00'+ convert(varchar, MONTH(@end_time_temp)) , 2) + right('00'+ convert(varchar, DAY(@end_time_temp)), 2) + '  23:59:59'


select 'Report Generation Time: ' + CONVERT(VARCHAR(10), GETDATE(), 111) + '  ' + CONVERT(VARCHAR(8), GETDATE(), 108)


---------------- Create Temp Table for Dates -------------------------

SELECT  
	'YYYY-MM-DD' as [Date] 
INTO #Date_Temp
FROM [VoucherTransaction]
WHERE 1 = 2			-- make it always false, in order to barely create a table without data



declare @counter int
set @counter = -(@no_of_days)
while @counter <= -1 + @TurnONtoday
begin
	select @counter_time_temp	= DateAdd(day, @counter, GETDATE() )  
	select @counter_time		= convert(varchar(4), YEAR(@counter_time_temp)) + right('00'+ convert(varchar, MONTH(@counter_time_temp)) , 2) + right('00'+ convert(varchar, DAY(@counter_time_temp)), 2) 

	INSERT INTO #Date_Temp ([Date]) VALUES ( left(convert(varchar, @counter_time, 120),10) )
	set @counter = @counter + 1
end
------------- End of Add new tab for Generate Report for eHS Transaction Summary ----------------


-- ********** Sub-report: eHSD0010-01 **********

-- =============================================
-- Declaration
-- =============================================

	DECLARE @current_dtm	datetime

	DECLARE @SchemeClaim TABLE (
		Scheme_Code		char(10),
		Display_Code	char(25),
		Display_Seq		smallint,
		Scheme_Desc		varchar(200)
	)

	CREATE TABLE #ReportData (
		Transaction_ID		char(20),
		Transaction_Dtm		datetime,
		Scheme_Display_Code	char(25),
		Doc_Code			char(20),
		Create_By_SmartID	char(1)
	)

	CREATE TABLE #ResultData (
		Trans_Date			varchar(10),
		Scheme_Display_Code	char(25),
		No_Of_Trans			int
	)

	CREATE TABLE #PivotTable (
		Trans_Date	varchar(10)
	)

	DECLARE @pivot_table_column_header		varchar(MAX)
	DECLARE @pivot_table_column_list		varchar(MAX)
	DECLARE @pivot_table_column_name_value	varchar(MAX)
	DECLARE @pivot_table_column_total		varchar(MAX)
	DECLARE @pivot_table_column_name_alias	varchar(MAX)

	DECLARE @pivot_table_column_name_alias1	varchar(MAX)
	DECLARE @pivot_table_column_name_alias2	varchar(MAX)
	DECLARE @pivot_table_column_name_alias3	varchar(MAX)

	DECLARE @pivot_table_column_1st			varchar(25)

	DECLARE @sql_script						varchar(MAX)

-- =============================================
-- Initialization
-- =============================================

	SET @current_dtm = GETDATE()

-- ---------------------------------------------
-- Prepare @SchemeClaim
-- ---------------------------------------------

	INSERT INTO @SchemeClaim (Scheme_Code, Display_Code, Display_Seq, Scheme_Desc)
	SELECT
		Scheme_Code,
		Display_Code,
		Display_Seq,
		Scheme_Desc
	FROM SchemeClaim WITH (NOLOCK)
	WHERE Effective_Dtm <= @current_dtm AND Record_Status = 'A' AND Scheme_Seq = 1

-- ---------------------------------------------
-- Prepare Column List for Dynamic SQL String
-- ---------------------------------------------

	SELECT
		@pivot_table_column_header = COALESCE(@pivot_table_column_header + ',', '') + '[' + LTRIM(RTRIM(Display_Code)) + '] varchar(50)',
		@pivot_table_column_list = COALESCE(@pivot_table_column_list + ',', '') + '[' + LTRIM(RTRIM(Display_Code)) + ']',
		@pivot_table_column_name_value = COALESCE(@pivot_table_column_name_value + ',', '') + '''' + LTRIM(RTRIM(Display_Code)) + '''',
		@pivot_table_column_total = COALESCE(@pivot_table_column_total + '+', '') + 'ISNULL([' + LTRIM(RTRIM(Display_Code)) + '],0)',
		@pivot_table_column_name_alias = COALESCE(@pivot_table_column_name_alias + ',', '') + 'ISNULL(PT.[' + LTRIM(RTRIM(Display_Code)) + '],0)',
		@pivot_table_column_name_alias1 = COALESCE(@pivot_table_column_name_alias1 + ',', '') + 'RT1.[' + LTRIM(RTRIM(Display_Code)) + ']',
		@pivot_table_column_name_alias2 = COALESCE(@pivot_table_column_name_alias2 + ',', '') + 'RT2.[' + LTRIM(RTRIM(Display_Code)) + ']',
		@pivot_table_column_name_alias3 = COALESCE(@pivot_table_column_name_alias3 + ',', '') + 'RT3.[' + LTRIM(RTRIM(Display_Code)) + ']'
	FROM @SchemeClaim
	ORDER BY Display_Seq

	SELECT TOP 1 @pivot_table_column_1st = '[' + LTRIM(RTRIM(Display_Code)) + ']' FROM @SchemeClaim ORDER BY Display_Seq

	SET @pivot_table_column_header = @pivot_table_column_header + ',[Total] varchar(50)'
	SET @pivot_table_column_name_value = @pivot_table_column_name_value + ',''Total'''
	SET @pivot_table_column_name_alias = @pivot_table_column_name_alias + ',ISNULL(PT.[Total],0)'

	SET @pivot_table_column_name_alias1 = @pivot_table_column_name_alias1 + ',RT1.[Total]'
	SET @pivot_table_column_name_alias2 = @pivot_table_column_name_alias2 + ',RT2.[Total]'
	SET @pivot_table_column_name_alias3 = @pivot_table_column_name_alias3 + ',RT3.[Total]'

-- ---------------------------------------------
-- Add Column for Pivot Table dynamically
-- ---------------------------------------------

	EXECUTE('ALTER TABLE #PivotTable ADD ' + @pivot_table_column_header)

-- =============================================
-- Retrieve Data
-- =============================================

	-- Retrieve Report Data for General Usage
	INSERT INTO #ReportData (
		Transaction_ID,
		Transaction_Dtm,
		Scheme_Display_Code,
		Doc_Code,
		Create_By_SmartID
	)
	SELECT
		VT.Transaction_ID,
		VT.Transaction_Dtm,
		SC.Display_Code,
		VT.Doc_Code,
		VT.Create_By_SmartID
	FROM VoucherTransaction VT WITH (NOLOCK)
		INNER JOIN SchemeClaim SC WITH (NOLOCK)
			ON VT.Scheme_Code = SC.Scheme_Code AND SC.Scheme_Seq = 1
	WHERE VT.Transaction_Dtm > @start_time AND VT.Transaction_Dtm < @end_time

-- ---------------------------------------------
-- Part 1 (Start)
-- ---------------------------------------------

	-- Generate Report Result for Part 1
	INSERT INTO #ResultData (
		Trans_Date,
		Scheme_Display_Code,
		No_Of_Trans
	)
	SELECT
		LEFT(CONVERT(varchar, Transaction_Dtm, 120), 10),
		Scheme_Display_Code,
		COUNT(Transaction_ID)
	FROM #ReportData
	GROUP BY
		LEFT(CONVERT(varchar, Transaction_Dtm, 120), 10),
		Scheme_Display_Code

	-- Generate Pivot Table for Result
	SET @sql_script = 'SELECT *,' + @pivot_table_column_total + '
		FROM (
			SELECT
				Trans_Date,
				Scheme_Display_Code,
				No_Of_Trans
			FROM
				#ResultData
		) DATA
		PIVOT (
			SUM(No_Of_Trans)
			FOR Scheme_Display_Code
			IN (' + @pivot_table_column_list + ')
		) FUNC'

	SET @sql_script = 'INSERT INTO #PivotTable ([Trans_Date],' + @pivot_table_column_list + ',[Total]) ' + @sql_script
	EXECUTE(@sql_script)

	-- Create Result Table for Part 1
	CREATE TABLE #ResultTable_01 (
		KeyJoin		varchar(10),
		Seq			int,
		Trans_Date	varchar(10)
	)

	-- Add Column for Result Table dynamically
	EXECUTE('ALTER TABLE #ResultTable_01 ADD ' + @pivot_table_column_header)

	-- Create Column Header dynamically
	SET @sql_script = 'INSERT INTO #ResultTable_01 ([KeyJoin],[Seq],[Trans_Date],' + @pivot_table_column_1st + ') VALUES (''header1'',0,'''',''No. of transactions'')'
	EXECUTE(@sql_script)

	SET @sql_script = 'INSERT INTO #ResultTable_01 ([KeyJoin],[Seq],[Trans_Date],' + @pivot_table_column_list + ',[Total]) VALUES (''header2'',1,'''',' + @pivot_table_column_name_value + ')'
	EXECUTE(@sql_script)

	-- Generate Result Table
	SET @sql_script = 'SELECT DT.[Date],2,DT.[Date],' + @pivot_table_column_name_alias + ' FROM
		#Date_Temp DT
			LEFT JOIN #PivotTable PT
				ON DT.[Date] = PT.[Trans_Date] COLLATE DATABASE_DEFAULT'

	SET @sql_script = 'INSERT INTO #ResultTable_01 ([KeyJoin],[Seq],[Trans_Date],' + @pivot_table_column_list + ',[Total]) ' + @sql_script
	EXECUTE (@sql_script)

	-- Release Resource
	TRUNCATE TABLE #ResultData
	TRUNCATE TABLE #PivotTable

-- ---------------------------------------------
-- Part 1 (End)
-- ---------------------------------------------

-- ---------------------------------------------
-- Part 2 (Start)
-- ---------------------------------------------

	-- Generate Report Result for Part 2
	INSERT INTO #ResultData (
		Trans_Date,
		Scheme_Display_Code,
		No_Of_Trans
	)
	SELECT
		LEFT(CONVERT(varchar, Transaction_Dtm, 120), 10),
		Scheme_Display_Code,
		COUNT(Transaction_ID)
	FROM #ReportData
	WHERE Doc_Code = 'HKIC'
	GROUP BY
		LEFT(CONVERT(varchar, Transaction_Dtm, 120), 10),
		Scheme_Display_Code

	-- Generate Pivot Table for Result
	SET @sql_script = 'SELECT *,' + @pivot_table_column_total + '
		FROM (
			SELECT
				Trans_Date,
				Scheme_Display_Code,
				No_Of_Trans
			FROM
				#ResultData
		) DATA
		PIVOT (
			SUM(No_Of_Trans)
			FOR Scheme_Display_Code
			IN (' + @pivot_table_column_list + ')
		) FUNC'

	SET @sql_script = 'INSERT INTO #PivotTable ([Trans_Date],' + @pivot_table_column_list + ',[Total]) ' + @sql_script
	EXECUTE(@sql_script)

	-- Create Result Table for Part 2
	CREATE TABLE #ResultTable_02 (
		KeyJoin		varchar(10),
		Seq			int,
		Trans_Date	varchar(10)
	)

	-- Add Column for Result Table dynamically
	EXECUTE('ALTER TABLE #ResultTable_02 ADD ' + @pivot_table_column_header)

	-- Create Column Header dynamically
	SET @sql_script = 'INSERT INTO #ResultTable_02 ([KeyJoin],[Seq],[Trans_Date],' + @pivot_table_column_1st + ') VALUES (''header1'',0,'''',''No. of transactions with document type of HKIC'')'
	EXECUTE(@sql_script)

	SET @sql_script = 'INSERT INTO #ResultTable_02 ([KeyJoin],[Seq],[Trans_Date],' + @pivot_table_column_list + ',[Total]) VALUES (''header2'',1,'''',' + @pivot_table_column_name_value + ')'
	EXECUTE(@sql_script)

	-- Generate Result Table
	SET @sql_script = 'SELECT DT.[Date],2,DT.[Date],' + @pivot_table_column_name_alias + ' FROM
		#Date_Temp DT
			LEFT JOIN #PivotTable PT
				ON DT.[Date] = PT.[Trans_Date] COLLATE DATABASE_DEFAULT'

	SET @sql_script = 'INSERT INTO #ResultTable_02 ([KeyJoin],[Seq],[Trans_Date],' + @pivot_table_column_list + ',[Total]) ' + @sql_script
	EXECUTE (@sql_script)

	-- Release Resource
	TRUNCATE TABLE #ResultData
	TRUNCATE TABLE #PivotTable

-- ---------------------------------------------
-- Part 2 (End)
-- ---------------------------------------------

-- ---------------------------------------------
-- Part 3 (Start)
-- ---------------------------------------------

	-- Generate Report Result for Part 3
	INSERT INTO #ResultData (
		Trans_Date,
		Scheme_Display_Code,
		No_Of_Trans
	)
	SELECT
		LEFT(CONVERT(varchar, Transaction_Dtm, 120), 10),
		Scheme_Display_Code,
		COUNT(Transaction_ID)
	FROM #ReportData
	WHERE Create_By_SmartID = 'Y'
	GROUP BY
		LEFT(CONVERT(varchar, Transaction_Dtm, 120), 10),
		Scheme_Display_Code

	-- Generate Pivot Table for Result
	SET @sql_script = 'SELECT *,' + @pivot_table_column_total + '
		FROM (
			SELECT
				Trans_Date,
				Scheme_Display_Code,
				No_Of_Trans
			FROM
				#ResultData
		) DATA
		PIVOT (
			SUM(No_Of_Trans)
			FOR Scheme_Display_Code
			IN (' + @pivot_table_column_list + ')
		) FUNC'

	SET @sql_script = 'INSERT INTO #PivotTable ([Trans_Date],' + @pivot_table_column_list + ',[Total]) ' + @sql_script
	EXECUTE(@sql_script)

	-- Create Result Table for Part 3
	CREATE TABLE #ResultTable_03 (
		KeyJoin		varchar(10),
		Seq			int,
		Trans_Date	varchar(10)
	)

	-- Add Column for Result Table dynamically
	EXECUTE('ALTER TABLE #ResultTable_03 ADD ' + @pivot_table_column_header)

	-- Create Column Header dynamically
	SET @sql_script = 'INSERT INTO #ResultTable_03 ([KeyJoin],[Seq],[Trans_Date],' + @pivot_table_column_1st + ') VALUES (''header1'',0,'''',''No. of transactions using Smart IC'')'
	EXECUTE(@sql_script)

	SET @sql_script = 'INSERT INTO #ResultTable_03 ([KeyJoin],[Seq],[Trans_Date],' + @pivot_table_column_list + ',[Total]) VALUES (''header2'',1,'''',' + @pivot_table_column_name_value + ')'
	EXECUTE(@sql_script)

	-- Generate Result Table
	SET @sql_script = 'SELECT DT.[Date],2,DT.[Date],' + @pivot_table_column_name_alias + ' FROM
		#Date_Temp DT
			LEFT JOIN #PivotTable PT
				ON DT.[Date] = PT.[Trans_Date] COLLATE DATABASE_DEFAULT'

	SET @sql_script = 'INSERT INTO #ResultTable_03 ([KeyJoin],[Seq],[Trans_Date],' + @pivot_table_column_list + ',[Total]) ' + @sql_script
	EXECUTE(@sql_script)

	-- Release Resource
	TRUNCATE TABLE #ResultData
	TRUNCATE TABLE #PivotTable

-- ---------------------------------------------
-- Part 3 (End)
-- ---------------------------------------------

-- =============================================
-- Return results
-- =============================================

	SET @sql_script = 'SELECT REPLACE(RT1.[Trans_Date],''-'',''/''),' + @pivot_table_column_name_alias1 + ','''',' + @pivot_table_column_name_alias2 + ','''',' + @pivot_table_column_name_alias3 + '
		FROM #ResultTable_01 RT1
			INNER JOIN #ResultTable_02 RT2
				ON RT1.KeyJoin = RT2.KeyJoin
			INNER JOIN #ResultTable_03 RT3
				ON RT1.KeyJoin = RT3.KeyJoin
		ORDER BY RT1.Seq, RT1.Trans_Date'

	EXECUTE(@sql_script)

	-- Release Resource
	DROP TABLE #ReportData
	DROP TABLE #ResultData
	DROP TABLE #PivotTable

	DROP TABLE #ResultTable_01
	DROP TABLE #ResultTable_02
	DROP TABLE #ResultTable_03

	DROP TABLE #Date_Temp


-- ********** Sub-report: eHSD0010-02 & eHSD0010-03 **********

create table #result
(
	report_dtm varchar(20),
	scheme_code char(10),
	time0 varchar(20),
	time1 varchar(20),
	time2 varchar(20),
	time3 varchar(20),
	time4 varchar(20),
	time5 varchar(20),
	time6 varchar(20),
	time7 varchar(20),
	time8 varchar(20),
	time9 varchar(20),
	time10 varchar(20),
	time11 varchar(20),
	time12 varchar(20),
	time13 varchar(20),
	time14 varchar(20),
	time15 varchar(20),
	time16 varchar(20),
	time17 varchar(20),
	time18 varchar(20),
	time19 varchar(20),
	time20 varchar(20),
	time21 varchar(20),
	time22 varchar(20),
	time23 varchar(20)
)

insert into #result
(
	report_dtm,	
	scheme_code,
	time0,
	time1,
	time2,
	time3,
	time4,
	time5,
	time6,
	time7,
	time8,
	time9,
	time10,
	time11,
	time12,
	time13,
	time14,
	time15,
	time16,
	time17,
	time18,
	time19,
	time20,
	time21,
	time22,
	time23
)
select CONVERT(VARCHAR(10), report_dtm, 111),
	scheme_code,
	time0,
	time1,
	time2,
	time3,
	time4,
	time5,
	time6,
	time7,
	time8,
	time9,
	time10,
	time11,
	time12,
	time13,
	time14,
	time15,
	time16,
	time17,
	time18,
	time19,
	time20,
	time21,
	time22,
	time23
from RpteHSD0010TransactionByHour WITH (NOLOCK)
where report_dtm between @start_dtm and @end_dtm

select CONVERT(VARCHAR, report_dtm, 111),	
	time0,
	time1,
	time2,
	time3,
	time4,
	time5,
	time6,
	time7,
	time8,
	time9,
	time10,
	time11,
	time12,
	time13,
	time14,
	time15,
	time16,
	time17,
	time18,
	time19,
	time20,
	time21,
	time22,
	time23
from #result
where scheme_code = 'ALL'
order by report_dtm

select
	sc.Display_Code,
	CONVERT(VARCHAR, r.report_dtm, 111),	
	r.time0,
	r.time1,
	r.time2,
	r.time3,
	r.time4,
	r.time5,
	r.time6,
	r.time7,
	r.time8,
	r.time9,
	r.time10,
	r.time11,
	r.time12,
	r.time13,
	r.time14,
	r.time15,
	r.time16,
	r.time17,
	r.time18,
	r.time19,
	r.time20,
	r.time21,
	r.time22,
	r.time23
from #result r
	inner join @SchemeClaim sc
		on r.scheme_code = sc.Scheme_Code COLLATE DATABASE_DEFAULT
where r.scheme_code <> 'ALL'
order by sc.Display_Seq, r.report_dtm


drop table #result


-- ********** Remark **********

	DECLARE @seq	int

	CREATE TABLE #Remark (
		Seq		int,
		Seq2	int,
		Col01	varchar(1000),
		Col02	varchar(1000)
	)

	SET @seq = 0

	INSERT INTO #Remark (Seq, Seq2, Col01, Col02) VALUES (@seq, NULL, '(A) Legend', '')

	SET @seq = @seq + 1

	INSERT INTO #Remark (Seq, Seq2, Col01, Col02) VALUES (@seq, NULL, '1. Scheme Name', '')
	
	SET @seq = @seq + 1

	INSERT INTO #Remark (Seq, Seq2, Col01, Col02)
	SELECT @seq, NULL, Display_Code, Scheme_Desc FROM @SchemeClaim

	SET @seq = @seq + 1

	INSERT INTO #Remark (Seq, Seq2, Col01, Col02) VALUES (@seq, NULL, '', '')

	SET @seq = @seq + 1

	INSERT INTO #Remark (Seq, Seq2, Col01, Col02) VALUES (@seq, NULL, '(B) Common Note(s) for the report', '')

	SET @seq = @seq + 1

	INSERT INTO #Remark (Seq, Seq2, Col01, Col02)
	VALUES (@seq, NULL, '1. All transactions are included (Reimbursable + Joined + Voided)', '')

	SELECT Col01, Col02 FROM #Remark ORDER BY Seq, Seq2, Col01


	DROP TABLE #Remark

END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_TransactionHourly_Stat] TO HCVU
GO
