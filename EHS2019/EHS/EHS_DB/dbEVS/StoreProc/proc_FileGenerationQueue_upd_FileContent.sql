IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_FileGenerationQueue_upd_FileContent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_FileGenerationQueue_upd_FileContent]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 30 Jun 2008
-- Description:	Update File Generation Queue File Content
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_FileGenerationQueue_upd_FileContent]
	@Generation_ID as char(12),
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
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	INSERT INTO [dbEVS_File].[dbo].[FileGenerationQueue_File]
		(Generation_ID, File_Content)
	VALUES
		(@Generation_ID, @File_Content)
	
--UPDATE [dbo].[FileGenerationQueue]

--SET 
--	[File_Content] = @File_Content 

--WHERE [Generation_ID] = @Generation_ID

END

GO

GRANT EXECUTE ON [dbo].[proc_FileGenerationQueue_upd_FileContent] TO HCVU
GO
