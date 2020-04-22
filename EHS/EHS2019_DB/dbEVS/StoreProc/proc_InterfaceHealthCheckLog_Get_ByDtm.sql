IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InterfaceHealthCheckLog_Get_ByDtm]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InterfaceHealthCheckLog_Get_ByDtm]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		3 December 2010
-- Description:		Get InterfaceHealthCheckLog
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_InterfaceHealthCheckLog_Get_ByDtm]
	@Interface_Code		char(10),
	@Function_Code		char(10),
	@Log_ID				char(5),
	@Start_Dtm			datetime,
	@End_Dtm			datetime
AS BEGIN

	SET NOCOUNT ON;
	

-- =============================================
-- Return result
-- =============================================

	SELECT
		System_Dtm,
		Log_Seq,
		Action_Dtm,
		Client_IP,
		Interface_Code,
		Function_Code,
		Log_ID,
		Description,
		System_Message
	FROM
		InterfaceHealthCheckLog
	WHERE
		System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
			AND (@Interface_Code IS NULL OR Interface_Code = @Interface_Code)
			AND (@Function_Code IS NULL OR Function_Code = @Function_Code)
			AND (@Log_ID IS NULL OR Log_ID = @Log_ID)
	ORDER BY
		System_Dtm DESC
		

END
GO

GRANT EXECUTE ON [dbo].[proc_InterfaceHealthCheckLog_Get_ByDtm] TO HCVU
GO
