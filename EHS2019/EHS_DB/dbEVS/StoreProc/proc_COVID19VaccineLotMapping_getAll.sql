IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLotMapping_getAll]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_COVID19VaccineLotMapping_getAll]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Created by:		Raiman Chong
-- Created date:	17 Feb 2021
-- CR No.:			CRE20-00XX
-- Description:		Immu Record
-- =============================================
-- =============================================
-- Modification History
-- Created by:		Chris YIM
-- Created date:	16 Dec 2020
-- CR No.:			CRE20-00XX
-- Description:		Immu Record
-- =============================================

CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotMapping_getAll]
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
		VLD.[Vaccine_Lot_No],
		VLD.[Brand_ID],
		VBD.[Brand_Name],
		[Brand_Name_Chi] = 
			CASE 
				WHEN VBD.[Brand_Name_Chi] IS NULL THEN VBD.[Brand_Name]
				WHEN VBD.[Brand_Name_Chi] = '' THEN VBD.[Brand_Name]
				ELSE VBD.[Brand_Name_Chi]
			END,
		[Display_Name] = 'COVID-19 vaccine (' + SUBSTRING(VBD.[Brand_Trade_Name],0,CHARINDEX(' ',VBD.[Brand_Trade_Name],0)) + ') (HK-' + VBD.[HK_Reg_No] + ')',
		[Display_Name_Chi] = N'2019冠狀病毒病疫苗',

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
		VLD.[Expiry_Date]
	FROM 
		[COVID19VaccineLotDetail] VLD WITH(NOLOCK)
			INNER JOIN [COVID19VaccineBrandDetail] VBD WITH(NOLOCK)
				ON VLD.[Brand_ID] = VBD.[Brand_ID]
	ORDER BY 
		VLD.[Vaccine_Lot_No]

END
GO

GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotMapping_getAll] TO HCSP

GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotMapping_getAll] TO HCVU

GO


