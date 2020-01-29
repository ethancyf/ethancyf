 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemLogSSO_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemLogSSO_add]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			    
-- Modified by:		
-- Modified date:	    
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT12-0008
-- Modified by:	Tommy TSE
-- Modified date:	29 Aug 2012
-- Description:	Rectify stored procedure permission
-- =============================================
-- =============================================
-- Author:			Paul Yip
-- Create date:	28-06-2010
-- Description:	Add SystemLogSSO
-- =============================================

CREATE procedure [dbo].[proc_SystemLogSSO_add]
  @client_ip			varchar(20)
, @user_id				varchar(20)
, @system_message		text
, @session_id			varchar(40)
, @browser				varchar(20)
, @os					varchar(20)
as

-- =============================================
-- Declaration
-- =============================================
DECLARE @year	varchar(2)
declare	@application_server	varchar(20)
-- =============================================
-- Initialization
-- =============================================
select @year = convert(varchar(2), getdate(), 12)
select @application_server = host_name()
-- =============================================
-- Insert Transcation
-- =============================================
IF @year = '08'
BEGIN
	INSERT INTO [SystemLogSSO08]
           ([System_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS]
           ,[Application_Server])
     VALUES
           (GetDate()
           ,@client_ip
           ,@user_id
           ,@system_message
           ,@session_id
           ,@browser
           ,@os
           ,@application_server)
END
ELSE IF @year = '09'
BEGIN
	INSERT INTO [SystemLogSSO09]
           ([System_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS]
           ,[Application_Server])
     VALUES
           (GetDate()
           ,@client_ip
           ,@user_id
           ,@system_message
           ,@session_id
           ,@browser
           ,@os
           ,@application_server)
END
ELSE IF @year = '10'
BEGIN
	INSERT INTO [SystemLogSSO10]
           ([System_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS]
           ,[Application_Server])
     VALUES
           (GetDate()
           ,@client_ip
           ,@user_id
           ,@system_message
           ,@session_id
           ,@browser
           ,@os
           ,@application_server)
END
ELSE IF @year = '11'
BEGIN
	INSERT INTO [SystemLogSSO11]
           ([System_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS]
           ,[Application_Server])
     VALUES
           (GetDate()
           ,@client_ip
           ,@user_id
           ,@system_message
           ,@session_id
           ,@browser
           ,@os
           ,@application_server)
END
ELSE IF @year = '12'
BEGIN
	INSERT INTO [SystemLogSSO12]
           ([System_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS]
           ,[Application_Server])
     VALUES
           (GetDate()
           ,@client_ip
           ,@user_id
           ,@system_message
           ,@session_id
           ,@browser
           ,@os
           ,@application_server)
END
ELSE IF @year = '13'
BEGIN
	INSERT INTO [SystemLogSSO13]
           ([System_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS]
           ,[Application_Server])
     VALUES
           (GetDate()
           ,@client_ip
           ,@user_id
           ,@system_message
           ,@session_id
           ,@browser
           ,@os
           ,@application_server)
END
ELSE IF @year = '14'
BEGIN
	INSERT INTO [SystemLogSSO14]
           ([System_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS]
           ,[Application_Server])
     VALUES
           (GetDate()
           ,@client_ip
           ,@user_id
           ,@system_message
           ,@session_id
           ,@browser
           ,@os
           ,@application_server)
END
ELSE IF @year = '15'
BEGIN
	INSERT INTO [SystemLogSSO15]
           ([System_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS]
           ,[Application_Server])
     VALUES
           (GetDate()
           ,@client_ip
           ,@user_id
           ,@system_message
           ,@session_id
           ,@browser
           ,@os
           ,@application_server)
END
ELSE IF @year = '16'
BEGIN
	INSERT INTO [SystemLogSSO16]
           ([System_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS]
           ,[Application_Server])
     VALUES
           (GetDate()
           ,@client_ip
           ,@user_id
           ,@system_message
           ,@session_id
           ,@browser
           ,@os
           ,@application_server)
