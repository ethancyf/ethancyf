IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0021-05]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0021-05]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	2 November 2015
-- CR No.:			CRE15-006
-- Description:		Rename of eHS
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE13-019-02
-- Modified by:		Winnie SUEN
-- Modified date:	04 Mar 2015
-- Description:		Rewrite function, Change the Column to Dynamic Gen
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE13-021-02
-- Modified by:		Tommy LAM
-- Modified date:	03 Jan 2014
-- Description:		Change the Column from "Year" to "Period"
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE13-001 
-- Modified by:		Koala CHENG
-- Modified date:	29 May 2013
-- Description:		Change to content of eHSD0021-04
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRP11-005
-- Modified by:		Helen Lam
-- Modified date:	05 APR 2012
-- Description:		CRP11-005 - Summary of Voucher claim break down by vaccination season
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSD0021-05]
@Report_Dtm		datetime = NULL
AS BEGIN
	SET NOCOUNT ON;
	
-- =============================================
-- Declaration
-- =============================================
	IF @Report_Dtm IS NOT NULL BEGIN
		SELECT @Report_Dtm = CONVERT(varchar, DATEADD(dd, 1, @Report_Dtm), 106)
	END ELSE BEGIN
		SELECT @Report_Dtm = CONVERT(varchar, GETDATE(), 106) -- "106" gives "dd MMM yyyy"  
	END
	

 CREATE TABLE #ResultData (  
  Scheme_Seq   varchar(10),  
  Scheme_Display_Code char(25),  
  No_Of_Trans   int  
 )  
  
 CREATE TABLE #PivotTable (  
  Scheme_Seq varchar(10)  
 )  

 DECLARE @SchemeClaim TABLE (  
  Scheme_Code  char(10),  
  Display_Code char(25),  
  Display_Seq  smallint,  
  Scheme_Desc  varchar(100)  
 )  
   
 DECLARE @pivot_table_column_header  varchar(MAX)  --[HCVS] varchar(50),[HCVSC] varchar(50)
 DECLARE @pivot_table_column_list  varchar(MAX)   -- [HCVS],[HCVSC]
 DECLARE @pivot_table_column_name_value varchar(MAX) --'HCVS','HCVSC'
 DECLARE @pivot_table_column_total  varchar(MAX)  --ISNULL([HCVS],0)+ISNULL([HCVSC],0)
 DECLARE @pivot_table_column_name_alias varchar(MAX) --ISNULL(PT.[HCVS],0),ISNULL(PT.[HCVSC],0)
  
 DECLARE @pivot_table_column_name_alias1 varchar(MAX)  --RT1.[HCVS],RT1.[HCVSC]
 DECLARE @pivot_table_column_name_alias2 varchar(MAX)  --RT2.[HCVS],RT2.[HCVSC]
  
 DECLARE @pivot_table_column_1st   varchar(25)  --[HCVS]
  
 DECLARE @sql_script      varchar(MAX)  
 
 DECLARE @current_dtm datetime
 
---------------- Create Temp Table for Period -------------------------  
  
SELECT DISTINCT (Scheme_Seq) INTO #Period_Temp
FROM SubsidizeGroupClaim
WHERE	Subsidize_Code = 'EHCVS'
		AND Claim_Period_From <= @Report_Dtm
		AND Record_Status = 'A'    

   
-- =============================================  
-- Initialization  
-- =============================================  
  
 SET @current_dtm = GETDATE()  
 
