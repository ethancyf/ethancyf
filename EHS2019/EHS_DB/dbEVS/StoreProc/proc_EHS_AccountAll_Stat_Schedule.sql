IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_AccountAll_Stat_Schedule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_AccountAll_Stat_Schedule]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	15 Jul 2020
-- CR. No			INT20-0024
-- Description:		(1) Add WITH (NOLOCK)
--					(2) Add index to temp tables
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Dickson Law
-- Modified date:	28 Dec 2017
-- CR. No			CRE14-016
-- Description:		(1) Add Acccount Breakdown by Alive and Deceased
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	15 June 2011
-- CR. No			CRE11-007
-- Description:		(1) Add Validated Acccount Breakdown by Status
--						Active, Suspended, Terminated
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	8 April 2011
-- Description:		(1) Modify the counting on invalid accounts
--					(2) Clear today record before insert
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	18 January 2010
-- Description:		Comment the unused part
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	12 January 2010
-- Description:		Do not insert date to the temporary table [_EHS_Account_Age]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	7 January 2010
-- Description:		(1) Reformat the code
--					(2) Add the statistics grouped by age group
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 20 Nov 2009
-- Description:	Prepare eHealth Account Statistic Report
--				including Validated, Special, Temporary and Invalid
--				*Priority in account counting method:
--					1. Validated Account (include terminated account)
--					2. Special Account	(exclude removed account)
--					3. Invalid Account (all account)
--					4. Temporary Account (exclude removed account)
--				e.g. Same identifiy document no. exists in Validated Account and Special Account/Temporary Account
--				, it will be counted as Validated Account
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_AccountAll_Stat_Schedule]
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Report setting
-- =============================================	
	DECLARE @Cutoff_Dtm datetime
	SET @Cutoff_Dtm = CONVERT(varchar(11), GETDATE(), 106) + ' 00:00'
	
	DECLARE @Report_Dtm datetime
	SET @Report_Dtm = DATEADD(dd, -1, @Cutoff_Dtm)

-- =============================================
-- Declaration
-- =============================================

	CREATE TABLE #EHS01_AllAccount (
		Voucher_Acc_ID			varchar(20),
		Temp_Voucher_Acc_ID		varchar(20),
		Special_Acc_ID			varchar(20),
		Invalid_Acc_ID			varchar(20),
		Doc_Code				char(10),
		Encrypt_Field1			varbinary(100),
		Scheme_Code				char(10),
		Exact_DOB				char(1),
		DOB						datetime,
		DOB_Adjust				datetime,
		Record_Status			char(1), --CRE11-007
		Deceased				char(1), --CRE14-016
		DOD						datetime,--CRE14-016
		Exact_DOD				char(1)  --CRE14-016
	)

	CREATE CLUSTERED INDEX PK_EHS01_AllAccount_1 ON #EHS01_AllAccount (Doc_Code, Encrypt_Field1) 

	CREATE TABLE #EHS01_GroupAccount (
		Account_Type			char(1),
		Doc_Code				char(10),
		Encrypt_Field1			varbinary(100),
		DOB						datetime,
		DOB_Adjust				datetime,
		Record_Status			char(10),
		Deceased				char(1), --CRE14-016
		DOD						datetime --CRE14-016
	)
	
	CREATE CLUSTERED INDEX PK_EHS01_GroupAccount_1 ON #EHS01_GroupAccount (Doc_Code, Encrypt_Field1) 

	DECLARE @ResultTableAccount table (
		CountType							varchar(10),
		No_Validated_Account				int,
		No_Special_Account					int,
		No_Temp_Account						int,
		No_Validated_Account_Active			int,
		No_Validated_Account_Suspended		int,
		No_Validated_Account_Terminated		int,
		Total_Account						int
	)

	
	--DECLARE @ResultTableAge table (
	--	ToSixMonth				int,
	--	SixMonthToTwoYear		int,
	--	TwoYearToSixYear		int,
	--	SixYearToNineYear		int,
	--	NineYearToElevenYear	int,
	--	ElevenYearToSixFiveYear	int,
	--	SixFiveYear				int,
	--	SixSixYear				int,
	--	SixSevenYear			int,
	--	SixEightYear			int,
	--	SixNineYear				int,
	--	SevenOYear				int,
	--	SevenOneYear			int,
	--	SevenTwoYear			int,
	--	SevenThreeYear			int,
	--	SevenFourYear			int,
	--	SevenFiveYear			int,
	--	SevenFiveYearTo			int,
	--	Total					int
	--)


