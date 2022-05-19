IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0004_RVP_Stat_Write]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0004_RVP_Stat_Write]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	21 April 2022
-- CR No.:			CRE21-022
-- Description:		Add sub-report for voidded COVID-19 records
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	3 August 2017
-- CR No.:			CRE16-026
-- Description:		Add PCV13
--					Change Temp Table Name
-- =============================================  
-- =============================================  
-- Author:			Lawrence TSANG
-- Create date:		4 November 2015
-- CR No.:			CRE15-006
-- Description:		Prepare data for eHSD0004 RVP Claim Report
-- =============================================  
  
CREATE PROCEDURE [dbo].[proc_EHS_eHSD0004_RVP_Stat_Write]   
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
	EXEC [proc_EHS_eHSD0004_01_PrepareData] @Cutoff_Dtm

	EXEC [proc_EHS_eHSD0004_02_03_PrepareData] @Cutoff_Dtm

	EXEC [proc_EHS_eHSD0004_04_PrepareData] @Cutoff_Dtm

	EXEC [proc_EHS_eHSD0004_05_PrepareData] @Cutoff_Dtm


-- =============================================  
-- Return results  
-- =============================================
	SELECT 'S' AS [Result]

 
END       
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0004_RVP_Stat_Write] TO HCVU
GO
