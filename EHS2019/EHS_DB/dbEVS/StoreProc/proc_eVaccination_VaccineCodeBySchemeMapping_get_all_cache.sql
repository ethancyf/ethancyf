IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_eVaccination_VaccineCodeBySchemeMapping_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_eVaccination_VaccineCodeBySchemeMapping_get_all_cache]
GO

/****** Object:  StoredProcedure [dbo].[proc_eVaccination_VaccineCodeMapping_get_all_cache]    Script Date: 06/25/2010 10:51:06 ******/
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
-- Author:		Koala Cheng
-- Create date: 13 Sep 2010
-- Description:	Retrieve all vaccine code mapping by scheme for 3rd party, e.g. 
--				HA vaccine code (001-0910) map to (SIV|1|SIV),
--				Base on different scheme map to scheme code for eHS,
--				E.g. CIVSS: SIV|1|SIV = CIVSS|1|CIV
--					 EVSS : SIV|1|SIV = EVSS|1|EIV
-- =============================================

CREATE PROCEDURE [dbo].[proc_eVaccination_VaccineCodeBySchemeMapping_get_all_cache]
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
		[Vaccine_Code_Source],
		[Vaccine_Code_Target]
	FROM
		[VaccineCodeBySchemeMapping] WITH (NOLOCK)
	ORDER BY [Scheme_Code], [Vaccine_Code_Source]
END
GO

GRANT EXECUTE ON [dbo].[proc_eVaccination_VaccineCodeBySchemeMapping_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_eVaccination_VaccineCodeBySchemeMapping_get_all_cache] TO HCVU
GO

