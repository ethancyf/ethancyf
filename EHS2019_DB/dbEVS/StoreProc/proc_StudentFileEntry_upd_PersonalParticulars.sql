IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntry_upd_PersonalParticulars]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntry_upd_PersonalParticulars]
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
-- Create by:		Chris YIM		
-- Create date:		29 Jul 2019
-- CR No.			CRE19-001
-- Description:		Update Personal Particulars From SP platform
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntry_upd_PersonalParticulars]
	@Student_File_ID		VARCHAR(15)	,
	@Student_Seq			INT,
	@Contact_No				VARCHAR(20),
	@Reject_Injection		CHAR(1),
	@Update_By				VARCHAR(20),
	@Update_Dtm				Datetime
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
		Contact_No = @Contact_No,
		Reject_Injection = @Reject_Injection,
		Last_Rectify_By = @Update_By,				
		Last_Rectify_Dtm = @Update_Dtm	
	WHERE 
		Student_File_ID = @Student_File_ID 
		AND Student_Seq = @Student_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_upd_PersonalParticulars] TO HCSP

GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_upd_PersonalParticulars] TO HCVU

GO

