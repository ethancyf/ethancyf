IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemLogInterface_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemLogInterface_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE17-010 (OCSSS integration)
-- Modified by:	    Winnie SUEN
-- Modified date:   30 May 2018
-- Description:		Add HCSP, HCVU access right 
-- =============================================
-- =============================================
-- Author:			Koala Cheng
-- Create date:		12-10-2010
-- Description:		Add SystemLogInterface
-- =============================================

CREATE procedure [dbo].[proc_SystemLogInterface_add]
@function_code			char(6)
, @severity_code		char(1)
, @message_code			char(5)
, @client_ip			varchar(20)
, @user_id				varchar(20)
, @data_entry_account	varchar(20)
, @url					varchar(255)
, @system_message		text
, @session_id			varchar(40)
, @browser				varchar(20)
, @os					varchar(20)
as

-- =============================================
-- Declaration
-- =============================================
DECLARE @year	varchar(2)

-- =============================================
-- Initialization
-- =============================================
select @year = convert(varchar(2), getdate(), 12)

-- =============================================
-- Insert Transcation
-- =============================================
IF @year = '10'
BEGIN
	INSERT INTO [SystemLogInterface10]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[DataEntryAccount]
           ,[URL]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@data_entry_account
           ,@url
           ,@system_message
           ,@session_id
           ,@browser
           ,@os)
END
ELSE IF @year = '11'
BEGIN
	INSERT INTO [SystemLogInterface11]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[DataEntryAccount]
           ,[URL]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@data_entry_account
           ,@url
           ,@system_message
           ,@session_id
           ,@browser
           ,@os)
END
ELSE IF @year = '12'
BEGIN
	INSERT INTO [SystemLogInterface12]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[DataEntryAccount]
           ,[URL]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@data_entry_account
           ,@url
           ,@system_message
           ,@session_id
           ,@browser
           ,@os)
END
ELSE IF @year = '13'
BEGIN
	INSERT INTO [SystemLogInterface13]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[DataEntryAccount]
           ,[URL]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@data_entry_account
           ,@url
           ,@system_message
           ,@session_id
           ,@browser
           ,@os)
END
ELSE IF @year = '14'
BEGIN
	INSERT INTO [SystemLogInterface14]
      ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[DataEntryAccount]
           ,[URL]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@data_entry_account
           ,@url
           ,@system_message
           ,@session_id
           ,@browser
           ,@os)
END
ELSE IF @year = '15'
BEGIN
	INSERT INTO [SystemLogInterface15]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[DataEntryAccount]
           ,[URL]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@data_entry_account
           ,@url
           ,@system_message
           ,@session_id
           ,@browser
           ,@os)
END
ELSE IF @year = '16'
BEGIN
	INSERT INTO [SystemLogInterface16]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[DataEntryAccount]
           ,[URL]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@data_entry_account
           ,@url
           ,@system_message
           ,@session_id
           ,@browser
           ,@os)
END
ELSE IF @year = '17'
BEGIN
	INSERT INTO [SystemLogInterface17]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[DataEntryAccount]
           ,[URL]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@data_entry_account
           ,@url
           ,@system_message
           ,@session_id
           ,@browser
           ,@os)
END
ELSE IF @year = '18'
BEGIN
	INSERT INTO [SystemLogInterface18]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[DataEntryAccount]
           ,[URL]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@data_entry_account
           ,@url
           ,@system_message
           ,@session_id
           ,@browser
           ,@os)
END
ELSE IF @year = '19'
BEGIN
	INSERT INTO [SystemLogInterface19]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[DataEntryAccount]
           ,[URL]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@data_entry_account
           ,@url
           ,@system_message
          ,@session_id
           ,@browser
           ,@os)
END
ELSE IF @year = '20'
BEGIN
	INSERT INTO [SystemLogInterface20]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[DataEntryAccount]
           ,[URL]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@data_entry_account
           ,@url
           ,@system_message
           ,@session_id
           ,@browser
           ,@os)
END
ELSE IF @year = '21'
BEGIN
	INSERT INTO [SystemLogInterface21]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[DataEntryAccount]
           ,[URL]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@data_entry_account
           ,@url
           ,@system_message
           ,@session_id
           ,@browser
           ,@os)
END
ELSE IF @year = '22'
BEGIN
	INSERT INTO [SystemLogInterface22]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[DataEntryAccount]
           ,[URL]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@data_entry_account
           ,@url
           ,@system_message
           ,@session_id
           ,@browser
           ,@os)
END
ELSE IF @year = '23'
BEGIN
	INSERT INTO [SystemLogInterface23]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[DataEntryAccount]
           ,[URL]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@data_entry_account
           ,@url
           ,@system_message
           ,@session_id
           ,@browser
           ,@os)
END
ELSE IF @year = '24'
BEGIN
	INSERT INTO [SystemLogInterface24]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[DataEntryAccount]
           ,[URL]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@data_entry_account
           ,@url
           ,@system_message
           ,@session_id
           ,@browser
           ,@os)
END
ELSE IF @year = '25'
BEGIN
	INSERT INTO [SystemLogInterface25]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[DataEntryAccount]
           ,[URL]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@data_entry_account
           ,@url
           ,@system_message
           ,@session_id
           ,@browser
           ,@os)
END
ELSE IF @year = '26'
BEGIN
	INSERT INTO [SystemLogInterface26]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[DataEntryAccount]
           ,[URL]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@data_entry_account
           ,@url
           ,@system_message
           ,@session_id
           ,@browser
           ,@os)
END
ELSE IF @year = '27'
BEGIN
	INSERT INTO [SystemLogInterface27]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[DataEntryAccount]
           ,[URL]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@data_entry_account
           ,@url
           ,@system_message
           ,@session_id
           ,@browser
           ,@os)
END
ELSE IF @year = '28'
BEGIN
	INSERT INTO [SystemLogInterface28]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[DataEntryAccount]
           ,[URL]
           ,[System_Message]
           ,[Session_ID]
           ,[Browser]
           ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@data_entry_account
           ,@url
           ,@system_message
           ,@session_id
           ,@browser
           ,@os)
END
GO

GRANT EXECUTE ON [dbo].[proc_SystemLogInterface_add] TO WSINT, WSEXT, HCSP, HCVU

GO