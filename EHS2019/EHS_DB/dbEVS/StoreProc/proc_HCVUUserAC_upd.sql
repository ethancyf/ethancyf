IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCVUUserAC_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCVUUserAC_upd]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Modification History  
-- Modified by:  Golden Yang  
-- Modified date: 11 Jun 2020
-- Description:  Add User Name, Chinese Name ,Contact No, Gender  
-- =============================================    
-- =============================================  
-- Author:   Billy Lam  
-- Create date:  09-06-2008  
-- Description:  Update HCVUUserAC Info  
-- =============================================  

CREATE Procedure [dbo].[proc_HCVUUserAC_upd]  
@User_ID   char(20)  
, @User_Name  varchar(48)  
, @HKID   varchar(20)  
, @Effective_Date datetime  
, @Expiry_Date  datetime  
, @Suspended  char(1)  
, @Account_Locked char(1)  
, @Update_By  varchar(20)  
, @Chinese_Name  nvarchar(100)  
, @Gender   char(1)  
, @Contact_No  varchar(20)  
, @tsmp    timestamp   
as  
-- =============================================  
-- Declaration  
-- =============================================  
declare @tmp_Suspended char(1)  
declare @tmp_Account_Locked char(1)  
declare @tmp_Password_Fail_Count tinyint  
  
declare @Encrypt_Field1 varbinary(100)  
declare @Encrypt_Field2 varbinary(100)  
  
--Golden 20200611 Begin  
declare @Encrypt_Field3 varbinary(100)  
--Golden 20200611 End  
  
-- =============================================  
-- Validation   
-- =============================================  
if (select tsmp from HCVUUserAC  
  where User_ID = @User_ID) != @tsmp  
begin  
 Raiserror('00011', 16, 1)  
 return @@error  
end  
  
if (select [User_ID] from HCVUUserAC  
  where Encrypt_Field1 = @Encrypt_Field1) <> @User_ID  
begin  
 Raiserror('00013', 16, 1)  
 return @@error  
end  
  
-- =============================================  
-- Initialization  
-- =============================================  
OPEN SYMMETRIC KEY sym_Key  
DECRYPTION BY ASYMMETRIC KEY asym_Key  
  
 select @Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @HKID)  
 select @Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @User_Name)  
  
 --Golden 20200611 Begin  
 select @Encrypt_Field3 = EncryptByKey(KEY_GUID('sym_Key'), @Chinese_Name)  
 --Golden 20200611 End  
  
   
CLOSE SYMMETRIC KEY sym_Key  
  
select @tmp_Suspended = Suspended  
, @tmp_Account_Locked = Account_Locked  
, @tmp_Password_Fail_Count = Password_Fail_Count  
from HCVUUserAC  
where User_ID = @User_ID  
  
/*  
if not @tmp_Suspended is NULL  and @Suspended is NULL  
begin  
 select @tmp_Password_Fail_Count = 0  
end  
*/  
  
if @tmp_Account_Locked = 'Y' and @Account_Locked = 'N'  
begin  
 select @tmp_Password_Fail_Count = 0  
end  
  
declare @Update_Dtm as datetime  
select @Update_Dtm = getdate()  
  
-- =============================================  
-- Update Transcation  
-- =============================================  
update HCVUUserAC  
set   
--User_Name = @User_Name  
--, User_ID_No = @HKID  
Effective_Date =  @Effective_Date  
, Expiry_Date = @Expiry_Date  
, Suspended = @Suspended  
, Account_Locked = @Account_Locked  
, Password_Fail_Count = @tmp_Password_Fail_Count  
, Update_By = @Update_By  
, Update_Dtm = @Update_Dtm  
, Encrypt_Field1 = @Encrypt_Field1  
, Encrypt_Field2 = @Encrypt_Field2  
, Encrypt_Field3 = @Encrypt_Field3  
, Sex = @Gender
, Contact_No = @Contact_No
where User_ID = @User_ID  
  
  
GO

GRANT EXECUTE ON [dbo].[proc_HCVUUserAC_upd] TO HCVU
GO
