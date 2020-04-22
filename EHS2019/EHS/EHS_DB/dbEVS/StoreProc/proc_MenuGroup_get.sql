IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MenuGroup_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MenuGroup_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
create procedure proc_MenuGroup_get
as

SELECT Group_Name
      ,Menu_Group_Desc
      ,Display_Seq
  FROM MenuGroup
where Record_Status = 'A'
order by Display_Seq

GO
