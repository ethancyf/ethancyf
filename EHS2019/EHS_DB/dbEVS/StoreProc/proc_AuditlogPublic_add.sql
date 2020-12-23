IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_AuditlogPublic_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_AuditlogPublic_add]
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
-- Modified by:		Koala CHENG
-- CR No.:			CRE11-XXX
-- Modified date:	09-10-2011
-- Description:		Add Language Log
-- =============================================
-- =============================================
-- Modified by:		Derek Leung
-- Modified date: 	25-01-2011
-- Description:		Insert log with view  CRE11-001
-- =============================================
-- =============================================
-- Author:			Koala Cheng
-- Create date:		12-01-2011
-- Description:		Add Extra detail columns (E_Doc_Code, E_Doc_No, E_SP_Doc_No)
-- =============================================
-- =============================================
-- Modification History	
-- Modified by:		Kathy LEE
-- Modified date: 	07 May 2010
-- Description:		Add Message Code in separated column
--					with decryption
-- =============================================
-- =============================================
-- Author:			Billy Lam
-- Create date:		06-05-2008
-- Description:		Add AuditlogPublic
-- =============================================
-- =============================================
-- Modification History	
-- Modified by:		Timothy LEUNG
-- Modified date: 		28-11-2008
-- Description:		Add Session ID in AuditLog
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Dedrick Ng
-- Modified date:	05-10-2009
-- Description:		Add new fields Browser, OS
-- =============================================

 CREATE procedure [dbo].[proc_AuditlogPublic_add] @action_time	datetime
,@end_time	datetime
,@action_key	varchar(20)
,@client_ip	varchar(20)
,@function_code	char(6)
,@log_id	varchar(10)
,@description	nvarchar(1000)
,@session_id varchar(40) 
,@browser	varchar(20)
,@os		varchar(20)
,@message_code varchar(525)
,@Doc_Code varchar(15)
,@Doc_No varchar(20)
,@SP_Doc_No varchar(20)
,@Language varchar(20) = NULL
as

declare @action_time_str	varchar(30)
declare @end_time_str		varchar(30)
declare @system_dtm_str			varchar(30)
declare	@application_server	varchar(20)
declare @year	varchar(2)
declare @System_Dtm datetime

select @System_Dtm = getdate()
select @action_time_str = convert(varchar(30), @action_time, 13)
select @end_time_str = convert(varchar(30), @end_time, 13)
select @system_dtm_str = convert(varchar(30), @System_Dtm, 13)
select @application_server = host_name()
select @year = convert(varchar(2), @system_dtm, 12)

declare @E_System_Dtm varbinary(60)
declare @E_Action_Dtm varbinary(60)
declare @E_End_Dtm varbinary(60)
declare @E_Action_Key varbinary(60)
declare @E_Client_IP varbinary(60)
declare @E_Function_Code varbinary(50)
declare @E_Log_ID varbinary(50)
declare @E_Description varbinary(2100)
declare @E_Application_Server varbinary(60)
declare @E_Session_ID varbinary(100)

declare @E_Browser varbinary(60)
declare @E_OS varbinary(60)

declare @E_Doc_Code varbinary(50)
declare @E_Doc_No varbinary(100)
declare @E_SP_Doc_No varbinary(100)
declare @E_Language varbinary(60)

EXEC [proc_SymmetricKey_open]

select @E_System_Dtm = EncryptByKey(KEY_GUID('sym_Key'), convert(varchar(23), @System_Dtm, 21))
select @E_Action_Dtm = EncryptByKey(KEY_GUID('sym_Key'), @action_time_str)
select @E_End_Dtm = EncryptByKey(KEY_GUID('sym_Key'), @end_time_str)
select @E_Action_Key = EncryptByKey(KEY_GUID('sym_Key'), @action_key)
select @E_Client_IP = EncryptByKey(KEY_GUID('sym_Key'), @client_ip)
select @E_Function_Code = EncryptByKey(KEY_GUID('sym_Key'), @function_code)
select @E_Log_ID = EncryptByKey(KEY_GUID('sym_Key'), @log_id)
select @E_Description = EncryptByKey(KEY_GUID('sym_Key'), @description)
select @E_Application_Server = EncryptByKey(KEY_GUID('sym_Key'), @application_server)
select @E_Session_ID = EncryptByKey(KEY_GUID('sym_Key'), @session_id)

select @E_Browser = EncryptByKey(KEY_GUID('sym_Key'), @browser)
select @E_OS = EncryptByKey(KEY_GUID('sym_Key'), @os)

select @E_Doc_Code = EncryptByKey(KEY_GUID('sym_Key'), @Doc_Code)
select @E_Doc_No = EncryptByKey(KEY_GUID('sym_Key'), @Doc_No)
select @E_SP_Doc_No = EncryptByKey(KEY_GUID('sym_Key'), @SP_Doc_No)
select @E_Language = EncryptByKey(KEY_GUID('sym_Key'), @Language)

EXEC [proc_SymmetricKey_close]

-- =============================================
-- Insert Transcation
-- =============================================

	INSERT INTO [ViewAuditlogPublic]
	 ( System_Dtm
	, E_System_Dtm
	, E_Action_Dtm
	, E_End_Dtm
	, E_Action_Key
	, E_Client_IP
	, E_Function_Code
	, E_Log_ID
	, E_Description
	, E_Application_Server
	, E_Session_ID
	, E_Browser, E_OS, Message_Code
	, E_Doc_Code, E_Doc_No, E_SP_Doc_No, E_Language)
	VALUES
	( @System_Dtm
	, @E_System_Dtm
	, @E_Action_Dtm
	, @E_End_Dtm
	, @E_Action_Key
	, @E_Client_IP
	, @E_Function_Code
	, @E_Log_ID
	, @E_Description
	, @E_Application_Server
	, @E_Session_ID
	, @E_Browser, @E_OS, @message_code
	, @E_Doc_Code, @E_Doc_No, @E_SP_Doc_No, @E_Language)

GO

GRANT EXECUTE ON [dbo].[proc_AuditlogPublic_add] TO HCPUBLIC
GO
