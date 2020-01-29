IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSA0011]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSA0011]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	7 February 2011
-- Description:		Change wordings
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		26 January 2011
-- Description:		eHSA0011 - Family doctor concept report
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSA0011]
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Report setting
-- =============================================
	DECLARE	@CutOff_Dtm		datetime
	SET @Cutoff_Dtm = '2010-10-31 23:59:59.997'


-- =============================================
-- Declaration
-- =============================================
	DECLARE @TransactionByAccountSP table (
		Voucher_Acc_ID		char(15),
		Transaction_Count	int,
		SP_Num				int,
		Voucher_Count		int
	)

	DECLARE @Result	table (
		Display_Seq			int,
		Transaction_Num		varchar(10),
		SP_1				int,
		SP_2				int,
		SP_3				int,
		SP_4				int,
		SP_5				int,
		SP_Total			int,
		Voucher_1			int,
		Voucher_2			int,
		Voucher_3			int,
		Voucher_4			int,
		Voucher_5			int,
		Voucher_Total		int,
		Transaction_Total	int
	)
	
	DECLARE @i	int
	
	DECLARE @Sheet1	table (
		Display_Seq			int,
		Col_1				varchar(200),
		Col_2				varchar(500)
	)
	
	DECLARE @Sheet2	table (
		Display_Seq			int,
		Col_1				varchar(200),
		Col_2				varchar(100),
		Col_3				varchar(100),
		Col_4				varchar(100),
		Col_5				varchar(100),
		Col_6				varchar(100),
		Col_7				varchar(100),
		Col_8				varchar(100)
	)
	
	DECLARE @Sheet3	table (
		Display_Seq			int,
		Col_1				varchar(200),
		Col_2				varchar(100),
		Col_3				varchar(100),
		Col_4				varchar(100),
		Col_5				varchar(100),
		Col_6				varchar(100),
		Col_7				varchar(100),
		Col_8				varchar(100)
	)


-- =============================================
-- Retrieve data
-- =============================================

-- ---------------------------------------------
-- Retrieve VoucherTransaction
-- ---------------------------------------------
	INSERT INTO @TransactionByAccountSP (
		Voucher_Acc_ID,
		Transaction_Count,
		SP_Num,
		Voucher_Count
	)
	SELECT
		Voucher_Acc_ID,
		COUNT(1),
		COUNT(DISTINCT SP_ID),
		SUM(Voucher_Before_Claim - Voucher_After_Claim)
	FROM
		VoucherTransaction
	WHERE
		Scheme_Code = 'HCVS'
			AND Transaction_Dtm <= @Cutoff_Dtm
			AND Service_Type = 'RMP'
			AND Record_Status NOT IN ('I', 'D')
			AND ISNULL(Invalidation, '') <> 'I'
			AND Voucher_Acc_ID <> ''
	GROUP BY
		Voucher_Acc_ID


