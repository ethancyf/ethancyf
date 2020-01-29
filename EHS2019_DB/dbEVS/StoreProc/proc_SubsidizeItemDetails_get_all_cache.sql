IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SubsidizeItemDetails_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SubsidizeItemDetails_get_all_cache]
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
-- Description:		Add new columns - 'Available_Item_Num', 'Internal_Use'
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 21 Aug 2009
-- Description:	Retrieve all SubsidizeItemDetails (1st Dose, 2nd Dose)
-- =============================================

CREATE PROCEDURE [dbo].[proc_SubsidizeItemDetails_get_all_cache] 
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
	[Subsidize_Item_Code],	
	[Available_Item_Code],
	[Display_Seq],
	[Available_Item_Desc],
	[Available_Item_Desc_Chi],
	[Available_Item_Desc_CN],
	[Available_Item_Num],
	[Internal_Use],
	[Create_By],
	[Create_Dtm],
	[Update_By],
	[Update_Dtm],
	[Record_Status]
FROM
	[SubsidizeItemDetails]
ORDER BY [Display_Seq] ASC

END
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeItemDetails_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeItemDetails_get_all_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeItemDetails_get_all_cache] TO WSEXT
Go