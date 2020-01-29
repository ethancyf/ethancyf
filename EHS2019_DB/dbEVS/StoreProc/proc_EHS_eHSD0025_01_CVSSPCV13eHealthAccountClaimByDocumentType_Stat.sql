IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0025_01_CVSSPCV13eHealthAccountClaimByDocumentType_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0025_01_CVSSPCV13eHealthAccountClaimByDocumentType_Stat]
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
-- Description: Retrieve eHealth Accout with Claim by DOC Type Statistic for CVSS PCV13
--				Copy from proc_EHS_eHealthAccountClaimByDocumentType_CIVSS_Stat
-- =============================================  

--exec proc_EHS_eHSD0025_01_CVSSPCV13eHealthAccountClaimByDocumentType_Stat '2013-12-05'; select * from RpteHSD0025eHealthAccountByDocumentTypeCVSSPCV13Stat
/*
CREATE Procedure [proc_EHS_eHSD0025_01_CVSSPCV13eHealthAccountClaimByDocumentType_Stat]  
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
SET @Scheme_Code = 'CVSSPCV13'
SET @Report_ID = 'eHSD0025'


OPEN SYMMETRIC KEY sym_Key   
 DECRYPTION BY ASYMMETRIC KEY asym_Key  
-- =============================================  
-- Return results  
-- =============================================  
SET @system_Dtm = getdate()  
  
CREATE TABLE #statNoOfAcc_withClaim_byDoc  
(  
 _Voucher_acc_ID   char(15),   
 _temp_voucher_acc_ID char(15),   
 _special_acc_ID   char(15),   
 _scheme_code   char(10),  
 _doc_code    char(20),  
 _doc_ID     varbinary(100)  
)  
  
CREATE TABLE #statNoOfAcc_withClaim_byDoc_distinct  
(  
 _scheme_code char(10),  
 _doc_code  char(20),  
 _doc_ID   varbinary(100)  
)  

CREATE TABLE #result_table  
(  
 _display_seq tinyint,   
 _result_value1 varchar(200) default '',  
 _result_value2 varchar(100) default '',  
 _result_value3 varchar(100) default '',  
 _result_value4 varchar(100) default '',  
 _result_value5 varchar(100) default '',  
 _result_value6 varchar(100) default '',  
 _result_value7 varchar(100) default '',  
 _result_value8 varchar(100) default '',  
 _result_value9 varchar(100) default '',  
 _result_value10 varchar(100) default ''  
)  
  
-- insert record for the final output format  
INSERT INTO #result_table (_display_seq, _result_value1)  
VALUES (0, 'eHS(S)D0025-01: Report on eHealth (Subsidies) Accounts with CVSSPCV13 claim transactions by document type')  
     
INSERT INTO #result_table (_display_seq)   
VALUES (1)  
     
INSERT INTO #result_table (_display_seq, _result_value1)  
VALUES (2, 'Reporting period: as at ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111))  
  
INSERT INTO #result_table (_display_seq)   
VALUES (3)  
  
INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5,   
       _result_value6, _result_value7, _result_value8, _result_value9, _result_value10)  
VALUES (10, @Scheme_Code, '', '', '', '', '', 'Total', '', @Scheme_Code, '')  
   
INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5,   
       _result_value6, _result_value7, _result_value8, _result_value9, _result_value10)  
VALUES (11, 'HKIC/HKBC', 'Doc/I', 'REPMT', 'ID235B', 'VISA', 'ADOPC', '', '', 'HKIC', 'HKBC')  
  
INSERT INTO #result_table (_display_seq)  
VALUES (12)  
  
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
SET  _result_value1 = (  
   SELECT count(distinct _doc_ID)  
   FROM #statNoOfAcc_withClaim_byDoc_distinct  
   WHERE _scheme_code = @Scheme_Code
      AND _doc_code IN ('HKIC', 'HKBC')  
  )  
WHERE _display_seq = 12  
  
UPDATE #result_table  
SET  _result_value2 = (  
   SELECT count(1)  
   FROM #statNoOfAcc_withClaim_byDoc_distinct  
   WHERE _scheme_code = @Scheme_Code
      AND _doc_code = 'Doc/I'  
  )  
WHERE _display_seq = 12  
  
UPDATE #result_table  
SET  _result_value3 = (  
   SELECT count(1)  
   FROM #statNoOfAcc_withClaim_byDoc_distinct  
   WHERE _scheme_code = @Scheme_Code
      AND _doc_code = 'REPMT'  
  )  
WHERE _display_seq = 12  
  
UPDATE #result_table  
SET  _result_value4 = (  
   SELECT count(1)  
   FROM #statNoOfAcc_withClaim_byDoc_distinct  
   WHERE _scheme_code = @Scheme_Code
      AND _doc_code = 'ID235B'  
  )  
WHERE _display_seq = 12  
  
UPDATE #result_table  
SET  _result_value5 = (  
   SELECT count(1)  
   FROM #statNoOfAcc_withClaim_byDoc_distinct  
   WHERE _scheme_code = @Scheme_Code
      AND _doc_code = 'VISA'  
  )  
WHERE _display_seq = 12  
  
UPDATE #result_table  
SET  _result_value6 = (  
   SELECT count(1)  
   FROM #statNoOfAcc_withClaim_byDoc_distinct  
   WHERE _scheme_code = @Scheme_Code
      AND _doc_code = 'ADOPC'  
  )  
WHERE _display_seq = 12  
  
UPDATE #result_table  
SET  _result_value7 = (  
   CONVERT(int, _result_value1)  
   + CONVERT(int, _result_value2)  
   + CONVERT(int, _result_value3)     
   + CONVERT(int, _result_value4)  
   + CONVERT(int, _result_value5)  
   + CONVERT(int, _result_value6)  
   + CONVERT(int, _result_value7)  
  )  
WHERE _display_seq = 12  
  
 
UPDATE #result_table  
SET  _result_value8 = ''  
WHERE _display_seq = 12  
  
  
UPDATE #result_table  
SET  _result_value9 = (  
   SELECT count(1)  
   FROM #statNoOfAcc_withClaim_byDoc_distinct  
   WHERE _scheme_code = @Scheme_Code
      AND _doc_code = 'HKIC'  
  )  
WHERE _display_seq = 12  
  
UPDATE #result_table  
SET  _result_value10 = (  
   SELECT count(1)  
   FROM #statNoOfAcc_withClaim_byDoc_distinct  
   WHERE _scheme_code = @Scheme_Code  
      AND _doc_code = 'HKBC'  
  )  
WHERE _display_seq = 12  
  
  
-- Retrieve the final result  
  
DELETE FROM RpteHSD0025CVSSPCV13eHealthAccountByDocumentTypeStat  
  
INSERT INTO RpteHSD0025CVSSPCV13eHealthAccountByDocumentTypeStat (  
 Result_Seq,  
 result_value1,  
 result_value2,  
 result_value3,  
 result_value4,  
 result_value5,  
 result_value6,  
 result_value7,  
 result_value8,  
 result_value9,  
 result_value10  
)  
SELECT   
 _display_Seq,  
 _result_value1,  
 _result_value2,  
 _result_value3,  
 _result_value4,  
 _result_value5,  
 _result_value6,  
 _result_value7,  
 _result_value8,  
 _result_value9,  
 _result_value10      
FROM  
 #result_table  
ORDER BY  
 _display_seq  
  
CLOSE SYMMETRIC KEY sym_Key  
  
DROP TABLE #statNoOfAcc_withClaim_byDoc  
DROP TABLE #statNoOfAcc_withClaim_byDoc_distinct  
DROP TABLE #result_table  
  
END  
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0025_01_CVSSPCV13eHealthAccountClaimByDocumentType_Stat] TO HCVU
GO
*/


