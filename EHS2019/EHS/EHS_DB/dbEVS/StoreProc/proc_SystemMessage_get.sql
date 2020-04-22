IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemMessage_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemMessage_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[proc_SystemMessage_get]
@cultureCode	varchar(10)
as
if @cultureCode = 'zh-tw'
begin

select 'SystemMessage' as resourceType
, @cultureCode as cultureCode
, function_code + '-' + severity_code + '-' + message_code as resourceKey
, Chinese_Description as resourceValue
from systemmessage

end
else
begin

select 'SystemMessage' as resourceType
, @cultureCode as cultureCode
, function_code + '-' + severity_code + '-' + message_code as resourceKey
, Description as resourceValue
from systemmessage

end

GO
