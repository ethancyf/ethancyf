IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_HCSPUserAC_upd_ActivationCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_HCSPUserAC_upd_ActivationCode]
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
-- Author:			Kathy LEE
-- Create date:		28 July 2008
-- Description:		Update and reset password fail count of HCSPUserAC
-- =============================================

CREATE procedure [dbo].[proc_HCSPUserAC_upd_ActivationCode]
	@sp_id char(8),  
	@activation_Code as varchar(100), 
	@update_by varchar(20), 
	@tsmp timestamp,
	@Activation_Code_Level	INT
as
BEGIN
	
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM HCSPUserAC
		WHERE SP_ID = @sp_id) != @tsmp
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

	UPDATE	HCSPUserAC
	Set		Activation_Code = @activation_Code,
			Update_By = @update_by,
			Update_Dtm = getdate(),
			Activation_Code_Level = @Activation_Code_Level
	WHERE	SP_ID = @sp_id
end

GO

GRANT EXECUTE ON [dbo].[proc_HCSPUserAC_upd_ActivationCode] TO HCVU
GO
