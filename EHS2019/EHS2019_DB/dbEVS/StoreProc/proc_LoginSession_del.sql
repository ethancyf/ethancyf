IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_LoginSession_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_LoginSession_del]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date:	11 May 2010
-- Description:	Delete Login Session when logout
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_LoginSession_del] 
	@Session_ID varchar(40)
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
	
	DELETE FROM [LoginSession] WHERE [Session_ID] = @Session_ID
	
END
GO

GRANT EXECUTE ON [dbo].[proc_LoginSession_del] TO HCSP
GO
