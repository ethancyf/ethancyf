IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SystemResource_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SystemResource_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE  procedure [dbo].[proc_SystemResource_get]
@resourceType	varchar(50)
, @cultureCode	varchar(10)
as

if @cultureCode = 'zh-tw'
begin
select ObjectType as resourceType
, @cultureCode as cultureCode
, ObjectName as resourceKey
, Chinese_Description as resourceValue
from dbo.SystemResource
where ObjectType = @resourceType
end
else
begin
select ObjectType as resourceType
, @cultureCode as cultureCode
, ObjectName as resourceKey
, Description as resourceValue
from dbo.SystemResource
where ObjectType = @resourceType
end

GO
