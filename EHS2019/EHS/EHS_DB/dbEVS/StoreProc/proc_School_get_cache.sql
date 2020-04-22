IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_School_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_School_get_cache]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	12 Jul 2019
-- CR No.			CRE19-001
-- Description:		Add [Scheme_Code]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:	8 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:	  	Get School
-- =============================================

CREATE PROCEDURE [dbo].[proc_School_get_cache] 
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
		Scheme_Code,
		School_Code,
		Name_Eng,
		Name_Chi,
		Address_Eng,
		Address_Chi,
		district_board,
		Record_Status,
		Create_By,
		Create_Dtm,
		Update_By,
		Update_Dtm,
		TSMP
	FROM
		School
	WHERE
		Record_Status = 'A'
	
	
END
GO

GRANT EXECUTE ON [dbo].[proc_School_get_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_School_get_cache] TO HCVU
GO