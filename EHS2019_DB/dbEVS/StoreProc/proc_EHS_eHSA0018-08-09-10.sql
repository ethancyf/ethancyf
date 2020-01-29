IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSA0018-08-09-10]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSA0018-08-09-10]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	1 February 2011
-- Description:		eHSA0018 - FHB statistics for 2010
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSA0018-08-09-10]
	@Year	int
AS BEGIN

	SET NOCOUNT ON;
	
-- =============================================
-- Declaration
-- =============================================
	DECLARE @Transaction table (
		Transaction_Month		smallint,
		Service_Type			char(5),
		SourceApp				varchar(10),
		No_Of_Transaction		int
	)
		
	DECLARE @ResultTableWebFull table(
		Result_Seq				smallint,
		Result_Value1			varchar(200) DEFAULT '',
		Result_Value2			varchar(50) DEFAULT '',
		Result_Value3			varchar(30) DEFAULT '',
		Result_Value4			varchar(30) DEFAULT '',
		Result_Value5			varchar(30) DEFAULT '',
		Result_Value6			varchar(30) DEFAULT '',
		Result_Value7			varchar(30) DEFAULT '',
		Result_Value8			varchar(30) DEFAULT '',
		Result_Value9			varchar(30) DEFAULT '',
		Result_Value10			varchar(30) DEFAULT '',
		Result_Value11			varchar(30) DEFAULT '',
		Result_Value12			varchar(30) DEFAULT '',
		Result_Value13			varchar(30) DEFAULT ''
	)

	DECLARE @ResultTableWebText table(
		Result_Seq				smallint,
		Result_Value1			varchar(200) DEFAULT '',
		Result_Value2			varchar(50) DEFAULT '',
		Result_Value3			varchar(30) DEFAULT '',
		Result_Value4			varchar(30) DEFAULT '',
		Result_Value5			varchar(30) DEFAULT '',
		Result_Value6			varchar(30) DEFAULT '',
		Result_Value7			varchar(30) DEFAULT '',
		Result_Value8			varchar(30) DEFAULT '',
		Result_Value9			varchar(30) DEFAULT '',
		Result_Value10			varchar(30) DEFAULT '',
		Result_Value11			varchar(30) DEFAULT '',
		Result_Value12			varchar(30) DEFAULT '',
		Result_Value13			varchar(30) DEFAULT ''
	)
	
	DECLARE @ResultTableIVRS table(
		Result_Seq				smallint,
		Result_Value1			varchar(200) DEFAULT '',
		Result_Value2			varchar(50) DEFAULT '',
		Result_Value3			varchar(30) DEFAULT '',
		Result_Value4			varchar(30) DEFAULT '',
		Result_Value5			varchar(30) DEFAULT '',
		Result_Value6			varchar(30) DEFAULT '',
		Result_Value7			varchar(30) DEFAULT '',
		Result_Value8			varchar(30) DEFAULT '',
		Result_Value9			varchar(30) DEFAULT '',
		Result_Value10			varchar(30) DEFAULT '',
		Result_Value11			varchar(30) DEFAULT '',
		Result_Value12			varchar(30) DEFAULT '',
		Result_Value13			varchar(30) DEFAULT ''
	)
	
	DECLARE @i smallint
	DECLARE @ENU	int
	DECLARE @RCM	int
	DECLARE @RCP	int
	DECLARE @RDT	int
	DECLARE @RMP	int
	DECLARE @RMT	int
	DECLARE @RNU	int
	DECLARE @ROT	int
	DECLARE @RPT	int
	DECLARE @RRD	int
	DECLARE @Total	int
	

-- =============================================
-- Retrieve data
-- =============================================

	INSERT INTO @Transaction (
		Transaction_Month,
		Service_Type,
		SourceApp,
		No_Of_Transaction
	)
	SELECT
		MONTH(Transaction_Dtm) AS [Transaction_Month],
		Service_Type,
		SourceApp,
		COUNT(1) AS [No_Of_Transaction]
	FROM
		VoucherTransaction
	WHERE
		Scheme_Code = 'HCVS'
			AND YEAR(Transaction_Dtm) = @Year
			AND Record_Status NOT IN ('I', 'D')
			AND ISNULL(Invalidation, '') <> 'I'
	GROUP BY
		MONTH(Transaction_Dtm),
		Service_Type,
		SourceApp



-- =============================================
-- Build frame (Web)
-- =============================================

