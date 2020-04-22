IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemMessage_get_zh_tw]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemMessage_get_zh_tw]
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
CREATE procedure [dbo].[proc_SystemMessage_get_zh_tw]
@platform		char(2)
as

select 'SystemMessage' as resourceType
, function_code + '-' + severity_code + '-' + message_code as resourceKey
, Chinese_Description as resourceValue
from systemmessage
where (function_code like '99%' or function_code like @platform + '%')

GO
