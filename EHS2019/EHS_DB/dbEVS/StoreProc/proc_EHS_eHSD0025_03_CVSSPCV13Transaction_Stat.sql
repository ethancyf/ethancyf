IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0025_03_CVSSPCV13Transaction_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0025_03_CVSSPCV13Transaction_Stat]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
  
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	11 Sep 2017
-- CR No.:			CRE17-003 (Stop vaccine daily stat)
-- Description:		Stored procedure is no longer used
-- =============================================     
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	2 November 2015
-- CR No.:			CRE15-006
-- Description:		Rename of eHS
-- =============================================
-- =============================================    
-- CR No.: CRE13-017-05
-- Author:  Karl LAM
-- Create date: 03 DEC 2013  
-- Description:  Create a new workbook for CVSSPCV13 Stat   
-- =============================================    
    
--exec proc_EHS_eHSD0025_03_CVSSPCV13Transaction_Stat '2013-12-05'; select * from [RpteHSD0025CVSSPCV13TransactionStat]
/*    
CREATE Procedure [proc_EHS_eHSD0025_03_CVSSPCV13Transaction_Stat]    
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
DECLARE @Scheme_Code as varchar(10)
DECLARE @Report_ID as varchar(30)          
-- =============================================    
-- Validation     
-- =============================================    
-- =============================================    
-- Initialization    
-- =============================================  
SET @Scheme_Code = 'CVSSPCV13'
SET @Report_ID = 'eHSD0025'

  
OPEN SYMMETRIC KEY sym_Key     
 DECRYPTION BY ASYMMETRIC KEY asym_Key    
SET @Report_Dtm = DATEADD(dd, -1, @Cutoff_Dtm)              
SET @system_Dtm = getdate()        
SET @Date_Range = 7    
-- =============================================    
-- Return results    
-- =============================================    
-- =============================================          
-- Temporary tables          
-- =============================================          
 DECLARE @Transaction table (          
  SP_ID      char(8),          
  Transaction_ID    char(20),          
  Transaction_Dtm    datetime,          
  Service_Receive_Dtm   datetime,   
  Practice_Display_Seq smallint,
  Practice_Name nvarchar(100),
  Practice_Name_Chi  nvarchar(100),         
  DOSE      char(20),          
  Scheme_seq      int,
  Per_Unit_Value	int,    
  Voucher_Acc_ID    char(15),          
  Temp_Voucher_Acc_ID   char(15),          
  Special_Acc_ID    char(15),          
  Invalid_Acc_ID    char(15),          
  Doc_Code     char(20),          
  Transaction_Status   char(1),          
  Reimbursement_Status  char(1),        
  Create_By_SmartID  char(1),       
  Row       int,
  Vaccine      char(20)            
 )          
          
 DECLARE @Account table (          
  SP_ID      char(8),          
  Transaction_ID    char(20),          
  Transaction_Dtm    datetime,          
  Service_Receive_Dtm   datetime,          
  DOSE   char(20),        
  DOB       datetime,          
  Exact_DOB     char(1),          
  Sex       char(1),          
  Doc_Code     char(20),          
  Transaction_Status   char(1),          
  Reimbursement_Status  char(1),            
  Row       int,
  Vaccine   char(20)            
 )          
           
 DECLARE @ResultTable table (          
  Result_Seq     int,          
  Result_Value1    varchar(100),          
  Result_Value2    varchar(100),          
  Result_Value3    varchar(100),          
  Result_Value4    varchar(100),          
  Result_Value5    nvarchar(100),           --Practice Name (In English)
  Result_Value6    nvarchar(100),           --Practice Name (In Chinese)
  Result_Value7    varchar(100),          
  Result_Value8    varchar(100),          
  Result_Value9    varchar(100),          
  Result_Value10    varchar(100),          
  Result_Value11    varchar(100),          
  Result_Value12    varchar(100),          
  Result_Value13    varchar(100),          
  Result_Value14    varchar(100),
  Result_Value15    varchar(100), 
  Result_Value16    varchar(100)           
 )          
    
          
-- ---------------------------------------------          
-- SIV transactions          
-- ---------------------------------------------                
-- Select transaction which scheme_code = 'CVSSPCV13'
 INSERT INTO @Transaction (          
  SP_ID,          
  Transaction_ID,          
  Transaction_Dtm,          
  Service_Receive_Dtm,  
  Practice_Display_Seq,
  Practice_Name,
  Practice_Name_Chi,          
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
  Vaccine            
 )          
 SELECT          
  VT.SP_ID,          
  VT.Transaction_ID,          
  VT.Transaction_Dtm,          
  VT.Service_Receive_Dtm,   
  VT.Practice_Display_Seq,  
  p.Practice_Name,
  p.Practice_Name_Chi,            
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
  SGC.Display_Code_For_Claim AS [Vaccine] 
 FROM          
  VoucherTransaction VT INNER JOIN transactiondetail td     
 ON vt.transaction_id = td.transaction_id  
	AND vt.scheme_code = @Scheme_Code 
	AND vt.scheme_code = td.scheme_code
 	INNER JOIN Practice p 
 ON vt.SP_ID = p.SP_ID 
	and vt.Practice_Display_Seq = p.Display_Seq 
	LEFT JOIN SubsidizeGroupClaim SGC
	on	td.Scheme_Code = SGC.Scheme_Code 
	and	td.Scheme_Seq = SGC.Scheme_Seq 
	and td.Subsidize_Code = SGC.Subsidize_Code   
 WHERE          
  VT.Scheme_Code = @Scheme_Code          
   AND VT.Transaction_Dtm <= @Cutoff_Dtm          
   AND VT.Transaction_Dtm > DATEADD(dd, -1 * @Date_Range + 1, @Report_Dtm)          
   AND VT.Record_Status NOT IN    
(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)     
AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'     
AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm)))       
AND (VT.Invalidation IS NULL OR VT.Invalidation NOT In     
(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)     
AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'    
AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm))))    
ORDER BY   VT.Transaction_Dtm
    
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
  Vaccine          
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
  VT.Vaccine          
 FROM          
  @Transaction VT          
   INNER JOIN PersonalInformation VP          
    ON VT.Voucher_Acc_ID = VP.Voucher_Acc_ID   COLLATE DATABASE_DEFAULT       
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
  Vaccine          
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
  VT.Vaccine          
 FROM          
  @Transaction VT          
   INNER JOIN TempPersonalInformation TP          
    ON VT.Temp_Voucher_Acc_ID = TP.Voucher_Acc_ID    COLLATE DATABASE_DEFAULT      
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
  Vaccine          
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
  VT.Vaccine          
 FROM          
  @Transaction VT          
   INNER JOIN SpecialPersonalInformation SP          
    ON VT.Special_Acc_ID = SP.Special_Acc_ID       COLLATE DATABASE_DEFAULT   
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
INSERT INTO @ResultTable (result_seq, result_value1)    
VALUES (0, 'eHS(S)D0025-03: Raw Data of CVSSPCV13 transactions')    
INSERT INTO @ResultTable (result_seq)    
VALUES (1)    
INSERT INTO @ResultTable (result_seq, result_value1)    
VALUES (2, 'Reporting period: the week ending ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111))    
INSERT INTO @ResultTable (result_seq)    
VALUES (3)    
INSERT INTO @ResultTable (result_seq, result_value1, result_value2, result_value3, result_value4, result_value5,    
result_value6, result_value7, result_value8, result_value9, result_value10, result_value11, result_value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16)    
VALUES (4, 'Transaction ID', 'Transaction Time', 'SPID', 'Practice No.','Practice Name (In English)','Practice Name (In Chinese)','Service Date', 'Subsidy', 'Dose', 'DOB', 'DOB Flag', 'Gender', 'Doc Type', 'Transaction Status', 'Reimbursement Status', 'Means of input')    
 
-- ---------------------------------------------          
-- Build data          
-- ---------------------------------------------          
          
 INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, Result_Value16)      
          
 SELECT          
  A.Row,         
  dbo.func_format_system_number(A.Transaction_ID),      
  CONVERT(varchar, T.Transaction_Dtm, 20),          
  T.SP_ID,          
  T.Practice_Display_Seq,
  T.Practice_Name,
  T.Practice_Name_Chi,
  CONVERT(varchar(10), T.Service_Receive_Dtm, 20),
  T.Vaccine as 'Subsidy',      
  CASE A.Dose          
   WHEN 'ONLYDOSE' THEN 'Only Dose'          
   ELSE SID.Available_Item_Desc          
  END AS [Dose],      
  CONVERT(varchar(10), A.DOB, 20),          
  A.Exact_DOB,          
  A.Sex,          
  A.Doc_Code,          
  SD1.Status_Description,          
  ISNULL(SD2.Status_Description, ''),    
  case when T.Create_By_SmartID = 'Y' THEN 'Card Reader' ELSE 'Manual' END Create_By_SmartID            
 FROM          
  @Account A INNER JOIN @Transaction T ON A.Transaction_id  COLLATE DATABASE_DEFAULT  = T.Transaction_ID   COLLATE DATABASE_DEFAULT 
   INNER JOIN SubsidizeItemDetails SID          
    ON A.Dose  COLLATE DATABASE_DEFAULT  = SID.Available_Item_Code    COLLATE DATABASE_DEFAULT      
     AND SID.Subsidize_Item_Code = 'PV13'   
   INNER JOIN StatusData SD1          
    ON A.Transaction_Status  COLLATE DATABASE_DEFAULT  = SD1.Status_Value COLLATE DATABASE_DEFAULT     
     AND SD1.Enum_Class = 'ClaimTransStatus'    
   LEFT JOIN StatusData SD2          
    ON A.Reimbursement_Status  COLLATE DATABASE_DEFAULT  = SD2.Status_Value    COLLATE DATABASE_DEFAULT      
     AND SD2.Enum_Class = 'ReimbursementStatus'    
ORDER BY  T.Transaction_Dtm          
-- =============================================          
-- Return result          
-- =============================================          
DELETE FROM [RpteHSD0025CVSSPCV13TransactionStat]    
INSERT INTO [RpteHSD0025CVSSPCV13TransactionStat] (          
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
          
CLOSE SYMMETRIC KEY sym_Key    
    
END    
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0025_03_CVSSPCV13Transaction_Stat] TO HCVU
GO
*/
