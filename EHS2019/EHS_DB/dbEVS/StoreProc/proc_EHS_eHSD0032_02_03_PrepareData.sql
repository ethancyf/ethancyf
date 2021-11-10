IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0032_02_03_PrepareData]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0032_02_03_PrepareData]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR# :			CRE21-017
-- Modified by:		Koala CHENG
-- Modified date:	29 Oct 2021
-- Description:		[03-QIV] Add column "No. of Reimbursed Transactions"
-- =============================================
-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	16 Jul 2020
-- CR. No			INT20-0025
-- Description:		(1) Add WITH (NOLOCK)
-- =============================================
-- ============================================
-- Modification History
-- Modified by:		Winnie SUEN	
-- Modified date:	04 Nov 2019
-- CR No.			CRE19-014 (Update vaccination checking report)
-- Description:		Change datatype TINYINT -> INT
-- =============================================
-- ============================================
-- Modification History
-- Modified by:		Winnie SUEN	
-- Modified date:	04 Oct 2019
-- CR No.			CRE19-001-05 (PPP 2019-20 - Report)
-- Description:		Rename PPP to PPP-PS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM		
-- Modified date:	16 Sep 2019
-- CR No.			CRE19-001
-- Description:		Change length of school code
-- =============================================
-- =============================================
-- CR No.:			CRE17-018-06
-- Author:			Winnie SUEN
-- Create date:		02 Oct 2018
-- Description:		PPP daily report - Sub report 02 & 03
-- =============================================    

CREATE Procedure [proc_EHS_eHSD0032_02_03_PrepareData]    
	@Cutoff_Dtm as DateTime    
AS    
BEGIN    
SET NOCOUNT ON;    

