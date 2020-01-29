IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeBackOffice_get_bySchemeCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeBackOffice_get_bySchemeCode]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:			Clark Yip
-- Create date:		04 Aug 2009
-- Description:		Get SchemeBackOffice by scheme code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    
-- Modified date:   
-- Description:	    
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_SchemeBackOffice_get_bySchemeCode]
						@scheme_code char(10)
as
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
	Scheme_Code
	, Scheme_Desc
	, Scheme_Desc_Chi
	, Display_Code
	, Display_Seq
	, Eligible_Professional
	, ReturnLogo_Enabled
	, Effective_dtm
	, Expiry_dtm
	, Create_by
	, Create_dtm
	, Update_by
	, Update_dtm
	, Record_Status
	, Scheme_Seq
	, Display_Subsidize_Desc
	FROM	SchemeBackOffice
	WHERE	Scheme_Code=@scheme_code
	ORDER BY Display_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeBackOffice_get_bySchemeCode] TO HCVU
GO
