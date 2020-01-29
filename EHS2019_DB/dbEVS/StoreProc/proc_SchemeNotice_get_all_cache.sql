IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeNotice_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeNotice_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	9 December 2014
-- CR No.:			CRE13-019-01
-- Description:		Support Simplified Chinese
-- =============================================
-- =============================================
-- CR No.: CRE13-024
-- Modified by:	Karl Lam
-- Modified date:	21 Feb 2014
-- Description:	Procedure to get all scheme notice
-- =============================================

--exec [proc_SchemeNotice_get_all_cache]

CREATE PROCEDURE [dbo].[proc_SchemeNotice_get_all_cache] 
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
	Scheme_Code,
	Notice_Seq,
	New_Period_From,
	New_Period_To,
	Display_Period_From,
	Display_Period_To,
	HTML_Content,
	HTML_Content_Chi,
	HTML_Content_CN
FROM
	SchemeNotice	
ORDER BY Scheme_Code, Notice_Seq 

END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeNotice_get_all_cache] TO HCSP
GO
