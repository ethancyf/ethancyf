IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0021]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0021]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE18-001: Performance tuning on internal statistic reports generation in eHS(S)
-- Modified by:		Koala CHENG
-- Modified date:	15 May 2018
-- Description:		Prepare new temp table #tempTransaction and #tempTransactionVaccineSeason
--					Fine tune #tempAccount
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE16-001
-- Modified by:		Winnie SUEN
-- Modified date:	28 Apr 2016
-- Description:		Replace table [StatVaccineMapping] with new table [VaccineSeason]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE13-021-02
-- Modified by:		Tommy LAM
-- Modified date:	03 Jan 2014
-- Description:		Generate Worksheet - "Remark" dynamically
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE13-001 
-- Modified by:		Koala CHENG
-- Modified date:	29 May 2013
-- Description:		Add Subsidize_type column to #tempAccount
--					Add sub report 07 - EHAPP claim break down by period
--					Re-order sub report of 04-06
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRP11-005
-- Modified by:		Helen Lam
-- Modified date:	05 APR 2012
-- Description:		CRP11-005 - Culmulative total transactions statistic report
-- =============================================
CREATE PROCEDURE [dbo].[proc_EHS_eHSD0021]
@Report_Dtm		datetime = NULL
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Report data
-- =============================================
	
	IF @Report_Dtm IS NOT NULL BEGIN
		SELECT @Report_Dtm = CONVERT(varchar, DATEADD(dd, 1, @Report_Dtm), 106)
	END ELSE BEGIN
		SELECT @Report_Dtm = CONVERT(varchar, GETDATE(), 106) -- "106" gives "dd MMM yyyy"  
	END

	CREATE TABLE #tempTransaction  (
		Scheme_Code			CHAR(10),
		--Scheme_Seq			SMALLINT,
		--Subsidize_Item_Code	CHAR(10),
		Subsidize_Type		CHAR(20),	
		Manual_Reimburse	CHAR(1),
		Record_Status		CHAR(1),
		[Invalidation]		CHAR(1),
		TxCount				INT,
	)

	CREATE TABLE #tempTransactionVaccineSeason  (
		TxCount				INT,
		Subsidize_Item_Code	CHAR(10),
		Season_Desc			VARCHAR(20)		
	)

	create table #tempAccount  (
		encrypt_field1		varbinary(100),
		Doc_code			varchar(20),
		scheme_code			varchar(10),
		scheme_seq			int,
		Subsidize_item_code		varchar(10)
	)
		
	DECLARE @SchemeClaim TABLE (
		Scheme_Code		char(10),
		Display_Code	char(25),
		Display_Seq		smallint,
		Scheme_Desc		varchar(100)
	)

	DECLARE @SubsidizeGroupClaim TABLE (
		Scheme_Code		char(10),
		Scheme_Seq		smallint
	)

	INSERT INTO @SchemeClaim (Scheme_Code, Display_Code, Display_Seq, Scheme_Desc)
	SELECT Scheme_Code, Display_Code, Display_Seq, Scheme_Desc
	FROM SchemeClaim WITH (NOLOCK)
	WHERE Effective_Dtm <= @Report_Dtm AND Record_Status = 'A' AND Scheme_Seq = 1

	INSERT INTO @SubsidizeGroupClaim (Scheme_Code, Scheme_Seq)
	SELECT Scheme_Code, Scheme_Seq
	FROM SubsidizeGroupClaim WITH (NOLOCK)
	WHERE Record_Status = 'A'
	GROUP BY Scheme_Code, Scheme_Seq
	HAVING MIN(Claim_Period_From) <= @Report_Dtm

	INSERT INTO #tempAccount
	SELECT		
			CASE
						WHEN [SPI].Special_Acc_ID IS NOT NULL		THEN [SPI].Encrypt_Field1
						WHEN [PI].Voucher_Acc_ID IS NOT NULL		THEN [PI].Encrypt_Field1
						WHEN [TPI].Voucher_Acc_ID IS NOT NULL		THEN [TPI].Encrypt_Field1
			END AS Encrypt_Field1,
			CASE RTRIM(VT.Doc_Code) WHEN 'HKBC' THEN 'HKIC' ELSE VT.Doc_Code END AS Doc_Code,
			TD.scheme_Code,
			TD.scheme_Seq,
			TD.Subsidize_Item_Code
	FROM				VoucherTransaction VT  WITH (NOLOCK)
	INNER JOIN			TransactionDetail TD  WITH (NOLOCK) ON VT.Transaction_ID = TD.Transaction_ID
	LEFT JOIN			SpecialPersonalInformation [SPI]  WITH (NOLOCK) ON VT.Special_Acc_ID = [SPI].Special_Acc_ID AND VT.Doc_Code = [SPI].Doc_Code
	LEFT JOIN			PersonalInformation [PI]  WITH (NOLOCK) ON VT.Voucher_Acc_ID = [PI].Voucher_Acc_ID AND VT.Doc_Code = [PI].Doc_Code
	LEFT JOIN			TempPersonalInformation [TPI]  WITH (NOLOCK) ON VT.Temp_Voucher_Acc_ID = [TPI].Voucher_Acc_ID AND VT.Doc_Code = [TPI].Doc_Code
	WHERE VT.Record_Status NOT IN ('I','D') AND ISNULL(VT.Invalidation,'') <> 'I'
	GROUP BY CASE
						WHEN [SPI].Special_Acc_ID IS NOT NULL		THEN [SPI].Encrypt_Field1
						WHEN [PI].Voucher_Acc_ID IS NOT NULL		THEN [PI].Encrypt_Field1
						WHEN [TPI].Voucher_Acc_ID IS NOT NULL		THEN [TPI].Encrypt_Field1
			END, 
			CASE RTRIM(VT.Doc_Code) WHEN 'HKBC' THEN 'HKIC' ELSE VT.Doc_Code END,
			TD.scheme_Code,
			TD.scheme_Seq,
			TD.Subsidize_Item_Code

	--select a.Encrypt_Field1, rtrim(a.doc_code), rtrim(a.scheme_code), a.scheme_seq, rtrim(a.Subsidize_item_code), rtrim(a.Subsidize_type) from
	--(
	--SELECT p.Encrypt_Field1, case rtrim(p.doc_code) when 'HKBC' then 'HKIC' else p.doc_code end as doc_code,
	-- vt.Scheme_Code, td.Scheme_Seq,  td.Subsidize_Item_Code,  si.Subsidize_type
	--FROM   dbo.VoucherTransaction AS vt  WITH (NOLOCK) INNER JOIN
	--	   dbo.PersonalInformation AS p  WITH (NOLOCK) ON vt.Voucher_Acc_ID = p.Voucher_Acc_ID AND vt.Doc_Code = p.Doc_Code 
	--		INNER JOIN dbo.TransactionDetail AS td  WITH (NOLOCK) ON vt.Transaction_ID = td.Transaction_ID
	--		INNER JOIN SubsidizeItem si  WITH (NOLOCK) ON td.Subsidize_Item_Code = si.Subsidize_Item_Code
	--WHERE     (vt.Voucher_Acc_ID IS NOT NULL AND vt.Voucher_Acc_ID <> '') AND (vt.Record_Status NOT IN ('I', 'D', 'W')  AND (vt.Invalidation IS NULL OR vt.Invalidation NOT IN ('I')))
	--and vt.Transaction_Dtm < @Report_Dtm
	--UNION
	--SELECT  p.Encrypt_Field1, case rtrim(p.doc_code) when 'HKBC' then 'HKIC' else p.doc_code end as doc_code,
	-- vt.Scheme_Code, td.Scheme_Seq,  td.Subsidize_Item_Code,  si.Subsidize_type
	--FROM    dbo.VoucherTransaction AS vt  WITH (NOLOCK) INNER JOIN
	--		dbo.TempPersonalInformation AS p  WITH (NOLOCK) ON vt.Temp_Voucher_Acc_ID = p.Voucher_Acc_ID AND vt.Doc_Code = p.Doc_Code 
	--		INNER JOIN dbo.TransactionDetail AS td  WITH (NOLOCK) ON vt.Transaction_ID = td.Transaction_ID 
	--		INNER JOIN SubsidizeItem si  WITH (NOLOCK) ON td.Subsidize_Item_Code = si.Subsidize_Item_Code
	--WHERE  (vt.Record_Status NOT IN ('I', 'D', 'W')  AND (vt.Invalidation IS NULL OR vt.Invalidation NOT IN ('I')))
	--and  ( vt.temp_voucher_acc_id<>'' and (vt.voucher_acc_id is null or rtrim(ltrim(vt.voucher_acc_id)) = '')
	--and (vt.special_acc_id is null or rtrim(ltrim(vt.special_acc_id)) = ''))
	--and vt.Transaction_Dtm < @Report_Dtm
	--UNION
	--SELECT    p.Encrypt_Field1, case rtrim(p.doc_code) when 'HKBC' then 'HKIC' else p.doc_code end as doc_code,
	--		vt.Scheme_Code, td.Scheme_Seq,  td.Subsidize_Item_Code,  si.Subsidize_type
	--FROM    dbo.VoucherTransaction AS vt  WITH (NOLOCK) INNER JOIN
	--		dbo.SpecialPersonalInformation AS p  WITH (NOLOCK) ON vt.Special_Acc_ID = p.Special_Acc_ID AND vt.Doc_Code = p.Doc_Code 
	--		INNER JOIN dbo.TransactionDetail AS td  WITH (NOLOCK) ON vt.Transaction_ID = td.Transaction_ID
	--		INNER JOIN SubsidizeItem si  WITH (NOLOCK) ON td.Subsidize_Item_Code = si.Subsidize_Item_Code
	--WHERE   (vt.Record_Status NOT IN ('I', 'D', 'W')  AND (vt.Invalidation IS NULL OR vt.Invalidation NOT IN ('I')))  
	--and ((vt.voucher_acc_id is null or rtrim(ltrim(vt.voucher_acc_id)) = '')
	--and (vt.special_acc_id is not null and rtrim(ltrim(vt.special_acc_id)) <> ''))
	--and vt.Transaction_Dtm < @Report_Dtm

	--) as a
	--group by a.Encrypt_Field1, a.doc_code, a.scheme_code, a.scheme_seq, a.Subsidize_item_code, a.Subsidize_type



	-- Prepare Raw Transaction Summary
	INSERT INTO #tempTransaction  (
		Scheme_Code,
		--Scheme_Seq,
		Manual_Reimburse,
		Record_Status,
		[Invalidation],
		TxCount)
	SELECT 
		VT.Scheme_Code,
		--TD.Scheme_Seq,
		ISNULL(Manual_Reimburse, 'N') Manual_Reimburse,
		Record_Status,
		ISNULL(Invalidation,'') AS [Invalidation],
		COUNT(distinct VT.Transaction_ID) TxCount
	FROM VoucherTransaction VT WITH (NOLOCK)
		--INNER JOIN TransactionDetail TD WITH (NOLOCK)
		--ON	VT.Transaction_ID = TD.Transaction_ID
	WHERE Transaction_Dtm < @Report_Dtm
	GROUP BY
		VT.Scheme_Code,
		--TD.Scheme_Seq,
		ISNULL(Manual_Reimburse, 'N'),
		Record_Status,
		ISNULL(Invalidation,'')

	-- Fill Subsidize_Type
	UPDATE tx SET tx.Subsidize_Type = S.Subsidize_Type
	FROM #tempTransaction tx 
			INNER JOIN (select DISTINCT SC.Scheme_Code, SI.Subsidize_Type from SchemeClaim SC  WITH (NOLOCK) INNER JOIN SubsidizeGroupClaim SGC WITH (NOLOCK)
					ON SC.Scheme_Code = SGC.Scheme_Code
					INNER JOIN Subsidize S WITH (NOLOCK)
					ON SGC.Subsidize_Code = S.Subsidize_Code
					INNER JOIN SubsidizeItem SI WITH (NOLOCK)
					ON S.Subsidize_Item_Code = SI.Subsidize_Item_Code) S
			ON tx.Scheme_Code = S.Scheme_Code

	-- Prepare Transaction Count by Vaccination season and Vaccine
	INSERT INTO #tempTransactionVaccineSeason (
		TxCount,
		Subsidize_Item_Code,
		Season_Desc
	)
	SELECT COUNT(1) AS cnt, vd.subsidize_item_code, VS.Season_Desc
		FROM vouchertransaction vt WITH (NOLOCK),transactiondetail vd WITH (NOLOCK)  
			INNER JOIN subsidizeItem si WITH (NOLOCK)
				ON vd.Subsidize_item_Code = si.Subsidize_item_Code
			LEFT JOIN VaccineSeason VS WITH (NOLOCK)
				ON vd.scheme_code = VS.scheme_code and vd.scheme_seq = VS.scheme_seq and vd.subsidize_item_code = VS.subsidize_item_code
		 WHERE transaction_dtm<@Report_Dtm AND vt.Record_Status NOT IN ('I', 'D','W')   
				AND (Invalidation IS NULL OR Invalidation NOT IN ('I')) AND si.Subsidize_Type = 'VACCINE' 
				AND vt.transaction_id=vd.transaction_id AND vt.scheme_code=vd.scheme_code  
		 GROUP BY VS.Season_Desc, vd.subsidize_item_code  