-- ---------------------------------------------  
-- Prepare @SchemeClaim  
-- ---------------------------------------------  
  
 INSERT INTO @SchemeClaim (Scheme_Code, Display_Code, Display_Seq, Scheme_Desc)  
 SELECT  
  sc.Scheme_Code,  
  sc.Display_Code,  
  sc.Display_Seq,  
  sc.Scheme_Desc  
 FROM SchemeClaim sc WITH (NOLOCK)  
 INNER JOIN SubsidizeGroupClaim sgc ON sc.Scheme_Code = sgc.Scheme_Code AND sc.Scheme_Seq = sgc.Scheme_Seq
 WHERE sc.Effective_Dtm <= @current_dtm AND sc.Record_Status = 'A' AND sc.Scheme_Seq = 1 
	AND sgc.Subsidize_Code = 'EHCVS'
		
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
  @pivot_table_column_name_alias2 = COALESCE(@pivot_table_column_name_alias2 + ',', '') + 'RT2.[' + LTRIM(RTRIM(Display_Code)) + ']'
 FROM @SchemeClaim
 ORDER BY Display_Seq  
  
 SELECT TOP 1 @pivot_table_column_1st = '[' + LTRIM(RTRIM(Display_Code)) + ']' FROM @SchemeClaim ORDER BY Display_Seq  
  
 SET @pivot_table_column_header = @pivot_table_column_header + ',[Total] varchar(50)'  
 SET @pivot_table_column_name_value = @pivot_table_column_name_value + ',''Total'''  
 SET @pivot_table_column_name_alias = @pivot_table_column_name_alias + ',ISNULL(PT.[Total],0)'  
  
 SET @pivot_table_column_name_alias1 = @pivot_table_column_name_alias1 + ',RT1.[Total]'  
 SET @pivot_table_column_name_alias2 = @pivot_table_column_name_alias2 + ',RT2.[Total]'  
  
-- ---------------------------------------------  
-- Add Column for Pivot Table dynamically  
-- ---------------------------------------------  
  
 EXECUTE('ALTER TABLE #PivotTable ADD ' + @pivot_table_column_header)  

