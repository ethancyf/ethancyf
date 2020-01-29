IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0024]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0024]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	28 November 2016
-- CR No.:			INT16-0022
-- Description:		Fix eHSD0024 counting of Pending Activation SP
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0027 Fix eHSD0024 scheme list
-- Modified by:		Tommy Lam
-- Modified date:	09 Dec 2013
-- Description:		Use [SchemeBackOffice].[Display_Code] instead of [SchemeEnrolClaimMap].[Scheme_Code_Claim] for display
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE13-016 Upgrade Excel verion to 2007
-- Modified by:		Karl Lam
-- Modified date:	17 Oct 2013
-- Description:		change @report_dtm_text to include HH:mm
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0015 - Fix internal report
-- Modified by:		Tommy Lam
-- Modified date:	08 Jul 2013
-- Description:		Include the Suspended Service Provider, Practice and Scheme Information in the Report of eHSD0024
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0013 Fix eHSD0024 scheme list
-- Modified by:		Tommy Lam
-- Modified date:	28 Jun 2013
-- Description:		Include the Expired Scheme Information in the Report of eHSD0024
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE13-005 Exclude the dummy SP A/C in the Report of eHSD0024
-- Modified by:		Tommy Lam
-- Modified date:	06 Jun 2013
-- Description:		Exclude the dummy SP A/C in the Report of eHSD0024
-- =============================================
-- =============================================
-- Author:			Tommy Lam
-- Create date:		15 May 2013
-- Description:		Get Report of eHSD0024
-- =============================================
--exec proc_EHS_eHSD0024
CREATE PROCEDURE [dbo].[proc_EHS_eHSD0024]
AS BEGIN

-- =============================================
-- Declaration
-- =============================================

	DECLARE @report_dtm			datetime
	DECLARE @report_dtm_text	varchar(50)

	CREATE TABLE #ReportResult (
		Seq		int,
		Col01	varchar(100)	DEFAULT '',
		Col02	varchar(30)		DEFAULT '',
		Col03	varchar(30)		DEFAULT '',
		Col04	varchar(30)		DEFAULT '',
		Col05	varchar(30)		DEFAULT '',
		Col06	varchar(30)		DEFAULT ''
	)

	CREATE TABLE #ReportData01 (
		Data01	int
	)

	CREATE TABLE #ReportData02 (
		Data01	varchar(100),
		Data02	int
	)

	DECLARE @AllActiveScheme TABLE (
		Scheme_Code		char(10),
		Display_Code	char(25)
	)

	DECLARE @AllActiveProfession TABLE (
		Service_Category_Code	varchar(3)
	)

	DECLARE @seq01	int
	DECLARE @seq02	int

-- =============================================
-- Initialization
-- =============================================
 
	SET @report_dtm = GETDATE()
	SET @report_dtm_text = 'Reporting period: as at ' + CONVERT(varchar(10), @report_dtm, 111) + ' ' + CONVERT(varchar(5), @report_dtm, 114) 

	INSERT INTO @AllActiveScheme (
		Scheme_Code,
		Display_Code
	)
	SELECT Scheme_Code, MIN(Display_Code)
		FROM SchemeBackOffice WITH (NOLOCK)
		WHERE @report_dtm >= Effective_Dtm AND Record_Status = 'A'
		GROUP BY Scheme_Code

	INSERT INTO @AllActiveProfession (
		Service_Category_Code
	)
	SELECT Service_Category_Code FROM Profession WITH (NOLOCK)
		WHERE (Enrol_Period_From IS NULL OR Enrol_Period_To IS NULL)
			OR (@report_dtm >= Enrol_Period_From)

	SET @seq01 = 0
	SET @seq02 = 0

-- **************************************************************************
-- Worksheet: Content (Start)
-- **************************************************************************

	SELECT 'Report Generation Time: ' + + CONVERT(varchar(10), @report_dtm, 111) + ' ' + CONVERT(varchar(5), @report_dtm, 114) 

-- ************************************************************************
-- Worksheet: Content (End)
-- ************************************************************************