-- ---------------------------------------------
-- Calculate summary
-- ---------------------------------------------
	SET @i = 1

	WHILE @i <= 10 BEGIN
		INSERT INTO @Result (
			Display_Seq,
			Transaction_Num
		)
		SELECT
			@i,
			CONVERT(varchar, @i)
			
		UPDATE
			@Result
		SET
			SP_1 = (
				SELECT
					COUNT(1)
				FROM
					@TransactionByAccountSP
				WHERE
					Transaction_Count = CONVERT(varchar, @i)
						AND SP_Num = 1
			),
			SP_2 = (
				SELECT
					COUNT(1)
				FROM
					@TransactionByAccountSP
				WHERE
					Transaction_Count = CONVERT(varchar, @i)
						AND SP_Num = 2
			),
			SP_3 = (
				SELECT
					COUNT(1)
				FROM
					@TransactionByAccountSP
				WHERE
					Transaction_Count = CONVERT(varchar, @i)
						AND SP_Num = 3
			),
			SP_4 = (
				SELECT
					COUNT(1)
				FROM
					@TransactionByAccountSP
				WHERE
					Transaction_Count = CONVERT(varchar, @i)
						AND SP_Num = 4
			),
			SP_5 = (
				SELECT
					COUNT(1)
				FROM
					@TransactionByAccountSP
				WHERE
					Transaction_Count = CONVERT(varchar, @i)
						AND SP_Num = 5
			),
			Voucher_1 = (
				SELECT
					ISNULL(SUM(Voucher_Count), 0)
				FROM
					@TransactionByAccountSP
				WHERE
					Transaction_Count = CONVERT(varchar, @i)
						AND SP_Num = 1
			),
			Voucher_2 = (
				SELECT
					ISNULL(SUM(Voucher_Count), 0)
				FROM
					@TransactionByAccountSP
				WHERE
					Transaction_Count = CONVERT(varchar, @i)
						AND SP_Num = 2
			),
			Voucher_3 = (
				SELECT
					ISNULL(SUM(Voucher_Count), 0)
				FROM
					@TransactionByAccountSP
				WHERE
					Transaction_Count = CONVERT(varchar, @i)
						AND SP_Num = 3
			),
			Voucher_4 = (
				SELECT
					ISNULL(SUM(Voucher_Count), 0)
				FROM
					@TransactionByAccountSP
				WHERE
					Transaction_Count = CONVERT(varchar, @i)
						AND SP_Num = 4
			),
			Voucher_5 = (
				SELECT
					ISNULL(SUM(Voucher_Count), 0)
				FROM
					@TransactionByAccountSP
				WHERE
					Transaction_Count = CONVERT(varchar, @i)
						AND SP_Num = 5
			)
		WHERE
			Transaction_Num = CONVERT(varchar, @i)
		
		SET @i = @i + 1
	END


-- ---------------------------------------------
-- Retrieve total (use another method for counter-checking)
-- ---------------------------------------------
	INSERT INTO @Result (Display_Seq, Transaction_Num) VALUES (100, 'Total')
	
	UPDATE
		@Result
	SET
		SP_1 = (SELECT COUNT(1) FROM @TransactionByAccountSP WHERE SP_Num = 1),
		SP_2 = (SELECT COUNT(1) FROM @TransactionByAccountSP WHERE SP_Num = 2),
		SP_3 = (SELECT COUNT(1) FROM @TransactionByAccountSP WHERE SP_Num = 3),
		SP_4 = (SELECT COUNT(1) FROM @TransactionByAccountSP WHERE SP_Num = 4),
		SP_5 = (SELECT COUNT(1) FROM @TransactionByAccountSP WHERE SP_Num = 5),
		SP_Total = (SELECT COUNT(1) FROM @TransactionByAccountSP),
		Voucher_1 = ISNULL((SELECT SUM(Voucher_Count) FROM @TransactionByAccountSP WHERE SP_Num = 1), 0),
		Voucher_2 = ISNULL((SELECT SUM(Voucher_Count) FROM @TransactionByAccountSP WHERE SP_Num = 2), 0),
		Voucher_3 = ISNULL((SELECT SUM(Voucher_Count) FROM @TransactionByAccountSP WHERE SP_Num = 3), 0),
		Voucher_4 = ISNULL((SELECT SUM(Voucher_Count) FROM @TransactionByAccountSP WHERE SP_Num = 4), 0),
		Voucher_5 = ISNULL((SELECT SUM(Voucher_Count) FROM @TransactionByAccountSP WHERE SP_Num = 5), 0),
		Voucher_Total = ISNULL((SELECT SUM(Voucher_Count) FROM @TransactionByAccountSP), 0)
	WHERE
		Transaction_Num = 'Total'


-- ---------------------------------------------
-- Calculate total
-- ---------------------------------------------
	UPDATE
		@Result
	SET
		SP_Total = SP_1 + SP_2 + SP_3 + SP_4 + SP_5,
		Voucher_Total = Voucher_1 + Voucher_2 + Voucher_3 + Voucher_4 + Voucher_5

	UPDATE
		@Result
	SET
		Transaction_Total = SP_Total * Transaction_Num		
	WHERE
		Transaction_Num <> 'Total'
		
	UPDATE
		@Result
	SET
		Transaction_Total = (SELECT SUM(Transaction_Total) FROM @Result WHERE Transaction_Num <> 'Total')
	WHERE
		Transaction_Num = 'Total'
		

-- =============================================
-- Build frame
-- =============================================

