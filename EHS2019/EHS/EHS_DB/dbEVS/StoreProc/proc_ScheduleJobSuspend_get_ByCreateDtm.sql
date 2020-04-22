IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ScheduleJobSuspend_get_ByCreateDtm]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ScheduleJobSuspend_get_ByCreateDtm]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		20 April 2011
-- Description:		Retrieve ScheduleJobSuspend by Create_Dtm
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ScheduleJobSuspend_get_ByCreateDtm]
	@Create_Dtm		datetime
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	SELECT
		COUNT(1)
	FROM
		ScheduleJobSuspend
	WHERE
		Create_Dtm > @Create_Dtm

	SELECT
		MAX(Create_Dtm)
	FROM
		ScheduleJobSuspend
	WHERE
		Create_Dtm > @Create_Dtm


END
GO

GRANT EXECUTE ON [dbo].[proc_ScheduleJobSuspend_get_ByCreateDtm] TO HCVU
GO
