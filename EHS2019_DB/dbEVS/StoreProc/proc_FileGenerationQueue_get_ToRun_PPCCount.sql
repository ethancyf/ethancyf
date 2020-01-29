IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_FileGenerationQueue_get_ToRun_PPCCount]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_FileGenerationQueue_get_ToRun_PPCCount]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No:			
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
-- =============================================
-- Author:			Winnie SUEN
-- CR No:			CRE15-016 Randomly genereate the valid claim transaction
-- Create date:		27 May 2016
-- Description:		Get no. of Post Payment Check report is pending for generation
-- =============================================

CREATE PROCEDURE [dbo].[proc_FileGenerationQueue_get_ToRun_PPCCount]
	
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

SELECT	
	COUNT(1)

FROM [dbo].[FileGenerationQueue] As FGQ, [dbo].[FileGeneration] AS FG

WHERE
	FGQ.[File_ID] = FG.[File_ID]
	AND (FG.Show_for_Generation = 'P') -- Post Payment Check
	AND (( FGQ.[Status] = 'P' AND FGQ.[Start_dtm] IS NULL ) OR ( FGQ.[Status] = 'E' AND FGQ.[Start_dtm] IS NULL))
	AND (FGQ.Generation_ID NOT IN (SELECT Generation_ID FROM [FileDownload]) -- for aberrant report, no record in FileDownload yet
		OR EXISTS (SELECT 1 FROM [FileDownload] AS FD 
					WHERE FD.Generation_ID = FGQ.Generation_ID AND (FD.Download_Status <> 'I'))) -- for other reports, at least one ppl is waiting
												
						


END

GO

GRANT EXECUTE ON [dbo].[proc_FileGenerationQueue_get_ToRun_PPCCount] TO HCVU
GO