-- =============================================
-- Retrieve data
-- =============================================
	SELECT @Report_Dtm = DATEADD(dd, -1, @Report_Dtm)
-- Content

	EXEC [proc_EHS_eHSD0021-Content] 

-- 01 for Total Summary of Transactions

	EXEC [proc_EHS_eHSD0021-01] @Report_Dtm
	
-- 02 for Transactions input by SP

	EXEC [proc_EHS_eHSD0021-02] @Report_Dtm

-- 03 for Transactions input by back office
	EXEC [proc_EHS_eHSD0021-03] @Report_Dtm
	
-- 04 Vaccine claim break down by scheme
	EXEC [proc_EHS_eHSD0021-04] @Report_Dtm

-- 05 Voucher claim break down by year
	EXEC [proc_EHS_eHSD0021-05] @Report_Dtm

-- 06 Vaccine claim break down by vaccination season
	EXEC [proc_EHS_eHSD0021-06] @Report_Dtm

-- 07 EHAPP claim break down by period
	EXEC [proc_EHS_eHSD0021-07] @Report_Dtm

-- Legend

	DECLARE @seq	int

	CREATE TABLE #Remark (
		Seq		smallint,
		Seq2	smallint,
		Col01	varchar(1000),
		Col02	varchar(1000)
	)

	SET @seq = 0

	INSERT INTO #Remark (Seq, Seq2, Col01, Col02) VALUES (@seq, NULL, '(A) Legend', '')

	SET @seq = @seq + 1

	INSERT INTO #Remark (Seq, Seq2, Col01, Col02) VALUES (@seq, NULL, '1. Scheme Name', '')
	
	SET @seq = @seq + 1

	INSERT INTO #Remark (Seq, Seq2, Col01, Col02)
	SELECT @seq, NULL, Display_Code, Scheme_Desc FROM @SchemeClaim

	SET @seq = @seq + 1

	INSERT INTO #Remark (Seq, Seq2, Col01, Col02) VALUES (@seq, NULL, '', '')

	SET @seq = @seq + 1

	INSERT INTO #Remark (Seq, Seq2, Col01, Col02) VALUES (@seq, NULL, '2. Vaccination Code', '')

	SET @seq = @seq + 1

	INSERT INTO #Remark (Seq, Seq2, Col01, Col02)
	SELECT @seq, NULL, VS.Subsidize_Item_Code, SI.Subsidize_Item_Desc
	FROM VaccineSeason VS
		INNER JOIN SubsidizeItem SI
			ON VS.Subsidize_Item_Code = SI.Subsidize_Item_Code
		INNER JOIN @SubsidizeGroupClaim SGC
			ON VS.Scheme_Code = SGC.Scheme_Code COLLATE DATABASE_DEFAULT AND VS.Scheme_Seq = SGC.Scheme_Seq
	GROUP BY VS.Subsidize_Item_Code, SI.Subsidize_Item_Desc

	SET @seq = @seq + 1

	INSERT INTO #Remark (Seq, Seq2, Col01, Col02) VALUES (@seq, NULL, '', '')

	SET @seq = @seq + 1

	INSERT INTO #Remark (Seq, Seq2, Col01, Col02) VALUES (@seq, NULL, '(B) Common Note(s) for the report', '')

	SET @seq = @seq + 1

	INSERT INTO #Remark (Seq, Seq2, Col01, Col02)
	VALUES (@seq, NULL, 'All claim transactions created under service providers (either created by back office users or service providers (or the delegated users))', '')

	SELECT Col01, Col02 FROM #Remark ORDER BY Seq, Seq2, Col01


	DROP TABLE #Remark


	DROP TABLE #tempAccount
	DROP TABLE #tempTransaction
	DROP TABLE #tempTransactionVaccineSeason
	set nocount off

END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0021] TO HCVU
GO
