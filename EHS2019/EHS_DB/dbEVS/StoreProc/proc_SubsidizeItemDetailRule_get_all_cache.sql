IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SubsidizeItemDetailRule_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SubsidizeItemDetailRule_get_all_cache]
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
-- Modified by:		Kathy LEE
-- Modified date:	7 Jul 2010
-- Description:		Grant Right to HCVU
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 19 Sep 2009
-- Description:	Retrieve all SubsidizeItemDetailRule: Dose -> Rule
-- =============================================

CREATE PROCEDURE [dbo].[proc_SubsidizeItemDetailRule_get_all_cache] 
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
		[Subsidize_Item_Code],
		[Available_Item_Code],
		[Rule_Group],
		[Rule_Name],
		[Type],
		[Dependence],
		[Operator],
		[Compare_Value],
		[Compare_Unit],
		[Check_From],
		[Check_To],
		[Checking_Method],
		[Handling_Method]
	FROM
		[SubsidizeItemDetailRule]

END
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeItemDetailRule_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeItemDetailRule_get_all_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeItemDetailRule_get_all_cache] TO WSEXT
Go