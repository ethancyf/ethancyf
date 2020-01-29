IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHealthAccountClaimByDocumentType_RVP_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHealthAccountClaimByDocumentType_RVP_Stat]
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
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	2 November 2015
-- CR No.:			CRE15-006
-- Description:		Rename of eHS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Eric Tse
-- Modified date:	21 October 2010
-- Description:		Modify the layout for fit with new report standard
--					(1) Filter invalidated transaction
--					(2) Modify header
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Derek LEUNG
-- Modified date:	15 September 2010
-- Description:		Include report ID in 1st line - update to new report standard
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	8 January 2010
-- Description:		Read for other documents (HKBC, Doc/I, REPMT, ID235B, VISA, ADOPC, EC)
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		21 October 2009
-- Description:		Retrieve eHealth account with claim by document type for scheme RVP
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

-- Create PROCEDURE [dbo].[proc_EHS_eHealthAccountClaimByDocumentType_RVP_Stat]
	-- @Cutoff_Dtm	datetime
-- AS BEGIN
	-- SET NOCOUNT ON;
-- -- =============================================
-- -- Declaration
-- -- =============================================
	-- DECLARE @RVPTransaction table (
		-- Voucher_Acc_ID		char(15),
		-- Temp_Voucher_Acc_ID	char(15),
		-- Special_Acc_ID		char(15),
		-- Invalid_Acc_ID		char(15),
		-- Doc_Code			char(20)
	-- )
	
	-- DECLARE @Account table (
		-- Doc_Code			char(20),
		-- Encrypt_Field1		varbinary(100)
	-- )
	
	-- DECLARE @ResultTable table(
		-- Result_Seq			smallint,
		-- Result_Value1		varchar(100),
		-- Result_Value2		varchar(100),
		-- Result_Value3		varchar(100),
		-- Result_Value4		varchar(100),
		-- Result_Value5		varchar(100),
		-- Result_Value6		varchar(100),
		-- Result_Value7		varchar(100),
		-- Result_Value8		varchar(100),
		-- Result_Value9		varchar(100),
		-- Result_Value10		varchar(100),
		-- Result_Value11		varchar(100)
	-- )
-- -- =============================================
-- -- Validation 
-- -- =============================================
-- -- =============================================
-- -- Initialization
-- -- =============================================

-- -- ---------------------------------------------
-- -- RVP transactions
-- -- ---------------------------------------------

	-- INSERT INTO @RVPTransaction (
		-- Voucher_Acc_ID,
		-- Temp_Voucher_Acc_ID,
		-- Special_Acc_ID,
		-- Invalid_Acc_ID,
		-- Doc_Code
	-- )
	-- SELECT
		-- ISNULL(Voucher_Acc_ID, ''),
		-- ISNULL(Temp_Voucher_Acc_ID, ''),
		-- ISNULL(Special_Acc_ID, ''),
		-- ISNULL(Invalid_Acc_ID, ''),
		-- Doc_Code
	-- FROM
		-- VoucherTransaction VT
	-- WHERE
		-- Scheme_Code = 'RVP'
-- AND Transaction_Dtm <= @Cutoff_Dtm
-- AND VT.Record_Status NOT IN
	-- (SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = 'eHSD0004') 
		-- AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status' 
		-- AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm)))			
-- AND (VT.Invalidation IS NULL OR VT.Invalidation NOT In 
	-- (SELECT Status_Value FROM StatStatusFilterMapping WHERE (report_id = 'ALL' OR report_id = 'eHSD0004') 
	-- AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'
	-- AND ((Effective_Date is null or Effective_Date>= @cutoff_dtm) AND (Expiry_Date is null or Expiry_Date < @cutoff_dtm))))


-- -- ---------------------------------------------
-- -- Validated accounts
-- -- ---------------------------------------------

	-- INSERT INTO @Account (
		-- Doc_Code,
		-- Encrypt_Field1
	-- )
	-- SELECT
		-- VP.Doc_Code,
		-- VP.Encrypt_Field1
	-- FROM
		-- @RVPTransaction VT
			-- INNER JOIN PersonalInformation VP
				-- ON VT.Voucher_Acc_ID = VP.Voucher_Acc_ID
					-- AND VT.Doc_Code = VP.Doc_Code
	-- WHERE
		-- VT.Voucher_Acc_ID <> ''


