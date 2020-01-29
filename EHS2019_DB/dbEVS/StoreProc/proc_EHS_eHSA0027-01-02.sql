IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSA0027-01-02]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSA0027-01-02]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Helen Lam
-- Modified date:	27 Jan 2012
-- Description:		eHSA0027 - FHB statistics for 2011 (CRD12-002)
-- =============================================
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

CREATE PROCEDURE [dbo].[proc_EHS_eHSA0027-01-02]
	@Year	int
AS BEGIN

	SET NOCOUNT ON;
	
-- =============================================
-- Declaration
-- =============================================
	DECLARE @strYear char(4)
	set @strYear=cast(@Year as char(4))
	DECLARE @Account table (
		Create_Month	smallint,
		Service_Type	char(5),
		Age				smallint,
		Create_Year		smallint
	)
	
	DECLARE @ResultTable1 table (
		Result_Seq		smallint,
		Result_Value1	varchar(250) DEFAULT '',
		Result_Value2	varchar(50) DEFAULT '',
		Result_Value3	varchar(50) DEFAULT '',
		Result_Value4	varchar(50) DEFAULT '',
		Result_Value5	varchar(50) DEFAULT '',
		Result_Value6	varchar(50) DEFAULT '',
		Result_Value7	varchar(50) DEFAULT '',
		Result_Value8	varchar(50) DEFAULT '',		
		Result_Value9	varchar(50) DEFAULT '',
		Result_Value10	varchar(50) DEFAULT '',
		Result_Value11	varchar(50) DEFAULT '',
		Result_Value12	varchar(50) DEFAULT '',
		Result_Value13	varchar(50) DEFAULT '',
		Result_Value14	varchar(50) DEFAULT '',
		Result_Value15	varchar(50) DEFAULT ''

	)
	
	DECLARE @ResultTable2 table (
		Result_Seq		smallint,
		Result_Value1	varchar(250) DEFAULT '',
		Result_Value2	varchar(50) DEFAULT ''
	)
	
	DECLARE @i		smallint
	DECLARE @ENU	int
	DECLARE @RCM	int
	DECLARE @RCP	int
	DECLARE @RDT	int
	DECLARE @RMP	int
	DECLARE @RMT	int
	DECLARE @RNU	int
	DECLARE @ROP	int
	DECLARE @ROT	int
	DECLARE @RPT	int
	DECLARE @RRD	int
	DECLARE @DH		int
	DECLARE @Total	int


-- =============================================
-- Retrieve data
-- =============================================

-- ---------------------------------------------
-- Validated account
-- ---------------------------------------------

	INSERT INTO @Account (
		Create_Month,
		Service_Type,
		Age,
		Create_Year
	)
	SELECT
		MONTH(VA.Effective_Dtm) AS [Create_Month],
		PR.Service_Category_Code AS [Service_Type],
		@Year - YEAR(VP.DOB) AS [Age],
		YEAR(VA.Effective_Dtm) AS [Create_Year]
	FROM
		VoucherAccount VA
			INNER JOIN PersonalInformation VP
				ON VA.Voucher_Acc_ID = VP.Voucher_Acc_ID
			INNER JOIN VoucherAccountCreationLog VAC
				ON VA.Voucher_Acc_ID = VAC.Voucher_Acc_ID
			LEFT JOIN Practice P
				ON VAC.SP_ID = P.SP_ID
					AND VAC.SP_Practice_Display_Seq = P.Display_Seq
			LEFT JOIN Professional PR
				ON P.SP_ID = PR.SP_ID
					AND P.Professional_Seq = PR.Professional_Seq
	WHERE
		VA.Record_Status = 'A'
			AND YEAR(VA.Effective_Dtm) <= @Year
			AND @Year - YEAR(VP.DOB) >= 70


-- =============================================
-- Build frame (01)
-- =============================================

	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1) VALUES
	(0, 'eHSA0027-01: Monthly statistics of validated HCVS-eligible eHealth accounts created in ' +@strYear+', broken down by professions')
	
	INSERT INTO @ResultTable1 (Result_Seq) VALUES
	(1)
	
	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1, Result_Value2) VALUES
	(2, NULL, 'No. of eHealth accounts')
	
--	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14,Result_Value15) VALUES
--	(10, NULL, 'ENU', 'RCM', 'RCP', 'RDT', 'RMP', 'RMT', 'RNU','ROP', 'ROT', 'RPT', 'RRD', NULL, 'DH', 'Total')

INSERT INTO @ResultTable1 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14) VALUES
	(10, NULL, 'ENU', 'RCM', 'RCP', 'RDT', 'RMP', 'RMT', 'RNU', 'ROT', 'RPT', 'RRD', NULL, 'DH', 'Total')

-- ---------------------------------------------
-- Data
-- ---------------------------------------------

