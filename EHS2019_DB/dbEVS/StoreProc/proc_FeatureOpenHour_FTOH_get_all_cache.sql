IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_FeatureOpenHour_FTOH_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_FeatureOpenHour_FTOH_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================  
-- Modification History  
-- CR No.:
-- Modified by:   
-- Modified date: 
-- Description:   
-- =============================================  
-- =============================================  
-- Author:		Koala CHENG
-- CR No.:		CRE12-014
-- Create date: 03 Jan 2013
-- Description: Get FeatureOpenHour setting for cache
-- =============================================    
CREATE PROCEDURE [dbo].[proc_FeatureOpenHour_FTOH_get_all_cache]   
AS  
BEGIN  

-- =============================================  
-- Declaration  
-- =============================================    
-- =============================================  
-- Validation   
-- =============================================  
-- =============================================  
-- Initialization  
-- =============================================  
-- =============================================  
-- Return results  
-- =============================================  
	SELECT 
		FTOH_Feature_Code,
		FTOH_From_Time,
		FTOH_To_Time,
		FTOH_Record_Status,
		FTOH_Create_Dtm,
		FTOH_Create_By
	FROM
		FeatureOpenHour_FTOH WITH (NOLOCK)
   
END  
GO

GRANT EXECUTE ON [dbo].[proc_FeatureOpenHour_FTOH_get_all_cache] TO HCVU
GO
