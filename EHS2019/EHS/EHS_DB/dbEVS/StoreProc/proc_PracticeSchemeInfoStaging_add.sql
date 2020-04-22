IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeSchemeInfoStaging_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeSchemeInfoStaging_add]
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
-- Description:		Add field Provide_Service
-- =============================================
-- =============================================
-- Author:			Kathy LEE
-- Create date:		29-05-2009
-- Description:		Insert Practice Scheme Info in Staging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	29 June 2009
-- Description:		Add MScheme_Code, ProvideServiceFee
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
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE Procedure [dbo].[proc_PracticeSchemeInfoStaging_add]
	@enrolment_ref_no char(15),
	@scheme_code char(10),
	@practice_display_seq smallint,
	@sp_id char(8),
	@service_fee smallint,
	@delist_status char(1),
	@delist_dtm datetime,
	@effective_dtm datetime,
	@record_status char(1),
	@remark nvarchar(255),
	@create_by varchar(20),
	@update_by varchar(20),
	@subsidize_code char(10),
	@provideservicefee char(1),
	@provide_service char(1),
	@Clinic_Type char(1)
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

	Insert into PracticeSchemeInfoStaging
	(
		Enrolment_Ref_No,
		Scheme_Code,
		Practice_Display_Seq,
		SP_ID,
		Service_Fee,
		Delist_Status,
		Delist_Dtm,
		Effective_Dtm,
		Record_Status,
		Remark,
		Create_Dtm,
		Create_By,
		Update_Dtm,
		Update_By,
		Subsidize_Code,
		ProvideServiceFee,
		Provide_Service,
		Clinic_Type
	)
	values
	(
		@enrolment_ref_no,
		@scheme_code,
		@practice_display_seq,
		@sp_id,
		@service_fee,
		@delist_status,
		@delist_dtm,
		@effective_dtm,
		@record_status,
		@remark,
		getdate(),
		@create_by,
		getdate(),
		@update_by,
		@subsidize_code,
		@provideservicefee,
		@provide_service,
		@Clinic_Type
	)
END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInfoStaging_add] TO HCVU
GO
