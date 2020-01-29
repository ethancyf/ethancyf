IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0005_02_PrepareData]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0005_02_PrepareData]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR. No			CRE14-016
-- Modified by:		Dickson Law			
-- Modified date:	02 Jan 2018
-- Description:		(1) Add Acccount Breakdown by Alive and Deceased
-- =============================================
-- =============================================  
-- CR No.:			CRE16-026
-- Modified by:     Marco CHOI
-- Modified date:   3 Aug 2017 
-- Description:     Add PCV13 
-- =============================================  
-- =============================================    
-- CR No.:			CRE16-002-04
-- Author:			Winnie SUEN
-- Create date:		30 Aug 2016
-- Description:		Retrieve Report on eHealth (Subsidies) accounts created by age
--					Copy from proc_EHS_eHealthAccount_Stat
-- =============================================   

CREATE PROCEDURE [dbo].[proc_EHS_eHSD0005_02_PrepareData] 
	@Cutoff_Dtm	datetime
AS BEGIN

-- =============================================
-- Declaration
-- =============================================
	DECLARE @Report_Dtm	datetime

	DECLARE @AccountTable AS table (
		Doc_Code					char(20),
		Encrypt_Field1				varbinary(100),
		DOB							datetime,
		DOB_Adjust					datetime,
		Exact_DOB					char(1),
		Create_Dtm					datetime,
		Earliest_Create_Dtm			datetime,
		Deceased					char(1),
		DOD							datetime,
		DOD_Adjust					datetime,
		Exact_DOD					char(1)
	)
	
	DECLARE @AccountTableT AS table (
		Doc_Code					char(20),
		Encrypt_Field1				varbinary(100),
		DOB							datetime,
		DOB_Adjust					datetime,
		Exact_DOB					char(1),
		Create_Dtm					datetime,
		Earliest_Create_Dtm			datetime,
		Deceased					char(1),
		DOD							datetime,
		DOD_Adjust					datetime,
		Exact_DOD					char(1)
	)
	
	DECLARE @AccountTableF AS table (
		Doc_Code					char(20),
		Encrypt_Field1				varbinary(100),
		DOB							datetime,
		DOB_Adjust					datetime,
		Exact_DOB					char(1),
		Create_Dtm					datetime,
		Earliest_Create_Dtm			datetime,
		Deceased					char(1),
		DOD							datetime,
		DOD_Adjust					datetime,
		Exact_DOD					char(1)
	)
	
	DECLARE @EarliestPersonalInformation AS table (
		Voucher_Acc_ID	char(15),
		Create_Dtm		datetime
	)
	
	DECLARE @DuplicatedID AS table (
		Encrypt_Field1	varbinary(100),
		Create_Dtm		datetime
	)
	
	DECLARE @DuplicatedID2 AS table (
		Doc_Code		char(20),
		Encrypt_Field1	varbinary(100),
		Create_Dtm		datetime
	)
	
	DECLARE @AccountTableAge AS table(
		Doc_Code		char(20),
		Encrypt_Field1	varbinary(100),
		AgeGroup		char(10), 
		Deceased		char(1)     
	)

	DECLARE @AccountTableAge_Pivot AS table(
		Deceased		char(1),
		AgeGroup		char(10), 
		GroupCount      INT    
	)



	DECLARE @ResultTable AS table(
		Result_Seq		smallint,
		Result_Value1	varchar(100),
		Result_Value2	varchar(100),
		Result_Value3	varchar(100),
		Result_Value4	varchar(100),
		Result_Value5	varchar(100),
		Result_Value6	varchar(100)
	)
	
	DECLARE @ResultTableAge table (
		[CountType]			VARCHAR(10),
		[Under6M]			INT,
		[6Mto2Y]			INT,
		[2Yto6Y] 			INT,
		[6Yto9Y] 			INT,
		[9Yto11Y]  			INT,
		[11Yto65Y]    		INT,
		[65Y]       		INT,
		[66Y]       		INT,
		[67Y]       		INT,
		[68Y]       		INT,
		[69Y]       		INT,
		[70Y]       		INT,
		[71Y]       		INT,
		[72Y]       		INT,
		[73Y]       		INT,
		[74Y]       		INT,
		[75Y]       		INT,
		[Over75Y] 			INT,
		[Total]				INT
	)
	
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	SET @Report_Dtm = DATEADD(dd, -1, @Cutoff_Dtm)


-- ---------------------------------------------
-- Validated Account
-- ---------------------------------------------

