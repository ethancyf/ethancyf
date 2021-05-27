IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0004_01_PrepareData]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0004_01_PrepareData]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Modified by:		Raiman Chong
-- Modified date:	04 May 2021
-- CR. No.:			CRE20-023
-- Description:		Add New Document Type CCIC, ROP140
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	16 Jul 2020
-- CR. No			INT20-0025
-- Description:		(1) Add WITH (NOLOCK)
-- =============================================
-- =============================================
-- Modified by:		Marco CHOI
-- Modified date:	11 Jan 2018
-- Description:		CRE16-004 Add Deceased Status
-- =============================================
-- =============================================
-- Author:			Marco CHOI
-- Create date:		25 Sep 2017
-- Description:		Add PCV13
--					SP rename from proc_EHS_eHealthAccountClaimByDocumentType_RVP_Stat
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSD0004_01_PrepareData]
	@Cutoff_Dtm	datetime
AS BEGIN

SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	CREATE TABLE #RVPTransaction (
		Voucher_Acc_ID		char(15),
		Temp_Voucher_Acc_ID	char(15),
		Special_Acc_ID		char(15),
		Invalid_Acc_ID		char(15),
		Doc_Code			char(20)
	)
	
	CREATE TABLE #Account (
		Doc_Code			char(20),
		Encrypt_Field1		varbinary(100),
		Deceased			bit
	)

	CREATE TABLE #DocTT (
		Doc_Code			char(20),
		Deceased			bit
	)

	CREATE TABLE #ResultCount (
		Doc_Code			char(20),
		Deceased			bit,
		TotalCount			int
	)
	
	CREATE TABLE #ResultTable (
		Result_Seq			smallint,
		Result_Value1		varchar(100) default '',
		Result_Value2		varchar(100) default '',
		Result_Value3		varchar(100) default '',
		Result_Value4		varchar(100) default '',
		Result_Value5		varchar(100) default '',
		Result_Value6		varchar(100) default '',
		Result_Value7		varchar(100) default '',
		Result_Value8		varchar(100) default '',
		Result_Value9		varchar(100) default '',
		Result_Value10		varchar(100) default '',
		Result_Value11		varchar(100) default '',
		Result_Value12		varchar(100) default '',
		Result_Value13		varchar(100) default '',
		Result_Value14		varchar(100) default '',
		Result_Value15		varchar(100) default '',
		Result_Value16		varchar(100) default '',
		Result_Value17		varchar(100) default '',
		Result_Value18		varchar(100) default '',
		Result_Value19		varchar(100) default '',
		Result_Value20		varchar(100) default ''
	)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

-- ---------------------------------------------
-- RVP transactions
-- ---------------------------------------------

	INSERT INTO #RVPTransaction (
		Voucher_Acc_ID,
		Temp_Voucher_Acc_ID,
		Special_Acc_ID,
		Invalid_Acc_ID,
		Doc_Code
	)
	SELECT
		ISNULL(Voucher_Acc_ID, ''),
		ISNULL(Temp_Voucher_Acc_ID, ''),
		ISNULL(Special_Acc_ID, ''),
		ISNULL(Invalid_Acc_ID, ''),
		Doc_Code
	FROM
		VoucherTransaction VT WITH (NOLOCK)
	WHERE
		Scheme_Code = 'RVP'
AND Transaction_Dtm <= @Cutoff_Dtm
AND VT.Record_Status NOT IN
	(SELECT Status_Value FROM StatStatusFilterMapping WITH (NOLOCK) WHERE (report_id = 'ALL' OR report_id = 'eHSD0004') 
		AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status' 
		AND ((Effective_Date is null or Effective_Date <= @cutoff_dtm) AND (Expiry_Date is null or @cutoff_dtm < Expiry_Date )))			
AND (VT.Invalidation IS NULL OR VT.Invalidation NOT In 
	(SELECT Status_Value FROM StatStatusFilterMapping WITH (NOLOCK) WHERE (report_id = 'ALL' OR report_id = 'eHSD0004') 
	AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'
	AND ((Effective_Date is null or Effective_Date <= @cutoff_dtm) AND (Expiry_Date is null or @cutoff_dtm < Expiry_Date))))