-- =============================================
-- Initialization
-- =============================================

-- ---------------------------------------------
-- Retrieve all acounts
-- ---------------------------------------------

-- ---------------------------------------------
-- Validated Account
-- ---------------------------------------------

	INSERT INTO #EHS01_AllAccount (
		Voucher_Acc_ID,
		Doc_Code,
		Encrypt_Field1,
		Scheme_Code,
		Exact_DOB,
		DOB,
		Record_Status,
		Deceased,
		DOD,
		Exact_DOD	
	)
	SELECT
		VA.Voucher_Acc_ID,
		VP.Doc_Code,
		VP.Encrypt_Field1,
		VA.Scheme_Code,
		VP.Exact_DOB,
		VP.DOB,
		VA.Record_Status,
		CASE WHEN VP.Deceased IS NULL THEN 'N' ELSE
			CASE WHEN VP.Deceased = 'Y' THEN 'Y' ELSE 'N' END
		END,
		VP.DOD, 
		VP.Exact_DOD 
	FROM
		VoucherAccount VA WITH (NOLOCK)
			INNER JOIN PersonalInformation VP WITH (NOLOCK)
				ON VA.Voucher_Acc_ID = VP.Voucher_Acc_ID
	WHERE
		VA.Effective_Dtm <= @Cutoff_Dtm


-- ---------------------------------------------
-- Temporary Account
-- ---------------------------------------------

	INSERT INTO #EHS01_AllAccount (
		Temp_Voucher_Acc_ID,
		Doc_Code,
		Encrypt_Field1,
		Scheme_Code,
		Exact_DOB,
		DOB,
		Record_Status,
		Deceased,
		DOD,
		Exact_DOD	
	)
	SELECT
		TA.Voucher_Acc_ID,
		TP.Doc_Code,
		TP.Encrypt_Field1,
		TA.Scheme_Code,
		TP.Exact_DOB,
		TP.DOB,
		TA.Record_Status,
		CASE WHEN TP.Deceased IS NULL THEN 'N' ELSE
			CASE WHEN TP.Deceased = 'Y' THEN 'Y' ELSE 'N' END
		END,
		TP.DOD, 
		TP.Exact_DOD 
	FROM
		TempVoucherAccount TA WITH (NOLOCK)
			INNER JOIN TempPersonalInformation TP WITH (NOLOCK)
				ON TA.Voucher_Acc_ID = TP.Voucher_Acc_ID
	WHERE
		TA.Record_Status <> 'D'
			AND TA.Account_Purpose IN ('C', 'V')
			AND TA.Create_Dtm <= @Cutoff_Dtm


-- ---------------------------------------------
-- Special Account
-- ---------------------------------------------

	INSERT INTO #EHS01_AllAccount (
		Special_Acc_ID,
		Doc_Code,
		Encrypt_Field1,
		Scheme_Code,
		Exact_DOB,
		DOB,
		Record_Status,
		Deceased,
		DOD,
		Exact_DOD	
	)
	SELECT
		SA.Special_Acc_ID,
		SP.Doc_Code,
		SP.Encrypt_Field1,
		SA.Scheme_Code,
		SP.Exact_DOB,
		SP.DOB,
		SA.Record_Status,
		CASE WHEN SP.Deceased IS NULL THEN 'N' ELSE
			CASE WHEN SP.Deceased = 'Y' THEN 'Y' ELSE 'N' END
		END,
		SP.DOD, 
		SP.Exact_DOD 
	FROM
		SpecialAccount SA WITH (NOLOCK)
			INNER JOIN SpecialPersonalInformation SP WITH (NOLOCK)
				ON SA.Special_Acc_ID = SP.Special_Acc_ID
	WHERE 
		SA.Record_Status <> 'D'
			AND SA.Create_Dtm <= @Cutoff_Dtm


