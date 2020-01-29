IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Subsidize_get_all]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_Subsidize_get_all]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	19 Sep 2017
-- CR No.:			CRE16-026-03 
-- Description:		Add Column - [Subsidize].[Vaccine_Type]
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		25 August 2016
-- CR No.:			CRE16-002
-- Description:		Retrieve Subsidize
-- =============================================

CREATE PROCEDURE [dbo].[proc_Subsidize_get_all] 
AS BEGIN

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
		Subsidize_Code,
		Subsidize_Item_Code,
		Display_Code,
		Display_Seq,
		Create_By,
		Create_Dtm,
		Update_By,
		Update_Dtm,
		Record_Status,
		Mth_Statement_Desc,
		Mth_Statement_Desc_Chi,
		Legend_Desc,
		Legend_Desc_Chi,
		Mth_Statement_Desc_CN,
		Legend_Desc_CN,
		Vaccine_Type
	FROM
		Subsidize
	WHERE
		Record_Status = 'A'


END
GO

GRANT EXECUTE ON [dbo].[proc_Subsidize_get_all] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_Subsidize_get_all] TO HCVU
GO

