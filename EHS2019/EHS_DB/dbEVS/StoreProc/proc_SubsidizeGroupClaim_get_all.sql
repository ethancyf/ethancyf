IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SubsidizeGroupClaim_get_all]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SubsidizeGroupClaim_get_all]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	24 May 2017
-- CR No.:			CRE16-026-03 (Add PCV13)
-- Description:		Add Column - [SubsidizeGroupClaim].[High_Risk_Option]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	30 Jan 2015
-- CR No.:				CRE13-019-02
-- Description:		Add Column -	[SubsidizeGroupClaim].[Consent_Form_Avail_Version]
--								Add Column -	[SubsidizeGroupClaim].[Consent_Form_Avail_Lang]
--								Add Column -	[SubsidizeGroupClaim].[Print_Option_Avail]
--								Add Column -	[SubsidizeGroupClaim].[Consent_Form_Avail_Version_CN]
--								Add Column -	[SubsidizeGroupClaim].[Consent_Form_Avail_Lang_CN]
--								Add Column -	[SubsidizeGroupClaim].[Print_Option_Avail_CN]
--								Remove Column -	[SubsidizeGroupClaim].[Consent_Form_Available]
--								Remove Column -	[SubsidizeGroupClaim].[PrintOption_Available]
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
-- CR No:			CRE13-018 Change Voucher Amount to 1 Dollar
-- Modified by:		Karl Lam
-- Modified date:	24 Jan 2014
-- Description:		Add Column - [SubsidizeGroupClaim].[CoPayment_Fee]
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE13-006 HCVS Ceiling
-- Modified by:		Karl Lam
-- Modified date:	13 Aug 2013
-- Description:		Remove Column - [SubsidizeGroupClaim].[Carry_Forward]
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE13-006 HCVS Ceiling
-- Modified by:		Tommy LAM
-- Modified date:	24 Jul 2013
-- Description:		Add Column - [SubsidizeGroupClaim].[Num_Subsidize_Ceiling]
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE13-001 EHAPP
-- Modified by:		Tommy LAM
-- Modified date:	19 Apr 2013
-- Description:		Add Column - [SubsidizeGroupClaim].[ClaimDeclaration_Available]
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE12-008-02 Allowing different subsidy level for each scheme at different date period
-- Modified by:		Twinsen CHAN
-- Modified date:	29 Nov 2012
-- Description:		Delete Column [SubsidizeGroupClaim].[Subsidize_Value]
--					Remove joining - Scheme_Seq between [SchemeClaim] and [SubsidizeGroupClaim]
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE12-008-01 Allowing different subsidy level for each scheme at different date period
-- Modified by:		Koala CHENG
-- Modified date:	19 Jun 2012
-- Description:		Add Result Field: Display_Code_For_Claim, Legend_Desc_For_Claim, Legend_Desc_For_Claim_Chi
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Vincent YUEN
-- Modified date:	26 Nov 2009
-- Description:		Add Result Field: Legend Description
-- =============================================
-- =============================================
-- Author:		Paul Yip
-- Create date: 05 Aug 2009
-- Description:	Retrieve the SubsidizeGroupClaim information
--				(Join with Subsidize & SubsidizeItem)
--				Filter will be done on middle tier
-- =============================================
CREATE PROCEDURE [dbo].[proc_SubsidizeGroupClaim_get_all] 
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
	SGC.[Scheme_Code],
	SGC.[Scheme_Seq],
	SGC.[Subsidize_Code],
	SGC.[Display_Seq],
	SGC.[Claim_Period_From],
	SGC.[Claim_Period_To],
	SGC.[Consent_Form_Compulsory],
	SGC.[Num_Subsidize],
	SGC.[Create_By],
	SGC.[Create_Dtm],
	SGC.[Update_By],
	SGC.[Update_Dtm],
	SGC.[Record_Status],
	SGC.[Last_Service_Dtm],
	S.[Display_Code],
	S.[Legend_Desc],
	S.[Legend_Desc_Chi],
	S.[Legend_Desc_CN],
	SI.[Subsidize_Item_Desc],
	SI.[Subsidize_item_Desc_Chi],
	SI.[Subsidize_Type],
	SI.[Subsidize_Item_Code],	
	SGC.[AdhocPrint_Available],
	SGC.[Display_Code_For_Claim],
	SGC.[Legend_Desc_For_Claim], 
	SGC.[Legend_Desc_For_Claim_Chi],
	SGC.[Legend_Desc_For_Claim_CN],
	SGC.[ClaimDeclaration_Available],
	SGC.[Num_Subsidize_Ceiling],
	SGC.[CoPayment_Fee],
	SGC.[Consent_Form_Avail_Version],
	SGC.[Consent_Form_Avail_Lang],
	SGC.[Print_Option_Avail],
	SGC.[Consent_Form_Avail_Version_CN],
	SGC.[Consent_Form_Avail_Lang_CN],
	SGC.[Print_Option_Avail_CN],
	SGC.[High_Risk_Option]
FROM
	[SubsidizeGroupClaim] SGC
		INNER JOIN [SchemeClaim] SC
			ON SGC.[Scheme_Code] = SC.[Scheme_Code]
		INNER JOIN [Subsidize] S
			ON SGC.[Subsidize_Code] = S.[Subsidize_Code]
		INNER JOIN [SubsidizeItem] SI 
			ON S.[Subsidize_Item_Code] = SI.[Subsidize_Item_Code]
ORDER BY SC.[Display_Seq] ASC, SC.[Scheme_Seq] ASC,  SGC.[Display_Seq] ASC

END
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeGroupClaim_get_all] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeGroupClaim_get_all] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeGroupClaim_get_all] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeGroupClaim_get_all] TO WSEXT
Go