-- ---------------------------------------------
-- Invalid Account
-- ---------------------------------------------

	--OPEN SYMMETRIC KEY sym_Key 
	--DECRYPTION BY ASYMMETRIC KEY asym_Key
	--
	--INSERT INTO #EHS01_AllAccount (
	--	Invalid_Acc_ID,
	--	Doc_Code,
	--	Encrypt_Field1,
	--	Scheme_Code,
	--	Exact_DOB,
	--	DOB,
	--	Record_Status,
	--	Deceased,
	--	DOD,
	--	Exact_DOD
	--)
	--SELECT 
	--	IA.Invalid_Acc_ID,
	--	IP.Doc_Code,
	--	IP.Encrypt_Field1,
	--	IA.Scheme_Code,
	--	IP.Exact_DOB,
	--	IP.DOB,
	--	IA.Record_Status,
	--	'N',
	--	NULL,
	--	NULL
	--FROM
	--	InvalidAccount IA
	--		INNER JOIN InvalidPersonalInformation IP
	--			ON IA.Invalid_Acc_ID = IP.Invalid_Acc_ID	
	--WHERE
	--	IA.Create_Dtm < @Cutoff_Dtm
	--		AND NOT (
	--			IP.Doc_Code = 'HKIC'
	--				AND IP.Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), ' X0000000')
	--		)
	--
	--CLOSE SYMMETRIC KEY sym_Key


-- ---------------------------------------------
-- Update all HKBC to HKIC (for easy counting)
-- ---------------------------------------------   

	UPDATE
		#EHS01_AllAccount
	SET
		Doc_Code = 'HKIC'
	WHERE
		Doc_Code = 'HKBC'


---- ---------------------------------------------
---- Patch the DOB (if only year and month is provided, set the date to the last day of month)
---- ---------------------------------------------
--
--	UPDATE
--		#EHS01_AllAccount
--	SET
--		DOB = DATEADD(mm, DATEDIFF(mm, 0, DATEADD(mm, 1, DOB)), -1)
--	WHERE
--		Exact_DOB IN ('M', 'U')
--			AND Scheme_Code IN ('CIVSS', 'HSIVSS', 'RVP')
--
--
---- ---------------------------------------------
---- Patch the DOB_Adjust
---- ---------------------------------------------
--
--	UPDATE
--		#EHS01_AllAccount
--	SET
--		DOB_Adjust = DOB
--
--	UPDATE
--		#EHS01_AllAccount
--	SET
--		DOB_Adjust = DATEADD(yyyy, 1, DOB)
--	WHERE
--		MONTH(DOB) > MONTH(@Report_Dtm)
--			OR ( MONTH(DOB) = MONTH(@Report_Dtm) AND DAY(DOB) > DAY(@Report_Dtm) )	
--
--
--	
---- ---------------------------------------------
---- Patch the DOD
---- ---------------------------------------------
--
--	UPDATE
--		#EHS01_AllAccount
--	SET
--		DOD = DATEADD(mm, DATEDIFF(mm, 0, DATEADD(mm, 1, DOD)), -1)
--	WHERE
--		Exact_DOD ='M'
--
--	UPDATE
--		#EHS01_AllAccount
--	SET
--		DOD = DATEADD(yy, DATEDIFF(yy, 0, DATEADD(yy, 1, DOD)), -1)
--	WHERE
--		Exact_DOD ='Y'

-- ---------------------------------------------
-- Group the account
-- ---------------------------------------------

	INSERT INTO #EHS01_GroupAccount (
		Doc_Code,
		Encrypt_Field1,
		Record_Status
	)
	SELECT DISTINCT
		-- NULL AS [Account_Type],
		Doc_Code,
		Encrypt_Field1,
		Record_Status
	FROM
		#EHS01_AllAccount


