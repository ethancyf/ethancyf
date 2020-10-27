IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntry_upd_DocCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntry_upd_DocCode]
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
-- Author:			Winnie SUEN
-- Modified date:	26 Augest 2020
-- CR No.:			CRE20-003 (Enhancement on Programme or Scheme using batch upload)
-- Description:		Update Doc Code of Entry when change doc code in Vaccination File Rectification
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntry_upd_DocCode]
	@Student_File_ID		VARCHAR(15),
	@Student_Seq			INT,
	@Doc_Code				CHAR(20),
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
		Doc_Code = @Doc_Code,
		Acc_Doc_Code = @Doc_Code,
		Last_Rectify_By = @Update_By,
		Last_Rectify_Dtm = @Update_Dtm	
	WHERE 
		Student_File_ID = @Student_File_ID AND Student_Seq = @Student_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_upd_DocCode] TO HCVU
GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_upd_DocCode] TO HCSP
GO

