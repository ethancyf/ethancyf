IF EXISTS (
SELECT 1 FROM Sys.Objects
WHERE [name] = 'proc_DistrictBoard_get_cache' AND [type] = 'P')
BEGIN
	DROP PROCEDURE proc_DistrictBoard_get_cache
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- CR No.:		CRE20-006 
-- Author:		Nichole IP
-- Create date:	06 May 2021
-- Description:	Grant right to HCSP 
-- =============================================
-- =============================================
-- CR No.:		CRE13-019-02
-- Author:		Winnie SUEN
-- Create date:	21 Apr 2015
-- Description:	Refine District Structure
-- =============================================
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_DistrictBoard_get_cache]
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
	DB.district_board,
	DB.district_board_chi,
	DB.district_board_shortname_SD,
	DB.area_name,
	DB.area_name_chi,
	DB.area_code,	
	DA.EForm_Input_Avail,
	DA.BO_Input_Avail,
	DA.SD_Input_Avail,
	DB.DHC_District_Code 
FROM
	DistrictBoard DB
	JOIN District_Area DA ON DB.Area_Code = DA.Area_Code
ORDER BY
	DB.Display_Seq
	
END
GO

GRANT EXECUTE ON [dbo].[proc_DistrictBoard_get_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_DistrictBoard_get_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_DistrictBoard_get_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_DistrictBoard_get_cache] TO WSEXT
GO