-- **************************************************************************
-- eHSD0024-01: Approved Service Provider Summary Breakdown By Status (Start)
-- **************************************************************************

	-- -------------------------
	-- Declaration
	-- -------------------------

	DECLARE @no_of_sp_A		int
	DECLARE @no_of_sp_D		int
	DECLARE @no_of_sp_N		int
	DECLARE @no_of_sp_S		int
	DECLARE @no_of_sp_Total	int

	DECLARE @min	int
	DECLARE @max	int
	DECLARE @mode	int

	-- -------------------------
	-- Initialization
	-- -------------------------

	SET @seq01 = 3
	SET @seq02 = 6

	-- -------------------------
	-- Create Header Information
	-- -------------------------

	INSERT INTO #ReportResult (Seq, Col01) VALUES (0, @report_dtm_text)
	INSERT INTO #ReportResult (Seq) VALUES (1)
	INSERT INTO #ReportResult (Seq, Col01, Col02, Col03, Col04, Col05, Col06)
		VALUES (2, '', 'Active', 'Pending Activation', 'Suspended', 'Delisted', 'Total')
	INSERT INTO #ReportResult (Seq) VALUES (4)
	INSERT INTO #ReportResult (Seq, Col01, Col02, Col03, Col04)
		VALUES (5, '', 'Min', 'Max', 'Mode')

	-- -------------------------
	-- Retrieve Data
	-- -------------------------

	-- (Part 1)
	
	SELECT @no_of_sp_A = COUNT(1)
	FROM
		ServiceProvider SP WITH (NOLOCK)
			INNER JOIN HCSPUserAC H WITH (NOLOCK)
				ON SP.SP_ID = H.SP_ID
	WHERE
		SP.Record_Status = 'A'
			AND H.SP_Password IS NOT NULL
			AND (NOT EXISTS (SELECT 1 FROM SPExceptionList SPEL WITH (NOLOCK) WHERE SP.SP_ID = SPEL.SP_ID))

	SELECT @no_of_sp_D = COUNT(1) FROM ServiceProvider SP WITH (NOLOCK) WHERE SP.Record_Status = 'D' AND (NOT EXISTS (SELECT 1 FROM SPExceptionList SPEL WITH (NOLOCK) WHERE SP.SP_ID = SPEL.SP_ID))
	
	SELECT
		@no_of_sp_N = COUNT(1)
	FROM
		ServiceProvider SP WITH (NOLOCK)
			INNER JOIN HCSPUserAC H WITH (NOLOCK)
				ON SP.SP_ID = H.SP_ID
	WHERE
		SP.Record_Status = 'A'
			AND H.SP_Password IS NULL
			AND (NOT EXISTS (SELECT 1 FROM SPExceptionList SPEL WITH (NOLOCK) WHERE H.SP_ID = SPEL.SP_ID))

	SELECT @no_of_sp_S = COUNT(1) FROM ServiceProvider SP WITH (NOLOCK) WHERE SP.Record_Status = 'S' AND (NOT EXISTS (SELECT 1 FROM SPExceptionList SPEL WITH (NOLOCK) WHERE SP.SP_ID = SPEL.SP_ID))
	SELECT @no_of_sp_Total = COUNT(1) FROM ServiceProvider SP WITH (NOLOCK) WHERE (NOT EXISTS (SELECT 1 FROM SPExceptionList SPEL WITH (NOLOCK) WHERE SP.SP_ID = SPEL.SP_ID))

	INSERT INTO #ReportResult (Seq, Col01, Col02, Col03, Col04, Col05, Col06)
		VALUES (@seq01, 'No. of Service Provider', @no_of_sp_A, @no_of_sp_N, @no_of_sp_S, @no_of_sp_D, @no_of_sp_Total)
	SET @seq01 = @seq01 + 1

	-- (Part 2: No. of Practice)

	INSERT INTO
		#ReportData01 (Data01)
	SELECT
		COUNT(1)
	FROM
		Practice P WITH (NOLOCK)
	WHERE
		(P.Record_Status = 'A' OR P.Record_Status = 'S') AND (NOT EXISTS (SELECT 1 FROM SPExceptionList SPEL WITH (NOLOCK) WHERE P.SP_ID = SPEL.SP_ID))
	GROUP BY
		P.SP_ID
	
	SELECT @min = MIN(Data01), @max = MAX(Data01) FROM #ReportData01
	SELECT TOP 1 @mode = Data01 FROM #ReportData01 GROUP BY Data01 ORDER BY COUNT(1) DESC

	INSERT INTO #ReportResult (Seq, Col01, Col02, Col03, Col04)
		VALUES (@seq02, 'No. of Practice', @min, @max, @mode)
	SET @seq02 = @seq02 + 1

	TRUNCATE TABLE #ReportData01

	-- (Part 2: No. of Data Entry Account)

	INSERT INTO
		#ReportData01 (Data01)
	SELECT
		COUNT(1)
	FROM
		DataEntryUserAC DEUA WITH (NOLOCK)
			INNER JOIN ServiceProvider SP WITH (NOLOCK)
				ON DEUA.SP_ID = SP.SP_ID
	WHERE
		(SP.Record_Status = 'A' OR SP.Record_Status = 'S') AND (NOT EXISTS (SELECT 1 FROM SPExceptionList SPEL WITH (NOLOCK) WHERE DEUA.SP_ID = SPEL.SP_ID))
	GROUP BY
		DEUA.SP_ID

	SELECT @min = MIN(Data01), @max = MAX(Data01) FROM #ReportData01
	SELECT TOP 1 @mode = Data01 FROM #ReportData01 GROUP BY Data01 ORDER BY COUNT(1) DESC

	INSERT INTO #ReportResult (Seq, Col01, Col02, Col03, Col04)
		VALUES (@seq02, 'No. of Data Entry Account', @min, @max, @mode)
	SET @seq02 = @seq02 + 1

	TRUNCATE TABLE #ReportData01

	-- -------------------------
	-- Return results
	-- -------------------------

	SELECT
		Col01,
		Col02,
		Col03,
		Col04,
		Col05,
		Col06
	FROM
		#ReportResult
	ORDER BY
		Seq

	-- -------------------------
	-- Reset Global Value
	-- -------------------------

	SET @seq01 = 0
	SET @seq02 = 0
	TRUNCATE TABLE #ReportData01
	TRUNCATE TABLE #ReportResult

