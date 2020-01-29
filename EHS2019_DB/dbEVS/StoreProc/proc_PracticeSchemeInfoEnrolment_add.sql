IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeSchemeInfoEnrolment_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeSchemeInfoEnrolment_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- CR No.:			CRE15-004 (Revamp VSS)
-- Modified by:		Chris YIM
-- Modified date:	08 Aug 2016
-- Description:		Add new column "Clinic_Type"
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE15-004 (TIV and QIV)
-- Modified by:		Chris YIM
-- Modified date:	19 June 2015
-- Description:		Add new column "ProvideService"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	4 August 2009
-- Description:		1. Rename MScheme_code -> Scheme_Code
--					2. Rename Scheme_Code -> Subsidize_Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	28 June 2009
-- Description:		Add "MScheme_Code" & "ProvideServiceFee"
-- =============================================
-- =============================================
-- Author:			Kathy LEE
-- Create date:		04-04-2009
-- Description:		Insert scheme information for each enrolment ref no. and each practice no.
-- =============================================

CREATE Procedure [dbo].[proc_PracticeSchemeInfoEnrolment_add]
	@enrolment_ref_no char(15),	
	@practice_display_seq smallint,
	@scheme_code char(10),
	@service_fee smallint,
	@subsidize_code char(10),
	@provideservicefee char(1),
	@provide_service char(1),
	@clinic_type char(1)
as
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

	Insert into PracticeSchemeInfoEnrolment
	(
		Enrolment_Ref_No,		
		Practice_Display_Seq,
		Scheme_Code,
		Service_Fee,
		Subsidize_Code,
		ProvideServiceFee,
		Provide_Service,
		Clinic_Type
	)
	values
	(
		@enrolment_ref_no,		
		@practice_display_seq,
		@scheme_code,
		@service_fee,
		@subsidize_code,
		@provideservicefee,
		@provide_service,
		@clinic_type
	)
END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInfoEnrolment_add] TO HCPUBLIC
GO