-- ---------------------------------------------
-- Sheet 1: Content
-- ---------------------------------------------
	INSERT INTO @Sheet1 (Display_Seq, Col_1, Col_2) VALUES (1, 'Sub Report ID', 'Sub Report Name')
	INSERT INTO @Sheet1 (Display_Seq, Col_1, Col_2) VALUES (2, 'eHSA0011-01', 'Report on accounts with voucher claims from different no. of medical practitioners')
	INSERT INTO @Sheet1 (Display_Seq, Col_1, Col_2) VALUES (3, 'eHSA0011-02', 'Report on number of vouchers claimed by number of transactions per eHealth account and number of medical practitioners involved')
	INSERT INTO @Sheet1 (Display_Seq, Col_1, Col_2) VALUES (10, '', '')
	INSERT INTO @Sheet1 (Display_Seq, Col_1, Col_2) VALUES (11, '', '')
	INSERT INTO @Sheet1 (Display_Seq, Col_1, Col_2) VALUES (20, 'Report Generation Time: ' + CONVERT(varchar, GETDATE(), 111) + ' ' + CONVERT(varchar(5), GETDATE(), 114), '')


-- ---------------------------------------------
-- Sheet 2: eHSA0011-01: Report on accounts with voucher claims from different no. of medical practitioners
-- ---------------------------------------------
	INSERT INTO @Sheet2 (Display_Seq, Col_1, Col_2, Col_3, Col_4, Col_5, Col_6, Col_7, Col_8) VALUES
	(1, 'eHSA0011-01: Report on accounts with voucher claims from different no. of medical practitioners', '', '', '', '', '', '', '')
	
	INSERT INTO @Sheet2 (Display_Seq, Col_1, Col_2, Col_3, Col_4, Col_5, Col_6, Col_7, Col_8) VALUES
	(2, '', '', '', '', '', '', '', '')
	
	INSERT INTO @Sheet2 (Display_Seq, Col_1, Col_2, Col_3, Col_4, Col_5, Col_6, Col_7, Col_8) VALUES
	(10, 'Reporting period: as at ' + CONVERT(varchar(2), @Cutoff_Dtm, 105) + ' ' + CONVERT(varchar(3), DATENAME(mm, @Cutoff_Dtm)) + ' ' + CONVERT(varchar(4), @Cutoff_Dtm, 111), '', '', '', '', '', '', '')
	
	INSERT INTO @Sheet2 (Display_Seq, Col_1, Col_2, Col_3, Col_4, Col_5, Col_6, Col_7, Col_8) VALUES
	(11, '', '', '', '', '', '', '', '')
	
	INSERT INTO @Sheet2 (Display_Seq, Col_1, Col_2, Col_3, Col_4, Col_5, Col_6, Col_7, Col_8) VALUES
	(20, '', 'No. of medical practitioners involved', '', '', '', '', '', '')
	
	INSERT INTO @Sheet2 (Display_Seq, Col_1, Col_2, Col_3, Col_4, Col_5, Col_6, Col_7, Col_8) VALUES
	(21, 'No. of transactions per eHealth A/C', '1', '2', '3', '4', '5', 'Total', 'Total No. of transactions')
	
	INSERT INTO @Sheet2 (Display_Seq, Col_1, Col_2, Col_3, Col_4, Col_5, Col_6, Col_7, Col_8) VALUES
	(22, '', 'No. of eHealth A/C involved', '', '', '', '', '', '')
	
	INSERT INTO @Sheet2 (Display_Seq, Col_1, Col_2, Col_3, Col_4, Col_5, Col_6, Col_7, Col_8)
	SELECT
		Display_Seq + 30,
		Transaction_Num,
		SP_1,
		SP_2,
		SP_3,
		SP_4,
		SP_5,
		SP_Total,
		Transaction_Total
	FROM
		@Result
	
	UPDATE @Sheet2 SET Col_3 = '-', Col_4 = '-', Col_5 = '-', Col_6 = '-' WHERE Display_Seq = 31
	UPDATE @Sheet2 SET Col_4 = '-', Col_5 = '-', Col_6 = '-' WHERE Display_Seq = 32
	UPDATE @Sheet2 SET Col_5 = '-', Col_6 = '-' WHERE Display_Seq = 33
	UPDATE @Sheet2 SET Col_6 = '-' WHERE Display_Seq = 34
	