-- Prepare a list of personal information having the earliest create datetime

	INSERT INTO @EarliestPersonalInformation (
		Voucher_Acc_ID,
		Create_Dtm
	)
	SELECT
		PI.Voucher_Acc_ID,
		MIN(PI.Create_Dtm) AS [Create_Dtm]
	FROM
		VoucherAccount VA
			INNER JOIN PersonalInformation PI
				ON VA.Voucher_Acc_ID = PI.Voucher_Acc_ID
	WHERE
		VA.Effective_Dtm <= @Cutoff_Dtm
	GROUP BY
		PI.Voucher_Acc_ID
		
	INSERT INTO @AccountTable (
		Doc_Code,
		Encrypt_Field1,
		DOB,
		DOB_Adjust,
		Exact_DOB,
		Create_Dtm,
		Earliest_Create_Dtm,
		Deceased,
		DOD,
		DOD_Adjust,
		Exact_DOD
	)
	SELECT
		PI.Doc_Code,
		PI.Encrypt_Field1,
		PI.DOB,
		PI.DOB,
		PI.Exact_DOB,
		PI.Create_Dtm,
		EPI.Create_Dtm AS [Earliest_Create_Dtm],
		CASE WHEN PI.Deceased IS NULL THEN 'N' ELSE
			CASE WHEN PI.Deceased = 'Y' THEN 'Y' ELSE 'N' END
		END,
		PI.DOD, 
		PI.DOD,
		PI.Exact_DOD 
	FROM
		VoucherAccount VA
			INNER JOIN PersonalInformation PI
				ON VA.Voucher_Acc_ID = PI.Voucher_Acc_ID
			INNER JOIN @EarliestPersonalInformation EPI
				ON VA.Voucher_Acc_ID = EPI.Voucher_Acc_ID
	WHERE
			VA.Effective_Dtm <= @Cutoff_Dtm



	DELETE FROM
		@AccountTable
	WHERE
		Create_Dtm <> Earliest_Create_Dtm


-- Update all the validated account's Create_Dtm to be '1900-01-01' so that it is always earlier than temporary and special

	UPDATE
		@AccountTable
	SET
		Create_Dtm = '1900-01-01'
	
	
-- ---------------------------------------------
-- Temporary Account
-- ---------------------------------------------

	INSERT INTO @AccountTable (
		Doc_Code,
		Encrypt_Field1,
		DOB,
		DOB_Adjust,
		Exact_DOB,
		Create_Dtm,
		Deceased,
		DOD,
		DOD_Adjust,
		Exact_DOD
	)
	SELECT
		TP.Doc_Code,
		TP.Encrypt_Field1,
		TP.DOB,
		TP.DOB,
		TP.Exact_DOB,
		TP.Create_Dtm,
		CASE WHEN TP.Deceased IS NULL THEN 'N' ELSE
			CASE WHEN TP.Deceased = 'Y' THEN 'Y' ELSE 'N' END
		END,	 
		TP.DOD, 
		TP.DOD,
		TP.Exact_DOD 
	FROM
		TempVoucherAccount TA
			INNER JOIN TempPersonalInformation TP
				ON TA.Voucher_Acc_ID = TP.Voucher_Acc_ID
	WHERE
		TA.Record_Status NOT IN ('V', 'D')
			AND TA.Create_Dtm <= @Cutoff_Dtm
			AND TA.Account_Purpose IN ('C', 'V')


-- ---------------------------------------------
-- Special Account
-- ---------------------------------------------
		
	INSERT INTO @AccountTable (
		Doc_Code,
		Encrypt_Field1,
		DOB,
		DOB_Adjust,
		Exact_DOB,
		Create_Dtm,
		Deceased,
		DOD,
		DOD_Adjust,
		Exact_DOD
	)
	SELECT
		SP.Doc_Code,
		SP.Encrypt_Field1,
		SP.DOB,
		SP.DOB,
		SP.Exact_DOB,
		SP.Create_Dtm,
		CASE WHEN SP.Deceased IS NULL THEN 'N' ELSE
			CASE WHEN SP.Deceased = 'Y' THEN 'Y' ELSE 'N' END
		END,	
		SP.DOD, 
		SP.DOD,
		SP.Exact_DOD 
	FROM
		SpecialAccount SA
			INNER JOIN SpecialPersonalInformation SP
				ON SA.Special_Acc_ID = SP.Special_Acc_ID			
	WHERE
		SA.Record_Status NOT IN ('V', 'D')
			AND SA.Create_Dtm <= @Cutoff_Dtm
			AND SA.Account_Purpose IN ('C', 'V')

-- ---------------------------------------------
-- Handle the data with same [Doc_Code] + [Encrypt_Field1]
-- ---------------------------------------------

-- Handle HKIC and HKBC

	INSERT INTO @DuplicatedID (
		Encrypt_Field1,
		Create_Dtm
	)
	SELECT
		Encrypt_Field1,
		MIN(Create_Dtm) AS [Create_Dtm]
	FROM
		@AccountTable
	WHERE
		Doc_Code IN ('HKIC', 'HKBC')
	GROUP BY
		Encrypt_Field1
	
