IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSU0005_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSU0005_Report_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR# :			CRE20-018-02
-- Modified by:		Koala CHENG
-- Modified date:	10 Mar 2021
-- Description:		DIsplay "eHS(S) from eHRSS" for eHRSS token
-- =============================================
-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- CR No.			CRE20-014-02 (Gov SIV 2020_21 - Phase 2)
-- Modified by:		Koala CHENG
-- Modified date:	06 Nov 2020
-- Description:		[03-Practice & BankAcc] Move Gov SIV to the end of VSS vaccine list
-- =============================================
-- =============================================
-- Modification History
-- CR No.			CRE16-022 (SDIR Remark)
-- Modified by:		CHRIS YIM
-- Modified date:	17 Feb 2020
-- Description:		Add columns [Mobile_Clinic],[Remarks_Desc] & [Remarks_Desc_Chi]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:		  CRE19-0XX
-- Modified by:	  Koala CHENG
-- Modified date: 14 Nov 2019
-- Description:	  1. Excel Sheet (01): Add "Provide DHC-related Services (Any practices)"
--				  2. Excel Sheet (03): Add "Provide DHC-related Services"
-- =============================================
-- =============================================
-- Modification History
-- CR No.:		  CRE17-016
-- Modified by:	  Koala CHENG
-- Modified date: 08 Aug 2018
-- Description:	  1. Excel Sheet (03): Add "PCD Status", "PCD Professional" and "Last Check Date of PCD Status"
--				  2. Excel Sheet (05): Add "SP PCD Status", "SP PCD Professional" and "SP Last Check Date of PCD Status"
-- =============================================
-- =============================================
-- Modification History
-- CR No.:		  CRE17-013
-- Modified by:	  Chris YIM
-- Modified date: 27 Feb 2018
-- Description:	  Extend bank account name to 300 chars
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	11 Apr 2017
-- CR No.:			CRE17-001
-- Description:		1. Change input parameter (@request_time, @ProfessionList) to nullable for eHSM0005 monthly basis report
--					2. Excel Sheet (02): Criteria, always show detail profession lists (Not show "Any")
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE16-018 (Display SP tentative email in HCVU)
-- Modified by:		Winnie SUEN
-- Modified date:	11 Nov 2016
-- Description:		For Sub-report - "eHSU0005-01":
--						1. Change the header "Tentative Email Address" to "Pending Email Address"
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE16-009 (To enable the eHS(S) to display Chinese Unicode in the field of "English Name of M.O.")
-- Modified by:		Koala CHENG
-- Modified date:	07 Sep 2016
-- Description:		Change MO English name from VARCHAR(100) to NVARCHAR(100)
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE16-002
-- Modified by:		Lawrence TSANG
-- Modified date:	9 August 2016
-- Description:		Add Clinic_Type
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	18 November 2015
-- CR No.:			CRE15-006
-- Description:		Rename of eHS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Winnie SUEN
-- Modified date: 15 July 2015
-- Description:	1. Don't retrieve those subsidizegroup which record_status = 'I' 
--				2. For Sub-report - "eHSU0005-01":
--						1. Add (With Active Practice Enrolled in [Scheme])
--				3. For Sub-report - "eHSU0005-03":
--						1. Display "No Service Provided" for subsidy which does not provide service
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Winnie SUEN
-- Modified date: 21 Apr 2015
-- Description:	1. Refine District Structure
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT14-0035 - Fix eHSU0005-01 Missing SP who without token
-- Modified by:		Chris YIM
-- Modified date:	16 Dec 2014
-- Description:		For Sub-report - "eHSU0005-01":
--						1. Include SP who is/are without token
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE13-008 - SP Amendment Report
-- Modified by:		Chris YIM
-- Modified date:	24 Oct 2014
-- Description:		For Sub-report - "eHSU0005-01":
--						1. Re-order the column "Tentative Email Address" between the column "Email Address" and "Fax No."
--						2. Status Description instead of Status Record
--					For Sub-report - "eHSU0005-03":
--						1. Update service fee with "No QIV"
--						2. Status Description instead of Status Record
--					For "Remarks"
--						1. Remove Scheme Status
-- =============================================
-- =============================================
-- Author:			Tommy Lam
-- Create date:		21 Nov 2013
-- CR No.:			CRE13-008 - SP Amendment Report
-- Description:		Get Report for eHSU0005
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSU0005_Report_get]
	@request_time	datetime = NULL, -- For filtering the back office effective scheme
	@ProfessionList	VARCHAR(5000) = NULL
AS BEGIN

-- =============================================
-- Declaration
-- =============================================

	DECLARE @current_dtm	datetime
	DECLARE @delimiter		VARCHAR(3)
	DECLARE @seq			int

	DECLARE @ProfessionListTemp TABLE (
		Service_Category_Code VARCHAR(3)
	)

	DECLARE @SchemeBackOffice TABLE (
		Scheme_Code		char(10),
		Display_Code	char(25),
		Display_Seq		smallint,
		Scheme_Desc		VARCHAR(100)
	)

	DECLARE @SubsidizeGroupBackOffice TABLE (
		Scheme_Code			char(10),
		Subsidize_Code		char(10),
		Scheme_Display_Seq	smallint,
		Display_Seq			smallint,
		Display_Desc		VARCHAR(50),
		-- 'CRE13-008 - SP Amendment Report [Start][Chris YIM]
		-- -----------------------------------------------------------------------------------------
		Service_Fee_Compulsory	char(1),
		Service_Fee_Compulsory_Wording	VARCHAR(100),	
		-- 'CRE13-008 - SP Amendment Report [End][Chris YIM]
		Record_Status		char(1)--' CRE15-004 TIV & QIV [Winnie]		
	)
	

	DECLARE @ServiceProviderStatus TABLE (
		Status_Value		char(3),
		Status_Description	VARCHAR(100)
	)

	DECLARE @MORelationship TABLE (
		Relationship		char(5),
		Relationship_Desc	VARCHAR(4000)
	)

	DECLARE @MOStatus TABLE (
		Status_Value		char(3),
		Status_Description	VARCHAR(100)
	)

	DECLARE @PracticeStatus TABLE (
		Status_Value		char(3),
		Status_Description	VARCHAR(100)
	)

	CREATE TABLE #SP_Filtered (
		SP_ID char(8)
	)

	CREATE TABLE #Practice_Filtered (
		SP_ID			char(8),
		Display_Seq		smallint
	)

	CREATE TABLE #SP_Filtered_With_Scheme_Info (
		SP_ID					char(8),
		Scheme_Display_Code		char(25),
		-- 'CRE13-008 - SP Amendment Report [Start][Chris YIM]
		-- -----------------------------------------------------------------------------------------
		Scheme_Record_Status	VARCHAR(100)
		-- 'CRE13-008 - SP Amendment Report [End][Chris YIM]
	)
	
	--' CRE15-004 TIV & QIV [Start][Winnie]
	CREATE TABLE #SP_Filtered_With_Practice_Scheme_Info (
		SP_ID					char(8),
		Scheme_Display_Code		char(25),
		Scheme_Record_Status	VARCHAR(100)
	)	
	--' CRE15-004 TIV & QIV [End][Winnie]
	
	CREATE TABLE #Practice_Filtered_With_Scheme_Info (
		SP_ID					char(8),
		Display_Seq				smallint,
		Scheme_Display_Code		char(25),
		-- 'CRE13-008 - SP Amendment Report [Start][Chris YIM]
		-- -----------------------------------------------------------------------------------------
		Scheme_Record_Status	VARCHAR(100)
		-- 'CRE13-008 - SP Amendment Report [End][Chris YIM]
	)

	CREATE TABLE #Practice_Filtered_With_Subsidize_Info (
		SP_ID					char(8),
		Display_Seq				smallint,
		Display_Desc			VARCHAR(50),
		-- 'CRE13-008 - SP Amendment Report [Start][Chris YIM]
		-- -----------------------------------------------------------------------------------------
		Service_Fee				VARCHAR(100)
		-- 'CRE13-008 - SP Amendment Report [End][Chris YIM]
	)

	DECLARE @pivot_table_column_header		VARCHAR(MAX)
	DECLARE @pivot_table_column_list		VARCHAR(MAX)
	DECLARE @pivot_table_column_name_alias	VARCHAR(MAX)
	DECLARE @pivot_table_column_name_value	VARCHAR(MAX)

	DECLARE @pivot_table_subsidize_column_header		VARCHAR(MAX)
	DECLARE @pivot_table_subsidize_column_list			VARCHAR(MAX)
	DECLARE @pivot_table_subsidize_column_name_alias	VARCHAR(MAX)
	DECLARE @pivot_table_subsidize_column_name_value	VARCHAR(MAX)

	--' CRE15-004 TIV & QIV [Start][Winnie]
	DECLARE @pivot_table_practice_scheme_column_header		VARCHAR(MAX)
	DECLARE @pivot_table_practice_scheme_column_list		VARCHAR(MAX)
	DECLARE @pivot_table_practice_scheme_column_name_alias	VARCHAR(MAX)
	DECLARE @pivot_table_practice_scheme_column_name_value	VARCHAR(MAX)	
	--' CRE15-004 TIV & QIV [End][Winnie]
	
	DECLARE @sql_script						VARCHAR(MAX)
	DECLARE @sql_script_pivot_table_insert	VARCHAR(MAX)

