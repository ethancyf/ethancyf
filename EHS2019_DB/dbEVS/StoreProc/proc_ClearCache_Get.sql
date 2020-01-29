IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ClearCache_Get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ClearCache_Get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		1 December 2010
-- Description:		Retrieve [ClearCache]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ClearCache_Get]
	@Application_Server		varchar(20)
AS BEGIN

	SET NOCOUNT ON;


-- =============================================
-- Execute
-- =============================================
	SELECT
		Request_ID,
		Application_Server,
		Cache_File,
		Record_Status,
		Message,
		Request_Dtm
	FROM
		ClearCache
	WHERE
		@Application_Server IS NULL OR Application_Server = @Application_Server
	ORDER BY
		Request_Dtm DESC
	

END
GO

GRANT EXECUTE ON [dbo].[proc_ClearCache_Get] TO HCVU
GO
