IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalStaging_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalStaging_del]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 31 May 2008
-- Description:	Delete Professional Information in table
--				"ProfessionalStaging"
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_ProfessionalStaging_del]
	@enrolment_ref_no char(15), @record_status char(1)
AS
BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
DECLARE	@tmp_professional table (Professional_Seq smallint)

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
INSERT INTO	@tmp_professional (Professional_Seq)
SELECT		Professional_Seq
FROM		PracticeStaging
WHERE		Enrolment_Ref_No = @enrolment_ref_no

-- =============================================
-- Return results
-- =============================================

	UPDATE	ProfessionalStaging
	SET		Record_Status = @record_status			
	WHERE	Enrolment_Ref_No = @enrolment_ref_no and
			Professional_Seq not in (SELECT Professional_Seq FROM @tmp_Professional)

	DELETE FROM	ProfessionalStaging
	WHERE	Enrolment_Ref_No = @enrolment_ref_no and Professional_Seq not in
			(SELECT Professional_Seq FROM @tmp_Professional)

	DELETE FROM	ProfessionalVerification
	WHERE	Enrolment_Ref_No = @enrolment_ref_no and Professional_Seq not in
			(SELECT Professional_Seq FROM @tmp_Professional)

END
GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalStaging_del] TO HCVU
GO
