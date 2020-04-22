IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DataEntryUserAcc_upd_StatusPwd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[proc_DataEntryUserAcc_upd_StatusPwd]
GO

-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	15 Jun 2017
-- CR No.			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Description:		add field [Data_Entry_Password_Level]}
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT15-0011 (Fix empty print option in HCSP Account)
-- Modified by:		Chris YIM
-- Modified date:	08 September 2015
-- Description:		Handle empty string of print option
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Stanley CHAN
-- Modified date: 	6 Feb 2009
-- Description:	Add print option
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 23 Jun 2008
-- Description:	Update Data Entry Account Status and/or password
-- =============================================

CREATE PROCEDURE [dbo].[proc_DataEntryUserAcc_upd_StatusPwd]
	@SP_ID char(8),
	@Data_Entry_Account varchar(20),
	@Data_Entry_Password varchar(100),
	@Record_Status char(1),
	@ChgPwd char(1),
	@Update_By varchar(20),
	@Account_Locked char(1),
	@ConsentPrintOption char(1),
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

	IF ISNULL(@ConsentPrintOption,'') <> '' 
	BEGIN
		UPDATE DataEntryUserAC
		SET ConsentPrintOption = @ConsentPrintOption
		WHERE
			SP_ID = @SP_ID and 
			Data_Entry_Account = @Data_Entry_Account
	END

	if @ChgPwd = 'Y' 
	begin
		update DataEntryUserAC
		set 
		Data_Entry_Password = @Data_Entry_Password,
		Data_Entry_Password_Level = @Data_Entry_Password_Level,
		Last_Pwd_Change_Dtm = getdate(),
		Password_Fail_Count = 0,
		Record_Status = @Record_Status,
		Update_By = @Update_By,
		Update_dtm = getdate(),
		Account_Locked = @Account_Locked
--		ConsentPrintOption = @ConsentPrintOption
		where 
		SP_ID = @SP_ID and 
		Data_Entry_Account = @Data_Entry_Account
	end
	else
	begin
	
		-- After Unlock Account, Reset Password Fail Count
		Update DataEntryUserAC 
			SET Password_Fail_Count = 0 
		WHERE
		SP_ID = @SP_ID and 
		Data_Entry_Account = @Data_Entry_Account AND
		Account_Locked = 'Y' AND @Account_Locked = 'N'
		

		update DataEntryUserAC
		set 
		Record_Status = @Record_Status,
		Update_By = @Update_By,
		Update_dtm = getdate(),
		Account_Locked = @Account_Locked
--		ConsentPrintOption = @ConsentPrintOption
		where 
		SP_ID = @SP_ID and 
		Data_Entry_Account = @Data_Entry_Account

	end
END
GO

GRANT EXECUTE ON [dbo].[proc_DataEntryUserAcc_upd_StatusPwd] TO HCSP
GO
