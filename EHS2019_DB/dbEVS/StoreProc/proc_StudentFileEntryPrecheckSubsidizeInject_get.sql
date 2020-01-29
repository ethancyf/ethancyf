IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntryPrecheckSubsidizeInject_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntryPrecheckSubsidizeInject_get]
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
-- Description:		Get Mark Inject for Batch
-- ============================================= 

CREATE PROCEDURE [dbo].[proc_StudentFileEntryPrecheckSubsidizeInject_get]
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
		SFEPCSI.[Student_File_ID]
		,SFEPCSI.[Student_Seq]
		,SFEPCSI.[Class_Name]
		,SFEPCSI.[Scheme_Code]
		,SFEPCSI.[Scheme_Seq]
		,SFEPCSI.[Subsidize_Code]
		,SFEPCSI.[Mark_Injection]
		,SFEPCSI.[Create_By]
		,SFEPCSI.[Create_Dtm]
		,SFEPCSI.[Update_By]
		,SFEPCSI.[Update_Dtm]
		,SFEPCSI.[TSMP]
	FROM 
		[StudentFileEntryPrecheckSubsidizeInject] SFEPCSI
			INNER JOIN [SubsidizeGroupClaim] SGC
				ON SFEPCSI.Scheme_Code = SGC.Scheme_Code AND SFEPCSI.Scheme_Seq = SGC.Scheme_Seq AND SFEPCSI.Subsidize_Code = SGC.Subsidize_Code
			LEFT OUTER JOIN ClaimCategory CC
				ON SFEPCSI.Class_Name = CC.Category_Code
	WHERE
		[Student_File_ID] = @Student_File_ID
	ORDER BY 
		SGC.Display_Seq, CC.Display_Seq, SFEPCSI.[Student_Seq]
	
END

GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryPrecheckSubsidizeInject_get] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryPrecheckSubsidizeInject_get] TO HCVU
GO

