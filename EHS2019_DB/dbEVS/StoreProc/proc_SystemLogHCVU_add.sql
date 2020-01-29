IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemLogHCVU_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemLogHCVU_add]
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
-- Modified date:	05-10-2009
-- Description:		Log Browser + OS
-- =============================================

CREATE procedure [dbo].[proc_SystemLogHCVU_add]
@function_code			char(6)
, @severity_code		char(1)
, @message_code			char(5)
, @client_ip			varchar(20)
, @user_id				varchar(20)
, @url					varchar(255)
, @system_message		text
, @session_id			varchar(40)
,@browser	varchar(20)
,@os		varchar(20)
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
	INSERT INTO [SystemLogHCVU08]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '09'
BEGIN
	INSERT INTO [SystemLogHCVU09]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '10'
BEGIN
	INSERT INTO [SystemLogHCVU10]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '11'
BEGIN
	INSERT INTO [SystemLogHCVU11]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '12'
BEGIN
	INSERT INTO [SystemLogHCVU12]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '13'
BEGIN
	INSERT INTO [SystemLogHCVU13]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '14'
BEGIN
	INSERT INTO [SystemLogHCVU14]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '15'
BEGIN
	INSERT INTO [SystemLogHCVU15]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '16'
BEGIN
	INSERT INTO [SystemLogHCVU16]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '17'
BEGIN
	INSERT INTO [SystemLogHCVU17]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '18'
BEGIN
	INSERT INTO [SystemLogHCVU18]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '19'
BEGIN
	INSERT INTO [SystemLogHCVU19]
           ([System_Dtm]
   ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '20'
BEGIN
	INSERT INTO [SystemLogHCVU20]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '21'
BEGIN
	INSERT INTO [SystemLogHCVU21]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '22'
BEGIN
	INSERT INTO [SystemLogHCVU22]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '23'
BEGIN
	INSERT INTO [SystemLogHCVU23]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '24'
BEGIN
	INSERT INTO [SystemLogHCVU24]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '25'
BEGIN
	INSERT INTO [SystemLogHCVU25]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '26'
BEGIN
	INSERT INTO [SystemLogHCVU26]
           ([System_Dtm]
       ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '27'
BEGIN
	INSERT INTO [SystemLogHCVU27]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '28'
BEGIN
	INSERT INTO [SystemLogHCVU28]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[User_ID]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@user_id
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END


GO

GRANT EXECUTE ON [dbo].[proc_SystemLogHCVU_add] TO HCVU
GO
