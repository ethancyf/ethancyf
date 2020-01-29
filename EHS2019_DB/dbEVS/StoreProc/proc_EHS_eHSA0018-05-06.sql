IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSA0018-05-06]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSA0018-05-06]
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

CREATE PROCEDURE [dbo].[proc_EHS_eHSA0018-05-06]
	@Year	int
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @Transaction table (
		Transaction_Month	tinyint,
		District_Board		varchar(20),
		Transaction_Count	int,
		Voucher_Count		int
	)
	
	DECLARE @TransactionGroup table (
		Transaction_Month	tinyint,
		District_Board		varchar(20),
		Transaction_Count	int,
		Voucher_Count		int
	)

	DECLARE @ResultTable1 table (
		Result_Seq		tinyint,
		Result_Value1	varchar(100) DEFAULT '',
		Result_Value2	varchar(20) DEFAULT '',
		Result_Value3	varchar(20) DEFAULT '',
		Result_Value4	varchar(20) DEFAULT '',
		Result_Value5	varchar(20) DEFAULT '',
		Result_Value6	varchar(20) DEFAULT '',
		Result_Value7	varchar(20) DEFAULT '',
		Result_Value8	varchar(20) DEFAULT '',
		Result_Value9	varchar(20) DEFAULT '',
		Result_Value10	varchar(20) DEFAULT '',
		Result_Value11	varchar(20) DEFAULT '',
		Result_Value12	varchar(20) DEFAULT '',
		Result_Value13	varchar(20) DEFAULT '',
		Result_Value14	varchar(20) DEFAULT '',
		Result_Value15	varchar(20) DEFAULT '',
		Result_Value16	varchar(20) DEFAULT '',
		Result_Value17	varchar(20) DEFAULT '',
		Result_Value18	varchar(20) DEFAULT '',
		Result_Value19	varchar(20) DEFAULT '',
		Result_Value20	varchar(20) DEFAULT '',
		Result_Value21	varchar(20) DEFAULT ''
	)
	
	DECLARE @ResultTable2 table (
		Result_Seq		tinyint,
		Result_Value1	varchar(100) DEFAULT '',
		Result_Value2	varchar(20) DEFAULT '',
		Result_Value3	varchar(20) DEFAULT '',
		Result_Value4	varchar(20) DEFAULT '',
		Result_Value5	varchar(20) DEFAULT '',
		Result_Value6	varchar(20) DEFAULT '',
		Result_Value7	varchar(20) DEFAULT '',
		Result_Value8	varchar(20) DEFAULT '',
		Result_Value9	varchar(20) DEFAULT '',
		Result_Value10	varchar(20) DEFAULT '',
		Result_Value11	varchar(20) DEFAULT '',
		Result_Value12	varchar(20) DEFAULT '',
		Result_Value13	varchar(20) DEFAULT '',
		Result_Value14	varchar(20) DEFAULT '',
		Result_Value15	varchar(20) DEFAULT '',
		Result_Value16	varchar(20) DEFAULT '',
		Result_Value17	varchar(20) DEFAULT '',
		Result_Value18	varchar(20) DEFAULT '',
		Result_Value19	varchar(20) DEFAULT '',
		Result_Value20	varchar(20) DEFAULT '',
		Result_Value21	varchar(20) DEFAULT ''
	)
	
	DECLARE @i		int
	DECLARE @CW		int
	DECLARE @EAST	int
	DECLARE @SOUTH	int
	DECLARE @WC		int
	DECLARE @KC		int
	DECLARE @KT		int
	DECLARE @SSP	int
	DECLARE @WTS	int
	DECLARE @YTM	int
	DECLARE @ISL	int
	DECLARE @KTS	int
	DECLARE @NORTH	int
	DECLARE @SK		int
	DECLARE @ST		int
	DECLARE @TM		int
	DECLARE @TP		int
	DECLARE @TW		int
	DECLARE @YL		int
	DECLARE @Total	int


-- =============================================
-- Retrieve data
-- =============================================

