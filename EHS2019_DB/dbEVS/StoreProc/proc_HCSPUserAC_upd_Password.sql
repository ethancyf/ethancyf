IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_upd_Password]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[proc_HCSPUserAC_upd_Password]
GO
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	15 Jun 2017
-- CR No.			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Description:		add field [SP_Password_Level]}
-- =============================================
-- =============================================
-- Author:			Clark YIP
-- Create date:		13-06-2008
-- Description:		Update Password of HCSPUserAC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_HCSPUserAC_upd_Password]
@sp_ID					char(8)
, @sp_Password			varchar(100)
, @sp_Password_level	INT
, @tsmp					timestamp
as

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM HCSPUserAC
		WHERE SP_ID = @sp_ID) != @tsmp
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@error
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
update HCSPUserAC
set [SP_Password] = @sp_Password
, SP_Password_Level = @sp_Password_level
, Password_Fail_Count = 0
, Last_Pwd_Change_Dtm = getdate()
, Update_By = @sp_ID
, Update_Dtm = getdate()
where sp_id = @sp_ID

GO

GRANT  EXECUTE  ON [dbo].[proc_HCSPUserAC_upd_Password]  TO [HCSP]
GO