-- First insert the accounts of HKIC and HKBC into @AccountTableT
		
	INSERT INTO @AccountTableT (
		Doc_Code,
		Encrypt_Field1,
		DOB,
		DOB_Adjust,
		Exact_DOB,
		Create_Dtm,
		Earliest_Create_Dtm,
		Deceased,
		DOD,
		DOD_Adjust,
		Exact_DOD
	)
	SELECT
		A.Doc_Code,
		A.Encrypt_Field1,
		A.DOB,
		A.DOB,
		A.Exact_DOB,
		A.Create_Dtm,
		DI.Create_Dtm AS [Earliest_Create_Dtm],
		A.Deceased,
		A.DOD,
		A.DOD,
		A.Exact_DOD
	FROM
		@AccountTable A
			INNER JOIN @DuplicatedID DI
				ON A.Encrypt_Field1 = DI.Encrypt_Field1 
					AND Doc_Code IN ('HKIC', 'HKBC')

	DELETE FROM
		@AccountTableT
	WHERE
		Earliest_Create_Dtm IS NOT NULL
			AND Create_Dtm <> Earliest_Create_Dtm

		
-- Then insert the accounts of not HKIC and not HKBC into @AccountTableT
		
	INSERT INTO @AccountTableT (
		Doc_Code,
		Encrypt_Field1,
		DOB,
		DOB_Adjust,
		Exact_DOB,
		Create_Dtm,
		Earliest_Create_Dtm,
		Deceased,
		DOD,
		DOD_Adjust,
		Exact_DOD
	)
	SELECT
		A.Doc_Code,
		A.Encrypt_Field1,
		A.DOB,
		A.DOB,
		A.Exact_DOB,
		A.Create_Dtm,
		NULL AS [Earliest_Create_Dtm],
		A.Deceased,
		A.DOD,
		A.DOD,
		A.Exact_DOD
	FROM
		@AccountTable A
	WHERE
		A.Doc_Code NOT IN ('HKIC', 'HKBC')


-- Handle all documents

	INSERT INTO @DuplicatedID2 (
		Doc_Code, 
		Encrypt_Field1, 
		Create_Dtm
	)
	SELECT
		Doc_Code,
		Encrypt_Field1,
		MIN(Create_Dtm) AS [Create_Dtm]
	FROM
		@AccountTableT
	GROUP BY
		Doc_Code,
		Encrypt_Field1

	INSERT INTO @AccountTableF (
		Doc_Code,
		Encrypt_Field1,
		DOB,
		DOB_Adjust,
		Exact_DOB,
		Create_Dtm,
		Earliest_Create_Dtm,
		Deceased,
		DOD,
		DOD_Adjust,
		Exact_DOD
	)
	SELECT
		A.Doc_Code,
		A.Encrypt_Field1,
		A.DOB,
		A.DOB,
		A.Exact_DOB,
		A.Create_Dtm,
		DI.Create_Dtm AS [Earliest_Create_Dtm],
		A.Deceased,
		A.DOD,
		A.DOD,
		A.Exact_DOD
	FROM
		@AccountTableT A
			INNER JOIN @DuplicatedID2 DI
				ON A.Doc_Code = DI.Doc_Code
					AND A.Encrypt_Field1 = DI.Encrypt_Field1
	
	DELETE FROM
		@AccountTableF
	WHERE
		Earliest_Create_Dtm IS NOT NULL
			AND Create_Dtm <> Earliest_Create_Dtm



-- =============================================
-- Return results
-- =============================================

