IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0028_02_eHealthAccountByAge_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0028_02_eHealthAccountByAge_Stat]
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
-- Description:		Retrieve Report on eHealth (Subsidies) accounts created by age
--					Copy from proc_EHS_eHealthAccount_Stat
-- =============================================   

-- CREATE PROCEDURE [dbo].[proc_EHS_eHSD0028_02_eHealthAccountByAge_Stat] 
	-- @Cutoff_Dtm	datetime
-- AS BEGIN
-- -- =============================================
-- -- Declaration
-- -- =============================================
	-- DECLARE @Report_Dtm	datetime

	-- DECLARE @AccountTable AS table (
		-- Doc_Code					char(20),
		-- Encrypt_Field1				varbinary(100),
		-- DOB							datetime,
		-- DOB_Adjust					datetime,
		-- Exact_DOB					char(1),
		-- Create_Dtm					datetime,
		-- Earliest_Create_Dtm			datetime
	-- )
	
	-- DECLARE @AccountTableT AS table (
		-- Doc_Code					char(20),
		-- Encrypt_Field1				varbinary(100),
		-- DOB							datetime,
		-- DOB_Adjust					datetime,
		-- Exact_DOB					char(1),
		-- Create_Dtm					datetime,
		-- Earliest_Create_Dtm			datetime
	-- )
	
	-- DECLARE @AccountTableF AS table (
		-- Doc_Code					char(20),
		-- Encrypt_Field1				varbinary(100),
		-- DOB							datetime,
		-- DOB_Adjust					datetime,
		-- Exact_DOB					char(1),
		-- Create_Dtm					datetime,
		-- Earliest_Create_Dtm			datetime
	-- )
	
	-- DECLARE @EarliestPersonalInformation AS table (
		-- Voucher_Acc_ID	char(15),
		-- Create_Dtm		datetime
	-- )
	
	-- DECLARE @DuplicatedID AS table (
		-- Encrypt_Field1	varbinary(100),
		-- Create_Dtm		datetime
	-- )
	
	-- DECLARE @DuplicatedID2 AS table (
		-- Doc_Code		char(20),
		-- Encrypt_Field1	varbinary(100),
		-- Create_Dtm		datetime
	-- )
	
	-- DECLARE @ResultTable AS table(
		-- Result_Seq		smallint,
		-- Result_Value1	varchar(100),
		-- Result_Value2	varchar(100),
		-- Result_Value3	varchar(100),
		-- Result_Value4	varchar(100),
		-- Result_Value5	varchar(100)
	-- )
	
	-- DECLARE @ResultTableAge table (
		-- ToSixMonth				int,
		-- SixMonthToTwoYear		int,
		-- TwoYearToSixYear		int,
		-- SixYearToNineYear		int,
		-- NineYearToElevenYear	int,
		-- ElevenYearToSixFiveYear	int,
		-- SixFiveYear				int,
		-- SixSixYear				int,
		-- SixSevenYear			int,
		-- SixEightYear			int,
		-- SixNineYear				int,
		-- SevenOYear				int,
		-- SevenOneYear			int,
		-- SevenTwoYear			int,
		-- SevenThreeYear			int,
		-- SevenFourYear			int,
		-- SevenFiveYear			int,
		-- SevenFiveYearTo			int,
		-- Total					int
	-- )
	
-- -- =============================================
-- -- Validation 
-- -- =============================================
-- -- =============================================
-- -- Initialization
-- -- =============================================
	-- SET @Report_Dtm = DATEADD(dd, -1, @Cutoff_Dtm)


-- -- ---------------------------------------------
-- -- Validated Account
-- -- ---------------------------------------------

