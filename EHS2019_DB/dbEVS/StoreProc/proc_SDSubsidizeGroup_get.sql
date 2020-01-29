IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SDSubsidizeGroup_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SDSubsidizeGroup_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		10 August 2016
-- CR No.:			CRE16-002
-- Description:		Retrieve SDSubsidizeGroup
-- =============================================

CREATE PROCEDURE [dbo].[proc_SDSubsidizeGroup_get] 
AS BEGIN

	SELECT
		Scheme_Code,
		Subsidize_Code,
		Subsidize_Desc,
		Subsidize_Desc_Chi,
		Subsidize_Short_Form,
		Subsidize_Short_Form_Chi,
		Search_Available,
		Search_Period_From,
		Search_Period_To,
		Search_Group,
		Subsidize_Item_Column_Name,
		Subsidize_Fee_Column_Name,
		Subsidize_Code_Secondary,
		Subsidize_Code_Combination,
		Display_Seq,
		Record_Status
	FROM
		SDSubsidizeGroup
	WHERE
		Record_Status = 'A'


END
GO

GRANT EXECUTE ON [dbo].[proc_SDSubsidizeGroup_get] TO HCPUBLIC
GO
