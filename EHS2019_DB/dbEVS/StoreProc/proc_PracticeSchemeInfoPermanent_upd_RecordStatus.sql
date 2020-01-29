IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeSchemeInfoPermanent_upd_RecordStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeSchemeInfoPermanent_upd_RecordStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Lawrence TSANG
-- Create date: 27 May 2009
-- Description:	Update PracticeSchemeInfo Record Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	4 August 2009
-- Description:		Add "Subsidize_Code"
-- =============================================
CREATE PROCEDURE [dbo].[proc_PracticeSchemeInfoPermanent_upd_RecordStatus]
	@SP_ID					char(8),
	@Scheme_Code			char(10),
	@Record_Status			char(1),
	@Delist_Status			char(1),
	@Remark					nvarchar(255),
	@Update_By				varchar(20),
	@Practice_Display_Seq	smallint,
	@TSMP					timestamp,
	@subsidize_code			char(10)
AS BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================

	IF (
		SELECT	TSMP 
		FROM	[dbo].[PracticeSchemeInfo]
		WHERE	SP_ID = @SP_ID
					AND Practice_Display_Seq = @Practice_Display_Seq 
					AND Scheme_Code = @Scheme_Code
					AND Subsidize_Code = @subsidize_code
	) != @TSMP
	BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
	END

-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================
	UPDATE	[dbo].[PracticeSchemeInfo]	
	SET		Update_By = @Update_By,
			Update_Dtm = GetDate(),
			Record_Status = @Record_Status,
			Delist_Status = @Delist_Status,
			Remark = @Remark
	WHERE	SP_ID = @SP_ID
				AND Practice_Display_Seq = @Practice_Display_Seq 
				AND Scheme_Code = @Scheme_Code
				AND Subsidize_Code = @subsidize_code
				
	IF @Record_Status = 'D' BEGIN
		UPDATE	[dbo].[PracticeSchemeInfo]	
		SET		Delist_Dtm = GetDate()
		WHERE	SP_ID = @SP_ID
					AND Practice_Display_Seq = @Practice_Display_Seq 
					AND Scheme_Code = @Scheme_Code
					AND Subsidize_Code = @subsidize_code
	END

END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInfoPermanent_upd_RecordStatus] TO HCVU
GO
