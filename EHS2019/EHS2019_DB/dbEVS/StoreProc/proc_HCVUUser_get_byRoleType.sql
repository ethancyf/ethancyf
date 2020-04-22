 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCVUUser_get_byRoleType]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCVUUser_get_byRoleType]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Paul YIP
-- Create date: 13 Oct 2010
-- Description:	Retrieve accessable user by role type
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_HCVUUser_get_byRoleType]
	@intRoleType as integer
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

 SELECT distinct [User_ID]
 FROM [UserRole]  
 where[Role_Type] = @intRoleType

END
GO

GRANT EXECUTE ON [dbo].[proc_HCVUUser_get_byRoleType] TO HCVU
GO
