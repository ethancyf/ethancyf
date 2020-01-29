IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_FileGenerationQueue_get_ToRun]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_FileGenerationQueue_get_ToRun]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	3 July 2019
-- CR No.:			CRE18-015 (Enable PCV13 weekly report eHS(S)W003 upon request)
-- Description:		Add criteria, filter out report which [Schedule_Gen_Dtm] is later than current date
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE15-016 Randomly genereate the valid claim transaction
-- Modified by:		Winnie SUEN
-- Modified date:	25 May 2016
-- Description:		Add criteria, the report must be waited by someone
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE13-016 Upgrade to excel 2007
-- Modified by:		Tommy Lam
-- Modified date:	15 Oct 2013
-- Description:		Support multiple [File_Type] input
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 30 Jun 2008
-- Description:	Retrieve File Generation Queue To Be Run
-- =============================================

CREATE PROCEDURE [dbo].[proc_FileGenerationQueue_get_ToRun]
	@File_Type_List as varchar(500)
	
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================

	DECLARE @delimiter varchar(5)

	DECLARE @FileType table (
		File_Type	varchar(50)
	)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================

	SET @delimiter = ','

	INSERT INTO @FileType (
		File_Type
		)
	SELECT Item FROM func_split_string(@File_Type_List, @delimiter)

OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key
-- =============================================
-- Return results
-- =============================================

SELECT	
	FGQ.[Generation_ID],
	FGQ.[File_ID],
	FGQ.[In_Parm],
	FGQ.[Output_File],
	FGQ.[Status],
	convert(varchar,DecryptByKey(FGQ.[File_Password])) as File_Password ,
	FGQ.[Request_Dtm],
	FGQ.[Request_By],
	FGQ.[Start_dtm],
	FGQ.[Complete_dtm],
	FGQ.[File_Description],
	FGQ.[Schedule_Gen_Dtm]

FROM [dbo].[FileGenerationQueue] As FGQ, [dbo].[FileGeneration] AS FG

WHERE
	FGQ.[File_ID] = FG.[File_ID]
	AND (EXISTS (SELECT 1 FROM @FileType AS FT WHERE FG.[File_Type] = FT.[File_Type]))
	AND (( FGQ.[Status] = 'P' AND FGQ.[Start_dtm] IS NULL ) OR ( FGQ.[Status] = 'E' AND FGQ.[Start_dtm] IS NULL))
	AND (FGQ.Generation_ID NOT IN (SELECT Generation_ID FROM [FileDownload]) -- for aberrant report, no record in FileDownload yet
		OR EXISTS (SELECT 1 FROM [FileDownload] AS FD 
					WHERE FD.Generation_ID = FGQ.Generation_ID AND (FD.Download_Status <> 'I'))) -- for other reports, at least one user is waiting
	AND (FGQ.[Schedule_Gen_Dtm] IS NULL	OR FGQ.[Schedule_Gen_Dtm] <= GETDATE())
						
CLOSE SYMMETRIC KEY sym_Key


END

GO

GRANT EXECUTE ON [dbo].[proc_FileGenerationQueue_get_ToRun] TO HCVU
GO