-- ---------------------------------------------
-- Patch data
-- ---------------------------------------------

	UPDATE
		@AccountTableF
	SET
		DOB = DATEADD(mm, DATEDIFF(mm, 0, DATEADD(mm, 1, DOB)), -1)
	WHERE
		Exact_DOB IN ('M', 'U')

	UPDATE
		@AccountTableF
	SET
		DOB_Adjust = DOB

	UPDATE
		@AccountTableF
	SET
		DOB_Adjust = DATEADD(yyyy, 1, DOB)
	WHERE
		MONTH(DOB) > MONTH(@Report_Dtm)
			OR ( MONTH(DOB) = MONTH(@Report_Dtm) AND DAY(DOB) > DAY(@Report_Dtm) )

	--Patch Exact DOD = M to last day of month
	UPDATE
		@AccountTableF
	SET
		DOD = DATEADD(mm, DATEDIFF(mm, 0, DATEADD(mm, 1, DOD)), -1)
	WHERE
		Exact_DOD IN ('M')

	--Patch Exact DOD = Y to last day of year
	UPDATE
		@AccountTableF
	SET
		DOD = DATEADD(yy, DATEDIFF(yy, 0, DATEADD(yy, 1, DOD)), -1)
	WHERE
		Exact_DOD IN ('Y')

	UPDATE
		@AccountTableF
	SET
		DOD_Adjust = DOD

	UPDATE
		@AccountTableF
	SET	
		DOD_Adjust = DATEADD(yyyy, -1, DOD)
	WHERE
		MONTH(DOB) > MONTH(DOD)
			OR ( MONTH(DOB) = MONTH(DOD) AND DAY(DOB) > DAY(DOD) )	

	INSERT INTO @AccountTableAge (
		Doc_Code,
		Encrypt_Field1,
		AgeGroup,	
		Deceased
	)
	SELECT 
		Doc_Code,
		Encrypt_Field1,
		'Under6M',
		Deceased
	FROM
		@AccountTableF
	WHERE 
		(DATEDIFF(dd, DOB, @Report_Dtm) < 182 AND Deceased ='N')
		OR 
		(DATEDIFF(dd, DOB, DOD) < 182 AND Deceased ='Y')
	
	
	INSERT INTO @AccountTableAge (
		Doc_Code,
		Encrypt_Field1,
		AgeGroup,	
		Deceased
	)
	SELECT 
		Doc_Code,
		Encrypt_Field1,
		'6Mto2Y',
		Deceased
	FROM
		@AccountTableF
	WHERE 
		(DATEDIFF(dd, DOB, @Report_Dtm) >= 182
			AND DATEDIFF(yyyy, DOB_Adjust, @Report_Dtm) < 2 
				AND Deceased ='N')
		OR 
		(DATEDIFF(dd, DOB, DOD) >= 182
			AND DATEDIFF(yyyy, DOB, DOD_Adjust) < 2  
				AND Deceased ='Y') 
	
	INSERT INTO @AccountTableAge (
		Doc_Code,
		Encrypt_Field1,
		AgeGroup,	
		Deceased
	)
	SELECT 
		Doc_Code,
		Encrypt_Field1,
		'2Yto6Y',
		Deceased
	FROM
		@AccountTableF
	WHERE 
		(DATEDIFF(yy, DOB_Adjust, @Report_Dtm) >= 2
			AND DATEDIFF(yyyy, DOB_Adjust, @Report_Dtm) < 6
				AND Deceased ='N')
		OR 
		(DATEDIFF(yy, DOB, DOD_Adjust) >= 2
			AND DATEDIFF(yyyy, DOB, DOD_Adjust) < 6
				AND Deceased ='Y') 
	
	INSERT INTO @AccountTableAge (
		Doc_Code,
		Encrypt_Field1,
		AgeGroup,	
		Deceased
	)
	SELECT 
		Doc_Code,
		Encrypt_Field1,
		'6Yto9Y',
		Deceased
	FROM
		@AccountTableF
	WHERE 
		(DATEDIFF(yy, DOB_Adjust, @Report_Dtm) >= 6
			AND DATEDIFF(yyyy, DOB_Adjust, @Report_Dtm) < 9
				AND Deceased ='N')
		OR 
		(DATEDIFF(yy, DOB, DOD_Adjust) >= 6
			AND DATEDIFF(yyyy, DOB, DOD_Adjust) < 9
				AND Deceased ='Y') 
	
	
	
	
	
	INSERT INTO @AccountTableAge (
		Doc_Code,
		Encrypt_Field1,
		AgeGroup,	
		Deceased
	)
	SELECT 
		Doc_Code,
		Encrypt_Field1,
		'9Yto11Y',
		Deceased
	FROM
		@AccountTableF
	WHERE 
		(DATEDIFF(yy, DOB_Adjust, @Report_Dtm) >= 9
			AND DATEDIFF(yyyy, DOB_Adjust, @Report_Dtm) < 11
				AND Deceased ='N')
		OR 
		(DATEDIFF(yy, DOB, DOD_Adjust) >= 9
			AND DATEDIFF(yyyy, DOB, DOD_Adjust) < 11
				AND Deceased ='Y') 
	
	INSERT INTO @AccountTableAge (
		Doc_Code,
		Encrypt_Field1,
		AgeGroup,	
		Deceased
	)
	SELECT 
		Doc_Code,
		Encrypt_Field1,
		'11Yto65Y',
		Deceased
	FROM
		@AccountTableF
	WHERE 
		(DATEDIFF(yy, DOB_Adjust, @Report_Dtm) >= 11
			AND DATEDIFF(yy, DOB_Adjust, @Report_Dtm) < 65 AND Deceased ='N')
		OR 
		(DATEDIFF(yy, DOB, DOD_Adjust) >= 11
			AND DATEDIFF(yyyy, DOB, DOD_Adjust) < 65 AND Deceased ='Y') 
	
	
	INSERT INTO @AccountTableAge (
		Doc_Code,
		Encrypt_Field1,
		AgeGroup,	
		Deceased
	)
	SELECT 
		Doc_Code,
		Encrypt_Field1,
		'65Y',
		Deceased
	FROM
		@AccountTableF
	WHERE 
		(DATEDIFF(yy, DOB, @Report_Dtm) = 65 AND DATEDIFF(yy, DOB_Adjust, @Report_Dtm) = 65 AND Deceased ='N')
		OR 
		(DATEDIFF(yy, DOB, DOD) = 65 AND DATEDIFF(yy, DOB, DOD_Adjust) = 65 AND Deceased ='Y')
	
	INSERT INTO @AccountTableAge (
		Doc_Code,
		Encrypt_Field1,
		AgeGroup,	
		Deceased
	)
	SELECT 
		Doc_Code,
		Encrypt_Field1,
		'66Y',
		Deceased
	FROM
		@AccountTableF
	WHERE 
		(DATEDIFF(yy, DOB, @Report_Dtm) = 66 AND Deceased ='N')
		OR 
		(DATEDIFF(yy, DOB, DOD) = 66 AND Deceased ='Y')
	
	
	INSERT INTO @AccountTableAge (
		Doc_Code,
		Encrypt_Field1,
		AgeGroup,	
		Deceased
	)
	SELECT 
		Doc_Code,
		Encrypt_Field1,
		'67Y',
		Deceased
	FROM
		@AccountTableF
	WHERE 
		(DATEDIFF(yy, DOB, @Report_Dtm) = 67 AND Deceased ='N')
		OR 
		(DATEDIFF(yy, DOB, DOD) = 67 AND Deceased ='Y')
	
	INSERT INTO @AccountTableAge (
		Doc_Code,
		Encrypt_Field1,
		AgeGroup,	
		Deceased
	)
	SELECT 
		Doc_Code,
		Encrypt_Field1,
		'68Y',
		Deceased
	FROM
		@AccountTableF
	WHERE 
		(DATEDIFF(yy, DOB, @Report_Dtm) = 68 AND Deceased ='N')
		OR 
		(DATEDIFF(yy, DOB, DOD) = 68 AND Deceased ='Y')
	
	INSERT INTO @AccountTableAge (
		Doc_Code,
		Encrypt_Field1,
		AgeGroup,	
		Deceased
	)
	SELECT 
		Doc_Code,
		Encrypt_Field1,
		'69Y',
		Deceased
	FROM
		@AccountTableF
	WHERE 
		(DATEDIFF(yy, DOB, @Report_Dtm) = 69 AND Deceased ='N')
		OR 
		(DATEDIFF(yy, DOB, DOD) = 69 AND Deceased ='Y')
	
	INSERT INTO @AccountTableAge (
		Doc_Code,
		Encrypt_Field1,
		AgeGroup,	
		Deceased
	)
	SELECT 
		Doc_Code,
		Encrypt_Field1,
		'70Y',
		Deceased
	FROM
		@AccountTableF
	WHERE 
		(DATEDIFF(yy, DOB, @Report_Dtm) = 70 AND Deceased ='N')
		OR 
		(DATEDIFF(yy, DOB, DOD) = 70 AND Deceased ='Y')
	
	INSERT INTO @AccountTableAge (
		Doc_Code,
		Encrypt_Field1,
		AgeGroup,	
		Deceased
	)
	SELECT 
		Doc_Code,
		Encrypt_Field1,
		'71Y',
		Deceased
	FROM
		@AccountTableF
	WHERE 
		(DATEDIFF(yy, DOB, @Report_Dtm) = 71 AND Deceased ='N')
		OR 
		(DATEDIFF(yy, DOB, DOD) = 71 AND Deceased ='Y')
	
	INSERT INTO @AccountTableAge (
		Doc_Code,
		Encrypt_Field1,
		AgeGroup,	
		Deceased
	)
	SELECT 
		Doc_Code,
		Encrypt_Field1,
		'72Y',
		Deceased
	FROM
		@AccountTableF
	WHERE 
		(DATEDIFF(yy, DOB, @Report_Dtm) = 72 AND Deceased ='N')
		OR 
		(DATEDIFF(yy, DOB, DOD) = 72 AND Deceased ='Y')
	
	INSERT INTO @AccountTableAge (
		Doc_Code,
		Encrypt_Field1,
		AgeGroup,	
		Deceased
	)
	SELECT 
		Doc_Code,
		Encrypt_Field1,
		'73Y',
		Deceased
	FROM
		@AccountTableF
	WHERE 
		(DATEDIFF(yy, DOB, @Report_Dtm) = 73 AND Deceased ='N')
		OR 
		(DATEDIFF(yy, DOB, DOD) = 73 AND Deceased ='Y')
	
	
	INSERT INTO @AccountTableAge (
		Doc_Code,
		Encrypt_Field1,
		AgeGroup,	
		Deceased
	)
	SELECT 
		Doc_Code,
		Encrypt_Field1,
		'74Y',
		Deceased
	FROM
		@AccountTableF
	WHERE 
		(DATEDIFF(yy, DOB, @Report_Dtm) = 74 AND Deceased ='N')
		OR 
		(DATEDIFF(yy, DOB, DOD) = 74 AND Deceased ='Y')
	
	INSERT INTO @AccountTableAge (
		Doc_Code,
		Encrypt_Field1,
		AgeGroup,	
		Deceased
	)
	SELECT 
		Doc_Code,
		Encrypt_Field1,
		'75Y',
		Deceased
	FROM
		@AccountTableF
	WHERE 
		(DATEDIFF(yy, DOB, @Report_Dtm) = 75 AND Deceased ='N')
		OR 
		(DATEDIFF(yy, DOB, DOD) = 75 AND Deceased ='Y')
	

	INSERT INTO @AccountTableAge (
		Doc_Code,
		Encrypt_Field1,
		AgeGroup,	
		Deceased
	)
	SELECT 
		Doc_Code,
		Encrypt_Field1,
		'Over75Y',
		Deceased
	FROM
		@AccountTableF
	WHERE 
		(DATEDIFF(yy, DOB, @Report_Dtm) > 75 AND Deceased ='N')
		OR 
		(DATEDIFF(yy, DOB, DOD) > 75 AND Deceased ='Y')
	
	

	INSERT INTO @ResultTableAge (
		[CountType]
		,[Under6M]
		,[6Mto2Y]
		,[2Yto6Y]
		,[6Yto9Y]
		,[9Yto11Y]
		,[11Yto65Y]
		,[65Y]
		,[66Y]
		,[67Y]
		,[68Y]
		,[69Y]
		,[70Y]
		,[71Y]
		,[72Y]
		,[73Y]
		,[74Y]
		,[75Y]
		,[Over75Y]
		,[Total]
	)
	SELECT
		CASE 
			WHEN Deceased ='N' 
				Then 'Alive'
				ELSE 'Deceased'
		END AS CountType,
		CONVERT(INT,ISNULL(pvt.[Under6M],0)) AS [Under6M],
		CONVERT(INT,ISNULL(pvt.[6Mto2Y],0)) AS [6Mto2Y],
		CONVERT(INT,ISNULL(pvt.[2Yto6Y],0)) AS [2Yto6Y],
		CONVERT(INT,ISNULL(pvt.[6Yto9Y],0)) AS [6Yto9Y],
		CONVERT(INT,ISNULL(pvt.[9Yto11Y],0)) AS [9Yto11Y],
		CONVERT(INT,ISNULL(pvt.[11Yto65Y],0)) AS [12Yto65Y],
		CONVERT(INT,ISNULL(pvt.[65Y],0)) AS [65Y],
		CONVERT(INT,ISNULL(pvt.[66Y],0)) AS [66Y],
		CONVERT(INT,ISNULL(pvt.[67Y],0)) AS [67Y],
		CONVERT(INT,ISNULL(pvt.[68Y],0)) AS [68Y],
		CONVERT(INT,ISNULL(pvt.[69Y],0)) AS [69Y],
		CONVERT(INT,ISNULL(pvt.[70Y],0)) AS [70Y],
		CONVERT(INT,ISNULL(pvt.[71Y],0)) AS [71Y],
		CONVERT(INT,ISNULL(pvt.[72Y],0)) AS [72Y],
		CONVERT(INT,ISNULL(pvt.[73Y],0)) AS [73Y],
		CONVERT(INT,ISNULL(pvt.[74Y],0)) AS [74Y],
		CONVERT(INT,ISNULL(pvt.[75Y],0)) AS [75Y],
		CONVERT(INT,ISNULL(pvt.[Over75Y],0)) AS [Over75Y],
		(CONVERT(INT,ISNULL(pvt.[Under6M],0))
			+CONVERT(INT,ISNULL(pvt.[6Mto2Y],0))
			+CONVERT(INT,ISNULL(pvt.[2Yto6Y],0))
			+CONVERT(INT,ISNULL(pvt.[6Yto9Y],0))
			+CONVERT(INT,ISNULL(pvt.[9Yto11Y],0))
			+CONVERT(INT,ISNULL(pvt.[11Yto65Y],0))
			+CONVERT(INT,ISNULL(pvt.[65Y],0))
			+CONVERT(INT,ISNULL(pvt.[66Y],0))
			+CONVERT(INT,ISNULL(pvt.[67Y],0)) 
			+CONVERT(INT,ISNULL(pvt.[68Y],0))
			+CONVERT(INT,ISNULL(pvt.[69Y],0)) 
			+CONVERT(INT,ISNULL(pvt.[70Y],0)) 
			+CONVERT(INT,ISNULL(pvt.[71Y],0))
			+CONVERT(INT,ISNULL(pvt.[72Y],0)) 
			+CONVERT(INT,ISNULL(pvt.[73Y],0)) 
			+CONVERT(INT,ISNULL(pvt.[74Y],0))
			+CONVERT(INT,ISNULL(pvt.[75Y],0))
			+CONVERT(INT,ISNULL(pvt.[Over75Y],0))) AS [TOTAL]
	FROM 
	(
	SELECT
		DISTINCT Deceased AS Deceased,
		AgeGroup,
		Count(1) AS GroupCount
	FROM 
		@AccountTableAge	
	GROUP BY 
		Deceased,AgeGroup
	) AS T
	PIVOT (
		MAX(GroupCount) 
		 FOR AgeGroup in (
		 [Under6M],[6Mto2Y],[2Yto6Y],[6Yto9Y],[9Yto11Y],[11Yto65Y],[65Y],[66Y],[67Y],[68Y],[69Y],[70Y],[71Y],[72Y],[73Y],[74Y],[75Y],[Over75Y],[Total])
	) AS pvt


	INSERT INTO @ResultTableAge (
		[CountType]
		,[Under6M]
		,[6Mto2Y]
		,[2Yto6Y]
		,[6Yto9Y]
		,[9Yto11Y]
		,[11Yto65Y]
		,[65Y]
		,[66Y]
		,[67Y]
		,[68Y]
		,[69Y]
		,[70Y]
		,[71Y]
		,[72Y]
		,[73Y]
		,[74Y]
		,[75Y]
		,[Over75Y]
		,[Total]
	)
	SELECT
		'Total' AS [CountType],
		CONVERT(INT,ISNULL(pvt.[Under6M],0)) AS [Under6M],
		CONVERT(INT,ISNULL(pvt.[6Mto2Y],0)) AS [6Mto2Y],
		CONVERT(INT,ISNULL(pvt.[2Yto6Y],0)) AS [2Yto6Y],
		CONVERT(INT,ISNULL(pvt.[6Yto9Y],0)) AS [6Yto9Y],
		CONVERT(INT,ISNULL(pvt.[9Yto11Y],0)) AS [9Yto11Y],
		CONVERT(INT,ISNULL(pvt.[11Yto65Y],0)) AS [11Yto65Y],
		CONVERT(INT,ISNULL(pvt.[65Y],0)) AS [65Y],
		CONVERT(INT,ISNULL(pvt.[66Y],0)) AS [66Y],
		CONVERT(INT,ISNULL(pvt.[67Y],0)) AS [67Y],
		CONVERT(INT,ISNULL(pvt.[68Y],0)) AS [68Y],
		CONVERT(INT,ISNULL(pvt.[69Y],0)) AS [69Y],
		CONVERT(INT,ISNULL(pvt.[70Y],0)) AS [70Y],
		CONVERT(INT,ISNULL(pvt.[71Y],0)) AS [71Y],
		CONVERT(INT,ISNULL(pvt.[72Y],0)) AS [72Y],
		CONVERT(INT,ISNULL(pvt.[73Y],0)) AS [73Y],
		CONVERT(INT,ISNULL(pvt.[74Y],0)) AS [74Y],
		CONVERT(INT,ISNULL(pvt.[75Y],0)) AS [75Y],
		CONVERT(INT,ISNULL(pvt.[Over75Y],0)) AS [Over75Y],
		(CONVERT(INT,ISNULL(pvt.[Under6M],0))
			+CONVERT(INT,ISNULL(pvt.[6Mto2Y],0))
			+CONVERT(INT,ISNULL(pvt.[2Yto6Y],0))
			+CONVERT(INT,ISNULL(pvt.[6Yto9Y],0))
			+CONVERT(INT,ISNULL(pvt.[9Yto11Y],0))
			+CONVERT(INT,ISNULL(pvt.[11Yto65Y],0))
			+CONVERT(INT,ISNULL(pvt.[65Y],0))
			+CONVERT(INT,ISNULL(pvt.[66Y],0))
			+CONVERT(INT,ISNULL(pvt.[67Y],0)) 
			+CONVERT(INT,ISNULL(pvt.[68Y],0))
			+CONVERT(INT,ISNULL(pvt.[69Y],0)) 
			+CONVERT(INT,ISNULL(pvt.[70Y],0)) 
			+CONVERT(INT,ISNULL(pvt.[71Y],0))
			+CONVERT(INT,ISNULL(pvt.[72Y],0)) 
			+CONVERT(INT,ISNULL(pvt.[73Y],0)) 
			+CONVERT(INT,ISNULL(pvt.[74Y],0))
			+CONVERT(INT,ISNULL(pvt.[75Y],0))
			+CONVERT(INT,ISNULL(pvt.[Over75Y],0))) AS [TOTAL]
	FROM 
	(
		SELECT
			DISTINCT AgeGroup,
			Count(1) AS GroupCount
		FROM 
			@AccountTableAge
		GROUP BY 
			AgeGroup
	) AS T
	PIVOT (
		MAX(GroupCount) 
		 FOR AgeGroup in (
		 [Under6M],[6Mto2Y],[2Yto6Y],[6Yto9Y],[9Yto11Y],[11Yto65Y],[65Y],[66Y],[67Y],[68Y],[69Y],[70Y],[71Y],[72Y],[73Y],[74Y],[75Y],[Over75Y],[Total])
	) AS pvt


