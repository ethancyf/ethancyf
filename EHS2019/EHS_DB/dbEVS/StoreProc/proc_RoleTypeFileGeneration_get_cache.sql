IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_RoleTypeFileGeneration_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_RoleTypeFileGeneration_get_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Billy Lam
-- Create date:		05-05-2008
-- Description:		Get RoleTypeFileGeneration for caching
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE Procedure dbo.proc_RoleTypeFileGeneration_get_cache
as

-- =============================================
-- Return results
-- =============================================
SELECT Role_Type
      , File_ID
FROM dbo.RoleTypeFileGeneration
order by Role_Type

GO

GRANT EXECUTE ON [dbo].[proc_RoleTypeFileGeneration_get_cache] TO HCVU
GO
