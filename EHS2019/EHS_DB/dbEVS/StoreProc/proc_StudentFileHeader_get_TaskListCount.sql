IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileHeader_get_TaskListCount]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileHeader_get_TaskListCount]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	21 September 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Get Task List Count
-- =============================================    

CREATE PROCEDURE [dbo].[proc_StudentFileHeader_get_TaskListCount]
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

	SELECT
		COUNT(DISTINCT S.Student_File_ID)
	FROM
		(
			SELECT 
				Student_File_ID
			FROM
				StudentFileHeader S
			WHERE
				Record_Status IN ('CU', 'CR', 'CT', 'CE')
		
			UNION
			
			SELECT 
				Student_File_ID
			FROM
				StudentFileHeaderStaging
			WHERE
				Record_Status IN ('CU', 'CR', 'CT', 'CE')
					
		) S
		

END
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeader_get_TaskListCount] TO HCVU
GO
