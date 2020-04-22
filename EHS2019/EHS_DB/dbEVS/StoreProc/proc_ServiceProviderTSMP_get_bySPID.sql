IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderTSMP_get_bySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderTSMP_get_bySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	4 November 2014
-- CR No.:			CRE13-029
-- Description:		RSA Server Upgrade - Grand EXECUTE to HCSP
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 29 June 2008
-- Description:	Get timestamp in Table "ServiceProvider"
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProviderTSMP_get_bySPID]
	@sp_id char(8)	
AS
BEGIN
	SET NOCOUNT ON;

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

	Select	TSMP
	FROM	ServiceProvider
	WHERE	SP_ID = @sp_id
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderTSMP_get_bySPID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderTSMP_get_bySPID] TO HCSP
GO
