IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeSchemeInfoTSMP_get_bySPIDPracticeDisplaySeq]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeSchemeInfoTSMP_get_bySPIDPracticeDisplaySeq]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		7 August 2009
-- Description:		Get timestamp from table [PracticeSchemeInfo]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
CREATE PROCEDURE [dbo].[proc_PracticeSchemeInfoTSMP_get_bySPIDPracticeDisplaySeq]
	@sp_id					char(8),
	@practice_display_seq	smallint
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
	SELECT	Scheme_Code,
			Subsidize_Code,
			TSMP
	FROM	PracticeSchemeInfo
	WHERE	SP_ID = @sp_id
				AND Practice_Display_Seq = @practice_display_seq
				
END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInfoTSMP_get_bySPIDPracticeDisplaySeq] TO HCVU
GO
