IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_FileGeneration_get_ByKey]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_FileGeneration_get_ByKey]
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
-- Modified by:		Lawrence TSANG
-- Modified date:	2 November 2015
-- CR No.:			CRE15-006
-- Description:		Rename of eHS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	05 Feb 2010
-- Description:		retrieve Get_Data_From_Bak
-- ============================================= 
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 30 Jun 2008
-- Description:	Retrieve File Generation By File ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	02 Dec 2009
-- Description:		Also retrieve "XLS_Parameter"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_FileGeneration_get_ByKey]
	@File_ID as varchar(30)
	
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
	[File_ID],
	[File_Name],
	[File_Desc],
	[File_Name_Prefix],
	[File_Type],
	[Display_Code],
	[Output_File_Name],
	[Output_File_Description],
	[File_Prepare_Data_SP],
	[File_Data_SP],
	[FilterCriteria_UC],
	[Report_Template],
	[Group_ID],
	[Is_SelfAccess],
	[Auto_Generate],
	[Show_for_Generation],
	[Frequency],
	[Day_of_Generation],
	[Message_Subject],
	[Message_Content],
	[Create_Dtm],
	[Create_By],
	[Update_Dtm],
	[Update_By],
	[Get_Data_From_Bak],
	[XLS_Parameter]

FROM [dbo].[FileGeneration]

WHERE
	[File_ID] = @File_ID

END
GO

GRANT EXECUTE ON [dbo].[proc_FileGeneration_get_ByKey] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_FileGeneration_get_ByKey] TO HCSP
GO

