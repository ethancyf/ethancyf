IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_upd_ActivationCode]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProvider_upd_ActivationCode]
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
-- Modification History
-- CR No.			CRE16-018 (Display SP tentative email in HCVU)
-- Modified by:		Winnie SUEN
-- Modified date:	22 Nov 2016
-- Description:		Throw error when tentative email is not exist
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 15 July 2008
-- Description:	Update Service Provider Email Change Activation Code
-- =============================================

CREATE PROCEDURE [dbo].[proc_ServiceProvider_upd_ActivationCode]
	@SP_ID	char(8),
	@Update_by varchar(20),
	@Activation_Code varchar(100),
	@tsmp timestamp,
	@checkTSMP tinyint,
	@Activation_Code_Level INT
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF @checkTSMP  = 1 AND (SELECT TSMP FROM ServiceProvider
		WHERE SP_ID = @SP_ID) != @TSMP
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@error
	END
	
	IF EXISTS (SELECT 1 FROM ServiceProvider 
				WHERE SP_ID = @SP_ID AND ISNULL(Tentative_Email,'') = '')
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

	UPDATE	[dbo].[ServiceProvider]
	Set
		[Activation_Code] = @Activation_Code,
		[Update_By] = @Update_By,
		[Update_Dtm] = getdate(),
		[Activation_Code_Level] = @Activation_Code_Level
		
	WHERE	[SP_ID] = @SP_ID
END


GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_upd_ActivationCode] TO HCVU
GO
