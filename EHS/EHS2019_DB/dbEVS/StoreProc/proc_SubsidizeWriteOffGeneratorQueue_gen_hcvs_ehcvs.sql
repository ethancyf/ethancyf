IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SubsidizeWriteOffGeneratorQueue_gen_hcvs_ehcvs]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_SubsidizeWriteOffGeneratorQueue_gen_hcvs_ehcvs]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Modified by:	    Winnie SUEN
-- Modified date:   27 Nov 2017
-- Description:		(1) Count age by DOD - DOB if deceased
--					(2) Handle Eligibility age from 70 -> 65
--					(3) Add [DOD],[Exaxt_DOD] in SubsidizeWriteOffGeneratorQueue
-- =============================================
-- ==========================================================================================
-- Modification History
-- Author:	Karl LAM
-- CR No.:	CRE13-018
-- Create Date:	21 May 2014
-- Description:	Use count of write off records instead of Max Scheme Seq for determining full write off profile
-- ==========================================================================================
-- ==========================================================================================
-- Author:	Tommy LAM
-- CR No.:	CRE13-006
-- Create Date:	08 Nov 2013
-- Description:	Generate record into table - [SubsidizeWriteOffGeneratorQueue] for HCVS - EHCVS
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_SubsidizeWriteOffGeneratorQueue_gen_hcvs_ehcvs]
AS BEGIN
-- ============================================================
-- Declaration
-- ============================================================

	DECLARE @output_message AS varchar(50)
	DECLARE @current_date AS datetime
	DECLARE @scheme_code AS char(10)
	DECLARE @subsidize_code AS char(10)
	DECLARE @record_status_p AS char(1)

	-- The Scheme Sequence for Subsidize Write Off Calculation
	DECLARE @latest_scheme_seq AS smallint
	
	DECLARE @FullWriteOffProfileCount as smallint

	-- For HCVS - EHCVS Personal Information
	CREATE TABLE #PersonalInfo (
		Doc_Code		char(20),
		Encrypt_Field1	varbinary(100),
		DOB				datetime,
		Exact_DOB		char(1),
		DOD				datetime,
		Exact_DOD		char(1),
		Logical_DOD		datetime,
		Age				INT
	)

	-- For HCVS - EHCVS Eligible Personal Information
	CREATE TABLE #TempSubsidizeWriteOffGeneratorQueue (
		Doc_Code		char(20),
		Encrypt_Field1	varbinary(100),
		DOB				datetime,
		Exact_DOB		char(1),
		DOD				datetime,
		Exact_DOD		char(1)
	)

	-- For HCVS - EHCVS Calculated Write Off Record
	CREATE TABLE #TempSubsidizeWriteOff (
		Doc_Code		char(20),
		Encrypt_Field1	varbinary(100),
		DOB				datetime,
		Exact_DOB		char(1)
	)

	-- For Voucher Eligibility Age Setting
	DECLARE @AgeEligibility as Table (
		EffectiveDtm		Datetime,
		EligibleAge			Int
	)

