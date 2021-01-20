IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ImmD_getStatistic]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ImmD_getStatistic]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Dickson Law
-- Modified date:	8 Jan 2018
-- Description:		Add HKIC for eHSD0013-02: No. of Temporary eHealth Account (Manual)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	8 April 2011
-- Description:		Refine the layout
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 30 Jun 2008
-- Description:	Retrieve Immd Interface file Statistic
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Dedrick Ng
-- Modified date: 14 Oct 2009
-- Description:	Remove obsolete fields
-- =============================================

CREATE PROCEDURE [dbo].[proc_ImmD_getStatistic]
	--@curDate as DateTime
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================

--CONVERT(nvarchar(11), @curDate, 106)

DECLARE @DATE_START as DATETIME
--SET @DATE_START = '2009Jan01'
SET @DATE_START = '2009Oct19'

DECLARE @INT_COUNT_DAY as INT
SET @INT_COUNT_DAY = 30

DECLARE @DateTime_CURRENT as DATETIME 
DECLARE @Date_CURRENT as DATETIME 
SET @DateTime_CURRENT = GetDate()
SET @Date_CURRENT = DATEADD(dd, -DATEDIFF(dd, @DateTime_CURRENT, 1), 1)

CREATE TABLE #immdRecordSubmit
(
	_datetime datetime, -- For Sorting
	_date nvarchar(30),
	_hkic int,
	_ec int,
	_hkbc int,
	_di int,
	_repmt int,
	_visa int,
	_adopc int,
	_total int
)

CREATE TABLE #immdRecordResult
(
	_datetime datetime, -- For Sorting
	_date nvarchar(30),
	_success int,
	_fail int,	
)

CREATE TABLE #immdTempVoucherAccount
(
	_datetime datetime, -- For Sorting
	_date nvarchar(30),
	_hkic_hkbc int,
	_ec int,
	_di int,
	_repmt int,
	_visa int,
	_adopc int,
	_hkic int,
	_hkbc int,
	_total int
)

CREATE TABLE #immdTempVoucherManual
(
	_datetime datetime, -- For Sorting
	_date nvarchar(30),
	_hkic int,
	_hkbc int,
	_ec int,
	_di int,
	_repmt int,
	_id235b int,
	_visa int,
	_adopc int,
	_total int
)

CREATE TABLE #immdValidatedAccount
(
	_datetime datetime, -- For Sorting
	_date nvarchar(30),
	_hkic_hkbc int,
	_ec int,
	_di int,
	_repmt int,
	_id235b int,
	_visa int,
	_adopc int,
	_hkic int,
	_hkbc int,
	_total int
)
/*
CREATE TABLE #immdManualGroup
(
	_datetime datetime, -- For Sorting
	_date nvarchar(30),
	_doc char(20),
	_count int
)
*/

DECLARE @temp_LoopDate as DATETIME
DECLARE @temp_UpperRange as DATETIME
DECLARE @temp_LowerRange as DATETIME

DECLARE @temp_CountDay as Int
Set @temp_CountDay = 0

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
EXEC [proc_SymmetricKey_open]
-- =============================================
-- Return results
-- =============================================

--------------------------------------------------
---------- Num of Record Submit to Immd ----------
--------------------------------------------------
--       Date      HKIC        EC     Total
-- 2009-01-01       119         1       120
-- 2009-01-02       537         2       539
--      Total       656         3       659
--------------------------------------------------

	SET @temp_LoopDate = @Date_CURRENT
	WHILE (DATEDIFF(d, @DATE_START, @temp_LoopDate) >=1 AND @temp_CountDay < @INT_COUNT_DAY )
	BEGIN

		SET @temp_LowerRange = DATEADD(d, -1, @temp_LoopDate)
		SET @temp_UpperRange = @temp_LoopDate
		
		INSERT INTO #immdRecordSubmit
		(
			_datetime,
			_date,
			_hkic,
			_ec,
			_hkbc,
			_di,
			_repmt,
			_visa,
			_adopc,
			_total
		)
		SELECT 
		
			@temp_LowerRange,
			LEFT(CONVERT(nvarchar(8),@temp_LowerRange, 112),4) + '-' + RIGHT(LEFT(CONVERT(nvarchar(8),@temp_LowerRange, 112),6),2) +'-' + RIGHT(LEFT(CONVERT(nvarchar(8),@temp_LowerRange, 112),8),2),		
