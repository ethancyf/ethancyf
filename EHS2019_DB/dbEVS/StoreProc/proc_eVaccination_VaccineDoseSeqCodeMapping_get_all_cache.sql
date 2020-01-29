IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_eVaccination_VaccineDoseSeqCodeMapping_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_eVaccination_VaccineDoseSeqCodeMapping_get_all_cache]
GO

/****** Object:  StoredProcedure [dbo].[proc_eVaccination_VaccineDoseSeqCodeMapping_get_all_cache]    Script Date: 06/25/2010 10:51:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	28 Oct 2019
-- CR No.:			CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface)
-- Description:	  	Add Column [VaccineDoseSeqCodeMapping].[Subsidize_Item_Code_Source]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Koala CHENG
-- Modified date:	14 Jun 2018
-- CR No.:			CRE18-004 (CIMS Vaccination Sharing)
-- Description:	  	Add Column [VaccineDoseSeqCodeMapping].[Display_Source_Vaccine_Dose_Desc]
-- =============================================
-- =============================================
-- Author:		Koala Cheng
-- Create date: 13 Sep 2010
-- Description:	Retrieve all dose code mapping for 3rd party, e.g. HA
--				Mapping include english/chinese name for display
-- =============================================

CREATE PROCEDURE [dbo].[proc_eVaccination_VaccineDoseSeqCodeMapping_get_all_cache] 
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
		[Source_System],
		[Target_System],
		[Vaccine_Dose_Seq_Code_Source],
		[Vaccine_Dose_Seq_Code_Target],
		[Vaccine_Dose_Seq_Code_Common],
		[Vaccine_Dose_Seq_Code_Desc],
		[Vaccine_Dose_Seq_Code_Desc_Chi],
		[Display_Source_Vaccine_Dose_Desc],
		[Subsidize_Item_Code_Source]
	FROM
		[VaccineDoseSeqCodeMapping] WITH (NOLOCK)
	ORDER BY [Source_System],[Target_System],[Vaccine_Dose_Seq_Code_Source],[Vaccine_Dose_Seq_Code_Target]
END
GO

GRANT EXECUTE ON [dbo].[proc_eVaccination_VaccineDoseSeqCodeMapping_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_eVaccination_VaccineDoseSeqCodeMapping_get_all_cache] TO HCVU
GO

