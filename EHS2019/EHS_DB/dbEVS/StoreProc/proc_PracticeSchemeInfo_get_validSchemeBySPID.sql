IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeSchemeInfo_get_validSchemeBySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeSchemeInfo_get_validSchemeBySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Paul YIP
-- Create date: 29 June 2009
-- Description:	Check if the SP has valid record of specified scheme in Practice Scheme Info
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	  
-- Modified date: 
-- Description:	  
-- =============================================
CREATE PROCEDURE [dbo].[proc_PracticeSchemeInfo_get_validSchemeBySPID]
	@SP_ID			char(8),
	@SCHEME_CODE	char(10)
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
	SELECT count(1)
	FROM PracticeSchemeInfo
	where 
	sp_id=@SP_ID and Scheme_Code = @SCHEME_CODE
	and (record_status  not in ('D','I') or record_status is null)

END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInfo_get_validSchemeBySPID] TO HCSP
GO