--			(Select Count(*) from TempVoucherAccSubmissionLOG Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND ISHKID = 'Y'),
--			(Select Count(*) from TempVoucherAccSubmissionLOG Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND ISHKID = 'N'),
			(Select Count(*) from TempVoucherAccSubmissionLOG Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'HKIC'),
			(Select Count(*) from TempVoucherAccSubmissionLOG Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'EC'),
			(Select Count(*) from TempVoucherAccSubmissionLOG Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'HKBC'),
			(Select Count(*) from TempVoucherAccSubmissionLOG Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'Doc/I'),
			(Select Count(*) from TempVoucherAccSubmissionLOG Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'REPMT'),
			(Select Count(*) from TempVoucherAccSubmissionLOG Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'VISA'),
			(Select Count(*) from TempVoucherAccSubmissionLOG Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'ADOPC'),
			(Select Count(*) from TempVoucherAccSubmissionLOG Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange))
			
		SET @temp_CountDay = @temp_CountDay + 1
		SET @temp_LoopDate = DATEADD(d, -1, @temp_LoopDate)
	END

	INSERT INTO #immdRecordSubmit 
	(
		_datetime,
		_date,
		_hkic,
		_ec,
		_hkbc,
		_di,
		_repmt,
		_visa,
		_adopc,
		_total
	)
	SELECT 	
		@Date_CURRENT,
		'Total',
--		(Select Count(*) from TempVoucherAccSubmissionLOG Where ISHKID = 'Y'),
--		(Select Count(*) from TempVoucherAccSubmissionLOG Where ISHKID = 'N'),
		(Select Count(*) from TempVoucherAccSubmissionLOG Where Doc_Code = 'HKIC'),
		(Select Count(*) from TempVoucherAccSubmissionLOG Where Doc_Code = 'EC'),
		(Select Count(*) from TempVoucherAccSubmissionLOG Where Doc_Code = 'HKBC'),
		(Select Count(*) from TempVoucherAccSubmissionLOG Where Doc_Code = 'Doc/I'),
		(Select Count(*) from TempVoucherAccSubmissionLOG Where Doc_Code = 'REPMT'),
		(Select Count(*) from TempVoucherAccSubmissionLOG Where Doc_Code = 'VISA'),
		(Select Count(*) from TempVoucherAccSubmissionLOG Where Doc_Code = 'ADOPC'),
		(Select Count(*) from TempVoucherAccSubmissionLOG )

---------------------------------------------
---------- Record Result from Immd ----------
---------------------------------------------
--       Date   Success      Fail
-- 2009-01-01       111         9
-- 2009-01-02       478        61

--(A unique HKID may occur more than 1 time in the records)
---------------------------------------------

	Set @temp_CountDay = 0
	SET @temp_LoopDate = @Date_CURRENT
	WHILE (DATEDIFF(d, @DATE_START, @temp_LoopDate) >=1 AND @temp_CountDay < @INT_COUNT_DAY )
	BEGIN

		SET @temp_LowerRange = DATEADD(d, -1, @temp_LoopDate)
		SET @temp_UpperRange = @temp_LoopDate
		
		INSERT INTO #immdRecordResult
		(
			_datetime,
			_date,
			_success,
			_fail
		)
		SELECT 
		
			@temp_LowerRange,
			LEFT(CONVERT(nvarchar(8),@temp_LowerRange, 112),4) + '-' + RIGHT(LEFT(CONVERT(nvarchar(8),@temp_LowerRange, 112),6),2) +'-' + RIGHT(LEFT(CONVERT(nvarchar(8),@temp_LowerRange, 112),8),2),		
			(Select Count(*) from TempVoucherAccMatchLog Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Valid_HKID = 'Y'),
			(Select Count(*) from TempVoucherAccMatchLog Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Valid_HKID = 'N')
			
		SET @temp_CountDay = @temp_CountDay + 1
		SET @temp_LoopDate = DATEADD(d, -1, @temp_LoopDate)
	END

	
-------------------------------------------------
---------- Num of Temp Voucher Account ----------
-------------------------------------------------

--       Date      HKIC        EC     Total
-- 2009-01-01       115         1       116
-- 2009-01-02       522         2       524
--      Total       636         3       636

