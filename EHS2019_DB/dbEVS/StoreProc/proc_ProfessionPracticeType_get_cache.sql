IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionPracticeType_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionPracticeType_get_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Modification History
-- CR No.:		
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
-- =============================================
-- CR No.:		CRE12-001
-- Author:		Koala CHENG
-- Create date: 18 Jan 2012
-- Description:	Retrieve data from ProfessionPracticeType Table
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionPracticeType_get_cache]
	
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

	SELECT p.Service_Category_Code, p.Item_No
	FROM ProfessionPracticeType  p WITH(NOLOCK)
		 INNER JOIN (SELECT Item_No, Display_Order FROM StaticData WITH(NOLOCK) WHERE Column_Name = 'PRACTICETYPE_PCD') s 
		 ON p.Item_No = s.Item_no
	WHERE  GETDATE() >= p.Effective_Dtm
	ORDER BY p.Service_Category_Code, s.Display_Order
END
GO

GRANT EXECUTE ON [dbo].[proc_ProfessionPracticeType_get_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_ProfessionPracticeType_get_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ProfessionPracticeType_get_cache] TO HCVU
GO
