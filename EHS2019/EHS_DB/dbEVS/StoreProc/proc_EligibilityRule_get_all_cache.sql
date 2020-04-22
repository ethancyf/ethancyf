IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EligibilityRule_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EligibilityRule_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
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
-- Create date: 21 Aug 2009
-- Description:	Retrieve all EligibilityRule 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	21 September 2009
-- Description:		Grant execute to HCPUBLIC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EligibilityRule_get_all_cache] 
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
	er.[Scheme_Code],
	er.[Scheme_Seq],
	er.[Subsidize_Code],
	er.[Rule_Group_Code],
	er.[Rule_Name],
	er.[Type],
	er.[Operator],
	er.[Value],
	er.[Unit],
	er.[Checking_Method],
	er.[Handling_Method],
	erm.[Function_Code],
	erm.[Severity_Code],
	erm.[Message_Code],
	erm.[ObjectName],
	erm.[ObjectName2],
	erm.[ObjectName3]
FROM
	[EligibilityRule] er LEFT OUTER JOIN [EligibilityRuleMessage] erm
		ON er.[Scheme_Code] = erm.[Scheme_Code] AND
			er.[Scheme_Seq] = erm.[Scheme_Seq] AND
			er.[Subsidize_Code] = erm.[Subsidize_Code] AND
			er.[Rule_Group_Code] = erm.[Rule_Group_Code] AND
			er.[Rule_Name] = erm.[Rule_Name] AND
			er.[Type] = erm.[Type]
ORDER BY [Rule_Group_Code] ASC

END
GO

GRANT EXECUTE ON [dbo].[proc_EligibilityRule_get_all_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_EligibilityRule_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_EligibilityRule_get_all_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_EligibilityRule_get_all_cache] TO WSEXT
Go