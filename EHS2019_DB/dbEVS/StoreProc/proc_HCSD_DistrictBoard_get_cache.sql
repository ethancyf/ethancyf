IF EXISTS (
SELECT 1 FROM Sys.Objects
WHERE [name] = 'proc_HCSD_DistrictBoard_get_cache' AND [type] = 'P')
BEGIN
	DROP PROCEDURE proc_HCSD_DistrictBoard_get_cache
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- CR No.:		CRE13-019-02
-- Author:		Winnie SUEN
-- Create date:	21 Apr 2015
-- Description:	Refine District Struture
-- =============================================
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_HCSD_DistrictBoard_get_cache]
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
	
	SELECT ltrim(rtrim(DB.district_board)) + ' (' + ltrim(rtrim(DB.district_board_shortname_SD)) + ')' as [district_board]
		  ,DB.district_board_chi
		  ,DB.district_board_shortname_SD
		  ,DB.area_name
		  ,DB.area_name_chi
		  ,DB.area_code
		  ,DA.EForm_Input_Avail
		  ,DA.BO_Input_Avail
		  ,DA.SD_Input_Avail
	FROM [dbo].[DistrictBoard] DB
	JOIN	District_Area DA ON DB.Area_Code = DA.Area_Code

	UNION

	SELECT DISTINCT '' AS [district_board]
		  ,'' AS district_board_chi
		  ,'' AS district_board_shortname_SD
		  ,DB.area_name
		  ,DB.area_name_chi
		  ,DB.area_code
		  ,DA.EForm_Input_Avail
		  ,DA.BO_Input_Avail
		  ,DA.SD_Input_Avail
	FROM [dbo].[DistrictBoard] DB
	JOIN	District_Area DA ON DB.Area_Code = DA.Area_Code

	ORDER BY [area_code], [district_board] 		

END
GO

GRANT EXECUTE ON [dbo].[proc_HCSD_DistrictBoard_get_cache] TO HCPUBLIC
GO



