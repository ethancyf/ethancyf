IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_RoleType_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_RoleType_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Modification History
-- Modified by:		Winnie SUEN
-- CR No.			CRE19-026 (HCVS hotline service) 
-- Modified date:	03 Feb 2020
-- Description:		Return [Available_HCVU_SubPlatform] from table RoleType
-- ============================================= 
-- =============================================
-- Author:			Billy Lam
-- Create date:		08-06-2008
-- Description:		Get RoleType
-- =============================================

CREATE Procedure dbo.proc_RoleType_get
as

select Role_Type, Role_Description, Available_HCVU_SubPlatform
from RoleType
where Record_Status = 'A'
order by Role_Type

GO

GRANT EXECUTE ON [dbo].[proc_RoleType_get] TO HCVU
GO
