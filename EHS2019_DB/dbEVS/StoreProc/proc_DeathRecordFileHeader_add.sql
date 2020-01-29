IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DeathRecordFileHeader_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DeathRecordFileHeader_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		27 April 2011
-- CR No.:			CRE11-007
-- Description:		Add DeathRecordFileHeader
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_DeathRecordFileHeader_add]
	@Death_Record_File_ID	char(15),
	@Description			varchar(100),
	@Import_By				varchar(20)
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
	
	INSERT INTO DeathRecordFileHeader (
		Death_Record_File_ID,
		Description,
		Import_Dtm,
		Import_By,
		Confirm_Dtm,
		Confirm_By,
		Match_Dtm,
		Remove_Dtm,
		Remove_By,
		Record_Status,
		Processing
	) VALUES (
		@Death_Record_File_ID,
		@Description,
		GETDATE(),
		@Import_By,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		'C',
		'N'
	)


END
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordFileHeader_add] TO HCVU
GO
