IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalSubmissionHeader_get_ByKey]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalSubmissionHeader_get_ByKey]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No:			CRE13-016 Upgrade Excel verion to 2007
-- Modified by:		Karl LAM
-- Modified date:	21 Oct 2013
-- Description:		Change File Name to Varchar(50)
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 13 Jun 2008
-- Description:	Retrieve Professional Submission Header By Key
-- =============================================

CREATE PROCEDURE [dbo].[proc_ProfessionalSubmissionHeader_get_ByKey]
	@File_Name varchar(50)
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
	PS.File_Name, PS.Export_Dtm, PS.Export_By, PS.Service_Category_Code, PS.Import_Dtm, 

PS.Import_By

FROM 
	[dbo].[ProfessionalSubmissionHeader] PS

	WHERE 
		PS.File_Name = @File_Name

ORDER BY
	PS.Export_Dtm DESC

END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalSubmissionHeader_get_ByKey] TO HCVU
GO
