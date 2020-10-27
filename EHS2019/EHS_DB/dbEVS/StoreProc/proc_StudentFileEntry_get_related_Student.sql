IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileEntry_get_related_Student]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileEntry_get_related_Student]
GO

SET ANSI_NULLS ON
GO
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
-- Modified by:		Winnie SUEN
-- Modified date:	22 Sep 2020
-- CR No.:			CRE20-003-03 (Enhancement on Programme or Scheme using batch upload)
-- Description:		Get all related Student by StudentFileEntry.[Original_File_ID], [Original_Student_Seq]
-- =============================================     

CREATE PROCEDURE [dbo].[proc_StudentFileEntry_get_related_Student] (
	@Student_File_ID VARCHAR(15), 
	@Student_Seq     INT
)
AS
BEGIN

/*
Example:
VF20200910-009 3 (RVP Precheck)	> VF20200910-010 2 (PCV13)
			`					> VF20200910-011 3 (23vPPV)
								> VF20200910-012 1 (SIV 1st Dose) > VF20200910-013 1 (SIV 2nd Dose)

Input:
@Student_File_ID = 'VF20200910-013'
@Student_Seq	 = 1

Return:
Student_File_ID		Student_Seq	
----------------	-----------	
VF20200910-009		3			
VF20200910-010		2			
VF20200910-011		3			
VF20200910-012		1			
VF20200910-013		1			

*/

    -- =============================================
    -- Declaration
    -- =============================================
	DECLARE @RelatedEntry TABLE (
		Student_File_ID		VARCHAR(15), 
		Student_Seq			INT
	)
    -- =============================================
    -- Validation
    -- =============================================

    IF @Student_File_ID IS NULL
        RETURN;
    IF @Student_Seq IS NULL
        RETURN;

    -- =============================================
    -- Initialization
    -- =============================================
    -- =============================================
    -- Return results
    -- =============================================

	-- Find all Parent 
	; WITH parent AS (	
		-- Self
		SELECT 
			Student_File_ID, Student_Seq
		FROM StudentFileEntry
		WHERE Student_File_ID = @Student_File_ID AND Student_Seq = @Student_Seq

		UNION ALL
		
		SELECT 
			SE.Original_Student_File_ID, SE.Original_Student_Seq
		FROM parent P
		INNER JOIN StudentFileEntry SE ON P.Student_File_ID = SE.Student_File_ID AND P.Student_Seq = SE.Student_Seq
		WHERE SE.Original_Student_File_ID IS NOT NULL

	-- Find all child
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

	-- File in Perm
	INSERT INTO @RelatedEntry (Student_File_ID, Student_Seq)
	SELECT Student_File_ID, Student_Seq
	FROM tree t

	UNION
	-- File in Staging 
	SELECT Student_File_ID, Student_Seq
	FROM treeStaging ts

	UNION
	SELECT Student_File_ID, Student_Seq
	FROM PARENT

	-- 
	SELECT 
		Student_File_ID, 
		Student_Seq
	FROM
		@RelatedEntry
	ORDER BY
		Student_File_ID, 
		Student_Seq

END;
GO


GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_get_related_Student] TO HCSP;
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileEntry_get_related_Student] TO HCVU;
GO