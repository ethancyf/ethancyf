 IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemDateTime_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemDateTime_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.			
-- Description:		
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		10 July 2017
-- CR No.:			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Description:		For checking Connection Strings
-- =============================================

CREATE PROCEDURE proc_SystemDateTime_get
AS BEGIN

	SELECT GETDATE() AS [DateTime]

END
GO

GRANT EXECUTE ON [dbo].[proc_SystemDateTime_get] TO HCVU
GO
