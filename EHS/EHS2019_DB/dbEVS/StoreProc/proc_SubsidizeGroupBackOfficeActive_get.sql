IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SubsidizeGroupBackOfficeActive_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SubsidizeGroupBackOfficeActive_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	19 June 2015
-- CR No.:			CRE15-004
-- Description:		Add field Subsidy_Compulsory 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	9 December 2014
-- CR No.:			CRE13-019-01
-- Description:		Support Simplified Chinese
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		04 Aug 2009
-- Description:		Get all active SubsidizeGroupBackOffice
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   05 Aug 2009
-- Description:	    Join the SchemeBackOffice to get the Scheme display code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   06 Aug 2009
-- Description:	    Add 2 fields (Service_Fee_Compulsory_Wording & Service_Fee_Compulsory_Wording_Chi)
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_SubsidizeGroupBackOfficeActive_get]
						
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
	s.Scheme_Code
	, s.Scheme_seq
	, s.Subsidize_code		
	, s.Display_Seq
	, s.Service_Fee_Enabled
	, s.Service_Fee_Compulsory	
	, s.Create_by
	, s.Create_dtm
	, s.Update_by
	, s.Update_dtm
	, s.Record_Status	
	, m.Display_Code as Scheme_Display_Code
	, s.Service_Fee_Compulsory_Wording
	, s.Service_Fee_Compulsory_Wording_Chi
	, s.Service_Fee_Compulsory_Wording_CN
	, Subsidy_Compulsory
	FROM	SubsidizeGroupBackOffice s, SchemeBackOffice m
	WHERE	s.Record_Status='A' and s.Scheme_Code=m.Scheme_Code and s.Scheme_Seq=m.Scheme_Seq
	ORDER BY s.Display_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeGroupBackOfficeActive_get] TO HCVU
GO
