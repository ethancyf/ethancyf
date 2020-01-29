IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ScheduleJobSuspend_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ScheduleJobSuspend_del]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		19 April 2011
-- Description:		Delete ScheduleJobSuspend
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ScheduleJobSuspend_del]
	@SJ_ID			varchar(30),
	@Start_Dtm		datetime
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
	
	DELETE FROM
		ScheduleJobSuspend
	WHERE
		SJ_ID = @SJ_ID
			AND Start_Dtm = @Start_Dtm

	RETURN @@ROWCOUNT
	

END
GO

GRANT EXECUTE ON [dbo].[proc_ScheduleJobSuspend_del] TO HCVU
GO
