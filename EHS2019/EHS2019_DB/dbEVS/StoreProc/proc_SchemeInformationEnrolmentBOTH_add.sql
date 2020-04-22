IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeInformationEnrolmentBOTH_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeInformationEnrolmentBOTH_add]
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

CREATE PROCEDURE [dbo].[proc_SchemeInformationEnrolmentBOTH_add]
	@enrolment_ref_no char(15), @scheme_code char(10), @service_fee_from smallint,
	@service_fee_to smallint, @scheme char(5)
	
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
		exec dbIVSS..proc_SchemeInformationEnrolment_add @enrolment_ref_no, @scheme_code, @service_fee_from, @service_fee_to
	END
	ELSE IF @scheme = 'EHCVS'
	BEGIN
		exec dbEVS..proc_SchemeInformationEnrolment_add @enrolment_ref_no, @scheme_code, @service_fee_from, @service_fee_to
	END
	ELSE IF @scheme = 'BOTH'
	BEGIN
		exec dbIVSS..proc_SchemeInformationEnrolment_add @enrolment_ref_no, @scheme_code, @service_fee_from, @service_fee_to

		exec dbEVS..proc_SchemeInformationEnrolment_add @enrolment_ref_no, @scheme_code, @service_fee_from, @service_fee_to

	END

END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformationEnrolmentBOTH_add] TO HCPUBLIC
GO
