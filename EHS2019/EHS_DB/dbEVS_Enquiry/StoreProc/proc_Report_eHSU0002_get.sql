IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Report_eHSU0002_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Report_eHSU0002_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE21-005
-- Modified by:		Koala CHENG
-- Modified date:	1 Jun 2021
-- Description:		Exclude COVID-19 from VSS and RVP
--					Add remarks item (B)2
-- =============================================
-- ==============================================
-- Modification History
-- Modified by:		Winnie SUEN	
-- Modified date:	11 Sep 2019
-- CR No.			CRE19-006 (DHC)
-- Description:		Display Remark
-- ===============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	4 December 2015
-- CR No.:			CRE15-006
-- Description:		Rename of eHS
-- =============================================
-- =============================================
-- Author:			Tommy Lam
-- Create date:		5 Dec 2012
-- Description:		Get Report for eHSU0002
-- =============================================

CREATE PROCEDURE [dbo].[proc_Report_eHSU0002_get]
	@request_time	datetime,
	@period_type	char(1),
	@From_Date		varchar(17),
	@To_Date		varchar(17),
	@Scheme_Code	char(10),
	@User_ID		char(20)
AS BEGIN

-- =============================================
-- Declaration
-- =============================================

	DECLARE @status_data_enum_class_sp_status	varchar(50)

	DECLARE @period_from	datetime
	DECLARE @period_to		datetime

	DECLARE @si_scheme_code		char(10)

	DECLARE @web	varchar(5)
	DECLARE @ivrs	varchar(5)
	DECLARE @pcs	varchar(5)

	CREATE TABLE #ReportResult (
		Seq		int,
		Col01	varchar(120),
		Col02	varchar(40),
		Col03	varchar(30),
		Col04	varchar(30),
		Col05	varchar(30),
		Col06	varchar(30),
		Col07	varchar(30),
		Col08	varchar(30),
		Col09	varchar(30),
		Col10	varchar(30),
		Col11	varchar(30),
		Col12	varchar(30)
	)

-- =============================================
-- Validation
-- =============================================

	IF @period_type IS NULL
		RETURN

	IF @period_type <> 'T' AND @period_type <> 'S'
		RETURN

	IF @From_Date IS NULL OR @To_Date IS NULL
		RETURN

	IF @From_Date = '' OR @To_Date = ''
		RETURN

	SET @period_from = @From_Date
	SET @period_to = @To_Date

	IF @period_from > @period_to
		RETURN

	IF @Scheme_Code IS NULL
		RETURN

	IF @Scheme_Code = ''
		RETURN

-- =============================================
-- Initialization
-- =============================================

	SET @status_data_enum_class_sp_status = 'ServiceProviderStatus'

	IF @Scheme_Code = 'EVSS'
		SET @si_scheme_code = 'EVSSHSIVSS'
	ELSE
		SET @si_scheme_code = @Scheme_Code

	SET @web = 'WEB'
	SET @ivrs = 'IVRS'
	SET @pcs = 'PCS'

-- ---------------------------------------------
-- Create Header Information of Excel Sheet: 01  
-- ---------------------------------------------

	INSERT INTO #ReportResult (Seq, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12)
	VALUES (0, 'eHS(S)U0002-01: Statistics of transactions and units of EHCP from ' + CONVERT(varchar(10), @period_from, 20) + ' to ' + CONVERT(varchar(10), @period_to, 20) + ' broken down by means of input', '', '', '', '', '', '', '', '', '', '', '')

	INSERT INTO #ReportResult (Seq, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12)
	VALUES (1, '', '', '', '', '', '', '', '', '', '', '', '')

	INSERT INTO #ReportResult (Seq, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12)
	VALUES (2, '', '', '', '', 'Internet (By Manual)', '', 'Internet (By Smart IC)', '', 'IVRS', '', 'PCS', '')

	INSERT INTO #ReportResult (Seq, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12)
	VALUES (3, 'SPID', LTRIM(RTRIM(@Scheme_Code)) + ' enrolment effective date', 'Profession', 'Status', 'No. of transactions', 'No. of units', 'No. of transactions', 'No. of units', 'No. of transactions', 'No. of units', 'No. of transactions', 'No. of units')