-- Unstructural address (Practice.District IS NOT NULL)

	INSERT INTO @Transaction (
		Transaction_Month,
		District_Board,
		Transaction_Count,
		Voucher_Count
	)
	SELECT
		MONTH(VT.Transaction_Dtm),
		D.District_Board,
		COUNT(1),
		SUM(VT.Voucher_Before_Claim - VT.Voucher_After_Claim)
	FROM
		VoucherTransaction VT
			INNER JOIN Practice P
				ON VT.SP_ID = P.SP_ID
					AND VT.Practice_Display_Seq = P.Display_Seq
					AND P.District IS NOT NULL
			INNER JOIN District D
				ON P.District = D.District_Code
	WHERE
		YEAR(VT.Transaction_Dtm) = @Year
			AND VT.Scheme_Code = 'HCVS'
			AND VT.Record_Status NOT IN ('I', 'D')
			AND ISNULL(VT.Invalidation, '') <> 'I'
	GROUP BY
		MONTH(VT.Transaction_Dtm),
		D.District_Board


-- Structural address (Practice.District IS NULL, Practice.Address_Code IS NOT NULL)

	INSERT INTO @Transaction (
		Transaction_Month,
		District_Board,
		Transaction_Count,
		Voucher_Count
	)
	SELECT
		MONTH(VT.Transaction_Dtm),
		D.District_Board,
		COUNT(1),
		SUM(VT.Voucher_Before_Claim - VT.Voucher_After_Claim)
	FROM
		VoucherTransaction VT
			INNER JOIN Practice P
				ON VT.SP_ID = P.SP_ID
					AND VT.Practice_Display_Seq = P.Display_Seq
					AND P.District IS NULL
			INNER JOIN address_detail AD
				ON P.Address_Code = AD.Record_ID
			INNER JOIN District D
				ON AD.District_Code = D.District_Code
	WHERE
		YEAR(VT.Transaction_Dtm) = @Year
			AND VT.Scheme_Code = 'HCVS'
			AND VT.Record_Status NOT IN ('I', 'D')
			AND ISNULL(VT.Invalidation, '') <> 'I'
	GROUP BY
		MONTH(VT.Transaction_Dtm),
		D.District_Board


-- Sum

	INSERT INTO @TransactionGroup (
		Transaction_Month,
		District_Board,
		Transaction_Count,
		Voucher_Count
	)
	SELECT
		Transaction_Month,
		District_Board,
		SUM(Transaction_Count),
		SUM(Voucher_Count)
	FROM
		@Transaction
	GROUP BY
		Transaction_Month,
		District_Board


-- =============================================
-- Build frame (05)
-- =============================================

-- ---------------------------------------------
-- Header
-- ---------------------------------------------

	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1) VALUES
	(0, 'eHSA0018-05: Monthly statistics of transactions claimed in 2010, broken down by 18 districts')
	
	INSERT INTO @ResultTable1 (Result_Seq) VALUES
	(1)
	
	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1, Result_Value2) VALUES
	(2, NULL, 'No. of transactions')

	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	(3, NULL, 'Hong Kong', NULL, NULL, NULL, 'Kowloon', NULL, NULL, NULL, NULL, 'New Territories')

	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20, Result_Value21) VALUES
	(10, NULL, 'CW', 'EAST', 'SOUTH', 'WC', 'KC', 'KT', 'SSP', 'WTS', 'YTM', 'ISL', 'KTS', 'NORTH', 'SK', 'ST', 'TM', 'TP', 'TW', 'YL', NULL, 'Total')


