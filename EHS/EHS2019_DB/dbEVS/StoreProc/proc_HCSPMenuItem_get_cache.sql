IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPMenuItem_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPMenuItem_get_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	16 February 2015
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
-- Modified by:		Koala CHENG
-- CR No.:			CRE11-024-02
-- Modified date:	14-10-2011
-- Description:		Add column [SystemResourceObjectName_ReturnBtn]
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 21 May 2008
-- Description:	Retrieve the Menu Item from Table
--				HCSPMenuItem
-- =============================================

-- =============================================
-- Modification History
-- Modified by:		Billy Lam
-- Modified date:	28-07-2008
-- Description:		add column "PopUp"
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   03-06-2009
-- Description:	    Add to select from column scheme_code, function_code, expiry_date
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Pak Ho LEE
-- Modified date:   07-07-2009
-- Description:	    Remove Where Cause for checking expiry date & effective date
--					This function retrieve all entry to cache and manipulate in middle tier
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Pak Ho LEE
-- Modified date:   05-Aug-2009
-- Description:	    Rename [MScheme_Code] -> [Scheme_Code] (of SchemeClaim)
-- =============================================

CREATE PROCEDURE [dbo].[proc_HCSPMenuItem_get_cache]
AS
BEGIN
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
		[Menu_Name], [Type], [Menu_Name_Chi], [Menu_Name_CN],
		[URL], [ImageURL], [Record_Status],
		[Role], [Effective_date], [PopUp], [Scheme_Code],
		[Function_Code], [Expiry_Date], [SystemResourceObjectName_ReturnBtn],
		[Available_HCSP_SubPlatform]
	FROM 
		[dbo].[HCSPMenuItem]
	WHERE
		[Record_Status] = 'A'
	ORDER BY [Display_Seq]
END
GO

GRANT EXECUTE ON [dbo].[proc_HCSPMenuItem_get_cache] TO HCSP
GO