-- ---------------------------------------------
-- Validated accounts
-- ---------------------------------------------

	INSERT INTO #Account (
		Doc_Code,
		Encrypt_Field1,
		Deceased
	)
	SELECT
		VP.Doc_Code,
		VP.Encrypt_Field1,
		CASE WHEN VP.Deceased IS NULL THEN 0 ELSE
			CASE WHEN VP.Deceased = 'Y' THEN 1 ELSE 0 END
		END
	FROM
		#RVPTransaction VT
			INNER JOIN PersonalInformation VP WITH (NOLOCK)
				ON VT.Voucher_Acc_ID = VP.Voucher_Acc_ID
					AND VT.Doc_Code = VP.Doc_Code
	WHERE
		VT.Voucher_Acc_ID <> ''


-- ---------------------------------------------
-- Temporary accounts
-- ---------------------------------------------

	INSERT INTO #Account (
		Doc_Code,
		Encrypt_Field1,
		Deceased
	)
	SELECT
		TP.Doc_Code,
		TP.Encrypt_Field1,
		CASE WHEN TP.Deceased IS NULL THEN 0 ELSE
			CASE WHEN TP.Deceased = 'Y' THEN 1 ELSE 0 END
		END
	FROM
		#RVPTransaction VT
			INNER JOIN TempPersonalInformation TP WITH (NOLOCK)
				ON VT.Temp_Voucher_Acc_ID = TP.Voucher_Acc_ID
	WHERE
		VT.Voucher_Acc_ID = ''
			AND VT.Temp_Voucher_Acc_ID <> ''
			AND VT.Special_Acc_ID = ''


-- ---------------------------------------------
-- Special accounts
-- ---------------------------------------------

	INSERT INTO #Account (
		Doc_Code,
		Encrypt_Field1,
		Deceased
	)
	SELECT
		SP.Doc_Code,
		SP.Encrypt_Field1,
		CASE WHEN SP.Deceased IS NULL THEN 0 ELSE
			CASE WHEN SP.Deceased = 'Y' THEN 1 ELSE 0 END
		END
	FROM
		#RVPTransaction VT
			INNER JOIN SpecialPersonalInformation SP WITH (NOLOCK)
				ON VT.Special_Acc_ID = SP.Special_Acc_ID
	WHERE
		VT.Voucher_Acc_ID = ''
			AND VT.Special_Acc_ID <> ''
			AND VT.Invalid_Acc_ID = ''

	
-- =============================================
-- Return results
-- =============================================

-- ---------------------------------------------
-- Build format
-- ---------------------------------------------

	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
								Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12,Result_Value13, Result_Value14, Result_Value15, Result_Value16) VALUES
	(0, 'eHS(S)D0004-01: Report on eHealth (Subsidies) Accounts with RVP claim transactions by document type', 
		'', '', '', '', '', '','', '', '', '', '', '', '', '','')
				
	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
								Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12,Result_Value13, Result_Value14, Result_Value15, Result_Value16) VALUES
	(1, '', '', '', '', '', '', '', '', '', '', '', '', '', '', '','')
				
	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
								Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12,Result_Value13, Result_Value14, Result_Value15, Result_Value16) VALUES
	(2, 'Reporting period: as at ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111),
		'', '', '', '', '', '', '', '', '', '', '', '', '', '','')
			
	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
								Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12,Result_Value13, Result_Value14, Result_Value15, Result_Value16) VALUES
	(3, '', '', '', '', '', '', '', '', '', '', '', '', '', '', '','')
		
	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
								Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12,Result_Value13, Result_Value14, Result_Value15, Result_Value16) VALUES
	(10, '', 'HKIC/HKBC', 'Doc/I', 'REPMT', 'ID235B', 'VISA', 'ADOPC', 'EC','OW','CCIC','ROP140','PASS', 'Total', '', 'HKIC', 'HKBC')

	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
								Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12,Result_Value13, Result_Value14, Result_Value15, Result_Value16 ) VALUES
	(11, 'Alive', '', '', '', '', '', '', '', '', '', '', '', '', '', '','')
	
	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
								Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12,Result_Value13, Result_Value14, Result_Value15, Result_Value16 ) VALUES
	(12, 'Deceased', '', '', '', '', '', '', '', '', '', '', '', '', '', '','')
	
	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
								Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12,Result_Value13, Result_Value14, Result_Value15, Result_Value16 ) VALUES
	(13, 'Total', '', '', '', '', '', '', '', '', '', '', '', '', '', '','')


