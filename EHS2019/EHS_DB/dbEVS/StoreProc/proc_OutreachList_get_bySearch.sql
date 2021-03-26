IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_OutreachList_get_bySearch]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_OutreachList_get_bySearch]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE proc_OutreachList_get_bySearch
	@Search_Wordings NVARCHAR(255),
	@Outreach_Type CHAR(5) = NULL
AS 
BEGIN
-- =============================================
-- Modification History
-- CR No.:			
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-023
-- Modified by:		Chris YIM
-- Modified date:	22 Mar 2021
-- Description:		Outreach List
-- =============================================

	DECLARE @IN_Search_Wordings NVARCHAR(255)

	SET @IN_Search_Wordings = '%' + @Search_Wordings + '%'

	SELECT 
		[Outreach_Code],
		[Type], 
		[District], 
		[Outreach_Name_Eng], 
		[Outreach_Name_Chi], 
		[Address_Eng], 
		[Address_Chi], 
		[Record_Status]
	FROM OutreachList
	WHERE 
		(
			([Outreach_Name_Eng] LIKE @IN_Search_Wordings OR [Outreach_Name_Chi] LIKE @IN_Search_Wordings)
			OR 
			([Outreach_Code] LIKE @IN_Search_Wordings)
			OR 
			([Address_Eng] LIKE @IN_Search_Wordings OR [Address_Chi] LIKE @IN_Search_Wordings)
		)
		AND [Record_Status] = 'A'
		AND ((@Outreach_Type IS NOT NULL AND [Type] = @Outreach_Type) OR @Outreach_Type IS NULL)	
END
GO

GRANT EXECUTE ON [dbo].[proc_OutreachList_get_bySearch] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_OutreachList_get_bySearch] TO HCVU
GO