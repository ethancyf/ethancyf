IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InternetMailLogRowCount_get_ByHour]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InternetMailLogRowCount_get_ByHour]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 26 Jun 2008
-- Description:	Retrieve Internet Mail Sent Count By Hour
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_InternetMailLogRowCount_get_ByHour]
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
	Count(1)
FROM
	[dbo].[InternetMailLog]

WHERE 
	DateDiff(hh, Sent_Dtm, GetDate()) = 0 AND Sent_Dtm <= GetDate()

END

GO

GRANT EXECUTE ON [dbo].[proc_InternetMailLogRowCount_get_ByHour] TO HCVU
GO
