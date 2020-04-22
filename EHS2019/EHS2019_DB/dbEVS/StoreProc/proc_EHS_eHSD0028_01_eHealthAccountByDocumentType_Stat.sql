IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0028_01_eHealthAccountByDocumentType_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0028_01_eHealthAccountByDocumentType_Stat]
GO

-- SET ANSI_NULLS ON
-- SET QUOTED_IDENTIFIER ON
-- GO

-- =============================================
-- Modification History
-- CR No.			CRE16-026-03 (Add PCV13)
-- Modified by:		Lawrence TSANG
-- Modified date:	17 October 2017
-- Description:		Stored procedure not used anymore
-- =============================================  
-- =============================================    
-- CR No.:			CRE16-002-04
-- Author:			Winnie SUEN
-- Create date:		30 Aug 2016
-- Description:		Retrieve eHealth Account created by DOC Type Statistic
--					Copy from proc_EHS_eHealthAccountByDocumentType_Stat
-- =============================================  


-- CREATE PROCEDURE [proc_EHS_eHSD0028_01_eHealthAccountByDocumentType_Stat] 
	-- @Cutoff_Dtm AS DATETIME
-- AS
-- BEGIN
	
	-- SET NOCOUNT ON;

	-- -- =============================================  
	-- -- Declaration  
	-- -- =============================================  
	-- DECLARE @system_Dtm AS DATETIME  
	-- DECLARE @Scheme_Code AS VARCHAR(10)
	-- DECLARE @Report_ID AS VARCHAR(30)  
	
	-- -- =============================================  
	-- -- Validation   
	-- -- =============================================  
	-- -- =============================================  
	-- -- Initialization  
	-- -- =============================================  
	-- SET @system_Dtm = GETDATE()
	-- SET @Scheme_Code = 'VSS'  
	-- SET @Report_ID = 'eHSD0028'  
	
	-- -- =============================================  
	-- -- Return results  
	-- -- =============================================  
	 -- CREATE TABLE #statNoOfAcc_byDoc (  
		-- _doc_code CHAR(20),
		-- _doc_ID VARBINARY(100)  
		-- )  
 	
	-- CREATE TABLE #statNoOfAcc_withClaim_byDoc (
		-- _Voucher_acc_ID CHAR(15),
		-- _temp_voucher_acc_ID CHAR(15),
		-- _special_acc_ID CHAR(15),
		-- _doc_code CHAR(20),
		-- _doc_ID VARBINARY(100)
		-- )

	-- CREATE TABLE #statNoOfAcc_withClaim_byDoc_distinct (
		-- _doc_code CHAR(20),
		-- _doc_ID VARBINARY(100)
		-- )
		

	-- CREATE TABLE #result_table (
		-- _display_seq TINYINT,
		-- _result_value1 VARCHAR(200) DEFAULT '',
		-- _result_value2 VARCHAR(100) DEFAULT '',
		-- _result_value3 VARCHAR(100) DEFAULT '',
		-- _result_value4 VARCHAR(100) DEFAULT '',
		-- _result_value5 VARCHAR(100) DEFAULT '',
		-- _result_value6 VARCHAR(100) DEFAULT '',
		-- _result_value7 VARCHAR(100) DEFAULT '',
		-- _result_value8 VARCHAR(100) DEFAULT '',
		-- _result_value9 VARCHAR(100) DEFAULT '',
		-- _result_value10 VARCHAR(100) DEFAULT '',
		-- _result_value11 VARCHAR(100) DEFAULT ''
		-- )

	-- -- insert record for the final output format  
	-- INSERT INTO #result_table (_display_seq, _result_value1)
	-- VALUES (0, 'eHS(S)D0028-01: Report on eHealth (Subsidies) accounts created (by document type)')

	-- INSERT INTO #result_table (_display_seq)
	-- VALUES (1)

	-- INSERT INTO #result_table (_display_seq, _result_value1)
	-- VALUES (2, 'Reporting period: as at ' + CONVERT(VARCHAR, DATEADD(dd, - 1, @Cutoff_Dtm), 111))

	-- INSERT INTO #result_table (_display_seq)
	-- VALUES (3)

	-- --=======================================
	-- -- (i) eHealth (Subsidies) accounts
	-- --=======================================

	-- INSERT INTO #result_table (_display_seq, _result_value1)
	-- VALUES (11, '(i) eHealth (Subsidies) accounts')

	-- INSERT INTO #result_table (_display_seq)
	-- VALUES (12)
	
	-- INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5,
		-- _result_value6, _result_value7, _result_value8, _result_value9, _result_value10, _result_value11)
	-- VALUES (13, 'HKIC/HKBC', 'Doc/I', 'REPMT', 'ID235B', 'VISA', 'ADOPC', 'EC', 'Total', '', 'HKIC', 'HKBC')
	
	-- INSERT INTO #result_table (_display_seq)
	-- VALUES (14)

	-- -- --------------------------------------------- 
	-- -- Validated Account  
	-- -- ---------------------------------------------  
	-- INSERT INTO #statNoOfAcc_byDoc (
		-- _doc_code,
		-- _doc_ID
	-- )  
	-- SELECT  
		-- VP.Doc_Code,  
		-- VP.Encrypt_Field1  
	-- FROM  
		-- VoucherAccount VA  
		-- INNER JOIN PersonalInformation VP ON VA.Voucher_Acc_ID = VP.Voucher_Acc_ID  
	-- WHERE  
		-- VA.Effective_Dtm <= @Cutoff_Dtm  
   
	-- -- ---------------------------------------------  
	-- -- Temporary Account  
	-- -- ---------------------------------------------  
  
	-- INSERT INTO #statNoOfAcc_byDoc (
		-- _doc_code,
		-- _doc_ID
	-- )  
	-- SELECT  
		-- TP.Doc_Code,  
		-- TP.Encrypt_Field1  
	-- FROM  
		-- TempVoucherAccount TA  
		-- INNER JOIN TempPersonalInformation TP ON TA.Voucher_Acc_ID = TP.Voucher_Acc_ID  
	-- WHERE  
		-- TA.Record_Status NOT IN ('V', 'D')  
		-- AND TA.Create_Dtm <= @Cutoff_Dtm  
  
	-- -- ---------------------------------------------  
	-- -- Special Account  
	-- -- ---------------------------------------------  
    
	-- INSERT INTO #statNoOfAcc_byDoc (
		-- _doc_code,
		-- _doc_ID
	-- )  
	-- SELECT  
		-- SP.Doc_Code,  
		-- SP.Encrypt_Field1  
	-- FROM  
		-- SpecialAccount SA  
		-- INNER JOIN SpecialPersonalInformation SP ON SA.Special_Acc_ID = SP.Special_Acc_ID     
	-- WHERE  
		-- SA.Record_Status NOT IN ('V', 'D')  
		-- AND SA.Create_Dtm <= @Cutoff_Dtm  
  

     
	-- UPDATE #result_table
	-- SET _result_value1 = (
			-- SELECT count(DISTINCT _doc_ID)
			-- FROM #statNoOfAcc_byDoc
			-- WHERE _doc_code IN ('HKIC', 'HKBC'))
	-- WHERE _display_seq = 14

	-- UPDATE #result_table
	-- SET _result_value2 = (
			-- SELECT count(DISTINCT _doc_ID)
			-- FROM #statNoOfAcc_byDoc
			-- WHERE _doc_code = 'Doc/I')
	-- WHERE _display_seq = 14

	-- UPDATE #result_table
	-- SET _result_value3 = (
			-- SELECT count(DISTINCT _doc_ID)
			-- FROM #statNoOfAcc_byDoc
			-- WHERE _doc_code = 'REPMT')
	-- WHERE _display_seq = 14

	-- UPDATE #result_table
	-- SET _result_value4 = (
			-- SELECT count(DISTINCT _doc_ID)
			-- FROM #statNoOfAcc_byDoc
			-- WHERE _doc_code = 'ID235B')
	-- WHERE _display_seq = 14

	-- UPDATE #result_table
	-- SET _result_value5 = (
			-- SELECT count(DISTINCT _doc_ID)
			-- FROM #statNoOfAcc_byDoc
			-- WHERE _doc_code = 'VISA')
	-- WHERE _display_seq = 14

	-- UPDATE #result_table
	-- SET _result_value6 = (
			-- SELECT count(DISTINCT _doc_ID)
			-- FROM #statNoOfAcc_byDoc
			-- WHERE _doc_code = 'ADOPC')
	-- WHERE _display_seq = 14

	-- UPDATE #result_table
	-- SET _result_value7 = (
			-- SELECT count(DISTINCT _doc_ID)
			-- FROM #statNoOfAcc_byDoc
			-- WHERE _doc_code = 'EC')
	-- WHERE _display_seq = 14

	-- UPDATE #result_table
	-- SET _result_value8 = (CONVERT(INT, _result_value1) + CONVERT(INT, _result_value2) + CONVERT(INT, _result_value3) + CONVERT(INT, _result_value4) + CONVERT(INT, _result_value5) + CONVERT(INT, _result_value6) + CONVERT(INT, _result_value7))
	-- WHERE _display_seq = 14

	-- UPDATE #result_table
	-- SET _result_value9 = ''
	-- WHERE _display_seq = 14

	-- UPDATE #result_table
	-- SET _result_value10 = (
			-- SELECT count(DISTINCT _doc_ID)
			-- FROM #statNoOfAcc_byDoc
			-- WHERE _doc_code = 'HKIC')
	-- WHERE _display_seq = 14

	-- UPDATE #result_table
	-- SET _result_value11 = (
			-- SELECT count(DISTINCT _doc_ID)
			-- FROM #statNoOfAcc_byDoc
			-- WHERE _doc_code = 'HKBC')
	-- WHERE _display_seq = 14
	
  
  
  
	-- --=======================================
	-- -- (ii) eHealth (Subsidies) accounts with VSS claim transactions
	-- --=======================================
	
	-- INSERT INTO #result_table (_display_seq)
	-- VALUES (20)
			
	-- INSERT INTO #result_table (_display_seq, _result_value1)
	-- VALUES (21, '(ii) eHealth (Subsidies) accounts with VSS claim transactions since 20 Oct 2016')

	-- INSERT INTO #result_table (_display_seq)
	-- VALUES (22)
	
	-- INSERT INTO #result_table (_display_seq, _result_value1, _result_value2, _result_value3, _result_value4, _result_value5,
		-- _result_value6, _result_value7, _result_value8, _result_value9, _result_value10, _result_value11)
	-- VALUES (23, 'HKIC/HKBC', 'Doc/I', 'REPMT', 'ID235B', 'VISA', 'ADOPC', 'EC', 'Total', '', 'HKIC', 'HKBC')

	-- INSERT INTO #result_table (_display_seq)
	-- VALUES (24)

	-- -- select the voucher transactions included for statistic  
	-- INSERT INTO #statNoOfAcc_withClaim_byDoc (
		-- _Voucher_acc_ID,
		-- _temp_voucher_acc_ID,
		-- _special_acc_ID,
		-- _doc_code
		-- )
		-- SELECT vTran.Voucher_acc_ID,
		-- vTran.temp_voucher_acc_ID,
		-- vTran.special_acc_ID,
		-- vTran.doc_code
		-- FROM voucherTransaction vTran 
		-- WHERE vTran.Scheme_Code = @Scheme_Code
		-- AND vTran.transaction_dtm <= @Cutoff_Dtm
		-- AND (
			-- vTran.Invalidation IS NULL
			-- OR vTran.Invalidation NOT IN (
				-- SELECT Status_Value
				-- FROM StatStatusFilterMapping
				-- WHERE ( report_id = 'ALL' OR report_id = @Report_ID)
					-- AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'
					-- AND (( Effective_Date IS NULL OR Effective_Date <= @cutoff_dtm)
						-- AND ( Expiry_Date IS NULL OR Expiry_Date >= @cutoff_dtm ))
				-- )
			-- )
		-- AND vTran.Record_Status NOT IN (
			-- SELECT Status_Value
			-- FROM StatStatusFilterMapping
			-- WHERE ( report_id = 'ALL'OR report_id = @Report_ID)
				-- AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'
				-- AND (( Effective_Date IS NULL OR Effective_Date <= @cutoff_dtm)
					-- AND ( Expiry_Date IS NULL OR Expiry_Date >= @cutoff_dtm))
			-- )



	-- -- update the DOC ID for validated ACC  
	-- UPDATE #statNoOfAcc_withClaim_byDoc
	-- SET _doc_ID = (
			-- SELECT pInfo.[Encrypt_Field1]
			-- FROM personalInformation pInfo,
				-- voucherAccount vACC
			-- WHERE pInfo.voucher_acc_ID COLLATE DATABASE_DEFAULT = #statNoOfAcc_withClaim_byDoc._Voucher_acc_ID COLLATE DATABASE_DEFAULT
				-- AND pInfo.Doc_Code COLLATE DATABASE_DEFAULT = #statNoOfAcc_withClaim_byDoc._Doc_Code COLLATE DATABASE_DEFAULT
				-- AND vAcc.voucher_acc_ID = pInfo.voucher_acc_ID
				-- AND vAcc.record_status <> 'D'
			-- )
	-- WHERE #statNoOfAcc_withClaim_byDoc._doc_ID IS NULL OR #statNoOfAcc_withClaim_byDoc._doc_ID = ''


	-- -- update the DOC ID for special ACC  
	-- UPDATE #statNoOfAcc_withClaim_byDoc
	-- SET _doc_ID = (
			-- SELECT sInfo.[Encrypt_Field1]
			-- FROM specialpersonalinformation sInfo,
				-- specialaccount sAcc
			-- WHERE sInfo.special_acc_ID COLLATE DATABASE_DEFAULT = #statNoOfAcc_withClaim_byDoc._special_acc_ID COLLATE DATABASE_DEFAULT
				-- AND sInfo.Doc_Code COLLATE DATABASE_DEFAULT = #statNoOfAcc_withClaim_byDoc._Doc_Code COLLATE DATABASE_DEFAULT
				-- AND sAcc.special_acc_ID = sInfo.special_acc_ID
				-- AND sAcc.record_status NOT IN ('V', 'D')
			-- )
	-- WHERE #statNoOfAcc_withClaim_byDoc._doc_ID IS NULL OR #statNoOfAcc_withClaim_byDoc._doc_ID = ''


	-- -- update the DOC ID for temp ACC  
	-- UPDATE #statNoOfAcc_withClaim_byDoc
	-- SET _doc_ID = (
			-- SELECT tInfo.[Encrypt_Field1]
			-- FROM temppersonalinformation tInfo,
				-- tempvoucheraccount tAcc
			-- WHERE tInfo.voucher_acc_ID COLLATE DATABASE_DEFAULT = #statNoOfAcc_withClaim_byDoc._temp_voucher_acc_ID COLLATE DATABASE_DEFAULT
				-- AND tInfo.Doc_Code COLLATE DATABASE_DEFAULT = #statNoOfAcc_withClaim_byDoc._Doc_Code COLLATE DATABASE_DEFAULT
				-- AND tAcc.voucher_acc_ID = tInfo.voucher_acc_ID
				-- AND tAcc.record_status NOT IN ('V', 'D')
			-- )
	-- WHERE #statNoOfAcc_withClaim_byDoc._doc_ID IS NULL OR #statNoOfAcc_withClaim_byDoc._doc_ID = ''


	-- INSERT INTO #statNoOfAcc_withClaim_byDoc_distinct (_doc_code, _doc_ID)
	-- SELECT DISTINCT _doc_code, _doc_ID 
	-- FROM #statNoOfAcc_withClaim_byDoc 
	-- WHERE _doc_ID IS NOT NULL AND _doc_ID <> ''


	-- UPDATE #result_table
	-- SET _result_value1 = (
			-- SELECT count(DISTINCT _doc_ID)
			-- FROM #statNoOfAcc_withClaim_byDoc_distinct
			-- WHERE _doc_code IN ('HKIC', 'HKBC'))
	-- WHERE _display_seq = 24

	-- UPDATE #result_table
	-- SET _result_value2 = (
			-- SELECT count(1)
			-- FROM #statNoOfAcc_withClaim_byDoc_distinct
			-- WHERE _doc_code = 'Doc/I')
	-- WHERE _display_seq = 24

	-- UPDATE #result_table
	-- SET _result_value3 = (
			-- SELECT count(1)
			-- FROM #statNoOfAcc_withClaim_byDoc_distinct
			-- WHERE _doc_code = 'REPMT')
	-- WHERE _display_seq = 24

	-- UPDATE #result_table
	-- SET _result_value4 = (
			-- SELECT count(1)
			-- FROM #statNoOfAcc_withClaim_byDoc_distinct
			-- WHERE _doc_code = 'ID235B')
	-- WHERE _display_seq = 24

	-- UPDATE #result_table
	-- SET _result_value5 = (
			-- SELECT count(1)
			-- FROM #statNoOfAcc_withClaim_byDoc_distinct
			-- WHERE _doc_code = 'VISA')
	-- WHERE _display_seq = 24

	-- UPDATE #result_table
	-- SET _result_value6 = (
			-- SELECT count(1)
			-- FROM #statNoOfAcc_withClaim_byDoc_distinct
			-- WHERE _doc_code = 'ADOPC')
	-- WHERE _display_seq = 24

	-- UPDATE #result_table
	-- SET _result_value7 = (
			-- SELECT count(1)
			-- FROM #statNoOfAcc_withClaim_byDoc_distinct
			-- WHERE _doc_code = 'EC')
	-- WHERE _display_seq = 24

	-- UPDATE #result_table
	-- SET _result_value8 = (CONVERT(INT, _result_value1) + CONVERT(INT, _result_value2) + CONVERT(INT, _result_value3) + CONVERT(INT, _result_value4) + CONVERT(INT, _result_value5) + CONVERT(INT, _result_value6) + CONVERT(INT, _result_value7))
	-- WHERE _display_seq = 24

	-- UPDATE #result_table
	-- SET _result_value9 = ''
	-- WHERE _display_seq = 24

	-- UPDATE #result_table
	-- SET _result_value10 = (
			-- SELECT count(1)
			-- FROM #statNoOfAcc_withClaim_byDoc_distinct
			-- WHERE _doc_code = 'HKIC')
	-- WHERE _display_seq = 24

	-- UPDATE #result_table
	-- SET _result_value11 = (
			-- SELECT count(1)
			-- FROM #statNoOfAcc_withClaim_byDoc_distinct
			-- WHERE _doc_code = 'HKBC')
	-- WHERE _display_seq = 24

		
	-- --===================================
	-- -- Retrieve the final result  
	-- --===================================
	-- DELETE
	-- FROM RpteHSD0028eHealthAccountByDocumentTypeStat

	-- INSERT INTO RpteHSD0028eHealthAccountByDocumentTypeStat (
		-- Display_Seq,
		-- Col1,
		-- Col2,
		-- Col3,
		-- Col4,
		-- Col5,
		-- Col6,
		-- Col7,
		-- Col8,
		-- Col9,
		-- Col10,
		-- Col11
		-- )
	-- SELECT _display_Seq,
		-- _result_value1,
		-- _result_value2,
		-- _result_value3,
		-- _result_value4,
		-- _result_value5,
		-- _result_value6,
		-- _result_value7,
		-- _result_value8,
		-- _result_value9,
		-- _result_value10,
		-- _result_value11
	-- FROM #result_table
	-- ORDER BY _display_seq


	-- DROP TABLE #statNoOfAcc_byDoc
	
	-- DROP TABLE #statNoOfAcc_withClaim_byDoc

	-- DROP TABLE #statNoOfAcc_withClaim_byDoc_distinct

	-- DROP TABLE #result_table
	
-- END
-- GO


-- GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0028_01_eHealthAccountByDocumentType_Stat] TO HCVU
-- GO


