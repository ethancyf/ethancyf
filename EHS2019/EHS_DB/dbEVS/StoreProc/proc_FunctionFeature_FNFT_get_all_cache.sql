IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_FunctionFeature_FNFT_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_FunctionFeature_FNFT_get_all_cache]
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
-- Description: Get FunctionFeature setting for cache
-- =============================================    
CREATE PROCEDURE [dbo].[proc_FunctionFeature_FNFT_get_all_cache]   
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
		FNFT_Function_Code,
		FNFT_Feature_Code,
		FNFT_Record_Status,
		FNFT_Create_Dtm,
		FNFT_Create_By
	FROM
		FunctionFeature_FNFT WITH (NOLOCK)
   
END  
GO

GRANT EXECUTE ON [dbo].[proc_FunctionFeature_FNFT_get_all_cache] TO HCVU
GO
