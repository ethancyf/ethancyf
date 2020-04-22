IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_FileGenerationQueue_get_WithFileContent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_FileGenerationQueue_get_WithFileContent]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM		
-- Modified date:	09 Jul 2019
-- CR No.			CRE19-001
-- Description:		Grant EXECUTE right for role HCSP
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	3 July 2019
-- CR No.:			CRE18-015 (Enable PCV13 weekly report eHS(S)W003 upon request)
-- Description:		Retrieve [Schedule_Gen_Dtm]
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 30 Jun 2008
-- Description:	Retrieve File Generation Queue With File Content
-- =============================================

CREATE PROCEDURE [dbo].[proc_FileGenerationQueue_get_WithFileContent]
	@Generation_ID as char(12)	
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
	DD.[File_Content],
	--FGQ.[File_Content],
	FGQ.[Status],
	convert(varchar,DecryptByKey(FGQ.[File_Password])) as File_Password ,
	FGQ.[Request_Dtm],
	FGQ.[Request_By],
	FGQ.[Start_dtm],
	FGQ.[Complete_dtm],
	FGQ.[File_Description],
	FGQ.[Schedule_Gen_Dtm]

FROM [dbo].[FileGenerationQueue] As FGQ, [dbEVS_File].[dbo].[FileGenerationQueue_File] DD


WHERE
	FGQ.[Generation_ID] = @Generation_ID AND FGQ.[Generation_ID] = DD.[Generation_ID]

CLOSE SYMMETRIC KEY sym_Key

END

GO

GRANT EXECUTE ON [dbo].[proc_FileGenerationQueue_get_WithFileContent] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_FileGenerationQueue_get_WithFileContent] TO HCSP
GO