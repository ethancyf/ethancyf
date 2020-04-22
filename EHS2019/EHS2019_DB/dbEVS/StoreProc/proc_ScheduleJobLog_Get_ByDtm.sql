IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ScheduleJobLog_Get_ByDtm]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ScheduleJobLog_Get_ByDtm]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Winnie SUEN
-- Create date:		2 Mar 2017
-- CR No.:			CRE16-019
-- Description:		Get Schedule Job log
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ScheduleJobLog_Get_ByDtm]
	@Program_ID		VARCHAR(30),
	@Log_ID			char(5),
	@Start_Dtm		datetime,
	@End_Dtm		datetime
AS BEGIN

	SET NOCOUNT ON;


-- =============================================
-- Return result
-- =============================================

	SELECT
		System_Dtm,
		Action_Dtm,
		End_Dtm,
		Program_ID,
		Log_ID,
		Description,
		Action_Key
	FROM
		ViewScheduleJobLog
	WHERE
		System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
			AND Program_ID = @Program_ID
			AND Log_ID = @Log_ID
	ORDER BY
		System_Dtm DESC




END
GO

GRANT EXECUTE ON [dbo].[proc_ScheduleJobLog_Get_ByDtm] TO HCVU
GO
