IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0025_CVSSPCV13_Stat_Read]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0025_CVSSPCV13_Stat_Read]
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
-- CR No.: CRE13-017-05
-- Author:  Karl LAM
-- Create date: 03 DEC 2013  
-- Description:  Newly added to select resultset for new stat for CVSSPCV13 scheme
-- =============================================  

-- exec [proc_EHS_eHSD0025_CVSSPCV13_Stat_Read]
/*
Create PROCEDURE [dbo].[proc_EHS_eHSD0025_CVSSPCV13_Stat_Read]     
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
-- From stored procedure: proc_EHS_eHSD0025_01_CVSSPCV13eHealthAccountClaimByDocumentType_Stat  
-- --------------------------------------------------  
 SELECT   
  Result_Value1,  
  Result_Value2,  
  Result_Value3,  
  Result_Value4,  
  Result_Value5,  
  Result_Value6,  
  Result_Value7,  
  Result_Value8,  
  Result_Value9,  
  Result_Value10
 FROM  
  RpteHSD0025CVSSPCV13eHealthAccountByDocumentTypeStat  
 ORDER BY  
  Result_Seq   
  
-- --------------------------------------------------  
-- From stored procedure: proc_EHS_eHSD0025_02_CVSSPCV13AgeReport_Stat   
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
  RpteHSD0025CVSSPCV13AgeReportStat  
 ORDER BY  
  Display_Seq  
  
-- --------------------------------------------------  
-- From stored procedure: proc_EHS_eHSD0025_03_CVSSPCV13Transaction_Stat  
-- --------------------------------------------------  
 SELECT  
  isnull(Result_Value1,''),    
  isnull(Result_Value2,''),    
  isnull(Result_Value3,''),    
  isnull(Result_Value4,''),    
  isnull(Result_Value5,''),    
  isnull(Result_Value6,''),    
  isnull(Result_Value7,''),    
  isnull(Result_Value8,''),    
  isnull(Result_Value9,''),    
  isnull(Result_Value10,''),    
  isnull(Result_Value11,''),    
  isnull(Result_Value12,''),    
  isnull(Result_Value13,''),     
  isnull(Result_Value14,''),
  isnull(Result_Value15,''),
  isnull(Result_Value16,'')
 FROM       
  RpteHSD0025CVSSPCV13TransactionStat  
 ORDER BY  
  Result_Seq  
  
-- --------------------------------------------------  
-- From stored procedure: [proc_EHS_VaccineRemark_Stat_Read]  
-- To Excel sheet:   eHSD0025-Remarks: Remarks  
-- --------------------------------------------------  
 EXEC [proc_EHS_VaccineRemark_Stat_Read]  
  
END  
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0025_CVSSPCV13_Stat_Read] TO HCVU
GO
*/
