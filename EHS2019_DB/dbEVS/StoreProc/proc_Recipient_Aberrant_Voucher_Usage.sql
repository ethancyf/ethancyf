IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Recipient_Aberrant_Voucher_Usage]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Recipient_Aberrant_Voucher_Usage]
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
-- Modified by:		Chris YIM
-- Modified date:	12 Mar 2019
-- CR No.:			CRE18-018 (Enhance aberrant report eHSD0015)
-- Description:		Remove criteria for the Aberrant Pattern **F case
--					- By the same enrolled service provider on the different practice
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	7 Feb 2018
-- CR No.:			CRE16-014 to 016 (Voucher aberrant and new monitoring)
-- Description:		New criteria for the Aberrant Pattern *S case
--					- By the same enrolled service provider on the same service date
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	10 November 2015
-- CR No.:			CRE15-006
-- Description:		Rename of eHS
-- =============================================
-- =============================================
-- Modification History
-- CR No.:		  CRE13-019-02
-- Modified by:	 Karl LAM
-- Modified date: 03 Jul 2015
-- Description:	  Add isnull for encrypt_Field11 while eHA retrieval
-- =============================================
-- =============================================
-- Modification History
-- CR No.:		  CRE13-019-02
-- Modified by:	  Winnie SUEN
-- Modified date: 14 Apr 2015
-- Description:	  New Case *C - Claim in different voucher schemes within short period
--				  All scheme under EHCVS Subsidize will be counted
-- =============================================
-- =============================================
-- Author:			Vincent YUEN
-- Create date:		2 FEB 2010
-- Description:		AR4 - Report on Aberrant Pattern in Use of Vouchers - voucher recipient perspective
-- =============================================

CREATE PROCEDURE [dbo].[proc_Recipient_Aberrant_Voucher_Usage] 
	@request_dtm			DATETIME = null,		-- The reference date to get @target_period_from and @target_period_to. It's [Request_Dtm] from [FileGenerationQueue] Table (* Passed in from Excel Generator. When changing this field, plz also update the corresponding Excel Generator)
	@target_period_from		DATETIME = null,		-- The Target Period From. If defined, it will override the value from the @request_dtm
	@target_period_to		DATETIME = null,		-- The Target Period To. If defined, it will override the value from the @request_dtm
	@is_debug bit = 0
AS BEGIN
-- =============================================
-- Declaration
-- =============================================

