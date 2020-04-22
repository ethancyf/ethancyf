IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_TransactionHourly_Stat_Schedule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_TransactionHourly_Stat_Schedule]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			INT13-0030
-- Modified by:		Tommy LAM
-- Modified date:	24 Dec 2013
-- Description:		Generate Scheme information dynamically
--					Rename Table [_TransactionByHour] to [RpteHSD0010TransactionByHour]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE13-001 
-- Modified by:		Koala CHENG
-- Modified date:	9 May 2013
-- Description:		1. Add EHAPP records
--					2. Add input parameter @Report_Dtm 
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	11 January 2010
-- Description:		Add HSIVSS
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 05 Jan 2009
-- Description:	Prepare the no of transaction group by hour in
--				table _TransactionByHour
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_TransactionHourly_Stat_Schedule]
	-- Add the parameters for the stored procedure here
	@Report_Dtm		DATETIME = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

declare @start_dtm datetime
declare @end_dtm datetime

IF @Report_Dtm IS NOT NULL 
BEGIN
	select @start_dtm  =  CONVERT(VARCHAR(11), @Report_Dtm, 106)
	select @end_dtm  = CONVERT(VARCHAR(11), dateadd(day, 1, @start_dtm), 106)
END
ELSE
BEGIN
	select @start_dtm  =  CONVERT(VARCHAR(11), dateadd(day, -1, GETDATE()), 106)
	select @end_dtm  =  CONVERT(VARCHAR(11), GETDATE(), 106)
END

-- =============================================
-- Declaration
-- =============================================

	DECLARE @current_dtm datetime

	DECLARE @SchemeClaim TABLE (
		Scheme_Code		char(10)
	)

	CREATE TABLE #ReportRawData (
		Transaction_ID		char(20),
		Trans_TimeSlot		smallint,
		Scheme_Code			char(10)
	)

	DECLARE @TransactionByHour TABLE (
		scheme_code	char(10),
		time0		varchar(10),
		time1		varchar(10),
		time2		varchar(10),
		time3		varchar(10),
		time4		varchar(10),
		time5		varchar(10),
		time6		varchar(10),
		time7		varchar(10),
		time8		varchar(10),
		time9		varchar(10),
		time10		varchar(10),
		time11		varchar(10),
		time12		varchar(10),
		time13		varchar(10),
		time14		varchar(10),
		time15		varchar(10),
		time16		varchar(10),
		time17		varchar(10),
		time18		varchar(10),
		time19		varchar(10),
		time20		varchar(10),
		time21		varchar(10),
		time22		varchar(10),
		time23		varchar(10)
	)

-- =============================================
-- Initialization
-- =============================================

	SET @current_dtm = GETDATE()

	DELETE FROM RpteHSD0010TransactionByHour WHERE report_dtm = @start_dtm

-- ---------------------------------------------
-- Prepare @SchemeClaim
-- ---------------------------------------------

	INSERT INTO @SchemeClaim (Scheme_Code) VALUES ('ALL')

	INSERT INTO @SchemeClaim (Scheme_Code)
	SELECT Scheme_Code
	FROM SchemeClaim WITH (NOLOCK)
	WHERE Effective_Dtm <= @current_dtm AND Record_Status = 'A' AND Scheme_Seq = 1

