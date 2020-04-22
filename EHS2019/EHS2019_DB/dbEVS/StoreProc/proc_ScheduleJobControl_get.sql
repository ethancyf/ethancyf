IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ScheduleJobControl_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ScheduleJobControl_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		20 April 2011
-- Description:		Get ScheduleJobControl
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ScheduleJobControl_get]
	@Control_ID		varchar(50),
	@Server_Name	varchar(50),
	@SJ_ID	varchar(30)
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
		c.Control_ID,
		c.Server_Name,
		n.SJ_Name,
		c.Data,
		c.Description,
		c.Update_Dtm
	FROM
		ScheduleJobControl c INNER JOIN ScheduleJob n
		ON c.SJ_ID = n.SJ_ID
	WHERE
		c.Control_ID = @Control_ID
			AND (@Server_Name IS NULL OR c.Server_Name = @Server_Name)
			AND (@SJ_ID IS NULL OR c.SJ_ID = @SJ_ID)

END
GO

GRANT EXECUTE ON [dbo].[proc_ScheduleJobControl_get] TO HCVU
GO
