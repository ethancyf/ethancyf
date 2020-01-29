IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_FileGeneration_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_FileGeneration_get_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Billy Lam
-- Create date:		05-05-2008
-- Description:		Get FileGeneration for caching
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE Procedure dbo.proc_FileGeneration_get_cache
as

select [File_ID]
, [File_Name]
from dbo.FileGeneration
order by [File_Name]

GO

GRANT EXECUTE ON [dbo].[proc_FileGeneration_get_cache] TO HCVU
GO