-- ---------------------------------------------  
-- Part 1 (Start)  
-- ---------------------------------------------    
 -- Generate Report Result for Part 1  
 INSERT INTO #ResultData (  
  Scheme_Seq,  
  Scheme_Display_Code,  
  No_Of_Trans
 )  
 SELECT    
  vd.Scheme_Seq,
  sc.Display_Code,
  COUNT(1)
 FROM VoucherTransaction vt WITH (NOLOCK) 
 INNER JOIN TransactionDetail vd WITH (NOLOCK) ON vt.transaction_id=vd.transaction_id and vt.scheme_code=vd.scheme_code
 INNER JOIN SchemeClaim sc WITH (NOLOCK) 
	ON vt.Scheme_Code = sc.Scheme_Code AND sc.Scheme_Seq = 1  
 WHERE vt.transaction_dtm < @Report_Dtm And vt.Record_Status Not in ('I', 'D','W') 
	AND (Invalidation IS NULL OR Invalidation Not in ('I'))  and vd.Subsidize_Code='EHCVS'	
 GROUP BY  
	vd.Scheme_Code, vd.Scheme_Seq, sc.Display_Code


 -- Generate Pivot Table for Result  
 SET @sql_script = 'SELECT *,' + @pivot_table_column_total + '  
  FROM (  
   SELECT  
    Scheme_Seq,  
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
  
 SET @sql_script = 'INSERT INTO #PivotTable ([Scheme_Seq],' + @pivot_table_column_list + ',[Total]) ' + @sql_script  
 EXECUTE(@sql_script)  
  
 -- Create Result Table for Part 1  
 CREATE TABLE #ResultTable_01 (  
  KeyJoin	varchar(10),  
  Seq		SMALLINT,  
  Col1		VARCHAR(100)  DEFAULT ''
 )  
  
 -- Add Column for Result Table dynamically  
 EXECUTE('ALTER TABLE #ResultTable_01 ADD ' + @pivot_table_column_header)  
  
 -- Create Column Header dynamically  
 SET @sql_script = 'INSERT INTO #ResultTable_01 ([KeyJoin],[Seq],[Col1],' + @pivot_table_column_1st + ') VALUES (''header1'',10,'''',''No. of transactions'')'  
 EXECUTE(@sql_script)  
  
 SET @sql_script = 'INSERT INTO #ResultTable_01 ([KeyJoin],[Seq],[Col1],' + @pivot_table_column_list + ',[Total]) VALUES (''header2'',11,''Period'',' + @pivot_table_column_name_value + ')'  
 EXECUTE(@sql_script)  


 -- Generate Result Table  
 SET @sql_script = 'SELECT P.[Scheme_Seq],12,P.[Scheme_Seq],' + @pivot_table_column_name_alias + ' FROM  
  #Period_Temp P  
   LEFT JOIN #PivotTable PT  
    ON P.[Scheme_Seq] = PT.[Scheme_Seq] COLLATE DATABASE_DEFAULT'  
  
 SET @sql_script = 'INSERT INTO #ResultTable_01 ([KeyJoin],[Seq],[Col1],' + @pivot_table_column_list + ',[Total]) ' + @sql_script  
 EXECUTE (@sql_script)  


UPDATE #ResultTable_01
 SET [Col1]= (CONVERT(varchar, sg.Claim_Period_From, 111) + ' - ' + CONVERT(varchar, sg.Last_Service_Dtm, 111))
 FROM  #ResultTable_01 r
 INNER JOIN (SELECT Scheme_Seq,Claim_Period_From,Last_Service_Dtm 
			 FROM SubsidizeGroupClaim WHERE Subsidize_Code='EHCVS' 
			 GROUP BY Scheme_Seq,Claim_Period_From,Last_Service_Dtm 
			 ) sg
 ON CONVERT(varchar, sg.Scheme_Seq) = r.[KeyJoin]

  
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
  Scheme_Seq,  
  Scheme_Display_Code,  
  No_Of_Trans
 )  
 SELECT    
  Scheme_seq,
  Scheme_Code,
  COUNT(1)
 FROM #tempAccount
 WHERE Subsidize_item_code='EHCVS'
 GROUP BY  
	Subsidize_Item_Code, Scheme_Seq, Scheme_Code

 -- Generate Pivot Table for Result  
 SET @sql_script = 'SELECT *
  FROM (  
   SELECT  
    Scheme_Seq,  
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
  
 SET @sql_script = 'INSERT INTO #PivotTable ([Scheme_Seq],' + @pivot_table_column_list + ') ' + @sql_script  
 EXECUTE(@sql_script)  
 

 --Update Total Account
 UPDATE #PivotTable
 SET Total = TA.Total
 FROM #PivotTable PT
 INNER JOIN
 (SELECT count(1) AS Total, Scheme_Seq
 FROM
 (SELECT 
  encrypt_field1,
  doc_code,
  Scheme_Seq
 FROM #tempAccount
 WHERE Subsidize_item_code='EHCVS'
 GROUP BY  
  encrypt_field1, doc_code, Scheme_Seq
  )g  
  GROUP BY Scheme_Seq
  ) TA ON PT.Scheme_Seq = TA.Scheme_Seq
 
  

 -- Create Result Table for Part 2  
 CREATE TABLE #ResultTable_02 (  
  KeyJoin	varchar(10),  
  Seq		SMALLINT,  
  Col1		VARCHAR(100)  DEFAULT ''
 )  
  
 -- Add Column for Result Table dynamically  
 EXECUTE('ALTER TABLE #ResultTable_02 ADD ' + @pivot_table_column_header)  
  
 -- Create Column Header dynamically  
 SET @sql_script = 'INSERT INTO #ResultTable_02 ([KeyJoin],[Seq],[Col1],' + @pivot_table_column_1st + ') VALUES (''header1'',10,'''',''No. of eHealth (Subsidies) Account involve'')'  
 EXECUTE(@sql_script)  
  
 SET @sql_script = 'INSERT INTO #ResultTable_02 ([KeyJoin],[Seq],[Col1],' + @pivot_table_column_list + ',[Total]) VALUES (''header2'',11,''Period'',' + @pivot_table_column_name_value + ')'  
 EXECUTE(@sql_script)  


 -- Generate Result Table  
 SET @sql_script = 'SELECT P.[Scheme_Seq],12,P.[Scheme_Seq],' + @pivot_table_column_name_alias + ' FROM  
  #Period_Temp P  
   LEFT JOIN #PivotTable PT  
    ON P.[Scheme_Seq] = PT.[Scheme_Seq] COLLATE DATABASE_DEFAULT'  
  
 SET @sql_script = 'INSERT INTO #ResultTable_02 ([KeyJoin],[Seq],[Col1],' + @pivot_table_column_list + ',[Total]) ' + @sql_script  
 EXECUTE (@sql_script)  

  
 -- Release Resource  
 TRUNCATE TABLE #ResultData  
 TRUNCATE TABLE #PivotTable  
  
