IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_InternetMail_after_upd_sent')
	DROP TRIGGER [dbo].[tri_InternetMail_after_upd_sent]
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
-- Description:	Move Internet Mail to InternetMail Log (After Image)
-- =============================================

CREATE Trigger [dbo].[tri_InternetMail_after_upd_sent]
	ON [dbo].[InternetMail]
	AFTER UPDATE
AS
BEGIN
	SET NOCOUNT ON;

-- deleted: old values
-- inserted: new values

INSERT INTO [dbo].[InternetMailLOG]
(
	[System_Dtm],
	[Mail_ID],
	[Version],
	[Mail_Address],
	[Mail_Language],
	[Eng_Parameter],
	[Chi_Parameter],
	[Send_Status],
	[Sent_Dtm],
	[SP_ID]
)
(
	SELECT
		[System_Dtm],
		[Mail_ID],
		[Version],
		[Mail_Address],
		[Mail_Language],
		[Eng_Parameter],
		[Chi_Parameter],
		[Send_Status],
		[Sent_Dtm],
		[SP_ID]
	FROM inserted

	WHERE [Sent_Dtm] IS NOT NULL AND [Send_Status] = 'S'
)

DELETE FROM [dbo].[InternetMail]

WHERE [Sent_Dtm] IS NOT NULL AND [Send_Status] = 'S'

END 
GO
