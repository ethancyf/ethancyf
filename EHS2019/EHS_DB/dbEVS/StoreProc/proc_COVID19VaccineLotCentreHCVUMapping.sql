IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLotCentreHCVUMapping]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_COVID19VaccineLotCentreHCVUMapping]
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
-- Created by:		Raiman Chong
-- Created date:	16 Feb 2021
-- CR No.:			CRE20-00XX
-- Description:		Immu Record
-- =============================================


CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotCentreHCVUMapping]
	@User_ID VARCHAR(20)
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
		VCVUM.Centre_ID,
		VCVUM.[User_ID],
		VC.Centre_Name

	FROM 
		[VaccineCentreVUMapping] VCVUM WITH(NOLOCK)
			Left outer JOIN [VaccineCentre] VC WITH(NOLOCK)
				ON VCVUM.[Centre_ID] = VC.[Centre_ID]
	Where VCVUM.[User_ID] = @User_ID
	ORDER BY 
		VCVUM.Centre_ID
END
GO

GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotCentreHCVUMapping] TO HCSP

GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotCentreHCVUMapping] TO HCVU

GO


