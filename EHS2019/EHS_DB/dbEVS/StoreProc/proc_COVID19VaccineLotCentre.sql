IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19VaccineLotCentre]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_COVID19VaccineLotCentre]
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

CREATE PROCEDURE [dbo].[proc_COVID19VaccineLotCentre]
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
		VC.Centre_ID,
		VC.Centre_Name
	FROM 
		[VaccineCentre] VC WITH(NOLOCK)
	ORDER BY 
		VC.[Centre_ID]

END
GO

GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotCentre] TO HCSP

GRANT EXECUTE ON [dbo].[proc_COVID19VaccineLotCentre] TO HCVU

GO


