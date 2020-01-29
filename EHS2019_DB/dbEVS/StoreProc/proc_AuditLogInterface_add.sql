IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_AuditlogInterface_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_AuditlogInterface_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- CR No.:			CRE12-001
-- Modified by:		Koala CHENG
-- Modified date: 	21-02-2012
-- Description:		Add HCSP, HCVU access right 
-- =============================================
-- =============================================
-- Modified by:		Derek Leung
-- Modified date: 		25-01-2011
-- Description:		Insert log with view  CRE11-001
-- =============================================
-- =============================================    
-- Modification History     
-- Modified by:    Paul Yip  
-- Modified date:  07-12-2010    
-- Description:    Add "E_Message_ID" field  
-- =============================================    
-- =============================================    
-- Author:   Koala Cheng    
-- Create date:  20-09-2010    
-- Description:  Add AuditlogInterface    
-- =============================================    
CREATE procedure [dbo].[proc_AuditlogInterface_add]     
 @action_time datetime    
,@end_time datetime    
,@action_key varchar(20)    
,@client_ip varchar(20)    
,@user_id varchar(20)    
,@data_entry_account varchar(20)    
,@function_code char(6)    
,@log_id varchar(10)    
,@description nvarchar(1000)    
,@session_id varchar(40)    
,@browser varchar(20)    
,@os varchar(20)    
,@message_code varchar(525)    
,@Data nvarchar(MAX)    
,@message_id varchar(40)
as    
    
-- =============================================    
-- Declaration    
-- =============================================    
    
declare @action_time_str varchar(30)    
declare @end_time_str  varchar(30)    
declare @system_dtm_str   varchar(30)    
declare @application_server varchar(20)    
declare @year varchar(2)    
declare @E_Action_Key varbinary(60)    
    
declare @System_Dtm datetime    
declare @E_System_Dtm varbinary(60)    
declare @E_Action_Dtm varbinary(60)    
declare @E_End_Dtm varbinary(60)    
declare @E_Client_IP varbinary(60)    
declare @E_User_ID varbinary(60)    
declare @E_DataEntryAccount varbinary(60)    
declare @E_Message_ID varBinary(100)

declare @E_Function_Code varbinary(50)    
declare @E_Log_ID varbinary(50)    
declare @E_Description varbinary(2100)    
declare @E_Application_Server varbinary(60)    
declare @E_Session_ID varbinary(100)    
    
declare @E_Browser varbinary(60)    
declare @E_OS varbinary(60)    
declare @E_Data varbinary(MAX)    
    
-- =============================================    
-- Initialization    
-- =============================================    
    
select @System_Dtm = getdate()    
select @action_time_str = convert(varchar(30), @action_time, 13)    
select @end_time_str = convert(varchar(30), @end_time, 13)    
select @system_dtm_str = convert(varchar(30), @System_Dtm, 13)    
select @application_server = host_name()    
    
    
select @year = convert(varchar(2), @system_dtm, 12)    
  
OPEN SYMMETRIC KEY sym_Key    
DECRYPTION BY ASYMMETRIC KEY asym_Key    
select @E_System_Dtm = EncryptByKey(KEY_GUID('sym_Key'), @system_dtm_str)    
select @E_Action_Dtm = EncryptByKey(KEY_GUID('sym_Key'), @action_time_str)    
select @E_End_Dtm = EncryptByKey(KEY_GUID('sym_Key'), @end_time_str)    
select @E_Action_Key = EncryptByKey(KEY_GUID('sym_Key'), @action_key)    
select @E_Client_IP = EncryptByKey(KEY_GUID('sym_Key'), @client_ip)    
select @E_User_ID = EncryptByKey(KEY_GUID('sym_Key'), @user_id)    
select @E_DataEntryAccount = EncryptByKey(KEY_GUID('sym_Key'), @data_entry_account)   
select @E_Message_ID = EncryptByKey(KEY_GUID('sym_Key'), @message_id)     
select @E_Function_Code = EncryptByKey(KEY_GUID('sym_Key'), @function_code)    
select @E_Log_ID = EncryptByKey(KEY_GUID('sym_Key'), @log_id)    
select @E_Description = EncryptByKey(KEY_GUID('sym_Key'), @description)    
select @E_Data = EncryptByKey(KEY_GUID('sym_Key'), @Data)    
select @E_Application_Server = EncryptByKey(KEY_GUID('sym_Key'), @application_server)    
select @E_Session_ID = EncryptByKey(KEY_GUID('sym_Key'), @session_id)    
    
select @E_Browser = EncryptByKey(KEY_GUID('sym_Key'), @browser)    
select @E_OS = EncryptByKey(KEY_GUID('sym_Key'), @os)    
    
CLOSE SYMMETRIC KEY sym_Key    
  
-- =============================================    
-- Insert transaction    
-- =============================================    
    

 insert into dbo.ViewAuditLogInterface
 (System_Dtm, E_System_Dtm, E_Action_Dtm, E_End_Dtm, E_Action_Key, E_Client_IP, E_User_ID    
 , E_DataEntryAccount, E_Function_Code, E_Log_ID, E_Description, E_Data, E_Application_Server, E_Session_ID, E_Browser, E_OS, Message_Code, E_Message_ID)    
 VALUES ( @System_Dtm    
 , @E_System_Dtm, @E_Action_Dtm, @E_End_Dtm, @E_Action_Key, @E_Client_IP, @E_User_ID    
 , @E_DataEntryAccount, @E_Function_Code, @E_Log_ID, @E_Description, @E_Data,  @E_Application_Server, @E_Session_ID, @E_Browser, @E_OS, @message_code, @E_Message_ID)    
    
  
GO

GRANT EXECUTE ON [dbo].[proc_AuditlogInterface_add] TO WSEXT
GO

GRANT EXECUTE ON [dbo].[proc_AuditlogInterface_add] TO WSINT
GO

GRANT EXECUTE ON [dbo].[proc_AuditlogInterface_add] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_AuditlogInterface_add] TO HCVU
GO

