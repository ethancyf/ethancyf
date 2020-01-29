IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeStaging_upd_RecordStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeStaging_upd_RecordStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 18 Sept 2008
-- Description:	Update Practice Record Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_PracticeStaging_upd_RecordStatus]
	@ERN	char(15),
	@Display_Seq	smallint,
	@Record_Status	char(1),
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
		SELECT TSMP FROM [dbo].[PracticeStaging]
		WHERE Enrolment_Ref_No = @ERN AND Display_Seq = @Display_Seq
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
	UPDATE [dbo].[PracticeStaging]	Set
		Update_By = @Update_By,
		Update_Dtm = GetDate(),
		Record_Status = @Record_Status
	WHERE
		Enrolment_Ref_No = @ERN AND Display_Seq = @Display_Seq
END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeStaging_upd_RecordStatus] TO HCVU
GO
