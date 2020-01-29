IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ClaimCategoryEligibility_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ClaimCategoryEligibility_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	13 Jan 2010
-- Description:		Add Message & Resource handling
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 19 Sep 2009
-- Description:	Retrieve all Claim Category -> Eligibility
-- =============================================

CREATE PROCEDURE [dbo].[proc_ClaimCategoryEligibility_get_all_cache] 
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
		cce.[Scheme_Code],
		cce.[Scheme_Seq],
		cce.[Subsidize_Code],
		cce.[Category_Code],
		cce.[Rule_Group_Code],
		cce.[Rule_Name],
		cce.[Type],
		cce.[Operator],	
		cce.[Value],
		cce.[Unit],
		cce.[Checking_Method],
		cce.[Handling_Method],
		ccem.[Function_Code],
		ccem.[Severity_Code],
		ccem.[Message_Code],
		ccem.[ObjectName],
		ccem.[ObjectName2],
		ccem.[ObjectName3]
	FROM
		[ClaimCategoryEligibility] cce LEFT OUTER JOIN [ClaimCategoryEligibilityMessage] ccem
			ON cce.[Scheme_Code] = ccem.[Scheme_Code] AND
				cce.[Scheme_Seq] = ccem.[Scheme_Seq] AND
				cce.[Subsidize_Code] = ccem.[Subsidize_Code] AND
				cce.[Category_Code] = ccem.[Category_Code] AND
				cce.[Rule_Group_Code] = ccem.[Rule_Group_Code] AND
				cce.[Rule_Name] = ccem.[Rule_Name] AND
				cce.[Type] = ccem.[Type]
END
GO

GRANT EXECUTE ON [dbo].[proc_ClaimCategoryEligibility_get_all_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_ClaimCategoryEligibility_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ClaimCategoryEligibility_get_all_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_ClaimCategoryEligibility_get_all_cache] TO WSEXT
Go