IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0030_Recipient_Aberrant_Age]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0030_Recipient_Aberrant_Age]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM		
-- Modified date:	12 Sep 2019
-- CR No.			CRE19-006
-- Description:		Include HCVSDHC Transaction
-- =============================================
-- =============================================
-- Author:			Winnie SUEN
-- Create date:		12 Feb 2018
-- CR No.:			CRE16-014 to 016 (Voucher aberrant and new monitoring)
-- Description:		Report on Aberrant Pattern in Use of Vouchers - eHA who used vouchers at the targeted age 
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSD0030_Recipient_Aberrant_Age] 
	@request_dtm			DATETIME = null,		-- The reference date to get @target_period_from and @target_period_to. It's [Request_Dtm] from [FileGenerationQueue] Table (* Passed in from Excel Generator. When changing this field, plz also update the corresponding Excel Generator)
	@target_period_from		DATETIME = null,		-- The Target Period From. If defined, it will override the value from the @request_dtm
	@target_period_to		DATETIME = null		-- The Target Period To. If defined, it will override the value from the @request_dtm
AS BEGIN
-- =============================================
-- Declaration
-- =============================================

---- Test Data
--SET @target_period_from = '2019-11-16'
--SET @target_period_to = '2019-11-17'

	------------------------------------------------------------------------------------------
	-- Report Summary
	DECLARE @Report_ID				VARCHAR(10)
	DECLARE @TotalNumberOfRecord	INT
	
	------------------------------------------------------------------------------------------
	-- Additional Report Criteria
	DECLARE @target_recipient_age	INT -- The target age for voucher recipient who used vouchers
	
	------------------------------------------------------------------------------------------
	-- Report Helper Field
	DECLARE @CurrentAge					INT = 0
	DECLARE @No_Of_Elders				INT = 0

	DECLARE @MaxAge_HCVS				INT = 0
	DECLARE @Total_No_Of_Elders_HCVS	INT = 0
	DECLARE @Total_No_Of_Tx_HCVS		INT = 0
	DECLARE @Total_No_Of_SP_HCVS		INT = 0

	DECLARE @MaxAge_HCVSDHC				INT = 0
	DECLARE @Total_No_Of_Elders_HCVSDHC	INT = 0
	DECLARE @Total_No_Of_Tx_HCVSDHC		INT = 0
	DECLARE @Total_No_Of_SP_HCVSDHC		INT = 0

	DECLARE @MaxAge_HCVSCHN				INT = 0
	DECLARE @Total_No_Of_Elders_HCVSCHN	INT = 0
	DECLARE @Total_No_Of_Tx_HCVSCHN		INT = 0
	DECLARE @Total_No_Of_SP_HCVSCHN		INT = 0

	------------------------------------------------------------------------------------------
	CREATE TABLE #VoucherTransaction (
		Transaction_ID			CHAR(20),
		Scheme_Code				CHAR(10),
		Voucher_Acc_ID			CHAR(15),
		Temp_Voucher_Acc_ID		CHAR(15),
		Identity_No				VARBINARY(100),
		Age						SMALLINT,
		E_Name					VARBINARY(100),
		Gender					CHAR(1),		
		Amount					MONEY,
		Amount_RMB				MONEY,
		Co_Payment				NVARCHAR(255),
		Co_Payment_RMB			NVARCHAR(255),
		SP_ID					CHAR(8),
		Service_Type			CHAR(5),		
		Practice_Display_Seq	SMALLINT,		
		Transaction_Dtm			DATETIME,
		Service_Receive_Dtm		DATETIME,
		DHC_Service				CHAR(1)
	)

	------------------------------------------------------------------------------------------
	-- Result Table

	-- Create Worksheet 02 Result Table - 01-HCVS Summary
	DECLARE @WS02 AS TABLE(
		Result_Seq int identity(1,1),
		Result_Value1 VARCHAR(100)	DEFAULT '',
		Result_Value2 VARCHAR(100)	DEFAULT ''
	)

	-- Create Worksheet 03 Result Table - 02-HCVS Tx
	DECLARE @WS03 AS TABLE(
		Result_Seq int identity(1,1),
		Result_Value1 VARCHAR(100)	DEFAULT '',	
		Result_Value2 VARCHAR(100)	DEFAULT '',	
		Result_Value3 VARCHAR(100)	DEFAULT '',
		Result_Value4 VARCHAR(100)	DEFAULT '',	
		Result_Value5 VARCHAR(100)	DEFAULT '',	
		Result_Value6 VARCHAR(100)	DEFAULT '',	
		Result_Value7 VARCHAR(100)	DEFAULT '',	
		Result_Value8 VARCHAR(100)	DEFAULT '',	
		Result_Value9 VARCHAR(100)	DEFAULT '',	

		Result_Value10 VARCHAR(100)	DEFAULT '',	
		Result_Value11 VARCHAR(100)	DEFAULT '',	
		Result_Value12 NVARCHAR(100)	DEFAULT '',	
		Result_Value13 VARCHAR(100)	DEFAULT '',	
		Result_Value14 VARCHAR(100)	DEFAULT '',	
		Result_Value15 VARCHAR(100)	DEFAULT '',	
		Result_Value16 VARCHAR(200)	DEFAULT '',	
		Result_Value17 NVARCHAR(200)	DEFAULT '',	
		Result_Value18 VARCHAR(300)	DEFAULT '',	
		Result_Value19 NVARCHAR(300)	DEFAULT '',
		Result_Value20 VARCHAR(100)	DEFAULT ''
	)

	-- Create Worksheet 04 Result Table - 03-HCVSDHC Summary
	DECLARE @WS04 AS TABLE(
		Result_Seq int identity(1,1),
		Result_Value1 VARCHAR(100)	DEFAULT '',
		Result_Value2 VARCHAR(100)	DEFAULT ''
	)

	-- Create Worksheet 05 Result Table - 04-HCVSDHC Tx
	DECLARE @WS05 AS TABLE(
		Result_Seq int identity(1,1),
		Result_Value1 VARCHAR(100)	DEFAULT '',	
		Result_Value2 VARCHAR(100)	DEFAULT '',	
		Result_Value3 VARCHAR(100)	DEFAULT '',
		Result_Value4 VARCHAR(100)	DEFAULT '',	
		Result_Value5 VARCHAR(100)	DEFAULT '',	
		Result_Value6 VARCHAR(100)	DEFAULT '',	
		Result_Value7 VARCHAR(100)	DEFAULT '',	
		Result_Value8 VARCHAR(100)	DEFAULT '',	
		Result_Value9 VARCHAR(100)	DEFAULT '',	
		Result_Value10 VARCHAR(100)	DEFAULT '',	
		Result_Value11 NVARCHAR(100)	DEFAULT '',	
		Result_Value12 VARCHAR(100)	DEFAULT '',	
		Result_Value13 VARCHAR(100)	DEFAULT '',	
		Result_Value14 VARCHAR(100)	DEFAULT '',	
		Result_Value15 VARCHAR(200)	DEFAULT '',	
		Result_Value16 NVARCHAR(200)	DEFAULT '',	
		Result_Value17 VARCHAR(300)	DEFAULT '',	
		Result_Value18 NVARCHAR(300)	DEFAULT '',	
		Result_Value19 VARCHAR(100)	DEFAULT ''	
	)
	
	-- Create Worksheet 06 Result Table - 05-HCVSCHN Summary
	DECLARE @WS06 AS TABLE(
		Result_Seq int identity(1,1),	-- Sorting Sequence
		Result_Value1 VARCHAR(100)	DEFAULT '',
		Result_Value2 VARCHAR(100)	DEFAULT ''
	)

	-- Create Worksheet 07 Result Table - 06-HCVSCHN Tx
	DECLARE @WS07 AS TABLE(
		Result_Seq int identity(1,1),
		Result_Value1 VARCHAR(100)	DEFAULT '',	
		Result_Value2 VARCHAR(100)	DEFAULT '',	
		Result_Value3 VARCHAR(100)	DEFAULT '',
		Result_Value4 VARCHAR(100)	DEFAULT '',	
		Result_Value5 VARCHAR(100)	DEFAULT '',	
		Result_Value6 VARCHAR(100)	DEFAULT '',	
		Result_Value7 VARCHAR(100)	DEFAULT '',	
		Result_Value8 nVARCHAR(100)	DEFAULT '',	
		Result_Value9 VARCHAR(100)	DEFAULT '',	
		Result_Value10 nVARCHAR(100)	DEFAULT '',	
		Result_Value11 VARCHAR(100)	DEFAULT '',
		Result_Value12 NVARCHAR(100)	DEFAULT '',	
		Result_Value13 VARCHAR(100)	DEFAULT '',	
		Result_Value14 VARCHAR(100)	DEFAULT '',	
		Result_Value15 VARCHAR(100)	DEFAULT '',	
		Result_Value16 VARCHAR(200)	DEFAULT '',	
		Result_Value17 NVARCHAR(200)	DEFAULT '',	
		Result_Value18 VARCHAR(300)	DEFAULT '',	
		Result_Value19 NVARCHAR(300)	DEFAULT '',	
		Result_Value20 VARCHAR(100)	DEFAULT ''	
	)

	-- Worksheet: Remark
	DECLARE @Remark AS TABLE (
		Result_Seq int identity(1,1),	
		Result_Value1	VARCHAR(1000)	DEFAULT '',
		Result_Value2	VARCHAR(1000)	DEFAULT ''
	)
	
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	 
	SET @Report_ID = 'eHSD0030'
	 
	-- Report Criteria 
	SELECT	@target_recipient_age = CONVERT(int, p.Parm_Value1) 
	FROM		SystemParameters p
	WHERE	p.Parameter_Name = 'eHS(S)D0030_TargetRecipientAge'

	-- Init the Request_Dtm (Reference) DateTime to Avoid Null value
	IF @request_dtm is null
		SET @request_dtm = GETDATE()

	-- The Pass 1 day, ensure the time start from 00:00 (datetime compare logic use ">=")
	IF @target_period_from is null
		SET @target_period_from = CONVERT(datetime, CONVERT(VARCHAR(10), DATEADD(d, -1, @request_dtm), 105), 105)
	ELSE
		SET @target_period_from = CONVERT(datetime, CONVERT(VARCHAR(10), @target_period_from, 105), 105)

	-- The Pass 1 day, ensure the time start from 00:00 (datetime compare logic use "<", so get today's date)
	IF @target_period_to is null
		SET @target_period_to = CONVERT(datetime, CONVERT(VARCHAR(10), @request_dtm, 105), 105)
	ELSE
		SET @target_period_to = CONVERT(datetime, CONVERT(VARCHAR(10), @target_period_to, 105), 105)


-- =============================================
-- Retrieve data
-- =============================================

	INSERT INTO #VoucherTransaction (
		Transaction_ID,
		Scheme_Code,
		Voucher_Acc_ID,
		Temp_Voucher_Acc_ID,
		Identity_No,
		Age,
		E_Name,
		Gender,
		Amount,
		Amount_RMB,
		Co_Payment,
		Co_Payment_RMB,
		SP_ID,
		Service_Type,
		Practice_Display_Seq,
		Transaction_Dtm,
		Service_Receive_Dtm,
		DHC_Service
	)
	SELECT
		VT.Transaction_ID,
		VT.Scheme_Code,
		ISNULL(VT.Voucher_Acc_ID, ''),
		ISNULL(VT.Temp_Voucher_Acc_ID, ''),
		ISNULL(VP.Encrypt_Field1, TP.Encrypt_Field1) AS [Identity_No],
		DATEDIFF(yy, ISNULL(VP.DOB, TP.DOB) , Service_Receive_Dtm) AS [Age],
		ISNULL(VP.Encrypt_Field2, TP.Encrypt_Field2) AS [E_Name],
		ISNULL(VP.Sex, TP.Sex) AS [Gender],
		VT.Claim_Amount,
		TD.Total_Amount_RMB,
		TAF1.AdditionalFieldValueCode AS [Co_Payment],
		TAF2.AdditionalFieldValueCode AS [Co_Payment_RMB],
		VT.SP_ID,
		VT.Service_Type,
		VT.Practice_Display_Seq,
		VT.Transaction_Dtm,
		VT.Service_Receive_Dtm,
		VT.DHC_Service
	FROM
		VoucherTransaction VT WITH (NOLOCK)
			INNER JOIN TransactionDetail TD WITH (NOLOCK)
				ON VT.Transaction_ID = TD.Transaction_ID
			LEFT JOIN TransactionAdditionalField TAF1 WITH (NOLOCK)
				ON VT.Transaction_ID = TAF1.Transaction_ID
					AND TAF1.AdditionalFieldID = 'CoPaymentFee'
			LEFT JOIN TransactionAdditionalField TAF2 WITH (NOLOCK)
				ON VT.Transaction_ID = TAF2.Transaction_ID
					AND TAF2.AdditionalFieldID = 'CoPaymentFeeRMB'
			LEFT JOIN PersonalInformation VP WITH (NOLOCK)
				ON VT.Voucher_Acc_ID = VP.Voucher_Acc_ID AND VT.Voucher_Acc_ID <> ''
			LEFT JOIN TempPersonalInformation TP WITH (NOLOCK)
				ON VT.Temp_Voucher_Acc_ID = TP.Voucher_Acc_ID AND VT.Voucher_Acc_ID = ''
	WHERE
		VT.Scheme_Code IN ('HCVS', 'HCVSCHN', 'HCVSDHC')
			AND VT.Transaction_Dtm >= @target_period_from
			AND VT.Transaction_Dtm < @target_period_to
			AND DATEDIFF(yy, ISNULL(VP.DOB, TP.DOB), Service_Receive_Dtm) >= @target_recipient_age -- VR reach targeted age
		AND VT.Record_Status NOT IN    
			(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)     
			AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'     
			AND ((Effective_Date is null or Effective_Date <= Transaction_Dtm) AND (Expiry_Date is null or Expiry_Date >= Transaction_Dtm)))       
		AND (VT.Invalidation IS NULL OR VT.Invalidation NOT IN
			(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)
			AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'    
			AND ((Effective_Date is null or Effective_Date <= Transaction_Dtm) AND (Expiry_Date is null or Expiry_Date >= Transaction_Dtm))))


