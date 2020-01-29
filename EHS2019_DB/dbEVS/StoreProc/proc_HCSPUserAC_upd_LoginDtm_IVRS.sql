IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_upd_LoginDtm_IVRS]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPUserAC_upd_LoginDtm_IVRS]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE16-004 (Enable SP to unlock account)
-- Modified by:	    Winnie SUEN
-- Modified date:   28 Dec 2017
-- Description:		Handle fail count wont + 1 when null
-- =============================================
-- =============================================
-- Author:			Tommy Cheung
-- Create date:		10-11-2008
-- Description:		Update IVRS Login Dtm, IVRS Fail Count of HCSPUserAC
-- =============================================

CREATE procedure [dbo].[proc_HCSPUserAC_upd_LoginDtm_IVRS]
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
if @Login_Status = 'S'
begin
	update HCSPUserAC
	set Last_IVRS_Login_Dtm = @current_dtm
	, Update_By = @SP_ID
	, Update_Dtm = getdate()
	, IVRS_Password_Fail_Count = 0
--	, Activation_Code = NULL
	where SP_ID = @SP_ID	
end
else
begin
	select @Password_Fail_Count = ISNULL(IVRS_Password_Fail_Count, 0)
	from HCSPUserAC
		where SP_ID = @SP_ID
		
	if @Password_Fail_Count < 255
	begin	
		select @Password_Fail_Count = @Password_Fail_Count + 1
	end

	if @Password_Fail_Count >= @Suspend_Count
	begin
		update HCSPUserAC
		set Last_IVRS_Unsuccess_login = @current_dtm
		, Update_By = @SP_ID
		, Update_Dtm = getdate()
		, IVRS_Password_Fail_Count = @Password_Fail_Count
		, IVRS_Locked = 'Y'
		where SP_ID = @SP_ID
	end
	else
	begin
		update HCSPUserAC
		set Last_IVRS_Unsuccess_login = @current_dtm
		, Update_By = @SP_ID
		, Update_Dtm = getdate()
		, IVRS_Password_Fail_Count = @Password_Fail_Count
		where SP_ID = @SP_ID
	end	
end

GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_upd_LoginDtm_IVRS] TO HCSP
GO
