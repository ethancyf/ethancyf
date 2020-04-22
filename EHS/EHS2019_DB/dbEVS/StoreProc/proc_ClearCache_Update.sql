IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ClearCache_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ClearCache_Update]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		1 December 2010
-- Description:		Update [ClearCache]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ClearCache_Update]
	@Request_ID		varchar(8),
	@Record_Status	char(1),
	@Message		varchar(255)
AS BEGIN

	SET NOCOUNT ON;


-- =============================================
-- Execute
-- =============================================
	UPDATE
		ClearCache
	SET
		Record_Status = @Record_Status,
		Message = @Message
	WHERE
		Request_ID = @Request_ID
	

END
GO

GRANT EXECUTE ON [dbo].[proc_ClearCache_Update] TO HCVU
GO