--
	DELETE #VoucherTransaction WHERE Age IS NULL
--

-- =============================================
-- Prepare Result Table
-- =============================================
	
-- ---------------------------------------------
-- For Excel Sheet (02): 01-HCVS Summary
-- ---------------------------------------------

	-- Header 1
	INSERT INTO @WS02 (Result_Value1) VALUES ('Transaction date: ' + CONVERT(VARCHAR(10), @target_period_from, 111))

	-- Report Parameter
	INSERT INTO @WS02 (Result_Value1) VALUES ('The target age for voucher recipient who used vouchers: ' + CONVERT(VARCHAR, @target_recipient_age))
	
	-- Line Break Before Data
	INSERT INTO @WS02 (Result_Value1) VALUES ('')


	SELECT
		@MaxAge_HCVS = MAX(Age),
		@Total_No_Of_Tx_HCVS = COUNT(Transaction_ID),
		@Total_No_Of_SP_HCVS = COUNT(Distinct SP_ID)
	FROM #VoucherTransaction 
	WHERE Scheme_Code = 'HCVS'

	-- Column header
	INSERT INTO @WS02 (Result_Value1, Result_Value2) VALUES ('Age', 'No. of elders')

	SET @CurrentAge = @target_recipient_age
	SET @No_Of_Elders = 0
		
	WHILE @CurrentAge <= @MaxAge_HCVS BEGIN
		
		SELECT 			
			@No_Of_Elders = COUNT(DISTINCT Identity_No)
		FROM
			#VoucherTransaction
		WHERE
			Scheme_Code = 'HCVS'
			AND Age = @CurrentAge	

		INSERT INTO @WS02 (Result_Value1, Result_Value2) 
		VALUES (@CurrentAge, @No_of_Elders)

		--
		SET @Total_No_Of_Elders_HCVS = @Total_No_Of_Elders_HCVS + @No_Of_Elders
		SET @CurrentAge = @CurrentAge + 1
	END

	INSERT INTO @WS02 (Result_Value1) VALUES ('')

	-- Total
	INSERT INTO @WS02 (Result_Value1, Result_Value2) VALUES ('Total no. of elders:', @Total_No_Of_Elders_HCVS)
	INSERT INTO @WS02 (Result_Value1, Result_Value2) VALUES ('Total no. of transactions:',  @Total_No_Of_Tx_HCVS)
	INSERT INTO @WS02 (Result_Value1, Result_Value2) VALUES ('Total no. of service providers involved:',  @Total_No_Of_SP_HCVS)

