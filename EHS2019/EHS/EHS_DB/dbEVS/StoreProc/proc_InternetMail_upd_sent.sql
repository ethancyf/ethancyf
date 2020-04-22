IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InternetMail_upd_sent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InternetMail_upd_sent]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Jun 2008
-- Description:	Update Internet Mail to Sent
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_InternetMail_upd_sent]
	@System_Dtm as datetime,
	@Mail_ID as char(10),
	@Version as char(20)
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

Update [dbo].[InternetMail] Set 
	
	[Send_Status] = 'S',
	[Sent_Dtm] = GetDate()

WHERE
	[System_Dtm] = @System_Dtm
	AND [Mail_ID] = @Mail_ID
	AND [Version] = @Version 

END

GO

GRANT EXECUTE ON [dbo].[proc_InternetMail_upd_sent] TO HCVU
GO
