IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_upd_Email]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProvider_upd_Email]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE16-007-02 (Refine system from CheckMarx findings)
-- Modified by:		Winnie SUEN
-- Modified date:	20 Jun 2017
-- Description:		Add field "Activation_Code_Level"
-- =============================================
-- =============================================
-- Author:		Clark YIP
-- Create date: 07 July 2008
-- Description:	Update the Email in
--				Table "ServiceProvider"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	  Clark YIP
-- Modified date: 16 Feb 2009
-- Description:	  After confirm change email, the password fail count in HCSPUserAC will be reset 
-- =============================================
-- =============================================
-- Modification History
-- CR No:			INT13-0028 - SP Amendment Report
-- Modified by:		Tommy LAM
-- Modified date:	20 Dec 2013
-- Description:		Add Column -	[ServiceProvider].[Tentative_Email_Input_By]
--									[ServiceProvider].[Data_Input_By]
--									[ServiceProvider].[Data_Input_Effective_Dtm]
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProvider_upd_Email]
	@SP_ID	char(8), 
	@TSMP timestamp
AS
BEGIN
	
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM ServiceProvider
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

	UPDATE	ServiceProvider
	Set		Update_By = @SP_ID,
			Update_Dtm = getdate(),
			Email = [Tentative_Email],
			Data_Input_By = [Tentative_Email_Input_By],
			Data_Input_Effective_Dtm = getdate(),
			[Tentative_Email_Input_By]=null,
			[Tentative_Email]=null,
			[Activation_Code]=null,
			[Activation_Code_Level] = NULL
	WHERE	SP_ID = @SP_ID
	
	update HCSPUserAC
	set Password_Fail_Count = 0	
	, Update_By = @sp_ID
	, Update_Dtm = getdate()
	where sp_id = @sp_ID
	
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_upd_Email] TO HCSP
GO