-- ---------------------------------------------
-- For Excel Sheet (03): 02-HCVS Tx
-- ---------------------------------------------
	-- Header 1
	INSERT INTO @WS03 (Result_Value1) VALUES ('Transaction date: ' + CONVERT(VARCHAR(10), @target_period_from, 111))

	-- Report Parameter
	INSERT INTO @WS03 (Result_Value1) VALUES ('The target age for voucher recipient who used vouchers: ' + CONVERT(VARCHAR, @target_recipient_age))
	
	-- Line Break Before Data
	INSERT INTO @WS03 (Result_Value1) VALUES ('')

	-- Column Header
	INSERT INTO @WS03 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20)
		VALUES ('S/N', 'Age (at the year of service date)', 'Recipient Name (In English)',  'Gender', 'eH(S)A ID / Reference No.', 'Transaction No.', 'Amount Claimed ($)', 'Service Date', 'DHC-Related Services', 'Net Service Fee Charged ($)', 'SP Name (In English)', 'SP Name (In Chinese)', 'SPID', 'Profession', 'Practice No.', 'Practice Name (In English)', 'Practice Name (In Chinese)', 'Practice Address (In English)', 'Practice Address (In Chinese)', 'Practice District')

	-- Report Content
	EXEC [proc_SymmetricKey_open]

	INSERT INTO @WS03 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20)
	SELECT
		ROW_NUMBER() OVER(ORDER BY VT.Age, VT.Transaction_ID) as [S/N],
		VT.Age,
		dbo.func_get_surname_n_initial(CONVERT(VARCHAR, DecryptByKey(VT.E_Name))) AS [Patient_Name_In_English],
		case VT.Gender			
				when 'F' then 'Female'
				when 'M' then 'Male' end as [Gender],
		CASE WHEN VT.Voucher_Acc_ID <> ''
			THEN dbo.func_format_voucher_account_number('V', VT.Voucher_Acc_ID)
			ELSE dbo.func_format_voucher_account_number('T', VT.Temp_Voucher_Acc_ID)
		END AS [eHA ID / Reference No.],
		dbo.func_format_system_number(VT.Transaction_ID) AS [Transaction_No],
		VT.Amount,
		CONVERT(VARCHAR(10), VT.Service_Receive_Dtm, 120) AS [Service_Date],
		[DHC_Service] = CASE WHEN VT.DHC_Service IS NULL THEN 'N' ELSE VT.DHC_Service END,
		ISNULL(VT.Co_Payment, '') AS [Co_Payment],
		CONVERT(VARCHAR, DecryptByKey(SP.Encrypt_Field2)) AS [SP_Name_In_English],
		CONVERT(nVARCHAR, DecryptByKey(SP.Encrypt_Field3)) AS [SP_Name_In_Chinese],
		VT.SP_ID,
		VT.Service_Type,
		PR.Display_Seq as [Practice No.],
		PR.Practice_Name AS [Practice_Name_In_English],
		ISNULL(PR.Practice_Name_Chi,'') AS [Practice_Name_In_Chinese],
		dbo.func_formatEngAddress(PR.Room, PR.Floor, PR.Block, PR.Building, PR.District) AS [Practice_Address_In_English],
		dbo.func_format_Address_Chi(PR.Room, PR.Floor, PR.Block, PR.Building_Chi, PR.District) AS [Practice_Address_In_Chinese],
		DIST.district_board
	FROM
		#VoucherTransaction VT
			INNER JOIN ServiceProvider SP
				ON VT.SP_ID = SP.SP_ID
			INNER JOIN Practice PR
				ON VT.SP_ID = PR.SP_ID
					AND VT.Practice_Display_Seq = PR.Display_Seq
			INNER JOIN District DIST
				ON PR.District = DIST.district_code
	WHERE
		Scheme_Code = 'HCVS'
	ORDER BY
		[S/N]

	EXEC [proc_SymmetricKey_close]

