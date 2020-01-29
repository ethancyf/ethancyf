IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ScheduleJobControl_update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ScheduleJobControl_update]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		20 April 2011
-- Description:		Update ScheduleJobControl 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ScheduleJobControl_update]
	@Control_ID		varchar(50),
	@Server_Name	varchar(50),
	@SJ_ID			varchar(50),
	@Data			varchar(510)
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
	
	UPDATE
		ScheduleJobControl
	SET
		Data = @Data,
		Update_Dtm = GETDATE()
	WHERE
		Control_ID = @Control_ID
			AND (@Server_Name IS NULL OR Server_Name = @Server_Name)
			AND (@SJ_ID IS NULL OR SJ_ID = @SJ_ID)

END
GO

GRANT EXECUTE ON [dbo].[proc_ScheduleJobControl_update] TO HCVU
GO
