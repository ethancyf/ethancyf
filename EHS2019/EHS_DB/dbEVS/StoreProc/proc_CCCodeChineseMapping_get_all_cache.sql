IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_CCCodeChineseMapping_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_CCCodeChineseMapping_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- CR No.			
-- Modified date:	
-- Description:		
-- =============================================
-- =============================================
-- Author:			Winnie SUEN
-- CR No.			CRE20-023-68 (COVID19)
-- Create date:		6 Dec 2021
-- Description:		Retrieve the [CCCodeChineseMapping]
-- =============================================

CREATE PROCEDURE [dbo].[proc_CCCodeChineseMapping_get_all_cache] 
AS
BEGIN
-- =============================================
--	 Declaration                                 	                        
-- =============================================
-- =============================================
--   Validation                                		  
-- =============================================
-- =============================================
--   Initialization                            		   
-- =============================================
-- =============================================
--   Return results                                              
-- =============================================
	SET NOCOUNT ON;
	
	SELECT 
		CCCode, 
		UniCode_Int, 
		Final_Character, 
		CCC_Head, 
		CCC_Tail		
	FROM 
		CCCodeChineseMapping WITH (NOLOCK)
	ORDER BY
		CCCode

END
GO

GRANT EXECUTE ON [dbo].[proc_CCCodeChineseMapping_get_all_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_CCCodeChineseMapping_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_CCCodeChineseMapping_get_all_cache] TO HCVU
GO
