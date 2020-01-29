IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemResource_get_cache_zh_tw]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemResource_get_cache_zh_tw]
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
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Paul Yip	
-- Modified date:	05-08-2009
-- Description:		Grant execution right to VU (for sending email)
-- =============================================

CREATE  procedure [dbo].[proc_SystemResource_get_cache_zh_tw]
@resourceType	varchar(50)
, @platform		char(2)
as

-- =============================================
-- Return results
-- =============================================
select ObjectType as resourceType
, ObjectName as resourceKey
, Chinese_Description as resourceValue
from dbo.SystemResource
where ObjectType = @resourceType
and (Platform = '99' or Platform = @platform)

GO

GRANT EXECUTE ON [dbo].[proc_SystemResource_get_cache_zh_tw] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SystemResource_get_cache_zh_tw] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SystemResource_get_cache_zh_tw] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SystemResource_get_cache_zh_tw] TO WSINT
GO
