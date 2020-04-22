if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[proc_HCSPUserAC_upd_Profile]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[proc_HCSPUserAC_upd_Profile]
GO



-- =============================================
-- Author:		Timothy LEUNG
-- Create date:		26-06-2008
-- Description:		Update Login Profile of HCSPUserAC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Stanley Chan
-- Modified date:	26 Jun 2009
-- Description:		add new Print Option field
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	15 Jun 2017
-- CR No.			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Description:		add field [SP_Password_Level, SP_IVRS_Password_Level]}
-- =============================================
CREATE PROCEDURE [dbo].[proc_HCSPUserAC_upd_Profile]
	@SP_ID char(8), 
	@SP_Password varchar(100),
	@Chg_SP_Password char(1),
	@SP_IVRS_Password varchar(100),
	@Chg_SP_IVRS_Password char(1),
	@Alias_Account varchar(20),
	@Chg_Alias_Account varchar(1),
	@Default_Language char(1),
	@Update_By varchar(20),
	@Print_Option char(1),
	@TSMP timestamp,
	@SP_Password_level int,
	@SP_IVRS_Password_level int
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
	declare @sysdtm datetime
	select @sysdtm = getdate()
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
	if @Chg_Alias_Account = 'Y' 
	begin
		update HCSPUserAC
		set 
		[Alias_Account] = @Alias_Account
		where SP_ID = @SP_ID
	end	

	if @Chg_SP_Password = 'Y' 
	begin
		update HCSPUserAC
		set 
		[SP_Password] = @SP_Password,
		[SP_Password_Level] = @SP_Password_Level,
		Password_Fail_Count = 0,
		Last_Pwd_Change_Dtm = @sysdtm
		where SP_ID = @SP_ID
	end	
	
	if @Chg_SP_IVRS_Password = 'Y' 
	begin
		update HCSPUserAC
		set 
		[SP_IVRS_Password] = @SP_IVRS_Password,
		[SP_IVRS_Password_Level] = @SP_IVRS_Password_Level,
		IVRS_Password_Fail_Count = 0,
		Last_IVRS_Pwd_Change_Dtm = @sysdtm
		where SP_ID = @SP_ID
	end

	if @Print_Option <> '' 
	begin
		update HCSPUserAC
		set 
		Default_Language = @Default_Language, 
		ConsentPrintOption = @Print_Option,
		Update_By = @SP_ID,
		Update_Dtm = @sysdtm
		where SP_ID = @SP_ID
	end
	else
	begin
			update HCSPUserAC
		set 
		Default_Language = @Default_Language, 
		Update_By = @SP_ID,
		Update_Dtm = @sysdtm
		where SP_ID = @SP_ID
	end	
END
go

grant execute on [dbo].[proc_HCSPUserAC_upd_Profile] to HCSP
go



