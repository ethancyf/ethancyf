IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Practice_upd_UnderModify]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Practice_upd_UnderModify]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 23 June 2008
-- Description:	Update Practice UnderModificiation Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_Practice_upd_UnderModify]
	@SP_ID	char(8),
	@Display_Seq	smallint,
	@UnderModification	char(1),
	@update_by	varchar(20),
	@tsmp timestamp
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================

	IF (
		SELECT TSMP FROM [dbo].[Practice]
		WHERE SP_ID = @SP_ID AND Display_Seq = @Display_Seq
	) != @tsmp
	BEGIN
		RAISERROR('00011', 16, 1)
	END

-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	UPDATE [dbo].[Practice]	Set
	
		UnderModification = @UnderModification,
		Update_By = @update_by,
		Update_Dtm = GetDate(),
		Record_Status = 'R'
	WHERE
		SP_ID = @SP_ID AND Display_Seq = @Display_Seq

END
GO
