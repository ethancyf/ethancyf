
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[proc_HCSPUserAC_udp_Activation]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [proc_HCSPUserAC_udp_Activation]
GO

-- =============================================
-- Author:			Clark YIP
-- Create date:		12-06-2008
-- Description:		Update HCSPUserAC
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Dickson Law
-- Modified date:	03 July 2017
-- CR No.			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Description:		Add [SP_Password_Level],[SP_IVRS_Password_Level] field
-- =============================================

CREATE PROCEDURE [dbo].[proc_HCSPUserAC_udp_Activation]
@sp_ID					char(8),
@sp_password			varchar(100),
@sp_password_level		INT,
@ivrs_password			varchar(100),
@ivrs_password_level	INT,
@alias					varchar(20),
@tsmp					timestamp
as
BEGIN
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM HCSPUserAC
		WHERE SP_ID = @sp_ID) != @tsmp
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

if @ivrs_password is not null
	begin
		UPDATE HCSPUserAC
		SET [SP_Password]=@sp_password
			  ,[SP_Password_Level]=@sp_password_level
			  ,[SP_IVRS_Password]=@ivrs_password
			  ,[SP_IVRS_Password_Level]=@ivrs_password_level
			  ,[Alias_Account]=@alias
			  ,[record_status]='A'
			  ,[Update_Dtm]=getdate()
			  ,[Update_by]=@sp_id  
			  ,[IVRS_Password_Fail_Count] = 0
			  ,[Password_Fail_Count]=0
			  ,[Last_Pwd_Change_Dtm]=getdate()
			  ,[Last_IVRS_Pwd_Change_Dtm]=getdate()
			  ,[Default_Language]='E'
		where SP_ID = @sp_ID and record_status = 'P' 
		and tsmp = @tsmp
	end
else
	begin	
		UPDATE HCSPUserAC
		SET [SP_Password]=@sp_password
			  ,[SP_Password_Level]=@sp_password_level
			  ,[SP_IVRS_Password]=@ivrs_password
			  ,[SP_IVRS_Password_Level]=@ivrs_password_level
			  ,[Alias_Account]=@alias
			  ,[record_status]='A'
			  ,[Update_Dtm]=getdate()
			  ,[Update_by]=@sp_id			  
			  ,[Password_Fail_Count]=0
			  ,[Last_Pwd_Change_Dtm]=getdate()
			  ,[Default_Language]='E'		
		where SP_ID = @sp_ID and record_status = 'P' 
		and tsmp = @tsmp
	end
END
GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_udp_Activation] TO [HCSP]
GO
