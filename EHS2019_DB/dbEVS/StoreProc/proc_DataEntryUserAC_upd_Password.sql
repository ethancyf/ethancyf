IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DataEntryUserAC_upd_Password]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[proc_DataEntryUserAC_upd_Password]
GO
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	15 Jun 2017
-- CR No.			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Description:		add field [Data_Entry_Password_Level]
-- =============================================
-- =============================================
-- Author:			Billy Lam
-- Create date:		14-07-2008
-- Description:		UPdate DataEntryUserAC password
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_DataEntryUserAC_upd_Password]
@SP_ID					CHAR(8)
, @Data_Entry_Account	VARCHAR(20)
, @Password				VARCHAR(100)
, @Data_Entry_Password_Level		INT
, @tsmp					TIMESTAMP
as

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
IF (SELECT tsmp FROM DataEntryUserAC
		WHERE SP_ID = @SP_ID AND Data_Entry_Account = @Data_Entry_Account) != @tsmp
BEGIN
	Raiserror('00011', 16, 1)
END

-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
UPDATE DataEntryUserAC
SET Data_Entry_Password = @Password
, Last_Pwd_Change_Dtm = getdate()
, Password_Fail_Count = 0
, Update_By = @Data_Entry_Account
, Update_Dtm = getdate()
, Data_Entry_Password_Level = @Data_Entry_Password_Level
WHERE SP_ID = @SP_ID
AND Data_Entry_Account = @Data_Entry_Account 

GO 

GRANT  EXECUTE  ON [dbo].[proc_DataEntryUserAC_upd_Password]  TO [HCSP]
GO