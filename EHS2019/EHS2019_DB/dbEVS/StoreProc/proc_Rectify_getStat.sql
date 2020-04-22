IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Rectify_getStat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Rectify_getStat]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 30 Jun 2008
-- Description:	Retrieve Immd Interface file Statistic
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_Rectify_getStat]
	--@curDate as DateTime
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================

DECLARE @intCurDay as INTEGER
DECLARE @intPassDay as INTEGER
DECLARE @totalCount as INTEGER

CREATE TABLE #rectifyOutStandingSummary
(
	_intFailDay int,
	_failDay varchar(30),
	_totalCount  int
)

CREATE TABLE #rectifySummary
(
	_intFailCount int,
	_failCount varchar(30),
	_outStandingCount int,
	_validatedCount  int,
	_voidCount	int
)

DECLARE @outstandingCount as INTEGER
DECLARE @validatedCount as INTEGER
DECLARE @voidCount as INTEGER
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================


---------------- Rectify OutStanding Summary ------------

SET @intCurDay = 1

WHILE (@intCurDay < 29 )
	BEGIN
		SET @intPassDay = @intCurDay - 1
		
		Select @totalCount = Count(*) FROM
		(
			(Select Voucher_Acc_ID , Count(*) as Fail_Count, Min(System_Dtm) as First_Validation, Max(System_Dtm) as Last_Validation  From TempVoucherAccMatchLog 
				Where Valid_HKID = 'N' Group By Voucher_Acc_ID
			) as Temp INNER JOIN TempVoucherAccount TVA On 
				Temp.Voucher_Acc_ID = TVA.Voucher_Acc_ID AND Record_Status in ('I','P','C')					
		) WHERE DateDIFF(Day, First_Validation , GetDate()) >= @intPassDay AND DateDIFF(Day, First_Validation , GetDate()) < @intCurDay
		
		
		INSERT INTO #rectifyOutStandingSummary
		(
			_intFailDay,
			_failDay,
			_totalCount
		)
		VALUES
		(
			@intCurDay,
			CAST(@intCurDay as VARCHAR),
			@totalCount
		)

		SET @intCurDay = @intCurDay + 1
	END

	
	SET @intPassDay = @intCurDay - 1

	Select @totalCount = Count(*) FROM
	(
		(Select Voucher_Acc_ID , Count(*) as Fail_Count, Min(System_Dtm) as First_Validation, Max(System_Dtm) as Last_Validation  From TempVoucherAccMatchLog 
			Where Valid_HKID = 'N' Group By Voucher_Acc_ID
		) as Temp INNER JOIN TempVoucherAccount TVA On 
			Temp.Voucher_Acc_ID = TVA.Voucher_Acc_ID AND Record_Status in ('I','P','C')					
	) WHERE DateDIFF(Day, First_Validation , GetDate()) >= @intPassDay

	INSERT INTO #rectifyOutStandingSummary
	(
		_intFailDay,
		_failDay,
		_totalCount
	)
	VALUES
	(
		@intCurDay,
		CAST(@intCurDay as VARCHAR) + ' or above',
		@totalCount
	)

	SELECT _failDay, _totalCount FROM #rectifyOutStandingSummary
	WHERE _totalCount > 0

---------------- Rectify OutStanding Summary ------------

---------------- Rectify Summary ------------

SET @intCurDay = 1

