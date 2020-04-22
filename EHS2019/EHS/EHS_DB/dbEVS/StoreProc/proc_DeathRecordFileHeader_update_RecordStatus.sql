IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DeathRecordFileHeader_update_RecordStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DeathRecordFileHeader_update_RecordStatus]
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

CREATE PROCEDURE [dbo].[proc_DeathRecordFileHeader_update_RecordStatus]
	@Death_Record_File_ID	char(15),
	@Record_Status			char(1),
	@User_ID				varchar(20)
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
	
	IF @Record_Status = 'R' BEGIN
		UPDATE
			DeathRecordFileHeader
		SET
			Record_Status = @Record_Status,
			Remove_Dtm = GETDATE(),
			Remove_By = @User_ID
		WHERE
			Death_Record_File_ID = @Death_Record_File_ID
	
	END ELSE IF @Record_Status = 'P' BEGIN
		UPDATE
			DeathRecordFileHeader
		SET
			Record_Status = @Record_Status,
			Confirm_Dtm = GETDATE(),
			Confirm_By = @User_ID
		WHERE
			Death_Record_File_ID = @Death_Record_File_ID
	
	END ELSE BEGIN
		UPDATE
			DeathRecordFileHeader
		SET
			Record_Status = @Record_Status
		WHERE
			Death_Record_File_ID = @Death_Record_File_ID

	END
	

END
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordFileHeader_update_RecordStatus] TO HCVU
GO
