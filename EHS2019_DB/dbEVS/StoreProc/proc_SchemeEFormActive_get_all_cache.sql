IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeEFormActive_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeEFormActive_get_all_cache]
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
-- CR No.:			CRE16-002 (VSS Revamp)
-- Modified by:		Chris YIM
-- Modified date:	05 Aug 2015
-- Description:		Add new column "Allow_Non_Clinic_Setting"
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 03 Aug 2009
-- Description:	Retrieve the active SchemeEFrom information
-- =============================================


CREATE PROCEDURE [dbo].[proc_SchemeEFormActive_get_all_cache] 
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

	select	Scheme_Code,
			Scheme_Seq,
			Scheme_Desc,
			Scheme_Desc_Chi,
			Display_code,
			Display_Seq,
			Service_Fee_Enabled,
			Eligible_Professional,
			Display_Subsidize_Desc,
			Enrol_Period_From,
			Enrol_Period_To,
			Create_by,
			Create_Dtm,
			Update_By,
			Update_Dtm,
			Record_Status,
			Allow_Non_Clinic_Setting,
			Join_PCD_Compulsory
	from schemeeform
	where record_status = 'A'
	order by display_seq

END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeEFormActive_get_all_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SchemeEFormActive_get_all_cache] TO HCVU
GO
