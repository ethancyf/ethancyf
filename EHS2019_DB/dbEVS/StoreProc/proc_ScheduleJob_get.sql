IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ScheduleJob_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ScheduleJob_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Koala Cheng
-- Create date:		13 May 2011
-- Description:		Get ScheduleJob
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_ScheduleJob_get]

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
		SJ_ID,
		SJ_Name,
		SJ_Path,
		Update_Dtm
	FROM
		ScheduleJob
	ORDER BY
		SJ_Name

END
GO

GRANT EXECUTE ON [dbo].[proc_ScheduleJob_get] TO HCVU
GO
