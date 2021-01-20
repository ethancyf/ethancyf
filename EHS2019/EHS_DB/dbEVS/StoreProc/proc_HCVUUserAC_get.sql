IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCVUUserAC_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCVUUserAC_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Author:			Billy Lam
-- Create date:		05-05-2008
-- Description:		Get HCVUUserAC for login
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   08-12-2008
-- Description:	    Add to get the field "Force_Pwd_Change"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	15 Jun 2017
-- CR No.			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Description:		add field [User_Password_Level]}
-- =============================================
CREATE procedure [dbo].[proc_HCVUUserAC_get]
@User_ID			char(20)
as
-- =============================================
-- Declaration
-- =============================================
declare @Last_Pwd_Change_Duration	int
declare @Last_Pwd_Change_Dtm	datetime

declare @Effective	char(1)
declare @Effective_Date	datetime
declare @Expiry_Date	datetime

declare @HCVUUser_ID	char(20)
declare	@Token_Cnt		int
declare @Suspended	char(1)

-- =============================================
-- Initialization
-- =============================================

--select @Effective = 'Y'
--select @Suspended = Null

select @Last_Pwd_Change_Dtm = [Last_Pwd_Change_Dtm]
, @Effective_Date = [Effective_Date]
, @Expiry_Date = [Expiry_Date]
, @Suspended = [Suspended]
, @HCVUUser_ID = User_ID
  FROM HCVUUserAC
where User_ID = @User_ID

if not @Last_Pwd_Change_Dtm is null
begin
	select @Last_Pwd_Change_Duration = datediff(dd, @Last_Pwd_Change_Dtm, getdate())
end
else
begin
	select @Last_Pwd_Change_Duration = null
end

if @Suspended is null
begin
	if datediff(dd, @Effective_Date, getdate()) < 0
	begin
		select @Suspended = 'Y'
	end
	if datediff(dd, @Expiry_Date, getdate()) >= 0
	begin
		select @Suspended = 'Y'
	end
end

select @Token_Cnt = count(1)
from Token
where User_ID = @HCVUUser_ID
and Record_Status = 'A'

-- =============================================
-- Return results
-- =============================================
EXEC [proc_SymmetricKey_open]

SELECT [User_ID]
      ,[User_Password]
      ,convert(varchar(40), DecryptByKey(Encrypt_Field2))  User_Name
      ,[Last_Pwd_Change_Dtm]
      ,[Last_Login_Dtm]
      ,[Last_Unsuccess_Login_Dtm]
      ,@Suspended					Suspended
      ,[Account_Locked]
      ,@Last_Pwd_Change_Duration	Last_Pwd_Change_Duration
      ,[Effective_Date]
      ,[Expiry_Date]
      ,@Token_Cnt				Token_Cnt
      ,[TSMP]
      ,Force_Pwd_Change
	  ,User_Password_Level Password_Level
  FROM HCVUUserAC
where User_ID = @User_ID

EXEC [proc_SymmetricKey_close]
GO

GRANT EXECUTE ON [dbo].[proc_HCVUUserAC_get] TO HCVU
GO

