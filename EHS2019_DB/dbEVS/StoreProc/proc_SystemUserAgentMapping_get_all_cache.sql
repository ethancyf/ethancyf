IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemUserAgentMapping_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemUserAgentMapping_get_all_cache]
GO

SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	19 October 2016
-- CR No.:			CRE16-019 (To implement token sharing between eHS(S) and eHRSS)
-- Description:		- Grant to WSEXT
--					- Add more fields
-- =============================================
-- =============================================
-- Modification History
-- Created by		Chris YIM
-- Created date	20 Oct 2016
-- CR No.			I-CRE16-006
-- Description		Capture detail client browser and OS information
-- =============================================
CREATE PROCEDURE [dbo].[proc_SystemUserAgentMapping_get_all_cache] 
AS
BEGIN

	SELECT
		UA_Info_ID,
		UA_Info_Type,
		UA_Info_RegEx,
		UA_Info_Output_Result
	FROM 
		[dbo].[SystemUserAgentMapping]
	ORDER BY
		UA_Info_ID

END
GO

GRANT EXECUTE ON [dbo].[proc_SystemUserAgentMapping_get_all_cache] TO HCSP, HCVU, HCPUBLIC, WSINT, WSEXT
GO