-- -- ---------------------------------------------
-- -- Temporary accounts
-- -- ---------------------------------------------

	-- INSERT INTO @Account (
		-- Doc_Code,
		-- Encrypt_Field1
	-- )
	-- SELECT
		-- TP.Doc_Code,
		-- TP.Encrypt_Field1
	-- FROM
		-- @RVPTransaction VT
			-- INNER JOIN TempPersonalInformation TP
				-- ON VT.Temp_Voucher_Acc_ID = TP.Voucher_Acc_ID
	-- WHERE
		-- VT.Voucher_Acc_ID = ''
			-- AND VT.Temp_Voucher_Acc_ID <> ''
			-- AND VT.Special_Acc_ID = ''


-- -- ---------------------------------------------
-- -- Special accounts
-- -- ---------------------------------------------

	-- INSERT INTO @Account (
		-- Doc_Code,
		-- Encrypt_Field1
	-- )
	-- SELECT
		-- SP.Doc_Code,
		-- SP.Encrypt_Field1
	-- FROM
		-- @RVPTransaction VT
			-- INNER JOIN SpecialPersonalInformation SP
				-- ON VT.Special_Acc_ID = SP.Special_Acc_ID
	-- WHERE
		-- VT.Voucher_Acc_ID = ''
			-- AND VT.Special_Acc_ID <> ''
			-- AND VT.Invalid_Acc_ID = ''

	
-- -- =============================================
-- -- Return results
-- -- =============================================

-- -- ---------------------------------------------
-- -- Build format
-- -- ---------------------------------------------

	-- INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
								-- Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	-- (0, 'eHS(S)D0004-01: Report on eHealth (Subsidies) Accounts with RVP claim transactions by document type', '', '', '', '', '', '',
								-- '', '', '', '')
				
	-- INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
								-- Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	-- (1, '', '', '', '', '', '', '',
								-- '', '', '', '')
				
	-- INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
								-- Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	-- (2, 'Reporting period: as at ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111),'','','','','','','','','','')
			
	-- INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
								-- Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	-- (3, '', '', '', '', '', '', '',
								-- '', '', '', '')
	
	-- INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
								-- Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	-- (4, 'RVP', '', '', '', '', '', '', 
								-- 'Total', '', 'RVP', '')
		
	-- INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
								-- Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	-- (10, 'HKIC/HKBC', 'Doc/I', 'REPMT', 'ID235B', 'VISA', 'ADOPC', 'EC',
								-- '', '', 'HKIC', 'HKBC')

	-- INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, 
								-- Result_Value8, Result_Value9, Result_Value10, Result_Value11) VALUES
	-- (11, '', '', '', '', '', '', '',
								-- '', '', '', '')


