IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_HCVRAll_Stat_Write]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_HCVRAll_Stat_Write]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		27 October 2009
-- Description:		Generate report for HCVR platform (Both web and IVRS)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_HCVRAll_Stat_Write] 
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
	DECLARE @Report_Dtm		datetime
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	SET @Report_Dtm = GETDATE()
-- =============================================
-- Return results
-- =============================================

	EXEC [proc_EHS_HCVRAll_Stat_Write_ByReportDtm] @Report_Dtm
	
END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_HCVRAll_Stat_Write] TO HCVU
GO
