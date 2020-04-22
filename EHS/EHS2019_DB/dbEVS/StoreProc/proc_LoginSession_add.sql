IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_LoginSession_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_LoginSession_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date:	11 May 2010
-- Description:	Add the Login Session of client browser
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_LoginSession_add] 
	@Session_ID varchar(40),
	@User_ID char(20),
	@DataEntry varchar(20)
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
	
	exec proc_LoginSession_check @Session_ID	

	INSERT INTO [LoginSession] ([Session_ID], [User_ID], [DataEntry], [Login_Dtm])
	VALUES (@Session_ID, @User_ID, @DataEntry, GetDate())
	
END
GO

GRANT EXECUTE ON [dbo].[proc_LoginSession_add] TO HCSP
GO
