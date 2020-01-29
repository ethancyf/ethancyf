IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalVerification_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalVerification_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 22 May 2008
-- Description:	Insert the Professional Verification Information
--				to Table ProfessionalVerification
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionalVerification_add]
	@enrolment_ref_no char(15), @professional_seq smallint, @sp_id char(8),
	@record_status	char(1)
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

	INSERT INTO	ProfessionalVerification
				(Enrolment_Ref_No,
				 Professional_Seq,
				 SP_ID,
				 Record_Status)
	VALUES		 (@enrolment_ref_no,
				 @professional_seq,
				 @sp_id,
				 @record_status)
END
GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalVerification_add] TO HCVU
GO
