IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ERNProcessedStaging_add]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ERNProcessedStaging_add]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Kathy LEE
-- Create date:		29-05-2009
-- Description:		Insert enrolment processed for in staging
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 Kathy LEE
-- Modified date: 03 July 2009
-- Description:	1. Remove suffix, mo_display_seq, scheme_code, update_by, update_dtm
--				2. Add Sub_Enrolment_Ref_No
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	 Kathy LEE
-- Modified date: 19 Oct 2009
-- Description:	1. if the row exist (Enrolment_ref_no and sub_enrolment_ref_no)
--					delete the corresponding row and insert
-- =============================================
CREATE Procedure [dbo].[proc_ERNProcessedStaging_add]
	@enrolment_ref_no char(15),
	@sp_id char(8),
	@create_by varchar(20),
	@sub_enrolment_ref_no char(15)
as
BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
declare @rowcount int
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

	select @rowcount = count(1) from ERNProcessedStaging
	where enrolment_ref_no = @enrolment_ref_no and 
		Sub_Enrolment_Ref_No =  @sub_enrolment_ref_no 
		
if @rowcount > 0
begin

	delete from ERNProcessedStaging
	where enrolment_ref_no = @enrolment_ref_no and 
		Sub_Enrolment_Ref_No =  @sub_enrolment_ref_no	
end
		
	Insert into ERNProcessedStaging
	(
		Enrolment_Ref_No,		
		SP_ID,		
		Create_dtm,
		Create_by,
		Sub_Enrolment_Ref_No
	)
	values
	(
		@enrolment_ref_no,		
		@sp_id,		
		getdate(),
		@create_by,
		@sub_enrolment_ref_no
	)
END
GO

GRANT EXECUTE ON [dbo].[proc_ERNProcessedStaging_add] TO HCVU
GO
