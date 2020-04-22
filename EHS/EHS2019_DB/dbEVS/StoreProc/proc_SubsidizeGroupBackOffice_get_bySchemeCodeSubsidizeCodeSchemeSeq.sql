IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SubsidizeGroupBackOffice_get_bySchemeCodeSubsidizeCodeSchemeSeq]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SubsidizeGroupBackOffice_get_bySchemeCodeSubsidizeCodeSchemeSeq]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:			Clark Yip
-- Create date:		05 Aug 2009
-- Description:		Get SubsidizeGroupBackOffice by Scheme code, subsidize code and scheme seq
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

CREATE PROCEDURE 	[dbo].[proc_SubsidizeGroupBackOffice_get_bySchemeCodeSubsidizeCodeSchemeSeq]
						@scheme_code char(10), 
						@subsidize_code char(10),
						@scheme_seq	smallint
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
	FROM	SubsidizeGroupBackOffice s, SchemeBackOffice m
	WHERE	s.Scheme_Code=@scheme_code and s.Scheme_seq=@scheme_seq and s.Subsidize_Code=@subsidize_code
			and s.Scheme_Code=m.Scheme_Code and s.Scheme_Seq=m.Scheme_Seq
	ORDER BY s.Display_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeGroupBackOffice_get_bySchemeCodeSubsidizeCodeSchemeSeq] TO HCVU
GO
