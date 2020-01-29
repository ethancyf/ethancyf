IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TokenDeactivated_get_byUserID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TokenDeactivated_get_byUserID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No:			CRE14-002 - PPI-ePR Migration
-- Modified by:		Tommy LAM
-- Modified date:	25 Mar 2014
-- Description:		Add Column -	[TokenDeactivated].[Project]
--									[TokenDeactivated].[Is_Share_Token]
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		22 July 2009
-- Description:		Get Information from Token Deactivated table
-- =============================================

CREATE PROCEDURE [dbo].[proc_TokenDeactivated_get_byUserID]
	@User_ID	char(20)
AS
BEGIN
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
	SELECT		[User_ID], 
				Token_Serial_No,
				Deactivate_Dtm,
				Deactivate_By,
				Remark,
				Project,
				Is_Share_Token
	FROM		TokenDeactivated
	WHERE		[User_ID] = @User_ID 
	ORDER BY	Deactivate_Dtm DESC

END 
GO

GRANT EXECUTE ON [dbo].[proc_TokenDeactivated_get_byUserID] TO HCVU
GO
