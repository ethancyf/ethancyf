IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ClaimRule_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ClaimRule_get_all_cache]
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
-- Modified by:		Kathy LEE
-- Modified date:	13 Jul 2010
-- Description:		Grant right to HCVU
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	11 Jan 2010
-- Description:		Add Message & Resource handling
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 21 Aug 2009
-- Description:	Retrieve all EligibilityRule 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	02 Dec 2009
-- Description:		Add Column Checking Method
-- =============================================

CREATE PROCEDURE [dbo].[proc_ClaimRule_get_all_cache] 
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
	cr.[Scheme_Code],
	cr.[Scheme_Seq],
	cr.[Subsidize_Code],
	cr.[Rule_Name],
	cr.[Target],
	cr.[Dependence],
	cr.[Operator],
	cr.[Compare_Value],
	cr.[Compare_Unit],
	cr.[Check_From],
	cr.[Check_To],
	cr.[Type],
	cr.[Rule_Group],
	cr.[Handling_Method],
	cr.[Checking_Method],	
	crm.[Function_Code],
	crm.[Severity_Code],
	crm.[Message_Code],
	crm.[ObjectName],
	crm.[ObjectName2],
	crm.[ObjectName3]
	
FROM
	[ClaimRule] cr
		LEFT OUTER JOIN [ClaimRuleMessage] crm
			ON
				cr.[Scheme_Code] = crm.[Scheme_Code] AND
				cr.[Scheme_Seq] = crm.[Scheme_Seq] AND
				cr.[Subsidize_Code] = crm.[Subsidize_Code] AND
				cr.[Rule_Name] = crm.[Rule_Name] AND
				cr.[Rule_Group] = crm.[Rule_Group]			 
	
ORDER BY [Rule_Group] ASC

END
GO

GRANT EXECUTE ON [dbo].[proc_ClaimRule_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_ClaimRule_get_all_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_ClaimRule_get_all_cache] TO WSEXT
GO