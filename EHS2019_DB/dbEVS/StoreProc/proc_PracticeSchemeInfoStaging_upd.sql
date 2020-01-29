IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeSchemeInfoStaging_upd]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeSchemeInfoStaging_upd]
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
-- Modified date:	25 June 2015
-- Description:		1. Add field Provide_Service
--					2. Check @exist_record_status by @subsidize_code
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 05 Jun 2009
-- Description:	Update the practice scheme info in
--				Table "PracticeSchemeInfoStaging"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Kathy LEE
-- Modified date: 01 July 2009
-- Description:	  Add "ProvideServiceFee"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Kathy LEE
-- Modified date: 04 August 2009
-- Description:	  Add "SubsidizeCode"
-- =============================================
CREATE PROCEDURE [dbo].[proc_PracticeSchemeInfoStaging_upd]
	@enrolment_ref_no char(15), 
	@scheme_code char(10), 
	@practice_display_seq smallint, 
	@service_fee smallint,
	@update_by varchar(20),
	@record_status char(1),
	@tsmp timestamp,
	@provideservicefee char(1),
	@subsidize_code char(10),
	@provide_service char(1),
	@Clinic_Type char(1)
AS
BEGIN
	
	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
declare @exist_record_status char(1)
-- =============================================
-- Validation 
-- =============================================
	IF (SELECT TSMP FROM PracticeSchemeInfoStaging
		WHERE Enrolment_Ref_No = @enrolment_ref_no and
				Practice_Display_Seq = @practice_display_seq and
				Scheme_code = @scheme_code and
				Subsidize_Code = @subsidize_code) != @tsmp
	BEGIN
		RAISERROR('00011', 16, 1)
		return @@error
	END
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	select @exist_record_status = record_status
	from practiceschemeinfostaging
	WHERE Enrolment_Ref_No = @enrolment_ref_no and
		  Practice_Display_Seq = @practice_display_seq and
		  Scheme_code = @scheme_code and 
		  Subsidize_Code = @subsidize_code -- added by winnie 30/06/2015
		  
	if @exist_record_status = 'A'
	begin
		UPDATE	PracticeSchemeInfoStaging
		Set		Service_Fee = @service_fee,			
				Update_By = @update_by,
				Update_Dtm = getdate(),
				ProvideServiceFee = @provideservicefee,
				Provide_Service = @provide_service,
				Clinic_Type = @Clinic_Type
		WHERE	Enrolment_Ref_No = @enrolment_ref_no and 
				Scheme_Code = @scheme_code and 
				Practice_Display_Seq=@practice_display_seq and
				Subsidize_Code = @subsidize_code
	end
	else
	begin
		UPDATE	PracticeSchemeInfoStaging
		Set		Service_Fee = @service_fee,			
				Update_By = @update_by,
				Update_Dtm = getdate(),
				Record_status = @record_status,
				ProvideServiceFee = @provideservicefee,
				Provide_Service = @provide_service,
				Clinic_Type = @Clinic_Type
		WHERE	Enrolment_Ref_No = @enrolment_ref_no and 
				Scheme_Code = @scheme_code and 
				Practice_Display_Seq=@practice_display_seq and
				Subsidize_Code = @subsidize_code
	end
		
END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInfoStaging_upd] TO HCVU
GO
