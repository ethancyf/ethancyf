IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemVariable_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemVariable_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		19 October 2016
-- CR No.:			CRE16-019 (To implement token sharing between eHS(S) and eHRSS)
-- Description:		Get SystemVariable
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_SystemVariable_get]
	@Variable_ID	varchar(50)
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
-- Process
-- =============================================

	SELECT
		Variable_ID,
		Variable_Value,
		Update_Dtm,
		Update_By,
		TSMP
	FROM
		SystemVariable
	WHERE
		Variable_ID = @Variable_ID


END
GO

GRANT EXECUTE ON [dbo].[proc_SystemVariable_get] TO HCVU, HCSP, WSEXT
GO
