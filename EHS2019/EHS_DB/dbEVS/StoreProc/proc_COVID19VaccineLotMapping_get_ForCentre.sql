IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLotMapping_get_ForCentre]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_COVID19VaccineLotMapping_get_ForCentre]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- Created by:		Chris YIM
-- Created date:	16 Dec 2020
-- CR No.:			CRE20-00XX
-- Description:		Immu Record
-- =============================================

CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotMapping_get_ForCentre]
	@SP_ID CHAR(8),
	@Practice_Display_Seq SMALLINT = NULL
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
		DISTINCT 
		VLM.[Vaccine_Lot_ID],
		VLD.[Vaccine_Lot_No],
		VBD.[Brand_ID],
		VBD.[Brand_Name],
		[Brand_Name_Chi] = 
			CASE 
				WHEN VBD.[Brand_Name_Chi] IS NULL THEN VBD.[Brand_Name]
				WHEN VBD.[Brand_Name_Chi] = '' THEN VBD.[Brand_Name]
				ELSE VBD.[Brand_Name_Chi]
			END,
		[Display_Name] = VBD.[Brand_Name] + ' - ' + VLD.[Vaccine_Lot_No],
		[Display_Name_Chi] = 
			CASE 
				WHEN VBD.[Brand_Name_Chi] IS NULL THEN VBD.[Brand_Name] + ' - ' + VLD.[Vaccine_Lot_No]
				WHEN VBD.[Brand_Name_Chi] = '' THEN VBD.[Brand_Name] + ' - ' + VLD.[Vaccine_Lot_No]
				ELSE VBD.[Brand_Name_Chi] + ' - ' + VLD.[Vaccine_Lot_No]
			END,
		VBD.[Brand_Trade_Name],
		[Brand_Trade_Name_Chi] = 
			CASE 
				WHEN VBD.[Brand_Trade_Name_Chi] IS NULL THEN VBD.[Brand_Trade_Name]
				WHEN VBD.[Brand_Trade_Name_Chi] = '' THEN VBD.[Brand_Trade_Name]
				ELSE VBD.[Brand_Trade_Name_Chi]
			END,
		VLM.[SP_ID],
		VLM.[Practice_Display_Seq],
		VLM.[Record_Status],
		VLM.[Service_Period_From],
		VLM.[Service_Period_To]
	FROM 
		[COVID19VaccineLotMapping] VLM WITH(NOLOCK)
			INNER JOIN [VaccineCentreSPMapping] VCSP WITH(NOLOCK)
				ON VLM.[Centre_ID] = VCSP.[Centre_ID]
					AND (ISNULL(VLM.[Booth],'All') = 'All' OR VLM.[Booth] = VCSP.[Booth])
			INNER JOIN [COVID19VaccineLotDetail] VLD WITH(NOLOCK)
				ON VLM.[Vaccine_Lot_No] = VLD.[Vaccine_Lot_No]
			INNER JOIN [COVID19VaccineBrandDetail] VBD WITH(NOLOCK)
				ON VLD.[Brand_ID] = VBD.[Brand_ID]
	WHERE
		VLM.[Service_Type] = 'CENTRE'
		AND VLM.[Record_Status] = 'A'
		AND VLM.[Lot_Status] = 'A'
		AND VCSP.[SP_ID] = @SP_ID
		AND (@Practice_Display_Seq IS NULL OR VCSP.[Practice_Display_Seq] = @Practice_Display_Seq)
	ORDER BY 
		VBD.[Brand_Name],
		VLM.[Vaccine_Lot_ID]
END
GO

GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotMapping_get_ForCentre] TO HCSP

GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotMapping_get_ForCentre] TO HCVU

GO


