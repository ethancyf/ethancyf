IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeSchemeInfoStaging_get_byErnPracticeDisplaySeq]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeSchemeInfoStaging_get_byErnPracticeDisplaySeq]
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
-- Modified date:	23 Dec 2009
-- Description:		Add "else" case in record status
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	10 Dec 2009
-- Description:		Don't retrieve those subsidizegroup
--					which record_status = 'I'
-- =============================================
-- =============================================
-- Author:		Clark YIP
-- Create date: 18 May 2009
-- Description:	Get PracticeSchemeInfoStaging Record Status
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
-- Modified by:		Kathy LEE
-- Modified date:	23 Nov 2009
-- Description:		Left join SPAccountMaintenance to get the information about the practice scheme whether
--					or not the practice scheme info is pending in account change confirmation
-- =============================================
CREATE PROCEDURE [dbo].[proc_PracticeSchemeInfoStaging_get_byErnPracticeDisplaySeq]
	@enrolment_ref_no	char(15),
	@practice_display_seq smallint	
AS
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
		scheme_code,	
		practice_display_seq,
		sp_id,		
		service_fee,		
		delist_status,
		delist_dtm,
		effective_dtm,
		record_status,
		remark,
		create_dtm,
		create_by,
		update_dtm,				
		update_by,
		subsidize_code,
		provideservicefee,
		scheme_display_seq,
		scheme_seq,
		provide_service,
		Clinic_Type
	)	
	SELECT	P.Enrolment_Ref_no,
			P.Scheme_Code,
			P.Practice_Display_Seq,
			P.SP_ID,
			P.Service_Fee,
			P.Delist_Status,
			P.Delist_Dtm,
			P.Effective_Dtm,
			P.Record_Status,
			P.Remark,
			P.Create_Dtm,
			P.Create_by,
			P.Update_Dtm,
			P.Update_By,
			--P.TSMP,
			P.subsidize_code,
			P.ProvideServiceFee,
			s.display_seq,
			s.scheme_seq,
			P.Provide_Service,
			P.Clinic_Type
			
	FROM 	PracticeSchemeInfoStaging P
				Inner join schemebackoffice s
					on  p.scheme_code = s.scheme_code and
						getdate() between s.effective_dtm and s.expiry_dtm					
	WHERE	P.Enrolment_Ref_No = @enrolment_ref_no 
				AND P.Practice_Display_Seq = @practice_Display_Seq
	

	update @temp_practice
	set subsidize_display_seq  = sg.display_seq,
		subsidize_record_status = sg.record_status
	from @temp_practice t, subsidizegroupbackoffice sg
	where t.scheme_code = sg.scheme_code and
		  t.subsidize_code = sg.subsidize_code and
		  t.scheme_seq = sg.scheme_seq
		  
	select	t.enrolment_ref_no,
			t.sp_id,
			t.practice_display_seq,
			t.scheme_code,
			t.service_fee,
			--t.record_status,
			case t.Record_Status	
				when 'A' then t.Record_Status
				when 'S' then case isNull(SPAM.Upd_Type, '')
									when '' then 'W'
									else SPAM.Upd_Type
								  end
				when 'I' then t.Record_Status 
				when 'V' then t.Record_Status
				when 'E' then case isNull(SPAM.Upd_Type, '')
								when '' then 'E'
								else SPAM.Upd_Type
							  end 
				when 'U' then t.Record_Status
				else t.Record_status
			end as [Record_status],
			t.delist_status,
			--t.remark,
			case t.Record_Status
				when 'U' then ''
				else
					case isNull(SPAM.Upd_Type, '')
						when '' then t.remark
						else SPAM.Remark
				end			
			end as [Remark],
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
	from	@temp_practice t
			inner join PracticeSchemeInfoStaging P
			on	t.enrolment_ref_no = p.enrolment_ref_no and
				t.Practice_Display_Seq = p.Practice_Display_Seq and
				t.scheme_code = p.scheme_code and
				t.subsidize_code = p.subsidize_code	
			left join SPAccountMaintenance SPAM
			ON isnull(t.sp_id,'') <> '' and
				t.SP_ID = SPAM.SP_ID and 
				t.practice_display_seq = SPAM.SP_Practice_Display_Seq and
				 SPAM.Record_Status = 'A' and
				t.scheme_code = SPAM.scheme_code
	where	t.subsidize_record_status <> 'I'
			
END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInfoStaging_get_byErnPracticeDisplaySeq] TO HCVU
GO