-- =============================================
-- Validation
-- =============================================
	IF @request_time IS NULL
		SET @request_time = GETDATE()
		
	IF @ProfessionList IS NULL
		SET @ProfessionList = ''

-- =============================================
-- Initialization
-- =============================================

	SET @current_dtm = GETDATE()
	SET @delimiter = ','

-- ---------------------------------------------
-- @ProfessionListTemp
-- ---------------------------------------------

	IF @ProfessionList = ''
		BEGIN
			INSERT INTO @ProfessionListTemp (
				Service_Category_Code
			)
			SELECT Service_Category_Code FROM Profession WITH (NOLOCK)
		END
	ELSE
		BEGIN
			INSERT INTO @ProfessionListTemp (
				Service_Category_Code
			)
			SELECT Item FROM func_split_string(@ProfessionList, @delimiter)
		END

-- ---------------------------------------------
-- @SchemeBackOffice
-- ---------------------------------------------

	INSERT INTO @SchemeBackOffice (Scheme_Code, Display_Code, Display_Seq, Scheme_Desc)
	SELECT
		Scheme_Code,
		MAX(Display_Code),
		MAX(Display_Seq),
		MAX(Scheme_Desc)
	FROM SchemeBackOffice WITH (NOLOCK)
	WHERE Effective_Dtm <= @request_time AND Record_Status = 'A'
	GROUP BY Scheme_Code

-- ---------------------------------------------
-- @SubsidizeGroupBackOffice
-- ---------------------------------------------

	INSERT INTO @SubsidizeGroupBackOffice (
		Scheme_Code,
		Subsidize_Code,
		Scheme_Display_Seq,
		Display_Seq,
		Display_Desc,
		-- 'CRE13-008 - SP Amendment Report [Start][Chris YIM]
		-- -----------------------------------------------------------------------------------------
		Service_Fee_Compulsory,
		Service_Fee_Compulsory_Wording,
		-- 'CRE13-008 - SP Amendment Report [End][Chris YIM]
		Record_Status	--' CRE15-004 TIV & QIV [Winnie]
	)
	SELECT
		Filtered_SGBO.Scheme_Code,
		Filtered_SGBO.Subsidize_Code,
		Filtered_SGBO.Scheme_Display_Seq,
		Filtered_SGBO.Display_Seq,
		Filtered_SGBO.Display_Desc,
		SGBO_SF.Service_Fee_Compulsory,
		SGBO_SF.Service_Fee_Compulsory_Wording,
		SGBO_SF.Record_Status--' CRE15-004 TIV & QIV [Winnie]
	FROM
		(SELECT
			SGBO.Scheme_Code,
			SGBO.Subsidize_Code,
			MAX(SBO.Display_Seq) AS Scheme_Display_Seq,
			MAX(SGBO.Display_Seq) AS Display_Seq, --MAX(S.Display_Seq) AS Display_Seq,
			LTRIM(RTRIM(MAX(SBO.Display_Code))) + '-' + LTRIM(RTRIM(MAX(S.Display_Code))) AS Display_Desc,
			-- 'CRE13-008 - SP Amendment Report [Start][Chris YIM]
			-- -----------------------------------------------------------------------------------------
			MAX(SGBO.Scheme_Seq) AS Scheme_Seq
			-- 'CRE13-008 - SP Amendment Report [End][Chris YIM]
		FROM
			SubsidizeGroupBackOffice SGBO WITH (NOLOCK)
				INNER JOIN Subsidize S WITH (NOLOCK)
					ON SGBO.Subsidize_Code = S.Subsidize_Code
				INNER JOIN SchemeBackOffice SBO WITH (NOLOCK)
					ON SGBO.Scheme_Code = SBO.Scheme_Code AND SGBO.Scheme_Seq = SBO.Scheme_Seq
		WHERE	SGBO.Service_Fee_Enabled = 'Y'
				AND SGBO.Record_Status <> 'I' --= 'A' 
				AND S.Record_Status = 'A'
				AND SBO.Record_Status = 'A'
		GROUP BY
			SGBO.Scheme_Code,
			SGBO.Subsidize_Code,
			-- 'CRE13-008 - SP Amendment Report [Start][Chris YIM]
			-- -----------------------------------------------------------------------------------------
			S.Subsidize_Item_Code
			) Filtered_SGBO
		INNER JOIN SubsidizeGroupBackOffice SGBO_SF WITH (NOLOCK)
			ON SGBO_SF.Scheme_Code = Filtered_SGBO.Scheme_Code AND SGBO_SF.Scheme_Seq = Filtered_SGBO.Scheme_Seq AND SGBO_SF.Subsidize_Code = Filtered_SGBO.Subsidize_Code
			-- 'CRE13-008 - SP Amendment Report [End][Chris YIM]


	
	-- For GOV SIV, move the display of GOV SIV to the end of VSS vaccine list
	UPDATE @SubsidizeGroupBackOffice SET Display_Seq = '901' WHERE Scheme_Code = 'VSS' AND Subsidize_Code = 'VPQIVG'
	UPDATE @SubsidizeGroupBackOffice SET Display_Seq = '902' WHERE Scheme_Code = 'VSS' AND Subsidize_Code = 'VCQIVG'
	UPDATE @SubsidizeGroupBackOffice SET Display_Seq = '903' WHERE Scheme_Code = 'VSS' AND Subsidize_Code = 'VCLAIVG'
	UPDATE @SubsidizeGroupBackOffice SET Display_Seq = '904' WHERE Scheme_Code = 'VSS' AND Subsidize_Code = 'VAQIVG'
	UPDATE @SubsidizeGroupBackOffice SET Display_Seq = '905' WHERE Scheme_Code = 'VSS' AND Subsidize_Code = 'VEQIVG'
	UPDATE @SubsidizeGroupBackOffice SET Display_Seq = '906' WHERE Scheme_Code = 'VSS' AND Subsidize_Code = 'VPIDQIVG'
	UPDATE @SubsidizeGroupBackOffice SET Display_Seq = '907' WHERE Scheme_Code = 'VSS' AND Subsidize_Code = 'VPIDLAIVG'
	UPDATE @SubsidizeGroupBackOffice SET Display_Seq = '908' WHERE Scheme_Code = 'VSS' AND Subsidize_Code = 'VDAQIVG'
	UPDATE @SubsidizeGroupBackOffice SET Display_Seq = '909' WHERE Scheme_Code = 'VSS' AND Subsidize_Code = 'VDALAIVG'	
-- ---------------------------------------------
-- @ServiceProviderStatus
-- ---------------------------------------------

	INSERT INTO @ServiceProviderStatus (Status_Value, Status_Description)
	SELECT Status_Value, Status_Description
	FROM StatusData WITH (NOLOCK)
	WHERE Enum_Class = 'ServiceProviderStatus'

-- ---------------------------------------------
-- @MORelationship
-- ---------------------------------------------

	INSERT INTO @MORelationship (Relationship, Relationship_Desc)
	SELECT Item_No, Data_Value
	FROM StaticData WITH (NOLOCK)
	WHERE Column_Name = 'PRACTICETYPE'

-- ---------------------------------------------
-- @MOStatus
-- ---------------------------------------------

	INSERT INTO @MOStatus (Status_Value, Status_Description)
	SELECT Status_Value, Status_Description
	FROM StatusData WITH (NOLOCK)
	WHERE Enum_Class = 'MedicalOrganizationStatus'

-- ---------------------------------------------
-- @PracticeStatus
-- ---------------------------------------------

	INSERT INTO @PracticeStatus (Status_Value, Status_Description)
	SELECT Status_Value, Status_Description
	FROM StatusData WITH (NOLOCK)
	WHERE Enum_Class = 'PracticeStatus'

