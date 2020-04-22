IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InternetMail_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InternetMail_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE16-004 (Enable SP to unlock account)
-- Modified by:	    Winnie SUEN
-- Modified date:   19 Dec 2017
-- Description:		Add [SP_ID]
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Jun 2008
-- Description:	Add Internet Mail
-- =============================================

CREATE PROCEDURE [dbo].[proc_InternetMail_add]
	@Mail_ID as char(10),
	@Version as char(20),
	@Mail_Address as varchar(255),
	@Mail_Language as char(1),
	@Eng_Parameter as varchar(2000),
	@Chi_Parameter as nvarchar(2000),
	@SP_ID as char(8)
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

INSERT INTO [dbo].[InternetMail]
(
	[System_Dtm], [Mail_ID], [Version], [Mail_Address], [Mail_Language],
	[Eng_Parameter], [Chi_Parameter], [Send_Status], [Sent_Dtm], [SP_ID]
)
VALUES
(
	GetDate(), @Mail_ID, @Version, @Mail_Address, @Mail_Language,
	@Eng_Parameter, @Chi_Parameter, 'P', Null, @SP_ID
)
END

GO

GRANT EXECUTE ON [dbo].[proc_InternetMail_add] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_InternetMail_add] TO HCVU
GO
