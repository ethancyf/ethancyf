IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeVaccineDetail_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeVaccineDetail_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 21 Aug 2009
-- Description:	Retrieve all SchemeVaccineDetails (Vaccine Fee)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================

CREATE PROCEDURE [dbo].[proc_SchemeVaccineDetail_get_all_cache] 
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
	[Scheme_Seq],
	[Subsidize_Code],
	[Vaccine_Fee],
	[Vaccine_Fee_Display_Enabled],
	[Injection_Fee],
	[Injection_Fee_Display_Enabled]
FROM
	[SchemeVaccineDetail]

END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeVaccineDetail_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SchemeVaccineDetail_get_all_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SchemeVaccineDetail_get_all_cache] TO WSEXT
GO