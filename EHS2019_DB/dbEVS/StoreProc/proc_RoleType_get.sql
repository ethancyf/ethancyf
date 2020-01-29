IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_RoleType_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_RoleType_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Billy Lam
-- Create date:		08-06-2008
-- Description:		Get RoleType
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE Procedure dbo.proc_RoleType_get
as

select Role_Type, Role_Description
from RoleType
where Record_Status = 'A'
order by Role_Type

GO

GRANT EXECUTE ON [dbo].[proc_RoleType_get] TO HCVU
GO
