IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccount_ProcessImmdFile]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccount_ProcessImmdFile]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Aug 2008
-- Description:	Process TempVoucherAccount & TempPersonalInformation For ImmD File 
--				(Prepare Record Only if No Record Generated within Today)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Dedrick Ng
-- Modified date: 30 Sep 2009
-- Description:	Remove obsolete fields
-- =============================================
CREATE PROCEDURE [dbo].[proc_TempVoucherAccount_ProcessImmdFile]
	@record_num	integer,
	@file_name varchar(100) OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================

DECLARE @tempRunTime Table
(
	StartTime datetime,
	EndTime	datetime
)

DECLARE @tempSubmission Table 
(
	Voucher_Acc_ID char(15),
	DOB varchar(10),
	--IsHKID char(1),
	Date_of_Issue varchar(10),
	Encrypt_Field1 varbinary(100),
	Account_Purpose char(1),
	EC_Serial_No varchar(10),
	EC_Reference_No varchar(15),
	--EC_Date datetime, 
	EC_Age smallint,
	EC_Date_of_Registration datetime,
	EC_DOB_Type char(1),
	Doc_Code char(20)
)

DECLARE @curDate as datetime
Set @curDate = GetDate()

DECLARE @remain_num integer


DECLARE @blnPrepareRecord as tinyint
Set @blnPrepareRecord = 1

DECLARE @tempStatus as char(1)

-- =============================================
-- Validation 
-- =============================================

DECLARE @default_value as nvarchar(100)
SET @default_value = '00:00-02:00'

DECLARE @Pair_Delimeter CHAR(1)
SET @Pair_Delimeter = ';'

DECLARE @Value_Delimeter CHAR(1)
SET @Value_Delimeter = '-'

DECLARE @runValuesList AS VARCHAR(1000)
DECLARE @run_Value AS VARCHAR(50)
DECLARE @run1 AS VARCHAR(50)
DECLARE @run2 AS VARCHAR(50)

DECLARE @StartPos INT, @Length INT

SELECT @runValuesList = Parm_Value1 FROM [SystemParameters] WHERE Parameter_Name = 'ImmdExportFileTime'


WHILE LEN(@runValuesList) > 0 
BEGIN
	SET @StartPos = CHARINDEX(@Pair_Delimeter, @runValuesList)
	IF @StartPos < 0 SET @StartPos = 0
		SET @Length = LEN(@runValuesList) - @StartPos - 1
    IF @Length < 0
		SET @Length = 0
	IF @StartPos > 0
	BEGIN
		SET @run_Value = SUBSTRING(@runValuesList, 1, @StartPos - 1)
		SET @runValuesList = SUBSTRING(@runValuesList, @StartPos + 1, LEN(@runValuesList) - @StartPos)
	END
	ELSE
	BEGIN
		SET @run_Value = @runValuesList
		SET @runValuesList = ''
	END
	
	SET @run1 = SUBSTRING(@run_Value, 1, CHARINDEX(@Value_Delimeter, @run_Value) - 1)
	SET @run2 = SUBSTRING(@run_Value, CHARINDEX(@Value_Delimeter, @run_Value) + 1, LEN(@run_Value) - CHARINDEX(@Value_Delimeter, @run_Value))
	
	if LEN(@run1) > 0 AND LEN(@run2) > 0
	BEGIN	
		INSERT @tempRunTime (StartTime, EndTime) VALUES( CONVERT(nvarchar(11), @curDate, 106) + ' ' + @run1, CONVERT(nvarchar(11), @curDate, 106) + ' ' + @run2)
	END	
END


Set @blnPrepareRecord = 0

DECLARE @curStartTime as datetime
DECLARE @curEndTime as datetime

IF (SELECT COUNT(1) FROM @tempRunTime t WHERE @curDate >= t.StartTime AND @curDate <= t.EndTime) > 0 
BEGIN
	SELECT TOP(1) @curStartTime = StartTime, @curEndTime = EndTime FROM @tempRunTime t WHERE @curDate >= t.StartTime AND @curDate <= t.EndTime

	if (@curStartTime IS NOT NULL AND @curEndTime IS NOT NULL)
	BEGIN
		IF (SELECT COUNT(1) FROM [dbo].[TempVoucherAccSubHeader] WHERE System_Dtm >= @curStartTime AND System_Dtm <= @curEndTime) = 0 
		BEGIN
			SET @blnPrepareRecord = 1
		END
	END	
