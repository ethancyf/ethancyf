IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_CMSVaccinationRecord_Stat_Write_ByDtm]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_CMSVaccinationRecord_Stat_Write_ByDtm]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		10 November 2010
-- Description:		Statistics for CMS Vaccination Record
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_CMSVaccinationRecord_Stat_Write_ByDtm]
	@Start_Dtm	datetime,
	@End_Dtm	datetime
AS BEGIN

	EXEC proc_EHS_CMSVaccinationRecordTransaction_Stat_Write_ByDtm @Start_Dtm, @End_Dtm
	
	EXEC proc_EHS_CMSVaccinationRecordTime_Stat_Write_ByDtm @Start_Dtm, @End_Dtm

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_CMSVaccinationRecord_Stat_Write_ByDtm] TO HCVU
GO
