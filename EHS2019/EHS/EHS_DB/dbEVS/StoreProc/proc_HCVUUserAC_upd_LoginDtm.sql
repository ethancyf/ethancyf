IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCVUUserAC_upd_LoginDtm]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCVUUserAC_upd_LoginDtm]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Billy Lam
-- Create date:		05-05-2008
-- Description:		Update Login_Dtm of HCVUUserAC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE procedure [dbo].[proc_HCVUUserAC_upd_LoginDtm]
@User_ID	char(20)
, @Login_Status	char(1)
, @Suspend_Count	tinyint
as


-- =============================================
-- Declaration
-- =============================================
declare @current_dtm	datetime

select @User_ID = ltrim(rtrim(@User_ID))
select @current_dtm = getdate()

declare @Password_Fail_Count	tinyint

-- =============================================
-- Update Transcation
-- =============================================
if @Login_Status = 'S'
begin
	update HCVUUserAC
	set Last_Login_Dtm = @current_dtm
	, Update_By = @User_ID
	, Update_Dtm = getdate()
	, Password_Fail_Count = 0
	where User_ID = @User_ID	
end
else
begin
	select @Password_Fail_Count = Password_Fail_Count
	from HCVUUserAC
	where User_ID = @User_ID
	
	if @Password_Fail_Count < 255
	begin 
		select @Password_Fail_Count = @Password_Fail_Count + 1
	end	

	if @Password_Fail_Count >= @Suspend_Count
	begin
		update HCVUUserAC
		set Last_Unsuccess_Login_Dtm = @current_dtm
		, Update_By = @User_ID
		, Update_Dtm = getdate()
		, Password_Fail_Count = @Password_Fail_Count
		--, Suspended = 'Y'
		, Account_Locked = 'Y'
		where User_ID = @User_ID
	end
	else
	begin	
		update HCVUUserAC
		set Last_Unsuccess_Login_Dtm = @current_dtm
		, Update_By = @User_ID
		, Update_Dtm = getdate()
		, Password_Fail_Count = @Password_Fail_Count
		where User_ID = @User_ID
	end
end

GO

GRANT EXECUTE ON [dbo].[proc_HCVUUserAC_upd_LoginDtm] TO HCVU
GO
