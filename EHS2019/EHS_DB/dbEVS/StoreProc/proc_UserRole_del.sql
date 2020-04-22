IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_UserRole_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_UserRole_del]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure dbo.proc_UserRole_del
@User_ID	varchar(20)
, @Role_Type	smallint
as

Delete from UserRole
where User_ID = @User_ID
and (@Role_Type is null or Role_Type = @Role_Type)

GO

GRANT EXECUTE ON [dbo].[proc_UserRole_del] TO HCVU
GO
