
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[proc_HCVUUserAC_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [proc_HCVUUserAC_add]
GO

-- =============================================
-- Author:		Billy Lam
-- Create date: 01 August 2008
-- Description:	Add HCVUUserAC
-- =============================================
-- =============================================
-- Modification	History
-- Modified by:		Dickson Law	
-- Modified date:	23 Jun 2017
-- CR No.			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Description:		Add [User_Password_Level] field
-- =============================================
CREATE Procedure [dbo].[proc_HCVUUserAC_add]
@User_ID	char(20)
, @User_Password	varchar(100)
, @User_Password_Level	INT
, @User_Name	varchar(48)
, @HKID	varchar(20)
, @Effective_Date	datetime
, @Expiry_Date		datetime
, @Suspended		char(1)
, @Account_Locked	char(1)
, @Create_By		varchar(20)
as

-- =============================================
-- Declaration
-- =============================================
declare @Create_Dtm	datetime

declare @Encrypt_Field1	varbinary(100)
declare @Encrypt_Field2 varbinary(100)

-- =============================================
-- Validation 
-- =============================================
if (select count(1) from HCVUUserAC
		where User_ID = @User_ID) <> 0
begin
	Raiserror('00012', 16, 1)
	return @@error
end

select @Create_Dtm = getdate()


OPEN SYMMETRIC KEY sym_Key
DECRYPTION BY ASYMMETRIC KEY asym_Key

	select @Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @HKID)
	select @Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @User_Name)
	
CLOSE SYMMETRIC KEY sym_Key

if (select count(1) from HCVUUserAC
		where Encrypt_Field1 = @Encrypt_Field1) <> 0
begin
	Raiserror('00013', 16, 1)
	return @@error
end

-- =============================================
-- Insert transaction
-- =============================================           
INSERT INTO [dbo].[HCVUUserAC]
           (
			[User_ID]
           ,[User_Password]
           --,[User_Name]
           --,[User_ID_No]
           ,[Last_Login_Dtm]
           ,[Last_Unsuccess_Login_Dtm]
           ,[Last_Pwd_Change_Dtm]
           ,[Password_Fail_Count]
           ,[Effective_Date]
           ,[Expiry_Date]
           ,[Suspended]
           ,[Account_Locked]
           ,[Create_By]
           ,[Create_Dtm]
           ,[Update_By]
           ,[Update_Dtm]
           ,[Encrypt_Field1]
           ,[Encrypt_Field2]
		   ,[User_Password_Level])
     VALUES
           (
           @User_ID
           ,@User_Password
           --,@User_Name
           --,@HKID
           ,null
           ,null
           ,null
           ,0
           ,@Effective_Date
           ,@Expiry_Date
           ,@Suspended
           ,@Account_Locked
           ,@Create_By
           ,@Create_Dtm
           ,@Create_By
           ,@Create_Dtm
           ,@Encrypt_Field1
           ,@Encrypt_Field2
           ,@User_Password_Level)

GO


GRANT EXECUTE ON [proc_HCVUUserAC_add] TO HCVU
GO