-- ---------------------------------------------
-- Prepare Column List for Dynamic SQL String
-- ---------------------------------------------

	-- For Scheme
	SELECT
		@pivot_table_column_header = COALESCE(@pivot_table_column_header + ',', '') + '[' + LTRIM(RTRIM(Display_Code)) + '] VARCHAR(100)',
		@pivot_table_column_list = COALESCE(@pivot_table_column_list + ',', '') + '[' + LTRIM(RTRIM(Display_Code)) + ']',
		@pivot_table_column_name_alias = COALESCE(@pivot_table_column_name_alias + ',', '') + 'PT.[' + LTRIM(RTRIM(Display_Code)) + ']',
		@pivot_table_column_name_value = COALESCE(@pivot_table_column_name_value + ',', '') + '''' + LTRIM(RTRIM(Display_Code)) + ''''
	FROM @SchemeBackOffice
	ORDER BY Display_Seq

	-- For Subsidize
	SELECT
		@pivot_table_subsidize_column_header = COALESCE(@pivot_table_subsidize_column_header + ',', '') + '[' + LTRIM(RTRIM(Display_Desc)) + '] VARCHAR(50)',
		@pivot_table_subsidize_column_list = COALESCE(@pivot_table_subsidize_column_list + ',', '') + '[' + LTRIM(RTRIM(Display_Desc)) + ']',
		@pivot_table_subsidize_column_name_alias = COALESCE(@pivot_table_subsidize_column_name_alias + ',', '') + 'PT2.[' + LTRIM(RTRIM(Display_Desc)) + ']',
		@pivot_table_subsidize_column_name_value = COALESCE(@pivot_table_subsidize_column_name_value + ',', '') + '''' + LTRIM(RTRIM(Display_Desc)) + ' ($)'''
	FROM @SubsidizeGroupBackOffice
	ORDER BY Scheme_Display_Seq, Display_Seq

	--' CRE15-004 TIV & QIV [Start][Winnie]
	-- For Scheme in Practice
	SELECT
		@pivot_table_practice_scheme_column_header = COALESCE(@pivot_table_practice_scheme_column_header + ',', '') + '[' + LTRIM(RTRIM(Display_Code)) + '] VARCHAR(100)',
		@pivot_table_practice_scheme_column_list = COALESCE(@pivot_table_practice_scheme_column_list + ',', '') + '[' + LTRIM(RTRIM(Display_Code)) + ']',
		@pivot_table_practice_scheme_column_name_alias = COALESCE(@pivot_table_practice_scheme_column_name_alias + ',', '') + 'PT2.[' + LTRIM(RTRIM(Display_Code)) + ']',
		@pivot_table_practice_scheme_column_name_value = COALESCE(@pivot_table_practice_scheme_column_name_value + ',', '') + '''With Active Practice Enrolled in ' + LTRIM(RTRIM(Display_Code)) + ''''
	FROM @SchemeBackOffice
	ORDER BY Display_Seq
	--' CRE15-004 TIV & QIV [End][Winnie]
	
-- =============================================
-- Retrieve Data
-- =============================================

	EXEC [proc_SymmetricKey_open]

-- ---------------------------------------------
-- #SP_Filtered
-- ---------------------------------------------

	INSERT INTO #SP_Filtered (
		SP_ID
	)
	SELECT
		SP.SP_ID
	FROM
		ServiceProvider SP WITH (NOLOCK)
			INNER JOIN Professional PL WITH (NOLOCK)
				ON SP.SP_ID = PL.SP_ID
			INNER JOIN @ProfessionListTemp PLT
				ON PL.Service_Category_Code = PLT.Service_Category_Code COLLATE DATABASE_DEFAULT
	WHERE
		(NOT EXISTS(SELECT 1 FROM SPExceptionList SPEL WITH (NOLOCK) WHERE SPEL.SP_ID = SP.SP_ID))
	GROUP BY
		SP.SP_ID

-- ---------------------------------------------
-- #Practice_Filtered
-- ---------------------------------------------

	INSERT INTO #Practice_Filtered (
		SP_ID,
		Display_Seq
	)
	SELECT
		P.SP_ID,
		P.Display_Seq
	FROM
		#SP_Filtered SP_F
			INNER JOIN Practice P WITH (NOLOCK)
				ON SP_F.SP_ID = P.SP_ID COLLATE DATABASE_DEFAULT

-- ---------------------------------------------
-- #SP_Filtered_With_Scheme_Info
-- ---------------------------------------------

	INSERT INTO #SP_Filtered_With_Scheme_Info (
		SP_ID,
		Scheme_Display_Code,
		Scheme_Record_Status
	)
	SELECT
		TEMP_1.SP_ID,
		TEMP_1.Display_Code,
		-- 'CRE13-008 - SP Amendment Report [Start][Chris YIM]
		-- -----------------------------------------------------------------------------------------
		[Record_Status] = 
		CASE
			WHEN ISNULL(SI.Record_Status, 'N') = 'A' THEN 'Active'
			WHEN ISNULL(SI.Record_Status, 'N') = 'S' THEN 'Suspended'
			WHEN ISNULL(SI.Record_Status, 'N') = 'D' THEN 'Delisted'
			WHEN ISNULL(SI.Record_Status, 'N') = 'N' THEN 'Not enrolled'
			ELSE 'Not enrolled'
		END
		-- 'CRE13-008 - SP Amendment Report [End][Chris YIM]
	FROM
		(
		SELECT
			SP.SP_ID,
			SBO.Scheme_Code,
			SBO.Display_Code
		FROM
			#SP_Filtered SP
				CROSS JOIN @SchemeBackOffice SBO
		) AS TEMP_1
			LEFT JOIN
				SchemeInformation SI WITH (NOLOCK)
					ON	TEMP_1.SP_ID = SI.SP_ID COLLATE DATABASE_DEFAULT
						AND TEMP_1.Scheme_Code = SI.Scheme_Code COLLATE DATABASE_DEFAULT

						
-- ---------------------------------------------
-- #Practice_Filtered_With_Scheme_Info
-- ---------------------------------------------

	INSERT INTO #Practice_Filtered_With_Scheme_Info (
		SP_ID,
		Display_Seq,
		Scheme_Display_Code,
		Scheme_Record_Status
	)
	SELECT
		TEMP_1.SP_ID,
		TEMP_1.Display_Seq,
		TEMP_1.Display_Code,
		-- 'CRE13-008 - SP Amendment Report [Start][Chris YIM]
		-- -----------------------------------------------------------------------------------------
		[Record_Status] = 
		CASE
			WHEN ISNULL(MAX(PSI.Record_Status), 'N') = 'A' THEN 'Active'
			WHEN ISNULL(MAX(PSI.Record_Status), 'N') = 'S' THEN 'Suspended'
			WHEN ISNULL(MAX(PSI.Record_Status), 'N') = 'D' THEN 'Delisted'
			WHEN ISNULL(MAX(PSI.Record_Status), 'N') = 'N' THEN 'Not enrolled'
			ELSE 'Not enrolled'
		END
		-- 'CRE13-008 - SP Amendment Report [End][Chris YIM]
	FROM
		(
		SELECT
			P_F.SP_ID,
			P_F.Display_Seq,
			SBO.Scheme_Code,
			SBO.Display_Code
		FROM
			#Practice_Filtered P_F
				CROSS JOIN @SchemeBackOffice SBO
		) TEMP_1
		LEFT JOIN PracticeSchemeInfo PSI WITH (NOLOCK)
			ON	TEMP_1.SP_ID = PSI.SP_ID COLLATE DATABASE_DEFAULT
				AND TEMP_1.Display_Seq = PSI.Practice_Display_Seq
				AND TEMP_1.Scheme_Code = PSI.Scheme_Code COLLATE DATABASE_DEFAULT
	GROUP BY
		TEMP_1.SP_ID,
		TEMP_1.Display_Seq,
		TEMP_1.Display_Code

-- ---------------------------------------------
-- ' CRE15-004 TIV & QIV [Winnie]
-- #SP_Filtered_With_Practice_Scheme_Info
-- ---------------------------------------------

	INSERT INTO #SP_Filtered_With_Practice_Scheme_Info (
		SP_ID,
		Scheme_Display_Code,
		Scheme_Record_Status
	)
	SELECT
		TEMP_1.SP_ID,
		TEMP_1.Display_Code,
		[Record_Status] = 
		CASE ISNULL(MIN(TEMP_2.Record_Status),'') -- find the active record 
			WHEN 'A' THEN 'Yes' -- Active
			WHEN 'S' THEN 'No' -- Suspended 
			WHEN 'D' THEN 'No' -- Delisted
			ELSE 'N/A' -- Not Enrolled
		END
	FROM
		(
		SELECT
			SP.SP_ID,
			SBO.Scheme_Code,
			SBO.Display_Code
		FROM
			#SP_Filtered SP
				CROSS JOIN @SchemeBackOffice SBO
		) AS TEMP_1
		LEFT JOIN 
		(
			SELECT
				TEMP_3.SP_ID,
				TEMP_3.Display_Seq,
				TEMP_3.Scheme_Code,
				[Record_Status] = MAX(PSI.Record_Status) -- randomly pick 1 record status as practice scheme status
			FROM
				(
				SELECT
					P_F.SP_ID,
					P_F.Display_Seq,
					SBO.Scheme_Code
				FROM
					#Practice_Filtered P_F
						CROSS JOIN @SchemeBackOffice SBO
				) TEMP_3
				LEFT JOIN PracticeSchemeInfo PSI WITH (NOLOCK)
					ON	TEMP_3.SP_ID = PSI.SP_ID COLLATE DATABASE_DEFAULT
						AND TEMP_3.Display_Seq = PSI.Practice_Display_Seq
						AND TEMP_3.Scheme_Code = PSI.Scheme_Code COLLATE DATABASE_DEFAULT
			GROUP BY
				TEMP_3.SP_ID,
				TEMP_3.Display_Seq,
				TEMP_3.Scheme_Code
		)TEMP_2
				ON	TEMP_1.SP_ID = TEMP_2.SP_ID COLLATE DATABASE_DEFAULT
					AND TEMP_1.Scheme_Code = TEMP_2.Scheme_Code COLLATE DATABASE_DEFAULT
	GROUP BY
		TEMP_1.SP_ID,
		TEMP_1.Display_Code
			
