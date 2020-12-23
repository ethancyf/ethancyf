IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_AuditLOGIVRSHCSP_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_AuditLOGIVRSHCSP_add]
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
-- Create date:		12-01-2011
-- Description:		Add Extra detail columns (E_Acc_Type, E_Acc_ID, E_Doc_Code, E_Doc_No
-- =============================================
-- =============================================
-- Author:			Pak Ho LEE
-- Create date:		19-11-2008
-- Description:		Add AuditLOGIVRSHCSP
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE procedure [dbo].[proc_AuditLOGIVRSHCSP_add]

@action_time datetime,
@end_time datetime,
@action_key varchar(20),
@unique_id varchar(40),
@user_id varchar(20),
@function_code char(6),
@log_id varchar(10),
@description nvarchar(1000),
@Acc_Type varchar(1),
@Acc_ID varchar(15),
@Doc_Code varchar(15),
@Doc_No varchar(20)
AS

-- =============================================
-- Declaration
-- =============================================

/*
@System_Dtm
@E_System_Dtm
@E_Action_Dtm
@E_End_Dtm
@E_Action_Key
@E_CallConnectionID
@E_User_ID
@E_Function_Code
@E_Log_ID
@E_Description
@E_Application_Server
*/

DECLARE @action_time_str	varchar(30)
DECLARE @end_time_str		varchar(30)
DECLARE @system_dtm_str			varchar(30)
DECLARE	@application_server	varchar(20)
DECLARE @year	varchar(2)
DECLARE @E_Action_Key varbinary(60)

DECLARE @System_Dtm datetime
DECLARE @E_System_Dtm varbinary(60)
DECLARE @E_Action_Dtm varbinary(60)
DECLARE @E_End_Dtm varbinary(60)
DECLARE @E_CallConnectionID varbinary(100)
DECLARE @E_User_ID varbinary(60)
DECLARE @E_Function_Code varbinary(50)
DECLARE @E_Log_ID varbinary(50)
DECLARE @E_Description varbinary(2100)
DECLARE @E_Application_Server varbinary(60)

DECLARE @E_Acc_Type varbinary(50)
DECLARE @E_Acc_ID varbinary(50)
DECLARE @E_Doc_Code varbinary(50)
DECLARE @E_Doc_No varbinary(100)

-- =============================================
-- Initialization
-- =============================================

select @System_Dtm = getdate()
select @action_time_str = convert(varchar(30), @action_time, 13)
select @end_time_str = convert(varchar(30), @end_time, 13)
select @system_dtm_str = convert(varchar(30), @System_Dtm, 13)
select @application_server = host_name()


select @year = convert(varchar(2), @system_dtm, 12)

EXEC [proc_SymmetricKey_open]

SELECT @E_System_Dtm = EncryptByKey(KEY_GUID('sym_Key'), @system_dtm_str)
SELECT @E_Action_Dtm = EncryptByKey(KEY_GUID('sym_Key'), @action_time_str)
SELECT @E_End_Dtm = EncryptByKey(KEY_GUID('sym_Key'), @end_time_str)
SELECT @E_Action_Key = EncryptByKey(KEY_GUID('sym_Key'), @action_key)
SELECT @E_CallConnectionID = EncryptByKey(KEY_GUID('sym_Key'), @unique_id)
SELECT @E_User_ID = EncryptByKey(KEY_GUID('sym_Key'), @user_id)
SELECT @E_Function_Code = EncryptByKey(KEY_GUID('sym_Key'), @function_code)
SELECT @E_Log_ID = EncryptByKey(KEY_GUID('sym_Key'), @log_id)
SELECT @E_Description = EncryptByKey(KEY_GUID('sym_Key'), @description)
SELECT @E_Application_Server = EncryptByKey(KEY_GUID('sym_Key'), @application_server)

select @E_Acc_Type = EncryptByKey(KEY_GUID('sym_Key'), @Acc_Type)
select @E_Acc_ID = EncryptByKey(KEY_GUID('sym_Key'), @Acc_ID)
select @E_Doc_Code = EncryptByKey(KEY_GUID('sym_Key'), @Doc_Code)
select @E_Doc_No = EncryptByKey(KEY_GUID('sym_Key'), @Doc_No)

EXEC [proc_SymmetricKey_close]

-- =============================================
-- Insert transaction
-- =============================================

	INSERT INTO [dbo].[ViewAuditLogIVRSHCSP]
	(
		System_Dtm,
		E_System_Dtm,
		E_Action_Dtm,
		E_End_Dtm,
		E_Action_Key,
		E_CallConnectionID,
		E_User_ID,
		E_Function_Code,
		E_Log_ID,
		E_Description,
		E_Application_Server,
		E_Acc_Type, E_Acc_ID, E_Doc_Code, E_Doc_No
	)
	VALUES
	(
		@System_Dtm,
		@E_System_Dtm,
		@E_Action_Dtm,
		@E_End_Dtm,
		@E_Action_Key,
		@E_CallConnectionID,
		@E_User_ID,
		@E_Function_Code,
		@E_Log_ID,
		@E_Description,
		@E_Application_Server, 
		@E_Acc_Type, @E_Acc_ID, @E_Doc_Code, @E_Doc_No
	)

GO

GRANT EXECUTE ON [dbo].[proc_AuditLOGIVRSHCSP_add] TO HCSP
GO