-- ---------------------------------------------
-- For Excel Sheet (04): 03-HCVS Summary
-- ---------------------------------------------

	-- Header 1
	INSERT INTO @WS04 (Result_Value1) VALUES ('Transaction date: ' + CONVERT(VARCHAR(10), @target_period_from, 111))

	-- Report Parameter
	INSERT INTO @WS04 (Result_Value1) VALUES ('The target age for voucher recipient who used vouchers: ' + CONVERT(VARCHAR, @target_recipient_age))
	
	-- Line Break Before Data
	INSERT INTO @WS04 (Result_Value1) VALUES ('')


	SELECT
		@MaxAge_HCVSDHC = MAX(Age),
		@Total_No_Of_Tx_HCVSDHC = COUNT(Transaction_ID),
		@Total_No_Of_SP_HCVSDHC = COUNT(Distinct SP_ID)
	FROM #VoucherTransaction 
	WHERE Scheme_Code = 'HCVSDHC'

	-- Column header
	INSERT INTO @WS04 (Result_Value1, Result_Value2) VALUES ('Age', 'No. of elders')

	SET @CurrentAge = @target_recipient_age
	SET @No_Of_Elders = 0
		
	WHILE @CurrentAge <= @MaxAge_HCVSDHC BEGIN
		
		SELECT 			
			@No_Of_Elders = COUNT(DISTINCT Identity_No)
		FROM
			#VoucherTransaction
		WHERE
			Scheme_Code = 'HCVSDHC'
			AND Age = @CurrentAge	

		INSERT INTO @WS04 (Result_Value1, Result_Value2) 
		VALUES (@CurrentAge, @No_of_Elders)

		--
		SET @Total_No_Of_Elders_HCVSDHC = @Total_No_Of_Elders_HCVSDHC + @No_Of_Elders
		SET @CurrentAge = @CurrentAge + 1
	END

	INSERT INTO @WS04 (Result_Value1) VALUES ('')

	-- Total
	INSERT INTO @WS04 (Result_Value1, Result_Value2) VALUES ('Total no. of elders:', @Total_No_Of_Elders_HCVSDHC)
	INSERT INTO @WS04 (Result_Value1, Result_Value2) VALUES ('Total no. of transactions:',  @Total_No_Of_Tx_HCVSDHC)
	INSERT INTO @WS04 (Result_Value1, Result_Value2) VALUES ('Total no. of service providers involved:',  @Total_No_Of_SP_HCVSDHC)