-- ---------------------------------------------
-- #Practice_Filtered_With_Subsidize_Info
-- ---------------------------------------------

	INSERT INTO #Practice_Filtered_With_Subsidize_Info (
		SP_ID,
		Display_Seq,
		Display_Desc,
		Service_Fee
	)
	SELECT
		TEMP_1.SP_ID,
		TEMP_1.Display_Seq,
		TEMP_1.Display_Desc,
		-- 'CRE13-008 - SP Amendment Report [Start][Chris YIM]
		-- -----------------------------------------------------------------------------------------		
		--' CRE15-004 TIV & QIV [Start][Winnie]
		[Service_Fee] =	
			CASE ISNULL(PSI.Provide_Service,'') 
				WHEN 'Y' THEN
					CASE WHEN PSI.Service_Fee IS NULL THEN 
							CASE WHEN TEMP_1.Service_Fee_Compulsory = 'N' THEN TEMP_1.Service_Fee_Compulsory_Wording
								 ELSE 'N/A'
							END
						 Else CONVERT(VARCHAR(100), PSI.Service_Fee)
					END
										
				WHEN 'N' THEN					
					CASE WHEN ISNULL(TEMP_1.Record_Status,'') <> 'E' THEN 'No Service Provided' -- No Service Provided
						 ELSE 'N/A' -- Historical Record
					END
					
				ELSE -- No PSI record
					CASE WHEN ISNULL(PS.Scheme_Code,'') <> '' THEN 'No Service Provided' -- Have enrolled scheme but not exist in PSI
						 ELSE 'N/A' -- Scheme not enrolled 
					END
			END
		--' CRE15-004 TIV & QIV [End][Winnie]
		-- 'CRE13-008 - SP Amendment Report [End][Chris YIM]
	FROM
		(
			SELECT
				P_F.SP_ID,
				P_F.Display_Seq,
				SGBO.Scheme_Code,
				SGBO.Subsidize_Code,
				SGBO.Display_Desc,
				SGBO.Record_Status,	--' CRE15-004 TIV & QIV [Winnie]
				-- 'CRE13-008 - SP Amendment Report [Start][Chris YIM]
				-- -----------------------------------------------------------------------------------------
				SGBO.Service_Fee_Compulsory,
				SGBO.Service_Fee_Compulsory_Wording
				-- 'CRE13-008 - SP Amendment Report [End][Chris YIM]			
			FROM
				#Practice_Filtered P_F
					CROSS JOIN @SubsidizeGroupBackOffice SGBO
		) TEMP_1
	LEFT JOIN PracticeSchemeInfo PSI WITH (NOLOCK)
		ON	TEMP_1.SP_ID = PSI.SP_ID COLLATE DATABASE_DEFAULT
			AND TEMP_1.Display_Seq = PSI.Practice_Display_Seq
			AND TEMP_1.Scheme_Code = PSI.Scheme_Code COLLATE DATABASE_DEFAULT
			AND TEMP_1.Subsidize_Code = PSI.Subsidize_Code COLLATE DATABASE_DEFAULT
	LEFT JOIN 
		(
			SELECT DISTINCT 
				SP_ID, Practice_Display_Seq, Scheme_Code
			FROM
				PracticeSchemeInfo WITH (NOLOCK)
		) PS
		ON	TEMP_1.SP_ID = PS.SP_ID COLLATE DATABASE_DEFAULT
			AND TEMP_1.Display_Seq = PS.Practice_Display_Seq
			AND TEMP_1.Scheme_Code = PS.Scheme_Code COLLATE DATABASE_DEFAULT


-- ---------------------------------------------
-- For Excel Sheet (02): Criteria
-- ---------------------------------------------

	DECLARE @criteria01 VARCHAR(5000)

	--IF @ProfessionList = ''
	--	BEGIN
	--		SET @criteria01 = 'Any'
	--	END
	--ELSE
	--	BEGIN
			SELECT
				@criteria01 = COALESCE(@criteria01 + ', ', '') + LTRIM(RTRIM(P.Service_Category_Desc)) + ' (' + LTRIM(RTRIM(P.Service_Category_Code)) + ')'
			FROM
				@ProfessionListTemp PLT
					INNER JOIN Profession P WITH (NOLOCK)
						ON PLT.Service_Category_Code = P.Service_Category_Code COLLATE DATABASE_DEFAULT
			ORDER BY P.Service_Category_Desc
	  --  END

