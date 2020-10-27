IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0004_04_PrepareData]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0004_04_PrepareData]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

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
-- Author:   		Marco CHOI
-- Create date:		3 August 2017
-- CR No.:			CRE16-026
-- Description:		Add PCV13   
--					SP rename from proc_EHS_RVPTransaction_Stat  
-- =============================================    
CREATE PROCEDURE [dbo].[proc_EHS_eHSD0004_04_PrepareData]           
 @Cutoff_Dtm datetime        
AS BEGIN          
           
-- =============================================          
-- Constant          
-- =============================================          
 DECLARE @Date_Range tinyint
 SET @Date_Range = 7   
           
-- =============================================          
-- Variables          
-- =============================================          
 DECLARE @Report_Dtm datetime
 SET @Report_Dtm = DATEADD(dd, -1, @Cutoff_Dtm)
           
           
-- =============================================          
-- Temporary tables          
-- =============================================          
 DECLARE @Transaction table (          
  SP_ID      char(8),          
  Transaction_ID    char(20),          
  Transaction_Dtm    datetime,          
  Service_Receive_Dtm   datetime,          
  Category_Code    varchar(50),          
  Vaccine      varchar(20),    
  Scheme_Seq int,
  Per_Unit_Value      int,
  Subsidize_Item_Code	char(10),   
  Dose      char(20),          
  Voucher_Acc_ID    char(15),          
  Temp_Voucher_Acc_ID   char(15),          
  Special_Acc_ID    char(15),          
  Invalid_Acc_ID    char(15),          
  Doc_Code     char(20),          
  Transaction_Status   char(1),          
  Reimbursement_Status  char(1),          
  RCH_Code     varchar(50),          
  RCH_Type     varchar(5),          
  Create_by_smartID char(1),    
  Row       int          
 )          
          
 DECLARE @Account table (          
  SP_ID      char(8),          
  Transaction_ID    char(20),          
  Transaction_Dtm    datetime,          
  Service_Receive_Dtm   datetime,          
  Category_Code    varchar(50),          
  Vaccine varchar(20),
  Scheme_Seq int,
  Dose      char(20),          
  DOB       datetime,          
  Exact_DOB     char(1),          
  Sex       char(1),          
  Doc_Code     char(20),          
  Transaction_Status   char(1),          
  Reimbursement_Status  char(1),          
  RCH_Code     varchar(50),          
  RCH_Type     varchar(5),          
  Row       int          
 )          
           
 DECLARE @ResultTable table (          
  Result_Seq     int,          
  Result_Value1    varchar(100) default '',    
  Result_Value2    varchar(100) default '',    
  Result_Value3    varchar(100) default '',               
  Result_Value4    varchar(100) default '',                
  Result_Value5    varchar(200) default '',                
  Result_Value6    varchar(100) default '',                
  Result_Value7    varchar(100) default '',                
  Result_Value8    varchar(100) default '',                
  Result_Value9    varchar(100) default '',                
  Result_Value10   varchar(100) default '',                
  Result_Value11   varchar(100) default '',                
  Result_Value12   varchar(100) default '',                
  Result_Value13   varchar(100) default '',                
  Result_Value14   varchar(100) default '',          
  Result_Value15   varchar(100) default '',    
  Result_Value16   varchar(100) default '',
  Result_Value17   varchar(100) default ''
 )          
           
          
-- =============================================          
-- Retrieve data          
-- =============================================          
          
