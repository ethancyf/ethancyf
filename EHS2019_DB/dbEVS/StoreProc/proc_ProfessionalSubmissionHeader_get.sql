IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalSubmissionHeader_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalSubmissionHeader_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Tommy TSE
-- Modified date:	9 Sep 2011
-- Description:		Profession related data is
--					retrieved from table [profession]
-- =============================================

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 28 May 2008
-- Description:	Retrieve Professional Submission Header
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionalSubmissionHeader_get]
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
	PS.File_Name, PS.Export_Dtm, PS.Export_By, PS.Service_Category_Code, PS.Import_Dtm, PS.Import_By,

	HCVUUser.User_ID as Export_User, HCVUUser2.User_ID as Import_User, [profession].[Service_Category_Desc] as Profession_Description
FROM 
	[dbo].[ProfessionalSubmissionHeader] PS

		LEFT OUTER JOIN [dbo].[profession]
			ON [profession].[Service_Category_Code] = PS.Service_Category_Code
		LEFT OUTER JOIN [dbo].[HCVUUserAC] HCVUUser
			ON HCVUUser.User_ID = PS.Export_By
		LEFT OUTER JOIN [dbo].[HCVUUserAC] HCVUUser2
			ON HCVUUser2.User_ID = PS.Import_By

	WHERE PS.Record_Status = 'A'
ORDER BY
	PS.Export_Dtm DESC

END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalSubmissionHeader_get] TO HCVU
GO
