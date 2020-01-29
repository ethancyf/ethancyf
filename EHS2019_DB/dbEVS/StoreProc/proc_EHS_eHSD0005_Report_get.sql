IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0005_Report_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0005_Report_get]
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
-- Author:			Marco CHOI
-- Create date:		25 Sep 2017
-- Description:		Add PCV13
--					SP rename from proc_EHS_AccountAll_Stat_Schedule_get
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSD0005_Report_get]
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
	DECLARE @ResultTableAccount table (
		Display_Seq		smallint,    
		Result_Value1	varchar(100),
		Result_Value2	varchar(100),
		Result_Value3	varchar(100),
		Result_Value4	varchar(100),
		Result_Value5	varchar(100)
	)
	
	DECLARE @ResultTableAge table (
		Display_Seq		smallint,
		Result_Value1	varchar(100),
		Result_Value2	varchar(100),
		Result_Value3	varchar(100),
		Result_Value4	varchar(100),
		Result_Value5	varchar(100),
		Result_Value6	varchar(100),
		Result_Value7	varchar(100),
		Result_Value8	varchar(100),
		Result_Value9	varchar(100),
		Result_Value10	varchar(100),
		Result_Value11	varchar(100),
		Result_Value12	varchar(100),
		Result_Value13	varchar(100),
		Result_Value14	varchar(100),
		Result_Value15	varchar(100),
		Result_Value16	varchar(100),
		Result_Value17	varchar(100),
		Result_Value18	varchar(100),
		Result_Value19	varchar(100),
		Result_Value20	varchar(100),
		Result_Value21	varchar(100)
	)
	
	DECLARE @ResultTable3 table (
		Display_Seq		smallint,
		Result_Value1	varchar(100),
		Result_Value2	varchar(100),
		Result_Value3	varchar(100),
		Result_Value4	varchar(100),
		Result_Value5	varchar(100),
		Result_Value6	varchar(100),
		Result_Value7	varchar(100),
		Result_Value8	varchar(100),
		Result_Value9	varchar(100),
		Result_Value10	varchar(100),
		Result_Value11	varchar(100),
		Result_Value12	varchar(100)
	)

	
	



-- =============================================
-- Initialization
-- =============================================

-- Build output format and data
	
	INSERT INTO @ResultTableAccount (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
	VALUES (0, 'Reporting period: as at ' + CONVERT(varchar, @Report_Dtm, 111), '', '', '', '')
	
	INSERT INTO @ResultTableAccount (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
	VALUES (1, '', '', '', '', '')

	INSERT INTO @ResultTableAccount (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
	VALUES (10, '', 'Validated Account', 'Special Account', 'Temporary Account','Total')

	INSERT INTO @ResultTableAccount (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
	SELECT
		20 + ROW_NUMBER() OVER(ORDER BY CountType ASC) AS [Display_Seq],
		CountType,
		validatedAC,
		specialAC,
		temporaryAC,
		--invalidAC,
		totalAC
	FROM
		_EHS_Account_ALL
	WHERE
		report_dtm = @Report_Dtm

	-- CRE11-007 
	-- ================================================================================================================
	INSERT INTO @ResultTableAccount (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
	VALUES (30, '', '', '', '', '')

	INSERT INTO @ResultTableAccount (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
	VALUES (31, 'Validated Account Breakdown by Status', '', '', '', '')

	INSERT INTO @ResultTableAccount (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
	VALUES (32, '', 'Active', 'Suspended', 'Terminated', 'Total')

	INSERT INTO @ResultTableAccount (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)
	SELECT
		33 + ROW_NUMBER() OVER(ORDER BY CountType ASC) AS [Display_Seq],
		CountType,
		validatedAC_Active,
		validatedAC_Suspended,
		validatedAC_Terminated,
		validatedAC
	FROM
		_EHS_Account_ALL
	WHERE
		report_dtm = @Report_Dtm

	-- ================================================================================================================

-- Build output format and data

	INSERT INTO @ResultTableAge (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
								Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, 
								Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20, Result_Value21)
	VALUES (0, 'Reporting period: as at ' + CONVERT(varchar, @Report_Dtm, 111), '', '', '', '', '', '',
								'', '', '', '', '', '', '', '',
								'', '', '', '', '', '')

	INSERT INTO @ResultTableAge (Display_Seq)
	VALUES (1)

	INSERT INTO @ResultTableAge (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
								Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, 
								Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20, Result_Value21)
	VALUES (2, '', '', '', '', '', '', '',
								'At age year**', '', '', '', '', '', '', '',
								'', '', '', '', '', '')

	INSERT INTO @ResultTableAge (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
								Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, 
								Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20, Result_Value21)
	VALUES (10, '', '<6 months', '6 months to <2 years', '2 years to <6 years', '6 years to <9 years', '9 years to <11 years', '11 years to <65 age year*', '65',
								'66', '67', '68', '69', '70', '71', '72', '73',
								'74', '75', '>75 age year', '', 'Total')

	INSERT INTO @ResultTableAge (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7,
								Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14, Result_Value15, 
								Result_Value16, Result_Value17, Result_Value18, Result_Value19, Result_Value20, Result_Value21)
	SELECT
		20 + ROW_NUMBER() OVER(ORDER BY CountType ASC) AS [Display_Seq],
		CountType,
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
		'' AS [Result_Value20],
		Total
	FROM
		RpteHSD0005_02_eHA_ByAge
	WHERE
		Report_Dtm = @Report_Dtm


-- Build output format and data

	INSERT INTO @ResultTable3 (Display_Seq, Result_Value1)
	VALUES (0, 'Reporting period: as at ' + CONVERT(varchar, @Report_Dtm, 111))
	
	INSERT INTO @ResultTable3 (Display_Seq)
	VALUES (1)
	
	INSERT INTO @ResultTable3 (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12)
	SELECT
		Display_Seq,
		CountType,
		Col1,
		Col2,
		Col3,
		Col4,
		Col5,
		Col6,
		Col7,
		Col8,
		Col9,
		Col10,
		Col11
	FROM
		RpteHSD0005_03_eHA_ByDocType
	WHERE
		Display_Seq IN (13, 14, 15, 16)
	
-- =============================================
-- Return results
-- =============================================

-- ---------------------------------------------
-- To Excel sheet: Content
-- ---------------------------------------------

	SELECT
		'Report Generation Time: ' + CONVERT(varchar, GETDATE(), 111) + ' ' + CONVERT(varchar(5), GETDATE(), 114)


-- ---------------------------------------------
-- To Excel sheet: 01-Acc Type
-- ---------------------------------------------

	SELECT
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5
	FROM
		@ResultTableAccount
	ORDER BY
		Display_Seq


-- ---------------------------------------------
-- To Excel sheet: 02-Age Group
-- ---------------------------------------------

	SELECT
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6,
		Result_Value7,
		Result_Value8,
		Result_Value9,
		Result_Value10,
		Result_Value11,
		Result_Value12,
		Result_Value13,
		Result_Value14,
		Result_Value15,
		Result_Value16,
		Result_Value17,
		Result_Value18,
		Result_Value19,
		Result_Value20,
		Result_Value21
	FROM
		@ResultTableAge
	ORDER BY 
		Display_Seq


-- ---------------------------------------------
-- To Excel sheet: 03-Doc Type
-- ---------------------------------------------

	SELECT
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6,
		Result_Value7,
		Result_Value8,
		Result_Value9,
		Result_Value10,
		Result_Value11,
		Result_Value12
	FROM
		@ResultTable3
	ORDER BY 
		Display_Seq


END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0005_Report_get] TO HCVU
GO
