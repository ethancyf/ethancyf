IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0031_ENHVSSO_Stat_Write]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0031_ENHVSSO_Stat_Write]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	04 Nov 2019
-- CR No.:			CRE19-016 (End of ENHVSSO daily report)
-- Description:		Stored procedure is no longer used
-- ============================================= 
-- =============================================
-- CR No.:			CRE17-018-05
-- Author:			Koala CHENG
-- Create date:		27 Sep 2018
-- Description:		ENHVSSO daily report - Write Whole Report 
-- =============================================    

/*
CREATE PROCEDURE [dbo].[proc_EHS_eHSD0031_ENHVSSO_Stat_Write]   
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
	EXEC proc_EHS_eHSD0031_01_PrepareData		 @Cutoff_Dtm
	
	EXEC proc_EHS_eHSD0031_02_03_PrepareData	 @Cutoff_Dtm
	
	EXEC proc_EHS_eHSD0031_04_PrepareData		 @Cutoff_Dtm

-- =============================================  
-- Return results  
-- =============================================
	SELECT 'S' AS [Result]

 
END       
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0031_ENHVSSO_Stat_Write] TO HCVU
GO
*/