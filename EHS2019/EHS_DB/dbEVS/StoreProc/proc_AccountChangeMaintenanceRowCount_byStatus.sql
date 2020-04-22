IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_AccountChangeMaintenanceRowCount_byStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_AccountChangeMaintenanceRowCount_byStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 03 July 2008
-- Description:	Get the no of record in "SPAccountMaintenance"
--				which is in Active Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_AccountChangeMaintenanceRowCount_byStatus]
	@Record_Status char(1)
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

	SELECT	COUNT(1)
	FROM	SPAccountMaintenance
	WHERE	Record_Status = @Record_Status
END
GO

GRANT EXECUTE ON [dbo].[proc_AccountChangeMaintenanceRowCount_byStatus] TO HCVU
GO
