IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_VaccineCentreSPMapping_getAll]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_VaccineCentreSPMapping_getAll]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Created by:		
-- Created date:	
-- CR No.:			
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- Created by:		Chris YIM
-- Created date:	17 Feb 2021
-- CR No.:			CRE20-0022
-- Description:		Immu Record
-- =============================================


CREATE PROCEDURE [dbo].[proc_VaccineCentreSPMapping_getAll]
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
		VCSP.[Centre_ID],
		VCSP.[Booth],
		VCSP.[SP_ID],
		VCSP.[Practice_Display_Seq],
		VC.[Centre_Name],
		VC.[Centre_Name_Chi]
	FROM 
		[VaccineCentreSPMapping] VCSP WITH(NOLOCK)
			INNER JOIN [VaccineCentre] VC WITH(NOLOCK)
				ON VCSP.[Centre_ID] = VC.[Centre_ID]
	ORDER BY 
		VCSP.[Centre_ID], VCSP.[Booth], VCSP.[SP_ID], VCSP.[Practice_Display_Seq]

END
GO

GRANT EXECUTE ON [dbo].[proc_VaccineCentreSPMapping_getAll] TO HCSP

GRANT EXECUTE ON [dbo].[proc_VaccineCentreSPMapping_getAll] TO HCVU

GO