-- ---------------------------------------------  
-- Part 2 (End)  
-- --------------------------------------------- 


-- =============================================
-- Construct layout
-- =============================================

	INSERT INTO #ResultTable_01 ([KeyJoin],[Seq],[Col1]) VALUES	
	('',0, 'eHS(S)D0021-05: Summary of voucher claim break down by period')
	
	INSERT INTO #ResultTable_01 ([KeyJoin],[Seq]) VALUES	
	('',1)

	INSERT INTO #ResultTable_01 ([KeyJoin],[Seq],[Col1]) VALUES	
	('',2, 'Reporting period: as at ' +CONVERT(varchar, DATEADD(dd, -1, @Report_Dtm), 111)  )

	INSERT INTO #ResultTable_01 ([KeyJoin],[Seq]) VALUES	
	('',3)

	INSERT INTO #ResultTable_01 ([KeyJoin],[Seq]) VALUES	
	('',18)

	INSERT INTO #ResultTable_01 ([KeyJoin],[Seq],[Col1]) VALUES	
	('',19, 'Notes:')

	INSERT INTO #ResultTable_01 ([KeyJoin],[Seq],[Col1]) VALUES	
	('',20, 'i) Invalidated transactions, removed and voided transactions are excluded')



-- =============================================  
-- Return results  
-- =============================================  

 SET @sql_script = 'SELECT RT1.[Col1],' + @pivot_table_column_name_alias1 + ',' + @pivot_table_column_name_alias2 + ' 
  FROM #ResultTable_01 RT1  
	LEFT JOIN #ResultTable_02 RT2  
	ON RT1.KeyJoin = RT2.KeyJoin 
  ORDER BY RT1.Seq, RT1.Col1'  
  
 EXECUTE(@sql_script)


 -- Release Resource  
 DROP TABLE #ResultData  
 DROP TABLE #PivotTable  
  
 DROP TABLE #ResultTable_01  
 DROP TABLE #ResultTable_02  
  
 DROP TABLE #Period_Temp
 	
--------------------------------------------------------------
--	DECLARE @ResultTable4 table (
--		Result_Seq		smallint,	
--		Result_Value2	varchar(100) DEFAULT '',
--		Result_Value3	varchar(100) DEFAULT '',
--		Result_Value4	varchar(100) DEFAULT ''

--	)

--	DECLARE @AllPeriod_HCVS TABLE (
--		Scheme_Seq		smallint
--	)

--	DECLARE @ReportTempResult TABLE (
--		Scheme_Seq		smallint,
--		No_Of_Trans		int
--	)

----DECLARE @tempAccount table
----	(
----		encrypt_field1			varbinary(100),
----		Doc_code				varchar(20),
----		Subsidize_item_code		varchar(10),
----		scheme_seq				int
----		
----	)

---- =============================================
---- Retrieve data
---- =============================================

--	INSERT INTO @AllPeriod_HCVS (Scheme_Seq)
--	SELECT Scheme_Seq FROM SubsidizeGroupClaim
--	WHERE	Scheme_Code = 'HCVS'
--			AND Subsidize_Code = 'EHCVS'
--			AND Claim_Period_From <= @Report_Dtm
--			AND Record_Status = 'A'

