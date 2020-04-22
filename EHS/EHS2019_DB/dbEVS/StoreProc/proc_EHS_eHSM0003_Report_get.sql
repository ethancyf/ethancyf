IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSM0003_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSM0003_Report_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================    
-- Modification History    
-- Modified by:      
-- Modified date:     
-- Description:      
-- =============================================    
-- =============================================
-- CR No.:			CRE16-010
-- Author:			Winnie SUEN
-- Create date:		27 Oct 2016
-- Description:		VO monthly aberrant report - eHSM0003
-- =============================================    

CREATE Procedure [proc_EHS_eHSM0003_Report_get]    
	@request_dtm			DATETIME = null,		-- The reference date to get @target_period_from and @target_period_to. It's [Request_Dtm] from [FileGenerationQueue] Table (* Passed in from Excel Generator. When changing this field, plz also update the corresponding Excel Generator)
	@target_period_from		DATETIME = null,		-- The Target Period From. If defined, it will override the value from the @request_dtm
	@target_period_to		DATETIME = null,		-- The Target Period To. If defined, it will override the value from the @request_dtm
	@is_debug				BIT = 0    
AS BEGIN    

SET NOCOUNT ON;    
	-- =============================================    
	-- Declaration    
	-- =============================================
	------------------------------------------------------------------------------------------
	-- Report Helper Field
	DECLARE @Report_ID				VARCHAR(30)	= 'eHSM0003'
	DECLARE @ResultCount			INT = 0

	------------------------------------------------------------------------------------------
	-- Date 
	DECLARE @Last_Year_Peroid_From	DATETIME
	DECLARE @Last_Year_Peroid_To	DATETIME
	
	DECLARE @TotalDays INT
	
	------------------------------------------------------------------------------------------
	-- Criteria Setting	
		
	DECLARE @TargetNoOfTransaction	INT -- Criteria 1
	DECLARE @TargetClaimPercentage	INT -- Criteria 3

	------------------------------------------------------------------------------------------
	-- Result Table
	DECLARE @TempResultTable AS TABLE(
		Result_Seq int identity(1,1),	-- Sorting Sequence
		Result_Value1 varchar(200),	-- SP Name (In English)
		Result_Value2 varchar(100),	-- SPID
		Result_Value3 varchar(100),	-- Category/Scheme
		Result_Value4 varchar(100),	-- (a) Total No. of Reimbursable SI Claims in Last Month
		Result_Value5 varchar(100),	-- (b) Avg No. of Reimbursable SI Claims per day in Last Month
		Result_Value6 varchar(100),	-- (c) No. of Reimbursed SI Claims in Last Year Last Month
		Result_Value7 varchar(100)	-- (d) Compare No. of Reimbursable SI Claims with the same ending period (a-c) / c
	)
	
	-- Remark Table
	DECLARE @RemarkTable AS TABLE(
		Seq int identity(1,1),	-- Sorting Sequence
		 Result_Value1 NVARCHAR(MAX),
		 Result_Value2 NVARCHAR(MAX)    
	)	
	
	------------------------------------------------------------------------------------------
	-- VSS Category Mapping Table						
	DECLARE @VSSCategoryMapping TABLE (
		Scheme_Code			VARCHAR(10),
		Category_Code	VARCHAR(10),
		Scheme_Code_Map		VARCHAR(10),
		Category_Group	VARCHAR(10),
		Category_Desc	VARCHAR(200),
		Display_Seq		INT
	)	
	
	------------------------------------------------------------------------------------------
	-- Temp Table for target period Transaction 
	CREATE TABLE #TempTargetPeriodTransaction (
		  SP_ID				CHAR(8)
		, Category_Group	VARCHAR(10)
		, NumOfTrans		INT
	)	

	------------------------------------------------------------------------------------------
	-- Temp Table for last year period Transaction 
	CREATE TABLE #TempLastYearPeriodTransaction (
		  SP_ID				CHAR(8)
		, Category_Group	VARCHAR(10)
		, NumOfTrans		INT
	)	
		
	------------------------------------------------------------------------------------------
	-- Temp Table for filtered SP
	CREATE TABLE #SP_Filtered (
		  SP_ID				CHAR(8)
	)	
	
	------------------------------------------------------------------------------------------
	-- Filtered SP Transaction
	CREATE TABLE #TempFilteredSPTransaction (
		Display_Seq			INT
		, SP_ID				CHAR(8)
		, Category_Group	VARCHAR(10)
		, NumOfTrans_C		INT
		, NumOfTrans_L		INT
		, ClaimPercentage	INT
	)	
	

	-- =============================================    
	-- Validation     
	-- =============================================    
	-- =============================================    
	-- Initialization    
	-- =============================================    

	------------------------------------------------------------------------------------------
	-- Init the Request_Dtm (Reference) DateTime to Avoid Null value
	IF @request_dtm is null
		SET @request_dtm = GETDATE()

	-- First Day of Last Month, ensure the time start from 00:00 (datetime compare logic use ">=")
	IF @target_period_from is null
		SET @target_period_from = CONVERT(datetime, CONVERT(varchar(10), DATEADD(MONTH, DATEDIFF(MONTH, 0, @request_dtm)-1, 0), 105), 105)
	ELSE
		SET @target_period_from = CONVERT(datetime, CONVERT(varchar(10), @target_period_from, 105), 105)

	-- Last Day of Last Month, ensure the time start from 00:00 (datetime compare logic use "<", so should be First Day of Current Month)
	IF @target_period_to is null
		SET @target_period_to = CONVERT(datetime, CONVERT(varchar(10), DATEADD(MONTH, DATEDIFF(MONTH, 0, @request_dtm), 0), 105), 105)
	ELSE
		SET @target_period_to = CONVERT(datetime, CONVERT(varchar(10), @target_period_to, 105), 105)


	-- Last Year Same Month
	SET @Last_Year_Peroid_From = DATEADD(YEAR, -1,@target_period_from)
	SET @Last_Year_Peroid_To = DATEADD(YEAR, -1,@target_period_to)
	
	SET @TotalDays = DATEDIFF(DAY, @target_period_from, @target_period_to)


	
	SELECT @TargetNoOfTransaction = CONVERT(INT, Parm_Value1) FROM SystemParameters WHERE Parameter_Name = 'eHS(S)M0003_TargetNoOfTransaction'
	SELECT @TargetClaimPercentage = CONVERT(INT, Parm_Value1) FROM SystemParameters WHERE Parameter_Name = 'eHS(S)M0003_TargetClaimPercentage'
	
	-- =============================================    
	-- Table
	-- =============================================
	    
	INSERT INTO @VSSCategoryMapping (Scheme_Code, Category_Code, Scheme_Code_Map, Category_Group, Category_Desc, Display_Seq)
	VALUES
	('VSS', 'VSSPREG', NULL, 'PW', 'VSS (Pregnant Women)', '1')
	,('VSS', 'VSSCHILD', 'CIVSS', 'CHILD', 'VSS (Children)/ CIVSS', '2')
	,('VSS', 'VSSELDER', 'EVSS', 'ELDER', 'VSS (Elders)/ EVSS', '3')
	,('VSS', 'VSSPID', 'PIDVSS', 'PID', 'VSS (Persons with Intellectual Disability)/ PIDVSS', '4')
	,('VSS', 'VSSDA', NULL, 'DA', 'VSS (Persons receiving Disability Allowance)', '5')

	-- =============================================    
	-- Prepare Data
	-- =============================================  
	
	
	--Retrieve all Reimbursable SI Claims under Scheme [VSS] in [Current Year Period] 
	INSERT INTO #TempTargetPeriodTransaction (SP_ID, Category_Group, NumOfTrans)
	SELECT		
		VT.SP_ID,
		CM.Category_Group,
		COUNT(DISTINCT VT.Transaction_ID)
	FROM	
		VoucherTransaction VT
	INNER JOIN	TransactionDetail TD ON VT.Transaction_ID = TD.Transaction_ID
	INNER JOIN	@VSSCategoryMapping CM ON VT.Scheme_Code = CM.Scheme_Code AND VT.Category_Code = CM.Category_Code
	WHERE	
		TD.Subsidize_Item_Code = 'SIV'
		AND VT.Transaction_Dtm >= @target_period_from
		AND VT.Transaction_Dtm < @target_period_to
		AND VT.Record_Status IN ('A','P','B','V','R','S')		
		AND (VT.Invalidation IS NULL 
			OR VT.Invalidation NOT IN
				(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)     
				AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'    
				AND ((Effective_Date is null or Effective_Date <= Transaction_Dtm) AND (Expiry_Date is null or Expiry_Date >= Transaction_Dtm))))
			
	GROUP BY VT.SP_ID, CM.Category_Group
	ORDER BY SP_ID
	
	
	-- Filter SP, Criteria 1: SP with SIV claims >= @TargetNoOfTransaction (100) in last month 
	INSERT INTO #SP_Filtered (SP_ID)
	SELECT SP_ID
	FROM 
		#TempTargetPeriodTransaction
	GROUP BY 
		SP_ID
	HAVING 
		SUM(NumOfTrans) >= @TargetNoOfTransaction

	
	--Retrieve all Reimbursable SI Claims under Scheme [VSS/CIVSS/EVSS/PIDVSS] in [Last Year Period] 
	INSERT INTO #TempLastYearPeriodTransaction (SP_ID, Category_Group, NumOfTrans)
	SELECT		
		VT.SP_ID,
		CM.Category_Group,
		COUNT(DISTINCT VT.Transaction_ID)
	FROM	
		VoucherTransaction VT
	INNER JOIN	TransactionDetail TD ON VT.Transaction_ID = TD.Transaction_ID
	INNER JOIN	@VSSCategoryMapping CM ON ((VT.Scheme_Code = CM.Scheme_Code AND VT.Category_Code = CM.Category_Code) 
											OR VT.Scheme_Code = CM.Scheme_Code_Map)
	WHERE	
		VT.SP_ID IN (SELECT SP_ID FROM #SP_Filtered)
		AND VT.Transaction_Dtm >= @Last_Year_Peroid_From
		AND VT.Transaction_Dtm < @Last_Year_Peroid_To
		AND TD.Subsidize_Item_Code = 'SIV'
		AND VT.Record_Status IN ('A','P','B','V','R','S')	
		AND (VT.Invalidation IS NULL 
			OR VT.Invalidation NOT IN
				(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)     
				AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'    
				AND ((Effective_Date is null or Effective_Date <= Transaction_Dtm) AND (Expiry_Date is null or Expiry_Date >= Transaction_Dtm))))		
	GROUP BY VT.SP_ID, CM.Category_Group, CM.Display_Seq
	ORDER BY SP_ID, CM.Display_Seq
	


	-- Criteria 2: SP with SIV Claim in same month in the last calendar year 
	DELETE #SP_Filtered
	WHERE SP_ID NOT IN (
						SELECT DISTINCT SP_ID
						FROM
							#TempLastYearPeriodTransaction)
							

	-- Construct Table
	INSERT INTO #TempFilteredSPTransaction (SP_ID, Category_Group, NumOfTrans_C, NumOfTrans_L, Display_Seq)
	SELECT	
		FM.SP_ID, 
		FM.Category_Group, 
		ISNULL(TT.NumOfTrans,0), 
		ISNULL(LT.NumOfTrans,0),
		FM.Display_Seq
	FROM 
		(SELECT F.SP_ID, M.Category_Group, M.Display_Seq
		 FROM
			#SP_Filtered F, @VSSCategoryMapping M) FM
	LEFT JOIN #TempTargetPeriodTransaction TT ON FM.SP_ID = TT.SP_ID AND FM.Category_Group = TT.Category_Group
	LEFT JOIN #TempLastYearPeriodTransaction LT ON FM.SP_ID = LT.SP_ID AND FM.Category_Group = LT.Category_Group

	
	INSERT INTO #TempFilteredSPTransaction (SP_ID, Category_Group, NumOfTrans_C, NumOfTrans_L, Display_Seq)
	SELECT SP_ID, 'TOTAL', SUM(NumOfTrans_C), SUM(NumofTrans_L), 0
	FROM 
		#TempFilteredSPTransaction
	GROUP BY
		SP_ID
	
	-- Compare claim percentage	
	UPDATE #TempFilteredSPTransaction
	SET ClaimPercentage = CONVERT(INT, ROUND(NumOfTrans_C * 100.0/NumOfTrans_L - 100,0))
	WHERE NumOfTrans_L > 0


	-- Criteria 3: The no. of SIV claims is @TargetClaimPercentage (50%) or higher than the same month in the last calendar year 
	DELETE #TempFilteredSPTransaction
	WHERE SP_ID NOT IN (
					SELECT DISTINCT SP_ID
					FROM
						#TempFilteredSPTransaction
					WHERE
						ISNULL(ClaimPercentage,0) >= @TargetClaimPercentage)



	IF @is_debug = 1
	BEGIN
		SELECT 'Last Year',* FROM #TempLastYearPeriodTransaction
		SELECT 'Current Year',* FROM #TempTargetPeriodTransaction
		select * from #TempFilteredSPTransaction
	END
	------------------------------------------------------------------------------------------
	-- Prepare Result Table
	-- Header 1
	INSERT INTO @TempResultTable (Result_Value1) VALUES ('Reporting period: ' + CONVERT(varchar(10), @target_period_from, 111) + 
														' to ' + CONVERT(varchar(10), DATEADD(d, -1, @target_period_to), 111))
														
	-- Line Break Before Data
	INSERT INTO @TempResultTable (Result_Value1) VALUES ('')

	-- Column Header
	INSERT INTO @TempResultTable (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7)
	VALUES ('SP Name (In English)', 'SPID', 'Category/Scheme', 
	'(a)
	Total No. of 
	Reimbursable SI Claims in ' + RIGHT(CONVERT(VARCHAR, @target_period_from, 106), 8), 
	'(b)
	Average No. of 
	Reimbursable SI Claims per day in ' + RIGHT(CONVERT(VARCHAR, @target_period_from, 106), 8) + '*', 
	'(c)
	No. of 
	Reimbursable SI Claims in ' + RIGHT(CONVERT(VARCHAR, @Last_Year_Peroid_From, 106), 8), 
	'(d) 
	Compare No. of 
	Reimbursable SI Claims with the same ending period 
	(a-c) / c*')

	OPEN SYMMETRIC KEY sym_Key     
	DECRYPTION BY ASYMMETRIC KEY asym_Key    
 
	-- Report Content
	INSERT INTO @TempResultTable (Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7)
	SELECT	
		CASE T.Category_Group WHEN 'TOTAL' THEN CONVERT(VARCHAR, DecryptByKey(SP.Encrypt_Field2)) ELSE '' END AS SP_Eng_Name,
		CASE T.Category_Group WHEN 'TOTAL' THEN '''' + T.SP_ID ELSE '' END AS SP_ID,
		CASE T.Category_Group WHEN 'TOTAL' THEN 'Total' ELSE M.Category_Desc END AS Category,
		T.NumOfTrans_C,
		CONVERT(INT, ROUND(T.NumOfTrans_C * 1.0/@TotalDays, 0)) AS AvgPerMonth,
		T.NumOfTrans_L,				
		CASE 
			WHEN T.NumOfTrans_L = 0 THEN 'N/A'
			WHEN ClaimPercentage < 0 THEN 'N/A'
			ELSE CONVERT(VARCHAR(10), ClaimPercentage) + '%' 
		END AS ClaimPercentage
	FROM	
		#TempFilteredSPTransaction T
	INNER JOIN	
		ServiceProvider SP ON T.SP_ID = SP.SP_ID
	LEFT JOIN	
		@VSSCategoryMapping M ON T.Category_Group = M.Category_Group
	ORDER BY 
		T.SP_ID ASC, T.Display_Seq ASC

	
	CLOSE SYMMETRIC KEY sym_Key   
	
	SELECT @ResultCount = COUNT(1) FROM #TempFilteredSPTransaction


	-- Line Break Before Remark
	INSERT INTO @TempResultTable (Result_Value1) VALUES ('')
	
	-- Remark
	INSERT INTO @TempResultTable (Result_Value1) VALUES ('Remark:')
	INSERT INTO @TempResultTable (Result_Value1, Result_Value2) VALUES ('*rounded to nearest whole number','')
	INSERT INTO @TempResultTable (Result_Value1, Result_Value2) VALUES ('(a) including claim transaction under VSS only','')
	INSERT INTO @TempResultTable (Result_Value1, Result_Value2) VALUES ('(c) including claim transaction under CIVSS, EVSS, PIDVSS and VSS','')
		
	
	-- Prepare Remark Sheet
	-- Lengend  
	INSERT INTO @RemarkTable (Result_Value1) Values ('(A) Legend')  

	-- Scheme  
	INSERT INTO @RemarkTable (Result_Value1) Values ('1. Scheme')  

	INSERT INTO @RemarkTable (Result_Value1, Result_Value2)
	SELECT Display_Code, Scheme_Desc  
	FROM SchemeClaim  
	ORDER BY Scheme_Code  

	-- Common Notes
	INSERT INTO @RemarkTable (Result_Value1) Values ('')  
	INSERT INTO @RemarkTable (Result_Value1) Values ('(B) Common Note(s) for the report')  	   
	INSERT INTO @RemarkTable (Result_Value1) Values ('1. All service providers are counted including delisted status and those in the exception list.')  	   
	INSERT INTO @RemarkTable (Result_Value1) Values ('2. Invalidated transactions, removed and voided transactions are excluded.')  	   
	INSERT INTO @RemarkTable (Result_Value1) Values ('3. % will be rounded up to nearest whole number.')
	   
	-- Definition of an Aberrant Case
	INSERT INTO @RemarkTable (Result_Value1) Values ('')  
	INSERT INTO @RemarkTable (Result_Value1) Values ('(C) Definition of an Aberrant Case')	   
	INSERT INTO @RemarkTable (Result_Value1) Values ('1. No .of transaction claimed for a SP in reporting month >= ' + CONVERT(VARCHAR(10), @TargetNoOfTransaction))
	INSERT INTO @RemarkTable (Result_Value1, Result_Value2) Values ('','''- Include all practices')
	INSERT INTO @RemarkTable (Result_Value1, Result_Value2) Values ('','''- Counting on SIV claim transaction only (QIV, TIV)')
	INSERT INTO @RemarkTable (Result_Value1) Values ('2. No. of transaction for a SP in month of last calendar year > 0')
	INSERT INTO @RemarkTable (Result_Value1, Result_Value2) Values ('','''- Include all practices')
	INSERT INTO @RemarkTable (Result_Value1, Result_Value2) Values ('','''- Counting on SIV claim transaction only (QIV, TIV)')
	INSERT INTO @RemarkTable (Result_Value1) Values ('3. Aberrant case criteria')
	INSERT INTO @RemarkTable (Result_Value1, Result_Value2) Values ('','''- Include all practices')
	INSERT INTO @RemarkTable (Result_Value1, Result_Value2) Values ('','''- Total no. of transaction for a SP in reporting month >= ' + CONVERT(VARCHAR(10), @TargetClaimPercentage) + '% of same month of last calendar year')
	INSERT INTO @RemarkTable (Result_Value1, Result_Value2) Values ('','''- Or no. of transaction for a SP in a category is >= ' + CONVERT(VARCHAR(10), @TargetClaimPercentage) + '% of same month of last calendar year')

	
-- =============================================
-- Return results
-- =============================================	

	-- Report Parameter
	SELECT	
		CASE WHEN ISNULL(@ResultCount, 0) > 0 THEN 'Y' ELSE 'N' END AS 'HaveResult',
		CONVERT(varchar(11), DATEADD(d, -1, @target_period_to), 106) AS 'Date'
					
	-- Result Set 1: Table of Content
	SELECT	
		'Report Generation Time: ' + CONVERT(varchar, GETDATE(), 111) + ' ' + CONVERT(VARCHAR(5), GETDATE(), 108) AS Result_Value

	
	-- Result Set 2: Record Detail
	SELECT	
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6,
		Result_Value7
	FROM	
		@TempResultTable
	ORDER BY	
		Result_Seq

	-- Result Set 3: Remark
	SELECT
		Result_Value1,
		Result_Value2
	FROM	
		@RemarkTable
	ORDER BY 
		Seq
	
	
	DROP TABLE #TempTargetPeriodTransaction    
	DROP TABLE #TempLastYearPeriodTransaction
	DROP TABLE #SP_Filtered
	DROP TABLE #TempFilteredSPTransaction

    
END    
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSM0003_Report_get] TO HCVU

GO
