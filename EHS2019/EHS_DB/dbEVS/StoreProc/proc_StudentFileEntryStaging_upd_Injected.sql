IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntryStaging_upd_Injected]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntryStaging_upd_Injected]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	2 Sep 2020
-- CR No.			CRE20-003 (Enhancement on Programme or Scheme using batch upload)
-- Description:		Update [Service_Receive_Dtm]
-- =============================================   
-- =============================================
-- Modification History
-- Created by:		Chris YIM		
-- Created date:	30 Aug 2019
-- CR No.			CRE19-001
-- Description:		Update StudentFileEntryStating - Injected
-- =============================================

CREATE PROCEDURE [dbo].[proc_StudentFileEntryStaging_upd_Injected]
	@Student_File_ID		VARCHAR(15),
	@Student_Seq			INT,
	@Injected				CHAR(1),
	@Service_Receive_Dtm	DATETIME,
	@Update_By				VARCHAR(20),
	@Update_Dtm				DATETIME,
	@TSMP					BINARY(8)
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================

	--IF (
	--	SELECT TSMP FROM StudentFileEntryStaging WHERE Student_File_ID = @Student_File_ID AND Student_Seq = @Student_Seq
	--) != @TSMP
	--BEGIN
	--	RAISERROR('00011', 16, 1)
	--	RETURN @@error
	--END

-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	UPDATE
		StudentFileEntryStaging
	SET	
		Injected = @Injected,
		Service_Receive_Dtm = @Service_Receive_Dtm,
		Update_By = @Update_By,
		Update_Dtm = @Update_Dtm
	WHERE
		Student_File_ID = @Student_File_ID
		 AND Student_Seq = @Student_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntryStaging_upd_Injected] TO HCSP
GO
