IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_D0017_ConnectionFailTransaction_Get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_D0017_ConnectionFailTransaction_Get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		25 November 2010
-- Description:		Statistics for getting connection failed transactions
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_D0017_ConnectionFailTransaction_Get]
	@Report_Dtm		datetime = NULL
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Report setting 
-- =============================================
	DECLARE @Ext_Ref_Status_CN char(3)
	SET @Ext_Ref_Status_CN = '_CN'
	
	DECLARE @Ext_Ref_Status_UN char(3)
	SET @Ext_Ref_Status_UN = '_UN'
	
	DECLARE @VaccinationScheme table (
		Scheme_Code		char(10)
	)
	INSERT INTO @VaccinationScheme (
		Scheme_Code
	)
	SELECT DISTINCT
		SC.Scheme_Code
	FROM
		SchemeClaim SC
			INNER JOIN SubsidizeGroupClaim SGC
				ON SC.Scheme_Code = SGC.Scheme_Code
					AND SC.Scheme_Seq = SGC.Scheme_Seq
					AND SC.Record_Status = 'A'
					AND SGC.Record_Status = 'A'
			INNER JOIN Subsidize S
				ON SGC.Subsidize_Code = S.Subsidize_Code
					AND S.Record_Status = 'A'
			INNER JOIN SubsidizeItem SI
				ON S.Subsidize_Item_Code = SI.Subsidize_Item_Code
					AND SI.Record_Status = 'A'
					AND SI.Subsidize_Type = 'VACCINE'
	
		
-- =============================================
-- Declaration 
-- =============================================
	DECLARE @VaccineTransactionFailCount	int
	

-- =============================================
-- Initialization
-- =============================================
	IF @Report_Dtm IS NULL BEGIN
		SET @Report_Dtm = CONVERT(varchar(10), DATEADD(dd, -1, GETDATE()), 120)
	END ELSE BEGIN
		SET @Report_Dtm = CONVERT(varchar(10), @Report_Dtm, 120)
	END
	

-- =============================================
-- Temporary tables
-- =============================================
	DECLARE @TransactionFail table (
		Transaction_ID					char(20),
		Transaction_Dtm					datetime,
		Service_Receive_Dtm				datetime,
		Scheme_Code						char(10),
		Subsidize_Code					char(10),
		Subsidize_Code_Display			char(25),
		Available_Item_Code				char(20),
		Available_Item_Code_Display		char(20),
		SP_ID							char(8)
	)
	
	DECLARE @ResultTable2 table (
		Result_Seq						smallint,
		Result_Value1					varchar(100),
		Result_Value2					varchar(100)
	)
	
	DECLARE @ResultTable3 table (
		Result_Seq						smallint,
		Result_Value1					varchar(100),
		Result_Value2					varchar(100),
		Result_Value3					varchar(100),
		Result_Value4					varchar(100),
		Result_Value5					varchar(100),
		Result_Value6					varchar(100),
		Result_Value7					varchar(100)
	)
	

-- =============================================
-- Retrieve data
-- =============================================
	SELECT
		@VaccineTransactionFailCount = COUNT(1)
	FROM
		VoucherTransaction
	WHERE
		Transaction_Dtm BETWEEN @Report_Dtm AND DATEADD(dd, 1, @Report_Dtm)
			AND Scheme_Code IN (SELECT Scheme_Code FROM @VaccinationScheme)
			AND (
				ISNULL(Ext_Ref_Status, '') LIKE @Ext_Ref_Status_CN
					OR ISNULL(Ext_Ref_Status, '') LIKE @Ext_Ref_Status_UN
				)

