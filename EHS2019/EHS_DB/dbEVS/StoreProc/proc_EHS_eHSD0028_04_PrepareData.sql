IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0028_04_PrepareData]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0028_04_PrepareData]
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
-- Modified date:	26 Nov 2020
-- CR No.:			INT20-0052
-- Description:		Fix temp table column for "Display_Code_For_Claim" 
-- =============================================
  -- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	24 Sep 2020
-- CR. No			INT20-0031
-- Description:		Fix eHSD0028 truncate error of category name
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	16 Jul 2020
-- CR. No			INT20-0025
-- Description:		(1) Add WITH (NOLOCK)
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE18-020 (HKIC Symbol Others)
-- Modified by:		Winnie SUEN
-- Modified date:	25 Feb 2019
-- Description:		Show HKIC Symbol Description
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	14 May 2018
-- CR No.:			CRE17-010
-- Description:		OCSSS Integration
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Dickson Law
-- Modified date:	09 Jan 2018
-- CR No.:			CRE14-016
-- Description:		Rename D0028-05 to D0028-04 sheet
-- =============================================
-- =============================================
-- CR No.:			CRE16-002-04
-- Author:			Winnie SUEN
-- Create date:		31 Aug 2016
-- Description:		Revamp VSS
-- =============================================
    
CREATE Procedure [proc_EHS_eHSD0028_04_PrepareData]    
 @Cutoff_Dtm as DateTime    
AS    
BEGIN    
 SET NOCOUNT ON;    
-- =============================================    
-- Declaration    
-- =============================================    
DECLARE @system_Dtm as datetime    
DECLARE @Report_Dtm datetime    
DECLARE @Date_Range tinyint   
DECLARE @Scheme_Code AS VARCHAR(10)
DECLARE @Report_ID AS VARCHAR(30)     
DECLARE @Str_HighRisk varchar(4000)	
DECLARE @Str_NonHighRisk varchar(4000) 
DECLARE @Str_NA varchar(10)
      
-- =============================================    
-- Validation     
-- =============================================    
-- =============================================    
-- Initialization    
-- =============================================    
EXEC [proc_SymmetricKey_open] 

SET @Report_Dtm = DATEADD(dd, -1, @Cutoff_Dtm)              
SET @system_Dtm = getdate()        
SET @Date_Range = 7    
SET @Scheme_Code = 'VSS'  
SET @Report_ID = 'eHSD0028'  
SELECT @Str_HighRisk = Data_Value FROM StaticData WITH (NOLOCK) WHERE Column_Name='VSS_RECIPIENTCONDITION' AND Item_No='HIGHRISK'
SELECT @Str_NonHighRisk = Data_Value FROM StaticData WITH (NOLOCK) WHERE Column_Name='VSS_RECIPIENTCONDITION' AND Item_No='NOHIGHRISK' 
SELECT @Str_NA = Description FROM SystemResource WITH (NOLOCK) WHERE ObjectType = 'Text' AND ObjectName='NA'
	