-- ************************************************************************
-- eHSD0024-01: Approved Service Provider Summary Breakdown By Status (End)
-- ************************************************************************

-- ******************************************************************************
-- eHSD0024-02: Approved Service Provider Summary Breakdown By Profession (Start)
-- ******************************************************************************

	-- -------------------------
	-- Declaration
	-- -------------------------

	DECLARE @no_of_sp_with_multi_prof	int

	-- -------------------------
	-- Initialization
	-- -------------------------

	SET @seq01 = 3

	-- -------------------------
	-- Create Header Information
	-- -------------------------

	INSERT INTO #ReportResult (Seq, Col01) VALUES (0, @report_dtm_text)
	INSERT INTO #ReportResult (Seq) VALUES (1)
	INSERT INTO #ReportResult (Seq, Col01, Col02) VALUES (2, 'Profession', 'No. of Service Provider')

	-- -------------------------
	-- Retrieve Data
	-- -------------------------

	-- (Part 1: Approved Service Provider Summary Breakdown By Profession)

	INSERT INTO #ReportData02 (Data01, Data02)
	SELECT
		P.Service_Category_Code,
		COUNT(1)
	FROM
		Professional P WITH (NOLOCK)
	WHERE
		P.Record_Status = 'A' AND (NOT EXISTS (SELECT 1 FROM SPExceptionList SPEL WITH (NOLOCK) WHERE P.SP_ID = SPEL.SP_ID))
	GROUP BY
		P.Service_Category_Code

	INSERT INTO #ReportResult (Seq, Col01, Col02)
	SELECT @seq01, AAP.Service_Category_Code, ISNULL(RD.Data02, 0)
		FROM @AllActiveProfession AAP
			LEFT JOIN #ReportData02 RD
				ON AAP.Service_Category_Code = RD.Data01 COLLATE DATABASE_DEFAULT

	SET @seq01 = @seq01 + 1

	INSERT INTO #ReportResult (Seq, Col01, Col02)
	SELECT @seq01, 'Total', SUM(Data02) FROM #ReportData02

	SET @seq01 = @seq01 + 1

	TRUNCATE TABLE #ReportData02

	-- (Part 2: No. of Service Provider with mulitple profession)

	INSERT INTO #ReportData02 (Data01, Data02)
	SELECT
		P.SP_ID,
		COUNT(1)
	FROM
		Professional P WITH (NOLOCK)
	WHERE
		P.Record_Status = 'A' AND (NOT EXISTS (SELECT 1 FROM SPExceptionList SPEL WITH (NOLOCK) WHERE P.SP_ID = SPEL.SP_ID))
	GROUP BY
		P.SP_ID

	SELECT @no_of_sp_with_multi_prof = COUNT(1) FROM #ReportData02 WHERE Data02 > 1

	INSERT INTO #ReportResult (Seq) VALUES (@seq01)
	SET @seq01 = @seq01 + 1

	INSERT INTO #ReportResult (Seq, Col01, Col02)
		VALUES (@seq01, 'No. of Service Provider with mulitple profession', @no_of_sp_with_multi_prof)

	SET @seq01 = @seq01 + 1

	TRUNCATE TABLE #ReportData02

	-- -------------------------
	-- Return results
	-- -------------------------

	SELECT
		Col01,
		Col02
	FROM
		#ReportResult
	ORDER BY
		Seq, Col01

	-- -------------------------
	-- Reset Global Value
	-- -------------------------

	SET @seq01 = 0
	TRUNCATE TABLE #ReportData02
	TRUNCATE TABLE #ReportResult

