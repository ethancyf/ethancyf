IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PatientPortalDoctorList_get_WithFileContent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[proc_PatientPortalDoctorList_get_WithFileContent]
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
-- Author:			Chris YIM
-- Create date:		08-06-2020
-- CR No.:			CRE20-005
-- Description:		Retrieve Doctor List
-- =============================================

CREATE PROCEDURE [dbo].[proc_PatientPortalDoctorList_get_WithFileContent]
	@File_Type AS VARCHAR(3)
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

	SELECT TOP 1
		[File_Content]
	FROM 
		[dbEVS_File].[dbo].[PatientPortalDoctorList_File]
	WHERE
		[File_Type] = @File_Type
	ORDER BY
		System_Dtm DESC

END
GO

GRANT EXECUTE ON [dbo].[proc_PatientPortalDoctorList_get_WithFileContent] TO WSEXT
GO

