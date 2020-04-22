IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MenuGroup_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MenuGroup_get_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Billy Lam
-- Create date:		05-05-2008
-- Description:		Get MenuGroup for caching
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE procedure [dbo].[proc_MenuGroup_get_cache]
as

-- =============================================
-- Return results
-- =============================================
SELECT Group_Name
      ,Menu_Group_Desc
      ,Display_Seq
      ,Effective_Date
      ,Description
      ,Chinese_Description
  FROM dbo.MenuGroup
where Record_Status = 'A'
order by Display_Seq

GO

GRANT EXECUTE ON [dbo].[proc_MenuGroup_get_cache] TO HCVU
GO
