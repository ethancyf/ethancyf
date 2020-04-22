IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSU0004_Stat_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSU0004_Stat_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	18 November 2015
-- CR No.:			CRE15-006
-- Description:		Rename of eHS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Winnie SUEN
-- Modified date: 21 Apr 2015
-- Description:	1. Refine District Struture
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE13-018 Change Voucher Amount to 1 Dollar
-- Modified by:		Tommy LAM
-- Modified date:	30 Jan 2014
-- Description:		For Sub-report - "eHSU0004-01":
--						1. Migrate value "1 HCV + $50" & "2 HCVs" of Column "Co-payment" to value "Use Voucher"
--						2. Add new Column "Use HCV Amount ($)"
-- =============================================
-- =============================================
-- CR No.:		CRE13-001-02
-- Author:		Karl LAM
-- Create date: 17 DEC 2013
-- Description:	Get Stat for report eHSD0004
-- ============================================= 
--exec proc_EHS_eHSU0004_Stat_get '', '2013-Apr-16 00:00',  '2013-jun-18 00:00'

CREATE PROCEDURE [dbo].[proc_EHS_eHSU0004_Stat_get]  
	@request_time datetime,
	@From_Date varchar(17),
	@To_Date varchar(17)	
AS BEGIN  

 SET NOCOUNT ON;  

-- =============================================  
-- Declaration  
-- =============================================  
 
DECLARE @wsContent varchar(30)    
DECLARE @wsCriteria varchar(30)    
DECLARE @ws01 varchar(30)    
DECLARE @wsRemark varchar(30)   
  
DECLARE @wsContent_ct int      
DECLARE @wsCriteria_ct int      
DECLARE @ws01_ct int      
DECLARE @wsRemark_ct int   
  
DECLARE @Report_ID Char(8)  
SET @Report_ID = 'eHSU0004' 

DECLARE @Scheme_Code_EHAPP char(10)  
SET @Scheme_Code_EHAPP = 'EHAPP'   
  
---- init worksheet key    
set @wsContent = 'Content'    
set @wsCriteria = 'Criteria'    
set @ws01 = '01'    
set @wsRemark = 'Remark'    
  
set @wsContent_ct = 1  
set @wsCriteria_ct = 1  
set @ws01_ct = 1  
set @wsRemark_ct = 1  
  
-- =============================================  
-- Report Setting  
-- =============================================  
  
IF @request_time IS NULL BEGIN    
  SET @request_time = DATEADD(dd, -1, CONVERT(VARCHAR(11), GETDATE(), 106)) -- "106" gives "dd MMM yyyy"    
END    
    
DECLARE @reporting_period varchar(50)      
DECLARE @reporting_date varchar(50)   
DECLARE @reporting_dtm datetime  
DECLARE @From_Dtm datetime
DECLARE @To_Dtm datetime

SELECT @From_Dtm = Cast(CONVERT(VARCHAR(11), @From_Date, 111) + ' 00:00:00' as datetime) 
SELECT @To_Dtm = Cast(CONVERT(VARCHAR(11), @To_Date, 111) + ' 00:00:00' as datetime) 

SET @reporting_date =  CONVERT(VARCHAR(10), @From_Dtm, 111) + ' to ' + CONVERT(VARCHAR(10), @To_Dtm, 111)
SET @reporting_period = 'Reporting period: ' + @reporting_date
 
---- Prepare ResultSet ----     
CREATE TABLE #WorkBook 
(    
WorkSheetID varchar(30),    
Result01 nvarchar(1000) default '',   
Result02 nvarchar(100) default '',    
Result03 nvarchar(100) default '',    
Result04 nvarchar(100) default '',    
Result05 nvarchar(100) default '',    
Result06 nvarchar(300) default '',    
Result07 nvarchar(200) default '',    
Result08 nvarchar(100) default '',    
Result09 nvarchar(100) default '',    
Result10 nvarchar(100) default '',    
Result11 nvarchar(100) default '',    
Result12 nvarchar(100) default '',    
Result13 nvarchar(100) default '',    
Result14 nvarchar(100) default '',    
Result15 nvarchar(100) default '',    
Result16 nvarchar(100) default '',    
Result17 nvarchar(100) default '',    
Result18 nvarchar(100) default '',    
Result19 nvarchar(100) default '',     
Result20 nvarchar(100) default '',  
Result21 nvarchar(100) default '',  
Result22 nvarchar(100) default '',  
Result23 nvarchar(100) default '',  
Result24 nvarchar(100) default '',  
Result25 nvarchar(100) default '',  
Result26 nvarchar(100) default '',  
Result27 nvarchar(100) default '',  
Result28 nvarchar(100) default '',  
Result29 nvarchar(100) default '',  
Result30 nvarchar(100) default '',  
Result31 nvarchar(100) default '',  
Result32 nvarchar(100) default '',  
Result33 nvarchar(100) default '',  
Result34 nvarchar(100) default '',  
Result35 nvarchar(100) default '',  
Result36 nvarchar(100) default '',  
Result37 nvarchar(100) default '',  
Result38 nvarchar(100) default '',  
Result39 nvarchar(100) default '',  
Result40 nvarchar(100) default '',  
DisplaySeq int       
)    
  
  
 -- =============================================  
