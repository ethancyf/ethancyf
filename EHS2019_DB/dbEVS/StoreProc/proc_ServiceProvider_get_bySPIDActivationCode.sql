IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_get_bySPIDActivationCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProvider_get_bySPIDActivationCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:			Clark YIP
-- Create date:		26-11-2008
-- Description:		Get ServiceProvider TSMP
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   23 Jan 2009
-- Description:	    Add the logic to join HCSPUserAC with the status=A only
-- =============================================
CREATE procedure [dbo].[proc_ServiceProvider_get_bySPIDActivationCode]
@SP_ID			char(8),
@code			varchar(100)
as
BEGIN
-- =============================================
-- Return results
-- =============================================

SELECT sp.[TSMP]
  FROM ServiceProvider sp, HCSPUserAC a
WHERE
sp.sp_id=a.sp_id
AND sp.SP_ID = @SP_ID
AND sp.[Record_Status]='A'
AND a.Record_status='A'
AND sp.[Activation_Code]=@code

END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_get_bySPIDActivationCode] TO HCSP
GO