-- ---------------------------------------------
-- Build result
-- ---------------------------------------------

	--INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6) VALUES
	--(0, 'eHS(S)D0028-02: Report on eHealth (Subsidies) accounts created (by age)', '', '', '', '','')
	--
	--INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6) VALUES
	--(1, '', '', '', '', '','')
	--
	--INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6) VALUES
	--(2, 'Reporting period: as at ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111), '', '', '', '','')
	--
	--INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6) VALUES
	--(3, '', '', '', '', '','')
	--
	--INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6) VALUES
	--(10,'', '6 months to less than 6 years', '6 years to less than 12 years', '12 years to less than 65 years', '>= 65 years', 'Total')
	--
	--
	--INSERT INTO @ResultTable (
	--	Result_Seq, 
	--	Result_Value1, 
	--	Result_Value2, 
	--	Result_Value3, 
	--	Result_Value4, 
	--	Result_Value5,
	--	Result_Value6
	--) 
	--SELECT 
	--11 + ROW_NUMBER() OVER(ORDER BY CountType ASC) AS [Display_Seq],
	--CountType,
	--[6Mto2Y] + [2Yto6Y] AS '6Mto6Y',
	--[6Yto9Y]+[9Yto11Y]+[11Yto12Y]  AS '6Yto12Y',
	--[12Yto65Y]AS 'Over65Y',
	--[65Y]
	--+ [66Y]
	--+ [67Y]
	--+ [68Y]
	--+ [69Y]
	--+ [70Y]
	--+ [71Y]
	--+ [72Y]
	--+ [73Y]
	--+ [74Y]
	--+ [75Y]
	--+ [Over75Y] AS 'Equal&Over65Y',
	--([6Mto2Y]
	--	+ [2Yto6Y]
	--	+ [6Yto9Y]
	--	+ [9Yto11Y]
	--	+ [11Yto12Y] 
	--	+ [12Yto65Y]
	--	+ [65Y]
	--	+ [66Y]
	--	+ [67Y]
	--	+ [68Y]
	--	+ [69Y]
	--	+ [70Y]
	--	+ [71Y]
	--	+ [72Y]
	--	+ [73Y]
	--	+ [74Y]
	--	+ [75Y]
	--	+ [Over75Y])
	--As 'Total'
	--FROM
	--@ResultTableAge

-- ---------------------------------------------
-- eHS(S)D0005-02 - eHealth Account Summary by age group
-- ---------------------------------------------


-- ---------------------------------------------
-- Insert to statistics table
-- ---------------------------------------------
	
	DELETE FROM RpteHSD0005_02_eHA_ByAge
	--
	INSERT INTO RpteHSD0005_02_eHA_ByAge (
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
		Total,
		CountType
	)
	SELECT
		GETDATE(),
		@Report_Dtm	
		,[Under6M]	
		,[6Mto2Y]	
		,[2Yto6Y] 	
		,[6Yto9Y] 	
		,[9Yto11Y]  	
		,[11Yto65Y]
		,[65Y]       
		,[66Y]       
		,[67Y]       
		,[68Y]       
		,[69Y]       
		,[70Y]       
		,[71Y]       
		,[72Y]       
		,[73Y]       
		,[74Y]       
		,[75Y]       
		,[Over75Y] 	
		,[Total]
		,[CountType]		
	FROM
		@ResultTableAge


END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0005_02_PrepareData] TO HCVU
GO
