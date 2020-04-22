IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeDocType_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeDocType_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Modification History
-- Modified by:			Pak Ho LEE
-- Modified date:		11 Oct 2010
-- Description:			Retrieve the Age Limit from DocType table (single storeage for age limit)
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 13 Aug 2009
-- Description:	Retrieve all Scheme(Claim) - Document Relation
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
 DT.[Age_LowerLimit],  
 DT.[Age_LowerLimitUnit],  
 DT.[Age_UpperLimit],  
 DT.[Age_UpperLimitUnit],  
 DT.[Age_CalMethod]  
FROM  
 [SchemeDocType] SDT INNER JOIN [DocType] DT ON SDT.Doc_Code = DT.Doc_Code

/*
SELECT
	[Scheme_Code],
	[Doc_Code],
	[Major_Doc],
	[Age_LowerLimit],
	[Age_LowerLimitUnit],
	[Age_UpperLimit],
	[Age_UpperLimitUnit],
	[Age_CalMethod]
FROM
	[SchemeDocType]
*/

END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeDocType_get_all_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SchemeDocType_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SchemeDocType_get_all_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SchemeDocType_get_all_cache] TO WSEXT
GO