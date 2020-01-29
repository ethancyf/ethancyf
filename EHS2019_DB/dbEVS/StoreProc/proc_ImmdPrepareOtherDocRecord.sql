   if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[proc_ImmdPrepareOtherDocRecord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[proc_ImmdPrepareOtherDocRecord]
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Modification History
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Modified by:	    Chris YIM
-- Modified date:   04 Dec 2017
-- Description:		Not to send deceased account to ImmD 
-- =============================================
-- =============================================
-- Author:		Paul Yip
-- Create date: 1 Feb 2010
-- Description:	Process TempVoucherAccount & TempPersonalInformation, SpecialAccount & SpecialPersonalInformation
--				For ImmD File: (For HKBC, Doc/I, REPMT, VISA, ADOPC)
-- =============================================
CREATE PROCEDURE [dbo].[proc_ImmdPrepareOtherDocRecord]
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================

-- Temp Table For SubmissionLOG
DECLARE @tempSubmission Table 
(
	Voucher_Acc_ID char(15),
	DOB varchar(10),
	Date_of_Issue varchar(10),
	Encrypt_Field1 varbinary(100),
	Encrypt_Field2 varbinary(100),
	Account_Purpose char(1),
	Sex char(1),
	Doc_Code char(20),
	Acc_Type char(1),
	Create_dtm datetime
)

--Temp Table For IMMD settings
DECLARE @tempSettings Table
(
	Join_Doc_Code char(20), 
	Request_File_prefix varchar(30), 
	Record_max_size integer
)

DECLARE @tempSubmissionRemove Table 
(
	Voucher_Acc_ID char(15)
)

DECLARE @tempDocCode as char(20) 
DECLARE @File_Name_Prefix as Varchar(50)

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
DECLARE @record_num integer
DECLARE @remain_num integer

-- Export File Name
DECLARE @file_name varchar(100)
DECLARE @temp_record_acc_id char(15)

DECLARE @dtmCur as DateTime
Set @dtmCur = GetDate()

DECLARE @DocType_EOF char(1)
DECLARE @tempSubmission_EOF char(1)

DECLARE @DI_AccPurpose_Submit_Original char(1)
DECLARE @REPMT_AccPurpose_Submit_Original char(1)
DECLARE @HKBC_AccPurpose_Submit_Original char(1)
DECLARE @VISA_AccPurpose_Submit_Original char(1)
DECLARE @ADOPC_AccPurpose_Submit_Original char(1)

--------------------------------------------------------------------------------------
--For Testing Only
/*
set @DI_AccPurpose_Submit_Original = 'Y'
set @REPMT_AccPurpose_Submit_Original = 'Y'
set @HKBC_AccPurpose_Submit_Original = 'N'
set @VISA_AccPurpose_Submit_Original = 'Y'
set @ADOPC_AccPurpose_Submit_Original = 'N'
*/

SELECT @DI_AccPurpose_Submit_Original = Parm_Value1 FROM [SystemParameters] WHERE [Parameter_Name] = 'DOCI_AccPurposeSubmitOriginal' 
SELECT @REPMT_AccPurpose_Submit_Original = Parm_Value1 FROM [SystemParameters] WHERE [Parameter_Name] = 'REPMT_AccPurposeSubmitOriginal'
SELECT @HKBC_AccPurpose_Submit_Original = Parm_Value1 FROM [SystemParameters] WHERE [Parameter_Name] = 'HKBC_AccPurposeSubmitOriginal'  
SELECT @VISA_AccPurpose_Submit_Original = Parm_Value1 FROM [SystemParameters] WHERE [Parameter_Name] = 'VISA_AccPurposeSubmitOriginal'
SELECT @ADOPC_AccPurpose_Submit_Original = Parm_Value1 FROM [SystemParameters] WHERE [Parameter_Name] = 'ADOPC_AccPurposeSubmitOriginal'


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
--------------------------------------------------------------------------------------


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


   
SET @DocType_EOF = 'N'



OPEN csr_DocType  
FETCH NEXT FROM csr_DocType INTO @tempDocCode, @File_Name_Prefix, @record_num

IF @@fetch_status <> 0
BEGIN
	SET @DocType_EOF = 'Y'
END


--WHILE @@fetch_status = 0  
WHILE @DocType_EOF <> 'Y'  
BEGIN  

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
	SELECT TOP (@record_num)
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
			'T' as Acc_Type,
			TVA.Voucher_Acc_ID, 
			CASE WHEN TPI.Exact_DOB = 'Y' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + '0000'
				WHEN TPI.Exact_DOB = 'M' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + '00'
				WHEN TPI.Exact_DOB = 'D' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.DOB))
				WHEN TPI.Exact_DOB = 'R' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB))
			ELSE '' END as DOB, 
			CASE WHEN TPI.Date_of_Issue Is NULL THEN ''
				ELSE CONVERT(varchar(4),DATEPART(YEAR,TPI.Date_of_Issue)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)) END as Date_of_Issue, 
			TPI.Encrypt_Field1,
			TPI.Encrypt_Field2,
			--CASE WHEN TPI.Doc_Code = 'Doc/I' OR TPI.Doc_Code = 'REPMT' OR TPI.Doc_Code = 'HKBC' OR TPI.Doc_Code = 'ADOPC' THEN TPI.Encrypt_Field2 ELSE NULL END as Encrypt_Field2,
			TVA.Account_Purpose,
			TPI.Sex,
			--CASE WHEN TPI.Doc_Code = 'Doc/I' OR TPI.Doc_Code = 'REPMT' THEN TPI.Sex ELSE NULL END as Sex,
			TPI.Doc_Code,
			TVA.Create_Dtm as Create_Dtm
		FROM
			[dbo].[TempVoucherAccount] TVA, [dbo].[TempPersonalInformation] TPI
		WHERE 
			TVA.Voucher_Acc_ID = TPI.Voucher_Acc_ID 
			AND TPI.Doc_Code = @tempDocCode
			AND TVA.Record_Status = 'P' AND (TPI.Validating ='N' OR TPI.Validating IS NULL) AND (TVA.Account_Purpose = 'C' OR TVA.Account_Purpose = 'V')
			AND (
				(TPI.Doc_Code = 'HKBC' AND TPI.Exact_DOB in ('D', 'M', 'Y')) OR 
				(TPI.Doc_Code = 'REPMT') OR 
				(TPI.Doc_Code = 'Doc/I') OR 				
				--(TPI.Doc_Code = 'REPMT' AND TPI.Date_of_Issue >= @REPMT_DOI) OR 
				--(TPI.Doc_Code = 'Doc/I' AND TPI.Date_of_Issue >= @DI_DOI) OR 
				(TPI.Doc_Code = 'ADOPC' AND TPI.Exact_DOB in ('D', 'M', 'Y')) OR
				(TPI.Doc_Code = 'VISA') 
			)
			AND TVA.Create_Dtm < @str_cut_off
			AND TVA.Deceased IS NULL
	)  tempAcc


