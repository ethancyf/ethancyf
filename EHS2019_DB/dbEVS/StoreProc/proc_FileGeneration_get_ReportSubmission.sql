IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_FileGeneration_get_ReportSubmission]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_FileGeneration_get_ReportSubmission]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	19 Apr 2016
-- CR No.			CRE15-016
-- Description:		Randomly genereate the valid claim transaction
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	2 January 2015
-- CR No.:			CRE13-019-02
-- Description:		Extend HCVS to China
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	05 Feb 2010
-- Description:		Handle Scheme, and retrieve Get_Data_From_Bak
-- ============================================= 
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 30 Jun 2008
-- Description:	Retrieve File Generation By File ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_FileGeneration_get_ReportSubmission]
	@User_ID varchar(20),
	@Show_for_Generation char(1)
AS
BEGIN
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

	FG.[File_ID],
	FG.[File_Name],
	FG.[File_Desc],
	FG.[Display_Code],
	FG.[File_Name_Prefix],
	FG.[File_Type],
	FG.[File_Data_SP],
	FG.[FilterCriteria_UC],
	FG.[Report_Template],
	FG.[Group_ID],
	FG.[Is_SelfAccess],
	FG.[Auto_Generate],
	FG.[Show_for_Generation],
	FG.[Frequency],
	FG.[Day_of_Generation],
	FG.[Message_Subject],
	FG.[Message_Content],
	FG.[Create_Dtm],
	FG.[Create_By],
	FG.[Update_Dtm],
	FG.[Update_By],
	FG.[Get_Data_From_Bak],
	FG.[XLS_Parameter]

FROM [FileGeneration] FG 
WHERE

	FG.[File_ID] IN (
		SELECT DISTINCT FG.[File_ID]
		FROM [FileGeneration] FG
				
			INNER JOIN [SchemeFileGeneration] SFG
				ON SFG.[File_ID] = FG.[File_ID]
			LEFT OUTER JOIN [RoleTypeFileGeneration] RTFG
				ON RTFG.[File_ID] = FG.[File_ID]
			LEFT OUTER JOIN [UserRole] UR
				ON UR.[Role_Type] = RTFG.[Role_Type]
		WHERE
			[Show_for_Generation] = @Show_for_Generation AND UR.[User_ID] = @User_ID AND
			(SFG.[Scheme_Code] = 'ALL' OR SFG.[Scheme_Code] = UR.[Scheme_Code])
	)
END
GO

GRANT EXECUTE ON [dbo].[proc_FileGeneration_get_ReportSubmission] TO HCVU
GO