-- Build frame  
-- =============================================  
  
---- Generate static layout ----      
---- Content    
  
INSERT INTO #WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, 'Sub Report ID','Sub Report Name',@wsContent_ct)   
SELECT @wsContent_ct=@wsContent_ct+1  
INSERT INTO #WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, 'eHS(S)U0004-01','Raw data of EHAPP transaction',@wsContent_ct)    
SELECT @wsContent_ct=@wsContent_ct+1  
INSERT INTO #WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, '','',@wsContent_ct)    
SELECT @wsContent_ct=@wsContent_ct+1  
INSERT INTO #WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsContent, '','',@wsContent_ct)    
SELECT @wsContent_ct=@wsContent_ct+1  
INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@wsContent, 'Report Generation Time: ' + CONVERT(VARCHAR(10), getdate(), 111) + ' ' + CONVERT(VARCHAR(5), getdate(), 114),@wsContent_ct)    
SELECT @wsContent_ct=@wsContent_ct+1  
  
--Criteria  
-- Insert workbook data
INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq)     
VALUES (@wsCriteria, 'Criteria' , @wsCriteria_ct)    
SELECT @wsCriteria_ct=@wsCriteria_ct + 1  

INSERT INTO #WorkBook (WorkSheetID, Result01, Result02, DisplaySeq)     
VALUES (@wsCriteria, 'Transaction Date' , @reporting_date, @wsCriteria_ct)    
SELECT @wsCriteria_ct=@wsCriteria_ct + 1  
  
--01 sub Report  
INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq)     
VALUES (@ws01, 'eHS(S)U0004-01: Raw data of EHAPP transaction' , @ws01_ct)    
SELECT @ws01_ct=@ws01_ct + 1  
INSERT INTO #WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws01, @ws01_ct)    
SELECT @ws01_ct=@ws01_ct + 1    
INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq) VALUES (@ws01, @reporting_period, @ws01_ct)      
SELECT @ws01_ct=@ws01_ct + 1    
INSERT INTO #WorkBook (WorkSheetID, DisplaySeq) VALUES (@ws01, @ws01_ct)  
SELECT @ws01_ct=@ws01_ct + 1    

INSERT INTO #WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, Result07, Result08, Result09, Result10,
						Result11, Result12, Result13, Result14, Result15, Result16, Result17, Result18, Result19, Result20, DisplaySeq) VALUES
(@ws01,'Transaction ID','Transaction Time','SPID','MO No.','MO Name (In English)','MO Name (In Chinese)','Practice No.','Practice Name (In English)','Practice Name (In Chinese)','Profession','District','District Board',	'Area',	'Service Date',	'eHealth (Subsidies) Account ID','Doc Type','Co-payment', 'Use HCV Amount ($)', 'Transaction Status',	'Means of input' ,@ws01_ct)
SELECT @ws01_ct=@ws01_ct + 1    



-- =============================================  
-- Prepare Data for eHSU0004-01  
-- =============================================  

INSERT INTO #WorkBook (WorkSheetID, Result01, Result02, Result03, Result04, Result05, Result06, Result07, Result08, Result09, Result10,
						Result11, Result12, Result13, Result14, Result15, Result16, Result17, Result18, Result19, Result20, DisplaySeq) 
						
