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
	@Acc_Doc_Code			CHAR(20),
	@Update_By				VARCHAR(20),
	@Update_Dtm				DATETIME
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @RelateEntry TABLE (
		Student_File_ID		VARCHAR(15),
		Student_Seq			INT
	)

	DECLARE @Temp_Voucher_Acc_ID  CHAR(15)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	SET @Temp_Voucher_Acc_ID = (SELECT Temp_Voucher_Acc_ID FROM StudentFileEntry WHERE Student_File_ID = @Student_File_ID AND Student_Seq = @Student_Seq)

-- =============================================
-- Return results
-- =============================================

	-- Handle Same Temp Account ID Original File
	IF ISNULL(@Temp_Voucher_Acc_ID,'') <> ''
	BEGIN

		-- Find all related Student by [Original_File_ID], [Original_Student_Seq]
		; WITH parent AS (	
		-- Self
		SELECT Student_File_ID, Student_Seq
		FROM StudentFileEntry
		WHERE Student_File_ID = @Student_File_ID AND Student_Seq = @Student_Seq

		UNION ALL
		-- Parent 
		SELECT SE.Original_Student_File_ID, SE.Original_Student_Seq
		FROM parent P
		INNER JOIN StudentFileEntry SE ON P.Student_File_ID = SE.Student_File_ID AND P.Student_Seq = SE.Student_Seq
		WHERE SE.Original_Student_File_ID IS NOT NULL

	), tree AS (
		SELECT	x.Original_Student_File_ID, x.Original_Student_Seq,
				x.Student_File_ID, x.Student_Seq				
		FROM StudentFileEntry x
		INNER JOIN parent ON x.Original_Student_File_ID = parent.Student_File_ID
							AND x.Original_Student_Seq = parent.Student_Seq
		UNION ALL

		SELECT	y.Original_Student_File_ID, y.Original_Student_Seq,
				y.Student_File_ID, y.Student_Seq				
		FROM StudentFileEntry y
		INNER JOIN tree t ON y.Original_Student_File_ID = t.Student_File_ID
						AND y.Original_Student_Seq = t.Student_Seq
	), treeStaging AS (
		SELECT	x.Original_Student_File_ID, x.Original_Student_Seq,
				x.Student_File_ID, x.Student_Seq				
		FROM StudentFileEntryStaging x
		INNER JOIN parent ON x.Original_Student_File_ID = parent.Student_File_ID
							AND x.Original_Student_Seq = parent.Student_Seq
		UNION ALL

		SELECT	y.Original_Student_File_ID, y.Original_Student_Seq,
				y.Student_File_ID, y.Student_Seq				
		FROM StudentFileEntryStaging y
		INNER JOIN tree t ON y.Original_Student_File_ID = t.Student_File_ID
						AND y.Original_Student_Seq = t.Student_Seq
	)

	INSERT INTO @RelateEntry (Student_File_ID, Student_Seq)
	SELECT Student_File_ID, Student_Seq	
	FROM tree
	UNION
	SELECT Student_File_ID, Student_Seq	
	FROM treeStaging
	UNION
	SELECT Student_File_ID, Student_Seq	 FROM PARENT
	
	-- SELECT * FROM @RelateEntry

	-- ============================================
	-- Update Doc Code for related Student Entry
	-- ============================================
	-- Perm
	UPDATE SE		
	SET 
		Doc_Code = @Doc_Code,
		Acc_Doc_Code = @Acc_Doc_Code,
		Update_By = @Update_By,
		Update_Dtm = @Update_Dtm
	FROM 
		StudentFileEntry SE
		INNER JOIN 
			@RelateEntry R ON SE.Student_File_ID = R.Student_File_ID AND SE.Student_Seq = R.Student_Seq
	WHERE 
		SE.Acc_Type = 'T' AND SE.Temp_Voucher_Acc_ID = @Temp_Voucher_Acc_ID

	-- Staging
	UPDATE SE		
	SET 
		Doc_Code = @Doc_Code,
		Acc_Doc_Code = @Acc_Doc_Code,
		Update_By = @Update_By,
		Update_Dtm = @Update_Dtm
	FROM 
		StudentFileEntryStaging SE
		INNER JOIN 
			@RelateEntry R ON SE.Student_File_ID = R.Student_File_ID AND SE.Student_Seq = R.Student_Seq
	WHERE 
		SE.Acc_Type = 'T' AND SE.Temp_Voucher_Acc_ID = @Temp_Voucher_Acc_ID

END


END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_upd_DocCode] TO HCVU
GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_upd_DocCode] TO HCSP
GO

