IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_FileDownload_get_ByGenID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_FileDownload_get_ByGenID]
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
-- Author:			Winnie SUEN
-- Create date:		26 May 2016
-- CR No.:			CRE15-016
-- Description:		Get Datadownload list by Generation ID and status
-- =============================================


CREATE PROCEDURE 	[dbo].[proc_FileDownload_get_ByGenID]
	 @Generation_ID			CHAR(12)	
	,@Download_Status		CHAR(1) = NULL

as
BEGIN
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
		[Generation_ID],
		[User_ID],
		[Download_Status],
		[Update_By],
		[Update_Dtm]

	FROM 
		[FileDownload]
					
	WHERE 
		[Generation_ID] = @Generation_ID
		AND (@Download_Status IS NULL OR 
			(@Download_Status = 'I' AND [Download_Status] = 'I') OR (@Download_Status <> 'I' AND [Download_Status] IN ('N', 'D')))
		
	ORDER BY
		[User_ID]



END
GO

GRANT EXECUTE ON [dbo].[proc_FileDownload_get_ByGenID] TO HCVU
GO
