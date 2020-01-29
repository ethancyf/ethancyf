IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_MOSchemeInformationEnrolment_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_MOSchemeInformationEnrolment_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kathy LEE
-- Create date: 17 August 2008
-- Description:	Insert the Medical Organization Scheme Information to Table
--				MOPracticeEnrolment
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_MOSchemeInformationEnrolment_add]
	@enrolment_ref_No	char(15), @scheme_Code	char(10), @mo_practice_display_seq	smallint,
	@service_fee_from	smallint, @service_fee_to	smallint
AS
BEGIN

	SET NOCOUNT ON;

  INSERT INTO MOSchemeInformationEnrolment
				(Enrolment_Ref_No,
				Scheme_Code,
				MO_Practice_Display_Seq,
				Service_Fee_From,
				Service_Fee_To)
	VALUES		(@enrolment_ref_no,
				@scheme_Code,
				@mo_practice_display_seq,
				@service_fee_from,
				@service_fee_to)
END
GO

GRANT EXECUTE ON [dbo].[proc_MOSchemeInformationEnrolment_add] TO HCPUBLIC
GO
