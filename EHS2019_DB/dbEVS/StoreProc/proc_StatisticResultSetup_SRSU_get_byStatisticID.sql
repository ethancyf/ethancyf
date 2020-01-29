IF EXISTS (
SELECT 1 FROM Sys.Objects
WHERE [name] = 'proc_StatisticResultSetup_SRSU_get_byStatisticID' AND [type] = 'P')
BEGIN
	DROP PROCEDURE proc_StatisticResultSetup_SRSU_get_byStatisticID
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Author:	Nick POON
-- Create Date:	19 Dec 2012
-- Description:	Retrieve statistic result setup by statistic ID - [StatisticResultSetup_SRSU]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_StatisticResultSetup_SRSU_get_byStatisticID]
	@statistic_id varchar(30)
AS
BEGIN
-- ============================================================
-- Declaration
-- ============================================================
-- ============================================================
-- Validation
-- ============================================================
-- ============================================================
-- Initialization
-- ============================================================
-- ============================================================
-- Return results
-- ============================================================
SELECT
	SRSU_Statistic_ID,
	SRSU_ColumnName,
	SRSU_DisplayDescResource,
	SRSU_DisplayColumnWidth,
	SRSU_DisplayValueFormat,
	SRSU_ExportDescResource,
	SRSU_ExportColumnWidth,
	SRSU_ExportValueFormat,
	SRSU_Create_Dtm,
	SRSU_Create_By,
	SRSU_Update_Dtm,
	SRSU_Update_By
FROM
	StatisticResultSetup_SRSU
WHERE
	SRSU_Statistic_ID = @statistic_id
	
END
GO

GRANT EXECUTE ON [dbo].[proc_StatisticResultSetup_SRSU_get_byStatisticID] TO HCVU
GO