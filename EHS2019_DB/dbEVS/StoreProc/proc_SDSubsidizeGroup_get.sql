IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SDSubsidizeGroup_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SDSubsidizeGroup_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modifier			Chris YIM
-- Modified date	16 January 2020
-- CR No.			CRE19-030
-- Description		Retrieve SDSubsidizeGroup
-- =============================================
-- =============================================
-- Author			Lawrence TSANG
-- Create date		10 August 2016
-- CR No.			CRE16-002
-- Description		Retrieve SDSubsidizeGroup
-- =============================================

CREATE PROCEDURE [dbo].[proc_SDSubsidizeGroup_get] 
AS BEGIN

	SELECT
		SDSG.Scheme_Code,
		SDSG.Subsidize_Code,
		CC.SD_Category_Name,
		CC.SD_Category_Name_Chi,
		SDSG.Subsidize_Desc,
		SDSG.Subsidize_Desc_Chi,
		SDSG.Subsidize_Short_Form,
		SDSG.Subsidize_Short_Form_Chi,
		SDSG.Search_Available,
		SDSG.Search_Period_From,
		SDSG.Search_Period_To,
		SDSG.Search_Group,
		SDSG.Subsidize_Item_Column_Name,
		SDSG.Subsidize_Fee_Column_Name,
		SDSG.Subsidize_Code_Secondary,
		SDSG.Subsidize_Code_Combination,
		SDSG.Display_Seq,
		SDSG.Record_Status
	FROM
		SDSubsidizeGroup SDSG
			LEFT OUTER JOIN SubsidizeGroupCategory SGC
				ON SDSG.Subsidize_Code = SGC.Subsidize_Code
			LEFT OUTER JOIN ClaimCategory CC
				ON SGC.Category_Code = CC.Category_Code
	WHERE
		SDSG.Record_Status = 'A'


END
GO

GRANT EXECUTE ON [dbo].[proc_SDSubsidizeGroup_get] TO HCPUBLIC
GO
