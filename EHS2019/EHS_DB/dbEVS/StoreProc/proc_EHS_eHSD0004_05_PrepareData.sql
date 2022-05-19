IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0004_05_PrepareData]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0004_05_PrepareData]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR. No			
-- Description:		
-- ============================================= 
-- =============================================      
-- Author:   		Koala CHENG
-- Create date:		21 April 2022
-- CR No.:			CRE21-022
-- Description:		Initial 
--					1. Enquire void COVID-19 vaccination record in pass 7 days   
--					2. Reference to existing SProc [proc_EHS_eHSD0004_04_PrepareData]
-- =============================================    
CREATE PROCEDURE [dbo].[proc_EHS_eHSD0004_05_PrepareData]           
 @Cutoff_Dtm datetime        
AS BEGIN          
           
-- =============================================          
-- Constant          
-- =============================================          
 DECLARE @Date_Range INT
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
  Vaccine      varchar(25),    
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
  Voided_By		varchar(50),
  Voided_Dtm		datetime,
  Voided_Remark	nvarchar(255),
  Removed_By	varchar(50),
  Removed_Dtm		datetime,
  Invalidation_Status	 char(1),
  Invalidation_By	 varchar(50),
  Invalidation_Time	 datetime,
  Invalidation_Reason	 nvarchar(255),
  Row       int          
 )          
          
 DECLARE @Account table (          
  SP_ID      char(8),          
  Transaction_ID    char(20),          
  Transaction_Dtm    datetime,          
  Service_Receive_Dtm   datetime,          
  Category_Code    varchar(50),          
  Vaccine varchar(25),
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
  Result_Value17   varchar(100) default '',
  Result_Value18   varchar(100) default '',
  Result_Value19   nvarchar(255) default '',
  Result_Value20   varchar(100) default '',
  Result_Value21   varchar(100) default '',
  Result_Value22   nvarchar(255) default '',
  Result_Value23   varchar(100) default '',
  Result_Value24   varchar(100) default '',
  Result_Value25   nvarchar(255) default ''
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
  Voided_By,
  Voided_Dtm,
  Voided_Remark,
  Removed_By,
  Removed_Dtm,
  Invalidation_Status,
  Invalidation_By,
  Invalidation_Time,
  Invalidation_Reason,
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
  VT.Void_By,
  VT.Void_Dtm,
  VT.Void_Remark,
  MR.Reject_By,
  MR.Reject_Dtm,
  VT.Invalidation AS [Invalidation_Status],       
  TI.Create_By AS [Invalidation_By],
  TI.Create_Dtm AS [Invalidation_Time],
  TI.Invalidation_Type AS [Invalidation_Reason],
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
	LEFT JOIN TransactionInvalidation TI WITH (NOLOCK)
	ON VT.Transaction_ID = TI.Transaction_ID 
	LEFT JOIN ManualReimbursement MR WITH (NOLOCK)
	ON VT.Transaction_ID = MR.Transaction_ID 
 WHERE          
  VT.Scheme_Code = 'RVP'          
AND 
(
((VT.Record_Status = 'I') AND (VT.Void_Dtm <= @Cutoff_Dtm AND VT.Void_Dtm > DATEADD(day, -1 * @Date_Range + 1, @Report_Dtm)))
 OR 
((VT.Record_Status = 'R' AND VT.Invalidation = 'I') AND (TI.Create_Dtm <= @Cutoff_Dtm AND TI.Create_Dtm > DATEADD(day, -1 * @Date_Range + 1, @Report_Dtm)))
OR
((VT.Record_Status = 'D') AND (
(MR.Reject_Dtm IS NULL AND VT.Update_Dtm <= @Cutoff_Dtm AND VT.Update_Dtm > DATEADD(day, -1 * @Date_Range + 1, @Report_Dtm))) OR 
(MR.Reject_Dtm IS NOT NULL AND MR.Reject_Dtm <= @Cutoff_Dtm AND MR.Reject_Dtm > DATEADD(day, -1 * @Date_Range + 1, @Report_Dtm)))
)
--AND TD.subsidize_item_code = 'C19'
ORDER BY VT.Transaction_Dtm 

-- Delete transaction which are not involved C19 subsidize
--DELETE a FROM @Transaction a WHERE NOT EXISTS (SELECT Transaction_ID FROM @Transaction b
--											WHERE b.Subsidize_Item_Code = 'C19'
--												AND a.Transaction_ID = b.Transaction_ID)

update @Transaction set Category_Code = case when taf2.AdditionalFieldValueCode = 'RESIDENT' then 'RESIDENT' else 'HCW' end
from @Transaction t left outer join TransactionAdditionalField TAF2 on t.Transaction_ID = taf2.Transaction_ID and  taf2.AdditionalFieldID = 'RecipientType'
where t.subsidize_item_code = 'C19' 

            
          
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
VALUES (0, 'eHS(S)D0004-05: Raw Data of RVP transactions (Voided)')    
INSERT INTO @ResultTable (Result_Seq)    
VALUES (1)    
INSERT INTO @ResultTable (Result_Seq, Result_Value1)    
VALUES (2, 'Reporting period: the week ending ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111))    
INSERT INTO @ResultTable (Result_Seq)    
VALUES (3)    
INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6,
 Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12,
 Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20, Result_Value21, Result_Value22, Result_Value23, Result_Value24, Result_Value25)     
VALUES (4, 'Transaction ID', 'Transaction Time', 'SPID', 'Service Date', 'Category', 'Subsidy', 'Dose', 'DOB', 'DOB Flag', 'Gender', 'Doc Type', 'Transaction Status', 'Reimbursement Status', 'RCH Code', 'RCH Type', 'Means of Input', 'Voided By', 'Void Time', 'Void Reason', 'Rejected By', 'Reject Time','Invalidation Status','Invalidated By','Invalidation Time','Invalidation Reason')          
         
-- ---------------------------------------------          
-- Build data          
-- ---------------------------------------------          
          
 INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6,
 Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12,
 Result_Value13, Result_Value14, Result_Value15, Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20, Result_Value21, Result_Value22, Result_Value23, Result_Value24, Result_Value25)          
        
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
  case when T.Create_By_SmartID = 'Y' THEN 'Card Reader' ELSE 'Manual' END Create_By_SmartID,
  ISNULL(T.Voided_By,''),
  ISNULL(CONVERT(varchar, T.Voided_Dtm, 20),''),
  ISNULL(T.Voided_Remark,''),
  ISNULL(T.Removed_By,''),
  ISNULL(CONVERT(varchar, T.Removed_Dtm, 20),''),
  case when T.Invalidation_Status = 'I' THEN (SELECT Status_Description FROM StatusData WITH (NOLOCK) WHERE Enum_Class = 'TransactionInvalidationStatus' AND Status_Value = 'I') ELSE '' END Invalidation_Status,
  ISNULL(T.Invalidation_By,''),
  ISNULL(CONVERT(varchar, T.Invalidation_Time, 20),''),
  ISNULL(SD3.Data_Value,'') AS [Invalidation Reason]
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
   LEFT JOIN StaticData SD3 WITH (NOLOCK)          
    ON T.Invalidation_Reason = SD3.Item_No         
     AND SD3.Column_Name = 'TransactionInvalidationType'  
              
-- =============================================          
-- Return result          
-- =============================================          
           
 DELETE FROM RpteHSD0004_05_RVP_Tx_Void          
           
 INSERT INTO RpteHSD0004_05_RVP_Tx_Void (          
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
  Result_Value16,
  Result_Value17,
  Result_Value18,
  Result_Value19,    
  Result_Value20,
  Result_Value21,
  Result_Value22,
  Result_Value23,
  Result_Value24,
  Result_Value25
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
  Result_Value20,
  Result_Value21,
  Result_Value22,
  Result_Value23,
  Result_Value24,
  Result_Value25
 FROM          
  @ResultTable
 ORDER BY 
  Result_Seq
          
          
END                      
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0004_05_PrepareData] TO HCVU
GO
