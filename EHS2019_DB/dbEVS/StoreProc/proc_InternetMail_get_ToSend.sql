IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InternetMail_get_ToSend]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InternetMail_get_ToSend]
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
-- Description:	Retrieve Internet Mail To Be Send with Record Count
-- =============================================

CREATE PROCEDURE [dbo].[proc_InternetMail_get_ToSend]
	@Record_count as int
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
	Top (@Record_count)
	[System_Dtm], [Mail_ID], [Version], [Mail_Address], [Mail_Language],
	[Eng_Parameter], [Chi_Parameter], [Send_Status], [Sent_Dtm], [SP_ID]

FROM
	[dbo].[InternetMail]

WHERE 
	Send_Status = 'P' And Sent_Dtm IS NULL

Order By System_Dtm ASC

END

GO

GRANT EXECUTE ON [dbo].[proc_InternetMail_get_ToSend] TO HCVU
GO
