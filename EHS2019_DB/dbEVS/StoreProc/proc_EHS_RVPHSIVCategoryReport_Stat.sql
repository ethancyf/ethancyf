IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_RVPHSIVCategoryReport_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_RVPHSIVCategoryReport_Stat]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	11 April 2011
-- Description:		Stored procedure is no longer used
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	29 January 2010
-- Description:		Add sections "By age group", "By dose", "By category"
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		8 January 2010
-- Description:		Generate report for the Vaccination Claim Report (Scheme RVP - HSIV)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

/*
CREATE PROCEDURE [dbo].[proc_EHS_RVPHSIVCategoryReport_Stat] 
	@Cutoff_Dtm	datetime
AS BEGIN
-- =============================================
-- Constant
-- =============================================
	DECLARE @Category_Hcw		char(10)
	DECLARE @Category_Resident	char(10)
	
	SET @Category_Hcw = 'HCW'
	SET @Category_Resident = 'RESIDENT'
	
	
-- =============================================
-- Temporary table
-- =============================================
	DECLARE @Transaction table (
		Transaction_ID				char(20),
		Voucher_Acc_ID				char(15),
		Temp_Voucher_Acc_ID			char(15),
		Special_Acc_ID				char(15),
		Invalid_Acc_ID				char(15),
		Doc_Code					char(20),
		Dose						char(20),
		Category_Code				varchar(50),
		Service_Receive_Dtm			datetime,
		SP_ID						char(8)
	)
	
	DECLARE @Account table (
		Service_Receive_Dtm			datetime,
		Doc_Code					char(20),
		Encrypt_Field1				varbinary(100),
		DOB							datetime,
		DOB_Adjust					datetime,
		Exact_DOB					char(1)
	)

	DECLARE @ResultTable table (
		Result_Seq					smallint,
		Result_Value1				varchar(100),
		Result_Value2				varchar(100),
		Result_Value3				varchar(100),
		Result_Value4				varchar(100)
	)
	

-- =============================================
-- Retrieve data
-- =============================================

-- ---------------------------------------------
-- RVP-HSIV transactions
-- ---------------------------------------------

	INSERT INTO @Transaction (
		Transaction_ID,
		Voucher_Acc_ID,
		Temp_Voucher_Acc_ID,
		Special_Acc_ID,
		Invalid_Acc_ID,
		Doc_Code,
		Dose,
		Category_Code,
		Service_Receive_Dtm,
		SP_ID
	)
	SELECT
		VT.Transaction_ID,
		ISNULL(VT.Voucher_Acc_ID, ''),
		ISNULL(VT.Temp_Voucher_Acc_ID, ''),
		ISNULL(VT.Special_Acc_ID, ''),
		ISNULL(VT.Invalid_Acc_ID, ''),
		VT.Doc_Code,
		TD.Available_Item_Code AS [Dose],
		TAF.AdditionalFieldValueCode AS [Category_Code],
		VT.Service_Receive_Dtm,
		VT.SP_ID
	FROM
		VoucherTransaction VT
			INNER JOIN TransactionDetail TD
				ON VT.Transaction_ID = TD.Transaction_ID
					AND TD.Subsidize_Item_Code = 'HSIV'
			INNER JOIN TransactionAdditionalField TAF
				ON VT.Transaction_ID = TAF.Transaction_ID
					AND TAF.AdditionalFieldID = 'CategoryCode'
	WHERE
		VT.Scheme_Code = 'RVP'
			AND VT.Transaction_Dtm <= @Cutoff_Dtm
			AND VT.Record_Status <> 'I'


-- ---------------------------------------------
-- Validated accounts
-- ---------------------------------------------

	INSERT INTO @Account (
		Service_Receive_Dtm,
		Doc_Code,
		Encrypt_Field1,
		DOB,
		DOB_Adjust,
		Exact_DOB
	)
	SELECT
		VT.Service_Receive_Dtm,
		VP.Doc_Code,
		VP.Encrypt_Field1,
		VP.DOB,
		NULL AS [DOB_Adjust],
		VP.Exact_DOB
	FROM
		@Transaction VT
			INNER JOIN PersonalInformation VP
				ON VT.Voucher_Acc_ID = VP.Voucher_Acc_ID
					AND VT.Doc_Code = VP.Doc_Code
	WHERE
		VT.Voucher_Acc_ID <> ''


-- ---------------------------------------------
-- Temporary accounts
-- ---------------------------------------------

	INSERT INTO @Account (
		Service_Receive_Dtm,
		Doc_Code,
		Encrypt_Field1,
		DOB,
		DOB_Adjust,
		Exact_DOB
	)
	SELECT
		VT.Service_Receive_Dtm,
		TP.Doc_Code,
		TP.Encrypt_Field1,
		TP.DOB,
		NULL AS [DOB_Adjust],
		TP.Exact_DOB
	FROM
		@Transaction VT
			INNER JOIN TempPersonalInformation TP
				ON VT.Temp_Voucher_Acc_ID = TP.Voucher_Acc_ID
	WHERE
		VT.Voucher_Acc_ID = ''
			AND VT.Temp_Voucher_Acc_ID <> ''
			AND VT.Special_Acc_ID = ''


-- ---------------------------------------------
-- Special accounts
-- ---------------------------------------------

	INSERT INTO @Account (
		Service_Receive_Dtm,
		Doc_Code,
		Encrypt_Field1,
		DOB,
		DOB_Adjust,
		Exact_DOB
	)
	SELECT
		VT.Service_Receive_Dtm,
		SP.Doc_Code,
		SP.Encrypt_Field1,
		SP.DOB,
		NULL AS [DOB_Adjust],
		SP.Exact_DOB
	FROM
		@Transaction VT
			INNER JOIN SpecialPersonalInformation SP
				ON VT.Special_Acc_ID = SP.Special_Acc_ID
	WHERE
		VT.Voucher_Acc_ID = ''
			AND VT.Special_Acc_ID <> ''
			AND VT.Invalid_Acc_ID = ''


-- ---------------------------------------------
-- Patch DOB
-- ---------------------------------------------

	UPDATE
		@Account
	SET
		DOB = CONVERT(varchar, YEAR(DOB)) + '-' + CONVERT(varchar, MONTH(DOB)) + '-' + CONVERT(varchar, DAY(DATEADD(d, -DAY(DATEADD(m, 1, DOB)), DATEADD(m, 1, DOB))))
	WHERE
		Exact_DOB IN ('M', 'U')

	UPDATE
		@Account
	SET
		DOB_Adjust = DOB

	UPDATE
		@Account
	SET
		DOB_Adjust = DATEADD(yyyy, 1, DOB)
	WHERE
		MONTH(DOB) > MONTH(Service_Receive_Dtm)
			OR ( MONTH(DOB) = MONTH(Service_Receive_Dtm) AND DAY(DOB) > DAY(Service_Receive_Dtm) )


-- ---------------------------------------------
-- Patch Doc_Code
-- ---------------------------------------------

	UPDATE
		@Account
	SET
		Doc_Code = 'HKIC'
	WHERE
		Doc_Code = 'HKBC'


-- =============================================
-- Process data
-- =============================================

-- ---------------------------------------------
-- Build frame
-- ---------------------------------------------
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4) VALUES
	(100, '(ii) By age group (HSIV)', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4) VALUES
	(101, '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4) VALUES
	(110, '6 months to <9 years', '9 years to <65 age year', '>=65 age year', 'No. of SP involved')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4) VALUES
	(111, '', '', '', '')
	
--	
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4) VALUES
	(120, '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4) VALUES
	(121, '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4) VALUES
	(122, '', '', '', '')

--

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4) VALUES
	(130, '(iii) By dose (HSIV)', '', '', '')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4) VALUES
	(131, '', '', '', '')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4) VALUES
	(140, '1st Dose', '2nd Dose', 'Only Dose', 'Total')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4) VALUES
	(141, '', '', '', '')

--

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4) VALUES
	(150, '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4) VALUES
	(151, '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4) VALUES
	(152, '', '', '', '')

--

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4) VALUES
	(160, '(iv) By category (HSIV)', '', '', '')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4) VALUES
	(161, '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4) VALUES
	(170, @Category_Hcw, @Category_Resident, 'Other Categories', 'Total')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4) VALUES
	(171, '', '', '', '')

--

	UPDATE	@ResultTable
	SET		Result_Value1 = (SELECT Category_Name FROM ClaimCategory WHERE Category_Code = @Category_Hcw)
	WHERE	Result_Seq = 170
	
	UPDATE	@ResultTable
	SET		Result_Value2 = (SELECT Category_Name FROM ClaimCategory WHERE Category_Code = @Category_Resident)
	WHERE	Result_Seq = 170


-- ---------------------------------------------
-- Build data
-- ---------------------------------------------

-- (ii) By age group (HSIV)

	UPDATE
		@ResultTable
	SET
		Result_Value1 = (
			SELECT COUNT(1) FROM (
				SELECT DISTINCT Doc_Code, Encrypt_Field1 
				FROM @Account
				WHERE DATEDIFF(dd, DOB, Service_Receive_Dtm) >= 182 AND DATEDIFF(yy, DOB_Adjust, Service_Receive_Dtm) < 9
			) T
		)
	WHERE
		Result_Seq = 111

	UPDATE
		@ResultTable
	SET
		Result_Value2 = (
			SELECT COUNT(1) FROM (
				SELECT DISTINCT Doc_Code, Encrypt_Field1 
				FROM @Account
				WHERE DATEDIFF(yy, DOB_Adjust, Service_Receive_Dtm) >= 9 AND DATEDIFF(yy, DOB, Service_Receive_Dtm) < 65
			) T
		)
	WHERE
		Result_Seq = 111

	UPDATE
		@ResultTable
	SET
		Result_Value3 = (
			SELECT COUNT(1) FROM (
				SELECT DISTINCT Doc_Code, Encrypt_Field1 
				FROM @Account
				WHERE DATEDIFF(yy, DOB, Service_Receive_Dtm) >= 65
			) T
		)
	WHERE
		Result_Seq = 111

	UPDATE
		@ResultTable
	SET
		Result_Value4 = (
			SELECT COUNT(DISTINCT SP_ID)
			FROM @Transaction
		)
	WHERE
		Result_Seq = 111


-- (iii) By dose (HSIV)

	UPDATE
		@ResultTable
	SET
		Result_Value1 = (
			SELECT COUNT(1)
			FROM @Transaction
			WHERE Dose = '1STDOSE'
		)
	WHERE
		Result_Seq = 141

	UPDATE
		@ResultTable
	SET
		Result_Value2 = (
			SELECT COUNT(1)
			FROM @Transaction
			WHERE Dose = '2NDDOSE'
		)
	WHERE
		Result_Seq = 141

	UPDATE
		@ResultTable
	SET
		Result_Value3 = (
			SELECT COUNT(1)
			FROM @Transaction
			WHERE Dose = 'ONLYDOSE'
		)
	WHERE
		Result_Seq = 141

	UPDATE
		@ResultTable
	SET
		Result_Value4 = (
			CONVERT(int, Result_Value1)
			+
			CONVERT(int, Result_Value2)
			+
			CONVERT(int, Result_Value3)
		)
	WHERE
		Result_Seq = 141


-- (iv) By category (HSIV)
	
	UPDATE
		@ResultTable
	SET
		Result_Value1 =
			(
			SELECT
				COUNT(1)
			FROM
				@Transaction
			WHERE
				Category_Code = @Category_Hcw
			)
	WHERE
		Result_Seq = 171
		
	UPDATE
		@ResultTable
	SET
		Result_Value2 =
			(
			SELECT
				COUNT(1)
			FROM
				@Transaction
			WHERE 
				Category_Code = @Category_Resident
			)
	WHERE
		Result_Seq = 171
		
	UPDATE
		@ResultTable
	SET
		Result_Value3 =
			(
			SELECT
				COUNT(1)
			FROM
				@Transaction
			WHERE 
				Category_Code <> @Category_Hcw
					AND Category_Code <> @Category_Resident
			)
	WHERE
		Result_Seq = 171
		
			
	UPDATE
		@ResultTable
	SET
		Result_Value4 =
			(
			CONVERT(int, Result_Value1)
			+
			CONVERT(int, Result_Value2)
			+
			CONVERT(int, Result_Value3)
			)
	WHERE
		Result_Seq = 171


-- ---------------------------------------------
-- Insert to statistics table
-- ---------------------------------------------

	INSERT INTO _EHS_RVPAgeReport_Stat (
		Display_Seq,
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
		Result_Value16
	)
	SELECT
		Result_Seq,
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		'',
		'',
		'',
		'',
		'',
		'',
		'',
		'',
		'',
		'',
		'',
		''
	FROM
		@ResultTable
	
		
END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_RVPHSIVCategoryReport_Stat] TO HCVU
GO
*/