SELECT  
  @ws01,
  VT.Transaction_ID,  
  [Transaction_Dtm] = CONVERT(varchar(11), VT.Transaction_Dtm, 106) + ' ' + CONVERT(varchar(8), VT.Transaction_Dtm, 114),  
  VT.SP_ID,  
  p.MO_Display_Seq,
  mo.MO_Eng_Name,
  mo.MO_Chi_Name,
  VT.Practice_Display_Seq,  
  p.Practice_Name,  
  p.Practice_Name_Chi, 
  VT.Service_Type,
  d.district_name,   
  d.district_board,
  da.area_name,
  [Service_Receive_Dtm] = CONVERT(varchar(11),VT.Service_Receive_Dtm, 106),    
  [eHealthAccountID] = case when isnull(VT.Voucher_Acc_ID,'') <> '' then dbo.func_format_voucher_account_number('V', VT.Voucher_Acc_ID)
							else dbo.func_format_voucher_account_number(tva.Record_Status, VT.Temp_Voucher_Acc_ID) end,
  dt.Doc_Display_Code,  
  [Copayment] = CoPayDesc.Data_Value,
  ISNULL(HCVAmount.AdditionalFieldValueCode, 'N/A'),
  [Record_Status] = TxStatus.Status_Description, 
  [Create_By_SmartID] = Case when VT.Create_By_SmartID = 'Y' THEN 'Card Reader' ELSE 'Manual' END,
  row_number() over (order by VT.Transaction_Dtm )  +  cast(@ws01_ct  as integer)
 FROM
  VoucherTransaction VT with(nolock)  
  INNER JOIN TransactionDetail TD with(nolock)  
    ON	VT.Transaction_ID COLLATE DATABASE_DEFAULT = TD.Transaction_ID  COLLATE DATABASE_DEFAULT
  LEFT OUTER JOIN TransactionAdditionalField CoPay with(nolock)  
    ON	VT.Transaction_ID COLLATE DATABASE_DEFAULT = CoPay.Transaction_ID COLLATE DATABASE_DEFAULT 
		AND CoPay.AdditionalFieldID COLLATE DATABASE_DEFAULT = 'CoPayment' COLLATE DATABASE_DEFAULT
  LEFT OUTER JOIN TransactionAdditionalField HCVAmount with(nolock)  
    ON	VT.Transaction_ID COLLATE DATABASE_DEFAULT = HCVAmount.Transaction_ID COLLATE DATABASE_DEFAULT 
		AND HCVAmount.AdditionalFieldID COLLATE DATABASE_DEFAULT = 'HCVAmount' COLLATE DATABASE_DEFAULT 
  INNER JOIN PracticeSchemeInfo info  with(nolock)  
    ON	VT.SP_ID COLLATE DATABASE_DEFAULT = info.SP_ID COLLATE DATABASE_DEFAULT
		and VT.Practice_Display_Seq = info.Practice_Display_Seq
  INNER JOIN SchemeEnrolClaimMap map with(nolock)  
    ON	info.Scheme_code COLLATE DATABASE_DEFAULT = map.Scheme_Code_Claim COLLATE DATABASE_DEFAULT 
		and map.Scheme_Code_Enrol = @Scheme_Code_EHAPP COLLATE DATABASE_DEFAULT
  LEFT OUTER JOIN PersonalInformation VP with(nolock)  
    ON	VT.Voucher_Acc_ID COLLATE DATABASE_DEFAULT = VP.Voucher_Acc_ID COLLATE DATABASE_DEFAULT
  LEFT OUTER JOIN TempPersonalInformation TP with(nolock)  
    ON	VT.Temp_Voucher_Acc_ID COLLATE DATABASE_DEFAULT = TP.Voucher_Acc_ID COLLATE DATABASE_DEFAULT
  INNER JOIN Practice p with(nolock)  
    ON	VT.SP_ID COLLATE DATABASE_DEFAULT = p.SP_ID COLLATE DATABASE_DEFAULT 
		and VT.Practice_Display_Seq = p.Display_Seq
  INNER JOIN MedicalOrganization mo  with(nolock)  
	ON	p.SP_ID COLLATE DATABASE_DEFAULT = mo.SP_ID COLLATE DATABASE_DEFAULT
		and p.MO_Display_Seq  = mo.Display_Seq 
  LEFT OUTER JOIN District d  with(nolock) 
	ON	p.district COLLATE DATABASE_DEFAULT = d.district_code COLLATE DATABASE_DEFAULT
  LEFT OUTER JOIN DistrictBoard db  with(nolock) 
	ON	db.District_Board COLLATE DATABASE_DEFAULT = d.District_Board COLLATE DATABASE_DEFAULT	
  LEFT OUTER JOIN district_area da  with(nolock) 
	ON	db.area_Code COLLATE DATABASE_DEFAULT = da.area_code COLLATE DATABASE_DEFAULT
  INNER JOIN StaticData CoPayDesc  with(nolock) 
	on	CoPayDesc.Column_Name COLLATE DATABASE_DEFAULT = 'EHAPP_COPAYMENT' COLLATE DATABASE_DEFAULT
		and CoPayDesc.Item_No COLLATE DATABASE_DEFAULT = CoPay.AdditionalFieldValueCode COLLATE DATABASE_DEFAULT
  INNER JOIN StatusData TxStatus with(nolock) 
	on	TxStatus.Enum_Class COLLATE DATABASE_DEFAULT = 'ClaimTransStatus' COLLATE DATABASE_DEFAULT
		and VT.Record_status COLLATE DATABASE_DEFAULT = TxStatus.Status_Value  COLLATE DATABASE_DEFAULT
  INNER JOIN DocType dt 
	on	dt.Doc_Code COLLATE DATABASE_DEFAULT = VT.Doc_Code COLLATE DATABASE_DEFAULT 
  LEFT OUTER JOIN tempVoucherAccount tva 
	on	VT.Temp_Voucher_Acc_Id COLLATE DATABASE_DEFAULT = tva.Voucher_Acc_ID COLLATE DATABASE_DEFAULT
 WHERE  
  VT.Scheme_Code COLLATE DATABASE_DEFAULT = @Scheme_Code_EHAPP COLLATE DATABASE_DEFAULT
 AND VT.Transaction_Dtm >= @From_Dtm 
 AND VT.Transaction_Dtm < dateadd(dd, 1, @To_Dtm)
 ORDER BY VT.Transaction_Dtm 
 