--DECLARE @Cutoff_Dtm as DateTime   
--SET @Cutoff_Dtm = '2018-11-20'
	-- =============================================    
	-- Declaration    
	-- =============================================

	DECLARE @result1 INT, @result2 INT, @result3 INT, @result4 INT, @result5 INT, @result6 INT, @result7 INT, @result8 INT, @result9 INT, @result10 INT
	, @result11 INT, @result12 INT, @result13 INT, @result14 INT, @result15 INT, @result16 INT

	--Determine Scheme seq    
	DECLARE @current_scheme_Seq INT
	DECLARE @schemeDate	DATETIME
	
	DECLARE @Scheme_Code AS VARCHAR(10)
	DECLARE @Report_ID AS VARCHAR(30)     
	
	DECLARE @SIV_ByCategory AS TABLE	-- SIV for current season
	(   
		Display_Seq		INT,
		Subsidize_Code	VARCHAR(10),
		Display_Code_For_Claim	VARCHAR(25),
		Category_Code	VARCHAR(10),
		Category_Seq	INT		
	)    	
	
	-- For SIV cursor
	DECLARE @SIV_Subsidize_Code		VARCHAR(10)
	DECLARE @SIV_Display_Code_For_Claim	VARCHAR(25)
	
	-- For Category cursor
	DECLARE @Category_Code	VARCHAR(10)
	DECLARE @Category_Seq	INT		
	DECLARE @QIV_Subsidize_Code		VARCHAR(10)
	--DECLARE @TIV_Subsidize_Code		VARCHAR(10)

	-- For Age Cursor
	DECLARE @Age VARCHAR(100)
	DECLARE @AgeDesc VARCHAR(100)
	DECLARE @DoseDesc VARCHAR(100)
	DECLARE @MustShow CHAR(1)
	DECLARE @AgeHeaderPrev VARCHAR(100) = ''

	-- For Age table
	DECLARE @ShowList VARCHAR(MAX)
	DECLARE @AgeHeaderList VARCHAR(MAX)
	DECLARE @DoseHeaderList VARCHAR(MAX)
	DECLARE @EmptyList VARCHAR(MAX)
		
	DECLARE @Current_Scheme_desc VARCHAR(20)
	DECLARE @Current_Season_Start_Dtm	DATETIME

	
	CREATE TABLE #account
	(    
		voucher_acc_id			char(15),    
		temp_voucher_acc_id		char(15),    
		identity_num			varchar(20),    
		doc_code				char(10),    
		dob						datetime     
	)      
	   
	-- =============================================    
	-- Validation     
	-- =============================================    
	-- =============================================    
	-- Initialization    
	-- =============================================    

	SET @Scheme_Code = 'PPP'  
	SET @Report_ID = 'eHSD0032'  
	SET @schemeDate = DATEADD(dd, -1, @Cutoff_Dtm)  

	EXEC @current_scheme_Seq = [proc_EHS_GetSchemeSeq_Stat] @Scheme_Code, @schemeDate  
	SELECT @Current_scheme_desc = Season_Desc FROM VaccineSeason WITH (NOLOCK) WHERE Scheme_Code = @Scheme_Code AND Scheme_Seq = @current_scheme_Seq AND Subsidize_Item_Code = 'SIV'
	SELECT @Current_Season_Start_Dtm = MIN(SG.Claim_Period_From)
	FROM SubsidizeGroupClaim  SG WITH (NOLOCK)
	INNER JOIN SubsidizeGroupClaimItemDetails SGD WITH (NOLOCK)
		ON SG.Scheme_Code = SGD.Scheme_Code
		AND SG.Scheme_Seq = SGD.Scheme_Seq
		AND SG.Subsidize_Code = SGD.Subsidize_Code
	WHERE SG.Scheme_Code = @Scheme_Code  
		AND SG.Scheme_Seq = @current_scheme_Seq

	-- Get all SIV of PPP with current season
	INSERT INTO @SIV_ByCategory (Display_Seq, Subsidize_Code, Display_Code_For_Claim, Category_Code, Category_Seq)    
	SELECT
		ROW_NUMBER () OVER (ORDER BY S.Display_Seq),
		S.Subsidize_Code,
		SGC.Display_Code_For_Claim,
		SCA.Category_Code,
		CC.Display_Seq
	FROM 
		[SubsidizeGroupClaim] SGC WITH (NOLOCK)
		INNER JOIN [Subsidize]	S WITH (NOLOCK)
			ON SGC.[Subsidize_Code] = S.[Subsidize_Code]
		INNER JOIN [SubsidizeGroupCategory] SCA WITH (NOLOCK)
			ON SCA.[Subsidize_Code] = S.[Subsidize_Code]
		INNER JOIN [ClaimCategory] CC WITH (NOLOCK)
			ON CC.Category_Code = SCA.Category_Code
	WHERE 
		SGC.[Scheme_Code] = @Scheme_Code		
		AND (S.[Subsidize_Item_Code] = 'SIV' AND SGC.[Scheme_Seq] = @current_scheme_Seq)
	ORDER BY S.Display_Seq

    EXEC [proc_SymmetricKey_open]

	-- =============================================    
	-- Table
	-- =============================================

	CREATE TABLE #Transaction		-- Transaction of PPP
	(    			
		voucher_acc_id			char(15),    
		temp_voucher_acc_id		char(15), 		
		Transaction_ID			VARCHAR(20),
		Service_Receive_Dtm		DATETIME,
		DOB						DATETIME,
		DOB_Adjust				DATETIME,
		Exact_DOB				CHAR(1),
		Dose					CHAR(20),
		SP_ID					CHAR(8),
		Subsidize_Code			VARCHAR(10),
		Subsidize_Item_Code		CHAR(10),
		SchoolCode				varchar(30),
		identity_num			varchar(20),
		doc_code				char(10),    
		--IsSIV					INT,
		--IsCurrentSeason		INT,
		Is1stDose				INT,
		Is2ndDose				INT,
		IsOnlyDose				INT,
		Record_Status			VARCHAR(1)
	)    

	CREATE TABLE #ResultTable_02   
	(    
		_display_seq INT,
		_result_value1 varchar(200) default '',    
		_result_value2 varchar(100) default '',    
		_result_value3 varchar(100) default '',    
		_result_value4 varchar(100) default '',    
		_result_value5 varchar(100) default '',    
		_result_value6 varchar(100) default '',    
		_result_value7 varchar(100) default '',    
		_result_value8 varchar(100) default '',    
		_result_value9 varchar(100) default '',    
		_result_value10 varchar(100) default '',    
		_result_value11 varchar(100) default '',    
		_result_value12 varchar(100) default '',    
		_result_value13 varchar(100) default '',    
		_result_value14 varchar(100) default '',    
		_result_value15 varchar(100) default '',
		_result_value16 varchar(100) default '',    
		_result_value17 varchar(100) default '',    
		_result_value18 varchar(100) default '',    
		_result_value19 varchar(100) default '',    
		_result_value20 varchar(100) default ''
	)

	CREATE TABLE #ResultTable_03   
	(    
		_display_seq INT,
		_result_value1 varchar(200) default '',    
		_result_value2 varchar(100) default '',    
		_result_value3 varchar(100) default '',    
		_result_value4 varchar(100) default '',    
		_result_value5 varchar(100) default '',    
		_result_value6 varchar(100) default '',    
		_result_value7 varchar(100) default '',    
		_result_value8 varchar(100) default '',    
		_result_value9 varchar(100) default '',    
		_result_value10 varchar(100) default ''
	)
	
	CREATE TABLE #ResultTable_02_age
	(    
		Display_Seq	INT, 
		Display_Code VARCHAR(50)
	)
	
	------------------------------------------------------------------------------------------	
	DECLARE @TempTransactionStatusFilter AS TABLE(
		Status_Name			CHAR(50)
		, Status_Value		CHAR(10)
	)	
		
	INSERT INTO @TempTransactionStatusFilter (Status_Name, Status_Value)
	SELECT	
			Status_Name, 
			Status_Value 
	FROM 
			StatStatusFilterMapping WITH (NOLOCK) 
	WHERE 
			(Report_id = 'ALL' OR Report_id = @Report_ID) 
			AND Table_Name = 'VoucherTransaction'
			AND (Effective_Date IS NULL OR Effective_Date <= @Cutoff_Dtm) 
			AND (Expiry_Date IS NULL OR Expiry_Date >= @Cutoff_Dtm)

						
	DECLARE @AgeDose table (
		Age			VARCHAR(100),
		AgeDesc		VARCHAR(100),
		DoseDesc	VARCHAR(100),
		MustShow	CHAR(1)
	)	
	    
	INSERT INTO @AgeDose (Age, AgeDesc, DoseDesc, MustShow)
	VALUES
	('_6M_6Y_1stDose', '6 months to less than 6 years', '1st dose', 'Y')
	,('_6M_6Y_2ndDose', '6 months to less than 6 years', '2nd dose', 'Y')
	,('_6M_6Y_OnlyDose', '6 months to less than 6 years', 'Only dose', 'Y')
	,('_6Y_9Y_1stDose', '6 years to less than 9 years', '1st dose', 'Y')
	,('_6Y_9Y_2ndDose', '6 years to less than 9 years', '2nd dose', 'Y')
	,('_6Y_9Y_OnlyDose', '6 years to less than 9 years', 'Only dose', 'Y')
	,('_9Y_12Y_1stDose', '9 years to less than 12 years', '1st dose', 'N')
	,('_9Y_12Y_2ndDose', '9 years to less than 12 years', '2nd dose', 'N')
	,('_9Y_12Y_OnlyDose', '9 years to less than 12 years', 'Only dose', 'Y')
	,('_12Y_1stDose', '>= 12 years', '1st dose', 'N')
	,('_12Y_2ndDose', '>= 12 years', '2nd dose', 'N')
	,('_12Y_OnlyDose', '>= 12 years', 'Only dose', 'Y')
		
	INSERT INTO #account
	(    
		voucher_acc_id,    
		temp_voucher_acc_id,    
		identity_num,    
		doc_code,    
		dob    
	)    
	SELECT 
		p.voucher_acc_id,    
		NULL,    
		convert(varchar, DecryptByKey(p.Encrypt_Field1)),    
		p.doc_code,    
		p.dob    
	FROM 
		voucheraccount va WITH (NOLOCK), personalinformation p WITH (NOLOCK)    
	WHERE 
		va.voucher_acc_id = p.voucher_acc_id    
		--and va.record_status <> 'D'    
		and va.create_dtm < @Cutoff_Dtm    
    
	INSERT INTO #account
	(    
		voucher_acc_id,    
		temp_voucher_acc_id,    
		identity_num,    
		doc_code,    
		dob    
	)    
	SELECT
		NULL,    
		p.voucher_acc_id,      
		convert(varchar, DecryptByKey(p.Encrypt_Field1)),    
		p.doc_code,    
		p.dob    
	FROM 
		tempvoucheraccount va WITH (NOLOCK), temppersonalinformation p WITH (NOLOCK)    
	WHERE 
		va.voucher_acc_id = p.voucher_acc_id    
		--and va.record_status <> 'D'    
		--and va.account_purpose in ('C', 'V')    
		and va.create_dtm < @Cutoff_Dtm    