-- (Rectified Temp Voucher Account will be re-submit to Immd for Validion)
-------------------------------------------------
	Set @temp_CountDay = 0
	SET @temp_LoopDate = @Date_CURRENT
	WHILE (DATEDIFF(d, @DATE_START, @temp_LoopDate) >=1 AND @temp_CountDay < @INT_COUNT_DAY )
	BEGIN

		SET @temp_LowerRange = DATEADD(d, -1, @temp_LoopDate)
		SET @temp_UpperRange = @temp_LoopDate
		
		INSERT INTO #immdTempVoucherAccount
		(
			_datetime,
			_date,
			_hkic_hkbc,
			_ec,
			_di,
			_repmt,
			_visa,
			_adopc,
			_hkic,
			_hkbc,
			_total
		)
		SELECT 
		
			@temp_LowerRange,
			LEFT(CONVERT(nvarchar(8),@temp_LowerRange, 112),4) + '-' + RIGHT(LEFT(CONVERT(nvarchar(8),@temp_LowerRange, 112),6),2) +'-' + RIGHT(LEFT(CONVERT(nvarchar(8),@temp_LowerRange, 112),8),2),
--			(SELECT Count(Distinct (CONVERT(char,DecryptByKey(Encrypt_Field1)))) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND IsHKID = 'Y'),
--			(SELECT Count(Distinct (CONVERT(char,DecryptByKey(Encrypt_Field1)))) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND IsHKID = 'N'),
			--(SELECT Count(Distinct (CONVERT(char,DecryptByKey(Encrypt_Field1)))) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'HKIC'),
			--(SELECT Count(Distinct (CONVERT(char,DecryptByKey(Encrypt_Field1)))) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'EC'),
			--(SELECT Count(Distinct (CONVERT(char,DecryptByKey(Encrypt_Field1)))) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange))
			(SELECT Count(Distinct Encrypt_Field1) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code in ('HKIC', 'HKBC')),
			(SELECT Count(Distinct Encrypt_Field1) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'EC'),
			(SELECT Count(Distinct Encrypt_Field1) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'Doc/I'),
			(SELECT Count(Distinct Encrypt_Field1) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'REPMT'),
			(SELECT Count(Distinct Encrypt_Field1) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'VISA'),
			(SELECT Count(Distinct Encrypt_Field1) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'ADOPC'),
			(SELECT Count(Distinct Encrypt_Field1) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'HKIC'),
			(SELECT Count(Distinct Encrypt_Field1) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'HKBC'),
			(SELECT Count(*) FROM (
			SELECT Distinct Encrypt_Field1, Case Doc_Code When 'HKIC' Then 'HKIC_HKBC' When 'HKBC' Then 'HKIC_HKBC'
			Else Doc_Code End as New_Doc
			FROM [dbo].[TempVoucherAccSubmissionLOG] Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange)) AS allAuto
			)
						
		--update #immdTempVoucherAccount 
		--set _ec = 
		--(SELECT Count(Distinct (CONVERT(char,DecryptByKey(Encrypt_Field1)))) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'EC') 
		--where _datetime = @temp_LowerRange  

		--update #immdTempVoucherAccount 
		--set _total = 
		--(SELECT Count(Distinct (CONVERT(char,DecryptByKey(Encrypt_Field1)))) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange)) 
		--where _datetime = @temp_LowerRange  

		SET @temp_CountDay = @temp_CountDay + 1
		SET @temp_LoopDate = DATEADD(d, -1, @temp_LoopDate)
	END

	INSERT INTO #immdTempVoucherAccount 
	(
		_datetime,
		_date,
		_hkic_hkbc,
		_ec,
		_di,
		_repmt,
		_visa,
		_adopc,
		_hkic,
		_hkbc,
		_total
	)
	SELECT 	
		@Date_CURRENT,
		'Total',