---- Test Data
--SET @target_period_from = '2019-11-21'
--SET @target_period_to = '2019-11-22'

	------------------------------------------------------------------------------------------
	-- Report Summary
	DECLARE @TotalNumberOfRecord int
	DECLARE	@TotalNumberOfRecipientWithS int
	DECLARE @TotalNumberOfRecipientWithC int
	DECLARE @TotalNumberOfRecipientWithSnC int

	------------------------------------------------------------------------------------------
	-- Additional Report Criteria
	DECLARE @target_transaction_count int						-- The target number of transaction for the multiple Transactions within Short Period case
	DECLARE @target_transaction_period_ori int				-- The target period (minute) for the multiple transactions within short period case
	DECLARE @target_transaction_period int					-- The target period (second) for the multiple transactions within short period case
	DECLARE @target_multiple_voucher_scheme_claim_period_ori int	-- The target period (minute) for the Claim in different voucher schemes within short period
	DECLARE @target_multiple_voucher_scheme_claim_period int		-- The target period (second) for the Claim in different voucher schemes within short period

	------------------------------------------------------------------------------------------
	-- Report Helper Field
	DECLARE @max_aberrant_group int
	DECLARE @current_aberrant_group int
	DECLARE @current_doc_code varchar(20)
	DECLARE @current_doc_identity_id varchar(20)
	DECLARE @current_adoption_prefix varchar(20)

	DECLARE @max_transaction_id int
	DECLARE @current_transaction_id int
	DECLARE @target_transaction_id int

	------------------------------------------------------------------------------------------
	-- Temp Table for Recipient's Transaction Count >= 2
	CREATE TABLE #TempTargetRecipient (
		aberrant_group int identity(1,1)
		, doc_code varchar(20) collate database_default
		, doc_identity_id varchar(20) collate database_default
		, adoption_prefix varchar(20) collate database_default
	)

	------------------------------------------------------------------------------------------
	-- Temp Table for All Recipient's Transaction (Voucher)
	DECLARE @TempRecipientTransaction AS TABLE (
		id int
		, transaction_id char(20)
		, transaction_dtm datetime
		, sp_id char(8)
		, practice_display_seq smallint
		, time_diff	int		-- Transaction Time Diff from Record 1 to Record 2
		, Scheme_Code CHAR (10)
		, Service_Dtm	DATETIME -- CRE16-014 to 016
		, DHC_Service	CHAR(1)
	)

	------------------------------------------------------------------------------------------
	-- Temp Table for All Recipient's Transaction (HCVS only)
	DECLARE @TempRecipientTransactionHCVS AS TABLE(
		id int
		, transaction_id char(20)
		, transaction_dtm datetime
		, sp_id char(8)
		, practice_display_seq smallint
		, time_diff	int		-- Transaction Time Diff from Record 1 to Record 2
		, Service_Dtm	DATETIME -- CRE16-014 to 016
	)


	------------------------------------------------------------------------------------------
	-- Temp Table for All Recipient's Transaction (HCVS only)
	DECLARE @TempRecipientTransactionForCaseS AS TABLE(
		id int
		, transaction_id char(20)
		, time_diff	int		-- Transaction Time Diff from Record 1 to Record 2
	)
	
	------------------------------------------------------------------------------------------
	-- Temp Table for All Recipient's Transaction
	DECLARE @TempRecipientTransactionForCaseC AS TABLE(
		id int
		, transaction_id char(20)
		, time_diff	int		-- Transaction Time Diff from Record 1 to Record 2
	)
	------------------------------------------------------------------------------------------
	-- Temp Table for Transaction treated as aberrant usage
	CREATE TABLE #TempAberrantTransaction (
		transaction_id char(20) collate database_default
		, aberrant_group int
		, aberrant_pattern char(1)		--  'S': Multiple transactions within short period; 'F': Voucher recipient moving fast
	)

	------------------------------------------------------------------------------------------
	-- Temp Table for Distincted Aberrant Transaction Group by Aberrant Group
	CREATE TABLE #TempDistinctAberrantTransaction (
		aberrant_group_display int
		, transaction_id char(20) collate database_default	
		, aberrant_group int
		, aberrant_pattern char(1)
	)

	------------------------------------------------------------------------------------------
	-- Result Table
	CREATE TABLE #TempResultTable (
		Result_Seq int identity(1,1),	-- Sorting Sequence
		Result_Value1 varchar(200),	-- Aberrant Group Column
		Result_Value2 varchar(100),	-- eHealth Account ID / Reference Number Column
		Result_Value3 varchar(100),	-- eHealth Account Document Type Column
		Result_Value4 varchar(100),	-- eHealth Account Document Number Column
		Result_Value5 varchar(100),	-- eHealth Account Name (Surname & Initial) Column
		Result_Value6 varchar(100),	-- eHealth Account Gender Column
		Result_Value7 varchar(100),	-- eHealth Account DOB Column 
		Result_Value8 varchar(100),	-- eHealth Account DOB Flag Column
		Result_Value9 varchar(100),	-- Transaction Number Column
		Result_Value10 varchar(100),	-- Date of Voucher Claim (Transaction Date; Format: yyyy/mm/dd) Column
		Result_Value11 varchar(100),	-- Time of Voucher Claim (Transaction Time; Format: HH:mm:ss) Column
		Result_Value12 varchar(100),	-- Service Date Column
		Result_Value13 varchar(100),	-- Scheme Code Column (added at 14/4/2015)
		Result_Value14 varchar(100),	-- SPID Column
		Result_Value15 varchar(100),	-- HCSP Name  Column
		Result_Value16 varchar(100),	-- HCSP Practice ID Column
		Result_Value17 varchar(100),	-- HCSP Practice Column
		Result_Value18 varchar(200),	-- HCSP Profession Column
		Result_Value19 varchar(100),	-- Practice District Column
		Result_Value20 varchar(200),	-- Reason of Visit Level 1
		Result_Value21 varchar(100),	-- Reason of Visit Level 2
		Result_Value22 varchar(100),	-- Number of Vouchers Claimed Column
		Result_Value23 varchar(100),	-- Aberrant Pattern Multiple transactions within short period Column
		Result_Value24 varchar(100),	-- Aberrant Pattern Voucher recipient moving fast Column
		Result_Value25 varchar(100),	-- Aberrant Pattern Claim in different voucher schemes within short period Column (added at 14/4/2015)
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
	-- Report Criteria (magnify the unit to SECOND)
	SELECT	@target_transaction_count = CONVERT(int, p.Parm_Value1) 
	FROM	SystemParameters p
	WHERE	p.Parameter_Name = 'eHS(S)D0015_TargetNoOfTransaction'

	SELECT	@target_transaction_period_ori = CONVERT(int, p.Parm_Value1),
			@target_transaction_period = CONVERT(int, p.Parm_Value1) * 60
	FROM	SystemParameters p
	WHERE	p.Parameter_Name = 'eHS(S)D0015_TargetNoOfTransactionPeriod'

	SELECT	@target_multiple_voucher_scheme_claim_period_ori = CONVERT(int, p.Parm_Value1),
			@target_multiple_voucher_scheme_claim_period = CONVERT(int, p.Parm_Value1) * 60
	FROM	SystemParameters p
	WHERE	p.Parameter_Name = 'eHS(S)D0015_TargetMultipleVoucherSchemeClaimPeriod'

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


	OPEN SYMMETRIC KEY sym_Key 
		DECRYPTION BY ASYMMETRIC KEY asym_Key

	------------------------------------------------------------------------------------------
	-- Prepare Table for the Target Recipient (Group by Document Type, Document Id as a Aberrant Group, document type "HKBC" is treated as "HKIC")
	INSERT INTO #TempTargetRecipient (doc_code, doc_identity_id, adoption_prefix)
		SELECT		
			doc_code,
			doc_identity_id,
			adoption_prefix
		FROM			
			(SELECT		
				[doc_code] = CASE	
					WHEN [SPI].Special_Acc_ID IS NOT NULL AND [SPI].Doc_Code = 'HKBC'	THEN 'HKIC' 
					WHEN [SPI].Special_Acc_ID IS NOT NULL								THEN [SPI].Doc_Code
					WHEN [PI].Voucher_Acc_ID IS NOT NULL AND [PI].Doc_Code = 'HKBC'		THEN 'HKIC' 
					WHEN [PI].Voucher_Acc_ID IS NOT NULL								THEN [PI].Doc_Code
					WHEN [TPI].Voucher_Acc_ID IS NOT NULL AND [TPI].Doc_Code = 'HKBC'	THEN 'HKIC' 
					WHEN [TPI].Voucher_Acc_ID IS NOT NULL								THEN [TPI].Doc_Code
					END,
				[doc_identity_id] =	CASE	
					WHEN [SPI].Special_Acc_ID IS NOT NULL		THEN CONVERT(varchar, DecryptByKey([SPI].Encrypt_Field1))
					WHEN [PI].Voucher_Acc_ID IS NOT NULL		THEN CONVERT(varchar, DecryptByKey([PI].Encrypt_Field1))
					WHEN [TPI].Voucher_Acc_ID IS NOT NULL		THEN CONVERT(varchar, DecryptByKey([TPI].Encrypt_Field1))
					END,
				[adoption_prefix] =	CASE	
					WHEN [SPI].Special_Acc_ID IS NOT NULL		THEN CONVERT(varchar, DecryptByKey([SPI].Encrypt_Field11))
					WHEN [PI].Voucher_Acc_ID IS NOT NULL		THEN CONVERT(varchar, DecryptByKey([PI].Encrypt_Field11))
					WHEN [TPI].Voucher_Acc_ID IS NOT NULL		THEN CONVERT(varchar, DecryptByKey([TPI].Encrypt_Field11))
					END
			FROM			
				VoucherTransaction VT
					INNER JOIN TransactionDetail TD 
						ON VT.Transaction_ID = TD.Transaction_ID 
					LEFT JOIN		
						SpecialPersonalInformation [SPI] 
							ON VT.Special_Acc_ID = [SPI].Special_Acc_ID
					LEFT JOIN		
						PersonalInformation [PI] 
							ON VT.Voucher_Acc_ID = [PI].Voucher_Acc_ID
					LEFT JOIN		
						TempPersonalInformation [TPI] 
							ON VT.Temp_Voucher_Acc_ID = [TPI].Voucher_Acc_ID
							AND ISNULL(VT.Voucher_Acc_ID, '') = ''
							AND ISNULL(VT.Special_Acc_ID, '') = ''
							AND ISNULL(VT.Invalid_Acc_ID, '') = ''
			WHERE		
				Transaction_Dtm >= @target_period_from
				AND Transaction_Dtm < @target_period_to
				AND TD.Subsidize_Code = 'EHCVS'
				AND VT.Record_Status IN ('P', 'V', 'A', 'R')
			) AS MatchedPersonalInfo
		GROUP BY	
			doc_code, doc_identity_id, adoption_prefix
		HAVING		
			COUNT(*) > 1
		ORDER BY	
			doc_code, doc_identity_id, adoption_prefix

	------------------------------------------------------------------------------------------
	-- Prepare Table for the Target Transaction w/ Aberrant Pattern
	-- For Each Aberrant Recipient, Extract their HCVS & HCVSC Transaction and Check if the Transaction fulfill the Report Criteria
	SELECT 
		@max_aberrant_group = MAX(aberrant_group)
	FROM		
		#TempTargetRecipient

	SET @current_aberrant_group = 1
	WHILE (@current_aberrant_group <= @max_aberrant_group)
	BEGIN
		SELECT	@current_doc_code = doc_code
						, @current_doc_identity_id = doc_identity_id
						, @current_adoption_prefix = adoption_prefix
		FROM		#TempTargetRecipient
		WHERE	aberrant_group = @current_aberrant_group

		------------------------------------------------------------------------------------------
		-- Retrieve Recipient's Transaction (Voucher)
		------------------------------------------------------------------------------------------
		DELETE @TempRecipientTransaction

		INSERT INTO @TempRecipientTransaction (id, transaction_id, transaction_dtm, sp_id, practice_display_seq, Scheme_Code, Service_Dtm, DHC_Service)
			SELECT		
				ROW_NUMBER() OVER (ORDER BY VT.transaction_dtm) AS id,
				VT.transaction_id, 
				VT.transaction_dtm, 
				VT.sp_id, 
				VT.practice_display_seq,
				VT.Scheme_Code,
				VT.Service_Receive_Dtm,
				VT.DHC_Service
			FROM			
				VoucherTransaction VT
					INNER JOIN TransactionDetail TD 
						ON VT.Transaction_ID = TD.Transaction_ID 
					LEFT JOIN SpecialPersonalInformation [SPI] 
						ON VT.Special_Acc_ID = [SPI].Special_Acc_ID
					LEFT JOIN PersonalInformation [PI] 
						ON VT.Voucher_Acc_ID = [PI].Voucher_Acc_ID
					LEFT JOIN TempPersonalInformation [TPI] 
						ON VT.Temp_Voucher_Acc_ID = [TPI].Voucher_Acc_ID
						AND ISNULL(VT.Voucher_Acc_ID, '') = ''
						AND ISNULL(VT.Special_Acc_ID, '') = ''
						AND ISNULL(VT.Invalid_Acc_ID, '') = ''
			WHERE		
				Transaction_Dtm >= @target_period_from
				AND Transaction_Dtm < @target_period_to
				AND TD.Subsidize_Code = 'EHCVS'
				AND VT.Record_Status IN ('P', 'V', 'A', 'R')
				-- Special Account
				AND (
						[SPI].Special_Acc_ID IS NULL 
						OR 
						(
							([SPI].Doc_Code = @current_doc_code OR ([SPI].Doc_Code = 'HKBC' AND @current_doc_code = 'HKIC'))
							AND CONVERT(varchar, DecryptByKey([SPI].Encrypt_Field1)) = @current_doc_identity_id
							AND ISNULL(CONVERT(varchar, DecryptByKey([SPI].Encrypt_Field11)),'') =  ISNULL(@current_adoption_prefix ,'')
						) 
					)
				-- Validated Account
				AND ([PI].Voucher_Acc_ID IS NULL 
						OR 
						(
							([PI].Doc_Code = @current_doc_code OR ([PI].Doc_Code = 'HKBC' AND @current_doc_code = 'HKIC'))
							AND CONVERT(varchar, DecryptByKey([PI].Encrypt_Field1)) = @current_doc_identity_id
							AND  ISNULL(CONVERT(varchar, DecryptByKey([PI].Encrypt_Field11)),'') =  ISNULL(@current_adoption_prefix ,'')
						) 
					)
				-- Temporary Account
				AND ([TPI].Voucher_Acc_ID IS NULL 
						OR 
						(
							([TPI].Doc_Code = @current_doc_code OR ([TPI].Doc_Code = 'HKBC' AND @current_doc_code = 'HKIC'))
							AND CONVERT(varchar, DecryptByKey([TPI].Encrypt_Field1)) = @current_doc_identity_id
							AND  ISNULL(CONVERT(varchar, DecryptByKey([TPI].Encrypt_Field11)),'') =  ISNULL(@current_adoption_prefix ,'')
						) 
					)
			ORDER BY transaction_dtm ASC

		------------------------------------------------------------------------------------------
		-- Set the Time Different between two transactions (Voucher)
		------------------------------------------------------------------------------------------
		UPDATE		
			T1
		SET			
			T1.time_diff = DATEDIFF(second, T2.transaction_dtm, T1.transaction_dtm)
		FROM		
			@TempRecipientTransaction T1
				INNER JOIN @TempRecipientTransaction T2 
					ON T1.id -1= T2.id

		UPDATE		
			@TempRecipientTransaction
		SET			
			time_diff = 0
		WHERE		
			time_diff IS NULL

		------------------------------------------------------------------------------------------
		-- Retrieve Recipient's Transaction (HCVS Only)
		------------------------------------------------------------------------------------------
		DELETE @TempRecipientTransactionHCVS

		INSERT INTO @TempRecipientTransactionHCVS (id, transaction_id, transaction_dtm, sp_id, practice_display_seq, Service_Dtm)
			SELECT		
				ROW_NUMBER() OVER (ORDER BY transaction_dtm) AS id,
				transaction_id, 
				transaction_dtm, 
				sp_id, 
				practice_display_seq,
				Service_Dtm
			FROM		
				@TempRecipientTransaction 
			WHERE		
				Scheme_Code = 'HCVS'
				AND (DHC_Service IS NULL OR DHC_Service = 'N')
			ORDER BY 
				transaction_dtm ASC

		------------------------------------------------------------------------------------------
		-- Set the Time Different between two transactions (HCVS Only)
		------------------------------------------------------------------------------------------
		UPDATE		
			T1
		SET			
			T1.time_diff = DATEDIFF(second, T2.transaction_dtm, T1.transaction_dtm)
		FROM		
			@TempRecipientTransactionHCVS T1
				INNER JOIN @TempRecipientTransactionHCVS T2 
					ON T1.id -1= T2.id

		UPDATE		
			@TempRecipientTransactionHCVS
		SET			
			time_diff = 0
		WHERE		
			time_diff IS NULL
															
		------------------------------------------------------------------------------------------
		-- Handle Multiple transactions within short period case (HCVS only)
		-- Windowing the transactions by the Target Transaction Count, check the sum of the Time Different fulfill the Target Transaction Period
		-- Transactions made by SAME Service Provider on SAME service date (Add criteria CRE16-014 to 016)
		------------------------------------------------------------------------------------------
		IF @target_transaction_count > 0
		BEGIN
			-- Not to handle target transaction count = 0's case
			SELECT	@max_transaction_id = MAX(id)
			FROM	@TempRecipientTransactionHCVS

			SET @current_transaction_id = 1
			WHILE (@current_transaction_id <= @max_transaction_id)
			BEGIN
				
				DELETE @TempRecipientTransactionForCaseS

				--Find the transaction id with same SP and same Service Date within the period
				INSERT INTO @TempRecipientTransactionForCaseS (id, transaction_id, time_diff)
				SELECT	TNext.id, 
						TNext.transaction_id, 
						DATEDIFF(second, TCurrent.transaction_dtm, TNext.transaction_dtm)
				FROM		@TempRecipientTransactionHCVS TCurrent
				JOIN		@TempRecipientTransactionHCVS TNext ON TNext.id >= TCurrent.id
				WHERE		TCurrent.id = @current_transaction_id
							AND TCurrent.sp_id = TNext.sp_id
							AND TCurrent.Service_Dtm = TNext.Service_Dtm
							AND DATEDIFF(second, TCurrent.transaction_dtm, TNext.transaction_dtm) <= @target_transaction_period
												
				IF @@rowcount >= @target_transaction_count
				BEGIN
					INSERT INTO #TempAberrantTransaction (transaction_id, aberrant_group, aberrant_pattern)
					SELECT		
						transaction_id
						,@current_aberrant_group
						,'S'
					FROM
						@TempRecipientTransactionForCaseS TS
				END

				SET @current_transaction_id = @current_transaction_id + 1
			END
			DELETE @TempRecipientTransactionForCaseS

		END
		
		------------------------------------------------------------------------------------------
		-- Handle Claim in different voucher schemes within short period case, count Voucher scheme (HCVS/HCVSDHC vs HCVSCHN)
		-- Windowing the transactions by the Target Transaction Count, check the sum of the Time Different fulfill the Target Transaction Period
		------------------------------------------------------------------------------------------
		IF @target_transaction_count > 0
		BEGIN
			-- Not to handle target transaction count = 0's case
			SELECT	@max_transaction_id = MAX(id)
			FROM		@TempRecipientTransaction

			
			SET @current_transaction_id = 1
			WHILE (@current_transaction_id <= @max_transaction_id)
			BEGIN		
				
				--Find the largest transaction id with different scheme claim within the period
				SET @target_transaction_id = 
				(SELECT TOP 1 TNext.id							
				FROM		@TempRecipientTransaction TCurrent
				JOIN		@TempRecipientTransaction TNext ON TNext.id > TCurrent.id
				WHERE		TCurrent.id = @current_transaction_id
							AND (	(TCurrent.Scheme_Code = 'HCVS' AND TNext.Scheme_Code = 'HCVSCHN')
								OR	(TCurrent.Scheme_Code = 'HCVSDHC' AND TNext.Scheme_Code = 'HCVSCHN')
								OR	(TCurrent.Scheme_Code = 'HCVSCHN' AND TNext.Scheme_Code IN ('HCVS','HCVSDHC'))
								)
							AND DATEDIFF(second, TCurrent.transaction_dtm, TNext.transaction_dtm) <= @target_multiple_voucher_scheme_claim_period
				ORDER BY	TNext.id DESC)
				
				DELETE @TempRecipientTransactionForCaseC
				
				IF @target_transaction_id IS NOT NULL
				BEGIN
					INSERT INTO @TempRecipientTransactionForCaseC (id, transaction_id, time_diff)
						SELECT	T.id
								,T.transaction_id
								,T.time_diff
						FROM	@TempRecipientTransaction T
						WHERE	T.id >= @current_transaction_id
								AND T.id <= @target_transaction_id
										
					INSERT INTO #TempAberrantTransaction (transaction_id, aberrant_group, aberrant_pattern)
						SELECT	transaction_id
								,@current_aberrant_group
								,'C'
						FROM	@TempRecipientTransactionForCaseC TC
				END
				
				SET @current_transaction_id = @current_transaction_id + 1
			END
			DELETE @TempRecipientTransactionForCaseC

		END
		
		SET @current_aberrant_group = @current_aberrant_group + 1
	END -- End of While
		
	------------------------------------------------------------------------------------------
	-- Remove Duplicate Record in #TempAberrantTransaction
	------------------------------------------------------------------------------------------
	INSERT INTO	#TempDistinctAberrantTransaction (aberrant_group_display, transaction_id, aberrant_group, aberrant_pattern)
		SELECT			
			AberrantGroupDisplay.aberrant_group_display
			,DistinctSource.transaction_id
			,DistinctSource.aberrant_group
			,DistinctSource.aberrant_pattern
		FROM				
			(SELECT 
				DISTINCT transaction_id
				,aberrant_group
				,aberrant_pattern
			FROM						
				#TempAberrantTransaction) AS DistinctSource
				INNER JOIN		
					(SELECT 
						DISTINCT ROW_NUMBER() OVER(ORDER BY aberrant_group) AS aberrant_group_display
						,aberrant_group
					FROM					
						#TempAberrantTransaction
					GROUP BY			
						aberrant_group) AS AberrantGroupDisplay
					ON DistinctSource.aberrant_group = AberrantGroupDisplay.aberrant_group

	------------------------------------------------------------------------------------------
	-- Set Summary Field
	------------------------------------------------------------------------------------------
	--S
	SELECT		
		@TotalNumberOfRecipientWithS = COUNT(DISTINCT ATS.aberrant_group)
	FROM			
		#TempDistinctAberrantTransaction ATS
			LEFT JOIN #TempDistinctAberrantTransaction ATC 
				ON ATS.aberrant_group = ATC.aberrant_group
				AND ATC.aberrant_pattern = 'C'
	WHERE		
		ATS.aberrant_pattern = 'S'
		AND ATC.aberrant_group IS NULL
								
	--C
	SELECT		
		@TotalNumberOfRecipientWithC = COUNT(DISTINCT ATC.aberrant_group)
	FROM			
		#TempDistinctAberrantTransaction ATC
			LEFT JOIN #TempDistinctAberrantTransaction ATS 
				ON ATC.aberrant_group = ATS.aberrant_group
				AND ATS.aberrant_pattern = 'S'
	WHERE		
		ATC.aberrant_pattern = 'C'
		AND ATS.aberrant_group IS NULL			

	-- S + C
	SELECT		
		@TotalNumberOfRecipientWithSnC = COUNT(DISTINCT ATS.aberrant_group)
	FROM			
		#TempDistinctAberrantTransaction ATS
			INNER JOIN #TempDistinctAberrantTransaction ATC 
				ON ATS.aberrant_group = ATC.aberrant_group
				AND ATC.aberrant_pattern = 'C'
	WHERE		
		ATS.aberrant_pattern = 'S'

	------------------------------------------------------------------------------------------
	IF @is_debug = 1
	BEGIN
		SELECT	
			'' as 'debug:', 
			@target_multiple_voucher_scheme_claim_period as '@target_multiple_voucher_scheme_claim_period', 
			@target_transaction_count as '@target_transaction_count', 
			@target_transaction_period as '@target_transaction_period',
			@target_period_from as '@target_period_from', 
			@target_period_to as '@target_period_to'

		SELECT	'' as 'debug:', * FROM #TempTargetRecipient

		SELECT	'' as 'debug:', * FROM	#TempDistinctAberrantTransaction
									
	END
	------------------------------------------------------------------------------------------


	------------------------------------------------------------------------------------------
	-- Prepare Result Table
	-- Header 1
	INSERT INTO #TempResultTable (Result_Value1) VALUES ('Reporting period: ' + CONVERT(varchar(10), @target_period_from, 111))

	-- Report Parameter
	INSERT INTO #TempResultTable (Result_Value1) VALUES ('The target number of transaction for the Aberrant Pattern *S case: ' + CONVERT(VARCHAR, @target_transaction_count))
	INSERT INTO #TempResultTable (Result_Value1) VALUES ('The target period (minute) for the Aberrant Pattern *S case: ' + CONVERT(VARCHAR, @target_transaction_period_ori))
	INSERT INTO #TempResultTable (Result_Value1) VALUES ('The target period (minute) for the Aberrant Pattern *C case: ' + CONVERT(VARCHAR, @target_multiple_voucher_scheme_claim_period_ori))

	-- Line Break Before Data
	INSERT INTO #TempResultTable (Result_Value1) VALUES ('')

	-- Column Header
	INSERT INTO #TempResultTable (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20, Result_Value21, Result_Value22, Result_Value23, Result_Value24)
		VALUES ('Aberrant Group', 'eHealth (Subsidies) Account ID / Reference No.', 'eHealth (Subsidies) Account Identity Document Type', 'eHealth (Subsidies) Account Identity Document No.', 'eHealth (Subsidies) Account Name', 'eHealth (Subsidies) Account Gender', 'eHealth (Subsidies) Account DOB', 'eHealth (Subsidies) Account DOB Flag', 'Transaction No.', 'Transaction Date', 'Transaction Time', 'Service Date', 'Scheme Code', 'Service Provider ID', 'Service Provider Name', 'Practice No.', 'Practice Name', 'Health Profession', 'Practice District', 'Reason for Visit (Level 1)', 'Reason for Visit (Level 2)', 'No. of Units Redeemed', 'Aberrant Pattern *S', 'Aberrant Pattern *C')

	-- Report Content
	INSERT INTO #TempResultTable (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20, Result_Value21, Result_Value22, Result_Value23, Result_Value24, Is_Data)
		SELECT		
			[Aberrant_id] = CASE 
				WHEN ATS.aberrant_group_display IS NOT NULL	THEN ATS.aberrant_group_display
				WHEN ATF.aberrant_group_display IS NOT NULL	THEN ATF.aberrant_group_display
				WHEN ATC.aberrant_group_display IS NOT NULL	THEN ATC.aberrant_group_display
				END,
			[ehs_account_id] = CASE	
				WHEN [SPI].Special_Acc_ID IS NOT NULL		THEN dbo.func_format_voucher_account_number('S', [SPI].Special_Acc_ID) 
				WHEN [PI].Voucher_Acc_ID IS NOT NULL		THEN dbo.func_format_voucher_account_number('V', [PI].Voucher_Acc_ID)
				WHEN [TPI].Voucher_Acc_ID IS NOT NULL		THEN dbo.func_format_voucher_account_number('T', [TPI].Voucher_Acc_ID)
				END,
			[doc_code] = CASE	
				WHEN [SPI].Special_Acc_ID IS NOT NULL		THEN [SPI].Doc_Code
				WHEN [PI].Voucher_Acc_ID IS NOT NULL		THEN [PI].Doc_Code
				WHEN [TPI].Voucher_Acc_ID IS NOT NULL		THEN [TPI].Doc_Code
				END,
			[doc_identity_id] = CASE	
				WHEN [SPI].Special_Acc_ID IS NOT NULL		THEN dbo.func_mask_doc_id([SPI].Doc_Code, CONVERT(varchar, DecryptByKey([SPI].Encrypt_Field1)), CONVERT(varchar, DecryptByKey([SPI].Encrypt_Field11)))
				WHEN [PI].Voucher_Acc_ID IS NOT NULL		THEN dbo.func_mask_doc_id([PI].Doc_Code, CONVERT(varchar, DecryptByKey([PI].Encrypt_Field1)), CONVERT(varchar, DecryptByKey([PI].Encrypt_Field11)))
				WHEN [TPI].Voucher_Acc_ID IS NOT NULL		THEN dbo.func_mask_doc_id([TPI].Doc_Code, CONVERT(varchar, DecryptByKey([TPI].Encrypt_Field1)), CONVERT(varchar, DecryptByKey([TPI].Encrypt_Field11)))
				END,
			[ehs_account_eng_name] = CASE	
				WHEN [SPI].Special_Acc_ID IS NOT NULL		THEN dbo.func_get_surname_n_initial(CONVERT(varchar, DecryptByKey([SPI].Encrypt_Field2)))
				WHEN [PI].Voucher_Acc_ID IS NOT NULL		THEN dbo.func_get_surname_n_initial(CONVERT(varchar, DecryptByKey([PI].Encrypt_Field2)))
				WHEN [TPI].Voucher_Acc_ID IS NOT NULL		THEN dbo.func_get_surname_n_initial(CONVERT(varchar, DecryptByKey([TPI].Encrypt_Field2)))
			END,
			[Sex] = CASE	
				WHEN [SPI].Special_Acc_ID IS NOT NULL		THEN [SPI].Sex
				WHEN [PI].Voucher_Acc_ID IS NOT NULL		THEN [PI].Sex
				WHEN [TPI].Voucher_Acc_ID IS NOT NULL		THEN [TPI].Sex
				END,
			[DOB] = CASE	
				WHEN [SPI].Special_Acc_ID IS NOT NULL		THEN CONVERT(varchar(10), [SPI].DOB, 111)
				WHEN [PI].Voucher_Acc_ID IS NOT NULL		THEN CONVERT(varchar(10), [PI].DOB, 111)
				WHEN [TPI].Voucher_Acc_ID IS NOT NULL		THEN CONVERT(varchar(10), [TPI].DOB, 111)
				END,
			[Exact_DOB] = CASE	
				WHEN [SPI].Special_Acc_ID IS NOT NULL		THEN [SPI].Exact_DOB
				WHEN [PI].Voucher_Acc_ID IS NOT NULL		THEN [PI].Exact_DOB
				WHEN [TPI].Voucher_Acc_ID IS NOT NULL		THEN [TPI].Exact_DOB
				END,
			[Transaction_ID] = dbo.func_format_system_number(T.transaction_id),
			[Transaction_Date] = CONVERT(varchar(10), VT.Transaction_Dtm, 111),
			[Transaction_Time] = CONVERT(varchar(10), VT.Transaction_Dtm, 108),
			[Service_Date] = CONVERT(varchar(10), VT.Service_Receive_Dtm, 111),
			VT.Scheme_Code,
			VT.SP_ID,
			[SP_Name] = dbo.func_get_surname_n_initial(CONVERT(varchar, DecryptByKey(SP.Encrypt_Field2))),
			VT.Practice_Display_Seq,
			P.Practice_Name,
			Prof.Service_Category_Code,
			D.District_Name,
			ISNULL(RV1.Reason_L1, 'N/A'),
			ISNULL(RV2.Reason_L2, 'N/A'),
			TD.Unit,
			[aberrant_pattern_S] = CASE WHEN ATS.transaction_id IS NULL THEN 'N' ELSE 'Y' END,
			[aberrant_pattern_C] = CASE WHEN ATC.transaction_id IS NULL THEN 'N' ELSE 'Y' END,
			1
		FROM			
			(SELECT DISTINCT transaction_id FROM #TempDistinctAberrantTransaction) T
				INNER JOIN	VoucherTransaction VT 
					ON T.transaction_id = VT.Transaction_Id
				INNER JOIN TransactionDetail TD 
					ON VT.Transaction_ID = TD.Transaction_ID
				INNER JOIN ServiceProvider SP 
					ON VT.SP_ID = SP.SP_ID
				INNER JOIN Practice P 
					ON SP.SP_ID = P.SP_ID AND VT.Practice_Display_Seq = P.Display_Seq
				LEFT JOIN Professional Prof 
					ON P.SP_ID = Prof.SP_ID AND P.Professional_Seq = Prof.Professional_Seq
				LEFT JOIN District D 
					ON P.District = D.district_code
				LEFT JOIN TransactionAdditionalField TAF1 
					ON VT.Transaction_ID = TAF1.Transaction_ID AND TAF1.AdditionalFieldID = 'Reason_for_Visit_L1'
				LEFT JOIN TransactionAdditionalField TAF2	
					ON VT.Transaction_ID = TAF2.Transaction_ID AND TAF2.AdditionalFieldID = 'Reason_for_Visit_L2'
				LEFT JOIN ReasonForVisitL1 RV1 
					ON Prof.Service_Category_Code = RV1.Professional_Code AND TAF1.AdditionalFieldValueCode = RV1.Reason_L1_Code
				LEFT JOIN ReasonForVisitL2 RV2 
					ON Prof.Service_Category_Code = RV2.Professional_Code 
					AND TAF1.AdditionalFieldValueCode = RV2.Reason_L1_Code
					AND TAF2.AdditionalFieldValueCode = RV2.Reason_L2_Code
				LEFT JOIN SpecialPersonalInformation [SPI] 
					ON VT.Special_Acc_ID = [SPI].Special_Acc_ID
				LEFT JOIN PersonalInformation [PI] 
					ON VT.Voucher_Acc_ID = [PI].Voucher_Acc_ID
				LEFT JOIN TempPersonalInformation [TPI] 
					ON VT.Temp_Voucher_Acc_ID = [TPI].Voucher_Acc_ID
					AND ISNULL(VT.Voucher_Acc_ID, '') = ''
					AND ISNULL(VT.Special_Acc_ID, '') = ''
					AND ISNULL(VT.Invalid_Acc_ID, '') = ''
				LEFT JOIN #TempDistinctAberrantTransaction ATS 
					ON T.transaction_id = ATS.transaction_id AND ATS.aberrant_pattern = 'S'
				LEFT JOIN #TempDistinctAberrantTransaction ATF 
					ON T.transaction_id = ATF.transaction_id AND ATF.aberrant_pattern = 'F'
				LEFT JOIN #TempDistinctAberrantTransaction ATC 
					ON T.transaction_id = ATC.transaction_id AND ATC.aberrant_pattern = 'C'
		ORDER BY 
			Aberrant_id, Transaction_Date, Transaction_Time

		SET @TotalNumberOfRecord = @@rowcount

	CLOSE SYMMETRIC KEY sym_Key

	IF @TotalNumberOfRecord IS NULL
		SET @TotalNumberOfRecord = 0 

	-- Report Summary	
	INSERT INTO @TempSummaryResultTable (Result_Value1) VALUES ('Summary:')
	INSERT INTO @TempSummaryResultTable (Result_Value1, Result_Value2) VALUES ('Total Number of Recipient with only Aberrant Pattern *S:', ISNULL(@TotalNumberOfRecipientWithS, 0))
	INSERT INTO @TempSummaryResultTable (Result_Value1, Result_Value2) VALUES ('Total Number of Recipient with only Aberrant Pattern *C:', ISNULL(@TotalNumberOfRecipientWithC, 0))
	INSERT INTO @TempSummaryResultTable (Result_Value1, Result_Value2) VALUES ('Total Number of Recipient with Aberrant Pattern *S and *C:', ISNULL(@TotalNumberOfRecipientWithSnC, 0))
	INSERT INTO @TempSummaryResultTable (Result_Value1, Result_Value2) VALUES ('Total Number of Record:', ISNULL(@TotalNumberOfRecord, 0))

-- =============================================
-- Return results
-- =============================================

	-- Report Parameter
	SELECT	
		CASE WHEN ISNULL(@TotalNumberOfRecord, 0) > 0 THEN 'Y' ELSE 'N' END AS 'HaveResult',
		CONVERT(varchar(11), @target_period_from, 106) AS 'Date',
		@target_transaction_count AS 'TargetTransactionCount',
		@target_transaction_period_ori AS 'TargetTransactionPeriod',
		@target_multiple_voucher_scheme_claim_period_ori AS 'TargetMultipleVoucherSchemeClaimPeriod'

	-- Result Set 1: Table of Content
	SELECT		
		'Report Generation Time: ' + CONVERT(varchar, GETDATE(), 111) + ' ' + CONVERT(VARCHAR(5), GETDATE(), 108) AS Result_Value

	-- Result Set 2: Summary 
	SELECT		
		Result_Value1,
		Result_Value2
	FROM
		@TempSummaryResultTable
	ORDER BY 
		Result_Seq

	-- Result Set 3: Record Detail
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
		Result_Value14,
		Result_Value15,
		Result_Value16,
		Result_Value17,
		Result_Value18,
		Result_Value19,
		Result_Value20,
		Result_Value21,
		Result_Value22,
		Result_Value23,
		Result_Value24
	FROM			
		#TempResultTable
	ORDER BY	
		Result_Seq
	
	DROP TABLE #TempTargetRecipient
	DROP TABLE #TempAberrantTransaction
	DROP TABLE #TempDistinctAberrantTransaction
	DROP TABLE #TempResultTable

END
GO

GRANT EXECUTE ON [dbo].[proc_Recipient_Aberrant_Voucher_Usage] TO HCVU
GO

