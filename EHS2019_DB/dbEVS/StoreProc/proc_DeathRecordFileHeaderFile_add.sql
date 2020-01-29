IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DeathRecordFileHeaderFile_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DeathRecordFileHeaderFile_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		27 April 2011
-- CR No.:			CRE11-007
-- Description:		Add the death record file
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_DeathRecordFileHeaderFile_add]
	@Death_Record_File_ID	char(15),
	@File_Content			image
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

	INSERT INTO [dbEVS_File].[dbo].[DeathRecordFileHeader_File] (
		Death_Record_File_ID,
		File_Content
	) VALUES (
		@Death_Record_File_ID,
		@File_Content
	)


END
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordFileHeaderFile_add] TO HCVU
GO
