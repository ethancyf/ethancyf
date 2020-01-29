IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemMessage_get_cache]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemMessage_get_cache]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Billy Lam
-- Create date:		04-05-2008
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE procedure [dbo].[proc_SystemMessage_get_cache]
@platform		char(2)
as

-- =============================================
-- Return results
-- =============================================
select 'SystemMessage' as resourceType
, function_code + '-' + severity_code + '-' + message_code as resourceKey
, Description as resourceValue
from dbo.SystemMessage
where (Platform = '99' or Platform = @platform)
--where (function_code like '99%' or function_code like @platform + '%')

GO

GRANT EXECUTE ON [dbo].[proc_SystemMessage_get_cache] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[proc_SystemMessage_get_cache] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SystemMessage_get_cache] TO HCVU
GO