--		(SELECT Count(Distinct (CONVERT(char,DecryptByKey(Encrypt_Field1)))) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where IsHKID = 'Y'),
--		(SELECT Count(Distinct (CONVERT(char,DecryptByKey(Encrypt_Field1)))) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where IsHKID = 'N'),
		--(SELECT Count(Distinct (CONVERT(char,DecryptByKey(Encrypt_Field1)))) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where Doc_Code = 'HKIC'),
		--(SELECT Count(Distinct (CONVERT(char,DecryptByKey(Encrypt_Field1)))) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where Doc_Code = 'EC'),
		--(SELECT Count(Distinct (CONVERT(char,DecryptByKey(Encrypt_Field1)))) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL)
		(SELECT Count(Distinct Encrypt_Field1) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where Doc_Code in ('HKIC', 'HKBC')),
		(SELECT Count(Distinct Encrypt_Field1) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where Doc_Code = 'EC'),
		(SELECT Count(Distinct Encrypt_Field1) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where Doc_Code = 'Doc/I'),
		(SELECT Count(Distinct Encrypt_Field1) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where Doc_Code = 'REPMT'),
		(SELECT Count(Distinct Encrypt_Field1) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where Doc_Code = 'VISA'),
		(SELECT Count(Distinct Encrypt_Field1) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where Doc_Code = 'ADOPC'),
		(SELECT Count(Distinct Encrypt_Field1) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where Doc_Code = 'HKIC'),
		(SELECT Count(Distinct Encrypt_Field1) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where Doc_Code = 'HKBC'),
		(SELECT Count(*) FROM (
		SELECT Distinct Encrypt_Field1, Case Doc_Code When 'HKIC' Then 'HKIC_HKBC' When 'HKBC' Then 'HKIC_HKBC'
		Else Doc_Code End as New_Doc
		FROM [dbo].[TempVoucherAccSubmissionLOG]) AS allAuto)
		
		--update #immdTempVoucherAccount 
		--set _ec = 
		--(SELECT Count(Distinct (CONVERT(char,DecryptByKey(Encrypt_Field1)))) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL Where Doc_Code = 'EC') 
		--where _date = 'Total'  
		
		--update #immdTempVoucherAccount 
		--set _total = 
		--(SELECT Count(Distinct (CONVERT(char,DecryptByKey(Encrypt_Field1)))) FROM [dbo].[TempVoucherAccSubmissionLOG] TVASL) 
		--where _date = 'Total'  
			
		

-------------------------------------------------
---------- Num of Temp Voucher Manual ----------
-------------------------------------------------

--       Date      HKIC        EC     Total
-- 2009-01-01       115         1       116
-- 2009-01-02       522         2       524
--      Total       636         3       636

-- (Rectified Temp Voucher Account will be re-submit to Immd for Validion)
-------------------------------------------------
	
	Set @temp_CountDay = 0
	--SET @temp_LoopDate = @Date_CURRENT
	SET @temp_LoopDate = DATEADD(d, -1, @Date_CURRENT)
	--WHILE (DATEDIFF(d, @DATE_START, @temp_LoopDate) >=1 AND @temp_CountDay < @INT_COUNT_DAY )
	WHILE (DATEDIFF(d, DATEADD(d, -1, @DATE_START), @temp_LoopDate) >=1 AND @temp_CountDay < @INT_COUNT_DAY )
	BEGIN

		--SET @temp_LowerRange = DATEADD(d, -1, @temp_LoopDate)
		SET @temp_LowerRange = @temp_LoopDate
		SET @temp_UpperRange = @temp_LoopDate
		
/*		
	Set @temp_CountDay = 0
	SET @temp_LoopDate = @Date_CURRENT
	WHILE (DATEDIFF(d, @DATE_START, @temp_LoopDate) >=1 AND @temp_CountDay < @INT_COUNT_DAY )
--	WHILE (DATEDIFF(d, DATEADD(d, -1, @DATE_START), @temp_LoopDate) >=1 AND @temp_CountDay < @INT_COUNT_DAY )
	BEGIN

		SET @temp_LowerRange = DATEADD(d, -1, @temp_LoopDate)
		SET @temp_UpperRange = @temp_LoopDate

		--SET @temp_LowerRange = @temp_LoopDate
		--SET @temp_UpperRange = DATEADD(d, 1, @temp_LoopDate)

		INSERT INTO #immdManualGroup
		(
			_datetime,
			_date,
			_doc,
			_count
		)
		SELECT 
		@temp_LowerRange, '', P.Doc_Code, Count(Distinct P.Encrypt_Field1) 
		FROM [dbo].[TempPersonalInformation] P
		Where (Exists (select 1 from VoucherAccountCreationLOG C Where C.Voucher_Acc_ID = P.Voucher_Acc_ID AND C.Transaction_Dtm > @temp_UpperRange AND C.Transaction_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND C.Voucher_Acc_Type = 'T')
		OR Exists (select 1 from TempVoucherAccMergeLOG M Where M.Temp_Voucher_Acc_ID = P.Voucher_Acc_ID AND M.System_Dtm > @temp_UpperRange AND M.System_Dtm <= DATEADD(d, 1, @temp_UpperRange)))
		AND NOT Exists (select 1 from TempVoucherAccSubmissionLOG S Where S.Voucher_Acc_ID = P.Voucher_Acc_ID AND S.System_Dtm > @temp_UpperRange AND S.System_Dtm <= DATEADD(d, 1, @temp_UpperRange))
		Group by P.Doc_Code
*/

		INSERT INTO #immdTempVoucherManual
		(
			_datetime,
			_date,
			_hkic,
			_hkbc,
			_ec,
			_di,
			_repmt,
			_id235b,
			_visa,
			_adopc,
			_total
		)
		SELECT 
		
			@temp_LowerRange,
			LEFT(CONVERT(nvarchar(8),@temp_LowerRange, 112),4) + '-' + RIGHT(LEFT(CONVERT(nvarchar(8),@temp_LowerRange, 112),6),2) +'-' + RIGHT(LEFT(CONVERT(nvarchar(8),@temp_LowerRange, 112),8),2),
			(SELECT Count(Distinct Encrypt_Field1) FROM TempVoucherAccManualSubLOG Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'HKIC'),
			(SELECT Count(Distinct Encrypt_Field1) FROM TempVoucherAccManualSubLOG Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'HKBC'),
			(SELECT Count(Distinct Encrypt_Field1) FROM TempVoucherAccManualSubLOG Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'EC'),
			(SELECT Count(Distinct Encrypt_Field1) FROM TempVoucherAccManualSubLOG Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'Doc/I'),
			(SELECT Count(Distinct Encrypt_Field1) FROM TempVoucherAccManualSubLOG Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'REPMT'),
			(SELECT Count(Distinct Encrypt_Field1) FROM TempVoucherAccManualSubLOG Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'ID235B'),
			(SELECT Count(Distinct Encrypt_Field1) FROM TempVoucherAccManualSubLOG Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'VISA'),
			(SELECT Count(Distinct Encrypt_Field1) FROM TempVoucherAccManualSubLOG Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange) AND Doc_Code = 'ADOPC'),
			(SELECT Count(*) FROM (SELECT Distinct Doc_Code, Encrypt_Field1 FROM TempVoucherAccManualSubLOG Where System_Dtm > @temp_UpperRange AND System_Dtm <= DATEADD(d, 1, @temp_UpperRange)) AS allManual)
			 
		SET @temp_CountDay = @temp_CountDay + 1
		SET @temp_LoopDate = DATEADD(d, -1, @temp_LoopDate)
	END
