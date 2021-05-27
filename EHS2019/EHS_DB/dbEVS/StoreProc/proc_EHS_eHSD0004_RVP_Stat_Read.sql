IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0004_RVP_Stat_Read]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0004_RVP_Stat_Read]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
 
-- =============================================
-- Modified by:		Raiman Chong
-- Modified date:	04 May 2021
-- CR. No.:			CRE20-023
-- Description:		Extends the column for [proc_EHS_eHSD0004_01_PrepareData] to add new Document Type CCIC, ROP140, PASS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	16 Jul 2020
-- CR. No			INT20-0025
-- Description:		(1) Add WITH (NOLOCK)
-- ============================================= 
 -- ============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	13 Jan 2020
-- CR No.:			INT20-0001
-- Description:		Fix content worksheet to order by [Display_Seq]
-- ============================================= 
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	11 Jan 2017
-- CR No.:			CRE16-004
-- Description:		Add Deceased Status
--					Add sub report 1 column
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
-- Modification History          
-- Modified By:  Eric Tse  
-- Modified date: 5 November 2010            
-- Description:  (1) Fix Means of input
-- =============================================   
-- =============================================    
-- Modification History    
-- Modified by:  Eric Tse    
-- Modified date: 25 October 2010    
-- Description: Combine RSIV and 23vPPV transaction to a single worksheet
-- ==========================================
-- =============================================    
-- Modification History    
-- Modified by:  Eric Tse    
-- Modified date: 25 October 2010    
-- Description: Modify the store procedure to fit with new report layout standard  
--    Change the name of store procedure from proc_EHS_VaccinationClaimReportRVP_Stat_Read  
--    (1) Build the content page  
--    (2) Add [proc_EHS_VaccineRemark_Stat_Read]    
-- =============================================    
-- =============================================    
-- Modification History    
-- Modified by:  Derek LEUNG    
-- Modified date: 15 September 2010    
-- Description:  Return date on 1st sheet. Update Template to meet new report standard    
-- =============================================    
-- =============================================    
-- Modification History    
-- Modified by:  Derek LEUNG    
-- Modified date:  16 August 2010    
-- Description:  Read RVP-PV / RVPSIV raw data    
--                    and remove reading RVP-HSIV raw data    
-- =============================================    
-- =============================================    
-- Modification History    
-- Modified by:  Derek LEUNG    
-- Modified date:  28 June 2010    
-- Description:  Read one more column for RVP-HSIV raw data (RCH Type)    
-- =============================================    
-- =============================================    
-- Modification History    
-- Modified by:  Lawrence TSANG    
-- Modified date:  15 March 2010    
-- Description:  Read one more column for RVP-HSIV raw data    
-- =============================================    
-- =============================================    
-- Modification History    
-- Modified by:  Lawrence TSANG    
-- Modified date:  4 March 2010    
-- Description:  Read one more column for RVP-HSIV raw data    
-- =============================================    
-- =============================================    
-- Modification History    
-- Modified by:  Lawrence TSANG    
-- Modified date:  29 January 2010    
-- Description:  Modify HSIVSS section    
-- =============================================    
-- =============================================    
-- Modification History    
-- Modified by:  Lawrence TSANG    
-- Modified date:  11 January 2010    
-- Description:  Read more columns for HSIVSS report    
-- =============================================    
-- =============================================    
-- Author:   Lawrence TSANG    
-- Create date:  28 October 2009    
-- Description:  Generate report for the Vaccination (RVP only)    
-- =============================================    
-- =============================================    
-- Modification History    
-- Modified by:  Lawrence TSANG    
-- Modified date: 6 November 2009    
-- Description:  Read more columns for Txn by Age (RVP)    
-- =============================================    
-- =============================================    
-- Modification History    
-- Modified by:      
-- Modified date:     
-- Description:      
-- =============================================    
    
Create PROCEDURE [dbo].[proc_EHS_eHSD0004_RVP_Stat_Read]       
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
EXEC @current_scheme_Seq = [proc_EHS_GetSchemeSeq_Stat] 'RVP', @schemeDate    

