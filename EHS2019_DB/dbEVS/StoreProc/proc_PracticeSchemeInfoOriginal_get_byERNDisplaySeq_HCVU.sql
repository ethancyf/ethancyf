IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeSchemeInfoOriginal_get_byERNDisplaySeq_HCVU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeSchemeInfoOriginal_get_byERNDisplaySeq_HCVU]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE16-002
-- Modified by:		Lawrence TSANG
-- Modified date:	8 August 2016
-- Description:		Add Clinic_Type
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE15-004
-- Modified by:		Winnie SUEN
-- Modified date:	19 June 2015
-- Description:		Add field Provide_Service
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	3 September 2014
-- CR No.:			CRE14-008
-- Description:		(QIV) Always carry EPV+IV to HCVU Data Entry
-- =============================================
-- =============================================
-- CR No.:		CRE12-001
-- Author:		Koala CHENG
-- Create date: 16 Jan 2012
-- Description:	Get the practice scheme information in enrolment by ERN and practice display seq
--					from practiceSchemeOriginal, SchemeBackOffice and SubsidizeGroupBackOffice
-- =============================================
CREATE Procedure [dbo].[proc_PracticeSchemeInfoOriginal_get_byERNDisplaySeq_HCVU]
	@enrolment_ref_no char(15),
	@practice_display_seq smallint
as
BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
declare @temp_practice table (	enrolment_ref_no char(15),								
								practice_display_seq smallint,
								scheme_code char(10),
								service_fee smallint,
								subsidize_code char(10),
								provideservicefee char(1),
								scheme_display_seq smallint,
								subsidize_display_seq smallint,
								scheme_seq smallint,
								subsidize_record_status char(1),
								provide_service char(1),
								Clinic_Type char(1))
								
DECLARE @carryToHCVU char(1)
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
		practice_display_seq,
		scheme_code,
		service_fee,		
		subsidize_code,
		provideservicefee,
		scheme_display_seq,
		scheme_seq,
		provide_service,
		Clinic_Type
	)
	SELECT	P.Enrolment_Ref_No,
			P.Practice_Display_Seq,
			P.Scheme_Code,
			P.Service_Fee,
			P.subsidize_code,
			P.ProvideServiceFee,
			se.display_seq,
			se.scheme_seq,
			P.Provide_Service,
			P.Clinic_Type
			
	FROM	PracticeSchemeInfoOriginal P
				Inner join SchemeBackOffice se
					on  p.scheme_code = se.scheme_code and
						getdate() between se.effective_dtm and se.expiry_dtm
					
	WHERE	P.Enrolment_Ref_No = @enrolment_ref_no
				AND P.Practice_Display_Seq = @practice_display_seq
				
	update @temp_practice
	set subsidize_display_seq  = sg.display_seq,
		subsidize_record_status = sg.record_status
	from @temp_practice t, subsidizegroupbackoffice sg
	where t.scheme_code = sg.scheme_code and
		  t.subsidize_code = sg.subsidize_code and
		  t.scheme_seq = sg.scheme_seq
		  
	select	enrolment_ref_no,
			practice_display_seq,
			scheme_code,
			service_fee,
			subsidize_code,
			provideservicefee,
			scheme_display_seq,
			subsidize_display_seq,
			provide_service,
			Clinic_Type
	from @temp_practice
	where subsidize_record_status <> 'I'


END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInfoOriginal_get_byERNDisplaySeq_HCVU] TO HCVU
GO
