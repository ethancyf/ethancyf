IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_School_get_Active_ByCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_School_get_Active_ByCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM		
-- Modified date:	05 Jul 2019
-- CR No.			CRE19-001
-- Description:		Search active school by school code & scheme code
-- =============================================
-- =============================================
-- Author:			Chris YIM
-- Create date:		24 Aug 2018
-- CR No.			CRE17-018
-- Description:		Search active school by school code
-- =============================================

CREATE PROCEDURE [proc_School_get_Active_ByCode]
	@SchoolCode VARCHAR(30),
	@SchemeCode VARCHAR(10)
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
		[School_Code] = @SchoolCode 
		AND LTRIM(RTRIM([Scheme_Code])) = @SchemeCode 
		AND [Record_Status] = 'A'

END
GO

GRANT EXECUTE ON [dbo].[proc_School_get_Active_ByCode] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_School_get_Active_ByCode] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_School_get_Active_ByCode] TO WSEXT
GO

