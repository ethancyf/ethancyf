IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SP_Frequent_Account_Rejection]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SP_Frequent_Account_Rejection]
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
-- Modified by:		Koala CHENG
-- Modified date:	16 Oct 2018
-- CR No.:			INT18-0019
-- Description:		Exclude back office created account
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	24 November 2015
-- CR No.:			CRE15-006
-- Description:		Rename of eHS
-- =============================================
-- =============================================
-- Author:				Vincent YUEN
-- Create date:		22 Jan 2010
-- Description:		AR3 - Report of HCSP with Frequent Rejection of Temporary Voucher Recipient Account
-- =============================================

CREATE PROCEDURE [dbo].[proc_SP_Frequent_Account_Rejection]
	@request_dtm			DATETIME = null,		-- The reference date to get @target_period_from and @target_period_to. It's [Request_Dtm] from [FileGenerationQueue] Table (* Passed in from Excel Generator. When changing this field, plz also update the corresponding Excel Generator)
	@target_period_from	DATETIME = null,		-- The Target Period From. If defined, it will override the value from the @request_dtm
	@target_period_to		DATETIME = null,		-- The Target Period To. If defined, it will override the value from the @request_dtm
	@is_debug bit = 0
AS BEGIN
-- =============================================
-- Declaration
-- =============================================

