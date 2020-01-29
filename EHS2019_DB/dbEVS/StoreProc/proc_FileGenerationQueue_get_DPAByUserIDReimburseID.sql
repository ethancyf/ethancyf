IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_FileGenerationQueue_get_DPAByUserIDReimburseID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_FileGenerationQueue_get_DPAByUserIDReimburseID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Clark YIP
-- Create date: 23 Aug 2008
-- Description:	Retrieve File Generation Queue of Detailed Payment Analysis Rpt By UserID & ReimburseID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_FileGenerationQueue_get_DPAByUserIDReimburseID]
	@File_ID as varchar(30),
	@File_Name as varchar(15),
	@User_ID as varchar(20)
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
	COUNT(1)

FROM [dbo].[FileGenerationQueue]

WHERE 
	[File_ID] = @File_ID AND
	In_Parm Like '%' + @File_Name+ '%' AND
	Output_File Like '%' +@File_Name+ '%' AND	
	Request_By = @User_ID AND
	Status <> 'I'

END

GO

GRANT EXECUTE ON [dbo].[proc_FileGenerationQueue_get_DPAByUserIDReimburseID] TO HCVU
GO