-- ---------------------------------------------    
-- Prepare Column List for Dynamic SQL String    
-- ---------------------------------------------    
	DECLARE @pivot_table_column_header  varchar(MAX)
	DECLARE @pivot_table_column_list  varchar(MAX)
	DECLARE @pivot_table_column_sum  varchar(MAX)

	DECLARE @sql NVARCHAR(MAX)
   
	SELECT    
		@pivot_table_column_header = COALESCE(@pivot_table_column_header + ',', '') + '[' + Age + '] INT',
		@pivot_table_column_list = COALESCE(@pivot_table_column_list + ',', '') + '[' + Age + ']',
		@pivot_table_column_sum = COALESCE(@pivot_table_column_sum + ',', '') + 'ISNULL(SUM([' + Age + ']),0)'
	FROM @AgeDose     
 
	SET @pivot_table_column_header = @pivot_table_column_header + ',[Total] INT'   	
	SET @pivot_table_column_list = @pivot_table_column_list + ',[Total]'
	SET @pivot_table_column_sum = @pivot_table_column_sum + ',ISNULL(SUM([Total]),0)'
	SET @sql = 'ALTER TABLE #ResultTable_02_age ADD ' + @pivot_table_column_header 
 
	EXECUTE(@sql)  

	-- =============================================    
	-- Prepare Data
	-- =============================================  
	--Retrieve all transcation 
	
	-- ---------------------------------------------
	-- Validated
	-- ---------------------------------------------
	INSERT INTO #Transaction    
	(      
		voucher_acc_id,
		temp_voucher_acc_id,      
		Transaction_ID,   
		Service_Receive_Dtm,   
		DOB,   
		DOB_Adjust,   
		Exact_DOB,   
		Dose,   
		SP_ID,
		Subsidize_Code,
		Subsidize_Item_Code,
		SchoolCode,
		--IsSIV,
		--IsCurrentSeason,
		Is1stDose,
		Is2ndDose,
		IsOnlyDose,
		Record_Status
	)    
	SELECT    
		vt.Voucher_Acc_ID,
		vt.Temp_Voucher_Acc_ID,    
		VT.Transaction_ID,   
		VT.Service_Receive_Dtm,   
		VR.DOB,   
		VR.DOB,   
		VR.Exact_DOB,   
		D.Available_Item_Code,   
		VT.SP_ID,
		D.Subsidize_Code,
		D.Subsidize_Item_Code,
		AF.AdditionalFieldValueCode,
		--CASE WHEN Subsidize_Item_Code = 'SIV' THEN 1 ELSE 0 END,
		--CASE WHEN Service_Receive_Dtm >= @Current_Season_Start_Dtm THEN 1 ELSE 0 END,
		CASE WHEN D.Available_Item_Code = '1STDOSE' THEN 1 ELSE 0 END,
		CASE WHEN D.Available_Item_Code = '2NDDOSE' THEN 1 ELSE 0 END,
		CASE WHEN D.Available_Item_Code = 'ONLYDOSE' THEN 1 ELSE 0 END,
		VT.Record_Status
	FROM VoucherTransaction VT WITH (NOLOCK)
		INNER JOIN TransactionDetail D WITH (NOLOCK) on VT.Transaction_ID = D.Transaction_ID  
		INNER JOIN TransactionAdditionalField AF WITH (NOLOCK) ON VT.Transaction_ID = AF.Transaction_ID  AND AF.AdditionalFieldID = 'SchoolCode'
		INNER JOIN PersonalInformation VR WITH (NOLOCK)		 
			ON VT.Voucher_Acc_ID = VR.Voucher_Acc_ID     
				AND VT.Doc_Code = VR.Doc_Code         
				AND VT.Voucher_Acc_ID IS NOT NULL    
				AND VT.Voucher_Acc_ID <> ''    
		INNER JOIN VoucherAccount A WITH (NOLOCK)    
				ON VT.Voucher_Acc_ID = A.Voucher_Acc_ID     
	WHERE VT.Scheme_Code= @Scheme_Code
			AND VT.Transaction_Dtm <= @Cutoff_Dtm
			AND (D.scheme_seq = @current_scheme_Seq)   -- Current Season
			AND D.Subsidize_Code IN (SELECT Subsidize_Code FROM @SIV_ByCategory)
			AND VT.record_status not in 
				(SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Record_Status')
			AND (vt.Invalidation IS NULL OR vt.Invalidation NOT In     
				(SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Invalidation'))



	-- ---------------------------------------------
	-- Temp
	-- ---------------------------------------------
	INSERT INTO #Transaction    
	(      
		voucher_acc_id,
		temp_voucher_acc_id,   
		Transaction_ID,   
		Service_Receive_Dtm,   
		DOB,   
		DOB_Adjust,   
		Exact_DOB,   
		Dose,   
		SP_ID,
		Subsidize_Code,
		Subsidize_Item_Code,
		SchoolCode,
		--IsSIV,
		--IsCurrentSeason,
		Is1stDose,
		Is2ndDose,
		IsOnlyDose,
		Record_Status
	)    
	SELECT    
		vt.Voucher_Acc_ID,
		vt.Temp_Voucher_Acc_ID,
		VT.Transaction_ID,   
		VT.Service_Receive_Dtm,   
		TVR.DOB,   
		TVR.DOB,   
		TVR.Exact_DOB,   
		D.Available_Item_Code,   
		VT.SP_ID,
		D.Subsidize_Code,
		D.Subsidize_Item_Code,
		AF.AdditionalFieldValueCode,
		--CASE WHEN Subsidize_Item_Code = 'SIV' THEN 1 ELSE 0 END,
		--CASE WHEN Service_Receive_Dtm >= @Current_Season_Start_Dtm THEN 1 ELSE 0 END,
		CASE WHEN D.Available_Item_Code = '1STDOSE' THEN 1 ELSE 0 END,
		CASE WHEN D.Available_Item_Code = '2NDDOSE' THEN 1 ELSE 0 END,
		CASE WHEN D.Available_Item_Code = 'ONLYDOSE' THEN 1 ELSE 0 END,
		VT.Record_Status
	FROM VoucherTransaction VT WITH (NOLOCK)    
		INNER JOIN TransactionDetail D WITH (NOLOCK)     
			ON VT.Transaction_ID = D.Transaction_ID     
		INNER JOIN TransactionAdditionalField AF WITH (NOLOCK) ON VT.Transaction_ID = AF.Transaction_ID  AND AF.AdditionalFieldID = 'SchoolCode'
		INNER JOIN TempPersonalInformation TVR WITH (NOLOCK)     
			ON VT.Temp_Voucher_Acc_ID = TVR.Voucher_Acc_ID     
				AND (VT.Voucher_Acc_ID = '' OR VT.Voucher_Acc_ID IS NULL)    
				AND VT.Special_Acc_ID IS NULL    
				AND VT.Invalid_Acc_ID IS NULL    
				AND VT.Temp_Voucher_Acc_ID <> ''     
				AND VT.Temp_Voucher_Acc_ID IS NOT NULL     
				AND VT.Doc_Code = TVR.Doc_Code    
		INNER JOIN TempVoucherAccount A WITH (NOLOCK)    
			ON VT.Temp_Voucher_Acc_ID = A.Voucher_Acc_ID      
	WHERE VT.Scheme_Code= @Scheme_Code
			AND VT.Transaction_Dtm <= @Cutoff_Dtm
			AND (D.scheme_seq = @current_scheme_Seq   AND D.Subsidize_Code IN (SELECT Subsidize_Code FROM @SIV_ByCategory))
			AND VT.record_status not in 
				(SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Record_Status')
			AND (vt.Invalidation IS NULL OR vt.Invalidation NOT In     
				(SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Invalidation'))
	  
	-- ---------------------------------------------
	-- Special
	-- ---------------------------------------------
	INSERT INTO #Transaction    
	(      
		Voucher_Acc_ID,
		Temp_Voucher_Acc_ID,
		Transaction_ID,   
		Service_Receive_Dtm,   
		DOB,   
		DOB_Adjust,   
		Exact_DOB,   
		Dose,   
		SP_ID,
		Subsidize_Code,
		Subsidize_Item_Code,
		SchoolCode,
		--IsSIV,
		--IsCurrentSeason,
		Is1stDose,
		Is2ndDose,
		IsOnlyDose,
		Record_Status
	)    
	SELECT    
		vt.Voucher_Acc_ID,
		vt.Temp_Voucher_Acc_ID,
		VT.Transaction_ID,   
		VT.Service_Receive_Dtm,   
		TVR.DOB,   
		TVR.DOB,   
		TVR.Exact_DOB,   
		D.Available_Item_Code,   
		VT.SP_ID,
		D.Subsidize_Code,
		D.Subsidize_Item_Code,
		AF.AdditionalFieldValueCode,
		--CASE WHEN Subsidize_Item_Code = 'SIV' THEN 1 ELSE 0 END,
		--CASE WHEN Service_Receive_Dtm >= @Current_Season_Start_Dtm THEN 1 ELSE 0 END,
		CASE WHEN D.Available_Item_Code = '1STDOSE' THEN 1 ELSE 0 END,
		CASE WHEN D.Available_Item_Code = '2NDDOSE' THEN 1 ELSE 0 END,
		CASE WHEN D.Available_Item_Code = 'ONLYDOSE' THEN 1 ELSE 0 END,
		VT.Record_Status
	FROM VoucherTransaction VT WITH (NOLOCK)    
		INNER JOIN TransactionDetail D WITH (NOLOCK)     
			ON VT.Transaction_ID = D.Transaction_ID    
		INNER JOIN TransactionAdditionalField AF WITH (NOLOCK) ON VT.Transaction_ID = AF.Transaction_ID  AND AF.AdditionalFieldID = 'SchoolCode'
		INNER JOIN SpecialPersonalInformation TVR WITH (NOLOCK) ON VT.Special_Acc_ID = TVR.Special_Acc_ID     
			AND VT.Special_Acc_ID IS NOT NULL    
			AND (VT.Voucher_Acc_ID IS NULL OR VT.Voucher_Acc_ID = '')    
			AND VT.Invalid_Acc_ID IS NULL    
			AND VT.Doc_Code = TVR.Doc_Code     
		INNER JOIN SpecialAccount A WITH (NOLOCK)    
			ON VT.Special_Acc_ID = A.Special_Acc_ID    
	WHERE VT.Scheme_Code= @Scheme_Code
			AND VT.Transaction_Dtm <= @Cutoff_Dtm
			AND (D.scheme_seq = @current_scheme_Seq   AND D.Subsidize_Code IN (SELECT Subsidize_Code FROM @SIV_ByCategory))
			AND VT.record_status not in 
				(SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Record_Status')
			AND (vt.Invalidation IS NULL OR vt.Invalidation NOT In     
				(SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Invalidation'))

	--

	-- ---------------------------------------------
	-- Patch data
	-- ---------------------------------------------
	update #Transaction    
	set identity_num = a.identity_num,   
		doc_code = a.doc_code,
		dob = a.dob    
	from #account a, #Transaction T    
	where isnull(T.voucher_acc_id ,'') = ''    
	and isnull(a.voucher_acc_id ,'') = ''    
	and T.temp_voucher_acc_id = a.temp_voucher_acc_id    
    
	update #Transaction    
	set identity_num = a.identity_num,   
		doc_code = a.doc_code,
		dob = a.dob     
	from #account a, #Transaction T   
	where isnull(T.voucher_acc_id ,'') <> ''    
	and isnull(a.voucher_acc_id ,'') <> ''    
	and T.voucher_acc_id = a.voucher_acc_id   

		
	-- DOB
	UPDATE	#Transaction
	SET		DOB = CONVERT(varchar, YEAR(DOB)) + '-' + CONVERT(varchar, MONTH(DOB)) + '-' + CONVERT(varchar, DAY(DATEADD(d, -DAY(DATEADD(m, 1, DOB)), DATEADD(m, 1, DOB))))
	WHERE	Exact_DOB IN ('M', 'U')

	UPDATE	#Transaction
	SET		DOB_Adjust = DOB

	UPDATE	#Transaction
	SET		DOB_Adjust = DATEADD(yyyy, 1, DOB)
	WHERE	MONTH(DOB) > MONTH(Service_receive_dtm)
			OR ( MONTH(DOB) = MONTH(Service_receive_dtm) AND DAY(DOB) > DAY(Service_receive_dtm) )
	
	
	-- Doc Code
	UPDATE #Transaction
	SET Doc_Code = 'HKIC'
	FROM #Transaction
	WHERE Doc_Code = 'HKBC'

	-- =============================================    
	-- Return results    
	-- ============================================= 
	-- insert record for the final output format    
	INSERT INTO #ResultTable_02 (_display_seq, _result_value1)    
	VALUES (1, REPLACE('eHS(S)D0032-02: Report on yearly PPP-PS claim transaction by age group and target group ([DATE])', '[DATE]', @current_scheme_desc))    
	INSERT INTO #ResultTable_02 (_display_seq)    
	VALUES (2)    
	INSERT INTO #ResultTable_02 (_display_seq, _result_value1)    
	VALUES (3, 'Reporting period: as at ' + FORMAT(DATEADD(dd, -1, @Cutoff_Dtm), 'yyyy/MM/dd'))
	INSERT INTO #ResultTable_02 (_display_seq)    
	VALUES (4)  

	-----------------------------------------
	-- (i) Seasonal Influenza Vaccination
	-----------------------------------------
	DECLARE @Row AS INT = 15

	INSERT INTO #ResultTable_02 (_display_seq, _result_value1) 
	VALUES (@Row, REPLACE('(i) Seasonal Influenza Vaccination ([DATE])', '[DATE]',@current_scheme_desc)) 
	
	SET @Row = @Row + 1
	    	
	-- SIV --		
	DECLARE Category_Cursor CURSOR FOR   
	SELECT DISTINCT Category_Code, Category_Seq FROM @SIV_ByCategory  ORDER BY Category_Seq

	OPEN Category_Cursor  
		FETCH NEXT FROM Category_Cursor INTO @Category_Code, @Category_Seq	
		   
		WHILE @@FETCH_STATUS  = 0 
		BEGIN  		
			SELECT @QIV_Subsidize_Code = Subsidize_Code
			FROM @SIV_ByCategory 
			WHERE Category_Code = @Category_Code AND Subsidize_Code LIKE '%QIV'

			--SELECT @TIV_Subsidize_Code = Subsidize_Code
			--FROM @SIV_ByCategory 
			--WHERE Category_Code = @Category_Code AND Subsidize_Code LIKE '%TIV'			
			
			INSERT INTO #ResultTable_02 (_display_seq) VALUES (@Row)		
			SET @Row = @Row + 1

			-- Header --				
			INSERT INTO #ResultTable_02 (_display_seq, _result_value1, _result_value2, _result_value3)
			VALUES	(@Row, '', '', 'No. of SP involved')
				
			UPDATE #ResultTable_02
			SET _result_value1 = (	SELECT Display_Code_For_claim 
									FROM @SIV_ByCategory 
									WHERE Subsidize_Code = @QIV_Subsidize_Code)
			WHERE _display_seq = @Row
		
			--UPDATE #ResultTable_02
			--SET _result_value2 = (	SELECT Display_Code_For_claim 
			--						FROM @SIV_ByCategory 
			--						WHERE Subsidize_Code = @TIV_Subsidize_Code)
			--WHERE _display_seq = @Row
	
			SET @Row = @Row + 1
		
			-- QIV --		
			SET @result1 = (SELECT COUNT(1) FROM #Transaction WHERE Subsidize_Code = @QIV_Subsidize_Code) -- AND IsSIV = 1 AND IsCurrentSeason = 1

			---- TIV --
			--SET @result2 = (SELECT COUNT(1) FROM #Transaction WHERE IsSIV = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = @TIV_Subsidize_Code)												

			-- No. of SP Involved
			SET @result3 = (SELECT COUNT(DISTINCT SP_ID) 
							FROM #Transaction 
							WHERE Subsidize_Code = @QIV_Subsidize_Code)
							--AND IsSIV = 1 AND IsCurrentSeason = 1
							--AND Subsidize_Code IN (@QIV_Subsidize_Code, @TIV_Subsidize_Code))
		

			INSERT INTO #ResultTable_02 (_display_seq, _result_value1, _result_value2,_result_value3)
			VALUES 
				(@Row, @result1, '', @result3)

		
			FETCH NEXT FROM Category_Cursor INTO @Category_Code, @Category_Seq
		END    
	CLOSE Category_Cursor  
	DEALLOCATE Category_Cursor  
	
/*
	-- Total
	-- start from display_seq = 46
	INSERT INTO #ResultTable_02 (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5)
	VALUES (46, 'QIV Total', 'TIV Total', 'Total', '', 'Total No. of SP involved')
			    
	INSERT INTO #ResultTable_02 (_display_seq) VALUES (47)
			    
	UPDATE #ResultTable_02 
	SET _result_value1 = (SELECT COUNT(1) FROM #Transaction WHERE IsSIV = 1 AND IsCurrentSeason = 1 AND Subsidize_Code LIKE '%QIV')
	WHERE _display_seq = 47

	UPDATE #ResultTable_02 
	SET _result_value2 = (SELECT COUNT(1) FROM #Transaction WHERE IsSIV = 1 AND IsCurrentSeason = 1 AND Subsidize_Code LIKE '%TIV')
	WHERE _display_seq = 47
				
	UPDATE #ResultTable_02 
	SET _result_value3 = (SELECT COUNT(1) FROM #Transaction WHERE IsSIV = 1 AND IsCurrentSeason = 1 AND (Subsidize_Code LIKE '%QIV' OR Subsidize_Code LIKE '%TIV'))
	WHERE _display_seq = 47

	UPDATE #ResultTable_02 
	SET _result_value5 = (SELECT COUNT(DISTINCT SP_ID) FROM #Transaction WHERE IsSIV = 1 AND IsCurrentSeason = 1)
	WHERE _display_seq = 47
*/	
	INSERT INTO #ResultTable_02 (_display_seq) VALUES (48)
	INSERT INTO #ResultTable_02 (_display_seq) VALUES (49)

	-----------------------------------------
	-- (ii) By age group  
	----------------------------------------- 

	-- SIV --	(60 - 90)		
	SET @Row = 60  -- Start from display_seq 60	

	DECLARE SIV_Cursor CURSOR FOR   
	SELECT Subsidize_Code, Display_Code_For_Claim FROM @SIV_ByCategory  

	OPEN SIV_Cursor  
	FETCH NEXT FROM SIV_Cursor INTO @SIV_Subsidize_Code, @SIV_Display_Code_For_Claim
	   
	WHILE @@FETCH_STATUS = 0  
	BEGIN  
						
		-- By Age
		-- 6m - <6y
		SET @result1 = (SELECT COUNT(1) FROM #Transaction 
						WHERE subsidize_code = @SIV_Subsidize_Code AND Is1stDose = 1
						AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 6)
						--AND IsSIV = 1 AND IsCurrentSeason = 1 

		SET @result2 = (SELECT COUNT(1) FROM #Transaction 
						WHERE subsidize_code = @SIV_Subsidize_Code AND Is2ndDose = 1
						AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 6)
						--AND IsSIV = 1 AND IsCurrentSeason = 1 

		SET @result3 = (SELECT COUNT(1) FROM #Transaction 
						WHERE subsidize_code = @SIV_Subsidize_Code AND IsOnlyDose = 1
						AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 6)
						--AND IsSIV = 1 AND IsCurrentSeason = 1 

		-- 6y - <9y
		SET @result4 = (SELECT COUNT(1) FROM #Transaction 
						WHERE subsidize_code = @SIV_Subsidize_Code AND Is1stDose = 1
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 6 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9)
						--AND IsSIV = 1 AND IsCurrentSeason = 1 	
		SET @result5 = (SELECT COUNT(1) FROM #Transaction 
						WHERE subsidize_code = @SIV_Subsidize_Code AND Is2ndDose = 1
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 6 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9)
						--AND IsSIV = 1 AND IsCurrentSeason = 1 

		SET @result6 = (SELECT COUNT(1) FROM #Transaction 
						WHERE subsidize_code = @SIV_Subsidize_Code AND IsOnlyDose = 1
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 6 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9)
						--AND IsSIV = 1 AND IsCurrentSeason = 1	

		-- 9y - <12y
		SET @result7 = (SELECT COUNT(1) FROM #Transaction 
						WHERE subsidize_code = @SIV_Subsidize_Code AND Is1stDose = 1
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 12)
						--AND IsSIV = 1 AND IsCurrentSeason = 1 

		SET @result8 = (SELECT COUNT(1) FROM #Transaction 
						WHERE subsidize_code = @SIV_Subsidize_Code AND Is2ndDose = 1
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 12)
						--AND IsSIV = 1 AND IsCurrentSeason = 1 

		SET @result9 = (SELECT COUNT(1) FROM #Transaction 
						WHERE subsidize_code = @SIV_Subsidize_Code AND IsOnlyDose = 1
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 12)
						--AND IsSIV = 1 AND IsCurrentSeason = 1 

		-- >=12y
		SET @result10 = (SELECT COUNT(1) FROM #Transaction 
						WHERE subsidize_code = @SIV_Subsidize_Code AND Is1stDose = 1
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 12)
						--AND IsSIV = 1 AND IsCurrentSeason = 1 

		SET @result11 = (SELECT COUNT(1) FROM #Transaction 
						WHERE subsidize_code = @SIV_Subsidize_Code AND Is2ndDose = 1
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 12)
						--AND IsSIV = 1 AND IsCurrentSeason = 1 

		SET @result12 = (SELECT COUNT(1) FROM #Transaction 
						WHERE subsidize_code = @SIV_Subsidize_Code AND IsOnlyDose = 1
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 12)
						--AND IsSIV = 1 AND IsCurrentSeason = 1 	
		-- Total
		SET @result13 = @result1 + @result2 + @result3 + @result4 + @result5 + @result6 + @result7 + @result8 + @result9 + @result10
						 + @result11 + @result12
		SET @sql = 'INSERT INTO #ResultTable_02_age (Display_Seq, Display_Code,' + @pivot_table_column_list + ') ' +
					'VALUES (' + CONVERT(VARCHAR,@Row) + ', ''' + @SIV_Display_Code_For_Claim + ''', ' +  
					CONVERT(VARCHAR, @result1) + ', ' + CONVERT(VARCHAR, @result2) + ', ' + CONVERT(VARCHAR, @result3) + ', ' +
					CONVERT(VARCHAR, @result4) + ', ' + CONVERT(VARCHAR, @result5) + ', ' + CONVERT(VARCHAR, @result6) + ', ' +
					CONVERT(VARCHAR, @result7) + ', ' + CONVERT(VARCHAR, @result8) + ', ' + CONVERT(VARCHAR, @result9) + ', ' +
					CONVERT(VARCHAR, @result10) + ', ' + CONVERT(VARCHAR, @result11) + ', ' + CONVERT(VARCHAR, @result12) + ', ' +
					CONVERT(VARCHAR, @result13) + ')'
		EXEC (@sql)

		-- Next SIV
		SET @Row = @Row + 1

		FETCH NEXT FROM SIV_Cursor INTO @SIV_Subsidize_Code, @SIV_Display_Code_For_Claim
	END  
  
	CLOSE SIV_Cursor  
	DEALLOCATE SIV_Cursor  
	
		
	-- Total of QIV (91)
	SET @sql = 'INSERT INTO #ResultTable_02_age (Display_Seq, Display_Code,' + @pivot_table_column_list + ') ' +
				'SELECT 91, ''Total of QIV'', ' + @pivot_table_column_sum + ' FROM #ResultTable_02_age WHERE Display_Seq>= 60 AND Display_Seq < 90'

	EXEC (@sql)

	-- End of SIV
				
	
	-- Column 
	DECLARE Age_Cursor CURSOR FOR   
	SELECT Age, AgeDesc, DoseDesc, MustShow FROM @AgeDose  

	OPEN Age_Cursor  
	FETCH NEXT FROM Age_Cursor INTO @Age, @AgeDesc, @DoseDesc, @MustShow
	   
	WHILE @@FETCH_STATUS = 0   
	BEGIN
		DECLARE @TotalCount as INT

		-- Total Count
		SET @sql = 'SELECT @TotalCount = SUM(' + @Age + ') FROM #ResultTable_02_age '
		SET @sql = @sql + ' WHERE Display_Seq = 91'
		EXEC sp_executesql @sql, N'@TotalCount INT OUTPUT', @TotalCount = @TotalCount OUTPUT
		
		IF @MustShow = 'Y' OR @TotalCount > 0 
		BEGIN
			
			-- Update Age Header
			IF @AgeDesc <> @AgeHeaderPrev
				SET @AgeHeaderPrev = @AgeDesc	
			ELSE
				SET @AgeDesc = ''
						
			-- Show List
			SET @ShowList = COALESCE(@ShowList + ',', '') + '[' + @Age + ']'			
			SET @AgeHeaderList = COALESCE(@AgeHeaderList + ',', '') + '''' + @AgeDesc + ''''
			SET @DoseHeaderList = COALESCE(@DoseHeaderList + ',', '') + '''' + @DoseDesc + ''''
		END
		ELSE
		BEGIN
			SET @EmptyList = ISNULL(@EmptyList,'') + ', '''''
		END

		FETCH NEXT FROM Age_Cursor INTO @Age, @AgeDesc, @DoseDesc, @MustShow
	END  
  
	CLOSE Age_Cursor  
	DEALLOCATE Age_Cursor  
	
	-- Insert to result table
	SET @ShowList = COALESCE(@ShowList + ',[Total]', '[Total]')
	SET @AgeHeaderList = COALESCE(@AgeHeaderList + ',''Total''', '''Total''')
	SET @DoseHeaderList = COALESCE(@DoseHeaderList + ',''''', '''''')

	-- (ii) By age group (50)
	INSERT INTO #ResultTable_02 (_display_seq, _result_value1)     
	VALUES (50, REPLACE('(ii) By age group ([DATE])', '[DATE]',@current_scheme_desc))  

	-- Age Header (51)
	SET @sql = 'INSERT INTO #ResultTable_02 (	_display_seq, _result_value1, _result_value2, _result_value3, _result_value4,   
				_result_value5, _result_value6, _result_value7, _result_value8, _result_value9, _result_value10,   
				_result_value11, _result_value12, _result_value13, _result_value14
				) VALUES (51, '''',' + @AgeHeaderList + @EmptyList + ')'
	EXEC (@sql)
	
	-- Dose Header (52)
	SET @sql = 'INSERT INTO #ResultTable_02 (	_display_seq, _result_value1, _result_value2, _result_value3, _result_value4,   
				_result_value5, _result_value6, _result_value7, _result_value8, _result_value9, _result_value10,   
				_result_value11, _result_value12, _result_value13, _result_value14
				) VALUES (52, '''',' + @DoseHeaderList + @EmptyList + ')'
	EXEC (@sql)	

	-- Result (60 - 91)
	SET @sql = 'INSERT INTO #ResultTable_02 (	_display_seq, _result_value1, _result_value2, _result_value3, _result_value4,   
				_result_value5, _result_value6, _result_value7, _result_value8, _result_value9, _result_value10,   
				_result_value11, _result_value12, _result_value13, _result_value14
				) SELECT Display_Seq, Display_Code,' + @ShowList + @EmptyList + ' FROM #ResultTable_02_age'
	EXEC (@sql)

	-- Fill Empty Row (90)
	INSERT INTO #ResultTable_02 (_display_seq) VALUES (90)

	INSERT INTO #ResultTable_02 (_display_seq) VALUES (92)
	INSERT INTO #ResultTable_02 (_display_seq) VALUES (93)

	-- Fill Remark
	INSERT INTO #ResultTable_02 (_display_seq, _result_value1)  
	VALUES (200, 'Remark:')	
	INSERT INTO #ResultTable_02 (_display_seq, _result_value1)  
	VALUES (201, 'a. Age = specific date - DOB')	
		




	-- =============================================    
	-- eHS(S)D0032-03: Report on yearly PPP claim transaction by school code (Current Season)
	-- ============================================= 

	-- insert record for the final output format    
	INSERT INTO #ResultTable_03 (_display_seq, _result_value1)    
	VALUES (1, REPLACE('eHS(S)D0032-03: Report on yearly PPP-PS claim transaction by school code ([DATE])', '[DATE]', @current_scheme_desc))    
	INSERT INTO #ResultTable_03 (_display_seq)    
	VALUES (2)    
	INSERT INTO #ResultTable_03 (_display_seq, _result_value1)    
	VALUES (3, 'Reporting period: as at ' + FORMAT(DATEADD(dd, -1, @Cutoff_Dtm), 'yyyy/MM/dd'))
	INSERT INTO #ResultTable_03 (_display_seq)    
	VALUES (4)  

	INSERT INTO #ResultTable_03 (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5, _result_value6, _result_value7)
	VALUES (5, 'School Code', '1st dose', '2nd dose', 'Only dose', 'Total', 'No. of Students', 'No. of Reimbursed Transactions')

	INSERT INTO #ResultTable_03 (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5, _result_value6, _result_value7)
	SELECT 
		10 + ROW_NUMBER () OVER (ORDER BY T.SchoolCode),
		T.SchoolCode, 
		SUM(T.Is1stDose) AS [1stDose],
		SUM(T.Is2ndDose) AS [2ndDose],
		SUM(T.IsOnlyDose) AS [OnlyDose],
		SUM(T.NoOfTrans) AS [Total],
		SUM(T.NoOfStudents) AS [NoOfStudents],
		SUM(T.NoOfReimbursedTrans) as [NoOfReimbursedTrans]
	FROM
		(
			SELECT 
				SchoolCode,
				doc_code, 
				COUNT(Transaction_ID) [NoOfTrans], 
				COUNT(DISTINCT identity_num) [NoOfStudents], -- Same doc code + doc id consider as 1 student
				SUM(Is1stDose) [Is1stDose],
				SUM(Is2ndDose) [Is2ndDose],
				SUM(IsOnlyDose) [IsOnlyDose],
				SUM(CASE WHEN Record_Status = 'R' THEN 1 ELSE 0 END) [NoOfReimbursedTrans]
			FROM
				#Transaction
			GROUP BY
				SchoolCode, doc_code
		) T
	GROUP BY
		T.SchoolCode

	-- =============================================    
	-- Final Result
	-- ============================================= 

	DELETE FROM RpteHSD0032_02_PPP_Tx_ByAgeGroup_ByYear   
	 
	INSERT INTO RpteHSD0032_02_PPP_Tx_ByAgeGroup_ByYear    
	(
		Display_Seq,    
		Col1,  Col2,  Col3,  Col4,  Col5,  
		Col6,  Col7,  Col8,  Col9,  Col10,   
		Col11, Col12, Col13, Col14, Col15, 
		Col16, Col17, Col18, Col19, Col20			
	)     
	SELECT  
		_display_seq,    
		_result_value1,  _result_value2,  _result_value3,  _result_value4,  _result_value5,   
		_result_value6,  _result_value7,  _result_value8,  _result_value9,  _result_value10,   
		_result_value11, _result_value12, _result_value13, _result_value14, _result_value15,
		_result_value16, _result_value17, _result_value18, _result_value19,	_result_value20 
	 FROM #ResultTable_02    
	 ORDER BY    
		_display_seq  
		
--

	DELETE FROM RpteHSD0032_03_PPP_Tx_BySchoolCode
	
	INSERT INTO RpteHSD0032_03_PPP_Tx_BySchoolCode    
	(
		Display_Seq,    
		Col1,  Col2,  Col3,  Col4,  Col5,  
		Col6,  Col7,  Col8,  Col9,  Col10	
	)     
	SELECT  
		_display_seq,    
		_result_value1,  _result_value2,  _result_value3,  _result_value4,  _result_value5,   
		_result_value6,  _result_value7,  _result_value8,  _result_value9,  _result_value10
	 FROM #ResultTable_03    
	 ORDER BY    
		_display_seq     
	   
	    
	EXEC [proc_SymmetricKey_close]

	DROP TABLE #account
	DROP TABLE #Transaction 
	    
	DROP TABLE #ResultTable_02    
	DROP TABLE #ResultTable_02_age

	DROP TABLE #ResultTable_03
    
END    
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0032_02_03_PrepareData] TO HCVU

GO

