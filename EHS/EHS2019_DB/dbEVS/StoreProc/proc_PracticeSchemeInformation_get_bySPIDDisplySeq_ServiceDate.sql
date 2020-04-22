IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeSchemeInformation_get_bySPIDDisplySeq_ServiceDate]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeSchemeInformation_get_bySPIDDisplySeq_ServiceDate]
GO

SET ANSI_NULLS ON
GO
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
-- CR No.:			CRE12-001
-- Modified by:		Tony FUNG
-- Modified date:	13 APR 2012
-- Description:		1. Grant permission to WSINT for PCDInterface
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Author:			Derek LEUNG
-- Create date:		20-08-2010
-- Description:		Get the practice scheme information 
--					copy from [proc_PracticeSchemeInformation_get_bySPIDDisplySeq], 
--					use service date instead of getdate()
-- =============================================


CREATE PROCEDURE [dbo].[proc_PracticeSchemeInformation_get_bySPIDDisplySeq_ServiceDate]
	@sp_id			char(8),
	@display_seq	smallint, 
	@service_date	datetime
AS
BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
declare @temp_practice table (	sp_id char(8),
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

/***********************************************
	[Record_Status] reference:
		Active						A  
		Suspended					W  
		DelistedVoluntary			V  
		DelistedInvoluntary			I  
		ActivePendingSuspend		SP 
		ActivePendingDelist			DP 
		SuspendedPendingDelist		XP 
		SuspendedPendingReactivate	YP 
 ***********************************************/

insert into @temp_practice
	(
		sp_id,
		practice_display_seq,
		scheme_code,
		service_fee,
		record_status,
		delist_status,
		remark,
		effective_dtm,
		delist_dtm,
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
	SELECT	P.SP_ID,
			P.Practice_Display_Seq,
			P.Scheme_Code,
			P.Service_Fee,
			CASE P.Record_Status
				WHEN 'A' THEN ISNULL(M.Upd_Type, 'A')		-- 3 outputs: A, SP, DP
				WHEN 'S' THEN 
					CASE ISNULL(M.Upd_Type, '')
						WHEN 'RP' THEN 'YP'
						WHEN 'DP' THEN 'XP'
						ELSE 'W'
					END										-- 3 outputs: W, YP, XP
				WHEN 'D' THEN P.Delist_Status				-- 2 outputs: V, I
			END AS [Record_Status],
			P.Delist_Status,
			CASE P.Record_Status
				WHEN 'A' THEN
					CASE ISNULL(M.Upd_Type, '')
						WHEN '' THEN ''
						ELSE ISNULL(M.Remark, '')
					END
				WHEN 'S' THEN ISNULL(P.Remark, '')
				WHEN 'D' THEN ISNULL(P.Remark, '')
			END AS [Remark],
			P.Effective_dtm,
			P.Delist_Dtm,
			P.Create_By,
			P.Create_Dtm,
			P.Update_By,
			P.Update_Dtm,
			--P.TSMP,
			P.subsidize_code,
			P.ProvideServiceFee,
			s.display_seq,
			s.scheme_seq,
			P.Provide_Service,
			P.Clinic_Type
			
	FROM	PracticeSchemeInfo P
				LEFT JOIN SPAccountMaintenance M
					ON P.SP_ID = M.SP_ID 
						AND M.Record_Status = 'A' 
						AND P.Practice_Display_Seq = M.SP_Practice_Display_Seq 
						AND P.Scheme_Code = M.Scheme_Code
				Inner join schemebackoffice s
					on  p.scheme_code = s.scheme_code and
						@service_date between s.effective_dtm and s.expiry_dtm
						
	WHERE	P.SP_ID = @sp_id 
				AND P.Practice_Display_Seq = @display_seq 
				
	update @temp_practice
	set subsidize_display_seq  = sg.display_seq,
		subsidize_record_status = sg.record_status
	from @temp_practice t, subsidizegroupbackoffice sg
	where t.scheme_code = sg.scheme_code and
		  t.subsidize_code = sg.subsidize_code and
		  t.scheme_seq = sg.scheme_seq
				
	
	select	t.sp_id,
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
	from	@temp_practice t, PracticeSchemeInfo P
	where	t.sp_id = p.sp_id and
			t.Practice_Display_Seq = p.Practice_Display_Seq and
			t.scheme_code = p.scheme_code and
			t.subsidize_code = p.subsidize_code	and
			t.subsidize_record_status <> 'I'

END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInformation_get_bySPIDDisplySeq_ServiceDate] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInformation_get_bySPIDDisplySeq_ServiceDate] TO WSEXT
Go

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInformation_get_bySPIDDisplySeq_ServiceDate] TO WSINT
GO