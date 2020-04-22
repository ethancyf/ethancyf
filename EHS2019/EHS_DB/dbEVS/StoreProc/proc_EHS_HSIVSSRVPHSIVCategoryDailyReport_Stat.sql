IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_HSIVSSRVPHSIVCategoryDailyReport_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_HSIVSSRVPHSIVCategoryDailyReport_Stat]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	30 August 2016
-- CR No.:			CRE16-002
-- Description:		Get [Category_Code] from table VoucherTransaction
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	18 November 2015
-- CR No.:			CRE15-006
-- Description:		Rename of eHS
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE11-014
-- Modified by:		Koala CHENG
-- Modified date:	23 June 2011
-- Description:		Change Parameter name for new parameter from generic report submit control
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	8 February 2010
-- Description:		Handle empty @param_value01 (Transaction Time From - Date part)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	5 February 2010
-- Description:		Add @request_time
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		4 February 2010
-- Description:		Generate report for HSIV Category Report (scheme HSIVSS and RVP)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_HSIVSSRVPHSIVCategoryDailyReport_Stat] 
	@request_time	datetime,
	@From_Date		varchar(255),
	@To_Date		varchar(255)
AS BEGIN

-- =============================================
-- Report setting
-- =============================================
	DECLARE @From_Dtm	datetime
	DECLARE @To_Dtm		datetime
	DECLARE @Now_Dtm	datetime
	
	SET @From_Dtm = @From_Date
	SET @To_Dtm = @To_Date
	IF @From_Dtm = ''
		SET @From_Dtm = (SELECT Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'HSIVClaimReportMinTime')
		
	SET @Now_Dtm = GETDATE()


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
		Category_Code				varchar(50),
		Precondition				varchar(50),
		Service_Receive_Dtm			datetime
	)

	DECLARE @Account table (
		Scheme_Code					char(10),
		Category_Code				varchar(50),
		Precondition				varchar(50),
		Age							smallint
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
		Result_Value9				varchar(100),
		Result_Value10				varchar(100),
		Result_Value11				varchar(100)
	)
	
	DECLARE @ResultTableNote table (
		Result_Seq					smallint,
		Result_Value1				varchar(100),
		Result_Value2				varchar(100)
	)
	
	
-- =============================================
-- Initialization 
-- =============================================
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
		Category_Code,
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
		VT.Category_Code,
		NULL AS [Precondition],
		VT.Service_Receive_Dtm
	FROM
		VoucherTransaction VT
			INNER JOIN TransactionDetail TD
				ON VT.Transaction_ID = TD.Transaction_ID
					AND TD.Subsidize_Item_Code = 'HSIV'
			--INNER JOIN TransactionAdditionalField TAF
			--	ON VT.Transaction_ID = TAF.Transaction_ID
			--		AND TAF.AdditionalFieldID = 'CategoryCode'
	WHERE
		(VT.Scheme_Code = 'HSIVSS' OR VT.Scheme_Code = 'RVP')
			AND VT.Transaction_Dtm >= @From_Dtm
			AND VT.Transaction_Dtm < @To_Dtm
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
		VT.Category_Code = 'PRECOND'


-- ---------------------------------------------
-- Validated accounts
-- ---------------------------------------------

	INSERT INTO @Account (
		Scheme_Code,
		Category_Code,
		Precondition,
		Age
	)
	SELECT
		VT.Scheme_Code,
		VT.Category_Code,
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
		Category_Code,
		Precondition,
		Age
	)
	SELECT
		VT.Scheme_Code,
		VT.Category_Code,
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
		Category_Code,
		Precondition,
		Age
	)
	SELECT
		VT.Scheme_Code,
		VT.Category_Code,
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


-- =============================================
-- Process data
-- =============================================

