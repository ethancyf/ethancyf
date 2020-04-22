IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_RoleTypeTaskList_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_RoleTypeTaskList_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[proc_RoleTypeTaskList_get]
as
SELECT Role_Type
      , TaskList_ID
FROM RoleTypeTaskList
order by Role_Type

GO
