IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SP_Aberrant_Voucher_Usage]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SP_Aberrant_Voucher_Usage]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM		
-- Modified date:	12 Sep 2019
-- CR No.			CRE19-006
-- Description:		Without HCVSDHC Transaction
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	10 November 2015
-- CR No.:			CRE15-006
-- Description:		Rename of eHS
-- =============================================
-- =============================================
-- Author:				Vincent YUEN
-- Create date:		29 Jan 2010
-- Description:		AR5 - Report on Aberrant Pattern in Use of Vouchers - HCSP perspective
-- =============================================
-- =============================================
-- Modification History
-- Modified by:			
-- Modified date:			
-- Description:			
-- =============================================

CREATE PROCEDURE [dbo].[proc_SP_Aberrant_Voucher_Usage] 
	@request_dtm			DATETIME = null,		-- The reference date to get @target_period_from and @target_period_to. It's [Request_Dtm] from [FileGenerationQueue] Table (* Passed in from Excel Generator. When changing this field, plz also update the corresponding Excel Generator)
	@target_period_from	DATETIME = null,		-- The Target Period From. If defined, it will override the value from the @request_dtm
	@target_period_to		DATETIME = null,		-- The Target Period To. If defined, it will override the value from the @request_dtm
	@is_debug bit = 0
AS BEGIN
-- =============================================
-- Declaration
-- =============================================

