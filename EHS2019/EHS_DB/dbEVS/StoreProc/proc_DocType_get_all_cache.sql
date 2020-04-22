IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DocType_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DocType_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	17 March 2015
-- CR No.:			CRE13-019-02
-- Description:		Extend HCVS to China
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	9 December 2014
-- CR No.:			CRE13-019-01
-- Description:		Support Simplified Chinese
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	17 May 2011
-- CR No.:			CRE11-007
-- Description:		Retrieve [Death_Record_Available]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	9 August 2010
-- Description:		1. Remove [Vaccination_Record_Available]
--					2. Add Back [Vaccination_Record_Available]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Derek LEUNG
-- Modified date:	6 August 2010
-- Description:		Retrieve --[Age_LowerLimit],
--							 --[Age_LowerLimitUnit],
--							 --[Age_UpperLimit],
--							 --[Age_UpperLimitUnit],
--						     --[Age_CalMethod],
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	6 July 2010
-- Description:		Retrieve [Vaccination_Record_Available]
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 13 Aug 2009
-- Description:	Retrieve all document type 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	21 August 2009
-- Description:		Add 'Doc_Identity_Desc' & 'Doc_Identity_Desc_Chi'
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_DocType_get_all_cache] 
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
	[Doc_Code],
	[Doc_Name],
	[Doc_Name_Chi],
	[Doc_Name_CN],
	[Doc_Display_Code],	
	[Display_Seq],
	[Doc_Identity_Desc],
	[Doc_Identity_Desc_Chi],
	[Doc_Identity_Desc_CN],
	[Age_LowerLimit],
	[Age_LowerLimitUnit],
	[Age_UpperLimit],
	[Age_UpperLimitUnit],
	[Age_CalMethod],
	[IMMD_Validate_Avail],
	[Help_Available],
	[Force_Manual_Validate],
	[Vaccination_Record_Available],
	[Death_Record_Available],
	[Available_HCSP_SubPlatform]
FROM
	[DocType]

ORDER BY [Display_Seq] ASC

END
GO

GRANT EXECUTE ON [dbo].[proc_DocType_get_all_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_DocType_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_DocType_get_all_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_DocType_get_all_cache] TO WSEXT
Go