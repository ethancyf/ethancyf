IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeSchemeInfoStaging_del_byERNDisplaySeq]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeSchemeInfoStaging_del_byERNDisplaySeq]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	19 Jul 2018
-- CR No.			CRE17-016
-- Description:		Fix missing [Clinic_Type]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE15-004
-- Modified by:		Winnie SUEN
-- Modified date:	25 June 2015
-- Description:		Add field Provide_Service
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	17 Dec 2009
-- Description:		1.Delete the practice scheme info\
--					based on Enrolment_Ref_No, Practice_Display_Seq
--					and Scheme_code
--					2.Comment out the TSMP Checking
--					3.Untick rejoin practice scheme (permanent status = 'D')
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 31 May 2008
-- Description:	Delete Practice Information in table
--				"PracticeStaging"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	04 August 2009
-- Description:		Add "SubsidizeCode"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	07 August 2009
-- Description:		Add @checktsmp
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	07 August 2009
-- Description:		Remove @checktsmp
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	14 Oct 2009
-- Description:		Check the corresponding practice scheme
--					in permanent table
--					if record status = 'D'
--					=> copy the information from permanent to staging
-- =============================================

CREATE PROCEDURE [dbo].[proc_PracticeSchemeInfoStaging_del_byERNDisplaySeq]
	@enrolment_ref_no		char(15), 
	@sp_id					char(8), 
	@scheme_code			char(10), 
	@practice_display_seq	smallint, 
	@record_status			char(1), 
	@update_by				varchar(20), 
	@tsmp					timestamp, 
	@subsidize_code			char(10)
AS BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
declare @exist_record_status char(1)

select @exist_record_status = record_status
from PracticeSchemeInfo
WHERE	SP_ID = @sp_id and
		Practice_Display_Seq = @practice_display_seq and
		Scheme_code = @scheme_code and
		subsidize_code = @subsidize_code
		
-- =============================================
-- Validation 
-- =============================================
	/*
	IF (	
		SELECT	TSMP 
		FROM	PracticeSchemeInfoStaging
		WHERE	Enrolment_Ref_No = @enrolment_ref_no 
					AND Practice_Display_Seq = @practice_display_seq 
					AND Scheme_code = @scheme_code 
					AND Subsidize_Code = @subsidize_code
		) != @tsmp
	BEGIN
		RAISERROR('00011', 16, 1)
		RETURN @@error
	END
	*/
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

if isnull(@exist_record_status,'') = 'D'
	BEGIN
		UPDATE	PracticeSchemeInfoStaging
		SET		Record_Status = @record_status,
				Update_by = @update_by,
				Update_Dtm = getdate()
		WHERE	Enrolment_Ref_No = @enrolment_ref_no and
				Practice_Display_Seq = @practice_display_seq and
				Scheme_code = @scheme_code and subsidize_code = @subsidize_code

		DELETE	FROM PracticeSchemeInfoStaging
		WHERE	Enrolment_Ref_No = @enrolment_ref_no and
				Practice_Display_Seq = @practice_display_seq and
				Scheme_code = @scheme_code and subsidize_code = @subsidize_code
	END
ELSE
	BEGIN
		UPDATE	PracticeSchemeInfoStaging
		SET		Record_Status = @record_status,
				Update_by = @update_by,
				Update_Dtm = getdate()
		WHERE	Enrolment_Ref_No = @enrolment_ref_no and
				Practice_Display_Seq = @practice_display_seq and
				Scheme_code = @scheme_code --and subsidize_code = @subsidize_code

		DELETE	FROM PracticeSchemeInfoStaging
		WHERE	Enrolment_Ref_No = @enrolment_ref_no and
				Practice_Display_Seq = @practice_display_seq and
				Scheme_code = @scheme_code --and subsidize_code = @subsidize_code
	END
	
	
			
			
if isnull(@exist_record_status,'') = 'D'
begin
		insert into PracticeSchemeInfostaging
		(
			enrolment_ref_no,
			subsidize_code,
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
			scheme_code,
			provideservicefee,
			provide_service,
			Clinic_Type
		)
		select 
			@enrolment_ref_no,
			subsidize_code,
			practice_display_seq,
			sp_id,
			service_fee,
			delist_status,
			delist_dtm,
			effective_dtm,
			case isnull(delist_status,'')
				when '' then record_status
				else delist_status
			end,
			remark,
			getdate(),
			@update_by,
			getdate(),
			@update_by,
			scheme_code,
			provideservicefee,
			provide_service,
			Clinic_Type
		from PracticeSchemeInfo
		WHERE	SP_ID = @sp_id and
			Practice_Display_Seq = @practice_display_seq and
			Scheme_code = @scheme_code and
			subsidize_code = @subsidize_code

end

END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInfoStaging_del_byERNDisplaySeq] TO HCVU
GO
