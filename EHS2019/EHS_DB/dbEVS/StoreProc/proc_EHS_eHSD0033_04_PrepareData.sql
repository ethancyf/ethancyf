IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0033_04_PrepareData]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0033_04_PrepareData]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
  
  -- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	16 Nov 2020
-- CR. No			CRE20-014-02 (GOV SIV - Phase 2)
-- Description:		(1) Rectify [Vaccine] column data type to VARCHAR(25)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	16 Nov 2020
-- CR No.:			INT20-0050
-- Description:		Fix temp table column for "Display_Code_For_Claim" 
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	11 Oct 2020
-- CR. No			INT20-0036
-- Description:		Fix category name too Long
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	16 Jul 2020
-- CR. No			INT20-0025
-- Description:		(1) Add WITH (NOLOCK)
-- =============================================
-- =============================================    
-- Author:			Winnie SUEN
-- Create date:		04 Oct 2019
-- CR No.			CRE19-001-05 (PPP 2019-20 - Report)
-- Description:		PPPKG daily report - Sub report 04 Raw data
-- =============================================  
    
CREATE Procedure [proc_EHS_eHSD0033_04_PrepareData]    
	@Cutoff_Dtm AS DATETIME    
AS    
BEGIN    
	SET NOCOUNT ON;    
	-- =============================================    
	-- Declaration    
	-- =============================================    
	DECLARE @Report_Dtm DATETIME    
	DECLARE @Date_Range TINYINT   
	DECLARE @Scheme_Code VARCHAR(10)
	DECLARE @Report_ID VARCHAR(30)     
      
	-- =============================================    
	-- Validation     
	-- =============================================    
	-- =============================================    
	-- Initialization    
	-- =============================================    
	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	SET @Report_Dtm = DATEADD(dd, -1, @Cutoff_Dtm)         
	SET @Date_Range = 7
	SET @Scheme_Code = 'PPPKG'
	SET @Report_ID = 'eHSD0033'

	-- =============================================    
	-- Return results    
	-- =============================================    
	-- =============================================          
	-- Temporary tables          
	-- =============================================          
	 CREATE TABLE #Transaction (  
		SP_ID					CHAR(8), 
		Transaction_ID			CHAR(20),
		Transaction_Dtm			DATETIME,
		Service_Receive_Dtm		DATETIME,
		Subsidize_Item_Code		VARCHAR(10),
		DOSE					CHAR(20),
		Scheme_seq				INT,
		Per_Unit_Value			INT, 
		Voucher_Acc_ID			CHAR(15),
		Temp_Voucher_Acc_ID		CHAR(15),
		Special_Acc_ID			CHAR(15),
		Invalid_Acc_ID			CHAR(15),
		Doc_Code				CHAR(20),
		Transaction_Status		CHAR(1),
		Reimbursement_Status	CHAR(1),
		Create_By_SmartID		CHAR(1),
		Row						INT,
		Vaccine					VARCHAR(25),
		Category_Code			VARCHAR(10),
		SchoolCode				VARCHAR(30)
	 )        

          
	CREATE TABLE #Account (          
		SP_ID					CHAR(8),
		Transaction_ID			CHAR(20),
		Transaction_Dtm			DATETIME,
		Service_Receive_Dtm		DATETIME,
		Subsidize_Item_Code		VARCHAR(10),
		DOSE					CHAR(20),
		DOB						DATETIME,
		Exact_DOB				CHAR(1),
		Sex						CHAR(1),
		Doc_Code				CHAR(20),
		Transaction_Status		CHAR(1),
		Reimbursement_Status	CHAR(1),
		Create_By_SmartID		CHAR(1),
		Row						INT,
		Vaccine					VARCHAR(25),
		Category_Code			VARCHAR(10),
		SchoolCode				VARCHAR(30)
	)          
           
	DECLARE @ResultTable TABLE (
		Result_Seq				INT,
		Result_Value1			VARCHAR(100) default '',
		Result_Value2			VARCHAR(100) default '',
		Result_Value3			VARCHAR(100) default '',
		Result_Value4			VARCHAR(100) default '',
		Result_Value5			VARCHAR(200) default '',
		Result_Value6			VARCHAR(100) default '',
		Result_Value7			VARCHAR(100) default '',
		Result_Value8			VARCHAR(100) default '',
		Result_Value9			VARCHAR(100) default '',
		Result_Value10			VARCHAR(100) default '',
		Result_Value11			VARCHAR(100) default '',
		Result_Value12			VARCHAR(100) default '',
		Result_Value13			VARCHAR(100) default '',
		Result_Value14			VARCHAR(100) default '',
		Result_Value15			VARCHAR(100) default '',
		Result_Value16			VARCHAR(100) default '',
		Result_Value17			VARCHAR(100) default '',
		Result_Value18			VARCHAR(100) default '',
		Result_Value19			VARCHAR(100) default '',
		Result_Value20			VARCHAR(100) default ''
	)          
    
          
	-- ---------------------------------------------          
	-- Transactions          
	-- ---------------------------------------------                

	INSERT INTO #Transaction (
		SP_ID,
		Transaction_ID,
		Transaction_Dtm,
		Service_Receive_Dtm,
		Subsidize_Item_Code,
		DOSE,
		Scheme_Seq,
		Per_Unit_Value,
		Voucher_Acc_ID,
		Temp_Voucher_Acc_ID,
		Special_Acc_ID,
		Invalid_Acc_ID,
		Doc_Code,
		Transaction_Status,
		Reimbursement_Status,
		Create_By_SmartID,
		Row,
		Vaccine,
		Category_Code,
		SchoolCode
	)          
	SELECT          
		VT.SP_ID,
		VT.Transaction_ID,
		VT.Transaction_Dtm,
		VT.Service_Receive_Dtm,
		TD.Subsidize_Item_Code,
		TD.Available_Item_Code AS [Dose],
		TD.Scheme_Seq,
		TD.Per_Unit_Value, 
		ISNULL(VT.Voucher_Acc_ID, ''), 
		ISNULL(VT.Temp_Voucher_Acc_ID, ''), 
		ISNULL(VT.Special_Acc_ID, ''), 
		ISNULL(VT.Invalid_Acc_ID, ''), 
		VT.Doc_Code, 
		VT.Record_Status AS [Transaction_Status],
		NULL AS [Reimbursement_Status],
		VT.create_by_smartid, 
		10 + ROW_NUMBER() OVER (ORDER BY VT.Transaction_Dtm),
		SGC.Display_Code_For_Claim AS [Vaccine],
		VT.Category_Code,
		TAF.AdditionalFieldValueCode AS [SchoolCode]
	FROM          
		VoucherTransaction VT WITH (NOLOCK) 
	INNER JOIN 
		transactiondetail td WITH (NOLOCK) ON vt.transaction_id = td.transaction_id  AND vt.scheme_code = @Scheme_Code AND vt.scheme_code = td.scheme_code     
	INNER JOIN 
		TransactionAdditionalField TAF WITH (NOLOCK) ON VT.Transaction_ID = TAF.Transaction_ID AND TAF.AdditionalFieldID = 'SchoolCode'
	LEFT JOIN 
		SubsidizeGroupClaim SGC WITH (NOLOCK) ON	td.Scheme_Code = SGC.Scheme_Code
									AND	td.Scheme_Seq = SGC.Scheme_Seq
									AND td.Subsidize_Code = SGC.Subsidize_Code   
	WHERE          
	VT.Scheme_Code = @Scheme_Code 
	AND VT.Transaction_Dtm <= @Cutoff_Dtm          
	AND VT.Transaction_Dtm > DATEADD(dd, -1 * @Date_Range + 1, @Report_Dtm)          
	AND VT.Record_Status NOT IN (
			SELECT Status_Value 
			FROM StatStatusFilterMapping WITH (NOLOCK) 
			WHERE (report_id = 'ALL' OR report_id = @Report_ID)     
			AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'     
			AND ((Effective_Date IS NULL OR Effective_Date <= @cutoff_dtm) AND (Expiry_Date IS NULL OR Expiry_Date >= @cutoff_dtm))
	)       
	AND (VT.Invalidation IS NULL OR VT.Invalidation NOT IN (     
			SELECT Status_Value 
			FROM StatStatusFilterMapping WITH (NOLOCK) 
			WHERE (report_id = 'ALL' OR report_id = @Report_ID)     
	AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'    
		AND ((Effective_Date IS NULL OR Effective_Date <= @cutoff_dtm) AND (Expiry_Date IS NULL OR Expiry_Date >= @cutoff_dtm))
	))    
	ORDER BY   
		VT.Transaction_Dtm

	-- ---------------------------------------------          
	-- Patch the Reimbursement_Status 
	-- for transaction created in payment outside eHS
	-- ---------------------------------------------          

	UPDATE 
		#Transaction 
	SET 
		Reimbursement_Status = 'R'
	WHERE 
		Transaction_Status = 'R'

