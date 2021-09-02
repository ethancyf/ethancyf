if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[proc_ImmdPrepareRecord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[proc_ImmdPrepareRecord]
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Modification History
-- CR No.:			INT21-0017 (Revise the ImmD export order)
-- Modified by:	    Chris YIM
-- Modified date:   13 Aug 2021
-- Description:		Revise the ImmD export order
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT-016 (To introduce 'Deceased' status into eHS)
-- Modified by:	    Chris YIM
-- Modified date:   04 Dec 2017
-- Description:		Not to send deceased account to ImmD 
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Modified by:	    Chris YIM
-- Modified date:   04 Dec 2017
-- Description:		Not to send deceased account to ImmD 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Paul Yip
-- Modified date: 13 Oct 2010
-- Description:	Fix for Smart IC rectification in HCSP
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Dedrick Ng
-- Modified date: 11 Mar 2010
-- Description:	Add doc_code in TempVoucherAccSubHeader
--				Remove Amend record with record status 'O' and 'A'
--				not exist together
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Dedrick Ng
-- Modified date: 29 Sep 2009
-- Description:	Amend the obsolete fields for TempPersonalInformation
--				EC_Date -> Date_of_Issue
--				HKID_Card -> Doc_Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	01 Jun 2009
-- Description:		System Parameter Add Scheme Code
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Process TempVoucherAccount & TempPersonalInformation For ImmD File 
--				(Prepare Record Only if No Record Generated within Today)
-- =============================================

CREATE PROCEDURE [dbo].[proc_ImmdPrepareRecord]
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
	Account_Purpose char(1),
	EC_Serial_No varchar(10),
	EC_Reference_No varchar(15),
	EC_Age smallint,
	EC_Date_of_Registration datetime,
	EC_DOB_Type char(1),
	Doc_Code char(20),
	Original_Amend_Acc_ID char(15)
)

DECLARE @tempSubmissionRemove Table 
(
	Voucher_Acc_ID char(15)
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
DECLARE @record_num integer
DECLARE @remain_num integer
SELECT @record_num = Convert(integer,Parm_Value1) FROM [SystemParameters] WHERE Parameter_Name = 'ImmdRecordMaxSize_HKIC_EC' AND [Scheme_Code] = 'ALL'

--EC Date of Issue 
DECLARE @str_EC_DOI as varchar(30) 
SELECT @str_EC_DOI = Parm_Value1 FROM [SystemParameters] WHERE Parameter_Name = 'EC_DOI' AND [Scheme_Code] = 'ALL'


-- Export File Name
DECLARE @file_name varchar(100)

DECLARE @blnPrepareRecord as tinyint
Set @blnPrepareRecord = 1

-- =============================================
-- Validation 
-- =============================================
Set @blnPrepareRecord = 1



-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

IF @blnPrepareRecord = 1 
BEGIN

-- INSERT THE Record To Temp Table 
-- 1. Insert Create New Account Case
	INSERT INTO @tempSubmission
	(
		Voucher_Acc_ID, 
		DOB,
		Date_of_Issue,
		Encrypt_Field1,
		Account_Purpose,
		EC_Serial_No, 
		EC_Reference_No,
		EC_Age,
		EC_Date_of_Registration,
		EC_DOB_Type,
		Doc_Code,
		Original_Amend_Acc_ID
	)
	SELECT TOP (@record_num)
		TVA.Voucher_Acc_ID, 
		CASE WHEN TPI.Exact_DOB = 'Y' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + '0000'
			WHEN TPI.Exact_DOB = 'M' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + '00'
			WHEN TPI.Exact_DOB = 'D' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.DOB))
			WHEN TPI.Exact_DOB = 'R' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB))
		ELSE '' END, --TPI.DOB
		CASE WHEN TPI.Date_of_Issue Is NULL THEN ''
		ELSE RIGHT(CONVERT(varchar(4),DATEPART(YEAR,TPI.Date_of_Issue)),2) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue))  + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)) END, --Date_of_Issue
		TPI.Encrypt_Field1,
		TVA.Account_Purpose,		
		EC_Serial_No,
		EC_Reference_No,
		EC_Age,
		EC_Date_of_Registration,
		Exact_DOB,
		TPI.Doc_Code,
		TVA.Original_Amend_Acc_ID
	FROM
		[dbo].[TempVoucherAccount] TVA, [dbo].[TempPersonalInformation] TPI, [dbo].[DocType] D
	WHERE 
		TVA.Voucher_Acc_ID = TPI.Voucher_Acc_ID 
		AND TPI.Doc_Code = D.Doc_Code
		AND D.Force_Manual_Validate = 'N'
		AND TVA.Record_Status = 'P' AND (TPI.Validating ='N' OR TPI.Validating IS NULL) AND (TVA.Account_Purpose = 'C' OR TVA.Account_Purpose = 'V')
		AND (
			TPI.Doc_Code = 'HKIC' OR 
			(
				TPI.Doc_Code = 'EC' AND 
				--TPI.Date_of_Issue >= '2003-Jun-23' AND 
				TPI.Date_of_Issue >= @str_EC_DOI AND 
				( Exact_DOB = 'D' OR Exact_DOB = 'M' OR Exact_DOB = 'Y' OR Exact_DOB = 'A' OR Exact_DOB = 'R')
			)
		)
		AND TVA.Create_Dtm < @str_cut_off
		AND TVA.Deceased IS NULL
		AND TVA.Scheme_Code NOT IN ('COVID19CVC','COVID19DH','COVID19RVP','COVID19OR','COVID19SR','COVID19SB')

	ORDER BY TVA.Create_Dtm ASC

