IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSA0018-03-04]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSA0018-03-04]
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

CREATE PROCEDURE [dbo].[proc_EHS_eHSA0018-03-04]
	@Year	int
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @Transaction table (
		Transaction_Month	tinyint,
		Service_Type		varchar(3),
		Transaction_Count	int,
		Voucher_Count		int
	)

	DECLARE	@ResultTable1 table (
		Result_Seq		tinyint,
		Result_Value1	varchar(100) DEFAULT '',
		Result_Value2	varchar(30) DEFAULT '',
		Result_Value3	varchar(10) DEFAULT '',
		Result_Value4	varchar(10) DEFAULT '',
		Result_Value5	varchar(10) DEFAULT '',
		Result_Value6	varchar(10) DEFAULT '',
		Result_Value7	varchar(10) DEFAULT '',
		Result_Value8	varchar(10) DEFAULT '',
		Result_Value9	varchar(10) DEFAULT '',
		Result_Value10	varchar(10) DEFAULT '',
		Result_Value11	varchar(10) DEFAULT '',
		Result_Value12	varchar(10) DEFAULT '',
		Result_Value13	varchar(10) DEFAULT ''
	)
	
	DECLARE	@ResultTable2 table (
		Result_Seq		tinyint,
		Result_Value1	varchar(100) DEFAULT '',
		Result_Value2	varchar(30) DEFAULT '',
		Result_Value3	varchar(10) DEFAULT '',
		Result_Value4	varchar(10) DEFAULT '',
		Result_Value5	varchar(10) DEFAULT '',
		Result_Value6	varchar(10) DEFAULT '',
		Result_Value7	varchar(10) DEFAULT '',
		Result_Value8	varchar(10) DEFAULT '',
		Result_Value9	varchar(10) DEFAULT '',
		Result_Value10	varchar(10) DEFAULT '',
		Result_Value11	varchar(10) DEFAULT '',
		Result_Value12	varchar(10) DEFAULT '',
		Result_Value13	varchar(10) DEFAULT ''
	)
	
	DECLARE @i int
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
		Transaction_Count,
		Voucher_Count
	)
	SELECT
		MONTH(Transaction_Dtm), 
		Service_Type, 
		COUNT(1),
		SUM(Voucher_Before_Claim - Voucher_After_Claim) 
	FROM
		VoucherTransaction
	WHERE
		YEAR(Transaction_Dtm) = @Year
			AND Scheme_Code = 'HCVS'
			AND Record_Status NOT IN ('I', 'D')
			AND ISNULL(Invalidation, '') <> 'I'
	GROUP BY
		MONTH(Transaction_Dtm), 
		Service_Type


-- =============================================
-- Build frame (03)
-- =============================================

-- ---------------------------------------------
-- Header
-- ---------------------------------------------

	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1) VALUES
	(0, 'eHSA0018-03: Monthly statistics of transactions claimed in 2010, broken down by professions')
	
	INSERT INTO @ResultTable1 (Result_Seq) VALUES
	(1)
	
	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1, Result_Value2) VALUES
	(2, NULL, 'No. of transactions')

	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13) VALUES
	(10, NULL, 'ENU', 'RCM', 'RCP', 'RDT', 'RMP', 'RMT', 'RNU', 'ROT', 'RPT', 'RRD', NULL, 'Total')


-- ---------------------------------------------
-- Data
-- ---------------------------------------------

	SET @i = 1
	
	WHILE @i <= 12 BEGIN
		SET @ENU = 0
		SET @RCM = 0
		SET @RCP = 0
		SET @RDT = 0
		SET @RMP = 0
		SET @RMT = 0
		SET @RNU = 0
		SET @ROT = 0
		SET @RPT = 0
		SET @RRD = 0
		SET @Total = 0

		SELECT @ENU = Transaction_Count FROM @Transaction WHERE Transaction_Month = @i AND Service_Type = 'ENU'
		SELECT @RCM = Transaction_Count FROM @Transaction WHERE Transaction_Month = @i AND Service_Type = 'RCM' 
		SELECT @RCP = Transaction_Count FROM @Transaction WHERE Transaction_Month = @i AND Service_Type = 'RCP'
		SELECT @RDT = Transaction_Count FROM @Transaction WHERE Transaction_Month = @i AND Service_Type = 'RDT' 
		SELECT @RMP = Transaction_Count FROM @Transaction WHERE Transaction_Month = @i AND Service_Type = 'RMP' 
		SELECT @RMT = Transaction_Count FROM @Transaction WHERE Transaction_Month = @i AND Service_Type = 'RMT' 
		SELECT @RNU = Transaction_Count FROM @Transaction WHERE Transaction_Month = @i AND Service_Type = 'RNU' 
		SELECT @ROT = Transaction_Count FROM @Transaction WHERE Transaction_Month = @i AND Service_Type = 'ROT' 
		SELECT @RPT = Transaction_Count FROM @Transaction WHERE Transaction_Month = @i AND Service_Type = 'RPT' 
		SELECT @RRD = Transaction_Count FROM @Transaction WHERE Transaction_Month = @i AND Service_Type = 'RRD' 
		SELECT @Total = ISNULL(SUM(Transaction_Count), 0) FROM @Transaction WHERE Transaction_Month = @i
	
		INSERT INTO @ResultTable1 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
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

		SET @i = @i + 1
	
	END


