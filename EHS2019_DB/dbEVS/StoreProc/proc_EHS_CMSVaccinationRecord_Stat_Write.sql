IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_CMSVaccinationRecord_Stat_Write]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_CMSVaccinationRecord_Stat_Write]
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

CREATE PROCEDURE [dbo].[proc_EHS_CMSVaccinationRecord_Stat_Write]
AS BEGIN

-- =============================================
-- Declaration
-- =============================================
	DECLARE @Start_Dtm	datetime
	DECLARE @End_Dtm	datetime
	
	SET @End_Dtm = CONVERT(varchar(10), GETDATE(), 20)
	SET @Start_Dtm = DATEADD(dd, -1, @End_Dtm)


-- =============================================
-- Execution
-- =============================================
	EXEC proc_EHS_CMSVaccinationRecord_Stat_Write_ByDtm @Start_Dtm, @End_Dtm
	
END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_CMSVaccinationRecord_Stat_Write] TO HCVU
GO
