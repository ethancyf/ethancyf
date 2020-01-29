IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0032_PPP_Stat_Write]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0032_PPP_Stat_Write]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================  
-- =============================================
-- CR No.:			CRE17-018-06
-- Author:			Winnie SUEN
-- Create date:		3 Oct 2018
-- Description:		PPP daily report - Write Whole Report 
-- =============================================    
  
CREATE PROCEDURE [dbo].[proc_EHS_eHSD0032_PPP_Stat_Write]   
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
	EXEC proc_EHS_eHSD0032_01_PrepareData		 @Cutoff_Dtm
	
	EXEC proc_EHS_eHSD0032_02_03_PrepareData	 @Cutoff_Dtm
	
	EXEC proc_EHS_eHSD0032_04_PrepareData		 @Cutoff_Dtm

-- =============================================  
-- Return results  
-- =============================================
	SELECT 'S' AS [Result]

 
END       
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0032_PPP_Stat_Write] TO HCVU
GO
