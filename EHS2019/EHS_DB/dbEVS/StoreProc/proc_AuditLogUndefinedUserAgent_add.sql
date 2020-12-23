IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_AuditLogUndefinedUserAgent_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_AuditLogUndefinedUserAgent_add]
GO

SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

GO

-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Created by:		Chris YIM
-- Created date:	24 Oct 2016
-- CR No.			I-CRE16-006
-- Description:		Capture detail client browser and OS information
-- =============================================

CREATE procedure [dbo].[proc_AuditLogUndefinedUserAgent_add]
	@session_id	varchar(40),
	@user_agent	varchar(500),
	@platform	varchar(2)
as

-- =============================================
-- Declaration
-- =============================================

DECLARE @System_Dtm datetime
DECLARE @E_Session_ID varbinary(100)
DECLARE @IN_User_Agent varchar(510)
DECLARE @IN_Platform varchar(20)

-- =============================================
-- Initialization
-- =============================================

SET @System_Dtm = getdate()

EXEC [proc_SymmetricKey_open]

SET @E_Session_ID = EncryptByKey(KEY_GUID('sym_Key'), @session_id)

EXEC [proc_SymmetricKey_close]

SET @IN_User_Agent = @user_agent
SET @IN_Platform = @platform

-- =============================================
-- Insert transaction
-- =============================================

INSERT INTO [dbo].[AuditLogUndefinedUserAgent](
	[System_Dtm], 
	[E_Session_ID], 
	[User_Agent],
	[Platform]
	)
VALUES( 
	@System_Dtm,
	@E_Session_ID, 
	@IN_User_Agent, 
	@platform
	)

GO

GRANT EXECUTE ON [dbo].[proc_AuditLogUndefinedUserAgent_add] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_AuditLogUndefinedUserAgent_add] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_AuditLogUndefinedUserAgent_add] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_AuditLogUndefinedUserAgent_add] TO WSINT
GO

