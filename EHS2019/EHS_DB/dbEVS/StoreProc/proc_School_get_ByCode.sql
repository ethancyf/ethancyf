IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_School_get_ByCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_School_get_ByCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by		Chris YIM		
-- Modified date	16 Sep 2019
-- CR No.			CRE19-001
-- Description		Change length of school code
-- =============================================
-- =============================================
-- Author			Chris YIM
-- Create date		24 Aug 2018
-- CR No.			CRE17-018
-- Description		Search the school by school code
-- =============================================

CREATE PROCEDURE [dbo].[proc_School_get_ByCode]
	@SchoolCode VARCHAR(30)
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
		[School_Code]
		,[Name_Eng]
		,[Name_Chi]
		,[Address_Eng]
		,[Address_Chi]
		,[district_board]
		,[Record_Status]
		,[Create_By]
		,[Create_Dtm]
		,[Update_By]
		,[Update_Dtm]
		,[TSMP]
	FROM 
		[School] WITH (NOLOCK)
	WHERE 
		[School_Code] = @SchoolCode

END
GO

GRANT EXECUTE ON [dbo].[proc_School_get_ByCode] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_School_get_ByCode] TO HCVU
GO

