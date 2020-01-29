IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EqvSubsidizePrevSeasonMap_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EqvSubsidizePrevSeasonMap_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	25 August 2016
-- CR No.:			CRE16-002
-- Description:		Stored procedure is not used anymore
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Author:		Koala Cheng
-- Create date: 13 Sep 2010
-- Description:	Retrieve all EqvSubsidizePrevSeasonMap
-- =============================================

/*
CREATE PROCEDURE [dbo].[proc_EqvSubsidizePrevSeasonMap_get_all_cache] 
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
	[Scheme_Code],
	[Scheme_Seq],
	[Subsidize_Item_Code],
	[Eqv_Scheme_Code],
	[Eqv_Scheme_Seq],
	[Eqv_Subsidize_Item_Code]
FROM
	[EqvSubsidizePrevSeasonMap]

END
GO

GRANT EXECUTE ON [dbo].[proc_EqvSubsidizePrevSeasonMap_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_EqvSubsidizePrevSeasonMap_get_all_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_EqvSubsidizePrevSeasonMap_get_all_cache] TO WSEXT
Go
*/
