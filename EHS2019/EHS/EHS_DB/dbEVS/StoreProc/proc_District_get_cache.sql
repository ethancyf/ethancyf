IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_District_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_District_get_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE13-019-02
-- Modified by:		Winnie SUEN
-- Modified date:	21 Apr 2015
-- Description:		Refine District Struture
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE12-001
-- Modified by:		Tony FUNG
-- Modified date:	07 Feb 2012
-- Description:		1. Grant permission to WSINT for PCDInterface
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 22 April 2008
-- Description:	 Retrieve the district
-- =============================================
CREATE PROCEDURE [dbo].[proc_District_get_cache] 
	@area_code	char(1)
AS
BEGIN
	if ltrim(rtrim(@area_code)) = ''
			SELECT @area_code = NULL
			
					
	select District_Code ,District_Name ,District_Board	,District_Area
			,District_Chi ,District_Board_Chi 
			,EForm_Input_Avail, BO_Input_Avail, SD_Input_Avail 
	FROM (
	SELECT '.' + area_code AS District_Code
			,'-- ' + RTRIM(area_name) + ' --' AS District_Name	
			, '' as District_Board, area_code as District_Area, '' as District_Chi, '' as District_Board_Chi
			, EForm_Input_Avail, BO_Input_Avail, SD_Input_Avail , 'Y' as For_Grouping
	FROM District_Area
	UNION
	SELECT D.District_Code , D.District_Name , D.District_Board, D.District_Area
			, D.District_Chi , D.District_Board_Chi
			, EForm_Input_Avail, BO_Input_Avail, SD_Input_Avail 
			,'N' as For_Grouping
	FROM	dbo.District D
	JOIN DistrictBoard DB ON D.District_Board = DB.District_Board
	JOIN District_Area DA ON DB.Area_Code = DA.Area_Code		
--	WHERE	(@area_code is null OR District_Area = @area_code)
	) temptable
	ORDER BY District_Area, For_Grouping DESC , District_Name ASC	
	
	
END
GO

GRANT EXECUTE ON [dbo].[proc_District_get_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_District_get_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_District_get_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_District_get_cache] TO WSEXT
Go

GRANT EXECUTE ON [dbo].[proc_District_get_cache] TO WSINT
GO