---- Test Data
--SET @target_period_from = '2019-11-16'
--SET @target_period_to = '2019-11-17'

	------------------------------------------------------------------------------------------
	-- Report Summary
	DECLARE @SummaryTotalSPValue int
	DECLARE @SummaryTotalRecordValue int

	------------------------------------------------------------------------------------------
	-- Additional Report Criteria
	DECLARE @target_transaction_count int 

	------------------------------------------------------------------------------------------
	-- Temp Table for SP's Transaction Count >= Criteria
	DECLARE @SPTransactionCount AS table (
		SP_ID char(8),
		TransactionCount int
	)

	------------------------------------------------------------------------------------------
	-- Target Transaction Table
	CREATE TABLE #VoucherTransaction (
		Transaction_ID char(20) collate database_default,
		Transaction_Dtm datetime,
		Service_Receive_Dtm datetime,
		Voucher_Acc_ID char(15) collate database_default,
		Temp_Voucher_Acc_ID char(15) collate database_default,
		Special_Acc_ID char(15) collate database_default,
		Invalid_Acc_ID char(15) collate database_default,
		SP_ID char(8) collate database_default,
		SP_Name varchar(50) collate database_default,
		Practice_Display_Seq smallint,
		Practice_Name varchar(100) collate database_default,
		Practice_Profession varchar(20) collate database_default,
		Practice_District varchar(50) collate database_default,
		Reason_of_Visit_L1 varchar(200) collate database_default,
		Reason_of_Visit_L2 varchar(200) collate database_default,
		No_Voucher_Claimed int
	)

	------------------------------------------------------------------------------------------
	-- Result Table
	CREATE TABLE #TempResultTable (
		Result_Seq int identity(1,1),	-- Sorting Sequence
		Result_Value1 varchar(200),	-- SPID Column
		Result_Value2 varchar(100),	-- HCSP Name Column
		Result_Value3 varchar(100),	-- HCSP Practice ID
		Result_Value4 varchar(100),	-- HCSP Practice Name Column
		Result_Value5 varchar(100),	-- HCSP Profession Column
		Result_Value6 varchar(100),	-- Practice District Column
		Result_Value7 varchar(100),	-- eHealth Account ID / Ref# Column 
		Result_Value8 varchar(100),	-- eHealth Account Document Type Column
		Result_Value9 varchar(100),	-- eHealth Account Document Number (Masked) Column
		Result_Value10 varchar(100),	-- eHealth Account Name Column
		Result_Value11 varchar(100),	-- eHealth Account Gender Column
		Result_Value12 varchar(100),	-- eHealth Account DOB Column
		Result_Value13 varchar(100),	-- eHealth Account DOB Flag Column
		Result_Value14 varchar(100),	-- Transaction Number Column
		Result_Value15 varchar(100),	-- Date of Voucher Claim Column
		Result_Value16 varchar(100),	-- Time of Voucher Claim Column
		Result_Value17 varchar(200),	-- Service Date Column
		Result_Value18 varchar(100),	-- Reason For Visit Level 1 Column
		Result_Value19 varchar(100),	-- Reason For Visit Level 2 Column
		Result_Value20 varchar(100),	-- Number of Vouchers Claimed Column		
		Is_Data	bit
	)
	
	------------------------------------------------------------------------------------------
	-- Summary Result Table
	DECLARE @TempSummaryResultTable AS TABLE (
		Result_Seq int identity(1,1), 
		Result_Value1 varchar(200),
		Result_Value2 varchar(50)
	)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

	------------------------------------------------------------------------------------------
	-- Report Criteria
	SELECT	@target_transaction_count = CONVERT(int, p.Parm_Value1) 
	FROM		SystemParameters p
	WHERE	p.Parameter_Name = 'eHS(S)D0016_TargetNoOfTransaction'

	-- Init the Request_Dtm (Reference) DateTime to Avoid Null value
	IF @request_dtm is null
		SET @request_dtm = GETDATE()

	-- The Pass 1 day, ensure the time start from 00:00 (datetime compare logic use ">=")
	IF @target_period_from is null
		SET @target_period_from = CONVERT(datetime, CONVERT(varchar(10), DATEADD(d, -1, @request_dtm), 105), 105)
	ELSE
		SET @target_period_from = CONVERT(datetime, CONVERT(varchar(10), @target_period_from, 105), 105)

	-- The Pass 1 day, ensure the time start from 00:00 (datetime compare logic use "<", so get today's date)
	IF @target_period_to is null
		SET @target_period_to = CONVERT(datetime, CONVERT(varchar(10), @request_dtm, 105), 105)
	ELSE
		SET @target_period_to = CONVERT(datetime, CONVERT(varchar(10), @target_period_to, 105), 105)

	------------------------------------------------------------------------------------------
	-- Prepare SP Transaction Count Table (Ignore "Voided" transaction)
	INSERT INTO @SPTransactionCount (SP_ID, TransactionCount)
		SELECT		SP_ID,
							COUNT(SP_ID)
		FROM			VoucherTransaction
		WHERE		Transaction_Dtm >= @target_period_from
							AND Transaction_Dtm < @target_period_to
							AND Record_Status IN ('P', 'V', 'A', 'R', 'S')
							AND Scheme_Code = 'HCVS'
							AND (DHC_Service IS NULL OR DHC_Service = 'N')
		GROUP BY	SP_ID
		HAVING		COUNT(SP_ID) >= @target_transaction_count

	SET @SummaryTotalSPValue = @@rowcount

	------------------------------------------------------------------------------------------
	-- Prepare Transaction inforamtion Table (without Personal information)
	OPEN SYMMETRIC KEY sym_Key 
		DECRYPTION BY ASYMMETRIC KEY asym_Key

	INSERT INTO #VoucherTransaction (Transaction_ID, Transaction_Dtm, Service_Receive_Dtm, Voucher_Acc_ID, Temp_Voucher_Acc_ID, Special_Acc_ID, Invalid_Acc_ID, SP_ID, SP_Name, Practice_Display_Seq, Practice_Name, Practice_Profession, Practice_District, Reason_of_Visit_L1, Reason_of_Visit_L2, No_Voucher_Claimed)
		SELECT		VT.Transaction_ID,
							VT.Transaction_Dtm,
							VT.Service_Receive_Dtm,
							VT.Voucher_Acc_ID,
							VT.Temp_Voucher_Acc_ID,
							VT.Special_Acc_ID,
							VT.Invalid_Acc_ID,
							SPCount.SP_ID,
							CONVERT(varchar, DecryptByKey(SP.Encrypt_Field2)),
							VT.Practice_Display_Seq,
							P.Practice_Name,
							Prof.Service_Category_Code,
							D.District_Name,				
							RV1.Reason_L1,
							RV2.Reason_L2,
							TD.Unit
		FROM			@SPTransactionCount SPCount
		INNER JOIN	VoucherTransaction VT ON	SPCount.SP_ID = VT.SP_ID
																	AND VT.Transaction_Dtm >= @target_period_from
																	AND VT.Transaction_Dtm < @target_period_to
																	AND VT.Record_Status IN ('P', 'V', 'A', 'R', 'S')
																	AND VT.Scheme_Code = 'HCVS'
																	AND (VT.DHC_Service IS NULL OR VT.DHC_Service = 'N')
		INNER JOIN	TransactionDetail TD ON VT.Transaction_ID = TD.Transaction_ID
		LEFT JOIN		ServiceProvider SP ON SPCount.SP_ID = SP.SP_ID
		LEFT JOIN		Practice P ON SP.SP_ID = P.SP_ID
													AND VT.Practice_Display_Seq = P.Display_Seq
		LEFT JOIN		Professional Prof ON P.SP_ID = Prof.SP_ID 
															AND P.Professional_Seq = Prof.Professional_Seq
		LEFT JOIN		District D ON P.District = D.district_code
		LEFT JOIN		TransactionAdditionalField TAF1 ON VT.Transaction_ID = TAF1.Transaction_ID
																				AND TAF1.AdditionalFieldID = 'Reason_for_Visit_L1'
		LEFT JOIN		TransactionAdditionalField TAF2 ON VT.Transaction_ID = TAF2.Transaction_ID
																				AND TAF2.AdditionalFieldID = 'Reason_for_Visit_L2'
		LEFT JOIN		ReasonForVisitL1 RV1 ON Prof.Service_Category_Code = RV1.Professional_Code 
																	AND TAF1.AdditionalFieldValueCode = RV1.Reason_L1_Code
		LEFT JOIN		ReasonForVisitL2 RV2 ON Prof.Service_Category_Code = RV2.Professional_Code 
																	AND TAF1.AdditionalFieldValueCode = RV2.Reason_L1_Code
																	AND TAF2.AdditionalFieldValueCode = RV2.Reason_L2_Code

	------------------------------------------------------------------------------------------
	IF @is_debug = 1
	BEGIN
		SELECT	'' as 'debug:', 
						@target_transaction_count as '@target_transaction_count', 
						@target_period_from as '@target_period_from', 
						@target_period_to as '@target_period_to'

		SELECT	'' as 'debug:', 
						*
		FROM		@SPTransactionCount

		SELECT	'' as 'debug:', 
						*
		FROM		#VoucherTransaction
	END
	------------------------------------------------------------------------------------------

	------------------------------------------------------------------------------------------
	-- Prepare Result Table
	-- Header 1
	INSERT INTO #TempResultTable (Result_Value1) VALUES ('Reporting period: ' + CONVERT(varchar(10), @target_period_from, 111))

	-- Report Parameter
	INSERT INTO #TempResultTable (Result_Value1) VALUES ('The target number of transactions to monitor: ' + CONVERT(VARCHAR, @target_transaction_count))

	-- Line Break Before Data
	INSERT INTO #TempResultTable (Result_Value1) VALUES ('')

	-- Column Header
	INSERT INTO #TempResultTable (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20)
		VALUES ('Service Provider ID', 'Service Provider Name', 'Practice No.', 'Practice Name', 'Health Profession', 'Practice District', 'eHealth (Subsidies) Account ID / Reference No.', 'eHealth (Subsidies) Account Identity Document Type', 'eHealth (Subsidies) Account Identity Document No.', 'eHealth (Subsidies) Account Name', 'eHealth (Subsidies) Account Gender', 'eHealth (Subsidies) Account DOB', 'eHealth (Subsidies) Account DOB Flag', 'Transaction No.', 'Transaction Date', 'Transaction Time', 'Service Date', 'Reason for Visit (Level 1)', 'Reason for Visit (Level 2)' , 'No. of Units Redeemed')

	-- Report Content
	INSERT INTO #TempResultTable (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20, Is_Data)
		SELECT		SP_ID,
							dbo.func_get_surname_n_initial(SP_Name) AS SP_Name,
							Practice_Display_Seq,
							Practice_Name,
							Practice_Profession,
							Practice_District,
							CASE	WHEN [IPI].Invalid_Acc_ID IS NOT NULL			THEN dbo.func_format_voucher_account_number('I', [IPI].Invalid_Acc_ID) 
										WHEN [SPI].Special_Acc_ID IS NOT NULL		THEN dbo.func_format_voucher_account_number('S', [SPI].Special_Acc_ID) 
										WHEN [PI].Voucher_Acc_ID IS NOT NULL		THEN dbo.func_format_voucher_account_number('V', [PI].Voucher_Acc_ID)
										WHEN [TPI].Voucher_Acc_ID IS NOT NULL		THEN dbo.func_format_voucher_account_number('T', [TPI].Voucher_Acc_ID)
							END AS ehs_account_id,
							CASE	WHEN [IPI].Invalid_Acc_ID IS NOT NULL			THEN [IPI].Doc_Code
										WHEN [SPI].Special_Acc_ID IS NOT NULL		THEN [SPI].Doc_Code
										WHEN [PI].Voucher_Acc_ID IS NOT NULL		THEN [PI].Doc_Code
										WHEN [TPI].Voucher_Acc_ID IS NOT NULL		THEN [TPI].Doc_Code
							END AS doc_code,
							CASE	WHEN [IPI].Invalid_Acc_ID IS NOT NULL			THEN dbo.func_mask_doc_id([IPI].Doc_Code, CONVERT(varchar, DecryptByKey([IPI].Encrypt_Field1)), CONVERT(varchar, DecryptByKey([IPI].Encrypt_Field11)))
										WHEN [SPI].Special_Acc_ID IS NOT NULL		THEN dbo.func_mask_doc_id([SPI].Doc_Code, CONVERT(varchar, DecryptByKey([SPI].Encrypt_Field1)), CONVERT(varchar, DecryptByKey([SPI].Encrypt_Field11)))
										WHEN [PI].Voucher_Acc_ID IS NOT NULL		THEN dbo.func_mask_doc_id([PI].Doc_Code, CONVERT(varchar, DecryptByKey([PI].Encrypt_Field1)), CONVERT(varchar, DecryptByKey([PI].Encrypt_Field11)))
										WHEN [TPI].Voucher_Acc_ID IS NOT NULL		THEN dbo.func_mask_doc_id([TPI].Doc_Code, CONVERT(varchar, DecryptByKey([TPI].Encrypt_Field1)), CONVERT(varchar, DecryptByKey([TPI].Encrypt_Field11)))
							END AS doc_identity_id,
							CASE	WHEN [IPI].Invalid_Acc_ID IS NOT NULL			THEN dbo.func_get_surname_n_initial(CONVERT(varchar, DecryptByKey([IPI].Encrypt_Field2)))
										WHEN [SPI].Special_Acc_ID IS NOT NULL		THEN dbo.func_get_surname_n_initial(CONVERT(varchar, DecryptByKey([SPI].Encrypt_Field2)))
										WHEN [PI].Voucher_Acc_ID IS NOT NULL		THEN dbo.func_get_surname_n_initial(CONVERT(varchar, DecryptByKey([PI].Encrypt_Field2)))
										WHEN [TPI].Voucher_Acc_ID IS NOT NULL		THEN dbo.func_get_surname_n_initial(CONVERT(varchar, DecryptByKey([TPI].Encrypt_Field2)))
							END AS ehs_account_eng_name,
							CASE	WHEN [IPI].Invalid_Acc_ID IS NOT NULL			THEN [IPI].Sex
										WHEN [SPI].Special_Acc_ID IS NOT NULL		THEN [SPI].Sex
										WHEN [PI].Voucher_Acc_ID IS NOT NULL		THEN [PI].Sex
										WHEN [TPI].Voucher_Acc_ID IS NOT NULL		THEN [TPI].Sex
							END AS Sex,
							--	CASE	WHEN [IPI].Invalid_Acc_ID IS NOT NULL			THEN dbo.func_format_DOB([IPI].DOB, [IPI].Exact_DOB, 'en-us', [IPI].EC_Age, [IPI].EC_Date_of_Registration)
							--				WHEN [SPI].Special_Acc_ID IS NOT NULL		THEN dbo.func_format_DOB([SPI].DOB, [SPI].Exact_DOB, 'en-us', [SPI].EC_Age, [SPI].EC_Date_of_Registration)
							--				WHEN [PI].Voucher_Acc_ID IS NOT NULL		THEN dbo.func_format_DOB([PI].DOB, [PI].Exact_DOB, 'en-us', [PI].EC_Age, [PI].EC_Date_of_Registration)
							--				WHEN [TPI].Voucher_Acc_ID IS NOT NULL		THEN dbo.func_format_DOB([TPI].DOB, [TPI].Exact_DOB, 'en-us', [TPI].EC_Age, [TPI].EC_Date_of_Registration)
							--	END AS DOB,
							CASE	WHEN [IPI].Invalid_Acc_ID IS NOT NULL			THEN CONVERT(varchar(10), [IPI].DOB, 111)
										WHEN [SPI].Special_Acc_ID IS NOT NULL		THEN CONVERT(varchar(10), [SPI].DOB, 111)
										WHEN [PI].Voucher_Acc_ID IS NOT NULL		THEN CONVERT(varchar(10), [PI].DOB, 111)
										WHEN [TPI].Voucher_Acc_ID IS NOT NULL		THEN CONVERT(varchar(10), [TPI].DOB, 111)
							END AS DOB,
							CASE	WHEN [IPI].Invalid_Acc_ID IS NOT NULL			THEN [IPI].Exact_DOB
										WHEN [SPI].Special_Acc_ID IS NOT NULL		THEN [SPI].Exact_DOB
										WHEN [PI].Voucher_Acc_ID IS NOT NULL		THEN [PI].Exact_DOB
										WHEN [TPI].Voucher_Acc_ID IS NOT NULL		THEN [TPI].Exact_DOB
							END AS Exact_DOB,
							dbo.func_format_system_number(VT.Transaction_ID) AS Transaction_ID,
							CONVERT(varchar(10), VT.Transaction_Dtm, 111) AS Transaction_Date,
							CONVERT(varchar(10), VT.Transaction_Dtm, 108) AS Transaction_Time,
							CONVERT(varchar(10), VT.Service_Receive_Dtm, 111) AS Service_Date,
							VT.Reason_of_Visit_L1,
							VT.Reason_of_Visit_L2,
							VT.No_Voucher_Claimed,
							1
				FROM				#VoucherTransaction VT
				LEFT JOIN			InvalidPersonalInformation [IPI] ON VT.Invalid_Acc_ID = [IPI].Invalid_Acc_ID
				LEFT JOIN			SpecialPersonalInformation [SPI] ON VT.Special_Acc_ID = [SPI].Special_Acc_ID
				LEFT JOIN			PersonalInformation [PI] ON VT.Voucher_Acc_ID = [PI].Voucher_Acc_ID
				LEFT JOIN			TempPersonalInformation [TPI] ON VT.Temp_Voucher_Acc_ID = [TPI].Voucher_Acc_ID
		ORDER BY SP_ID, Practice_Display_Seq, transaction_date, transaction_time

	SET @SummaryTotalRecordValue = @@rowcount

	CLOSE SYMMETRIC KEY sym_Key


	-- Report Summary
	INSERT INTO @TempSummaryResultTable (Result_Value1) VALUES ('Summary:')
	INSERT INTO @TempSummaryResultTable (Result_Value1, Result_Value2) VALUES ('Total Number of HCSP: ',  ISNULL(@SummaryTotalSPValue, 0))
	INSERT INTO @TempSummaryResultTable (Result_Value1, Result_Value2) VALUES ('Total Number of Record: ', ISNULL(@SummaryTotalRecordValue, 0))

-- =============================================
-- Return results
-- =============================================


	-- Report Parameter
	SELECT	CASE WHEN ISNULL(@SummaryTotalRecordValue, 0) > 0 THEN 'Y' ELSE 'N' END AS 'HaveResult',
					CONVERT(varchar(11), @target_period_from, 106) AS 'Date',
					@target_transaction_count AS 'TargetTransactionCount'

	-- Result Set 1: Table of Content 
	SELECT		'Report Generation Time: ' + CONVERT(varchar, GETDATE(), 111) + ' ' + CONVERT(VARCHAR(5), GETDATE(), 108) AS Result_Value

	-- Result Set 2: Summary
	SELECT		Result_Value1,
						Result_Value2
	FROM			@TempSummaryResultTable
	ORDER BY Result_Seq

	-- Result Set 3: Record Detail
	SELECT		Result_Value1,
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
						Result_Value20
	FROM			#TempResultTable
	ORDER BY	Result_Seq
	

	DROP TABLE #VoucherTransaction
	DROP TABLE #TempResultTable

END
GO

GRANT EXECUTE ON [dbo].[proc_SP_Aberrant_Voucher_Usage] TO HCVU
GO