-- ---------------------------------------------          
-- SIV transactions          
-- ---------------------------------------------          
          
 INSERT INTO @Transaction (          
  SP_ID,          
  Transaction_ID,          
  Transaction_Dtm,          
  Service_Receive_Dtm,          
  Category_Code, 
  Vaccine,         
  Scheme_Seq,
  Per_Unit_Value,
  Subsidize_Item_Code,
  Dose,          
  Voucher_Acc_ID,          
  Temp_Voucher_Acc_ID,          
  Special_Acc_ID,          
  Invalid_Acc_ID,          
  Doc_Code,          
  Transaction_Status,          
  Reimbursement_Status,          
  RCH_Code,          
  RCH_Type,         
  Create_by_smartID,     
  Row          
 )          
 SELECT      
  VT.SP_ID,          
  VT.Transaction_ID,          
  VT.Transaction_Dtm,          
  VT.Service_Receive_Dtm,          
  VT.Category_Code,
  SGC.Display_Code_For_Claim [Vaccine],
  td.Scheme_Seq,
  td.Per_Unit_Value,
  TD.Subsidize_Item_Code,          
  TD.Available_Item_Code AS [Dose],          
  ISNULL(VT.Voucher_Acc_ID, ''),          
  ISNULL(VT.Temp_Voucher_Acc_ID, ''),          
  ISNULL(VT.Special_Acc_ID, ''),          
  ISNULL(VT.Invalid_Acc_ID, ''),          
  VT.Doc_Code,          
  VT.Record_Status AS [Transaction_Status],          
  NULL AS [Reimbursement_Status],          
  TAF2.AdditionalFieldValueCode AS [RCH_Code],          
  HL.Type AS [RCH_Type],           
  VT.Create_by_smartID,    
  10 + ROW_NUMBER() OVER (ORDER BY VT.Transaction_Dtm)          
 FROM          
  VoucherTransaction VT WITH (NOLOCK)          
   INNER JOIN TransactionDetail TD WITH (NOLOCK)          
    ON VT.Transaction_ID = TD.Transaction_ID                     
   --INNER JOIN TransactionAdditionalField TAF1          
   -- ON VT.Transaction_ID = TAF1.Transaction_ID          
   --  AND TAF1.AdditionalFieldID = 'CategoryCode'          
   INNER JOIN TransactionAdditionalField TAF2 WITH (NOLOCK)          
    ON VT.Transaction_ID = TAF2.Transaction_ID          
     AND TAF2.AdditionalFieldID = 'RHCCode'          
   INNER JOIN RVPHomeList HL WITH (NOLOCK)          
    ON TAF2.AdditionalFieldValueCode = HL.RCH_Code
	LEFT JOIN SubsidizeGroupClaim SGC
	on	td.Scheme_Code = SGC.Scheme_Code
	and	td.Scheme_Seq = SGC.Scheme_Seq
	and td.Subsidize_Code = SGC.Subsidize_Code              
 WHERE          
  VT.Scheme_Code = 'RVP'          
AND VT.Transaction_Dtm <= @Cutoff_Dtm          
AND VT.Transaction_Dtm > DATEADD(dd, -1 * @Date_Range + 1, @Report_Dtm)             
AND VT.Record_Status NOT IN    
 (SELECT Status_Value FROM StatStatusFilterMapping WITH (NOLOCK) WHERE (report_id = 'ALL' OR report_id = 'eHSD0004')     
 AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'     
 AND ((Effective_Date is null or Effective_Date>= @Cutoff_Dtm) AND (Expiry_Date is null or Expiry_Date < @Cutoff_Dtm)))       
AND (VT.Invalidation IS NULL OR VT.Invalidation NOT In     
 (SELECT Status_Value FROM StatStatusFilterMapping WITH (NOLOCK) WHERE (report_id = 'ALL' OR report_id = 'eHSD0004')     
 AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'    
 AND ((Effective_Date is null or Effective_Date>= @Cutoff_Dtm) AND (Expiry_Date is null or Expiry_Date < @Cutoff_Dtm))))
ORDER BY VT.Transaction_Dtm

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
-- In this patching, payment outside claim would not handle
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
  Category_Code,  
  Vaccine,        
  Dose,          
  DOB,          
  Exact_DOB,          
  Sex,          
  Doc_Code,          
  Transaction_Status,          
  Reimbursement_Status,          
  RCH_Code,          
  RCH_Type,          
  Row          
 )          
 SELECT          
  VT.SP_ID,          
  VT.Transaction_ID,          
  VT.Transaction_Dtm,          
  VT.Service_Receive_Dtm,          
  VT.Category_Code,          
  VT.Vaccine,          
  VT.Dose,          
  VP.DOB,          
  VP.Exact_DOB,          
  VP.Sex,          
  VT.Doc_Code,          
  VT.Transaction_Status,          
  VT.Reimbursement_Status,          
  VT.RCH_Code,          
  VT.RCH_Type,          
  VT.Row          
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
  Category_Code,          
  Vaccine,        
  Dose,          
  DOB,          
  Exact_DOB,          
  Sex,          
  Doc_Code,          
  Transaction_Status,          
  Reimbursement_Status,          
  RCH_Code,          
  RCH_Type,          
  Row          
 )          
 SELECT          
  VT.SP_ID,          
  VT.Transaction_ID,          
  VT.Transaction_Dtm,          
  VT.Service_Receive_Dtm,          
  VT.Category_Code,          
  VT.Vaccine,
  VT.Dose,          
  TP.DOB,          
  TP.Exact_DOB,          
  TP.Sex,          
  VT.Doc_Code,          
  VT.Transaction_Status,          
  VT.Reimbursement_Status,          
  VT.RCH_Code,          
  VT.RCH_Type,          
  VT.Row          
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
  Category_Code, 
  Vaccine,         
  Dose,          
  DOB,          
  Exact_DOB,          
  Sex,          
  Doc_Code,          
  Transaction_Status,          
  Reimbursement_Status,          
  RCH_Code,          
  RCH_Type,          
  Row          
 )          
 SELECT          
  VT.SP_ID,          
  VT.Transaction_ID,          
  VT.Transaction_Dtm,          
  VT.Service_Receive_Dtm,          
  VT.Category_Code,          
  VT.Vaccine,
  VT.Dose,          
  SP.DOB,          
  SP.Exact_DOB,          
  SP.Sex,          
  VT.Doc_Code,          
  VT.Transaction_Status,          
  VT.Reimbursement_Status,          
  VT.RCH_Code,          
  VT.RCH_Type,          
  VT.Row          
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
           