-- ---------------------------------------------
-- Build frame
-- ---------------------------------------------
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	(0, 'eHS(S)D0014-01: HSIV Claim Report', '', '', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	(1, 'Transaction Time: ' + CONVERT(varchar, @From_Dtm, 111) + ' ' + CONVERT(varchar(5), @From_Dtm, 8) + ' to ' + CONVERT(varchar, @To_Dtm, 111) + ' ' + CONVERT(varchar(5), @To_Dtm, 8), '', '', '', '', '', '', '', '', '', '')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	(2, '', '', '', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	(3, 'By category', '', '', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	(4, '', '', '', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	(5, '', @Category_Child, @Category_Elder, @Category_Hcw, '', @Category_Precond, '', '', @Category_Resident, '', 'Total')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	(6, '', '', '', 'With age <65', 'With age >=65', 'With age <65', '', 'With age >=65', 'With age <65', 'With age >=65', '')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	(7, '', '', '', '', '', 'Medical condition of Pregnancy', 'Other medical condition', '', '', '', '')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	(11, 'HSIVSS', '', '', '', '', '', '', '', '', '', '')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	(12, 'RVP', '', '', '', '', '', '', '', '', '', '')
	
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
	SET		Result_Value6 = (SELECT Category_Name FROM ClaimCategory WHERE Category_Code = @Category_Precond)
	WHERE	Result_Seq = 5

	UPDATE	@ResultTable
	SET		Result_Value9 = (SELECT Category_Name FROM ClaimCategory WHERE Category_Code = @Category_Resident)
	WHERE	Result_Seq = 5


-- ---------------------------------------------
-- Build data
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
					AND Category_Code = @Category_Child
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
					AND Category_Code = @Category_Elder
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
					AND Category_Code = @Category_Hcw
					AND Age < 65
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
					AND Category_Code = @Category_Hcw
					AND Age >= 65
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
					AND Category_Code = @Category_Precond
					AND Age < 65
					AND ISNULL(Precondition, '') = 'P5'
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
					AND Category_Code = @Category_Precond
					AND Age < 65
					AND ISNULL(Precondition, '') <> 'P5'
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
					AND Category_Code = @Category_Precond
					AND Age >= 65
			)
	WHERE
		Result_Seq = 11
	
	UPDATE
		@ResultTable
	SET
		Result_Value9 =
			(
			SELECT
				COUNT(1)
			FROM
				@Account
			WHERE 
				Scheme_Code = 'HSIVSS' 
					AND Category_Code = @Category_Resident
					AND Age < 65
			)
	WHERE
		Result_Seq = 11

	UPDATE
		@ResultTable
	SET
		Result_Value10 =
			(
			SELECT
				COUNT(1)
			FROM
				@Account
			WHERE 
				Scheme_Code = 'HSIVSS' 
					AND Category_Code = @Category_Resident
					AND Age >= 65
			)
	WHERE
		Result_Seq = 11
			
	UPDATE
		@ResultTable
	SET
		Result_Value11 =
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
			+
			CONVERT(int, Result_Value9)
			+
			CONVERT(int, Result_Value10)
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
					AND Category_Code = @Category_Child
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
					AND Category_Code = @Category_Elder
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
					AND Category_Code = @Category_Hcw
					AND Age < 65
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
					AND Category_Code = @Category_Hcw
					AND Age >= 65
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
					AND Category_Code = @Category_Precond
					AND Age < 65
					AND ISNULL(Precondition, '') = 'P5'
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
					AND Category_Code = @Category_Precond
					AND Age < 65
					AND ISNULL(Precondition, '') <> 'P5'
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
					AND Category_Code = @Category_Precond
					AND Age >= 65
			)
	WHERE
		Result_Seq = 12
	
	UPDATE
		@ResultTable
	SET
		Result_Value9 =
			(
			SELECT
				COUNT(1)
			FROM
				@Account
			WHERE 
				Scheme_Code = 'RVP' 
					AND Category_Code = @Category_Resident
					AND Age < 65
			)
	WHERE
		Result_Seq = 12

	UPDATE
		@ResultTable
	SET
		Result_Value10 =
			(
			SELECT
				COUNT(1)
			FROM
				@Account
			WHERE 
				Scheme_Code = 'RVP' 
					AND Category_Code = @Category_Resident
					AND Age >= 65
			)
	WHERE
		Result_Seq = 12
			
	UPDATE
		@ResultTable
	SET
		Result_Value11 =
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
			+
			CONVERT(int, Result_Value9)
			+
			CONVERT(int, Result_Value10)
			)
	WHERE
		Result_Seq = 12


-- Patch the not available fields to NA

	UPDATE
		@ResultTable
	SET
		Result_Value9 = 'NA',
		Result_Value10 = 'NA'
	WHERE
		Result_Seq = 11
		
	UPDATE
		@ResultTable
	SET
		Result_Value2 = 'NA',
		Result_Value3 = 'NA',
		Result_Value6 = 'NA',
		Result_Value7 = 'NA',
		Result_Value8 = 'NA'
	WHERE
		Result_Seq = 12


-- ---------------------------------------------
-- Build frame (Note)
-- ---------------------------------------------
	
	INSERT INTO @ResultTableNote (Result_Seq, Result_Value1, Result_Value2)
	SELECT
		1,
		'Request Time',
		CONVERT(varchar, @request_time, 111) + ' ' + CONVERT(varchar, @request_time, 8)
	
	INSERT INTO @ResultTableNote (Result_Seq, Result_Value1, Result_Value2)
	SELECT
		1,
		'Generation Time',
		CONVERT(varchar, @Now_Dtm, 111) + ' ' + CONVERT(varchar, @Now_Dtm, 8)
			
	INSERT INTO @ResultTableNote (Result_Seq, Result_Value1, Result_Value2) VALUES
	(3, '', '')
	
	INSERT INTO @ResultTableNote (Result_Seq, Result_Value1, Result_Value2)
	SELECT
		4,
		'Transaction Time From',
		CONVERT(varchar, @From_Dtm, 111) + ' ' + CONVERT(varchar(5), @From_Dtm, 8)
			
	INSERT INTO @ResultTableNote (Result_Seq, Result_Value1, Result_Value2)
	SELECT
		5,
		'Transaction Time To',
		CONVERT(varchar, @To_Dtm, 111) + ' ' + CONVERT(varchar(5), @To_Dtm, 8)


-- =============================================
-- Return result
-- =============================================

	SELECT ''
	
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
		Result_Value11
	FROM
		@ResultTable
	ORDER BY
		Result_Seq

--

	SELECT
		Result_Value1,
		Result_Value2
	FROM
		@ResultTableNote
	ORDER BY
		Result_Seq


END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_HSIVSSRVPHSIVCategoryDailyReport_Stat] TO HCVU
GO
