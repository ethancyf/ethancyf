IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalSubmissionHeader_upd_Import]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalSubmissionHeader_upd_Import]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================  
-- Modification History  
-- CR No:			CRE13-016 Upgrade Excel verion to 2007
-- Modified by:  Karl LAM  
-- Modified date: 21 Oct 2013  
-- Description:  Change File Name to Varchar(50)  
-- =============================================  
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 29 May 2008
-- Description:	Update Professional Submission Header For Import
-- =============================================

CREATE PROCEDURE [dbo].[proc_ProfessionalSubmissionHeader_upd_Import]
	@File_Name varchar(50),
	@Import_By varchar(20),
	@Import_Dtm datetime OUTPUT
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

	UPDATE [dbo].[ProfessionalSubmissionHeader]
	SET 
		[Import_Dtm] = GetDate(),
		[Import_By] = @Import_By 

	WHERE File_Name = @File_Name

	SELECT
		@Import_Dtm = [Import_Dtm] 
	FROM [dbo].[ProfessionalSubmissionHeader]
	
	WHERE
		File_Name = @File_Name

END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalSubmissionHeader_upd_Import] TO HCVU
GO