/*
	INSERT INTO #immdManualGroup
	(
		_datetime,
		_date,
		_doc,
		_count
	)
	SELECT 
	@Date_CURRENT, 'Total', P.Doc_Code, Count(Distinct P.Encrypt_Field1) 
	FROM [dbo].[TempPersonalInformation] P
	Where (Exists (select 1 from VoucherAccountCreationLOG C Where C.Voucher_Acc_ID = P.Voucher_Acc_ID AND C.Voucher_Acc_Type = 'T')
	OR Exists (select 1 from TempVoucherAccMergeLOG M Where M.Temp_Voucher_Acc_ID = P.Voucher_Acc_ID))
	AND NOT Exists (select 1 from TempVoucherAccSubmissionLOG S Where S.Voucher_Acc_ID = P.Voucher_Acc_ID)
	Group by P.Doc_Code
*/
	INSERT INTO #immdTempVoucherManual 
	(
		_datetime,
		_date,
		_hkic,
		_hkbc,
		_ec,
		_di,
		_repmt,
		_id235b,
		_visa,
		_adopc,
		_total
	)
	SELECT 	
		@Date_CURRENT,
		'Total',
		(SELECT Count(Distinct Encrypt_Field1) FROM TempVoucherAccManualSubLOG Where Doc_Code = 'HKIC'),
		(SELECT Count(Distinct Encrypt_Field1) FROM TempVoucherAccManualSubLOG Where Doc_Code = 'HKBC'),
		(SELECT Count(Distinct Encrypt_Field1) FROM TempVoucherAccManualSubLOG Where Doc_Code = 'EC'),
		(SELECT Count(Distinct Encrypt_Field1) FROM TempVoucherAccManualSubLOG Where Doc_Code = 'Doc/I'),
		(SELECT Count(Distinct Encrypt_Field1) FROM TempVoucherAccManualSubLOG Where Doc_Code = 'REPMT'),
		(SELECT Count(Distinct Encrypt_Field1) FROM TempVoucherAccManualSubLOG Where Doc_Code = 'ID235B'),
		(SELECT Count(Distinct Encrypt_Field1) FROM TempVoucherAccManualSubLOG Where Doc_Code = 'VISA'),
		(SELECT Count(Distinct Encrypt_Field1) FROM TempVoucherAccManualSubLOG Where Doc_Code = 'ADOPC'),
		(SELECT Count(*) FROM (SELECT Distinct Doc_Code, Encrypt_Field1 FROM TempVoucherAccManualSubLOG) AS allManual)