-- ---------------------------------------------
-- For Excel Sheet (05): 02-HCVS Tx
-- ---------------------------------------------
	-- Header 1
	INSERT INTO @WS05 (Result_Value1) VALUES ('Transaction date: ' + CONVERT(VARCHAR(10), @target_period_from, 111))

	-- Report Parameter
	INSERT INTO @WS05 (Result_Value1) VALUES ('The target age for voucher recipient who used vouchers: ' + CONVERT(VARCHAR, @target_recipient_age))
	
	-- Line Break Before Data
	INSERT INTO @WS05 (Result_Value1) VALUES ('')

	-- Column Header
	INSERT INTO @WS05 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19)
		VALUES ('S/N', 'Age (at the year of service date)', 'Recipient Name (In English)',  'Gender', 'eH(S)A ID / Reference No.', 'Transaction No.', 'Amount Claimed ($)', 'Service Date', 'Net Service Fee Charged ($)', 'SP Name (In English)', 'SP Name (In Chinese)', 'SPID', 'Profession', 'Practice No.', 'Practice Name (In English)', 'Practice Name (In Chinese)', 'Practice Address (In English)', 'Practice Address (In Chinese)', 'Practice District')

	-- Report Content
	EXEC [proc_SymmetricKey_open]

	INSERT INTO @WS05 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19)
	SELECT
		ROW_NUMBER() OVER(ORDER BY VT.Age, VT.Transaction_ID) as [S/N],
		VT.Age,
		dbo.func_get_surname_n_initial(CONVERT(VARCHAR, DecryptByKey(VT.E_Name))) AS [Patient_Name_In_English],
		case VT.Gender			
				when 'F' then 'Female'
				when 'M' then 'Male' end as [Gender],
		CASE WHEN VT.Voucher_Acc_ID <> ''
			THEN dbo.func_format_voucher_account_number('V', VT.Voucher_Acc_ID)
			ELSE dbo.func_format_voucher_account_number('T', VT.Temp_Voucher_Acc_ID)
		END AS [eHA ID / Reference No.],
		dbo.func_format_system_number(VT.Transaction_ID) AS [Transaction_No],
		VT.Amount,
		CONVERT(VARCHAR(10), VT.Service_Receive_Dtm, 120) AS [Service_Date],
		ISNULL(VT.Co_Payment, '') AS [Co_Payment],
		CONVERT(VARCHAR, DecryptByKey(SP.Encrypt_Field2)) AS [SP_Name_In_English],
		CONVERT(NVARCHAR, DecryptByKey(SP.Encrypt_Field3)) AS [SP_Name_In_Chinese],
		VT.SP_ID,
		VT.Service_Type,
		PR.Display_Seq as [Practice No.],
		PR.Practice_Name AS [Practice_Name_In_English],
		ISNULL(PR.Practice_Name_Chi,'') AS [Practice_Name_In_Chinese],
		dbo.func_formatEngAddress(PR.Room, PR.Floor, PR.Block, PR.Building, PR.District) AS [Practice_Address_In_English],
		dbo.func_format_Address_Chi(PR.Room, PR.Floor, PR.Block, PR.Building_Chi, PR.District) AS [Practice_Address_In_Chinese],
		DIST.district_board
	FROM
		#VoucherTransaction VT
			INNER JOIN ServiceProvider SP
				ON VT.SP_ID = SP.SP_ID
			INNER JOIN Practice PR
				ON VT.SP_ID = PR.SP_ID
					AND VT.Practice_Display_Seq = PR.Display_Seq
			INNER JOIN District DIST
				ON PR.District = DIST.district_code
	WHERE
		Scheme_Code = 'HCVSDHC'
	ORDER BY
		[S/N]

	EXEC [proc_SymmetricKey_close]

