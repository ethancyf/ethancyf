IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_get_bySPIDActivationCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPUserAC_get_bySPIDActivationCode]
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
CREATE procedure [dbo].[proc_HCSPUserAC_get_bySPIDActivationCode]
@SP_ID			char(8),
@code			varchar(100)
as
BEGIN
-- =============================================
-- Return results
-- =============================================
SELECT [TSMP]      
  FROM HCSPUserAC
WHERE
SP_ID = @SP_ID
AND [Record_Status]='A'
AND [Activation_Code]=@code

END
GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_get_bySPIDActivationCode] TO HCSP
GO
