IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MedicalOrganizationStaging_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MedicalOrganizationStaging_del]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 31 May 2008
-- Description:	Delete MO Information in table
--				"MeidcalOrganizationStaging" and 
--				Reorder mo display seq
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_MedicalOrganizationStaging_del]
	@enrolment_ref_no char(15), @display_seq smallint, 
	@record_status char(1), @update_by varchar(20), @tsmp timestamp
AS
BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM MedicalOrganizationStaging
		WHERE Enrolment_Ref_No = @enrolment_ref_no and
				Display_Seq = @display_seq) != @tsmp
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
	
	UPDATE	MedicalOrganizationStaging
	SET		Record_Status = @record_status,
			Update_by = @update_by,
			Update_Dtm = getdate()
	WHERE	Enrolment_Ref_No = @enrolment_ref_no and
			Display_Seq = @display_seq

	DELETE	FROM MedicalOrganizationStaging
	WHERE	Enrolment_Ref_No = @enrolment_ref_no and
			Display_Seq = @display_seq

	UPDATE	MedicalOrganizationStaging
	SET		Display_Seq = Display_Seq - 1
	WHERE	Display_Seq > @display_seq and
			Enrolment_Ref_No = @enrolment_ref_no

END
GO

GRANT EXECUTE ON [dbo].[proc_MedicalOrganizationStaging_del] TO HCVU
GO