-- ---------------------------------------------          
-- Patch the Reimbursement_Status          
-- ---------------------------------------------          
          
	UPDATE          
		#Transaction          
	SET          
		Reimbursement_Status =           
		CASE RAT.Authorised_Status          
			WHEN 'R' THEN 'G'          
			ELSE RAT.Authorised_Status          
		END          
	FROM          
		#Transaction VT          
		INNER JOIN ReimbursementAuthTran RAT WITH (NOLOCK)          
		ON VT.Transaction_ID = RAT.Transaction_ID          
	WHERE VT.Transaction_Status = 'A'         	
                    
          
	-- ---------------------------------------------          
	-- Patch the Transaction_Status          
	-- ---------------------------------------------          
          
	UPDATE          
		#Transaction          
	SET          
		Transaction_Status = 'R'    
	WHERE          
		Reimbursement_Status = 'G'    
          
          
	-- ---------------------------------------------          
	-- Validated accounts          
	-- ---------------------------------------------          
          
	INSERT INTO #Account (          
		SP_ID, 
		Transaction_ID, 
		Transaction_Dtm, 
		Service_Receive_Dtm, 
		Subsidize_Item_Code,
		DOSE, 
		DOB, 
		Exact_DOB, 
		Sex, 
		Doc_Code, 
		Transaction_Status, 
		Reimbursement_Status,
		Row,
		Create_By_SmartID,
		Vaccine,
		Category_Code,
		SchoolCode
	)          
	SELECT          
		VT.SP_ID, 
		VT.Transaction_ID, 
		VT.Transaction_Dtm, 
		VT.Service_Receive_Dtm, 
		VT.Subsidize_Item_Code,
		VT.DOSE,
		VP.DOB, 
		VP.Exact_DOB, 
		VP.Sex, 
		VT.Doc_Code, 
		VT.Transaction_Status, 
		VT.Reimbursement_Status,
		VT.Row,
		VT.Create_By_SmartID,
		VT.Vaccine,
		VT.Category_Code,
		VT.SchoolCode
	FROM          
		#Transaction VT          
	INNER JOIN 
		PersonalInformation VP ON VT.Voucher_Acc_ID = VP.Voucher_Acc_ID          
									AND VT.Doc_Code = VP.Doc_Code          
	WHERE          
		VT.Voucher_Acc_ID <> ''          
          
          
	-- ---------------------------------------------          
	-- Temporary accounts          
	-- ---------------------------------------------          
          
	INSERT INTO #Account (          
		SP_ID, 
		Transaction_ID, 
		Transaction_Dtm, 
		Service_Receive_Dtm, 
		Subsidize_Item_Code,
		DOSE,
		DOB, 
		Exact_DOB, 
		Sex, 
		Doc_Code, 
		Transaction_Status, 
		Reimbursement_Status,
		Row,
		Create_By_SmartID,
		Vaccine,
		Category_Code,
		SchoolCode     
	)          
	SELECT          
		VT.SP_ID,
		VT.Transaction_ID,
		VT.Transaction_Dtm,
		VT.Service_Receive_Dtm,
		VT.Subsidize_Item_Code,
		VT.DOSE,
		TP.DOB,
		TP.Exact_DOB,
		TP.Sex,
		VT.Doc_Code,
		VT.Transaction_Status,
		VT.Reimbursement_Status,
		VT.Row,
		VT.Create_By_SmartID,
		VT.Vaccine,
		VT.Category_Code,
		VT.SchoolCode
	FROM          
		#Transaction VT          
		INNER JOIN TempPersonalInformation TP WITH (NOLOCK)          
		ON VT.Temp_Voucher_Acc_ID = TP.Voucher_Acc_ID          
	WHERE          
		VT.Voucher_Acc_ID = ''          
		AND VT.Temp_Voucher_Acc_ID <> ''          
		AND VT.Special_Acc_ID = ''          
          
          
	-- ---------------------------------------------
	-- Special accounts          
	-- ---------------------------------------------
          
	INSERT INTO #Account (          
		SP_ID,
		Transaction_ID,
		Transaction_Dtm,
		Service_Receive_Dtm,
		Subsidize_Item_Code,
		DOSE,
		DOB,
		Exact_DOB,
		Sex,
		Doc_Code,
		Transaction_Status,
		Reimbursement_Status,
		Row,
		Create_By_SmartID,
		Vaccine,
		Category_Code,
		SchoolCode  
	)          
	SELECT          
		VT.SP_ID,
		VT.Transaction_ID,
		VT.Transaction_Dtm,
		VT.Service_Receive_Dtm,
		VT.Subsidize_Item_Code,
		VT.DOSE,
		SP.DOB,
		SP.Exact_DOB,
		SP.Sex,
		VT.Doc_Code,
		VT.Transaction_Status,
		VT.Reimbursement_Status,
		VT.Row,
		VT.Create_By_SmartID,
		VT.Vaccine,
		VT.Category_Code,
		VT.SchoolCode
	FROM          
		#Transaction VT          
		INNER JOIN SpecialPersonalInformation SP WITH (NOLOCK)          
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
	DECLARE @Display_Text_RecepientCondition 	VARCHAR(100)
	SELECT @Display_Text_RecepientCondition = Description FROM SystemResource WITH (NOLOCK) WHERE ObjectType='Text' AND ObjectName='RecipientCondition'
                  
	INSERT INTO @ResultTable (result_seq, result_value1)    
	VALUES (0, 'eHS(S)D0033-04: Raw Data of PPP-KG claim transactions')    
	INSERT INTO @ResultTable (result_seq)    
	VALUES (1)    
	INSERT INTO @ResultTable (result_seq, result_value1)    
	VALUES (2, 'Reporting period: the week ending ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111))    
	INSERT INTO @ResultTable (result_seq)    
	VALUES (3)    
	INSERT INTO @ResultTable (result_seq, result_value1, result_value2, result_value3, result_value4, result_value5, 
	result_value6, result_value7, result_value8, result_value9, result_value10, result_value11, result_value12, Result_Value13, 
	Result_Value14, Result_Value15)
	VALUES (4, 'Transaction ID', 'Transaction Time', 'SPID', 'Service Date', 'Category', 'Subsidy', 'Dose',
	 'DOB', 'DOB Flag', 'Gender', 'Doc Type', 'School Code', 'Transaction Status', 'Reimbursement Status', 'Means of Input')
    
	-- ---------------------------------------------          
	-- Build data          
	-- ---------------------------------------------          
      
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, 
	Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, 
	Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15)
          
	SELECT          
		A.Row,
		dbo.func_format_system_number(A.Transaction_ID),
		CONVERT(varchar, A.Transaction_Dtm, 20),
		A.SP_ID,
		FORMAT(A.Service_Receive_Dtm, 'yyyy/MM/dd'),
		CC.Category_Name AS [Category],
		A.Vaccine as 'Subsidy',
		CASE A.Dose
			WHEN 'ONLYDOSE' THEN 'Only Dose'
			ELSE SID.Available_Item_Desc
		END AS [Dose],
		FORMAT(A.DOB, 'yyyy/MM/dd'),
		A.Exact_DOB,
		A.Sex,
		A.Doc_Code,
		A.SchoolCode,
		SD1.Status_Description,
		ISNULL(SD2.Status_Description, ''),
		CASE when A.Create_By_SmartID = 'Y' THEN 'Card Reader' ELSE 'Manual' END Create_By_SmartID
	FROM          
	#Account A
	INNER JOIN SubsidizeItemDetails SID WITH (NOLOCK)
		ON A.Dose = SID.Available_Item_Code
		AND SID.Subsidize_Item_Code = A.Subsidize_Item_Code
	INNER JOIN StatusData SD1 WITH (NOLOCK)
		ON A.Transaction_Status = SD1.Status_Value
		AND SD1.Enum_Class = 'ClaimTransStatus'
	INNER JOIN ClaimCategory CC WITH (NOLOCK)
		ON A.Category_Code = CC.Category_Code
	LEFT JOIN StatusData SD2 WITH (NOLOCK)
		ON A.Reimbursement_Status = SD2.Status_Value
			AND SD2.Enum_Class = 'ReimbursementStatus'
               
	-- =============================================          
	-- Return result          
	-- =============================================          
	DELETE FROM [RpteHSD0033_04_PPPKG_Tx_Raw]
	INSERT INTO [RpteHSD0033_04_PPPKG_Tx_Raw] (
		Display_Seq,
		Col1,
		Col2,
		Col3,
		Col4,
		Col5,
		Col6,
		Col7,
		Col8,
		Col9,
		Col10,
		Col11,
		Col12,
		Col13,
		Col14,
		Col15,
		Col16,
		Col17,
		Col18,
		Col19,
		Col20
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
		Result_Value13,
		Result_Value14,
		Result_Value15,
		Result_Value16,
		Result_Value17,
		Result_Value18,
		Result_Value19,
		Result_Value20
	FROM          
		@ResultTable          
	ORDER BY 
		Result_Seq        
          
	CLOSE SYMMETRIC KEY sym_Key    
    
	DROP TABLE #Transaction
	DROP TABLE #Account

END    
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0033_04_PrepareData] TO HCVU
GO

