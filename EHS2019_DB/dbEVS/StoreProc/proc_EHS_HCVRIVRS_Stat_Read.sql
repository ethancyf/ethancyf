IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_HCVRIVRS_Stat_Read]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_HCVRIVRS_Stat_Read]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		23 October 2009
-- Description:		Generate report for HCVR platform - IVRS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	27 October 2009
-- Description:		Select the top 14 rows only
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_HCVRIVRS_Stat_Read] 
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
	DECLARE @ResultTable table (
		Report_Dtm		datetime,
		Result_Value1	varchar(100),
		Result_Value2	varchar(100),
		Result_Value3	varchar(100)
	)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	INSERT INTO @ResultTable (
		Report_Dtm,
		Result_Value1,
		Result_Value2,
		Result_Value3
	)
	SELECT TOP 14
		Report_Dtm,
		Result_Value1,
		Result_Value2,
		Result_Value3
	FROM
		_EHS_HCVRIVRS_Stat
	ORDER BY
		Report_Dtm DESC
		
-- =============================================
-- Return results
-- =============================================
	SELECT 
		CONVERT(varchar, Report_Dtm, 111) AS [Report_Dtm],
		Result_Value1,
		Result_Value2,
		Result_Value3
	FROM
		@ResultTable
	ORDER BY
		Report_Dtm
	
END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_HCVRIVRS_Stat_Read] TO HCVU
GO
