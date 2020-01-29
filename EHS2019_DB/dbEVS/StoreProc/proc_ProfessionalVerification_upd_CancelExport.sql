IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalVerification_upd_CancelExport]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalVerification_upd_CancelExport]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 13 June 2008
-- Description:	Update Professional Verification Cancel Export 
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionalVerification_upd_CancelExport]
	@Enrolment_Ref_No char(15),
	@Professional_Seq smallint,
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
		WHERE Enrolment_Ref_No =  @Enrolment_Ref_No AND Professional_Seq = @Professional_Seq
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

	UPDATE [dbo].[ProfessionalVerification]
	SET
		[Export_By] = NULL,
		[Export_Dtm] = NULL,
		[Record_Status] = 'U'	
	WHERE
		[Enrolment_Ref_No] = @Enrolment_Ref_No AND
		[Professional_Seq ] = @Professional_Seq 

END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalVerification_upd_CancelExport] TO HCVU
GO
