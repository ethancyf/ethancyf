IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_eVaccination_VaccineCodeMapping_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_eVaccination_VaccineCodeMapping_get_all_cache]
GO

/****** Object:  StoredProcedure [dbo].[proc_eVaccination_VaccineCodeMapping_get_all_cache]    Script Date: 06/25/2010 10:51:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	05 Jan 2021
-- CR No.:			CRE20-0XX (Immu record)
-- Description:		Add column [Vaccine_Brand_ID_Source] & [Vaccine_Brand_ID_Target]
-- =============================================
-- =============================================
-- Author:		Koala Cheng
-- Create date: 13 Sep 2010
-- Description:	Retrieve all vaccine code mapping for 3rd party, e.g. HA
--				Mapping include english/chinese name for display
-- =============================================

CREATE PROCEDURE [dbo].[proc_eVaccination_VaccineCodeMapping_get_all_cache]
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
		[Vaccine_Code_Source],
		[Vaccine_Code_Target],
		[Vaccine_Code_Common],
		[Vaccine_Code_Desc],
		[Vaccine_Code_Desc_Chi],
		[For_Enquiry],
		[For_Bar],
		[For_Display],
		[Vaccine_Brand_ID_Source],
		[Vaccine_Brand_ID_Target]
	FROM
		[VaccineCodeMapping] WITH (NOLOCK)
	ORDER BY [Source_System], [Target_System], [Vaccine_Code_Source], [Vaccine_Brand_ID_Source], [Vaccine_Code_Target], [Vaccine_Brand_ID_Target]
END
GO

GRANT EXECUTE ON [dbo].[proc_eVaccination_VaccineCodeMapping_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_eVaccination_VaccineCodeMapping_get_all_cache] TO HCVU
GO

