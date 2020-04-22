IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemLogHCVR_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemLogHCVR_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Timothy LEUNG
-- Create date:		08-12-2008
-- Description:		Add SystemLogHCVR
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Dedrick Ng
-- Modified date:	05-10-2009
-- Description:		Add new fields Browser, OS
-- =============================================
CREATE procedure [dbo].[proc_SystemLogHCVR_add]
@function_code			char(6)
, @severity_code		char(1)
, @message_code			char(5)
, @client_ip			varchar(20)
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
	INSERT INTO [SystemLogVR08]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '09'
BEGIN
	INSERT INTO [SystemLogVR09]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '10'
BEGIN
	INSERT INTO [SystemLogVR10]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '11'
BEGIN
	INSERT INTO [SystemLogVR11]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '12'
BEGIN
	INSERT INTO [SystemLogVR12]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '13'
BEGIN
	INSERT INTO [SystemLogVR13]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[URL]
 ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '14'
BEGIN
	INSERT INTO [SystemLogVR14]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '15'
BEGIN
	INSERT INTO [SystemLogVR15]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '16'
BEGIN
	INSERT INTO [SystemLogVR16]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '17'
BEGIN
	INSERT INTO [SystemLogVR17]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '18'
BEGIN
	INSERT INTO [SystemLogVR18]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '19'
BEGIN
	INSERT INTO [SystemLogVR19]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '20'
BEGIN
	INSERT INTO [SystemLogVR20]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '21'
BEGIN
	INSERT INTO [SystemLogVR21]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '22'
BEGIN
	INSERT INTO [SystemLogVR22]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '23'
BEGIN
	INSERT INTO [SystemLogVR23]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '24'
BEGIN
	INSERT INTO [SystemLogVR24]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '25'
BEGIN
	INSERT INTO [SystemLogVR25]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '26'
BEGIN
	INSERT INTO [SystemLogVR26]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '27'
BEGIN
	INSERT INTO [SystemLogVR27]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END
ELSE IF @year = '28'
BEGIN
	INSERT INTO [SystemLogVR28]
           ([System_Dtm]
           ,[Function_Code]
           ,[Severity_Code]
           ,[Message_Code]
           ,[Client_IP]
           ,[URL]
           ,[System_Message]
           ,[Session_ID] ,[Browser] ,[OS])
     VALUES
           (GetDate()
           ,@function_code
           ,@severity_code
           ,@message_code
           ,@client_ip
           ,@url
           ,@system_message
           ,@session_id ,@browser ,@os)
END


GO

GRANT EXECUTE ON [dbo].[proc_SystemLogHCVR_add] TO HCPUBLIC
GO