-- =============================================    
-- Return results    
-- =============================================    
-- =============================================          
-- Temporary tables          
-- =============================================          
 DECLARE @Transaction table (          
  SP_ID				char(8),          
  Transaction_ID    char(20),          
  Transaction_Dtm	datetime,          
  Service_Receive_Dtm	datetime,  
  Subsidize_Item_Code varchar(10),          
  DOSE				char(20),          
  IsHighRisk		char(1),            
  Scheme_seq		int,
  Per_Unit_Value	int,    
  Voucher_Acc_ID    char(15),          
  Temp_Voucher_Acc_ID   char(15),          
  Special_Acc_ID    char(15),          
  Invalid_Acc_ID    char(15),          
  Doc_Code			char(20),          
  Transaction_Status   char(1),          
  Reimbursement_Status  char(1),        
  Create_By_SmartID	char(1),       
  Row				int,
  Vaccine			varchar(25),
  Category_Code		varchar(10),
  HKIC_Symbol		char(1)
 )        

          
 DECLARE @Account table (          
  SP_ID				char(8),          
  Transaction_ID    char(20),          
  Transaction_Dtm	datetime,          
  Service_Receive_Dtm	datetime,          
  DOSE				char(20),        
  DOB				datetime,          
  Exact_DOB			char(1),          
  Sex				char(1),          
  Doc_Code			char(20),          
  Transaction_Status	char(1),          
  Reimbursement_Status	char(1),            
  Row				int,
  Vaccine			varchar(25),
  Category_Code		varchar(10)      
 )          
           
 DECLARE @ResultTable table (          
  Result_Seq     int,          
  Result_Value1    varchar(200),          
  Result_Value2    varchar(200),          
  Result_Value3    varchar(200),          
  Result_Value4    varchar(200),          
  Result_Value5    varchar(200),          
  Result_Value6    varchar(200),          
  Result_Value7    varchar(200),          
  Result_Value8    varchar(200),          
  Result_Value9    varchar(200),          
  Result_Value10    varchar(200),          
  Result_Value11    varchar(200),          
  Result_Value12    varchar(200),          
  Result_Value13    varchar(200),          
  Result_Value14    varchar(200),          
  Result_Value15    varchar(200),
  Result_Value16	varchar(200)          
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
	IsHighRisk,
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
  HKIC_Symbol            
 )          
 SELECT          
  VT.SP_ID,          
  VT.Transaction_ID,          
  VT.Transaction_Dtm,          
  VT.Service_Receive_Dtm,    
  TD.Subsidize_Item_Code,         
  TD.Available_Item_Code AS [Dose],     
	VT.High_Risk,   
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
  VT.HKIC_Symbol
 FROM          
  VoucherTransaction VT WITH (NOLOCK) 
  INNER JOIN transactiondetail td WITH (NOLOCK)     
	ON vt.transaction_id = td.transaction_id  AND vt.scheme_code = @Scheme_Code AND vt.scheme_code = td.scheme_code     
	LEFT JOIN SubsidizeGroupClaim SGC WITH (NOLOCK)
	ON	td.Scheme_Code = SGC.Scheme_Code
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
		AND ((Effective_Date is null or Effective_Date <= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date >= @cutoff_dtm))
	)       
	AND (VT.Invalidation IS NULL OR VT.Invalidation NOT IN (     
			SELECT Status_Value 
			FROM StatStatusFilterMapping WITH (NOLOCK) 
			WHERE (report_id = 'ALL' OR report_id = @Report_ID)     
AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'    
			AND ((Effective_Date is null or Effective_Date <= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date >= @cutoff_dtm))
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
   INNER JOIN ReimbursementAuthTran RAT WITH (NOLOCK)          
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
   INNER JOIN PersonalInformation VP WITH (NOLOCK)          
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
   INNER JOIN TempPersonalInformation TP WITH (NOLOCK)          
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
VALUES (0, 'eHS(S)D0028-04: Raw Data of VSS transactions')    
INSERT INTO @ResultTable (result_seq)    
VALUES (1)    
INSERT INTO @ResultTable (result_seq, result_value1)    
VALUES (2, 'Reporting period: the week ending ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111))    
INSERT INTO @ResultTable (result_seq)    
VALUES (3)    
INSERT INTO @ResultTable (result_seq, result_value1, result_value2, result_value3, result_value4, result_value5,    
result_value6, result_value7, result_value8, result_value9, result_value10, result_value11, result_value12, Result_Value13, 
Result_Value14, Result_Value15, Result_Value16)    
VALUES (4, 'Transaction ID', 'Transaction Time', 'SPID', 'Service Date', 'Category', 'Subsidy', 'Dose', @Display_Text_RecepientCondition,
 'DOB', 'DOB Flag', 'Gender', 'Doc Type', 'HKIC Symbol', 'Transaction Status', 'Reimbursement Status', 'Means of Input')
    
-- ---------------------------------------------          
-- Build data          
-- ---------------------------------------------          
      
 INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, 
 Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, 
 Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, 
 Result_Value16)
          
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
	CASE T.IsHighRisk 
		WHEN 'Y' THEN @Str_HighRisk
		WHEN 'N' THEN @Str_NonHighRisk
		ELSE 'N/A'
		END AS IsHighRisk,
  FORMAT(A.DOB, 'yyyy/MM/dd'),              
  A.Exact_DOB,          
  A.Sex,          
  A.Doc_Code,
  CASE WHEN ISNULL(SD3.Status_Description, '') = '' THEN  @Str_NA ELSE SD3.Status_Description END,   
  SD1.Status_Description,          
  ISNULL(SD2.Status_Description, ''),   
  case when T.Create_By_SmartID = 'Y' THEN 'Card Reader' ELSE 'Manual' END Create_By_SmartID            
 FROM          
@Account A 
	INNER JOIN @Transaction T 
		ON A.Transaction_id = T.Transaction_ID 
			AND A.Vaccine = T.Vaccine    
	INNER JOIN SubsidizeItemDetails SID WITH (NOLOCK)          
		ON A.Dose = SID.Available_Item_Code          
			AND SID.Subsidize_Item_Code = T.Subsidize_Item_Code       
	INNER JOIN StatusData SD1 WITH (NOLOCK)          
		ON A.Transaction_Status = SD1.Status_Value          
			AND SD1.Enum_Class = 'ClaimTransStatus'    
	INNER JOIN ClaimCategory CC WITH (NOLOCK)          
		ON A.Category_Code = CC.Category_Code 
	LEFT JOIN StatusData SD2 WITH (NOLOCK)          
		ON A.Reimbursement_Status = SD2.Status_Value          
			AND SD2.Enum_Class = 'ReimbursementStatus'
	LEFT JOIN StatusData SD3 WITH (NOLOCK)
		ON T.HKIC_Symbol = SD3.Status_Value
			AND SD3.Enum_Class = 'HKICSymbol'               
-- =============================================          
-- Return result          
-- =============================================          
DELETE FROM [RpteHSD0028_04_VSS_Tx_Raw]    
INSERT INTO [RpteHSD0028_04_VSS_Tx_Raw] (          
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
          
EXEC [proc_SymmetricKey_close] 
    
END    
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0028_04_PrepareData] TO HCVU
GO
