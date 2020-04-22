IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_upd_UnderModifyAndRecordStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ServiceProvider_upd_UnderModifyAndRecordStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 12 August 2008
-- Description:	Update the status of Record_Status and UnderModification in
--				Table "ServiceProvider"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark Yip
-- Modified date: 08 May 2009
-- Description:	  Remove the Delist_Status, delist_dtm
-- =============================================
CREATE PROCEDURE [dbo].[proc_ServiceProvider_upd_UnderModifyAndRecordStatus]
	@SP_ID	char(8),
	@Record_Status char(1),
	--@Delist_Status char(1),
	@UnderModification char(1),
	@Update_By varchar(20), 
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
IF @Record_Status = 'D'
BEGIN
	UPDATE	ServiceProvider
	Set		Record_Status = @Record_Status,
			--Delist_Status = @Delist_Status,
			UnderModification = @UnderModification,
			Update_By = @Update_By,
			Update_Dtm = getdate()
			--Delist_Dtm = getDate()
	WHERE	SP_ID = @SP_ID
END

ELSE
BEGIN
	UPDATE	ServiceProvider
	Set		Record_Status = @Record_Status,
			UnderModification = @UnderModification,
			Update_By = @Update_By,
			Update_Dtm = getdate()
	WHERE	SP_ID = @SP_ID
END
END
GO

GRANT EXECUTE ON [dbo].[proc_ServiceProvider_upd_UnderModifyAndRecordStatus] TO HCVU
GO
