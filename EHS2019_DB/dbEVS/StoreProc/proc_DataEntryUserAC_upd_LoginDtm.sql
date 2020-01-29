IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DataEntryUserAC_upd_LoginDtm]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DataEntryUserAC_upd_LoginDtm]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Billy Lam
-- Create date: 31 July 2008
-- Description:	update login dtm of Data Entry Account
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE Procedure [dbo].[proc_DataEntryUserAC_upd_LoginDtm]
@SP_ID	char(8)
, @Data_Entry_Account	varchar(20)
, @Login_Status	char(1)
, @Suspend_Count	tinyint
as

-- =============================================
-- Declaration
-- =============================================
declare @current_dtm	datetime
declare @Password_Fail_Count	tinyint

-- =============================================
-- Initialization
-- =============================================
select @SP_ID = ltrim(rtrim(@SP_ID))
select @current_dtm = getdate()

-- =============================================
-- Update Transcation
-- =============================================

if @Login_Status = 'S'
begin
	update DataEntryUserAC
	set Last_Login_Dtm = @current_dtm
	, Update_By = @SP_ID
	, Update_Dtm = getdate()
	, Password_Fail_Count = 0
	where SP_ID = @SP_ID
	and Data_Entry_Account = @Data_Entry_Account	
end
else
begin
	select @Password_Fail_Count = Password_Fail_Count
	from DataEntryUserAC
		where SP_ID = @SP_ID
		and Data_Entry_Account = @Data_Entry_Account
	
	if @Password_Fail_Count < 255
	begin	
		select @Password_Fail_Count = @Password_Fail_Count + 1
	end

	if @Password_Fail_Count >= @Suspend_Count
	begin
		update DataEntryUserAC
		set Last_Unsuccess_Login_Dtm = @current_dtm
		, Update_By = @SP_ID
		, Update_Dtm = getdate()
		, Password_Fail_Count = @Password_Fail_Count
		, Account_Locked = 'Y'
		where SP_ID = @SP_ID
		and Data_Entry_Account = @Data_Entry_Account
	end
	else
	begin
		update DataEntryUserAC
		set Last_Unsuccess_Login_Dtm = @current_dtm
		, Update_By = @SP_ID
		, Update_Dtm = getdate()
		, Password_Fail_Count = @Password_Fail_Count
		where SP_ID = @SP_ID
		and Data_Entry_Account = @Data_Entry_Account
	end	
	
end

GO

GRANT EXECUTE ON [dbo].[proc_DataEntryUserAC_upd_LoginDtm] TO HCSP
GO
