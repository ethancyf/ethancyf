IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileHeaderStaging_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileHeaderStaging_del]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM		
-- Modified date:	28 Aug 2019
-- CR No.			CRE19-001
-- Description:		Grant EXECUTE right for role HCSP
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	26 August 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Delete StudentFileHeaderStaging
-- =============================================  

CREATE PROCEDURE [dbo].[proc_StudentFileHeaderStaging_del]
	@Student_File_ID			varchar(15),
	@Update_By					varchar(20),
	@TSMP						binary(8)
AS BEGIN

	SET NOCOUNT ON;
	
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================

	IF (
		SELECT TSMP FROM StudentFileHeaderStaging WHERE Student_File_ID = @Student_File_ID
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
		StudentFileHeaderStaging
	SET	
		Record_Status = 'R',
		Update_By = @Update_By,
		Update_Dtm = GETDATE()
	WHERE
		Student_File_ID = @Student_File_ID

	--
	
	DELETE
		StudentFileHeaderStaging
	WHERE
		Student_File_ID = @Student_File_ID
		
	--
	
	DELETE
		StudentFileEntryStaging
	WHERE
		Student_File_ID = @Student_File_ID


END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeaderStaging_del] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeaderStaging_del] TO HCSP
GO