-- 2. Insert Amendment Case - HKBC, ADOPC 
	-- Only Amend Record is submitted

	Set @remain_num = @record_num - (Select Count(1) From @tempSubmission)

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
		Acc_Type
	)
	SELECT TOP (@remain_num)
		TVA.Voucher_Acc_ID, 
		CASE WHEN TPI.Exact_DOB = 'Y' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + '0000'
			WHEN TPI.Exact_DOB = 'M' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + '00'
			WHEN TPI.Exact_DOB = 'D' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.DOB))
			WHEN TPI.Exact_DOB = 'R' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB))
		ELSE '' END, 
		
		CASE WHEN TPI.Date_of_Issue Is NULL THEN ''
		ELSE CONVERT(varchar(4),DATEPART(YEAR,TPI.Date_of_Issue)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)) END as Date_of_Issue, 
		TPI.Encrypt_Field1,
		TPI.Encrypt_Field2,
		--CASE WHEN TPI.Doc_Code = 'Doc/I' OR TPI.Doc_Code = 'REPMT' OR TPI.Doc_Code = 'HKBC' OR TPI.Doc_Code = 'ADOPC' THEN TPI.Encrypt_Field2 ELSE NULL END as Encrypt_Field2,
		TVA.Account_Purpose,	
		TPI.Sex,
		--CASE WHEN TPI.Doc_Code = 'Doc/I' OR TPI.Doc_Code = 'REPMT' THEN TPI.Sex ELSE NULL END as Sex,
		TPI.Doc_Code,
		'T'
	FROM
		[dbo].[TempVoucherAccount] TVA, [dbo].[TempPersonalInformation] TPI
	WHERE 
		TVA.Voucher_Acc_ID = TPI.Voucher_Acc_ID AND 
		TPI.Doc_Code = @tempDocCode AND
		TVA.Record_Status = 'P' AND (TPI.Validating ='N' OR TPI.Validating IS NULL) AND 
		(TVA.Account_Purpose = 'A') AND 
		(
			(@REPMT_AccPurpose_Submit_Original = 'N' AND TPI.Doc_Code = 'REPMT') OR
			(@DI_AccPurpose_Submit_Original = 'N' AND TPI.Doc_Code = 'Doc/I') OR
			(@ADOPC_AccPurpose_Submit_Original = 'N' AND TPI.Doc_Code = 'ADOPC' AND TPI.Exact_DOB in ('D', 'M', 'Y')) OR
			(@HKBC_AccPurpose_Submit_Original = 'N' AND TPI.Doc_Code = 'HKBC' AND TPI.Exact_DOB in ('D', 'M', 'Y')) OR
			(@VISA_AccPurpose_Submit_Original = 'N' AND TPI.Doc_Code = 'VISA')   
		)
		AND TVA.Create_dtm < @str_cut_off
		AND TVA.Deceased IS NULL
	ORDER BY TVA.Create_Dtm ASC
	
