IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntryPrecheckSubsidizeInject_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntryPrecheckSubsidizeInject_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================   
-- =============================================
-- Modification History
-- Created by:		Chris YIM		
-- Created date:	06 Sep 2019
-- CR No.			CRE19-001
-- Description:		Update Mark Inject for Batch
-- ============================================= 

CREATE PROCEDURE [dbo].[proc_StudentFileEntryPrecheckSubsidizeInject_upd]
	@Student_File_ID	VARCHAR(15),
	@Student_Seq		INT,
	@Scheme_Code		CHAR(10),
	@Scheme_Seq			SMALLINT,
	@Subsidize_Code		CHAR(10),
	@Mark_Injection		CHAR(1),
	@Update_By			VARCHAR(20),
	@Update_Dtm			DATETIME,
	@TSMP				BINARY(8)

AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================

	IF (
		SELECT 
			TSMP 
		FROM 
			StudentFileEntryPrecheckSubsidizeInject
		WHERE 
			Student_File_ID = @Student_File_ID 
			AND Student_Seq = @Student_Seq 
			AND Scheme_Code = @Scheme_Code
			AND Scheme_Seq = @Scheme_Seq
			AND Subsidize_Code = @Subsidize_Code
	) != @TSMP
	BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
	END

-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	UPDATE
		StudentFileEntryPrecheckSubsidizeInject
	SET	
		Mark_Injection = @Mark_Injection,
		Update_By = @Update_By,
		Update_Dtm = @Update_Dtm
	WHERE 
		Student_File_ID = @Student_File_ID 
		AND Student_Seq = @Student_Seq 
		AND Scheme_Code = @Scheme_Code
		AND Scheme_Seq = @Scheme_Seq
		AND Subsidize_Code = @Subsidize_Code

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryPrecheckSubsidizeInject_upd] TO HCSP
GO
