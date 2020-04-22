IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalSubmissionHeader_upd_markdelete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalSubmissionHeader_upd_markdelete]
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
-- Description:	Update Professional Submission Header Mark Delete
-- =============================================

CREATE PROCEDURE [dbo].[proc_ProfessionalSubmissionHeader_upd_markdelete]
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

	UPDATE [dbo].[ProfessionalSubmissionHeader]
	SET 
		[Record_Status] = 'I'

	WHERE File_Name = @File_Name
END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalSubmissionHeader_upd_markdelete] TO HCVU
GO
