IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCVUUser_get_For4LevelAlertInbox]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCVUUser_get_For4LevelAlertInbox]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Clark YIP
-- Create date: 21 Oct 2008
-- Description:	Retrieve accessable user on the ultimate alert in 4 Level alert
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_HCVUUser_get_For4LevelAlertInbox]
	@function_code as char(6)		
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

select distinct ur.[User_id] from RoleSecurity rs, roleType rt, UserRole ur
where rs.function_code=@function_code
and rs.role_type=rt.role_type and rt.role_type=ur.role_type
and record_status='A'	

END
GO

GRANT EXECUTE ON [dbo].[proc_HCVUUser_get_For4LevelAlertInbox] TO HCVU
GO
