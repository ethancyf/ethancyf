IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_get_NotActivated_BySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPUserAC_get_NotActivated_BySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Winnie SUEN
-- CR No.		CRE16-026 (Change email for locked SP)
-- Create date: 12 Sep 2017
-- Description:	Get the SP if it's not yet activated (password is null)
-- =============================================
-- =============================================
-- Modification History
-- CR No.			
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
CREATE PROCEDURE [dbo].[proc_HCSPUserAC_get_NotActivated_BySPID]
	@SP_ID			char(8)
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
	FROM HCSPUserAC
	WHERE
		sp_id=@SP_ID 
		AND ISNULL(SP_Password,'') = ''
END
GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_get_NotActivated_BySPID] TO HCSP, HCVU
GO
