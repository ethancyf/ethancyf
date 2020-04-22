IF EXISTS (
SELECT 1 FROM Sys.Objects
WHERE [name] = 'proc_SDDistrictBoard_get_all' AND [type] = 'P')
BEGIN
	DROP PROCEDURE proc_SDDistrictBoard_get_all
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Author:	Nick POON
-- Create Date:	23 Nov 2012
-- Description:	Retrieve SD District list - [SDDistrictBoard]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_SDDistrictBoard_get_all]
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
	district_board,
	district_board_shortname_SD	
FROM
	SDDistrictBoard
ORDER BY
	district_board
	
END
GO

GRANT EXECUTE ON [dbo].[proc_SDDistrictBoard_get_all] TO HCVU
GO