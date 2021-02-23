IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeDocType_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeDocType_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Modification History  
-- Modified by:   Raiman
-- Modified date:  20 Jan 2021  
-- Description:   Change AgeLimit of schemeDocTypeTable for Covid19 Scheme Visa Doctype 
-- ============================================= 
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- ============================================= 
-- =============================================  
-- Modification History  
-- Modified by:   Pak Ho LEE  
-- Modified date:  11 Oct 2010  
-- Description:   Retrieve the Age Limit from DocType table (single storeage for age limit)  
-- =============================================  
-- =============================================  
-- Author:  Pak Ho LEE  
-- Create date: 13 Aug 2009  
-- Description: Retrieve all Scheme(Claim) - Document Relation  
-- =============================================  
-- =============================================  
-- Modification History  
-- Modified by:    
-- Modified date:   
-- Description:    
-- =============================================  



CREATE PROCEDURE [dbo].[proc_SchemeDocType_get_all_cache]   
AS  
BEGIN  
  
 SET NOCOUNT ON;  
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
  
SELECT  
	SDT.[Scheme_Code],  
	SDT.[Doc_Code],  
	SDT.[Major_Doc],  
	[Age_LowerLimit] = CASE WHEN LTRIM(RTRIM(SDT.[Override_Age_Limit])) = 'Y' THEN SDT.[Age_LowerLimit] ELSE DT.[Age_LowerLimit] END,
	[Age_LowerLimitUnit] = CASE WHEN LTRIM(RTRIM(SDT.[Override_Age_Limit])) = 'Y' THEN SDT.[Age_LowerLimitUnit] ELSE DT.[Age_LowerLimitUnit] END, 
	[Age_UpperLimit] = CASE WHEN LTRIM(RTRIM(SDT.[Override_Age_Limit])) = 'Y' THEN SDT.[Age_UpperLimit] ELSE DT.[Age_UpperLimit] END, 
	[Age_UpperLimitUnit] = CASE WHEN LTRIM(RTRIM(SDT.[Override_Age_Limit])) = 'Y' THEN SDT.[Age_UpperLimitUnit] ELSE DT.[Age_UpperLimitUnit] END,
	[Age_CalMethod] = CASE WHEN LTRIM(RTRIM(SDT.[Override_Age_Limit])) = 'Y' THEN SDT.[Age_CalMethod] ELSE DT.[Age_CalMethod] END
FROM  
	[SchemeDocType] SDT WITH(NOLOCK)
		INNER JOIN [DocType] DT  WITH(NOLOCK)
			ON SDT.Doc_Code = DT.Doc_Code
    
END  
Go

GRANT EXECUTE ON [dbo].[proc_SchemeDocType_get_all_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SchemeDocType_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SchemeDocType_get_all_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SchemeDocType_get_all_cache] TO WSEXT
GO