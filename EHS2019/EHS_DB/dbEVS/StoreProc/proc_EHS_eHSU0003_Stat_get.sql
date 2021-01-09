IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSU0003_Stat_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSU0003_Stat_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	23 Oct 2019
-- CR No.:			INT19-021
-- Description:		Handle Scheme Display Code with "-"
--					- Use Scheme Code for temp table's column name
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	18 November 2015
-- CR No.:			CRE15-006
-- Description:		Rename of eHS
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE14-002 - PPI-ePR Migration
-- Modified by:		Tommy LAM
-- Modified date:	24 Mar 2014
-- Description:		Change code of [Token].[Project] from "EHCVS" to "EHS"
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE13-003-01
-- Modified by:		Koala CHENG
-- Modified date:	12 Aug 2012
-- Description:		Add criteria worksheet
-- =============================================
-- Author:		Karl LAM
-- Create date: 22 May 2013
-- Description:	Get Stat for report eHSD0003
-- ============================================= 
--exec proc_EHS_eHSU0003_Stat_get '', '2013-07-11'

CREATE PROCEDURE [dbo].[proc_EHS_eHSU0003_Stat_get]  
	@ProfessionList varchar(5000),
	@request_time datetime
AS BEGIN  

 SET NOCOUNT ON;  

-- =============================================  
-- Declaration  
-- =============================================  
  
DECLARE @wsContent varchar(30)    
DECLARE @wsCriteria varchar(30)    
DECLARE @ws01 varchar(30)    
DECLARE @wsRemark varchar(30)   
  
DECLARE @wsContent_ct int      
DECLARE @wsCriteria_ct int      
DECLARE @ws01_ct int      
DECLARE @wsRemark_ct int   
  
DECLARE @Report_ID Char(8)  
SET @Report_ID = 'eHSU0003'  
  
DECLARE @Project_Code_EHS char(5)  
SET @Project_Code_EHS = 'EHS'  
  
---- init worksheet key    
set @wsContent = 'Content'    
set @wsCriteria = 'Criteria'    
set @ws01 = '01'    
set @wsRemark = 'Remark'    
  
set @wsContent_ct = 1  
set @wsCriteria_ct = 1  
set @ws01_ct = 1  
set @wsRemark_ct = 1  
  
  
-- =============================================  
-- Report Setting  
-- =============================================  
  
IF @request_time IS NULL BEGIN    
  SET @request_time = DATEADD(dd, -1, CONVERT(VARCHAR(11), GETDATE(), 106)) -- "106" gives "dd MMM yyyy"    
END    
    
DECLARE @reporting_period as varchar(50)      
DECLARE @reporting_dtm as datetime  
      
SET @reporting_period = 'Reporting period: as at ' + CONVERT(VARCHAR(10), @request_time, 111)      
SELECT @reporting_dtm = Cast(CONVERT(VARCHAR(10), @request_time, 111) + ' 23:59:59' as datetime)  
  
--Get profession list  
DECLARE @tbl_Profession table   
(Service_Category_Code  varchar(3))  
  
DECLARE @Profession varchar(5001)  
SELECT @Profession = @ProfessionList + ','  
  
DECLARE @prof_substring varchar(50)  
DECLARE @Index INT   
SET @Index = -1  
  
WHILE (LEN(@Profession) > 0)  
BEGIN  
 SET @Index = CHARINDEX(',' , @Profession)  
  
 IF (@Index > 1 )  
 BEGIN  
  SET @prof_substring = LEFT(@Profession, @Index - 1)  
   
  INSERT INTO @tbl_Profession SELECT ltrim(rtrim((@prof_substring)))  
  
  SET @Profession = RIGHT(@Profession, (LEN(@Profession) -   @Index))  
 END  
 ELSE  
 BREAK  
 END  
  
  
  
