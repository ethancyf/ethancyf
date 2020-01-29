IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SubsidizeGroupBackOffice_get_bySchemeCodeSchemeSeq]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SubsidizeGroupBackOffice_get_bySchemeCodeSchemeSeq]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History      
-- CR No.:			CRE16-021 Transfer VSS category to PCD
-- Modified by:		Winnie SUEN
-- Modified date:	20 Dec 2016
-- Description:		Grant permission to WSINT and HCPUBLIC for PCDInterface
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	15 Aug 2016
-- CR No.:			CRE16-002
-- Description:		Revamp VSS
-- =============================================
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
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	10 Dec 2009
-- Description:		Don't retrieve those subsidizegroup
--					which record_status = 'I'
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		04 Aug 2009
-- Description:		Get SubsidizeGroupBackOffice by Scheme code and scheme seq
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
-- =============================================
-- Modification History
-- Modified by:	    Paul YIP
-- Modified date:   07 Aug 2009
-- Description:	    Grant Right to HCSP
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_SubsidizeGroupBackOffice_get_bySchemeCodeSchemeSeq]
	@scheme_code char(10),
	@scheme_seq	smallint
AS BEGIN
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
	, s.Subsidy_Compulsory
	, SGT.Category_Code
	FROM
		SubsidizeGroupBackOffice s
			INNER JOIN SchemeBackOffice m
				ON s.Scheme_Code = m.Scheme_Code
					AND s.Scheme_Seq = m.Scheme_Seq
			LEFT JOIN SubsidizeGroupCategory SGT
				ON s.Scheme_Code = SGT.Scheme_Code
					AND s.Subsidize_Code = SGT.Subsidize_Code
	WHERE
		S.Scheme_Code = @scheme_code
			AND s.Scheme_seq = @scheme_seq
			and s.record_status <> 'I'
	ORDER BY
		s.Display_Seq


END
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeGroupBackOffice_get_bySchemeCodeSchemeSeq] TO HCSP, HCVU, HCPUBLIC, WSINT
GO

