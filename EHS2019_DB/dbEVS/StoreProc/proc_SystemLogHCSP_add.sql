IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemLogHCSP_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemLogHCSP_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Billy Lam
-- Create date:		06-05-2008
-- Description:		Add SystemLogHCSP
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Timothy LEUNG
-- Modified date:	28-11-2008
-- Description:		Add Session ID 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	08-12-2008
-- Description:		Handle change to year 2009 - 2028
-- =============================================
-- =============================================
-- Modification History	
-- Modified by:		Pak Ho LEE
-- Modified date: 	06-05-2009
-- Description:		Add Browser Version & OS in System Log
-- =============================================
CREATE procedure [dbo].[proc_SystemLogHCSP_add]
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
IF @year = '08'
BEGIN
	INSERT INTO [SystemLogHCSP08]
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
ELSE IF @year = '09'
BEGIN
	INSERT INTO [SystemLogHCSP09]
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
ELSE IF @year = '10'
BEGIN
	INSERT INTO [SystemLogHCSP10]
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
	INSERT INTO [SystemLogHCSP11]
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
	INSERT INTO [SystemLogHCSP12]
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
	INSERT INTO [SystemLogHCSP13]
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
	INSERT INTO [SystemLogHCSP14]
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
	INSERT INTO [SystemLogHCSP15]
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
	INSERT INTO [SystemLogHCSP16]
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
	INSERT INTO [SystemLogHCSP17]
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
	INSERT INTO [SystemLogHCSP18]
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
	INSERT INTO [SystemLogHCSP19]
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
	INSERT INTO [SystemLogHCSP20]
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
	INSERT INTO [SystemLogHCSP21]
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
	INSERT INTO [SystemLogHCSP22]
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
	INSERT INTO [SystemLogHCSP23]
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
	INSERT INTO [SystemLogHCSP24]
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
	INSERT INTO [SystemLogHCSP25]
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
	INSERT INTO [SystemLogHCSP26]
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
	INSERT INTO [SystemLogHCSP27]
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
	INSERT INTO [SystemLogHCSP28]
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

GRANT EXECUTE ON [dbo].[proc_SystemLogHCSP_add] TO HCSP
GO
