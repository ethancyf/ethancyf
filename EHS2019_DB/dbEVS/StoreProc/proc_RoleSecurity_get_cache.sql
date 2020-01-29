IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_RoleSecurity_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_RoleSecurity_get_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Dickson
-- Modified date:	16 August 2017
-- Description:		Obsolete table [BOFunctionSchemeMapping]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Timothy LEUNG
-- Modified date:		31 August 2009
-- Description:		Handle access right by Scheme_Code
-- =============================================
-- =============================================
-- Author:			Billy Lam
-- Create date:		05-05-2008
-- Description:		Get RoleSecurity for caching
-- =============================================

CREATE procedure [dbo].[proc_RoleSecurity_get_cache]
as

-- =============================================
-- Return results
-- =============================================

SELECT Role_Type ,Function_Code  
FROM dbo.RoleSecurity
order by Role_Type, Function_Code

/*
select r.role_type as Role_Type, r.function_code as Function_Code, b.scheme_code as Scheme_Code
from rolesecurity r, bofunctionschememapping b
where r.function_code = b.function_code
*/
GO

GRANT EXECUTE ON [dbo].[proc_RoleSecurity_get_cache] TO HCVU
GO
