IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0026_OMPCV13E_Stat_Write]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0026_OMPCV13E_Stat_Write]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	11 Sep 2017
-- CR No.:			CRE17-003 (Stop vaccine daily stat)
-- Description:		Stored procedure is no longer used
-- =============================================
-- =============================================  
-- Author:			Lawrence TSANG
-- Create date:		4 November 2015
-- CR No.:			CRE15-006
-- Description:		Prepare data for eHSD0026 OMPCV13E Claim Report
-- =============================================  
/*
CREATE PROCEDURE [dbo].[proc_EHS_eHSD0026_OMPCV13E_Stat_Write]   
	@Cutoff_Dtm datetime = NULL
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
	IF @Cutoff_Dtm IS NULL BEGIN
		SET @Cutoff_Dtm = CONVERT(VARCHAR(10), GETDATE(), 120)
	END


-- =============================================  
-- Generate data
-- =============================================  
	EXEC [proc_EHS_eHSD0026_01_OMPCV13EeHealthAccountClaimByDocumentType_Stat] @Cutoff_Dtm

	EXEC [proc_EHS_eHSD0026_02_OMPCV13EAgeReport_Stat] @Cutoff_Dtm

	EXEC [proc_EHS_eHSD0026_03_OMPCV13ETransaction_Stat] @Cutoff_Dtm


-- =============================================  
-- Return results  
-- =============================================
	SELECT 'S' AS [Result]

 
END       
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0026_OMPCV13E_Stat_Write] TO HCVU
GO
*/
