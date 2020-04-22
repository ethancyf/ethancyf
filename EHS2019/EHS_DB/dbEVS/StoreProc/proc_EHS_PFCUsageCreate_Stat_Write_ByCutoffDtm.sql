IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_PFCUsageCreate_Stat_Write_ByCutoffDtm]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_PFCUsageCreate_Stat_Write_ByCutoffDtm]
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

CREATE PROCEDURE [dbo].[proc_EHS_PFCUsageCreate_Stat_Write_ByCutoffDtm] 
	@Cutoff_Dtm	datetime
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
	DECLARE @PreFill table (
		Doc_Code		char(20),
		Count_Doc_Code	int
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
		Result_Value8	varchar(100)
	)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

-- ---------------------------------------------
-- Retrieve data
-- ---------------------------------------------

	INSERT INTO @PreFill (
		Doc_Code,
		Count_Doc_Code
	)
	SELECT
		Doc_Code,
		COUNT(Doc_Code)
	FROM
		PreFillPersonalInformation
	WHERE
		Create_Dtm
			BETWEEN	CONVERT(varchar(11), DATEADD(dd, -1, @Cutoff_Dtm), 106) + ' 00:00:00'
			AND		CONVERT(varchar(11), DATEADD(dd, -1, @Cutoff_Dtm), 106) + ' 23:59:59'
	GROUP BY
		Doc_Code
				
-- ---------------------------------------------
-- Build output format
-- ---------------------------------------------
	
	INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, 
								Result_Value5, Result_Value6, Result_Value7, Result_Value8) 
	VALUES (10, '', '', '', '', '', '', '', '')

-- ---------------------------------------------
-- Build data
-- ---------------------------------------------

	UPDATE
		@ResultTable
	SET
		Result_Value1 = (
						SELECT
							Count_Doc_Code
						FROM
							@PreFill
						WHERE
							Doc_Code = 'HKIC'
						)
	WHERE
		Display_Seq = 10

	UPDATE
		@ResultTable
	SET
		Result_Value2 = (
						SELECT
							Count_Doc_Code
						FROM
							@PreFill
						WHERE
							Doc_Code = 'HKBC'
						)
	WHERE
		Display_Seq = 10
		
	UPDATE
		@ResultTable
	SET
		Result_Value3 = (
						SELECT
							Count_Doc_Code
						FROM
							@PreFill
						WHERE
							Doc_Code = 'Doc/I'
						)
	WHERE
		Display_Seq = 10
			
	UPDATE
		@ResultTable
	SET
		Result_Value4 = (
						SELECT
							Count_Doc_Code
						FROM
							@PreFill
						WHERE
							Doc_Code = 'REPMT'
						)
	WHERE
		Display_Seq = 10
				
	UPDATE
		@ResultTable
	SET
		Result_Value5 = (
						SELECT
							Count_Doc_Code
						FROM
							@PreFill
						WHERE
							Doc_Code = 'ID235B'
						)
	WHERE
		Display_Seq = 10
				
	UPDATE
		@ResultTable
	SET
		Result_Value6 = (
						SELECT
							Count_Doc_Code
						FROM
							@PreFill
						WHERE
							Doc_Code = 'VISA'
						)
	WHERE
		Display_Seq = 10
				
	UPDATE
		@ResultTable
	SET
		Result_Value7 = (
						SELECT
							Count_Doc_Code
						FROM
							@PreFill
						WHERE
							Doc_Code = 'ADOPC'
						)
	WHERE
		Display_Seq = 10
				
	UPDATE
		@ResultTable
	SET
		Result_Value8 = (
						SELECT
							SUM(Count_Doc_Code)
						FROM
							@PreFill
						)
	WHERE
		Display_Seq = 10
		
-- ---------------------------------------------
-- Insert data to temporary table
-- ---------------------------------------------

	INSERT INTO _EHS_PFCUsage_Stat (
		System_Dtm,
		Report_Dtm,
		Report_Type,
		Display_Seq,
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6,
		Result_Value7,
		Result_Value8
	)
	SELECT
		GETDATE(),
		DATEADD(dd, -1, @Cutoff_Dtm),
		'C',
		Display_Seq,
		ISNULL(Result_Value1, '0'),
		ISNULL(Result_Value2, '0'),
		ISNULL(Result_Value3, '0'),
		ISNULL(Result_Value4, '0'),
		ISNULL(Result_Value5, '0'),
		ISNULL(Result_Value6, '0'),
		ISNULL(Result_Value7, '0'),
		ISNULL(Result_Value8, '0')
	FROM
		@ResultTable
	ORDER BY
		Display_Seq
	
END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_PFCUsageCreate_Stat_Write_ByCutoffDtm] TO HCVU
GO
