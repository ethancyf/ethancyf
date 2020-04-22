IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntryPrecheckSubsidizeInject_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntryPrecheckSubsidizeInject_del]
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
-- Description:		Update Assign Date for Batch
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntryPrecheckSubsidizeInject_del]
	@Student_File_ID	VARCHAR(15),
	@Student_Seq		INT,
	@Scheme_Code		CHAR(10),
	@Scheme_Seq			SMALLINT,
	@Subsidize_Code		CHAR(10),
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

	DELETE FROM
		StudentFileEntryPrecheckSubsidizeInject
	WHERE 
		Student_File_ID = @Student_File_ID 
		AND Student_Seq = @Student_Seq 
		AND Scheme_Code = @Scheme_Code
		AND Scheme_Seq = @Scheme_Seq
		AND Subsidize_Code = @Subsidize_Code

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryPrecheckSubsidizeInject_del] TO HCSP
GO

