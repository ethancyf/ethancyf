IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SubsidizeGroupCategory_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SubsidizeGroupCategory_get_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History      
-- CR No.:			CRE16-021 Transfer VSS category to PCD
-- Modified by:		Winnie SUEN
-- Modified date:	20 Dec 2016
-- Description:		Grant permission to WSINT, HCVU and HCSP for PCDInterface
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		10 August 2016
-- CR No.:			CRE16-002
-- Description:		Retrieve SubsidizeGroupCategory
-- =============================================

CREATE PROCEDURE [dbo].[proc_SubsidizeGroupCategory_get_cache] 
AS BEGIN

	SELECT
		SGC.Scheme_Code,
		SGC.Subsidize_Code,
		SGC.Category_Code,
		SGC.IsMedicalCondition,
		SGC.Record_Status,
		CC.Category_Name,
		CC.Category_Name_Chi,
		CC.Display_Seq,
		CC.Category_Name_CN,
		CC.SD_Category_Name,
		CC.SD_Category_Name_Chi
	FROM
		SubsidizeGroupCategory SGC
			INNER JOIN ClaimCategory CC
				ON SGC.Category_Code = CC.Category_Code
	WHERE
		SGC.Record_Status = 'A'


END
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeGroupCategory_get_cache] TO HCPUBLIC, WSINT, HCVU, HCSP
GO
