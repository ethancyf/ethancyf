 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ImmdOutstandingInboxMessage_del_bySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ImmdOutstandingInboxMessage_del_bySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Paul Yip
-- Create date:	21 July 2010
-- Description:	Delete ImmD Outstanding Inbox Message by SPID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ImmdOutstandingInboxMessage_del_bySPID] 
	@SP_ID as char(8)
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
	
	DELETE FROM [ImmdOutstandingInboxMessage] WHERE [SP_ID] = @SP_ID
	
END
GO

GRANT EXECUTE ON [dbo].[proc_ImmdOutstandingInboxMessage_del_bySPID] TO HCVU
GO