-- ****************************************************************************
-- eHSD0024-02: Approved Service Provider Summary Breakdown By Profession (End)
-- ****************************************************************************

-- **************************************************************************
-- eHSD0024-03: Approved Service Provider Summary Breakdown By Scheme (Start)
-- **************************************************************************

	-- -------------------------
	-- Declaration
	-- -------------------------
	-- -------------------------
	-- Initialization
	-- -------------------------

	SET @seq01 = 3

	-- -------------------------
	-- Create Header Information
	-- -------------------------

	INSERT INTO #ReportResult (Seq, Col01) VALUES (0, @report_dtm_text)
	INSERT INTO #ReportResult (Seq) VALUES (1)
	INSERT INTO #ReportResult (Seq, Col01, Col02) VALUES (2, 'Scheme', 'No. of Service Provider')

	-- -------------------------
	-- Retrieve Data
	-- -------------------------

	INSERT INTO #ReportData02 (Data01, Data02)
	SELECT
		SI.Scheme_Code,
		COUNT(1)
	FROM
		SchemeInformation SI WITH (NOLOCK)
	WHERE
		(SI.Record_Status = 'A' OR SI.Record_Status = 'S')
		AND (NOT EXISTS (SELECT 1 FROM SPExceptionList SPEL WITH (NOLOCK) WHERE SI.SP_ID = SPEL.SP_ID))
	GROUP BY
		SI.Scheme_Code

	INSERT INTO #ReportResult (Seq, Col01, Col02)
	SELECT @seq01, AAS.Display_Code, ISNULL(RD.Data02, 0)
		FROM @AllActiveScheme AAS
			LEFT JOIN #ReportData02 RD 
				ON AAS.Scheme_Code = RD.Data01 COLLATE DATABASE_DEFAULT

	SET @seq01 = @seq01 + 1

	INSERT INTO #ReportResult (Seq, Col01, Col02)
	SELECT @seq01, 'Total', SUM(Data02) FROM #ReportData02

	SET @seq01 = @seq01 + 1

	-- -------------------------
	-- Return results
	-- -------------------------

	SELECT
		Col01,
		Col02
	FROM
		#ReportResult
	ORDER BY
		Seq, Col01

	-- -------------------------
	-- Reset Global Value
	-- -------------------------

	SET @seq01 = 0
	TRUNCATE TABLE #ReportData02
	TRUNCATE TABLE #ReportResult

-- ************************************************************************
-- eHSD0024-03: Approved Service Provider Summary Breakdown By Scheme (End)
-- ************************************************************************

-- *************************************************************
-- eHSD0024-04: Practice Summary Breakdown By Profession (Start)
-- *************************************************************

	-- -------------------------
	-- Declaration
	-- -------------------------
	-- -------------------------
	-- Initialization
	-- -------------------------

	SET @seq01 = 3

	-- -------------------------
	-- Create Header Information
	-- -------------------------

	INSERT INTO #ReportResult (Seq, Col01) VALUES (0, @report_dtm_text)
	INSERT INTO #ReportResult (Seq) VALUES (1)
	INSERT INTO #ReportResult (Seq, Col01, Col02) VALUES (2, 'Profession', 'No. of Practice')

	-- -------------------------
	-- Retrieve Data
	-- -------------------------

	INSERT INTO #ReportData02 (Data01, Data02)
	SELECT
		PF.Service_Category_Code,
		COUNT(1)
	FROM
		Practice PT WITH (NOLOCK) INNER JOIN Professional PF WITH (NOLOCK)
			ON PT.SP_ID = PF.SP_ID AND PT.Professional_Seq = PF.Professional_Seq
	WHERE
		(PT.Record_Status = 'A' OR PT.Record_Status = 'S')
		AND PF.Record_Status = 'A'
		AND (NOT EXISTS (SELECT 1 FROM SPExceptionList SPEL WITH (NOLOCK) WHERE PT.SP_ID = SPEL.SP_ID))
	GROUP BY
		PF.Service_Category_Code

	INSERT INTO #ReportResult (Seq, Col01, Col02)
	SELECT @seq01, AAP.Service_Category_Code, ISNULL(RD.Data02, 0)
		FROM @AllActiveProfession AAP
			LEFT JOIN #ReportData02 RD
				ON AAP.Service_Category_Code = RD.Data01 COLLATE DATABASE_DEFAULT

	SET @seq01 = @seq01 + 1

	INSERT INTO #ReportResult (Seq, Col01, Col02)
	SELECT @seq01, 'Total', SUM(Data02) FROM #ReportData02

	SET @seq01 = @seq01 + 1

	-- -------------------------
	-- Return results
	-- -------------------------

	SELECT
		Col01,
		Col02
	FROM
		#ReportResult
	ORDER BY
		Seq, Col01

	-- -------------------------
	-- Reset Global Value
	-- -------------------------

	SET @seq01 = 0
	TRUNCATE TABLE #ReportData02
	TRUNCATE TABLE #ReportResult

