IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ccc_big5_get_byCCCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ccc_big5_get_byCCCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- CR No.			CRE15-014
-- Modified date:	31 Dec 2015
-- Description:		Return [UniCode_Int] instead of [chi_character]
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 07 May 2008
-- Description:	Retrieve the Chinese Char by
--				CCCode
-- =============================================
CREATE PROCEDURE [dbo].[proc_ccc_big5_get_byCCCode] 
	@ccc  	        		char(5),
	--@chi_character       	nvarchar(4) output,
	@UniCode_Int	       	int output,
	@return_code    		int output,
	@return_msg     		varchar(255) output
AS
BEGIN
-- =============================================
--	 Declaration                                 	                        
-- =============================================
	DECLARE @big5   nvarchar(2)
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
	EXECUTE proc_ccc_big5_get_byIntenralCCCode  @ccc, @UniCode_Int output, @return_code output, @return_msg output

	IF @return_code <> 0
	RETURN


END
GO

GRANT EXECUTE ON [dbo].[proc_ccc_big5_get_byCCCode] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_ccc_big5_get_byCCCode] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ccc_big5_get_byCCCode] TO HCVU
GO