INSERT INTO @ContentTable (Value1, Value2)
SELECT 'eHS(S)D0004-01', 'Report on eHealth (Subsidies) Accounts with RVP claim transactions by document type'
INSERT INTO @ContentTable (Value1, Value2)
SELECT 'eHS(S)D0004-02', REPLACE('Report on yearly RVP transaction ([DATE])', '[DATE]',  (SELECT Season_Desc FROM VaccineSeason WITH (NOLOCK) WHERE Scheme_Code = 'RVP' AND Scheme_Seq = @current_scheme_Seq AND Subsidize_Item_Code = 'SIV'))
INSERT INTO @ContentTable (Value1, Value2)
SELECT 'eHS(S)D0004-03', 'Report on RVP transaction (by cutoff date)'
INSERT INTO @ContentTable (Value1, Value2)
SELECT 'eHS(S)D0004-04', 'Raw Data of RVP transactions' 

INSERT INTO @ContentTable (Value1) SELECT ''
INSERT INTO @ContentTable (Value1) SELECT ''
INSERT INTO @ContentTable (Value1) SELECT 'Report Generation Time: ' + @strGenDtm    

SELECT 	ISNULL(Value1, ''),	ISNULL(Value2,'')
FROM @ContentTable  
ORDER BY Display_Seq
-- --------------------------------------------------    
-- From stored procedure: proc_EHS_eHSD0004_01_PrepareData    
-- To Excel sheet:   01-eHA (RVP)    
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
	Result_Value10,    
	Result_Value11,    
	Result_Value12,
	Result_Value13,
	Result_Value14,
	Result_Value15,
	Result_Value16
FROM
	RpteHSD0004_01_eHA_RVP_Tx_ByDocType WITH (NOLOCK) 
ORDER BY    
	Display_Seq     

-- --------------------------------------------------    
-- From stored procedure: proc_EHS_eHSD0004_02_03_PrepareData    
-- To Excel sheet:   02-Txn (By Yearly) (RVP)    
-- --------------------------------------------------    
SELECT    
	IsNull(Result_Value1,''),     
	IsNull(Result_Value2,''),    
	IsNull(Result_Value3,''),    
	IsNull(Result_Value4,''),    
	IsNull(Result_Value5,''),    
	IsNull(Result_Value6,''),    
	IsNull(Result_Value7,''),    
	IsNull(Result_Value8,''),    
	IsNull(Result_Value9,''),    
	IsNull(Result_Value10,''),    
	IsNull(Result_Value11,''),    
	IsNull(Result_Value12,''),    
	IsNull(Result_Value13,''),  
	IsNull(Result_Value14,''),    
	IsNull(Result_Value15,''),  
	IsNull(Result_Value16,'')    
FROM     
	RpteHSD0004_02_RVP_Tx_ByYear WITH (NOLOCK)
ORDER BY    
	Display_Seq    

-- --------------------------------------------------    
-- From stored procedure: proc_EHS_eHSD0004_02_03_PrepareData    
-- To Excel sheet:   03-Txn by Cut-off date (RVP)    
-- --------------------------------------------------    
SELECT    
	IsNull(Result_Value1,''),     
	IsNull(Result_Value2,''),    
	IsNull(Result_Value3,''),    
	IsNull(Result_Value4,''),    
	IsNull(Result_Value5,''),    
	IsNull(Result_Value6,''),    
	IsNull(Result_Value7,''),    
	IsNull(Result_Value8,''),    
	IsNull(Result_Value9,''),    
	IsNull(Result_Value10,''),    
	IsNull(Result_Value11,''),    
	IsNull(Result_Value12,''),    
	IsNull(Result_Value13,''),  
	IsNull(Result_Value14,''),    
	IsNull(Result_Value15,''),  
	IsNull(Result_Value16,'')    
FROM  
	RpteHSD0004_03_RVP_Tx_ByCutoffDate WITH (NOLOCK)
ORDER BY    
	Display_Seq    
	
-- --------------------------------------------------    
-- From stored procedure: proc_EHS_eHSD0004_04_PrepareData    
-- To Excel sheet:   04-Txn raw (RVP)    
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
	Result_Value10,    
	Result_Value11,    
	Result_Value12,    
	Result_Value13,     
	Result_Value14,    
	Result_Value15,    
	Result_Value16   
FROM      
	RpteHSD0004_04_RVP_Tx_Raw WITH (NOLOCK)   
ORDER BY    
	Display_Seq    

-- --------------------------------------------------    
-- From stored procedure: [proc_EHS_VaccineRemark_Stat_Read]    
-- To Excel sheet:   eHSD0004-Remarks: Remarks    
-- --------------------------------------------------    
EXEC [proc_EHS_VaccineRemark_Stat_Read]    
    
END    
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0004_RVP_Stat_Read] TO HCVU
GO
