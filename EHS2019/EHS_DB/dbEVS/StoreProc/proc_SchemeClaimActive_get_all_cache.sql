IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeClaimActive_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeClaimActive_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	27 April 2021
-- CR No.:			CRE20-023
-- Description:		Add Column    - [SchemeClaim].[AllowTempAccBOClaim]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	10 August 2018
-- CR No.:			CRE17-018
-- Description:		Add Column    - [SchemeClaim].[Readonly_HCSP]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	2 January 2015
-- CR No.:			CRE13-019-02
-- Description:		Extend HCVS to China
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
-- CR No:			CRE13-001 EHAPP
-- Modified by:		Tommy LAM, Koala CHENG
-- Modified date:	03 Apr 2013
-- Description:		Add Column - Reimbursement_Available, Confirmed_Transaction_Status
--					Add Column - PCSClaim_Available
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE12-008-02 Allowing different subsidy level for each scheme at different date period
-- Modified by:		Twinsen CHAN
-- Modified date:	27 Dec 2012
-- Description:		Add column - Control_Type, Control_Setting
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE12-001
-- Modified by:		Tony FUNG
-- Modified date:	07 Feb 2012
-- Description:		1. Grant permission to WSINT for PCDInterface
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 05 Aug 2009
-- Description:	Retrieve the active SchemeClaim information
--				Filter will be done on middle tier
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	06 Aug 2009
-- Description:		Add effective & expiry dtm
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	13 Aug 2009
-- Description:		Add PreFill Search Available
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	20 Aug 2009
-- Description:		Add TSWCheckingEnable
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	21 September 2009
-- Description:		Grant execute to HCPUBLIC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================

CREATE PROCEDURE [dbo].[proc_SchemeClaimActive_get_all_cache] 
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
	[Scheme_Code],
	[Scheme_Seq],
	[Scheme_Desc],
	[Scheme_Desc_Chi],	
	[Scheme_Desc_CN],	
	[Display_Code],
	[Display_Seq],
	[BalanceEnquiry_Available],
	[IVRS_Available],
	[TextOnly_Available],
	[Claim_Period_From],
	[Claim_Period_To],
	[Create_By],
	[Create_Dtm],
	[Update_By],
	[Update_Dtm],
	[Record_Status],
	[Effective_dtm],
	[Expiry_dtm],
	[PreFill_Search_Available],
	[TSWCheckingEnable],
	[Control_Type],
	[Control_Setting],
	[PCSClaim_Available],
	[Confirmed_Transaction_Status],
	[Reimbursement_Mode],
	[Reimbursement_Currency],
	[Available_HCSP_SubPlatform],
	[ProperPractice_Avail],
	[ProperPractice_SectionID],
	[Readonly_HCSP],
	[AllowTempAccBOClaim]
FROM
	[SchemeClaim]
WHERE
	[Record_Status] = 'A'
ORDER BY [Display_Seq] ASC

END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeClaimActive_get_all_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SchemeClaimActive_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SchemeClaimActive_get_all_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SchemeClaimActive_get_all_cache] TO WSEXT
GO

GRANT EXECUTE ON [dbo].[proc_SchemeClaimActive_get_all_cache] TO WSINT
GO

