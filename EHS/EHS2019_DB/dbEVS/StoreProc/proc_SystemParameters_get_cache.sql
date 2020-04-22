IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemParameters_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemParameters_get_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Billy Lam
-- Create date:		28-07-2008
-- Description:		return SystemParameters
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	26-05-2009
-- Description:		Add Scheme Code: If Get By Empty Scheme Code, Retrieve 'All'
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================

CREATE  procedure [dbo].[proc_SystemParameters_get_cache]
AS

-- =============================================
-- Return results
-- =============================================

SELECT 
	[Parameter_Name], [Parm_Value1], [Parm_Value2], [Scheme_Code]
FROM
	[dbo].[SystemParameters]
WHERE
	[Record_Status] = 'A'
	AND External_Use = 'N'
	

GO

GRANT EXECUTE ON [dbo].[proc_SystemParameters_get_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SystemParameters_get_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SystemParameters_get_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SystemParameters_get_cache] TO WSEXT
GO