-- ---------------------------------------------
-- Patch the account type
-- ---------------------------------------------
	
	--Temporary Account
	UPDATE
		#EHS01_GroupAccount
	SET
		Account_Type = 'T',
		DOB = AA.DOB,
		DOB_Adjust = AA.DOB_Adjust,
		Deceased = AA.Deceased,
		DOD = AA.DOD
	FROM
		#EHS01_GroupAccount GA
			INNER JOIN #EHS01_AllAccount AA
				ON GA.Doc_Code = AA.Doc_Code
					AND GA.Encrypt_Field1 = AA.Encrypt_Field1
	WHERE
		ISNULL(AA.Temp_Voucher_Acc_ID, '') <> ''

	--Special Account
	UPDATE
		#EHS01_GroupAccount
	SET
		Account_Type = 'S',
		DOB = AA.DOB,
		DOB_Adjust = AA.DOB_Adjust,
		Deceased = AA.Deceased,
		DOD = AA.DOD
	FROM
		#EHS01_GroupAccount GA
			INNER JOIN #EHS01_AllAccount AA
				ON GA.Doc_Code = AA.Doc_Code
					AND GA.Encrypt_Field1 = AA.Encrypt_Field1
	WHERE
		ISNULL(AA.Special_Acc_ID, '') <> ''
	
	----Invalid Account
	--UPDATE
	--	#EHS01_GroupAccount
	--SET
	--	Account_Type = 'I',
	--	DOB = AA.DOB,
	--	DOB_Adjust = AA.DOB_Adjust,
	--	Deceased = AA.Deceased,
	--	DOD = AA.DOD
	--FROM
	--	#EHS01_GroupAccount GA
	--		INNER JOIN #EHS01_AllAccount AA
	--			ON GA.Doc_Code = AA.Doc_Code
	--				AND GA.Encrypt_Field1 = AA.Encrypt_Field1
	--WHERE
	--	ISNULL(AA.Invalid_Acc_ID, '') <> ''

	--Validated Account
	UPDATE
		#EHS01_GroupAccount
	SET
		Account_Type = 'V',
		DOB = AA.DOB,
		DOB_Adjust = AA.DOB_Adjust,
		Deceased = AA.Deceased,
		DOD = AA.DOD
	FROM
		#EHS01_GroupAccount GA
			INNER JOIN #EHS01_AllAccount AA
				ON GA.Doc_Code = AA.Doc_Code
					AND GA.Encrypt_Field1 = AA.Encrypt_Field1
	WHERE
		ISNULL(AA.Voucher_Acc_ID, '') <> ''


-- =============================================
-- Return result
-- =============================================

-- ---------------------------------------------
-- eHealth Account Summary
-- ---------------------------------------------

	INSERT INTO @ResultTableAccount VALUES ('Alive', 0, 0, 0, 0, 0, 0, 0)
	INSERT INTO @ResultTableAccount VALUES ('Deceased', 0, 0, 0, 0, 0, 0, 0)
	INSERT INTO @ResultTableAccount VALUES ('Total', 0, 0, 0, 0, 0, 0, 0)

	--Count of Alive Validated Account
	UPDATE	@ResultTableAccount
	SET		No_Validated_Account = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type = 'V' AND Deceased='N'
				) T
			)
	WHERE CountType = 'Alive'
	
	--Count of Deceased Validated Account
	UPDATE	@ResultTableAccount
	SET		No_Validated_Account = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type = 'V' AND Deceased='Y'
				) T
			)
	WHERE CountType = 'Deceased'

	
	--Count of Validated Account (Alive + Deceased)
	UPDATE	@ResultTableAccount
	SET		No_Validated_Account = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type = 'V'
				) T
			)
	WHERE CountType = 'Total'
	
	--Count of Alive Special Account
	UPDATE	@ResultTableAccount
	SET		No_Special_Account = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type = 'S' AND Deceased='N'
				) T
			)
	WHERE CountType = 'Alive'
	
	--Count of Deceased Special Account		
	UPDATE	@ResultTableAccount
	SET		No_Special_Account = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type = 'S' AND Deceased='Y'
				) T
			)
	WHERE CountType = 'Deceased'
	
	--Count of Special Account (Alive + Deceased)
	UPDATE	@ResultTableAccount
	SET		No_Special_Account = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type = 'S'
				) T
			)	
	WHERE CountType = 'Total'
	
	--Count of Alive Temporary Account
	UPDATE	@ResultTableAccount
	SET		No_Temp_Account = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type = 'T' AND Deceased='N'
				) T
			)
	WHERE CountType = 'Alive'
	
	--Count of Deceased Temporary Account
	UPDATE	@ResultTableAccount
	SET		No_Temp_Account = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type = 'T' AND Deceased='Y'
				) T
			)
	WHERE CountType = 'Deceased'
	
	--Count of Temporary Account (Alive + Deceased)
	UPDATE	@ResultTableAccount
	SET		No_Temp_Account = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type = 'T'
				) T
			)
	WHERE CountType = 'Total'
	