---- Prepare ResultSet ----     
CREATE TABLE #WorkBook 
(    
WorkSheetID varchar(30),    
Result01 varchar(300) default '',    --SPID, used for joining in prepare SP Scheme Data
Result02 varchar(1000) default '',    
Result03 nvarchar(100) default '',    
Result04 varchar(100) default '',    
Result05 varchar(100) default '',    
Result06 nvarchar(300) default '',    
Result07 varchar(200) default '',    
Result08 varchar(100) default '',    
Result09 varchar(100) default '',    
Result10 varchar(100) default '',    
Result11 varchar(100) default '',    
Result12 varchar(100) default '',    
Result13 varchar(100) default '',    
Result14 varchar(100) default '',    
Result15 varchar(100) default '',    
Result16 varchar(100) default '',    
Result17 varchar(100) default '',    
Result18 varchar(100) default '',    
Result19 varchar(100) default '',     
Result20 varchar(100) default '',  
Result21 varchar(100) default '',  
Result22 varchar(100) default '',  
Result23 varchar(100) default '',  
Result24 varchar(100) default '',  
Result25 varchar(100) default '',  
Result26 varchar(100) default '',  
Result27 varchar(100) default '',  
Result28 varchar(100) default '',  
Result29 varchar(100) default '',  
Result30 varchar(100) default '',  
Result31 varchar(100) default '',  
Result32 varchar(100) default '',  
Result33 varchar(100) default '',  
Result34 varchar(100) default '',  
Result35 varchar(100) default '',  
Result36 varchar(100) default '',  
Result37 varchar(100) default '',  
Result38 varchar(100) default '',  
Result39 varchar(100) default '',  
Result40 varchar(100) default '',  
DisplaySeq int       
)    
  
  
 -- =============================================  
-- Build frame  
-- =============================================  
  
---- Generate static layout ----      
---- Content    
  
INSERT INTO #WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, 'Sub Report ID','Sub Report Name',@wsContent_ct)   
SELECT @wsContent_ct=@wsContent_ct+1  
INSERT INTO #WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, 'eHS(S)U0003-01','Service Provider With Token Pending Activation Summary',@wsContent_ct)    
SELECT @wsContent_ct=@wsContent_ct+1  
INSERT INTO #WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, '','',@wsContent_ct)    
SELECT @wsContent_ct=@wsContent_ct+1  
INSERT INTO #WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, '','',@wsContent_ct)    
SELECT @wsContent_ct=@wsContent_ct+1  
INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@wsContent, 'Report Generation Time: ' + CONVERT(VARCHAR(10), getdate(), 111) + ' ' + CONVERT(VARCHAR(5), getdate(), 114),@wsContent_ct)    
SELECT @wsContent_ct=@wsContent_ct+1  
  
--01 sub Report  
INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq)     
VALUES (@ws01, 'eHS(S)U0003-01: Service Provider With Token Pending Activation Summary' , @ws01_ct)    
SELECT @ws01_ct=@ws01_ct + 1  
INSERT INTO #WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws01, @ws01_ct)    
SELECT @ws01_ct=@ws01_ct + 1    
INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@ws01, @reporting_period, @ws01_ct)      
SELECT @ws01_ct=@ws01_ct + 1    
INSERT INTO #WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws01, @ws01_ct)  

 -- =============================================  
-- Prepare Dynamic Sql 
-- =============================================  
DECLARE @tbl_ValidScheme Table (
Scheme_Code char(10),
Display_Code char(25),
Display_Seq int,
Scheme_Desc	varchar	(100))

DECLARE @tbl_Mapping Table (
Display_Seq int,
WS_Col varchar(100),
Scheme_Code varchar(10))

CREATE TABLE #SP_Scheme_Data (
SP_ID char(8),
Scheme_Code char(10))

CREATE TABLE #TBL_SP_Scheme_Consolidate (SP_ID char(8))

DECLARE @NAME_SP_Scheme_Consolidate char(26)
SET @NAME_SP_Scheme_Consolidate = '#TBL_SP_Scheme_Consolidate'

--Find out valid scheme
INSERT INTO @tbl_ValidScheme
SELECT Distinct Scheme_Code, Display_Code , Display_Seq , Scheme_Desc
FROM SchemeBackOffice 
Where Record_Status = 'A' 
ORDER BY Display_Seq 

--Prepare mapping table for later use
INSERT INTO @tbl_Mapping (Display_Seq,WS_Col, Scheme_Code) 
SELECT Display_Seq, null, Scheme_Code FROM @tbl_ValidScheme ORDER BY Display_Seq

--Find out Scheme Count
DECLARE @Scheme_Cnt int
SELECT @Scheme_Cnt = Count(Scheme_Code) FROM @tbl_ValidScheme

--Prepare Result Workbook columns
DECLARE @Result_Values varchar(2000)
DECLARE @Result_Values_Cnt int
SET @Result_Values_Cnt = 0
SELECT @Result_Values = '''' + @ws01 + ''''

SELECT @Result_Values = @Result_Values + ', ''SPID'''
SELECT @Result_Values_Cnt = @Result_Values_Cnt + 1

SELECT @Result_Values = @Result_Values + ', ''SP Name (In English)'''
SELECT @Result_Values_Cnt = @Result_Values_Cnt + 1

SELECT @Result_Values = @Result_Values + ', ''SP Name (In Chinese)'''
SELECT @Result_Values_Cnt = @Result_Values_Cnt + 1

