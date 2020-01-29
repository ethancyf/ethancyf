IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeSchemeInfoEnrolment_get_byERNDisplaySeq]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeSchemeInfoEnrolment_get_byERNDisplaySeq]
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
-- CR No.:			CRE15-004
-- Modified by:		Winnie SUEN
-- Modified date:	19 June 2015
-- Description:		Add field Provide_Service
-- =============================================
-- =============================================
-- Author:			Kathy LEE
-- Create date:		04 Jun 2009
-- Description:		Get the practice scheme information in enrolment by ERN and practice display seq
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	27 June
-- Description:		"MScheme_Code" & "ProviderServiceFee"
--					will also retrieved
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	30 June 2009
-- Description:		Inner join MasterScheme and VoucherScheme to get the two Sequence_No
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	04 Aug 2009
-- Description:		Retrieve information joining SchemeEForm and SubsidizeGroupEForm
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE Procedure [dbo].[proc_PracticeSchemeInfoEnrolment_get_byERNDisplaySeq]
	@enrolment_ref_no char(15),
	@practice_display_seq smallint
as
BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
declare @temp_practice table (	enrolment_ref_no char(15),
								enrolment_dtm datetime,
								practice_display_seq smallint,
								scheme_code char(10),
								service_fee smallint,
								subsidize_code char(10),
								provideservicefee char(1),
								scheme_display_seq smallint,
								subsidize_display_seq smallint,
								scheme_seq smallint,
								provide_service char(1),
								clinic_type char(1))
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================


	insert into @temp_practice
	(
		enrolment_ref_no,
		enrolment_dtm,
		practice_display_seq,
		scheme_code,
		service_fee,		
		subsidize_code,
		provideservicefee,
		scheme_display_seq,
		scheme_seq,
		provide_service,
		clinic_type
	)
	SELECT	P.Enrolment_Ref_No,
			s.enrolment_dtm,
			P.Practice_Display_Seq,
			P.Scheme_Code,
			P.Service_Fee,
			P.subsidize_code,
			P.ProvideServiceFee,
			se.display_seq,
			se.scheme_seq,
			P.Provide_Service,
			P.Clinic_Type
			
	FROM	PracticeSchemeInfoEnrolment P
				inner join serviceproviderenrolment s
					on p.enrolment_ref_no = s.enrolment_ref_no
				Inner join schemeEForm se
					on  p.scheme_code = se.scheme_code and
						s.enrolment_dtm between se.enrol_period_from and se.enrol_period_to
					
	WHERE	P.Enrolment_Ref_No = @enrolment_ref_no
				AND P.Practice_Display_Seq = @practice_display_seq
				
	update @temp_practice
	set subsidize_display_seq  = sg.display_seq
	from @temp_practice t, subsidizegroupeform sg
	where t.scheme_code = sg.scheme_code and
		  t.subsidize_code = sg.subsidize_code and
		  t.scheme_seq = sg.scheme_seq and
		  t.enrolment_dtm between sg.enrol_period_from and sg.enrol_period_to
		  
	select	enrolment_ref_no,
			practice_display_seq,
			scheme_code,
			service_fee,
			subsidize_code,
			provideservicefee,
			scheme_display_seq,
			subsidize_display_seq,
			provide_service,
			clinic_type
	from @temp_practice


END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInfoEnrolment_get_byERNDisplaySeq] TO HCPUBLIC
GO
