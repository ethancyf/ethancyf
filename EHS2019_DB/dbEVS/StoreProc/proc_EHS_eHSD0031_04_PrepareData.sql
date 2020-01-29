IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0031_04_PrepareData]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0031_04_PrepareData]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
    
-- =============================================
-- Modification History
-- CR No.:			CRE18-020 (HKIC Symbol Others)
-- Modified by:		Winnie SUEN
-- Modified date:	25 Feb 2019
-- Description:		Show HKIC Symbol Description
-- =============================================
-- =============================================
-- CR No.:			CRE17-018-05
-- Author:			Koala CHENG
-- Create date:		26 Sep 2018
-- Description:		ENHVSSO daily report - Sub report 04 Raw data
-- =============================================    
    
CREATE Procedure [proc_EHS_eHSD0031_04_PrepareData]    
	@Cutoff_Dtm AS DATETIME    
AS    
BEGIN    
	SET NOCOUNT ON;    
	-- =============================================    
	-- Declaration    
	-- =============================================    
	DECLARE @system_Dtm DATETIME    
	DECLARE @Report_Dtm DATETIME    
	DECLARE @Date_Range TINYINT   
	DECLARE @Scheme_Code VARCHAR(10)
	DECLARE @Report_ID VARCHAR(30)     
	DECLARE @Str_NA VARCHAR(10)
      
	-- =============================================    
	-- Validation     
	-- =============================================    
	-- =============================================    
	-- Initialization    
	-- =============================================    
	OPEN SYMMETRIC KEY sym_Key
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	SET @Report_Dtm = DATEADD(dd, -1, @Cutoff_Dtm)         
	SET @system_Dtm = getdate()
	SET @Date_Range = 7
	SET @Scheme_Code = 'ENHVSSO'
	SET @Report_ID = 'eHSD0031'

	SELECT @Str_NA = Description FROM SystemResource WHERE ObjectType = 'Text' AND ObjectName='NA'
	
	-- =============================================    
	-- Return results    
	-- =============================================    
	-- =============================================          
	-- Temporary tables          
	-- =============================================          
	 DECLARE @Transaction TABLE (  
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
		Vaccine					CHAR(20),
		Category_Code			VARCHAR(10),
		HKIC_Symbol				CHAR(1),
		PlaceVaccination		VARCHAR(50)
	 )        

          
	DECLARE @Account TABLE (          
		SP_ID					CHAR(8),
		Transaction_ID			CHAR(20),
		Transaction_Dtm			DATETIME,
		Service_Receive_Dtm		DATETIME,
		DOSE					CHAR(20),
		DOB						DATETIME,
		Exact_DOB				CHAR(1),
		Sex						CHAR(1),
		Doc_Code				CHAR(20),
		Transaction_Status		CHAR(1),
		Reimbursement_Status	CHAR(1),
		Row						INT,
		Vaccine					CHAR(20),
		Category_Code			VARCHAR(10)  
	)          
           
	DECLARE @ResultTable TABLE (
		Result_Seq				INT,
		Result_Value1			VARCHAR(100),
		Result_Value2			VARCHAR(100),
		Result_Value3			VARCHAR(100),
		Result_Value4			VARCHAR(100),
		Result_Value5			VARCHAR(100),
		Result_Value6			VARCHAR(100),
		Result_Value7			VARCHAR(100),
		Result_Value8			VARCHAR(100),
		Result_Value9			VARCHAR(100),
		Result_Value10			VARCHAR(100),
		Result_Value11			VARCHAR(100),
		Result_Value12			VARCHAR(100),
		Result_Value13			VARCHAR(100),
		Result_Value14			VARCHAR(100),
		Result_Value15			VARCHAR(100),
		Result_Value16			VARCHAR(100)
	)          
    
          
	-- ---------------------------------------------          
	-- Transactions          
	-- ---------------------------------------------                

	INSERT INTO @Transaction (
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
		HKIC_Symbol,
		PlaceVaccination          
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
		VT.HKIC_Symbol,
		TAF.AdditionalFieldValueCode
	FROM          
		VoucherTransaction VT 
		INNER JOIN transactiondetail td     
		ON vt.transaction_id = td.transaction_id  AND vt.scheme_code = @Scheme_Code AND vt.scheme_code = td.scheme_code     
		INNER JOIN TransactionAdditionalField TAF
		ON VT.Transaction_ID = TAF.Transaction_ID AND TAF.AdditionalFieldID = 'PlaceVaccination'
		LEFT JOIN SubsidizeGroupClaim SGC
		ON	td.Scheme_Code = SGC.Scheme_Code
			AND	td.Scheme_Seq = SGC.Scheme_Seq
			AND td.Subsidize_Code = SGC.Subsidize_Code   
	WHERE          
	VT.Scheme_Code = @Scheme_Code 
	AND VT.Transaction_Dtm <= @Cutoff_Dtm          
	AND VT.Transaction_Dtm > DATEADD(dd, -1 * @Date_Range + 1, @Report_Dtm)          
	AND VT.Record_Status NOT IN (
			SELECT Status_Value 
			FROM StatStatusFilterMapping 
			WHERE (report_id = 'ALL' OR report_id = @Report_ID)     
			AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'     
			AND ((Effective_Date IS NULL OR Effective_Date <= @cutoff_dtm) AND (Expiry_Date IS NULL OR Expiry_Date >= @cutoff_dtm))
	)       
	AND (VT.Invalidation IS NULL OR VT.Invalidation NOT IN (     
			SELECT Status_Value 
			FROM StatStatusFilterMapping 
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
		@Transaction 
	SET 
		Reimbursement_Status = 'R'
	WHERE 
		Transaction_Status = 'R'

-- ---------------------------------------------          
-- Patch the Reimbursement_Status          
-- ---------------------------------------------          
          
	UPDATE          
		@Transaction          
	SET          
		Reimbursement_Status =           
		CASE RAT.Authorised_Status          
			WHEN 'R' THEN 'G'          
			ELSE RAT.Authorised_Status          
		END          
	FROM          
		@Transaction VT          
		INNER JOIN ReimbursementAuthTran RAT          
		ON VT.Transaction_ID = RAT.Transaction_ID          
	WHERE VT.Transaction_Status = 'A'         	
                    
          
	-- ---------------------------------------------          
	-- Patch the Transaction_Status          
	-- ---------------------------------------------          
          
	UPDATE          
		@Transaction          
	SET          
		Transaction_Status = 'R'    
	WHERE          
		Reimbursement_Status = 'G'    
          
          
	-- ---------------------------------------------          
	-- Validated accounts          
	-- ---------------------------------------------          
          
	INSERT INTO @Account (          
		SP_ID, 
		Transaction_ID, 
		Transaction_Dtm, 
		Service_Receive_Dtm, 
		DOSE, 
		DOB, 
		Exact_DOB, 
		Sex, 
		Doc_Code, 
		Transaction_Status, 
		Reimbursement_Status,
		Row,
		Vaccine,
		Category_Code       
	)          
	SELECT          
		VT.SP_ID, 
		VT.Transaction_ID, 
		VT.Transaction_Dtm, 
		VT.Service_Receive_Dtm, 
		VT.DOSE,
		VP.DOB, 
		VP.Exact_DOB, 
		VP.Sex, 
		VT.Doc_Code, 
		VT.Transaction_Status, 
		VT.Reimbursement_Status,
		VT.Row,
		VT.Vaccine,
		VT.Category_Code        
	FROM          
		@Transaction VT          
		INNER JOIN PersonalInformation VP          
		ON VT.Voucher_Acc_ID = VP.Voucher_Acc_ID          
			AND VT.Doc_Code = VP.Doc_Code          
	WHERE          
		VT.Voucher_Acc_ID <> ''          
          
          
	-- ---------------------------------------------          
	-- Temporary accounts          
	-- ---------------------------------------------          
          
	INSERT INTO @Account (          
		SP_ID, 
		Transaction_ID, 
		Transaction_Dtm, 
		Service_Receive_Dtm, 
		DOSE,
		DOB, 
		Exact_DOB, 
		Sex, 
		Doc_Code, 
		Transaction_Status, 
		Reimbursement_Status,
		Row,
		Vaccine,
		Category_Code          
	)          
	SELECT          
		VT.SP_ID,
		VT.Transaction_ID,
		VT.Transaction_Dtm,
		VT.Service_Receive_Dtm,
		VT.DOSE,
		TP.DOB,
		TP.Exact_DOB,
		TP.Sex,
		VT.Doc_Code,
		VT.Transaction_Status,
		VT.Reimbursement_Status,
		VT.Row,
		VT.Vaccine,
		VT.Category_Code           
	FROM          
		@Transaction VT          
		INNER JOIN TempPersonalInformation TP          
		ON VT.Temp_Voucher_Acc_ID = TP.Voucher_Acc_ID          
	WHERE          
		VT.Voucher_Acc_ID = ''          
		AND VT.Temp_Voucher_Acc_ID <> ''          
		AND VT.Special_Acc_ID = ''          
          
          
	-- ---------------------------------------------
	-- Special accounts          
	-- ---------------------------------------------
          
	INSERT INTO @Account (          
		SP_ID,
		Transaction_ID,
		Transaction_Dtm,
		Service_Receive_Dtm,
		DOSE,
		DOB,
		Exact_DOB,
		Sex,
		Doc_Code,
		Transaction_Status,
		Reimbursement_Status,
		Row,
		Vaccine,
		Category_Code   
	)          
	SELECT          
		VT.SP_ID,
		VT.Transaction_ID,
		VT.Transaction_Dtm,
		VT.Service_Receive_Dtm,
		VT.DOSE,
		SP.DOB,
		SP.Exact_DOB,
		SP.Sex,
		VT.Doc_Code,
		VT.Transaction_Status,
		VT.Reimbursement_Status,
		VT.Row,
		VT.Vaccine,
		VT.Category_Code
	FROM          
		@Transaction VT          
		INNER JOIN SpecialPersonalInformation SP          
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
	SELECT @Display_Text_RecepientCondition = Description FROM SystemResource WHERE ObjectType='Text' AND ObjectName='RecipientCondition'
                  
	INSERT INTO @ResultTable (result_seq, result_value1)    
	VALUES (0, 'eHS(S)D0031-04: Raw Data of ENHVSSO claim transactions')    
	INSERT INTO @ResultTable (result_seq)    
	VALUES (1)    
	INSERT INTO @ResultTable (result_seq, result_value1)    
	VALUES (2, 'Reporting period: the week ending ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111))    
	INSERT INTO @ResultTable (result_seq)    
	VALUES (3)    
	INSERT INTO @ResultTable (result_seq, result_value1, result_value2, result_value3, result_value4, result_value5, 
	result_value6, result_value7, result_value8, result_value9, result_value10, result_value11, result_value12, Result_Value13, 
	Result_Value14, Result_Value15, Result_Value16)    
	VALUES (4, 'Transaction ID', 'Transaction Time', 'SPID', 'Service Date', 'Category', 'Subsidy', 'Dose',
	 'DOB', 'DOB Flag', 'Gender', 'Doc Type', 'HKIC Symbol', 'Place of Vaccination', 'Transaction Status', 'Reimbursement Status', 'Means of Input')
    
	-- ---------------------------------------------          
	-- Build data          
	-- ---------------------------------------------          
      
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, 
	Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, 
	Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16)
          
	SELECT          
		A.Row,
		dbo.func_format_system_number(A.Transaction_ID),
		CONVERT(varchar, T.Transaction_Dtm, 20),
		T.SP_ID,
		FORMAT(T.Service_Receive_Dtm, 'yyyy/MM/dd'),
		CC.Category_Name AS [Category],
		T.Vaccine as 'Subsidy',
		CASE A.Dose
			WHEN 'ONLYDOSE' THEN 'Only Dose'
			ELSE SID.Available_Item_Desc
		END AS [Dose],
		FORMAT(A.DOB, 'yyyy/MM/dd'),
		A.Exact_DOB,
		A.Sex,
		A.Doc_Code,
		CASE WHEN ISNULL(SD4.Status_Description, '') = '' THEN  @Str_NA ELSE SD4.Status_Description END HKIC_Symbol,
		SD3.Data_Value,
		SD1.Status_Description,
		ISNULL(SD2.Status_Description, ''),
		CASE when T.Create_By_SmartID = 'Y' THEN 'Card Reader' ELSE 'Manual' END Create_By_SmartID
	FROM          
	@Account A
	INNER JOIN @Transaction T
		ON A.Transaction_id = T.Transaction_ID
		AND A.Vaccine = T.Vaccine
	INNER JOIN SubsidizeItemDetails SID
		ON A.Dose = SID.Available_Item_Code
		AND SID.Subsidize_Item_Code = T.Subsidize_Item_Code
	INNER JOIN StatusData SD1
		ON A.Transaction_Status = SD1.Status_Value
		AND SD1.Enum_Class = 'ClaimTransStatus'
	INNER JOIN StaticData SD3
		ON T.PlaceVaccination = SD3.Item_No
		AND SD3.Column_Name = 'ENHVSSO_PLACEOFVACCINATION'
	INNER JOIN ClaimCategory CC
		ON A.Category_Code = CC.Category_Code
	LEFT JOIN StatusData SD2
		ON A.Reimbursement_Status = SD2.Status_Value
			AND SD2.Enum_Class = 'ReimbursementStatus'
	LEFT JOIN StatusData SD4
		ON T.HKIC_Symbol = SD4.Status_Value
			AND SD4.Enum_Class = 'HKICSymbol'

	-- =============================================          
	-- Return result          
	-- =============================================          
	DELETE FROM [RpteHSD0031_04_ENHVSSO_Tx_Raw]
	INSERT INTO [RpteHSD0031_04_ENHVSSO_Tx_Raw] (
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
		Col16
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
		Result_Value16 
	FROM          
		@ResultTable          
	ORDER BY 
		Result_Seq        
          
	CLOSE SYMMETRIC KEY sym_Key    
    
END    
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0031_04_PrepareData] TO HCVU
GO
