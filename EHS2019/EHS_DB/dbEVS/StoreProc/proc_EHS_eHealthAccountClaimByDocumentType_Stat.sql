IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHealthAccountClaimByDocumentType_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHealthAccountClaimByDocumentType_Stat]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Mattie LO
-- Create date: 16 Oct 2009
-- Description:	Retrieve eHealth Accout with Claim by DOC Type Statistic
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	16 October 2009
-- Description:		Reformat the output
-- =============================================
-- =============================================
-- Modification History	
-- Modified by:		Mattie LO
-- Modified date:		18 October 2009
-- Description:		Update the count by doc type
-- =============================================
-- =============================================
-- Modification History	
-- Modified by:		Kathy LEE
-- Modified date:	20 October 2009
-- Description:		Retrieve eHealth Accout with Claim by DOC Type Statistic
--					for Scheme 'CIVSS' only
-- =============================================
-- =============================================
-- Modification History	
-- Modified by:		Lawrence TSANG
-- Modified date:	20 October 2009
-- Description:		(1) Reformat the output
--					(2) Correct the total
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	28 October 2009
-- Description:		Change the date format to YYYY/MM/DD (code 111)
-- =============================================
-- =============================================
-- Modification History	
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE Procedure [proc_EHS_eHealthAccountClaimByDocumentType_Stat]
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
	_Voucher_acc_ID			char(15), 
	_temp_voucher_acc_ID	char(15), 
	_special_acc_ID			char(15), 
	_scheme_code			char(10),
	_doc_code				char(20),
	_doc_ID					varbinary(100)
)

CREATE TABLE #statNoOfAcc_withClaim_byDoc_distinct
(
	_scheme_code	char(10),
	_doc_code		char(20),
	_doc_ID			varbinary(100)
)

/*
CREATE TABLE #tmpResult_table
(
	_scheme_code_internal	char(10),
	_doc_code				char(20),
	_count					integer
)
*/

CREATE TABLE #result_table
(
	_display_seq	tinyint, 
	_result_value1	varchar(100),
	_result_value2	varchar(100),
	_result_value3	varchar(100),
	_result_value4	varchar(100),
	_result_value5	varchar(100),
	_result_value6	varchar(100),
	_result_value7	varchar(100),
	_result_value8	varchar(100),
	_result_value9	varchar(100),
	_result_value10	varchar(100)
)

-- insert record for the final output format

INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5, 
							_result_value6, _result_value7, _result_value8, _result_value9, _result_value10)
VALUES (0, 'eHealth Accounts with CIVSS claim transactions (as at ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111) + ')', '', '', '', '',
			'', '', '', '', '')
			
INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5, 
							_result_value6, _result_value7, _result_value8, _result_value9, _result_value10)
VALUES (1, 'By document type', '', '', '', '',
			'', '', '', '', '')
			
INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5, 
							_result_value6, _result_value7, _result_value8, _result_value9, _result_value10)
VALUES (2, '', '', '', '', '',
			'', '', '', '', '')

INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5, 
							_result_value6, _result_value7, _result_value8, _result_value9, _result_value10)
VALUES (10, 'CIVSS', '', '', '', '', '', 'Total', '', 'CIVSS**', '')
	
INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5, 
							_result_value6, _result_value7, _result_value8, _result_value9, _result_value10)
VALUES (11, 'HKIC/HKBC**', 'Doc/I', 'REPMT', 'ID235B', 'VISA', 'ADOPC', '', '', 'HKIC', 'HKBC')

INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5, 
							_result_value6, _result_value7, _result_value8, _result_value9, _result_value10)
VALUES (12, '', '', '', '', '', 
			'', '', '', '', '')

-- select the voucher transactions included for statistic
INSERT INTO #statNoOfAcc_withClaim_byDoc	(
		_Voucher_acc_ID,
		_temp_voucher_acc_ID,
		_special_acc_ID,
		_scheme_code,
		_doc_code	)
