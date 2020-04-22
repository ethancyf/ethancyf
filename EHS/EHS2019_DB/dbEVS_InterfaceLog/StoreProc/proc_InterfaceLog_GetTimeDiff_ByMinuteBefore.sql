IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InterfaceLog_GetTimeDiff_ByMinuteBefore]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InterfaceLog_GetTimeDiff_ByMinuteBefore]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	19 June 2018
-- CR No.:			CRE18-004 (CIMS Vaccination Sharing)
-- Description:		Add input parm [Request_System]
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		1 December 2010
-- Description:		Get Interface log for checking CMS connection
-- =============================================

CREATE PROCEDURE [dbo].[proc_InterfaceLog_GetTimeDiff_ByMinuteBefore]
	@Function_Code	char(6),
	@Log_ID			char(5),
	@Minute_Before	int,
	@Request_System	VARCHAR(10)
AS BEGIN

	SET NOCOUNT ON;


-- =============================================
-- Configration
-- =============================================
	DECLARE @End_Dtm		datetime
	DECLARE @Start_Dtm		datetime
	SET @End_Dtm = GETDATE()
	SET @Start_Dtm = DATEADD(mi, -1 * @Minute_Before, @End_Dtm)


-- =============================================
-- Return result
-- =============================================
	EXEC [proc_InterfaceLog_GetTimeDiff_ByDtm] @Function_Code, @Log_ID, @Start_Dtm, @End_Dtm, @Request_System


END
GO

GRANT EXECUTE ON [dbo].[proc_InterfaceLog_GetTimeDiff_ByMinuteBefore] TO WSINT
GO