-- =============================================  
-- Prepare Remark worksheet   
-- =============================================  
INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq)     
VALUES (@wsRemark, '(A) Legend' , @wsRemark_ct)    
SELECT @wsRemark_ct=@wsRemark_ct + 1  

INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq)     
VALUES (@wsRemark, '1. Identity Document Type' , @wsRemark_ct)    
SELECT @wsRemark_ct=@wsRemark_ct + 1  

INSERT INTO #WorkBook  (WorkSheetID, Result01, Result02, DisplaySeq)   
SELECT @wsRemark, Doc_Display_Code, Doc_Name,row_number() over (order by Display_Seq) + cast(@wsRemark_ct as integer)
FROM DocType with(nolock)
SELECT @wsRemark_ct= max(DisplaySeq) + 1 FROM   #WorkBook WHERE WorkSheetID = @wsRemark
 
INSERT INTO #WorkBook (WorkSheetID, Result01, Result02, DisplaySeq) VALUES (@wsRemark, '','',@wsRemark_ct)    
SELECT @wsRemark_ct=@wsRemark_ct+1  

INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq)     
VALUES (@wsRemark, '2. Profession Type' , @wsRemark_ct)    
SELECT @wsRemark_ct=@wsRemark_ct + 1  

INSERT INTO #WorkBook  (WorkSheetID, Result01, Result02, DisplaySeq)     
SELECT	@wsRemark,
		rtrim(Service_Category_Code) as Service_Category_Code,
		rtrim(Service_Category_Desc) as Service_Category_Desc,
		row_number() over (order by Service_Category_Code) + cast(@wsRemark_ct as integer)
FROM Profession with(nolock)
ORDER BY Service_Category_Code

SELECT @wsRemark_ct= max(DisplaySeq) + 1 FROM   #WorkBook WHERE WorkSheetID = @wsRemark

INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq)     
VALUES (@wsRemark, '' , @wsRemark_ct)    
SELECT @wsRemark_ct=@wsRemark_ct + 1  

INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq)     
VALUES (@wsRemark, '(B) Common Note(s) for the report' , @wsRemark_ct)   
SELECT @wsRemark_ct=@wsRemark_ct + 1  

INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq)     
VALUES (@wsRemark, '1. Transactions:' , @wsRemark_ct)    
SELECT @wsRemark_ct=@wsRemark_ct + 1  

INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq)     
VALUES (@wsRemark, '   a. All claim transactions created under service providers (either created by back office users or service providers (or the delegated users))' , @wsRemark_ct)    
SELECT @wsRemark_ct=@wsRemark_ct + 1  


INSERT INTO #WorkBook (WorkSheetID, Result01, DisplaySeq)     
VALUES (@wsRemark, '   b. Include all status (e.g. voided, removed)' , @wsRemark_ct)    
SELECT @wsRemark_ct=@wsRemark_ct + 1  

-- =============================================  
-- Get the resultset for whole workbook    
-- =============================================  

SELECT     
Result01, Result02, Result03, Result04, Result05,    
Result06, Result07, Result08, Result09, Result10,    
Result11, Result12, Result13, Result14, Result15,    
Result16, Result17, Result18, Result19, Result20     
FROM #WorkBook WHERE WorkSheetID = @wscontent    
ORDER BY DisplaySeq     
    
SELECT     
Result01, Result02  
FROM #WorkBook WHERE WorkSheetID = @wsCriteria    
ORDER BY DisplaySeq     

SELECT     
Result01, Result02, Result03, Result04, Result05,    
Result06, Result07, Result08, Result09, Result10,    
Result11, Result12, Result13, Result14, Result15,    
Result16, Result17, Result18, Result19, Result20, Result21    
FROM #WorkBook WHERE WorkSheetID = @ws01    
ORDER BY DisplaySeq     

SELECT     
Result01, Result02, Result03, Result04, Result05,    
Result06, Result07, Result08, Result09, Result10,    
Result11, Result12, Result13, Result14, Result15,    
Result16, Result17, Result18, Result19, Result20, Result21    
FROM #WorkBook WHERE WorkSheetID = @wsRemark   
ORDER BY DisplaySeq     


END     
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSU0004_Stat_get] TO HCVU
GO
