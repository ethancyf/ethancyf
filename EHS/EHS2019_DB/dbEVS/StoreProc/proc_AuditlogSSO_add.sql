IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_AuditlogSSO_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_AuditlogSSO_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modified by:		Derek Leung
-- Modified date: 	25-01-2011
-- Description:		Insert log with view  CRE11-001
-- =============================================
-- =============================================  
-- Author:   Paul Yip  
-- Create date:  14-06-2010  
-- Description:  Add SSO Auditlog  
-- =============================================  
  
  
CREATE procedure [dbo].[proc_AuditlogSSO_add]   
 @client_ip varchar(20)  
,@user_id varchar(20)  
,@log_id varchar(10)  
,@log_type char(1)  
,@description nvarchar(4000)  
,@session_id varchar(40)  
,@browser varchar(20)  
,@os varchar(20)  
as  
  
-- =============================================  
-- Declaration  
-- =============================================  
  
declare @application_server varchar(20)  
declare @year varchar(2)  
  
declare @System_Dtm datetime  
declare @E_Client_IP varbinary(60)  
declare @E_User_ID varbinary(60)  
declare @E_Description varbinary(5000)  
declare @E_Application_Server varbinary(60)  
declare @E_Session_ID varbinary(100)  
declare @E_Log_ID varbinary(60)  
declare @E_Log_Type varbinary(60)  
  
declare @E_Browser varbinary(60)  
declare @E_OS varbinary(60)  
  
-- =============================================  
-- Initialization  
-- =============================================  
  
select @System_Dtm = getdate()  
select @application_server = host_name()  
  
  
select @year = convert(varchar(2), @system_dtm, 12)  
  
OPEN SYMMETRIC KEY sym_Key  
DECRYPTION BY ASYMMETRIC KEY asym_Key  
select @E_Client_IP = EncryptByKey(KEY_GUID('sym_Key'), @client_ip)  
select @E_User_ID = EncryptByKey(KEY_GUID('sym_Key'), @user_id)  
select @E_Description = EncryptByKey(KEY_GUID('sym_Key'), @description)  
select @E_Application_Server = EncryptByKey(KEY_GUID('sym_Key'), @application_server)  
select @E_Session_ID = EncryptByKey(KEY_GUID('sym_Key'), @session_id)  
  
select @E_Log_ID = EncryptByKey(KEY_GUID('sym_Key'), @log_id)  
select @E_Log_Type = EncryptByKey(KEY_GUID('sym_Key'), @log_type)  
  
select @E_Browser = EncryptByKey(KEY_GUID('sym_Key'), @browser)  
select @E_OS = EncryptByKey(KEY_GUID('sym_Key'), @os)  
  
CLOSE SYMMETRIC KEY sym_Key  
  
-- =============================================  
-- Insert transaction  
-- =============================================  
  

 insert into dbo.ViewAuditlogSSO
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS, E_Log_ID,E_Log_Type)   
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS, @E_Log_ID, @E_Log_Type)  

Go

  
GRANT EXECUTE ON [dbo].[proc_AuditlogSSO_add] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_AuditlogSSO_add] TO HCVU
GO