-- -- Prepare a list of personal information having the earliest create datetime

	-- INSERT INTO @EarliestPersonalInformation (
		-- Voucher_Acc_ID,
		-- Create_Dtm
	-- )
	-- SELECT
		-- PI.Voucher_Acc_ID,
		-- MIN(PI.Create_Dtm) AS [Create_Dtm]
	-- FROM
		-- VoucherAccount VA
			-- INNER JOIN PersonalInformation PI
				-- ON VA.Voucher_Acc_ID = PI.Voucher_Acc_ID
	-- WHERE
		-- VA.Effective_Dtm <= @Cutoff_Dtm
	-- GROUP BY
		-- PI.Voucher_Acc_ID
		
		
	-- INSERT INTO @AccountTable (
		-- Doc_Code,
		-- Encrypt_Field1,
		-- DOB,
		-- DOB_Adjust,
		-- Exact_DOB,
		-- Create_Dtm,
		-- Earliest_Create_Dtm
	-- )
	-- SELECT
		-- PI.Doc_Code,
		-- PI.Encrypt_Field1,
		-- PI.DOB,
		-- PI.DOB,
		-- PI.Exact_DOB,
		-- PI.Create_Dtm,
		-- EPI.Create_Dtm AS [Earliest_Create_Dtm]
	-- FROM
		-- VoucherAccount VA
			-- INNER JOIN PersonalInformation PI
				-- ON VA.Voucher_Acc_ID = PI.Voucher_Acc_ID
			-- INNER JOIN @EarliestPersonalInformation EPI
				-- ON VA.Voucher_Acc_ID = EPI.Voucher_Acc_ID
	-- WHERE
			-- VA.Effective_Dtm <= @Cutoff_Dtm
	
	-- DELETE FROM
		-- @AccountTable
	-- WHERE
		-- Create_Dtm <> Earliest_Create_Dtm


-- -- Update all the validated account's Create_Dtm to be '1900-01-01' so that it is always earlier than temporary and special

	-- UPDATE
		-- @AccountTable
	-- SET
		-- Create_Dtm = '1900-01-01'
	
	
-- -- ---------------------------------------------
-- -- Temporary Account
-- -- ---------------------------------------------

	-- INSERT INTO @AccountTable (
		-- Doc_Code,
		-- Encrypt_Field1,
		-- DOB,
		-- DOB_Adjust,
		-- Exact_DOB,
		-- Create_Dtm
	-- )
	-- SELECT
		-- TP.Doc_Code,
		-- TP.Encrypt_Field1,
		-- TP.DOB,
		-- TP.DOB,
		-- TP.Exact_DOB,
		-- TP.Create_Dtm
	-- FROM
		-- TempVoucherAccount TA
			-- INNER JOIN TempPersonalInformation TP
				-- ON TA.Voucher_Acc_ID = TP.Voucher_Acc_ID
	-- WHERE
		-- TA.Record_Status NOT IN ('V', 'D')
			-- AND TA.Create_Dtm <= @Cutoff_Dtm
			-- AND TA.Account_Purpose IN ('C', 'V')


-- -- ---------------------------------------------
-- -- Special Account
-- -- ---------------------------------------------
		
	-- INSERT INTO @AccountTable (
		-- Doc_Code,
		-- Encrypt_Field1,
		-- DOB,
		-- DOB_Adjust,
		-- Exact_DOB,
		-- Create_Dtm
	-- )
	-- SELECT
		-- SP.Doc_Code,
		-- SP.Encrypt_Field1,
		-- SP.DOB,
		-- SP.DOB,
		-- SP.Exact_DOB,
		-- SP.Create_Dtm
	-- FROM
		-- SpecialAccount SA
			-- INNER JOIN SpecialPersonalInformation SP
				-- ON SA.Special_Acc_ID = SP.Special_Acc_ID			
	-- WHERE
		-- SA.Record_Status NOT IN ('V', 'D')
			-- AND SA.Create_Dtm <= @Cutoff_Dtm
			-- AND SA.Account_Purpose IN ('C', 'V')


-- -- ---------------------------------------------
-- -- Handle the data with same [Doc_Code] + [Encrypt_Field1]
-- -- ---------------------------------------------

-- -- Handle HKIC and HKBC

	-- INSERT INTO @DuplicatedID (
		-- Encrypt_Field1,
		-- Create_Dtm
	-- )
	-- SELECT
		-- Encrypt_Field1,
		-- MIN(Create_Dtm) AS [Create_Dtm]
	-- FROM
		-- @AccountTable
	-- WHERE
		-- Doc_Code IN ('HKIC', 'HKBC')
	-- GROUP BY
		-- Encrypt_Field1

	
