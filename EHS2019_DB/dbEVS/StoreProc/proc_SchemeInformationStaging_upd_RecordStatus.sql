IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeInformationStaging_upd_RecordStatus]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeInformationStaging_upd_RecordStatus]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Clark YIP
-- Create date: 19 May 2009
-- Description:	Update the record status of SchemeInformationStaging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 Clark YIP
-- Modified date: 27 Jul 2009
-- Description:	 Add the tsmp checking
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	3 August 2009
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
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
CREATE PROCEDURE [dbo].[proc_SchemeInformationStaging_upd_RecordStatus]
	@enrolment_ref_no	char(15),
	@scheme_code		char(10),
	@record_status		char(1),
	@update_by			varchar(20),
	@remark				nvarchar(255),
	@tsmp				timestamp
AS
BEGIN
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
IF (
		SELECT	TSMP 
		FROM	[dbo].[SchemeInformationStaging]
		WHERE	Enrolment_Ref_No = @enrolment_ref_no 
					AND scheme_code = @scheme_code
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
	UPDATE	[dbo].[SchemeInformationStaging]
	SET		Record_Status = @record_status, 
			Remark = @remark,
			Update_By = @update_by, 
			Update_Dtm = GETDATE()
	WHERE	Enrolment_Ref_No = @enrolment_ref_no 
				AND Scheme_Code = @scheme_code
				
	IF @Record_Status = 'V' OR @Record_Status = 'I' BEGIN
		UPDATE	[dbo].[SchemeInformationStaging]
		SET		Delist_Dtm = GETDATE()
		WHERE	Enrolment_Ref_No = @enrolment_ref_no 
					AND Scheme_Code = @scheme_code
	
	END

END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformationStaging_upd_RecordStatus] TO HCVU
GO
