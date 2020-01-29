IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_FileGenerationQueue_upd_status]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_FileGenerationQueue_upd_status]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Winnie SUEN
-- Create date: 25 May 2016
-- Description:	Update File Generation Queue Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_FileGenerationQueue_upd_status]
	@Generation_ID	char(12)
	,@Status		char(1)		-- P / E / C / I / T
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

UPDATE [dbo].[FileGenerationQueue]

SET 
	[Status] = @Status
	,[Start_dtm] = CASE @Status WHEN 'E' THEN NULL ELSE [Start_dtm] END
	,[Complete_dtm] = CASE WHEN @Status IN ('C', 'T') THEN GetDate() ELSE [Complete_dtm] END

WHERE [Generation_ID] = @Generation_ID

END

GO

GRANT EXECUTE ON [dbo].[proc_FileGenerationQueue_upd_status] TO HCVU
GO
