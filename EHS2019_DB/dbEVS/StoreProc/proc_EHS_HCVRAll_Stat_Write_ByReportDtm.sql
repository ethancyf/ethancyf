IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_HCVRAll_Stat_Write_ByReportDtm]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_HCVRAll_Stat_Write_ByReportDtm]
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

CREATE PROCEDURE [dbo].[proc_EHS_HCVRAll_Stat_Write_ByReportDtm] 
	@Report_Dtm	datetime
AS BEGIN
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

	EXEC [proc_EHS_HCVR_Stat_Write_ByReportDtm] @Report_Dtm

	EXEC [proc_EHS_HCVRIVRS_Stat_Write_ByReportDtm] @Report_Dtm
	
END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_HCVRAll_Stat_Write_ByReportDtm] TO HCVU
GO