-- =============================================
-- Retrieve Data
-- =============================================

-- ---------------------------------------------
-- Get Raw Data by Transaction  
-- ---------------------------------------------

	SELECT
		VT.SP_ID,
		SI.Effective_Dtm,
		VT.Service_Type,
		SD.Status_Description,
		VT.Transaction_ID,
		SUM(TD.Unit) AS [No_Of_Unit],
		ISNULL(VT.Create_By_SmartID, 'N') AS [Create_By_SmartID],
		CASE VT.SourceApp
			WHEN 'IVRS' THEN @ivrs
			WHEN 'ExternalWS' THEN @pcs
			ELSE @web
		END AS [SourceApp]

	INTO
		#ReportRawData

	FROM
		VoucherTransaction VT WITH (NOLOCK)
			INNER JOIN TransactionDetail TD WITH (NOLOCK)
				ON VT.Transaction_ID = TD.Transaction_ID
			INNER JOIN ServiceProvider SP WITH (NOLOCK)
				ON VT.SP_ID = SP.SP_ID
			INNER JOIN SchemeInformation SI WITH (NOLOCK)
				ON VT.SP_ID = SI.SP_ID
					AND SI.Scheme_Code = @si_scheme_code
			INNER JOIN StatusData SD WITH (NOLOCK)
				ON SP.Record_Status = SD.Status_Value
					AND SD.Enum_Class = @status_data_enum_class_sp_status

	WHERE
		(VT.Record_Status <> 'I' AND VT.Record_Status <> 'D')
			AND ((@period_type = 'T' AND VT.Transaction_Dtm >= @period_from AND VT.Transaction_Dtm < DATEADD(dd, 1, @period_to))
				OR (@period_type = 'S' AND VT.Service_Receive_Dtm >= @period_from AND VT.Service_Receive_Dtm < DATEADD(dd, 1, @period_to)))
			AND (VT.Scheme_Code = @Scheme_Code)
			AND (ISNULL(VT.Invalidation, '') <> 'I')
			AND NOT (VT.Scheme_Code IN ('VSS','RVP') and TD.Subsidize_Item_Code = 'C19')

	GROUP BY
		VT.SP_ID,
		SI.Effective_Dtm,
		VT.Service_Type,
		SD.Status_Description,
		VT.Transaction_ID,
		ISNULL(VT.Create_By_SmartID, 'N'),
		CASE VT.SourceApp
			WHEN 'IVRS' THEN @ivrs
			WHEN 'ExternalWS' THEN @pcs
			ELSE @web
		END

-- ---------------------------------------------
-- Get Data by Service Provider  
-- ---------------------------------------------

	SELECT
		SP_ID,
		Effective_Dtm,
		Service_Type,
		Status_Description,
		COUNT(Transaction_ID) AS [No_Of_Transaction],
		SUM(No_Of_Unit) AS [No_Of_Unit],
		Create_By_SmartID,
		SourceApp

	INTO
		#ReportData

	FROM
		#ReportRawData

	GROUP BY
		SP_ID,
		Effective_Dtm,
		Service_Type,
		Status_Description,
		Create_By_SmartID,
		SourceApp

-- =============================================
-- Return results
-- =============================================