-- 2. Insert EC Amendment Case
	-- Only Amend Record is submitted

	Set @remain_num = @record_num - (Select Count(1) From @tempSubmission)
	
	INSERT INTO @tempSubmission
	(
		Voucher_Acc_ID, 
		DOB,
		Date_of_Issue,
		Encrypt_Field1,
		Account_Purpose,
		EC_Serial_No,
		EC_Reference_No,
		EC_Age,
		EC_Date_of_Registration,
		EC_DOB_Type,
		Doc_Code,
		Original_Amend_Acc_ID
	)
	SELECT TOP (@remain_num)
		TVA.Voucher_Acc_ID, 
		CASE WHEN TPI.Exact_DOB = 'Y' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + '0000'
			WHEN TPI.Exact_DOB = 'M' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + '00'
			WHEN TPI.Exact_DOB = 'D' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.DOB))
			WHEN TPI.Exact_DOB = 'R' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB))
		ELSE '' END, --TPI.DOB
		CASE WHEN TPI.Date_of_Issue Is NULL THEN ''
		ELSE RIGHT(CONVERT(varchar(4),DATEPART(YEAR,TPI.Date_of_Issue)),2) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue))  + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)) END, --Date_of_Issue
		TPI.Encrypt_Field1,
		TVA.Account_Purpose,		
		EC_Serial_No,
		EC_Reference_No,
		EC_Age,
		EC_Date_of_Registration,
		TPI.Exact_DOB,
		TPI.Doc_Code,
		TVA.Original_Amend_Acc_ID
	FROM
		[dbo].[TempVoucherAccount] TVA, [dbo].[TempPersonalInformation] TPI, [dbo].[DocType] D
	WHERE 
		TVA.Voucher_Acc_ID = TPI.Voucher_Acc_ID
		AND TPI.Doc_Code = D.Doc_Code 
		AND D.Force_Manual_Validate = 'N' 
		AND TVA.Record_Status = 'P' AND (TPI.Validating ='N' OR TPI.Validating IS NULL) AND (TVA.Account_Purpose = 'A') 
		AND TPI.Doc_Code = 'EC' 
		-- AND TPI.Date_of_Issue >= '2003-Jun-23'
		AND TPI.Date_of_Issue >= @str_EC_DOI  
		AND	( Exact_DOB = 'D' OR Exact_DOB = 'M' OR Exact_DOB = 'Y' OR Exact_DOB = 'A' OR Exact_DOB = 'R')
		AND TVA.Create_dtm < @str_cut_off
		AND TVA.Deceased IS NULL
		AND TVA.Scheme_Code NOT IN ('COVID19CVC','COVID19DH','COVID19RVP','COVID19OR','COVID19SR','COVID19SB')

	ORDER BY TVA.Create_Dtm ASC
	
	
