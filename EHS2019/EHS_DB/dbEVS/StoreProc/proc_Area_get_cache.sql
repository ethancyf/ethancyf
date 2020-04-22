IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Area_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Area_get_cache]
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
-- Description:	Retrieve the area
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_Area_get_cache]
	@area_code	char(1)
AS
BEGIN
	
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	if ltrim(rtrim(@area_code)) = ''
		SELECT @area_code = NULL
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	SELECT	Area_Code ,Area_name ,Area_chi, 
			EForm_Input_Avail, BO_Input_Avail, SD_Input_Avail
	FROM	dbo.District_Area
	WHERE	(@area_code is null OR area_code = @area_code)
			--or ((area_code <> '4') and (@platform <> '03' or area_code <> '5')))
END
GO

GRANT EXECUTE ON [dbo].[proc_Area_get_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_Area_get_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_Area_get_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_Area_get_cache] TO WSEXT
Go

GRANT EXECUTE ON [dbo].[proc_Area_get_cache] TO WSINT
Go