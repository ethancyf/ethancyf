IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0033_PPPKG_Stat_Read]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0033_PPPKG_Stat_Read]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	09 Nov 2020
-- CR. No			CRE20-014-02 (GOV SIV - Phase 2)
-- Description:		() Add 2 sub report for QIV Govt and LAIV Govt by school code
--						[03-QIV (Govt)] and [03-LAIV (Govt)]
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
-- Author:			Winnie SUEN
-- Create date:		04 Oct 2019
-- CR No.			CRE19-001-05 (PPP 2019-20 - Report)
-- Description:		PPPKG daily report - Read Whole Report Result
-- =============================================  

Create PROCEDURE [dbo].[proc_EHS_eHSD0033_PPPKG_Stat_Read]     
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

DECLARE @current_scheme_Seq		INT
DECLARE @Scheme_Code			CHAR(10) = 'PPPKG'

DECLARE @Current_Scheme_desc VARCHAR(20)
DECLARE @Scheme_Display_Code VARCHAR(25)

SET @strGenDtm = FORMAT(GETDATE(), 'yyyy/MM/dd HH:mm')
SET @schemeDate = CONVERT(VARCHAR(11), DATEADD(dd, -1, @strGenDtm), 111)

EXEC @current_scheme_Seq = [proc_EHS_GetSchemeSeq_Stat] @Scheme_Code, @schemeDate    

SELECT @Current_scheme_desc = Season_Desc FROM VaccineSeason WITH (NOLOCK) WHERE Scheme_Code = @Scheme_Code AND Scheme_Seq = @current_scheme_Seq AND Subsidize_Item_Code = 'SIV'
SELECT @Scheme_Display_Code = RTRIM(Display_Code) FROM SchemeClaim WITH (NOLOCK) WHERE Scheme_Code = @Scheme_Code

INSERT INTO @ContentTable (Value1, Value2)
SELECT 'eHS(S)D0033-01', 'Report on eHealth (Subsidies) accounts created (by document type)'
INSERT INTO @ContentTable (Value1, Value2)
SELECT 'eHS(S)D0033-02', FORMATMESSAGE('Report on yearly %s claim transaction by age group and target group (%s)', @Scheme_Display_Code, @Current_scheme_desc)
INSERT INTO @ContentTable (Value1, Value2)
SELECT 'eHS(S)D0033-03', FORMATMESSAGE('Report on yearly %s claim transaction by school code (%s)', @Scheme_Display_Code, @Current_scheme_desc)
INSERT INTO @ContentTable (Value1, Value2)
SELECT 'eHS(S)D0033-04', FORMATMESSAGE('Raw Data of %s claim transactions', @Scheme_Display_Code)

INSERT INTO @ContentTable (Value1) SELECT ''
INSERT INTO @ContentTable (Value1) SELECT ''
INSERT INTO @ContentTable (Value1) SELECT 'Report Generation Time: ' + @strGenDtm    

SELECT 	ISNULL(Value1, ''),	ISNULL(Value2,'')
FROM @ContentTable  
ORDER BY
	Display_Seq

-- --------------------------------------------------    
-- From stored procedure: proc_EHS_eHSD0033_01_PrepareData
-- To Excel sheet:   eHSD0033-01: Report on eHealth (Subsidies) accounts created (by doc type)    
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
	isnull(Col19,'')

FROM         
	RpteHSD0033_01_PPPKG_eHA_ByDocType WITH (NOLOCK)    
ORDER BY    
	Display_Seq    
     
-- --------------------------------------------------  
-- From stored procedure: proc_EHS_eHSD0033_02_03_PrepareData
-- To Excel sheet:   eHSD0033-02: Report on yearly PPPKG claim transaction by age group and target group (current season)
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
	isnull(Col20,'')
FROM    
	RpteHSD0033_02_PPPKG_Tx_ByAgeGroup_ByYear WITH (NOLOCK)  
ORDER BY  
	Display_Seq	  
  
-- --------------------------------------------------  
-- From stored procedure: proc_EHS_eHSD0033_02_03_PrepareData
-- To Excel sheet:   eHSD0033-03: Report on yearly PPPKG claim transaction by school code (current season) (QIV)
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
	RpteHSD0033_03_PPPKG_QIV_Tx_BySchoolCode WITH (NOLOCK)
ORDER BY  
	Display_Seq  

-- --------------------------------------------------  
-- From stored procedure: proc_EHS_eHSD0033_02_03_PrepareData
-- To Excel sheet:   eHSD0033-03: Report on yearly PPPKG claim transaction by school code (current season) (LAIV)
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
	RpteHSD0033_03_PPPKG_LAIV_Tx_BySchoolCode WITH (NOLOCK)
ORDER BY  
	Display_Seq  

-- --------------------------------------------------  
-- From stored procedure: proc_EHS_eHSD0033_02_03_PrepareData
-- To Excel sheet:   eHSD0033-03: Report on yearly PPPKG claim transaction by school code (current season) (QIV Govt)
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
	RpteHSD0033_03_PPPKG_QIVG_Tx_BySchoolCode WITH (NOLOCK)
ORDER BY  
	Display_Seq  

-- --------------------------------------------------  
-- From stored procedure: proc_EHS_eHSD0033_02_03_PrepareData
-- To Excel sheet:   eHSD0033-03: Report on yearly PPPKG claim transaction by school code (current season) (LAIV Govt)
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
	RpteHSD0033_03_PPPKG_LAIVG_Tx_BySchoolCode WITH (NOLOCK)
ORDER BY  
	Display_Seq  

-- --------------------------------------------------  
-- From stored procedure: proc_EHS_eHSD0033_04_PrepareData  
-- To Excel sheet:   eHSD0033-04: Raw Data of PPPKG claim transactions
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
	isnull(Col20,'')
FROM       
	RpteHSD0033_04_PPPKG_Tx_Raw WITH (NOLOCK)  
ORDER BY  
	Display_Seq  
  
-- --------------------------------------------------  
-- From stored procedure: [proc_EHS_VaccineRemark_Stat_Read]  
-- To Excel sheet:   eHSD0033-Remarks: Remarks  
-- --------------------------------------------------  
 EXEC [proc_EHS_VaccineRemark_Stat_Read]  
  
END  
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0033_PPPKG_Stat_Read] TO HCVU
GO

