 IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_HCVUUserAC_after_upd')
	BEGIN
		DROP  Trigger [dbo].[tri_HCVUUserAC_after_upd]
	END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 27 Sept 2008
-- Description:	Trigger an insert statment into HCVUUserACLOG
--				when a row is updated / inserted / deleted into HCVUUserAC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	08-12-2008
-- Description:		Add the field Force_Pwd_Change
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Marco CHOI
-- Modified date:	13 Jun 2017
-- CR No.			I-CRE17-007-02
-- Description:		Add field [User_Password_Level]
-- =============================================
CREATE TRIGGER [dbo].[tri_HCVUUserAC_after_upd] 
   ON  [dbo].[HCVUUserAC]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	
	SET NOCOUNT ON;

    INSERT INTO HCVUUserACLOG
	(
		System_Dtm,
		[User_ID],
		User_Password,
		Last_Login_Dtm,
		Last_Unsuccess_Login_Dtm,
		Last_Pwd_Change_Dtm,
		Password_Fail_Count,
		Effective_Date,
		Expiry_Date,
		Suspended,
		Account_Locked,
		Create_By,
		Create_Dtm,
		Update_By,
		Update_Dtm,
		Encrypt_Field1,
		Encrypt_Field2,
		Force_Pwd_Change,
		User_Password_Level
	)
	SELECT
		getdate(),
		[User_ID],
		User_Password,
		Last_Login_Dtm,
		Last_Unsuccess_Login_Dtm,
		Last_Pwd_Change_Dtm,
		Password_Fail_Count,
		Effective_Date,
		Expiry_Date,
		Suspended,
		Account_Locked,
		Create_By,
		Create_Dtm,
		Update_By,
		Update_Dtm,
		Encrypt_Field1,
		Encrypt_Field2,
		Force_Pwd_Change,
		User_Password_Level
	FROM inserted

END

GO

