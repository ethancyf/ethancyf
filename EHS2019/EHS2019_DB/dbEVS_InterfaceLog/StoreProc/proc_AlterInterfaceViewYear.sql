IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_AlterInterfaceViewYear]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_AlterInterfaceViewYear]
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
-- Description:		Alter Year in View for log in InterfaceLog
-- =============================================


CREATE PROCEDURE [dbo].[proc_AlterInterfaceViewYear]
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
	-- ViewAuditLogInterface
	-- -----------------------
	SET @strSQL = 'ALTER VIEW [dbo].[ViewAuditLogInterface]'
	SET @strSQL =  @strSQL + 'AS ' 
	SET @strSQL =  @strSQL + 'SELECT     AuditLogInterface' + @strYear +  '.* '
	SET @strSQL =  @strSQL + 'FROM       AuditLogInterface' + @strYear

	EXEC(@strSQL)

END
GO
