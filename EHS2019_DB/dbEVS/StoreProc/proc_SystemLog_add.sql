IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemLog_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemLog_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[proc_SystemLog_add]
@function_code		char(6)
, @severity_code	char(1)
, @message_code		char(5)
, @client_ip			varchar(20)
, @user_id			varchar(20)
, @link				varchar(255)
, @system_message	text
as
INSERT INTO SystemLog
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[Link]
           ,[System_Message])
     VALUES
           (getdate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@link
           ,@system_message)
GO
