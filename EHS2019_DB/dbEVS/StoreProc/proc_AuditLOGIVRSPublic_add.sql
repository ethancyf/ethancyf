IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_AuditLOGIVRSPublic_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_AuditLOGIVRSPublic_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Pak Ho LEE
-- Create date:		19-11-2008
-- Description:		Add AuditLOGIVRSPublic
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE procedure [dbo].[proc_AuditLOGIVRSPublic_add]

@action_time datetime,
@end_time datetime,
@action_key varchar(20),
@unique_id varchar(40),
@function_code char(6),
@log_id varchar(10),
@description nvarchar(1000)

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
DECLARE @E_Function_Code varbinary(50)
DECLARE @E_Log_ID varbinary(50)
DECLARE @E_Description varbinary(2100)
DECLARE @E_Application_Server varbinary(60)


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

SELECT @E_System_Dtm = EncryptByKey(KEY_GUID('sym_Key'), @system_dtm_str)
SELECT @E_Action_Dtm = EncryptByKey(KEY_GUID('sym_Key'), @action_time_str)
SELECT @E_End_Dtm = EncryptByKey(KEY_GUID('sym_Key'), @end_time_str)
SELECT @E_Action_Key = EncryptByKey(KEY_GUID('sym_Key'), @action_key)
SELECT @E_CallConnectionID = EncryptByKey(KEY_GUID('sym_Key'), @unique_id)
SELECT @E_Function_Code = EncryptByKey(KEY_GUID('sym_Key'), @function_code)
SELECT @E_Log_ID = EncryptByKey(KEY_GUID('sym_Key'), @log_id)
SELECT @E_Description = EncryptByKey(KEY_GUID('sym_Key'), @description)
SELECT @E_Application_Server = EncryptByKey(KEY_GUID('sym_Key'), @application_server)

CLOSE SYMMETRIC KEY sym_Key

-- =============================================
-- Insert transaction
-- =============================================

IF @year = '08'
BEGIN
	INSERT INTO [dbo].[AuditLogIVRSPublic08]
	(
		System_Dtm,
		E_System_Dtm,
		E_Action_Dtm,
		E_End_Dtm,
		E_Action_Key,
		E_CallConnectionID,
		E_Function_Code,
		E_Log_ID,
		E_Description,
		E_Application_Server
	)
	VALUES
	(
		@System_Dtm,
		@E_System_Dtm,
		@E_Action_Dtm,
		@E_End_Dtm,
		@E_Action_Key,
		@E_CallConnectionID,
		@E_Function_Code,
		@E_Log_ID,
		@E_Description,
		@E_Application_Server
	)
END
ELSE IF @year = '09'
BEGIN
	INSERT INTO [dbo].[AuditLogIVRSPublic09]
	(
		System_Dtm,
		E_System_Dtm,
		E_Action_Dtm,
		E_End_Dtm,
		E_Action_Key,
		E_CallConnectionID,
		E_Function_Code,
		E_Log_ID,
		E_Description,
		E_Application_Server
	)
	VALUES
	(
		@System_Dtm,
		@E_System_Dtm,
		@E_Action_Dtm,
		@E_End_Dtm,
		@E_Action_Key,
		@E_CallConnectionID,
		@E_Function_Code,
		@E_Log_ID,
		@E_Description,
		@E_Application_Server
	)
END
ELSE IF @year = '10'
BEGIN
	INSERT INTO [dbo].[AuditLogIVRSPublic10]
	(
		System_Dtm,
		E_System_Dtm,
		E_Action_Dtm,
		E_End_Dtm,
		E_Action_Key,
		E_CallConnectionID,
		E_Function_Code,
		E_Log_ID,
		E_Description,
		E_Application_Server
	)
	VALUES
	(
		@System_Dtm,
		@E_System_Dtm,
		@E_Action_Dtm,
		@E_End_Dtm,
		@E_Action_Key,
		@E_CallConnectionID,
		@E_Function_Code,
		@E_Log_ID,
		@E_Description,
		@E_Application_Server
	)
END
ELSE IF @year = '11'
BEGIN
	INSERT INTO [dbo].[AuditLogIVRSPublic11]
	(
		System_Dtm,
		E_System_Dtm,
		E_Action_Dtm,
		E_End_Dtm,
		E_Action_Key,
		E_CallConnectionID,
		E_Function_Code,
		E_Log_ID,
		E_Description,
		E_Application_Server
	)
	VALUES
	(
		@System_Dtm,
		@E_System_Dtm,
		@E_Action_Dtm,
		@E_End_Dtm,
		@E_Action_Key,
		@E_CallConnectionID,
		@E_Function_Code,
		@E_Log_ID,
		@E_Description,
		@E_Application_Server
	)
