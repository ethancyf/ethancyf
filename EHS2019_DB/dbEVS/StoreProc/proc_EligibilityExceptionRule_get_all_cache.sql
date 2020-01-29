IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EligibilityExceptionRule_get_all_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EligibilityExceptionRule_get_all_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	29 Aug 2016
-- CR No.			CRE16-002
-- Description:		Revamp VSS : Drop Column
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Derek LEUNG (added by Lawrence TSANG)
-- Modified date:	11 April 2011
-- Description:		Grant execute to WSEXT
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	13 Jan 2010
-- Description:		Add Message & Resource handling
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 09 Sep 2009
-- Description:	Retrieve all EligibilityExceptionRule 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EligibilityExceptionRule_get_all_cache] 
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

	eer.[Scheme_Code],
	eer.[Scheme_Seq],
	eer.[Subsidize_Code],
	eer.[Rule_Group_Code],
	eer.[Exception_Group_Code],
	eer.[Rule_Name],
	eer.[Type],
	eer.[Operator],
	eer.[Value],
	eer.[Unit],	
	eer.[Handling_Method],
	eerm.[ObjectName],
	eerm.[ObjectName2],
	eerm.[ObjectName3]
FROM
	[EligibilityExceptionRule] eer LEFT OUTER JOIN [EligibilityExceptionRuleMessage] eerm
		ON eer.[Scheme_Code] = eerm.[Scheme_Code] AND
			eer.[Scheme_Seq] = eerm.[Scheme_Seq] AND
			eer.[Subsidize_Code] = eerm.[Subsidize_Code] AND
			eer.[Rule_Group_Code] = eerm.[Rule_Group_Code] AND
			eer.[Exception_Group_Code] = eerm.[Exception_Group_Code] AND
			eer.[Rule_Name] = eerm.[Rule_Name] AND
			eer.[Type] = eerm.[Type]
ORDER BY [Rule_Group_Code] ASC, [Exception_Group_Code] ASC

END
GO

GRANT EXECUTE ON [dbo].[proc_EligibilityExceptionRule_get_all_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_EligibilityExceptionRule_get_all_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_EligibilityExceptionRule_get_all_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_EligibilityExceptionRule_get_all_cache] TO WSEXT
GO