-- 3. Insert Remaining HKIC Case Amendment Case 	
	
	Set @remain_num = @record_num - (Select Count(1) From @tempSubmission)

	if @remain_num % 2 = 1
		Set @remain_num =@remain_num - 1
		
	INSERT INTO @tempSubmission
	(
		Voucher_Acc_ID, 
		DOB,
		--IsHKID,
		Date_of_Issue,
		Encrypt_Field1,
		Account_Purpose,
		EC_Serial_No,
		EC_Reference_No,
		--EC_Date,
		EC_Age,
		EC_Date_of_Registration,
		EC_DOB_Type,
		Doc_Code,
		Original_Amend_Acc_ID
	)
	SELECT TOP (@remain_num)
		TVA.Voucher_Acc_ID, 
		CASE WHEN TPI.Exact_DOB = 'Y' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + '0000'
			WHEN TPI.Exact_DOB = 'M' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + '00'
			WHEN TPI.Exact_DOB = 'D' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.DOB))
			WHEN TPI.Exact_DOB = 'R' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB))
		ELSE '' END, 
		RIGHT(CONVERT(varchar(4),DATEPART(YEAR,TPI.Date_of_Issue)),2) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue))  + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)), --Date_of_Issue
		TPI.Encrypt_Field1,
		TVA.Account_Purpose,		
		EC_Serial_No,
		EC_Reference_No,
		EC_Age,
		EC_Date_of_Registration,
		TPI.Exact_DOB,
		TPI.Doc_Code,
		TVA.Original_Amend_Acc_ID
	FROM
		[dbo].[TempVoucherAccount] TVA, [dbo].[TempPersonalInformation] TPI, [dbo].[DocType] D
	WHERE 
		TVA.Voucher_Acc_ID = TPI.Voucher_Acc_ID
		AND TPI.Doc_Code = D.Doc_Code 
		AND D.Force_Manual_Validate = 'N' 
		AND TVA.Record_Status = 'P' 
		AND (TPI.Validating ='N' OR TPI.Validating IS NULL) 
		AND (TVA.Account_Purpose = 'A' OR TVA.Account_Purpose = 'O') 
		AND TPI.Doc_Code = 'HKIC'
		AND TVA.Create_Dtm < @str_cut_off
		AND TVA.Deceased IS NULL
		AND TVA.Scheme_Code NOT IN ('COVID19CVC','COVID19DH','COVID19RVP','COVID19OR','COVID19SR','COVID19SB')
	ORDER BY TVA.Create_Dtm ASC

-- 4. Insert Create New Account Case

	Set @remain_num = @record_num - (Select Count(1) From @tempSubmission)

	INSERT INTO @tempSubmission
	(
		Voucher_Acc_ID, 
		DOB,
		Date_of_Issue,
		Encrypt_Field1,
		Account_Purpose,
		EC_Serial_No, 
		EC_Reference_No,
		EC_Age,
		EC_Date_of_Registration,
		EC_DOB_Type,
		Doc_Code,
		Original_Amend_Acc_ID
	)
	SELECT TOP (@remain_num)
		TVA.Voucher_Acc_ID, 
		CASE WHEN TPI.Exact_DOB = 'Y' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + '0000'
			WHEN TPI.Exact_DOB = 'M' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + '00'
			WHEN TPI.Exact_DOB = 'D' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.DOB))
			WHEN TPI.Exact_DOB = 'R' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB))
		ELSE '' END, --TPI.DOB
		CASE WHEN TPI.Date_of_Issue Is NULL THEN ''
		ELSE RIGHT(CONVERT(varchar(4),DATEPART(YEAR,TPI.Date_of_Issue)),2) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue))  + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)) END, --Date_of_Issue
		TPI.Encrypt_Field1,
		TVA.Account_Purpose,		
		EC_Serial_No,
		EC_Reference_No,
		EC_Age,
		EC_Date_of_Registration,
		Exact_DOB,
		TPI.Doc_Code,
		TVA.Original_Amend_Acc_ID
	FROM
		[dbo].[TempVoucherAccount] TVA, [dbo].[TempPersonalInformation] TPI, [dbo].[DocType] D
	WHERE 
		TVA.Voucher_Acc_ID = TPI.Voucher_Acc_ID 
		AND TPI.Doc_Code = D.Doc_Code
		AND D.Force_Manual_Validate = 'N'
		AND TVA.Record_Status = 'P' AND (TPI.Validating ='N' OR TPI.Validating IS NULL) AND (TVA.Account_Purpose = 'C' OR TVA.Account_Purpose = 'V')
		AND (
			TPI.Doc_Code = 'HKIC' OR 
			(
				TPI.Doc_Code = 'EC' AND 
				--TPI.Date_of_Issue >= '2003-Jun-23' AND 
				TPI.Date_of_Issue >= @str_EC_DOI AND 
				( Exact_DOB = 'D' OR Exact_DOB = 'M' OR Exact_DOB = 'Y' OR Exact_DOB = 'A' OR Exact_DOB = 'R')
			)
		)
		AND TVA.Create_Dtm < @str_cut_off
		AND TVA.Deceased IS NULL
		AND TVA.Scheme_Code IN ('COVID19CVC','COVID19DH','COVID19RVP','COVID19OR','COVID19SR','COVID19SB')
	ORDER BY TVA.Create_Dtm ASC


