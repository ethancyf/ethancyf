IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_AlterViewYear]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_AlterViewYear]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================
-- =============================================
-- Author:			Chris YIM
-- Create date:		09 Mar 2017
-- CR No.:			CRE16-019
-- Description:		Alter Year in View for log 
-- =============================================


CREATE PROCEDURE [dbo].[proc_AlterViewYear]
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
	DECLARE @strSQL varchar(500)
	DECLARE @strYear varchar(2)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	SET @strSQL = ''
	SET @strYear = (SELECT CONVERT(VARCHAR(2), GETDATE(), 12))  

-- =============================================
-- Return results
-- =============================================

	-- -----------------------
	-- ViewAuditLogHCSP
	-- -----------------------
	SET @strSQL = 'ALTER VIEW [dbo].[ViewAuditLogHCSP] '
	SET @strSQL =  @strSQL + 'AS ' 
	SET @strSQL =  @strSQL + 'SELECT     AuditLogHCSP' + @strYear +  '.* '
	SET @strSQL =  @strSQL + 'FROM       AuditLogHCSP' + @strYear

	EXEC(@strSQL)

	-- -----------------------
	-- ViewAuditLogHCVU
	-- -----------------------
	SET @strSQL = ''

	SET @strSQL = 'ALTER VIEW [dbo].[ViewAuditLogHCVU] '
	SET @strSQL =  @strSQL + 'AS ' 
	SET @strSQL =  @strSQL + 'SELECT     AuditLogHCVU' + @strYear +  '.* '
	SET @strSQL =  @strSQL + 'FROM         AuditLogHCVU' + @strYear

	EXEC(@strSQL)

	-- -----------------------
	-- ViewAuditLogIVRSHCSP
	-- -----------------------
	SET @strSQL = ''

	SET @strSQL = 'ALTER VIEW [dbo].[ViewAuditLogIVRSHCSP] '
	SET @strSQL =  @strSQL + 'AS ' 
	SET @strSQL =  @strSQL + 'SELECT     AuditLogIVRSHCSP' + @strYear +  '.* '
	SET @strSQL =  @strSQL + 'FROM         AuditLogIVRSHCSP' + @strYear

	EXEC(@strSQL)

	-- -----------------------
	-- ViewAuditLogIVRSVR
	-- -----------------------
	SET @strSQL = ''

	SET @strSQL = 'ALTER VIEW [dbo].[ViewAuditLogIVRSVR] '
	SET @strSQL =  @strSQL + 'AS ' 
	SET @strSQL =  @strSQL + 'SELECT     AuditLogIVRSVR' + @strYear +  '.* '
	SET @strSQL =  @strSQL + 'FROM         AuditLogIVRSVR' + @strYear

	EXEC(@strSQL)

	-- -----------------------
	-- ViewAuditLogPublic
	-- -----------------------
	SET @strSQL = ''

	SET @strSQL = 'ALTER VIEW [dbo].[ViewAuditLogPublic] '
	SET @strSQL =  @strSQL + 'AS ' 
	SET @strSQL =  @strSQL + 'SELECT     AuditLogPublic' + @strYear +  '.* '
	SET @strSQL =  @strSQL + 'FROM         AuditLogPublic' + @strYear

	EXEC(@strSQL)

	-- -----------------------
	-- ViewAuditLogSSO
	-- -----------------------
	SET @strSQL = ''

	SET @strSQL = 'ALTER VIEW [dbo].[ViewAuditLogSSO] '
	SET @strSQL =  @strSQL + 'AS ' 
	SET @strSQL =  @strSQL + 'SELECT     AuditLogSSO' + @strYear +  '.* '
	SET @strSQL =  @strSQL + 'FROM         AuditLogSSO' + @strYear

	EXEC(@strSQL)

	-- -----------------------
	-- ViewAuditLogVR
	-- -----------------------
	SET @strSQL = ''

	SET @strSQL = 'ALTER VIEW [dbo].[ViewAuditLogVR] '
	SET @strSQL =  @strSQL + 'AS ' 
	SET @strSQL =  @strSQL + 'SELECT     AuditLogVR' + @strYear +  '.* '
	SET @strSQL =  @strSQL + 'FROM         AuditLogVR' + @strYear

	EXEC(@strSQL)

	-- -----------------------
	-- ViewScheduleJobLog
	-- -----------------------
	SET @strSQL = ''

	SET @strSQL = 'ALTER VIEW [dbo].[ViewScheduleJobLog] '
	SET @strSQL =  @strSQL + 'AS ' 
	SET @strSQL =  @strSQL + 'SELECT ScheduleJobLog' + @strYear +  '.* '
	SET @strSQL =  @strSQL + 'FROM ScheduleJobLog' + @strYear

	EXEC(@strSQL)

END
GO
