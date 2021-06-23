IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PassportIssueRegion_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PassportIssueRegion_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Raiman Chong
-- Modified date:	27 May 2021
-- CR No.:			CRE20-0023
-- Description:		Select All PassportIssueRegion for document type passport 
-- =============================================


CREATE PROCEDURE [dbo].[proc_PassportIssueRegion_get_all_cache]
AS
BEGIN
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
		NATIONALITY_CD,
		NATIONALITY_DESC,
		NATIONALITY_CHINESE_DESC,
		NATIONALITY_CN_DESC,
		Display_Eng,
		Display_Chi,
		Display_CN
	FROM 
		[PassportIssueRegion] Ic WITH(NOLOCK)
		order by display_Seq, NATIONALITY_DESC
 
END
GO

GRANT EXECUTE ON [dbo].[proc_PassportIssueRegion_get_all_cache] TO HCSP

GRANT EXECUTE ON [dbo].[proc_PassportIssueRegion_get_all_cache] TO HCVU

GO


