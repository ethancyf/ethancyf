 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_get_sso_active_assertion]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_get_sso_active_assertion]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			    
-- Modified by:		
-- Modified date:	    
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT12-0008
-- Modified by:	Tommy TSE
-- Modified date:	29 Aug 2012
-- Description:	Rectify stored procedure permission
-- =============================================
-- =============================================
-- Author:		    Paul Yip
-- Create date:   29 Dec 2009
-- Description:	Retrieve SSO active assertion by artifact
-- =============================================


CREATE PROCEDURE [dbo].[proc_get_sso_active_assertion] 
	@v_in_artifact VARCHAR(255)
AS
BEGIN

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
		txn_id,
		artifact,
		assertion,
		read_count,
		creation_datetime
	FROM
		sso_active_assertion
	WHERE
		artifact=@v_in_artifact AND
		creation_datetime >= Dateadd(d, -1, GETDATE())


END 

GO

GRANT EXECUTE ON [dbo].[proc_get_sso_active_assertion] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_get_sso_active_assertion] TO HCVU
GO
