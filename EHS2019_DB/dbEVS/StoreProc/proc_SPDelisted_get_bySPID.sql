IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SPDelisted_get_bySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SPDelisted_get_bySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 11 July 2008
-- Description:	Retrieve the latest Return Info in Table
--				"SPDelisted"
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_SPDelisted_get_bySPID]
	@sp_id char(8)
AS
BEGIN

	SET NOCOUNT ON;

	SELECT	SP_ID, Logo_Return_Dtm, Token_Return_Dtm, Create_Dtm, Create_By,
			Update_Dtm, Update_By, TSMP
	FROM	SPDelisted
	WHERE	SP_ID = @sp_id
   
END
GO

GRANT EXECUTE ON [dbo].[proc_SPDelisted_get_bySPID] TO HCVU
GO