-- ---------------------------------------------
-- For Excel Sheet (06): 05-HCVSCHN Summary
-- ---------------------------------------------

	-- Header 1
	INSERT INTO @WS06 (Result_Value1) VALUES ('Transaction date: ' + CONVERT(VARCHAR(10), @target_period_from, 111))

	-- Report Parameter
	INSERT INTO @WS06 (Result_Value1) VALUES ('The target age for voucher recipient who used vouchers: ' + CONVERT(VARCHAR, @target_recipient_age))
	
	-- Line Break Before Data
	INSERT INTO @WS06 (Result_Value1) VALUES ('')


	SELECT
		@MaxAge_HCVSCHN = MAX(Age),
		@Total_No_Of_Tx_HCVSCHN = COUNT(Transaction_ID),
		@Total_No_Of_SP_HCVSCHN = COUNT(Distinct SP_ID)
	FROM #VoucherTransaction 
	WHERE Scheme_Code = 'HCVSCHN'

	-- Column header
	INSERT INTO @WS06 (Result_Value1, Result_Value2) VALUES ('Age', 'No. of elders')

	SET @CurrentAge = @target_recipient_age
	SET @No_Of_Elders = 0

	WHILE @CurrentAge <= @MaxAge_HCVSCHN BEGIN
		
		SELECT 			
			@No_Of_Elders = COUNT(DISTINCT Identity_No)
		FROM
			#VoucherTransaction
		WHERE
			Scheme_Code = 'HCVSCHN'
			AND Age = @CurrentAge	

		INSERT INTO @WS06 (Result_Value1, Result_Value2) 
		VALUES (@CurrentAge, @No_of_Elders)

		--
		SET @Total_No_Of_Elders_HCVSCHN = @Total_No_Of_Elders_HCVSCHN + @No_Of_Elders
		SET @CurrentAge = @CurrentAge + 1
	END

	INSERT INTO @WS06 (Result_Value1) VALUES ('')

	-- Total
	INSERT INTO @WS06 (Result_Value1, Result_Value2) VALUES ('Total no. of elders:', @Total_No_Of_Elders_HCVSCHN)
	INSERT INTO @WS06 (Result_Value1, Result_Value2) VALUES ('Total no. of transactions:',  @Total_No_Of_Tx_HCVSCHN)
	INSERT INTO @WS06 (Result_Value1, Result_Value2) VALUES ('Total no. of service providers involved:',  @Total_No_Of_SP_HCVSCHN)
	
