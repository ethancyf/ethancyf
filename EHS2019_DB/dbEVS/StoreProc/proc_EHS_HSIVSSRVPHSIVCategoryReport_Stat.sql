IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_HSIVSSRVPHSIVCategoryReport_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_HSIVSSRVPHSIVCategoryReport_Stat]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	30 August 2016
-- Description:		Stored procedure is no longer used
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	3 February 2010
-- Description:		Add tab "Content" and "02 - Transaction raw"
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		18 January 2010
-- Description:		Generate report for HSIV Cateogry Report (scheme HSIVSS and RVP)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
/*
CREATE PROCEDURE [dbo].[proc_EHS_HSIVSSRVPHSIVCategoryReport_Stat] 
AS BEGIN

-- =============================================
-- Report setting
-- =============================================
	DECLARE @Cutoff_Dtm	datetime
	SET @Cutoff_Dtm = CONVERT(varchar(11), GETDATE(), 106) + ' 13:00'
	
	DECLARE @Start_Dtm datetime
	SET @Start_Dtm = convert(varchar(11), @Cutoff_Dtm, 106)
	
	
-- =============================================
-- Constant
-- =============================================
	DECLARE @Category_Child		char(10)
	DECLARE @Category_Elder		char(10)
	DECLARE @Category_Hcw		char(10)
	DECLARE @Category_Precond	char(10)
	DECLARE @Category_Resident	char(10)
	
	SET @Category_Child	= 'CHILD'
	SET @Category_Elder	= 'ELDER'
	SET @Category_Hcw = 'HCW'
	SET @Category_Precond = 'PRECOND'
	SET @Category_Resident = 'RESIDENT'
	
-- =============================================
-- Declaration
-- =============================================
	DECLARE @Transaction table (
		Transaction_ID				char(20),
		Voucher_Acc_ID				char(15),
		Temp_Voucher_Acc_ID			char(15),
		Special_Acc_ID				char(15),
		Invalid_Acc_ID				char(15),
		Doc_Code					char(20),
		Scheme_Code					char(10),
		AdditionalFieldValueCode	varchar(50),
		Precondition				varchar(50),
		Service_Receive_Dtm			datetime
	)

	DECLARE @Account table (
		Scheme_Code					char(10),
		AdditionalFieldValueCode	varchar(50),
		Precondition				varchar(50),
		Age							smallint
	)
	
	DECLARE @TransactionRaw table (
		Scheme_Code					char(10),
		Transaction_Dtm				datetime,
		Transaction_ID				char(20),
		SP_ID						char(8),
		Service_Receive_Dtm			datetime,
		Category_Code				varchar(50),
		Precondition				varchar(250),
		Dose						char(20),
		Voucher_Acc_ID				char(15),
		Temp_Voucher_Acc_ID			char(15),
		Special_Acc_ID				char(15),
		Invalid_Acc_ID				char(15),
		Doc_Code					char(20),
		Row							int
	)

	DECLARE @AccountRaw table (
		Scheme_Code					char(10),
		Transaction_Dtm				datetime,
		Transaction_ID				char(20),
		SP_ID						char(8),
		Service_Receive_Dtm			datetime,
		Category_Code				varchar(50),
		Precondition				varchar(250),
		Dose						char(20),
		DOB							datetime,
		Exact_DOB					char(1),
		Row							int
	)
	
	DECLARE @ResultTable table (
		Result_Seq					smallint,
		Result_Value1				varchar(100),
		Result_Value2				varchar(100),
		Result_Value3				varchar(100),
		Result_Value4				varchar(100),
		Result_Value5				varchar(100),
		Result_Value6				varchar(100),
		Result_Value7				varchar(100),
		Result_Value8				varchar(100),
		Result_Value9				varchar(100)
	)
	
	DECLARE @ResultTableRaw table (
		Result_Seq					smallint,
		Result_Value1				varchar(100),
		Result_Value2				varchar(100),
		Result_Value3				varchar(100),
		Result_Value4				varchar(100),
		Result_Value5				varchar(100),
		Result_Value6				varchar(100),
		Result_Value7				varchar(250),
		Result_Value8				varchar(100),
		Result_Value9				varchar(100),
		Result_Value10				varchar(100)
	)
	
	DECLARE @EffectiveScheme table (
		Scheme_Code		char(10),
		Scheme_Seq		smallint
	)
	
-- =============================================
-- Initialization 
-- =============================================
	INSERT INTO @EffectiveScheme (
		Scheme_Code,
		Scheme_Seq
	)
	SELECT
		Scheme_Code,
		MAX(Scheme_Seq)
	FROM
		SchemeClaim
	WHERE
		GETDATE() >= Effective_Dtm
			AND Record_Status = 'A'
	GROUP BY
		Scheme_Code


-- =============================================
-- Retrieve data
-- =============================================

-- ---------------------------------------------
-- HSIVSS and RVP transactions
-- ---------------------------------------------

	INSERT INTO @Transaction (
		Transaction_ID,
		Voucher_Acc_ID,
		Temp_Voucher_Acc_ID,
		Special_Acc_ID,
		Invalid_Acc_ID,
		Doc_Code,
		Scheme_Code,
		AdditionalFieldValueCode,
		Precondition,
		Service_Receive_Dtm
	)
	SELECT
		VT.Transaction_ID,
		ISNULL(VT.Voucher_Acc_ID, ''),
		ISNULL(VT.Temp_Voucher_Acc_ID, ''),
		ISNULL(VT.Special_Acc_ID, ''),
		ISNULL(VT.Invalid_Acc_ID, ''),
		VT.Doc_Code,
		VT.Scheme_Code,
		TAF.AdditionalFieldValueCode,
		NULL AS [Precondition],
		VT.Service_Receive_Dtm
	FROM
		VoucherTransaction VT
			INNER JOIN TransactionDetail TD
				ON VT.Transaction_ID = TD.Transaction_ID
					AND TD.Subsidize_Item_Code = 'HSIV'
			INNER JOIN TransactionAdditionalField TAF
				ON VT.Transaction_ID = TAF.Transaction_ID
					AND TAF.AdditionalFieldID = 'CategoryCode'
	WHERE
		(VT.Scheme_Code = 'HSIVSS'
			OR VT.Scheme_Code = 'RVP')
			AND VT.Transaction_Dtm <= @Cutoff_Dtm
			AND VT.Record_Status <> 'I'


-- ---------------------------------------------
-- Patch the pre-condition
-- ---------------------------------------------

	UPDATE
		@Transaction
	SET
		Precondition = TAF.AdditionalFieldValueCode
	FROM
		@Transaction VT
			INNER JOIN TransactionAdditionalField TAF
				ON VT.Transaction_ID = TAF.Transaction_ID
					AND TAF.AdditionalFieldID = 'PreCondition'
	WHERE
		VT.AdditionalFieldValueCode = 'PRECOND'


-- ---------------------------------------------
-- Validated accounts
-- ---------------------------------------------

	INSERT INTO @Account (
		Scheme_Code,
		AdditionalFieldValueCode,
		Precondition,
		Age
	)
	SELECT
		VT.Scheme_Code,
		VT.AdditionalFieldValueCode,
		VT.Precondition,
		DATEDIFF(yy, VP.DOB, VT.Service_Receive_Dtm) AS [Age]
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
		Scheme_Code,
		AdditionalFieldValueCode,
		Precondition,
		Age
	)
	SELECT
		VT.Scheme_Code,
		VT.AdditionalFieldValueCode,
		VT.Precondition,
		DATEDIFF(yy, TP.DOB, VT.Service_Receive_Dtm) AS [Age]
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
		Scheme_Code,
		AdditionalFieldValueCode,
		Precondition,
		Age
	)
	SELECT
		VT.Scheme_Code,
		VT.AdditionalFieldValueCode,
		VT.Precondition,
		DATEDIFF(yy, SP.DOB, VT.Service_Receive_Dtm) AS [Age]
	FROM
		@Transaction VT
			INNER JOIN SpecialPersonalInformation SP
				ON VT.Special_Acc_ID = SP.Special_Acc_ID
	WHERE
		VT.Voucher_Acc_ID = ''
			AND VT.Special_Acc_ID <> ''
			AND VT.Invalid_Acc_ID = ''



-- ---------------------------------------------
-- Transaction raw
-- ---------------------------------------------

	INSERT INTO @TransactionRaw (
		Scheme_Code,
		Transaction_Dtm,
		Transaction_ID,
		SP_ID,
		Service_Receive_Dtm,
		Category_Code,
		Precondition,
		Dose,
		Voucher_Acc_ID,
		Temp_Voucher_Acc_ID,
		Special_Acc_ID,
		Invalid_Acc_ID,
		Doc_Code,
		Row
	)
	SELECT
		VT.Scheme_Code,
		VT.Transaction_Dtm,
		VT.Transaction_ID,
		VT.SP_ID,
		VT.Service_Receive_Dtm,
		TAF.AdditionalFieldValueCode AS [Category_Code],
		NULL AS [Precondition],
		TD.Available_Item_Code AS [Dose],
		ISNULL(VT.Voucher_Acc_ID, ''),
		ISNULL(VT.Temp_Voucher_Acc_ID, ''),
		ISNULL(VT.Special_Acc_ID, ''),
		ISNULL(VT.Invalid_Acc_ID, ''),
		VT.Doc_Code,
		10 + ROW_NUMBER() OVER (ORDER BY VT.Scheme_Code, VT.Transaction_Dtm)
	FROM
		VoucherTransaction VT
			INNER JOIN TransactionDetail TD
				ON VT.Transaction_ID = TD.Transaction_ID
					AND TD.Subsidize_Item_Code = 'HSIV'
			INNER JOIN TransactionAdditionalField TAF
				ON VT.Transaction_ID = TAF.Transaction_ID
					AND TAF.AdditionalFieldID = 'CategoryCode'
	WHERE
		(VT.Scheme_Code = 'HSIVSS' OR VT.Scheme_Code = 'RVP')
			AND VT.Transaction_Dtm BETWEEN @Start_Dtm AND @Cutoff_Dtm
			AND VT.Record_Status <> 'I'


-- ---------------------------------------------
-- Patch the pre-condition
-- ---------------------------------------------

	UPDATE
		@TransactionRaw
	SET
		Precondition = SD.Data_Value
	FROM
		@TransactionRaw VT
			INNER JOIN TransactionAdditionalField TAF
				ON VT.Transaction_ID = TAF.Transaction_ID
					AND TAF.AdditionalFieldID = 'PreCondition'
			INNER JOIN StaticData SD
				ON TAF.AdditionalFieldValueCode = SD.Item_No
					AND SD.Column_Name = 'PreCondition'
	WHERE
		VT.Category_Code = 'PRECOND'


-- ---------------------------------------------
-- Validated accounts
-- ---------------------------------------------

	INSERT INTO @AccountRaw (
		Scheme_Code,
		Transaction_Dtm,
		Transaction_ID,
		SP_ID,
		Service_Receive_Dtm,
		Category_Code,
		Precondition,
		Dose,
		DOB,
		Exact_DOB,
		Row
	)
	SELECT
		VT.Scheme_Code,
		VT.Transaction_Dtm,
		VT.Transaction_ID,
		VT.SP_ID,
		VT.Service_Receive_Dtm,
		VT.Category_Code,
		VT.Precondition,
		VT.Dose,
		VP.DOB,
		VP.Exact_DOB,
		VT.Row
	FROM
		@TransactionRaw VT
			INNER JOIN PersonalInformation VP
				ON VT.Voucher_Acc_ID = VP.Voucher_Acc_ID
					AND VT.Doc_Code = VP.Doc_Code
	WHERE
		VT.Voucher_Acc_ID <> ''


-- ---------------------------------------------
-- Temporary accounts
-- ---------------------------------------------

	INSERT INTO @AccountRaw (
		Scheme_Code,
		Transaction_Dtm,
		Transaction_ID,
		SP_ID,
		Service_Receive_Dtm,
		Category_Code,
		Precondition,
		Dose,
		DOB,
		Exact_DOB,
		Row
	)
	SELECT
		VT.Scheme_Code,
		VT.Transaction_Dtm,
		VT.Transaction_ID,
		VT.SP_ID,
		VT.Service_Receive_Dtm,
		VT.Category_Code,
		VT.Precondition,
		VT.Dose,
		TP.DOB,
		TP.Exact_DOB,
		VT.Row
	FROM
		@TransactionRaw VT	
			INNER JOIN TempPersonalInformation TP
				ON VT.Temp_Voucher_Acc_ID = TP.Voucher_Acc_ID
	WHERE
		VT.Voucher_Acc_ID = ''
			AND VT.Temp_Voucher_Acc_ID <> ''
			AND VT.Special_Acc_ID = ''


-- ---------------------------------------------
-- Special accounts
-- ---------------------------------------------

	INSERT INTO @AccountRaw (
		Scheme_Code,
		Transaction_Dtm,
		Transaction_ID,
		SP_ID,
		Service_Receive_Dtm,
		Category_Code,
		Precondition,
		Dose,
		DOB,
		Exact_DOB,
		Row
	)
	SELECT
		VT.Scheme_Code,
		VT.Transaction_Dtm,
		VT.Transaction_ID,
		VT.SP_ID,
		VT.Service_Receive_Dtm,
		VT.Category_Code,
		VT.Precondition,
		VT.Dose,
		SP.DOB,
		SP.Exact_DOB,
		VT.Row
	FROM
		@TransactionRaw VT	
			INNER JOIN SpecialPersonalInformation SP
				ON VT.Special_Acc_ID = SP.Special_Acc_ID
	WHERE
		VT.Voucher_Acc_ID = ''
			AND VT.Special_Acc_ID <> ''
			AND VT.Invalid_Acc_ID = ''


-- =============================================
-- Process data
-- =============================================

-- ---------------------------------------------
-- Build frame (01)
-- ---------------------------------------------
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) VALUES
	(0, 'HSIV Transaction Statistic Summary', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) VALUES
	(1, 'As at ' + CONVERT(varchar, @Cutoff_Dtm, 111) + ' 13:00', '', '', '', '', '', '', '', '')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) VALUES
	(2, '', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) VALUES
	(3, 'By category', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) VALUES
	(4, '', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) VALUES
	(5, '', @Category_Child, @Category_Elder, @Category_Hcw, @Category_Precond, '', '', @Category_Resident, 'Total')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) VALUES
	(6, '', '', '', '', 'With age <65**', '', 'With age >=65**', '', '')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) VALUES
	(7, '', '', '', '', 'Medical condition of Pregnancy', 'Other medical condition', '', '', '')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) VALUES
	(11, 'HSIVSS', '', '', '', '', '', '', '', '')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) VALUES
	(12, 'RVP', '', '', '', '', '', '', '', '')
	
--

	UPDATE	@ResultTable
	SET		Result_Value2 = (SELECT Category_Name FROM ClaimCategory WHERE Category_Code = @Category_Child)
	WHERE	Result_Seq = 5
	
	UPDATE	@ResultTable
	SET		Result_Value3 = (SELECT Category_Name FROM ClaimCategory WHERE Category_Code = @Category_Elder)
	WHERE	Result_Seq = 5
	
	UPDATE	@ResultTable
	SET		Result_Value4 = (SELECT Category_Name FROM ClaimCategory WHERE Category_Code = @Category_Hcw)
	WHERE	Result_Seq = 5
	
	UPDATE	@ResultTable
	SET		Result_Value5 = (SELECT Category_Name FROM ClaimCategory WHERE Category_Code = @Category_Precond)
	WHERE	Result_Seq = 5

	UPDATE	@ResultTable
	SET		Result_Value8 = (SELECT Category_Name FROM ClaimCategory WHERE Category_Code = @Category_Resident)
	WHERE	Result_Seq = 5

-- ---------------------------------------------
-- Build data (01)
-- ---------------------------------------------

-- HSIVSS
	
	UPDATE
		@ResultTable
	SET
		Result_Value2 =
			(
			SELECT
				COUNT(1)
			FROM
				@Account
			WHERE
				Scheme_Code = 'HSIVSS' 
					AND AdditionalFieldValueCode = @Category_Child
			)
	WHERE
		Result_Seq = 11
		
	UPDATE
		@ResultTable
	SET
		Result_Value3 =
			(
			SELECT
				COUNT(1)
			FROM
				@Account
			WHERE 
				Scheme_Code = 'HSIVSS' 
					AND AdditionalFieldValueCode = @Category_Elder
			)
	WHERE
		Result_Seq = 11
		
	UPDATE
		@ResultTable
	SET
		Result_Value4 =
			(
			SELECT
				COUNT(1)
			FROM
				@Account
			WHERE 
				Scheme_Code = 'HSIVSS' 
					AND AdditionalFieldValueCode = @Category_Hcw
			)
	WHERE
		Result_Seq = 11
		
	UPDATE
		@ResultTable
	SET
		Result_Value5 =
			(
			SELECT
				COUNT(1)
			FROM
				@Account
			WHERE 
				Scheme_Code = 'HSIVSS' 
					AND AdditionalFieldValueCode = @Category_Precond
					AND Age < 65
					AND ISNULL(Precondition, '') = 'P5'
			)
	WHERE
		Result_Seq = 11
		
	UPDATE
		@ResultTable
	SET
		Result_Value6 =
			(
			SELECT
				COUNT(1)
			FROM
				@Account
			WHERE 
				Scheme_Code = 'HSIVSS' 
					AND AdditionalFieldValueCode = @Category_Precond
					AND Age < 65
					AND ISNULL(Precondition, '') <> 'P5'
			)
	WHERE
		Result_Seq = 11
		
	UPDATE
		@ResultTable
	SET
		Result_Value7 =
			(
			SELECT
				COUNT(1)
			FROM
				@Account
			WHERE 
				Scheme_Code = 'HSIVSS' 
					AND AdditionalFieldValueCode = @Category_Precond
					AND Age >= 65
			)
	WHERE
		Result_Seq = 11
	
	UPDATE
		@ResultTable
	SET
		Result_Value8 =
			(
			SELECT
				COUNT(1)
			FROM
				@Account
			WHERE 
				Scheme_Code = 'HSIVSS' 
					AND AdditionalFieldValueCode = @Category_Resident
			)
	WHERE
		Result_Seq = 11
			
	UPDATE
		@ResultTable
	SET
		Result_Value9 =
			(
			CONVERT(int, Result_Value2)
			+
			CONVERT(int, Result_Value3)
			+
			CONVERT(int, Result_Value4)
			+
			CONVERT(int, Result_Value5)
			+
			CONVERT(int, Result_Value6)
			+
			CONVERT(int, Result_Value7)
			+
			CONVERT(int, Result_Value8)
			)
	WHERE
		Result_Seq = 11
		
-- RVP

	UPDATE
		@ResultTable
	SET
		Result_Value2 =
			(
			SELECT
				COUNT(1)
			FROM
				@Account
			WHERE 
				Scheme_Code = 'RVP' 
					AND AdditionalFieldValueCode = @Category_Child
			)
	WHERE
		Result_Seq = 12
		
	UPDATE
		@ResultTable
	SET
		Result_Value3 =
			(
			SELECT
				COUNT(1)
			FROM
				@Account
			WHERE 
				Scheme_Code = 'RVP' 
					AND AdditionalFieldValueCode = @Category_Elder
			)
	WHERE
		Result_Seq = 12
		
	UPDATE
		@ResultTable
	SET
		Result_Value4 =
			(
			SELECT
				COUNT(1)
			FROM
				@Account
			WHERE 
				Scheme_Code = 'RVP' 
					AND AdditionalFieldValueCode = @Category_Hcw
			)
	WHERE
		Result_Seq = 12
		
	UPDATE
		@ResultTable
	SET
		Result_Value5 =
			(
			SELECT
				COUNT(1)
			FROM
				@Account
			WHERE 
				Scheme_Code = 'RVP' 
					AND AdditionalFieldValueCode = @Category_Precond
					AND Age < 65
					AND ISNULL(Precondition, '') = 'P5'
			)
	WHERE
		Result_Seq = 12
		
	UPDATE
		@ResultTable
	SET
		Result_Value6 =
			(
			SELECT
				COUNT(1)
			FROM
				@Account
			WHERE 
				Scheme_Code = 'RVP' 
					AND AdditionalFieldValueCode = @Category_Precond
					AND Age < 65
					AND ISNULL(Precondition, '') <> 'P5'
			)
	WHERE
		Result_Seq = 12
		
	UPDATE
		@ResultTable
	SET
		Result_Value7 =
			(
			SELECT
				COUNT(1)
			FROM
				@Account
			WHERE 
				Scheme_Code = 'RVP' 
					AND AdditionalFieldValueCode = @Category_Precond
					AND Age >= 65
			)
	WHERE
		Result_Seq = 12
	
	UPDATE
		@ResultTable
	SET
		Result_Value8 =
			(
			SELECT
				COUNT(1)
			FROM
				@Account
			WHERE 
				Scheme_Code = 'RVP' 
					AND AdditionalFieldValueCode = @Category_Resident
			)
	WHERE
		Result_Seq = 12
			
	UPDATE
		@ResultTable
	SET
		Result_Value9 =
			(
			CONVERT(int, Result_Value2)
			+
			CONVERT(int, Result_Value3)
			+
			CONVERT(int, Result_Value4)
			+
			CONVERT(int, Result_Value5)
			+
			CONVERT(int, Result_Value6)
			+
			CONVERT(int, Result_Value7)
			+
			CONVERT(int, Result_Value8)
			)
	WHERE
		Result_Seq = 12

-- Patch the not available fields to NA

	UPDATE
		@ResultTable
	SET
		Result_Value8 = 'NA'
	WHERE
		Result_Seq = 11
		
	UPDATE
		@ResultTable
	SET
		Result_Value2 = 'NA',
		Result_Value3 = 'NA',
		Result_Value5 = 'NA',
		Result_Value6 = 'NA',
		Result_Value7 = 'NA'
	WHERE
		Result_Seq = 12


-- ---------------------------------------------
-- Build frame (02)
-- ---------------------------------------------
	
	INSERT INTO @ResultTableRaw (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10) VALUES
	(0, 'HSIV Transactions', '', '', '', '', '', '', '', '', '')

	INSERT INTO @ResultTableRaw (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10) VALUES
	(1, 'Transaction time: ' + CONVERT(varchar, @Cutoff_Dtm, 111) + ' 00:00 to 13:00', '', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTableRaw (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10) VALUES
	(2, '', '', '', '', '', '', '', '', '', '')
		
	INSERT INTO @ResultTableRaw (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10) VALUES
	(5, 'Scheme', 'Transaction Time', 'Transaction ID', 'SPID', 'Service Date', 'Category', 'Medical Condition (if applicable)', 'Dose', 'DOB', 'DOB Flag')


-- ---------------------------------------------
-- Build data (02)
-- ---------------------------------------------
			
	INSERT INTO @ResultTableRaw (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10)
	SELECT
		A.Row,
		SC.Display_Code AS [Scheme],
		CONVERT(varchar, A.Transaction_Dtm, 20),
		LEFT(A.Transaction_ID, 7) + '-' + CONVERT(varchar, CONVERT(int, SUBSTRING(A.Transaction_ID, 8, 8))) + '-' + SUBSTRING(A.Transaction_ID, 16, 1) AS [Transaction ID],
		A.SP_ID,
		CONVERT(varchar(10), A.Service_Receive_Dtm, 20),
		CC.Category_Name AS [Category_Code],
		ISNULL(A.Precondition, ''),
		CASE A.Dose
			WHEN 'ONLYDOSE' THEN 'Only Dose'
			ELSE SID.Available_Item_Desc
		END AS [Dose],
		CONVERT(varchar(10), A.DOB, 20),
		A.Exact_DOB
	FROM
		@AccountRaw A
			INNER JOIN @EffectiveScheme ES
				ON A.Scheme_Code = ES.Scheme_Code
			INNER JOIN SchemeClaim SC
				ON ES.Scheme_Code = SC.Scheme_Code
					AND ES.Scheme_Seq = SC.Scheme_Seq
			INNER JOIN ClaimCategory CC
				ON A.Category_Code = CC.Category_Code
			INNER JOIN SubsidizeItemDetails SID
				ON A.Dose = SID.Available_Item_Code
					AND SID.Subsidize_Item_Code = 'HSIV'

		
-- =============================================
-- Return result
-- =============================================

-- 01

	SELECT
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6,
		Result_Value7,
		Result_Value8,
		Result_Value9
	FROM
		@ResultTable
	ORDER BY
		Result_Seq


-- 02

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
		Result_Value10
	FROM
		@ResultTableRaw
	ORDER BY
		Result_Seq


END 
GO


GRANT EXECUTE ON [dbo].[proc_EHS_HSIVSSRVPHSIVCategoryReport_Stat] TO HCVU
GO
*/