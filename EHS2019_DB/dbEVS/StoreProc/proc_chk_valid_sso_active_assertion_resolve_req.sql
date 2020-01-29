 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_chk_valid_sso_active_assertion_resolve_req]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_chk_valid_sso_active_assertion_resolve_req]
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
-- Description:	Check if an Assertion Resolve Requet is valid
--				        the asertion must exist in the persistent storage and the read count should be 0
-- =============================================

CREATE PROCEDURE [dbo].[proc_chk_valid_sso_active_assertion_resolve_req] 
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
	COUNT(*) chk_rst
FROM
	sso_active_assertion
WHERE
	artifact=@v_in_artifact AND 
	read_count=0 AND
	creation_datetime >= Dateadd(d, -1, GETDATE())

END 

GO

GRANT EXECUTE ON [dbo].[proc_chk_valid_sso_active_assertion_resolve_req] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_chk_valid_sso_active_assertion_resolve_req] TO HCVU
GO
