IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_HSIVSSTransaction_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_HSIVSSTransaction_Stat]
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
-- Modified date:	15 March 2010
-- Description:		Add Gender
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	4 March 2010
-- Description:		Separate Transaction Status and Reimbursement Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	4 March 2010
-- Description:		Fix the Record Status for the reimbursing transactions
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		27 January 2010
-- Description:		Generate report for HSIVSS Transactions
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
/*
CREATE PROCEDURE [dbo].[proc_EHS_HSIVSSTransaction_Stat] 
	@Cutoff_Dtm	datetime
AS BEGIN
	
-- =============================================
-- Constant
-- =============================================
	DECLARE @Date_Range	tinyint
	SET @Date_Range = 7
	
	
-- =============================================
-- Variables
-- =============================================
	DECLARE @Report_Dtm datetime
	SET @Report_Dtm = DATEADD(dd, -1, @Cutoff_Dtm)
	
	
-- =============================================
-- Temporary tables
-- =============================================
	DECLARE @Transaction table (
		SP_ID						char(8),
		Transaction_ID				char(20),
		Transaction_Dtm				datetime,
		Service_Receive_Dtm			datetime,
		Category_Code				varchar(50),
		Precondition				varchar(250),
		Dose						char(20),
		Voucher_Acc_ID				char(15),
		Temp_Voucher_Acc_ID			char(15),
		Special_Acc_ID				char(15),
		Invalid_Acc_ID				char(15),
		Doc_Code					char(20),
		Transaction_Status			char(1),
		Reimbursement_Status		char(1),
		Row							int
	)

	DECLARE @Account table (
		SP_ID						char(8),
		Transaction_ID				char(20),
		Transaction_Dtm				datetime,
		Service_Receive_Dtm			datetime,
		Category_Code				varchar(50),
		Precondition				varchar(250),
		Dose						char(20),
		DOB							datetime,
		Exact_DOB					char(1),
		Sex							char(1),
		Doc_Code					char(20),
		Transaction_Status			char(1),
		Reimbursement_Status		char(1),
		Row							int
	)
	
	DECLARE @ResultTable table (
		Result_Seq					int,
		Result_Value1				varchar(100),
		Result_Value2				varchar(100),
		Result_Value3				varchar(100),
		Result_Value4				varchar(100),
		Result_Value5				varchar(100),
		Result_Value6				varchar(250),
		Result_Value7				varchar(100),
		Result_Value8				varchar(100),
		Result_Value9				varchar(100),
		Result_Value10				varchar(100),
		Result_Value11				varchar(100),
		Result_Value12				varchar(100),
		Result_Value13				varchar(100)
	)
	

-- =============================================
-- Retrieve data
-- =============================================

-- ---------------------------------------------
-- HSIVSS transactions
-- ---------------------------------------------

	INSERT INTO @Transaction (
		SP_ID,
		Transaction_ID,
		Transaction_Dtm,
		Service_Receive_Dtm,
		Category_Code,
		Precondition,
		Dose,
		Voucher_Acc_ID,
		Temp_Voucher_Acc_ID,
		Special_Acc_ID,
		Invalid_Acc_ID,
		Doc_Code,
		Transaction_Status,
		Reimbursement_Status,
		Row
	)
	SELECT
		VT.SP_ID,
		VT.Transaction_ID,
		VT.Transaction_Dtm,
		VT.Service_Receive_Dtm,
		TAF.AdditionalFieldValueCode AS [Category_Code],
		NULL AS [Precondition],
		TD.Available_Item_Code AS [Dose],
		ISNULL(VT.Voucher_Acc_ID, ''),
		ISNULL(VT.Temp_Voucher_Acc_ID, ''),
		ISNULL(VT.Special_Acc_ID, ''),
		ISNULL(VT.Invalid_Acc_ID, ''),
		VT.Doc_Code,
		VT.Record_Status AS [Transaction_Status],
		NULL AS [Reimbursement_Status],
		10 + ROW_NUMBER() OVER (ORDER BY VT.Transaction_Dtm)
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
			AND VT.Transaction_Dtm > DATEADD(dd, -1 * @Date_Range + 1, @Report_Dtm)
			AND VT.Record_Status <> 'I'


-- ---------------------------------------------
-- Patch the pre-condition
-- ---------------------------------------------

	UPDATE
		@Transaction
	SET
		Precondition = SD.Data_Value
	FROM
		@Transaction VT
			INNER JOIN TransactionAdditionalField TAF
				ON VT.Transaction_ID = TAF.Transaction_ID
					AND TAF.AdditionalFieldID = 'PreCondition'
			INNER JOIN StaticData SD
				ON TAF.AdditionalFieldValueCode = SD.Item_No
					AND SD.Column_Name = 'PreCondition'
	WHERE
		VT.Category_Code = 'PRECOND'


-- ---------------------------------------------
-- Patch the Reimbursement_Status
-- ---------------------------------------------

	UPDATE
		@Transaction
	SET
		Reimbursement_Status = 
			CASE RAT.Authorised_Status
				WHEN 'R' THEN 'G'
				ELSE RAT.Authorised_Status
			END
	FROM
		@Transaction VT
			INNER JOIN ReimbursementAuthTran RAT
				ON VT.Transaction_ID = RAT.Transaction_ID


-- ---------------------------------------------
-- Patch the Transaction_Status
-- ---------------------------------------------

	UPDATE
		@Transaction
	SET
		Transaction_Status = 'R'
	WHERE
		Reimbursement_Status = 'G'


-- ---------------------------------------------
-- Validated accounts
-- ---------------------------------------------

	INSERT INTO @Account (
		SP_ID,
		Transaction_ID,
		Transaction_Dtm,
		Service_Receive_Dtm,
		Category_Code,
		Precondition,
		Dose,
		DOB,
		Exact_DOB,
		Sex,
		Doc_Code,
		Transaction_Status,
		Reimbursement_Status,
		Row
	)
	SELECT
		VT.SP_ID,
		VT.Transaction_ID,
		VT.Transaction_Dtm,
		VT.Service_Receive_Dtm,
		VT.Category_Code,
		VT.Precondition,
		VT.Dose,
		VP.DOB,
		VP.Exact_DOB,
		VP.Sex,
		VT.Doc_Code,
		VT.Transaction_Status,
		VT.Reimbursement_Status,
		VT.Row
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
		SP_ID,
		Transaction_ID,
		Transaction_Dtm,
		Service_Receive_Dtm,
		Category_Code,
		Precondition,
		Dose,
		DOB,
		Exact_DOB,
		Sex,
		Doc_Code,
		Transaction_Status,
		Reimbursement_Status,
		Row
	)
	SELECT
		VT.SP_ID,
		VT.Transaction_ID,
		VT.Transaction_Dtm,
		VT.Service_Receive_Dtm,
		VT.Category_Code,
		VT.Precondition,
		VT.Dose,
		TP.DOB,
		TP.Exact_DOB,
		TP.Sex,
		VT.Doc_Code,
		VT.Transaction_Status,
		VT.Reimbursement_Status,
		VT.Row
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
		SP_ID,
		Transaction_ID,
		Transaction_Dtm,
		Service_Receive_Dtm,
		Category_Code,
		Precondition,
		Dose,
		DOB,
		Exact_DOB,
		Sex,
		Doc_Code,
		Transaction_Status,
		Reimbursement_Status,
		Row
	)
	SELECT
		VT.SP_ID,
		VT.Transaction_ID,
		VT.Transaction_Dtm,
		VT.Service_Receive_Dtm,
		VT.Category_Code,
		VT.Precondition,
		VT.Dose,
		SP.DOB,
		SP.Exact_DOB,
		SP.Sex,
		VT.Doc_Code,
		VT.Transaction_Status,
		VT.Reimbursement_Status,
		VT.Row
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
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13) VALUES
	(0, 'HSIVSS transactions (as at ' + CONVERT(varchar, @Report_Dtm, 111) + ')', '', '', '', '', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13) VALUES
	(1, '', '', '', '', '', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13) VALUES
	(5, 'Transaction ID', 'Transaction Time', 'SPID', 'Service Date', 'Category', 'Medical Condition', 'Dose', 'DOB', 'DOB Flag', 'Gender', 'Doc Type', 'Transaction Status', 'Reimbursement Status')


-- ---------------------------------------------
-- Build data
-- ---------------------------------------------

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
	SELECT
		A.Row,
		LEFT(A.Transaction_ID, 7) + '-' + CONVERT(varchar, CONVERT(int, SUBSTRING(A.Transaction_ID, 8, 8))) + '-' + SUBSTRING(A.Transaction_ID, 16, 1) AS [Transaction ID],
		CONVERT(varchar, A.Transaction_Dtm, 20),
		A.SP_ID,
		CONVERT(varchar(10), A.Service_Receive_Dtm, 20),
		CC.Category_Name AS [Category_Code],
		ISNULL(A.Precondition, ''),
		CASE A.Dose
			WHEN 'ONLYDOSE' THEN 'Only Dose'
			ELSE SID.Available_Item_Desc
		END AS [Dose],
		CONVERT(varchar(10), A.DOB, 20),
		A.Exact_DOB,
		A.Sex,
		A.Doc_Code,
		SD1.Status_Description,
		ISNULL(SD2.Status_Description, '')
	FROM
		@Account A
			INNER JOIN ClaimCategory CC
				ON A.Category_Code = CC.Category_Code
			INNER JOIN SubsidizeItemDetails SID
				ON A.Dose = SID.Available_Item_Code
					AND SID.Subsidize_Item_Code = 'HSIV'
			INNER JOIN StatusData SD1
				ON A.Transaction_Status = SD1.Status_Value
					AND SD1.Enum_Class = 'ClaimTransStatus'
			LEFT JOIN StatusData SD2
				ON A.Reimbursement_Status = SD2.Status_Value
					AND SD2.Enum_Class = 'ReimbursementStatus'


-- =============================================
-- Return result
-- =============================================

	DELETE FROM _HSIVSSTransaction_Stat

	INSERT INTO _HSIVSSTransaction_Stat (
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
		Result_Value12,
		Result_Value13
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
		Result_Value12,
		Result_Value13
	FROM
		@ResultTable


END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_HSIVSSTransaction_Stat] TO HCVU
GO
*/