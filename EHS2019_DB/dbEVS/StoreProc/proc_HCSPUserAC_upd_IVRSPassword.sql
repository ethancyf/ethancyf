IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_upd_IVRSPassword]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[proc_HCSPUserAC_upd_IVRSPassword]
GO
-- =============================================
-- Author:		Timothy LEUNG
-- Create date:		26-06-2008
-- Description:		Update IVRS Password of HCSPUserAC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	15 Jun 2017
-- CR No.			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Description:		add field [SP_IVRS_Password_Level]}
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_HCSPUserAC_upd_IVRSPassword]
	@SP_ID	char(8),
	@SP_IVRS_Password	varchar(100),
	@sp_IVRS_Password_level	INT,
	@TSMP	timestamp 
AS
BEGIN
	SET NOCOUNT ON;	
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM HCSPUserAC
		WHERE SP_ID = @SP_ID) != @TSMP
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
	set [SP_IVRS_Password] = @SP_IVRS_Password
	, SP_IVRS_Password_Level = @sp_IVRS_Password_level
	, Last_IVRS_Pwd_Change_Dtm = getdate()
	, IVRS_Password_Fail_Count = 0
	, Update_By = @SP_ID
	, Update_Dtm = getdate()
	where SP_ID = @SP_ID

END

GO

GRANT  EXECUTE  ON [dbo].[proc_HCSPUserAC_upd_IVRSPassword]  TO [HCSP]
GO