/*
		UPDATE #immdTempVoucherManual SET _hkbc = 0 WHERE _hkbc IS NULL 
		
		UPDATE #immdTempVoucherManual SET _ec = 0 WHERE _ec IS NULL 
		
		UPDATE #immdTempVoucherManual SET _di = 0 WHERE _di IS NULL 
		
		UPDATE #immdTempVoucherManual SET _repmt = 0 WHERE _repmt IS NULL 
		
		UPDATE #immdTempVoucherManual SET _id235b = 0 WHERE _id235b IS NULL 
		
		UPDATE #immdTempVoucherManual SET _visa = 0 WHERE _visa IS NULL 
		
		UPDATE #immdTempVoucherManual SET _adopc = 0 WHERE _adopc IS NULL 

*/

----------------------------------------------
---------- Num of Validated Account ----------
----------------------------------------------

--       Date      HKIC        EC     Total
-- 2009-01-01       106         1       116
-- 2009-01-02       463         2       524
--      Total       569         3       572
----------------------------------------------
	Set @temp_CountDay = 0
	--SET @temp_LoopDate = @Date_CURRENT
	SET @temp_LoopDate = DATEADD(d, -1, @Date_CURRENT)
	--WHILE (DATEDIFF(d, @DATE_START, @temp_LoopDate) >=1 AND @temp_CountDay < @INT_COUNT_DAY )
	WHILE (DATEDIFF(d, DATEADD(d, -1, @DATE_START), @temp_LoopDate) >=1 AND @temp_CountDay < @INT_COUNT_DAY )
	BEGIN

		--SET @temp_LowerRange = DATEADD(d, -1, @temp_LoopDate)
		SET @temp_LowerRange = @temp_LoopDate
		SET @temp_UpperRange = @temp_LoopDate
		
		INSERT INTO #immdValidatedAccount
		(
			_datetime,
			_date,
			_hkic_hkbc,
			_ec,
			_di,
			_repmt,
			_id235b,
			_visa,
			_adopc,
			_hkic,
			_hkbc,
			_total
		)
		SELECT 
		
			@temp_LowerRange,
			LEFT(CONVERT(nvarchar(8),@temp_LowerRange, 112),4) + '-' + RIGHT(LEFT(CONVERT(nvarchar(8),@temp_LowerRange, 112),6),2) +'-' + RIGHT(LEFT(CONVERT(nvarchar(8),@temp_LowerRange, 112),8),2),
