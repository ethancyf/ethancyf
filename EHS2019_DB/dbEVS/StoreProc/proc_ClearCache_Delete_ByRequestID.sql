IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ClearCache_Delete_ByRequestID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ClearCache_Delete_ByRequestID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		1 December 2010
-- Description:		Delete [ClearCache]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ClearCache_Delete_ByRequestID]
	@Request_ID		varchar(8)
AS BEGIN

	SET NOCOUNT ON;


-- =============================================
-- Execute
-- =============================================
	DELETE FROM
		ClearCache
	WHERE
		Request_ID = @Request_ID
	

END
GO

GRANT EXECUTE ON [dbo].[proc_ClearCache_Delete_ByRequestID] TO HCVU
GO