WHILE (@intCurDay < 10 )
	BEGIN
			
		Select @outstandingCount = Count(*) FROM
		(
			(Select Voucher_Acc_ID , Count(*) as Fail_Count, Min(System_Dtm) as First_Validation, Max(System_Dtm) as Last_Validation  From TempVoucherAccMatchLog 
				Where Valid_HKID = 'N' Group By Voucher_Acc_ID
			) as Temp INNER JOIN TempVoucherAccount TVA On 
				Temp.Voucher_Acc_ID = TVA.Voucher_Acc_ID AND Record_Status in ('I','P','C')					
		)WHERE Fail_Count = @intCurDay
		
		Select @validatedCount = Count(*) FROM
		(
			(Select Voucher_Acc_ID , Count(*) as Fail_Count, Min(System_Dtm) as First_Validation, Max(System_Dtm) as Last_Validation  From TempVoucherAccMatchLog 
				Where Valid_HKID = 'N' Group By Voucher_Acc_ID
			) as Temp INNER JOIN TempVoucherAccount TVA On 
				Temp.Voucher_Acc_ID = TVA.Voucher_Acc_ID AND Record_Status in ('V')					
		)WHERE Fail_Count = @intCurDay
		
		Select @voidCount = Count(*) FROM
		(
			(Select Voucher_Acc_ID , Count(*) as Fail_Count, Min(System_Dtm) as First_Validation, Max(System_Dtm) as Last_Validation  From TempVoucherAccMatchLog 
				Where Valid_HKID = 'N' Group By Voucher_Acc_ID
			) as Temp INNER JOIN TempVoucherAccount TVA On 
				Temp.Voucher_Acc_ID = TVA.Voucher_Acc_ID AND Record_Status in ('D')					
		)WHERE Fail_Count = @intCurDay
			
		INSERT INTO #rectifySummary
		(
			_intFailCount,
			_failCount,
			_outStandingCount,
			_validatedCount,
			_voidCount
		)
		VALUES
		(
			@intCurDay,
			@intCurDay,
			@outstandingCount,
			@validatedCount,
			@voidCount
		)
		
		SET @intCurDay = @intCurDay + 1
	END
	
	Select @outstandingCount = Count(*) FROM
	(
		(Select Voucher_Acc_ID , Count(*) as Fail_Count, Min(System_Dtm) as First_Validation, Max(System_Dtm) as Last_Validation  From TempVoucherAccMatchLog 
			Where Valid_HKID = 'N' Group By Voucher_Acc_ID
		) as Temp INNER JOIN TempVoucherAccount TVA On 
			Temp.Voucher_Acc_ID = TVA.Voucher_Acc_ID AND Record_Status in ('I','P','C')					
	)WHERE Fail_Count >= @intCurDay
	
	Select @validatedCount = Count(*) FROM
	(
		(Select Voucher_Acc_ID , Count(*) as Fail_Count, Min(System_Dtm) as First_Validation, Max(System_Dtm) as Last_Validation  From TempVoucherAccMatchLog 
			Where Valid_HKID = 'N' Group By Voucher_Acc_ID
		) as Temp INNER JOIN TempVoucherAccount TVA On 
			Temp.Voucher_Acc_ID = TVA.Voucher_Acc_ID AND Record_Status in ('V')					
	)WHERE Fail_Count >= @intCurDay
	
	Select @voidCount = Count(*) FROM
	(
		(Select Voucher_Acc_ID , Count(*) as Fail_Count, Min(System_Dtm) as First_Validation, Max(System_Dtm) as Last_Validation  From TempVoucherAccMatchLog 
			Where Valid_HKID = 'N' Group By Voucher_Acc_ID
		) as Temp INNER JOIN TempVoucherAccount TVA On 
			Temp.Voucher_Acc_ID = TVA.Voucher_Acc_ID AND Record_Status in ('D')					
	)WHERE Fail_Count >= @intCurDay
	
	INSERT INTO #rectifySummary
	(
		_intFailCount,
		_failCount,
		_outStandingCount,
		_validatedCount,
		_voidCount
	)
	VALUES
	(
		@intCurDay,
		'10 or above',
		@outstandingCount,
		@validatedCount,
		@voidCount
	)
	
	SELECT _failCount, _outStandingCount, _validatedCount, _voidCount FROM #rectifySummary
	
---------------- Rectify Summary ------------

---------------- Rectify Raw data -----------------------

SELECT
	Temp1.Voucher_Acc_ID,
	Fail_Count,
	--Create_Dtm,
	CONVERT(VARCHAR(11), Create_Dtm, 106) + ' ' + CONVERT(VARCHAR(8), Create_Dtm, 108),
	CONVERT(VARCHAR(11), DATEADD(dd, -DATEDIFF(dd, First_Validation, 1), 1), 106),
	CONVERT(VARCHAR(11), DATEADD(dd, -DATEDIFF(dd, Last_Validation, 1), 1), 106),
	
	CASE WHEN Record_Status = 'I' THEN 'Outstanding'
		WHEN Record_Status = 'P' THEN 'Outstanding'
		WHEN Record_Status = 'C' THEN 'Outstanding'
		WHEN Record_Status = 'V' THEN 'Validated and converted to Validated Account'
		WHEN Record_Status = 'D' THEN 'Voided'
		ELSE ''
	END
FROM
(
	Select Voucher_Acc_ID , Count(*) as Fail_Count, Min(System_Dtm) as First_Validation, Max(System_Dtm) as Last_Validation  From TempVoucherAccMatchLog 
	Where Valid_HKID = 'N' Group By Voucher_Acc_ID
) as Temp1 LEFT OUTER JOIN TempVoucherAccount TVA 
	On Temp1.Voucher_Acc_ID = TVA.Voucher_Acc_ID 
	WHERE Record_Status in ('I', 'P', 'C')


---------------- Rectify Raw data -----------------------

END

GO

GRANT EXECUTE ON [dbo].[proc_Rectify_getStat] TO HCVU
GO