-- 5. Insert EC Amendment Case
	-- Only Amend Record is submitted

	Set @remain_num = @record_num - (Select Count(1) From @tempSubmission)
	
	INSERT INTO @tempSubmission
	(
		Voucher_Acc_ID, 
		DOB,
		Date_of_Issue,
		Encrypt_Field1,
		Account_Purpose,
		EC_Serial_No,
		EC_Reference_No,
		EC_Age,
		EC_Date_of_Registration,
		EC_DOB_Type,
		Doc_Code,
		Original_Amend_Acc_ID
	)
	SELECT TOP (@remain_num)
		TVA.Voucher_Acc_ID, 
		CASE WHEN TPI.Exact_DOB = 'Y' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + '0000'
			WHEN TPI.Exact_DOB = 'M' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + '00'
			WHEN TPI.Exact_DOB = 'D' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.DOB))
			WHEN TPI.Exact_DOB = 'R' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB))
		ELSE '' END, --TPI.DOB
		CASE WHEN TPI.Date_of_Issue Is NULL THEN ''
		ELSE RIGHT(CONVERT(varchar(4),DATEPART(YEAR,TPI.Date_of_Issue)),2) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue))  + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)) END, --Date_of_Issue
		TPI.Encrypt_Field1,
		TVA.Account_Purpose,		
		EC_Serial_No,
		EC_Reference_No,
		EC_Age,
		EC_Date_of_Registration,
		TPI.Exact_DOB,
		TPI.Doc_Code,
		TVA.Original_Amend_Acc_ID
	FROM
		[dbo].[TempVoucherAccount] TVA, [dbo].[TempPersonalInformation] TPI, [dbo].[DocType] D
	WHERE 
		TVA.Voucher_Acc_ID = TPI.Voucher_Acc_ID
		AND TPI.Doc_Code = D.Doc_Code 
		AND D.Force_Manual_Validate = 'N' 
		AND TVA.Record_Status = 'P' AND (TPI.Validating ='N' OR TPI.Validating IS NULL) AND (TVA.Account_Purpose = 'A') 
		AND TPI.Doc_Code = 'EC' 
		-- AND TPI.Date_of_Issue >= '2003-Jun-23'
		AND TPI.Date_of_Issue >= @str_EC_DOI  
		AND	( Exact_DOB = 'D' OR Exact_DOB = 'M' OR Exact_DOB = 'Y' OR Exact_DOB = 'A' OR Exact_DOB = 'R')
		AND TVA.Create_dtm < @str_cut_off
		AND TVA.Deceased IS NULL
		AND TVA.Scheme_Code IN ('COVID19CVC','COVID19DH','COVID19RVP','COVID19OR','COVID19SR','COVID19SB')
	ORDER BY TVA.Create_Dtm ASC
	
	
