IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileHeaderPrecheckDate_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileHeaderPrecheckDate_upd]
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

CREATE PROCEDURE [dbo].[proc_StudentFileHeaderPrecheckDate_upd]
	@Student_File_ID									VARCHAR(15),
	@Scheme_Code										CHAR(10),
	@Scheme_Seq											SMALLINT,
	@Subsidize_Code										CHAR(10),
	@Service_Receive_Dtm								DATETIME,
	@Final_Checking_Report_Generation_Date				DATETIME,
	@Service_Receive_Dtm_2ndDose						DATETIME,
	@Final_Checking_Report_Generation_Date_2ndDose		DATETIME,
	@Update_By											VARCHAR(20),
	@Update_Dtm											DATETIME,
	@TSMP												BINARY(8)

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
			StudentFileHeaderPrecheckDate 
		WHERE 
			Student_File_ID = @Student_File_ID 
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
		StudentFileHeaderPrecheckDate
	SET	
		Service_Receive_Dtm = @Service_Receive_Dtm,
		Final_Checking_Report_Generation_Date = @Final_Checking_Report_Generation_Date,
		Service_Receive_Dtm_2ndDose = @Service_Receive_Dtm_2ndDose,
		Final_Checking_Report_Generation_Date_2ndDose = @Final_Checking_Report_Generation_Date_2ndDose,
		Update_By = @Update_By,
		Update_Dtm = @Update_Dtm
	WHERE 
		Student_File_ID = @Student_File_ID 
		AND Scheme_Code = @Scheme_Code
		AND Scheme_Seq = @Scheme_Seq
		AND Subsidize_Code = @Subsidize_Code

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeaderPrecheckDate_upd] TO HCSP
GO
