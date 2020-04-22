IF EXISTS (
SELECT 1 FROM Sys.Objects
WHERE [name] = 'proc_StatisticCriteriaDetail_SCDE_get_byStatisticID' AND [type] = 'P')
BEGIN
	DROP PROCEDURE proc_StatisticCriteriaDetail_SCDE_get_byStatisticID
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Author:	Nick POON
-- Create Date:	19 Dec 2012
-- Description:	Retrieve statistic criteria detail by statistic ID - [StatisticCriteriaDetail_SCDE]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_StatisticCriteriaDetail_SCDE_get_byStatisticID]
	@statistic_id varchar(30),
	@control_id varchar(50)
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
	SCDE_Statistic_ID,
	SCDE_ControlID,
	SCDE_FieldID,
	SCDE_DescResource,
	SCDE_Visible,
	SCDE_DefaultValue,
	SCDE_SPParamName,
	SCDE_Create_Dtm,
	SCDE_Create_By,
	SCDE_Update_Dtm,
	SCDE_Update_By
FROM
	StatisticCriteriaDetail_SCDE
WHERE
	SCDE_Statistic_ID = @statistic_id
	AND SCDE_ControlID = @control_id	
END
GO

GRANT EXECUTE ON [dbo].[proc_StatisticCriteriaDetail_SCDE_get_byStatisticID] TO HCVU
GO