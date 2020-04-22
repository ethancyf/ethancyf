IF EXISTS (
SELECT 1 FROM Sys.Objects
WHERE [name] = 'proc_StatisticCriteriaSetup_SCSU_get_byStatisticID' AND [type] = 'P')
BEGIN
	DROP PROCEDURE proc_StatisticCriteriaSetup_SCSU_get_byStatisticID
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Author:	Nick POON
-- Create Date:	19 Dec 2012
-- Description:	Retrieve statistic criteria setup by statistic ID - [StatisticCriteriaSetup_SCSU]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_StatisticCriteriaSetup_SCSU_get_byStatisticID]
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
	SCSU_Statistic_ID,
	SCSU_ControlID,
	SCSU_ControlName,
	SCSU_DisplaySeq,
	SCSU_Create_Dtm,
	SCSU_Create_By,
	SCSU_Update_Dtm,
	SCSU_Update_By
FROM
	StatisticCriteriaSetup_SCSU
WHERE
	SCSU_Statistic_ID = @statistic_id
ORDER BY
	SCSU_DisplaySeq
END
GO

GRANT EXECUTE ON [dbo].[proc_StatisticCriteriaSetup_SCSU_get_byStatisticID] TO HCVU
GO