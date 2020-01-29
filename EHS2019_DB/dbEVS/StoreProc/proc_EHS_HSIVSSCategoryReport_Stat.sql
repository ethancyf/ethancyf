IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_HSIVSSCategoryReport_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_HSIVSSCategoryReport_Stat]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	30 August 2016
-- CR No.:			CRE16-002
-- Description:		Stored procedure is no longer used
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	28 January 2010
-- Description:		Add the sections Age Group and Dose
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		21 December 2009
-- Description:		Generate report for the Vaccination Claim Report (Scheme HSIVSS)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
/*
CREATE PROCEDURE [dbo].[proc_EHS_HSIVSSCategoryReport_Stat] 
	@Cutoff_Dtm	datetime
AS BEGIN
-- =============================================
-- Constant
-- =============================================
	DECLARE @Category_Child		char(10)
	DECLARE @Category_Elder		char(10)
	DECLARE @Category_Hcw		char(10)
	DECLARE @Category_Precond	char(10)
	
	SET @Category_Child	= 'CHILD'
	SET @Category_Elder	= 'ELDER'
	SET @Category_Hcw = 'HCW'
	SET @Category_Precond = 'PRECOND'


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
		Precondition				varchar(50),
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
		Result_Value4				varchar(100),
		Result_Value5				varchar(100),
		Result_Value6				varchar(100),
		Result_Value7				varchar(250),
		Result_Value8				varchar(100),
		Result_Value9				varchar(100),
		Result_Value10				varchar(100),
		Result_Value11				varchar(100),
		Result_Value12				varchar(100)
	)
	
	
-- =============================================
-- Retrieve data
-- =============================================

-- ---------------------------------------------
-- HSIVSS transactions
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
		Precondition,
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
		NULL AS [Precondition],
		VT.Service_Receive_Dtm,
		VT.SP_ID
	FROM
		VoucherTransaction VT
			INNER JOIN TransactionDetail TD
				ON VT.Transaction_ID = TD.Transaction_ID
			INNER JOIN TransactionAdditionalField TAF
				ON VT.Transaction_ID = TAF.Transaction_ID
					AND TAF.AdditionalFieldID = 'CategoryCode'
	WHERE
		VT.Scheme_Code = 'HSIVSS'
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
		VT.Category_Code = 'PRECOND'


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
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(0, 'HSIVSS transactions (as at ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111) + ')', '', '', '', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(1, '', '', '', '', '', '', '', '', '', '', '', '')

--
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(10, '(i) By age group', '', '', '', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(11, '', '', '', '', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(20, '6 months to less than 9 years', '9 years to <65 age year', '>=65 age year', 'No. of SP involved', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(21, '', '', '', '', '', '', '', '', '', '', '', '')
	
--

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(30, '', '', '', '', '', '', '', '', '', '', '', '')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(31, '', '', '', '', '', '', '', '', '', '', '', '')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(32, '', '', '', '', '', '', '', '', '', '', '', '')

--

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(40, '(ii) By dose', '', '', '', '', '', '', '', '', '', '', '')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(41, '', '', '', '', '', '', '', '', '', '', '', '')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(50, '1st Dose', '2nd Dose', 'Only Dose', 'Total', '', '', '', '', '', '', '', '')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(51, '', '', '', '', '', '', '', '', '', '', '', '')

--

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(60, '', '', '', '', '', '', '', '', '', '', '', '')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(61, '', '', '', '', '', '', '', '', '', '', '', '')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(62, '', '', '', '', '', '', '', '', '', '', '', '')

--

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(71, '(iii) By category', '', '', '', '', '', '', '', '', '', '', '')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(72, '', '', '', '', '', '', '', '', '', '', '', '')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(80, @Category_Child, @Category_Elder, @Category_Hcw, @Category_Precond, '', '', '', '', '', 'Other Categories', '', 'Total')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(81, '', '', '', 'Cardiopulmonary disease', 'Severe obesity, metabolic or renal disease', 'Immuno-deficiency', 'Neuro-logical condition', 'Pregnancy', 'Children and adolescents', '', '', '')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12) VALUES
	(82, '', '', '', '', '', '', '', '', '', '', '', '')

--

	UPDATE	@ResultTable
	SET		Result_Value1 = (SELECT Category_Name FROM ClaimCategory WHERE Category_Code = @Category_Child)
	WHERE	Result_Seq = 80
	
	UPDATE	@ResultTable
	SET		Result_Value2 = (SELECT Category_Name FROM ClaimCategory WHERE Category_Code = @Category_Elder)
	WHERE	Result_Seq = 80
	
	UPDATE	@ResultTable
	SET		Result_Value3 = (SELECT Category_Name FROM ClaimCategory WHERE Category_Code = @Category_Hcw)
	WHERE	Result_Seq = 80
	
	UPDATE	@ResultTable
	SET		Result_Value4 = (SELECT Category_Name FROM ClaimCategory WHERE Category_Code = @Category_Precond)
	WHERE	Result_Seq = 80


-- ---------------------------------------------
-- Build data
-- ---------------------------------------------

-- (i) By age group

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
		Result_Seq = 21

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
		Result_Seq = 21

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
		Result_Seq = 21

	UPDATE
		@ResultTable
	SET
		Result_Value4 = (
			SELECT COUNT(DISTINCT SP_ID)
			FROM @Transaction
		)
	WHERE
		Result_Seq = 21


-- (ii) By dose

	UPDATE
		@ResultTable
	SET
		Result_Value1 = (
			SELECT COUNT(1)
			FROM @Transaction
			WHERE Dose = '1STDOSE'
		)
	WHERE
		Result_Seq = 51

	UPDATE
		@ResultTable
	SET
		Result_Value2 = (
			SELECT COUNT(1)
			FROM @Transaction
			WHERE Dose = '2NDDOSE'
		)
	WHERE
		Result_Seq = 51

	UPDATE
		@ResultTable
	SET
		Result_Value3 = (
			SELECT COUNT(1)
			FROM @Transaction
			WHERE Dose = 'ONLYDOSE'
		)
	WHERE
		Result_Seq = 51

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
		Result_Seq = 51


-- (iii) By category
	
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
				Category_Code = @Category_Child
			)
	WHERE
		Result_Seq = 82
		
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
				Category_Code = @Category_Elder
			)
	WHERE
		Result_Seq = 82
		
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
				Category_Code = @Category_Hcw
			)
	WHERE
		Result_Seq = 82
		
	UPDATE
		@ResultTable
	SET
		Result_Value4 =
			(
			SELECT
				COUNT(1)
			FROM
				@Transaction
			WHERE 
				Category_Code = @Category_Precond
					AND ISNULL(Precondition, '') = 'P1'
			)
	WHERE
		Result_Seq = 82
		
	UPDATE
		@ResultTable
	SET
		Result_Value5 =
			(
			SELECT
				COUNT(1)
			FROM
				@Transaction
			WHERE 
				Category_Code = @Category_Precond
					AND ISNULL(Precondition, '') = 'P2'
			)
	WHERE
		Result_Seq = 82
	
	UPDATE
		@ResultTable
	SET
		Result_Value6 =
			(
			SELECT
				COUNT(1)
			FROM
				@Transaction
			WHERE 
				Category_Code = @Category_Precond
					AND ISNULL(Precondition, '') = 'P3'
			)
	WHERE
		Result_Seq = 82	
	
	UPDATE
		@ResultTable
	SET
		Result_Value7 =
			(
			SELECT
				COUNT(1)
			FROM
				@Transaction
			WHERE 
				Category_Code = @Category_Precond
					AND ISNULL(Precondition, '') = 'P4'
			)
	WHERE
		Result_Seq = 82	
	
	UPDATE
		@ResultTable
	SET
		Result_Value8 =
			(
			SELECT
				COUNT(1)
			FROM
				@Transaction
			WHERE 
				Category_Code = @Category_Precond
					AND ISNULL(Precondition, '') = 'P5'
			)
	WHERE
		Result_Seq = 82
		
	UPDATE
		@ResultTable
	SET
		Result_Value9 =
			(
			SELECT
				COUNT(1)
			FROM
				@Transaction
			WHERE 
				Category_Code = @Category_Precond
					AND ISNULL(Precondition, '') = 'P6'
			)
	WHERE
		Result_Seq = 82	
	
	UPDATE
		@ResultTable
	SET
		Result_Value10 =
			(
			SELECT
				COUNT(1)
			FROM
				@Transaction
			WHERE 
				Category_Code <> @Category_Child
					AND Category_Code <> @Category_Elder
					AND Category_Code <> @Category_Hcw
					AND Category_Code <> @Category_Precond
			)
	WHERE
		Result_Seq = 82
			
	UPDATE
		@ResultTable
	SET
		Result_Value12 =
			(
			CONVERT(int, Result_Value1)
			+
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
		Result_Seq = 82


-- ---------------------------------------------
-- Insert to statistics table
-- ---------------------------------------------

	DELETE FROM _HSIVSSCategoryReport_Stat
	
	INSERT INTO _HSIVSSCategoryReport_Stat (
		Result_Seq,
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
		Result_Value12
	)
	SELECT
		Result_Seq,
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
		Result_Value12
	FROM
		@ResultTable
	ORDER BY
		Result_Seq
		
		
END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_HSIVSSCategoryReport_Stat] TO HCVU
GO
*/