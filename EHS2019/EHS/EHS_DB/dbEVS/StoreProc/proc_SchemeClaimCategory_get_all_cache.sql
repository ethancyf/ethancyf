IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeClaimCategory_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeClaimCategory_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	9 December 2014
-- CR No.:			CRE13-019-01
-- Description:		Support Simplified Chinese
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 19 Sep 2009
-- Description:	Retrieve all Scheme Claim Category
-- =============================================

CREATE PROCEDURE [dbo].[proc_SchemeClaimCategory_get_all_cache] 
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
		SCC.[Scheme_Code],
		SCC.[Scheme_Seq],
		SCC.[Subsidize_Code],
		SCC.[Category_Code],
		SCC.[IsMedicalCondition],
		SCC.[Record_Status],
		CC.[Category_Name],	
		CC.[Category_Name_Chi],
		CC.[Category_Name_CN],
		CC.[Display_Seq]
		
	FROM
		[SchemeClaimCategory] SCC INNER JOIN [ClaimCategory] CC
			ON SCC.[Category_Code] = CC.[Category_Code]
	WHERE
		[Record_Status] = 'A'
		
	ORDER BY CC.[Display_Seq]

END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeClaimCategory_get_all_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SchemeClaimCategory_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SchemeClaimCategory_get_all_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SchemeClaimCategory_get_all_cache] TO WSEXT
GO