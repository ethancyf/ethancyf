IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Interface_SystemResource_Get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Interface_SystemResource_Get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		5 October 2010
-- Description:		Get SystemResource
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_Interface_SystemResource_Get]
	@Platform	char(2)
AS BEGIN
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
		ObjectType,
		ObjectName,
		Description,
		Chinese_Description,
		Record_Status,
		Platform,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By
	FROM
		SystemResource
	WHERE
		(Platform = @Platform
				OR Platform = '99')
			AND Record_Status = 'A'

END
GO

GRANT EXECUTE ON [dbo].[proc_Interface_SystemResource_Get] TO WSEXT
GO