-- -- First insert the accounts of HKIC and HKBC into @AccountTableT
		
	-- INSERT INTO @AccountTableT (
		-- Doc_Code,
		-- Encrypt_Field1,
		-- DOB,
		-- DOB_Adjust,
		-- Exact_DOB,
		-- Create_Dtm,
		-- Earliest_Create_Dtm
	-- )
	-- SELECT
		-- A.Doc_Code,
		-- A.Encrypt_Field1,
		-- A.DOB,
		-- A.DOB,
		-- A.Exact_DOB,
		-- A.Create_Dtm,
		-- DI.Create_Dtm AS [Earliest_Create_Dtm]
	-- FROM
		-- @AccountTable A
			-- INNER JOIN @DuplicatedID DI
				-- ON A.Encrypt_Field1 = DI.Encrypt_Field1 
					-- AND Doc_Code IN ('HKIC', 'HKBC')

	-- DELETE FROM
		-- @AccountTableT
	-- WHERE
		-- Earliest_Create_Dtm IS NOT NULL
			-- AND Create_Dtm <> Earliest_Create_Dtm

		
-- -- Then insert the accounts of not HKIC and not HKBC into @AccountTableT
		
	-- INSERT INTO @AccountTableT (
		-- Doc_Code,
		-- Encrypt_Field1,
		-- DOB,
		-- DOB_Adjust,
		-- Exact_DOB,
		-- Create_Dtm,
		-- Earliest_Create_Dtm
	-- )
	-- SELECT
		-- A.Doc_Code,
		-- A.Encrypt_Field1,
		-- A.DOB,
		-- A.DOB,
		-- A.Exact_DOB,
		-- A.Create_Dtm,
		-- NULL AS [Earliest_Create_Dtm]
	-- FROM
		-- @AccountTable A
	-- WHERE
		-- A.Doc_Code NOT IN ('HKIC', 'HKBC')


-- -- Handle all documents

	-- INSERT INTO @DuplicatedID2 (
		-- Doc_Code, 
		-- Encrypt_Field1, 
		-- Create_Dtm
	-- )
	-- SELECT
		-- Doc_Code,
		-- Encrypt_Field1,
		-- MIN(Create_Dtm) AS [Create_Dtm]
	-- FROM
		-- @AccountTableT
	-- GROUP BY
		-- Doc_Code,
		-- Encrypt_Field1

	-- INSERT INTO @AccountTableF (
		-- Doc_Code,
		-- Encrypt_Field1,
		-- DOB,
		-- DOB_Adjust,
		-- Exact_DOB,
		-- Create_Dtm,
		-- Earliest_Create_Dtm
	-- )
	-- SELECT
		-- A.Doc_Code,
		-- A.Encrypt_Field1,
		-- A.DOB,
		-- A.DOB,
		-- A.Exact_DOB,
		-- A.Create_Dtm,
		-- DI.Create_Dtm AS [Earliest_Create_Dtm]
	-- FROM
		-- @AccountTableT A
			-- INNER JOIN @DuplicatedID2 DI
				-- ON A.Doc_Code = DI.Doc_Code
					-- AND A.Encrypt_Field1 = DI.Encrypt_Field1
	
	-- DELETE FROM
		-- @AccountTableF
	-- WHERE
		-- Earliest_Create_Dtm IS NOT NULL
			-- AND Create_Dtm <> Earliest_Create_Dtm

		
-- -- =============================================
-- -- Return results
-- -- =============================================

-- -- ---------------------------------------------
-- -- Patch data
-- -- ---------------------------------------------

	
	-- UPDATE
		-- @AccountTableF
	-- SET
		-- DOB = CONVERT(varchar, YEAR(DOB)) + '-' + CONVERT(varchar, MONTH(DOB)) + '-' + CONVERT(varchar, DAY(DATEADD(d, -DAY(DATEADD(m, 1, DOB)), DATEADD(m, 1, DOB))))
	-- WHERE
		-- Exact_DOB IN ('M', 'U')

	-- UPDATE
		-- @AccountTableF
	-- SET
		-- DOB_Adjust = DOB

	-- UPDATE
		-- @AccountTableF
	-- SET
		-- DOB_Adjust = DATEADD(yyyy, 1, DOB)
	-- WHERE
		-- MONTH(DOB) > MONTH(@Report_Dtm)
			-- OR ( MONTH(DOB) = MONTH(@Report_Dtm) AND DAY(DOB) > DAY(@Report_Dtm) )

	
