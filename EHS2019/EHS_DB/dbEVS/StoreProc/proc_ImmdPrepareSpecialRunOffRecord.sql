     if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[proc_ImmdPrepareSpecialRunOffRecord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[proc_ImmdPrepareSpecialRunOffRecord]
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go
-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Martin Tang
-- Modified date:	16 June 2021
-- Description:		Extend patient name's maximum length (varbinary 100->200)
-- =============================================
-- =============================================
-- Author:		Paul Yip
-- Create date: 22-2-2010
-- Description:	Process SpecialAccount & SpecialPersonalInformation
--				For ImmD File: (For HKBC, Doc/I, REPMT, VISA, ADOPC)
-- =============================================
CREATE PROCEDURE [dbo].[proc_ImmdPrepareSpecialRunOffRecord]
	@Doc_Code char(20),
	@File_Name_Prefix varchar(50),
	@record_num integer
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================

-- Temp Table For SubmissionLOG
DECLARE @tempSubmission Table 
(
	id_num int IDENTITY(1,1),
	Voucher_Acc_ID char(15),
	DOB varchar(10),
	Date_of_Issue varchar(10),
	Encrypt_Field1 varbinary(100),
	Encrypt_Field2 varbinary(200),
	Account_Purpose char(1),
	Sex char(1),
	Doc_Code char(20),
	Acc_Type char(1),
	Create_dtm datetime
)


-- Current Date & Cut off DateTime
DECLARE @curDate as datetime
--For dev, T+1, for live, T
Set @curDate = GetDate()
-- hardcode testing start
--Set @curDate = Dateadd(day, 1, GetDate())
-- hardcode testing end

DECLARE @str_cut_off as varchar(30) 
DECLARE @str_cut_off_time as varchar(30) 

SELECT @str_cut_off_time = Parm_Value1 FROM [SystemParameters] WHERE Parameter_Name = 'ImmdExportFileCutoffTime' AND [Scheme_Code] = 'ALL'
Set @str_cut_off = CONVERT(nvarchar(11), @curDate, 106) + ' ' + RTRIM(LTRIM(@str_cut_off_time))


-- Max Record Limit
--DECLARE @record_num integer
DECLARE @remain_num integer


-- Export File Name
DECLARE @file_name varchar(100)


DECLARE @dtmCur as DateTime
Set @dtmCur = GetDate()

DECLARE @DocType_EOF char(1)
DECLARE @tempSubmission_EOF char(1)

DECLARE @No_of_File integer
DECLARE @Starting_id_num integer
DECLARE @Ending_id_num integer

--DECLARE @DI_DOI AS DATETIME  
--DECLARE @REPMT_DOI AS DATETIME  

--DECLARE @Chk_REPMT_DOI char(1)  
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

/*
SELECT @DI_DOI = CONVERT(DateTime, Parm_Value1) FROM [SystemParameters] WHERE [Parameter_Name] = 'DI_DOI' AND [Scheme_Code] = 'ALL'  
SELECT @REPMT_DOI = CONVERT(DateTime, Parm_Value1) FROM [SystemParameters] WHERE [Parameter_Name] = 'REPMT_DOI' AND [Scheme_Code] = 'ALL'
SELECT @Chk_REPMT_DOI = Parm_Value1 FROM [SystemParameters] WHERE [Parameter_Name] = 'Chk_REPMT_DOI' AND [Scheme_Code] = 'ALL'
IF @Chk_REPMT_DOI = 'N' 
BEGIN
	SET @REPMT_DOI = CONVERT(DateTime, '1 Jan 1900')
END 
*/

-- INSERT THE Record To Temp Table 
-- 1. Insert Create New Account Case
	INSERT INTO @tempSubmission
	(
		Voucher_Acc_ID, 
		DOB,
		Date_of_Issue,
		Encrypt_Field1,
		Encrypt_Field2,
		Account_Purpose,
		Sex,
		Doc_Code,
		Acc_Type,
		Create_dtm 
	)
	SELECT
		Voucher_Acc_ID,
		DOB,
		Date_of_Issue,
		Encrypt_Field1,
		Encrypt_Field2,
		Account_Purpose,
		Sex,
		Doc_Code,
		Acc_Type,
		Create_Dtm
	from
	(
		SELECT 
			'S' as Acc_Type,
			SA.Special_Acc_ID As Voucher_Acc_ID, 
			CASE WHEN SPI.Exact_DOB = 'Y' THEN CONVERT(varchar(4),DATEPART(YEAR,SPI.DOB)) + '0000'
				WHEN SPI.Exact_DOB = 'M' THEN CONVERT(varchar(4),DATEPART(YEAR,SPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,SPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,SPI.DOB)) + '00'
				WHEN SPI.Exact_DOB = 'D' THEN CONVERT(varchar(4),DATEPART(YEAR,SPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,SPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,SPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,SPI.DOB)))) + CONVERT(varchar(2),DATEPART(DAY,SPI.DOB))
				WHEN SPI.Exact_DOB = 'R' THEN CONVERT(varchar(4),DATEPART(YEAR,SPI.DOB))
			ELSE '' END as DOB, 
			CASE WHEN SPI.Date_of_Issue Is NULL THEN ''
			ELSE CONVERT(varchar(4),DATEPART(YEAR,SPI.Date_of_Issue)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,SPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(MONTH,SPI.Date_of_Issue)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,SPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(DAY,SPI.Date_of_Issue)) END as Date_of_Issue, 
			SPI.Encrypt_Field1,
			SPI.Encrypt_Field2,
			--CASE WHEN SPI.Doc_Code = 'Doc/I' OR SPI.Doc_Code = 'REPMT' OR SPI.Doc_Code = 'HKBC' OR SPI.Doc_Code = 'ADOPC' THEN SPI.Encrypt_Field2 ELSE NULL END as Encrypt_Field2,
			SA.Account_Purpose,	
			SPI.Sex,		
			--CASE WHEN SPI.Doc_Code = 'Doc/I' OR SPI.Doc_Code = 'REPMT' THEN SPI.Sex ELSE NULL END as Sex,
			SPI.Doc_Code,
			SA.Create_Dtm as Create_Dtm
		FROM
			[dbo].[SpecialAccount] SA, [dbo].[SpecialPersonalInformation] SPI
		WHERE 
			SA.Special_Acc_ID = SPI.Special_Acc_ID 
			AND SPI.Doc_Code = @Doc_Code
			AND SA.Record_Status = 'P' AND (SPI.Validating ='N' OR SPI.Validating IS NULL) AND (SA.Account_Purpose = 'C' OR SA.Account_Purpose = 'V')
			AND (
				(SPI.Doc_Code = 'HKBC' AND SPI.Exact_DOB in ('D', 'M', 'Y')) OR 
				(SPI.Doc_Code = 'REPMT') OR 
				(SPI.Doc_Code = 'Doc/I') OR 
				--(SPI.Doc_Code = 'REPMT' AND SPI.Date_of_Issue >= @REPMT_DOI) OR 
				--(SPI.Doc_Code = 'Doc/I' AND SPI.Date_of_Issue >= @DI_DOI) OR 
				(SPI.Doc_Code = 'ADOPC' AND SPI.Exact_DOB in ('D', 'M', 'Y')) OR
				(SPI.Doc_Code = 'VISA') 
			)
			AND
			SA.Create_Dtm < @str_cut_off
	)  temp order by Voucher_Acc_ID asc

	
	
	--second loop
	Set @No_of_File = (Select Count(1) From @tempSubmission) / @record_num
	SET @remain_num = (Select Count(1) From @tempSubmission) % @record_num
	if (@remain_num > 0) 
		Set @No_of_File = @No_of_File + 1


	Set @Starting_id_num = 1
	Set @Ending_id_num = @record_num


	While (@No_of_File > 0)
	Begin
		
		Set @dtmCur = DateAdd(s, 1, @dtmCur)
		Set @curDate = @dtmCur

		Set @file_name = @File_Name_Prefix + 
			--REPLACE(UPPER(RTRIM(@Doc_Code)), '/', '') + '_' + 
			CONVERT(varchar(4),DATEPART(YEAR,@curDate)) + 
			LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,@curDate)))) + CONVERT(varchar(2),DATEPART(MONTH,@curDate)) +
			LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,@curDate)))) + CONVERT(varchar(2),DATEPART(DAY,@curDate)) +
			LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(HOUR,@curDate)))) + CONVERT(varchar(2),DATEPART(HOUR,@curDate)) +
			LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MINUTE,@curDate)))) + CONVERT(varchar(2),DATEPART(MINUTE,@curDate)) +
			LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(SECOND,@curDate)))) + CONVERT(varchar(2),DATEPART(SECOND,@curDate)) + '.xml'

			
		INSERT INTO [dbo].[TempVoucherAccSubmissionLOG]
		(
			System_Dtm,
			Voucher_Acc_ID,
			DOB,
			Date_of_Issue,
			Encrypt_Field1,
			Encrypt_Field2,
			Sex,
			Record_Status,		
			Doc_Code,
			File_name,
			Acc_Type
		)
		SELECT 
			@dtmCur,
			Voucher_Acc_ID,
			DOB,
			Date_of_Issue,
			Encrypt_Field1,
			Encrypt_Field2,
			Sex,
			CASE WHEN Account_Purpose = 'C' THEN 'N'
				WHEN Account_Purpose = 'V' THEN 'N'
				WHEN Account_Purpose = 'A' THEN 'A'
				WHEN Account_Purpose = 'O' THEN 'O'
				ELSE Account_Purpose END,
			Doc_Code,
			@file_name,
			Acc_Type
			
		FROM @tempSubmission
		WHERE id_num >= @Starting_id_num AND id_num <= @Ending_id_num
		ORDER BY Voucher_Acc_ID ASC

		INSERT INTO [dbo].[TempVoucherAccSubHeader]
		(
			System_Dtm, File_Name, Record_Status, Join_Doc_Code
		)
		VALUES
		(
			@dtmCur, @file_name, 'P', @Doc_Code
		)

		UPDATE [dbo].[SpecialPersonalInformation]
		Set 
			Validating = 'Y'
		WHERE 
			Special_Acc_ID IN
			(
				SELECT DISTINCT (Voucher_Acc_ID) FROM @tempSubmission WHERE id_num >= @Starting_id_num AND id_num <= @Ending_id_num
			)
		
		
		DELETE FROM @tempSubmission
		WHERE id_num >= @Starting_id_num AND id_num <= @Ending_id_num

		Set @Starting_id_num = @Starting_id_num + @record_num
		Set @Ending_id_num = @Ending_id_num + @record_num
		Set @No_of_File = @No_of_File - 1
	END


END
GO


