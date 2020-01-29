IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchoolList_get_ByName]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchoolList_get_ByName]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [proc_SchoolList_get_ByName]
	@SchoolName NVARCHAR(255),
	@SchemeCode VARCHAR(10)
AS 
BEGIN

-- =============================================
-- Modification History
-- Modified by:		Chris YIM		
-- Modified date:	05 Jul 2019
-- CR No.			CRE19-001
-- Description:		Search school list by school name & scheme code
-- =============================================
-- =============================================
-- Author:			Chris YIM
-- Create date:		24 Aug 2018
-- CR No.			CRE17-018
-- Description:		Search the school List by school name
-- =============================================

	DECLARE @IN_SchoolName NVARCHAR(255)

	SET @IN_SchoolName = '%' + @SchoolName + '%'

	SELECT 
		[School_Code]
		,[Scheme_Code]
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
		(
			([Name_Eng] LIKE @IN_SchoolName OR [Name_Chi] LIKE @IN_SchoolName)
			OR
			([School_Code] LIKE @IN_SchoolName)
		)
		AND LTRIM(RTRIM([Scheme_Code])) = @SchemeCode 
		AND [Record_Status] = 'A'

END
GO

GRANT EXECUTE ON [dbo].[proc_SchoolList_get_ByName] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SchoolList_get_ByName] TO HCVU
GO

