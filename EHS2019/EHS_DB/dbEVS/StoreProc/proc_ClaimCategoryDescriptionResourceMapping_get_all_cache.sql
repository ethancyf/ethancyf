IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ClaimCategoryDescriptionResourceMapping_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ClaimCategoryDescriptionResourceMapping_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Vincent YUEN
-- Create date: 7 Jan 2010
-- Description:	Retrieve all ClaimCategoryDescriptionResourceMapping
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ClaimCategoryDescriptionResourceMapping_get_all_cache]
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
		  [Scheme_Code]
		  ,[Scheme_Seq]
		  ,[Category_Code]
		  ,[Rule_Group]
		  ,[Operator]
		  ,[Compare_Value]
		  ,[Compare_Unit]
		  ,[Checking_Method]
		  ,[Resource_Type]
		  ,[Resource_Name]
		  ,[Is_Adult]
		  ,[Record_Status]
	FROM 
		[ClaimCategoryDescriptionResourceMapping]
	ORDER BY
		[Scheme_Code] ASC ,[Scheme_Seq] ASC ,[Category_Code] ASC ,[Rule_Group] ASC

END
GO

GRANT EXECUTE ON [dbo].[proc_ClaimCategoryDescriptionResourceMapping_get_all_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_ClaimCategoryDescriptionResourceMapping_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ClaimCategoryDescriptionResourceMapping_get_all_cache] TO HCVU
GO