-- ---------------------------------------------
-- For Excel Sheet (03): 01-Service Provider (Part 1)
-- ---------------------------------------------

	-- Create Temp Result Table
	CREATE TABLE #WS03_Part1 (
		Seq		int,
		Seq2	datetime,
		Col01	VARCHAR(10),	-- SPID
		Col02	VARCHAR(100),	-- SP Name (In English)
		Col03	NVARCHAR(100),	-- SP Name (In Chinese)
		Col04	VARCHAR(30),	-- Profile Effective Date
		Col05	VARCHAR(20),	-- Data Input By
		Col06	VARCHAR(300),	-- Correspondence Address
		Col07	VARCHAR(20),	-- District
		Col08	VARCHAR(20),	-- District Board
		Col09	VARCHAR(50),	-- Area
		Col10	VARCHAR(255),	-- Email Address
		Col11	VARCHAR(255),	-- Pending Email Address
		Col12	VARCHAR(30),	-- Daytime Contact Phone No.
		Col13	VARCHAR(20),	-- Fax No.
		Col14	VARCHAR(100),	-- SP Status
		Col15	VARCHAR(100),	-- PCD Status
		Col16	VARCHAR(100),	-- PCD Professional
		Col17	VARCHAR(50),	-- Last check date of PCD Status
		Col18	VARCHAR(5000),	-- Profession
		Col19	VARCHAR(30),	-- Token Serial No.
		Col20	VARCHAR(30),	-- Token Issued By
		Col21	VARCHAR(30),	-- Is Share Token
		Col22	VARCHAR(30),	-- Token Serial No. (New)
		Col23	VARCHAR(30),	-- Token Issued By (New)
		Col24	VARCHAR(30),	-- Is Share Token (New)
		Col25	VARCHAR(100)	-- Provide DHC-related Services (Any practices)
	)

	-- Create Column Header
	SET @seq = 0

	INSERT INTO #WS03_Part1 (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16, Col17, Col18, Col19, Col20, Col21, Col22, Col23, Col24, Col25)
	VALUES (
		@seq, NULL,
		'SPID', 'SP Name (In English)', 'SP Name (In Chinese)', 'Profile Effective Date', 'Data Input By',
		'Correspondence Address', 'District', 'District Board', 'Area', 'Email Address','Pending Email Address',
		'Daytime Contact Phone No.', 'Fax No.', 'SP Status', 'PCD Status', 'PCD Professional', 'Last check date of PCD Status','Profession', 'Token Serial No.',
		'Token Issued By', 'Is Share Token', 'Token Serial No. (New)', 'Token Issued By (New)', 'Is Share Token (New)', 'Provide DHC-related Services (Any practices)'	
	)

	-- Create Report Result
	SET @seq = @seq + 1

	DECLARE @ProjectEHR AS NVARCHAR(MAX)
	SELECT @ProjectEHR = Description FROM SystemResource WITH (NOLOCK) WHERE ObjectType = 'Text' AND ObjectName in ('TokenEHR')

	INSERT INTO #WS03_Part1 (Seq, Seq2, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16, Col17, Col18, Col19, Col20, Col21, Col22, Col23, Col24, Col25)
	SELECT
		@seq,
		SP.Data_Input_Effective_Dtm,
		SP.SP_ID,
		CONVERT(VARCHAR(100), DecryptByKey(SP.Encrypt_Field2)),
		CONVERT(NVARCHAR(100), DecryptByKey(SP.Encrypt_Field3)),
		-- 'CRE13-008 - SP Amendment Report [Start][Chris YIM]
		-- -----------------------------------------------------------------------------------------
		CONVERT(VARCHAR(10), SP.Data_Input_Effective_Dtm, 111),
		-- 'CRE13-008 - SP Amendment Report [End][Chris YIM]
		SP.Data_Input_By,
		[dbo].[func_formatEngAddress](SP.[Room], SP.[Floor], SP.[Block], SP.[Building], SP.[District]),
		D.district_name,
		D.district_board,
		DA.area_name,
		SP.Email,
		-- 'CRE13-008 - SP Amendment Report [Start][Chris YIM]
		-- -----------------------------------------------------------------------------------------
		SP.Tentative_Email,
		SP.Phone_Daytime,
		SP.Fax,
		SPS.Status_Description,
		CASE
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = '' THEN 'Unknown'
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = 'NN' THEN 'Not enrolled'
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = 'NU' THEN 'Not enrolled (Unprocessed)'
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = 'NP' THEN 'Not enrolled (Processing)'
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = 'EN' THEN 'Enrolled'
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = 'EU' THEN 'Enrolled (Unprocessed)'
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = 'EP' THEN 'Enrolled (Processing)'
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = 'DN' THEN 'Delisted'
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = 'DU' THEN 'Delisted (Unprocessed)'
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = 'DP' THEN 'Delisted (Processing)'
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = 'II' THEN 'N/A'
			ELSE 'Unknown'
		END AS [PCD_Status],
		REPLACE(SP.PCD_Professional,',',', ') AS [PCD_Professional],
		CASE 
			WHEN SP.PCD_Status_Last_Check_Dtm IS NULL THEN ''
			ELSE CONVERT(VARCHAR(10), SP.PCD_Status_Last_Check_Dtm, 111)
		END AS [PCD_Status_Last_Check_Dtm],
		STUFF(
			(SELECT (', ' + LTRIM(RTRIM(PL.Service_Category_Code)))
				FROM (SELECT DISTINCT SP_ID, Service_Category_Code, MIN(Professional_Seq) AS 'Professional_Seq'
						FROM Professional 
						WHERE Record_Status = 'A'
						Group by SP_ID, Service_Category_Code) PL 
				WHERE PL.SP_ID = SP.SP_ID 
				ORDER BY PL.Professional_Seq, PL.Service_Category_Code FOR XML PATH ('')),1,2,''),

		T.Token_Serial_No,
		CASE WHEN T_EHR_Current.Token_Serial_No IS NOT NULL THEN @ProjectEHR ELSE SD1.Data_Value END AS [Project],
		[Is_Share_Token] = 
		CASE
			WHEN T.Is_Share_Token = 'Y' THEN 'Yes'
			WHEN T.Is_Share_Token = 'N' THEN 'No'
			-- 'INT14-0035 - Fix eHSU0005-01 Missing SP who without token [Start][Chris YIM]
			-- -----------------------------------------------------------------------------------------
			ELSE NULL
			-- 'INT14-0035 - Fix eHSU0005-01 Missing SP who without token [End][Chris YIM]
		END,

		T.Token_Serial_No_Replacement,
		CASE WHEN T_EHR_Replace.Token_Serial_No IS NOT NULL THEN @ProjectEHR ELSE SD2.Data_Value END AS [Project_Replacement],
		[Is_Share_Token_Replacement] = 
		CASE
			WHEN T.Is_Share_Token_Replacement = 'Y' THEN 'Yes'
			WHEN T.Is_Share_Token_Replacement = 'N' THEN 'No'
			ELSE NULL
		END,
		-- 'CRE13-008 - SP Amendment Report [End][Chris YIM]
		[DHC_Service] = IIF(DHC.SP_ID IS NOT NULL, 'Yes','No')
	FROM
		#SP_Filtered SPF
			INNER JOIN ServiceProvider SP WITH (NOLOCK)
				ON SPF.SP_ID = SP.SP_ID COLLATE DATABASE_DEFAULT
			INNER JOIN district D WITH (NOLOCK)
				ON SP.District = D.district_code COLLATE DATABASE_DEFAULT
			INNER JOIN DistrictBoard DB WITH (NOLOCK)
				ON D.District_Board = DB.District_Board COLLATE DATABASE_DEFAULT
			INNER JOIN district_area DA WITH (NOLOCK)
				ON DB.Area_Code = DA.area_code COLLATE DATABASE_DEFAULT
			-- 'INT14-0035 - Fix eHSU0005-01 Missing SP who without token [Start][Chris YIM]
			-- -----------------------------------------------------------------------------------------
			FULL OUTER JOIN Token T WITH (NOLOCK)
			-- 'INT14-0035 - Fix eHSU0005-01 Missing SP who without token [End][Chris YIM]
				ON SP.SP_ID = T.[User_ID] COLLATE DATABASE_DEFAULT
			LEFT JOIN TokenEHR T_EHR_Current WITH (NOLOCK)
				ON T.Token_Serial_No = T_EHR_Current.Token_Serial_No
			LEFT JOIN TokenEHR T_EHR_Replace WITH (NOLOCK)
				ON T.Token_Serial_No_Replacement = T_EHR_Replace.Token_Serial_No
			INNER JOIN @ServiceProviderStatus SPS
				ON SP.Record_Status = SPS.Status_Value COLLATE DATABASE_DEFAULT
			LEFT JOIN StaticData SD1
				ON SD1.Column_Name = 'TOKEN_ISSUE_BY'
					AND T.Project = SD1.Item_No
			LEFT JOIN StaticData SD2
				ON SD2.Column_Name = 'TOKEN_ISSUE_BY'
					AND T.Project_Replacement = SD2.Item_No
			-- SP has at least one professional providing DHC-related Services
			LEFT JOIN (SELECT DISTINCT p.SP_ID FROM DHCSPMapping d WITH (NOLOCK) 
						INNER JOIN Professional p WITH (NOLOCK)
						ON d.Service_Category_Code = p.Service_Category_Code
							AND d.Registration_Code = p.Registration_Code
							AND p.Record_Status = 'A')  DHC
				ON SPF.SP_ID = DHC.SP_ID

-- ---------------------------------------------
-- For Excel Sheet (03): 01-Service Provider (Part 2)
-- ---------------------------------------------

	-- Create Pivot Table
	CREATE TABLE #PivotTable_WS03 (
		SP_ID char(8)
	)

	-- Add Column for Pivot Table dynamically
	EXECUTE('ALTER TABLE #PivotTable_WS03 ADD ' + @pivot_table_column_header)

	-- Prepare Dynamic SQL String for INSERT statement of Pivot Table
	SET @sql_script_pivot_table_insert = ' ([SP_ID],' + @pivot_table_column_list + ') '

	-- Create Column Header dynamically
	SET @sql_script = 'INSERT INTO #PivotTable_WS03' + @sql_script_pivot_table_insert + 'VALUES (''SPID'',' + @pivot_table_column_name_value + ')'
	EXECUTE(@sql_script)

	-- Generate Pivot Table for Result
	SET @sql_script = 'SELECT *
		FROM (
			SELECT
				SP_ID,
				Scheme_Display_Code,
				Scheme_Record_Status
			FROM
				#SP_Filtered_With_Scheme_Info
		) DATA
		PIVOT (
			MAX(Scheme_Record_Status)
			FOR Scheme_Display_Code
			IN (' + @pivot_table_column_list + ')
		) FUNC'

	SET @sql_script = 'INSERT INTO #PivotTable_WS03' + @sql_script_pivot_table_insert + @sql_script
	EXECUTE(@sql_script)


-- ---------------------------------------------
-- ' CRE15-004 TIV & QIV [Winnie]
-- For Excel Sheet (03): 01-Service Provider (Part 3)
-- ---------------------------------------------

	-- Create Pivot Table
	CREATE TABLE #PivotTable_WS03_P3 (
		SP_ID char(8)
	)

	-- Add Column for Pivot Table dynamically
	EXECUTE('ALTER TABLE #PivotTable_WS03_P3 ADD ' + @pivot_table_practice_scheme_column_header)

	-- Prepare Dynamic SQL String for INSERT statement of Pivot Table
	SET @sql_script_pivot_table_insert = ' ([SP_ID],' + @pivot_table_practice_scheme_column_list + ') '

	-- Create Column Header dynamically
	SET @sql_script = 'INSERT INTO #PivotTable_WS03_P3' + @sql_script_pivot_table_insert + 'VALUES (''SPID'',' + @pivot_table_practice_scheme_column_name_value + ')'
	EXECUTE(@sql_script)

	-- Generate Pivot Table for Result
	SET @sql_script = 'SELECT *
		FROM (
			SELECT
				SP_ID,
				Scheme_Display_Code,
				Scheme_Record_Status
			FROM
				#SP_Filtered_With_Practice_Scheme_Info
		) DATA
		PIVOT (
			MAX(Scheme_Record_Status)
			FOR Scheme_Display_Code
			IN (' + @pivot_table_practice_scheme_column_list + ')
		) FUNC'

	SET @sql_script = 'INSERT INTO #PivotTable_WS03_P3' + @sql_script_pivot_table_insert + @sql_script
	EXECUTE(@sql_script)

	DROP TABLE #SP_Filtered_With_Practice_Scheme_Info

