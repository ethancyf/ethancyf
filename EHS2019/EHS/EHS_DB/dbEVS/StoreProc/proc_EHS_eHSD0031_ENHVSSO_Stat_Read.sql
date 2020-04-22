IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0031_ENHVSSO_Stat_Read]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0031_ENHVSSO_Stat_Read]
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
-- Description:		ENHVSSO daily report - Read Whole Report Result
-- =============================================    

/*
Create PROCEDURE [dbo].[proc_EHS_eHSD0031_ENHVSSO_Stat_Read]     
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

-- --------------------------------------------------    
-- Content Page    
-- --------------------------------------------------  
DECLARE @ContentTable table (   
	Display_Seq		INT IDENTITY(1,1),     
	Value1			VARCHAR(100),      
	Value2			VARCHAR(100) 
)  
DECLARE @strGenDtm		VARCHAR(50)    
DECLARE @schemeDate		DATETIME
  
SET @strGenDtm = CONVERT(VARCHAR(11), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108)      
SET @strGenDtm = LEFT(@strGenDtm, LEN(@strGenDtm)-3)  
SET @schemeDate = CONVERT(VARCHAR(11), DATEADD(dd, -1, @strGenDtm), 111)

DECLARE @current_scheme_Seq				INT
EXEC @current_scheme_Seq = [proc_EHS_GetSchemeSeq_Stat] 'VSS', @schemeDate    

INSERT INTO @ContentTable (Value1, Value2)
SELECT 'eHS(S)D0031-01', 'Report on eHealth (Subsidies) accounts created (by document type)'
INSERT INTO @ContentTable (Value1, Value2)
SELECT 'eHS(S)D0031-02', REPLACE('Report on yearly ENHVSSO claim transaction by age group and target group ([DATE])', '[DATE]',  (SELECT Season_Desc FROM VaccineSeason WHERE Scheme_Code = 'VSS' AND Scheme_Seq = @current_scheme_Seq AND Subsidize_Item_Code = 'SIV' ))
INSERT INTO @ContentTable (Value1, Value2)
SELECT 'eHS(S)D0031-03', REPLACE('Report on yearly ENHVSSO claim transaction by place of vaccination ([DATE])', '[DATE]',  (SELECT Season_Desc FROM VaccineSeason WHERE Scheme_Code = 'VSS' AND Scheme_Seq = @current_scheme_Seq AND Subsidize_Item_Code = 'SIV' )) 
INSERT INTO @ContentTable (Value1, Value2)
SELECT 'eHS(S)D0031-04', 'Raw Data of ENHVSSO claim transactions' 

INSERT INTO @ContentTable (Value1) SELECT ''
INSERT INTO @ContentTable (Value1) SELECT ''
INSERT INTO @ContentTable (Value1) SELECT 'Report Generation Time: ' + @strGenDtm    

SELECT 	ISNULL(Value1, ''),	ISNULL(Value2,'')
FROM @ContentTable  


-- --------------------------------------------------    
-- From stored procedure: proc_EHS_eHSD0031_01_PrepareData
-- To Excel sheet:   eHSD0031-01: Report on eHealth (Subsidies) accounts created (by doc type)    
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
  isnull(Col12,'')   
 FROM         
  RpteHSD0031_01_ENHVSSO_eHA_ByDocType    
 ORDER BY    
  Display_Seq    
     
-- --------------------------------------------------  
-- From stored procedure: proc_EHS_eHSD0031_02_03_PrepareData
-- To Excel sheet:   eHSD0031-02: Report on yearly ENHVSSO claim transaction by age group and target group (current season)
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
  isnull(Col16,''),  
  isnull(Col17,''),  
  isnull(Col18,''),  
  isnull(Col19,''),   
  isnull(Col20,''),  
  isnull(Col21,''),  
  isnull(Col22,''),  
  isnull(Col23,''),  
  isnull(Col24,''),  
  isnull(Col25,''),  
  isnull(Col26,''),  
  isnull(Col27,''),  
  isnull(Col28,''),  
  isnull(Col29,'')    
 FROM    
  RpteHSD0031_02_ENHVSSO_Tx_ByAgeGroup_ByYear  
 ORDER BY  
  Display_Seq  
  
-- --------------------------------------------------  
-- From stored procedure: proc_EHS_eHSD0031_02_03_PrepareData
-- To Excel sheet:   eHSD0031-03: Report on cumulative yearly ENHVSSO claim transaction by place of vaccination (2018/19)
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
  RpteHSD0031_03_ENHVSSO_Tx_ByPlaceOfVaccination  
 ORDER BY  
  Display_Seq  

-- --------------------------------------------------  
-- From stored procedure: proc_EHS_eHSD0031_04_PrepareData  
-- To Excel sheet:   eHSD0031-04: Raw Data of ENHVSSO claim transactions
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
  RpteHSD0031_04_ENHVSSO_Tx_Raw  
 ORDER BY  
  Display_Seq  
  
-- --------------------------------------------------  
-- From stored procedure: [proc_EHS_VaccineRemark_Stat_Read]  
-- To Excel sheet:   eHSD0031-Remarks: Remarks  
-- --------------------------------------------------  
 EXEC [proc_EHS_VaccineRemark_Stat_Read]  
  
END  
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0031_ENHVSSO_Stat_Read] TO HCVU
GO

*/