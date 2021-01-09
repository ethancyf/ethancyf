IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSW0002]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSW0002]
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
-- Modified by:		Winnie SUEN
-- Modified date:	22 Nov 2019
-- CR No.:			INT19-0029
-- Description:		Fix Parameter Sniffing
--					Fix Insert into [#TempTargetRecipientForCaseV] without validated account id
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	19 Nov 2019
-- CR No.:			INT19-0028
-- Description:		Add - WITH (NOLOCK)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	26 Sep 2018
-- CR No.:			CRE17-010 (OCSSS integration - VSS Aberrant)
-- Description:		[01] - Add Summary
--					[02] - New sub report OCSSS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	26 May 2016
-- CR No.:			INT16-0006
-- Description:		Change @table to #table
-- =============================================
-- =============================================
-- Author:			Winnie SUEN
-- Create date:		21 Apr 2016
-- Description:		New Weekly Aberrant Report for VSS
-- =============================================


CREATE PROCEDURE [dbo].[proc_EHS_eHSW0002] 
	@request_dtm			DATETIME = null,		-- The reference date to get @target_period_from and @target_period_to. It's [Request_Dtm] from [FileGenerationQueue] Table (* Passed in from Excel Generator. When changing this field, plz also update the corresponding Excel Generator)
	@target_period_from		DATETIME = null,		-- The Target Period From. If defined, it will override the value from the @request_dtm
	@target_period_to		DATETIME = null,		-- The Target Period To. If defined, it will override the value from the @request_dtm
	@is_debug				BIT = 0

AS BEGIN

-- =============================================
-- Declaration
-- =============================================

-- Test Data
--SET @target_period_from = '2014-01-01'
--SET @target_period_to = '2016-04-28'

	DECLARE @In_request_dtm				DATETIME = @request_dtm
	DECLARE @In_target_period_from		DATETIME = @target_period_from
	DECLARE @In_target_period_to		DATETIME = @target_period_to
	DECLARE @In_is_debug				BIT = @is_debug

	------------------------------------------------------------------------------------------
	-- Report Helper Field
	DECLARE @Report_ID				VARCHAR(30)	= 'eHSW0002'
	DECLARE @Current_Season_Start	DATETIME
	DECLARE @Current_Season_End		DATETIME	
	DECLARE @Age_Limit				INT	= 13
	DECLARE @ResultCount			INT = 0
	------------------------------------------------------------------------------------------
	-- Temp Table for Season Period
	DECLARE @TempSeasonPeriod AS TABLE(
		Season_Seq			SMALLINT
		, Season_Desc		VARCHAR(100)
		, Season_Start		DATETIME		--(Compare logic use ">=")
		, Season_End		DATETIME		--(Compare logic use "<")
	)	
	
	------------------------------------------------------------------------------------------	
	-- Temp Table for SubsidizeItem included in this report
	DECLARE @TempSubsidizeItem AS TABLE(
		Subsidize_Item_Code	CHAR(10)
	)	

	------------------------------------------------------------------------------------------	
	-- Temp Table for SubsidizeItem included in this report
	DECLARE @TempTransactionStatusFilter AS TABLE(
		Status_Name			CHAR(50)
		, Status_Value		CHAR(10)
	)	
	
	------------------------------------------------------------------------------------------
	-- Temp Table for all target Transaction 
	CREATE TABLE #TempTargetRecipientTransaction (
		  Transaction_ID	CHAR(20)
		, Transaction_Dtm	DATETIME
		, Service_Receive_Dtm  DATETIME
		, Voucher_Acc_ID	CHAR(15)
		, Doc_Code			CHAR(20)
		, DOB				DATETIME
		, DOB_Adjust		DATETIME
		, Exact_DOB			CHAR(1)
		, Eng_Name			CHAR(100)			
		, Aberrant_Pattern	CHAR(1)
	)

	CREATE NONCLUSTERED INDEX IX_TempTargetRecipientTransaction_Transaction_ID
		ON #TempTargetRecipientTransaction (Transaction_ID); 

	CREATE NONCLUSTERED INDEX IX_TempTargetRecipientTransaction_Voucher_Acc_ID_Doc_Code
		ON #TempTargetRecipientTransaction (Voucher_Acc_ID, Doc_Code); 

	------------------------------------------------------------------------------------------
	-- Temp Table for Recipient for Case V
	CREATE TABLE #TempTargetRecipientForCaseV (
		Voucher_Acc_ID		CHAR(15)
		, Doc_Code			CHAR(20)
	)

	CREATE NONCLUSTERED INDEX IX_TempTargetRecipientForCaseV_Voucher_Acc_ID_Doc_Code
		ON #TempTargetRecipientForCaseV (Voucher_Acc_ID, Doc_Code); 

	------------------------------------------------------------------------------------------
	-- Temp Table for Recipient Base for Case R
	CREATE TABLE #TempTargetRecipientBaseForCaseR ( 
		ROWNUM		INT
		, System_Dtm		DATETIME 
		, Voucher_Acc_ID	CHAR(15)	
		, DOC_Code			CHAR(20)
		, DOB				DATETIME
		, Exact_DOB			CHAR(1)
		, Eng_Name			CHAR(100)	
	)  

	CREATE NONCLUSTERED INDEX IX_TempTargetRecipientBaseForCaseR_Voucher_Acc_ID_Doc_Code
		ON #TempTargetRecipientBaseForCaseR (Voucher_Acc_ID, Doc_Code, System_Dtm); 

	------------------------------------------------------------------------------------------
	-- Temp Table for Recipient for Case R
	CREATE TABLE #TempTargetRecipientForCaseR (
		Voucher_Acc_ID		CHAR(15)
		, Doc_Code			CHAR(20)
	)

	CREATE NONCLUSTERED INDEX IX_TempTargetRecipientForCaseR_Voucher_Acc_ID_Doc_Code
		ON #TempTargetRecipientForCaseR (Voucher_Acc_ID, Doc_Code); 
	
	------------------------------------------------------------------------------------------	
	-- Temp Table for grouping Target Recipient and Season
	CREATE TABLE #TempTargetRecipientGroup ( 
		Aberrant_Group		INT
		, Transaction_ID	CHAR(20) 
		, Voucher_Acc_ID	CHAR(15)	
		, DOC_Code			CHAR(20)
		, DOB				DATETIME
		, DOB_Adjust		DATETIME
		, Exact_DOB			CHAR(1)
		, Eng_Name			CHAR(100)	
		, Season_Seq		SMALLINT
	)  
  	
	------------------------------------------------------------------------------------------
	-- Temp Table for Transaction treated as aberrant usage
	CREATE TABLE #TempAberrantTransaction (
		Aberrant_Group		INT		
		, Transaction_ID	CHAR(20)
		, Transaction_Dtm	DATETIME
		, DOB_Adjust		DATETIME 
		, Service_Receive_Dtm  DATETIME		
	)

	------------------------------------------------------------------------------------------
	-- Temp Table for all Transaction with OCSSS connection fail
	CREATE TABLE #OCSSS_Fail_Transaction (
		  Transaction_ID	CHAR(20)
		, Transaction_Dtm	DATETIME
		, Scheme_Code		CHAR(10)
		, SP_ID				CHAR(8)
		, HKIC_Symbol		CHAR(1)
	)

	------------------------------------------------------------------------------------------
	-- Result Table
	DECLARE @TempResultTable_01 AS TABLE(
		Result_Seq int identity(1,1),	-- Sorting Sequence
		Result_Value1 varchar(200),	-- Aberrant Group Column
		Result_Value2 varchar(100),	-- eHealth Account ID
		Result_Value3 varchar(100),	-- eHealth Account Document Type Column
		Result_Value4 varchar(100),	-- eHealth Account Name (Surname & Initial) Column
		Result_Value5 varchar(100),	-- eHealth Account Gender Column
		Result_Value6 varchar(100),	-- eHealth Account DOB Column 
		Result_Value7 varchar(100),	-- eHealth Account DOB Flag Column
		Result_Value8 varchar(100),	-- Transaction Number Column
		Result_Value9 varchar(100),	-- SPID Column
		Result_Value10 varchar(100),	-- Date Time of Voucher Claim (Transaction Time; Format:yyyy/mm/dd HH:mm:ss) Column
		Result_Value11 varchar(100),	-- Service Date Column
		Result_Value12 varchar(100),	-- Scheme Column		
		Result_Value13 varchar(100),	-- Subsidy Column
		Result_Value14 varchar(100),	-- Dose Column
		Result_Value15 varchar(100),	-- Aberrant Pattern [Claimed SIV in Reporting Period] Column
		Result_Value16 varchar(100),	-- Aberrant Pattern [eHealth (Subsidies) Account is Validated in Reporting Period] Column
		Result_Value17 varchar(100),	-- Aberrant Pattern [Rectified English Name or DOB in Reporting Period] Column
		Is_Data	bit
	)
	
	DECLARE @TempResultTable_02 AS TABLE(
		Result_Seq int identity(1,1),	-- Sorting Sequence
		Result_Value1 varchar(200),	-- Transaction No.
		Result_Value2 varchar(100),	-- Transcation Time
		Result_Value3 varchar(100),	-- Scheme
		Result_Value4 varchar(100),	-- Service Provider ID
		Result_Value5 varchar(100)	-- HKIC Symbol
	)
	------------------------------------------------------------------------------------------

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

	------------------------------------------------------------------------------------------
	-- Init the Request_Dtm (Reference) DateTime to Avoid Null value
	IF @In_request_dtm is null
		SET @In_request_dtm = GETDATE()

	-- The Pass 7 day, ensure the time start from 00:00 (datetime compare logic use ">=")
	IF @In_target_period_from is null
		SET @In_target_period_from = CONVERT(datetime, CONVERT(varchar(10), DATEADD(d, -7, @In_request_dtm), 105), 105)
	ELSE
		SET @In_target_period_from = CONVERT(datetime, CONVERT(varchar(10), @In_target_period_from, 105), 105)

	-- The Pass 1 day, ensure the time start from 00:00 (datetime compare logic use "<", so get today's date)
	IF @In_target_period_to is null
		SET @In_target_period_to = CONVERT(datetime, CONVERT(varchar(10), @In_request_dtm, 105), 105)
	ELSE
		SET @In_target_period_to = CONVERT(datetime, CONVERT(varchar(10), @In_target_period_to, 105), 105)

	------------------------------------------------------------------------------------------
	INSERT INTO @TempSubsidizeItem (Subsidize_Item_Code) VALUES ('SIV'), ('SIVSH')
	
	
	------------------------------------------------------------------------------------------
	INSERT INTO @TempTransactionStatusFilter (Status_Name, Status_Value)
	SELECT	
			Status_Name, 
			Status_Value 
	FROM 
			StatStatusFilterMapping WITH (NOLOCK)
	WHERE 
			(Report_id = 'ALL' OR Report_id = @Report_ID)     
			AND (Effective_Date IS NULL OR Effective_Date <= @In_target_period_to) 
			AND (Expiry_Date IS NULL OR Expiry_Date >= @In_target_period_to)
	
	
	------------------------------------------------------------------------------------------
	EXEC [proc_SymmetricKey_open]

	------------------------------------------------------------------------------------------
	-- Prepare Table for Season Period (Only count for SIV and SIVSH)
	
	;WITH CTE AS (
		SELECT 
				rownum = ROW_NUMBER() OVER (ORDER BY VS.Season_Seq),
				VS.Season_Seq,
				VS.Season_Desc,
				CONVERT(datetime, CONVERT(varchar(10), MIN(SC.Claim_Period_From), 105), 105) AS [Season_Start]
		FROM 
				SubsidizeGroupClaimItemDetails SD WITH (NOLOCK)
		INNER JOIN	SubsidizeGroupClaim SC WITH (NOLOCK) ON SD.Subsidize_Code = SC.Subsidize_Code AND SD.Scheme_Seq = SC.Scheme_Seq
		INNER JOIN	VaccineSeason VS WITH (NOLOCK) ON SD.Scheme_Code = VS.Scheme_Code AND SD.Scheme_Seq = VS.Scheme_Seq AND SD.Subsidize_Item_Code = VS.Subsidize_Item_Code
		INNER JOIN	@TempSubsidizeItem TSI ON SD.Subsidize_Item_Code = TSI.Subsidize_Item_Code
		GROUP BY 
				VS.Season_Seq, VS.Season_Desc
	)

	INSERT INTO @TempSeasonPeriod (Season_Seq, Season_Desc, Season_Start, Season_End)
	(
		SELECT
				CTE.Season_Seq,
				CTE.Season_Desc,
				CTE.Season_Start,
				ISNULL(nex.Season_Start,'2100-01-01') AS [Season_End] -- Start Date of next season start
		FROM 
				CTE
		LEFT JOIN	CTE nex ON nex.rownum = CTE.rownum + 1
	)
	
	
	-- Find current season period based on the report period (Can be cross season)		
	SET	@Current_Season_Start = (SELECT MAX(Season_Start) FROM @TempSeasonPeriod WHERE	@In_target_period_from >= Season_Start)
	SET	@Current_Season_End = (SELECT MIN(Season_End) FROM @TempSeasonPeriod WHERE	@In_target_period_to <= Season_End)


	--======================================================================
	--	Step 1: Find target transaction
	--======================================================================	
	------------------------------------------------------------------------------------------
	-- Prepare Table for the Target Recipient Transaction for Case C (Claimed SIV in Reporting Period)
	INSERT INTO #TempTargetRecipientTransaction (Transaction_ID, Transaction_Dtm, Service_Receive_Dtm, Voucher_Acc_ID, Doc_Code, Aberrant_Pattern)
	SELECT		
			VT.Transaction_ID,
			VT.Transaction_Dtm,
			VT.Service_Receive_Dtm,
			VT.Voucher_Acc_ID,
			VT.Doc_Code,
			'C'
	FROM	
			VoucherTransaction VT WITH (NOLOCK)
	INNER JOIN	TransactionDetail TD WITH (NOLOCK) ON VT.Transaction_ID = TD.Transaction_ID
	INNER JOIN	@TempSubsidizeItem TSI ON TD.Subsidize_Item_Code = TSI.Subsidize_Item_Code
	WHERE	
			VT.Transaction_Dtm >= @In_target_period_from
			AND VT.Transaction_Dtm < @In_target_period_to
			AND	VT.Voucher_Acc_ID <> ''
			AND (VT.Invalidation IS NULL   
				OR VT.Invalidation NOT IN (SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Invalidation'))
			AND VT.Record_Status NOT IN
				(SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Record_Status') 					
						
	------------------------------------------------------------------------------------------
	-- Prepare Table for the Target Recipient for Case V (Who being validated in Reporting Period)
	INSERT INTO #TempTargetRecipientForCaseV (Voucher_Acc_ID, Doc_Code)
	SELECT	DISTINCT 
			TVA.Validated_Acc_ID,
			TPI.Doc_Code
	FROM
		(
			SELECT  
					Voucher_Acc_ID
			FROM   
					TempVoucherAccMatchLog WITH (NOLOCK) 
			WHERE	
					Processed = 'Y'  
					AND Return_Dtm >= @In_target_period_from  
					AND Return_Dtm < @In_target_period_to  
					AND Valid_HKID = 'Y'
			UNION ALL  
			SELECT  
					Voucher_Acc_ID
			FROM   
					TempVoucherAccManualMatchLOG WITH (NOLOCK)
			WHERE
					Return_Dtm >= @In_target_period_from  
					AND Return_Dtm < @In_target_period_to  
					AND Valid = 'Y' 
		)	MatchResult
    INNER JOIN	TempVoucherAccount TVA WITH (NOLOCK) ON MatchResult.Voucher_Acc_ID = TVA.Voucher_Acc_ID 
    INNER JOIN	TempPersonalInformation TPI WITH (NOLOCK) ON MatchResult.Voucher_Acc_ID = TPI.Voucher_Acc_ID
    WHERE
			TVA.Account_Purpose NOT IN ('A', 'O')  			
			AND TVA.Validated_Acc_ID <> ''
	
	-- Prepare Table for the Target Recipient Transaction for Case V (SIV Claim in ANY Season)
	INSERT INTO #TempTargetRecipientTransaction (Transaction_ID, Transaction_Dtm, Service_Receive_Dtm, Voucher_Acc_ID, Doc_Code, Aberrant_Pattern)
	SELECT		
			VT.Transaction_ID,
			VT.Transaction_Dtm,
			VT.Service_Receive_Dtm,
			VT.Voucher_Acc_ID,
			VT.Doc_Code,
			'V'
	FROM	
			#TempTargetRecipientForCaseV RV
	INNER JOIN	VoucherTransaction VT WITH (NOLOCK) ON RV.Voucher_Acc_ID = VT.Voucher_Acc_ID AND RV.Doc_Code = VT.Doc_Code 
	INNER JOIN	TransactionDetail TD WITH (NOLOCK) ON VT.Transaction_ID = TD.Transaction_ID 
	INNER JOIN	@TempSubsidizeItem TSI ON TD.Subsidize_Item_Code = TSI.Subsidize_Item_Code
	WHERE					
			(VT.Invalidation IS NULL   
				OR VT.Invalidation NOT IN (SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Invalidation'))
			AND VT.Record_Status NOT IN
				(SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Record_Status')

	------------------------------------------------------------------------------------------
	-- Prepare Table for the Target Recipient for Case R (Who rectified English name or DOB in Reporting Period)	



	INSERT INTO #TempTargetRecipientBaseForCaseR 
		(ROWNUM, System_Dtm, Voucher_Acc_ID, DOC_Code, DOB, Exact_DOB, Eng_Name)
	SELECT  
			ROW_NUMBER() OVER (PARTITION BY Voucher_Acc_ID, Doc_Code ORDER BY System_Dtm) AS ROWNUM, 
			System_Dtm,	
			Voucher_Acc_ID,
			Doc_Code,
			DOB,
			Exact_DOB,
			dbo.func_Remove_EngNameSpecialChar(CONVERT(varchar, DecryptByKey(Encrypt_Field2))) AS EngName			
	FROM
			PersonalInfoAmendHistory H1 WITH (NOLOCK)			
	WHERE	
			EXISTS (SELECT  
							DISTINCT Voucher_Acc_ID
					FROM   
							PersonalInfoAmendHistory H2 WITH (NOLOCK)
					WHERE										
							Voucher_Acc_ID <> ''
							AND H1.Voucher_Acc_ID = Voucher_Acc_ID AND H1.Doc_Code = Doc_Code
							AND System_Dtm >= @In_target_period_from AND System_Dtm < @In_target_period_to  
							AND Record_Status = 'A')
			AND Record_Status = 'A'

	-- Compare with previous record	
	INSERT INTO #TempTargetRecipientForCaseR (Voucher_Acc_ID, Doc_Code)
	(	
		SELECT	DISTINCT 
				R.Voucher_Acc_ID, 
				R.Doc_Code
		FROM	
				#TempTargetRecipientBaseForCaseR R
					INNER JOIN	#TempTargetRecipientBaseForCaseR R_PREV 
						ON R.Voucher_Acc_ID = R_PREV.Voucher_Acc_ID 
							AND R.Doc_Code = R_PREV.Doc_Code 
							AND R_PREV.ROWNUM = R.ROWNUM - 1
		WHERE 
				R.System_Dtm >= @In_target_period_from  
				AND R.System_Dtm < @In_target_period_to   	
				AND ( DATEDIFF(dd, R.DOB, R_PREV.DOB) <> 0
					OR R.Exact_DOB <> R_PREV.Exact_DOB
					OR R.Eng_Name <> R_PREV.Eng_Name
				)
	)

	-- Prepare Table for the Target Recipient Transaction for Case R (SIV Claim in CURRENT Season)
	INSERT INTO #TempTargetRecipientTransaction (Transaction_ID, Transaction_Dtm, Service_Receive_Dtm, Voucher_Acc_ID, Doc_Code, Aberrant_Pattern)
	SELECT		
			VT.Transaction_ID,
			VT.Transaction_Dtm,
			VT.Service_Receive_Dtm,
			VT.Voucher_Acc_ID,
			VT.Doc_Code,
			'R'
	FROM	
			#TempTargetRecipientForCaseR RR
	INNER JOIN	VoucherTransaction VT ON RR.Voucher_Acc_ID = VT.Voucher_Acc_ID AND RR.Doc_Code = VT.Doc_Code
	INNER JOIN	TransactionDetail TD ON VT.Transaction_ID = TD.Transaction_ID
	INNER JOIN	@TempSubsidizeItem TSI ON TD.Subsidize_Item_Code = TSI.Subsidize_Item_Code
	WHERE		
			VT.Service_receive_dtm >= @Current_Season_Start
			AND VT.Service_receive_dtm < @Current_Season_End	
			AND (VT.Invalidation IS NULL   
				OR VT.Invalidation NOT IN (SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Invalidation'))
			AND VT.Record_Status NOT IN
				(SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Record_Status')
						
	------------------------------------------------------------------------------------------								
	UPDATE	RT
	SET
			DOB = [PI].DOB,
			DOB_Adjust = CASE	WHEN [PI].Exact_DOB IN ('M','U') THEN DATEADD(M, DATEDIFF(M, 0, DATEADD(M, 1, [PI].DOB)), -1) --Format the Exact_DOB is month case, date value become the last day of that month   
								ELSE [PI].DOB END,
			Eng_Name = dbo.func_Remove_EngNameSpecialChar(CONVERT(varchar, DecryptByKey([PI].Encrypt_Field2))),
			Doc_Code = CASE	WHEN [PI].Doc_Code IN ('HKIC', 'HKBC') THEN 'HKICBC'  
							ELSE [PI].Doc_Code END,  -- Consider HKIC & HKBC as same person
			Exact_DOB = CASE [PI].Exact_DOB WHEN 'T' THEN 'D'
								WHEN 'U' THEN 'M'
								WHEN 'V' THEN 'Y'
								ELSE [PI].Exact_DOB END  -- Regardless of “exact date” and “In Word” date 							
	FROM	
			#TempTargetRecipientTransaction RT
	INNER JOIN	PersonalInformation [PI] ON RT.Voucher_Acc_ID = [PI].Voucher_Acc_ID AND RT.Doc_Code = [PI].Doc_Code

	-- For easy to count age
	UPDATE      
			#TempTargetRecipientTransaction      
	SET      
			DOB_Adjust = DATEADD(yyyy, 1, DOB_Adjust)      
	WHERE       
			MONTH(DOB_Adjust) > MONTH(Service_receive_dtm)      
			OR  (MONTH(DOB_Adjust) = MONTH(Service_receive_dtm) AND DAY(DOB_Adjust) > DAY(Service_receive_dtm))


	-- Remove the record that	1. transaction date later than cut off date
	--							2. age >= Age Limit at service receive date
	DELETE	#TempTargetRecipientTransaction
	WHERE 
			Transaction_Dtm	>= @In_target_period_to
			OR DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= @Age_Limit
	--======================================================================
	--	End of Step 1: Find target transaction
	--======================================================================	
	
		
	--======================================================================
	--	Step 2: Form Group
	--======================================================================	
	INSERT INTO #TempTargetRecipientGroup (Aberrant_Group, Transaction_ID, Voucher_Acc_ID, DOC_Code, DOB, DOB_Adjust, Exact_DOB, Eng_Name, Season_Seq)
	SELECT	DISTINCT
			DENSE_RANK() OVER(ORDER BY Eng_Name, DOB, Doc_Code, Voucher_Acc_ID, Season_Seq) AS Aberrant_Group,
			RT.Transaction_ID,
			RT.Voucher_Acc_ID,
			RT.Doc_Code,
			RT.DOB,
			RT.DOB_Adjust,
			RT.Exact_DOB,
			RT.Eng_Name,
			SP.Season_Seq
	FROM 
			#TempTargetRecipientTransaction RT 
	INNER JOIN	@TempSeasonPeriod SP ON SP.Season_Start <= RT.Service_Receive_Dtm AND SP.Season_End > RT.Service_Receive_Dtm
	ORDER BY 
			Aberrant_Group
	
	--======================================================================
	--	End of Step 2: Form Group
	--======================================================================			

	--======================================================================
	--	Step 3: Find matched transaction for each group
	--======================================================================
	------------------------------------------------------------------------------------------
	-- Prepare Table for the Matched Transaction w/ Aberrant Pattern
	-- For Each Aberrant group, Check if the Transaction fulfill the Report Criteria	
		
	INSERT INTO #TempAberrantTransaction (Aberrant_Group, Transaction_ID, Transaction_Dtm, DOB_Adjust, Service_Receive_Dtm)
	SELECT	
			RG.Aberrant_Group,
			VT.Transaction_ID,
			VT.Transaction_Dtm,
			CASE	WHEN [PI].Exact_DOB IN ('M','U') THEN DATEADD(M, DATEDIFF(M, 0, DATEADD(M, 1, [PI].DOB)), -1) --Format the Exact_DOB is month case, date value become the last day of that month   
					ELSE [PI].DOB END AS DOB_Adjust,
			VT.Service_Receive_Dtm			
	FROM	
			(
				SELECT	DISTINCT
						Aberrant_Group,
						Season_Seq,
						DOB,
						DOB_Adjust,
						Exact_DOB,
						Voucher_Acc_ID,
						Doc_Code,
						Eng_Name
				FROM
						#TempTargetRecipientGroup
			) RG
	INNER JOIN	@TempSeasonPeriod SP ON RG.Season_Seq = SP.Season_Seq
	INNER JOIN	VoucherTransaction VT WITH (NOLOCK) ON SP.Season_Start <= VT.Service_receive_dtm AND SP.Season_End > VT.Service_receive_dtm
	INNER JOIN	TransactionDetail TD WITH (NOLOCK) ON VT.Transaction_ID = TD.Transaction_ID 
	INNER JOIN	PersonalInformation [PI] WITH (NOLOCK) ON VT.Voucher_Acc_ID = [PI].Voucher_Acc_ID AND VT.Doc_Code = [PI].Doc_Code
	INNER JOIN	@TempSubsidizeItem TSI ON TD.Subsidize_Item_Code = TSI.Subsidize_Item_Code
	WHERE		
			
			VT.Voucher_Acc_ID <> ''	
			AND (VT.Invalidation IS NULL   
				OR VT.Invalidation NOT IN (SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Invalidation'))
			AND VT.Record_Status NOT IN
				(SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Record_Status')  
			AND [PI].DOB = RG.DOB
			AND 
				CASE [PI].Exact_DOB WHEN 'T' THEN 'D'
									WHEN 'U' THEN 'M'  
									WHEN 'V' THEN 'Y'
				ELSE [PI].Exact_DOB END = RG.Exact_DOB
			AND VT.Voucher_Acc_ID <> RG.Voucher_Acc_ID
			AND ((RG.DOC_Code = 'HKICBC' AND VT.Doc_Code NOT IN ('HKIC', 'HKBC'))
				OR (RG.DOC_Code <> 'HKICBC' AND VT.Doc_Code <> RG.Doc_Code))
			AND dbo.func_Remove_EngNameSpecialChar(CONVERT(varchar, DecryptByKey([PI].Encrypt_Field2))) = RG.Eng_Name
			
	ORDER BY 
			Aberrant_Group, transaction_dtm ASC				
			
			
	-- For easy to count age
	UPDATE      
			#TempAberrantTransaction      
	SET      
			DOB_Adjust = DATEADD(yyyy, 1, DOB_Adjust)      
	WHERE       
			MONTH(DOB_Adjust) > MONTH(Service_receive_dtm)      
			OR  (MONTH(DOB_Adjust) = MONTH(Service_receive_dtm) AND DAY(DOB_Adjust) > DAY(Service_receive_dtm))


	-- Remove the record that	1. transaction date later than cut off date
	--							2. age >= Age Limit at service receive date
	DELETE	#TempAberrantTransaction
	WHERE 
			Transaction_Dtm	>= @In_target_period_to
			OR DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= @Age_Limit
			

			
	-- Combine the target transaction and matched transaction			
	INSERT INTO #TempAberrantTransaction (Aberrant_Group, Transaction_ID)
	SELECT		
			Aberrant_Group, 
			Transaction_ID
	FROM	
			#TempTargetRecipientGroup
	WHERE	
			Aberrant_Group IN (SELECT DISTINCT Aberrant_Group FROM #TempAberrantTransaction)			


	-- Reorder the Aberrant_Group
	UPDATE	AT
	SET 
			Aberrant_Group = Temp.RowNum
	FROM	
			#TempAberrantTransaction AT
	INNER JOIN 
	( 
		SELECT	
				Aberrant_Group, 
				DENSE_RANK() OVER (ORDER BY Aberrant_Group) as RowNum
		FROM	
				#TempAberrantTransaction
	) Temp
	ON AT.Aberrant_Group = Temp.Aberrant_Group
	
	--======================================================================
	--	End of Step 3: Find matched transaction for each group
	--======================================================================		


	--======================================================================
	--	eHSW0002-02: Find Vaccine claim with OCSSS Connection Fail 
	--======================================================================
	DECLARE @OCSSS_Scheme varchar(500)
	SELECT @OCSSS_Scheme = Parm_Value1 FROM SystemParameters WHERE Parameter_Name = 'OCSSS_Scheme'

	-- Prepare table for transaction claimed when fail to connect OCSSS
	INSERT INTO #OCSSS_Fail_Transaction (Transaction_ID, Transaction_Dtm, Scheme_Code, SP_ID, HKIC_Symbol)
	SELECT		
			VT.Transaction_ID,
			VT.Transaction_Dtm,
			VT.Scheme_Code,
			VT.SP_ID,
			VT.HKIC_Symbol
	FROM	
			VoucherTransaction VT WITH (NOLOCK)
	INNER JOIN	TransactionDetail TD WITH (NOLOCK) ON VT.Transaction_ID = TD.Transaction_ID
	INNER JOIN Subsidizeitem SI ON TD.subsidize_item_Code = SI.Subsidize_Item_Code AND SI.Subsidize_Type = 'VACCINE'
	WHERE	
			VT.Transaction_Dtm >= @In_target_period_from
			AND VT.Transaction_Dtm < @In_target_period_to
			AND VT.Scheme_Code IN (SELECT ITEM FROM func_Split_string(@OCSSS_Scheme,';')) -- Scheme for OCSSS checking
			AND	VT.HKIC_Symbol IN ('C','U')
			AND VT.OCSSS_Ref_Status IN ('C','N')	-- Connection Fail / OCSSS is turn off
			AND VT.Manual_Reimburse = 'N'			-- Exclude Back office claim
			AND (VT.Invalidation IS NULL   
				OR VT.Invalidation NOT IN (SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Invalidation'))
			AND VT.Record_Status NOT IN
				(SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Record_Status') 	
	ORDER BY
		VT.Transaction_Dtm
	--======================================================================
	--	End of eHSW0002-02: Find Vaccine claim with OCSSS Connection Fail 
	--======================================================================

	------------------------------------------------------------------------------------------

	IF @In_is_debug = 1
	BEGIN
		SELECT	'' as 'debug:', * FROM @TempSeasonPeriod					
		SELECT '' as 'debug:', @Current_Season_Start AS Current_Season_Start, @Current_Season_End AS Current_Season_End
		SELECT '' as 'debug:', * FROM #TempTargetRecipientTransaction
		SELECT '' as 'debug:', * FROM #TempTargetRecipientGroup order by Aberrant_Group 
		SELECT '' as 'debug:', * FROM #TempAberrantTransaction order by Aberrant_Group 
	END
	
	------------------------------------------------------------------------------------------



	------------------------------------------------------------------------------------------

	-- ---------------------------------------
	-- Excel Worksheet (01 - Multiple doc. types)
	-- ---------------------------------------

	-- Prepare Result Table
	-- Header 1
	INSERT INTO @TempResultTable_01 (Result_Value1) VALUES ('Reporting period: ' + CONVERT(varchar(10), @In_target_period_from, 111) + 
														' to ' + CONVERT(varchar(10), DATEADD(d, -1, @In_target_period_to), 111))
														
	-- Line Break Before Data
	INSERT INTO @TempResultTable_01 (Result_Value1) VALUES ('')

	-- Summary
	INSERT INTO @TempResultTable_01 (Result_Value1, Result_Value2, Result_Value3)
		SELECT 'Total no. of aberrant group:', '', COUNT(DISTINCT Aberrant_Group) FROM #TempAberrantTransaction

	INSERT INTO @TempResultTable_01 (Result_Value1, Result_Value2, Result_Value3)
		SELECT 'Total no. of claim transactions:', '', COUNT(DISTINCT Transaction_ID) FROM #TempAberrantTransaction

	-- Line Break Before Data
	INSERT INTO @TempResultTable_01 (Result_Value1) VALUES ('')

	-- Column Header
	INSERT INTO @TempResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17)
		VALUES ('Aberrant Group', 'eHealth (Subsidies) Account ID', 'eHealth (Subsidies) Account Identity Document Type', 'eHealth (Subsidies) Account Name', 'eHealth (Subsidies) Account Gender', 'eHealth (Subsidies) Account DOB', 'eHealth (Subsidies) Account DOB Flag', 'Transaction No.', 'Service Provider ID', 'Transaction Time', 'Service Date', 'Scheme', 'Subsidy', 'Dose', 'Claimed SIV in Reporting Period', 'eHealth (Subsidies) Account is Validated in Reporting Period', 'Rectified English Name or DOB in Reporting Period')

	-- Report Content
	INSERT INTO @TempResultTable_01 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Is_Data)
		SELECT	
				T.Aberrant_Group,
				dbo.func_format_voucher_account_number('V', [PI].Voucher_Acc_ID) AS ehs_account_id,
				[PI].Doc_Code,
				dbo.func_get_surname_n_initial(CONVERT(varchar, DecryptByKey([PI].Encrypt_Field2))) AS ehs_account_eng_name,
				[PI].Sex,
				CONVERT(varchar(10), [PI].DOB, 111) AS DOB,
				[PI].Exact_DOB,
				dbo.func_format_system_number(T.Transaction_ID) AS Transaction_ID,
				VT.SP_ID,
				CONVERT(varchar(10), VT.Transaction_Dtm, 111) + ' ' + 
				CONVERT(varchar(10), VT.Transaction_Dtm, 108) AS Transaction_Time,
				CONVERT(varchar(10), VT.Service_Receive_Dtm, 111) AS Service_Date,
				VT.Scheme_Code,
				SGC.Display_Code_For_Claim,
				SD.Available_Item_Desc,
				CASE WHEN ATC.Transaction_ID IS NULL THEN 'N' ELSE 'Y' END AS Aberrant_Pattern_C, 
				CASE WHEN ATV.Transaction_ID IS NULL THEN 'N' ELSE 'Y' END AS Aberrant_Pattern_V,
				CASE WHEN ATR.Transaction_ID IS NULL THEN 'N' ELSE 'Y' END AS Aberrant_Pattern_R,
				1
		FROM	
				#TempAberrantTransaction T
		INNER JOIN	VoucherTransaction VT WITH (NOLOCK) ON T.Transaction_ID = VT.Transaction_ID
		INNER JOIN	TransactionDetail TD WITH (NOLOCK) ON VT.Transaction_ID = TD.Transaction_ID
		INNER JOIN	PersonalInformation [PI] WITH (NOLOCK) ON VT.Voucher_Acc_ID = [PI].Voucher_Acc_ID AND VT.Doc_Code = [PI].Doc_Code
		INNER JOIN	SubsidizeGroupClaim SGC ON TD.Scheme_Code = SGC.Scheme_Code AND TD.Scheme_Seq = SGC.Scheme_Seq AND TD.Subsidize_Code = SGC.Subsidize_Code
		INNER JOIN	SubsidizeItemDetails SD ON TD.Subsidize_Item_Code = SD.Subsidize_Item_Code AND TD.Available_Item_Code = SD.Available_Item_Code
		LEFT JOIN	#TempTargetRecipientTransaction ATC ON T.Transaction_ID = ATC.Transaction_ID AND ATC.Aberrant_Pattern = 'C'
		LEFT JOIN	#TempTargetRecipientTransaction ATV ON T.Transaction_ID = ATV.Transaction_ID AND ATV.Aberrant_Pattern = 'V'
		LEFT JOIN	#TempTargetRecipientTransaction ATR ON T.Transaction_ID = ATR.Transaction_ID AND ATR.Aberrant_Pattern = 'R'
		ORDER BY 
				Aberrant_Group, ehs_account_id, Doc_Code, Transaction_Time


		SET @ResultCount = @@rowcount
				
	EXEC [proc_SymmetricKey_close]


-- ---------------------------------------
-- Excel Worksheet (02 - OCSSS)
-- ---------------------------------------
	-- Header 1
	INSERT INTO @TempResultTable_02 (Result_Value1) VALUES ('Reporting period: ' + CONVERT(varchar(10), @In_target_period_from, 111) + 
														' to ' + CONVERT(varchar(10), DATEADD(d, -1, @In_target_period_to), 111))
														
	-- Line Break Before Data
	INSERT INTO @TempResultTable_02 (Result_Value1) VALUES ('')

	-- Summary
	INSERT INTO @TempResultTable_02 (Result_Value1, Result_Value2, Result_Value3)
		SELECT 'Total no. of claim transactions:', '', COUNT(1) FROM #OCSSS_Fail_Transaction

	-- Line Break
	INSERT INTO @TempResultTable_02 (Result_Value1) VALUES ('')
	
	-- Column Header
	INSERT INTO @TempResultTable_02 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
		VALUES ('Transaction No.', 'Transaction Time', 'Scheme', 'Service Provider ID', 'HKIC Symbol')


	-- Report Content
	INSERT INTO @TempResultTable_02 (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
		SELECT	
				dbo.func_format_system_number(Transaction_ID) AS Transaction_ID,				
				FORMAT(Transaction_Dtm, 'yyyy/MM/dd HH:mm:ss') AS Transaction_Time,
				Scheme_Code,
				SP_ID,
				HKIC_Symbol
		FROM	
				#OCSSS_Fail_Transaction
		ORDER BY 
				Transaction_Time
	

	SET @ResultCount += @@rowcount

-- =============================================
-- Return results
-- =============================================

	-- Report Parameter
	SELECT	CASE WHEN ISNULL(@ResultCount, 0) > 0 THEN 'Y' ELSE 'N' END AS 'HaveResult',
			CONVERT(varchar(11), DATEADD(d, -1, @In_target_period_to), 106) AS 'Date'
					
	-- Result Set 1: Table of Content
	SELECT	'Report Generation Time: ' + CONVERT(varchar, GETDATE(), 111) + ' ' + CONVERT(VARCHAR(5), GETDATE(), 108) AS Result_Value


	-- Result Set 2: Multiple doc. types
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
			Result_Value17
	FROM	
			@TempResultTable_01
	ORDER BY	
			Result_Seq

	
	-- Result Set 3: OCSSS
	SELECT	
			Result_Value1,
			Result_Value2,
			Result_Value3,
			Result_Value4,
			Result_Value5
	FROM	
			@TempResultTable_02
	ORDER BY	
			Result_Seq

-- =============================================
-- Finalizer
-- =============================================
	DROP TABLE #TempTargetRecipientTransaction
	DROP TABLE #TempTargetRecipientForCaseV
	DROP TABLE #TempTargetRecipientForCaseR
	DROP TABLE #TempTargetRecipientGroup
	DROP TABLE #TempAberrantTransaction
	DROP TABLE #OCSSS_Fail_Transaction
	

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSW0002] TO HCVU
GO

