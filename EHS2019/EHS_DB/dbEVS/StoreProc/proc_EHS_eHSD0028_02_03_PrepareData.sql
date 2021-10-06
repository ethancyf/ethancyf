IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0028_02_03_PrepareData]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0028_02_03_PrepareData]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR# :			CRE21-010
-- Modified by:		Martin Tang
-- Modified date:	08 Sep 2021
-- Description:		VSS(2021 - 2022) Remove TIV and Add RIV
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
-- Modified by:		Chris YIM
-- Modified date:	7 Aug 2020
-- CR No.			CRE19-031 (VSS MMR Upload)
-- Description:		Add MMR-NIA
-- =============================================
-- ============================================= 
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	4 Oct 2019
-- CR No.:			CRE19-001
-- Description:		Add LAIV
-- ============================================= 
-- ============================================= 
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	8 Aug 2019
-- CR No.:			CRE19-005
-- Description:		Add MMR
-- ============================================= 
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	24 Sep 2018
-- CR No.:			CRE17-018-06
-- Description:		Add QIV-A
--					Add 50 to 64 age year
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Dickson Law
-- Modified date:	09 Jan 2018
-- CR No.:			CRE14-016
-- Description:		Rename store proc [proc_EHS_eHSD0028_03_04_PrepareData] 
-- 					to [proc_EHS_eHSD0028_02_03_PrepareData]
-- =============================================  
-- =============================================  
-- CR No.:			CRE16-026
-- Modified by:     Marco CHOI
-- Modified date:   3 Aug 2017 
-- Description:     Add PCV13 
-- =============================================    
-- =============================================
-- CR No.:			CRE16-002-04
-- Author:			Winnie SUEN
-- Create date:		31 Aug 2016
-- Description:		Revamp VSS
-- =============================================    

CREATE Procedure [proc_EHS_eHSD0028_02_03_PrepareData]    
	@Cutoff_Dtm as DateTime    
AS    
BEGIN    
SET NOCOUNT ON;    

--DECLARE @Cutoff_Dtm as DateTime   
--SET @Cutoff_Dtm = '2017-09-19'
	-- =============================================    
	-- Declaration    
	-- =============================================
	DECLARE @Str_HighRisk varchar(4000)	
	SET @Str_HighRisk = 'High Risk'

	DECLARE @result1 INT, @result2 INT, @result3 INT, @result4 INT, @result5 INT, @result6 INT, @result7 INT, @result8 INT, @result9 INT, @result10 INT
	, @result11 INT, @result12 INT, @result13 INT, @result14 INT, @result15 INT, @result16 INT, @result17 INT, @result18 INT, @result19 INT, @result20 INT
	, @result21 INT, @result22 INT, @result23 INT, @result24 INT, @result25 INT, @result26 INT, @result27 INT, @result28 INT, @result29 INT, @result30 INT
	, @result31 INT, @result32 INT, @result33 INT, @result34 INT, @result35 INT, @result36 INT, @result37 INT, @result38 INT, @result39 INT, @result40 INT
	, @result41 INT, @result42 INT
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
	DECLARE @LAIV_Subsidize_Code	VARCHAR(10)
	DECLARE @RIV_Subsidize_Code		VARCHAR(10)

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


	CREATE table  #temp_HCVS
	(    
		voucher_acc_id		CHAR(15),    
		temp_voucher_acc_id	CHAR(15),    
		transaction_id		VARCHAR(20),    
		Reason1				CHAR(1),    
		Reason2				CHAR(1),    
		identity_num		VARCHAR(20),    
		dob					DATETIME,    
		service_receive_dtm	DATETIME,    
		SP_ID				CHAR(8)    
	)  
	
	CREATE table #account(    
		voucher_acc_id		CHAR(15),    
		temp_voucher_acc_id	CHAR(15),    
		identity_num		VARCHAR(20),    
		doc_code			CHAR(10),    
		dob					DATETIME     
	)      
	   
	-- =============================================    
	-- Validation     
	-- =============================================    
	-- =============================================    
	-- Initialization    
	-- =============================================    

	SET @Scheme_Code = 'VSS'  
	SET @Report_ID = 'eHSD0028'  
	SET @schemeDate = DATEADD(dd, -1, @Cutoff_Dtm)  
	
	EXEC @current_scheme_Seq = [proc_EHS_GetSchemeSeq_Stat] @Scheme_Code, @schemeDate  
	SELECT @Current_scheme_desc = Season_Desc FROM VaccineSeason WHERE Scheme_Code = @Scheme_Code AND Scheme_Seq = @current_scheme_Seq AND Subsidize_Item_Code = 'SIV'
	SELECT @Current_Season_Start_Dtm = MIN(SG.Claim_Period_From)
	FROM SubsidizeGroupClaim  SG
	INNER JOIN SubsidizeGroupClaimItemDetails SGD
		ON SG.Scheme_Code = SGD.Scheme_Code
		AND SG.Scheme_Seq = SGD.Scheme_Seq
		AND SG.Subsidize_Code = SGD.Subsidize_Code
	WHERE SG.Scheme_Code = @Scheme_Code  
		AND SG.Scheme_Seq = @current_scheme_Seq

	-- Get all SIV of VSS with current season
	INSERT INTO @SIV_ByCategory (Display_Seq, Subsidize_Code, Display_Code_For_Claim, Category_Code, Category_Seq)    
	SELECT
		ROW_NUMBER () OVER (ORDER BY S.Display_Seq),
		S.Subsidize_Code,
		SGC.Display_Code_For_Claim,
		SCA.Category_Code,
		CC.Display_Seq
	FROM 
		[SubsidizeGroupClaim] SGC
		INNER JOIN [Subsidize]	S
			ON SGC.[Subsidize_Code] = S.[Subsidize_Code]
		INNER JOIN [SubsidizeGroupCategory] SCA
			ON SCA.[Subsidize_Code] = S.[Subsidize_Code]
		INNER JOIN [ClaimCategory] CC
			ON CC.Category_Code = SCA.Category_Code
	
	WHERE 
		SGC.[Scheme_Code] = @Scheme_Code		
		AND (S.[Subsidize_Item_Code] = 'SIV' AND SGC.[Scheme_Seq] = @current_scheme_Seq)
	ORDER BY CC.Display_Seq, SGC.Display_Seq


EXEC [proc_SymmetricKey_open] 

	-- =============================================    
	-- Table
	-- =============================================

	CREATE TABLE #temp_VSS		-- Transaction with SIV and PV
	(    			
		voucher_acc_id			CHAR(15),    
		temp_voucher_acc_id		CHAR(15), 		
		Transaction_ID			VARCHAR(20),
		Service_Receive_Dtm		DATETIME,
		DOB						DATETIME,
		DOB_Adjust				DATETIME,
		Exact_DOB				CHAR(1),
		Dose					CHAR(20),
		SP_ID					CHAR(8),
		Subsidize_Code			VARCHAR(10),
		Subsidize_Item_Code		CHAR(10),
		identity_num			VARCHAR(20), 
		High_Risk				CHAR(1),
		IsWithVoucher			TINYINT,
		IsSIV					TINYINT,
		IsPV					TINYINT,
		IsPV13					TINYINT,
		IsMMR					TINYINT,
		IsCurrentSeason			TINYINT
	)    
	--CREATE INDEX IX_VSS on #temp_VSS (Transaction_ID)    


	CREATE TABLE #result_table   
	(    
		_display_seq tinyint,
		_result_value1 VARCHAR(200) default '',    
		_result_value2 VARCHAR(100) default '',    
		_result_value3 VARCHAR(100) default '',    
		_result_value4 VARCHAR(100) default '',    
		_result_value5 VARCHAR(100) default '',    
		_result_value6 VARCHAR(100) default '',    
		_result_value7 VARCHAR(100) default '',    
		_result_value8 VARCHAR(100) default '',    
		_result_value9 VARCHAR(100) default '',    
		_result_value10 VARCHAR(100) default '',    
		_result_value11 VARCHAR(100) default '',    
		_result_value12 VARCHAR(100) default '',    
		_result_value13 VARCHAR(100) default '',    
		_result_value14 VARCHAR(100) default '',    
		_result_value15 VARCHAR(100) default '',
		_result_value16 VARCHAR(100) default '',    
		_result_value17 VARCHAR(100) default '',    
		_result_value18 VARCHAR(100) default '',    
		_result_value19 VARCHAR(100) default '',    
		_result_value20 VARCHAR(100) default '', 		
		_result_value21 VARCHAR(100) default '',    
		_result_value22 VARCHAR(100) default '',    
		_result_value23 VARCHAR(100) default '',    
		_result_value24 VARCHAR(100) default '',    
		_result_value25 VARCHAR(100) default '',    
		_result_value26 VARCHAR(100) default '',    
		_result_value27 VARCHAR(100) default '',    
		_result_value28 VARCHAR(100) default '',    
		_result_value29 VARCHAR(100) default '',		
		_result_value30 VARCHAR(100) default '',
		_result_value31 VARCHAR(100) default '',
		_result_value32 VARCHAR(100) default '',
		_result_value33 VARCHAR(100) default '',
		_result_value34 VARCHAR(100) default '',
		_result_value35 VARCHAR(100) default '',
		_result_value36 VARCHAR(100) default '',
		_result_value37 VARCHAR(100) default '',
		_result_value38 VARCHAR(100) default '',
		_result_value39 VARCHAR(100) default '',
		_result_value40 VARCHAR(100) default '',
		_result_value41 VARCHAR(100) default '',
		_result_value42 VARCHAR(100) default ''
	)

	CREATE TABLE #Subresult_table   
	(    
		_display_seq tinyint,
		_result_value1 VARCHAR(200) default '',    
		_result_value2 VARCHAR(100) default '',    
		_result_value3 VARCHAR(100) default '',    
		_result_value4 VARCHAR(100) default '',    
		_result_value5 VARCHAR(100) default '',    
		_result_value6 VARCHAR(100) default '',    
		_result_value7 VARCHAR(100) default '',    
		_result_value8 VARCHAR(100) default '',    
		_result_value9 VARCHAR(100) default '',    
		_result_value10 VARCHAR(100) default '',    
		_result_value11 VARCHAR(100) default '',    
		_result_value12 VARCHAR(100) default '',    
		_result_value13 VARCHAR(100) default '',    
		_result_value14 VARCHAR(100) default '',    
		_result_value15 VARCHAR(100) default '',
		_result_value16 VARCHAR(100) default '',    
		_result_value17 VARCHAR(100) default '',    
		_result_value18 VARCHAR(100) default '',    
		_result_value19 VARCHAR(100) default '',    
		_result_value20 VARCHAR(100) default '', 		
		_result_value21 VARCHAR(100) default '',    
		_result_value22 VARCHAR(100) default '',    
		_result_value23 VARCHAR(100) default '',    
		_result_value24 VARCHAR(100) default '',    
		_result_value25 VARCHAR(100) default '',    
		_result_value26 VARCHAR(100) default '',    
		_result_value27 VARCHAR(100) default '',    
		_result_value28 VARCHAR(100) default '',    
		_result_value29 VARCHAR(100) default '',
		_result_value30 VARCHAR(100) default '',	
		_result_value31 VARCHAR(100) default '',	
		_result_value32 VARCHAR(100) default '',
		_result_value33 VARCHAR(100) default '',
		_result_value34 VARCHAR(100) default '',
		_result_value35 VARCHAR(100) default '',
		_result_value36 VARCHAR(100) default '',
		_result_value37 VARCHAR(100) default '',
		_result_value38 VARCHAR(100) default '',
		_result_value39 VARCHAR(100) default '',
		_result_value40 VARCHAR(100) default '',
		_result_value41 VARCHAR(100) default '',
		_result_value42 VARCHAR(100) default ''
	)
	
	CREATE TABLE #result_table_age
	(    
		Display_Seq tinyint, 
		Display_Code VARCHAR(50)
	)

	CREATE TABLE #result_table_age2
	(    
		Display_Seq tinyint, 
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
			StatStatusFilterMapping 
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
	,('_6M_6Y_3rdDose', '6 months to less than 6 years', '3rd dose', 'N')

	,('_6Y_9Y_1stDose', '6 years to less than 9 years', '1st dose', 'Y')
	,('_6Y_9Y_2ndDose', '6 years to less than 9 years', '2nd dose', 'Y')
	,('_6Y_9Y_OnlyDose', '6 years to less than 9 years', 'Only dose', 'Y')
	,('_6Y_9Y_3rdDose', '6 years to less than 9 years', '3rd dose', 'N')

	,('_9Y_12Y_1stDose', '9 years to less than 12 years', '1st dose', 'N')
	,('_9Y_12Y_2ndDose', '9 years to less than 12 years', '2nd dose', 'N')
	,('_9Y_12Y_OnlyDose', '9 years to less than 12 years', 'Only dose', 'Y')
	,('_9Y_12Y_3rdDose', '9 years to less than 12 years', '3rd dose', 'N')

	,('_12Y_16Y_1stDose', '12 years to less than 16 years', '1st dose', 'N')
	,('_12Y_16Y_2ndDose', '12 years to less than 16 years', '2nd dose', 'N')
	,('_12Y_16Y_OnlyDose', '12 years to less than 16 years', 'Only dose', 'Y')
	,('_12Y_16Y_3rdDose', '12 years to less than 16 years', '3rd dose', 'N')

	,('_16Y_50Y_1stDose', '16 years to less than 50 age year', '1st dose', 'N')
	,('_16Y_50Y_2ndDose', '16 years to less than 50 age year', '2nd dose', 'N')
	,('_16Y_50Y_OnlyDose', '16 years to less than 50 age year', 'Only dose', 'Y')
	,('_16Y_50Y_3rdDose', '16 years to less than 50 age year', '3rd dose', 'N')

	,('_50Y_64Y_1stDose', '50 to 64 age year', '1st dose', 'N')
	,('_50Y_64Y_2ndDose', '50 to 64 age year', '2nd dose', 'N')
	,('_50Y_64Y_OnlyDose', '50 to 64 age year', 'Only dose', 'Y')
	,('_50Y_64Y_3rdDose', '50 to 64 age year', '3rd dose', 'N')

	,('_65Y_1stDose', 'At 65 age year', '1st dose', 'N')
	,('_65Y_2ndDose', 'At 65 age year', '2nd dose', 'N')
	,('_65Y_OnlyDose', 'At 65 age year', 'Only dose', 'Y')
	,('_65Y_3rdDose', 'At 65 age year', '3rd dose', 'N')

	,('_66Y_69Y_1stDose', '66 to 69 age year', '1st dose', 'N')
	,('_66Y_69Y_2ndDose', '66 to 69 age year', '2nd dose', 'N')
	,('_66Y_69Y_OnlyDose', '66 to 69 age year', 'Only dose', 'Y')
	,('_66Y_69Y_3rdDose', '66 to 69 age year', '3rd dose', 'N')

	,('_70Y_79Y_1stDose', '70 to 79 age year', '1st dose', 'N')
	,('_70Y_79Y_2ndDose', '70 to 79 age year', '2nd dose', 'N')
	,('_70Y_79Y_OnlyDose', '70 to 79 age year', 'Only dose', 'Y')
	,('_70Y_79Y_3rdDose', '70 to 79 age year', '3rd dose', 'N')

	,('_80Y_1stDose', '>= 80 age year', '1st dose', 'N')
	,('_80Y_2ndDose', '>= 80 age year', '2nd dose', 'N')
	,('_80Y_OnlyDose', '>= 80 age year', 'Only dose', 'Y')
	,('_80Y_3rdDose', '>= 80 age year', 'Only dose', 'N')


	INSERT INTO #temp_HCVS(    
		 transaction_id,    
		 voucher_acc_id,    
		 temp_voucher_acc_id,    
		 service_receive_dtm,    
		 SP_ID    
	)  
	SELECT 
		Transaction_ID, 
		Voucher_Acc_ID, 
		Temp_Voucher_Acc_ID, 
		Service_Receive_Dtm, 
		SP_ID 
	FROM 
		VoucherTransaction    
	WHERE Scheme_Code = 'HCVS'    
		AND Service_Receive_Dtm < @Cutoff_Dtm 
		AND Service_Type = 'RMP'    
		AND Record_Status NOT IN (SELECT Status_Value 
									FROM StatStatusFilterMapping 
									WHERE (report_id = 'ALL' OR report_id = 'eHSD0028')     
										AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'     
										AND ((Effective_Date IS NULL OR Effective_Date <= @cutoff_dtm) AND ([Expiry_Date] IS NULL OR [Expiry_Date] >= @cutoff_dtm))
									)    
		AND (Invalidation IS NULL 
			OR Invalidation NOT IN (SELECT Status_Value 
									FROM StatStatusFilterMapping 
									WHERE (report_id = 'ALL' OR report_id = 'eHSD0028')     
										AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'    
										AND ((Effective_Date is null or Effective_Date <= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date >= @cutoff_dtm))
									)
			)
        
	UPDATE #temp_HCVS    
	SET Reason1 = 'Y'    
	FROM #temp_HCVS T, TransactionAdditionalField TAF    
	WHERE T.transaction_id = TAF.Transaction_ID     
		AND TAF.AdditionalfieldID = 'Reason_for_Visit_L1'     
		AND TAF.AdditionalFieldValueCode = '1' 

	UPDATE #temp_HCVS    
	SET Reason2 = 'Y'    
	FROM #temp_HCVS T, TransactionAdditionalField TAF    
	WHERE T.transaction_id = TAF.Transaction_ID     
		AND TAF.AdditionalFieldID = 'Reason_for_Visit_L2'     
		AND TAF.AdditionalFieldValueCode = '3' 

	INSERT INTO #account(    
		voucher_acc_id,    
		temp_voucher_acc_id,    
		identity_num,    
		doc_code,    
		dob    
	)    
	SELECT
		PInfo.Voucher_Acc_ID,    
		NULL,    
		CONVERT(VARCHAR, DecryptByKey(PInfo.Encrypt_Field1)),    
		PInfo.Doc_Code,    
		PInfo.DOB    
	FROM VoucherAccount VA, PersonalInformation PInfo    
	WHERE VA.Voucher_Acc_ID = PInfo.Voucher_Acc_ID    
		AND VA.Create_Dtm < @Cutoff_Dtm    
    
	INSERT INTO #account(    
		voucher_acc_id,    
		temp_voucher_acc_id,    
		identity_num,    
		doc_code,    
		dob    
	)      
	SELECT 
		NULL,    
		TPInfo.Voucher_Acc_ID,      
		CONVERT(VARCHAR, DecryptByKey(TPInfo.Encrypt_Field1)),    
		TPInfo.Doc_Code,    
		TPInfo.DOB    
	FROM TempVoucherAccount TVA, TempPersonalInformation TPInfo    
	WHERE TVA.Voucher_Acc_ID = TPInfo.Voucher_Acc_ID    
	and TVA.Create_Dtm < @Cutoff_Dtm    

