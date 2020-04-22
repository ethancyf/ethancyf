IF EXISTS (
SELECT 1 FROM Sys.Objects
WHERE [name] = 'proc_StatisticSetup_STSU_get_byStatisticID' AND [type] = 'P')
BEGIN
	DROP PROCEDURE proc_StatisticSetup_STSU_get_byStatisticID
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Author:	Nick POON
-- Create Date:	09 Nov 2012
-- Description:	Retrieve statistic list by statistic ID - [StatisticSetup_STSU]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_StatisticSetup_STSU_get_byStatisticID]
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
	STSU_Statistic_ID,
	STSU_Desc,
	STSU_ExecSP,
	STSU_Create_Dtm,
	STSU_Create_By,
	STSU_Update_Dtm,
	STSU_Update_By,
	STSU_Record_Status,
	STSU_Scheme,
	STSU_Remark
FROM
	StatisticSetup_STSU
WHERE
	STSU_Statistic_ID = @statistic_id
	
END
GO

GRANT EXECUTE ON [dbo].[proc_StatisticSetup_STSU_get_byStatisticID] TO HCVU
GO