-- -- ---------------------------------------------
-- -- Build result
-- -- ---------------------------------------------

	-- INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5) VALUES
	-- (0, 'eHS(S)D0028-02: Report on eHealth (Subsidies) accounts created (by age)', '', '', '', '')
	
	-- INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5) VALUES
	-- (1, '', '', '', '', '')
	
	-- INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5) VALUES
	-- (2, 'Reporting period: as at ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111), '', '', '', '')

	-- INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5) VALUES
	-- (3, '', '', '', '', '')

	-- INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5) VALUES
	-- (10, '6 months to less than 6 years', '6 years to less than 12 years', '12 years to less than 65 years', '>= 65 years', 'Total')

	-- INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5) VALUES
	-- (11, '', '', '', '', '')
		
	
	-- UPDATE
		-- @ResultTable
	-- SET
		-- Result_Value1 = 
			-- (
			-- SELECT
				-- COUNT(1)
			-- FROM
				-- @AccountTableF
			-- WHERE 
				-- DATEDIFF(dd, DOB, @Report_Dtm) >= 182
					-- AND DATEDIFF(yyyy, DOB_Adjust, @Report_Dtm) < 6
			-- )
	-- WHERE
		-- Result_Seq = 11

	
	-- UPDATE
		-- @ResultTable
	-- SET
		-- Result_Value2 = 
			-- (
			-- SELECT
				-- COUNT(1)
			-- FROM
				-- @AccountTableF
			-- WHERE 
				-- DATEDIFF(yyyy, DOB_Adjust, @Report_Dtm) >= 6 
					-- AND DATEDIFF(yyyy, DOB_Adjust, @Report_Dtm) < 12
			-- )
	-- WHERE
		-- Result_Seq = 11
					
	
	-- UPDATE
		-- @ResultTable
	-- SET
		-- Result_Value3 = 
			-- (
			-- SELECT
				-- COUNT(1)
			-- FROM
				-- @AccountTableF
			-- WHERE 
				-- DATEDIFF(yyyy, DOB_Adjust, @Report_Dtm) >= 12 
					-- AND DATEDIFF(yyyy, DOB, @Report_Dtm) < 65
			-- )
	-- WHERE
		-- Result_Seq = 11
			
	-- UPDATE
		-- @ResultTable
	-- SET
		-- Result_Value4 = 
			-- (
			-- SELECT
				-- COUNT(1)
			-- FROM
				-- @AccountTableF
			-- WHERE 
				-- DATEDIFF(yyyy, DOB, @Report_Dtm) >= 65
			-- )
	-- WHERE
		-- Result_Seq = 11
		
					
	-- UPDATE
		-- @ResultTable
	-- SET
		-- Result_Value5 = 
			-- (
			-- CONVERT(int, Result_Value1)
			-- +
			-- CONVERT(int, Result_Value2)
			-- +
			-- CONVERT(int, Result_Value3)
			-- +
			-- CONVERT(int, Result_Value4)			
			-- )
	-- WHERE
		-- Result_Seq = 11


