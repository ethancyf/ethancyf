IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemResource_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemResource_get_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	31 Aug 2016
-- CR No.:			CRE16-002
-- Description:		Revamp VSS
-- =============================================
-- =============================================
-- Author:			Billy Lam
-- Create date:		04-05-2008
-- Description:		return SystemResource of en-US
-- =============================================

CREATE  procedure [dbo].[proc_SystemResource_get_cache]
@resourceType	varchar(50)
, @platform		char(2)
as

-- =============================================
-- Return results
-- =============================================
select ObjectType as resourceType
, ObjectName as resourceKey
, Description as resourceValue
from dbo.SystemResource
where ObjectType = @resourceType
and (Platform = '99' or Platform = @platform)

GO

GRANT EXECUTE ON [dbo].[proc_SystemResource_get_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SystemResource_get_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SystemResource_get_cache] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SystemResource_get_cache] TO WSINT
GO
