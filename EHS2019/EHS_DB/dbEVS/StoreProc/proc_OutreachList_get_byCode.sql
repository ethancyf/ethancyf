IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_OutreachList_get_byCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_OutreachList_get_byCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Chris YIM
-- Modified date:	22 Mar 2021
-- Description:		Outreach List
-- =============================================
CREATE PROCEDURE [dbo].[proc_OutreachList_get_byCode]
	@Outreach_Code VARCHAR(10)
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
		[Outreach_Code],
		[Type], 
		[District], 
		[Outreach_Name_Eng], 
		[Outreach_Name_Chi], 
		[Address_Eng],
		[Address_Chi],
		[Record_Status],
		[Create_By], 
		[Create_Dtm],
		[Update_By], 
		[Update_Dtm], 
		[TSMP]
	FROM OutreachList
	WHERE [Outreach_Code] = @Outreach_Code

END
GO

GRANT EXECUTE ON [dbo].[proc_OutreachList_get_byCode] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_OutreachList_get_byCode] TO HCVU
GO
