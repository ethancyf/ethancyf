IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_FileGenerationUserControl_Get_ByFileID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_FileGenerationUserControl_Get_ByFileID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		23 February 2011
-- Description:		Retrieve File Generation User Control by File ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_FileGenerationUserControl_Get_ByFileID]
	@File_ID	varchar(30)
AS BEGIN

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
		File_ID,
		Display_Seq,
		UC_ID,
		UC_Setting,
		Parameter_Suffix
	FROM
		FileGenerationUserControl
	WHERE
		File_ID = @File_ID
	ORDER BY
		Display_Seq
		
		
END
GO

GRANT EXECUTE ON [dbo].[proc_FileGenerationUserControl_Get_ByFileID] TO HCVU
GO