-- ---------------------------------------------
-- For Excel Sheet (04): 02-MO
-- ---------------------------------------------

	-- Create Temp Result Table
	CREATE TABLE #WS04 (
		Seq		int,
		Seq2	datetime,
		Col01	VARCHAR(10),
		Col02	VARCHAR(100),
		Col03	NVARCHAR(100),
		Col04	VARCHAR(30),
		Col05	VARCHAR(20),
		Col06	VARCHAR(10),
		Col07	NVARCHAR(100),
		Col08	NVARCHAR(100),
		Col09	VARCHAR(50),
		Col10	VARCHAR(30),
		Col11	VARCHAR(20),
		Col12	VARCHAR(255),
		Col13	VARCHAR(300),
		Col14	VARCHAR(20),
		Col15	VARCHAR(20),
		Col16	VARCHAR(50),
		Col17	VARCHAR(4000),
		Col18	VARCHAR(100)
	)

	-- Create Column Header (Entered in Excel Template)
	SET @seq = 0

	--INSERT INTO #WS04 (
	--	Seq, Seq2,
	--	Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10,
	--	Col11, Col12, Col13, Col14, Col15, Col16, Col17, Col18, Col19
	--)
	--VALUES (
	--	@seq, NULL,
	--	'SPID', 'SP Name (In English)', 'SP Name (In Chinese)', 'Profile Effective Date', 'Data Input By',
	--	'MO No.', 'MO Name (In English)', 'MO Name (In Chinese)', 'Business Registration No.', 'Daytime Contact Phone No.',
	--	'Fax No.', 'Email Address', 'Practice Address (in English)', 'Practice Address (in Chinese)', 'District',
	--	'District Board', 'Area', 'Relationship with MO', 'MO Status'
	--)

	-- Create Report Result
	SET @seq = @seq + 1

	INSERT INTO #WS04 (
		Seq, Seq2,
		Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10,
		Col11, Col12, Col13, Col14, Col15, Col16, Col17, Col18
	)
	SELECT
		@seq,
		SP.Data_Input_Effective_Dtm,
		SP.SP_ID,
		CONVERT(VARCHAR(100), DecryptByKey(SP.Encrypt_Field2)),
		CONVERT(NVARCHAR(100), DecryptByKey(SP.Encrypt_Field3)),
		-- 'CRE13-008 - SP Amendment Report [Start][Chris YIM]
		-- -----------------------------------------------------------------------------------------
		CONVERT(VARCHAR(10), SP.Data_Input_Effective_Dtm, 111),
		-- 'CRE13-008 - SP Amendment Report [End][Chris YIM]
		SP.Data_Input_By,
		MO.Display_Seq,
		MO.MO_Eng_Name,
		MO.MO_Chi_Name,
		MO.BR_Code,
		MO.Phone_Daytime,
		MO.Fax,
		MO.Email,
		[dbo].[func_formatEngAddress](MO.[Room], MO.[Floor], MO.[Block], MO.[Building], MO.[District]),
--		[dbo].[func_format_Address_Chi](MO.[Room], MO.[Floor], MO.[Block], MO.[Building_Chi], MO.[District]),
		D.district_name,
		D.district_board,
		DA.area_name,
		MOR.Relationship_Desc,
		MOS.Status_Description
	FROM
		#SP_Filtered SPF
			INNER JOIN ServiceProvider SP WITH (NOLOCK)
				ON SPF.SP_ID = SP.SP_ID COLLATE DATABASE_DEFAULT
			INNER JOIN MedicalOrganization MO WITH (NOLOCK)
				ON SP.SP_ID = MO.SP_ID COLLATE DATABASE_DEFAULT
			INNER JOIN district D WITH (NOLOCK)
				ON MO.District = D.district_code COLLATE DATABASE_DEFAULT
			INNER JOIN DistrictBoard DB WITH (NOLOCK)
				ON D.District_Board = DB.District_Board COLLATE DATABASE_DEFAULT				
			INNER JOIN district_area DA WITH (NOLOCK)
				ON DB.Area_Code = DA.area_code COLLATE DATABASE_DEFAULT
			INNER JOIN @MORelationship MOR
				ON MO.Relationship = MOR.Relationship COLLATE DATABASE_DEFAULT
			INNER JOIN @MOStatus MOS
				ON MO.Record_Status = MOS.Status_Value COLLATE DATABASE_DEFAULT

-- ---------------------------------------------
-- For Excel Sheet (05): 03-Practice & BankAcc (Part 1)
-- ---------------------------------------------

	-- Create Temp Result Table
	CREATE TABLE #WS05_Part1 (
		Seq		INT,
		Seq2	DATETIME,
		Col01	VARCHAR(10),	-- SPID
		Col02	VARCHAR(100),	-- SP Name (In English)
		Col03	NVARCHAR(100),	-- SP Name (In Chinese)
		Col04	VARCHAR(30),	-- Profile Effective Date
		Col05	VARCHAR(20),	-- Data Input By
		--' CRE15-004 TIV & QIV [Start][Winnie] Add SP Status
		Col06	VARCHAR(100),	-- SP Status
		Col07	VARCHAR(100),	-- SP PCD Status
		Col08	VARCHAR(100),	-- SP PCD Professional
		Col09	VARCHAR(50),	-- SP Last check date of PCD Status
		--' CRE15-004 TIV & QIV [End][Winnie]
		Col10	VARCHAR(20),	-- Practice No.
		-- 'CRE13-008 - SP Amendment Report [Start][Chris YIM]
		-- -----------------------------------------------------------------------------------------
		Col11	VARCHAR(10),	-- MO No.
		-- 'CRE13-008 - SP Amendment Report [End][Chris YIM]	
		Col12	NVARCHAR(100),	-- MO Name (In English)
		Col13	NVARCHAR(100),	-- MO Name (In Chinese)
		Col14	NVARCHAR(100),	-- Practice Name (In English)
		Col15	NVARCHAR(100),	-- Practice Name (In Chinese)
		Col16	VARCHAR(300),	-- Practice Address (In English)
		Col17	NVARCHAR(300),	-- Practice Address (In Chinese)
		Col18	VARCHAR(20),	-- District
		Col19	VARCHAR(20),	-- District Board
		Col20	VARCHAR(50),	-- Area
		Col21	VARCHAR(30),	-- Practice Status
		Col22	VARCHAR(20),	-- Profession
		Col23	VARCHAR(50),	-- Professional Registration No.
		Col24	VARCHAR(50),	-- Phone No. of Practice
		Col25	VARCHAR(100),	-- Mobile_Clinic
		Col26	NVARCHAR(200),	-- Remarks Description (In English)
		Col27	NVARCHAR(200),	-- Remarks Description (In Chinese)	
		Col28	NVARCHAR(100),	-- Bank Name
		Col29	NVARCHAR(100),	-- Branch Name
		Col30	NVARCHAR(300),	-- Bank Account Name
		Col31	VARCHAR(100)	-- Provide DHC-related Services
	)

	-- Create Column Header
	SET @seq = 0

	INSERT INTO #WS05_Part1 (
		Seq, Seq2,
		Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10,
		Col11, Col12, Col13, Col14, Col15, Col16, Col17, Col18, Col19, Col20,
		Col21, Col22, Col23, Col24, Col25, Col26, Col27, Col28, Col29, Col30,
		Col31
	)
	VALUES (
		@seq, NULL,
		'SPID', 'SP Name (In English)', 'SP Name (In Chinese)', 'Profile Effective Date', 'Data Input By', 'SP Status', 'SP PCD Status', 'SP PCD Professional', 'SP Last check date of PCD Status',
		'Practice No.', 'MO No.', 'MO Name (In English)', 'MO Name (In Chinese)', 'Practice Name (In English)',
		'Practice Name (In Chinese)', 'Practice Address (In English)', 'Practice Address (In Chinese)', 
		'District', 'District Board', 'Area', 'Practice Status', 'Profession', 'Professional Registration No.',
		'Phone No. of Practice', 'Mobile Clinic', 'Remarks (In English)', 'Remarks (In Chinese)',
		'Bank Name', 'Branch Name', 'Bank Account Name', 'Provide DHC-related Services'
	)

	-- Create Report Result
	SET @seq = @seq + 1

	INSERT INTO #WS05_Part1 (
		Seq, Seq2,
		Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10,
		Col11, Col12, Col13, Col14, Col15, Col16, Col17, Col18, Col19, Col20,
		Col21, Col22, Col23, Col24, Col25, Col26, Col27, Col28, Col29, Col30,
		Col31
	)
	SELECT
		@seq,
		SP.Data_Input_Effective_Dtm,
		SP.SP_ID,
		CONVERT(VARCHAR(100), DecryptByKey(SP.Encrypt_Field2)),
		CONVERT(NVARCHAR(100), DecryptByKey(SP.Encrypt_Field3)),
		CONVERT(VARCHAR(10), SP.Data_Input_Effective_Dtm, 111),
		SP.Data_Input_By,
		SPS.Status_Description, -- ' Add Column SP Status [Winnie]
		CASE
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = '' THEN 'Unknown'
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = 'NN' THEN 'Not enrolled'
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = 'NU' THEN 'Not enrolled (Unprocessed)'
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = 'NP' THEN 'Not enrolled (Processing)'
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = 'EN' THEN 'Enrolled'
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = 'EU' THEN 'Enrolled (Unprocessed)'
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = 'EP' THEN 'Enrolled (Processing)'
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = 'DN' THEN 'Delisted'
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = 'DU' THEN 'Delisted (Unprocessed)'
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = 'DP' THEN 'Delisted (Processing)'
			WHEN ISNULL(SP.PCD_Account_Status,'') + ISNULL(SP.PCD_Enrolment_Status,'') = 'II' THEN 'N/A'
			ELSE 'Unknown'
		END AS [PCD_Status],
		REPLACE(SP.PCD_Professional,',',', ') AS [PCD_Professional],
		CASE 
			WHEN SP.PCD_Status_Last_Check_Dtm IS NULL THEN ''
			ELSE CONVERT(VARCHAR(10), SP.PCD_Status_Last_Check_Dtm, 111)
		END AS [PCD_Status_Last_Check_Dtm],
		P.Display_Seq,
		MO.Display_Seq,
		MO.MO_Eng_Name,
		MO.MO_Chi_Name,
		P.Practice_Name,
		P.Practice_Name_Chi,
		[dbo].[func_formatEngAddress](P.[Room], P.[Floor], P.[Block], P.[Building], P.[District]),
		[dbo].[func_format_Address_Chi](P.[Room], P.[Floor], P.[Block], P.[Building_Chi], P.[District]),
		D.district_name,
		D.district_board,
		DA.area_name,
		PS.Status_Description,
		PL.Service_Category_Code,
		PL.Registration_Code,
		P.Phone_Daytime,
		[Mobile_Clinic] = IIF(P.[Mobile_Clinic] = 'Y', 'Yes','No'),
		P.[Remarks_Desc],
		P.[Remarks_Desc_Chi],
		BA.Bank_Name,
		BA.Branch_Name,
		BA.Bank_Acc_Holder,
		DHC_Service = IIF(DHC.Display_Seq IS NOT NULL, 'Yes','No')
	FROM
		#SP_Filtered SPF
			INNER JOIN ServiceProvider SP WITH (NOLOCK)
				ON SPF.SP_ID = SP.SP_ID COLLATE DATABASE_DEFAULT
			INNER JOIN Practice P WITH (NOLOCK)
				ON SP.SP_ID = P.SP_ID COLLATE DATABASE_DEFAULT
			INNER JOIN MedicalOrganization MO WITH (NOLOCK)
				ON P.SP_ID = MO.SP_ID COLLATE DATABASE_DEFAULT AND P.MO_Display_Seq = MO.Display_Seq
			INNER JOIN district D WITH (NOLOCK)
				ON P.District = D.district_code COLLATE DATABASE_DEFAULT
			INNER JOIN DistrictBoard DB WITH (NOLOCK)
				ON D.District_Board = DB.District_Board COLLATE DATABASE_DEFAULT
			INNER JOIN district_area DA WITH (NOLOCK)
				ON DB.Area_Code = DA.area_code COLLATE DATABASE_DEFAULT
			INNER JOIN @PracticeStatus PS
				ON P.Record_Status = PS.Status_Value COLLATE DATABASE_DEFAULT
			INNER JOIN Professional PL WITH (NOLOCK)
				ON P.SP_ID = PL.SP_ID COLLATE DATABASE_DEFAULT AND P.Professional_Seq = PL.Professional_Seq
			INNER JOIN BankAccount BA WITH (NOLOCK)
				ON P.SP_ID = BA.SP_ID COLLATE DATABASE_DEFAULT AND P.Display_Seq = BA.SP_Practice_Display_Seq AND BA.Display_Seq = 1
			INNER JOIN @ServiceProviderStatus SPS
				ON SP.Record_Status = SPS.Status_Value COLLATE DATABASE_DEFAULT
			-- The practice is providing DHC-related Services 
			LEFT JOIN (SELECT pt.SP_ID, pt.Display_Seq FROM DHCSPMapping d WITH (NOLOCK) 
						INNER JOIN Professional p WITH (NOLOCK)
						ON d.Service_Category_Code = p.Service_Category_Code
							AND d.Registration_Code = p.Registration_Code
							AND p.Record_Status = 'A'
						INNER JOIN Practice pt WITH (NOLOCK)
						ON p.SP_ID = pt.SP_ID
							AND p.Professional_Seq = pt.Professional_Seq)  DHC
				ON P.SP_ID = DHC.SP_ID
					AND P.Display_Seq = DHC.Display_Seq
