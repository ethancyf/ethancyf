IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Token_get_byUserID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Token_get_byUserID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		6 April 2011
-- Description:		Retrieve Token by User ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_Token_get_byUserID]
	@User_ID	char(20)
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
-- Return result
-- =============================================

	SELECT
		User_ID,
		Token_Serial_No,
		Project,
		Issue_By,
		Issue_Dtm,
		ISNULL(Token_Serial_No_Replacement, '') AS [Token_Serial_No_Replacement],
		Record_Status,
		Update_By,
		Update_Dtm,
		TSMP
	FROM
		Token
	WHERE
		User_ID = @User_ID


END
GO

GRANT EXECUTE ON [dbo].[proc_Token_get_byUserID] TO WSINT
GO