SELECT @Result_Values = @Result_Values + ', ''Profession'''
SELECT @Result_Values_Cnt = @Result_Values_Cnt + 1

SELECT @Result_Values = @Result_Values + ', ''SP Status'''
SELECT @Result_Values_Cnt = @Result_Values_Cnt + 1

SELECT @Result_Values = @Result_Values + ', ''Token Replacement Time'''
SELECT @Result_Values_Cnt = @Result_Values_Cnt + 1

SELECT @Result_Values = @Result_Values + ', ''Correspondence Address'''
SELECT @Result_Values_Cnt = @Result_Values_Cnt + 1

SELECT @Result_Values = @Result_Values + ', ''Daytime Contact Phone No.'''
SELECT @Result_Values_Cnt = @Result_Values_Cnt + 1

--Prepare insert statement
DECLARE @AllSchemes VARCHAR(4000)
SELECT @AllSchemes = COALESCE(@AllSchemes + ',', '') + '''' +  rtrim(Display_Code) + ''''
FROM @tbl_ValidScheme
ORDER BY Display_Seq 

DECLARE @Insert_Columns varchar(2000)
SET @Insert_Columns = 'WorkSheetID,'
        
DECLARE @intFlag int
DECLARE @Result_Col_Name varchar(10)
SET @intFlag = 1

WHILE (@intFlag <= @Scheme_Cnt + @Result_Values_Cnt)
BEGIN 
	SELECT @Result_Col_Name = 'Result'+ right('00' + cast(@intFlag as varchar(2)), 2)
	SELECT @Insert_Columns = @Insert_Columns + @Result_Col_Name  + ','
	--update mapping table for later use
	UPDATE @tbl_Mapping SET WS_Col = @Result_Col_Name WHERE Display_Seq = (@intFlag - @Result_Values_Cnt)
	SELECT @intFlag = @intFlag + 1
END
      
SELECT @Insert_Columns =  @Insert_Columns +  'DisplaySeq'

DECLARE @sql varchar(2000)
SET @sql =   
'INSERT INTO #WorkBook (' + @Insert_Columns + ') VALUES (' +
@Result_Values + ',' + @AllSchemes +  ',' + cast(@ws01_ct as varchar) + ')'
EXECUTE  (@sql)

SELECT @ws01_ct=@ws01_ct + 1  

-- =============================================  
-- Prepare Data for eHSU0003-01  
-- =============================================  
EXEC [proc_SymmetricKey_open]
  
 INSERT INTO #WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, Result07, Result08, DisplaySeq)   
  
 SELECT @ws01 ,  
   t.User_ID,   
   [Eng_Name] =  convert(varchar(40), DecryptByKey(sp.[Encrypt_Field2])),    
   [Chi_Name] = convert(nvarchar, DecryptByKey(sp.[Encrypt_Field3])) ,    
   [Profession] =   isnull( Case isnull(@ProfessionList,'') when '' then  
        --when professionlist = '' then select the primary profession (profession seq No = 1)  
          (SELECT top 1 rtrim(p.Service_Category_Code)  
          FROM Professional p          
          WHERE   p.Record_Status = 'A'                   
           AND sp.SP_ID = p.SP_ID   
          ORDER BY Professional_Seq)  
        else  
        --when professionlist <> '' then select the highest priority profession (profession seq No) which matched with the criteria   
          (SELECT top 1 rtrim(p.Service_Category_Code)  
          FROM Professional p   
           INNER JOIN @tbl_Profession tp on p.Service_Category_Code = tp.Service_Category_Code  
          WHERE    p.Record_Status = 'A'                   
           AND sp.SP_ID = p.SP_ID   
          ORDER BY Professional_Seq)  
        end, 'N/A'),  
   [SP_Status] = sd.Status_Description,  
   [Last_replacement_Dtm] = CONVERT(VARCHAR(11), t.Last_replacement_Dtm, 106) + ' ' + CONVERT(VARCHAR(8), t.Last_replacement_Dtm, 108),  
   [Address] = dbo.func_formatEngAddress(sp.Room, sp.Floor, sp.Block, sp.Building, sp.District),  
   [Phone] = sp.Phone_Daytime,  
   @ws01_ct  
 FROM TOKEN t with(nolock) INNER JOIN  ServiceProvider sp with(nolock) on t.User_ID = sp.SP_ID  
     INNER JOIN StatusData sd with(nolock) on Enum_Class = 'SPAccountStatus' AND sp.Record_Status = sd.Status_Value  
 WHERE t.Last_replacement_Dtm is not null       
  AND t.Last_Replacement_Activate_Dtm is null  
  AND t.Last_replacement_Dtm <= @reporting_dtm  
  AND t.Project = @Project_Code_EHS        --ONLY RETRIEVE HCVS RECORD  
  AND sp.Record_Status in ('A', 'S')		 --ONLY RETRIEVE SP ACTIVE/SUSPEND RECORD  	
  AND (isnull(@ProfessionList,'') = ''   
    OR  EXISTS (  
     SELECT SP_ID   
     FROM  Professional p INNER JOIN @tbl_Profession tp on p.Service_Category_Code = tp.Service_Category_Code AND p.Record_Status = 'A'  
     WHERE sp.SP_ID = p.SP_ID  
    )  
   )  
 ORDER BY t.Last_replacement_Dtm, SP_ID, [Profession], sp.Record_Status  
  
  
