IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[_proc_DataEntryUserACAll_get_byPractice]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[_proc_DataEntryUserACAll_get_byPractice]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO



create Procedure [dbo].[_proc_DataEntryUserACAll_get_byPractice]
@SP_ID	char(8)
, @SP_Practice_Display_Seq	smallint
as
begin
/*
select d.SP_ID
, d.Data_Entry_Account
, d.SP_Practice_Display_Seq
from DataEntryUserAC d
where d.SP_ID = @SP_ID
and (@SP_Practice_Display_Seq is null or d.SP_Practice_Display_Seq = @SP_Practice_Display_Seq)
*/

select distinct d.SP_ID
, d.Data_Entry_Account
from DataEntryUserAC d, DataEntryPracticeMapping p
where d.SP_ID = @SP_ID
and d.SP_ID = p.SP_ID
and d.Data_Entry_Account = p.Data_Entry_Account
and (@SP_Practice_Display_Seq is null or p.SP_Practice_Display_Seq = @SP_Practice_Display_Seq)
order by d.Data_Entry_Account
end
GO

GRANT EXECUTE ON [dbo].[_proc_DataEntryUserACAll_get_byPractice] TO HCSP
GO
