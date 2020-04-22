IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0026_02_OMPCV13EAgeReport_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0026_02_OMPCV13EAgeReport_Stat]
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
-- Modification History  
-- CR No.:			CRE14-021 Use of IV for Southern Hemisphere Vaccine under RVP
-- Modified by:		Chris YIM
-- Modified date:	29 April 2015
-- Description:		Rectify the sub title to "(ii) By age group^"
-- =============================================  
-- =============================================  
-- CR No.:		CRE14-017-04
-- Author:		Winnie SUEN
-- Create date: 27 Jan 2015
-- Description: Retrieve OMPCV13E transaction by age group
--				Copy from proc_EHS_EVSSAgeReport_Stat
-- =============================================  
/*    
CREATE Procedure [proc_EHS_eHSD0026_02_OMPCV13EAgeReport_Stat]    
 @Cutoff_Dtm as DateTime    
AS    
BEGIN    
SET NOCOUNT ON;    
-- =============================================    
-- Declaration    
-- =============================================    
DECLARE @Scheme_Code as varchar(10)
DECLARE @Report_ID as varchar(30)
DECLARE @current_scheme_Seq int    
DECLARE @schemeDate Datetime  

CREATE TABLE #temp_OMPCV13E    
(    
	voucher_acc_id			CHAR(15),    
	temp_voucher_acc_id		CHAR(15),    
	Special_Acc_ID    		CHAR(15),
	transaction_id			VARCHAR(20) COLLATE Chinese_Taiwan_Stroke_CI_AS,     
	identity_num			VARCHAR(20),    
	dob						DATETIME,    
	service_receive_dtm		DATETIME,    
	SP_ID					CHAR(8)    
)    
CREATE INDEX IX_VAT on #temp_OMPCV13E (voucher_acc_id)    
    
    
CREATE TABLE #account    
(    
	voucher_acc_id			CHAR(15),    
	temp_voucher_acc_id		CHAR(15),    
	Special_Acc_ID    		CHAR(15),
	identity_num			VARCHAR(20),    
	doc_code				CHAR(10),    
	dob						DATETIME
)    
CREATE INDEX IX_VAT on #account (identity_num)    

-- =============================================    
-- Validation     
-- =============================================    
-- =============================================    
-- Initialization    
-- =============================================    
SET @Scheme_Code = 'OMPCV13E'
SET @Report_ID = 'eHSD0026'
SET @schemeDate = DATEADD(dd, -1, @Cutoff_Dtm)  


EXEC @current_scheme_Seq = [proc_EHS_GetSchemeSeq_Stat] @Scheme_Code, @schemeDate  


-- =============================================    
-- Return results    
-- =============================================    
OPEN SYMMETRIC KEY sym_Key     
 DECRYPTION BY ASYMMETRIC KEY asym_Key    
    
    
INSERT INTO #temp_OMPCV13E  
(    
	transaction_id,    
	voucher_acc_id,    
	temp_voucher_acc_id,
	Special_Acc_ID,
	service_receive_dtm,    
	SP_ID
)    
SELECT vt.transaction_id, voucher_acc_id, temp_voucher_acc_id, Special_Acc_ID, service_receive_dtm, SP_ID
FROM vouchertransaction vt    
inner join transactiondetail td on vt.transaction_id = td.transaction_id
WHERE   
vt.scheme_code = @Scheme_Code 
and transaction_dtm < @Cutoff_Dtm    
and record_status not in (SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)     
AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'     
AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm)))    
AND td.subsidize_code in (	select subsidize_code from SubsidizeGroupClaimItemDetails 
							where scheme_code = @Scheme_Code)    
AND (vt.Invalidation IS NULL OR vt.Invalidation NOT In     
(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)     
AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'    
AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm))))   
  
  
-- =============================================    
-- Account
-- =============================================    
INSERT INTO #account    
(    
	voucher_acc_id,    
	temp_voucher_acc_id, 
	Special_Acc_ID,   
	identity_num,    
	doc_code,    
	dob    
)    
SELECT p.voucher_acc_id,    
  NULL,
  NULL,
  convert(varchar, DecryptByKey(p.Encrypt_Field1)),    
  p.doc_code,    
  p.dob    
FROM voucheraccount va, personalinformation p    
WHERE va.voucher_acc_id = p.voucher_acc_id    
and va.create_dtm < @Cutoff_Dtm    
    
    
INSERT INTO #account    
(    
	voucher_acc_id,    
	temp_voucher_acc_id,
	Special_Acc_ID,
	identity_num,    
	doc_code,    
	dob    
)    
SELECT NULL,    
  p.voucher_acc_id,      
  NULL,
  convert(varchar, DecryptByKey(p.Encrypt_Field1)),    
  p.doc_code,    
  p.dob    
FROM tempvoucheraccount va, temppersonalinformation p    
WHERE va.voucher_acc_id = p.voucher_acc_id       
AND va.create_dtm < @Cutoff_Dtm    
    
    
INSERT INTO #account    
(    
	voucher_acc_id,    
	temp_voucher_acc_id,
	Special_Acc_ID,
	identity_num,    
	doc_code,    
	dob    
)    
SELECT NULL,    
  NULL,
  p.Special_Acc_ID,        
  convert(varchar, DecryptByKey(p.Encrypt_Field1)),    
  p.doc_code,    
  p.dob    
FROM specialaccount sa, specialpersonalinformation p    
WHERE sa.Special_Acc_ID = p.Special_Acc_ID       
AND sa.create_dtm < @Cutoff_Dtm    



UPDATE #temp_OMPCV13E  
SET identity_num = a.identity_num,    
    dob = a.dob    
FROM #account a, #temp_OMPCV13E pcv  
WHERE isnull(pcv.voucher_acc_id ,'') <> ''    
and isnull(a.voucher_acc_id ,'') <> ''    
and pcv.voucher_acc_id = a.voucher_acc_id    
 
 
UPDATE #temp_OMPCV13E  
SET identity_num = a.identity_num,    
    dob = a.dob    
FROM #account a, #temp_OMPCV13E pcv    
WHERE isnull(pcv.temp_voucher_acc_id ,'') <> ''    
and isnull(a.temp_voucher_acc_id ,'') <> ''    
and pcv.temp_voucher_acc_id = a.temp_voucher_acc_id    


UPDATE #temp_OMPCV13E  
SET identity_num = a.identity_num,    
    dob = a.dob    
FROM #account a, #temp_OMPCV13E pcv    
WHERE isnull(pcv.Special_Acc_ID ,'') <> ''    
and isnull(a.Special_Acc_ID ,'') <> ''    
and pcv.Special_Acc_ID = a.Special_Acc_ID           




CREATE TABLE #result_table    
(    
  Display_Seq tinyint,     
  Col1 varchar(100) default '',    
  Col2 varchar(100) default '',    
  Col3 varchar(100) default '',    
  Col4 varchar(100) default '',    
  Col5 varchar(100) default '',    
  Col6 varchar(100) default '',    
  Col7 varchar(100) default '',    
  Col8 varchar(100) default '',    
  Col9 varchar(100) default '',    
  Col10 varchar(100) default ''
)    
    
-- insert record for the final output format    
INSERT INTO #result_table (Display_Seq, Col1)    
VALUES (0, 'eHS(S)D0026-02: Report on OMPCV13E transaction by age group')    
INSERT INTO #result_table (Display_Seq)    
VALUES (1)    
INSERT INTO #result_table (Display_Seq, Col1)    
VALUES (2, 'Reporting period: as at ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111))    
INSERT INTO #result_table (Display_Seq)    
VALUES (3)    
INSERT INTO #result_table (Display_Seq)    
VALUES (4)    

  
INSERT INTO #result_table (Display_Seq, Col1)     
VALUES (10, '(i) By age group^')    
INSERT INTO #result_table (Display_Seq, Col2,Col3,Col4,Col5,Col6)     
VALUES (11, '''=65 age year','66 to 69 age year','70 to 79 age year','>= 80 age year','Total')    
    
INSERT INTO #result_table (Display_Seq, Col1)     
SELECT TOP 1 12,Display_Code_For_Claim
FROM [SubsidizeGroupClaim]    
WHERE Scheme_Code = @Scheme_Code AND Scheme_seq = @current_scheme_Seq
    
update #result_table set     
Col2 = (select count(distinct identity_num) from #temp_OMPCV13E    
where datepart(year,service_receive_dtm) - datepart(year,dob) = 65),    
Col3 = (select count(distinct identity_num) from #temp_OMPCV13E     
where datepart(year,service_receive_dtm) - datepart(year,dob) >= 66 and datepart(year,service_receive_dtm) - datepart(year,dob) < 70),  
Col4 = (select count(distinct identity_num) from #temp_OMPCV13E     
where datepart(year,service_receive_dtm) - datepart(year,dob) >= 70 and datepart(year,service_receive_dtm) - datepart(year,dob) < 80),  
Col5 = (select count(distinct identity_num) from #temp_OMPCV13E     
where datepart(year,service_receive_dtm) - datepart(year,dob) >=80)    
WHERE Display_Seq = 12    
update #result_table set     
Col6 = (select cast(Col2 as int) +  cast(Col3 as int) +  cast(Col4 as int) +  cast(Col5 as int) FROM #result_table    
WHERE Display_Seq = 12) WHERE Display_Seq = 12
    

-- INSERT record to final result    
DELETE FROM RpteHSD0026OMPCV13EAgeReportStat  
INSERT INTO RpteHSD0026OMPCV13EAgeReportStat
( 
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
  Col10 
)     
SELECT        
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
  Col10  
 FROM #result_table    
 ORDER BY    
  Display_Seq    
    
    
CLOSE SYMMETRIC KEY sym_Key    
        
  
drop table #temp_OMPCV13E   
drop table #account    
    
DROP TABLE #result_table    
    
END    
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0026_02_OMPCV13EAgeReport_Stat] TO HCVU
GO
*/
