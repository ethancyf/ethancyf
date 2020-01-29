IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ViewEqvSubsidizeMap_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ViewEqvSubsidizeMap_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		20 September 2016
-- CR No.:			CRE16-002
-- Description:		Retrieve all ViewEqvSubsidizeMap
-- =============================================

CREATE PROCEDURE [dbo].[proc_ViewEqvSubsidizeMap_get_all_cache] 
AS BEGIN

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
	Scheme_Code,
	Scheme_Seq,
	Subsidize_Item_Code,
	Eqv_Scheme_Code,
	Eqv_Scheme_Seq,
	Eqv_Subsidize_Item_Code
FROM
	ViewEqvSubsidizeMap

END
GO

GRANT EXECUTE ON [dbo].[proc_ViewEqvSubsidizeMap_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ViewEqvSubsidizeMap_get_all_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_ViewEqvSubsidizeMap_get_all_cache] TO WSEXT
Go