-- ***********************************************************
-- eHSD0024-04: Practice Summary Breakdown By Profession (End)
-- ***********************************************************

-- *********************************************************
-- eHSD0024-05: Practice Summary Breakdown By Scheme (Start)
-- *********************************************************

	-- -------------------------
	-- Declaration
	-- -------------------------
	-- -------------------------
	-- Initialization
	-- -------------------------

	SET @seq01 = 3

	-- -------------------------
	-- Create Header Information
	-- -------------------------

	INSERT INTO #ReportResult (Seq, Col01) VALUES (0, @report_dtm_text)
	INSERT INTO #ReportResult (Seq) VALUES (1)
	INSERT INTO #ReportResult (Seq, Col01, Col02) VALUES (2, 'Scheme', 'No. of Practice')

	-- -------------------------
	-- Retrieve Data
	-- -------------------------

	INSERT INTO #ReportData02 (Data01, Data02)
	SELECT
		A.Scheme_Code,
		COUNT(1)
	FROM
		(SELECT PSI.Scheme_Code
			FROM PracticeSchemeInfo PSI WITH (NOLOCK)
			WHERE (PSI.Record_Status = 'A' OR PSI.Record_Status = 'S')
				AND (NOT EXISTS (SELECT 1 FROM SPExceptionList SPEL WITH (NOLOCK) WHERE PSI.SP_ID = SPEL.SP_ID))
			GROUP BY PSI.SP_ID, PSI.Practice_Display_Seq, PSI.Scheme_Code) AS A
	GROUP BY
		A.Scheme_Code

	INSERT INTO #ReportResult (Seq, Col01, Col02)
	SELECT @seq01, AAS.Display_Code, ISNULL(RD.Data02, 0)
		FROM @AllActiveScheme AAS
			LEFT JOIN #ReportData02 RD 
				ON AAS.Scheme_Code = RD.Data01 COLLATE DATABASE_DEFAULT

	SET @seq01 = @seq01 + 1

	INSERT INTO #ReportResult (Seq, Col01, Col02)
	SELECT @seq01, 'Total', SUM(Data02) FROM #ReportData02

	SET @seq01 = @seq01 + 1

	-- -------------------------
	-- Return results
	-- -------------------------

	SELECT
		Col01,
		Col02
	FROM
		#ReportResult
	ORDER BY
		Seq, Col01

	-- -------------------------
	-- Reset Global Value
	-- -------------------------

	SET @seq01 = 0
	TRUNCATE TABLE #ReportData02
	TRUNCATE TABLE #ReportResult

-- *******************************************************
-- eHSD0024-05: Practice Summary Breakdown By Scheme (End)
-- *******************************************************

-- ****************************************************
-- eHSD0024-06: Dummy Service Provider Accounts (Start)
-- ****************************************************

	-- -------------------------
	-- Declaration
	-- -------------------------
	-- -------------------------
	-- Initialization
	-- -------------------------
	-- -------------------------
	-- Create Header Information
	-- -------------------------
	-- -------------------------
	-- Retrieve Data
	-- -------------------------
	-- -------------------------
	-- Return results
	-- -------------------------

	SELECT SP_ID FROM dbo.SPExceptionList WITH (NOLOCK)

	-- -------------------------
	-- Reset Global Value
	-- -------------------------

-- **************************************************
-- eHSD0024-06: Dummy Service Provider Accounts (End)
-- **************************************************

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0024] TO HCVU
GO