-- =============================================
-- Retrieve Data
-- =============================================

	INSERT INTO #ReportRawData (
		Transaction_ID,
		Trans_TimeSlot,
		Scheme_Code
	)
	SELECT
		VT.Transaction_ID,
		CASE
			WHEN VT.Transaction_Dtm BETWEEN @start_dtm AND DATEADD(HOUR, 1, @start_dtm) THEN 0
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 1, @start_dtm) AND DATEADD(HOUR, 2, @start_dtm) THEN 1
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 2, @start_dtm) AND DATEADD(HOUR, 3, @start_dtm) THEN 2
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 3, @start_dtm) AND DATEADD(HOUR, 4, @start_dtm) THEN 3
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 4, @start_dtm) AND DATEADD(HOUR, 5, @start_dtm) THEN 4
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 5, @start_dtm) AND DATEADD(HOUR, 6, @start_dtm) THEN 5
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 6, @start_dtm) AND DATEADD(HOUR, 7, @start_dtm) THEN 6
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 7, @start_dtm) AND DATEADD(HOUR, 8, @start_dtm) THEN 7
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 8, @start_dtm) AND DATEADD(HOUR, 9, @start_dtm) THEN 8
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 9, @start_dtm) AND DATEADD(HOUR, 10, @start_dtm) THEN 9
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 10, @start_dtm) AND DATEADD(HOUR, 11, @start_dtm) THEN 10
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 11, @start_dtm) AND DATEADD(HOUR, 12, @start_dtm) THEN 11
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 12, @start_dtm) AND DATEADD(HOUR, 13, @start_dtm) THEN 12
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 13, @start_dtm) AND DATEADD(HOUR, 14, @start_dtm) THEN 13
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 14, @start_dtm) AND DATEADD(HOUR, 15, @start_dtm) THEN 14
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 15, @start_dtm) AND DATEADD(HOUR, 16, @start_dtm) THEN 15
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 16, @start_dtm) AND DATEADD(HOUR, 17, @start_dtm) THEN 16
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 17, @start_dtm) AND DATEADD(HOUR, 18, @start_dtm) THEN 17
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 18, @start_dtm) AND DATEADD(HOUR, 19, @start_dtm) THEN 18
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 19, @start_dtm) AND DATEADD(HOUR, 20, @start_dtm) THEN 19
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 20, @start_dtm) AND DATEADD(HOUR, 21, @start_dtm) THEN 20
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 21, @start_dtm) AND DATEADD(HOUR, 22, @start_dtm) THEN 21
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 22, @start_dtm) AND DATEADD(HOUR, 23, @start_dtm) THEN 22
			WHEN VT.Transaction_Dtm BETWEEN DATEADD(HOUR, 23, @start_dtm) AND DATEADD(HOUR, 24, @start_dtm) THEN 23
		END,
		VT.Scheme_Code
	FROM VoucherTransaction VT WITH (NOLOCK)
	WHERE VT.Transaction_Dtm BETWEEN @start_dtm AND @end_dtm

-- ---------------------------------------------
-- Result for All Scheme
-- ---------------------------------------------

	INSERT INTO @TransactionByHour (
		scheme_code,
		time0, time1, time2, time3, time4,
		time5, time6, time7, time8, time9,
		time10, time11, time12, time13, time14,
		time15, time16, time17, time18, time19,
		time20, time21, time22, time23
	)
	SELECT 'ALL', *
	FROM (
		SELECT
			Transaction_ID,
			Trans_TimeSlot
		FROM
			#ReportRawData
	) DATA
	PIVOT (
		COUNT(Transaction_ID)
		FOR Trans_TimeSlot
		IN ([0],[1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23])
	) FUNC

-- ---------------------------------------------
-- Result for Each Scheme
-- ---------------------------------------------

	INSERT INTO @TransactionByHour (
		scheme_code,
		time0, time1, time2, time3, time4,
		time5, time6, time7, time8, time9,
		time10, time11, time12, time13, time14,
		time15, time16, time17, time18, time19,
		time20, time21, time22, time23
	)
	SELECT *
	FROM (
		SELECT
			Transaction_ID,
			Trans_TimeSlot,
			Scheme_Code
		FROM
			#ReportRawData
	) DATA
	PIVOT (
		COUNT(Transaction_ID)
		FOR Trans_TimeSlot
		IN ([0],[1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23])
	) FUNC

-- =============================================
-- Return results
-- =============================================

	INSERT INTO RpteHSD0010TransactionByHour (
		system_dtm,
		report_dtm,
		scheme_code,
		time0, time1, time2, time3, time4,
		time5, time6, time7, time8, time9,
		time10, time11, time12, time13, time14,
		time15, time16, time17, time18, time19,
		time20, time21, time22, time23
	)
	SELECT
		@current_dtm,
		@start_dtm,
		SC.Scheme_Code,
		ISNULL(TBH.time0, 0),
		ISNULL(TBH.time1, 0),
		ISNULL(TBH.time2, 0),
		ISNULL(TBH.time3, 0),
		ISNULL(TBH.time4, 0),
		ISNULL(TBH.time5, 0),
		ISNULL(TBH.time6, 0),
		ISNULL(TBH.time7, 0),
		ISNULL(TBH.time8, 0),
		ISNULL(TBH.time9, 0),
		ISNULL(TBH.time10, 0),
		ISNULL(TBH.time11, 0),
		ISNULL(TBH.time12, 0),
		ISNULL(TBH.time13, 0),
		ISNULL(TBH.time14, 0),
		ISNULL(TBH.time15, 0),
		ISNULL(TBH.time16, 0),
		ISNULL(TBH.time17, 0),
		ISNULL(TBH.time18, 0),
		ISNULL(TBH.time19, 0),
		ISNULL(TBH.time20, 0),
		ISNULL(TBH.time21, 0),
		ISNULL(TBH.time22, 0),
		ISNULL(TBH.time23, 0)
	FROM
		@SchemeClaim SC
			LEFT JOIN @TransactionByHour TBH
				ON SC.Scheme_Code = TBH.scheme_code


	DROP TABLE #ReportRawData

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_TransactionHourly_Stat_Schedule] TO HCVU
GO
