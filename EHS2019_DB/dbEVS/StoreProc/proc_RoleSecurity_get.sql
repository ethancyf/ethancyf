IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_RoleSecurity_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_RoleSecurity_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[proc_RoleSecurity_get]

as

SELECT Role_Type ,Function_Code  
FROM RoleSecurity
order by Role_Type, Function_Code

GO
