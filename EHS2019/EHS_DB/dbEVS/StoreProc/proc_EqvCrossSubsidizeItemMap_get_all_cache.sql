IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EqvCrossSubsidizeItemMap_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EqvCrossSubsidizeItemMap_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Karl LAM
-- Create date: 08 Apr 2015
-- Description:	Retrieve all EqvCrossSubsidizeItemMap
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date:	
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_EqvCrossSubsidizeItemMap_get_all_cache] 
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
	[Subsidize_Code],
	[Eqv_Scheme_Code],
	[Eqv_Scheme_Seq],
	[Eqv_Subsidize_Code]	
FROM
	[EqvCrossSubsidizeItemMap]

END
GO

GRANT EXECUTE ON [dbo].[proc_EqvCrossSubsidizeItemMap_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_EqvCrossSubsidizeItemMap_get_all_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_EqvCrossSubsidizeItemMap_get_all_cache] TO WSEXT
Go

