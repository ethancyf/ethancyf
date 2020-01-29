IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_upd_LoginDtm]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPUserAC_upd_LoginDtm]
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
-- Modification History
-- CR No.:			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Modified by:		Winnie SUEN
-- Modified date:	20 Jun 2017
-- Description:		Add field "Activation_Code_Level"
-- =============================================
-- =============================================
-- Author:			Billy Lam
-- Create date:		05-05-2008
-- Description:		Update Login_Dtm of HCSPUserAC
-- =============================================

CREATE procedure [dbo].[proc_HCSPUserAC_upd_LoginDtm]
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
	set Last_Login_Dtm = @current_dtm
	, Update_By = @SP_ID
	, Update_Dtm = getdate()
	, Password_Fail_Count = 0
	, Activation_Code = NULL
	, Activation_Code_Level = NULL
	where SP_ID = @SP_ID	
end
else
begin
	select @Password_Fail_Count = ISNULL(Password_Fail_Count, 0) 
	from HCSPUserAC
		where SP_ID = @SP_ID
		
	if @Password_Fail_Count < 255
	begin	
		select @Password_Fail_Count = @Password_Fail_Count + 1
	end

	if @Password_Fail_Count >= @Suspend_Count
	begin
		update HCSPUserAC
		set Last_Unsuccess_Login_Dtm = @current_dtm
		, Update_By = @SP_ID
		, Update_Dtm = getdate()
		, Password_Fail_Count = @Password_Fail_Count
		, Record_Status = 'S'
		where SP_ID = @SP_ID
	end
	else
	begin
		update HCSPUserAC
		set Last_Unsuccess_Login_Dtm = @current_dtm
		, Update_By = @SP_ID
		, Update_Dtm = getdate()
		, Password_Fail_Count = @Password_Fail_Count
		where SP_ID = @SP_ID
	end	
end

GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_upd_LoginDtm] TO HCSP
GO