-- CRE11-007 
-- Obsolete Invalid Account Total
-- ================================================================================================================
--	UPDATE	@ResultTableAccount
--	SET		No_Invalid_Account = (
--				SELECT COUNT(1) FROM (
--					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type = 'I'
--				) T
--			)			
-- ================================================================================================================

	-- CRE11-007 
	-- ================================================================================================================

	--Count of Alive Active Validated Account
	UPDATE	@ResultTableAccount
	SET		No_Validated_Account_Active = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type = 'V' AND Record_Status = 'A' AND Deceased='N'
				) T
			)
	WHERE CountType = 'Alive'
	
	--Count of Deceased Active Validated Account
	UPDATE	@ResultTableAccount
	SET		No_Validated_Account_Active = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type = 'V' AND Record_Status = 'A' AND Deceased='Y'
				) T
			)
	WHERE CountType = 'Deceased'

	--Count of Active Validated Account (Alive + Deceased)
	UPDATE	@ResultTableAccount
	SET		No_Validated_Account_Active = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type = 'V' AND Record_Status = 'A'
				) T
			)
	WHERE CountType = 'Total'

	--Count of Alive Suspended Validated Account
	UPDATE	@ResultTableAccount
	SET		No_Validated_Account_Suspended = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type = 'V' AND Record_Status = 'S' AND Deceased='N'
				) T
			)
	WHERE CountType = 'Alive'

	--Count of Deceased Suspended Validated Account
	UPDATE	@ResultTableAccount
	SET		No_Validated_Account_Suspended = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type = 'V' AND Record_Status = 'S' AND Deceased='Y'
				) T
			)
	WHERE CountType = 'Deceased'

	--Count of Suspended Validated Account (Alive + Deceased)
	UPDATE	@ResultTableAccount
	SET		No_Validated_Account_Suspended = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type = 'V' AND Record_Status = 'S'
				) T
			)
	WHERE CountType = 'Total'

	--Count of Alive Terminated Validated Account
	UPDATE	@ResultTableAccount
	SET		No_Validated_Account_Terminated = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type = 'V' AND Record_Status = 'D' AND Deceased='N'
				) T
			)
	WHERE CountType = 'Alive'
	
	--Count of Deceased Terminated Validated Account
	UPDATE	@ResultTableAccount
	SET		No_Validated_Account_Terminated = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type = 'V' AND Record_Status = 'D' AND Deceased='Y'
				) T
			)
	WHERE CountType = 'Deceased'
	
	--Count of Terminated Validated Account (Alive + Deceased)
	UPDATE	@ResultTableAccount
	SET		No_Validated_Account_Terminated = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type = 'V' AND Record_Status = 'D'
				) T
			)
	WHERE CountType = 'Total'

	-- ================================================================================================================
	
	--Count of Alive Account
	UPDATE	@ResultTableAccount
	SET		Total_Account = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type <> 'I' AND Deceased='N'
				) T
			)	
	WHERE CountType = 'Alive'
	
	--Count of Deceased Account
	UPDATE	@ResultTableAccount
	SET		Total_Account = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type <> 'I' AND Deceased='Y'
				) T
			)	
	WHERE CountType = 'Deceased'
	
	--Count of Account (Alive + Deceased)
	UPDATE	@ResultTableAccount
	SET		Total_Account = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 FROM #EHS01_GroupAccount WHERE Account_Type <> 'I'
				) T
			)	
	WHERE CountType = 'Total'

