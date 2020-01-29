IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalVerification_del]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalVerification_del]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 20 June 2008
-- Description:	Delete Professional Verification Record
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionalVerification_del]
	@Enrolment_Ref_No char(15),
	@Professional_Seq smallint,
	@tsmp timestamp,
	@checkTSMP tinyint
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
	IF @checkTSMP = 1 AND (
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

DELETE FROM [dbo].[ProfessionalVerification]

WHERE 
	Enrolment_Ref_No = @Enrolment_Ref_No AND
	Professional_Seq = @Professional_Seq
END

GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalVerification_del] TO HCVU
GO
