IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_RVPSIVTransaction_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_RVPSIVTransaction_Stat]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	11 April 2011
-- Description:		Stored procedure is no longer used
-- =============================================
-- =============================================          
-- Modification History        
-- Modified By:  Eric Tse
-- Modified date: 26 October 2010          
-- Description:  (1) Fit with new report layout standard
--				 (2) RSIV will show current season transaction only
--				 (3) Filter invalidated transaction
--				 (4) Handle payment outside eHS status
-- =============================================   
-- =============================================          
-- Modification History        
-- Modified By:  Derek LEUNG          
-- Modified date: 15 September 2010          
-- Description:  Include report id in 1st line - update to meet new report standard      
-- =============================================          
-- =============================================      
-- Author:  Derek LEUNG      
-- Create date: 16 August 2010      
-- Description:  Generate report for RVP-SIV Transactions        
-- =============================================

/*
CREATE PROCEDURE [dbo].[proc_EHS_RVPSIVTransaction_Stat]         
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
  Result_Value5    varchar(100) default '',              
  Result_Value6    varchar(100) default '',              
  Result_Value7    varchar(100) default '',              
  Result_Value8    varchar(100) default '',              
  Result_Value9    varchar(100) default '',              
  Result_Value10   varchar(100) default '',              
  Result_Value11   varchar(100) default '',              
  Result_Value12   varchar(100) default '',              
  Result_Value13   varchar(100) default '',              
  Result_Value14   varchar(100) default '',        
  Result_Value15   varchar(100) default ''  
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
  TAF1.AdditionalFieldValueCode AS [Category_Code],        
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
  VoucherTransaction VT        
   INNER JOIN TransactionDetail TD        
    ON VT.Transaction_ID = TD.Transaction_ID        
     --AND TD.Subsidize_Item_Code = 'SIV'      
   INNER JOIN TransactionAdditionalField TAF1        
    ON VT.Transaction_ID = TAF1.Transaction_ID        
     AND TAF1.AdditionalFieldID = 'CategoryCode'        
   INNER JOIN TransactionAdditionalField TAF2        
    ON VT.Transaction_ID = TAF2.Transaction_ID        
     AND TAF2.AdditionalFieldID = 'RHCCode'        
   INNER JOIN RVPHomeList HL        
    ON TAF2.AdditionalFieldValueCode = HL.RCH_Code        
 WHERE        
  VT.Scheme_Code = 'RVP'        
AND VT.Transaction_Dtm <= @Cutoff_Dtm        
AND VT.Transaction_Dtm > DATEADD(dd, -1 * @Date_Range + 1, @Report_Dtm)           
AND VT.Record_Status NOT IN  
 (SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = 'eHSD0004')   
 AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'   
 AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm)))     
AND (VT.Invalidation IS NULL OR VT.Invalidation NOT In   
 (SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = 'eHSD0004')   
 AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'  
 AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm))))  
        
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
  Category_Code,        
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
         
INSERT INTO @ResultTable (Result_Seq, Result_Value1)  
VALUES (0, 'eHSD0004-03: Raw Data of RSIV transactions under RVP')  
INSERT INTO @ResultTable (Result_Seq)  
VALUES (1)  
INSERT INTO @ResultTable (Result_Seq, Result_Value1)  
VALUES (2, 'Reporting period: the week ending ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111))  
INSERT INTO @ResultTable (Result_Seq)  
VALUES (3)  
INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14,Result_Value15)   
VALUES (4, 'Transaction ID', 'Transaction Time', 'SPID', 'Service Date', 'Category', 'Dose', 'DOB', 'DOB Flag', 'Gender', 'Doc Type', 'Transaction Status', 'Reimbursement Status', 'RCH Code', 'RCH Type', 'Means of Input')        
        
        
-- ---------------------------------------------        
-- Build data        
-- ---------------------------------------------        
        
 INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14,Result_Value15)    
    
      
 SELECT        
  A.Row,        
  dbo.func_format_system_number(A.Transaction_ID),  
  --LEFT(A.Transaction_ID, 7) + '-' + CONVERT(varchar, CONVERT(int, SUBSTRING(A.Transaction_ID, 8, 8))) + '-' + SUBSTRING(A.Transaction_ID, 16, 1) AS [Transaction ID],        
  CONVERT(varchar, A.Transaction_Dtm, 20),        
  A.SP_ID,        
  CONVERT(varchar(10), A.Service_Receive_Dtm, 20),        
  CC.Category_Name AS [Category_Code],        
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
  A.RCH_Code,         
  A.RCH_Type,  
  case when T.Create_By_SmartID = 'Y' THEN 'Card Reader' ELSE 'Manual' END Create_By_SmartID      
 FROM        
  @Account A INNER JOIN @Transaction T ON A.Transaction_id = T.Transaction_ID       
   INNER JOIN ClaimCategory CC        
    ON A.Category_Code = CC.Category_Code        
   INNER JOIN SubsidizeItemDetails SID        
    ON A.Dose = SID.Available_Item_Code        
     AND SID.Subsidize_Item_Code = 'SIV'      
   INNER JOIN StatusData SD1        
    ON A.Transaction_Status = SD1.Status_Value        
     AND SD1.Enum_Class = 'ClaimTransStatus'        
   LEFT JOIN StatusData SD2        
    ON A.Reimbursement_Status = SD2.Status_Value        
     AND SD2.Enum_Class = 'ReimbursementStatus'        
            
-- =============================================        
-- Return result        
-- =============================================        
        
 DELETE FROM _RVPSIVTransaction_Stat        
         
 INSERT INTO _RVPSIVTransaction_Stat (        
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
  Result_Value15  
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
  Result_Value15  
 FROM        
  @ResultTable        
        
        
END                
GO

GRANT EXECUTE ON [dbo].[proc_EHS_RVPSIVTransaction_Stat] TO HCVU
GO
*/
