IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SubsidizeGroupClaimItemDetails_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SubsidizeGroupClaimItemDetails_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	9 December 2014
-- CR No.:			CRE13-019-01
-- Description:		Support Simplified Chinese
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Twinsen CHAN
-- Modified date:	17 Dec 2012
-- Description:		Add new columns [SubsidizeItemDetails].[Available_Item_Num], [SubsidizeItemDetails].[Internal_Use]
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 13 Aug 2010
-- Description:	Retrieve all SubsidizeGroupClaimItemDetails (Scheme + Subsidy -> Dose relation)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_SubsidizeGroupClaimItemDetails_get_all_cache]
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
	SGCID.[Scheme_Code],
	SGCID.[Scheme_Seq],
	SGCID.[Subsidize_Code],
	SGCID.[Subsidize_Item_Code],
	SGCID.[Available_Item_Code],
	SGCID.[Create_By],
	SGCID.[Create_Dtm],
	SGCID.[Update_By],
	SGCID.[Update_Dtm],
	SGCID.[Record_Status],
	item.[Display_Seq],
	item.[Available_Item_Desc],
	item.[Available_Item_Desc_Chi],
	item.[Available_Item_Desc_CN],
	item.[Available_Item_Num],
	item.[Internal_Use]

FROM
	[SubsidizeGroupClaimItemDetails] SGCID 
	INNER JOIN [SubsidizeItemDetails] item ON
		item.[Subsidize_Item_Code] = SGCID.[Subsidize_Item_Code] AND
		item.[Available_Item_Code] = SGCID.[Available_Item_Code]
		
WHERE
	SGCID.[Record_Status] = 'A'
ORDER BY item.[Subsidize_Item_Code], item.[Display_Seq] ASC

END
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeGroupClaimItemDetails_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeGroupClaimItemDetails_get_all_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeGroupClaimItemDetails_get_all_cache] TO WSEXT
Go