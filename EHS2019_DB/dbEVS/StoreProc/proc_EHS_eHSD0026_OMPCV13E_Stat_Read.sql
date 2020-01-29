IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0026_OMPCV13E_Stat_Read]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0026_OMPCV13E_Stat_Read]
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
-- CR No.:			CRE14-017-04
-- Author:			Winnie SUEN
-- Create date:		27 Jan 2015
-- Description:		Newly added to select resultset for new stat for OMPCV13E scheme
-- =============================================  

-- exec [proc_EHS_eHSD0026_OMPCV13E_Stat_Read]
/*
Create PROCEDURE [dbo].[proc_EHS_eHSD0026_OMPCV13E_Stat_Read]     
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
-- From stored procedure: proc_EHS_eHSD0026_01_OMPCV13EeHealthAccountClaimByDocumentType_Stat  
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
  isnull(Col10,'')
 FROM  
  RpteHSD0026OMPCV13EeHealthAccountByDocumentTypeStat  
 ORDER BY  
  Display_Seq   
-- --------------------------------------------------  
-- From stored procedure: proc_EHS_eHSD0026_02_OMPCV13EAgeReport_Stat   
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
  isnull(Col10,'')
 FROM    
  RpteHSD0026OMPCV13EAgeReportStat  
 ORDER BY  
  Display_Seq  

-- --------------------------------------------------  
-- From stored procedure: proc_EHS_eHSD0026_03_OMPCV13ETransaction_Stat  
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
  isnull(Col14,''),
  isnull(Col15,''),
  isnull(Col16,'')
 FROM       
  RpteHSD0026OMPCV13ETransactionStat  
 ORDER BY  
  Display_Seq  
  
-- --------------------------------------------------  
-- From stored procedure: [proc_EHS_VaccineRemark_Stat_Read]  
-- To Excel sheet:   eHSD0026-Remarks: Remarks  
-- --------------------------------------------------  
 EXEC [proc_EHS_VaccineRemark_Stat_Read]  
  
END  
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0026_OMPCV13E_Stat_Read] TO HCVU
GO
*/

