IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_upd_EmailByHCVU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProvider_upd_EmailByHCVU]
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
-- CR No:			INT13-0028 - SP Amendment Report
-- Modified by:		Tommy LAM
-- Modified date:	08 Jan 2014
-- Description:		Add Column -	[ServiceProvider].[Tentative_Email_Input_By]
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 13 Nov 2008
-- Description:	HCVU User Update the Email in 
--				Table "ServiceProvider"
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProvider_upd_EmailByHCVU]
	@SP_ID	char(8),
	@update_by varchar(20),
	@email varchar(255),
	@TSMP timestamp,
	@checkTSMP tinyint
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
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	UPDATE	ServiceProvider
	Set		Update_By = @update_by,
			Update_Dtm = getdate(),
			Email = @email,
			[Tentative_Email]=null,
			[Activation_Code]=null,
			[Tentative_Email_Input_By]=null,
			[Activation_Code_Level] = NULL
	WHERE	SP_ID = @SP_ID
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_upd_EmailByHCVU] TO HCVU
GO
