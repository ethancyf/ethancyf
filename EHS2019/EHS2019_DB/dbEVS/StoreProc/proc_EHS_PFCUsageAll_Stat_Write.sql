IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_PFCUsageAll_Stat_Write]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_PFCUsageAll_Stat_Write]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		27 October 2009
-- Description:		Generate report for the usage of PFC platform
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_PFCUsageAll_Stat_Write] 
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
	DECLARE @Cutoff_Dtm		datetime
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	SET @Cutoff_Dtm = CONVERT(varchar(11), GETDATE(), 106) + ' 00:00:00'
-- =============================================
-- Return results
-- =============================================

	EXEC [proc_EHS_PFCUsageCreate_Stat_Write_ByCutoffDtm] @Cutoff_Dtm

	EXEC [proc_EHS_PFCUsageUse_Stat_Write_ByCutoffDtm] @Cutoff_Dtm
	
END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_PFCUsageAll_Stat_Write] TO HCVU
GO
