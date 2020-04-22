IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_upd_LoginDtm_NonLoginPage]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPUserAC_upd_LoginDtm_NonLoginPage]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE16-004 (Enable SP to unlock account)
-- Modified by:	    Winnie SUEN
-- Modified date:   28 Dec 2017
-- Description:		Handle fail count >= 255
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		05-05-2008
-- Description:		Update Fail Count of HCSPUserAC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   04-02-2009
-- Description:	    Not to reset the last login datetime when login success from non-login page
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   12-02-2009
-- Description:	    Not to reset the Password_Fail_Count to 0 even the status is successful
-- =============================================
CREATE procedure [dbo].[proc_HCSPUserAC_upd_LoginDtm_NonLoginPage]
@SP_ID	char(8)
, @Login_Status	char(1)
, @Suspend_Count	tinyint
as

-- =============================================
-- Declaration
-- =============================================
declare @current_dtm	datetime

select @SP_ID = ltrim(rtrim(@SP_ID))
select @current_dtm = getdate()

declare @Password_Fail_Count	tinyint

-- =============================================
-- Update Transcation
-- =============================================
if @Login_Status = 'S'	--successful
begin
	update HCSPUserAC
	set --Last_Login_Dtm = NULL,
	 Update_By = @SP_ID
	, Update_Dtm = getdate()
	--, Password_Fail_Count = 0
	--, Activation_Code = NULL
	where SP_ID = @SP_ID	
end
else	--fail
begin
	select @Password_Fail_Count = IsNull(Password_Fail_Count,0) 
	from HCSPUserAC
		where SP_ID = @SP_ID
	
	if @Password_Fail_Count < 255
	begin	
		select @Password_Fail_Count = @Password_Fail_Count + 1
	end		
	
	if @Password_Fail_Count >= @Suspend_Count
	begin
		update HCSPUserAC
		set Update_By = @SP_ID
		, Update_Dtm = getdate()
		, Password_Fail_Count = @Password_Fail_Count
		, Record_Status = 'S'
		where SP_ID = @SP_ID
	end
	else
	begin
		update HCSPUserAC
		set Update_By = @SP_ID
		, Update_Dtm = getdate()
		, Password_Fail_Count = @Password_Fail_Count
		where SP_ID = @SP_ID
	end	
end

GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_upd_LoginDtm_NonLoginPage] TO HCSP
GO
