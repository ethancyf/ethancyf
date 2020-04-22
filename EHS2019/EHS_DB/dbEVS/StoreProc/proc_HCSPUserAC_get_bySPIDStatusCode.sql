IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_get_bySPIDStatusCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPUserAC_get_bySPIDStatusCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:			Clark YIP
-- Create date:		12-06-2008
-- Description:		Get HCSPUserAC TSMP
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE procedure [dbo].[proc_HCSPUserAC_get_bySPIDStatusCode]
@SP_ID			char(8),
@code			varchar(100)
as
BEGIN
-- =============================================
-- Return results
-- =============================================
SELECT [TSMP]      
  FROM HCSPUserAC
where (SP_ID = @SP_ID AND record_status='P' 
AND Activation_code = @code)

END
GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_get_bySPIDStatusCode] TO HCSP
GO
