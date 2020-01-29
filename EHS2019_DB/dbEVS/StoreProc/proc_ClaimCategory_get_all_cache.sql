IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ClaimCategory_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ClaimCategory_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History      
-- CR No.:			CRE16-021 Transfer VSS category to PCD
-- Modified by:		Winnie SUEN
-- Modified date:	20 Dec 2016
-- Description:		Grant permission to WSINT for PCDInterface
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	9 December 2014
-- CR No.:			CRE13-019-01
-- Description:		Support Simplified Chinese
-- =============================================
-- =============================================
-- Author:			Vincent YUEN
-- Create date:	18 Nov 2009
-- Description:	Retrieve the Category information
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ClaimCategory_get_all_cache] 
AS
BEGIN

	SET NOCOUNT ON;
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
		[Category_Code],
		[Category_Name],
		[Category_Name_Chi], 
		[Category_Name_CN], 
		[Display_Seq]
	FROM 
		[ClaimCategory]
	ORDER BY 
		[Display_Seq] ASC

END
GO

GRANT EXECUTE ON [dbo].[proc_ClaimCategory_get_all_cache] TO HCPUBLIC, HCSP, HCVU, WSINT
GO