--			(Select Count(*) from VoucherAccountCreationLOG VA INNER JOIN PersonalInformation PI ON VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where Voucher_Acc_Type = 'V' And Transaction_Dtm > @temp_UpperRange AND Transaction_Dtm <= DATEADD(d, 1, @temp_UpperRange) And PI.HKID_Card = 'Y'),
--			(Select Count(*) from VoucherAccountCreationLOG VA INNER JOIN PersonalInformation PI ON VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where Voucher_Acc_Type = 'V' And Transaction_Dtm > @temp_UpperRange AND Transaction_Dtm <= DATEADD(d, 1, @temp_UpperRange) And PI.HKID_Card = 'N'),
			(Select Count(*) from VoucherAccountCreationLOG VA INNER JOIN PersonalInformation PI ON VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where Voucher_Acc_Type = 'V' And Transaction_Dtm > @temp_UpperRange AND Transaction_Dtm <= DATEADD(d, 1, @temp_UpperRange) And PI.Doc_Code in ('HKIC', 'HKBC')),
			(Select Count(*) from VoucherAccountCreationLOG VA INNER JOIN PersonalInformation PI ON VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where Voucher_Acc_Type = 'V' And Transaction_Dtm > @temp_UpperRange AND Transaction_Dtm <= DATEADD(d, 1, @temp_UpperRange) And PI.Doc_Code = 'EC'),
			(Select Count(*) from VoucherAccountCreationLOG VA INNER JOIN PersonalInformation PI ON VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where Voucher_Acc_Type = 'V' And Transaction_Dtm > @temp_UpperRange AND Transaction_Dtm <= DATEADD(d, 1, @temp_UpperRange) And PI.Doc_Code = 'Doc/I'),
			(Select Count(*) from VoucherAccountCreationLOG VA INNER JOIN PersonalInformation PI ON VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where Voucher_Acc_Type = 'V' And Transaction_Dtm > @temp_UpperRange AND Transaction_Dtm <= DATEADD(d, 1, @temp_UpperRange) And PI.Doc_Code = 'REPMT'),
			(Select Count(*) from VoucherAccountCreationLOG VA INNER JOIN PersonalInformation PI ON VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where Voucher_Acc_Type = 'V' And Transaction_Dtm > @temp_UpperRange AND Transaction_Dtm <= DATEADD(d, 1, @temp_UpperRange) And PI.Doc_Code = 'ID235B'),
			(Select Count(*) from VoucherAccountCreationLOG VA INNER JOIN PersonalInformation PI ON VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where Voucher_Acc_Type = 'V' And Transaction_Dtm > @temp_UpperRange AND Transaction_Dtm <= DATEADD(d, 1, @temp_UpperRange) And PI.Doc_Code = 'VISA'),
			(Select Count(*) from VoucherAccountCreationLOG VA INNER JOIN PersonalInformation PI ON VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where Voucher_Acc_Type = 'V' And Transaction_Dtm > @temp_UpperRange AND Transaction_Dtm <= DATEADD(d, 1, @temp_UpperRange) And PI.Doc_Code = 'ADOPC'),
			--(Select Count(*) from VoucherAccountCreationLOG VA INNER JOIN PersonalInformation PI ON VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where Voucher_Acc_Type = 'V' And Transaction_Dtm > @temp_UpperRange AND Transaction_Dtm <= DATEADD(d, 1, @temp_UpperRange) And PI.Doc_Code = 'HKIC'),
			(Select COUNT(*) from TempVoucherAccount TA INNER JOIN TempPersonalInformation TP ON TA.Voucher_Acc_ID = TP.Voucher_Acc_ID, VoucherAccountCreationLOG VC
			Where TA.Validated_Acc_ID = VC.Voucher_Acc_ID AND TP.Doc_Code = 'HKIC' AND VC.Voucher_Acc_Type = 'V' And VC.Transaction_Dtm > @temp_UpperRange AND VC.Transaction_Dtm <= DATEADD(d, 1, @temp_UpperRange) 
			And Not Exists (Select 1 from TempVoucherAccMergeLog m Where m.Temp_Voucher_Acc_ID = TA.Voucher_Acc_ID) AND TA.Account_Purpose Not in ('A','O')),
--			(Select Count(*) from VoucherAccountCreationLOG VA INNER JOIN PersonalInformation PI ON VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where Voucher_Acc_Type = 'V' And Transaction_Dtm > @temp_UpperRange AND Transaction_Dtm <= DATEADD(d, 1, @temp_UpperRange) And PI.Doc_Code = 'HKBC'),
			0,
			(Select Count(*) from VoucherAccountCreationLOG VA INNER JOIN PersonalInformation PI ON VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where Voucher_Acc_Type = 'V' And Transaction_Dtm > @temp_UpperRange AND Transaction_Dtm <= DATEADD(d, 1, @temp_UpperRange))

		SET @temp_CountDay = @temp_CountDay + 1
		SET @temp_LoopDate = DATEADD(d, -1, @temp_LoopDate)
	END

	INSERT INTO #immdValidatedAccount 
	(
		_datetime,
		_date,
		_hkic_hkbc,
		_ec,
		_di,
		_repmt,
		_id235b,
		_visa,
		_adopc,
		_hkic,
		_hkbc,
		_total
	)
	SELECT 	
		@Date_CURRENT,
		'Total',