-- -- ---------------------------------------------
-- -- eHS(S)D0005-02 - eHealth Account Summary by age group
-- -- ---------------------------------------------

	-- INSERT INTO @ResultTableAge VALUES (0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)

	-- UPDATE	@ResultTableAge
	-- SET		ToSixMonth = (
				-- SELECT COUNT(1) FROM (
					-- SELECT DISTINCT Doc_Code, Encrypt_Field1 
					-- FROM @AccountTableF
					-- WHERE DATEDIFF(dd, DOB, @Report_Dtm) < 182
				-- ) T
			-- )
	
	-- UPDATE	@ResultTableAge
	-- SET		SixMonthToTwoYear = (
				-- SELECT COUNT(1) FROM (
					-- SELECT DISTINCT Doc_Code, Encrypt_Field1 
					-- FROM @AccountTableF
					-- WHERE DATEDIFF(dd, DOB, @Report_Dtm) >= 182 AND DATEDIFF(yy, DOB_Adjust, @Report_Dtm) < 2
				-- ) T
			-- )
	
	-- UPDATE	@ResultTableAge
	-- SET		TwoYearToSixYear = (
				-- SELECT COUNT(1) FROM (
					-- SELECT DISTINCT Doc_Code, Encrypt_Field1 
					-- FROM @AccountTableF
					-- WHERE DATEDIFF(yy, DOB_Adjust, @Report_Dtm) >= 2 AND DATEDIFF(yy, DOB_Adjust, @Report_Dtm) < 6
				-- ) T
			-- )
	
	-- UPDATE	@ResultTableAge
	-- SET		SixYearToNineYear = (
				-- SELECT COUNT(1) FROM (
					-- SELECT DISTINCT Doc_Code, Encrypt_Field1 
					-- FROM @AccountTableF
					-- WHERE DATEDIFF(yy, DOB_Adjust, @Report_Dtm) >= 6 AND DATEDIFF(yy, DOB_Adjust, @Report_Dtm) < 9
				-- ) T
			-- )
	
	-- UPDATE	@ResultTableAge
	-- SET		NineYearToElevenYear = (
				-- SELECT COUNT(1) FROM (
					-- SELECT DISTINCT Doc_Code, Encrypt_Field1 
					-- FROM @AccountTableF
					-- WHERE DATEDIFF(yy, DOB_Adjust, @Report_Dtm) >= 9 AND DATEDIFF(yy, DOB_Adjust, @Report_Dtm) < 11
				-- ) T
			-- )
	
	-- UPDATE	@ResultTableAge
	-- SET		ElevenYearToSixFiveYear = (
				-- SELECT COUNT(1) FROM (
					-- SELECT DISTINCT Doc_Code, Encrypt_Field1 
					-- FROM @AccountTableF
					-- WHERE DATEDIFF(yy, DOB_Adjust, @Report_Dtm) >= 11 AND DATEDIFF(yy, DOB_Adjust, @Report_Dtm) < 65 AND DATEDIFF(yy, DOB, @Report_Dtm) < 65
				-- ) T
			-- )
	
	-- UPDATE	@ResultTableAge
	-- SET		SixFiveYear = (
				-- SELECT COUNT(1) FROM (
					-- SELECT DISTINCT Doc_Code, Encrypt_Field1 
					-- FROM @AccountTableF
					-- WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 65
				-- ) T
			-- )
	
	-- UPDATE	@ResultTableAge
	-- SET		SixSixYear = (
				-- SELECT COUNT(1) FROM (
					-- SELECT DISTINCT Doc_Code, Encrypt_Field1 
					-- FROM @AccountTableF
					-- WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 66
				-- ) T
			-- )
	
	-- UPDATE	@ResultTableAge
	-- SET		SixSevenYear = (
				-- SELECT COUNT(1) FROM (
					-- SELECT DISTINCT Doc_Code, Encrypt_Field1 
					-- FROM @AccountTableF
					-- WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 67
				-- ) T
			-- )
	
	-- UPDATE	@ResultTableAge
	-- SET		SixEightYear = (
				-- SELECT COUNT(1) FROM (
					-- SELECT DISTINCT Doc_Code, Encrypt_Field1 
					-- FROM @AccountTableF
					-- WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 68
				-- ) T
			-- )
	
	-- UPDATE	@ResultTableAge
	-- SET		SixNineYear = (
				-- SELECT COUNT(1) FROM (
					-- SELECT DISTINCT Doc_Code, Encrypt_Field1 
					-- FROM @AccountTableF
					-- WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 69
				-- ) T
			-- )
	
	-- UPDATE	@ResultTableAge
	-- SET		SevenOYear = (
				-- SELECT COUNT(1) FROM (
					-- SELECT DISTINCT Doc_Code, Encrypt_Field1 
					-- FROM @AccountTableF
					-- WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 70
				-- ) T
			-- )
	
	-- UPDATE	@ResultTableAge
	-- SET		SevenOneYear = (
				-- SELECT COUNT(1) FROM (
					-- SELECT DISTINCT Doc_Code, Encrypt_Field1 
					-- FROM @AccountTableF
					-- WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 71
				-- ) T
			-- )
	
	-- UPDATE	@ResultTableAge
	-- SET		SevenTwoYear = (
				-- SELECT COUNT(1) FROM (
					-- SELECT DISTINCT Doc_Code, Encrypt_Field1 
					-- FROM @AccountTableF
					-- WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 72
				-- ) T
			-- )
	
	-- UPDATE	@ResultTableAge
	-- SET		SevenThreeYear = (
				-- SELECT COUNT(1) FROM (
					-- SELECT DISTINCT Doc_Code, Encrypt_Field1 
					-- FROM @AccountTableF
					-- WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 73
				-- ) T
			-- )
	
	-- UPDATE	@ResultTableAge
	-- SET		SevenFourYear = (
				-- SELECT COUNT(1) FROM (
					-- SELECT DISTINCT Doc_Code, Encrypt_Field1 
					-- FROM @AccountTableF
					-- WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 74
				-- ) T
			-- )
	
	-- UPDATE	@ResultTableAge
	-- SET		SevenFiveYear = (
				-- SELECT COUNT(1) FROM (
					-- SELECT DISTINCT Doc_Code, Encrypt_Field1 
					-- FROM @AccountTableF
					-- WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 75
				-- ) T
			-- )
	
	-- UPDATE	@ResultTableAge
	-- SET		SevenFiveYearTo = (
				-- SELECT COUNT(1) FROM (
					-- SELECT DISTINCT Doc_Code, Encrypt_Field1 
					-- FROM @AccountTableF
					-- WHERE DATEDIFF(yy, DOB, @Report_Dtm) > 75
				-- ) T
			-- )
	
	-- UPDATE	@ResultTableAge
	-- SET		Total = (
				-- CONVERT(int, ToSixMonth) +
				-- CONVERT(int, SixMonthToTwoYear) +
				-- CONVERT(int, TwoYearToSixYear) +
				-- CONVERT(int, SixYearToNineYear) +
				-- CONVERT(int, NineYearToElevenYear) +
				-- CONVERT(int, ElevenYearToSixFiveYear) +
				-- CONVERT(int, SixFiveYear) +
				-- CONVERT(int, SixSixYear) +
				-- CONVERT(int, SixSevenYear) +
				-- CONVERT(int, SixEightYear) +
				-- CONVERT(int, SixNineYear) +
				-- CONVERT(int, SevenOYear) +
				-- CONVERT(int, SevenOneYear) +
				-- CONVERT(int, SevenTwoYear) +
				-- CONVERT(int, SevenThreeYear) +
				-- CONVERT(int, SevenFourYear) +
				-- CONVERT(int, SevenFiveYear) +
				-- CONVERT(int, SevenFiveYearTo)
			-- )
	

