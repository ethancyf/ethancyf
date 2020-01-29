IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalEnrolmentBOTH_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalEnrolmentBOTH_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kathy LEE
-- Create date: 24 April 2008
-- Description:	Insert the Service Provider to Table
--				ServiceProviderEnrolment
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================

CREATE PROCEDURE [dbo].[proc_ProfessionalEnrolmentBOTH_add]
	@enrolment_ref_no char(15), @professional_seq smallint, @service_category_code char(5),
	@registration_code varchar(15), @scheme char(5)
	
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
	if @scheme = 'IVSS'
	BEGIN
		exec dbIVSS..proc_ProfessionalEnrolment_add @enrolment_ref_no, @professional_seq, @service_category_code,
													@registration_code
	END
	ELSE IF @scheme = 'EHCVS'
	BEGIN
		exec dbEVS..proc_ProfessionalEnrolment_add @enrolment_ref_no, @professional_seq, @service_category_code,
													@registration_code
	END
	ELSE IF @scheme = 'BOTH'
	BEGIN
		exec dbIVSS..proc_ProfessionalEnrolment_add @enrolment_ref_no, @professional_seq, @service_category_code,
													@registration_code

		exec dbEVS..proc_ProfessionalEnrolment_add @enrolment_ref_no, @professional_seq, @service_category_code,
													@registration_code

	END

END
GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalEnrolmentBOTH_add] TO HCPUBLIC
GO
