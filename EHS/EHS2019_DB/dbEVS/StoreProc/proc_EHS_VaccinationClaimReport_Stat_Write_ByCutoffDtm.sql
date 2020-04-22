IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_VaccinationClaimReport_Stat_Write_ByCutoffDtm]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_VaccinationClaimReport_Stat_Write_ByCutoffDtm]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	4 November 2015
-- CR No.:			CRE15-006
-- Description:		Stored procedure not used anymore. Separate to each scheme
-- =============================================
-- =============================================
-- Modification History
-- CR No. :			CRE15-005-03
-- Modified by:		Winnie SUEN
-- Modified date:	29 Jul 2015
-- Description:		(1) add new section PIDVSS
-- =============================================
-- =============================================
-- Modification History
-- CR No. :			CRE14-017-04
-- Modified by:		Winnie SUEN
-- Modified date:	27 Jan 2015
-- Description:		(1) add [proc_EHS_eHSD0002_06_EVSS23vPPVConflict_Stat] for EVSS      
--					(2) add new section OMPCV13E
-- =============================================
-- =============================================
-- Modification History
-- CR No. :			CRE13-017-05
-- Modified by:		Karl Lam
-- Modified date:	03 December 2013
-- Description:	add section CVSSPCV13
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Eric Tse
-- Modified date:	04 November 2010
-- Description:	add [proc_EHS_RVPTransaction_Stat]  
--		remvoe [proc_EHS_RVPPVTransaction_Stat]
--		remvoe [proc_EHS_RVPSIVTransaction_Stat]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Eric Tse
-- Modified date:	27 October 2010
-- Description:		(1) add [proc_EHS_eHealthAccountClaimByDocumentType_EVSS_Stat]
--					(2) add [proc_EHS_eHealthAccountClaimByDocumentType_CIVSS_Stat]	
--					(3) add [proc_EHS_EVSSTransaction_Stat]
--					(4) add [proc_EHS_CIVSSTransaction_Stat]
-- =============================================
-- =============================================  
-- Modification History  
-- Modified by:  Derek LEUNG  
-- Modified date: 17 August 2010  
-- Description:  Remove [proc_EHS_RVPHSIVCategoryReport_Stat]  
--                  Add [proc_EHS_RVPSIVCategoryReport_Stat]  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Derek LEUNG  
-- Modified date: 16 August 2010  
-- Description:  Remove [proc_EHS_RVPHSIVTransaction_Stat]  
--                  Add [proc_EHS_RVPSIVTransaction_Stat], [proc_EHS_RVPPVTransaction_Stat]  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Lawrence TSANG  
-- Modified date: 1 February 2010  
-- Description:  Modify HSIVSS section  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Lawrence TSANG  
-- Modified date: 11 January 2010  
-- Description:  (1) Exchange the order of [proc_EHS_eHealthAccount_Stat] and [proc_EHS_eHealthAccountByDocumentType_Stat]  
--     (2) Add 4 stored procedures for HSIVSS report  
-- =============================================  
-- =============================================  
-- Author:   Lawrence TSANG  
-- Create date:  15 October 2009  
-- Description:  Generate report for the Vaccination  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:  Lawrence TSANG  
-- Modified date: 21 October 2009  
-- Description:  Add two more stored procedure for scheme RVP: [proc_EHS_eHealthAccountClaimByDocumentType_RVP_Stat], [proc_EHS_RVPAgeReport_Stat]  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:    
-- Modified date:   
-- Description:    
-- =============================================  

/*  
CREATE PROCEDURE [dbo].[proc_EHS_VaccinationClaimReport_Stat_Write_ByCutoffDtm]   
 @Cutoff_Dtm datetime  
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
  
-- Vaccination Claim  
-- =============================================  
-- EVSS
-- =============================================  
  
 EXEC [proc_EHS_eHealthAccountClaimByDocumentType_EVSS_Stat] @Cutoff_Dtm  
   
 EXEC [proc_EHS_EVSSAgeReport_Stat] @Cutoff_Dtm  
  
 EXEC [proc_EHS_EVSSTransaction_Stat] @Cutoff_Dtm  
  
 EXEC [proc_EHS_eHealthAccount_Stat] @Cutoff_Dtm  

 EXEC [proc_EHS_eHealthAccountByDocumentType_Stat] @Cutoff_Dtm
 
 EXEC [proc_EHS_eHSD0002_06_EVSS23vPPVConflict_Stat] @Cutoff_Dtm

-- =============================================  
-- CIVSS
-- =============================================  
 EXEC [proc_EHS_CIVSSAgeReport_Stat] @Cutoff_Dtm 

 EXEC [proc_EHS_eHealthAccountClaimByDocumentType_CIVSS_Stat] @Cutoff_Dtm
 
 EXEC [proc_EHS_CIVSSTransaction_Stat] @Cutoff_Dtm
  
-- =============================================  
-- RVP
-- =============================================  
 EXEC [proc_EHS_eHealthAccountClaimByDocumentType_RVP_Stat] @Cutoff_Dtm

 EXEC [proc_EHS_RVPAgeReport_Stat] @Cutoff_Dtm

 EXEC [proc_EHS_RVPTransaction_Stat] @Cutoff_Dtm
 
  
-- =============================================  
-- CVSSPCV13
-- =============================================  
 EXEC [proc_EHS_eHSD0025_01_CVSSPCV13eHealthAccountClaimByDocumentType_Stat] @Cutoff_Dtm

 EXEC [proc_EHS_eHSD0025_02_CVSSPCV13AgeReport_Stat] @Cutoff_Dtm

 EXEC [proc_EHS_eHSD0025_03_CVSSPCV13Transaction_Stat] @Cutoff_Dtm

-- =============================================  
-- OMPCV13E
-- =============================================  
 EXEC [proc_EHS_eHSD0026_01_OMPCV13EeHealthAccountClaimByDocumentType_Stat] @Cutoff_Dtm

 EXEC [proc_EHS_eHSD0026_02_OMPCV13EAgeReport_Stat] @Cutoff_Dtm

 EXEC [proc_EHS_eHSD0026_03_OMPCV13ETransaction_Stat] @Cutoff_Dtm
 
-- =============================================  
-- PIDVSS
-- =============================================  
 EXEC [proc_EHS_eHSD0027_01_PIDVSSeHealthAccountClaimByDocumentType_Stat] @Cutoff_Dtm

 EXEC [proc_EHS_eHSD0027_02_PIDVSSAgeReport_Stat] @Cutoff_Dtm

 EXEC [proc_EHS_eHSD0027_03_PIDVSSTransaction_Stat] @Cutoff_Dtm
 
END       
--GO

GRANT EXECUTE ON [dbo].[proc_EHS_VaccinationClaimReport_Stat_Write_ByCutoffDtm] TO HCVU
--GO
*/