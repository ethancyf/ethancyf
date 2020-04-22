IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MenuItem_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MenuItem_get_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Modification History
-- Modified by:		Winnie SUEN
-- CR No.			CRE19-026 (HCVS hotline service) 
-- Modified date:	03 Feb 2020
-- Description:		Add Column [Available_HCVU_SubPlatform]
-- ============================================= 
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	20 May 2011
-- CR No.:			CRE11-007
-- Description:		Change to new table structure:
--						Drop [Parent_Item]
--						Add [Item_ID], [Parent_Item_ID]
-- =============================================
-- =============================================
-- Author:			Billy Lam
-- Create date:		05-05-2008
-- Description:		Get MenuItem for caching
-- =============================================

CREATE procedure [dbo].[proc_MenuItem_get_cache]
AS BEGIN

-- =============================================
-- Return results
-- =============================================
	SELECT 
		Function_Code,
		Resource_Key,
		Item_Name,
		Group_Name,
		Display_Seq,
		URL,
		Record_Status,
		Effective_Date,
		Description,
		Chinese_Description,
		Item_ID,
		Parent_Item_ID,
		Available_HCVU_SubPlatform
	FROM
		MenuItem
	WHERE
		Record_Status = 'A'

END
GO

GRANT EXECUTE ON [dbo].[proc_MenuItem_get_cache] TO HCVU
GO
