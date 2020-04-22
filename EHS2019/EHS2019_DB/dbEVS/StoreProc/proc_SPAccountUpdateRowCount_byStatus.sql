IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SPAccountUpdateRowCount_byStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SPAccountUpdateRowCount_byStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 25 June 2008
-- Description:	Get the no of record in "ServiceProvdierStaging"
--				which is in Data Entry Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_SPAccountUpdateRowCount_byStatus]
	@progress_status char(1)
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
	FROM	SPAccountUpdate
	WHERE	Progress_Status = @progress_status
END
GO

GRANT EXECUTE ON [dbo].[proc_SPAccountUpdateRowCount_byStatus] TO HCVU
GO
