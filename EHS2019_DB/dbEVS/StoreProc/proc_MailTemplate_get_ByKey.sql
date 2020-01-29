IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MailTemplate_get_ByKey]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MailTemplate_get_ByKey]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Jun 2008
-- Description:	Retrieve Mail Template By Key
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_MailTemplate_get_ByKey]
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

SELECT
	[Mail_ID], [Version], [Mail_Type], [Mail_Subject_Eng], [Mail_Subject_Chi],
	[Mail_Body_Eng], [Mail_Body_Chi], [Record_Status], 
	[Create_By], [Create_Dtm], [Update_By], [Update_Dtm]

FROM
	[dbo].[MailTemplate]

WHERE 
	[Mail_ID] = @Mail_ID And Version = @Version 

END

GO

GRANT EXECUTE ON [dbo].[proc_MailTemplate_get_ByKey] TO HCVU
GO
