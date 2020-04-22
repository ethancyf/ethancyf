IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemResource_get_cache_zh_cn]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemResource_get_cache_zh_cn]
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
-- Author:			Lawrence TSANG
-- Create date:		9 December 2014
-- CR No.:			CRE13-019-01
-- Description:		Support Simplified Chinese
-- =============================================

CREATE  procedure [dbo].[proc_SystemResource_get_cache_zh_cn]
@resourceType	varchar(50)
, @platform		char(2)
as

-- =============================================
-- Return results
-- =============================================
select ObjectType as resourceType
, ObjectName as resourceKey
, CN_Description as resourceValue
from dbo.SystemResource
where ObjectType = @resourceType
and (Platform = '99' or Platform = @platform)

GO

GRANT EXECUTE ON [dbo].[proc_SystemResource_get_cache_zh_cn] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SystemResource_get_cache_zh_cn] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SystemResource_get_cache_zh_cn] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SystemResource_get_cache_zh_cn] TO WSINT
GO