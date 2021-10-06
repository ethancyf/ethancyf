IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0028_VSS_Stat_Read]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0028_VSS_Stat_Read]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Martin Tang
-- Modified date:	08 Sep 2021
-- CR. No			CRE21-010
-- Description:		Add select column in sheet 1
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	16 Jul 2020
-- CR. No			INT20-0025
-- Description:		(1) Add WITH (NOLOCK)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	24 Mar 2020
-- CR No.:			INT20-0005
-- Description:		Fix incorrect display order
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	14 May 2018
-- CR No.:			CRE17-010
-- Description:	    OCSSS Integration
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Dickson Law
-- Modified date:	09 Jan 2018
-- CR No.:			CRE14-016
-- Description:	    Remove sheet eHSD0028-02 and reorder other table name	
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

-- exec [proc_EHS_eHSD0028_VSS_Stat_Read]

Create PROCEDURE [dbo].[proc_EHS_eHSD0028_VSS_Stat_Read]     
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
SELECT 'eHS(S)D0028-01', 'Report on eHealth (Subsidies) accounts created (by document type)'
INSERT INTO @ContentTable (Value1, Value2)
SELECT 'eHS(S)D0028-02', REPLACE('Report on yearly VSS claim transaction by age group and target group ([DATE])', '[DATE]',  (SELECT Season_Desc FROM VaccineSeason WITH (NOLOCK) WHERE Scheme_Code = 'VSS' AND Scheme_Seq = @current_scheme_Seq AND Subsidize_Item_Code = 'SIV' ))
INSERT INTO @ContentTable (Value1, Value2)
SELECT 'eHS(S)D0028-03', 'Report on cumulative VSS claim transaction by age group and target group' 
INSERT INTO @ContentTable (Value1, Value2)
SELECT 'eHS(S)D0028-04', 'Raw Data of VSS claim transactions' 

INSERT INTO @ContentTable (Value1) SELECT ''
INSERT INTO @ContentTable (Value1) SELECT ''
INSERT INTO @ContentTable (Value1) SELECT 'Report Generation Time: ' + @strGenDtm    

SELECT 	ISNULL(Value1, ''),	ISNULL(Value2,'')
FROM @ContentTable  
ORDER BY
	Display_Seq

-- --------------------------------------------------    
-- From stored procedure: proc_EHS_eHSD0028_01_PrepareData
-- To Excel sheet:   eHSD0028-01: Report on eHealth (Subsidies) accounts created (by doc type)    
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
  RpteHSD0028_01_eHA_ByDocType WITH (NOLOCK)    
 ORDER BY    
  Display_Seq    
     
-- --------------------------------------------------  
-- From stored procedure: proc_EHS_eHSD0028_03_04_PrepareData
-- To Excel sheet:   eHSD0028-02: Report on yearly VSS claim transaction by age group and target group (current season)
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
  RpteHSD0028_02_VSS_Tx_ByAgeGroup_ByYear WITH (NOLOCK)  
 ORDER BY  
  Display_Seq  
  
-- --------------------------------------------------  
-- From stored procedure: proc_EHS_eHSD0028_03_04_PrepareData
-- To Excel sheet:   eHSD0028-03: Report on cumulative VSS claim transaction by age group and target group
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
  RpteHSD0028_03_VSS_Tx_ByAgeGroup_Cumulative WITH (NOLOCK)  
 ORDER BY  
  Display_Seq  

-- --------------------------------------------------  
-- From stored procedure: proc_EHS_eHSD0028_05_PrepareData  
-- To Excel sheet:   eHSD0028-04: Raw Data of VSS claim transactions
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
  RpteHSD0028_04_VSS_Tx_Raw WITH (NOLOCK)  
 ORDER BY  
  Display_Seq  
  
-- --------------------------------------------------  
-- From stored procedure: [proc_EHS_VaccineRemark_Stat_Read]  
-- To Excel sheet:   eHSD0028-Remarks: Remarks  
-- --------------------------------------------------  
 EXEC [proc_EHS_VaccineRemark_Stat_Read]  
  
END  
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0028_VSS_Stat_Read] TO HCVU
GO

