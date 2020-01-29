IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalVerification_upd_status]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalVerification_upd_status]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 24 June 2008
-- Description:	Update ProfessionalVerification Record Status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionalVerification_upd_status]
	@Enrolment_Ref_No char(15),
	@Professional_Seq smallint,
	@Record_Status char(1),
	@tsmp timestamp

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
		SELECT TSMP FROM [dbo].[ProfessionalVerification]
		WHERE Enrolment_Ref_No = @Enrolment_Ref_No AND Professional_Seq = @Professional_Seq
	) != @tsmp
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

	UPDATE ProfessionalVerification
		Set
			Record_Status = @Record_Status 
		WHERE
			Enrolment_Ref_No = @Enrolment_Ref_No AND Professional_Seq = @Professional_Seq

END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalVerification_upd_status] TO HCVU
GO