-- 3. Insert Amendment Case - VISA, Doc/I, REPMT 
	-- Both Original and Amend Record are submitted

	if @remain_num % 2 = 1
		Set @remain_num =@remain_num - 1
	
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
		Acc_Type
		--Create_Dtm
	)
	SELECT TOP (@remain_num)
		TVA.Voucher_Acc_ID, 
		CASE WHEN TPI.Exact_DOB = 'Y' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + '0000'
			WHEN TPI.Exact_DOB = 'M' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + '00'
			WHEN TPI.Exact_DOB = 'D' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.DOB))
			WHEN TPI.Exact_DOB = 'R' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB))
		ELSE '' END, 
		
		CASE WHEN TPI.Date_of_Issue Is NULL THEN ''
		ELSE CONVERT(varchar(4),DATEPART(YEAR,TPI.Date_of_Issue)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)) END as Date_of_Issue, 
		TPI.Encrypt_Field1,
		TPI.Encrypt_Field2,
		--CASE WHEN TPI.Doc_Code = 'Doc/I' OR TPI.Doc_Code = 'REPMT' OR TPI.Doc_Code = 'HKBC' OR TPI.Doc_Code = 'ADOPC' THEN TPI.Encrypt_Field2 ELSE NULL END as Encrypt_Field2,
		TVA.Account_Purpose,		
		TPI.Sex,
		--CASE WHEN TPI.Doc_Code = 'Doc/I' OR TPI.Doc_Code = 'REPMT' THEN TPI.Sex ELSE NULL END as Sex,
		TPI.Doc_Code,
		'T'
	FROM
		[dbo].[TempVoucherAccount] TVA, [dbo].[TempPersonalInformation] TPI
	WHERE 
		TVA.Voucher_Acc_ID = TPI.Voucher_Acc_ID AND 
		TPI.Doc_Code = @tempDocCode AND
		TVA.Record_Status = 'P' AND 
		(TPI.Validating ='N' OR TPI.Validating IS NULL) AND 
		(TVA.Account_Purpose = 'A' OR TVA.Account_Purpose = 'O') AND 
		(
			(@REPMT_AccPurpose_Submit_Original = 'Y' AND TPI.Doc_Code = 'REPMT') OR
			(@DI_AccPurpose_Submit_Original = 'Y' AND TPI.Doc_Code = 'Doc/I') OR
			(@ADOPC_AccPurpose_Submit_Original = 'Y' AND TPI.Doc_Code = 'ADOPC' AND TPI.Exact_DOB in ('D', 'M', 'Y')) OR
			(@HKBC_AccPurpose_Submit_Original = 'Y' AND TPI.Doc_Code = 'HKBC'AND TPI.Exact_DOB in ('D', 'M', 'Y')) OR
			(@VISA_AccPurpose_Submit_Original = 'Y' AND TPI.Doc_Code = 'VISA')  
		)
		AND TVA.Create_dtm < @str_cut_off
		AND TVA.Deceased IS NULL
	ORDER BY TVA.Create_Dtm ASC
	
	
