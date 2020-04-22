IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SubsidizeGroupEForm_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SubsidizeGroupEForm_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No.:			CRE16-002 (VSS Revamp)
-- Modified by:		Chris YIM
-- Modified date:	05 Aug 2015
-- Description:		Add new column "Category"
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE15-004 (TIV and QIV)
-- Modified by:		Chris YIM
-- Modified date:	19 June 2015
-- Description:		Add new column "Subsidy_Compulsory"
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE12-001
-- Modified by:		Koala CHENG
-- Modified date:	28 Jan	2012
-- Description:		Allow HCVU to use for query EForm subsidize info for show Original Copy
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 03 Aug 2009
-- Description:	Retrieve the active subsidize group information for e-Enrolment
-- =============================================
CREATE PROCEDURE [dbo].[proc_SubsidizeGroupEForm_get_all_cache] 
		
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

	select	
		sg.scheme_code,
		sg.scheme_seq,
		sg.Subsidize_Code,
		sg.display_seq,
		sg.enrol_period_from,
		sg.enrol_period_to,
		sg.service_fee_enabled,
		sg.subsidy_compulsory,
		sg.service_fee_compulsory,
		sg.service_fee_appform_wording,
		sg.service_fee_appform_wording_chi,
		sg.Service_Fee_Compulsory_Wording,
		sg.Service_Fee_Compulsory_Wording_Chi,
		sg.display_subsidize_desc,
		sg.create_by,
		sg.create_dtm,
		sg.update_by,
		sg.update_dtm,
		sg.record_status,
		sg.display_code,
		sg.display_code_chi,
		sg.subsidize_item_desc,
		sg.subsidize_item_desc_chi,
		CC.Category_Name,
		CC.Category_Name_Chi,
		CC.Category_Name_CN,
		CC.Display_Seq AS [Category_Display_Seq]
	from subsidizegroupeform SG
		LEFT JOIN SubsidizeGroupCategory SGC
			ON SG.Subsidize_Code = SGC.Subsidize_Code
		LEFT JOIN ClaimCategory CC
			ON SGC.Category_Code = CC.Category_Code
	where sg.record_status = 'A'
	order by sg.scheme_code, sg.scheme_seq, CC.Display_Seq, sg.display_seq

END
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeGroupEForm_get_all_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SubsidizeGroupEForm_get_all_cache] TO HCVU
GO

