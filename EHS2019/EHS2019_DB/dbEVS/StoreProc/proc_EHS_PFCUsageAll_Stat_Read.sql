IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_PFCUsageAll_Stat_Read]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_PFCUsageAll_Stat_Read]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		27 October 2009
-- Description:		Generate report for the usage of PFC platform
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_PFCUsageAll_Stat_Read] 
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
	DECLARE @ReportDtmTable table (
		Report_Dtm	datetime
	)
	
	DECLARE @ResultTable table (
		Display_Seq		smallint,
		Result_Value1	varchar(100),
		Result_Value2	varchar(100),
		Result_Value3	varchar(100),
		Result_Value4	varchar(100),
		Result_Value5	varchar(100),
		Result_Value6	varchar(100),
		Result_Value7	varchar(100),
		Result_Value8	varchar(100),
		Result_Value9	varchar(100)
	)
	
	DECLARE @MaxReportDtm	datetime
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	INSERT INTO @ReportDtmTable (
		Report_Dtm
	)
	SELECT TOP 14
		Report_Dtm
	FROM
		_EHS_PFCUsage_Stat
	GROUP BY
		Report_Dtm
	ORDER BY
		Report_Dtm DESC
		
	SELECT @MaxReportDtm = MAX(Report_Dtm) FROM @ReportDtmTable
-- =============================================
-- Return results
-- =============================================

-- ---------------------------------------------
-- To Excel sheet:	PFC created by doc type
-- ---------------------------------------------

	-- Build output format and data
	
	INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, 
								Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) 
	VALUES (0, 'No of record created by Online Pre-filling Consent Form Module (as at ' + CONVERT(varchar, @MaxReportDtm, 111) + ')', '', '', '', 
								'', '', '', '', '')
	
	INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, 
								Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) 
	VALUES (1, 'By document type', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, 
								Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) 
	VALUES (2, '', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, 
								Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) 
	VALUES (3, 'Date', 'HKIC', 'HKBC', 'Doc/I', 'REPMT', 'ID235B', 'VISA', 'ADOPC', 'Total')
	
	INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, 
								Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) 
	SELECT
		P.Display_Seq,
		CONVERT(varchar, P.Report_Dtm, 111),
		P.Result_Value1,
		P.Result_Value2,
		P.Result_Value3,
		P.Result_Value4,
		P.Result_Value5,
		P.Result_Value6,
		P.Result_Value7,
		P.Result_Value8
	FROM 
		_EHS_PFCUsage_Stat P
			INNER JOIN @ReportDtmTable R
				ON P.Report_Dtm = R.Report_Dtm
	WHERE
		P.Report_Type = 'C'
	ORDER BY
		P.Report_Dtm, P.Display_Seq

	INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, 
								Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) 
	VALUES (20, 'Total', '', '', '', '', '', '', '', '')
	
	-- Count total
	
	UPDATE
		@ResultTable
	SET
		Result_Value2 = (SELECT SUM(CONVERT(int, Result_Value2)) FROM @ResultTable WHERE Display_Seq = 10),
		Result_Value3 = (SELECT SUM(CONVERT(int, Result_Value3)) FROM @ResultTable WHERE Display_Seq = 10),
		Result_Value4 = (SELECT SUM(CONVERT(int, Result_Value4)) FROM @ResultTable WHERE Display_Seq = 10),
		Result_Value5 = (SELECT SUM(CONVERT(int, Result_Value5)) FROM @ResultTable WHERE Display_Seq = 10),
		Result_Value6 = (SELECT SUM(CONVERT(int, Result_Value6)) FROM @ResultTable WHERE Display_Seq = 10),
		Result_Value7 = (SELECT SUM(CONVERT(int, Result_Value7)) FROM @ResultTable WHERE Display_Seq = 10),
		Result_Value8 = (SELECT SUM(CONVERT(int, Result_Value8)) FROM @ResultTable WHERE Display_Seq = 10),
		Result_Value9 = (SELECT SUM(CONVERT(int, Result_Value9)) FROM @ResultTable WHERE Display_Seq = 10)
	WHERE
		Display_Seq = 20
		
	-- Select result
	
	SELECT
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4, 
		Result_Value5,
		Result_Value6,
		Result_Value7,
		Result_Value8,
		Result_Value9
	FROM
		@ResultTable
	ORDER BY
		Display_Seq
		
	DELETE FROM @ResultTable
	
-- ---------------------------------------------
-- To Excel sheet:	PFC used by doc type
-- ---------------------------------------------

	-- Build output format and data
	
	INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, 
								Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) 
	VALUES (0, 'No of Online Pre-filling Consent Form Record used (as at ' + CONVERT(varchar, @MaxReportDtm, 111) + ')', '', '', '', 
								'', '', '', '', '')
								
	INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, 
								Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) 
	VALUES (1, 'By document type', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, 
								Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) 
	VALUES (2, '', '', '', '', '', '', '', '', '')
	
	INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, 
								Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) 
	VALUES (3, 'Date', 'HKIC', 'HKBC', 'Doc/I', 'REPMT', 'ID235B', 'VISA', 'ADOPC', 'Total')
	
	INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, 
								Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) 
	SELECT
		P.Display_Seq,
		CONVERT(varchar, P.Report_Dtm, 111),
		P.Result_Value1,
		P.Result_Value2,
		P.Result_Value3,
		P.Result_Value4,
		P.Result_Value5,
		P.Result_Value6,
		P.Result_Value7,
		P.Result_Value8
	FROM 
		_EHS_PFCUsage_Stat P
			INNER JOIN @ReportDtmTable R
				ON P.Report_Dtm = R.Report_Dtm
	WHERE
		P.Report_Type = 'U'
	ORDER BY
		P.Report_Dtm, P.Display_Seq

	INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, 
								Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9) 
	VALUES (20, 'Total', '', '', '', '', '', '', '', '')
	
	-- Count total
	
	UPDATE
		@ResultTable
	SET
		Result_Value2 = (SELECT SUM(CONVERT(int, Result_Value2)) FROM @ResultTable WHERE Display_Seq = 10),
		Result_Value3 = (SELECT SUM(CONVERT(int, Result_Value3)) FROM @ResultTable WHERE Display_Seq = 10),
		Result_Value4 = (SELECT SUM(CONVERT(int, Result_Value4)) FROM @ResultTable WHERE Display_Seq = 10),
		Result_Value5 = (SELECT SUM(CONVERT(int, Result_Value5)) FROM @ResultTable WHERE Display_Seq = 10),
		Result_Value6 = (SELECT SUM(CONVERT(int, Result_Value6)) FROM @ResultTable WHERE Display_Seq = 10),
		Result_Value7 = (SELECT SUM(CONVERT(int, Result_Value7)) FROM @ResultTable WHERE Display_Seq = 10),
		Result_Value8 = (SELECT SUM(CONVERT(int, Result_Value8)) FROM @ResultTable WHERE Display_Seq = 10),
		Result_Value9 = (SELECT SUM(CONVERT(int, Result_Value9)) FROM @ResultTable WHERE Display_Seq = 10)
	WHERE
		Display_Seq = 20
		
	-- Select result
	
	SELECT
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4, 
		Result_Value5,
		Result_Value6,
		Result_Value7,
		Result_Value8,
		Result_Value9
	FROM
		@ResultTable
	ORDER BY
		Display_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_PFCUsageAll_Stat_Read] TO HCVU
GO
