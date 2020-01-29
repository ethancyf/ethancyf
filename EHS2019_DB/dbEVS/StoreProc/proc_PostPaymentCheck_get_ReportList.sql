IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PostPaymentCheck_get_ReportList]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PostPaymentCheck_get_ReportList]
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
-- Author:			Chris YIM
-- Create date:		20 Jun 2016
-- CR No.			CRE15-016
-- Description:		Randomly genereate the valid claim transaction
-- =============================================
CREATE PROCEDURE [dbo].[proc_PostPaymentCheck_get_ReportList]
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
	FG.[Show_for_Generation],
	FG.[Display_Code],
	FG.[File_Name],
	FG.[File_Desc]

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

GRANT EXECUTE ON [dbo].[proc_PostPaymentCheck_get_ReportList] TO HCVU
GO
