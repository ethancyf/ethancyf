IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_eVaccination_HKMTTVaccineSeasonMapping_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_eVaccination_HKMTTVaccineSeasonMapping_get_all_cache]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
-- =============================================
-- Author:		Koala CHENG
-- Create date: 4 Jun 2018
-- Description:	Retrieve all HKMTT vaccine season mapping 
-- =============================================

CREATE PROCEDURE [dbo].[proc_eVaccination_HKMTTVaccineSeasonMapping_get_all_cache] 
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
		Source_System,				
		Target_System,				
		Vaccine_Type_Source,	
		Injection_Dtm_From_Source,
		Injection_Dtm_To_Source,	
		Vaccine_Code_Target,		
		Vaccine_Code_Desc,			
		Vaccine_Code_Desc_Chi,
		For_Bar,
		For_Display,
		Provider
	FROM	
		[HKMTTVaccineSeasonMapping] WITH (NOLOCK)
	ORDER BY 
		[Source_System], [Target_System], Vaccine_Type_Source, Injection_Dtm_From_Source
END
GO

GRANT EXECUTE ON [dbo].[proc_eVaccination_HKMTTVaccineSeasonMapping_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_eVaccination_HKMTTVaccineSeasonMapping_get_all_cache] TO HCVU
GO