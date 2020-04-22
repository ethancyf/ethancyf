IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MenuItem_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MenuItem_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[proc_MenuItem_get]
as

SELECT Function_Code
      ,Description	as Resource_Key
      ,Group_Name
      ,Display_Seq
      ,URL
      ,Parent_Item	  
  FROM MenuItem
where Record_Status = 'A'
order by Function_Code

GO