-- 6. Insert Remaining HKIC Case Amendment Case 	
	
	Set @remain_num = @record_num - (Select Count(1) From @tempSubmission)

	if @remain_num % 2 = 1
		Set @remain_num =@remain_num - 1
		
	INSERT INTO @tempSubmission
	(
		Voucher_Acc_ID, 
		DOB,
		--IsHKID,
		Date_of_Issue,
		Encrypt_Field1,
		Account_Purpose,
		EC_Serial_No,
		EC_Reference_No,
		--EC_Date,
		EC_Age,
		EC_Date_of_Registration,
		EC_DOB_Type,
		Doc_Code,
		Original_Amend_Acc_ID
	)
	SELECT TOP (@remain_num)
		TVA.Voucher_Acc_ID, 
		CASE WHEN TPI.Exact_DOB = 'Y' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + '0000'
			WHEN TPI.Exact_DOB = 'M' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + '00'
			WHEN TPI.Exact_DOB = 'D' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.DOB))
			WHEN TPI.Exact_DOB = 'R' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB))
		ELSE '' END, 
		RIGHT(CONVERT(varchar(4),DATEPART(YEAR,TPI.Date_of_Issue)),2) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue))  + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)), --Date_of_Issue
		TPI.Encrypt_Field1,
		TVA.Account_Purpose,		
		EC_Serial_No,
		EC_Reference_No,
		EC_Age,
		EC_Date_of_Registration,
		TPI.Exact_DOB,
		TPI.Doc_Code,
		TVA.Original_Amend_Acc_ID
	FROM
		[dbo].[TempVoucherAccount] TVA, [dbo].[TempPersonalInformation] TPI, [dbo].[DocType] D
	WHERE 
		TVA.Voucher_Acc_ID = TPI.Voucher_Acc_ID
		AND TPI.Doc_Code = D.Doc_Code 
		AND D.Force_Manual_Validate = 'N' 
		AND TVA.Record_Status = 'P' 
		AND (TPI.Validating ='N' OR TPI.Validating IS NULL) 
		AND (TVA.Account_Purpose = 'A' OR TVA.Account_Purpose = 'O') 
		AND TPI.Doc_Code = 'HKIC'
		AND TVA.Create_Dtm < @str_cut_off
		AND TVA.Deceased IS NULL
		AND TVA.Scheme_Code IN ('COVID19CVC','COVID19DH','COVID19RVP','COVID19OR','COVID19SR','COVID19SB')
	ORDER BY TVA.Create_Dtm ASC

----------------------------------------------------------------------------------------------
	--Fix for Smart IC rectification in HCSP
----------------------------------------------------------------------------------------------

	DECLARE @tempOriginalAmendAcc Table 
	(
		Original_Amend_Acc_ID char(15)
	)

	INSERT INTO @tempOriginalAmendAcc
	(
		Original_Amend_Acc_ID
	)
	SELECT Original_Amend_Acc_ID FROM @tempSubmission
	WHERE Original_Amend_Acc_ID IS NOT NULL AND 
		Original_Amend_Acc_ID NOT IN (
		SELECT DISTINCT Voucher_Acc_ID FROM @tempSubmission
		WHERE Account_Purpose = 'O' AND Doc_Code = 'HKIC'
		)
	