-- ============================================================
-- Validation
-- ============================================================
-- ============================================================
-- Initialization
-- ============================================================

	SET @output_message = 'Record(s) generated successfully: '
	SET @current_date = GETDATE()
	SET @scheme_code = 'HCVS'
	SET @subsidize_code = 'EHCVS'
	SET @record_status_p = 'P'

	-- Get HCVS - EHCVS latest Scheme Sequence (Start)
	SELECT	@latest_scheme_seq = MAX(Scheme_Seq)
	FROM SubsidizeGroupClaim
	WHERE	Scheme_Code = @scheme_code
			AND Subsidize_Code = @subsidize_code
			AND Record_Status = 'A'
			AND Claim_Period_From <= @current_date
			AND DATEADD(dd, 1, Last_Service_Dtm) > @current_date
						
	SELECT	@FullWriteOffProfileCount = Count(Scheme_Seq)
	FROM SubsidizeGroupClaim
	WHERE	Scheme_Code = @scheme_code
			AND Subsidize_Code = @subsidize_code
			AND Record_Status = 'A'			
			AND Claim_Period_From <= @current_date
						
	IF @latest_scheme_seq IS NULL
		RETURN
	-- Get HCVS - EHCVS latest Scheme Sequence (End)

	-- Get HCVS - EHCVS Eligible Min. Age (Start)
	
	--	EffectiveDtm	EligibleAge
	--	-----------------------------
	--	2017-07-01		64
	--	2009-01-01		69
	--	-----------------------------

	-- Handle Eligibility age from 70 -> 65
	INSERT INTO @AgeEligibility (EffectiveDtm, EligibleAge)
	SELECT 
		MIN( ISNULL(ER2.Value, SGC.Claim_Period_From)) ,
		ER.Value
	FROM 
		SubsidizeGroupClaim SGC
		INNER JOIN EligibilityRule ER ON SGC.Scheme_Code = ER.Scheme_Code
									AND SGC.Scheme_Seq = ER.Scheme_Seq
									AND SGC.Subsidize_Code = ER.subsidize_code
									AND ER.Rule_Name = 'MinAge'
									AND ER.[Type] = 'AGE'
		LEFT JOIN EligibilityRule ER2 ON ER.Scheme_Code = ER2.Scheme_Code
									AND ER.Scheme_Seq = ER2.Scheme_Seq
									AND ER.Subsidize_Code = ER2.subsidize_code
									AND ER.Rule_Group_Code = ER2.Rule_Group_Code
									AND ER2.Rule_Name = 'ServiceOnOrAfterDate'
									AND ER2.[Type] = 'SERVICEDTM'									
	WHERE	
		SGC.Scheme_Code = @scheme_code
		AND SGC.Subsidize_Code = @subsidize_code
		AND SGC.Record_Status = 'A'
		AND SGC.Claim_Period_From <= @current_date
	GROUP BY
		ER.Value



-- Get HCVS - EHCVS Eligible Min. Age (End)

--------------------------------------------------------------------------------------


	-- Get HCVS - EHCVS Personal Information (Start)
	INSERT INTO #PersonalInfo (
		Doc_Code,
		Encrypt_Field1,
		DOB,
		Exact_DOB,
		DOD,
		Exact_DOD,
		Logical_DOD,
		Age
	)
	SELECT 			
		Doc_Code, Encrypt_Field1, DOB, Exact_DOB, DOD, Exact_DOD, Logical_DOD,
		CASE WHEN DOD IS NULL 
			THEN YEAR(@current_date) - YEAR(DOB) 
			ELSE YEAR(DOD) - YEAR(DOB) 
		END AS [Age]
	FROM
	(
		SELECT DISTINCT Doc_Code, Encrypt_Field1, DOB, Exact_DOB, DOD, Exact_DOD, DOD AS [Logical_DOD]
			FROM PersonalInformation
		UNION
		SELECT DISTINCT TPI.Doc_Code, TPI.Encrypt_Field1, TPI.DOB, TPI.Exact_DOB, TPI.DOD, TPI.Exact_DOD, TPI.DOD AS [Logical_DOD]
			FROM TempPersonalInformation TPI
				INNER JOIN TempVoucherAccount TVA
					ON TPI.Voucher_Acc_ID = TVA.Voucher_Acc_ID
			WHERE	(NOT(TVA.Record_Status IN ('D', 'V')))		-- Exclude Removed Account (D) and Validated Account (V)
					AND (NOT(TVA.Record_Status = 'I' AND TVA.Account_Purpose = 'O'))	-- Exclude Amendment Original Account
		UNION
		SELECT DISTINCT SPI.Doc_Code, SPI.Encrypt_Field1, SPI.DOB, SPI.Exact_DOB, SPI.DOD, SPI.Exact_DOD, SPI.DOD AS [Logical_DOD]
			FROM SpecialPersonalInformation SPI
				INNER JOIN SpecialAccount SA
					ON SPI.Special_Acc_ID = SA.Special_Acc_ID
			WHERE	(NOT(SA.Record_Status IN ('D', 'V')))		-- Exclude Removed Account (D) and Validated Account (V)
					AND (NOT(SA.Record_Status = 'I' AND SA.Account_Purpose = 'O'))	-- Exclude Amendment Original Account
	) PInfo 
	
	--Patch Logical DOD
	UPDATE #PersonalInfo set Logical_DOD = dateadd(day, -1, dateadd(month,1, Cast((Cast(Year(DOD) as char(4)) + '-' + cast(DATENAME(MONTH, DOD) as char(3)) + '-01')  as datetime))) -- End of Month
	WHERE Exact_DOD =  'M' 

	UPDATE #PersonalInfo set Logical_DOD = Cast((Cast(Year(DOD) as char(4)) + '-Dec-31')  as datetime) --End of Year
	WHERE Exact_DOD =  'Y' 

	-- Get HCVS - EHCVS Personal Information (End)


	-- Get HCVS - EHCVS Eligible Personal Information (Start)
	-- Only include account which eligible to claim voucher 
	INSERT INTO #TempSubsidizeWriteOffGeneratorQueue (
		Doc_Code,
		Encrypt_Field1,
		DOB,
		Exact_DOB,
		DOD,
		Exact_DOD
	)
	SELECT Doc_Code, Encrypt_Field1, DOB, Exact_DOB, DOD, Exact_DOD
		FROM #PersonalInfo PInfo
		CROSS APPLY
		(	
			-- Get latest Eligible age before deceased
			SELECT TOP 1
				EligibleAge
			FROM
				@AgeEligibility AE
			WHERE
				PInfo.Logical_DOD IS NULL OR PInfo.Logical_DOD >= AE.EffectiveDtm
			ORDER BY
				AE.EffectiveDtm DESC
		) AS T 
	WHERE 
		PInfo.Age >= T.EligibleAge + 1

	-- Get HCVS - EHCVS Eligible Personal Information (End)

	-- Get HCVS - EHCVS Calculated Write Off Record (Start)
	INSERT INTO #TempSubsidizeWriteOff (
		Doc_Code,
		Encrypt_Field1,
		DOB,
		Exact_DOB
	)
	SELECT
		Doc_Code,
		Encrypt_Field1,
		DOB,
		Exact_DOB
	FROM
		eHASubsidizeWriteOff
	WHERE
		Scheme_Code = @scheme_code		
		AND Subsidize_Code = @subsidize_code
	GROUP BY Doc_Code, Encrypt_Field1, DOB, Exact_DOB 
	HAVING count(Encrypt_Field1) = @FullWriteOffProfileCount
	-- Get HCVS - EHCVS Calculated Write Off Record (End)