-- ---------------------------------------------
-- For Excel Sheet (05): 03-Practice & BankAcc (Part 2)
-- ---------------------------------------------

	-- Create Pivot Table
	CREATE TABLE #PivotTable_WS05_P2 (
		SP_ID			char(8),
		Display_Seq		VARCHAR(20)
	)

	-- Add Column for Pivot Table dynamically
	EXECUTE('ALTER TABLE #PivotTable_WS05_P2 ADD ' + @pivot_table_column_header)

	-- Prepare Dynamic SQL String for INSERT statement of Pivot Table
	SET @sql_script_pivot_table_insert = ' ([SP_ID],[Display_Seq],' + @pivot_table_column_list + ') '

	-- Create Column Header dynamically
	SET @sql_script = 'INSERT INTO #PivotTable_WS05_P2' + @sql_script_pivot_table_insert + 'VALUES (''SPID'',''Practice No.'',' + @pivot_table_column_name_value + ')'
	EXECUTE(@sql_script)

	-- Generate Pivot Table for Result
	SET @sql_script = 'SELECT *
		FROM (
			SELECT
				SP_ID,
				Display_Seq,
				Scheme_Display_Code,
				Scheme_Record_Status
			FROM
				#Practice_Filtered_With_Scheme_Info
		) DATA
		PIVOT (
			MAX(Scheme_Record_Status)
			FOR Scheme_Display_Code
			IN (' + @pivot_table_column_list + ')
		) FUNC'

	SET @sql_script = 'INSERT INTO #PivotTable_WS05_P2' + @sql_script_pivot_table_insert + @sql_script
	EXECUTE(@sql_script)


-- ---------------------------------------------
-- For Excel Sheet (05): 03-Practice & BankAcc (Part 1.5) - Scheme with non-clinic settings
-- ---------------------------------------------

	CREATE TABLE #PivotTable_WS05_NonClinicScheme (
		SP_ID				char(8),
		Display_Seq			VARCHAR(20),
		Scheme_Code			char(10),
		Scheme_Display_Seq	smallint
	)
	
	CREATE TABLE #PivotTable_WS05_NonClinicSchemeSummary (
		SP_ID			char(8),
		Display_Seq		VARCHAR(20),
		Scheme_Code		VARCHAR(8000)
	)
	
	--
	
	INSERT INTO #PivotTable_WS05_NonClinicScheme (
		SP_ID,
		Display_Seq,
		Scheme_Code,
		Scheme_Display_Seq
	)
	SELECT
		P.SP_ID,
		P.Display_Seq,
		SB.Display_Code,
		SB.Display_Seq
	FROM
		#Practice_Filtered P
			INNER JOIN PracticeSchemeInfo PSI
				ON P.SP_ID = PSI.SP_ID
					AND P.Display_Seq = PSI.Practice_Display_Seq
					AND PSI.Clinic_Type = 'N'
			INNER JOIN @SchemeBackOffice SB
				ON PSI.Scheme_Code = SB.Scheme_Code
	GROUP BY
		P.SP_ID,
		P.Display_Seq,
		SB.Display_Code,
		SB.Display_Seq
		
	--
	
	INSERT INTO #PivotTable_WS05_NonClinicSchemeSummary (
		SP_ID,
		Display_Seq,
		Scheme_Code
	)
	SELECT
		SP_ID,
		Display_Seq,
		RIGHT(Scheme_Code, LEN(Scheme_Code) - 2)
	FROM (
		SELECT
			SP_ID,
			Display_Seq,
			(SELECT
				', ' + CONVERT(VARCHAR, RTRIM(Scheme_Code)) 
			FROM
				#PivotTable_WS05_NonClinicScheme
			WHERE
				SP_ID = S.SP_ID
					AND Display_Seq = S.Display_Seq
			ORDER BY
				Scheme_Display_Seq
			FOR
				XML PATH('')
			) AS Scheme_Code
		FROM
			#PivotTable_WS05_NonClinicScheme S
		GROUP BY
			SP_ID,
			Display_Seq
	) T
	
	-- Add header
	
	INSERT INTO #PivotTable_WS05_NonClinicSchemeSummary (
		SP_ID,
		Display_Seq,
		Scheme_Code
	)
	SELECT
		'SPID',
		'Practice No.',
		'Scheme with non-clinic settings'


-- ---------------------------------------------
-- For Excel Sheet (05): 03-Practice & BankAcc (Part 3)
-- ---------------------------------------------

	-- Create Pivot Table
	CREATE TABLE #PivotTable_WS05_P3 (
		SP_ID			char(8),
		Display_Seq		VARCHAR(20)
	)

	-- Add Column for Pivot Table dynamically
	EXECUTE('ALTER TABLE #PivotTable_WS05_P3 ADD ' + @pivot_table_subsidize_column_header)

	-- Prepare Dynamic SQL String for INSERT statement of Pivot Table
	SET @sql_script_pivot_table_insert = ' ([SP_ID],[Display_Seq],' + @pivot_table_subsidize_column_list + ') '

	-- Create Column Header dynamically
	SET @sql_script = 'INSERT INTO #PivotTable_WS05_P3' + @sql_script_pivot_table_insert + 'VALUES (''SPID'',''Practice No.'',' + @pivot_table_subsidize_column_name_value + ')'
	EXECUTE(@sql_script)
	
	-- Generate Pivot Table for Result
	SET @sql_script = 'SELECT *
		FROM (
			SELECT
				SP_ID,
				Display_Seq,
				Display_Desc,
				Service_Fee
			FROM
				#Practice_Filtered_With_Subsidize_Info
		) DATA
		PIVOT (
			MAX(Service_Fee)
			FOR Display_Desc
			IN (' + @pivot_table_subsidize_column_list + ')
		) FUNC'

	SET @sql_script = 'INSERT INTO #PivotTable_WS05_P3' + @sql_script_pivot_table_insert + @sql_script
	EXECUTE(@sql_script)

