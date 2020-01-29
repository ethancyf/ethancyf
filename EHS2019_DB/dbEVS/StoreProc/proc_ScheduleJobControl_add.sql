IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ScheduleJobControl_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ScheduleJobControl_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		20 April 2011
-- Description:		Add ScheduleJobControl 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ScheduleJobControl_add]
	@Control_ID		varchar(50),
	@Server_Name	varchar(50),
	@SJ_ID			varchar(30),
	@Data			varchar(510),
	@Description	varchar(510)
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
	
	INSERT INTO ScheduleJobControl (
		Control_ID,
		Server_Name,
		SJ_ID,
		Data,
		Description,
		Update_Dtm
	) VALUES (
		@Control_ID,
		@Server_Name,
		@SJ_ID,
		@Data,
		@Description,
		GETDATE()
	)


END
GO

GRANT EXECUTE ON [dbo].[proc_ScheduleJobControl_add] TO HCVU
GO