-- -- ---------------------------------------------
-- -- Insert to statistics table
-- -- ---------------------------------------------
	
	-- DELETE FROM RpteHSD0028eHealthAccountByAgeStat

	-- INSERT INTO RpteHSD0028eHealthAccountByAgeStat (
		-- Display_Seq,
		-- Col1,
		-- Col2,
		-- Col3,
		-- Col4,
		-- Col5
	-- )
	-- SELECT
		-- Result_Seq,
		-- Result_Value1,
		-- Result_Value2,
		-- Result_Value3,
		-- Result_Value4,
		-- Result_Value5
	-- FROM
		-- @ResultTable
	-- ORDER BY
		-- Result_Seq

-- --

	-- INSERT INTO RpteHSD0028eHealthAccountByAgeGroupStat (
		-- System_Dtm,
		-- Report_Dtm,
		-- ToSixMonth,
		-- SixMonthToTwoYear,
		-- TwoYearToSixYear,
		-- SixYearToNineYear,
		-- NineYearToElevenYear,
		-- ElevenYearToSixFiveYear,
		-- SixFiveYear,
		-- SixSixYear,
		-- SixSevenYear,
		-- SixEightYear,
		-- SixNineYear,
		-- SevenOYear,
		-- SevenOneYear,
		-- SevenTwoYear,
		-- SevenThreeYear,
		-- SevenFourYear,
		-- SevenFiveYear,
		-- SevenFiveYearTo,
		-- Total
	-- )
	-- SELECT
		-- GETDATE(),
		-- @Report_Dtm,
		-- ToSixMonth,
		-- SixMonthToTwoYear,
		-- TwoYearToSixYear,
		-- SixYearToNineYear,
		-- NineYearToElevenYear,
		-- ElevenYearToSixFiveYear,
		-- SixFiveYear,
		-- SixSixYear,
		-- SixSevenYear,
		-- SixEightYear,
		-- SixNineYear,
		-- SevenOYear,
		-- SevenOneYear,
		-- SevenTwoYear,
		-- SevenThreeYear,
		-- SevenFourYear,
		-- SevenFiveYear,
		-- SevenFiveYearTo,
		-- Total
	-- FROM
		-- @ResultTableAge
	
		
-- END
-- GO

-- GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0028_02_eHealthAccountByAge_Stat] TO HCVU
-- GO
