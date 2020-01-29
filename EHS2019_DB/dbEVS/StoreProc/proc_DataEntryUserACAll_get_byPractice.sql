IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DataEntryUserACAll_get_byPractice]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DataEntryUserACAll_get_byPractice]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[proc_DataEntryUserACAll_get_byPractice]
@SP_ID	char(8)
, @SP_Practice_Display_Seq	smallint
as

select d.SP_ID
, d.Data_Entry_Account
, d.SP_Practice_Display_Seq
from DataEntryUserAC d
where d.SP_ID = @SP_ID
and (@SP_Practice_Display_Seq is null or d.SP_Practice_Display_Seq = @SP_Practice_Display_Seq)

GO
