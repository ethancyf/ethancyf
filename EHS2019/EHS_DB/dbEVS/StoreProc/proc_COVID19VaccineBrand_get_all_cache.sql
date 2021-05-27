IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineBrand_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_COVID19VaccineBrand_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	16 Apr 2021
-- CR No.:			CRE20-0023-43
-- Description:		Add column [Discharge_Window_Min]
-- =============================================
-- =============================================
-- Modification History
-- Created by:		Chris YIM
-- Created date:	26 Feb 2021
-- CR No.:			CRE20-0022
-- Description:		Immu Record
-- =============================================

CREATE PROCEDURE [dbo].[proc_COVID19VaccineBrand_get_all_cache]
AS
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
		VBD.[Brand_ID],
		VBD.[Brand_Name],
		[Brand_Name_Chi] = 
			CASE 
				WHEN VBD.[Brand_Name_Chi] IS NULL THEN VBD.[Brand_Name]
				WHEN VBD.[Brand_Name_Chi] = '' THEN VBD.[Brand_Name]
				ELSE VBD.[Brand_Name_Chi]
			END,
		VBD.[Brand_Trade_Name],
		[Brand_Trade_Name_Chi] = 
			CASE 
				WHEN VBD.[Brand_Trade_Name_Chi] IS NULL THEN VBD.[Brand_Trade_Name]
				WHEN VBD.[Brand_Trade_Name_Chi] = '' THEN VBD.[Brand_Trade_Name]
				ELSE VBD.[Brand_Trade_Name_Chi]
			END,
		VBD.[Brand_Printout_Name],
		[Brand_Printout_Name_Chi] = 
			CASE 
				WHEN VBD.[Brand_Printout_Name_Chi] IS NULL THEN VBD.[Brand_Printout_Name]
				WHEN VBD.[Brand_Printout_Name_Chi] = '' THEN VBD.[Brand_Printout_Name]
				ELSE VBD.[Brand_Printout_Name_Chi]
			END,
		VBD.[HK_Reg_No],
		VBD.[Manufacturer],
		VBD.[Vaccination_Window_Min],
		VBD.[Vaccination_Window_Max],
		VBD.[Discharge_Window_Min]
	FROM 
		[COVID19VaccineBrandDetail] VBD WITH(NOLOCK)
	ORDER BY 
		VBD.[Display_Seq]

END
GO

GRANT EXECUTE ON [dbo].[proc_COVID19VaccineBrand_get_all_cache] TO HCSP

GRANT EXECUTE ON [dbo].[proc_COVID19VaccineBrand_get_all_cache] TO HCVU

GO


