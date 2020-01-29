IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_FileGenerationQueue_get_ByFileIDFileName]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_FileGenerationQueue_get_ByFileIDFileName]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	3 July 2019
-- CR No.:			CRE18-015 (Enable PCV13 weekly report eHS(S)W003 upon request)
-- Description:		Retrieve [Schedule_Gen_Dtm]
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 07 Jul 2008
-- Description:	Retrieve File Generation Queue By FileID & File Name
-- =============================================

CREATE PROCEDURE [dbo].[proc_FileGenerationQueue_get_ByFileIDFileName]
	@File_ID as varchar(30),
	@File_Name as char(15)
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
	[Generation_ID], 
	[File_ID], 
	[In_Parm],
	[Output_File],
	[Status],
	[File_Password],
	[Request_Dtm],
	[Request_By],
	[Start_dtm],
	[Complete_dtm],
	[File_Description],
	[Schedule_Gen_Dtm]

FROM [dbo].[FileGenerationQueue]

WHERE 
	[File_ID] = @File_ID AND
	In_Parm Like '%' + @File_Name+ '%' AND
	Output_File Like '%' + @File_Name + '%' AND 
	Status = 'P'

END

GO

GRANT EXECUTE ON [dbo].[proc_FileGenerationQueue_get_ByFileIDFileName] TO HCVU
GO
