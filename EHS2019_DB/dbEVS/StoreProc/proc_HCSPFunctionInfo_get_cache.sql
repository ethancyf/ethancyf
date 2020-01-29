IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPFunctionInfo_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPFunctionInfo_get_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Billy Lam
-- Create date:		05-05-2008
-- Description:		Get HCSPFunctionInfo for caching
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	07-07-2009
-- Description:		Add Column [MScheme_Code], [Effective_date], [Expiry_Date], [Record_Status]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Pak Ho LEE
-- Modified date:   05-Aug-2009
-- Description:	    Rename [MScheme_Code] -> [Scheme_Code] (of SchemeClaim)
-- =============================================
CREATE Procedure [dbo].[proc_HCSPFunctionInfo_get_cache]
as
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
	LOWER([Path]) Path,
	[Function_Code],
	[Role],
	[Scheme_Code],
	[Effective_date],
	[Expiry_Date]
FROM 
	[dbo].[HCSPFunctionInfo]
WHERE
	[Record_Status] = 'A'
ORDER BY [Function_Code] ASC

GO

GRANT EXECUTE ON [dbo].[proc_HCSPFunctionInfo_get_cache] TO HCSP
GO
