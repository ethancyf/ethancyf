IF EXISTS (
SELECT 1 FROM Sys.Objects
WHERE [name] = 'proc_StatisticCriteriaAdditionDetail_SCAD_get_byStatisticID' AND [type] = 'P')
BEGIN
	DROP PROCEDURE proc_StatisticCriteriaAdditionDetail_SCAD_get_byStatisticID
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Author:	Nick POON
-- Create Date:	19 Dec 2012
-- Description:	Retrieve statistic criteria addition detail by statistic ID - [StatisticCriteriaAdditionDetail_SCAD]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_StatisticCriteriaAdditionDetail_SCAD_get_byStatisticID]
	@statistic_id varchar(30),
	@control_id varchar(50),
	@field_id varchar(50)
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
	SCAD_Statistic_ID,
	SCAD_ControlID,
	SCAD_FieldID,
	SCAD_SetupType,
	SCAD_SetupValue,
	SCAD_Create_Dtm,
	SCAD_Create_By,
	SCAD_Update_Dtm,
	SCAD_Update_By
FROM
	StatisticCriteriaAdditionDetail_SCAD
WHERE
	SCAD_Statistic_ID = @statistic_id
	AND SCAD_ControlID = @control_id
	AND SCAD_FieldID = @field_id	
END
GO

GRANT EXECUTE ON [dbo].[proc_StatisticCriteriaAdditionDetail_SCAD_get_byStatisticID] TO HCVU
GO