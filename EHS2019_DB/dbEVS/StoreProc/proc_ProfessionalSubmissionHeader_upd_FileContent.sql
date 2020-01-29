IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalSubmissionHeader_upd_FileContent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalSubmissionHeader_upd_FileContent]
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
-- Create date: 30 Jun 2008
-- Description:	Update File Generation Queue File Content
-- =============================================

CREATE PROCEDURE [dbo].[proc_ProfessionalSubmissionHeader_upd_FileContent]
	@File_Name as varchar(50),
	@File_Content as image
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	DECLARE @valid as tinyint 
	SET @Valid = 1
	
	IF (
		SELECT Count(1) FROM [dbEVS_File].[dbo].[ProfessionalSubmissionHeader_File]
		WHERE [File_Name] =  @File_Name
	) > 0
	BEGIN
		SET @valid = 0
		RAISERROR('00011', 16, 1)
		return @@error
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	IF @valid = 1
	BEGIN
		INSERT INTO [dbEVS_File].[dbo].[ProfessionalSubmissionHeader_File]
			([File_Name], [File_Content])
		VALUES
			(@File_Name, @File_Content)
	END
--UPDATE [dbo].[ProfessionalSubmissionHeader]

--SET 
--	[File_Content] = @File_Content 

--WHERE [File_Name] = @File_Name

END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalSubmissionHeader_upd_FileContent] TO HCVU
GO
