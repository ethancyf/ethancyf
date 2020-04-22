IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchmemeInformationEnrolment_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchmemeInformationEnrolment_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 21 July 2008
-- Description:	Insert the Scheme Information to Table
--				SchemeInformationEnrolment
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_SchmemeInformationEnrolment_add]
	@enrolment_ref_no char(15), @scheme_code char(10), @service_fee_from smallint,
	@service_fee_to smallint
AS
BEGIN

	SET NOCOUNT ON;

  INSERT INTO SchemeInformationEnrolment
				(Enrolment_Ref_No,
				Scheme_Code,
				Service_Fee_From,
				Service_Fee_To)
	VALUES		(@enrolment_ref_no,
				@scheme_code,
				@service_fee_from,
				@service_fee_to)
END
GO

GRANT EXECUTE ON [dbo].[proc_SchmemeInformationEnrolment_add] TO HCPUBLIC
GO