-- ---------------------------------------------
-- Data
-- ---------------------------------------------

	SET @i = 1
	
	WHILE @i <= 12 BEGIN
		SET @CW = 0
		SET @EAST = 0
		SET @SOUTH = 0
		SET @WC = 0
		SET @KC = 0
		SET @KT = 0
		SET @SSP = 0
		SET @WTS = 0
		SET @YTM = 0
		SET @ISL = 0
		SET @KTS = 0
		SET @NORTH = 0
		SET @SK = 0
		SET @ST = 0
		SET @TM = 0
		SET @TP = 0
		SET @TW = 0
		SET @YL = 0
		SET @Total = 0
		
		SELECT @CW = Transaction_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'CENTRAL & WEST.'
		SELECT @EAST = Transaction_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'EASTERN'
		SELECT @SOUTH = Transaction_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'SOUTHERN'
		SELECT @WC = Transaction_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'WAN CHAI'
		
		SELECT @KC = Transaction_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'KOWLOON CITY'
		SELECT @KT = Transaction_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'KWUN TONG'
		SELECT @SSP = Transaction_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'SHAM SHUI PO'
		SELECT @WTS = Transaction_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'WONG TAI SIN'
		SELECT @YTM = Transaction_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'YAU TSIM MONG'
		
		SELECT @ISL = Transaction_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'ISLANDS'
		SELECT @KTS = Transaction_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'KWAI TSING'
		SELECT @NORTH = Transaction_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'NORTH'
		SELECT @SK = Transaction_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'SAI KUNG'
		SELECT @ST = Transaction_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'SHA TIN'
		SELECT @TM = Transaction_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'TUEN MUN'
		SELECT @TP = Transaction_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'TAI PO'
		SELECT @TW = Transaction_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'TSUEN WAN'
		SELECT @YL = Transaction_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'YUEN LONG'
		
		SELECT @Total = ISNULL(SUM(Transaction_Count), 0) FROM @TransactionGroup WHERE Transaction_Month = @i
	
		INSERT INTO @ResultTable1 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20, Result_Value21)
		SELECT
			10 + @i,
			CONVERT(varchar(10), DATEADD(mm, @i - 1, CONVERT(varchar, @Year) + '-01-01'), 120),
			@CW,
			@EAST,
			@SOUTH,
			@WC,
			@KC,
			@KT,
			@SSP,
			@WTS,
			@YTM,
			@ISL,
			@KTS,
			@NORTH,
			@SK,
			@ST,
			@TM,
			@TP,
			@TW,
			@YL,
			NULL,
			@Total

		SET @i = @i + 1
	
	END


-- ---------------------------------------------
-- Footer
-- ---------------------------------------------

	INSERT INTO @ResultTable1 (Result_Seq) VALUES
	(30)

	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20, Result_Value21)
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
		SUM(CONVERT(int, Result_Value12)),
		SUM(CONVERT(int, Result_Value13)),
		SUM(CONVERT(int, Result_Value14)),
		SUM(CONVERT(int, Result_Value15)),
		SUM(CONVERT(int, Result_Value16)),
		SUM(CONVERT(int, Result_Value17)),
		SUM(CONVERT(int, Result_Value18)),
		SUM(CONVERT(int, Result_Value19)),
		NULL,
		SUM(CONVERT(int, Result_Value21))
	FROM
		@ResultTable1
	WHERE
		Result_Seq BETWEEN 11 AND 22


-- =============================================
-- Build frame (06)
-- =============================================

-- ---------------------------------------------
-- Header
-- ---------------------------------------------

	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1) VALUES
	(0, 'eHSA0018-06: Monthly statistics of vouchers claimed in 2010, broken down by 18 districts')
	
	INSERT INTO @ResultTable2 (Result_Seq) VALUES
	(1)
	
	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1, Result_Value2) VALUES
	(2, NULL, 'No. of vouchers')

	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	(3, NULL, 'Hong Kong', NULL, NULL, NULL, 'Kowloon', NULL, NULL, NULL, NULL, 'New Territories')

	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20, Result_Value21) VALUES
	(10, NULL, 'CW', 'EAST', 'SOUTH', 'WC', 'KC', 'KT', 'SSP', 'WTS', 'YTM', 'ISL', 'KTS', 'NORTH', 'SK', 'ST', 'TM', 'TP', 'TW', 'YL', NULL, 'Total')


