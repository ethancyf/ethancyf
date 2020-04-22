IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ScheduleJobSuspend_get_Outstanding]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ScheduleJobSuspend_get_Outstanding]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		19 April 2011
-- Description:		Retrieve outstanding ScheduleJobSuspend
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ScheduleJobSuspend_get_Outstanding]
	@SJ_ID		varchar(30) = NULL
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
		s.SJ_ID,
		n.SJ_Name,
		s.Start_Dtm,
		s.End_Dtm,
		s.Description,
		s.Create_Dtm,
		s.Create_By
	FROM
		ScheduleJobSuspend s INNER JOIN ScheduleJob n
		ON s.SJ_ID = n.SJ_ID
	WHERE
		(@SJ_ID IS NULL OR s.SJ_ID = @SJ_ID)
			AND (s.End_Dtm IS NULL OR GETDATE() < s.End_Dtm)
	ORDER BY
		s.Start_Dtm,
		s.End_Dtm


END
GO

GRANT EXECUTE ON [dbo].[proc_ScheduleJobSuspend_get_Outstanding] TO HCVU
GO