-- ---------------------------------------------
-- Build data
-- ---------------------------------------------
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'HKIC', 0
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'HKIC', 1
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'HKBC', 0
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'HKBC', 1
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'Doc/I', 0
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'Doc/I', 1
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'REPMT', 0
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'REPMT', 1
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'ID235B', 0
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'ID235B', 1
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'VISA', 0
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'VISA', 1
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'ADOPC', 0
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'ADOPC', 1
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'EC', 0
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'EC', 1
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'OW', 0
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'OW', 1
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'CCIC', 0
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'CCIC', 1
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'ROP140', 0
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'ROP140', 1
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'PASS', 0
	INSERT INTO #DocTT( Doc_Code, Deceased) SELECT 'PASS', 1


	INSERT INTO #ResultCount (Doc_Code, Deceased, TotalCount)
	SELECT d.Doc_Code, d.Deceased, isnull(a.TotalCount,0) TotalCount
	from #DocTT d
	LEFT JOIN (
		SELECT Doc_Code, Deceased, count(DISTINCT Encrypt_Field1) as TotalCount
		FROM #Account
		GROUP BY Doc_Code, Deceased
	) a	 
	ON d.Doc_Code = a.Doc_Code
	and d.Deceased = a.Deceased

	INSERT INTO #ResultCount (Doc_Code, Deceased, TotalCount)
	SELECT 'HKIC/HKBC', Deceased, count(DISTINCT Encrypt_Field1) as TotalCount
	FROM #Account
	WHERE Doc_Code = 'HKIC' OR Doc_Code = 'HKBC'
	GROUP BY Deceased
		
	INSERT INTO #ResultCount (Doc_Code, Deceased, TotalCount)
	SELECT 'Total', Deceased, count(DISTINCT Encrypt_Field1) as TotalCount
	FROM #Account
	GROUP BY Deceased

	--Alive
	UPDATE #ResultTable SET Result_Value2 =	 TotalCount FROM #ResultCount WHERE Result_Seq = 11 AND Deceased = 0 AND Doc_Code = 'HKIC/HKBC'
	UPDATE #ResultTable SET Result_Value3 =	 TotalCount	FROM #ResultCount WHERE Result_Seq = 11 AND Deceased = 0 AND Doc_Code = 'Doc/I' 					 
	UPDATE #ResultTable SET Result_Value4 =	 TotalCount	FROM #ResultCount WHERE Result_Seq = 11 AND Deceased = 0 AND Doc_Code = 'REPMT' 					 
	UPDATE #ResultTable SET Result_Value5 =	 TotalCount	FROM #ResultCount WHERE Result_Seq = 11 AND Deceased = 0 AND Doc_Code = 'ID235B' 				  
	UPDATE #ResultTable SET Result_Value6 =	 TotalCount	FROM #ResultCount WHERE Result_Seq = 11 AND Deceased = 0 AND Doc_Code = 'VISA' 					
	UPDATE #ResultTable SET Result_Value7 =	 TotalCount	FROM #ResultCount WHERE Result_Seq = 11 AND Deceased = 0 AND Doc_Code = 'ADOPC' 					
	UPDATE #ResultTable SET Result_Value8 =	 TotalCount	FROM #ResultCount WHERE Result_Seq = 11 AND Deceased = 0 AND Doc_Code = 'EC' 		
	UPDATE #ResultTable SET Result_Value9 =	 TotalCount	FROM #ResultCount WHERE Result_Seq = 11 AND Deceased = 0 AND Doc_Code = 'OW' 	
	UPDATE #ResultTable SET Result_Value10 = TotalCount	FROM #ResultCount WHERE Result_Seq = 11 AND Deceased = 0 AND Doc_Code = 'CCIC' 	
	UPDATE #ResultTable SET Result_Value11 = TotalCount	FROM #ResultCount WHERE Result_Seq = 11 AND Deceased = 0 AND Doc_Code = 'ROP140' 	
	UPDATE #ResultTable SET Result_Value12 = TotalCount	FROM #ResultCount WHERE Result_Seq = 11 AND Deceased = 0 AND Doc_Code = 'PASS' 	
	UPDATE #ResultTable SET Result_Value13 = TotalCount FROM #ResultCount WHERE Result_Seq = 11 AND Deceased = 0 AND Doc_Code = 'Total'  										  
	UPDATE #ResultTable SET Result_Value15 = TotalCount	FROM #ResultCount WHERE Result_Seq = 11 AND Deceased = 0 AND Doc_Code = 'HKIC' 					 
	UPDATE #ResultTable SET Result_Value16 = TotalCount	FROM #ResultCount WHERE Result_Seq = 11 AND Deceased = 0 AND Doc_Code = 'HKBC' 					 
	
	--Deceased
	UPDATE #ResultTable SET Result_Value2 =	 TotalCount FROM #ResultCount WHERE Result_Seq = 12 AND Deceased = 1 AND Doc_Code = 'HKIC/HKBC'
	UPDATE #ResultTable SET Result_Value3 =	 TotalCount	FROM #ResultCount WHERE Result_Seq = 12 AND Deceased = 1 AND Doc_Code = 'Doc/I' 					 
	UPDATE #ResultTable SET Result_Value4 =	 TotalCount	FROM #ResultCount WHERE Result_Seq = 12 AND Deceased = 1 AND Doc_Code = 'REPMT' 					 
	UPDATE #ResultTable SET Result_Value5 =	 TotalCount	FROM #ResultCount WHERE Result_Seq = 12 AND Deceased = 1 AND Doc_Code = 'ID235B' 				  
	UPDATE #ResultTable SET Result_Value6 =	 TotalCount	FROM #ResultCount WHERE Result_Seq = 12 AND Deceased = 1 AND Doc_Code = 'VISA' 					
	UPDATE #ResultTable SET Result_Value7 =	 TotalCount	FROM #ResultCount WHERE Result_Seq = 12 AND Deceased = 1 AND Doc_Code = 'ADOPC' 					
	UPDATE #ResultTable SET Result_Value8 =	 TotalCount	FROM #ResultCount WHERE Result_Seq = 12 AND Deceased = 1 AND Doc_Code = 'EC' 	
	UPDATE #ResultTable SET Result_Value9 =	 TotalCount	FROM #ResultCount WHERE Result_Seq = 12 AND Deceased = 1 AND Doc_Code = 'OW' 
	UPDATE #ResultTable SET Result_Value10 = TotalCount	FROM #ResultCount WHERE Result_Seq = 12 AND Deceased = 1 AND Doc_Code = 'CCIC' 
	UPDATE #ResultTable SET Result_Value11 = TotalCount	FROM #ResultCount WHERE Result_Seq = 12 AND Deceased = 1 AND Doc_Code = 'ROP140' 
	UPDATE #ResultTable SET Result_Value12 = TotalCount	FROM #ResultCount WHERE Result_Seq = 12 AND Deceased = 1 AND Doc_Code = 'PASS' 
	UPDATE #ResultTable SET Result_Value13 = TotalCount FROM #ResultCount WHERE Result_Seq = 12 AND Deceased = 1 AND Doc_Code = 'Total'  										  
	UPDATE #ResultTable SET Result_Value15 = TotalCount	FROM #ResultCount WHERE Result_Seq = 12 AND Deceased = 1 AND Doc_Code = 'HKIC' 					 
	UPDATE #ResultTable SET Result_Value16 = TotalCount	FROM #ResultCount WHERE Result_Seq = 12 AND Deceased = 1 AND Doc_Code = 'HKBC' 	
	
	--Total
	UPDATE #ResultTable SET Result_Value2 =	 (SELECT SUM(TotalCount) FROM #ResultCount WHERE Doc_Code = 'HKIC/HKBC'	GROUP BY Doc_Code) WHERE Result_Seq = 13
	UPDATE #ResultTable SET Result_Value3 =	 (SELECT SUM(TotalCount) FROM #ResultCount WHERE Doc_Code = 'Doc/I' 	GROUP BY Doc_Code) WHERE Result_Seq = 13				 
	UPDATE #ResultTable SET Result_Value4 =	 (SELECT SUM(TotalCount) FROM #ResultCount WHERE Doc_Code = 'REPMT' 	GROUP BY Doc_Code) WHERE Result_Seq = 13				 
	UPDATE #ResultTable SET Result_Value5 =	 (SELECT SUM(TotalCount) FROM #ResultCount WHERE Doc_Code = 'ID235B' 	GROUP BY Doc_Code) WHERE Result_Seq = 13			  
	UPDATE #ResultTable SET Result_Value6 =	 (SELECT SUM(TotalCount) FROM #ResultCount WHERE Doc_Code = 'VISA' 		GROUP BY Doc_Code) WHERE Result_Seq = 13			
	UPDATE #ResultTable SET Result_Value7 =	 (SELECT SUM(TotalCount) FROM #ResultCount WHERE Doc_Code = 'ADOPC' 	GROUP BY Doc_Code) WHERE Result_Seq = 13				
	UPDATE #ResultTable SET Result_Value8 =	 (SELECT SUM(TotalCount) FROM #ResultCount WHERE Doc_Code = 'EC' 		GROUP BY Doc_Code) WHERE Result_Seq = 13	
    UPDATE #ResultTable SET Result_Value9 =	 (SELECT SUM(TotalCount) FROM #ResultCount WHERE Doc_Code = 'OW' 		GROUP BY Doc_Code) WHERE Result_Seq = 13
	UPDATE #ResultTable SET Result_Value10 = (SELECT SUM(TotalCount) FROM #ResultCount WHERE Doc_Code = 'CCIC' 		GROUP BY Doc_Code) WHERE Result_Seq = 13	
	UPDATE #ResultTable SET Result_Value11 = (SELECT SUM(TotalCount) FROM #ResultCount WHERE Doc_Code = 'ROP140' 	GROUP BY Doc_Code) WHERE Result_Seq = 13	
	UPDATE #ResultTable SET Result_Value12 = (SELECT SUM(TotalCount) FROM #ResultCount WHERE Doc_Code = 'PASS' 		GROUP BY Doc_Code) WHERE Result_Seq = 13	
	UPDATE #ResultTable SET Result_Value13 = (SELECT SUM(TotalCount) FROM #ResultCount WHERE Doc_Code = 'Total'  	GROUP BY Doc_Code) WHERE Result_Seq = 13									  
	UPDATE #ResultTable SET Result_Value15 = (SELECT SUM(TotalCount) FROM #ResultCount WHERE Doc_Code = 'HKIC' 		GROUP BY Doc_Code) WHERE Result_Seq = 13			 
	UPDATE #ResultTable SET Result_Value16 = (SELECT SUM(TotalCount) FROM #ResultCount WHERE Doc_Code = 'HKBC' 		GROUP BY Doc_Code) WHERE Result_Seq = 13																																					 
		
		

-- ---------------------------------------------
-- Insert to statistics table
-- ---------------------------------------------
	
	DELETE FROM RpteHSD0004_01_eHA_RVP_Tx_ByDocType

	INSERT INTO RpteHSD0004_01_eHA_RVP_Tx_ByDocType (
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
		Result_Value20
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
		Result_Value20
	FROM
		#ResultTable
	ORDER BY
		Result_Seq


	DROP TABLE #RVPTransaction 
	DROP TABLE #Account 
	DROP TABLE #DocTT 
	DROP TABLE #ResultCount 
	DROP TABLE #ResultTable 

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0004_01_PrepareData] TO HCVU
GO
