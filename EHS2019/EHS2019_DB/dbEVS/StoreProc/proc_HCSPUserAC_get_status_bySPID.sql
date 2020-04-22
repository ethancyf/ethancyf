IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_get_status_bySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPUserAC_get_status_bySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE16-004 (Enable SP to unlock account)
-- Modified by:	    Winnie SUEN
-- Modified date:   22 Dec 2017
-- Description:		Add [IVRS_Locked]
-- =============================================
-- =============================================
-- Author:			Clark YIP
-- Create date:		07-08-2008
-- Description:		Get HCSPUserAC Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	13-11-2008
-- Description:		Define the column name
-- =============================================
CREATE procedure [dbo].[proc_HCSPUserAC_get_status_bySPID]
@SP_ID			char(8)

as
BEGIN
-- =============================================
-- Return results
-- =============================================
SELECT 
	a.Record_Status as UserAcc_RecordStatus, 
	sp.Record_status as SP_RecordStatus,
	ISNULL(a.IVRS_Locked,'') as UserAcc_IVRS_Locked
FROM HCSPUserAC a, ServiceProvider sp
WHERE
a.SP_ID = @SP_ID
AND a.SP_ID=sp.SP_ID

END
GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_get_status_bySPID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_get_status_bySPID] TO HCVU
GO
