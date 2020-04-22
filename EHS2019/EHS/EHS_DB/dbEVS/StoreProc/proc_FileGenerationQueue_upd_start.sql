IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_FileGenerationQueue_upd_start]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_FileGenerationQueue_upd_start]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 30 Jun 2008
-- Description:	Update File Generation Queue Start DateTime
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_FileGenerationQueue_upd_start]
	@Generation_ID as char(12)		
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF
	(
		SELECT Count(1) FROM [dbo].[FileGenerationQueue] WHERE
		[Generation_ID] = @Generation_ID AND [Start_dtm] IS NOT NULL
	) > 0
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@error
	END

-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

UPDATE [dbo].[FileGenerationQueue]

SET 
	[Start_dtm] = GetDate()

WHERE [Generation_ID] = @Generation_ID

END

GO

GRANT EXECUTE ON [dbo].[proc_FileGenerationQueue_upd_start] TO HCVU
GO
