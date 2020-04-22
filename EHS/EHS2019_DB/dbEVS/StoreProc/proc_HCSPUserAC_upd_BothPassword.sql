IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_upd_BothPassword]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[proc_HCSPUserAC_upd_BothPassword]
GO
-- =============================================
-- Author:			Clark YIP
-- Create date:		30 Dec 2008
-- Description:		Update Both Password of HCSPUserAC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	15 Jun 2017
-- CR No.			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Description:		add field [SP_Password_Level], [SP_IVRS_Password_Level]}
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE procedure [dbo].[proc_HCSPUserAC_upd_BothPassword]
@sp_ID	char(8)
, @sp_Password	varchar(100)
, @SP_IVRS_Password	varchar(100)
, @sp_Password_level	INT
, @sp_IVRS_Password_level	INT
, @tsmp	timestamp
as
BEGIN
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
, [SP_IVRS_Password] = @SP_IVRS_Password
, SP_Password_Level = @sp_Password_level
, SP_IVRS_Password_Level = @sp_IVRS_Password_level
, Password_Fail_Count = 0
, IVRS_Password_Fail_Count = 0
, Last_Pwd_Change_Dtm = getdate()
, Last_IVRS_Pwd_Change_Dtm = getdate()
, Update_By = @sp_ID
, Update_Dtm = getdate()
where sp_id = @sp_ID

END

GO

GRANT  EXECUTE  ON [dbo].[proc_HCSPUserAC_upd_BothPassword]  TO [HCSP]
GO
