IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0033_PPPKG_Stat_Write]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0033_PPPKG_Stat_Write]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- ============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.			
-- Description:		
-- =============================================
-- =============================================    
-- Author:			Winnie SUEN
-- Create date:		04 Oct 2019
-- CR No.			CRE19-001-05 (PPP 2019-20 - Report)
-- Description:		PPPKG daily report - Write Whole Report Result
-- =============================================  
  
CREATE PROCEDURE [dbo].[proc_EHS_eHSD0033_PPPKG_Stat_Write]   
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
	EXEC proc_EHS_eHSD0033_01_PrepareData		 @Cutoff_Dtm
	
	EXEC proc_EHS_eHSD0033_02_03_PrepareData	 @Cutoff_Dtm
	
	EXEC proc_EHS_eHSD0033_04_PrepareData		 @Cutoff_Dtm

-- =============================================  
-- Return results  
-- =============================================
	SELECT 'S' AS [Result]

 
END       
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0033_PPPKG_Stat_Write] TO HCVU
GO
