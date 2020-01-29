IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0005_03_PrepareData]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0005_03_PrepareData]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
  
-- =============================================
-- Modification History
-- Modified by:		Dickson Law	
-- Modified date:	02 Jan 2018
-- CR No.:			CRE14-016
-- Description:		(1) Add Acccount Breakdown by Alive and Deceased	
-- =============================================
-- =============================================    
-- CR No.:			CRE16-002-04
-- Author:			Winnie SUEN
-- Create date:		30 Aug 2016
-- Description:		Retrieve eHealth Account created by DOC Type Statistic
--					Copy from proc_EHS_eHealthAccountByDocumentType_Stat
-- =============================================  


CREATE PROCEDURE [proc_EHS_eHSD0005_03_PrepareData] 
	@Cutoff_Dtm AS DATETIME
AS
BEGIN

	SET NOCOUNT ON;

	-- =============================================  
	-- Declaration  
	-- =============================================  
	DECLARE @system_Dtm AS DATETIME  
	DECLARE @Scheme_Code AS VARCHAR(10)
	DECLARE @Report_ID AS VARCHAR(30)  
	
	-- =============================================  
	-- Validation   
	-- =============================================  
	-- =============================================  
	-- Initialization  
	-- =============================================  
	SET @system_Dtm = GETDATE()
	SET @Scheme_Code = 'VSS'  
	SET @Report_ID = 'eHSD0005'  
	
	-- =============================================  
	-- Return results  
	-- =============================================  
	 CREATE TABLE #statNoOfAcc_byDoc (  
		_doc_code CHAR(20),
		_doc_ID VARBINARY(100),
		_Deceased CHAR(1)
		)  
		

	CREATE TABLE #result_table (
		_display_seq TINYINT,
		_countType VARCHAR(10),
		_result_value1 VARCHAR(200) DEFAULT '',
		_result_value2 VARCHAR(100) DEFAULT '',
		_result_value3 VARCHAR(100) DEFAULT '',
		_result_value4 VARCHAR(100) DEFAULT '',
		_result_value5 VARCHAR(100) DEFAULT '',
		_result_value6 VARCHAR(100) DEFAULT '',
		_result_value7 VARCHAR(100) DEFAULT '',
		_result_value8 VARCHAR(100) DEFAULT '',
		_result_value9 VARCHAR(100) DEFAULT '',
		_result_value10 VARCHAR(100) DEFAULT '',
		_result_value11 VARCHAR(100) DEFAULT ''
		)

	--=======================================
	-- (i) eHealth (Subsidies) accounts
	--=======================================
	
	INSERT INTO #result_table (_display_seq, _countType, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5,
		_result_value6, _result_value7, _result_value8, _result_value9, _result_value10, _result_value11)
	VALUES (13, '', 'HKIC/HKBC', 'Doc/I', 'REPMT', 'ID235B', 'VISA', 'ADOPC', 'EC', 'Total', '', 'HKIC', 'HKBC')
	
	INSERT INTO #result_table (_display_seq, _countType)
	VALUES (14 ,'Alive')

	INSERT INTO #result_table (_display_seq, _countType)
	VALUES (15 ,'Deceased')

	INSERT INTO #result_table (_display_seq, _countType)
	VALUES (16 ,'Total')

	-- --------------------------------------------- 
	-- Validated Account  
	-- ---------------------------------------------  
	INSERT INTO #statNoOfAcc_byDoc (
		_doc_code,
		_doc_ID,
		_Deceased
	)  
	SELECT  
		VP.Doc_Code,  
		VP.Encrypt_Field1,
		CASE WHEN VP.Deceased IS NULL THEN 'N' ELSE
			CASE WHEN VP.Deceased = 'Y' THEN 'Y' ELSE 'N' END
		END
	FROM  
		VoucherAccount VA  
		INNER JOIN PersonalInformation VP ON VA.Voucher_Acc_ID = VP.Voucher_Acc_ID  
	WHERE  
		VA.Effective_Dtm <= @Cutoff_Dtm  
   	
	-- ---------------------------------------------  
	-- Temporary Account  
	-- ---------------------------------------------  
  
	INSERT INTO #statNoOfAcc_byDoc (
		_doc_code,
		_doc_ID,
		_Deceased
	)  
	SELECT  
		TP.Doc_Code,  
		TP.Encrypt_Field1,
		CASE WHEN TP.Deceased IS NULL THEN 'N' ELSE
			CASE WHEN TP.Deceased = 'Y' THEN 'Y' ELSE 'N' END
		END
	FROM  
		TempVoucherAccount TA  
		INNER JOIN TempPersonalInformation TP ON TA.Voucher_Acc_ID = TP.Voucher_Acc_ID  
	WHERE  
		TA.Record_Status NOT IN ('V', 'D')  
		AND TA.Create_Dtm <= @Cutoff_Dtm  
  
	-- ---------------------------------------------  
	-- Special Account  
	-- ---------------------------------------------  
    
	INSERT INTO #statNoOfAcc_byDoc (
		_doc_code,
		_doc_ID,
		_Deceased
	)  
	SELECT  
		SP.Doc_Code,  
		SP.Encrypt_Field1,
		CASE WHEN SP.Deceased IS NULL THEN 'N' ELSE
			CASE WHEN SP.Deceased = 'Y' THEN 'Y' ELSE 'N' END
		END
	FROM  
		SpecialAccount SA  
		INNER JOIN SpecialPersonalInformation SP ON SA.Special_Acc_ID = SP.Special_Acc_ID     
	WHERE  
		SA.Record_Status NOT IN ('V', 'D')  
		AND SA.Create_Dtm <= @Cutoff_Dtm  
  
	
	UPDATE #result_table
	SET _result_value1 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code IN ('HKIC', 'HKBC') AND _Deceased ='N'),
	    _result_value2 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'Doc/I' AND _Deceased ='N'),
		_result_value3 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'REPMT' AND _Deceased ='N'),
		 _result_value4 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'ID235B' AND _Deceased ='N'),
		_result_value5 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'VISA' AND _Deceased ='N'),
		_result_value6 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'ADOPC' AND _Deceased ='N'),
		_result_value7 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'EC' AND _Deceased ='N'),
		_result_value9 = '',
		_result_value10 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'HKIC' AND _Deceased ='N'),
		_result_value11 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'HKBC' AND _Deceased ='N')
	WHERE _display_seq = 14

	UPDATE #result_table
	SET _result_value1 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code IN ('HKIC', 'HKBC') AND _Deceased ='Y'),
		_result_value2 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'Doc/I' AND _Deceased ='Y'),
		_result_value3 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'REPMT' AND _Deceased ='Y'),
		 _result_value4 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'ID235B' AND _Deceased ='Y'),
		_result_value5 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'VISA' AND _Deceased ='Y'),
		_result_value6 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'ADOPC' AND _Deceased ='Y'),
		_result_value7 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'EC' AND _Deceased ='Y'),
		_result_value9 = '',
		_result_value10 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'HKIC' AND _Deceased ='Y'),
		_result_value11 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'HKBC' AND _Deceased ='Y')
	WHERE _display_seq = 15

	UPDATE #result_table
	SET	_result_value1 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code IN ('HKIC', 'HKBC')),
		_result_value2 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'Doc/I'),
		_result_value3 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'REPMT'),
		_result_value4 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'ID235B'),
		_result_value5 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'VISA'),
		_result_value6 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'ADOPC'),
		_result_value7 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'EC'),
		_result_value9 = '',
		_result_value10 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'HKIC'),
		_result_value11 = (
			SELECT count(DISTINCT _doc_ID)
			FROM #statNoOfAcc_byDoc
			WHERE _doc_code = 'HKBC')
	WHERE _display_seq = 16

	UPDATE #result_table
	SET _result_value8 = (CONVERT(INT, _result_value1) + CONVERT(INT, _result_value2) + CONVERT(INT, _result_value3) + CONVERT(INT, _result_value4) + CONVERT(INT, _result_value5) + CONVERT(INT, _result_value6) + CONVERT(INT, _result_value7))
	WHERE _display_seq = 14
	UPDATE #result_table
	SET _result_value8 = (CONVERT(INT, _result_value1) + CONVERT(INT, _result_value2) + CONVERT(INT, _result_value3) + CONVERT(INT, _result_value4) + CONVERT(INT, _result_value5) + CONVERT(INT, _result_value6) + CONVERT(INT, _result_value7))
	WHERE _display_seq = 15
	UPDATE #result_table
	SET _result_value8 = (CONVERT(INT, _result_value1) + CONVERT(INT, _result_value2) + CONVERT(INT, _result_value3) + CONVERT(INT, _result_value4) + CONVERT(INT, _result_value5) + CONVERT(INT, _result_value6) + CONVERT(INT, _result_value7))
	WHERE _display_seq = 16
	
	--===================================
	-- Retrieve the final result  
	--===================================
	DELETE FROM RpteHSD0005_03_eHA_ByDocType

	INSERT INTO RpteHSD0005_03_eHA_ByDocType (
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
		Col10,
		Col11,
		CountType
		)
	SELECT _display_Seq,
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
		_countType
	FROM #result_table
	ORDER BY _display_seq

	DROP TABLE #statNoOfAcc_byDoc

	DROP TABLE #result_table
	
END
GO


GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0005_03_PrepareData] TO HCVU
GO


