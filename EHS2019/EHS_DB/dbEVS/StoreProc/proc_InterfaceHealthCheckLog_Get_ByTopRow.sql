IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InterfaceHealthCheckLog_Get_ByTopRow]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InterfaceHealthCheckLog_Get_ByTopRow]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	18 December 2012
-- Description:		Performance Tuning
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		15 November 2010
-- Description:		Get InterfaceHealthCheckLog
-- =============================================
CREATE PROCEDURE [dbo].[proc_InterfaceHealthCheckLog_Get_ByTopRow]
	@Interface_Code		char(10),
	@Function_Code		char(10),
	@Log_ID				char(5),
	@Top_Row			int
AS BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Retrieve data
-- =============================================
	
	SELECT  TOP (@Top_Row)
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
		InterfaceHealthCheckLog WITH (NOLOCK)
	WHERE  
		(@Interface_Code IS NULL OR Interface_Code = @Interface_Code)  
		AND (@Function_Code IS NULL OR Function_Code = @Function_Code)  
		AND (@Log_ID IS NULL OR Log_ID = @Log_ID)  
	ORDER BY  
		System_Dtm DESC  
		

END
GO

GRANT EXECUTE ON [dbo].[proc_InterfaceHealthCheckLog_Get_ByTopRow] TO HCVU
GO