EXEC [proc_SymmetricKey_close]
  
  
-- =============================================  
--Prepare SP Scheme Data
-- =============================================  

INSERT INTO #SP_Scheme_Data
SELECT	sp.SP_ID, vs.Scheme_Code 
FROM	SchemeInformation sp with(nolock) INNER JOIN #WorkBook wb on rtrim(sp.SP_ID) = rtrim(wb.Result01) collate DATABASE_DEFAULT 
		INNER JOIN @tbl_ValidScheme vs on rtrim(sp.Scheme_Code) = rtrim(vs.Scheme_Code) collate DATABASE_DEFAULT
WHERE	sp.Record_Status in ('A','S') --ONLY RETRIEVE SCHEME STATUS ACTIVE/SUSPEND

DECLARE @SchemeDataCol varchar(1000)
DECLARE @SchemeDataCol_AlterTable varchar(1000)

SELECT @SchemeDataCol = COALESCE(@SchemeDataCol + ', ','') + QUOTENAME(rtrim(Scheme_Code))
FROM  @tbl_ValidScheme 
GROUP BY Scheme_Code ORDER BY Scheme_Code

SELECT @SchemeDataCol_AlterTable = COALESCE(@SchemeDataCol_AlterTable + ', ','') + QUOTENAME(rtrim(Scheme_Code)) + ' int'
FROM  @tbl_ValidScheme 
GROUP BY Scheme_Code ORDER BY Scheme_Code

DECLARE @sql_alter_table varchar(2000)
SET @sql_alter_table  = 'ALTER TABLE ' + @NAME_SP_Scheme_Consolidate + ' ADD ' + @SchemeDataCol_AlterTable + ';'

EXEC(@sql_alter_table)

DECLARE @sql_pvt varchar(2000)
SET @sql_pvt= 'INSERT INTO ' + @NAME_SP_Scheme_Consolidate + '(SP_ID,' + @SchemeDataCol + ')' +
				' SELECT SP_ID,' + @SchemeDataCol + ' FROM (SELECT SP_ID,Scheme_Code FROM #SP_Scheme_Data) src 
				PIVOT (count(Scheme_Code) FOR Scheme_Code
				IN ('+ @SchemeDataCol +')) pvt ;'				
EXEC(@sql_pvt)

-- =============================================  
--	Update Result Table (By Scheme one by one)
-- =============================================  
DECLARE @WS_Col varchar(100)
DECLARE @Scheme_Code varchar(10)
DECLARE @sql_map varchar(2000)

DECLARE Cursor_upd CURSOR FOR 
        (select WS_Col, Scheme_Code FROM @tbl_Mapping)

OPEN Cursor_upd
	FETCH NEXT FROM Cursor_upd INTO @WS_Col,@Scheme_Code
	
WHILE @@FETCH_STATUS = 0
BEGIN
	SELECT @sql_map = 'UPDATE #Workbook SET ' + @WS_Col + 
					' = Case #TBL_SP_Scheme_Consolidate.' + QUOTENAME(RTRIM(@Scheme_Code)) + 
					' when 0 then ''N'' when null then ''N'' else ''Y'' end ' +
					'FROM #TBL_SP_Scheme_Consolidate WHERE #Workbook.Result01 = #TBL_SP_Scheme_Consolidate.SP_ID COLLATE DATABASE_DEFAULT 
					AND #Workbook.WorkSheetID = ''' + @ws01 + ''''
	Exec (@sql_map)
	
	FETCH NEXT FROM Cursor_upd INTO @WS_Col,@Scheme_Code
END

CLOSE Cursor_upd
DEALLOCATE Cursor_upd     

