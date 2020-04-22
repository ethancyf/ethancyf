IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntryPrecheckSubsidizeInject_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntryPrecheckSubsidizeInject_add]
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
-- Modified by:		Chris YIM		
-- Modified date:	06 Sep 2019
-- CR No.			CRE19-001
-- Description:		Add Mark Inject for batch
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntryPrecheckSubsidizeInject_add]
	@Student_File_ID	VARCHAR(15),
	@Student_Seq		INT,
	@Class_Name			NVARCHAR(10),
	@Scheme_Code		CHAR(10),
	@Scheme_Seq			SMALLINT,
	@Subsidize_Code		CHAR(10),
	@Mark_Injection		CHAR(1),
	@Create_By			VARCHAR(20),
	@Create_Dtm			DATETIME,
	@Update_By			VARCHAR(20),
	@Update_Dtm			DATETIME

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
	
	INSERT INTO [StudentFileEntryPrecheckSubsidizeInject] (
		[Student_File_ID]
		,[Student_Seq]
		,[Class_Name]
		,[Scheme_Code]
		,[Scheme_Seq]
		,[Subsidize_Code]
		,[Mark_Injection]
		,[Create_By]
		,[Create_Dtm]
		,[Update_By]
		,[Update_Dtm]
	) VALUES (
		@Student_File_ID
		,@Student_Seq
		,@Class_Name
		,@Scheme_Code
		,@Scheme_Seq
		,@Subsidize_Code
		,@Mark_Injection
		,@Create_By
		,@Create_Dtm
		,@Update_By
		,@Update_Dtm
	)
	
END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryPrecheckSubsidizeInject_add] TO HCSP
GO

