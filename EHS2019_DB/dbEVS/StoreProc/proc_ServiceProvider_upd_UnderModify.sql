IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_upd_UnderModify]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProvider_upd_UnderModify]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 04 June 2008
-- Description:	Update the status of UnderModification in
--				Table "ServiceProvider"
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProvider_upd_UnderModify]
	@sp_id	char(8), @undermodification char(1), @update_by varchar(20), @tsmp timestamp
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

	UPDATE	ServiceProvider
	Set		UnderModification = @undermodification,
			Update_By = @update_by,
			Update_Dtm = getdate()
	WHERE	SP_ID = @sp_id
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_upd_UnderModify] TO HCVU
GO
