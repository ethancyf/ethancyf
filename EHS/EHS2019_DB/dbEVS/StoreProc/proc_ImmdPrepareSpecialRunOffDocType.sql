     if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[proc_ImmdPrepareSpecialRunOffDocType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[proc_ImmdPrepareSpecialRunOffDocType]
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Author:		Paul Yip
-- Create date: 22-2-2010
-- Description:	Process SpecialAccount & SpecialPersonalInformation
--				For ImmD File: (For HKBC, Doc/I, REPMT, VISA, ADOPC)
-- =============================================
CREATE PROCEDURE [dbo].[proc_ImmdPrepareSpecialRunOffDocType]
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================

DECLARE @tempDocCode as char(20) 
DECLARE @File_Name_Prefix as Varchar(50)

-- Current Date & Cut off DateTime
--DECLARE @curDate as datetime
--For dev, T+1, for live, T
--Set @curDate = GetDate()
-- hardcode testing start
--Set @curDate = Dateadd(day, 1, GetDate())
-- hardcode testing end

--DECLARE @str_cut_off as varchar(30) 
--DECLARE @str_cut_off_time as varchar(30) 

--SELECT @str_cut_off_time = Parm_Value1 FROM [SystemParameters] WHERE Parameter_Name = 'ImmdExportFileCutoffTime' AND [Scheme_Code] = 'ALL'
--Set @str_cut_off = CONVERT(nvarchar(11), @curDate, 106) + ' ' + RTRIM(LTRIM(@str_cut_off_time))

--Temp Table For IMMD settings
DECLARE @tempSettings Table
(
	Join_Doc_Code char(20), 
	Request_File_prefix varchar(30), 
	Record_max_size integer
)


-- Max Record Limit
DECLARE @record_num integer
--DECLARE @remain_num integer


-- Export File Name
DECLARE @file_name varchar(100)

--DECLARE @temp_record_acc_id char(15)
--DECLARE @temp_app_seq_no integer

--DECLARE @dtmCur as DateTime
--Set @dtmCur = GetDate()

DECLARE @DocType_EOF char(1)
--DECLARE @tempSubmission_EOF char(1)

DECLARE @No_of_File integer
DECLARE @Starting_id_num integer
DECLARE @Ending_id_num integer


DECLARE @temp_file_name_prefix as Varchar(50)
DECLARE @temp_record_num integer

SELECT @temp_file_name_prefix = Parm_Value1 FROM [SystemParameters] WHERE [Parameter_Name] = 'ImmdRequestFilePrefix_HKBC' 
SELECT @temp_record_num = CONVERT(INTEGER,Parm_Value1) FROM [SystemParameters] WHERE [Parameter_Name] = 'ImmdRecordMaxSize_HKBC'
INSERT INTO @tempSettings VALUES ('HKBC', @temp_file_name_prefix, @temp_record_num)
		
SELECT @temp_file_name_prefix = Parm_Value1 FROM [SystemParameters] WHERE [Parameter_Name] = 'ImmdRequestFilePrefix_DOCI' 
SELECT @temp_record_num = CONVERT(INTEGER,Parm_Value1) FROM [SystemParameters] WHERE [Parameter_Name] = 'ImmdRecordMaxSize_DOCI'
INSERT INTO @tempSettings VALUES ('Doc/I', @temp_file_name_prefix, @temp_record_num)
		
SELECT @temp_file_name_prefix = Parm_Value1 FROM [SystemParameters] WHERE [Parameter_Name] = 'ImmdRequestFilePrefix_REPMT' 
SELECT @temp_record_num = CONVERT(INTEGER,Parm_Value1) FROM [SystemParameters] WHERE [Parameter_Name] = 'ImmdRecordMaxSize_REPMT'
INSERT INTO @tempSettings VALUES ('REPMT', @temp_file_name_prefix, @temp_record_num)
		
SELECT @temp_file_name_prefix = Parm_Value1 FROM [SystemParameters] WHERE [Parameter_Name] = 'ImmdRequestFilePrefix_VISA' 
SELECT @temp_record_num = CONVERT(INTEGER,Parm_Value1) FROM [SystemParameters] WHERE [Parameter_Name] = 'ImmdRecordMaxSize_VISA'
INSERT INTO @tempSettings VALUES ('VISA', @temp_file_name_prefix, @temp_record_num)
		
SELECT @temp_file_name_prefix = Parm_Value1 FROM [SystemParameters] WHERE [Parameter_Name] = 'ImmdRequestFilePrefix_ADOPC' 
SELECT @temp_record_num = CONVERT(INTEGER,Parm_Value1) FROM [SystemParameters] WHERE [Parameter_Name] = 'ImmdRecordMaxSize_ADOPC'
INSERT INTO @tempSettings VALUES ('ADOPC', @temp_file_name_prefix, @temp_record_num)	

--DECLARE @DI_DOI AS DATETIME  
--DECLARE @REPMT_DOI AS DATETIME  
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

DECLARE csr_DocType CURSOR FOR  
SELECT Join_Doc_Code, Request_File_prefix, Record_max_size
FROM @tempSettings
--WHERE Join_Doc_Code in ('HKBC', 'Doc/I', 'REPMT', 'VISA', 'ADOPC') 
--SELECT Doc_Code, Immd_File_Name, Immd_Max_Size   
--FROM DocType
--WHERE Doc_Code in ('HKBC', 'Doc/I', 'REPMT', 'VISA', 'ADOPC')
--AND Force_Manual_Validate = 'N'
   
SET @DocType_EOF = 'N'

--SELECT @DI_DOI = CONVERT(DateTime, Parm_Value1) FROM [SystemParameters] WHERE [Parameter_Name] = 'DI_DOI' AND [Scheme_Code] = 'ALL'  
--SELECT @REPMT_DOI = CONVERT(DateTime, Parm_Value1) FROM [SystemParameters] WHERE [Parameter_Name] = 'REPMT_DOI' AND [Scheme_Code] = 'ALL'

OPEN csr_DocType  
FETCH NEXT FROM csr_DocType INTO @tempDocCode, @File_Name_Prefix, @record_num

--SET @record_num = 4

IF @@fetch_status <> 0
BEGIN
	SET @DocType_EOF = 'Y'
END


WHILE @DocType_EOF <> 'Y'  
BEGIN  

	EXEC proc_ImmdPrepareSpecialRunOffRecord @tempDocCode, @File_Name_Prefix, @record_num

	FETCH NEXT FROM csr_DocType INTO @tempDocCode, @File_Name_Prefix, @record_num    
	
	IF @@fetch_status <> 0
	BEGIN
		SET @DocType_EOF = 'Y'
	END
	
END

CLOSE csr_DocType  
DEALLOCATE csr_DocType  

END
GO


