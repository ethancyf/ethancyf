IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ScheduleJobSuspend_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ScheduleJobSuspend_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		19 April 2011
-- Description:		Add ScheduleJobSuspend
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ScheduleJobSuspend_add]
	@SJ_ID			varchar(30),
	@Start_Dtm		datetime,
	@End_Dtm		datetime,
	@Description	varchar(510),
	@Create_By		varchar(8)
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
	
	INSERT INTO ScheduleJobSuspend (
		SJ_ID,
		Start_Dtm,
		End_Dtm,
		Description,
		Create_Dtm,
		Create_By
	) VALUES (
		@SJ_ID,
		@Start_Dtm,
		@End_Dtm,
		@Description,
		GETDATE(),
		@Create_By
	)


END
GO

GRANT EXECUTE ON [dbo].[proc_ScheduleJobSuspend_add] TO HCVU
GO
