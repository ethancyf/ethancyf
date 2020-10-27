IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntry_upd_Sync_PersonalParticulars]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntry_upd_Sync_PersonalParticulars]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by		
-- Modified date	
-- CR No.			
-- Description		
-- =============================================
-- =============================================
-- Modification History
-- Create by		Chris YIM		
-- Create date		23 Sep 2019
-- CR No.			CRE20-003-03
-- Description		Update Personal Particulars
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntry_upd_Sync_PersonalParticulars]
	@Student_File_ID		VARCHAR(15)	,
	@Student_Seq			INT,
	@Class_No				NVARCHAR(10),
	@Contact_No				VARCHAR(20),
	@Update_By				VARCHAR(20),
	@Update_Dtm				DATETIME
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

	UPDATE 
		StudentFileEntry 
	SET 
		Class_No = @Class_No,
		Contact_No = @Contact_No,
		Last_Rectify_By = @Update_By,				
		Last_Rectify_Dtm = @Update_Dtm
	WHERE 
		Student_File_ID = @Student_File_ID 
		AND Student_Seq = @Student_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_upd_Sync_PersonalParticulars] TO HCSP

GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_upd_Sync_PersonalParticulars] TO HCVU

GO

