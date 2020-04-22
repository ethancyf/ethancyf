IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0027_PIDVSS_Stat_Read]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0027_PIDVSS_Stat_Read]
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
-- CR No.:			CRE15-005-04
-- Author:			Winnie SUEN
-- Create date:		26 Aug 2015
-- Description:		Newly added to select resultset for new stat for PIDVSS scheme
-- =============================================  

-- exec [proc_EHS_eHSD0027_PIDVSS_Stat_Read]
/*
Create PROCEDURE [dbo].[proc_EHS_eHSD0027_PIDVSS_Stat_Read]     
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
Declare @strGenDtm varchar(50)    
SET @strGenDtm = CONVERT(VARCHAR(11), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108)    
SET @strGenDtm = LEFT(@strGenDtm, LEN(@strGenDtm)-3)    
SELECT 'Report Generation Time: ' + @strGenDtm  
-- --------------------------------------------------  
-- From stored procedure: proc_EHS_eHSD0027_01_PIDVSSeHealthAccountClaimByDocumentType_Stat  
-- --------------------------------------------------  
 SELECT   
  isnull(Col1,''),  
  isnull(Col2,''),  
  isnull(Col3,''),  
  isnull(Col4,''),  
  isnull(Col5,''),  
  isnull(Col6,''),  
  isnull(Col7,''),  
  isnull(Col8,''),  
  isnull(Col9,''),  
  isnull(Col10,''),
  isnull(Col11,'')
 FROM  
  RpteHSD0027PIDVSSeHealthAccountByDocumentTypeStat  
 ORDER BY  
  Display_Seq   
-- --------------------------------------------------  
-- From stored procedure: proc_EHS_eHSD0027_02_PIDVSSAgeReport_Stat   
-- --------------------------------------------------  
 SELECT    
  isnull(Col1,''),  
  isnull(Col2,''),  
  isnull(Col3,''),  
  isnull(Col4,''),  
  isnull(Col5,''),  
  isnull(Col6,''),  
  isnull(Col7,''),  
  isnull(Col8,''),  
  isnull(Col9,''),  
  isnull(Col10,''),
  isnull(Col11,'')
 FROM    
  RpteHSD0027PIDVSSAgeReportStat  
 ORDER BY  
  Display_Seq  

-- --------------------------------------------------  
-- From stored procedure: proc_EHS_eHSD0027_03_PIDVSSTransaction_Stat  
-- --------------------------------------------------  
 SELECT  
  isnull(Col1,''),  
  isnull(Col2,''),  
  isnull(Col3,''),  
  isnull(Col4,''),  
  isnull(Col5,''),  
  isnull(Col6,''),  
  isnull(Col7,''),  
  isnull(Col8,''),  
  isnull(Col9,''),  
  isnull(Col10,''),
  isnull(Col11,''),
  isnull(Col12,''),
  isnull(Col13,''),
  isnull(Col14,'')
 FROM       
  RpteHSD0027PIDVSSTransactionStat  
 ORDER BY  
  Display_Seq  
  
-- --------------------------------------------------  
-- From stored procedure: [proc_EHS_VaccineRemark_Stat_Read]  
-- To Excel sheet:   eHSD0027-Remarks: Remarks  
-- --------------------------------------------------  
 EXEC [proc_EHS_VaccineRemark_Stat_Read]  
  
END  
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0027_PIDVSS_Stat_Read] TO HCVU
GO
*/
