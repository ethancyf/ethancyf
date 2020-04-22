IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_FileGenerationQueue_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_FileGenerationQueue_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM		
-- Modified date:	11 Sep 2019
-- CR No.			CRE19-001
-- Description:		Grant EXECUTE right for role HCSP
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	3 July 2019
-- CR No.:			CRE18-015 (Enable PCV13 weekly report eHS(S)W003 upon request)
-- Description:		Add input Param @Schedule_Gen_Dtm
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	18 November 2015
-- CR No.:			CRE15-006
-- Description:		Rename of eHS
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 30 Jun 2008
-- Description:	Add File Generation Queue
-- =============================================

CREATE PROCEDURE [dbo].[proc_FileGenerationQueue_add]
	@Generation_ID as char(12),
	@File_ID as varchar(30),
	@In_Parm as ntext,
	@Output_File as varchar(100),
	@Status	as char(1),
	@File_Password as varchar(30),
	@Request_By as varchar(20),
	@File_Description as nvarchar(500),
	@Schedule_Gen_Dtm	as DATETIME
	
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

INSERT INTO [dbo].[FileGenerationQueue]
(
	[Generation_ID],
	[File_ID],
	[In_Parm],
	[Output_File],
	[File_Content],
	[Status],
	[File_Password],
	[Request_Dtm],
	[Request_By],
	[Start_dtm],
	[Complete_dtm],
	[File_Description],
	[Schedule_Gen_Dtm]
)
VALUES
(
	@Generation_ID,
	@File_ID,
	@In_Parm,
	@Output_File,
	Null,
	@Status,
	EncryptByKey(KEY_GUID('sym_Key'), @File_Password),
	GetDate(),
	@Request_By,
	NULL,
	NULL,
	@File_Description,
	@Schedule_Gen_Dtm
)

CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_FileGenerationQueue_add] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_FileGenerationQueue_add] TO HCSP
GO

