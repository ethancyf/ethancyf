IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ccc_big5_get_byUnicode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ccc_big5_get_byUnicode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM		
-- Modified date:	12 Aug 2019
-- CR No.			CRE19-001
-- Description:		Grant EXECUTE right for role HCSP
-- =============================================
-- =============================================
-- Author:			Winnie SUEN
-- Create date:		28 Aug 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Get CCCode by unicode 
-- =============================================
CREATE PROCEDURE [dbo].[proc_ccc_big5_get_byUnicode]
	   @UniCode_Int			INT									
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
    SELECT 	
		ccc_head as CCC_Head 
		,ccc_tail as CCC_Tail
		,unicode_int as UniCode_Int
		,phonetic_text  as Phonetic_Text
    FROM  
		dbo.ccc_big5
    WHERE  
		UniCode_Int = @UniCode_Int

END
GO

GRANT EXECUTE ON [dbo].[proc_ccc_big5_get_byUnicode] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_ccc_big5_get_byUnicode] TO HCSP
GO

