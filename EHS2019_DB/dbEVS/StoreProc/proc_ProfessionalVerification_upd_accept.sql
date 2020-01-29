IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalVerification_upd_accept]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalVerification_upd_accept]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 2 June 2008
-- Description:	Update ProfessionalVerification for Accept
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionalVerification_upd_accept]
	@Enrolment_Ref_No char(15),
	@Professional_Seq smallint,
	@Confirm_By varchar(20),
	@Confirm_Dtm datetime,
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
			Confirm_By = @Confirm_By,
			Confirm_Dtm = @Confirm_Dtm,
			Final_result = 'Y',
			Record_Status = 'C'
		WHERE
			Enrolment_Ref_No = @Enrolment_Ref_No AND Professional_Seq = @Professional_Seq

END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalVerification_upd_accept] TO HCVU
GO
