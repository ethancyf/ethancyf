IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ICWAuditLog_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ICWAuditLog_Add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		1 December 2010
-- Description:		Insert into [ClearCache]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ICWAuditLog_Add]
	@Staff_ID			char(8),
	@Function_Code		char(6),
	@Log_ID				char(5),
	@Description		varchar(255)
AS BEGIN

	SET NOCOUNT ON;


-- =============================================
-- Execute
-- =============================================
	INSERT INTO ICWAuditLog (
		System_Dtm,
		Staff_ID,
		Function_Code,
		Log_ID,
		Description
	) VALUES (
		GETDATE(),
		@Staff_ID,
		@Function_Code,
		@Log_ID,
		@Description
	)
	

END
GO

GRANT EXECUTE ON [dbo].[proc_ICWAuditLog_Add] TO HCVU
GO
