IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0028_VSS_Stat_Write]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0028_VSS_Stat_Write]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Dickson Law
-- Modified date:	09 Jan 2018
-- CR No.:			CRE14-016
-- Description:	    Remove store proc [proc_EHS_eHSD0028_02_PrepareData] and reorder other store proc name	
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
-- CR No.:			CRE16-002-04
-- Author:			Winnie SUEN
-- Create date:		30 Aug 2016
-- Description:		Revamp VSS
-- =============================================  
  
CREATE PROCEDURE [dbo].[proc_EHS_eHSD0028_VSS_Stat_Write]   
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
	EXEC proc_EHS_eHSD0028_01_PrepareData		 @Cutoff_Dtm
	
	EXEC proc_EHS_eHSD0028_02_03_PrepareData	 @Cutoff_Dtm
	
	EXEC proc_EHS_eHSD0028_04_PrepareData		 @Cutoff_Dtm
	
	EXEC proc_EHS_eHSD0005_02_PrepareData		 @Cutoff_Dtm
	
	EXEC proc_EHS_eHSD0005_03_PrepareData		 @Cutoff_Dtm


-- =============================================  
-- Return results  
-- =============================================
	SELECT 'S' AS [Result]

 
END       
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0028_VSS_Stat_Write] TO HCVU
GO