INSERT INTO @ResultTable (Result_Seq, Result_Value1)    
VALUES (0, 'eHS(S)D0004-04: Raw Data of RVP transactions')    
INSERT INTO @ResultTable (Result_Seq)    
VALUES (1)    
INSERT INTO @ResultTable (Result_Seq, Result_Value1)    
VALUES (2, 'Reporting period: the week ending ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111))    
INSERT INTO @ResultTable (Result_Seq)    
VALUES (3)    
INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6,
 Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12,
 Result_Value13, Result_Value14, Result_Value15, Result_Value16)     
VALUES (4, 'Transaction ID', 'Transaction Time', 'SPID', 'Service Date', 'Category', 'Subsidy', 'Dose', 'DOB', 'DOB Flag', 'Gender', 'Doc Type', 'Transaction Status', 'Reimbursement Status', 'RCH Code', 'RCH Type', 'Means of Input')          
          
          
-- ---------------------------------------------          
-- Build data          
-- ---------------------------------------------          
          
 INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6,
 Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12,
 Result_Value13, Result_Value14, Result_Value15, Result_Value16)          
        
 SELECT          
  A.Row,          
  dbo.func_format_system_number(A.Transaction_ID),    
  --LEFT(A.Transaction_ID, 7) + '-' + CONVERT(varchar, CONVERT(int, SUBSTRING(A.Transaction_ID, 8, 8))) + '-' + SUBSTRING(A.Transaction_ID, 16, 1) AS [Transaction ID],          
  CONVERT(varchar, A.Transaction_Dtm, 20),          
  A.SP_ID,          
  FORMAT(A.Service_Receive_Dtm, 'yyyy/MM/dd'),          
  CC.Category_Name AS [Category_Code],          
  A.Vaccine AS [Subsidy],
  CASE A.Dose          
   WHEN 'ONLYDOSE' THEN 'Only Dose'          
   ELSE SID.Available_Item_Desc          
  END AS [Dose],   
  FORMAT(A.DOB, 'yyyy/MM/dd'),              
  A.Exact_DOB,          
  A.Sex,          
  A.Doc_Code,          
  SD1.Status_Description,          
  ISNULL(SD2.Status_Description, ''),          
  A.RCH_Code,           
  A.RCH_Type,    
  case when T.Create_By_SmartID = 'Y' THEN 'Card Reader' ELSE 'Manual' END Create_By_SmartID        
 FROM          
  @Account A INNER JOIN @Transaction T ON A.Transaction_id = T.Transaction_ID AND A.Vaccine = T.Vaccine        
   INNER JOIN ClaimCategory CC WITH (NOLOCK)          
    ON A.Category_Code = CC.Category_Code          
   INNER JOIN SubsidizeItemDetails SID WITH (NOLOCK)          
    ON A.Dose = SID.Available_Item_Code          
--     AND SID.Subsidize_Item_Code = 'SIV'        
     AND SID.Subsidize_Item_Code = T.Subsidize_Item_Code        
   INNER JOIN StatusData SD1 WITH (NOLOCK)          
    ON A.Transaction_Status = SD1.Status_Value          
     AND SD1.Enum_Class = 'ClaimTransStatus'          
   LEFT JOIN StatusData SD2 WITH (NOLOCK)          
    ON A.Reimbursement_Status = SD2.Status_Value          
     AND SD2.Enum_Class = 'ReimbursementStatus'          
              
-- =============================================          
-- Return result          
-- =============================================          
          
 DELETE FROM RpteHSD0004_04_RVP_Tx_Raw          
           
 INSERT INTO RpteHSD0004_04_RVP_Tx_Raw (          
  Display_Seq,          
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
          
          
END                      
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0004_04_PrepareData] TO HCVU
GO