--

	INSERT INTO @TransactionFail (
		Transaction_ID,
		Transaction_Dtm,
		Service_Receive_Dtm,
		Scheme_Code,
		Subsidize_Code,
		Subsidize_Code_Display,
		Available_Item_Code,
		Available_Item_Code_Display,
		SP_ID
	)
	SELECT
		VT.Transaction_ID,
		VT.Transaction_Dtm,
		VT.Service_Receive_Dtm,
		VT.Scheme_Code,
		TD.Subsidize_Code,
		CASE 
			WHEN SVM.Season IS NULL THEN S.Display_Code
			ELSE LTRIM(RTRIM(S.Display_Code)) + ' ' + SVM.Season
		END AS [Subsidize_Code_Display],
		TD.Available_Item_Code,
		CASE SID.Available_Item_Desc 
			WHEN 'Injection' THEN 'Only Dose'
			ELSE SID.Available_Item_Desc 
		END AS [Available_Item_Code_Display],
		VT.SP_ID
	FROM
		VoucherTransaction VT
			INNER JOIN TransactionDetail TD
				ON VT.Transaction_ID = TD.Transaction_ID
			INNER JOIN Subsidize S
				ON TD.Subsidize_Code = S.Subsidize_Code
					AND TD.Subsidize_Item_Code = S.Subsidize_Item_Code
			INNER JOIN SubsidizeItemDetails SID
				ON TD.Subsidize_Item_Code = SID.Subsidize_Item_Code
					AND TD.Available_Item_Code = SID.Available_Item_Code
			LEFT JOIN StatVaccineMapping SVM
				ON TD.Scheme_Code = SVM.Scheme_Code
					AND TD.Scheme_Seq = SVM.Scheme_Seq
					AND TD.Subsidize_Item_Code = SVM.Subsidize_Item_Code
	WHERE
		VT.Transaction_Dtm BETWEEN DATEADD(dd, -6, @Report_Dtm) AND DATEADD(dd, 1, @Report_Dtm)
			AND VT.Scheme_Code IN (SELECT Scheme_Code FROM @VaccinationScheme)
			AND (
				ISNULL(VT.Ext_Ref_Status, '') LIKE @Ext_Ref_Status_CN
					OR ISNULL(VT.Ext_Ref_Status, '') LIKE @Ext_Ref_Status_UN
				)


-- =============================================
-- Prepare result table
-- =============================================

-- ---------------------------------------------
-- Sheet 2: Summary of connection failed transactions
-- ---------------------------------------------
	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1, Result_Value2) VALUES (1, 'Reporting period: ' + CONVERT(varchar, @Report_Dtm, 111), '')
	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1, Result_Value2) VALUES (2, '', '')
	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1, Result_Value2) VALUES (11, 'Transaction Date', 'No. of vaccination transactions failed to connect CMS')
	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1, Result_Value2) VALUES (12, CONVERT(varchar, @Report_Dtm, 20), @VaccineTransactionFailCount)


-- ---------------------------------------------
-- Sheet 3: Raw data of connection failed transactions
-- ---------------------------------------------
	INSERT INTO @ResultTable3 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7) VALUES
	 (1, 'Reporting period: the week ending ' + CONVERT(varchar, @Report_Dtm, 111), '', '', '', '', '', '')

	INSERT INTO @ResultTable3 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7) VALUES
	 (2, '', '', '', '', '', '', '')

	INSERT INTO @ResultTable3 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7) VALUES
	 (10, 'Transaction No.', 'Transaction Time', 'Service Date', 'Scheme', 'Subsidy', 'Dose', 'SPID')

	INSERT INTO @ResultTable3 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7)
	SELECT
		10 + ROW_NUMBER() OVER (ORDER BY Transaction_Dtm DESC, Transaction_ID, Available_Item_Code_Display) AS [Result_Seq],
		LEFT(Transaction_ID, 7) + '-' + CONVERT(varchar, CONVERT(int, SUBSTRING(Transaction_ID, 8, 8))) + '-' + SUBSTRING(Transaction_ID, 16, 1),
		CONVERT(varchar, Transaction_Dtm, 20),
		CONVERT(varchar(10), Service_Receive_Dtm, 20),
		RTRIM(Scheme_Code),
		RTRIM(Subsidize_Code_Display),
		RTRIM(Available_Item_Code_Display),
		SP_ID
	FROM
		@TransactionFail
		

-- =============================================
-- Return
-- =============================================

-- ---------------------------------------------
-- Sheet 1: Content
-- ---------------------------------------------
	SELECT 'Report Generation Time: ' + CONVERT(varchar, GETDATE(), 111) + ' ' + CONVERT(varchar, GETDATE(), 108)


-- ---------------------------------------------
-- Sheet 2: Summary of connection failed transactions
-- ---------------------------------------------
	SELECT
		Result_Value1,
		Result_Value2
	FROM
		@ResultTable2
	ORDER BY
		Result_Seq
	

-- ---------------------------------------------
-- Sheet 3: Raw data of connection failed transactions
-- ---------------------------------------------
	SELECT
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6,
		Result_Value7
	FROM
		@ResultTable3
	ORDER BY
		Result_Seq


END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_D0017_ConnectionFailTransaction_Get] TO HCVU
GO
