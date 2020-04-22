IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeSchemeInfoStaging_upd_RecordStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeSchemeInfoStaging_upd_RecordStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Clark YIP
-- Create date: 06 May 2009
-- Description:	Update PracticeSchemeInfoStaging Record Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	03 August 2009
-- Description:		Add [Remark]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	3 August 2009
-- Description:		Change Delist_Dtm if the action is delist
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	4 August 2009
-- Description:		Add "Subsidize_code"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_PracticeSchemeInfoStaging_upd_RecordStatus]
	@Enrolment_Ref_No		char(15),
	@Scheme_Code			char(10),
	@Record_Status			char(1),
	@Update_By				varchar(20),
	@Practice_Display_Seq	smallint,
	@Remark					nvarchar(255),
	@TSMP					timestamp,
	@subsidize_code			char(10)
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
		SELECT	TSMP 
		FROM	[dbo].[PracticeSchemeInfoStaging]
		WHERE	Enrolment_Ref_No = @Enrolment_Ref_No 
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
	UPDATE	[dbo].[PracticeSchemeInfoStaging]	
	SET		Record_Status = @Record_Status,
			Remark = @Remark,
			Update_By = @Update_By,
			Update_Dtm = GETDATE()
	WHERE	Enrolment_Ref_No = @Enrolment_Ref_No 
				AND Practice_Display_Seq = @Practice_Display_Seq 
				AND Scheme_Code = @Scheme_Code
				AND Subsidize_Code = @subsidize_code
				
	IF @Record_Status = 'V' OR @Record_Status = 'I' BEGIN
	UPDATE	[dbo].[PracticeSchemeInfoStaging]	
	SET		Delist_Dtm = GETDATE()
	WHERE	Enrolment_Ref_No = @Enrolment_Ref_No 
				AND Practice_Display_Seq = @Practice_Display_Seq 
				AND Scheme_Code = @Scheme_Code
				AND Subsidize_Code = @subsidize_code
	END
				
END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInfoStaging_upd_RecordStatus] TO HCVU
GO