--		(Select Count(*) from PersonalInformation PI INNER JOIN VoucherAccount VA On VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where HKID_Card ='Y'),
--		(Select Count(*) from PersonalInformation PI INNER JOIN VoucherAccount VA On VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where HKID_Card ='N'),
		(Select Count(*) from PersonalInformation PI INNER JOIN VoucherAccount VA On VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where Doc_Code in ('HKIC', 'HKBC')
		AND Exists (Select 1 from VoucherAccountCreationLOG VC Where VC.Voucher_Acc_ID = PI.Voucher_Acc_ID and VC.Transaction_Dtm < @Date_CURRENT)),
		(Select Count(*) from PersonalInformation PI INNER JOIN VoucherAccount VA On VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where Doc_Code = 'EC'
		AND Exists (Select 1 from VoucherAccountCreationLOG VC Where VC.Voucher_Acc_ID = PI.Voucher_Acc_ID and VC.Transaction_Dtm < @Date_CURRENT)),
		(Select Count(*) from PersonalInformation PI INNER JOIN VoucherAccount VA On VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where Doc_Code = 'Doc/I'
		AND Exists (Select 1 from VoucherAccountCreationLOG VC Where VC.Voucher_Acc_ID = PI.Voucher_Acc_ID and VC.Transaction_Dtm < @Date_CURRENT)),
		(Select Count(*) from PersonalInformation PI INNER JOIN VoucherAccount VA On VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where Doc_Code = 'REPMT'
		AND Exists (Select 1 from VoucherAccountCreationLOG VC Where VC.Voucher_Acc_ID = PI.Voucher_Acc_ID and VC.Transaction_Dtm < @Date_CURRENT)),
		(Select Count(*) from PersonalInformation PI INNER JOIN VoucherAccount VA On VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where Doc_Code = 'ID235B'
		AND Exists (Select 1 from VoucherAccountCreationLOG VC Where VC.Voucher_Acc_ID = PI.Voucher_Acc_ID and VC.Transaction_Dtm < @Date_CURRENT)),
		(Select Count(*) from PersonalInformation PI INNER JOIN VoucherAccount VA On VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where Doc_Code = 'VISA'
		AND Exists (Select 1 from VoucherAccountCreationLOG VC Where VC.Voucher_Acc_ID = PI.Voucher_Acc_ID and VC.Transaction_Dtm < @Date_CURRENT)),
		(Select Count(*) from PersonalInformation PI INNER JOIN VoucherAccount VA On VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where Doc_Code = 'ADOPC'
		AND Exists (Select 1 from VoucherAccountCreationLOG VC Where VC.Voucher_Acc_ID = PI.Voucher_Acc_ID and VC.Transaction_Dtm < @Date_CURRENT)),
		(Select Count(*) from PersonalInformation PI INNER JOIN VoucherAccount VA On VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where Doc_Code = 'HKIC'
		AND Exists (Select 1 from VoucherAccountCreationLOG VC Where VC.Voucher_Acc_ID = PI.Voucher_Acc_ID and VC.Transaction_Dtm < @Date_CURRENT)),
--		(Select Count(*) from PersonalInformation PI INNER JOIN VoucherAccount VA On VA.Voucher_Acc_ID = PI.Voucher_Acc_ID Where Doc_Code = 'HKBC'),
		0,
		(Select Count(*) from PersonalInformation PI INNER JOIN VoucherAccount VA On VA.Voucher_Acc_ID = PI.Voucher_Acc_ID
		Where Exists (Select 1 from VoucherAccountCreationLOG VC Where VC.Voucher_Acc_ID = PI.Voucher_Acc_ID and VC.Transaction_Dtm < @Date_CURRENT))


-- =============================================
-- Return results
-- =============================================

-- ---------------------------------------------
-- To Excel sheet: Content
-- ---------------------------------------------

	SELECT
		'Report Generation Time: ' + CONVERT(varchar, GETDATE(), 111) + ' ' + CONVERT(varchar(5), GETDATE(), 114)


-- ---------------------------------------------
-- To Excel sheet: eHSD0013-01: No. of Temporary eHealth Account			
-- ---------------------------------------------

SELECT _date, _hkic_hkbc, _ec, _di, _repmt, _visa, _adopc, _total, '', _hkic, _hkbc FROM #immdTempVoucherAccount ORDER BY _datetime ASC


-- ---------------------------------------------
-- To Excel sheet: eHSD0013-02: No. of Temporary eHealth Account (Manual)
-- ---------------------------------------------

SELECT _date, _hkic, _hkbc, _ec, _di, _repmt, _id235b, _visa, _adopc, _total FROM #immdTempVoucherManual ORDER BY _datetime ASC


-- ---------------------------------------------
-- To Excel sheet: eHSD0013-03: No. of Validated Document
-- ---------------------------------------------

SELECT _date, _hkic_hkbc, _ec, _di, _repmt, _id235b, _visa, _adopc, _total, '', _hkic, _hkic_hkbc - _hkic FROM #immdValidatedAccount ORDER BY _datetime ASC


-- ---------------------------------------------
-- To Excel sheet: eHSD0013-04: No. of Record
-- ---------------------------------------------
		
SELECT _date, _hkic, _ec,  _hkbc, _di, _repmt, _visa, _adopc, _total FROM #immdRecordSubmit ORDER BY _datetime ASC


-- ---------------------------------------------
-- To Excel sheet: eHSD0013-05: Result From Immd (By Record#)
-- ---------------------------------------------

SELECT _date, _success, _fail FROM #immdRecordResult ORDER BY _datetime ASC


EXEC [proc_SymmetricKey_close]

END

GO

GRANT EXECUTE ON [dbo].[proc_ImmD_getStatistic] TO HCVU
GO
