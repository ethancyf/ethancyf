 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_upd_sso_active_assertion_read_count]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_upd_sso_active_assertion_read_count]
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
-- Description:	increae the readcount of a particular assertion by 1 
-- =============================================
CREATE PROCEDURE [dbo].[proc_upd_sso_active_assertion_read_count] 
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


--increae the readcount by 1 
UPDATE
	sso_active_assertion
SET
	read_count=read_count+1
WHERE
	artifact=@v_in_artifact;

END 

GO

GRANT EXECUTE ON [dbo].[proc_upd_sso_active_assertion_read_count] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_upd_sso_active_assertion_read_count] TO HCVU
GO