-- ============================================================
-- Return results
-- ============================================================

	-- Delete all Records in [SubsidizeWriteOffGeneratorQueue]
	DELETE FROM SubsidizeWriteOffGeneratorQueue

	-- Generate Record into [SubsidizeWriteOffGeneratorQueue]
	INSERT INTO SubsidizeWriteOffGeneratorQueue (
		Row_ID,
		Doc_Code,
		Encrypt_Field1,
		DOB,
		Exact_DOB,
		DOD,
		Exact_DOD,
		Scheme_Code,
		Subsidize_Code,
		Create_Dtm,
		Update_Dtm,
		Record_Status
	)
	SELECT
		ROW_NUMBER() OVER(ORDER BY TWOGQ.Doc_Code, TWOGQ.Encrypt_Field1, TWOGQ.DOB, TWOGQ.Exact_DOB),
		TWOGQ.Doc_Code,
		TWOGQ.Encrypt_Field1,
		TWOGQ.DOB,
		TWOGQ.Exact_DOB,
		TWOGQ.DOD,
		TWOGQ.Exact_DOD,
		@scheme_code,
		@subsidize_code,
		@current_date,
		@current_date,
		@record_status_p
	FROM
		#TempSubsidizeWriteOffGeneratorQueue TWOGQ
			LEFT JOIN #TempSubsidizeWriteOff TSWO
				ON	TWOGQ.Doc_Code = TSWO.Doc_Code
					AND TWOGQ.Encrypt_Field1 = TSWO.Encrypt_Field1
					AND TWOGQ.DOB = TSWO.DOB
					AND TWOGQ.Exact_DOB = TSWO.Exact_DOB
					COLLATE DATABASE_DEFAULT
	WHERE TSWO.Doc_Code IS NULL

	-- Display Result
	SELECT @output_message + CONVERT(varchar(10), COUNT(1)) FROM SubsidizeWriteOffGeneratorQueue

END
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeWriteOffGeneratorQueue_gen_hcvs_ehcvs] TO HCVU
GO

