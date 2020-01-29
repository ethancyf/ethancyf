IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_CrossSubsidizeRelation_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_CrossSubsidizeRelation_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Karl LAM
-- Create date: 08 Apr 2015
-- Description:	Retrieve all CrossSubsidizeRelation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date:	
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_CrossSubsidizeRelation_get_all_cache] 
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
	[Relate_Scheme_Code],
	[Relate_Scheme_Seq],
	[Relate_Subsidize_Code]	
FROM
	[CrossSubsidizeRelation]

END
GO

GRANT EXECUTE ON [dbo].[proc_CrossSubsidizeRelation_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_CrossSubsidizeRelation_get_all_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_CrossSubsidizeRelation_get_all_cache] TO WSEXT
Go

