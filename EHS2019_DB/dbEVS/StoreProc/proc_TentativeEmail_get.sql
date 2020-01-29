IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TentativeEmail_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TentativeEmail_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Clark YIP
-- Create date: 22 Jun 2008
-- Description:	Get the SP_ID, tentative email (if any) of give SPID / username
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_TentativeEmail_get]
	@spid_username	varchar(20)	
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
	SELECT s.[Tentative_Email],s.SP_ID, s.tsmp, s.Enrolment_Ref_No
	FROM	ServiceProvider s, HCSPUserAC u
	WHERE
		u.SP_ID = s.SP_ID
		AND	(u.SP_ID = @spid_username OR u.Alias_account = @spid_username)
		AND s.[Tentative_Email] is not null
	
END

GO

GRANT EXECUTE ON [dbo].[proc_TentativeEmail_get] TO HCSP
GO
