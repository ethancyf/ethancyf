IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_AuditlogSSO_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_AuditlogSSO_add]
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
-- Author:   Paul Yip  
-- Create date:  19-01-2010  
-- Description:  Add SSO Auditlog  
-- =============================================  
  
  
CREATE procedure [dbo].[proc_AuditlogSSO_add]   
 @client_ip varchar(20)  
,@user_id varchar(20)  
,@description nvarchar(1000)  
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
declare @E_Description varbinary(2100)  
declare @E_Application_Server varbinary(60)  
declare @E_Session_ID varbinary(100)  
  
declare @E_Browser varbinary(60)  
declare @E_OS varbinary(60)  
  
-- =============================================  
-- Initialization  
-- =============================================  
  
select @System_Dtm = getdate()  
select @application_server = host_name()  
  
  
select @year = convert(varchar(2), @system_dtm, 12)  
  
EXEC [proc_SymmetricKey_open]

select @E_Client_IP = EncryptByKey(KEY_GUID('sym_Key'), @client_ip)  
select @E_User_ID = EncryptByKey(KEY_GUID('sym_Key'), @user_id)  
select @E_Description = EncryptByKey(KEY_GUID('sym_Key'), @description)  
select @E_Application_Server = EncryptByKey(KEY_GUID('sym_Key'), @application_server)  
select @E_Session_ID = EncryptByKey(KEY_GUID('sym_Key'), @session_id)  
  
select @E_Browser = EncryptByKey(KEY_GUID('sym_Key'), @browser)  
select @E_OS = EncryptByKey(KEY_GUID('sym_Key'), @os)  
  
EXEC [proc_SymmetricKey_close]
  
-- =============================================  
-- Insert transaction  
-- =============================================  
  
if @year = '08'  
begin  
 insert into dbo.AuditlogSSO08  
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS)  
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS)  
end  
else if @year = '09'  
begin  
 insert into dbo.AuditlogSSO09  
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS)  
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS)  
end  
else if @year = '10'  
begin  
 insert into dbo.AuditlogSSO10  
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS)  
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS)  
end  
else if @year = '11'  
begin  
 insert into dbo.AuditlogSSO11  
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS)  
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS)  
end  
else if @year = '12'  
begin  
 insert into dbo.AuditlogSSO12  
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS)  
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS)  
end  
else if @year = '13'  
begin  
 insert into dbo.AuditlogSSO13  
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS)  
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS)  
end  
else if @year = '14'  
begin  
 insert into dbo.AuditlogSSO14  
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS)  
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS)  
end  
else if @year = '15'  
begin  
 insert into dbo.AuditlogSSO15  
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS)  
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS)  
end  
else if @year = '16'  
begin  
 insert into dbo.AuditlogSSO16  
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS)  
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS)  
end  
else if @year = '17'  
begin  
 insert into dbo.AuditlogSSO17  
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS)  
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS)  
end  
else if @year = '18'  
begin  
 insert into dbo.AuditlogSSO18  
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS)  
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS)  
end  
else if @year = '19'  
begin  
 insert into dbo.AuditlogSSO19  
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS)  
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS)  
end  
else if @year = '20'  
begin  
 insert into dbo.AuditlogSSO20  
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS)  
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS)  
end  
else if @year = '21'  
begin  
 insert into dbo.AuditlogSSO21  
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS)  
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS)  
end  
else if @year = '22'  
begin  
 insert into dbo.AuditlogSSO22  
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS)  
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS)  
end  
else if @year = '23'  
begin  
 insert into dbo.AuditlogSSO23  
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS)  
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS)  
end  
else if @year = '24'  
begin  
 insert into dbo.AuditlogSSO24  
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS)  
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS)  
end  
else if @year = '25'  
begin  
 insert into dbo.AuditlogSSO25  
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS)  
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS)  
end  
else if @year = '26'  
begin  
 insert into dbo.AuditlogSSO26  
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS)  
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS)  
end  
else if @year = '27'  
begin  
 insert into dbo.AuditlogSSO27  
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS)  
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS)  
end  
else if @year = '28'  
begin  
 insert into dbo.AuditlogSSO28  
 (System_Dtm, E_Client_IP, E_User_ID, E_Description, E_Application_Server, E_Session_ID, E_Browser, E_OS)  
 VALUES ( @System_Dtm, @E_Client_IP, @E_User_ID, @E_Description, @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS)  
end  
  
  
GRANT EXECUTE ON [dbo].[proc_AuditlogSSO_add] TO HCSSO
GO

