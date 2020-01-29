IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MedicalOrganizationStaging_upd_status]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MedicalOrganizationStaging_upd_status]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Clark YIP
-- Create date: 06 May 2009
-- Description:	Update MedicalOrganizationStaging Record Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_MedicalOrganizationStaging_upd_status]
	@enrolment_ref_no	char(15),	
	@Display_Seq	smallint,
	@Record_Status	char(1),
	@Update_By	varchar(20),
	@tsmp		timestamp
	
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
		SELECT TSMP FROM [dbo].[MedicalOrganizationStaging]
		WHERE Enrolment_Ref_No = @enrolment_ref_no AND Display_Seq = @Display_Seq
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
	UPDATE [dbo].[MedicalOrganizationStaging]	Set
		Update_By = @Update_By,
		Update_Dtm = GetDate(),
		Record_Status = @Record_Status
	WHERE Enrolment_Ref_No = @enrolment_ref_no AND Display_Seq = @Display_Seq
END
GO

GRANT EXECUTE ON [dbo].[proc_MedicalOrganizationStaging_upd_status] TO HCVU
GO
