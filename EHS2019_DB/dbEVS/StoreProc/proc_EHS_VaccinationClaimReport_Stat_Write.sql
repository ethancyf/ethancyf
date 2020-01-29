IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_VaccinationClaimReport_Stat_Write]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_VaccinationClaimReport_Stat_Write]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	4 November 2015
-- CR No.:			CRE15-006
-- Description:		Stored procedure not used anymore. Separate to each scheme
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		15 October 2009
-- Description:		Generate report for the Vaccination
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	21 October 2009
-- Description:		Just execute the stored procedure with parameter @Cutoff_Dtm
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	6 Nov 2009
-- Description:		1. Set @Cutoff_Dtm at 00:00
--					2. Set @Cutoff_Dtm as varchar
-- =============================================

/*
CREATE PROCEDURE [dbo].[proc_EHS_VaccinationClaimReport_Stat_Write] 
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
	--DECLARE @Cutoff_Dtm	datetime
	DECLARE @Cutoff_Dtm	varchar(20)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	--SET @Cutoff_Dtm = GETDATE()
	set @Cutoff_Dtm = convert(varchar, getdate(), 106) + ' 00:00'
-- =============================================
-- Return results
-- =============================================

	EXEC [proc_EHS_VaccinationClaimReport_Stat_Write_ByCutoffDtm] @Cutoff_Dtm
	
	select 'S' as [Result]

END
--GO

GRANT EXECUTE ON [dbo].[proc_EHS_VaccinationClaimReport_Stat_Write] TO HCVU
--GO
*/