END
ELSE IF @year = '17'
BEGIN
	INSERT INTO [SystemLogSSO17]
           ([System_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS]
           ,[Application_Server])
     VALUES
           (GetDate()
           ,@client_ip
           ,@user_id
           ,@system_message
           ,@session_id
           ,@browser
           ,@os
           ,@application_server)
END
ELSE IF @year = '18'
BEGIN
	INSERT INTO [SystemLogSSO18]
           ([System_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS]
           ,[Application_Server])
     VALUES
           (GetDate()
           ,@client_ip
           ,@user_id
           ,@system_message
           ,@session_id
           ,@browser
           ,@os
           ,@application_server)
END
ELSE IF @year = '19'
BEGIN
	INSERT INTO [SystemLogSSO19]
           ([System_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS]
           ,[Application_Server])
     VALUES
           (GetDate()
           ,@client_ip
           ,@user_id
           ,@system_message
           ,@session_id
           ,@browser
           ,@os
           ,@application_server)
END
ELSE IF @year = '20'
BEGIN
	INSERT INTO [SystemLogSSO20]
           ([System_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS]
           ,[Application_Server])
     VALUES
           (GetDate()
           ,@client_ip
           ,@user_id
           ,@system_message
           ,@session_id
           ,@browser
           ,@os
           ,@application_server)
END
ELSE IF @year = '21'
BEGIN
	INSERT INTO [SystemLogSSO21]
            ([System_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS]
           ,[Application_Server])
     VALUES
           (GetDate()
           ,@client_ip
           ,@user_id
           ,@system_message
           ,@session_id
           ,@browser
           ,@os
           ,@application_server)
END
ELSE IF @year = '22'
BEGIN
	INSERT INTO [SystemLogSSO22]
           ([System_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS]
           ,[Application_Server])
     VALUES
           (GetDate()
           ,@client_ip
           ,@user_id
           ,@system_message
           ,@session_id
           ,@browser
           ,@os
           ,@application_server)
END
ELSE IF @year = '23'
BEGIN
	INSERT INTO [SystemLogSSO23]
           ([System_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS]
           ,[Application_Server])
     VALUES
           (GetDate()
           ,@client_ip
           ,@user_id
           ,@system_message
           ,@session_id
           ,@browser
           ,@os
           ,@application_server)
END
ELSE IF @year = '24'
BEGIN
	INSERT INTO [SystemLogSSO24]
           ([System_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS]
           ,[Application_Server])
     VALUES
           (GetDate()
           ,@client_ip
           ,@user_id
           ,@system_message
           ,@session_id
           ,@browser
           ,@os
           ,@application_server)
END
ELSE IF @year = '25'
BEGIN
	INSERT INTO [SystemLogSSO25]
           ([System_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS]
           ,[Application_Server])
     VALUES
           (GetDate()
           ,@client_ip
           ,@user_id
           ,@system_message
           ,@session_id
           ,@browser
           ,@os
           ,@application_server)
END
ELSE IF @year = '26'
BEGIN
	INSERT INTO [SystemLogSSO26]
           ([System_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS]
           ,[Application_Server])
     VALUES
           (GetDate()
           ,@client_ip
           ,@user_id
           ,@system_message
           ,@session_id
           ,@browser
           ,@os
           ,@application_server)
END
ELSE IF @year = '27'
BEGIN
	INSERT INTO [SystemLogSSO27]
           ([System_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS]
           ,[Application_Server])
     VALUES
           (GetDate()
           ,@client_ip
           ,@user_id
           ,@system_message
           ,@session_id
           ,@browser
           ,@os
           ,@application_server)
END
ELSE IF @year = '28'
BEGIN
	INSERT INTO [SystemLogSSO28]
           ([System_Dtm]
           ,[Client_IP]
           ,[User_ID]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS]
           ,[Application_Server])
     VALUES
           (GetDate()
           ,@client_ip
           ,@user_id
           ,@system_message
           ,@session_id
           ,@browser
           ,@os
           ,@application_server)

END 

GO

GRANT EXECUTE ON [dbo].[proc_SystemLogSSO_add] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SystemLogSSO_add] TO HCSP
GO

