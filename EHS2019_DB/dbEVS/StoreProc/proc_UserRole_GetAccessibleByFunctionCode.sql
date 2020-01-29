IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_UserRole_GetAccessibleByFunctionCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_UserRole_GetAccessibleByFunctionCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		24 May 2011
-- CR No.:			CRE11-007
-- Description:		Get the users which are accessible to a certain function
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_UserRole_GetAccessibleByFunctionCode]
	@Function_Code		char(6)
AS BEGIN

-- =============================================
-- Return results
-- =============================================
	SELECT DISTINCT
		U.User_ID
	FROM
		UserRole U
			INNER JOIN RoleSecurity S
				ON U.Role_Type = S.Role_Type
	WHERE
		S.Function_Code = @Function_Code
	ORDER BY
		U.User_ID


END
GO

GRANT EXECUTE ON [dbo].[proc_UserRole_GetAccessibleByFunctionCode] TO HCVU
GO
