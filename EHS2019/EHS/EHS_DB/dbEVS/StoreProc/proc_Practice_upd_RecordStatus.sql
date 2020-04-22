IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_Practice_upd_RecordStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_Practice_upd_RecordStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 03 July 2008
-- Description:	Update Practice Record Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Clark Yip
-- Modified date: 08 May 2009
-- Description:	  Remove the Delist_Status, delist_dtm
-- =============================================
CREATE PROCEDURE [dbo].[proc_Practice_upd_RecordStatus]
	@SP_ID	char(8),
	@Display_Seq	smallint,
	@Record_Status	char(1),
	--@Delist_Status	char(1),
	@Update_By	varchar(20),
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

	IF (
		SELECT TSMP FROM [dbo].[Practice]
		WHERE SP_ID = @SP_ID AND Display_Seq = @Display_Seq
	) != @TSMP
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

	UPDATE [dbo].[Practice]	Set
		Update_By = @Update_By,
		Update_Dtm = GetDate(),
		--Delist_Dtm = GetDate(),
		--Delist_Status = @Delist_Status,
		Record_Status = @Record_Status
	WHERE
		SP_ID = @SP_ID AND Display_Seq = @Display_Seq
END

ELSE
BEGIN

	UPDATE [dbo].[Practice]	Set
		Update_By = @Update_By,
		Update_Dtm = GetDate(),
		Record_Status = @Record_Status
	WHERE
		SP_ID = @SP_ID AND Display_Seq = @Display_Seq
END

END
GO

GRANT EXECUTE ON [dbo].[proc_Practice_upd_RecordStatus] TO HCVU
GO
