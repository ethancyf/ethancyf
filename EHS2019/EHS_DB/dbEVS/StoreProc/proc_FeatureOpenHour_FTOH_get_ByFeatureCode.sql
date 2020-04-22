IF EXISTS (
SELECT 1 FROM Sys.Objects
WHERE [name] = 'proc_FeatureOpenHour_FTOH_get_ByFeatureCode' AND [type] = 'P')
BEGIN
	DROP PROCEDURE proc_FeatureOpenHour_FTOH_get_ByFeatureCode
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Author:	Nick POON
-- Create Date:	18 Dec 2012
-- Description:	Get Feature Open Hour By Feature Code - [FeatureOpenHour_FTOH]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_FeatureOpenHour_FTOH_get_ByFeatureCode]
	@feature_code varchar(50)
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
	FTOH_Feature_Code,
	FTOH_From_Time,
	FTOH_To_Time
FROM
	FeatureOpenHour_FTOH
WHERE
	FTOH_Feature_Code = @feature_code
	AND FTOH_Record_Status = 'A'
END
GO

GRANT EXECUTE ON [dbo].[proc_FeatureOpenHour_FTOH_get_ByFeatureCode] TO HCVU
GO