-- =============================================  
-- Prepare Remark worksheet   
-- =============================================  
INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq)     
VALUES (@wsRemark, '(A) Legend' , @wsRemark_ct)    --Excel Conditional Formatting: Font bolded if starts with ()
SELECT @wsRemark_ct=@wsRemark_ct + 1  

INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq)     
VALUES (@wsRemark, '1. Profession Type' , @wsRemark_ct)    
SELECT @wsRemark_ct=@wsRemark_ct + 1  

INSERT INTO #WorkBook  (WorkSheetID, Result01, Result02, DisplaySeq)     
SELECT	@wsRemark,
		rtrim(Service_Category_Code) as Service_Category_Code,
		rtrim(Service_Category_Desc) as Service_Category_Desc,
		@wsRemark_ct 
FROM Profession with(nolock)
ORDER BY Service_Category_Code
SELECT @wsRemark_ct=@wsRemark_ct + 1  

INSERT INTO #WorkBook (WorkSheetID, Result01,Result02, DisplaySeq)     
VALUES (@wsRemark, 'N/A' , 'Profession is not applicable when no active practice', @wsRemark_ct)    
SELECT @wsRemark_ct=@wsRemark_ct + 1  

INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq)     
VALUES (@wsRemark, '' , @wsRemark_ct)    
SELECT @wsRemark_ct=@wsRemark_ct + 1  

INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq)     
VALUES (@wsRemark, '2. Scheme Name' , @wsRemark_ct)    
SELECT @wsRemark_ct=@wsRemark_ct + 1  

INSERT INTO #WorkBook  (WorkSheetID, Result01, Result02, DisplaySeq)     
SELECT	@wsRemark,
		rtrim(Display_Code) as Display_Code,
		rtrim(Scheme_Desc) as Scheme_Desc,
		@wsRemark_ct 
FROM @tbl_ValidScheme
ORDER BY Display_Code
SELECT @wsRemark_ct=@wsRemark_ct + 1  


INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq)     
VALUES (@wsRemark, '' , @wsRemark_ct)    
SELECT @wsRemark_ct=@wsRemark_ct + 1  

INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq)     
VALUES (@wsRemark, '(B) Common Note(s) for the report' , @wsRemark_ct)    --Excel Conditional Formatting: Font bolded if starts with ()
SELECT @wsRemark_ct=@wsRemark_ct + 1  

INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq)     
VALUES (@wsRemark, '1. Listed profession in first active practice only ' , @wsRemark_ct)    
SELECT @wsRemark_ct=@wsRemark_ct + 1  




-- =============================================  
-- Prepare Data for eHSU0003-Criteria  
-- =============================================  

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

-- Insert workbook data
INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq)     
VALUES (@wsCriteria, 'Criteria' , @wsCriteria_ct)    
SELECT @wsCriteria_ct=@wsCriteria_ct + 1  

INSERT INTO #WorkBook (WorkSheetID, Result01, Result02, DisplaySeq)     
VALUES (@wsCriteria, 'Health Profession' , rtrim(@HealthProfessionList), @wsCriteria_ct)    
SELECT @wsCriteria_ct=@wsCriteria_ct + 1  

-- =============================================  
-- Get the resultset for whole workbook    
-- =============================================  

SELECT     
Result01, Result02, Result03, Result04, Result05,    
Result06, Result07, Result08, Result09, Result10,    
Result11, Result12, Result13, Result14, Result15,    
Result16, Result17, Result18, Result19, Result20     
FROM #WorkBook WHERE WorkSheetID = @wscontent    
ORDER BY DisplaySeq     
    
SELECT     
Result01, Result02  
FROM #WorkBook WHERE WorkSheetID = @wsCriteria    
ORDER BY DisplaySeq     

SELECT     
Result01, Result02, Result03, Result04, Result05,    
Result06, Result07, Result08, Result09, Result10,    
Result11, Result12, Result13, Result14, Result15,    
Result16, Result17, Result18, Result19, Result20, Result21    
FROM #WorkBook WHERE WorkSheetID = @ws01    
ORDER BY DisplaySeq     

SELECT     
Result01, Result02, Result03, Result04, Result05,    
Result06, Result07, Result08, Result09, Result10,    
Result11, Result12, Result13, Result14, Result15,    
Result16, Result17, Result18, Result19, Result20, Result21    
FROM #WorkBook WHERE WorkSheetID = @wsRemark   
ORDER BY DisplaySeq     

-- =============================================  
-- House Keeping  
-- =============================================  
DROP TABLE #WorkBook
DROP TABLE #SP_Scheme_Data
DROP TABLE #TBL_SP_Scheme_Consolidate

    
END     
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSU0003_Stat_get] TO HCVU
GO