-- ---------------------------------------------
-- eHealth Account Summary by age group
-- ---------------------------------------------

/*
	INSERT INTO @ResultTableAge VALUES (0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)

	UPDATE	@ResultTableAge
	SET		ToSixMonth = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 
					FROM #EHS01_GroupAccount 
					WHERE DATEDIFF(dd, DOB, @Report_Dtm) < 182
				) T
			)
	
	UPDATE	@ResultTableAge
	SET		SixMonthToTwoYear = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 
					FROM #EHS01_GroupAccount 
					WHERE DATEDIFF(dd, DOB, @Report_Dtm) >= 182 AND DATEDIFF(yy, DOB_Adjust, @Report_Dtm) < 2
				) T
			)
	
	UPDATE	@ResultTableAge
	SET		TwoYearToSixYear = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 
					FROM #EHS01_GroupAccount 
					WHERE DATEDIFF(yy, DOB_Adjust, @Report_Dtm) >= 2 AND DATEDIFF(yy, DOB_Adjust, @Report_Dtm) < 6
				) T
			)
	
	UPDATE	@ResultTableAge
	SET		SixYearToNineYear = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 
					FROM #EHS01_GroupAccount 
					WHERE DATEDIFF(yy, DOB_Adjust, @Report_Dtm) >= 6 AND DATEDIFF(yy, DOB_Adjust, @Report_Dtm) < 9
				) T
			)
	
	UPDATE	@ResultTableAge
	SET		NineYearToElevenYear = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 
					FROM #EHS01_GroupAccount 
					WHERE DATEDIFF(yy, DOB_Adjust, @Report_Dtm) >= 9 AND DATEDIFF(yy, DOB_Adjust, @Report_Dtm) < 11
				) T
			)
	
	UPDATE	@ResultTableAge
	SET		ElevenYearToSixFiveYear = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 
					FROM #EHS01_GroupAccount 
					WHERE DATEDIFF(yy, DOB_Adjust, @Report_Dtm) >= 11 AND DATEDIFF(yy, DOB_Adjust, @Report_Dtm) < 65 AND DATEDIFF(yy, DOB, @Report_Dtm) < 65
				) T
			)
	
	UPDATE	@ResultTableAge
	SET		SixFiveYear = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 
					FROM #EHS01_GroupAccount 
					WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 65
				) T
			)
	
	UPDATE	@ResultTableAge
	SET		SixSixYear = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 
					FROM #EHS01_GroupAccount 
					WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 66
				) T
			)
	
	UPDATE	@ResultTableAge
	SET		SixSevenYear = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 
					FROM #EHS01_GroupAccount 
					WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 67
				) T
			)
	
	UPDATE	@ResultTableAge
	SET		SixEightYear = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 
					FROM #EHS01_GroupAccount 
					WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 68
				) T
			)
	
	UPDATE	@ResultTableAge
	SET		SixNineYear = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 
					FROM #EHS01_GroupAccount 
					WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 69
				) T
			)
	
	UPDATE	@ResultTableAge
	SET		SevenOYear = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 
					FROM #EHS01_GroupAccount 
					WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 70
				) T
			)
	
	UPDATE	@ResultTableAge
	SET		SevenOneYear = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 
					FROM #EHS01_GroupAccount 
					WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 71
				) T
			)
	
	UPDATE	@ResultTableAge
	SET		SevenTwoYear = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 
					FROM #EHS01_GroupAccount 
					WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 72
				) T
			)
	
	UPDATE	@ResultTableAge
	SET		SevenThreeYear = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 
					FROM #EHS01_GroupAccount 
					WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 73
				) T
			)
	
	UPDATE	@ResultTableAge
	SET		SevenFourYear = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 
					FROM #EHS01_GroupAccount 
					WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 74
				) T
			)
	
	UPDATE	@ResultTableAge
	SET		SevenFiveYear = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 
					FROM #EHS01_GroupAccount 
					WHERE DATEDIFF(yy, DOB, @Report_Dtm) = 75
				) T
			)
	
	UPDATE	@ResultTableAge
	SET		SevenFiveYearTo = (
				SELECT COUNT(1) FROM (
					SELECT DISTINCT Doc_Code, Encrypt_Field1 
					FROM #EHS01_GroupAccount 
					WHERE DATEDIFF(yy, DOB, @Report_Dtm) > 75
				) T
			)
	
	UPDATE	@ResultTableAge
	SET		Total = (
				CONVERT(int, ToSixMonth) +
				CONVERT(int, SixMonthToTwoYear) +
				CONVERT(int, TwoYearToSixYear) +
				CONVERT(int, SixYearToNineYear) +
				CONVERT(int, NineYearToElevenYear) +
				CONVERT(int, ElevenYearToSixFiveYear) +
				CONVERT(int, SixFiveYear) +
				CONVERT(int, SixSixYear) +
				CONVERT(int, SixSevenYear) +
				CONVERT(int, SixEightYear) +
				CONVERT(int, SixNineYear) +
				CONVERT(int, SevenOYear) +
				CONVERT(int, SevenOneYear) +
				CONVERT(int, SevenTwoYear) +
				CONVERT(int, SevenThreeYear) +
				CONVERT(int, SevenFourYear) +
				CONVERT(int, SevenFiveYear) +
				CONVERT(int, SevenFiveYearTo)
			)
*/