-- ---------------------------------------------
-- For Excel Sheet (07): 06-HCVSCHN Tx
-- ---------------------------------------------
	-- Header 1
	INSERT INTO @WS07 (Result_Value1) VALUES ('Transaction date: ' + CONVERT(VARCHAR(10), @target_period_from, 111))

	-- Report Parameter
	INSERT INTO @WS07 (Result_Value1) VALUES ('The target age for voucher recipient who used vouchers: ' + CONVERT(VARCHAR, @target_recipient_age))
	
	-- Line Break Before Data
	INSERT INTO @WS07 (Result_Value1) VALUES ('')

	-- Column Header
	INSERT INTO @WS07 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20)
		VALUES ('S/N', 'Age (at the year of service date)', 'Recipient Name (In English)',  'Gender', 'eH(S)A ID / Reference No.', 'Transaction No.', 'Amount Claimed ($)', N'Voucher Amount Claimed (¥)', 'Service Date', N'Net Service Fee Charged (¥)', 'SP Name (In English)', 'SP Name (In Chinese)', 'SPID', 'Profession', 'Practice No.', 'Practice Name (In English)', 'Practice Name (In Chinese)', 'Practice Address (In English)', 'Practice Address (In Chinese)', 'Practice District')

	-- Report Content
	EXEC [proc_SymmetricKey_open]

	INSERT INTO @WS07 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20)
	SELECT
		ROW_NUMBER() OVER(ORDER BY VT.Age, VT.Transaction_ID) as [S/N],
		VT.Age,
		dbo.func_get_surname_n_initial(CONVERT(VARCHAR, DecryptByKey(VT.E_Name))) AS [Patient_Name_In_English],
		case VT.Gender			
				when 'F' then 'Female'
				when 'M' then 'Male' end as [Gender],
		CASE WHEN VT.Voucher_Acc_ID <> ''
			THEN dbo.func_format_voucher_account_number('V', VT.Voucher_Acc_ID)
			ELSE dbo.func_format_voucher_account_number('T', VT.Temp_Voucher_Acc_ID)
		END AS [eHA ID / Reference No.],
		dbo.func_format_system_number(VT.Transaction_ID) AS [Transaction_No],
		VT.Amount,
		VT.Amount_RMB,
		CONVERT(VARCHAR(10), VT.Service_Receive_Dtm, 120) AS [Service_Date],
		ISNULL(VT.Co_Payment_RMB, '') AS [Co_Payment],
		CONVERT(VARCHAR, DecryptByKey(SP.Encrypt_Field2)) AS [SP_Name_In_English],
		CONVERT(nVARCHAR, DecryptByKey(SP.Encrypt_Field3)) AS [SP_Name_In_Chinese],
		VT.SP_ID,
		VT.Service_Type,
		PR.Display_Seq as [Practice No.],
		PR.Practice_Name AS [Practice_Name_In_English],
		ISNULL(PR.Practice_Name_Chi,'') AS [Practice_Name_In_Chinese],
		dbo.func_formatEngAddress(PR.Room, PR.Floor, PR.Block, PR.Building, PR.District) AS [Practice_Address_In_English],
		dbo.func_format_Address_Chi(PR.Room, PR.Floor, PR.Block, PR.Building_Chi, PR.District) AS [Practice_Address_In_Chinese],
		DIST.district_board
	FROM
		#VoucherTransaction VT
			INNER JOIN ServiceProvider SP
				ON VT.SP_ID = SP.SP_ID
			INNER JOIN Practice PR
				ON VT.SP_ID = PR.SP_ID
					AND VT.Practice_Display_Seq = PR.Display_Seq
			INNER JOIN District DIST
				ON PR.District = DIST.district_code
	WHERE
		Scheme_Code = 'HCVSCHN'
	ORDER BY
		[S/N]

	EXEC [proc_SymmetricKey_close]


