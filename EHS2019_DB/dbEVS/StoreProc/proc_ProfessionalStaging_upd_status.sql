IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalStaging_upd_status]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalStaging_upd_status]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 19 Jun 2008
-- Description:	Update Professional Staging Status 
--				"ProfessionalStaging"
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_ProfessionalStaging_upd_status]
	@enrolment_ref_no char(15),
	@Professional_Seq smallint,
	@record_status char(1)
AS
BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	UPDATE	ProfessionalStaging
	SET		Record_Status = @record_status			
	WHERE	Enrolment_Ref_No = @enrolment_ref_no and
			Professional_Seq = @Professional_Seq
END


GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalStaging_upd_status] TO HCVU
GO
