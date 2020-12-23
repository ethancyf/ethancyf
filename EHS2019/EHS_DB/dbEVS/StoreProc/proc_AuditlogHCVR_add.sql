IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_AuditlogHCVR_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_AuditlogHCVR_add]
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
-- Modified by:		Derek Leung
-- Modified date: 	25-01-2011
-- Description:		Insert log with view  CRE11-001
-- =============================================
-- =============================================
-- Author:			Koala Cheng
-- Create date:		18-01-2011
-- Description:		Add Extra detail columns (E_Acc_Type, E_Acc_ID, E_Doc_Code, E_Doc_No)
-- =============================================
-- =============================================
-- Modification History	
-- Modified by:		Kathy LEE
-- Modified date: 	07 May 2010
-- Description:		Add Message Code in separated column
--					with decryption
-- =============================================
-- =============================================
-- Author:			Timothy LEUNG
-- Create date:		08-12-2008
-- Description:		Add AuditlogHCVR
-- =============================================
-- =============================================
-- Modification History	
-- Modified by:		Dedrick Ng
-- Modified date:	05-10-2009
-- Description:		Add new fields Browser, OS
-- =============================================

 CREATE procedure [dbo].[proc_AuditlogHCVR_add] @action_time	datetime
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
,@Acc_Type varchar(1)
,@Acc_ID varchar(15)
,@Doc_Code varchar(15)
,@Doc_No varchar(20)
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

declare @E_Acc_Type varbinary(50)
declare @E_Acc_ID varbinary(50)
declare @E_Doc_Code varbinary(50)
declare @E_Doc_No varbinary(100)

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

select @E_Acc_Type = EncryptByKey(KEY_GUID('sym_Key'), @Acc_Type)
select @E_Acc_ID = EncryptByKey(KEY_GUID('sym_Key'), @Acc_ID)
select @E_Doc_Code = EncryptByKey(KEY_GUID('sym_Key'), @Doc_Code)
select @E_Doc_No = EncryptByKey(KEY_GUID('sym_Key'), @Doc_No)

EXEC [proc_SymmetricKey_close]

-- =============================================
-- Insert Transcation
-- =============================================


	INSERT INTO [ViewAuditlogVR]
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
	, E_Session_ID, E_Browser, E_OS, Message_Code
	, E_Acc_Type, E_Acc_ID, E_Doc_Code, E_Doc_No)
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
	, @E_Session_ID, @E_Browser, @E_OS, @message_code
	, @E_Acc_Type, @E_Acc_ID, @E_Doc_Code, @E_Doc_No)


GO

GRANT EXECUTE ON [dbo].[proc_AuditlogHCVR_add] TO HCPUBLIC
GO
