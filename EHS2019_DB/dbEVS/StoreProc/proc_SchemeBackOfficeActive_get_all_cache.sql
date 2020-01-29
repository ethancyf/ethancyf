IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeBackOfficeActive_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeBackOfficeActive_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	19 Jul 2018
-- CR No.			CRE17-016
-- Description:		Checking of PCD status during VSS enrolment
--					Add [Join_PCD_Compulsory] column
-- =============================================
-- =============================================
-- Modification History      
-- CR No.:			CRE16-021 Transfer VSS category to PCD
-- Modified by:		Winnie SUEN
-- Modified date:	20 Dec 2016
-- Description:		Grant permission to WSINT and HCPUBLIC for PCDInterface
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE16-002
-- Modified by:		Lawrence TSANG
-- Modified date:	5 August 2016
-- Description:		Add Allow_Non_Clinic_Setting
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE13-019-02
-- Modified by:		Winnie SUEN
-- Modified date:	31 December 2014
-- Description:		Add AllowFreeTextBankACNo
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
-- Description:		Get all active SchemeBackOffice
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Paul Yip
-- Modified date:   07 Aug 2009
-- Description:	    Grant Right to HCSP
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    
-- Modified date:   
-- Description:	    
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_SchemeBackOfficeActive_get_all_cache]
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
	, Scheme_Desc_CN
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
	, AllowFreeTextBankACNo
	, Allow_Non_Clinic_Setting
	, Join_PCD_Compulsory
	FROM	SchemeBackOffice
	WHERE	Record_Status='A'
	ORDER BY Display_Seq

END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeBackOfficeActive_get_all_cache] TO HCSP, HCVU, HCPUBLIC, WSINT
GO

