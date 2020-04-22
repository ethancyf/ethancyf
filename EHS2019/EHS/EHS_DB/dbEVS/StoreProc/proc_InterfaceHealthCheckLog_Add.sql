IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InterfaceHealthCheckLog_Add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InterfaceHealthCheckLog_Add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Karl LAM
-- Modified date:	15 Oct 2013
-- Description:		Change [Description] from varchar(50) to varchar(510)
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		16 November 2010
-- Description:		Add InterfaceHealthCheckLog
-- =============================================


CREATE PROCEDURE [dbo].[proc_InterfaceHealthCheckLog_Add]
	@Action_Dtm			datetime,
	@Client_IP			varchar(20),
	@Interface_Code		char(10),
	@Function_Code		char(10),
	@Log_ID				char(5),
	@Description		varchar(510),
	@System_Message		varchar(510)
AS BEGIN

-- =============================================
-- Insert Transcation
-- =============================================
	INSERT INTO InterfaceHealthCheckLog (
		System_Dtm,
		Action_Dtm,
		Client_IP,
		Interface_Code,
		Function_Code,
		Log_ID,
		Description,
		System_Message
	) 
	SELECT
		GETDATE() AS [System_Dtm],
		@Action_Dtm,
		@Client_IP,
		@Interface_Code,
		@Function_Code,
		@Log_ID,
		@Description,
		@System_Message

END
GO

GRANT EXECUTE ON [dbo].[proc_InterfaceHealthCheckLog_Add] TO HCVU
GO
