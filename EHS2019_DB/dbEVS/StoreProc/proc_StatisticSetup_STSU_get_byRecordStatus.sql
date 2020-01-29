IF EXISTS (
SELECT 1 FROM Sys.Objects
WHERE [name] = 'proc_StatisticSetup_STSU_get_byRecordStatus' AND [type] = 'P')
BEGIN
	DROP PROCEDURE proc_StatisticSetup_STSU_get_byRecordStatus
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Modification History
-- Modified by:		Chris YIM	
-- Modified date:	30 May 2016
-- CR No.			CRE15-016
-- Description:		Randomly generate the valid claim transaction
-- ==========================================================================================
-- ==========================================================================================
-- Author:	Nick POON
-- Create Date:	09 Nov 2012
-- Description:	Retrieve statistic list by record status - [StatisticSetup_STSU]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_StatisticSetup_STSU_get_byRecordStatus]
	@record_status char(1),
	@show_for_generation char(1)
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
	FG.Show_for_Generation,
	FG.Display_Code,
	FG.[File_ID],
	FG.[File_Name],
	FG.File_Desc
FROM
	StatisticSetup_STSU STSU
		INNER JOIN FileGeneration FG
			ON STSU.STSU_Statistic_ID = FG.[File_ID]
WHERE
	STSU.STSU_Record_Status = @record_status
	AND FG.Show_for_Generation = @show_for_generation
ORDER BY
	STSU_Statistic_ID
	
END
GO

GRANT EXECUTE ON [dbo].[proc_StatisticSetup_STSU_get_byRecordStatus] TO HCVU
GO