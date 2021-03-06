IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionDHC_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionDHC_get_all_cache]
GO

SET ANSI_NULLS ON
GO
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
-- CR No.:			CRE19-006 (DHC)
-- Modified by:		Winnie SUEN
-- Modified date:	24 Jun 2019
-- Description:		Retrieve all max claim amount in DHC Service for profession
-- =============================================

CREATE PROCEDURE [dbo].[proc_ProfessionDHC_get_all_cache] 
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
		[Service_Category_Code],
		[Claim_Amt_Max]
	FROM
		[ProfessionDHC] WITH (NOLOCK)
	ORDER BY [Service_Category_Code]
END
GO

GRANT EXECUTE ON [dbo].[proc_ProfessionDHC_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ProfessionDHC_get_all_cache] TO HCVU
GO
