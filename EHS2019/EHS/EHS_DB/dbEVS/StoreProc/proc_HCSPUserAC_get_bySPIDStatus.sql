IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_get_bySPIDStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPUserAC_get_bySPIDStatus]
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
-- Modified by:	Tommy Cheung
-- Modified date: 18-06-2008
-- Description:	Add new parameter Record_Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	  Kathy LEE
-- Modified date: 15-07-2009
-- Description:	  Add Record Status Checking before update
-- =============================================

CREATE procedure [dbo].[proc_HCSPUserAC_get_bySPIDStatus]
@SP_ID			char(8),
@Record_Status	char(1)
as
BEGIN
-- =============================================
-- Return results
-- =============================================
if @record_status is not null 
begin
	if @record_status = 'S'
	begin
		set @record_status = null
	end
end 

SELECT [TSMP]      
  FROM HCSPUserAC
WHERE
SP_ID = @SP_ID
AND (@Record_Status is null or Record_Status=@Record_Status)

END
GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_get_bySPIDStatus] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_get_bySPIDStatus] TO HCVU
GO
