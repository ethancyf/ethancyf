/*
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[proc_EHS_eHealthAccountClaimByDocumentType_EVSS_Stat]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[proc_EHS_eHealthAccountClaimByDocumentType_EVSS_Stat]
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	11 April 2011
-- Description:		Stored procedure is no longer used
-- =============================================    
-- =============================================    
-- Modification History    
-- Modified by:  Eric Tse    
-- Modified date: 21 October 2010    
-- Description:  Create a new workbook for EVSS Stat    
--    this is reengine from [proc_EHS_eHealthAccountClaimByDocumentType_Stat]  
--    this modification focus on new report layout standand and filter invalidation transaction     
-- =============================================    
    
CREATE Procedure [proc_EHS_eHealthAccountClaimByDocumentType_EVSS_Stat]    
 @Cutoff_Dtm as DateTime    
AS    
BEGIN    
 SET NOCOUNT ON;    
-- =============================================    
-- Declaration    
-- =============================================    
DECLARE @system_Dtm as datetime    
-- =============================================    
-- Validation     
-- =============================================    
-- =============================================    
-- Initialization    
-- =============================================    
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
    
--CREATE TABLE #tmpResult_table    
--(    
-- _scheme_code_internal char(10),    
-- _doc_code    char(20),    
-- _count     integer    
--)    
    
CREATE TABLE #result_table    
(    
 _display_seq tinyint,     
 _result_value1 varchar(100) default '',    
 _result_value2 varchar(100) default '',    
 _result_value3 varchar(100) default '',    
 _result_value4 varchar(100) default '',    
 _result_value5 varchar(100) default '',    
 _result_value6 varchar(100) default '',    
 _result_value7 varchar(100) default '',    
 _result_value8 varchar(100) default '',    
 _result_value9 varchar(100) default '',    
 _result_value10 varchar(100) default '',    
 _result_value11 varchar(100) default '',    
 _result_value12 varchar(100) default '',    
 _result_value13 varchar(100) default '',    
 _result_value14 varchar(100) default ''    
)    
    
-- insert record for the final output format    
INSERT INTO #result_table (_display_seq, _result_value1)    
VALUES (0, 'eHSD0002-01: Report on eHealth Accounts with EVSS claim transactions by document type')    
INSERT INTO #result_table (_display_seq)    
VALUES (1)    
INSERT INTO #result_table (_display_seq, _result_value1)    
VALUES (2, 'Reporting period: as at ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111))    
INSERT INTO #result_table (_display_seq)    
VALUES (3)    
INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3)    
VALUES (4, 'EVSS', '', 'Total')    
INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3)    
VALUES (5, 'HKIC', 'EC', '')    
INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3)    
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
 WHERE Record_Status not in     
(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = 'eHSD0002')     
AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'     
AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm)))       
 and vTran.Scheme_Code = 'EVSS'    
AND vTran.transaction_dtm <= @Cutoff_Dtm    
  AND (vTran.Invalidation IS NULL OR vTran.Invalidation NOT In     
(SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = 'eHSD0002')     
AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'    
AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm))))    
  --and SP_ID NOT IN (SELECT SP_ID FROM SPExceptionList)    
  )    
    
-- update the DOC ID for validated ACC    
UPDATE #statNoOfAcc_withClaim_byDoc    
SET _doc_ID = (SELECT CONVERT(varbinary(100), DecryptByKey(pInfo.[Encrypt_Field1]))    
 FROM personalInformation pInfo, voucherAccount vACC    
 WHERE pInfo.voucher_acc_ID COLLATE DATABASE_DEFAULT = #statNoOfAcc_withClaim_byDoc._Voucher_acc_ID COLLATE DATABASE_DEFAULT    
  and pInfo.Doc_Code COLLATE DATABASE_DEFAULT = #statNoOfAcc_withClaim_byDoc._Doc_Code COLLATE DATABASE_DEFAULT    
  and vAcc.voucher_acc_ID = pInfo.voucher_acc_ID    
  and vAcc.record_status <> 'D'    
  --and vAcc.create_by NOT IN (SELECT SP_ID FROM SPExceptionList)    
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
  --and sAcc.create_by NOT IN (SELECT SP_ID FROM SPExceptionList)    
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
  --and tAcc.create_by NOT IN (SELECT SP_ID FROM SPExceptionList)    
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
    
--INSERT INTO #tmpResult_table    
--(    
-- _scheme_code_internal,    
-- _doc_code,    
-- _count )     
--(SELECT _scheme_code,    
--  _doc_code,    
--  count(1)    
--FROM #statNoOfAcc_withClaim_byDoc_distinct    
--WHERE (_doc_ID is not NULL AND _doc_ID <> '')    
--GROUP BY _scheme_code,    
--  _doc_code    
--)    
    
UPDATE #result_table    
SET  _result_value1 = (    
   SELECT count (distinct _doc_ID)    
   FROM #statNoOfAcc_withClaim_byDoc_distinct    
   WHERE _scheme_code = 'EVSS'    
      AND _doc_code = 'HKIC'    
  )    
WHERE _display_seq = 12    
    
UPDATE #result_table    
SET  _result_value2 = (    
   SELECT count (distinct _doc_ID)    
   FROM #statNoOfAcc_withClaim_byDoc_distinct    
   WHERE _scheme_code = 'EVSS'    
      AND _doc_code = 'EC'    
  )    
WHERE _display_seq = 12    
    
UPDATE #result_table    
SET  _result_value3 = (    
   CONVERT(int, _result_value1)    
   + CONVERT(int, _result_value2)    
  )    
WHERE _display_seq = 12    
    
-- Retrieve the final result    
    
DELETE FROM _eHealthAccountByDocumentType_EVSS_Stat    
    
INSERT INTO _eHealthAccountByDocumentType_EVSS_Stat (    
-- _system_dtm,    
-- _report_dtm,    
 result_seq,    
 result_value1,    
 result_value2,    
 result_value3,    
 result_value4,    
 result_value5,    
 result_value6,    
 result_value7,    
 result_value8,    
 result_value9,    
 result_value10,    
 result_value11,    
 result_value12,    
 result_value13    
)    
SELECT     
-- @system_Dtm,    
-- convert(datetime, convert(varchar(10), @cutoff_Dtm, 102)),    
 _display_seq,    
 _result_value1,    
 _result_value2,    
 _result_value3,    
 _result_value4,    
 _result_value5,    
 _result_value6,    
 _result_value7,    
 _result_value8,    
 _result_value9,    
 _result_value10,    
 _result_value11,    
 _result_value12,    
 _result_value13    
FROM    
 #result_table    
ORDER BY    
 _display_seq    
    
CLOSE SYMMETRIC KEY sym_Key    
    
DROP TABLE #statNoOfAcc_withClaim_byDoc    
DROP TABLE #statNoOfAcc_withClaim_byDoc_distinct    
--DROP TABLE #tmpResult_table    
DROP TABLE #result_table    
    
END    
GO

Grant execute on [dbo].[proc_EHS_eHealthAccountClaimByDocumentType_EVSS_Stat] to HCVU
GO 


--GRANT EXEC ON Stored_Procedure_Name TO PUBLIC

--GO

*/