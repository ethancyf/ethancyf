IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_get_TSMP]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPUserAC_get_TSMP]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Modification History
-- CR No.			CRE16-026 (Change email for locked SP)
-- Modified by:		Winnie SUEN
-- Modified date:	12 Sep 2017
-- Description:		Grant right to HCVU
-- =============================================
-- =============================================
-- Author:			Clark YIP
-- Create date:		13-08-2008
-- Description:		Get HCSPUserAC TSMP
-- =============================================

CREATE procedure [dbo].[proc_HCSPUserAC_get_TSMP]
@SP_ID			char(8)

as
BEGIN
-- =============================================
-- Return results
-- =============================================
SELECT [TSMP]      
  FROM HCSPUserAC
WHERE
SP_ID = @SP_ID

END
GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_get_TSMP] TO HCSP, HCVU
GO
