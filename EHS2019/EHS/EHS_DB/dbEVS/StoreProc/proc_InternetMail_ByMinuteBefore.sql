IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_InternetMail_ByMinuteBefore]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_InternetMail_ByMinuteBefore]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			
-- Modified by:	    
-- Modified date:   
-- Description:		
-- =============================================
-- =============================================
-- CR No.:			CRE16-004 (Enable SP to unlock account)
-- Author:			Winnie SUEN
-- Create date:		19 Dec 2017
-- Description:		Retreive same email submitted for same SP in X minute
-- =============================================


CREATE PROCEDURE [dbo].[proc_InternetMail_ByMinuteBefore]
	@SP_ID as char(8),
	@Mail_ID as char(10),
	@Minute_Before as Int
	
AS
BEGIN
	SET NOCOUNT ON;

-- =============================================
-- Configration
-- =============================================
	DECLARE @End_Dtm		datetime
	DECLARE @Start_Dtm		datetime
	SET @End_Dtm = GETDATE()
	SET @Start_Dtm = DATEADD(mi, -1 * @Minute_Before, @End_Dtm)

-- =============================================
-- Return results
-- =============================================

SELECT
	[System_Dtm], [Mail_ID], [Version], [Mail_Address], [Mail_Language],
	[Eng_Parameter], [Chi_Parameter], [Send_Status], [Sent_Dtm], [SP_ID]
FROM
(
	SELECT *
	FROM
		InternetMail
	UNION
	SELECT * 
	FROM
		InternetMailLog
) M
WHERE
	System_Dtm BETWEEN @Start_Dtm AND @End_Dtm
	AND SP_ID = @SP_ID
	AND Mail_ID = @Mail_ID

END

GO

GRANT EXECUTE ON [dbo].[proc_InternetMail_ByMinuteBefore] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_InternetMail_ByMinuteBefore] TO HCVU
GO
