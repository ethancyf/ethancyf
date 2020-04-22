IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ProfessionalVerification_add_wholeRecord]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_ProfessionalVerification_add_wholeRecord]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Clark YIP
-- Create date: 10 Jul 2009
-- Description:	Insert the Professional Verification Information
--				to Table ProfessionalVerification by a whole record
--				(Purpose is to add back the professional verification
--				 record from IVSS)
-- =============================================

-- =============================================
-- Modification History
-- Modified by:	
-- Modified date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[proc_ProfessionalVerification_add_wholeRecord]
	@enrolment_ref_no char(15), 
	@professional_seq smallint, 
	@sp_id char(8),
	@export_by varchar(20),
	@export_dtm datetime,
	@import_by varchar(20),
	@import_dtm datetime,
	@verification_result char(1),
	@verification_remark nvarchar(1000),
	@final_result char(1),
	@confirm_by varchar(20),
	@confirm_dtm datetime,
	@void_by varchar(20),
	@void_dtm datetime,
	@defer_by varchar(20),
	@defer_dtm datetime,
	@record_status	char(1)
AS
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

	INSERT INTO	ProfessionalVerification
				(	Enrolment_Ref_No,
					Professional_Seq,
					SP_ID,
					export_by,
					export_dtm,
					import_by,
					import_dtm,
					verification_result,
					verification_remark,
					final_result,
					confirm_by,
					confirm_dtm,
					void_by,
					void_dtm,
					defer_by,
					defer_dtm,					
					Record_Status)
	VALUES		 (	@enrolment_ref_no, 
					@professional_seq, 
					@sp_id,
					@export_by,
					@export_dtm,
					@import_by,
					@import_dtm,
					@verification_result,
					@verification_remark,
					@final_result,
					@confirm_by,
					@confirm_dtm,
					@void_by,
					@void_dtm,
					@defer_by,
					@defer_dtm,
					@record_status)
END
GO

GRANT EXECUTE ON [dbo].[proc_ProfessionalVerification_add_wholeRecord] TO HCVU
GO