INSERT INTO @tempSubmission
	(
		Voucher_Acc_ID, 
		DOB,
		--IsHKID,
		Date_of_Issue,
		Encrypt_Field1,
		Account_Purpose,
		EC_Serial_No,
		EC_Reference_No,
		--EC_Date,
		EC_Age,
		EC_Date_of_Registration,
		EC_DOB_Type,
		Doc_Code,
		Original_Amend_Acc_ID
	)
	SELECT 
		TVA.Voucher_Acc_ID, 
		CASE WHEN TPI.Exact_DOB = 'Y' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + '0000'
			WHEN TPI.Exact_DOB = 'M' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + '00'
			WHEN TPI.Exact_DOB = 'D' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.DOB))
			WHEN TPI.Exact_DOB = 'R' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB))
		ELSE '' END, 
		RIGHT(CONVERT(varchar(4),DATEPART(YEAR,TPI.Date_of_Issue)),2) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue))  + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)), --Date_of_Issue
		TPI.Encrypt_Field1,
		TVA.Account_Purpose,		
		EC_Serial_No,
		EC_Reference_No,
		EC_Age,
		EC_Date_of_Registration,
		TPI.Exact_DOB,
		TPI.Doc_Code,
		TVA.Original_Amend_Acc_ID
	FROM
		[dbo].[TempVoucherAccount] TVA, [dbo].[TempPersonalInformation] TPI, [dbo].[DocType] D, @tempOriginalAmendAcc OTA
	WHERE 
		TVA.Voucher_Acc_ID = TPI.Voucher_Acc_ID
		AND TPI.Doc_Code = D.Doc_Code
		AND D.Force_Manual_Validate = 'N'
		AND (TPI.Validating ='N' OR TPI.Validating IS NULL)
		AND (TVA.Account_Purpose = 'O')
		AND TPI.Doc_Code = 'HKIC'
		AND TVA.Voucher_Acc_ID = OTA.Original_Amend_Acc_ID
		AND TVA.Deceased IS NULL
	ORDER BY TVA.Create_Dtm ASC

----------------------------------------------------------------------------------------------

	INSERT INTO @tempSubmissionRemove
	(
		Voucher_Acc_ID
	)
	SELECT Voucher_Acc_ID FROM @tempSubmission
	WHERE Doc_Code = 'HKIC'
	AND Account_Purpose = 'A' AND Original_Amend_Acc_ID NOT IN 
	(
		SELECT Voucher_Acc_ID FROM @tempSubmission
		WHERE Account_Purpose = 'O'
	)
	UNION ALL
	SELECT Voucher_Acc_ID FROM @tempSubmission
	WHERE Doc_Code = 'HKIC'
	AND Account_Purpose = 'O' AND Voucher_Acc_ID NOT IN
	(
		SELECT Original_Amend_Acc_ID FROM @tempSubmission
		WHERE Account_Purpose = 'A'
	)

	DELETE FROM @tempSubmission 
	WHERE Voucher_Acc_ID IN (
		SELECT Voucher_Acc_ID FROM @tempSubmissionRemove
	)


DECLARE @dtmCur as DateTime
Set @dtmCur = GetDate()

	DECLARE @file_prefix varchar(30)
	SELECT @file_prefix = Parm_Value1 FROM [SystemParameters] WHERE Parameter_Name = 'ImmdRequestFilePrefix_HKIC_EC' AND [Scheme_Code] = 'ALL'

	Set @file_name = LTRIM(RTRIM(@file_prefix)) + 
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
		Record_Status,		
		EC_Serial_No,
		EC_Reference_No,
		EC_Age,
		EC_Date_of_Registration,
		EC_DOB_Type,
		Doc_Code,
		File_name
	)
	SELECT 
		@dtmCur,
		Voucher_Acc_ID,
		DOB,
		Date_of_Issue,
		Encrypt_Field1,
		CASE WHEN Account_Purpose = 'C' THEN 'N'
			WHEN Account_Purpose = 'V' THEN 'N'
			WHEN Account_Purpose = 'A' THEN 'A'
			WHEN Account_Purpose = 'O' THEN 'O'
			ELSE Account_Purpose END,
		EC_Serial_No,
		EC_Reference_No,
		EC_Age,
		EC_Date_of_Registration,
		EC_DOB_Type,
		Doc_Code,
		@file_name
		
	FROM @tempSubmission
	ORDER BY Voucher_Acc_ID ASC


	INSERT INTO [dbo].[TempVoucherAccSubHeader]
	(
		System_Dtm, File_Name, Record_Status, Join_Doc_Code
	)
	VALUES
	(
		@dtmCur, @file_name, 'P', 'HKIC_EC'
	)

	UPDATE [dbo].[TempPersonalInformation]
	Set 
		Validating = 'Y'
	WHERE 
		Voucher_Acc_ID IN
		(
			SELECT DISTINCT (Voucher_Acc_ID) FROM @tempSubmission
		)
	
END

END
GO