-- ---------------------------------------------
-- For Excel Sheet (07): Remarks
-- ---------------------------------------------

	-- Create Temp Result Table
	CREATE TABLE #WS07 (
		Seq		int,
		Seq2	int,
		Col01	VARCHAR(1000),
		Col02	VARCHAR(1000)
	)

	SET @seq = 0

	INSERT INTO #WS07 (Seq, Seq2, Col01, Col02)
	VALUES (@seq, NULL, '(A) Legend', '')

	SET @seq = @seq + 1

	INSERT INTO #WS07 (Seq, Seq2, Col01, Col02)
	VALUES (@seq, NULL, '1. Scheme Name', '')
	
	SET @seq = @seq + 1

	INSERT INTO #WS07 (Seq, Seq2, Col01, Col02)
	SELECT @seq, NULL, Display_Code, Scheme_Desc FROM @SchemeBackOffice

	SET @seq = @seq + 1

	INSERT INTO #WS07 (Seq, Seq2, Col01, Col02)
	VALUES (@seq, NULL, '', '')

	SET @seq = @seq + 1

	INSERT INTO #WS07 (Seq, Seq2, Col01, Col02)
	VALUES (@seq, NULL, '2. Profession Type', '')

	SET @seq = @seq + 1

	INSERT INTO #WS07 (Seq, Seq2, Col01, Col02)
	SELECT @seq, NULL, Service_Category_Code, Service_Category_Desc FROM Profession WITH (NOLOCK)

	SET @seq = @seq + 1

	INSERT INTO #WS07 (Seq, Seq2, Col01, Col02)
	VALUES (@seq, NULL, '', '')

	SET @seq = @seq + 1

	INSERT INTO #WS07 (Seq, Seq2, Col01, Col02)
	VALUES (@seq, NULL, '3. With Active Practice Enrolled in Scheme', '')

	SET @seq = @seq + 1

	INSERT INTO #WS07 (Seq, Seq2, Col01, Col02)
	VALUES (@seq, NULL, 'Yes - At least one practice enrolled in the scheme with active status', '')
	
	SET @seq = @seq + 1

	INSERT INTO #WS07 (Seq, Seq2, Col01, Col02)
	VALUES (@seq, NULL, 'No - No practices enrolled in the scheme with active status (i.e. suspended or delisted)', '')
	
	SET @seq = @seq + 1

	INSERT INTO #WS07 (Seq, Seq2, Col01, Col02)
	VALUES (@seq, NULL, 'N/A - No practices enrolled in the scheme', '')
				
	SET @seq = @seq + 1

	INSERT INTO #WS07 (Seq, Seq2, Col01, Col02)
	VALUES (@seq, NULL, '', '')
	
	SET @seq = @seq + 1

	INSERT INTO #WS07 (Seq, Seq2, Col01, Col02)
	VALUES (@seq, NULL, '(B) Common Note(s) for the report', '')

	SET @seq = @seq + 1

	INSERT INTO #WS07 (Seq, Seq2, Col01, Col02)
	VALUES (@seq, NULL, '1. Dummy Service Provider accounts are excluded from the statistic', '')

	EXEC [proc_SymmetricKey_close]

	DROP TABLE #SP_Filtered
	DROP TABLE #Practice_Filtered
	DROP TABLE #SP_Filtered_With_Scheme_Info
	DROP TABLE #Practice_Filtered_With_Scheme_Info
	DROP TABLE #Practice_Filtered_With_Subsidize_Info

-- =============================================
-- Return results
-- =============================================
-- ---------------------------------------------
-- To Excel Sheet (01): Content
-- ---------------------------------------------

	SELECT 'Report Generation Time: ' + CONVERT(VARCHAR(10), @current_dtm, 111) + ' ' + CONVERT(VARCHAR(5), @current_dtm, 114)

-- ---------------------------------------------
-- To Excel Sheet (02): Criteria
-- ---------------------------------------------

	SELECT 'Health Profession', @criteria01

-- ---------------------------------------------
-- To Excel Sheet (03): 01-Service Provider
-- ---------------------------------------------

	SET @sql_script = '
		SELECT
			P1.Col01, P1.Col02, P1.Col03, P1.Col04, P1.Col05, P1.Col06, P1.Col07, P1.Col08, P1.Col09, P1.Col10,
			P1.Col11, P1.Col12, P1.Col13, P1.Col14, P1.Col15, P1.Col16, P1.Col17, P1.Col18, P1.Col19, P1.Col20,
			P1.Col21, P1.Col22, P1.Col23, P1.Col24, P1.Col25, ' + @pivot_table_column_name_alias + ',' +  @pivot_table_practice_scheme_column_name_alias + '
		FROM #WS03_Part1 P1
			INNER JOIN #PivotTable_WS03 PT
				ON P1.Col01 = PT.SP_ID COLLATE DATABASE_DEFAULT
			INNER JOIN #PivotTable_WS03_P3 PT2
				ON P1.Col01 = PT2.SP_ID COLLATE DATABASE_DEFAULT
		ORDER BY P1.Seq ASC, P1.Col04 DESC, P1.Col01 ASC'
	EXECUTE(@sql_script)



	DROP TABLE #WS03_Part1
	DROP TABLE #PivotTable_WS03
	DROP TABLE #PivotTable_WS03_P3


-- ---------------------------------------------
-- To Excel Sheet (04): 02-MO
-- ---------------------------------------------

	SELECT
		Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10,
		Col11, Col12, Col13, Col14, Col15, Col16, Col17, Col18
	FROM #WS04
	ORDER BY Seq ASC, Col04 DESC, Col01 ASC, Col06 ASC

	DROP TABLE #WS04

-- ---------------------------------------------
-- To Excel Sheet (05): 03-Practice & BankAcc
-- ---------------------------------------------

	SET @sql_script = '
		SELECT
			P1.Col01, P1.Col02, P1.Col03, P1.Col04, P1.Col05, P1.Col06, P1.Col07, P1.Col08, P1.Col09, P1.Col10,
			P1.Col11, P1.Col12, P1.Col13, P1.Col14, P1.Col15, P1.Col16, P1.Col17, P1.Col18, P1.Col19, P1.Col20,
			P1.Col21, P1.Col22, P1.Col23, P1.Col24, P1.Col28, P1.Col29, P1.Col30, P1.Col31, ' + @pivot_table_column_name_alias +','
			+ 'ISNULL(NC.Scheme_Code,''N/A''),'
			+ @pivot_table_subsidize_column_name_alias + ', P1.Col25, P1.Col26, P1.Col27
		FROM #WS05_Part1 P1
			INNER JOIN #PivotTable_WS05_P2 PT
				ON	P1.Col01 = PT.SP_ID COLLATE DATABASE_DEFAULT
					AND P1.Col10 = PT.Display_Seq COLLATE DATABASE_DEFAULT
			INNER JOIN #PivotTable_WS05_P3 PT2
				ON	P1.Col01 = PT2.SP_ID COLLATE DATABASE_DEFAULT
					AND P1.Col10 = PT2.Display_Seq COLLATE DATABASE_DEFAULT
			LEFT JOIN #PivotTable_WS05_NonClinicSchemeSummary NC
				ON P1.Col01 = NC.SP_ID COLLATE DATABASE_DEFAULT
					AND P1.Col10 = NC.Display_Seq COLLATE DATABASE_DEFAULT
		ORDER BY P1.Seq ASC, P1.Col04 DESC, P1.Col01 ASC, P1.Col10 ASC'
	

	EXECUTE(@sql_script)

	DROP TABLE #WS05_Part1
	DROP TABLE #PivotTable_WS05_P2
	DROP TABLE #PivotTable_WS05_P3
	DROP TABLE #PivotTable_WS05_NonClinicScheme
	DROP TABLE #PivotTable_WS05_NonClinicSchemeSummary

-- ---------------------------------------------
-- To Excel Sheet (06): 04-Dummy SP Account
-- ---------------------------------------------

	SELECT SP_ID FROM SPExceptionList WITH (NOLOCK) ORDER BY SP_ID

-- ---------------------------------------------
-- To Excel Sheet (07): Remarks
-- ---------------------------------------------

	SELECT Col01, Col02 FROM #WS07 ORDER BY Seq, Seq2, Col01

	DROP TABLE #WS07

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSU0005_Report_get] TO HCVU
GO

