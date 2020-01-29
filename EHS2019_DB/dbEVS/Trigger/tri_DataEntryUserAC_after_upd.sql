 IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_DataEntryUserAC_after_upd')
	BEGIN
		DROP  Trigger [dbo].[tri_DataEntryUserAC_after_upd]
	END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 27 Sept 2008
-- Description:	Trigger an insert statment into DataEntryUserACLOG
--				when a row is updated / inserted / deleted into DataEntryUserAC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Stanley CHAN
-- Modified date:	06 Feb 2009
-- Description:		Add ConsentPrintOption
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Marco CHOI
-- Modified date:	13 Jun 2017
-- CR No.			I-CRE17-007-02
-- Description:		Add Field [Data_Entry_Password_Level]
-- =============================================
CREATE TRIGGER [dbo].[tri_DataEntryUserAC_after_upd] 
   ON  [dbo].[DataEntryUserAC]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	
	SET NOCOUNT ON;

    INSERT INTO DataEntryUserACLOG
	(
		System_Dtm,
		SP_ID,
		Data_Entry_Account,
		Data_Entry_Password,
		Last_Login_Dtm,
		Last_Unsuccess_Login_Dtm,
		Last_Pwd_Change_Dtm,
		Password_Fail_Count,
		Record_Status,
		Account_Locked,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		ConsentPrintOption,
		Data_Entry_Password_Level
	)
	SELECT
		getdate(),
		SP_ID,
		Data_Entry_Account,
		Data_Entry_Password,
		Last_Login_Dtm,
		Last_Unsuccess_Login_Dtm,
		Last_Pwd_Change_Dtm,
		Password_Fail_Count,
		Record_Status,
		Account_Locked,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		ConsentPrintOption,
		Data_Entry_Password_Level
	FROM inserted

END

GO


