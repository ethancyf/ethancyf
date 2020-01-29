IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProviderTentativeEmail_get_bySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProviderTentativeEmail_get_bySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

/*
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	14 Nov 2016
-- CR No.			CRE16-018 (Display SP tentative email in HCVU)
-- Description:		Obsolete
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 16 July 2008
-- Description:	Get Tentative Email in Table "ServiceProvider"
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProviderTentativeEmail_get_bySPID]
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
	Select	isNull(Tentative_Email, '') as Tentative_Email
	FROM	ServiceProvider
	WHERE	SP_ID = @sp_id
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProviderTentativeEmail_get_bySPID] TO HCVU
GO
*/