-- ---------------------------------------------    
-- Prepare Column List for Dynamic SQL String    
-- ---------------------------------------------    
	DECLARE @pivot_table_column_header  VARCHAR(MAX)
	DECLARE @pivot_table_column_list  VARCHAR(MAX)
	DECLARE @pivot_table_column_sum  VARCHAR(MAX)

	DECLARE @sql NVARCHAR(MAX)
   
	SELECT    
		@pivot_table_column_header = COALESCE(@pivot_table_column_header + ',', '') + '[' + Age + '] INT',
		@pivot_table_column_list = COALESCE(@pivot_table_column_list + ',', '') + '[' + Age + ']',
		@pivot_table_column_sum = COALESCE(@pivot_table_column_sum + ',', '') + 'ISNULL(SUM([' + Age + ']),0)'
	FROM @AgeDose     
 
	SET @pivot_table_column_header = @pivot_table_column_header + ',[Total] INT'   	
	SET @pivot_table_column_list = @pivot_table_column_list + ',[Total]'
	SET @pivot_table_column_sum = @pivot_table_column_sum + ',ISNULL(SUM([Total]),0)'
	SET @sql = 'ALTER TABLE #result_table_Age ADD ' + @pivot_table_column_header 
 
	EXECUTE(@sql)  

	-- =============================================    
	-- Prepare Data
	-- =============================================  
	--Retrieve all transcation 

	-- ---------------------------------------------
	-- Validated
	-- ---------------------------------------------
	INSERT INTO #temp_VSS    
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
		High_Risk,
		IsSIV,
		IsPV,
		IsPV13,
		IsMMR,
		IsCurrentSeason			
	)    
	SELECT    
		VT.Voucher_Acc_ID,
		VT.Temp_Voucher_Acc_ID,    
		VT.Transaction_ID,   
		VT.Service_Receive_Dtm,   
		VR.DOB,   
		VR.DOB,   
		VR.Exact_DOB,   
		D.Available_Item_Code,   
		VT.SP_ID,
		D.Subsidize_Code,
		D.Subsidize_Item_Code,
		VT.High_Risk,
		CASE WHEN Subsidize_Item_Code = 'SIV' THEN 1 ELSE 0 END,
		CASE WHEN Subsidize_Item_Code = 'PV' THEN 1 ELSE 0 END,
		CASE WHEN Subsidize_Item_Code = 'PV13' THEN 1 ELSE 0 END,
		CASE WHEN Subsidize_Item_Code = 'MMR' THEN 1 ELSE 0 END,
		CASE WHEN Service_Receive_Dtm >= @Current_Season_Start_Dtm THEN 1 ELSE 0 END
	FROM VoucherTransaction VT
		INNER JOIN TransactionDetail D on VT.Transaction_ID = D.Transaction_ID     
		INNER JOIN PersonalInformation VR    
			ON VT.Voucher_Acc_ID = VR.Voucher_Acc_ID     
				AND VT.Doc_Code = VR.Doc_Code         
				AND VT.Voucher_Acc_ID IS NOT NULL    
				AND VT.Voucher_Acc_ID <> ''    
		INNER JOIN VoucherAccount A    
				ON VT.Voucher_Acc_ID = A.Voucher_Acc_ID     
	WHERE VT.Scheme_Code= @Scheme_Code
			AND VT.Transaction_Dtm <= @Cutoff_Dtm
			AND (D.Subsidize_Item_Code = 'PV' 
				OR D.Subsidize_Item_Code = 'PV13' 
				OR D.Subsidize_Item_Code = 'MMR' 
				OR (D.scheme_seq = @current_scheme_Seq   AND D.Subsidize_Code IN (SELECT Subsidize_Code FROM @SIV_ByCategory)))
			AND VT.record_status not in 
				(SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Record_Status')
			AND (vt.Invalidation IS NULL OR vt.Invalidation NOT In     
				(SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Invalidation'))


	-- ---------------------------------------------
	-- Temp
	-- ---------------------------------------------
	INSERT INTO #temp_VSS    
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
		High_Risk,
		IsSIV,
		IsPV,
		IsPV13,
		IsMMR,
		IsCurrentSeason
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
		VT.High_Risk,
		CASE WHEN Subsidize_Item_Code = 'SIV' THEN 1 ELSE 0 END,
		CASE WHEN Subsidize_Item_Code = 'PV' THEN 1 ELSE 0 END,
		CASE WHEN Subsidize_Item_Code = 'PV13' THEN 1 ELSE 0 END,
		CASE WHEN Subsidize_Item_Code = 'MMR' THEN 1 ELSE 0 END,
		CASE WHEN Service_Receive_Dtm >= @Current_Season_Start_Dtm THEN 1 ELSE 0 END
	FROM VoucherTransaction VT    
		INNER JOIN TransactionDetail D     
			ON VT.Transaction_ID = D.Transaction_ID     
		INNER JOIN TempPersonalInformation TVR     
			ON VT.Temp_Voucher_Acc_ID = TVR.Voucher_Acc_ID     
				AND (VT.Voucher_Acc_ID = '' OR VT.Voucher_Acc_ID IS NULL)    
				AND VT.Special_Acc_ID IS NULL    
				AND VT.Invalid_Acc_ID IS NULL    
				AND VT.Temp_Voucher_Acc_ID <> ''     
				AND VT.Temp_Voucher_Acc_ID IS NOT NULL     
				AND VT.Doc_Code = TVR.Doc_Code    
		INNER JOIN TempVoucherAccount A    
			ON VT.Temp_Voucher_Acc_ID = A.Voucher_Acc_ID      
	WHERE VT.Scheme_Code= @Scheme_Code
			AND VT.Transaction_Dtm <= @Cutoff_Dtm
			AND (D.Subsidize_Item_Code = 'PV' 
				OR D.Subsidize_Item_Code = 'PV13' 
				OR D.Subsidize_Item_Code = 'MMR' 
				OR (D.scheme_seq = @current_scheme_Seq   AND D.Subsidize_Code IN (SELECT Subsidize_Code FROM @SIV_ByCategory)))
			AND VT.record_status not in 
				(SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Record_Status')
			AND (vt.Invalidation IS NULL OR vt.Invalidation NOT In     
				(SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Invalidation'))
	  
	-- ---------------------------------------------
	-- Special
	-- ---------------------------------------------
	INSERT INTO #temp_VSS    
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
		High_Risk,
		IsSIV,
		IsPV,
		IsPV13,
		IsMMR,
		IsCurrentSeason
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
		VT.High_Risk,		
		CASE WHEN Subsidize_Item_Code = 'SIV' THEN 1 ELSE 0 END,
		CASE WHEN Subsidize_Item_Code = 'PV' THEN 1 ELSE 0 END,
		CASE WHEN Subsidize_Item_Code = 'PV13' THEN 1 ELSE 0 END,
		CASE WHEN Subsidize_Item_Code = 'MMR' THEN 1 ELSE 0 END,
		CASE WHEN Service_Receive_Dtm >= @Current_Season_Start_Dtm THEN 1 ELSE 0 END
	FROM VoucherTransaction VT    
		INNER JOIN TransactionDetail D     
			ON VT.Transaction_ID = D.Transaction_ID    
		INNER JOIN SpecialPersonalInformation TVR ON VT.Special_Acc_ID = TVR.Special_Acc_ID     
			AND VT.Special_Acc_ID IS NOT NULL    
			AND (VT.Voucher_Acc_ID IS NULL OR VT.Voucher_Acc_ID = '')    
			AND VT.Invalid_Acc_ID IS NULL    
			AND VT.Doc_Code = TVR.Doc_Code     
		INNER JOIN SpecialAccount A    
			ON VT.Special_Acc_ID = A.Special_Acc_ID    
	WHERE VT.Scheme_Code= @Scheme_Code
			AND VT.Transaction_Dtm <= @Cutoff_Dtm
			AND (D.Subsidize_Item_Code = 'PV' 
				OR D.Subsidize_Item_Code = 'PV13' 
				OR D.Subsidize_Item_Code = 'MMR' 
				OR (D.scheme_seq = @current_scheme_Seq   AND D.Subsidize_Code IN (SELECT Subsidize_Code FROM @SIV_ByCategory)))
			AND VT.record_status not in 
				(SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Record_Status')
			AND (vt.Invalidation IS NULL OR vt.Invalidation NOT In     
				(SELECT Status_Value FROM @TempTransactionStatusFilter WHERE Status_Name = 'Invalidation'))

	UPDATE #temp_HCVS    
	SET 
		identity_num = a.identity_num,    
		dob = a.dob    
	FROM #account a, #temp_HCVS v    
	WHERE ISNULL(v.voucher_acc_id ,'') = ''    
		AND ISNULL(a.voucher_acc_id ,'') = ''    
		AND v.temp_voucher_acc_id = a.temp_voucher_acc_id    
    
	UPDATE #temp_HCVS    
	SET 
		identity_num = a.identity_num,    
		dob = a.dob    
	FROM #account a, #temp_HCVS v    
	WHERE ISNULL(v.voucher_acc_id ,'') <> ''    
		AND ISNULL(a.voucher_acc_id ,'') <> ''    
		AND v.voucher_acc_id = a.voucher_acc_id     

	--
	-- ---------------------------------------------
	-- Patch data
	-- ---------------------------------------------
	UPDATE #temp_VSS    
	SET 
		identity_num = a.identity_num,    
		dob = a.dob    
	FROM #account a, #temp_VSS vss    
	WHERE ISNULL(vss.voucher_acc_id ,'') = ''    
		AND ISNULL(a.voucher_acc_id ,'') = ''    
		AND vss.temp_voucher_acc_id = a.temp_voucher_acc_id    
    
	UPDATE #temp_VSS    
	SET 
		identity_num = a.identity_num,    
		dob = a.dob    
	FROM #account a, #temp_VSS vss   
	WHERE ISNULL(vss.voucher_acc_id ,'') <> ''    
		AND ISNULL(a.voucher_acc_id ,'') <> ''    
		AND vss.voucher_acc_id = a.voucher_acc_id   

	UPDATE #temp_VSS
	SET 
		IsWithVoucher = 1
	FROM #temp_VSS vss, #temp_HCVS hcvs
	WHERE vss.Service_Receive_Dtm = hcvs.service_receive_dtm   
		AND vss.identity_num = hcvs.identity_num 
		AND (hcvs.Reason1='Y' AND hcvs.Reason2='Y')
		AND vss.SP_ID = hcvs.SP_ID
		

	UPDATE	#temp_VSS
	SET		DOB = CONVERT(VARCHAR, YEAR(DOB)) + '-' + CONVERT(VARCHAR, MONTH(DOB)) + '-' + CONVERT(VARCHAR, DAY(DATEADD(d, -DAY(DATEADD(m, 1, DOB)), DATEADD(m, 1, DOB))))
	WHERE	Exact_DOB IN ('M', 'U')

	UPDATE	#temp_VSS
	SET		DOB_Adjust = DOB

	UPDATE	#temp_VSS
	SET		DOB_Adjust = DATEADD(yyyy, 1, DOB)
	WHERE	MONTH(DOB) > MONTH(Service_receive_dtm)
			OR ( MONTH(DOB) = MONTH(Service_receive_dtm) AND DAY(DOB) > DAY(Service_receive_dtm) )

	--SELECT * FROM #temp_VSS WHERE Subsidize_Code = 'VNIAMMR'

--SELECT
--		Transaction_ID,    
--		Service_Receive_Dtm,    
--		DOB,    
--		DOB_Adjust,    
--		Exact_DOB,    
--		Dose,    
--		SP_ID,
--		Subsidize_Code,
--		Subsidize_Item_Code
--	--INTO #temp_VSSSIV
--	FROM
--		#temp_VSS
	--SELECT
	--	Transaction_ID,    
	--	Service_Receive_Dtm,    
	--	DOB,    
	--	DOB_Adjust,    
	--	Exact_DOB,    
	--	Dose,    
	--	SP_ID,
	--	Subsidize_Code,
	--	Subsidize_Item_Code
	--INTO #temp_VSSSIV
	--FROM
	--	#temp_VSS
	--WHERE
	--	#temp_VSS.Subsidize_Item_Code = 'SIV'		
  
 --   --23vPPV
	--SELECT
	--	Transaction_ID,    
	--	Service_Receive_Dtm,    
	--	DOB,    
	--	DOB_Adjust,    
	--	Exact_DOB,    
	--	Dose,    
	--	SP_ID,
	--	Subsidize_Code,
	--	Subsidize_Item_Code,
	--	High_Risk
	--INTO #temp_VSSPV
	--FROM
	--	#temp_VSS
	--WHERE
	--	#temp_VSS.Subsidize_Item_Code = 'PV'	
	
	----PCV13
	--SELECT
	--	Transaction_ID,    
	--	Service_Receive_Dtm,    
	--	DOB,    
	--	DOB_Adjust,    
	--	Exact_DOB,    
	--	Dose,    
	--	SP_ID,
	--	Subsidize_Code,
	--	Subsidize_Item_Code,
	--	High_Risk,
	--	withVoucher
	--INTO #temp_VSSPV13
	--FROM
	--	#temp_VSS
	--WHERE
	--	#temp_VSS.Subsidize_Item_Code = 'PV13'	
	
	-- =============================================    
	-- Return results    
	-- ============================================= 
	-- insert record for the final output format    
	INSERT INTO #result_table (_display_seq, _result_value1)    
	VALUES (1, REPLACE('eHS(S)D0028-02: Report on yearly VSS claim transaction by age group and target group ([DATE])', '[DATE]', @current_scheme_desc))    
	INSERT INTO #result_table (_display_seq)    
	VALUES (2)    
	INSERT INTO #result_table (_display_seq, _result_value1)    
	VALUES (3, 'Reporting period: as at ' + CONVERT(VARCHAR, DATEADD(dd, -1, @Cutoff_Dtm), 111))    
	INSERT INTO #result_table (_display_seq)    
	VALUES (4)  

	-----------------------------------------
	-- (i) Pneumococcal Vaccination  
	-----------------------------------------
	INSERT INTO #result_table (_display_seq, _result_value1)    
	VALUES (5, REPLACE('(i) Pneumococcal Vaccination ([DATE])', '[DATE]', @current_scheme_desc))     	       
	INSERT INTO #result_table (_display_seq) VALUES (6)   
	INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5)  
	VALUES (7, '', 'Sub-total', '', '', 'No. of SP involved')    
	INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5)
	VALUES (8, '23vPPV', '', '', '', '')    
	INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5)
	VALUES (9, 'PCV13', '', '', '', '') 
	INSERT INTO #result_table (_display_seq) VALUES (10)

	UPDATE #result_table    
	SET _result_value2 = (SELECT COUNT(1) FROM #temp_VSS WHERE IsPV = 1 AND IsCurrentSeason = 1)   
	,_result_value5 = (SELECT COUNT(DISTINCT SP_ID) FROM #temp_VSS WHERE IsPV = 1 AND IsCurrentSeason = 1)  
	WHERE _display_seq = 8

	UPDATE #result_table    
	SET _result_value2 = (SELECT COUNT(1) FROM #temp_VSS WHERE IsPV13 = 1 AND IsCurrentSeason = 1)
	,_result_value5 = (SELECT COUNT(DISTINCT SP_ID) FROM #temp_VSS WHERE IsPV13 = 1 AND IsCurrentSeason = 1)    
	WHERE _display_seq = 9

	-----------------------------------------
	-- (ii) Measles Vaccination  
	-----------------------------------------
	INSERT INTO #result_table (_display_seq, _result_value1)    
	VALUES (11, REPLACE('(ii) Measles Vaccination ([DATE])', '[DATE]', @current_scheme_desc))     	       
	INSERT INTO #result_table (_display_seq) VALUES (12)   
	INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5)  
	VALUES (13, '', 'Sub-total', '', '', 'No. of SP involved')    
	INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5)
	VALUES (14, 'MMR-FDH', '', '', '', ''),    
		   (15, 'MMR-NIA', '', '', '', '')    
	INSERT INTO #result_table (_display_seq) VALUES (16)

	UPDATE #result_table    
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #temp_VSS WHERE Subsidize_Code ='VFDHMMR' AND IsMMR = 1 AND IsCurrentSeason = 1),
		_result_value5 = (SELECT COUNT(DISTINCT SP_ID) FROM #temp_VSS WHERE Subsidize_Code ='VFDHMMR' AND  IsMMR = 1 AND IsCurrentSeason = 1)    
	WHERE _display_seq = 14

	UPDATE #result_table    
	SET 
		_result_value2 = (SELECT COUNT(1) FROM #temp_VSS WHERE Subsidize_Code ='VNIAMMR' AND IsMMR = 1 AND IsCurrentSeason = 1),
		_result_value5 = (SELECT COUNT(DISTINCT SP_ID) FROM #temp_VSS WHERE Subsidize_Code ='VNIAMMR' AND  IsMMR = 1 AND IsCurrentSeason = 1)    
	WHERE _display_seq = 15

	-----------------------------------------
	-- (iii) Seasonal Influenza Vaccination
	-----------------------------------------
	INSERT INTO #result_table (_display_seq, _result_value1) 
	VALUES (17, REPLACE('(iii) Seasonal Influenza Vaccination ([DATE])', '[DATE]',@current_scheme_desc)) 
	INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5)
	VALUES (18, '', '', '', '', 'No. of SP involved')
			    	
	-- SIV --	
	DECLARE @Row AS INT = 19
	DECLARE Category_Cursor CURSOR FOR   
	SELECT DISTINCT Category_Code, Category_Seq FROM @SIV_ByCategory  ORDER BY Category_Seq

	OPEN Category_Cursor  
		FETCH NEXT FROM Category_Cursor INTO @Category_Code, @Category_Seq	
		   
		WHILE @@FETCH_STATUS  = 0 
		BEGIN  		
			SET @QIV_Subsidize_Code = ''
			--SET @TIV_Subsidize_Code = ''
			SET @LAIV_Subsidize_Code = ''
			SET @RIV_Subsidize_Code = ''

			SELECT @QIV_Subsidize_Code = Subsidize_Code
			FROM @SIV_ByCategory 
			WHERE Category_Code = @Category_Code AND Subsidize_Code LIKE '%QIV'

			--SELECT @TIV_Subsidize_Code = Subsidize_Code
			--FROM @SIV_ByCategory 
			--WHERE Category_Code = @Category_Code AND Subsidize_Code LIKE '%TIV'			

			SELECT @LAIV_Subsidize_Code = Subsidize_Code
			FROM @SIV_ByCategory 
			WHERE Category_Code = @Category_Code AND Subsidize_Code LIKE '%LAIV'
			
			SELECT @RIV_Subsidize_Code = Subsidize_Code
			FROM @SIV_ByCategory 
			WHERE Category_Code = @Category_Code AND Subsidize_Code LIKE '%RIV'	
			
			-- Header --				
			INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4)
			VALUES	(@Row, '', '', '', 'Sub-total')
				
			UPDATE #result_table
			SET _result_value1 = (	SELECT Display_Code_For_claim 
									FROM @SIV_ByCategory 
									WHERE Subsidize_Code = @QIV_Subsidize_Code)
			WHERE _display_seq = @Row
		
			--UPDATE #result_table
			--SET _result_value2 = (	SELECT Display_Code_For_claim 
			--						FROM @SIV_ByCategory 
			--						WHERE Subsidize_Code = @TIV_Subsidize_Code)
			--WHERE _display_seq = @Row

			UPDATE #result_table
			SET _result_value2 = (	SELECT ISNULL(Display_Code_For_claim,'')
									FROM @SIV_ByCategory 
									WHERE Subsidize_Code = @LAIV_Subsidize_Code)
			WHERE _display_seq = @Row

			UPDATE #result_table
			SET _result_value3 = (	SELECT Display_Code_For_claim 
									FROM @SIV_ByCategory 
									WHERE Subsidize_Code = @RIV_Subsidize_Code)
			WHERE _display_seq = @Row
	
			SET @Row = @Row + 1
		
			-- QIV --		
			SET @result1 = (SELECT COUNT(1) FROM #temp_VSS WHERE IsSIV = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = @QIV_Subsidize_Code)		
			-- TIV --
			--SET @result2 = (SELECT COUNT(1) FROM #temp_VSS WHERE IsSIV = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = @TIV_Subsidize_Code)			
			-- LAIV --
			SET @result2 = CASE 
								WHEN (SELECT Display_Code_For_claim FROM @SIV_ByCategory WHERE Subsidize_Code = @LAIV_Subsidize_Code) IS NULL THEN NULL
								ELSE (SELECT COUNT(1) FROM #temp_VSS WHERE IsSIV = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = @LAIV_Subsidize_Code)	 
						   END				
			-- RIV --
			SET @result3 = CASE 
								WHEN (SELECT Display_Code_For_claim FROM @SIV_ByCategory WHERE Subsidize_Code = @RIV_Subsidize_Code) IS NULL THEN NULL
								ELSE (SELECT COUNT(1) FROM #temp_VSS WHERE IsSIV = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = @RIV_Subsidize_Code)	 
						   END	

			-- Sub-total
			SET @result4 = @result1 + @result2 + ISNULL(@result3,0)		
			-- No. of SP Involved
			SET @result5 = (SELECT COUNT(DISTINCT SP_ID) 
							FROM #temp_VSS 
							WHERE IsSIV = 1 AND IsCurrentSeason = 1
							AND (Subsidize_Code IN (@QIV_Subsidize_Code, @RIV_Subsidize_Code, @LAIV_Subsidize_Code))
								OR
								(@LAIV_Subsidize_Code IS NULL OR Subsidize_Code = @LAIV_Subsidize_Code))

			--UPDATE #result_table 
			--SET
			--	_result_value1 = @result1,
			--	_result_value2 = @result2, 
			--	_result_value3 = @result3,
			--	_result_value4 = CASE 
			--						WHEN (SELECT Display_Code_For_claim FROM @SIV_ByCategory WHERE Subsidize_Code = @LAIV_Subsidize_Code) IS NULL THEN '' 
			--						ELSE @result4 
			--					 END,
			--	_result_value5 = @result5
			--WHERE 
			--	_display_seq = @Row

			INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5)
			VALUES 
				(@Row, @result1, @result2, @result3, @result4, @result5)
		
			SET @Row = @Row + 1

			INSERT INTO #result_table (_display_seq) VALUES (@Row)
		
			SET @Row = @Row + 1
		
			FETCH NEXT FROM Category_Cursor INTO @Category_Code, @Category_Seq
		END    
	CLOSE Category_Cursor  
	DEALLOCATE Category_Cursor  
	
	-- Total
	-- start from display_seq = 46
	INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5)
	VALUES (46, 'QIV Total', 'LAIV Total', 'RIV Total', 'Total', 'Total No. of SP involved')
			    
	INSERT INTO #result_table (_display_seq) VALUES (47)
			    
	UPDATE #result_table 
	SET _result_value1 = (SELECT COUNT(1) FROM #temp_VSS WHERE IsSIV = 1 AND IsCurrentSeason = 1 AND Subsidize_Code LIKE '%QIV')
	WHERE _display_seq = 47

	--UPDATE #result_table 
	--SET _result_value2 = (SELECT COUNT(1) FROM #temp_VSS WHERE IsSIV = 1 AND IsCurrentSeason = 1 AND Subsidize_Code LIKE '%TIV')
	--WHERE _display_seq = 47
				
	UPDATE #result_table 
	SET _result_value2 = (SELECT COUNT(1) FROM #temp_VSS WHERE IsSIV = 1 AND IsCurrentSeason = 1 AND Subsidize_Code LIKE '%LAIV')
	WHERE _display_seq = 47

	UPDATE #result_table 
	SET _result_value3 = (SELECT COUNT(1) FROM #temp_VSS WHERE IsSIV = 1 AND IsCurrentSeason = 1 AND Subsidize_Code LIKE '%RIV')
	WHERE _display_seq = 47

	UPDATE #result_table 
	SET _result_value4 = (SELECT COUNT(1) FROM #temp_VSS WHERE IsSIV = 1 AND IsCurrentSeason = 1 AND (Subsidize_Code LIKE '%QIV' OR Subsidize_Code LIKE '%RIV' OR Subsidize_Code LIKE '%LAIV'))
	WHERE _display_seq = 47

	UPDATE #result_table 
	SET _result_value5 = (SELECT COUNT(DISTINCT SP_ID) FROM #temp_VSS WHERE IsSIV = 1 AND IsCurrentSeason = 1)
	WHERE _display_seq = 47
		
	INSERT INTO #result_table (_display_seq) VALUES (48)
	INSERT INTO #result_table (_display_seq) VALUES (49)

	-----------------------------------------
	-- (iv) By age group  
	----------------------------------------- 
	INSERT INTO #result_table (_display_seq, _result_value1)     
	VALUES (50, REPLACE('(iv) By age group ([DATE])', '[DATE]',@current_scheme_desc))  

	-- SIV --	
	SET @Row = 60  -- Start from display_seq 60	
	
	DECLARE SIV_Cursor CURSOR FOR   
	SELECT Subsidize_Code, Display_Code_For_Claim FROM @SIV_ByCategory  

	OPEN SIV_Cursor  
	FETCH NEXT FROM SIV_Cursor INTO @SIV_Subsidize_Code, @SIV_Display_Code_For_Claim
	   
	WHILE @@FETCH_STATUS = 0  
	BEGIN  
						
		-- By Age
		-- 6m - <6y
		SET @result1 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '1STDOSE'
						AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 6)
			
		SET @result2 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '2NDDOSE'
						AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 6)
		
		SET @result3 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = 'ONLYDOSE'
						AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 6)

		SET @result4 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '3RDDOSE'
						AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 6)
		
		-- 6y - <9y
		SET @result5 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '1STDOSE'
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 6 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9)
									
		SET @result6 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '2NDDOSE'
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 6 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9)
				
		SET @result7 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = 'ONLYDOSE'
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 6 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9)

		SET @result8 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '3RDDOSE'
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 6 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9)
				
		-- 9y - <12y
		SET @result9 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '1STDOSE'
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 12)
		
		SET @result10 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '2NDDOSE'
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 12)
						
		SET @result11 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = 'ONLYDOSE'
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 12)

		SET @result12 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '3RDDOSE'
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 12)

		-- 12y - <16y
		SET @result13 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '1STDOSE'
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 12 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 16)
		
		SET @result14 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '2NDDOSE'
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 12 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 16)
						
		SET @result15 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = 'ONLYDOSE'
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 12 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 16)

		SET @result16 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '3RDDOSE'
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 12 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 16)

		-- 16y - <50y
		SET @result17 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '1STDOSE'
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 16 AND DateDiff(yy, DOB, Service_receive_dtm) < 50)
		
		SET @result18 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '2NDDOSE'
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 16 AND DateDiff(yy, DOB, Service_receive_dtm) < 50)
		
		SET @result19 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = 'ONLYDOSE'
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 16 AND DateDiff(yy, DOB, Service_receive_dtm) < 50)

		SET @result20 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '3RDDOSE'
						AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 16 AND DateDiff(yy, DOB, Service_receive_dtm) < 50)

		-- 50y - 64y
		SET @result21 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '1STDOSE'
						AND DateDiff(yy, DOB, Service_receive_dtm) >= 50 AND DateDiff(yy, DOB, Service_receive_dtm) <= 64)
		
		SET @result22 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '2NDDOSE'
						AND DateDiff(yy, DOB, Service_receive_dtm) >= 50 AND DateDiff(yy, DOB, Service_receive_dtm) <= 64)
		
		SET @result23 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = 'ONLYDOSE'
						AND DateDiff(yy, DOB, Service_receive_dtm) >= 50 AND DateDiff(yy, DOB, Service_receive_dtm) <= 64)

		SET @result24 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '3RDDOSE'
						AND DateDiff(yy, DOB, Service_receive_dtm) >= 50 AND DateDiff(yy, DOB, Service_receive_dtm) <= 64)
		
		-- 65y
		SET @result25 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '1STDOSE'
						AND DateDiff(yy, DOB, Service_receive_dtm) = 65)
		
		SET @result26 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '2NDDOSE'
						AND DateDiff(yy, DOB, Service_receive_dtm) = 65)
		
		SET @result27 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = 'ONLYDOSE'
						AND DateDiff(yy, DOB, Service_receive_dtm) = 65)

		SET @result28 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '3RDDOSE'
						AND DateDiff(yy, DOB, Service_receive_dtm) = 65)

		-- 66y - 69y
		SET @result29 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '1STDOSE'
						AND DateDiff(yy, DOB, Service_receive_dtm) >= 66 AND DateDiff(yy, DOB, Service_receive_dtm) <= 69)
		
		SET @result30 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '2NDDOSE'
						AND DateDiff(yy, DOB, Service_receive_dtm) >= 66 AND DateDiff(yy, DOB, Service_receive_dtm) <= 69)
		
		SET @result31 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = 'ONLYDOSE'
						AND DateDiff(yy, DOB, Service_receive_dtm) >= 66 AND DateDiff(yy, DOB, Service_receive_dtm) <= 69)

		SET @result32 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '3RDDOSE'
						AND DateDiff(yy, DOB, Service_receive_dtm) >= 66 AND DateDiff(yy, DOB, Service_receive_dtm) <= 69)

		-- 70y - 79y
		SET @result33 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '1STDOSE'
						AND DateDiff(yy, DOB, Service_receive_dtm) >= 70 AND DateDiff(yy, DOB, Service_receive_dtm) <= 79)
		
		SET @result34 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '2NDDOSE'
						AND DateDiff(yy, DOB, Service_receive_dtm) >= 70 AND DateDiff(yy, DOB, Service_receive_dtm) <= 79)
									
		SET @result35 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = 'ONLYDOSE'
						AND DateDiff(yy, DOB, Service_receive_dtm) >= 70 AND DateDiff(yy, DOB, Service_receive_dtm) <= 79)

		SET @result36 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '3RDDOSE'
						AND DateDiff(yy, DOB, Service_receive_dtm) >= 70 AND DateDiff(yy, DOB, Service_receive_dtm) <= 79)
									
		-- 80y
		SET @result37 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '1STDOSE'
						AND DateDiff(yy, DOB, Service_receive_dtm) >= 80)

		SET @result38 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '2NDDOSE'
						AND DateDiff(yy, DOB, Service_receive_dtm) >= 80)
		
		SET @result39 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = 'ONLYDOSE'
						AND DateDiff(yy, DOB, Service_receive_dtm) >= 80)

		SET @result40 = (SELECT COUNT(1) FROM #temp_VSS 
						WHERE IsSIV = 1 AND IsCurrentSeason = 1 
						AND subsidize_code = @SIV_Subsidize_Code AND Dose = '3RDDOSE'
						AND DateDiff(yy, DOB, Service_receive_dtm) >= 80)
								
		-- Total
		SET @result41 = @result1 + @result2 + @result3 + @result4 + @result5 + @result6 + @result7 + @result8 + @result9 + @result10
					+ @result11 + @result12 + @result13 + @result14 + @result15 + @result16 + @result17 + @result18 + @result19 + @result20
					+ @result21 + @result22 + @result23 + @result24 + @result25 + @result26 + @result27 + @result28 + @result29 + @result30
					+ @result31 + @result32 + @result33 + @result34 + @result35 + @result36 + @result37 + @result38 + @result39 + @result40

		SET @sql = 'INSERT INTO #result_table_age (Display_Seq, Display_Code,' + @pivot_table_column_list + ') ' +
					'VALUES (' + CONVERT(VARCHAR,@Row) + ', ''' + @SIV_Display_Code_For_Claim + ''', ' +  
					CONVERT(VARCHAR, @result1) + ', ' + CONVERT(VARCHAR, @result2) + ', ' + CONVERT(VARCHAR, @result3) + ', ' +
					CONVERT(VARCHAR, @result4) + ', ' + CONVERT(VARCHAR, @result5) + ', ' + CONVERT(VARCHAR, @result6) + ', ' +
					CONVERT(VARCHAR, @result7) + ', ' + CONVERT(VARCHAR, @result8) + ', ' + CONVERT(VARCHAR, @result9) + ', ' +
					CONVERT(VARCHAR, @result10) + ', ' + CONVERT(VARCHAR, @result11) + ', ' + CONVERT(VARCHAR, @result12) + ', ' +
					CONVERT(VARCHAR, @result13) + ', ' + CONVERT(VARCHAR, @result14) + ', ' + CONVERT(VARCHAR, @result15) + ', ' +
					CONVERT(VARCHAR, @result16) + ', ' + CONVERT(VARCHAR, @result17) + ', ' + CONVERT(VARCHAR, @result18) + ', ' +
					CONVERT(VARCHAR, @result19) + ', ' + CONVERT(VARCHAR, @result20) + ', ' + CONVERT(VARCHAR, @result21) + ', ' +
					CONVERT(VARCHAR, @result22) + ', ' + CONVERT(VARCHAR, @result23) + ', ' + CONVERT(VARCHAR, @result24) + ', ' +
					CONVERT(VARCHAR, @result25) + ', ' + CONVERT(VARCHAR, @result26) + ', ' + CONVERT(VARCHAR, @result27) + ', ' +
					CONVERT(VARCHAR, @result28) + ', ' + CONVERT(VARCHAR, @result29) + ', ' + CONVERT(VARCHAR, @result30) + ', ' + 
					CONVERT(VARCHAR, @result31) + ', ' + CONVERT(VARCHAR, @result32) + ', ' + CONVERT(VARCHAR, @result33) + ', ' +
					CONVERT(VARCHAR, @result34) + ', ' + CONVERT(VARCHAR, @result35) + ', ' + CONVERT(VARCHAR, @result36) + ', ' +
					CONVERT(VARCHAR, @result37) + ', ' + CONVERT(VARCHAR, @result38) + ', ' + CONVERT(VARCHAR, @result39) + ', ' +
					CONVERT(VARCHAR, @result40) + ', ' + CONVERT(VARCHAR, @result41) + ')'

		EXEC (@sql)

		-- Next SIV
		SET @Row = @Row + 1

		FETCH NEXT FROM SIV_Cursor INTO @SIV_Subsidize_Code, @SIV_Display_Code_For_Claim
	END  
  
	CLOSE SIV_Cursor  
	DEALLOCATE SIV_Cursor  
	
		
	-- Total of QIV + LAIV + RIV
	SET @sql = 'INSERT INTO #result_table_age (Display_Seq, Display_Code,' + @pivot_table_column_list + ') ' +
				'SELECT 91, ''Total of QIV + LAIV + RIV'', ' + @pivot_table_column_sum + ' FROM #result_table_age WHERE Display_Seq>= 60 AND Display_Seq < 90'

	EXEC (@sql)

	-- End of SIV
	
	-- 23vPPV --
	-- By Age
	-- 6m - <6y
	SELECT 
		@result1 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result2 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result3 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result4 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 AND IsCurrentSeason = 1 
	AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 6
	
	-- 6y - <9y
	SELECT 
		@result5 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result6 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result7 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result8 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 6 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9
				
	-- 9y - <12y
	SELECT 
		@result9 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result10 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result11 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result12 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 12
			
	-- 12y - <16y
	SELECT 
		@result13 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result14 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result15 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result16 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 12 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 16
		
	-- 16y - <50y
	SELECT 
		@result17 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result18 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result19 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result20 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 16 AND DateDiff(yy, DOB, Service_receive_dtm) < 50
		
	-- 50y - 64y
	SELECT 
		@result21 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result22 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result23 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result24 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 50 AND DateDiff(yy, DOB, Service_receive_dtm) <= 64

	-- 65y
	SELECT 
		@result25 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result26 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result27 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result28 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) = 65
									
	-- 66y - 69y
	SELECT 
		@result29 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result30 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result31 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result32 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 66 AND DateDiff(yy, DOB, Service_receive_dtm) <= 69

	-- 70y - 79y	
	SELECT 
		@result33 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result34 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result35 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result36 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 70 AND DateDiff(yy, DOB, Service_receive_dtm) <= 79

						
	-- 80y
	SELECT 
		@result37 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result38 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result39 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result40 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 80
							
	-- Total
	SET @result41 = @result1 + @result2 + @result3 + @result4 + @result5 + @result6 + @result7 + @result8 + @result9 + @result10
					+ @result11 + @result12 + @result13 + @result14 + @result15 + @result16 + @result17 + @result18 + @result19 + @result20
					+ @result21 + @result22 + @result23 + @result24 + @result25 + @result26 + @result27 + @result28 + @result29 + @result30
					+ @result31 + @result32 + @result33 + @result34 + @result35 + @result36 + @result37 + @result38 + @result39 + @result40
					 
	SET @sql = 'INSERT INTO #result_table_age (Display_Seq, Display_Code,' + @pivot_table_column_list + ') ' +
				'VALUES (100 , ''' + '23vPPV' + ''', ' +  
				CONVERT(VARCHAR, @result1) + ', ' + CONVERT(VARCHAR, @result2) + ', ' + CONVERT(VARCHAR, @result3) + ', ' +
				CONVERT(VARCHAR, @result4) + ', ' + CONVERT(VARCHAR, @result5) + ', ' + CONVERT(VARCHAR, @result6) + ', ' +
				CONVERT(VARCHAR, @result7) + ', ' + CONVERT(VARCHAR, @result8) + ', ' + CONVERT(VARCHAR, @result9) + ', ' +
				CONVERT(VARCHAR, @result10) + ', ' + CONVERT(VARCHAR, @result11) + ', ' + CONVERT(VARCHAR, @result12) + ', ' +
				CONVERT(VARCHAR, @result13) + ', ' + CONVERT(VARCHAR, @result14) + ', ' + CONVERT(VARCHAR, @result15) + ', ' +
				CONVERT(VARCHAR, @result16) + ', ' + CONVERT(VARCHAR, @result17) + ', ' + CONVERT(VARCHAR, @result18) + ', ' +
				CONVERT(VARCHAR, @result19) + ', ' + CONVERT(VARCHAR, @result20) + ', ' + CONVERT(VARCHAR, @result21) + ', ' +
				CONVERT(VARCHAR, @result22) + ', ' + CONVERT(VARCHAR, @result23) + ', ' + CONVERT(VARCHAR, @result24) + ', ' +
				CONVERT(VARCHAR, @result25) + ', ' + CONVERT(VARCHAR, @result26) + ', ' + CONVERT(VARCHAR, @result27) + ', ' +
				CONVERT(VARCHAR, @result28) + ', ' + CONVERT(VARCHAR, @result29) + ', ' + CONVERT(VARCHAR, @result30) + ', ' + 
				CONVERT(VARCHAR, @result31) + ', ' + CONVERT(VARCHAR, @result32) + ', ' + CONVERT(VARCHAR, @result33) + ', ' +
				CONVERT(VARCHAR, @result34) + ', ' + CONVERT(VARCHAR, @result35) + ', ' + CONVERT(VARCHAR, @result36) + ', ' +
				CONVERT(VARCHAR, @result37) + ', ' + CONVERT(VARCHAR, @result38) + ', ' + CONVERT(VARCHAR, @result39) + ', ' +
				CONVERT(VARCHAR, @result40) + ', ' + CONVERT(VARCHAR, @result41) + ')'

	EXEC (@sql)
	
	-- 23vPPV End --
		
	-- 23vPPV (High Risk)--
	-- By Age-- 6m - <6y
	SELECT 
		@result1 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result2 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result3 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result4 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 AND IsCurrentSeason = 1 
	AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 6
	AND High_Risk IS NOT NULL AND High_Risk ='Y'

	-- 6y - <9y
	SELECT 
		@result5 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result6 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result7 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result8 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 6 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
				
	-- 9y - <12y
	SELECT 
		@result9 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result10 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result11 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result12 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 12
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
			
	-- 12y - <16y
	SELECT 
		@result13 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result14 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result15 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result16 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 12 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 16
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
		
	-- 16y - <50y
	SELECT 
		@result17 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result18 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result19 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result20 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 16 AND DateDiff(yy, DOB, Service_receive_dtm) < 50
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
		
	-- 50y - 64y
	SELECT 
		@result21 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result22 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result23 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result24 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 16 AND DateDiff(yy, DOB, Service_receive_dtm) <= 64
	AND High_Risk IS NOT NULL AND High_Risk ='Y'

	-- 65y
	SELECT 
		@result25 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result26 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result27 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result28 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) = 65
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
									
	-- 66y - 69y
	SELECT 
		@result29 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result30 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result31 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result32 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 66 AND DateDiff(yy, DOB, Service_receive_dtm) <= 69
	AND High_Risk IS NOT NULL AND High_Risk ='Y'

	-- 70y - 79y	
	SELECT 
		@result33 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result34 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result35 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result36 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 70 AND DateDiff(yy, DOB, Service_receive_dtm) <= 79
	AND High_Risk IS NOT NULL AND High_Risk ='Y'

						
	-- 80y
	SELECT 
		@result37 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result38 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result39 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result40 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 80
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
							
	-- Total
	SET @result41 = @result1 + @result2 + @result3 + @result4 + @result5 + @result6 + @result7 + @result8 + @result9 + @result10
					+ @result11 + @result12 + @result13 + @result14 + @result15 + @result16 + @result17 + @result18 + @result19 + @result20
					+ @result21 + @result22 + @result23 + @result24 + @result25 + @result26 + @result27 + @result28 + @result29 + @result30
					+ @result31 + @result32 + @result33 + @result34 + @result35 + @result36 + @result37 + @result38 + @result39 + @result40
					 
	SET @sql = 'INSERT INTO #result_table_age (Display_Seq, Display_Code,' + @pivot_table_column_list + ') ' +
				'VALUES (101 , ''' + '23vPPV (' + @Str_HighRisk + ')'', ' +  
				CONVERT(VARCHAR, @result1) + ', ' + CONVERT(VARCHAR, @result2) + ', ' + CONVERT(VARCHAR, @result3) + ', ' +
				CONVERT(VARCHAR, @result4) + ', ' + CONVERT(VARCHAR, @result5) + ', ' + CONVERT(VARCHAR, @result6) + ', ' +
				CONVERT(VARCHAR, @result7) + ', ' + CONVERT(VARCHAR, @result8) + ', ' + CONVERT(VARCHAR, @result9) + ', ' +
				CONVERT(VARCHAR, @result10) + ', ' + CONVERT(VARCHAR, @result11) + ', ' + CONVERT(VARCHAR, @result12) + ', ' +
				CONVERT(VARCHAR, @result13) + ', ' + CONVERT(VARCHAR, @result14) + ', ' + CONVERT(VARCHAR, @result15) + ', ' +
				CONVERT(VARCHAR, @result16) + ', ' + CONVERT(VARCHAR, @result17) + ', ' + CONVERT(VARCHAR, @result18) + ', ' +
				CONVERT(VARCHAR, @result19) + ', ' + CONVERT(VARCHAR, @result20) + ', ' + CONVERT(VARCHAR, @result21) + ', ' +
				CONVERT(VARCHAR, @result22) + ', ' + CONVERT(VARCHAR, @result23) + ', ' + CONVERT(VARCHAR, @result24) + ', ' +
				CONVERT(VARCHAR, @result25) + ', ' + CONVERT(VARCHAR, @result26) + ', ' + CONVERT(VARCHAR, @result27) + ', ' +
				CONVERT(VARCHAR, @result28) + ', ' + CONVERT(VARCHAR, @result29) + ', ' + CONVERT(VARCHAR, @result30) + ', ' + 
				CONVERT(VARCHAR, @result31) + ', ' + CONVERT(VARCHAR, @result32) + ', ' + CONVERT(VARCHAR, @result33) + ', ' +
				CONVERT(VARCHAR, @result34) + ', ' + CONVERT(VARCHAR, @result35) + ', ' + CONVERT(VARCHAR, @result36) + ', ' +
				CONVERT(VARCHAR, @result37) + ', ' + CONVERT(VARCHAR, @result38) + ', ' + CONVERT(VARCHAR, @result39) + ', ' +
				CONVERT(VARCHAR, @result40) + ', ' + CONVERT(VARCHAR, @result41) + ')'

	EXEC (@sql)
	
	-- 23vPPV (High Risk) End --

	-- PCV13 (High Risk)--
	-- By Age-- 6m - <6y
	SELECT 
		@result1 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result2 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result3 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result4 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsCurrentSeason = 1 
	AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 6
	AND High_Risk IS NOT NULL AND High_Risk ='Y'

	-- 6y - <9y
	SELECT 
		@result5 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result6 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result7 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result8 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 6 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
				
	-- 9y - <12y
	SELECT 
		@result9 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result10 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result11 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result12 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 12
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
			
	-- 12y - <16y
	SELECT 
		@result13 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result14 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result15 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result16 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 12 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 16
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
		
	-- 16y - <50y
	SELECT 
		@result17 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result18 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result19 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result20 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 16 AND DateDiff(yy, DOB, Service_receive_dtm) < 50
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
	
	-- 50y - 64y
	SELECT 
		@result21 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result22 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result23 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result24 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 50 AND DateDiff(yy, DOB, Service_receive_dtm) <= 64
	AND High_Risk IS NOT NULL AND High_Risk ='Y'

	-- 65y
	SELECT 
		@result25 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result26 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result27 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result28 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) = 65
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
									
	-- 66y - 69y
	SELECT 
		@result29 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result30 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result31 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result32 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 66 AND DateDiff(yy, DOB, Service_receive_dtm) <= 69
	AND High_Risk IS NOT NULL AND High_Risk ='Y'

	-- 70y - 79y	
	SELECT 
		@result33 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result34 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result35 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result36 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 70 AND DateDiff(yy, DOB, Service_receive_dtm) <= 79
	AND High_Risk IS NOT NULL AND High_Risk ='Y'

						
	-- 80y
	SELECT 
		@result37 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result38 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result39 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result40 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsCurrentSeason = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 80
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
							
	-- Total
	SET @result41 = @result1 + @result2 + @result3 + @result4 + @result5 + @result6 + @result7 + @result8 + @result9 + @result10
					+ @result11 + @result12 + @result13 + @result14 + @result15 + @result16 + @result17 + @result18 + @result19 + @result20
					+ @result21 + @result22 + @result23 + @result24 + @result25 + @result26 + @result27 + @result28 + @result29 + @result30
					+ @result31 + @result32 + @result33 + @result34 + @result35 + @result36 + @result37 + @result38 + @result39 + @result40
					 
	SET @sql = 'INSERT INTO #result_table_age (Display_Seq, Display_Code,' + @pivot_table_column_list + ') ' +
				'VALUES (102 , ''' + 'PCV13 (' + @Str_HighRisk + ')'', ' +  
				CONVERT(VARCHAR, @result1) + ', ' + CONVERT(VARCHAR, @result2) + ', ' + CONVERT(VARCHAR, @result3) + ', ' +
				CONVERT(VARCHAR, @result4) + ', ' + CONVERT(VARCHAR, @result5) + ', ' + CONVERT(VARCHAR, @result6) + ', ' +
				CONVERT(VARCHAR, @result7) + ', ' + CONVERT(VARCHAR, @result8) + ', ' + CONVERT(VARCHAR, @result9) + ', ' +
				CONVERT(VARCHAR, @result10) + ', ' + CONVERT(VARCHAR, @result11) + ', ' + CONVERT(VARCHAR, @result12) + ', ' +
				CONVERT(VARCHAR, @result13) + ', ' + CONVERT(VARCHAR, @result14) + ', ' + CONVERT(VARCHAR, @result15) + ', ' +
				CONVERT(VARCHAR, @result16) + ', ' + CONVERT(VARCHAR, @result17) + ', ' + CONVERT(VARCHAR, @result18) + ', ' +
				CONVERT(VARCHAR, @result19) + ', ' + CONVERT(VARCHAR, @result20) + ', ' + CONVERT(VARCHAR, @result21) + ', ' +
				CONVERT(VARCHAR, @result22) + ', ' + CONVERT(VARCHAR, @result23) + ', ' + CONVERT(VARCHAR, @result24) + ', ' +
				CONVERT(VARCHAR, @result25) + ', ' + CONVERT(VARCHAR, @result26) + ', ' + CONVERT(VARCHAR, @result27) + ', ' +
				CONVERT(VARCHAR, @result28) + ', ' + CONVERT(VARCHAR, @result29) + ', ' + CONVERT(VARCHAR, @result30) + ', ' + 
				CONVERT(VARCHAR, @result31) + ', ' + CONVERT(VARCHAR, @result32) + ', ' + CONVERT(VARCHAR, @result33) + ', ' +
				CONVERT(VARCHAR, @result34) + ', ' + CONVERT(VARCHAR, @result35) + ', ' + CONVERT(VARCHAR, @result36) + ', ' +
				CONVERT(VARCHAR, @result37) + ', ' + CONVERT(VARCHAR, @result38) + ', ' + CONVERT(VARCHAR, @result39) + ', ' +
				CONVERT(VARCHAR, @result40) + ', ' + CONVERT(VARCHAR, @result41) + ')'

	EXEC (@sql)
		
	-- PCV13 (High Risk)End --
		
	-- PCV13 + with Voucher--
	-- By Age	
	-- By Age-- 6m - <6y
	SELECT 
		@result1 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result2 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result3 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result4 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsCurrentSeason = 1 AND IsWithVoucher = 1
	AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 6
	AND High_Risk IS NOT NULL AND High_Risk ='Y'

	-- 6y - <9y
	SELECT 
		@result5 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result6 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result7 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result8 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsCurrentSeason = 1 AND IsWithVoucher = 1
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 6 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
				
	-- 9y - <12y
	SELECT 
		@result9 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result10 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result11 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result12 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsCurrentSeason = 1 AND IsWithVoucher = 1
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 12
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
			
	-- 12y - <16y
	SELECT 
		@result13 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result14 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result15 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result16 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsCurrentSeason = 1 AND IsWithVoucher = 1
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 12 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 16
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
		
	-- 16y - <50y
	SELECT 
		@result17 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result18 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result19 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result20 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsCurrentSeason = 1 AND IsWithVoucher = 1
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 16 AND DateDiff(yy, DOB, Service_receive_dtm) < 50
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
		
	-- 50y - 64y
	SELECT 
		@result21 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result22 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result23 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result24 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsCurrentSeason = 1 AND IsWithVoucher = 1
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 50 AND DateDiff(yy, DOB, Service_receive_dtm) <= 64
	AND High_Risk IS NOT NULL AND High_Risk ='Y'

	-- 65y
	SELECT 
		@result25 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result26 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result27 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result28 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsCurrentSeason = 1 AND IsWithVoucher = 1
	AND DateDiff(yy, DOB, Service_receive_dtm) = 65
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
									
	-- 66y - 69y
	SELECT 
		@result29 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result30 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result31 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result32 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsCurrentSeason = 1 AND IsWithVoucher = 1
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 66 AND DateDiff(yy, DOB, Service_receive_dtm) <= 69
	AND High_Risk IS NOT NULL AND High_Risk ='Y'

	-- 70y - 79y	
	SELECT 
		@result33 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result34 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result35 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result36 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsCurrentSeason = 1 AND IsWithVoucher = 1
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 70 AND DateDiff(yy, DOB, Service_receive_dtm) <= 79
	AND High_Risk IS NOT NULL AND High_Risk ='Y'

						
	-- 80y
	SELECT 
		@result37 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result38 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result39 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result40 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsCurrentSeason = 1 AND IsWithVoucher = 1
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 80
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
									
	-- Total
	SET @result41 = @result1 + @result2 + @result3 + @result4 + @result5 + @result6 + @result7 + @result8 + @result9 + @result10
					+ @result11 + @result12 + @result13 + @result14 + @result15 + @result16 + @result17 + @result18 + @result19 + @result20
					+ @result21 + @result22 + @result23 + @result24 + @result25 + @result26 + @result27 + @result28 + @result29 + @result30
					+ @result31 + @result32 + @result33 + @result34 + @result35 + @result36 + @result37 + @result38 + @result39 + @result40
					 
	SET @sql = 'INSERT INTO #result_table_age (Display_Seq, Display_Code,' + @pivot_table_column_list + ') ' +
				'VALUES (103 , ''PCV13 and using healthcare voucher**'', ' +  
				CONVERT(VARCHAR, @result1) + ', ' + CONVERT(VARCHAR, @result2) + ', ' + CONVERT(VARCHAR, @result3) + ', ' +
				CONVERT(VARCHAR, @result4) + ', ' + CONVERT(VARCHAR, @result5) + ', ' + CONVERT(VARCHAR, @result6) + ', ' +
				CONVERT(VARCHAR, @result7) + ', ' + CONVERT(VARCHAR, @result8) + ', ' + CONVERT(VARCHAR, @result9) + ', ' +
				CONVERT(VARCHAR, @result10) + ', ' + CONVERT(VARCHAR, @result11) + ', ' + CONVERT(VARCHAR, @result12) + ', ' +
				CONVERT(VARCHAR, @result13) + ', ' + CONVERT(VARCHAR, @result14) + ', ' + CONVERT(VARCHAR, @result15) + ', ' +
				CONVERT(VARCHAR, @result16) + ', ' + CONVERT(VARCHAR, @result17) + ', ' + CONVERT(VARCHAR, @result18) + ', ' +
				CONVERT(VARCHAR, @result19) + ', ' + CONVERT(VARCHAR, @result20) + ', ' + CONVERT(VARCHAR, @result21) + ', ' +
				CONVERT(VARCHAR, @result22) + ', ' + CONVERT(VARCHAR, @result23) + ', ' + CONVERT(VARCHAR, @result24) + ', ' +
				CONVERT(VARCHAR, @result25) + ', ' + CONVERT(VARCHAR, @result26) + ', ' + CONVERT(VARCHAR, @result27) + ', ' +
				CONVERT(VARCHAR, @result28) + ', ' + CONVERT(VARCHAR, @result29) + ', ' + CONVERT(VARCHAR, @result30) + ', ' + 
				CONVERT(VARCHAR, @result31) + ', ' + CONVERT(VARCHAR, @result32) + ', ' + CONVERT(VARCHAR, @result33) + ', ' +
				CONVERT(VARCHAR, @result34) + ', ' + CONVERT(VARCHAR, @result35) + ', ' + CONVERT(VARCHAR, @result36) + ', ' +
				CONVERT(VARCHAR, @result37) + ', ' + CONVERT(VARCHAR, @result38) + ', ' + CONVERT(VARCHAR, @result39) + ', ' +
				CONVERT(VARCHAR, @result40) + ', ' + CONVERT(VARCHAR, @result41) + ')'

	EXEC (@sql)
	
	-- PCV13 With Voucher End --
	

	-- MMR-FDH --
	-- By Age
	-- 6m - <6y
	SELECT 
		@result1 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result2 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result3 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result4 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = 'VFDHMMR' 
	AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 6
	
	-- 6y - <9y
	SELECT 
		@result5 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result6 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result7 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result8 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = 'VFDHMMR' 
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 6 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9
				
	-- 9y - <12y
	SELECT 
		@result9 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result10 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result11 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result12 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = 'VFDHMMR' 
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 12
			
	-- 12y - <16y
	SELECT 
		@result13 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result14 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result15 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result16 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = 'VFDHMMR' 
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 12 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 16
		
	-- 16y - <50y
	SELECT 
		@result17 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result18 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result19 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result20 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = 'VFDHMMR' 
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 16 AND DateDiff(yy, DOB, Service_receive_dtm) < 50
		
	-- 50y - 64y
	SELECT 
		@result21 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result22 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result23 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result24 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = 'VFDHMMR' 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 50 AND DateDiff(yy, DOB, Service_receive_dtm) <= 64

	-- 65y
	SELECT 
		@result25 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result26 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result27 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result28 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = 'VFDHMMR' 
	AND DateDiff(yy, DOB, Service_receive_dtm) = 65
									
	-- 66y - 69y
	SELECT 
		@result29 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result30 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result31 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result32 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = 'VFDHMMR' 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 66 AND DateDiff(yy, DOB, Service_receive_dtm) <= 69

	-- 70y - 79y	
	SELECT 
		@result33 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result34 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result35 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result36 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = 'VFDHMMR'
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 70 AND DateDiff(yy, DOB, Service_receive_dtm) <= 79

						
	-- 80y
	SELECT 
		@result37 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result38 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result39 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result40 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = 'VFDHMMR'
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 80
							
	-- Total
	SET @result41 = @result1 + @result2 + @result3 + @result4 + @result5 + @result6 + @result7 + @result8 + @result9 + @result10
					+ @result11 + @result12 + @result13 + @result14 + @result15 + @result16 + @result17 + @result18 + @result19 + @result20
					+ @result21 + @result22 + @result23 + @result24 + @result25 + @result26 + @result27 + @result28 + @result29 + @result30
					+ @result31 + @result32 + @result33 + @result34 + @result35 + @result36 + @result37 + @result38 + @result39 + @result40
					 
	SET @sql = 'INSERT INTO #result_table_age (Display_Seq, Display_Code,' + @pivot_table_column_list + ') ' +
				'VALUES (104 , ''' + 'MMR-FDH' + ''', ' +  
				CONVERT(VARCHAR, @result1) + ', ' + CONVERT(VARCHAR, @result2) + ', ' + CONVERT(VARCHAR, @result3) + ', ' +
				CONVERT(VARCHAR, @result4) + ', ' + CONVERT(VARCHAR, @result5) + ', ' + CONVERT(VARCHAR, @result6) + ', ' +
				CONVERT(VARCHAR, @result7) + ', ' + CONVERT(VARCHAR, @result8) + ', ' + CONVERT(VARCHAR, @result9) + ', ' +
				CONVERT(VARCHAR, @result10) + ', ' + CONVERT(VARCHAR, @result11) + ', ' + CONVERT(VARCHAR, @result12) + ', ' +
				CONVERT(VARCHAR, @result13) + ', ' + CONVERT(VARCHAR, @result14) + ', ' + CONVERT(VARCHAR, @result15) + ', ' +
				CONVERT(VARCHAR, @result16) + ', ' + CONVERT(VARCHAR, @result17) + ', ' + CONVERT(VARCHAR, @result18) + ', ' +
				CONVERT(VARCHAR, @result19) + ', ' + CONVERT(VARCHAR, @result20) + ', ' + CONVERT(VARCHAR, @result21) + ', ' +
				CONVERT(VARCHAR, @result22) + ', ' + CONVERT(VARCHAR, @result23) + ', ' + CONVERT(VARCHAR, @result24) + ', ' +
				CONVERT(VARCHAR, @result25) + ', ' + CONVERT(VARCHAR, @result26) + ', ' + CONVERT(VARCHAR, @result27) + ', ' +
				CONVERT(VARCHAR, @result28) + ', ' + CONVERT(VARCHAR, @result29) + ', ' + CONVERT(VARCHAR, @result30) + ', ' + 
				CONVERT(VARCHAR, @result31) + ', ' + CONVERT(VARCHAR, @result32) + ', ' + CONVERT(VARCHAR, @result33) + ', ' +
				CONVERT(VARCHAR, @result34) + ', ' + CONVERT(VARCHAR, @result35) + ', ' + CONVERT(VARCHAR, @result36) + ', ' +
				CONVERT(VARCHAR, @result37) + ', ' + CONVERT(VARCHAR, @result38) + ', ' + CONVERT(VARCHAR, @result39) + ', ' +
				CONVERT(VARCHAR, @result40) + ', ' + CONVERT(VARCHAR, @result41) + ')'

	EXEC (@sql)
	
	-- MMR-FDH End --


	-- MMR-NIA --
	-- By Age
	-- 6m - <6y
	SELECT 
		@result1 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result2 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result3 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result4 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = 'VNIAMMR' 
	AND DateDiff(dd, DOB, Service_receive_dtm) >= 182 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 6
	
	-- 6y - <9y
	SELECT 
		@result5 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result6 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result7 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result8 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = 'VNIAMMR' 
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 6 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 9
				
	-- 9y - <12y
	SELECT 
		@result9 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result10 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result11 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result12 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = 'VNIAMMR' 
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 9 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 12
			
	-- 12y - <16y
	SELECT 
		@result13 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result14 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result15 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result16 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = 'VNIAMMR' 
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 12 AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) < 16
		
	-- 16y - <50y
	SELECT 
		@result17 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result18 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result19 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result20 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = 'VNIAMMR' 
	AND DateDiff(yy, DOB_Adjust, Service_receive_dtm) >= 16 AND DateDiff(yy, DOB, Service_receive_dtm) < 50
		
	-- 50y - 64y
	SELECT 
		@result21 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result22 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result23 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result24 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = 'VNIAMMR' 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 50 AND DateDiff(yy, DOB, Service_receive_dtm) <= 64

	-- 65y
	SELECT 
		@result25 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result26 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result27 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result28 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = 'VNIAMMR' 
	AND DateDiff(yy, DOB, Service_receive_dtm) = 65
									
	-- 66y - 69y
	SELECT 
		@result29 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result30 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result31 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result32 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = 'VNIAMMR' 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 66 AND DateDiff(yy, DOB, Service_receive_dtm) <= 69

	-- 70y - 79y	
	SELECT 
		@result33 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result34 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result35 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result36 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = 'VNIAMMR'
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 70 AND DateDiff(yy, DOB, Service_receive_dtm) <= 79

						
	-- 80y
	SELECT 
		@result37 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result38 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result39 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result40 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND IsCurrentSeason = 1 AND Subsidize_Code = 'VNIAMMR'
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 80
							
	-- Total
	SET @result41 = @result1 + @result2 + @result3 + @result4 + @result5 + @result6 + @result7 + @result8 + @result9 + @result10
					+ @result11 + @result12 + @result13 + @result14 + @result15 + @result16 + @result17 + @result18 + @result19 + @result20
					+ @result21 + @result22 + @result23 + @result24 + @result25 + @result26 + @result27 + @result28 + @result29 + @result30
					+ @result31 + @result32 + @result33 + @result34 + @result35 + @result36 + @result37 + @result38 + @result39 + @result40
					 
	SET @sql = 'INSERT INTO #result_table_age (Display_Seq, Display_Code,' + @pivot_table_column_list + ') ' +
				'VALUES (104 , ''' + 'MMR-NIA' + ''', ' +  
				CONVERT(VARCHAR, @result1) + ', ' + CONVERT(VARCHAR, @result2) + ', ' + CONVERT(VARCHAR, @result3) + ', ' +
				CONVERT(VARCHAR, @result4) + ', ' + CONVERT(VARCHAR, @result5) + ', ' + CONVERT(VARCHAR, @result6) + ', ' +
				CONVERT(VARCHAR, @result7) + ', ' + CONVERT(VARCHAR, @result8) + ', ' + CONVERT(VARCHAR, @result9) + ', ' +
				CONVERT(VARCHAR, @result10) + ', ' + CONVERT(VARCHAR, @result11) + ', ' + CONVERT(VARCHAR, @result12) + ', ' +
				CONVERT(VARCHAR, @result13) + ', ' + CONVERT(VARCHAR, @result14) + ', ' + CONVERT(VARCHAR, @result15) + ', ' +
				CONVERT(VARCHAR, @result16) + ', ' + CONVERT(VARCHAR, @result17) + ', ' + CONVERT(VARCHAR, @result18) + ', ' +
				CONVERT(VARCHAR, @result19) + ', ' + CONVERT(VARCHAR, @result20) + ', ' + CONVERT(VARCHAR, @result21) + ', ' +
				CONVERT(VARCHAR, @result22) + ', ' + CONVERT(VARCHAR, @result23) + ', ' + CONVERT(VARCHAR, @result24) + ', ' +
				CONVERT(VARCHAR, @result25) + ', ' + CONVERT(VARCHAR, @result26) + ', ' + CONVERT(VARCHAR, @result27) + ', ' +
				CONVERT(VARCHAR, @result28) + ', ' + CONVERT(VARCHAR, @result29) + ', ' + CONVERT(VARCHAR, @result30) + ', ' + 
				CONVERT(VARCHAR, @result31) + ', ' + CONVERT(VARCHAR, @result32) + ', ' + CONVERT(VARCHAR, @result33) + ', ' +
				CONVERT(VARCHAR, @result34) + ', ' + CONVERT(VARCHAR, @result35) + ', ' + CONVERT(VARCHAR, @result36) + ', ' +
				CONVERT(VARCHAR, @result37) + ', ' + CONVERT(VARCHAR, @result38) + ', ' + CONVERT(VARCHAR, @result39) + ', ' +
				CONVERT(VARCHAR, @result40) + ', ' + CONVERT(VARCHAR, @result41) + ')'

	EXEC (@sql)
	
	-- MMR-NIA End --

	
	DECLARE Age_Cursor CURSOR FOR   
	SELECT Age, AgeDesc, DoseDesc, MustShow FROM @AgeDose  

	OPEN Age_Cursor  
	FETCH NEXT FROM Age_Cursor INTO @Age, @AgeDesc, @DoseDesc, @MustShow
	   
	WHILE @@FETCH_STATUS = 0   
	BEGIN
		DECLARE @TotalCount as INT

		-- Total Count
		SET @sql = 'SELECT @TotalCount = SUM(' + @Age + ') FROM #result_table_age '
		SET @sql = @sql + ' WHERE Display_Seq = 91 OR Display_Seq = 100 OR Display_Seq = 101 OR Display_Seq = 102 OR Display_Seq = 103 OR Display_Seq = 104'		
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

	SET @sql = 'INSERT INTO #result_table (	_display_seq, _result_value1, _result_value2, _result_value3, _result_value4,   
				_result_value5, _result_value6, _result_value7, _result_value8, _result_value9, _result_value10,   
				_result_value11, _result_value12, _result_value13, _result_value14, _result_value15, _result_value16,   
				_result_value17, _result_value18, _result_value19, _result_value20, _result_value21, _result_value22,   
				_result_value23, _result_value24, _result_value25, _result_value26, _result_value27, _result_value28, _result_value29, _result_value30, _result_value31, _result_value32,
				_result_value33, _result_value34, _result_value35, _result_value36, _result_value37, _result_value38, _result_value39, _result_value40, _result_value41, _result_value42
				) SELECT Display_Seq, Display_Code,' + @ShowList + @EmptyList + ' FROM #result_table_age'
	EXEC (@sql)
	
	---- Age Header
	SET @sql = 'INSERT INTO #result_table (	_display_seq, _result_value1, _result_value2, _result_value3, _result_value4,   
				_result_value5, _result_value6, _result_value7, _result_value8, _result_value9, _result_value10,   
				_result_value11, _result_value12, _result_value13, _result_value14, _result_value15, _result_value16,   
				_result_value17, _result_value18, _result_value19, _result_value20, _result_value21, _result_value22,   
				_result_value23, _result_value24, _result_value25, _result_value26, _result_value27, _result_value28, _result_value29, _result_value30, _result_value31, _result_value32,
				_result_value33, _result_value34, _result_value35, _result_value36, _result_value37, _result_value38, _result_value39, _result_value40, _result_value41, _result_value42
				) VALUES (51, '''',' + @AgeHeaderList + @EmptyList + ')'
	EXEC (@sql)
	
	---- Dose Header
	SET @sql = 'INSERT INTO #result_table (	_display_seq, _result_value1, _result_value2, _result_value3, _result_value4,   
				_result_value5, _result_value6, _result_value7, _result_value8, _result_value9, _result_value10,   
				_result_value11, _result_value12, _result_value13, _result_value14, _result_value15, _result_value16,   
				_result_value17, _result_value18, _result_value19, _result_value20, _result_value21, _result_value22,   
				_result_value23, _result_value24, _result_value25, _result_value26, _result_value27, _result_value28, _result_value29, _result_value30, _result_value31, _result_value32,
				_result_value33, _result_value34, _result_value35, _result_value36, _result_value37, _result_value38, _result_value39, _result_value40, _result_value41, _result_value42
				) VALUES (52, '''',' + @DoseHeaderList + @EmptyList + ')'
	EXEC (@sql)	

	-- Fill Empty Row
	INSERT INTO #result_table (_display_seq) VALUES (90)
	INSERT INTO #result_table (_display_seq) VALUES (92)

	-- Fill Remark
	INSERT INTO #result_table (_display_seq) VALUES (200)	
	INSERT INTO #result_table (_display_seq, _result_value1)  
	VALUES (201, 'Remark:')	
	INSERT INTO #result_table (_display_seq, _result_value1)  
	VALUES (202, 'a. For age < 50 years old')	
	INSERT INTO #result_table (_display_seq, _result_value1)  
	VALUES (203, '    - Age = specific date - DOB')	
	INSERT INTO #result_table (_display_seq, _result_value1)  
	VALUES (204, 'b. For age >= 50 years old')	
	INSERT INTO #result_table (_display_seq, _result_value1)  
	VALUES (205, '    - Age at year = year of the specific date - year of DOB')	
	INSERT INTO #result_table (_display_seq, _result_value1)  
	VALUES (206, 'Service date since ' + FORMAT(@Current_Season_Start_Dtm, 'dd MMM yyyy'))	
	INSERT INTO #result_table (_display_seq, _result_value1)  
	VALUES (207, '** Claim healthcare voucher by same service provider on the same service date with PCV13 vaccination with principal reason for visit selected as "Preventive: Immunisation and chemoprophylaxis"')
	
	
	-- =============================================    
	-- Sub Report
	-- ============================================= 	
	--Reset variable 	
	SET @EmptyList = NULL
	SET @ShowList = NULL
	SET @AgeHeaderList  = NULL
	SET @DoseHeaderList = NULL
	DELETE FROM @AgeDose


	INSERT INTO #Subresult_table (_display_seq, _result_value1)    
	VALUES (1, 'eHS(S)D0028-03: Report on cumulative VSS claim transaction by age group and target group')    
	INSERT INTO #Subresult_table (_display_seq)    
	VALUES (2)    
	INSERT INTO #Subresult_table (_display_seq, _result_value1)    
	VALUES (3, 'Reporting period: as at ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111))    
	INSERT INTO #Subresult_table (_display_seq)    
	VALUES (4)  

	-----------------------------------------
	-- (i) Pneumococcal Vaccination  
	-----------------------------------------
	INSERT INTO #Subresult_table (_display_seq, _result_value1)    
	VALUES (5, '(i) Pneumococcal Vaccination')     	       
	INSERT INTO #Subresult_table (_display_seq) VALUES (6)   
	INSERT INTO #Subresult_table (_display_seq, _result_value1, _result_value2, _result_value3)  
	VALUES (7, '', 'Sub-total', 'No. of SP involved')    
	INSERT INTO #Subresult_table (_display_seq, _result_value1, _result_value2, _result_value3)
	VALUES (8, '23vPPV', '', '')    
	INSERT INTO #Subresult_table (_display_seq, _result_value1, _result_value2, _result_value3)
	VALUES (9, 'PCV13', '', '') 
	INSERT INTO #Subresult_table (_display_seq) VALUES (10)

	UPDATE #Subresult_table    
	SET _result_value2 = (SELECT COUNT(1) FROM #temp_VSS WHERE IsPV = 1 )  
		,_result_value3 = (SELECT COUNT(DISTINCT SP_ID) FROM #temp_VSS WHERE IsPV = 1 )   
	WHERE _display_seq = 8

	UPDATE #Subresult_table    
	SET _result_value2 = (SELECT COUNT(1) FROM #temp_VSS WHERE IsPV13 = 1 )  
		,_result_value3 = (SELECT COUNT(DISTINCT SP_ID) FROM #temp_VSS WHERE IsPV13 = 1 )    
	WHERE _display_seq = 9



	-----------------------------------------
	-- (ii) Measles Vaccination  
	-----------------------------------------
	INSERT INTO #Subresult_table (_display_seq, _result_value1)    
	VALUES (11, '(ii) Measles Vaccination')     	       
	INSERT INTO #Subresult_table (_display_seq) VALUES (12)   
	INSERT INTO #Subresult_table (_display_seq, _result_value1, _result_value2, _result_value3)  
	VALUES (13, '', 'Sub-total', 'No. of SP involved')    
	INSERT INTO #Subresult_table (_display_seq, _result_value1, _result_value2, _result_value3)
	VALUES (14, 'MMR-FDH^', '', ''),
		   (15, 'MMR-NIA^^', '', '')    
	INSERT INTO #Subresult_table (_display_seq) VALUES (16)

	UPDATE #Subresult_table    
	SET _result_value2 = (SELECT COUNT(1) FROM #temp_VSS WHERE IsMMR = 1 AND Subsidize_Code = 'VFDHMMR' )  
		,_result_value3 = (SELECT COUNT(DISTINCT SP_ID) FROM #temp_VSS WHERE IsMMR = 1 AND Subsidize_Code = 'VFDHMMR' )   
	WHERE _display_seq = 14

	UPDATE #Subresult_table    
	SET _result_value2 = (SELECT COUNT(1) FROM #temp_VSS WHERE IsMMR = 1 AND Subsidize_Code = 'VNIAMMR'  )  
		,_result_value3 = (SELECT COUNT(DISTINCT SP_ID) FROM #temp_VSS WHERE IsMMR = 1 AND Subsidize_Code = 'VNIAMMR' )   
	WHERE _display_seq = 15

	INSERT INTO @AgeDose (Age, AgeDesc, DoseDesc, MustShow)
	VALUES
	('_64Y_1stDose',		'<65 age year',			'1st dose',		'N')
	,('_64Y_2ndDose',		'<65 age year',			'2nd dose',		'N')
	,('_64Y_OnlyDose',		'<65 age year',			'Only dose',	'Y')
	,('_64Y_3rdDose',		'<65 age year',			'3rd dose',		'N')

	,('_65Y_1stDose',		'''''=65 age year',		'1st dose',		'N')
	,('_65Y_2ndDose',		'''''=65 age year',		'2nd dose',		'N')
	,('_65Y_OnlyDose',		'''''=65 age year',		'Only dose',	'Y')
	,('_65Y_3rdDose',		'''''=65 age year',		'3rd dose',		'N')

	,('_66Y_69Y_1stDose',	'66 to 69 age year',	'1st dose',		'N')
	,('_66Y_69Y_2ndDose',	'66 to 69 age year',	'2nd dose',		'N')
	,('_66Y_69Y_OnlyDose',	'66 to 69 age year',	'Only dose',	'Y')
	,('_66Y_69Y_3rdDose',	'66 to 69 age year',	'3rd dose',		'N')

	,('_70Y_79Y_1stDose',	'70 to 79 age year',	'1st dose',		'N')
	,('_70Y_79Y_2ndDose',	'70 to 79 age year',	'2nd dose',		'N')
	,('_70Y_79Y_OnlyDose',	'70 to 79 age year',	'Only dose',	'Y')
	,('_70Y_79Y_3rdDose',	'70 to 79 age year',	'3rd dose',		'N')

	,('_80Y_1stDose',		'>= 80 age year',		'1st dose',		'N')
	,('_80Y_2ndDose',		'>= 80 age year',		'2nd dose',		'N')
	,('_80Y_OnlyDose',		'>= 80 age year',		'Only dose',	'Y')
	,('_80Y_3rdDose',		'>= 80 age year',		'3rd dose',		'N')

	SET @pivot_table_column_header	= NULL
	SET @pivot_table_column_list	= NULL 
	SET @pivot_table_column_sum		= NULL

	SELECT    
		@pivot_table_column_header = COALESCE(@pivot_table_column_header + ',', '') + '[' + Age + '] INT',
		@pivot_table_column_list = COALESCE(@pivot_table_column_list + ',', '') + '[' + Age + ']',
		@pivot_table_column_sum = COALESCE(@pivot_table_column_sum + ',', '') + 'ISNULL(SUM([' + Age + ']),0)'
	FROM @AgeDose     
 
	SET @pivot_table_column_header = @pivot_table_column_header + ',[Total] INT'   	
	SET @pivot_table_column_list = @pivot_table_column_list + ',[Total]'
	SET @pivot_table_column_sum = @pivot_table_column_sum + ',ISNULL(SUM([Total]),0)'
	SET @sql = 'ALTER TABLE #result_table_Age2 ADD ' + @pivot_table_column_header 
	  	
	EXECUTE(@sql)  
	-----------------------------------------
	-- (iii) By age group  
	----------------------------------------- 
	INSERT INTO #Subresult_table (_display_seq, _result_value1)     
	VALUES (50, '(iii) By age group')  
	-- 23vPPV --
	-- By Age		
	-- <65y
	SELECT 
		@result1 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result2 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result3 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result4 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) < 65
		
	-- 65y
	SELECT 
		@result5 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result6 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result7 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result8 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) = 65
									
	-- 66y - 69y
	SELECT 
		@result9 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result10 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result11 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result12 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 66 AND DateDiff(yy, DOB, Service_receive_dtm) <= 69

	-- 70y - 79y	
	SELECT 
		@result13 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result14 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result15 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result16 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 70 AND DateDiff(yy, DOB, Service_receive_dtm) <= 79

						
	-- 80y
	SELECT 
		@result17 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result18 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result19 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result20 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1  
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 80
							
	-- Total
	SET @result21 = @result1 + @result2 + @result3 + @result4 + @result5 + @result6 + @result7 + @result8 + @result9 + @result10
					+ @result11 + @result12 + @result13 + @result14 + @result15 + @result16 + @result17 + @result18 + @result19 + @result20
					 
	SET @sql = 'INSERT INTO #result_table_age2 (Display_Seq, Display_Code,' + @pivot_table_column_list + ') ' +
				'VALUES (100 , ''' + '23vPPV#' + ''', ' +  
				CONVERT(VARCHAR, @result1) + ', ' + CONVERT(VARCHAR, @result2) + ', ' + CONVERT(VARCHAR, @result3) + ', ' +	CONVERT(VARCHAR, @result4) + ', ' + 
				CONVERT(VARCHAR, @result5) + ', ' + CONVERT(VARCHAR, @result6) + ', ' + CONVERT(VARCHAR, @result7) + ', ' + CONVERT(VARCHAR, @result8) + ', ' + 
				CONVERT(VARCHAR, @result9) + ', ' + CONVERT(VARCHAR, @result10) + ', ' + CONVERT(VARCHAR, @result11) + ', ' + CONVERT(VARCHAR, @result12) + ', ' +
				CONVERT(VARCHAR, @result13) + ', ' + CONVERT(VARCHAR, @result14) + ', ' + CONVERT(VARCHAR, @result15) + ', ' + CONVERT(VARCHAR, @result16) + ', ' +
				CONVERT(VARCHAR, @result17) + ', ' + CONVERT(VARCHAR, @result18) + ', ' + CONVERT(VARCHAR, @result19) + ', ' + CONVERT(VARCHAR, @result20) + ', ' +
				CONVERT(VARCHAR, @result21) + ')'
				
	EXEC (@sql)
	
	-- 23vPPV End --
		
	-- 23vPPV (High Risk)--
	-- By Age	
	-- <65y
	SELECT 
		@result1 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result2 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result3 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result4 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) < 65
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
		
	-- 65y
	SELECT 
		@result5 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result6 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result7 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result8 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) = 65
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
									
	-- 66y - 69y
	SELECT 
		@result9 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result10 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result11 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result12 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 66 AND DateDiff(yy, DOB, Service_receive_dtm) <= 69
	AND High_Risk IS NOT NULL AND High_Risk ='Y'

	-- 70y - 79y	
	SELECT 
		@result13 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result14 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result15 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result16 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 70 AND DateDiff(yy, DOB, Service_receive_dtm) <= 79
	AND High_Risk IS NOT NULL AND High_Risk ='Y'

						
	-- 80y
	SELECT 
		@result17 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result18 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result19 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result20 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 80
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
							
	-- Total
	SET @result21 = @result1 + @result2 + @result3 + @result4 + @result5 + @result6 + @result7 + @result8 + @result9 + @result10
					+ @result11 + @result12 + @result13 + @result14 + @result15 + @result16 + @result17 + @result18 + @result19 + @result20
					 
	SET @sql = 'INSERT INTO #result_table_age2 (Display_Seq, Display_Code,' + @pivot_table_column_list + ') ' +
				'VALUES (101 , ''' + '23vPPV (' + @Str_HighRisk + ')'', ' +  
				CONVERT(VARCHAR, @result1) + ', ' + CONVERT(VARCHAR, @result2) + ', ' + CONVERT(VARCHAR, @result3) + ', ' +	CONVERT(VARCHAR, @result4) + ', ' + 
				CONVERT(VARCHAR, @result5) + ', ' + CONVERT(VARCHAR, @result6) + ', ' + CONVERT(VARCHAR, @result7) + ', ' + CONVERT(VARCHAR, @result8) + ', ' + 
				CONVERT(VARCHAR, @result9) + ', ' + CONVERT(VARCHAR, @result10) + ', ' + CONVERT(VARCHAR, @result11) + ', ' + CONVERT(VARCHAR, @result12) + ', ' +
				CONVERT(VARCHAR, @result13) + ', ' + CONVERT(VARCHAR, @result14) + ', ' + CONVERT(VARCHAR, @result15) + ', ' + CONVERT(VARCHAR, @result16) + ', ' +
				CONVERT(VARCHAR, @result17) + ', ' + CONVERT(VARCHAR, @result18) + ', ' + CONVERT(VARCHAR, @result19) + ', ' + CONVERT(VARCHAR, @result20) + ', ' +
				CONVERT(VARCHAR, @result21) + ')'

	EXEC (@sql)
	
	-- 23vPPV (High Risk) End --

	-- PCV13 (High Risk)--
	-- By Age
	-- <65y
	SELECT 
		@result1 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result2 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result3 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result4 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) < 65
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
		
	-- 65y
	SELECT 
		@result5 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result6 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result7 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result8 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) = 65
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
									
	-- 66y - 69y
	SELECT 
		@result9 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result10 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result11 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result12 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 66 AND DateDiff(yy, DOB, Service_receive_dtm) <= 69
	AND High_Risk IS NOT NULL AND High_Risk ='Y'

	-- 70y - 79y	
	SELECT 
		@result13 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result14 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result15 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result16 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 70 AND DateDiff(yy, DOB, Service_receive_dtm) <= 79
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
					
	-- 80y
	SELECT 
		@result17 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result18 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result19 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result20 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 80
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
							
	-- Total
	SET @result21 = @result1 + @result2 + @result3 + @result4 + @result5 + @result6 + @result7 + @result8 + @result9 + @result10
					+ @result11 + @result12 + @result13 + @result14 + @result15 + @result16 + @result17 + @result18 + @result19 + @result20

	SET @sql = 'INSERT INTO #result_table_age2 (Display_Seq, Display_Code,' + @pivot_table_column_list + ') ' +
				'VALUES (102 , ''' + 'PCV13 (' + @Str_HighRisk + ')**'', ' +  
				CONVERT(VARCHAR, @result1) + ', ' + CONVERT(VARCHAR, @result2) + ', ' + CONVERT(VARCHAR, @result3) + ', ' +	CONVERT(VARCHAR, @result4) + ', ' + 
				CONVERT(VARCHAR, @result5) + ', ' + CONVERT(VARCHAR, @result6) + ', ' + CONVERT(VARCHAR, @result7) + ', ' + CONVERT(VARCHAR, @result8) + ', ' + 
				CONVERT(VARCHAR, @result9) + ', ' + CONVERT(VARCHAR, @result10) + ', ' + CONVERT(VARCHAR, @result11) + ', ' + CONVERT(VARCHAR, @result12) + ', ' +
				CONVERT(VARCHAR, @result13) + ', ' + CONVERT(VARCHAR, @result14) + ', ' + CONVERT(VARCHAR, @result15) + ', ' + CONVERT(VARCHAR, @result16) + ', ' +
				CONVERT(VARCHAR, @result17) + ', ' + CONVERT(VARCHAR, @result18) + ', ' + CONVERT(VARCHAR, @result19) + ', ' + CONVERT(VARCHAR, @result20) + ', ' +
				CONVERT(VARCHAR, @result21) + ')'

	EXEC (@sql)
		
	-- PCV13 (High Risk)End --
		
	-- PCV13 + with Voucher--
	-- By Age	
	-- By Age		
	-- <65y
	SELECT 
		@result1 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result2 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result3 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result4 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsWithVoucher = 1
	AND DateDiff(yy, DOB, Service_receive_dtm) < 65
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
		
	-- 65y
	SELECT 
		@result5 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result6 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result7 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result8 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsWithVoucher = 1
	AND DateDiff(yy, DOB, Service_receive_dtm) = 65
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
									
	-- 66y - 69y
	SELECT 
		@result9 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result10 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result11 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result12 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsWithVoucher = 1
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 66 AND DateDiff(yy, DOB, Service_receive_dtm) <= 69
	AND High_Risk IS NOT NULL AND High_Risk ='Y'

	-- 70y - 79y	
	SELECT 
		@result13 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result14 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result15 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result16 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsWithVoucher = 1
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 70 AND DateDiff(yy, DOB, Service_receive_dtm) <= 79
	AND High_Risk IS NOT NULL AND High_Risk ='Y'

						
	-- 80y
	SELECT 
		@result17 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result18 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result19 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result20 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsPV13 = 1 AND IsWithVoucher = 1
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 80
	AND High_Risk IS NOT NULL AND High_Risk ='Y'
									
	-- Total
	SET @result21 = @result1 + @result2 + @result3 + @result4 + @result5 + @result6 + @result7 + @result8 + @result9 + @result10
					+ @result11 + @result12 + @result13 + @result14 + @result15 + @result16 + @result17 + @result18 + @result19 + @result20
					 
	SET @sql = 'INSERT INTO #result_table_age2 (Display_Seq, Display_Code,' + @pivot_table_column_list + ') ' +
				'VALUES (103 , ''PCV13 and using healthcare voucher***'', ' +  
				CONVERT(VARCHAR, @result1) + ', ' + CONVERT(VARCHAR, @result2) + ', ' + CONVERT(VARCHAR, @result3) + ', ' +	CONVERT(VARCHAR, @result4) + ', ' + 
				CONVERT(VARCHAR, @result5) + ', ' + CONVERT(VARCHAR, @result6) + ', ' + CONVERT(VARCHAR, @result7) + ', ' + CONVERT(VARCHAR, @result8) + ', ' + 
				CONVERT(VARCHAR, @result9) + ', ' + CONVERT(VARCHAR, @result10) + ', ' + CONVERT(VARCHAR, @result11) + ', ' + CONVERT(VARCHAR, @result12) + ', ' +
				CONVERT(VARCHAR, @result13) + ', ' + CONVERT(VARCHAR, @result14) + ', ' + CONVERT(VARCHAR, @result15) + ', ' + CONVERT(VARCHAR, @result16) + ', ' +
				CONVERT(VARCHAR, @result17) + ', ' + CONVERT(VARCHAR, @result18) + ', ' + CONVERT(VARCHAR, @result19) + ', ' + CONVERT(VARCHAR, @result20) + ', ' +
				CONVERT(VARCHAR, @result21) + ')'

	EXEC (@sql)
	
	-- PCV13 With Voucher End --


	-- MMR-FDH --
	-- By Age		
	-- <65y
	SELECT 
		@result1 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result2 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result3 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result4 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND Subsidize_Code = 'VFDHMMR'
	AND DateDiff(yy, DOB, Service_receive_dtm) < 65
		
	-- 65y
	SELECT 
		@result5 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result6 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result7 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result8 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND Subsidize_Code = 'VFDHMMR'
	AND DateDiff(yy, DOB, Service_receive_dtm) = 65
									
	-- 66y - 69y
	SELECT 
		@result9 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result10 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result11 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result12 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND Subsidize_Code = 'VFDHMMR'
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 66 AND DateDiff(yy, DOB, Service_receive_dtm) <= 69

	-- 70y - 79y	
	SELECT 
		@result13 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result14 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result15 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result16 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND Subsidize_Code = 'VFDHMMR'
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 70 AND DateDiff(yy, DOB, Service_receive_dtm) <= 79

						
	-- 80y
	SELECT 
		@result17 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result18 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result19 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result20 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND Subsidize_Code = 'VFDHMMR'
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 80
							
	-- Total
	SET @result21 = @result1 + @result2 + @result3 + @result4 + @result5 + @result6 + @result7 + @result8 + @result9 + @result10
					+ @result11 + @result12 + @result13 + @result14 + @result15 + @result16 + @result17 + @result18 + @result19 + @result20
					 
	SET @sql = 'INSERT INTO #result_table_age2 (Display_Seq, Display_Code,' + @pivot_table_column_list + ') ' +
				'VALUES (104 , ''' + 'MMR-FDH^' + ''', ' +  
				CONVERT(VARCHAR, @result1) + ', ' + CONVERT(VARCHAR, @result2) + ', ' + CONVERT(VARCHAR, @result3) + ', ' +	CONVERT(VARCHAR, @result4) + ', ' + 
				CONVERT(VARCHAR, @result5) + ', ' + CONVERT(VARCHAR, @result6) + ', ' + CONVERT(VARCHAR, @result7) + ', ' + CONVERT(VARCHAR, @result8) + ', ' + 
				CONVERT(VARCHAR, @result9) + ', ' + CONVERT(VARCHAR, @result10) + ', ' + CONVERT(VARCHAR, @result11) + ', ' + CONVERT(VARCHAR, @result12) + ', ' +
				CONVERT(VARCHAR, @result13) + ', ' + CONVERT(VARCHAR, @result14) + ', ' + CONVERT(VARCHAR, @result15) + ', ' + CONVERT(VARCHAR, @result16) + ', ' +
				CONVERT(VARCHAR, @result17) + ', ' + CONVERT(VARCHAR, @result18) + ', ' + CONVERT(VARCHAR, @result19) + ', ' + CONVERT(VARCHAR, @result20) + ', ' +
				CONVERT(VARCHAR, @result21) + ')'
				
	EXEC (@sql)
	
	-- MMR-FDH End --


	-- MMR-NIA --
	-- By Age		
	-- <65y
	SELECT 
		@result1 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result2 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result3 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result4 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND Subsidize_Code = 'VNIAMMR'
	AND DateDiff(yy, DOB, Service_receive_dtm) < 65
		
	-- 65y
	SELECT 
		@result5 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result6 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result7 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result8 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND Subsidize_Code = 'VNIAMMR'
	AND DateDiff(yy, DOB, Service_receive_dtm) = 65
									
	-- 66y - 69y
	SELECT 
		@result9 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result10 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result11 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result12 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND Subsidize_Code = 'VNIAMMR'
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 66 AND DateDiff(yy, DOB, Service_receive_dtm) <= 69

	-- 70y - 79y	
	SELECT 
		@result13 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result14 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result15 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result16 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND Subsidize_Code = 'VNIAMMR'
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 70 AND DateDiff(yy, DOB, Service_receive_dtm) <= 79

						
	-- 80y
	SELECT 
		@result17 = ISNULL(SUM(CASE WHEN Dose = '1STDOSE'  THEN 1 ELSE 0 END), 0),
		@result18 = ISNULL(SUM(CASE WHEN Dose = '2NDDOSE'  THEN 1 ELSE 0 END), 0),
		@result19 = ISNULL(SUM(CASE WHEN Dose = 'ONLYDOSE' THEN 1 ELSE 0 END), 0),
		@result20 = ISNULL(SUM(CASE WHEN Dose = '3RDDOSE' THEN 1 ELSE 0 END), 0)
	FROM #temp_VSS 
	WHERE IsMMR = 1 AND Subsidize_Code = 'VNIAMMR'
	AND DateDiff(yy, DOB, Service_receive_dtm) >= 80
							
	-- Total
	SET @result21 = @result1 + @result2 + @result3 + @result4 + @result5 + @result6 + @result7 + @result8 + @result9 + @result10
					+ @result11 + @result12 + @result13 + @result14 + @result15 + @result16 + @result17 + @result18 + @result19 + @result20
					 
	SET @sql = 'INSERT INTO #result_table_age2 (Display_Seq, Display_Code,' + @pivot_table_column_list + ') ' +
				'VALUES (104 , ''' + 'MMR-NIA^^' + ''', ' +  
				CONVERT(VARCHAR, @result1) + ', ' + CONVERT(VARCHAR, @result2) + ', ' + CONVERT(VARCHAR, @result3) + ', ' +	CONVERT(VARCHAR, @result4) + ', ' + 
				CONVERT(VARCHAR, @result5) + ', ' + CONVERT(VARCHAR, @result6) + ', ' + CONVERT(VARCHAR, @result7) + ', ' + CONVERT(VARCHAR, @result8) + ', ' + 
				CONVERT(VARCHAR, @result9) + ', ' + CONVERT(VARCHAR, @result10) + ', ' + CONVERT(VARCHAR, @result11) + ', ' + CONVERT(VARCHAR, @result12) + ', ' +
				CONVERT(VARCHAR, @result13) + ', ' + CONVERT(VARCHAR, @result14) + ', ' + CONVERT(VARCHAR, @result15) + ', ' + CONVERT(VARCHAR, @result16) + ', ' +
				CONVERT(VARCHAR, @result17) + ', ' + CONVERT(VARCHAR, @result18) + ', ' + CONVERT(VARCHAR, @result19) + ', ' + CONVERT(VARCHAR, @result20) + ', ' +
				CONVERT(VARCHAR, @result21) + ')'
				
	EXEC (@sql)
	
	-- MMR-NIA End --


	DECLARE Age_Cursor2 CURSOR FOR   
	SELECT Age, AgeDesc, DoseDesc, MustShow FROM @AgeDose  

	OPEN Age_Cursor2 
	FETCH NEXT FROM Age_Cursor2 INTO @Age, @AgeDesc, @DoseDesc, @MustShow
	   
	WHILE @@FETCH_STATUS = 0   
	BEGIN
		DECLARE @TotalCount2 as INT

		-- Total Count
		SET @sql = 'SELECT @TotalCount2 = SUM(' + @Age + ') FROM #result_table_age2 '
		SET @sql = @sql + ' WHERE Display_Seq = 91 OR Display_Seq = 100 OR Display_Seq = 101 OR Display_Seq = 102 OR Display_Seq = 103 OR Display_Seq = 104'		
		EXEC sp_executesql @sql, N'@TotalCount2 INT OUTPUT', @TotalCount2 = @TotalCount2 OUTPUT
		
		IF @MustShow = 'Y' OR @TotalCount2 > 0 
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

		FETCH NEXT FROM Age_Cursor2 INTO @Age, @AgeDesc, @DoseDesc, @MustShow
	END  
  
	CLOSE Age_Cursor2  
	DEALLOCATE Age_Cursor2  
	
	-- Insert to sub result table
	SET @ShowList = COALESCE(@ShowList + ',[Total]', '[Total]')
	SET @AgeHeaderList = COALESCE(@AgeHeaderList + ',''Total''', '''Total''')
	SET @DoseHeaderList = COALESCE(@DoseHeaderList + ',''''', '''''')

	SET @sql = 'INSERT INTO #Subresult_table (	_display_seq, _result_value1, _result_value2, _result_value3, _result_value4,   
				_result_value5, _result_value6, _result_value7, _result_value8, _result_value9, _result_value10,   
				_result_value11, _result_value12, _result_value13, _result_value14, _result_value15, _result_value16,   
				_result_value17, _result_value18, _result_value19, _result_value20, _result_value21, _result_value22) SELECT Display_Seq, Display_Code,' + 
				@ShowList + @EmptyList + ' FROM #result_table_age2'
	EXEC (@sql)
	
	---- Age Header
	SET @sql = 'INSERT INTO #Subresult_table (	_display_seq, _result_value1, _result_value2, _result_value3, _result_value4,   
				_result_value5, _result_value6, _result_value7, _result_value8, _result_value9, _result_value10,   
				_result_value11, _result_value12, _result_value13, _result_value14, _result_value15, _result_value16,   
				_result_value17, _result_value18, _result_value19, _result_value20, _result_value21, _result_value22) VALUES (51, '''',' + 
				@AgeHeaderList + @EmptyList + ')'
	EXEC (@sql)
	
	---- Dose Header
	SET @sql = 'INSERT INTO #Subresult_table (	_display_seq, _result_value1, _result_value2, _result_value3, _result_value4,   
				_result_value5, _result_value6, _result_value7, _result_value8, _result_value9, _result_value10,   
				_result_value11, _result_value12, _result_value13, _result_value14, _result_value15, _result_value16,   
				_result_value17, _result_value18, _result_value19, _result_value20, _result_value21, _result_value22) VALUES (52, '''',' + 
				@DoseHeaderList + @EmptyList + ')'
	EXEC (@sql)	

	
	-- Final Result
	DELETE FROM RpteHSD0028_02_VSS_Tx_ByAgeGroup_ByYear   
	 
	INSERT INTO RpteHSD0028_02_VSS_Tx_ByAgeGroup_ByYear    
	(
		Display_Seq,    
		Col1,  Col2,  Col3,  Col4,  Col5,  
		Col6,  Col7,  Col8,  Col9,  Col10,   
		Col11, Col12, Col13, Col14, Col15, 
		Col16, Col17, Col18, Col19, Col20,
		Col21, Col22, Col23, Col24, Col25, 
		Col26, Col27, Col28, Col29				
	)     
	SELECT  
		_display_seq,    
		_result_value1,  _result_value2,  _result_value3,  _result_value4,  _result_value5,   
		_result_value6,  _result_value7,  _result_value8,  _result_value9,  _result_value10,   
		_result_value11, _result_value12, _result_value13, _result_value14, _result_value15,
		_result_value16, _result_value17, _result_value18, _result_value19,	_result_value20,   
		_result_value21, _result_value22, _result_value23, _result_value24, _result_value25,   
		_result_value26, _result_value27, _result_value28, _result_value29		 
	 FROM #result_table    
	 ORDER BY    
		_display_seq  
		
	DELETE FROM RpteHSD0028_03_VSS_Tx_ByAgeGroup_Cumulative
	
	INSERT INTO RpteHSD0028_03_VSS_Tx_ByAgeGroup_Cumulative    
	(
		Display_Seq,    
		Col1,  Col2,  Col3,  Col4,  Col5,  
		Col6,  Col7,  Col8,  Col9,  Col10,   
		Col11, Col12, Col13, Col14, Col15, 
		Col16, Col17, Col18, Col19, Col20,
		Col21, Col22, Col23, Col24, Col25, 
		Col26, Col27, Col28, Col29				
	)     
	SELECT  
		_display_seq,    
		_result_value1,  _result_value2,  _result_value3,  _result_value4,  _result_value5,   
		_result_value6,  _result_value7,  _result_value8,  _result_value9,  _result_value10,   
		_result_value11, _result_value12, _result_value13, _result_value14, _result_value15,
		_result_value16, _result_value17, _result_value18, _result_value19,	_result_value20,   
		_result_value21, _result_value22, _result_value23, _result_value24, _result_value25,   
		_result_value26, _result_value27, _result_value28, _result_value29	 
	 FROM #Subresult_table    
	 ORDER BY    
		_display_seq     
	   
	    
	EXEC [proc_SymmetricKey_close] 

	DROP TABLE #account
	DROP TABLE #temp_HCVS
	DROP TABLE #temp_VSS 
	    
	DROP TABLE #result_table    
	DROP TABLE #result_table_age
	DROP TABLE #result_table_age2

	DROP TABLE #Subresult_table
    
END    
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0028_02_03_PrepareData] TO HCVU

GO