-- ---------------------------------------------
-- Insert to statistics table
-- ---------------------------------------------

-- Clear today record if exist
	
	DELETE _EHS_Account_ALL WHERE report_dtm = @Report_Dtm
	
	INSERT INTO _EHS_Account_ALL (
		system_dtm,
		report_dtm,
		validatedAC,
		specialAC,
		temporaryAC,
		validatedAC_Active,
		validatedAC_Suspended,
		validatedAC_Terminated,
		totalAC,
		CountType
	)
	SELECT
		GETDATE()
		,@Report_Dtm
		,No_Validated_Account							
		,No_Special_Account						
		,No_Temp_Account									
		,No_Validated_Account_Active			
		,No_Validated_Account_Suspended	
		,No_Validated_Account_Terminated	
		,Total_Account	
		,CountType											
	FROM
		@ResultTableAccount


		SELECT * FROM _EHS_Account_ALL
/*
	INSERT INTO _EHS_Account_Age (
		System_Dtm,
		Report_Dtm,
		ToSixMonth,
		SixMonthToTwoYear,
		TwoYearToSixYear,
		SixYearToNineYear,
		NineYearToElevenYear,
		ElevenYearToSixFiveYear,
		SixFiveYear,
		SixSixYear,
		SixSevenYear,
		SixEightYear,
		SixNineYear,
		SevenOYear,
		SevenOneYear,
		SevenTwoYear,
		SevenThreeYear,
		SevenFourYear,
		SevenFiveYear,
		SevenFiveYearTo,
		Total
	)
	SELECT
		GETDATE(),
		@Report_Dtm,
		ToSixMonth,
		SixMonthToTwoYear,
		TwoYearToSixYear,
		SixYearToNineYear,
		NineYearToElevenYear,
		ElevenYearToSixFiveYear,
		SixFiveYear,
		SixSixYear,
		SixSevenYear,
		SixEightYear,
		SixNineYear,
		SevenOYear,
		SevenOneYear,
		SevenTwoYear,
		SevenThreeYear,
		SevenFourYear,
		SevenFiveYear,
		SevenFiveYearTo,
		Total
	FROM
		@ResultTableAge
*/

-- ---------------------------------------------
-- Drop the temporary tables
-- ---------------------------------------------
		
	DROP TABLE #EHS01_AllAccount
	DROP TABLE #EHS01_GroupAccount


END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_AccountAll_Stat_Schedule] TO HCVU
GO