END
ELSE IF @year = '12'
BEGIN
	INSERT INTO [dbo].[AuditLogIVRSPublic12]
	(
		System_Dtm,
		E_System_Dtm,
		E_Action_Dtm,
		E_End_Dtm,
		E_Action_Key,
		E_CallConnectionID,
		E_Function_Code,
		E_Log_ID,
		E_Description,
		E_Application_Server
	)
	VALUES
	(
		@System_Dtm,
		@E_System_Dtm,
		@E_Action_Dtm,
		@E_End_Dtm,
		@E_Action_Key,
		@E_CallConnectionID,
		@E_Function_Code,
		@E_Log_ID,
		@E_Description,
		@E_Application_Server
	)
END
ELSE IF @year = '13'
BEGIN
	INSERT INTO [dbo].[AuditLogIVRSPublic13]
	(
		System_Dtm,
		E_System_Dtm,
		E_Action_Dtm,
		E_End_Dtm,
		E_Action_Key,
		E_CallConnectionID,
		E_Function_Code,
		E_Log_ID,
		E_Description,
		E_Application_Server
	)
	VALUES
	(
		@System_Dtm,
		@E_System_Dtm,
		@E_Action_Dtm,
		@E_End_Dtm,
		@E_Action_Key,
		@E_CallConnectionID,
		@E_Function_Code,
		@E_Log_ID,
		@E_Description,
		@E_Application_Server
	)
END
ELSE IF @year = '14'
BEGIN
	INSERT INTO [dbo].[AuditLogIVRSPublic14]
	(
		System_Dtm,
		E_System_Dtm,
		E_Action_Dtm,
		E_End_Dtm,
		E_Action_Key,
		E_CallConnectionID,
		E_Function_Code,
		E_Log_ID,
		E_Description,
		E_Application_Server
	)
	VALUES
	(
		@System_Dtm,
		@E_System_Dtm,
		@E_Action_Dtm,
		@E_End_Dtm,
		@E_Action_Key,
		@E_CallConnectionID,
		@E_Function_Code,
		@E_Log_ID,
		@E_Description,
		@E_Application_Server
	)
END
ELSE IF @year = '15'
BEGIN
	INSERT INTO [dbo].[AuditLogIVRSPublic15]
	(
		System_Dtm,
		E_System_Dtm,
		E_Action_Dtm,
		E_End_Dtm,
		E_Action_Key,
		E_CallConnectionID,
		E_Function_Code,
		E_Log_ID,
		E_Description,
		E_Application_Server
	)
	VALUES
	(
		@System_Dtm,
		@E_System_Dtm,
		@E_Action_Dtm,
		@E_End_Dtm,
		@E_Action_Key,
		@E_CallConnectionID,
		@E_Function_Code,
		@E_Log_ID,
		@E_Description,
		@E_Application_Server
	)
END
ELSE IF @year = '16'
BEGIN
	INSERT INTO [dbo].[AuditLogIVRSPublic16]
	(
		System_Dtm,
		E_System_Dtm,
		E_Action_Dtm,
		E_End_Dtm,
		E_Action_Key,
		E_CallConnectionID,
		E_Function_Code,
		E_Log_ID,
		E_Description,
		E_Application_Server
	)
	VALUES
	(
		@System_Dtm,
		@E_System_Dtm,
		@E_Action_Dtm,
		@E_End_Dtm,
		@E_Action_Key,
		@E_CallConnectionID,
		@E_Function_Code,
		@E_Log_ID,
		@E_Description,
		@E_Application_Server
	)
END
ELSE IF @year = '17'
BEGIN
	INSERT INTO [dbo].[AuditLogIVRSPublic17]
	(
		System_Dtm,
		E_System_Dtm,
		E_Action_Dtm,
		E_End_Dtm,
		E_Action_Key,
		E_CallConnectionID,
		E_Function_Code,
		E_Log_ID,
		E_Description,
		E_Application_Server
	)
	VALUES
	(
		@System_Dtm,
		@E_System_Dtm,
		@E_Action_Dtm,
		@E_End_Dtm,
		@E_Action_Key,
		@E_CallConnectionID,
		@E_Function_Code,
		@E_Log_ID,
		@E_Description,
		@E_Application_Server
	)
END
ELSE IF @year = '18'
BEGIN
	INSERT INTO [dbo].[AuditLogIVRSPublic18]
	(
		System_Dtm,
		E_System_Dtm,
		E_Action_Dtm,
		E_End_Dtm,
		E_Action_Key,
		E_CallConnectionID,
		E_Function_Code,
		E_Log_ID,
		E_Description,
		E_Application_Server
	)
	VALUES
	(
		@System_Dtm,
		@E_System_Dtm,
		@E_Action_Dtm,
		@E_End_Dtm,
		@E_Action_Key,
		@E_CallConnectionID,
		@E_Function_Code,
		@E_Log_ID,
		@E_Description,
		@E_Application_Server
	)
END

GO

GRANT EXECUTE ON [dbo].[proc_AuditLOGIVRSPublic_add] TO HCPUBLIC
GO
