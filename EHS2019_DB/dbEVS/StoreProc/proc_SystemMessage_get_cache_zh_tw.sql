IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemMessage_get_cache_zh_tw]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemMessage_get_cache_zh_tw]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	05 Sep 2019
-- CR No.			CRE19-001-04 (RVP Precheck 2019/20)
-- Description:		Grant access to HCVU
-- =============================================
-- =============================================
-- Author:			Billy Lam
-- Create date:		04-05-2008
-- Description:		
-- =============================================

CREATE procedure [dbo].[proc_SystemMessage_get_cache_zh_tw]
@platform		char(2)
as

-- =============================================
-- Return results
-- =============================================
select 'SystemMessage' as resourceType
, function_code + '-' + severity_code + '-' + message_code as resourceKey
, Chinese_Description as resourceValue
from dbo.SystemMessage
where (Platform = '99' or Platform = @platform)
--where (function_code like '99%' or function_code like @platform + '%')

GO

GRANT EXECUTE ON [dbo].[proc_SystemMessage_get_cache_zh_tw] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SystemMessage_get_cache_zh_tw] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SystemMessage_get_cache_zh_tw] TO HCVU
GO