-- 4. Insert Special Account Case

	Set @remain_num = @record_num - (Select Count(1) From @tempSubmission)

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
	SELECT TOP (@remain_num)
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
			AND SPI.Doc_Code = @tempDocCode
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
			AND	SA.Create_Dtm < @str_cut_off
			AND SA.Deceased IS NULL
	)  SpecialAcc

	-----------------------------------------For VISA (Remove non-pair records if needed)-----------------------------------------
	IF @tempDocCode = 'VISA' and @VISA_AccPurpose_Submit_Original = 'Y'
	BEGIN
		INSERT INTO @tempSubmissionRemove
		(
			Voucher_Acc_ID
		)
		SELECT Voucher_Acc_ID FROM @tempSubmission
		WHERE Doc_Code = 'VISA'
		AND Account_Purpose = 'A'
		AND Encrypt_Field1 NOT IN (
			SELECT Encrypt_Field1 FROM @tempSubmission
			WHERE Doc_Code = 'VISA'
			AND Account_Purpose = 'O'
		)
		UNION ALL
		SELECT Voucher_Acc_ID FROM @tempSubmission
		WHERE Doc_Code = 'VISA'
		AND Account_Purpose = 'O'
		AND Encrypt_Field1 NOT IN (
			SELECT Encrypt_Field1 FROM @tempSubmission
			WHERE Doc_Code = 'VISA'
			AND Account_Purpose = 'A'
		)
		
		DELETE FROM @tempSubmission 
		WHERE Voucher_Acc_ID IN (
			SELECT Voucher_Acc_ID FROM @tempSubmissionRemove
		)
	END

	-----------------------------------------For REPMT (Remove non-pair records if needed)-----------------------------------------
	IF @tempDocCode = 'REPMT' AND @REPMT_AccPurpose_Submit_Original = 'Y'
	BEGIN
		INSERT INTO @tempSubmissionRemove
		(
			Voucher_Acc_ID
		)
		SELECT Voucher_Acc_ID FROM @tempSubmission
		WHERE Doc_Code = 'REPMT'
		AND Account_Purpose = 'A'
		AND Encrypt_Field1 NOT IN (
			SELECT Encrypt_Field1 FROM @tempSubmission
			WHERE Doc_Code = 'REPMT'
			AND Account_Purpose = 'O'
		)
		UNION ALL
		SELECT Voucher_Acc_ID FROM @tempSubmission
		WHERE Doc_Code = 'REPMT'
		AND Account_Purpose = 'O'
		AND Encrypt_Field1 NOT IN (
			SELECT Encrypt_Field1 FROM @tempSubmission
			WHERE Doc_Code = 'REPMT'
			AND Account_Purpose = 'A'
		)
		
		DELETE FROM @tempSubmission 
		WHERE Voucher_Acc_ID IN (
			SELECT Voucher_Acc_ID FROM @tempSubmissionRemove
		)
	END

	-----------------------------------------For Doc/I (Remove non-pair records if needed)-----------------------------------------
	IF @tempDocCode = 'Doc/I' AND @DI_AccPurpose_Submit_Original = 'Y'
	BEGIN
		INSERT INTO @tempSubmissionRemove
		(
			Voucher_Acc_ID
		)
		SELECT Voucher_Acc_ID FROM @tempSubmission
		WHERE Doc_Code = 'Doc/I'
		AND Account_Purpose = 'A'
		AND Encrypt_Field1 NOT IN (
			SELECT Encrypt_Field1 FROM @tempSubmission
			WHERE Doc_Code = 'Doc/I'
			AND Account_Purpose = 'O'
		)
		UNION ALL
		SELECT Voucher_Acc_ID FROM @tempSubmission
		WHERE Doc_Code = 'Doc/I'
		AND Account_Purpose = 'O'
		AND Encrypt_Field1 NOT IN (
			SELECT Encrypt_Field1 FROM @tempSubmission
			WHERE Doc_Code = 'Doc/I'
			AND Account_Purpose = 'A'
		)
		
		DELETE FROM @tempSubmission 
		WHERE Voucher_Acc_ID IN (
			SELECT Voucher_Acc_ID FROM @tempSubmissionRemove
		)
	END

	-----------------------------------------For HKBC (Remove non-pair records if needed)-----------------------------------------
	IF @tempDocCode = 'HKBC' AND @HKBC_AccPurpose_Submit_Original = 'Y'
	BEGIN
		INSERT INTO @tempSubmissionRemove
		(
			Voucher_Acc_ID
		)
		SELECT Voucher_Acc_ID FROM @tempSubmission
		WHERE Doc_Code = 'HKBC'
		AND Account_Purpose = 'A'
		AND Encrypt_Field1 NOT IN (
			SELECT Encrypt_Field1 FROM @tempSubmission
			WHERE Doc_Code = 'HKBC'
			AND Account_Purpose = 'O'
		)
		UNION ALL
		SELECT Voucher_Acc_ID FROM @tempSubmission
		WHERE Doc_Code = 'HKBC'
		AND Account_Purpose = 'O'
		AND Encrypt_Field1 NOT IN (
			SELECT Encrypt_Field1 FROM @tempSubmission
			WHERE Doc_Code = 'HKBC'
			AND Account_Purpose = 'A'
		)
		
		DELETE FROM @tempSubmission 
		WHERE Voucher_Acc_ID IN (
			SELECT Voucher_Acc_ID FROM @tempSubmissionRemove
		)
	END

	-----------------------------------------For ADOPC (Remove non-pair records if needed)-----------------------------------------
	IF @tempDocCode = 'ADOPC' AND @ADOPC_AccPurpose_Submit_Original = 'Y'
	BEGIN
		INSERT INTO @tempSubmissionRemove
		(
			Voucher_Acc_ID
		)
		SELECT Voucher_Acc_ID FROM @tempSubmission
		WHERE Doc_Code = 'ADOPC'
		AND Account_Purpose = 'A'
		AND Encrypt_Field1 NOT IN (
			SELECT Encrypt_Field1 FROM @tempSubmission
			WHERE Doc_Code = 'ADOPC'
			AND Account_Purpose = 'O'
		)
		UNION ALL
		SELECT Voucher_Acc_ID FROM @tempSubmission
		WHERE Doc_Code = 'ADOPC'
		AND Account_Purpose = 'O'
		AND Encrypt_Field1 NOT IN (
			SELECT Encrypt_Field1 FROM @tempSubmission
			WHERE Doc_Code = 'ADOPC'
			AND Account_Purpose = 'A'
		)
		
		DELETE FROM @tempSubmission 
		WHERE Voucher_Acc_ID IN (
			SELECT Voucher_Acc_ID FROM @tempSubmissionRemove
		)
	END



	
	Set @dtmCur = DateAdd(s, 1, @dtmCur)
	Set @curDate = @dtmCur

	Set @file_name = @File_Name_Prefix + 
		--REPLACE(UPPER(RTRIM(@tempDocCode)), '/', '') + '_' + 
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
	ORDER BY Voucher_Acc_ID ASC

	INSERT INTO [dbo].[TempVoucherAccSubHeader]
	(
		System_Dtm, File_Name, Record_Status, Join_Doc_Code
	)
	VALUES
	(
		@dtmCur, @file_name, 'P', @tempDocCode
	)

	UPDATE [dbo].[TempPersonalInformation]
	Set 
		Validating = 'Y'
	WHERE 
		Voucher_Acc_ID IN
		(
			SELECT DISTINCT (Voucher_Acc_ID) FROM @tempSubmission
		)
	
	UPDATE [dbo].[SpecialPersonalInformation]
	Set 
		Validating = 'Y'
	WHERE 
		Special_Acc_ID IN
		(
			SELECT DISTINCT (Voucher_Acc_ID) FROM @tempSubmission
		)
	
	DELETE FROM @tempSubmission
	
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

