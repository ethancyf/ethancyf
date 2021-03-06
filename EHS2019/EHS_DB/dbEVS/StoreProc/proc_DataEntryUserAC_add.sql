if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[proc_DataEntryUserAC_add]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)਍ഀ
drop procedure [dbo].[proc_DataEntryUserAC_add]਍ഀ
GO਍ഀ
਍ഀ
/****** Object:  StoredProcedure [dbo].[proc_DataEntryUserAC_add]    Script Date: 07/17/2008 15:41:36 ******/਍ഀ
SET ANSI_NULLS ON਍ഀ
GO਍ഀ
SET QUOTED_IDENTIFIER ON਍ഀ
GO਍ഀ
਍ഀ
਍ഀ
-- =============================================਍ഀ
-- Author:		Timothy LEUNG਍ഀ
-- Create date: 23 Jun 2008਍ഀ
-- Description:	Add Data Entry Account ਍ഀ
-- =============================================਍ഀ
-- =============================================਍ഀ
-- Modification History਍ഀ
-- Modified by:		Stanley Chan਍ഀ
-- Modified date:	26 Jun 2009਍ഀ
-- Description:		add new Print Option field਍ഀ
-- =============================================਍ഀ
-- =============================================਍ഀ
-- Modification History਍ഀ
-- Modified by:		Marco CHOI਍ഀ
-- Modified date:	15 Jun 2017਍ഀ
-- CR No.			I-CRE16-007-02 (Refine system from CheckMarx findings)਍ഀ
-- Description:		add field [Data_Entry_Password_Level]}਍ഀ
-- =============================================਍ഀ
CREATE PROCEDURE [dbo].[proc_DataEntryUserAC_add]਍ഀ
	@SP_ID char(8),਍ഀ
	@Data_Entry_Account varchar(20),਍ഀ
	@Data_Entry_Password varchar(100),਍ഀ
	@Record_Status char(1),਍ഀ
	@Create_By varchar(20),਍ഀ
	@Update_By varchar(20),਍ഀ
	@Account_Locked char(1),਍ഀ
	@ConsentPrintOption char(1),਍ഀ
	@Data_Entry_Password_Level	INT਍ഀ
AS਍ഀ
BEGIN਍ഀ
	SET NOCOUNT ON;਍ഀ
-- =============================================਍ഀ
-- Declaration਍ഀ
-- =============================================਍ഀ
-- =============================================਍ഀ
-- Validation ਍ഀ
-- =============================================਍ഀ
-- =============================================਍ഀ
-- Initialization਍ഀ
-- =============================================਍ഀ
-- =============================================਍ഀ
-- Return results਍ഀ
-- =============================================਍ഀ
	insert into DataEntryUserAC਍ഀ
	(਍ഀ
	SP_ID, ਍ഀ
	Data_Entry_Account,਍ഀ
	Data_Entry_Password,਍ഀ
	Last_Pwd_Change_Dtm,਍ഀ
	Record_Status,਍ഀ
	Create_By,਍ഀ
	Create_Dtm,਍ഀ
	Update_By,਍ഀ
	Update_Dtm,਍ഀ
	Password_fail_Count,਍ഀ
	Account_Locked ,਍ഀ
	ConsentPrintOption,਍ഀ
	Data_Entry_Password_Level਍ഀ
	)਍ഀ
	values਍ഀ
	(਍ഀ
	@SP_ID, ਍ഀ
	@Data_Entry_Account,਍ഀ
	@Data_Entry_Password,਍ഀ
	getdate(),਍ഀ
	@Record_Status,਍ഀ
	@Create_By,਍ഀ
	getdate(),਍ഀ
	@Update_By,਍ഀ
	getdate(),਍ഀ
	0,਍ഀ
	@Account_Locked,਍ഀ
	@ConsentPrintOption,਍ഀ
	@Data_Entry_Password_Level਍ഀ
	)	਍ഀ
END਍ഀ
go਍ഀ
਍ഀ
grant execute on [dbo].[proc_DataEntryUserAC_add]  to HCSP਍ഀ
go਍ഀ