-- ---------------------------------------------
-- Header
-- ---------------------------------------------

	INSERT INTO @ResultTableWebFull (Result_Seq, Result_Value1) VALUES
	(0, 'eHSA0018-08: Cumulative monthly statistics of transactions submitted through Internet (Full version) in 2010, broken down by professions')
	
	INSERT INTO @ResultTableWebFull (Result_Seq) VALUES
	(1)
	
	INSERT INTO @ResultTableWebFull (Result_Seq, Result_Value1, Result_Value2) VALUES
	(2, NULL, 'No. of transactions')

	INSERT INTO @ResultTableWebFull (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13) VALUES
	(10, NULL, 'ENU', 'RCM', 'RCP', 'RDT', 'RMP', 'RMT', 'RNU', 'ROT', 'RPT', 'RRD', NULL, 'Total')


-- ---------------------------------------------
-- Data
-- ---------------------------------------------
					
	SELECT @i = 1
	
	WHILE @i <= 12 BEGIN
		SELECT @ENU = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'ENU' AND (SourceApp = 'WEB' OR SourceApp = 'WEB-FULL')
		SELECT @RCM = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RCM' AND (SourceApp = 'WEB' OR SourceApp = 'WEB-FULL') 
		SELECT @RCP = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RCP' AND (SourceApp = 'WEB' OR SourceApp = 'WEB-FULL')
		SELECT @RDT = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RDT' AND (SourceApp = 'WEB' OR SourceApp = 'WEB-FULL')
		SELECT @RMP = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RMP' AND (SourceApp = 'WEB' OR SourceApp = 'WEB-FULL')
		SELECT @RMT = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RMT' AND (SourceApp = 'WEB' OR SourceApp = 'WEB-FULL')
		SELECT @RNU = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RNU' AND (SourceApp = 'WEB' OR SourceApp = 'WEB-FULL')
		SELECT @ROT = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'ROT' AND (SourceApp = 'WEB' OR SourceApp = 'WEB-FULL')
		SELECT @RPT = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RPT' AND (SourceApp = 'WEB' OR SourceApp = 'WEB-FULL')
		SELECT @RRD = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RRD' AND (SourceApp = 'WEB' OR SourceApp = 'WEB-FULL')
		SELECT @Total = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND (SourceApp = 'WEB' OR SourceApp = 'WEB-FULL')
	
		INSERT INTO @ResultTableWebFull (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
		SELECT
			10 + @i,
			CONVERT(varchar(10), DATEADD(mm, @i - 1, CONVERT(varchar, @Year) + '-01-01'), 120),
			@ENU, 
			@RCM, 
			@RCP, 
			@RDT, 
			@RMP, 
			@RMT, 
			@RNU, 
			@ROT, 
			@RPT, 
			@RRD,
			NULL,
			@Total
	
		SELECT @i = @i + 1
	
	END


-- =============================================
-- Build frame (Web-Text)
-- =============================================

-- ---------------------------------------------
-- Header
-- ---------------------------------------------

	INSERT INTO @ResultTableWebText (Result_Seq, Result_Value1) VALUES
	(0, 'eHSA0018-09: Cumulative monthly statistics of transactions submitted through Internet (Text-only version) in 2010, broken down by professions')
	
	INSERT INTO @ResultTableWebText (Result_Seq) VALUES
	(1)
	
	INSERT INTO @ResultTableWebText (Result_Seq, Result_Value1, Result_Value2) VALUES
	(2, NULL, 'No. of transactions')

	INSERT INTO @ResultTableWebText (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13) VALUES
	(10, NULL, 'ENU', 'RCM', 'RCP', 'RDT', 'RMP', 'RMT', 'RNU', 'ROT', 'RPT', 'RRD', NULL, 'Total')


-- ---------------------------------------------
-- Data
-- ---------------------------------------------
					
	SELECT @i = 1
	
	WHILE @i <= 12 BEGIN
		SELECT @ENU = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'ENU' AND SourceApp = 'WEB-TEXT'
		SELECT @RCM = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RCM' AND SourceApp = 'WEB-TEXT'
		SELECT @RCP = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RCP' AND SourceApp = 'WEB-TEXT'
		SELECT @RDT = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RDT' AND SourceApp = 'WEB-TEXT'
		SELECT @RMP = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RMP' AND SourceApp = 'WEB-TEXT'
		SELECT @RMT = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RMT' AND SourceApp = 'WEB-TEXT'
		SELECT @RNU = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RNU' AND SourceApp = 'WEB-TEXT'
		SELECT @ROT = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'ROT' AND SourceApp = 'WEB-TEXT'
		SELECT @RPT = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RPT' AND SourceApp = 'WEB-TEXT'
		SELECT @RRD = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RRD' AND SourceApp = 'WEB-TEXT'
		SELECT @Total = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND SourceApp = 'WEB-TEXT'
	
		INSERT INTO @ResultTableWebText (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
		SELECT
			10 + @i,
			CONVERT(varchar(10), DATEADD(mm, @i - 1, CONVERT(varchar, @Year) + '-01-01'), 120),
			@ENU, 
			@RCM, 
			@RCP, 
			@RDT, 
			@RMP, 
			@RMT, 
			@RNU, 
			@ROT, 
			@RPT, 
			@RRD,
			NULL,
			@Total
	
		SELECT @i = @i + 1
	
	END


-- =============================================
-- Build frame (IVRS)
-- =============================================

-- ---------------------------------------------
-- Header
-- ---------------------------------------------

	INSERT INTO @ResultTableIVRS (Result_Seq, Result_Value1) VALUES
	(0, 'eHSA0018-10: Cumulative monthly statistics of transactions submitted through IVRS in 2010, broken down by professions')
	
	INSERT INTO @ResultTableIVRS (Result_Seq) VALUES
	(1)
	
	INSERT INTO @ResultTableIVRS (Result_Seq, Result_Value1, Result_Value2) VALUES
	(2, NULL, 'No. of transactions')

	INSERT INTO @ResultTableIVRS (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13) VALUES
	(10, NULL, 'ENU', 'RCM', 'RCP', 'RDT', 'RMP', 'RMT', 'RNU', 'ROT', 'RPT', 'RRD', NULL, 'Total')


-- ---------------------------------------------
-- Data
-- ---------------------------------------------
					
	SELECT @i = 1
	
	WHILE @i <= 12 BEGIN
		SELECT @ENU = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'ENU' AND SourceApp = 'IVRS'
		SELECT @RCM = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RCM' AND SourceApp = 'IVRS'
		SELECT @RCP = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RCP' AND SourceApp = 'IVRS'
		SELECT @RDT = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RDT' AND SourceApp = 'IVRS'
		SELECT @RMP = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RMP' AND SourceApp = 'IVRS'
		SELECT @RMT = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RMT' AND SourceApp = 'IVRS'
		SELECT @RNU = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RNU' AND SourceApp = 'IVRS'
		SELECT @ROT = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'ROT' AND SourceApp = 'IVRS'
		SELECT @RPT = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RPT' AND SourceApp = 'IVRS'
		SELECT @RRD = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND Service_Type = 'RRD' AND SourceApp = 'IVRS'
		SELECT @Total = ISNULL(SUM(No_Of_Transaction), 0) FROM @Transaction WHERE Transaction_Month <= @i AND SourceApp = 'IVRS'
	
		INSERT INTO @ResultTableIVRS (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
		SELECT
			10 + @i,
			CONVERT(varchar(10), DATEADD(mm, @i - 1, CONVERT(varchar, @Year) + '-01-01'), 120),
			@ENU, 
			@RCM, 
			@RCP, 
			@RDT, 
			@RMP, 
			@RMT, 
			@RNU, 
			@ROT, 
			@RPT, 
			@RRD,
			NULL,
			@Total
	
		SELECT @i = @i + 1
	
	END


-- =============================================
-- Return result
-- =============================================

	SELECT 
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6,
		Result_Value7,
		Result_Value8,
		Result_Value9,
		Result_Value10,
		Result_Value11,
		Result_Value12,
		Result_Value13
	FROM
		@ResultTableWebFull
	ORDER BY
		Result_Seq
		
--

	SELECT 
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6,
		Result_Value7,
		Result_Value8,
		Result_Value9,
		Result_Value10,
		Result_Value11,
		Result_Value12,
		Result_Value13
	FROM
		@ResultTableWebText
	ORDER BY
		Result_Seq
		
--

	SELECT 
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6,
		Result_Value7,
		Result_Value8,
		Result_Value9,
		Result_Value10,
		Result_Value11,
		Result_Value12,
		Result_Value13
	FROM
		@ResultTableIVRS
	ORDER BY
		Result_Seq


END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSA0018-08-09-10] TO HCVU
GO