-- ---------------------------------------------
-- Create Report Result of Excel Sheet: 01  
-- ---------------------------------------------

	INSERT INTO #ReportResult (Seq, Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12)
	SELECT
		4,
		LTRIM(RTRIM(RD.SP_ID)),
		CONVERT(varchar(10), RD.Effective_Dtm, 20),
		LTRIM(RTRIM(RD.Service_Type)),
		RD.Status_Description,
		CONVERT(varchar(10), ISNULL(RD1.No_Of_Transaction, 0)),
		CONVERT(varchar(10), ISNULL(RD1.No_Of_Unit, 0)),
		CONVERT(varchar(10), ISNULL(RD2.No_Of_Transaction, 0)),
		CONVERT(varchar(10), ISNULL(RD2.No_Of_Unit, 0)),
		CONVERT(varchar(10), ISNULL(RD3.No_Of_Transaction, 0)),
		CONVERT(varchar(10), ISNULL(RD3.No_Of_Unit, 0)),
		CONVERT(varchar(10), ISNULL(RD4.No_Of_Transaction, 0)),
		CONVERT(varchar(10), ISNULL(RD4.No_Of_Unit, 0))
	FROM
		(SELECT DISTINCT SP_ID, Effective_Dtm, Service_Type, Status_Description FROM #ReportData WITH (NOLOCK)) RD
		LEFT JOIN #ReportData RD1 WITH (NOLOCK)
			ON RD.SP_ID = RD1.SP_ID
				AND RD.Service_Type = RD1.Service_Type
				AND RD1.Create_By_SmartID = 'N'
				AND RD1.SourceApp = @web
		LEFT JOIN #ReportData RD2 WITH (NOLOCK)
			ON RD.SP_ID = RD2.SP_ID
				AND RD.Service_Type = RD2.Service_Type
				AND RD2.Create_By_SmartID = 'Y'
				AND RD2.SourceApp = @web
		LEFT JOIN #ReportData RD3 WITH (NOLOCK)
			ON RD.SP_ID = RD3.SP_ID
				AND RD.Service_Type = RD3.Service_Type
				AND RD3.SourceApp = @ivrs
		LEFT JOIN #ReportData RD4 WITH (NOLOCK)
			ON RD.SP_ID = RD4.SP_ID
				AND RD.Service_Type = RD4.Service_Type
				AND RD4.SourceApp = @pcs

	DROP TABLE #ReportData

-- ---------------------------------------------  
-- To Excel Sheet: Content  
-- --------------------------------------------- 

	SELECT 'Report Generation Time: ' + CONVERT(varchar(10), GETDATE(), 111) + ' ' + CONVERT(varchar(5), GETDATE(), 114)

-- ---------------------------------------------  
-- To Excel Sheet: 01  
-- --------------------------------------------- 

	SELECT
		Col01,
		Col02,
		Col03,
		Col04,
		Col05,
		Col06,
		Col07,
		Col08,
		Col09,
		Col10,
		Col11,
		Col12
	FROM #ReportResult
	ORDER BY Seq, Col01, Col03

	DROP TABLE #ReportResult


-- --------------------------------------------------  
-- To Excel sheet:   eHSM0011-Remarks: Remarks  
-- --------------------------------------------------  
	DECLARE @tblRemark AS TABLE (
		Seq	INT identity(1,1),
		Result_Value1 NVARCHAR(MAX),    
		Result_Value2 NVARCHAR(MAX)  
	)

-- Lengend

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '(A) Legend', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '1.Health Profession', ''
	
	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT Service_Category_Code, Service_Category_Desc 
	FROM Profession
	ORDER BY Service_Category_Code


	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '2.Scheme', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT Display_Code, Scheme_Desc
	FROM SchemeClaim WITH (NOLOCK)
	WHERE Record_Status = 'A' AND Scheme_Seq = 1
	ORDER BY Scheme_Code

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '', ''

	-- Common Note

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '(B) Common Note(s) for the report', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '1. Enrolment of optometrists to HCVS since Dec 2011', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '2. All COVID-19 claims under VSS and RVP are excluded.', ''

	INSERT INTO @tblRemark (Result_Value1, Result_Value2)
	SELECT '', ''


	SELECT Result_Value1, Result_Value2
	FROM @tblRemark
	ORDER BY Seq


END
GO

GRANT EXECUTE ON [dbo].[proc_Report_eHSU0002_get] TO HCVU
GO
