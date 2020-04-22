IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSD_get_District_byArea_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSD_get_District_byArea_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Mattie LO
-- Create date: 29 August 2009
-- Description:	 Retrieve the 18 districts list
-- =============================================
CREATE PROCEDURE [dbo].[proc_HCSD_get_District_byArea_cache] 
	@area_code char(1)
AS
BEGIN

	SELECT ltrim(rtrim([district_board])) + ' (' + ltrim(rtrim([district_board_shortname_SD])) + ')' as [district_board]
		  ,[district_board_chi]
		  ,[district_board_shortname_SD]
		  ,[area_name]
		  ,[area_name_chi]
		  ,[area_code]
	FROM [dbo].[SDDistrictBoard]
	WHERE [area_code] = @area_code OR @area_code IS NULL

	UNION

	SELECT distinct '' as [district_board]
		  ,'' as [district_board_chi]
		  ,'' as [district_board_shortname_SD]
		  ,[area_name]
		  ,[area_name_chi]
		  ,[area_code]
	FROM [dbo].[SDDistrictBoard]
	WHERE [area_code] = @area_code OR @area_code IS NULL

	ORDER BY [area_code], [district_board] 	
END
GO

GRANT EXECUTE ON [dbo].[proc_HCSD_get_District_byArea_cache] TO HCPUBLIC
GO
