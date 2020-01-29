IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeSchemeInfoStaging_get_byERN]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeSchemeInfoStaging_get_byERN]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE16-002
-- Modified by:		Lawrence TSANG
-- Modified date:	5 August 2016
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
-- Modified by:		Kathy LEE
-- Modified date:	10 Dec 2009
-- Description:		Don't retrieve those subsidizegroup
--					which record_status = 'I'
-- =============================================
-- =============================================
-- Author:			Kathy LEE
-- Create date:		02 Jun 209
-- Description:		Get the practice scheme information in staging
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
-- Description:		Retrieve information joining SchemeBackOffice and SubsidizeGroupBackOffice
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Paul Yip	
-- Modified date:	05 Aug 2009
-- Description:		include ERN in output
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
CREATE Procedure [dbo].[proc_PracticeSchemeInfoStaging_get_byERN]
	@enrolment_ref_no char(15)
as
BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
declare @temp_practice table (	enrolment_ref_no char(15),
								sp_id char(8),
								practice_display_seq smallint,
								scheme_code char(10),
								service_fee smallint,
								record_status char(2),
								delist_status char(1),
								remark nvarchar(255),
								effective_dtm datetime,
								delist_dtm datetime,
								create_by varchar(20),
								create_dtm datetime,
								update_by varchar(20),
								update_dtm datetime,
								subsidize_code char(10),
								provideservicefee char(1),
								scheme_display_seq smallint,
								subsidize_display_seq smallint,
								scheme_seq smallint,
								subsidize_record_status char(1),
								provide_service char(1),
								Clinic_Type char(1))

declare @sp_id as char(8)
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
		sp_id,
		practice_display_seq,
		scheme_code,
		service_fee,		
		delist_status,
		delist_dtm,
		effective_dtm,
		record_status,
		remark,
		create_by,
		create_dtm,
		update_by,
		update_dtm,
		subsidize_code,
		provideservicefee,
		scheme_display_seq,
		scheme_seq,
		provide_service,
		Clinic_Type
	)	
	SELECT	P.Enrolment_Ref_No,
			P.SP_ID,
			P.Practice_Display_Seq,
			P.Scheme_Code,
			P.Service_Fee,
			P.delist_status,
			P.delist_dtm,
			P.effective_dtm,
			P.record_status,
			P.remark,
			P.create_by,
			P.create_dtm,
			P.update_by,
			P.update_dtm,
			--P.tsmp,
			P.subsidize_code,
			P.ProvideServiceFee,
			s.display_seq,
			s.scheme_seq,
			P.Provide_Service,
			P.Clinic_Type
		
	FROM	PracticeSchemeInfoStaging P
				Inner join schemebackoffice s
					on  p.scheme_code = s.scheme_code and
						getdate() between s.effective_dtm and s.expiry_dtm
						
	WHERE	P.Enrolment_Ref_No = @enrolment_ref_no
	
	update @temp_practice
	set subsidize_display_seq  = sg.display_seq,
		subsidize_record_status = sg.record_status
	from @temp_practice t, subsidizegroupbackoffice sg
	where t.scheme_code = sg.scheme_code and
		  t.subsidize_code = sg.subsidize_code and
		  t.scheme_seq = sg.scheme_seq
		  
		select t.enrolment_ref_no,	
			t.sp_id,
			t.practice_display_seq,
			t.scheme_code,
			t.service_fee,
			t.record_status,
			t.delist_status,
			t.remark,
			t.effective_dtm,
			t.delist_dtm,
			t.create_by,
			t.create_dtm,
			t.update_by,
			t.update_dtm,
			t.subsidize_code,
			t.provideservicefee,
			t.scheme_display_seq,
			t.subsidize_display_seq,
			p.tsmp,
			t.provide_service,
			t.Clinic_Type
	from	@temp_practice t, PracticeSchemeInfoStaging P
	where	t.enrolment_ref_no = p.enrolment_ref_no and
			t.Practice_Display_Seq = p.Practice_Display_Seq and
			t.scheme_code = p.scheme_code and
			t.subsidize_code = p.subsidize_code	and
			t.subsidize_record_status <> 'I'

END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInfoStaging_get_byERN] TO HCVU
GO
