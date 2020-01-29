IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemMessage_get_cache_zh_cn]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemMessage_get_cache_zh_cn]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		9 December 2014
-- CR No.:			CRE13-019-01
-- Description:		Support Simplified Chinese
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

CREATE procedure [dbo].[proc_SystemMessage_get_cache_zh_cn]
@platform		char(2)
as

-- =============================================
-- Return results
-- =============================================
select 'SystemMessage' as resourceType
, function_code + '-' + severity_code + '-' + message_code as resourceKey
, CN_Description as resourceValue
from dbo.SystemMessage
where (Platform = '99' or Platform = @platform)
--where (function_code like '99%' or function_code like @platform + '%')

GO

GRANT EXECUTE ON [dbo].[proc_SystemMessage_get_cache_zh_cn] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SystemMessage_get_cache_zh_cn] TO HCSP
GO
