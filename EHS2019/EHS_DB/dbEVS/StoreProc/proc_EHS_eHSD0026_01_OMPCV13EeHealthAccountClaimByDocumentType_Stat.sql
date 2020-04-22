IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0026_01_OMPCV13EeHealthAccountClaimByDocumentType_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0026_01_OMPCV13EeHealthAccountClaimByDocumentType_Stat]
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
-- CR No.:		CRE14-017-04
-- Author:		Winnie SUEN
-- Create date: 27 Jan 2015
-- Description: Retrieve eHealth Accout with Claim by DOC Type Statistic for EVSS PCV13
--				Copy from proc_EHS_eHSD0025_01_CVSSPCV13eHealthAccountClaimByDocumentType_Stat
-- =============================================  
/*
CREATE Procedure [proc_EHS_eHSD0026_01_OMPCV13EeHealthAccountClaimByDocumentType_Stat]  
 @Cutoff_Dtm as DateTime  
AS  
BEGIN  
 SET NOCOUNT ON;  
-- =============================================  
-- Declaration  
-- =============================================  
DECLARE @system_Dtm as datetime  
DECLARE @Scheme_Code as varchar(10)
DECLARE @Report_ID as varchar(30)
-- =============================================  
-- Validation   
-- =============================================  
-- =============================================  
-- Initialization  
-- =============================================  
SET @Scheme_Code = 'OMPCV13E'
SET @Report_ID = 'eHSD0026'


OPEN SYMMETRIC KEY sym_Key   
 DECRYPTION BY ASYMMETRIC KEY asym_Key  
-- =============================================  
-- Return results  
-- =============================================  
SET @system_Dtm = getdate()  
  
CREATE TABLE #statNoOfAcc_withClaim_byDoc  
(  
 _Voucher_acc_ID			char(15),   
 _temp_voucher_acc_ID char(15),   
 _special_acc_ID			char(15),   
 _scheme_code					char(10),  
 _doc_code						char(20),  
 _doc_ID							varbinary(100)  
)  
  
CREATE TABLE #statNoOfAcc_withClaim_byDoc_distinct  
(  
 _scheme_code char(10),  
 _doc_code  char(20),  
 _doc_ID   varbinary(100)  
)  

CREATE TABLE #result_table  
(  
  Display_Seq tinyint,     
  Col1 varchar(200) default '',    
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
VALUES (0, 'eHS(S)D0026-01: Report on eHealth (Subsidies) Accounts with OMPCV13E claim transactions by document type')  
     
INSERT INTO #result_table (Display_Seq)   
VALUES (1)  
     
INSERT INTO #result_table (Display_Seq, Col1)  
VALUES (2, 'Reporting period: as at ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111))  
  
INSERT INTO #result_table (Display_Seq)   
VALUES (3)  
  
INSERT INTO #result_table (Display_Seq, Col1, Col2, Col3)  
VALUES (10, @Scheme_Code, '', 'Total')  
   
INSERT INTO #result_table (Display_Seq, Col1, Col2, Col3)  
VALUES (11, 'HKIC', 'EC', '')  
  
INSERT INTO #result_table (Display_Seq, Col1, Col2, Col3)    
VALUES (12, '', '', '')     
  
-- select the voucher transactions included for statistic  
INSERT INTO #statNoOfAcc_withClaim_byDoc (  
  _Voucher_acc_ID,  
  _temp_voucher_acc_ID,  
  _special_acc_ID,  
  _scheme_code,  
  _doc_code )  
(SELECT Voucher_acc_ID,  
  temp_voucher_acc_ID,  
  special_acc_ID,  
  scheme_code,  
  doc_code  
 FROM voucherTransaction vTran   
 WHERE  vTran.Scheme_Code = @Scheme_Code
  and vTran.transaction_dtm <= @Cutoff_Dtm   
AND (vTran.Invalidation IS NULL 
	OR vTran.Invalidation NOT In   
		(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)   
		AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'  
		AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm))))  
AND vTran.Record_Status not in   
		(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = @Report_ID)   
		AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'   
		AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm)))    
)  
  
-- update the DOC ID for validated ACC  
UPDATE #statNoOfAcc_withClaim_byDoc  
SET _doc_ID = (SELECT CONVERT(varbinary(100), DecryptByKey(pInfo.[Encrypt_Field1]))  
 FROM personalInformation pInfo, voucherAccount vACC  
 WHERE pInfo.voucher_acc_ID COLLATE DATABASE_DEFAULT = #statNoOfAcc_withClaim_byDoc._Voucher_acc_ID COLLATE DATABASE_DEFAULT  
  and pInfo.Doc_Code COLLATE DATABASE_DEFAULT = #statNoOfAcc_withClaim_byDoc._Doc_Code COLLATE DATABASE_DEFAULT  
  and vAcc.voucher_acc_ID = pInfo.voucher_acc_ID  
  and vAcc.record_status <> 'D'    
 )  
WHERE (#statNoOfAcc_withClaim_byDoc._doc_ID is NULL or #statNoOfAcc_withClaim_byDoc._doc_ID = '')  
  
-- update the DOC ID for special ACC  
UPDATE #statNoOfAcc_withClaim_byDoc  
SET _doc_ID = (SELECT CONVERT(varbinary(100), DecryptByKey(sInfo.[Encrypt_Field1]))  
 FROM specialpersonalinformation sInfo, specialaccount sAcc  
 WHERE sInfo.special_acc_ID COLLATE DATABASE_DEFAULT = #statNoOfAcc_withClaim_byDoc._special_acc_ID COLLATE DATABASE_DEFAULT  
  and sInfo.Doc_Code COLLATE DATABASE_DEFAULT = #statNoOfAcc_withClaim_byDoc._Doc_Code COLLATE DATABASE_DEFAULT  
  and sAcc.special_acc_ID = sInfo.special_acc_ID  
  and sAcc.record_status NOT IN ('V', 'D')    
 )  
WHERE (#statNoOfAcc_withClaim_byDoc._doc_ID is NULL or #statNoOfAcc_withClaim_byDoc._doc_ID = '')  
  
-- update the DOC ID for temp ACC  
UPDATE #statNoOfAcc_withClaim_byDoc  
SET _doc_ID = (SELECT CONVERT(varbinary(100), DecryptByKey(tInfo.[Encrypt_Field1]))  
 FROM temppersonalinformation tInfo, tempvoucheraccount tAcc  
 WHERE tInfo.voucher_acc_ID COLLATE DATABASE_DEFAULT = #statNoOfAcc_withClaim_byDoc._temp_voucher_acc_ID COLLATE DATABASE_DEFAULT  
  and tInfo.Doc_Code COLLATE DATABASE_DEFAULT = #statNoOfAcc_withClaim_byDoc._Doc_Code COLLATE DATABASE_DEFAULT  
  and tAcc.voucher_acc_ID = tInfo.voucher_acc_ID  
  and tAcc.record_status NOT IN ('V', 'D')    
 )  
WHERE (#statNoOfAcc_withClaim_byDoc._doc_ID is NULL or #statNoOfAcc_withClaim_byDoc._doc_ID = '')  
  
INSERT INTO #statNoOfAcc_withClaim_byDoc_distinct  
(  
 _scheme_code,  
 _doc_code,  
 _doc_ID)  
(SELECT distinct _scheme_code,  
    _doc_code,  
    _doc_ID  
FROM #statNoOfAcc_withClaim_byDoc  
WHERE (_doc_ID is not NULL AND _doc_ID <> '')  
)  

UPDATE #result_table    
SET  Col1 = (    
   SELECT count (distinct _doc_ID)    
   FROM #statNoOfAcc_withClaim_byDoc_distinct    
   WHERE _scheme_code = @Scheme_Code
      AND _doc_code = 'HKIC'    
  )    
WHERE Display_Seq = 12    
    
UPDATE #result_table    
SET  Col2 = (    
   SELECT count (distinct _doc_ID)    
   FROM #statNoOfAcc_withClaim_byDoc_distinct    
   WHERE _scheme_code = @Scheme_Code
      AND _doc_code = 'EC'    
  )    
WHERE Display_Seq = 12    
    
UPDATE #result_table    
SET  Col3 = (    
   CONVERT(int, Col1)    
   + CONVERT(int, Col2)    
  )    
WHERE Display_Seq = 12    
  
  
-- Retrieve the final result  
  
DELETE FROM RpteHSD0026OMPCV13EeHealthAccountByDocumentTypeStat  
  
INSERT INTO RpteHSD0026OMPCV13EeHealthAccountByDocumentTypeStat (  
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
FROM  
 #result_table  
ORDER BY  
 Display_Seq  
  
CLOSE SYMMETRIC KEY sym_Key  
  
DROP TABLE #statNoOfAcc_withClaim_byDoc  
DROP TABLE #statNoOfAcc_withClaim_byDoc_distinct  
DROP TABLE #result_table  
  
END  
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0026_01_OMPCV13EeHealthAccountClaimByDocumentType_Stat] TO HCVU
GO
*/