-- ---------------------------------------------
-- Sheet 3: eHSA0011-02: Report on number of vouchers claimed by number of transactions per eHealth account and number of medical practitioners involved
-- ---------------------------------------------

	INSERT INTO @Sheet3 (Display_Seq, Col_1, Col_2, Col_3, Col_4, Col_5, Col_6, Col_7, Col_8) VALUES
	(1, 'eHSA0011-02: Report on number of vouchers claimed by number of transactions per eHealth account and number of medical practitioners involved', '', '', '', '', '', '', '')
	
	INSERT INTO @Sheet3 (Display_Seq, Col_1, Col_2, Col_3, Col_4, Col_5, Col_6, Col_7, Col_8) VALUES
	(2, '', '', '', '', '', '', '', '')
	
	INSERT INTO @Sheet3 (Display_Seq, Col_1, Col_2, Col_3, Col_4, Col_5, Col_6, Col_7, Col_8) VALUES
	(10, 'Reporting period: as at ' + CONVERT(varchar(2), @Cutoff_Dtm, 105) + ' ' + CONVERT(varchar(3), DATENAME(mm, @Cutoff_Dtm)) + ' ' + CONVERT(varchar(4), @Cutoff_Dtm, 111), '', '', '', '', '', '', '')
	
	INSERT INTO @Sheet3 (Display_Seq, Col_1, Col_2, Col_3, Col_4, Col_5, Col_6, Col_7, Col_8) VALUES
	(11, '', '', '', '', '', '', '', '')
	
	INSERT INTO @Sheet3 (Display_Seq, Col_1, Col_2, Col_3, Col_4, Col_5, Col_6, Col_7, Col_8) VALUES
	(20, '', 'No. of medical practitioners involved', '', '', '', '', '', '')
	
	INSERT INTO @Sheet3 (Display_Seq, Col_1, Col_2, Col_3, Col_4, Col_5, Col_6, Col_7, Col_8) VALUES
	(21, 'No. of transactions per eHealth A/C', '1', '2', '3', '4', '5', 'Total', 'Total No. of transactions')
	
	INSERT INTO @Sheet3 (Display_Seq, Col_1, Col_2, Col_3, Col_4, Col_5, Col_6, Col_7, Col_8) VALUES
	(22, '', 'No. of vouchers involved', '', '', '', '', '', '')
	
	INSERT INTO @Sheet3 (Display_Seq, Col_1, Col_2, Col_3, Col_4, Col_5, Col_6, Col_7, Col_8)
	SELECT
		Display_Seq + 30,
		Transaction_Num,
		Voucher_1,
		Voucher_2,
		Voucher_3,
		Voucher_4,
		Voucher_5,
		Voucher_Total,
		Transaction_Total
	FROM
		@Result

	UPDATE @Sheet3 SET Col_3 = '-', Col_4 = '-', Col_5 = '-', Col_6 = '-' WHERE Display_Seq = 31
	UPDATE @Sheet3 SET Col_4 = '-', Col_5 = '-', Col_6 = '-' WHERE Display_Seq = 32
	UPDATE @Sheet3 SET Col_5 = '-', Col_6 = '-' WHERE Display_Seq = 33
	UPDATE @Sheet3 SET Col_6 = '-' WHERE Display_Seq = 34


-- =============================================
-- Return result
-- =============================================
	SELECT Col_1, Col_2 FROM @Sheet1 ORDER BY Display_Seq
	SELECT Col_1, Col_2, Col_3, Col_4, Col_5, Col_6, Col_7, Col_8 FROM @Sheet2 ORDER BY Display_Seq
	SELECT Col_1, Col_2, Col_3, Col_4, Col_5, Col_6, Col_7, Col_8 FROM @Sheet3 ORDER BY Display_Seq


-- =============================================
-- Checking
-- =============================================
/*
	SELECT
		COUNT(1) AS [SP>6]
	FROM
		@TransactionByAccountSP
	WHERE
		SP_Num >= 6
*/
		

END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSA0011] TO HCVU
GO