-- Jan to Dec
	
	SET @i = 1
	
	WHILE @i <= 12 BEGIN
		SELECT @ENU = COUNT(1) FROM @Account WHERE Create_Month = @i AND Service_Type = 'ENU' AND Create_Year = @Year
		SELECT @RCM = COUNT(1) FROM @Account WHERE Create_Month = @i AND Service_Type = 'RCM' AND Create_Year = @Year 
		SELECT @RCP = COUNT(1) FROM @Account WHERE Create_Month = @i AND Service_Type = 'RCP' AND Create_Year = @Year
		SELECT @RDT = COUNT(1) FROM @Account WHERE Create_Month = @i AND Service_Type = 'RDT' AND Create_Year = @Year
		SELECT @RMP = COUNT(1) FROM @Account WHERE Create_Month = @i AND Service_Type = 'RMP' AND Create_Year = @Year
		SELECT @RMT = COUNT(1) FROM @Account WHERE Create_Month = @i AND Service_Type = 'RMT' AND Create_Year = @Year
		SELECT @RNU = COUNT(1) FROM @Account WHERE Create_Month = @i AND Service_Type = 'RNU' AND Create_Year = @Year
		--SELECT @ROP = COUNT(1) FROM @Account WHERE Create_Month = @i AND Service_Type = 'ROP' AND Create_Year = @Year
		SELECT @ROT = COUNT(1) FROM @Account WHERE Create_Month = @i AND Service_Type = 'ROT' AND Create_Year = @Year
		SELECT @RPT = COUNT(1) FROM @Account WHERE Create_Month = @i AND Service_Type = 'RPT' AND Create_Year = @Year
		SELECT @RRD = COUNT(1) FROM @Account WHERE Create_Month = @i AND Service_Type = 'RRD' AND Create_Year = @Year
		SELECT @DH = COUNT(1) FROM @Account WHERE Create_Month = @i AND Service_Type IS NULL AND Create_Year = @Year
		SELECT @Total = COUNT(1) FROM @Account WHERE Create_Month = @i AND Create_Year = @Year
	
		INSERT INTO @ResultTable1 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14)
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
			@DH,
			@Total
	
		SET @i = @i + 1
	
	END

	INSERT INTO @ResultTable1 (Result_Seq) VALUES
	(30)


-- Total

INSERT INTO @ResultTable1 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14)
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
		SUM(CONVERT(int, Result_Value13)),
		SUM(CONVERT(int, Result_Value14))
	FROM
		@ResultTable1
	WHERE
		Result_Seq BETWEEN 11 AND 22


-- Accounts created before 2010

	INSERT INTO @ResultTable1 (Result_Seq) VALUES
	(41)
	

-- Note

	INSERT INTO @ResultTable1 (Result_Seq) VALUES
	(50)
	
	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1) VALUES
	(52, 'Note:')

	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1) VALUES
	(60, 'i) For eHealth account with several temporary accounts, the account is counted under the profession of the practice of the earliest temp. account being converted to validated account')
	
	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1) VALUES
	(61, 'ii) The above statistics include active account only')
	
	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1)
	SELECT
		62,
		'iii) Count of cumulative terminated account: ' + CONVERT(varchar, COUNT(DISTINCT VA.Voucher_Acc_ID))
	FROM
		VoucherAccount VA
			INNER JOIN PersonalInformation VP
				ON VA.Voucher_Acc_ID = VP.Voucher_Acc_ID
	WHERE
		VA.Record_Status = 'D'
			AND @Year - YEAR(VP.DOB) >= 70
			AND YEAR(VA.Effective_Dtm) <= @Year
	
	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1)
	SELECT
		63,	
		'iv) Count of cumulative suspended account: ' + CONVERT(varchar, COUNT(DISTINCT VA.Voucher_Acc_ID))
	FROM
		VoucherAccount VA
			INNER JOIN PersonalInformation VP
				ON VA.Voucher_Acc_ID = VP.Voucher_Acc_ID
	WHERE
		VA.Record_Status = 'S'
			AND @Year - YEAR(VP.DOB) >= 70
			AND YEAR(VA.Effective_Dtm) <= @Year


-- =============================================
-- Build frame (02)
-- =============================================

	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1) VALUES
	(0, 'eHSA0027-02: Validated HCVS-eligible eHealth accounts as at 31 Dec ' +@strYear+', broken down by ages')
	
	INSERT INTO @ResultTable2 (Result_Seq) VALUES
	(1)
	
	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1, Result_Value2) VALUES
	(2, 'Age', 'No. of eHealth accounts')
	

-- ---------------------------------------------
-- Data
-- ---------------------------------------------
					
-- Age 70 to 99
					
	SET @i = 70
	
	WHILE @i <= 99 BEGIN
		INSERT INTO @ResultTable2 (Result_Seq, Result_Value1, Result_Value2)
		SELECT
			@i,
			@i,
			COUNT(1)
		FROM
			@Account
		WHERE
			Age = @i
	
		SET @i = @i + 1
	
	END


-- Age >= 100

	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1, Result_Value2)
	SELECT
		100,
		'>=100',
		COUNT(1)
	FROM
		@Account
	WHERE
		Age >= 100


-- Blank line

	INSERT INTO @ResultTable2 (Result_Seq) VALUES
	(200)


-- Total

	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1, Result_Value2)
	SELECT
		300,
		'Total',
		SUM(CONVERT(int, Result_Value2))
	FROM
		@ResultTable2
	WHERE
		Result_Seq BETWEEN 70 AND 100


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
		Result_Value14
	FROM
		@ResultTable1
	ORDER BY
		Result_Seq

--

	SELECT 
		Result_Value1,
		Result_Value2
	FROM
		@ResultTable2
	ORDER BY
		Result_Seq


set nocount off
END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSA0027-01-02] TO HCVU
GO