-- Test Data
--SET @target_period_from = '2008-01-01'
--SET @target_period_to = '2010-01-25'

	------------------------------------------------------------------------------------------
	-- Report Summary
	DECLARE @SummaryTotalSPValue int

	------------------------------------------------------------------------------------------
	-- Additional Report Criteria
	DECLARE @target_reject_attempt int 

	-- Helper Field
	DECLARE @ResultCount int

	------------------------------------------------------------------------------------------
	-- Temp Table for SP's Reject Count
	DECLARE @SPRejectCount AS table (
		SP_ID char(8),
		RejectCount int
	)

	------------------------------------------------------------------------------------------
	-- Temp Table for SP Practice's Reject Count
	DECLARE @SPPracticeRejectCount AS table (
		SP_ID char(8),
		Practice_Display_Seq smallint,
		RejectCount int
	)

	------------------------------------------------------------------------------------------
	-- Result Table
	CREATE TABLE #ResultTable (  
		Result_Seq int identity(1,1),	-- Sorting Sequence
		Result_Value1 varchar(200),	-- SP ID Column
		Result_Value2 varchar(100),	-- SP Name Column
		Result_Value3 varchar(100),	-- Practice ID Column
		Result_Value4 varchar(100),	-- Practice English Name Column
		Result_Value5 varchar(300),	-- Practice Address Column
		Result_Value6 varchar(100),	-- Practice Telephone Number Column
		Result_Value7 varchar(100),	-- Number of Rejection Column
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
	SELECT	@target_reject_attempt = CONVERT(int, p.Parm_Value1) 
	FROM		SystemParameters p
	WHERE	p.Parameter_Name = 'eHS(S)W0001_TargetRejectedAttempt'

	-- Init the Request_Dtm (Reference) DateTime to Avoid Null value
	IF @request_dtm is null
		SET @request_dtm = GETDATE()

	-- The Pass 7 day, ensure the time start from 00:00 (datetime compare logic use ">=")
	IF @target_period_from is null
		SET @target_period_from = CONVERT(datetime, CONVERT(varchar(10), DATEADD(d, -7, @request_dtm), 105), 105)
	ELSE
		SET @target_period_from = CONVERT(datetime, CONVERT(varchar(10), @target_period_from, 105), 105)

	-- The Pass 1 day, ensure the time start from 00:00 (datetime compare logic use "<", so get today's date)
	IF @target_period_to is null
		SET @target_period_to = CONVERT(datetime, CONVERT(varchar(10), @request_dtm, 105), 105)
	ELSE
		SET @target_period_to = CONVERT(datetime, CONVERT(varchar(10), @target_period_to, 105), 105)

	------------------------------------------------------------------------------------------
	-- Prepare SP Reject Count Table
--	INSERT INTO	 @SPRejectCount (SP_ID, RejectCount)
--		SELECT		VA.SP_ID,
--							COUNT(VA.SP_ID) AS SP_RejectCount
--		FROM			TempVoucherAccMatchLog MatchResult
--		INNER JOIN	VoucherAccountCreationLOG VA ON MatchResult.Voucher_Acc_ID = VA.Voucher_Acc_ID
--		WHERE		MatchResult.Processed = 'Y'
--							AND MatchResult.Return_Dtm >= @target_period_from
--							AND MatchResult.Return_Dtm < @target_period_to
--							AND MatchResult.Valid_HKID = 'N'
--		GROUP BY	VA.SP_ID
--		HAVING		COUNT(VA.SP_ID) >= @target_reject_attempt
	INSERT INTO	 @SPRejectCount (SP_ID, RejectCount)
		SELECT		SP_ID,
							COUNT(SP_ID) AS SP_RejectCount
		FROM			(
								SELECT		VA.SP_ID
								FROM			TempVoucherAccMatchLog MatchResult
								INNER JOIN	VoucherAccountCreationLOG VA ON MatchResult.Voucher_Acc_ID = VA.Voucher_Acc_ID AND ISNULL(Create_By_BO,'') <> 'Y'
								WHERE		MatchResult.Processed = 'Y'
													AND MatchResult.Return_Dtm >= @target_period_from
													AND MatchResult.Return_Dtm < @target_period_to
													AND MatchResult.Valid_HKID = 'N'
								UNION ALL
								SELECT		VA.SP_ID
								FROM			TempVoucherAccManualMatchLOG MatchResult
								INNER JOIN	VoucherAccountCreationLOG VA ON MatchResult.Voucher_Acc_ID = VA.Voucher_Acc_ID AND ISNULL(Create_By_BO,'') <> 'Y'
								INNER JOIN	TempVoucherAccount TVA ON MatchResult.Voucher_Acc_ID = TVA.Voucher_Acc_ID
																									AND TVA.Account_Purpose NOT IN ('A', 'O')
								WHERE		MatchResult.Return_Dtm >= @target_period_from
													AND MatchResult.Return_Dtm < @target_period_to
													AND MatchResult.Valid = 'N'
							) SP_MatchLog
		GROUP BY	SP_ID
		HAVING		COUNT(SP_ID) >= @target_reject_attempt

	-- Store the Total Number of SP
	SET @SummaryTotalSPValue = @@rowcount


	------------------------------------------------------------------------------------------
	-- Prepare SP Reject Count Table
--	INSERT INTO	 @SPPracticeRejectCount (SP_ID, Practice_Display_Seq, RejectCount)
--		SELECT		VA.SP_ID,
--							VA.SP_Practice_Display_Seq, 
--							COUNT(*) AS RejectCount
--		FROM			TempVoucherAccMatchLog MatchResult
--		INNER JOIN	VoucherAccountCreationLOG VA ON MatchResult.Voucher_Acc_ID = VA.Voucher_Acc_ID
--		INNER JOIN	@SPRejectCount SP ON VA.SP_ID = SP.SP_ID
--		WHERE		MatchResult.Processed = 'Y'
--							AND MatchResult.Return_Dtm >= @target_period_from
--							AND MatchResult.Return_Dtm < @target_period_to
--							AND MatchResult.Valid_HKID = 'N'
--		GROUP BY	VA.SP_ID, VA.SP_Practice_Display_Seq
	INSERT INTO	 @SPPracticeRejectCount (SP_ID, Practice_Display_Seq, RejectCount)
		SELECT		SP_ID,
							SP_Practice_Display_Seq,
							COUNT(*) AS RejectCount
		FROM			(
								SELECT		VA.SP_ID,
													VA.SP_Practice_Display_Seq
								FROM			TempVoucherAccMatchLog MatchResult
								INNER JOIN	VoucherAccountCreationLOG VA ON MatchResult.Voucher_Acc_ID = VA.Voucher_Acc_ID AND ISNULL(Create_By_BO,'') <> 'Y'
								INNER JOIN	@SPRejectCount SP ON VA.SP_ID = SP.SP_ID
								WHERE		MatchResult.Processed = 'Y'
													AND MatchResult.Return_Dtm >= @target_period_from
													AND MatchResult.Return_Dtm < @target_period_to
													AND MatchResult.Valid_HKID = 'N'
								UNION ALL
								SELECT		VA.SP_ID,
													VA.SP_Practice_Display_Seq
								FROM			TempVoucherAccManualMatchLOG MatchResult
								INNER JOIN	VoucherAccountCreationLOG VA ON MatchResult.Voucher_Acc_ID = VA.Voucher_Acc_ID AND ISNULL(Create_By_BO,'') <> 'Y'
								INNER JOIN	@SPRejectCount SP ON VA.SP_ID = SP.SP_ID
								WHERE		MatchResult.Return_Dtm >= @target_period_from
													AND MatchResult.Return_Dtm < @target_period_to
													AND MatchResult.Valid = 'N'
							) P_MatchLog
		GROUP BY	SP_ID, SP_Practice_Display_Seq


	------------------------------------------------------------------------------------------
	IF @is_debug = 1
	BEGIN
		SELECT	'' AS 'debug:',
						@target_period_from AS '@target_period_from', 
						@target_period_to AS '@target_period_to',
						@target_reject_attempt AS '@target_reject_attempt'

		SELECT	'' AS 'debug:',
						* 
		FROM		@SPRejectCount

		SELECT	'' AS 'debug:',
						* 
		FROM		@SPPracticeRejectCount
	END
	------------------------------------------------------------------------------------------


	------------------------------------------------------------------------------------------
	-- Prepare Result Table
	-- Header (Reporting Period)
	INSERT INTO #ResultTable (Result_Value1) VALUES ('Reporting period: ' + CONVERT(varchar(10), @target_period_from, 111) +  ' to ' + CONVERT(varchar(10), DATEADD(d, -1, @target_period_to), 111))

	-- Report Parameter
	INSERT INTO #ResultTable (Result_Value1) VALUES ('The target rejected attempt: ' + CONVERT(VARCHAR, @target_reject_attempt))

	-- Line Break Before Report Data
	INSERT INTO #ResultTable (Result_Value1) VALUES ('')

	-- Column Header
	INSERT INTO #ResultTable (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7)
		VALUES ('Service Provider ID', 'Service Provider Name', ' Practice No.', 'Practice Name', 'Practice Address', 'Practice Phone No.', 'No. of Rejection Episodes')

	-- Report Content
	EXEC [proc_SymmetricKey_open]

		INSERT INTO #ResultTable (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Is_Data)   
			SELECT		PracticeRejectCount.SP_ID, 
								dbo.func_get_surname_n_initial(CONVERT(varchar, DecryptByKey(SP.Encrypt_Field2))),
								PracticeRejectCount.Practice_Display_Seq, 
								P.Practice_Name,
								ISNULL('Room ' + P.Room +', ', '') + ISNULL('Floor ' + P.[Floor] +', ', '') + ISNULL('Block ' + P.Block +', ', '') + ISNULL(P.Building + ', ', '') + ISNULL(D.District_Name, ''),
								P.Phone_Daytime,
								PracticeRejectCount.RejectCount,
								1
			FROM			@SPPracticeRejectCount PracticeRejectCount
			LEFT JOIN		ServiceProvider SP ON PracticeRejectCount.SP_ID = SP.SP_ID
			LEFT JOIN		Practice P ON PracticeRejectCount.SP_ID = P.SP_ID
														AND PracticeRejectCount.Practice_Display_Seq = P.Display_Seq
			LEFT JOIN		District D ON P.District = D.district_code
			ORDER BY	PracticeRejectCount.SP_ID, PracticeRejectCount.Practice_Display_Seq ASC

		SET @ResultCount = @@rowcount

	EXEC [proc_SymmetricKey_close]


	-- Report Summary
	INSERT INTO @TempSummaryResultTable (Result_Value1) VALUES ('Summary:')
	INSERT INTO @TempSummaryResultTable (Result_Value1, Result_Value2) VALUES ('Total Number of HCSP:', ISNULL(@SummaryTotalSPValue, 0))

-- =============================================
-- Return results
-- =============================================

	-- Report Parameter
	SELECT	CASE WHEN ISNULL(@ResultCount, 0) > 0 THEN 'Y' ELSE 'N' END AS 'HaveResult',
					CONVERT(varchar(11), @target_period_from, 106) AS 'DateFrom',
					CONVERT(varchar(11), DATEADD(d, -1, @target_period_to), 106) AS 'DateTo',
					@target_reject_attempt AS 'TargetRejectAttempt'

	-- Result Set 1: Table of Content
	SELECT		'Report Generation Time: ' + CONVERT(varchar, GETDATE(), 111) + ' ' + CONVERT(VARCHAR(5), GETDATE(), 108) AS Result_Value

	-- Result Set 2: Summary 
	SELECT		Result_Value1,
						Result_Value2
	FROM			@TempSummaryResultTable
	ORDER BY	Result_Seq

	-- Result Set 3: Detail Record
	SELECT		Result_Value1,
						Result_Value2,
						Result_Value3,
						Result_Value4,
						Result_Value5, 
						Result_Value6, 
						Result_Value7
	FROM			#ResultTable
	ORDER BY	Result_Seq


	DROP TABLE #ResultTable
	
END
GO

GRANT EXECUTE ON [dbo].[proc_SP_Frequent_Account_Rejection] TO HCVU
GO