(SELECT Voucher_acc_ID,
		temp_voucher_acc_ID,
		special_acc_ID,
		scheme_code,
		doc_code
	FROM voucherTransaction vTran 
	WHERE Record_Status <> 'I'
		and vTran.transaction_dtm <= @Cutoff_Dtm
		--and vTran.service_receive_dtm >= '19 OCT 2009'
		--and vTran.Service_Receive_Dtm <= @Cutoff_Dtm
		--and vTran.Scheme_Code IN ('CIVSS', 'EVSS')
		and vTran.Scheme_Code IN ('CIVSS')
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
/*
INSERT INTO #tmpResult_table
(
	_scheme_code_internal,
	_doc_code,
	_count	)	
(SELECT _scheme_code,
		_doc_code,
		count(1)
FROM #statNoOfAcc_withClaim_byDoc_distinct
WHERE (_doc_ID is not NULL AND _doc_ID <> '')
GROUP BY _scheme_code,
		_doc_code
)
*/
UPDATE	#result_table
SET		_result_value1 = (
			SELECT	count(distinct _doc_ID)
			FROM	#statNoOfAcc_withClaim_byDoc_distinct
			WHERE	_scheme_code = 'CIVSS'
						AND _doc_code IN ('HKIC', 'HKBC')
		)
WHERE	_display_seq = 12

UPDATE	#result_table
SET		_result_value2 = (
			SELECT	count(1)
			FROM	#statNoOfAcc_withClaim_byDoc_distinct
			WHERE	_scheme_code = 'CIVSS'
						AND _doc_code = 'Doc/I'
		)
WHERE	_display_seq = 12

UPDATE	#result_table
SET		_result_value3 = (
			SELECT	count(1)
			FROM	#statNoOfAcc_withClaim_byDoc_distinct
			WHERE	_scheme_code = 'CIVSS'
						AND _doc_code = 'REPMT'
		)
WHERE	_display_seq = 12

UPDATE	#result_table
SET		_result_value4 = (
			SELECT	count(1)
			FROM	#statNoOfAcc_withClaim_byDoc_distinct
			WHERE	_scheme_code = 'CIVSS'
						AND _doc_code = 'ID235B'
		)
WHERE	_display_seq = 12

UPDATE	#result_table
SET		_result_value5 = (
			SELECT	count(1)
			FROM	#statNoOfAcc_withClaim_byDoc_distinct
			WHERE	_scheme_code = 'CIVSS'
						AND _doc_code = 'VISA'
		)
WHERE	_display_seq = 12

UPDATE	#result_table
SET		_result_value6 = (
			SELECT	count(1)
			FROM	#statNoOfAcc_withClaim_byDoc_distinct
			WHERE	_scheme_code = 'CIVSS'
						AND _doc_code = 'ADOPC'
		)
WHERE	_display_seq = 12

UPDATE	#result_table
SET		_result_value7 = (
			CONVERT(int, _result_value1)
			+ CONVERT(int, _result_value2)
			+ CONVERT(int, _result_value3)			
			+ CONVERT(int, _result_value4)
			+ CONVERT(int, _result_value5)
			+ CONVERT(int, _result_value6)
			+ CONVERT(int, _result_value7)
		)
WHERE	_display_seq = 12


/*UPDATE	#result_table
SET		_result_value10 = (
			SELECT	count(1)
			FROM	#statNoOfAcc_withClaim_byDoc_distinct
			WHERE	_scheme_code = 'EVSS'
						AND _doc_code = 'HKIC'
		)
WHERE	_display_seq = 12*/

/*UPDATE	#result_table
SET		_result_value11 = (
			SELECT	count(1)
			FROM	#statNoOfAcc_withClaim_byDoc_distinct
			WHERE	_scheme_code = 'EVSS'
						AND _doc_code = 'EC'
		)
WHERE	_display_seq = 12*/

/*UPDATE	#result_table
SET		_result_value12 = (
			CONVERT(int, _result_value10)
			+ CONVERT(int, _result_value11)
		)
WHERE	_display_seq = 12*/

UPDATE	#result_table
SET		_result_value8 = ''
WHERE	_display_seq = 12


UPDATE	#result_table
SET		_result_value9 = (
			SELECT	count(1)
			FROM	#statNoOfAcc_withClaim_byDoc_distinct
			WHERE	_scheme_code = 'CIVSS'
						AND _doc_code = 'HKIC'
		)
WHERE	_display_seq = 12

UPDATE	#result_table
SET		_result_value10 = (
			SELECT	count(1)
			FROM	#statNoOfAcc_withClaim_byDoc_distinct
			WHERE	_scheme_code = 'CIVSS'
						AND _doc_code = 'HKBC'
		)
WHERE	_display_seq = 12


-- Retrieve the final result

DELETE FROM _eHealthAccountClaimByDocumentType_Stat

INSERT INTO _eHealthAccountClaimByDocumentType_Stat (
--	_system_dtm,
--	_report_dtm,
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
	_result_value10
)
SELECT 
--	@system_Dtm,
--	convert(datetime, convert(varchar(10), @cutoff_Dtm, 102)),
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
	_result_value10
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

GRANT EXECUTE ON [dbo].[proc_EHS_eHealthAccountClaimByDocumentType_Stat] TO HCVU
GO
