IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_StudentFileHeaderPrecheckDate_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_StudentFileHeaderPrecheckDate_get]
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
-- Created by:		Chris YIM		
-- Created date:	06 Sep 2019
-- CR No.			CRE19-001
-- Description:		Get Assign Date for Batch
-- ============================================= 

CREATE PROCEDURE [dbo].[proc_StudentFileHeaderPrecheckDate_get]
	@Student_File_ID	varchar(15)
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
		SFHPCD.[Student_File_ID]
		,SFHPCD.[Scheme_Code]
		,SFHPCD.[Scheme_Seq]
		,SFHPCD.[Subsidize_Code]
		,SFHPCD.[Subsidize_Item_Code]
		,SFHPCD.[Class_Name]
		,SFHPCD.[Service_Receive_Dtm]
		,SFHPCD.[Final_Checking_Report_Generation_Date]
		,SFHPCD.[Service_Receive_Dtm_2ndDose]
		,SFHPCD.[Final_Checking_Report_Generation_Date_2ndDose]
		,SFHPCD.[Create_By]
		,SFHPCD.[Create_Dtm]
		,SFHPCD.[Update_By]
		,SFHPCD.[Update_Dtm]
		,SFHPCD.[TSMP]
		,SFE.*
	FROM 
		[StudentFileHeaderPrecheckDate] SFHPCD
			INNER JOIN (SELECT 
							ROW_NUMBER() OVER(PARTITION BY Student_File_ID ORDER BY Student_Seq ASC) AS [Student_Seq], 		
							ROW_NUMBER() OVER(PARTITION BY Student_File_ID, Class_Name ORDER BY Student_Seq ASC) AS [Class_Seq], 				
							Student_File_ID, 
							Class_Name 
						FROM [StudentFileEntry]) SFE 
				ON SFHPCD.Student_File_ID = SFE.Student_File_ID AND UPPER(SFHPCD.Class_Name) = UPPER(SFE.Class_Name) AND SFE.[Class_Seq] = 1
	WHERE
		SFHPCD.[Student_File_ID] = @Student_File_ID
	ORDER BY 
		SFE.[Student_Seq]


			--INNER JOIN [SubsidizeGroupClaim] SGC
			--	ON SFHPCD.Scheme_Code = SGC.Scheme_Code AND SFHPCD.Scheme_Seq = SGC.Scheme_Seq AND SFHPCD.Subsidize_Code = SGC.Subsidize_Code
			--LEFT OUTER JOIN ClaimCategory CC
			--	ON SFHPCD.Class_Name = CC.Category_Code
	--WHERE
	--	SFHPCD.[Student_File_ID] = @Student_File_ID
	--ORDER BY 
	--	SGC.Display_Seq, CC.Display_Seq
	
END

GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeaderPrecheckDate_get] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_StudentFileHeaderPrecheckDate_get] TO HCVU
GO

