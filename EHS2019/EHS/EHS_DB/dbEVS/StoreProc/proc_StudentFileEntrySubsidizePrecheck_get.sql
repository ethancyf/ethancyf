IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntrySubsidizePrecheck_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntrySubsidizePrecheck_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.			
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- Created by:		Chris YIM		
-- Created date:	06 Sep 2019
-- CR No.			CRE19-001
-- Description:		Get Subsidize Pre-Check Result for Batch
-- ============================================= 

CREATE PROCEDURE [dbo].[proc_StudentFileEntrySubsidizePrecheck_get]
	@Student_File_ID	varchar(15)
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
		[Student_File_ID]
		,[Student_Seq]
		,[Class_Name]
		,[Scheme_Code]
		,[Scheme_Seq]
		,[Subsidize_Code]
		,[Entitle_ONLYDOSE]
		,[Entitle_1STDOSE]
		,[Entitle_2NDDOSE]
		,[Remark_ONLYDOSE]
		,[Remark_1STDOSE]
		,[Remark_2NDDOSE]
		,[Entitle_Inject_Fail_Reason]
		,[Inject_ONLYDOSE_1STDOSE]
		,[Inject_2NDDOSE]
		,[Create_Dtm]
	FROM 
		[StudentFileEntrySubsidizePrecheck]
	WHERE
		[Student_File_ID] = @Student_File_ID
	
END

GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntrySubsidizePrecheck_get] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntrySubsidizePrecheck_get] TO HCVU
GO