-- ---------------------------------------------
-- Data
-- ---------------------------------------------

	SET @i = 1
	
	WHILE @i <= 12 BEGIN
		SET @CW = 0
		SET @EAST = 0
		SET @SOUTH = 0
		SET @WC = 0
		SET @KC = 0
		SET @KT = 0
		SET @SSP = 0
		SET @WTS = 0
		SET @YTM = 0
		SET @ISL = 0
		SET @KTS = 0
		SET @NORTH = 0
		SET @SK = 0
		SET @ST = 0
		SET @TM = 0
		SET @TP = 0
		SET @TW = 0
		SET @YL = 0
		SET @Total = 0
		
		SELECT @CW = Voucher_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'CENTRAL & WEST.'
		SELECT @EAST = Voucher_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'EASTERN'
		SELECT @SOUTH = Voucher_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'SOUTHERN'
		SELECT @WC = Voucher_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'WAN CHAI'
		
		SELECT @KC = Voucher_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'KOWLOON CITY'
		SELECT @KT = Voucher_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'KWUN TONG'
		SELECT @SSP = Voucher_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'SHAM SHUI PO'
		SELECT @WTS = Voucher_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'WONG TAI SIN'
		SELECT @YTM = Voucher_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'YAU TSIM MONG'
		
		SELECT @ISL = Voucher_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'ISLANDS'
		SELECT @KTS = Voucher_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'KWAI TSING'
		SELECT @NORTH = Voucher_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'NORTH'
		SELECT @SK = Voucher_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'SAI KUNG'
		SELECT @ST = Voucher_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'SHA TIN'
		SELECT @TM = Voucher_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'TUEN MUN'
		SELECT @TP = Voucher_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'TAI PO'
		SELECT @TW = Voucher_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'TSUEN WAN'
		SELECT @YL = Voucher_Count FROM @TransactionGroup WHERE Transaction_Month = @i AND District_Board = 'YUEN LONG'
		
		SELECT @Total = ISNULL(SUM(Voucher_Count), 0) FROM @TransactionGroup WHERE Transaction_Month = @i
	
		INSERT INTO @ResultTable2 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20, Result_Value21)
		SELECT
			10 + @i,
			CONVERT(varchar(10), DATEADD(mm, @i - 1, CONVERT(varchar, @Year) + '-01-01'), 120),
			@CW,
			@EAST,
			@SOUTH,
			@WC,
			@KC,
			@KT,
			@SSP,
			@WTS,
			@YTM,
			@ISL,
			@KTS,
			@NORTH,
			@SK,
			@ST,
			@TM,
			@TP,
			@TW,
			@YL,
			NULL,
			@Total

		SET @i = @i + 1
	
	END


-- ---------------------------------------------
-- Footer
-- ---------------------------------------------

	INSERT INTO @ResultTable2 (Result_Seq) VALUES
	(30)

	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20, Result_Value21)
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
		SUM(CONVERT(int, Result_Value12)),
		SUM(CONVERT(int, Result_Value13)),
		SUM(CONVERT(int, Result_Value14)),
		SUM(CONVERT(int, Result_Value15)),
		SUM(CONVERT(int, Result_Value16)),
		SUM(CONVERT(int, Result_Value17)),
		SUM(CONVERT(int, Result_Value18)),
		SUM(CONVERT(int, Result_Value19)),
		NULL,
		SUM(CONVERT(int, Result_Value21))
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
		Result_Value13,
		Result_Value14,
		Result_Value15,
		Result_Value16,
		Result_Value17,
		Result_Value18,
		Result_Value19,
		Result_Value20,
		Result_Value21
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
		Result_Value13,
		Result_Value14,
		Result_Value15,
		Result_Value16,
		Result_Value17,
		Result_Value18,
		Result_Value19,
		Result_Value20,
		Result_Value21
	FROM
		@ResultTable2
	ORDER BY
		Result_Seq


END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSA0018-05-06] TO HCVU
GO