--	INSERT INTO @ReportTempResult (Scheme_Seq, No_Of_Trans)
--	SELECT Scheme_seq ,count(1) from #tempAccount
--	where scheme_code='HCVS' Group by subsidize_item_code, Scheme_seq

----Got Voucher eHealth Account 

----insert into @tempAccount
----select distinct encrypt_field1,
----case rtrim(doc_code) when 'HKBC' then 'HKIC' else doc_code end, 
----rtrim(subsidize_item_code),
----scheme_seq from view_vouchertranacc where scheme_code='HCVS' and Transaction_Dtm < @Report_Dtm


---- =============================================
---- Construct layout
---- =============================================


--	INSERT INTO @ResultTable4 (Result_Seq, Result_Value2) VALUES
--	(0, 'eHSD0021-05: Summary of voucher claim break down by period')
	
--	INSERT INTO @ResultTable4 (Result_Seq) VALUES
--	(1)
		
--	INSERT INTO @ResultTable4 (Result_Seq, Result_Value2) VALUES
--	(2, 'Reporting period: as at ' +CONVERT(varchar, DATEADD(dd, -1, @Report_Dtm), 111)  )
--	INSERT INTO @ResultTable4 (Result_Seq) VALUES
--	(3)
--	insert into @ResultTable4 (Result_Seq, Result_Value2,Result_Value3,Result_Value4) 
--	values( 4 , 'Period' ,'No. of Transaction','No. of eHealth Account involve')

	
--	insert into @ResultTable4 (Result_Seq, Result_Value2,Result_Value3,Result_Value4)
--	SELECT 5, AP.Scheme_Seq, 0, ISNULL(RTR.No_Of_Trans, 0)
--	FROM @AllPeriod_HCVS AP
--		LEFT JOIN @ReportTempResult RTR
--			ON AP.Scheme_Seq = RTR.Scheme_Seq
--	order by AP.Scheme_Seq


--	update @ResultTable4 set Result_Value3=  T.cnt from @ResultTable4 r, 
--	(select count(1) as cnt, vd.scheme_seq from vouchertransaction vt,transactiondetail vd
--	where transaction_dtm<@Report_Dtm And Record_Status Not in ('I', 'D','W') 
--	And (Invalidation IS NULL OR Invalidation Not in ('I'))  and vt.scheme_code='hcvs'
--	and vt.transaction_id=vd.transaction_id and vt.scheme_code=vd.scheme_code
--	group by vd.scheme_code , vd.scheme_seq, vd.subsidize_item_code
--	) as T
--	where  T.scheme_seq=r.Result_Value2 and r.Result_seq>=5


--	update @ResultTable4
--	 set Result_Value2= (CONVERT(varchar, sg.claim_period_from, 111) + ' - ' + CONVERT(varchar, sg.Last_Service_Dtm, 111))
--	 from  subsidizegroupclaim sg,@ResultTable4 r
--	 where sg.scheme_seq=r.Result_Value2
--	 and sg.scheme_code='HCVS' and r.Result_seq>=5



--	INSERT INTO @ResultTable4 (Result_Seq) VALUES
--	(12)
--	INSERT INTO @ResultTable4 (Result_Seq) VALUES
--	(13)	

--INSERT INTO @ResultTable4 (Result_Seq,Result_Value2,Result_Value3 ) VALUES
--	(19, 'Notes:','')
--	INSERT INTO @ResultTable4 (Result_Seq,Result_Value2,Result_Value3 ) VALUES
--	(20, 'i) Invalidated transactions, removed and voided transactions are excluded','')

---- ---------------------------------------------
---- Select Data
---- ---------------------------------------------



--	SELECT 		
--		Result_Value2,
--		Result_Value3,
--		Result_Value4
--	FROM
--		@ResultTable4
--	ORDER BY
--		Result_Seq


set nocount off

END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0021-05] TO HCVU
GO