END

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
		Doc_Code
	)
	SELECT TOP (@record_num)
		TVA.Voucher_Acc_ID, 
		CASE WHEN TPI.Exact_DOB = 'Y' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + '0000'
			WHEN TPI.Exact_DOB = 'M' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + '00'
			WHEN TPI.Exact_DOB = 'D' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.DOB))
			WHEN TPI.Exact_DOB = 'R' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB))
		ELSE '' END, --TPI.DOB
		--TPI.HKID_Card,
		CASE WHEN TPI.Date_of_Issue Is NULL THEN ''
		ELSE RIGHT(CONVERT(varchar(4),DATEPART(YEAR,TPI.Date_of_Issue)),2) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue))  + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)) END, --Date_of_Issue
		TPI.Encrypt_Field1,
		TVA.Account_Purpose,		
		EC_Serial_No,
		EC_Reference_No,
		--EC_Date,
		EC_Age,
		EC_Date_of_Registration,
		Exact_DOB,
		TPI.Doc_Code
		
	FROM
		[dbo].[TempVoucherAccount] TVA, [dbo].[TempPersonalInformation] TPI
	WHERE 
		TVA.Voucher_Acc_ID = TPI.Voucher_Acc_ID AND TVA.Record_Status = 'P' AND (TPI.Validating ='N' OR TPI.Validating IS NULL) AND (TVA.Account_Purpose = 'C' OR TVA.Account_Purpose = 'V')
		AND (
--			TPI.HKID_Card = 'Y' OR 
			TPI.Doc_Code = 'HKIC' OR 
			(
--				TPI.HKID_Card = 'N' AND 
				TPI.Doc_Code = 'EC' AND 
--				TPI.EC_Date >= '2003-Jun-23' AND 
				TPI.Date_of_Issue >= '2003-Jun-23' AND 
				( Exact_DOB = 'D' OR Exact_DOB = 'M' OR Exact_DOB = 'Y' OR Exact_DOB = 'A' OR Exact_DOB = 'R')
			)
		)

	ORDER BY TVA.Create_Dtm ASC


-- 2. Insert EC Amendment Case
	-- Only Amend Record is submitted

	Set @remain_num = @record_num - (Select Count(1) From @tempSubmission)
	
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
		Doc_Code
	)
	SELECT TOP (@remain_num)
		TVA.Voucher_Acc_ID, 
		CASE WHEN TPI.Exact_DOB = 'Y' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + '0000'
			WHEN TPI.Exact_DOB = 'M' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + '00'
			WHEN TPI.Exact_DOB = 'D' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.DOB))
			WHEN TPI.Exact_DOB = 'R' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB))
		ELSE '' END, --TPI.DOB
		--TPI.HKID_Card,
		
		CASE WHEN TPI.Date_of_Issue Is NULL THEN ''
		ELSE RIGHT(CONVERT(varchar(4),DATEPART(YEAR,TPI.Date_of_Issue)),2) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue))  + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)) END, --Date_of_Issue
		TPI.Encrypt_Field1,
		TVA.Account_Purpose,		
		EC_Serial_No,
		EC_Reference_No,
		--EC_Date,
		EC_Age,
		EC_Date_of_Registration,
		TPI.Exact_DOB,
		TPI.Doc_Code
		
	FROM
		[dbo].[TempVoucherAccount] TVA, [dbo].[TempPersonalInformation] TPI
	WHERE 
		TVA.Voucher_Acc_ID = TPI.Voucher_Acc_ID AND 
		TVA.Record_Status = 'P' AND (TPI.Validating ='N' OR TPI.Validating IS NULL) AND 
		(TVA.Account_Purpose = 'A') AND 
--		TPI.HKID_Card = 'N' AND 
		TPI.Doc_Code = 'EC' AND 
