IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DeathRecordFileHeader_update_Processing]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DeathRecordFileHeader_update_Processing]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		28 April 2011
-- CR No.:			CRE11-007
-- Description:		Update DeathRecordFileHeader
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_DeathRecordFileHeader_update_Processing]
	@Death_Record_File_ID	char(15),
	@Processing				char(1)
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
	
	-- Update to start processing
	IF @Processing = 'Y' BEGIN

		UPDATE
			DeathRecordFileHeader
		SET
			Processing = @Processing
		WHERE
			Death_Record_File_ID = @Death_Record_File_ID
			AND Record_Status = 'P' -- Processing File
			AND Processing = 'N' -- Schedule Job no processing
	END
	-- Update to finished check duplicate entry and move death record from Staging to Entry table
	ELSE IF @Processing = 'E' BEGIN 

		UPDATE
			DeathRecordFileHeader
		SET
			Processing = @Processing
		WHERE
			Death_Record_File_ID = @Death_Record_File_ID
			AND Record_Status = 'P' -- Processing File
			AND Processing = 'Y' -- Schedule Job processing
	END
	-- Update to finished match death record and wrote to Match Result table
	ELSE IF @Processing = 'M' BEGIN

		UPDATE
			DeathRecordFileHeader
		SET
			Processing = @Processing
		WHERE
			Death_Record_File_ID = @Death_Record_File_ID
			AND Record_Status = 'P' -- Processing File
			AND Processing = 'E' -- Schedule Job finish check duplicate death record entry
	END
	ELSE IF @Processing = 'N' BEGIN

		UPDATE
			DeathRecordFileHeader
		SET
			Processing = @Processing
		WHERE
			Death_Record_File_ID = @Death_Record_File_ID	
	END
	SELECT @@ROWCOUNT
END
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordFileHeader_update_Processing] TO HCVU
GO
