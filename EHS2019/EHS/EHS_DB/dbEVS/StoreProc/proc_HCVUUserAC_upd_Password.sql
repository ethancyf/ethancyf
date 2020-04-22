IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCVUUserAC_upd_Password]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[proc_HCVUUserAC_upd_Password]
GO
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	15 Jun 2017
-- CR No.			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Description:		add field [User_Password_Level]}
-- =============================================
-- =============================================
-- Author:			Billy Lam
-- Create date:		05-05-2008
-- Description:		Update Password of HCSPUserAC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE procedure [dbo].[proc_HCVUUserAC_upd_Password]
@User_ID	char(20)
, @User_Password	varchar(100)
, @User_Password_Level	INT
, @Update_By		char(20)
, @tsmp				timestamp
as

-- =============================================
-- Validation 
-- =============================================
if (select tsmp from HCVUUserAC
		where User_ID = @User_ID) != @tsmp
begin
	Raiserror('00011', 16, 1)
	return @@error
end

-- =============================================
-- Update Transcation
-- =============================================
update HCVUUserAC
set User_Password = @User_Password
, User_Password_Level = @User_Password_Level
, Update_By = @Update_By
, Update_Dtm = getdate()
, Last_Pwd_Change_Dtm = getdate()
where User_ID = @User_ID
GO

GRANT  EXECUTE  ON [dbo].[proc_HCVUUserAC_upd_Password]  TO [HCVU]
GO

