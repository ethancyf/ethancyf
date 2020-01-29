IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DataEntryUserAcc_Maint_upd_StatusPwd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[proc_DataEntryUserAcc_Maint_upd_StatusPwd]
GO


-- =============================================
-- Author:		Timothy Leung
-- Create date: 23 Jun 2008
-- Description:	Update Data Entry Account (In Maint) Status and/or password
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	07 Aug 2009
-- Description:		Remove Print Option in Data Entry Account Maint
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	15 Jun 2017
-- CR No.			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Description:		add field [Data_Entry_Password_Level]}
-- =============================================

CREATE PROCEDURE [dbo].[proc_DataEntryUserAcc_Maint_upd_StatusPwd]
	@SP_ID char(8),
	@Data_Entry_Account varchar(20),
	@Data_Entry_Password varchar(100),
	@Record_Status char(1),
	@ChgPwd char(1),
	@Update_By varchar(20),
	@Account_Locked char(1),
	@Data_Entry_Password_Level		INT
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	IF @ChgPwd = 'Y' 
	BEGIN
		UPDATE [DataEntryUserAC]
		SET
			[Data_Entry_Password] = @Data_Entry_Password,
			[Data_Entry_Password_Level] = @Data_Entry_Password_Level,
			[Last_Pwd_Change_Dtm] = getdate(),
			[Password_Fail_Count] = 0,
			[Record_Status] = @Record_Status,
			[Update_By] = @Update_By,
			[Update_dtm] = getdate(),
			[Account_Locked] = @Account_Locked
		WHERE
			[SP_ID] = @SP_ID AND
			[Data_Entry_Account] = @Data_Entry_Account
	END
	ELSE
	BEGIN
		-- After Unlock Account, Reset Password Fail Count
		UPDATE
			[DataEntryUserAC]
		SET [Password_Fail_Count] = 0 
		WHERE
			[SP_ID] = @SP_ID AND
			[Data_Entry_Account] = @Data_Entry_Account AND
			[Account_Locked] = 'Y' AND @Account_Locked = 'N'
			

		UPDATE [DataEntryUserAC]
		SET
			[Record_Status] = @Record_Status,
			[Update_By] = @Update_By,
			[Update_dtm] = getdate(),
			[Account_Locked] = @Account_Locked
		WHERE
			[SP_ID] = @SP_ID AND 
			[Data_Entry_Account] = @Data_Entry_Account
	END
	
END
GO

GRANT  EXECUTE  ON [dbo].[proc_DataEntryUserAcc_Maint_upd_StatusPwd]  TO [HCSP]
GO