-- ---------------------------------------------
-- Footer
-- ---------------------------------------------

	INSERT INTO @ResultTable1 (Result_Seq) VALUES
	(30)

	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	SELECT
		40,
		'Total',
		SUM(CONVERT(int, Result_Value2)),
		SUM(CONVERT(int, Result_Value3)),
		SUM(CONVERT(int, Result_Value4)),
		SUM(CONVERT(int, Result_Value5)),
		SUM(CONVERT(int, Result_Value6)),
		SUM(CONVERT(int, Result_Value7)),
		SUM(CONVERT(int, Result_Value8)),
		SUM(CONVERT(int, Result_Value9)),
		SUM(CONVERT(int, Result_Value10)),
		SUM(CONVERT(int, Result_Value11)),
		NULL,
		SUM(CONVERT(int, Result_Value13))
	FROM
		@ResultTable1
	WHERE
		Result_Seq BETWEEN 11 AND 22


-- =============================================
-- Build frame
-- =============================================

-- ---------------------------------------------
-- Header
-- ---------------------------------------------

	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1) VALUES
	(0, 'eHSA0018-04: Monthly statistics of vouchers claimed in 2010, broken down by professions')
	
	INSERT INTO @ResultTable2 (Result_Seq) VALUES
	(1)
	
	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1, Result_Value2) VALUES
	(2, NULL, 'No. of vouchers')

	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13) VALUES
	(10, NULL, 'ENU', 'RCM', 'RCP', 'RDT', 'RMP', 'RMT', 'RNU', 'ROT', 'RPT', 'RRD', NULL, 'Total')


-- ---------------------------------------------
-- Data
-- ---------------------------------------------

	SET @i = 1
	
	WHILE @i <= 12 BEGIN
		SET @ENU = 0
		SET @RCM = 0
		SET @RCP = 0
		SET @RDT = 0
		SET @RMP = 0
		SET @RMT = 0
		SET @RNU = 0
		SET @ROT = 0
		SET @RPT = 0
		SET @RRD = 0
		SET @Total = 0

		SELECT @ENU = Voucher_Count FROM @Transaction WHERE Transaction_Month = @i AND Service_Type = 'ENU'
		SELECT @RCM = Voucher_Count FROM @Transaction WHERE Transaction_Month = @i AND Service_Type = 'RCM' 
		SELECT @RCP = Voucher_Count FROM @Transaction WHERE Transaction_Month = @i AND Service_Type = 'RCP'
		SELECT @RDT = Voucher_Count FROM @Transaction WHERE Transaction_Month = @i AND Service_Type = 'RDT' 
		SELECT @RMP = Voucher_Count FROM @Transaction WHERE Transaction_Month = @i AND Service_Type = 'RMP' 
		SELECT @RMT = Voucher_Count FROM @Transaction WHERE Transaction_Month = @i AND Service_Type = 'RMT' 
		SELECT @RNU = Voucher_Count FROM @Transaction WHERE Transaction_Month = @i AND Service_Type = 'RNU' 
		SELECT @ROT = Voucher_Count FROM @Transaction WHERE Transaction_Month = @i AND Service_Type = 'ROT' 
		SELECT @RPT = Voucher_Count FROM @Transaction WHERE Transaction_Month = @i AND Service_Type = 'RPT' 
		SELECT @RRD = Voucher_Count FROM @Transaction WHERE Transaction_Month = @i AND Service_Type = 'RRD' 
		SELECT @Total = ISNULL(SUM(Voucher_Count), 0) FROM @Transaction WHERE Transaction_Month = @i
	
		INSERT INTO @ResultTable2 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
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

		SET @i = @i + 1
	
	END


-- ---------------------------------------------
-- Footer
-- ---------------------------------------------

	INSERT INTO @ResultTable2 (Result_Seq) VALUES
	(30)

	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	SELECT
		40,
		'Total',
		SUM(CONVERT(int, Result_Value2)),
		SUM(CONVERT(int, Result_Value3)),
		SUM(CONVERT(int, Result_Value4)),
		SUM(CONVERT(int, Result_Value5)),
		SUM(CONVERT(int, Result_Value6)),
		SUM(CONVERT(int, Result_Value7)),
		SUM(CONVERT(int, Result_Value8)),
		SUM(CONVERT(int, Result_Value9)),
		SUM(CONVERT(int, Result_Value10)),
		SUM(CONVERT(int, Result_Value11)),
		NULL,
		SUM(CONVERT(int, Result_Value13))
	FROM
		@ResultTable2
	WHERE
		Result_Seq BETWEEN 11 AND 22


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
		@ResultTable1
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
		@ResultTable2
	ORDER BY
		Result_Seq


END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSA0018-03-04] TO HCVU
GO

