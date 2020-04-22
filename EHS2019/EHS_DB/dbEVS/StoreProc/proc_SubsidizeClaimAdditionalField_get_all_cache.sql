IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SubsidizeClaimAdditionalField_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SubsidizeClaimAdditionalField_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 21 Aug 2009
-- Description:	Retrieve all SubsidizeClaimAdditionalField 
--				(Indicate Addition Field to be capture)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_SubsidizeClaimAdditionalField_get_all_cache] 
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
	[Display_Seq],
	[AdditionalFieldID],
	[AdditionalFieldType],
	[Display_Name],
	[Display_Name_Chi],
	[List_Column],
	[Mandatory]
FROM
	[SubsidizeClaimAdditionalField]
ORDER BY [Display_Seq] ASC

END
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeClaimAdditionalField_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeClaimAdditionalField_get_all_cache] TO HCVU
GO