-- ---------------------------------------------
-- For Excel Sheet (06): Remark
-- ---------------------------------------------

	INSERT INTO @Remark (Result_Value1, Result_Value2)
	VALUES ('(A) Legend', '')

	INSERT INTO @Remark (Result_Value1, Result_Value2)
	VALUES ('1. Profession Type' , '')    
	
	INSERT INTO @Remark (Result_Value1, Result_Value2)    
	SELECT	rtrim(Service_Category_Code) as Service_Category_Code,
			rtrim(Service_Category_Desc) as Service_Category_Desc
	FROM Profession
	ORDER BY Service_Category_Code

	INSERT INTO @Remark (Result_Value1, Result_Value2) VALUES ('', '')

	INSERT INTO @Remark (Result_Value1, Result_Value2)
	VALUES ('(B) Common Note(s) for the report', '')

	INSERT INTO @Remark (Result_Value1, Result_Value2)
	VALUES ('1. eHealth (Subsidies) Account:', '')
	
	INSERT INTO @Remark (Result_Value1, Result_Value2)
	VALUES ('   a. Age =  year of service date - year of DOB', '')

	INSERT INTO @Remark (Result_Value1, Result_Value2)
	VALUES ('   b. Recipients with same document no. and age is counted as one recipient in sub report 01, 03 and 05', '')

	INSERT INTO @Remark (Result_Value1, Result_Value2)
	VALUES ('      (e.g. 2 eH(S)A with same document no. but with birth date of ''3 Jun 1901'' and ''5 Dec 1901'' will be counted as 1,', '')

	INSERT INTO @Remark (Result_Value1, Result_Value2)
	VALUES ('              2 eH(S)A with same document no. but with birth date of ''3 Jun 1901'' and ''1902'' will be counted as 2) ', '')

	INSERT INTO @Remark (Result_Value1, Result_Value2)
	VALUES ('   c. If a recipient with transactions in both schemes, the recipient is counted in both schemes', '')

	INSERT INTO @Remark (Result_Value1, Result_Value2) VALUES ('', '')

	INSERT INTO @Remark (Result_Value1, Result_Value2)
	VALUES ('2. Voucher Transaction:', '')
	
	INSERT INTO @Remark (Result_Value1, Result_Value2)
	VALUES ('   a. All claim transactions created under service providers (either created by back office users or service providers (or the delegated users))', '')

	INSERT INTO @Remark (Result_Value1, Result_Value2)
	VALUES ('   b. Exclude those reimbursed transactions with invalidation status marked as Invalidated', '')
		
	INSERT INTO @Remark (Result_Value1, Result_Value2)
	VALUES ('   c. Exclude voided/removed transactions', '')

	INSERT INTO @Remark (Result_Value1, Result_Value2)
	VALUES ('   d. All transactions are filtered by transaction date regardless of the service date', '')


-- =============================================
-- Return result
-- =============================================

-- ---------------------------------------------
-- Report Summary	
-- ---------------------------------------------
	
	SELECT @TotalNumberOfRecord = COUNT(*) FROM #VoucherTransaction

	-- Report Parameter
	SELECT	CASE WHEN ISNULL(@TotalNumberOfRecord, 0) > 0 THEN 'Y' ELSE 'N' END AS 'HaveResult',
					CONVERT(VARCHAR(11), @target_period_from, 106) AS 'Date',
					@target_recipient_age AS 'TargetRecipientAge'

-- ---------------------------------------------
-- To Excel Sheet (01): Content
-- ---------------------------------------------

	SELECT 'Report Generation Time: ' + CONVERT(VARCHAR(10), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(5), GETDATE(), 108)

-- ---------------------------------------------
-- To Excel Sheet (02): 01-HCVS Summary
-- ---------------------------------------------

	SELECT Result_Value1, Result_Value2 
	FROM @WS02 
	ORDER BY Result_Seq

-- ---------------------------------------------
-- To Excel Sheet (03): 02-HCVS Tx
-- ---------------------------------------------

	SELECT	Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, 
			Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, 
			Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, 
			Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20
	FROM @WS03 
	ORDER BY Result_Seq

-- ---------------------------------------------
-- To Excel Sheet (04): 03-HCVSDHC Summary
-- ---------------------------------------------

	SELECT Result_Value1, Result_Value2 
	FROM @WS04 
	ORDER BY Result_Seq

-- ---------------------------------------------
-- To Excel Sheet (05): 04-HCVSDHC Tx
-- ---------------------------------------------

	SELECT	Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, 
			Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, 
			Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, 
			Result_Value16, Result_Value17, Result_Value18, Result_Value19
	FROM @WS05
	ORDER BY Result_Seq

-- ---------------------------------------------
-- To Excel Sheet (06): 05-HCVSCHN Summary
-- ---------------------------------------------

	SELECT Result_Value1, Result_Value2 
	FROM @WS06
	ORDER BY Result_Seq

-- ---------------------------------------------
-- To Excel Sheet (07): 06-HCVSCHN Tx
-- ---------------------------------------------

	SELECT	Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, 
			Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, 
			Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, 
			Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20
	FROM @WS07 
	ORDER BY Result_Seq

-- ---------------------------------------------
-- To Excel Sheet (08): Remark
-- ---------------------------------------------
	
	SELECT Result_Value1, Result_Value2 
	FROM @Remark 
	ORDER BY Result_Seq


-- =============================================
-- House Keeping
-- =============================================
	DROP TABLE #VoucherTransaction

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0030_Recipient_Aberrant_Age] TO HCVU
GO

