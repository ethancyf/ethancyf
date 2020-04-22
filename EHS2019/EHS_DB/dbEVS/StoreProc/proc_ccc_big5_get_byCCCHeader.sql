IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ccc_big5_get_byCCCHeader]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ccc_big5_get_byCCCHeader]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- CR No.			CRE15-014
-- Modified date:	31 Dec 2015
-- Description:		Return [UniCode_Int] instead of [big5]
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 07 May 2008
-- Description:	Retrieve the cccode tail with the ccc header
-- =============================================
CREATE PROCEDURE [dbo].[proc_ccc_big5_get_byCCCHeader]
	   @ccc_head			char(4)										
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
    SELECT 	 ccc_tail   as CCC_Tail
			,unicode_int as UniCode_Int--nchar(unicode_int)	as Big5   
			,phonetic_text  as Phonetic_Text
    FROM  dbo.ccc_big5
    WHERE  ccc_head = @ccc_head
    ORDER BY ccc_tail
END
GO

GRANT EXECUTE ON [dbo].[proc_ccc_big5_get_byCCCHeader] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_ccc_big5_get_byCCCHeader] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ccc_big5_get_byCCCHeader] TO HCVU
GO
