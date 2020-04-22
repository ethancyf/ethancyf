IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeEnrolClaimMap_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeEnrolClaimMap_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Derek LEUNG
-- Modified date:	06 Aug 2010
-- Description:		Grant Permission to HCVU
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 06 Aug 2009
-- Description:	Retrieve the SchemeEnrol <-> SchemeClaim Mapping
-- =============================================



CREATE PROCEDURE [dbo].[proc_SchemeEnrolClaimMap_get_all_cache] 
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
	[Scheme_Code_Enrol],
	[Scheme_Code_Claim]
FROM
	[SchemeEnrolClaimMap]
WHERE
	[Record_Status] = 'A'

END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeEnrolClaimMap_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SchemeEnrolClaimMap_get_all_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SchemeEnrolClaimMap_get_all_cache] TO WSEXT
Go