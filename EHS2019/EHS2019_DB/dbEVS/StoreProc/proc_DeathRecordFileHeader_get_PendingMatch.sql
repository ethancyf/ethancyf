IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DeathRecordFileHeader_get_PendingMatch]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DeathRecordFileHeader_get_PendingMatch]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		04 May 2011
-- CR No.:			CRE11-007
-- Description:		Get outstading DeathRecordFileHeader to be matched by the schedule job
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_DeathRecordFileHeader_get_PendingMatch]
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
	
	SELECT
		Death_Record_File_ID, Processing
	FROM
		DeathRecordFileHeader
	WHERE
		Record_Status = 'P'
			AND Processing IN ('Y','E','M','N')
	ORDER BY
		Confirm_Dtm
		

END
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordFileHeader_get_PendingMatch] TO HCVU
GO