-- -- ---------------------------------------------
-- -- Build data
-- -- ---------------------------------------------
	
	-- UPDATE
		-- @ResultTable
	-- SET
		-- Result_Value1 =
			-- (
			-- SELECT
				-- COUNT(DISTINCT Encrypt_Field1)
			-- FROM
				-- @Account
			-- WHERE 
				-- Doc_Code = 'HKIC'
					-- OR Doc_Code = 'HKBC'
			-- )
	-- WHERE
		-- Result_Seq = 11
			
	-- UPDATE
		-- @ResultTable
	-- SET
		-- Result_Value2 =
			-- (
			-- SELECT
				-- COUNT(DISTINCT Encrypt_Field1)
			-- FROM
				-- @Account
			-- WHERE 
				-- Doc_Code = 'Doc/I'
			-- )
	-- WHERE
		-- Result_Seq = 11

	-- UPDATE
		-- @ResultTable
	-- SET
		-- Result_Value3 =
			-- (
			-- SELECT
				-- COUNT(DISTINCT Encrypt_Field1)
			-- FROM
				-- @Account
			-- WHERE 
				-- Doc_Code = 'REPMT'
			-- )
	-- WHERE
		-- Result_Seq = 11
		
	-- UPDATE
		-- @ResultTable
	-- SET
		-- Result_Value4 =
			-- (
			-- SELECT
				-- COUNT(DISTINCT Encrypt_Field1)
			-- FROM
				-- @Account
			-- WHERE 
				-- Doc_Code = 'ID235B'
			-- )
	-- WHERE
		-- Result_Seq = 11
		
	-- UPDATE
		-- @ResultTable
	-- SET
		-- Result_Value5 =
			-- (
			-- SELECT
				-- COUNT(DISTINCT Encrypt_Field1)
			-- FROM
				-- @Account
			-- WHERE 
				-- Doc_Code = 'VISA'
			-- )
	-- WHERE
		-- Result_Seq = 11
		
	-- UPDATE
		-- @ResultTable
	-- SET
		-- Result_Value6 =
			-- (
			-- SELECT
				-- COUNT(DISTINCT Encrypt_Field1)
			-- FROM
				-- @Account
			-- WHERE 
				-- Doc_Code = 'ADOPC'
			-- )
	-- WHERE
		-- Result_Seq = 11
		
	-- UPDATE
		-- @ResultTable
	-- SET
		-- Result_Value7 =
			-- (
			-- SELECT
				-- COUNT(DISTINCT Encrypt_Field1)
			-- FROM
				-- @Account
			-- WHERE 
				-- Doc_Code = 'EC'
			-- )
	-- WHERE
		-- Result_Seq = 11
		
	-- UPDATE
		-- @ResultTable
	-- SET
		-- Result_Value8 =
			-- (
			-- CONVERT(int, Result_Value1)
			-- +
			-- CONVERT(int, Result_Value2)
			-- +
			-- CONVERT(int, Result_Value3)
			-- +
			-- CONVERT(int, Result_Value4)
			-- +
			-- CONVERT(int, Result_Value5)
			-- +
			-- CONVERT(int, Result_Value6)
			-- +
			-- CONVERT(int, Result_Value7)
			-- )
	-- WHERE
		-- Result_Seq = 11
		
	-- UPDATE
		-- @ResultTable
	-- SET
		-- Result_Value10 =
			-- (
			-- SELECT
				-- COUNT(DISTINCT Encrypt_Field1)
			-- FROM
				-- @Account
			-- WHERE 
				-- Doc_Code = 'HKIC'
			-- )
	-- WHERE
		-- Result_Seq = 11
		
	-- UPDATE
		-- @ResultTable
	-- SET
		-- Result_Value11 =
			-- (
			-- SELECT
				-- COUNT(DISTINCT Encrypt_Field1)
			-- FROM
				-- @Account
			-- WHERE 
				-- Doc_Code = 'HKBC'
			-- )
	-- WHERE
		-- Result_Seq = 11
		

-- -- ---------------------------------------------
-- -- Insert to statistics table
-- -- ---------------------------------------------
	
	-- DELETE FROM _eHealthAccountByDocumentType_RVP_Stat

	-- INSERT INTO _eHealthAccountByDocumentType_RVP_Stat (
		-- Display_Seq,
		-- Result_Value1,
		-- Result_Value2,
		-- Result_Value3,
		-- Result_Value4,
		-- Result_Value5,
		-- Result_Value6,
		-- Result_Value7,
		-- Result_Value8,
		-- Result_Value9,
		-- Result_Value10,
		-- Result_Value11
	-- ) 
	-- SELECT
		-- Result_Seq,
		-- Result_Value1,
		-- Result_Value2,
		-- Result_Value3,
		-- Result_Value4,
		-- Result_Value5,
		-- Result_Value6,
		-- Result_Value7,
		-- Result_Value8,
		-- Result_Value9,
		-- Result_Value10,
		-- Result_Value11
	-- FROM
		-- @ResultTable
	-- ORDER BY
		-- Result_Seq


-- END
-- GO

-- GRANT EXECUTE ON [dbo].[proc_EHS_eHealthAccountClaimByDocumentType_RVP_Stat] TO HCVU
-- GO
