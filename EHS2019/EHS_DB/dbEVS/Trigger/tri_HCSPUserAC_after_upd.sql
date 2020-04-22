 IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_HCSPUserAC_after_upd')
	BEGIN
		DROP  Trigger [dbo].[tri_HCSPUserAC_after_upd]
	END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 27 Sept 2008
-- Description:	Trigger an insert statment into HCSPUserACLOG
--				when a row is updated / inserted / deleted into HCSPUserAC
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
-- Description:		Add field [SP_Password_Level], [SP_IVRS_Password_Level], [Activation_Code_Level]
-- =============================================
CREATE TRIGGER [dbo].[tri_HCSPUserAC_after_upd] 
   ON  [dbo].[HCSPUserAC]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	
	SET NOCOUNT ON;

    INSERT INTO HCSPUserACLOG
	(
		System_Dtm,
		SP_ID,
		SP_Password,
		SP_IVRS_Password,
		Alias_Account,
		Last_Login_Dtm,
		Last_Unsuccess_Login_Dtm,
		Last_Pwd_Change_Dtm,
		Password_Fail_Count,
		Last_IVRS_Login_Dtm,
		Last_IVRS_Unsuccess_login,
		Last_IVRS_Pwd_Change_Dtm,
		IVRS_Password_Fail_Count,
		Default_Language,
		Activation_Code,
		Record_Status,
		Create_By,
		Create_Dtm,
		Update_By,
		Update_Dtm,
		ConsentPrintOption,
		SP_Password_Level,
		SP_IVRS_Password_Level,
		Activation_Code_Level
	)
	SELECT
		getdate(),
		SP_ID,
		SP_Password,
		SP_IVRS_Password,
		Alias_Account,
		Last_Login_Dtm,
		Last_Unsuccess_Login_Dtm,
		Last_Pwd_Change_Dtm,
		Password_Fail_Count,	
		Last_IVRS_Login_Dtm,
		Last_IVRS_Unsuccess_login,
		Last_IVRS_Pwd_Change_Dtm,
		IVRS_Password_Fail_Count,
		Default_Language,
		Activation_Code,
		Record_Status,
		Create_By,
		Create_Dtm,
		Update_By,
		Update_Dtm,
		ConsentPrintOption,
		SP_Password_Level,
		SP_IVRS_Password_Level,
		Activation_Code_Level
	FROM inserted

END

GO
