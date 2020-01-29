IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_LoginSession_check]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_LoginSession_check]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date:	11 May 2010
-- Description:	Check Login Session of client browser exist
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_LoginSession_check] 
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
	
	 IF (SELECT COUNT(1) FROM [LoginSession] WHERE [Session_ID] = @Session_ID) != 0
	 BEGIN
		RAISERROR('00015', 16, 1)  
		RETURN @@error  
	 END   	
END
GO

GRANT EXECUTE ON [dbo].[proc_LoginSession_check] TO HCSP
GO