--		TPI.EC_Date >= '2003-Jun-23' AND 
		TPI.Date_of_Issue >= '2003-Jun-23' AND 
		( Exact_DOB = 'D' OR Exact_DOB = 'M' OR Exact_DOB = 'Y' OR Exact_DOB = 'A' OR Exact_DOB = 'R')

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
		Doc_Code
	)
	SELECT TOP (@remain_num)
		TVA.Voucher_Acc_ID, 
		CASE WHEN TPI.Exact_DOB = 'Y' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + '0000'
			WHEN TPI.Exact_DOB = 'M' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + '00'
			WHEN TPI.Exact_DOB = 'D' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.DOB)) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.DOB)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.DOB))
			WHEN TPI.Exact_DOB = 'R' THEN CONVERT(varchar(4),DATEPART(YEAR,TPI.DOB))
		ELSE '' END, --TPI.DOB
		--TPI.HKID_Card,
		RIGHT(CONVERT(varchar(4),DATEPART(YEAR,TPI.Date_of_Issue)),2) + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(MONTH,TPI.Date_of_Issue))  + LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)))) + CONVERT(varchar(2),DATEPART(DAY,TPI.Date_of_Issue)), --Date_of_Issue
		TPI.Encrypt_Field1,
		TVA.Account_Purpose,		
		EC_Serial_No,
		EC_Reference_No,
		--EC_Date,
		EC_Age,
		EC_Date_of_Registration,
		TPI.Exact_DOB,
		TPI.Doc_Code
		
	FROM
		[dbo].[TempVoucherAccount] TVA, [dbo].[TempPersonalInformation] TPI
	WHERE 
		TVA.Voucher_Acc_ID = TPI.Voucher_Acc_ID AND TVA.Record_Status = 'P' AND 
		(TPI.Validating ='N' OR TPI.Validating IS NULL) AND 
		(TVA.Account_Purpose = 'A' OR TVA.Account_Purpose = 'O') AND
--		TPI.HKID_Card = 'Y'
		TPI.Doc_Code = 'HKIC'
		
	ORDER BY TVA.Create_Dtm ASC


Set @file_name = 'HA_HCV_' + 
		CONVERT(varchar(4),DATEPART(YEAR,@curDate)) + 
		LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MONTH,@curDate)))) + CONVERT(varchar(2),DATEPART(MONTH,@curDate)) +
		LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(DAY,@curDate)))) + CONVERT(varchar(2),DATEPART(DAY,@curDate)) +
		LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(HOUR,@curDate)))) + CONVERT(varchar(2),DATEPART(HOUR,@curDate)) +
		LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(MINUTE,@curDate)))) + CONVERT(varchar(2),DATEPART(MINUTE,@curDate)) +
		LEFT('00', 2-LEN(CONVERT(varchar(2),DATEPART(SECOND,@curDate)))) + CONVERT(varchar(2),DATEPART(SECOND,@curDate)) + '.xml'
		

-- INSERT INTO [TempVoucherAccSubmissionLOG]
	INSERT INTO [dbo].[TempVoucherAccSubmissionLOG]
	(
		System_Dtm,
		Voucher_Acc_ID,
		DOB,
		--IsHKID,
		Date_of_Issue,
		Encrypt_Field1,
		Record_Status,		
		EC_Serial_No,
		EC_Reference_No,
		--EC_Date,
		EC_Age,
		EC_Date_of_Registration,
		EC_DOB_Type,
		Doc_Code,
		File_name
	)
	SELECT 
		GetDate(),
		Voucher_Acc_ID,
		DOB,
		--IsHKID,
		Date_of_Issue,
		Encrypt_Field1,
		CASE WHEN Account_Purpose = 'C' THEN 'N'
			WHEN Account_Purpose = 'V' THEN 'N'
			WHEN Account_Purpose = 'A' THEN 'A'
			WHEN Account_Purpose = 'O' THEN 'O'
			ELSE Account_Purpose END,
		EC_Serial_No,
		EC_Reference_No,
		--EC_Date,
		EC_Age,
		EC_Date_of_Registration,
		EC_DOB_Type,
		Doc_Code,
		@file_name
		
	FROM @tempSubmission
	ORDER BY Voucher_Acc_ID ASC


	INSERT INTO [dbo].[TempVoucherAccSubHeader]
	(
		System_Dtm, File_Name, Record_Status
	)
	VALUES
	(
		GetDate(), @file_name, 'P'
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

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccount_ProcessImmdFile] TO HCVU
GO
